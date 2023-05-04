<%--================================================== Revision History ============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                17-02-2023        V2.0.36           Pallab              25575 : Report pages design modification
2.0                03-05-2023        V2.0.38           Pallab              26016 : Vendor Ledger module zoom popup upper part visible issue fix for small device
====================================================== Revision History ================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Vendor_Ledger.aspx.cs" Inherits="Reports.Reports.GridReports.Vendor_Ledger" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
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

        .dxgvControl_PlasticBlue#ShowGrid {
            width: 100% !important;
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

        #ListBoxBranches {
            width: 200px;
        }

        #ListBoxProjects{
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }

        .btnOkformultiselection {
            border-width: 1px;
            padding: 4px 10px;
            font-size: 13px !important;
            margin-right: 6px;
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


    <script type="text/javascript">

        //function ClearGridLookup() {
        //    var grid = gridquotationLookup.GetGridView();
        //    grid.UnselectRows();
        //}

        function GetChecked() {
            if ($("#chkallvendors").is(":checked") == true) {
                gridquotationLookup.SetEnabled(false);
                gridquotationLookup.SetValue(null);
            }
            else {
                gridquotationLookup.SetEnabled(true);
            }

        }

        $(function () {
            cProjectPanel.PerformCallback('BindProjectGrid');
        });

        $(function () {

            // BindBranches(null);
            //    BindCustomerVendor(0);


            cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);

            //cGroupComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);  //Suvankar


            //    cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);


            //cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + 2);

            // cVendorComponentPanel.PerformCallback('BindComponentGrid' + '~' + 2);


            $("#drp_partytype").change(function () {
                var end = $("#drp_partytype").val();

                if (end == '1') {

                    $("#Label3").text('Customer');
                }
                else if (end == '2') {

                    $("#Label3").text('Vendor');
                }
                else if (end == '0') {


                    $("#Label3").text('Customer/Vendor');
                }

                BindCustomerVendor(end);
            });
        });

        function CallbackPanelEndCall(s, e) {
            <%--Rev Subhra 18-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }


        function BindBranches(noteTilte) {
            var lBox = $('select[id$=ListBoxBranches]');
            var listItems = [];
            var selectedNoteId = '';
            if (noteTilte) {

                selectedNoteId = noteTilte;
            }
            lBox.empty();


            $.ajax({
                type: "POST",
                url: 'PartyLedgerPostingReport.aspx/GetBranchesList',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify({ NoteId: selectedNoteId }),
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
                        ListActivityType();

                        $('#ListBoxBranches').trigger("chosen:updated");
                        $('#ListBoxBranches').prop('disabled', false).trigger("chosen:updated");
                    }
                    else {
                        lBox.empty();
                        $('#ListBoxBranches').trigger("chosen:updated");
                        $('#ListBoxBranches').prop('disabled', true).trigger("chosen:updated");

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //  alert(textStatus);
                }
            });


        }


        //function BindGroups(noteTilte) {    //Suvankar
        //    var lBox = $('select[id$=ListBoxGroup]');
        //    var listItems = [];
        //    var selectedNoteId = '';
        //    if (noteTilte) {

        //        selectedNoteId = noteTilte;
        //    }
        //    lBox.empty();


        //    $.ajax({
        //        type: "POST",
        //        url: 'PartyLedgerPostingReport.aspx/GetGroupList',
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
        //                ListActivityTypeGroup();

        //                $('#ListBoxGroup').trigger("chosen:updated");
        //                $('#ListBoxGroup').prop('disabled', false).trigger("chosen:updated");
        //            }
        //            else {
        //                lBox.empty();
        //                $('#ListBoxGroup').trigger("chosen:updated");
        //                $('#ListBoxGroup').prop('disabled', true).trigger("chosen:updated");

        //            }
        //        },
        //        error: function (XMLHttpRequest, textStatus, errorThrown) {
        //            //  alert(textStatus);
        //        }
        //    });


        //}



        function ListActivityType() {

            $('#ListBoxBranches').chosen();
            $('#ListBoxBranches').fadeIn();

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
        function ListActivityTypeGroup() {   //Suvankar

            $('#ListBoxGroup').chosen();
            $('#ListBoxGroup').fadeIn();

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



        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                // BindLedgerType(Ids);                    

                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })

            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            if (ProjectSelection == "0") {
                $('#divProj').addClass('hidden');
            }
            else {
                $('#divProj').removeClass('hidden');
            }

            $("#ListBoxGroup").chosen().change(function () {   //Suvankar
                var Ids = $(this).val();

                $('#<%=hdnSelectedGroups.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })

                 <%-- $("#ListBoxLedgerType").chosen().change(function () {
                 var Ids = $(this).val();

                 $('#<%=hdnSelectedLedger.ClientID %>').val(Ids);
                 $('#MandatoryLedgerType').attr('style', 'display:none');

             })--%>

            $("#ListBoxCustomerVendor").chosen().change(function () {
                var Ids = $(this).val();

              <%--  $('#<%=hdnSelectedCustomerVendor.ClientID %>').val(Ids);
                $('#MandatoryCustomerType').attr('style', 'display:none');--%>

            })

        })



        function BindCustomerVendor(type) {


            // cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + 'CL');
            //cVendorComponentPanel.PerformCallback('BindComponentGrid' + '~' + 'DV');
        }




    </script>

    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'GroupSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#GroupModel').modal('hide');
                    ctxtGroup.SetText(Name);
                    GetObjectID('hdnSelectedGroups').value = key;

                    ctxtVendName.SetText('');
                    $('#txtVendSearch').val('')

                    //when vendor tagged with group and want to de select and clear text previously selected vendor.
                    var OtherDetailsvend = {}
                    OtherDetailsvend.SearchKey = "NoVendorWithAnyGroup";
                    var HeaderCaptionVend = [];
                    HeaderCaptionVend.push("Vendor Name");
                    //callonServerM("Services/Master.asmx/GetVendor", OtherDetailsvend, "VendorTable", HeaderCaptionVend, "dPropertyIndex", "SetSelectedValues", "VendorSource");
                    callonServerM("Services/Master.asmx/GetGroupWiseVendor", OtherDetailsvend, "VendorTable", HeaderCaptionVend, "dPropertyIndex", "SetSelectedValues", "VendorSource");
                }
                else {
                    ctxtGroup.SetText('');
                    GetObjectID('hdnSelectedGroups').value = '';
                }
            }
            else if (ArrName == 'VendorSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#VendModel').modal('hide');
                    ctxtVendName.SetText(Name);
                    GetObjectID('hdnVendorId').value = key;
                }
                else {
                    ctxtVendName.SetText('');
                    GetObjectID('hdnVendorId').value = '';
                }
            }

        }

    </script>
    <%-- For multiselection when click on ok button--%>

    <%-- For Group multiselection--%>
    <script type="text/javascript">

        $(document).ready(function () {
            $('#GroupModel').on('shown.bs.modal', function () {
                $('#txtGroupSearch').focus();
            })
        })

        var GroupArr = new Array();
        $(document).ready(function () {
            var GroupObj = new Object();
            GroupObj.Name = "GroupSource";
            GroupObj.ArraySource = GroupArr;
            arrMultiPopup.push(GroupObj);
        })
        function GroupButnClick(s, e) {
            $('#GroupModel').modal('show');
        }

        function Group_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#GroupModel').modal('show');
            }
        }

        function Groupkeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtGroupSearch").val()) == "" || $.trim($("#txtGroupSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtGroupSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {

                var HeaderCaption = [];
                HeaderCaption.push("Group Description");

                if ($("#txtGroupSearch").val() != "") {
                    callonServerM("Services/Master.asmx/GetGroup", OtherDetails, "GroupTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "GroupSource");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[dPropertyIndex=0]"))
                    $("input[dPropertyIndex=0]").focus();
            }
        }

        function SetfocusOnseach(indexName) {
            if (indexName == "dPropertyIndex")
                $('#txtGroupSearch').focus();
            else
                $('#txtGroupSearch').focus();
        }
    </script>
    <%-- For Group multiselection--%>


    <%-- For Vendor multiselection--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#VendModel').on('shown.bs.modal', function () {
                $('#txtVendSearch').focus();
            })
        })

        var VendArr = new Array();
        $(document).ready(function () {
            var VendObj = new Object();
            VendObj.Name = "VendorSource";
            VendObj.ArraySource = VendArr;
            arrMultiPopup.push(VendObj);
        })
        function VendorButnClick(s, e) {
            $('#VendModel').modal('show');
        }

        function Vendor_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#VendModel').modal('show');
            }
        }

        function Vendorkeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtVendSearch").val()) == "" || $.trim($("#txtVendSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtVendSearch").val();
            OtherDetails.GroupIDs = hdnSelectedGroups.value;
            OtherDetails.Type = "2";

            if (e.code == "Enter" || e.code == "NumpadEnter") {

                var HeaderCaption = [];
                HeaderCaption.push("Vendor Name");

                //callonServerM("Services/Master.asmx/GetGroupWiseVendor", OtherDetails, "VendorTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "VendorSource");
                callonServerM("Services/Master.asmx/GetVendor", OtherDetails, "VendorTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "VendorSource");

                //if (ctxtGroup.GetValue() == "" || ctxtGroup.GetValue() == null) {
                //    if ($("#txtVendSearch").val() != "") {
                //        callonServerM("Services/Master.asmx/GetVendor", OtherDetails, "VendorTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "VendorSource");
                //    }
                //}
                //else {
                //    if ($("#txtVendSearch").val() != "") {
                //        OtherDetails.Type = "2";
                //        callonServerM("Services/Master.asmx/GetGroupWiseVendor", OtherDetails, "VendorTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "VendorSource");
                //    }
                //}

            }
            else if (e.code == "ArrowDown") {
                if ($("input[dPropertyIndex=0]"))
                    $("input[dPropertyIndex=0]").focus();
            }
        }

        //function SetSelectedValues(Id, Name) {
        //    var key = Id;
        //    if (key != null && key != '') {
        //        $('#VendModel').modal('hide');
        //        ctxtVendName.SetText(Name);
        //        GetObjectID('hdnVendorId').value = key;
        //    }
        //    else {
        //        ctxtVendName.SetText('');
        //        GetObjectID('hdnVendorId').value = '';
        //    }
        //}


        function SetfocusOnseach(indexName) {
            if (indexName == "dPropertyIndex")
                $('#txtVendSearch').focus();
            else
                $('#txtVendSearch').focus();
        }
    </script>
    <%-- For Vendor multiselection--%>


    <script type="text/javascript">


        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // Grid.PerformCallback('');
        }

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            var ProjectSelectionInReport = document.getElementById('hdnProjectSelectionInReport').value;
            e.preventDefault;
            var data = "OnDateChanged";
            $("#drdExport").val(0);
            $("#hfIsVendFilter").val("Y");
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            //CallServer(data, "");
            // alert( data);

            if ($("#chkallvendors").is(":checked") == false) {
                //if (ctxtVendName.GetValue() == null) {
                if (ctxtVendName.GetValue() == null && ctxtGroup.GetValue() == null) {
                    jAlert('Please select atleast one Vendor.');
                }
                else if (ProjectSelection == "1") {
                    if (ProjectSelectionInReport == "Yes" && gridprojectLookup.GetValue() == null) {
                        jAlert('Please select atleast one Project for generate the report.');
                    }
                    else if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                        jAlert('Please select atleast one branch for generate the report.');
                    }
                    else {
                        cCallbackPanel.PerformCallback();
                    }
                }
                else {
                    //cCallbackPanel.PerformCallback();
                    if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                        jAlert('Please select atleast one branch for generate the report.');
                    }
                    else {
                        cCallbackPanel.PerformCallback();
                    }
                    //Grid.PerformCallback(data);
                }
            }
            else {
                //cCallbackPanel.PerformCallback();
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback();
                }
                //Grid.PerformCallback(data);
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

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function abc() {
            $("#drdExport").val(0);
        }
        function OpenPOSDetails(Uniqueid, type, docno) {
            //  alert(type);

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
            else if (type == 'PR') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///    window.open('/OMS/Management/Activities/BranchRequisitionReturn.aspx?key=' + Uniqueid + '&req=V', '_blank')
                url = '/OMS/Management/Activities/PReturn.aspx.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=PR';

            }
            else if (type == 'SC') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                /// window.open('/OMS/Management/Activities/CustomerReturn.aspx?key=' + Uniqueid + '&req=V&type=' + type, '_blank')

                url = '/OMS/Management/Activities/CustomerReturn.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }
            else if (type == 'CBV') {
                url = '/OMS/Management/dailytask/CashBankEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=' + type;
            }
            else if (type == 'JV') {
                url = '/OMS/Management/dailytask/JournalEntry.aspx?key=' + Uniqueid + '&IsTagged=1&req=' + docno;
            }
            else if (type == 'CP' || type == 'CR') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///    window.open('/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&req=V&type=CRP', '_blank')
                url = '/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CRP';
            }
            else if (type == 'CDN' || type == 'CCN') {
                ///    window.location.href = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + invoice + '&req=V&type=' + type;

                ///    window.open('/OMS/Management/Activities/CustomerReceiptPayment.aspx?key=' + Uniqueid + '&req=V&type=CRP', '_blank')
                url = '/OMS/Management/Activities/CustomerNote.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=CDCN';
            }
            else if (type == 'DNV' || type == 'CNV') {
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
    <%--    function CloseGridQuotationLookup() {
            if ($('#<%=hdfVendor.ClientID %>').val() == '1')
            { $('#<%=hdfVendor.ClientID %>').val('0'); }
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
           
            gridquotationLookup.Focus();
        }--%>

        function CloseGridLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function selectAll_Project() {
            gridprojectLookup.gridView.SelectRows();
        }
        function unselectAll_Project() {
            gridprojectLookup.gridView.UnselectRows();
        }
        function CloseGridProjectLookup() {
            gridprojectLookup.ConfirmCurrentSelection();
            gridprojectLookup.HideDropDown();
            gridprojectLookup.Focus();
        }
        <%--  function CloseGridQuotationLookupGroup() {  //Suvankar
            gridGroupLookup.ConfirmCurrentSelection();
            gridGroupLookup.HideDropDown();
            gridGroupLookup.Focus();
            gridquotationLookup.gridView.UnselectRows();
            $('#<%=hdfVendor.ClientID %>').val('1');
            gridquotationLookup.gridView.Refresh();

        }--%>
        <%--   function CloseGridQuotationLookupGroupLostFocus()
        {
            gridquotationLookup.gridView.UnselectRows();
             $('#<%=hdfVendor.ClientID %>').val('Y');
              gridquotationLookup.gridView.Refresh();   
        }--%>

        function Groupwiseledger() {
            var key = gridGroupLookup.GetGridView().GetRowKey(gridGroupLookup.GetGridView().GetFocusedRowIndex());
            //cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + 'GrpWs');
            //// cVendorComponentPanel.PerformCallback('BindComponentGrid' + '~' + 'GrpWs');

            gridquotationLookup.gridView.Refresh();
        }
        function Callback2_EndCallback() {
            $("#drdExport").val(0);
        }
        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
        }
        //function selectAllGroup() {  //Suvankar
        //    gridGroupLookup.gridView.SelectRows();
        //}
        //function unselectAllGroup() {  //Suvankar
        //    gridGroupLookup.gridView.UnselectRows();
        //}

        //function selectAll_vendor() {
        //    gridquotationLookup.gridView.SelectRows();
        //}

        //function unselectAll_vendor() {
        //    gridquotationLookup.gridView.UnselectRows();
        //}

    </script>
    <style>
        .paddtable > tbody > tr > td {
            padding-bottom: 10px;
        }
    </style>

    <style>
        #ShowGrid{
            width: 100% !important;
        }
    </style>

    <style>
        .plhead a {
            font-size: 16px;
            padding-left: 10px;
            position: relative;
            width: 100%;
            display: block;
            padding: 9px 10px 5px 10px;
        }

            .plhead a > i {
                position: absolute;
                top: 11px;
                right: 15px;
            }

        #accordion {
            margin-bottom: 10px;
        }

        .companyName {
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 15px;
        }


        .plhead a.collapsed .fa-minus-circle {
            display: none;
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
                margin-bottom: 0 !important;
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

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img
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
            right: -2px;
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
        .simple-select:disabled::after
        {
            background: #1111113b;
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

        /*table
        {
            max-width: 99% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f7f7f7;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/

        /*Rev 2.0*/

        #ASPXPopupControl2_PW-1 , #popupApproval_PW-1 , #ASPXPopupControl1_PW-1
        {
            position: fixed !important;
            top: 10% !important;
            left: 10% !important;
        }

        @media only screen and (max-width: 1450px) and (min-width: 1300px)
        {
            #ASPXPopupControl2_PW-1 , #popupApproval_PW-1 , #ASPXPopupControl1_PW-1
            {
                /*position:fixed !important;*/
                left: 60px !important;
                top: 8% !important;
            }
        }

        /*Rev end 2.0*/
    </style>

    <script>
        $(document).ready(function () {
            $('.navbar-minimalize').click(function () {
                Grid.Refresh();
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">
        <%--<div class="panel-title">
            <h3>Vendor Ledger</h3>
        </div>--%>
        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
            <div class="panel panel-info">
                <div class="panel-heading" role="tab" id="headingOne">
                    <h4 class="panel-title plhead">
                        <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="collapsed">
                            <asp:Label ID="RptHeading" runat="Server" Text="" Width="470px" Style="font-weight: bold;"></asp:Label>
                            <i class="fa fa-plus-circle"></i>
                            <i class="fa fa-minus-circle"></i>
                        </a>
                    </h4>
                </div>
                <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                    <div class="panel-body">
                        <div class="companyName">
                            <asp:Label ID="CompName" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                          <%--Rev Subhra 11-12-2018   0017670--%>
                            <div>
                                <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
                            </div>
                          <%--End of Rev--%>
                        <div>
                            <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompOth" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompPh" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="DateRange" runat="Server" Text="" Width="470px"></asp:Label>
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
            <div class="col-md-2 branch-selection-box">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>
                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentBranchPanel" ClientInstanceName="cBranchComponentPanel" OnCallback="Componentbranch_Callback">
                        <%--<dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchComponentPanel">--%>
                        <panelcollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                    OnDataBinding="lookup_branch_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <%--DataSourceID="BranchEntityServerModeDataSource" --%>
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" width="200px" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="branch_description" SortOrder="Ascending" Visible="true" width="200px" VisibleIndex="2" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
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
                                                                <dxe:ASPxButton ID="ASPxButtonselect" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" UseSubmitBehavior="False"/>
                                                           <%-- </div>--%>
                                                            <dxe:ASPxButton ID="ASPxButton1unselect" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" UseSubmitBehavior="False"/>
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookupbranch" UseSubmitBehavior="False" />
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
                        </panelcollection>
                    </dxe:ASPxCallbackPanel>

                    <%--<dx:linqservermodedatasource id="BranchEntityServerModeDataSource" runat="server" onselecting="BranchEntityServerModeDataSource_Selecting"
                        contexttypename="ERPDataClassesDataContext" tablename="v_BranchList" />--%>
                </div>
            </div>

            <div class="col-md-2">
                <%-- <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="Group : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>
                   
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="HiddenField3" runat="server" />
                     <dxe:ASPxCallbackPanel runat="server" ID="GroupComponentPanel" ClientInstanceName="cGroupComponentPanel" OnCallback="ComponentGroup_Callback">

                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_Group" SelectionMode="Multiple" runat="server" ClientInstanceName="gridGroupLookup"
                                    OnDataBinding="lookup_Group_DataBinding"
                                    KeyFieldName="GroupCode" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="GroupCode" Visible="true" Width="0" VisibleIndex="1" Caption="Group Code" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="GroupDescription" SortOrder="Ascending" Visible="true" Width="160" VisibleIndex="2" Caption="Group Description" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllGroup" />
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllGroup" />
                                                            <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookupGroup" UseSubmitBehavior="False" />
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
                                    <ClientSideEvents ValueChanged="Groupwiseledger" LostFocus="CloseGridQuotationLookupGroupLostFocus" />
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>--%>

                <dxe:ASPxLabel ID="ASPxLabel1" style="color: #141414; font-weight: bold;" runat="server" Text="Group :">
                </dxe:ASPxLabel>
                <dxe:ASPxButtonEdit ID="txtGroup" runat="server" ReadOnly="true" ClientInstanceName="ctxtGroup" Width="100%" TabIndex="5">
                    <buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </buttons>
                    <clientsideevents buttonclick="function(s,e){GroupButnClick();}" keydown="function(s,e){Group_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>


            </div>

            <div class="col-md-2">
                <%-- <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="Vendor : " CssClass="mylabel1"
                        Width="110px"></asp:Label>
                </div>
                <div>
                     <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cVendorComponentPanel" OnCallback="ComponentVendor_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_vendor" SelectionMode="Multiple" runat="server" TabIndex="7" ClientInstanceName="gridquotationLookup"
                                    OnDataBinding="lookup_vendor_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="Name" SortOrder="Ascending" Visible="true" VisibleIndex="1" Caption="Name" Width="180" Settings-AutoFilterCondition="Contains">
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
                                                                <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_vendor" />
                                                            </div>
                                                            <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_vendor" />                                                            
                                                            <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                        <SettingsPager Mode="ShowPager">
                                        </SettingsPager>

                                        <SettingsPager PageSize="10">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                        </SettingsPager>

                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>                                  
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>                       
                    </dxe:ASPxCallbackPanel>
                   
                    <asp:HiddenField ID="hdnSelectedCustomerVendor" runat="server" />
                    <span id="MandatoryCustomerType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                </div>--%>

                <dxe:ASPxLabel ID="lbl_Vendor" style="color: #141414; font-weight: bold;" runat="server" Text="Vendor :">
                </dxe:ASPxLabel>
                <dxe:ASPxButtonEdit ID="txtVendName" runat="server" ReadOnly="true" ClientInstanceName="ctxtVendName" Width="100%" TabIndex="6">
                    <buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </buttons>
                    <clientsideevents buttonclick="function(s,e){VendorButnClick();}" keydown="function(s,e){Vendor_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </div>

            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <buttonstyle width="13px">
                        </buttonstyle>
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
                    <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </div>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>

            <div class="col-md-2">
               <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                <div style="padding-top: 16px">
                    <div style="padding-right: 5px; vertical-align: middle; padding-top: 6px">
                        <asp:CheckBox ID="chkparty" runat="server" Checked="true" />
                        Consider Party Inv. Date
                    </div>
                </div>
               </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-2 hide">
                <div class="hide">
                    <div style="padding-left: 1px">
                        <div style="padding-left: 1px; vertical-align: middle; padding-top: 6px">
                            <asp:CheckBox ID="chkallvendors" runat="server" onChange="GetChecked()" />
                            Select all Vendors
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-2" style="padding-top: 10px; width:235px" id="divProj">
                <div style="color: #b5285f; /*font-weight: bold;*/" class="clsTo">
                    <asp:Label ID="lblProj" runat="Server" Text="Project : " CssClass="mylabel1"></asp:Label>
                </div>
                <asp:ListBox ID="ListBoxProjects" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="60px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                <dxe:ASPxCallbackPanel runat="server" ID="ProjectPanel" ClientInstanceName="cProjectPanel" OnCallback="Project_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_project" SelectionMode="Multiple" runat="server" ClientInstanceName="gridprojectLookup"
                                OnDataBinding="lookup_project_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="project_code" Visible="true" VisibleIndex="1" width="200px" Caption="Project code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="project_name" Visible="true" VisibleIndex="2" width="200px" Caption="Project Name" Settings-AutoFilterCondition="Contains">
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
                                                            <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Project" UseSubmitBehavior="False"/>
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Project" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridProjectLookup" UseSubmitBehavior="False" />
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

                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedProjects" runat="server" />
            </div>

            <div class="col-md-2">
                <div style="padding-top: 30px">
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <table class="pull-left paddtable">
            <tr>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
        </table>
        <div class="pull-right">
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback2_EndCallback"
                            DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Auto">
                            <%--OnDataBound="Showgrid_Htmlprepared" OnCustomSummaryCalculate="dgvVIEW_CustomSummaryCalculate" OnCustomCallback="Grid_CustomCallback" Settings-HorizontalScrollBarMode="Visible" Settings-VerticalScrollableHeight="180" Settings-VerticalScrollBarMode="Auto"--%>
                            <columns>
                                <%--<dxe:GridViewDataTextColumn FieldName="RID" Caption="RID" Width="50px" VisibleIndex="1" />--%>
                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" Width="200px" VisibleIndex="1" />
                                <%--<dxe:GridViewDataTextColumn FieldName="CustomerVendor" Caption="Customer/Vendor" Width="15%" VisibleIndex="3" />--%>
                                <dxe:GridViewDataTextColumn FieldName="DATE" Caption="Transaction Date" Width="150px" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />

                                <%--<dxe:GridViewDataTextColumn FieldName="DOCUMENT_NO" Caption="DOCUMENT NO." Width="12%" VisibleIndex="5" />--%>


                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Voucher_No" Caption="Voucher No." Width="150px">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>

                                        <a href="javascript:void(0)" onclick="OpenPOSDetails('<%#Eval("DOC_ID") %>','<%#Eval("MODULE_TYPE") %>','<%#Eval("Voucher_No") %>')" class="pad">
                                            <%#Eval("Voucher_No")%>
                                        </a>
                                    </DataItemTemplate>

                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CustomerVendor" Caption="Vendor Name" Width="250px" VisibleIndex="4" />

                                <dxe:GridViewDataTextColumn FieldName="PROJ_NAME" Caption="Project Name" Width="200px" VisibleIndex="5" />

                                <dxe:GridViewDataTextColumn FieldName="GROUPNAME" Caption="Group Desc." Width="250px" VisibleIndex="6" />

                                <dxe:GridViewDataTextColumn FieldName="Party_InvoiceNo" Caption="Party Invoice No" VisibleIndex="7" Width="150px" />

                                <dxe:GridViewDataTextColumn FieldName="Party_InvoiceDate" Caption="Party Invoice Date" VisibleIndex="8" Width="150px" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />


                                <%--<dxe:GridViewDataTextColumn FieldName="MODULE_TYPE" Caption="MODULE TYPE" VisibleIndex="7" />--%>
                                <dxe:GridViewDataTextColumn FieldName="TYPE" Caption="Voucher Type" Width="150px" VisibleIndex="9" />
                                <dxe:GridViewDataTextColumn FieldName="Particulars" Caption="Particulars" Width="200px" VisibleIndex="10" />
                                <dxe:GridViewDataTextColumn FieldName="Header_Narration" Caption="Header Narration" Width="300px" VisibleIndex="11" />
                                <dxe:GridViewDataTextColumn FieldName="DEBIT" Caption="Debit" VisibleIndex="12" Width="120px" >
                                    <%--PropertiesTextEdit-DisplayFormatString="0.00" /--%>
                                 <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CREDIT" Caption="Credit" Width="120px" VisibleIndex="13" >
                                    <%--PropertiesTextEdit-DisplayFormatString="0.00" /--%>
                                <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Closing_Balance" Caption="Balance" Width="120px" VisibleIndex="14" FooterCellStyle-HorizontalAlign="Right">
                                    <%--PropertiesTextEdit-DisplayFormatString="0.00" /--%>
                                 <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Closebal_DBCR" Caption="Type" Width="80px" VisibleIndex="15" />

                            </columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" AllowSort ="false" />
                            <settings showfooter="true" showgrouppanel="true" showgroupfooter="VisibleIfExpanded" />
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior autoexpandallgroups="true" columnresizemode="Control" />
                            <settings showgrouppanel="false" showstatusbar="Visible" showfilterrow="true" />
                            <settingssearchpanel visible="false" />
                            <settingspager pagesize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </settingspager>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                            <totalsummary>
                                <dxe:ASPxSummaryItem FieldName="DEBIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CREDIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Closing_Balance" SummaryType="Custom" >
                                </dxe:ASPxSummaryItem>
                                <dxe:ASPxSummaryItem FieldName="Closebal_DBCR" SummaryType="Custom" />
                            </totalsummary>
                        </dxe:ASPxGridView>

                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                            ContextTypeName="ReportSourceDataContext" TableName="CUSTVENDLEDGER_REPORT" />

                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <panelcollection>
            <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsVendFilter" runat="server" />
            </dxe:PanelContent>
        </panelcollection>
        <clientsideevents endcallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <!--Vendor Modal -->
    <div class="modal fade" id="VendModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Vendor Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Vendorkeydown(event)" id="txtVendSearch" width="100%" placeholder="Search By Vendor Name" />
                    <div id="VendorTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">id</th>
                                <th>Vendor Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default" onclick="DeSelectAll('VendorSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('VendorSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <!--Vendor Modal -->

    <!--Group Modal -->
    <div class="modal fade" id="GroupModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Group Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Groupkeydown(event)" id="txtGroupSearch" width="100%" placeholder="Search By Group Name" />
                    <div id="GroupTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">id</th>
                                <th>Group Description</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default" onclick="DeSelectAll('GroupSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('GroupSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <!--Group Modal -->

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <contentcollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </contentcollection>
        <clientsideevents closeup="BudgetAfterHide" />
    </dxe:ASPxPopupControl>

    <asp:HiddenField ID="hdnVendorId" runat="server" />
    <asp:HiddenField ID="hdfVendor" runat="server" />
    <asp:HiddenField ID="hdnSelectedGroups" runat="server" />
    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectionInReport" runat="server" />
</asp:Content>

