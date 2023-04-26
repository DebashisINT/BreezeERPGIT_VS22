<%--====================================================== Revision History ============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                13-02-2023        2.0.36           Pallab              25575 : Report pages modification
2.0                24-04-2023        2.0.38           Pallab              25908 : Ledger Posting - Detail Report module zoom popup upper part visible issue fix
====================================================== Revision History ================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="LedgerPostingReport.aspx.cs" Inherits="Reports.Reports.GridReports.LedgerPostingReport" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>

    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        #pageControl, .dxtc-content {
            overflow: visible !important;
        }

        #MandatoryAssign {
            position: absolute;
            right: -17px;
            top: 6px;
        }

        .btnOkformultiselection {
        border-width: 1px;
        padding: 4px 10px;
        font-size: 13px !important;
        margin-right: 6px;
        }

        #MandatorySupervisorAssign {
            position: absolute;
            right: 1px;
            top: 27px;
        }

        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        /*#ListBoxBranches {
            width: 200px;
        }*/

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }

        /*rev Pallab*/
        .branch-selection-box .dxlpLoadingPanel_PlasticBlue tbody, .branch-selection-box .dxlpLoadingPanelWithContent_PlasticBlue tbody, #divProj .dxlpLoadingPanel_PlasticBlue tbody, #divProj .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            bottom: 0 !important;
        }
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 80px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/
    </style>
    <%-- For Single selection when click on ok button--%>
    <script type="text/javascript">
              
              function ValueSelected(e, indexName) {
                  if (e.code == "Enter" || e.code == "NumpadEnter") {
                      if (indexName == "ledgerIndex")
                      {
                         var Id = e.target.parentElement.parentElement.cells[0].innerText;
                         var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                         if (Id) {
                            SetLedger(Id, name);
                         }
                      }
                      else if (indexName == "SubledgerIndex")
                      {
                          var Id = e.target.parentElement.parentElement.cells[0].innerText;
                          var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                          //var type = e.target.parentElement.parentElement.cells[2].innerText;
                          if (Id) {
                              SetSubLedger(Id, name);
                          }
                      }
                     
                  }
                  else if (e.code == "ArrowDown") {
                      thisindex = parseFloat(e.target.getAttribute(indexName));
                      thisindex++;
                      if (thisindex < 10)
                          $("input[" + indexName + "=" + thisindex + "]").focus();
                  }
                  else if (e.code == "ArrowUp") {
                      thisindex = parseFloat(e.target.getAttribute(indexName));
                      thisindex--;
                      if (thisindex > -1)
                          $("input[" + indexName + "=" + thisindex + "]").focus();
                      else {
                          $('#txtLedgerSearch').focus();
                      }
                  }
              }

          </Script>
    <%-- For Single selection when click on ok button--%>

  <%--For Ledger Single Selection--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#LedgModel').on('shown.bs.modal', function () {
                $('#txtLedgerSearch').focus();
            })
        })
        function LedgerButnClick(s, e) {
            $('#LedgModel').modal('show');
        }

        function Ledger_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#LedgModel').modal('show');
            }
        }

        function Ledgerkeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtLedgerSearch").val()) == "" || $.trim($("#txtLedgerSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtLedgerSearch").val();
            OtherDetails.BranchID = hdnSelectedBranches.value;

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Description");

                if ($("#txtLedgerSearch").val() != "") {
                    callonServer("Services/Master.asmx/GetLedgerBind", OtherDetails, "LedgerTable", HeaderCaption, "ledgerIndex", "SetLedger");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ledgerIndex=0]"))
                    $("input[ledgerIndex=0]").focus();
            }
        }

        function SetLedger(Id, Name) {
            var key = Id;
            if (key != null && key != '') {
                $('#LedgModel').modal('hide');
                ctxtLedger.SetText(Name);
                GetObjectID('hdnSelectedLedger').value = key;
                ctxtLedger.Focus();
            }
            else {
                ctxtLedger.SetText('');
                GetObjectID('hdnSelectedLedger').value = '';
            }
        }

    </Script>
    <%--For Ledger Single Selection--%>

     <%--For Sub Ledger Single Selection--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#SubLedgModel').on('shown.bs.modal', function () {
                $('#txtSubLedgerSearch').focus();
            })
        })
        function SubLedgerButnClick(s, e) {
            $('#SubLedgModel').modal('show');
        }

        function SubLedger_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#SubLedgModel').modal('show');
            }
        }

        function SubLedgerkeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtSubLedgerSearch").val()) == "" || $.trim($("#txtSubLedgerSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtSubLedgerSearch").val();
            if (hdnSelectedLedger.value != 0)
            {
                OtherDetails.LedgerId = hdnSelectedLedger.value;
            }
            else {
                OtherDetails.LedgerId = "0";
            }

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("SubLedger");
                HeaderCaption.push("Type");

                if ($("#txtSubLedgerSearch").val() != "") {
                    callonServer("Services/Master.asmx/GetSubLedgerBind", OtherDetails, "SubLedgerTable", HeaderCaption, "SubledgerIndex", "SetSubLedger");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[SubledgerIndex=0]"))
                    $("input[SubledgerIndex=0]").focus();
            }
        }

        function SetSubLedger(Id, Name) {
            var key = Id;
            if (key != null && key != '') {
                $('#SubLedgModel').modal('hide');
                ctxtSubLedger.SetText(Name);
                GetObjectID('hdnSelectedSubLedger').value = key;
                GetObjectID('hdnSelectedSubLedgerType').value = $.grep(mycallonServerObj, function (element) { return element.ID == key })[0].Type;
                ctxtSubLedger.Focus();

            }
            else {
                ctxtLedger.SetText('');
                GetObjectID('hdnSelectedSubLedger').value = '';
                GetObjectID('hdnSelectedSubLedgerType').value = '';
            }
        }

    </Script>
    <%--For Sub Ledger Single Selection--%>



    <script type="text/javascript">

        $(function () {
            //cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + 0);
            cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            function OnWaitingGridKeyPress(e) {

                if (e.code == "Enter") {

                }
            }
        });

       $(document).ready(function () {
            <%--$("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })--%>

             $("#ddlbranchHO").change(function () {
                 var Ids = $(this).val();
                $('#MandatoryActivityType').attr('style', 'display:none');
                $("#hdnSelectedBranches").val('');
                cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
         })


        //function BindActivityType(noteTilte) {
        //    var lBox = $('select[id$=ListBoxBranches]');
        //    var listItems = [];
        //    var selectedNoteId = '';
        //    if (noteTilte) {

        //        selectedNoteId = noteTilte;
        //    }
        //    lBox.empty();


        //    $.ajax({
        //        type: "POST",
        //        url: 'LedgerPostingReport.aspx/GetBranchesList',
        //        contentType: "application/json; charset=utf-8",
        //        data: JSON.stringify({ NoteId: selectedNoteId }),
        //        dataType: "json",
        //        success: function (msg) {
        //            var list = msg.d;

        //            if (list.length > 0) {

        //                for (var i = 0; i < list.length; i++) {

        //                    var id = '';
        //                    var name = '';
        //                    id = list[i].split('|')[1];
        //                    name = list[i].split('|')[0];

        //                    listItems.push('<option value="' +
        //                    id + '">' + name
        //                    + '</option>');
        //                }



        //                $(lBox).append(listItems.join(''));
        //                ListActivityType();

        //                $('#ListBoxBranches').trigger("chosen:updated");
        //                $('#ListBoxBranches').prop('disabled', false).trigger("chosen:updated");
        //            }
        //            else {
        //                lBox.empty();
        //                $('#ListBoxBranches').trigger("chosen:updated");
        //                $('#ListBoxBranches').prop('disabled', true).trigger("chosen:updated");

        //            }
        //        },
        //        error: function (XMLHttpRequest, textStatus, errorThrown) {
        //            //  alert(textStatus);
        //        }
        //    });


        //}

        //function ListActivityType() {

        //    $('#ListBoxBranches').chosen();
        //    $('#ListBoxBranches').fadeIn();

        //    var config = {
        //        '.chsnProduct': {},
        //        '.chsnProduct-deselect': { allow_single_deselect: true },
        //        '.chsnProduct-no-single': { disable_search_threshold: 10 },
        //        '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
        //        '.chsnProduct-width': { width: "100%" }
        //    }
        //    for (var selector in config) {
        //        $(selector).chosen(config[selector]);
        //    }
        //}

        //function ListLedgerType() {

        //    $('#ListBoxLedgerType').chosen();
        //    $('#ListBoxLedgerType').fadeIn();

        //    var config = {
        //        '.chsnProduct': {},
        //        '.chsnProduct-deselect': { allow_single_deselect: true },
        //        '.chsnProduct-no-single': { disable_search_threshold: 10 },
        //        '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
        //        '.chsnProduct-width': { width: "100%" }
        //    }
        //    for (var selector in config) {
        //        $(selector).chosen(config[selector]);
        //    }
        //}
        function ListCustomerVendor() {

            $('#ListBoxCustomerVendor').chosen();
            $('#ListBoxCustomerVendor').fadeIn();

            var config = {
                '.chsnProduct': {},
                '.chsnProduct-deselect': { allow_single_deselect: true },
                '.chsnProduct-no-single': { disable_search_threshold: 10 },
                '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsnProduct-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }
        }

        function BranchValuewiseledger() {
            //if (gridbranchLookup.GetValue() == null) {
            //    gridledgerLookup.SetEnabled(false);
            //    gridledgerLookup.SetValue(null);
            //}
            //else {
            //cProductComponentPanel_ledger.PerformCallback('BindComponentGrid' + '~' + 0);
            //gridledgerLookup.SetValue(null);
            //}
            gridbranchLookup.GetGridView().GetSelectedFieldValues("ID", GotSelectedValues);
        }
        function GotSelectedValues(selectedValues) {
            if (selectedValues.length == 0)
            {

            }
            else
            {
                values = "";
                for (i = 0; i < selectedValues.length; i++) {
                    if (values == "") {
                        values = selectedValues[i] + "";
                    }
                    else
                    {
                        values = values +','+ selectedValues[i] + "";
                    }
                    
                }
                $("#hdnSelectedBranches").val(values);
            }
         
        }
        //function LedgerValuewiseSubledger() {
        //    //if (gridbranchLookup.GetValue() == null) {
        //    //    jAlert('Please select atleast one branch');
        //    //    gridledgerLookup.SetValue(null);
        //    //}
        //    //else {
        //        var key = gridledgerLookup.GetGridView().GetRowKey(gridledgerLookup.GetGridView().GetFocusedRowIndex());
        //        cEmployeetComponentPanel.PerformCallback('BindComponentGrid' + '~' + key);
        //    //}
        //}

        function OnGetRowValuesCallback(values) {
            alert(values);
        }

        $(document).ready(function () {


            //BindLedgerType(0);
            //cProductComponentPanel_ledger.PerformCallback('BindComponentGrid' + '~' + 0);

            //cEmployeetComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);

<%--            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                // BindLedgerType(Ids);
                cProductComponentPanel_ledger.PerformCallback('BindComponentGrid' + '~' + Ids);
                //BindCustomerVendor(Ids);
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })--%>

            $("#ListBoxLedgerType").chosen().change(function () {
                var Ids = $(this).val();

                $('#<%=hdnSelectedLedger.ClientID %>').val(Ids);
                $('#MandatoryLedgerType').attr('style', 'display:none');

            })

            $("#ListBoxCustomerVendor").chosen().change(function () {
                var Ids = $(this).val();

                <%--$('#<%=hdnSelectedCustomerVendor.ClientID %>').val(Ids);--%>
                $('#MandatoryCustomerType').attr('style', 'display:none');

            })

        })

        //function BindLedgerType(Ids) {
        //    var lBox = $('select[id$=ListBoxLedgerType]');
        //    lBox.clearQueue();
        //    var listItems = [];
        //    $.ajax({
        //        type: "POST",
        //        url: 'LedgerPostingReport.aspx/BindLedgerType',
        //        data: "{'Ids':'" + Ids + "'}",
        //        //  data: JSON.stringify({ Ids: Ids }),
        //        contentType: "application/json; charset=utf-8",

        //        dataType: "json",
        //        success: function (msg) {
        //            var list = msg.d;

        //            if (list.length > 0) {

        //                for (var i = 0; i < list.length; i++) {

        //                    var id = '';
        //                    var name = '';
        //                    id = list[i].split('|')[1];
        //                    name = list[i].split('|')[0];

        //                    listItems.push('<option value="' +
        //                    id + '">' + name
        //                    + '</option>');

        //                }

        //                $(lBox).append(listItems.join(''));
        //                ListLedgerType();

        //                $('#ListBoxLedgerType').trigger("chosen:updated");
        //                $('#ListBoxLedgerType').prop('disabled', false).trigger("chosen:updated");
        //            }
        //            else {
        //                lBox.empty();
        //                $('#ListBoxLedgerType').trigger("chosen:updated");
        //                $('#ListBoxLedgerType').prop('disabled', true).trigger("chosen:updated");

        //            }
        //        },
        //        error: function (XMLHttpRequest, textStatus, errorThrown) {
        //            //  alert(textStatus);
        //        }
        //    });


        //}

        function BindCustomerVendor() {

            var lBox = $('select[id$=ListBoxCustomerVendor]');
            var listItems = [];
            $.ajax({
                type: "POST",
                url: 'LedgerPostingReport.aspx/BindCustomerVendor',
                //data: "{'Ids':'" + Ids + "'}",                   
                contentType: "application/json; charset=utf-8",

                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    if (list.length > 0) {
                        for (var i = 0; i < list.length; i++) {

                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }


                        $(lBox).append(listItems.join(''));
                        ListCustomerVendor();

                        $('#ListBoxCustomerVendor').trigger("chosen:updated");
                        $('#ListBoxCustomerVendor').prop('disabled', false).trigger("chosen:updated");
                    }
                    else {
                        lBox.empty();
                        $('#ListBoxCustomerVendor').trigger("chosen:updated");
                        $('#ListBoxCustomerVendor').prop('disabled', true).trigger("chosen:updated");

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //  alert(textStatus);
                }
            });


        }

        function Callback2_EndCallback() {
            // alert('');
            $("#drdExport").val(0);
        }
        function CallbackPanelEndCall(s, e) {
             <%--Rev Subhra 18-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }
    </script>

    <script type="text/javascript">

        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // Grid.PerformCallback('');
        }

        function btn_ShowRecordsClick(e) {
            //e.preventDefault;
            //var data = "OnDateChanged";
            //$("#hfIsLedgerPostingFilter").val("Y");
            //$("#drdExport").val(0);

            //if (gridbranchLookup.GetValue() == null) {
            //    jAlert('Please select atleast one branch');
            //}
            //else {
            //data += '~' + cxdeFromDate.GetDate();
            //data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // alert( data);


            //var s = gridemployeeLookup.GetRowValues(gridemployeeLookup.FocusedRowIndex, "Description")

            <%-- var grid = gridemployeeLookup.GetGridView();
            grid.GetRowValues(grid.GetFocusedRowIndex(), 'Type;Doc Code', OnGetRowValues);

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;--%>


            //Grid.PerformCallback(data);
           
            //}

            //data += '~' + cxdeFromDate.GetDate();
            //data += '~' + cxdeToDate.GetDate();
            ////CallServer(data, "");
            //// alert( data);
            //Grid.PerformCallback(data);



            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            var data = "OnDateChanged";
            $("#hfIsLedgerPostingFilter").val("Y");
            $("#drdExport").val(0);
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();

            if (hdnSelectedSubLedgerType.value == "Sub Ledger") {
                if (ctxtLedger.GetValue() == null) {
                    jAlert('Please select Ledger first', "Alert", function () {
                    });
                }
            }
            if (ctxtLedger.GetValue() == null && ctxtSubLedger.GetValue() == null) {
                jAlert('Please select at least one Ledger or Sub Ledger', "Alert", function () {
                });
            }
            else if (gridbranchLookup.GetValue() == null && ctxtLedger.GetValue() != null && ctxtSubLedger.GetValue() == null) {
                jAlert('Please select at least one Branch', "Alert", function () {
                });
            }
            else {
                //cCallbackPanel.PerformCallback(data + '~' + hdnSelectedSubLedgerType.value + '~' + hdnSelectedSubLedger.value + '~' + $("#ddlbranchHO").val());
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback(data + '~' + hdnSelectedSubLedgerType.value + '~' + hdnSelectedSubLedger.value + '~' + $("#ddlbranchHO").val());
                }
            }

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
        }

        function GetDateFormat(today) {
            if (today != "") {
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                today = dd + '-' + mm + '-' + yyyy;
            }

            return today;
        }

        //function OnGetRowValues(types) {
        //    //   alert(types[0] + ' ' + types[1]);
        //    var data = "OnDateChanged";

        //    //Grid.PerformCallback(data + '~' + types[0] + '~' + types[1]);
        //    if (types[0] == "Sub Ledger") {
        //        if (gridledgerLookup.GetValue() == null) {
        //            jAlert('Please select Ledger first', "Alert", function () {
        //            });
        //        }
        //        else {
        //            //Grid.PerformCallback(data + '~' + types[0] + '~' + types[1]);
        //            ////Grid.PerformCallback(data + '~' + types[0] + '~' + types[1] + '~' + $("#ddlbranchHO").val());
        //            cCallbackPanel.PerformCallback(data + '~' + types[0] + '~' + types[1] + '~' + $("#ddlbranchHO").val());
        //        }
        //    }
        //    if (gridledgerLookup.GetValue() == null && gridemployeeLookup.GetValue() == null) {
        //        jAlert('Please select at least one Ledger or Sub Ledger', "Alert", function () {
        //        });
        //    }
        //        //else {
        //        //    Grid.PerformCallback(data + '~' + types[0] + '~' + types[1]);
        //        //}
        //    else if (gridbranchLookup.GetValue() == null && gridledgerLookup.GetValue() != null && gridemployeeLookup.GetValue() == null) {
        //        jAlert('Please select at least one Branch', "Alert", function () {
        //        });
        //    }
        //    else {
        //        //Grid.PerformCallback(data + '~' + types[0] + '~' + types[1]);
        //        ////Grid.PerformCallback(data + '~' + types[0] + '~' + types[1] + '~' + $("#ddlbranchHO").val());

        //        cCallbackPanel.PerformCallback(data + '~' + types[0] + '~' + types[1] + '~' + $("#ddlbranchHO").val());
        //    }
        //}


        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function abc() {
            // alert();
            $("#drdExport").val(0);

        }

        function OpenPOSDetails(Uniqueid, type, docno) {
            var url = '';
            if (type == 'POS') {
                //  window.location.href = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + invoice + '&Viemode=1';
                //   window.open('/OMS/Management/Activities/posSalesInvoice.aspx?key=' + Uniqueid + '&Viemode=1', '_blank')

                url = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&Viemode=1';
            }
            else if (type == 'SI') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                //   window.open('/OMS/Management/Activities/SalesInvoice.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')

                url = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }


            else if (type == 'PC') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                //  window.open('/OMS/Management/Activities/PurchaseChallan.aspx?key=' + Uniqueid + '&req=V&status=1&type=' + type, '_blank')

                url = '/OMS/Management/Activities/PurchaseChallan.aspx?key=' + Uniqueid + '&req=V&IsTagged=1&type=' + type;
            }

            else if (type == 'SR') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                // window.open('/OMS/Management/Activities/SalesReturn.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')
                url = '/OMS/Management/Activities/SalesReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'SRM') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                //  window.open('/OMS/Management/Activities/ReturnManual.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')
                url = '/OMS/Management/Activities/ReturnManual.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            else if (type == 'SRN') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                //  window.open('/OMS/Management/Activities/ReturnNormal.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')
                url = '/OMS/Management/Activities/ReturnNormal.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;

            }


            else if (type == 'PI') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///   window.open('/OMS/Management/Activities/PurchaseInvoice.aspx?key=' + Uniqueid + '&req=V&type=PB', '_blank')
                url = '/OMS/Management/Activities/PurchaseInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PB';

            }

            else if (type == 'VP' || type == 'VR') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                /// window.open('/OMS/Management/Activities/VendorPaymentReceipt.aspx?key=' + Uniqueid + '&req=V&type=VPR', '_blank')
                url = '/OMS/Management/Activities/VendorPaymentReceipt.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=VPR';
            }

            else if (type == 'IP') {
                url = '/OMS/Management/Activities/InfluencerPayment.aspx?key=Edit&Id=' + Uniqueid + '&req=View';
            }

            else if (type == 'PR') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///    window.open('/OMS/Management/Activities/BranchRequisitionReturn.aspx?key=' + Uniqueid + '&req=V', '_blank')
                url = '/OMS/Management/Activities/PReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PR';

            }


            else if (type == 'SC') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                /// window.open('/OMS/Management/Activities/CustomerReturn.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')

                url = '/OMS/Management/Activities/CustomerReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }


            else if (type == 'CP' || type == 'CR') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///    window.open('/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&req=V&type=CRP', '_blank')
                url = '/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CRP';
            }

            else if (type == 'JV') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///    window.open('/OMS/Management/Activities/BranchRequisitionReturn.aspx?key=' + Uniqueid + '&req=V', '_blank')
                url = '/OMS/Management/dailytask/JournalEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=' + docno;

            }
            else if (type == 'CBV') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///    window.open('/OMS/Management/Activities/BranchRequisitionReturn.aspx?key=' + Uniqueid + '&req=V', '_blank')
                url = '/OMS/Management/dailytask/CashBankEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=V';
            }

            else if (type == 'CV') {
                url = '/OMS/Management/dailytask/ContraVoucher.aspx?key=' + Uniqueid + '&IsTagged=1&req=' + docno;
            }

            else if (type == 'CNC' || type == 'DNC') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///    window.open('/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&req=V&type=CRP', '_blank')
                url = '/OMS/Management/Activities/CustomerNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
            }


            else if (type == 'CNV' || type == 'DNV') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///    window.open('/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&req=V&type=CRP', '_blank')
                url = '/OMS/Management/Activities/VendorDebitCreditNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
            }


            else if (type == 'TPB') {

                url = '/OMS/Management/Activities/TPurchaseInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }


            else if (type == 'TSI') {

                url = '/OMS/Management/Activities/TSalesInvoice.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }

            popupbudget.SetContentUrl(url);
            popupbudget.Show();

        }
        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }

        function CloseGridQuotationLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            gridquotationLookup.Focus();
        }

        function CloseGridemployeeeLookup() {
            gridemployeeLookup.ConfirmCurrentSelection();
            gridemployeeLookup.HideDropDown();
            gridemployeeLookup.Focus();
        }

        function CloseGridQuotationLookupledger() {
            gridledgerLookup.ConfirmCurrentSelection();
            gridledgerLookup.HideDropDown();
            gridledgerLookup.Focus();
        }

        function CloseGridBranchLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function selectAll_Branch() {
            gridbranchLookup.gridView.SelectRows();
        }

        function unselectAll_Branch() {
            gridbranchLookup.gridView.UnselectRows();
        }

        function selectAll() {
            gridledgerLookup.gridView.SelectRows();
        }

        function unselectAll() {
            gridledgerLookup.gridView.UnselectRows();
        }


        function selectAll_Party() {
            gridquotationLookup.gridView.SelectRows();
        }

        function unselect_AllParty() {
            gridquotationLookup.gridView.UnselectRows();
        }

        function selectAll_Employee() {
            gridemployeeLookup.gridView.SelectRows();
        }

        function unselect_Employee() {
            gridemployeeLookup.gridView.UnselectRows();
        }

    </script>

    <style>
        .plhead a {
            font-size:16px;
            padding-left:10px;
            position:relative;
            width:100%;
            display:block;
            padding:9px 10px 5px 10px;
        }
        .plhead a>i {
            position:absolute;
            top:11px;
            right:15px;
        }
        #accordion {
            margin-bottom:10px;
        }
        .companyName {
            font-size:16px;
            font-weight:bold;
            margin-bottom:15px;
        }
       
        .plhead a.collapsed .fa-minus-circle{
            display:none;
        }

        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo
        {
            color: #141414 !important;
            font-size: 14px;
                font-weight: 500 !important;
        }

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 4px 4px 4px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid
        {
                max-width: 98%  !important;
        }
        /*Rev end 1.0*/

        /*Rev 2.0*/

        #ASPXPopupControl2_PW-1
        {
            position: fixed !important;
            top: 10% !important;
            left: 10% !important;
        }

        @media only screen and (max-width: 1450px) and (min-width: 1300px)
        {
            #ASPXPopupControl2_PW-1
            {
                /*position:fixed !important;*/
                left: 10px !important;
                top: 8% !important;
            }
        }

        /*Rev end 2.0*/
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                Grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                Grid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    Grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    Grid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">
        <%--<div class="panel-title">
            <h3>Ledger Posting</h3>
        </div>--%>
        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
          <div class="panel panel-info">
            <div class="panel-heading" role="tab" id="headingOne">
              <h4 class="panel-title plhead">
                <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="collapsed">
                  <asp:Label ID="RptHeading" runat="Server" Text="" Width="470px" style="font-weight:bold;"></asp:Label>
                    <i class="fa fa-plus-circle" ></i>
                    <i class="fa fa-minus-circle"></i>
                </a>
              </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
              <div class="panel-body">
                    <div class="companyName">
                        <asp:Label ID="CompName" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <%--Rev Subhra 11-12-2018   0017670--%>
                    <div>
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                  <%--End of Rev--%>
                    <div>
                        <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompOth" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="CompPh" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>       
                    <div>
                        <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="DateRange" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
              </div>
            </div>
          </div>
        </div>
    </div>
    <%--Rev 1.0: "outer-div-main" class add: --%>
    <div class="outer-div-main">
        <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row">
            <%--Rev 1.0--%>
            <%--<div class="col-md-2">--%>
            <div class="col-md-2 simple-select">
                <%--Rev end 1.0--%>
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2 branch-selection-box">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>

                    <%--<asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>
                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <%--<span id="MandatoryActivityType" style="display: none" class="validclass">--%>
                        <%--<img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>--%>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchPanel" OnCallback="Componentbranch_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                    OnDataBinding="lookup_branch_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", " TabIndex="1">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" width="200px" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="2" width="200px" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <%--<div class="hide">--%>
                                                                <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Branch" UseSubmitBehavior="False"/>
                                                            <%--</div>--%>
                                                            <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Branch" UseSubmitBehavior="False"/>                                                            
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridBranchLookupbranch" UseSubmitBehavior="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                        <SettingsPager Mode="ShowPager">
                                        <PageSizeItemSettings Items="10, 20, 50, 100, 150, 200" Visible="True"></PageSizeItemSettings>
                                        </SettingsPager>

                                        <SettingsPager PageSize="20">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                        </SettingsPager>

                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>
                                    <ClientSideEvents ValueChanged="BranchValuewiseledger" />
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>

                    </dxe:ASPxCallbackPanel>

                </div>
            </div>
            <div class="col-md-2">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label2" runat="Server" Text="Ledger : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>
                    <%--<asp:ListBox ID="ListBoxLedgerType" Visible="false" runat="server" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>


                  <%--  <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel_ledger" ClientInstanceName="cProductComponentPanel_ledger" OnCallback="ComponentLedger_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="ASPxGridLookupledger" runat="server" TabIndex="7" ClientInstanceName="gridledgerLookup"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewDataColumn FieldName="AccountName" Visible="true" VisibleIndex="1" Caption="Description" Width="180" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <div class="hide">
                                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                                            </div>
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" />                                                            
                                                            <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookupledger" UseSubmitBehavior="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                        <SettingsPager Mode="ShowPager">
                                        </SettingsPager>

                                        <SettingsPager PageSize="20">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                        </SettingsPager>

                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>
                                      <ClientSideEvents ValueChanged="LedgerValuewiseSubledger" />
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>

                    <asp:HiddenField ID="hdnSelectedLedger" runat="server" />

                    <span id="MandatoryLedgerType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>--%>

                   <dxe:ASPxButtonEdit ID="txtLedger" runat="server" ReadOnly="true" ClientInstanceName="ctxtLedger" Width="100%" TabIndex="2">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){LedgerButnClick();}" KeyDown="function(s,e){Ledger_KeyDown(s,e);}" />
                   </dxe:ASPxButtonEdit>
                    <asp:HiddenField ID="hdnSelectedLedger" runat="server" />

                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Sub Ledger : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>


                   <%-- <dxe:ASPxCallbackPanel runat="server" ID="panel_employee" ClientInstanceName="cEmployeetComponentPanel" OnCallback="ComponentEmployee_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="grid_lookupemployee"  runat="server" TabIndex="7" ClientInstanceName="gridemployeeLookup" 
                                    OnDataBinding="lookup_Employee_DataBinding"
                                    KeyFieldName="Doc Code" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewDataColumn FieldName="Doc Code" Visible="False" VisibleIndex="1" Caption="Code" Width="180" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="Description" Visible="true" VisibleIndex="2" Caption="SubLedger" Width="180" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                        <dxe:GridViewDataColumn FieldName="Type" Visible="true" VisibleIndex="3" Caption="Type" Width="180" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <div class="hide">
                                                                <dxe:ASPxButton ID="btn_party" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Employee" />
                                                            </div>
                                                            <dxe:ASPxButton ID="btn_dparty" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselect_Employee" />                                                            
                                                            <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridemployeeeLookup" UseSubmitBehavior="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                        <SettingsPager Mode="ShowPager">
                                        </SettingsPager>

                                        <SettingsPager PageSize="20">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                        </SettingsPager>

                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>



                    <asp:HiddenField ID="HiddenField1" runat="server" />


                    <span id="MandatoryCustomerType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>--%>

                      <dxe:ASPxButtonEdit ID="txtSubLedger" runat="server" ReadOnly="true" ClientInstanceName="ctxtSubLedger" Width="100%" TabIndex="3">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){SubLedgerButnClick();}" KeyDown="function(s,e){SubLedger_KeyDown(s,e);}" />
                      </dxe:ASPxButtonEdit>
                    <asp:HiddenField ID="hdnSelectedSubLedger" runat="server" />
                    <asp:HiddenField ID="hdnSelectedSubLedgerType" runat="server" />
                    
                </div>
            </div>
            <div class="col-md-2">
               
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                
                <div>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy" TabIndex="4" 
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>
            <div class="col-md-2">
               
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>              
                    <div>
                        <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy" TabIndex="5"
                            UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>

                        </dxe:ASPxDateEdit>
                    </div>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>
            <div class="clear"></div>
            <%--<div class="col-md-2">--%>
                <div class="hide">
                    <div id="ckpar" style="padding-top: 7px;">
                        <asp:CheckBox runat="server" ID="chkparty" Checked="false" Text="Search by Party Inv. Date" />
                    </div>
                </div>
            <%--</div>--%>
            <div class="col-md-2" style="margin-top: 15px;">
                <div>
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <%-- <% if (rights.CanExport)
                       { %>--%>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                    <%-- <% } %>--%>
                </div>
            </div>
        </div>
        <table class="pull-left">
           <tr>        
               <%-- <div class="col-md-2">--%>                
                
                <%--</div>--%>

                <%--<td style="padding-left: 15px">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Party : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>--%>
                <%--<td style="width: 254px">
                    <asp:ListBox ID="ListBoxCustomerVendor" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>


                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cProductComponentPanel" OnCallback="ComponentProduct_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_quotation" SelectionMode="Multiple" runat="server" TabIndex="7" ClientInstanceName="gridquotationLookup"
                                    OnDataBinding="lookup_quotation_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="20" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="Name" Visible="true" VisibleIndex="1" Caption="Name" Width="180" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>


                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxButton ID="btn_party" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Party" />
                                                            <dxe:ASPxButton ID="btn_dparty" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselect_AllParty" />

                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                        <SettingsPager Mode="ShowPager">
                                        </SettingsPager>

                                        <SettingsPager PageSize="20">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                        </SettingsPager>

                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>
                                    <%--  <ClientSideEvents ValueChanged="function(s, e) { InvoiceNumberChanged();}" />--%>
                <%-- </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>--%>
                <%--<ClientSideEvents EndCallback="componentEndCallBack" />--%>
                <%-- </dxe:ASPxCallbackPanel>--%>



                <%--<asp:HiddenField ID="hdnSelectedCustomerVendor" runat="server" />--%>


                <%--<span id="MandatoryCustomerType" style="display: none" class="validclass">--%>
                <%--<img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>--%>

                <%--</td>--%>
            </tr>
            <tr>            
            </tr>
        </table>

        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <%-- <div  id="divRowview">--%>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyboardSupport="true" KeyFieldName="SLNO"
                             ClientSideEvents-BeginCallback="Callback2_EndCallback"
                            DataSourceID="GenerateEntityServerModeDataSource" OnSummaryDisplayText="ShowGrid_SummaryDisplayText" 
                            SettingsBehavior-AllowFocusedRow="true" SettingsBehavior-AllowSelectSingleRowOnly="true" Settings-HorizontalScrollBarMode="Visible">
                            <Columns>


                                <%-- <dxe:GridViewDataComboBoxColumn FieldName="SL" Caption="SL"  Width="10%" VisibleIndex="1">                               
                            </dxe:GridViewDataComboBoxColumn>--%>
                                <%--  <dxe:GridViewDataTextColumn FieldName="SL" Caption="SL" Width="15%" VisibleIndex="1" />
                            <dxe:GridViewDataTextColumn FieldName="CASHBANKID" Caption="CASHBANKID" Width="15%" VisibleIndex="2" />
                            <dxe:GridViewDataTextColumn FieldName="CASHBANKNAME" Caption="CASHBANK NAME" Width="15%" VisibleIndex="3" />--%>
                                <%-- <dxe:GridViewDataTextColumn FieldName="ACCOUNT_GROUP" Caption="ACCOUNT GROUP" Width="9%" VisibleIndex="4" />--%>

                                <%--<dxe:GridViewDataTextColumn FieldName="GROUP_NAME" Caption="Group" Width="10%" VisibleIndex="0" GroupIndex="0">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="SUBGRP_NAME" Caption="Sub Group" Width="10%" VisibleIndex="1" GroupIndex="1">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT" Caption="Ledger Desc" Width="15%" VisibleIndex="2" GroupIndex="2">
                                </dxe:GridViewDataTextColumn>--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" Width="10%" VisibleIndex="1" />--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="GROUP_NAME" Caption="Group" Width="10%" VisibleIndex="2" />--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="SUBGRP_NAME" Caption="Sub Group" Width="10%" VisibleIndex="3" />--%> 
                                <%-- <dxe:GridViewDataTextColumn FieldName="DOCUMENT_NO" Caption="Document No" Width="12%" VisibleIndex="6" />--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="Particulars" Caption="Particulars" Width="15%" VisibleIndex="6" />--%>

                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" Width="20%" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn FieldName="TRAN_DATE" Caption="Document Date" Width="15%" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />
                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="DOCUMENT_NO" Caption="Document No" Width="20%">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>

                                        <a href="javascript:void(0)" onclick="OpenPOSDetails('<%#Eval("DOC_ID") %>','<%#Eval("MODULE_TYPE") %>' ,'<%#Eval("DOCUMENT_NO") %>')" class="pad">
                                            <%#Eval("DOCUMENT_NO")%>
                                        </a>
                                    </DataItemTemplate>

                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="TRAN_TYPE" Caption="Document Type" VisibleIndex="4" Width="18%"/>
                                <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT" Caption="Ledger Desc" Width="15%" VisibleIndex="5" />
                                <dxe:GridViewDataTextColumn FieldName="CUSTOMERVENDOR" Caption="Details" VisibleIndex="6" Width="22%" />
                                <dxe:GridViewDataTextColumn FieldName="HNARRATION" Caption="Narration" VisibleIndex="7" Width="20%" />
                                <dxe:GridViewDataTextColumn FieldName="DEBIT" Caption="Dr." Width="12%" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CREDIT" Caption="Cr." Width="12%" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="NET_AMT" Caption="Balance" Width="12%" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00" />
                                <dxe:GridViewDataTextColumn FieldName="DRCR" Caption="Dr./Cr." Width="8%" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00" />                                
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="False" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="CUSTOMERVENDOR" SummaryType="Custom" />
                                <dxe:ASPxSummaryItem FieldName="DEBIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CREDIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="NET_AMT" SummaryType="Custom" />
                                <dxe:ASPxSummaryItem FieldName="DRCR" SummaryType="Custom" />
                            </TotalSummary>

                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="LEDGERPOSTRUNNINGBAL_REPORT"></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>
    
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
        Width="1310px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="BudgetAfterHide" />
    </dxe:ASPxPopupControl>

 <!--Ledger Modal -->
    <div class="modal fade" id="LedgModel" role="dialog">
        <div class="modal-dialog">
            <!-- Ledger content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Ledger Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Ledgerkeydown(event)" id="txtLedgerSearch" autofocus width="100%" placeholder="Search By Ledger" />
                    <div id="LedgerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Description</th>
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
    <!--ledger Modal -->

<!--Sub Ledger Modal -->
    <div class="modal fade" id="SubLedgModel" role="dialog">
        <div class="modal-dialog">
            <!-- Sub Ledger content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Sub Ledger Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="SubLedgerkeydown(event)" id="txtSubLedgerSearch" autofocus width="100%" placeholder="Search By Sub Ledger" />
                    <div id="SubLedgerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>SubLedger</th>
                                <th>Type</th>
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
    <!--Sub ledger Modal -->

   <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsLedgerPostingFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
</dxe:ASPxCallbackPanel>
    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>