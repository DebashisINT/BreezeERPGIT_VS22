<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                30-03-2023        2.0.36           Pallab              25768: CRM pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Activities.aspx.cs" Inherits="ERP.OMS.Management.Master.Activities" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://code.jquery.com/jquery-2.2.4.js"></script>
<script src="https://rawgit.com/RobinHerbots/jquery.inputmask/3.x/dist/jquery.inputmask.bundle.js"></script>

      <script src="../Activities/JS/SearchPopup.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="Js/ActivityProducts.js"></script>
    <link href="../../../assests/css/custom/PMSStyles.css" rel="stylesheet" />

    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <link href="../../../ckeditor/contents.css" rel="stylesheet" />
    <%--<script src="../../../ckeditor/jquery-1.10.2.min.js"></script>--%>
    <script src="../../../ckeditor/ckeditor.js"></script>
    <link rel="stylesheet" href="http://cdn.datatables.net/1.10.19/css/jquery.dataTables.min.css" />
    <script src="http://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    
    <%--Rev Mantis Issue 22801_Sushanta--%>
    <link href="/assests/pluggins/TimePicker/bootstrap-timepicker.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/TimePicker/bootstrap-timepicker.min.js"></script>
    <%--End of Rev Mantis Issue 22801_Sushanta--%>

    <%--//Subhra--%>
    <style>
         #tbodyActivityProduct>td{
             padding: 2px 25px;
         }
         #dataTbl_wrapper .dataTables_scrollHeadInner table>thead>tr>th:not(:last-child) {
             border-right:#333;
         }

          .boxStyle {
            padding: 5px;
    background: #f7f7f7;
    margin: 0 15px 8px 15px;
    border: 1px solid #ccc;
        }
        .link {
            cursor:pointer;
        }
        .pdLeft0 {
            padding-left:0 !important;
        }
        #dataTbl_wrapper .dataTables_scrollHeadInner, #dataTbl_wrapper .dataTables_scrollHeadInner table {
            width:100% !important;
        }
         #dataTbl_wrapper .dataTables_scrollHeadInner table>thead>tr>th {
             background:#337ab7;
             color:#fff;
             padding: 2px 15px;
         }
         #tbodyActivityProduct>td{
             padding: 2px 25px;
         }
         #dataTbl_wrapper .dataTables_scrollHeadInner table>thead>tr>th:not(:last-child) {
             border-right:#333;
         }
         .modal-footer .btn {
             margin-top:0;
             margin-bottom:0;
         }
         /*Rev Mantis Issue 22801_Sushanta*/
        .bootstrap-timepicker-widget table td input {
            margin: 0 auto !important;
        }

        .bootstrap-timepicker-meridian {
            font-size: 11px;
        }
        /*End of Rev Mantis Issue 22801_Sushanta*/

    </style>
    <%--//--For Product Button on Lead Acticity--%>
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

        #ListBoxBranches {
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
               #ActivityPopup_PW-1 .dxpc-header, #popupShowHistory_PW-1 .dxpc-header {
                background: #3ca1e8 ;
                background-image: none !important;
                padding: 11px 20px;
                border: none;
                    border-radius: 15px 15px 0 0;
            }
            #ActivityPopup_PW-1 .dxpc-contentWrapper, #popupShowHistory_PW-1 .dxpc-contentWrapper {
                    background: #fff;
                    border-radius:0 0 15px 15px;
            }
             #ActivityPopup_PW-1 .dxpc-mainDiv, #popupShowHistory_PW-1 .dxpc-mainDiv {
                 background-color:transparent !important;
             }
             #ActivityPopup_PW-1 .modal-footer, #popupShowHistory_PW-1 .modal-footer {
                 text-align:left;
             }
             #ActivityPopup_PW-1 .dxpc-shadow, #popupShowHistory_PW-1 .dxpc-shadow {
                 box-shadow:none;
             }
             .myAssignTarget {
        margin-bottom: 0;
    }
         #cmbPriority {
            border-radius:3px;
        }
        .myAssignTarget > li {
            list-style-type: none;
            display: inline-block;
            font-size: 11px;
            text-align: center;
        }

            .myAssignTarget > li:not(:last-child) {
                margin-right: 15px;
            }

            .myAssignTarget > li.mainCircle {
                border: 1px solid #a2d3d8;
                border-radius: 8px;
                overflow: hidden;
            }

            .myAssignTarget > li .heading {
                padding: 2px 12px;
                background: #6d82c5;
                color: #fff;
            }

            .myAssignTarget > li .Num {
                font-size: 14px;
            }

            .myAssignTarget > li.mainHeadCenter {
                font-size: 12px;
                transform: translateY(-16px);
            }

            #myAssignTargetpopup {
                padding: 0;
            }

            #myAssignTargetpopup > li .heading {
                padding: 6px 12px;
                background: #7f96dc;
                font-weight: 600;
                color: #fff;
            }

            #myAssignTargetpopup li .Num {
                font-size: 14px;
                padding: 5px;
            }
            #ContactModel {
                z-index:99999
            }
             #ProductModel {
                z-index:9999999
            }
             .btn-product {
                 margin-top: 13px;
                background: #4c9e9e;
                color: #fff;
                box-shadow: 0 3px 0 #3b7d7d;
                -webkit-transition:all 0.2s ease;
                -moz-transition:all 0.2s ease;
                transition:all 0.2s ease;
             }
             .btn-product:hover {
                 box-shadow: 0 3px 0 #3b7d7d, 0 5px 3px rgba(0,0,0,0.12);
                 background: #159e9e;
                 -webkit-transform:translateY(-3px);
                 -moz-transform:translateY(-3px);
                 transform:translateY(-3px);
                 color:#fff;
             }
             #dataTbl {
                 width:100%;
                 border:1px solid #dedada;
             }
             #dataTbl>thead>tr>th {
                 background:#4b60a2;
                 color:#fff;
             }
             #dataTbl>thead>tr>th, #dataTbl>tbody>tr>td {
                 padding:9px 8px;
             }
             #dataTbl>tbody>tr:not(:last-child)>td {
                 border-bottom:1px solid #dedada;
             }
             #dataTbl>tbody>tr>td:not(:last-child) {
                 border-right:1px solid #dedada;
             }
             .iconCD {
                display: inline-block;
                margin-right: 10px;

             }

              .badge {
            display: inline-block;
            min-width: 10px;
            padding: 5px 8px;
            font-size: 12px;
            font-weight: bold;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #929ef0;
            border-radius: 10px;
        }

        .btn .badge {
            background: #fff;
            color: #333;
            margin-left: 4px;
            font-size: 14px;
        }

        .btn-badge-color1:hover, .btn-badge-color2:hover, .btn-badge-color3:hover, .btn-badge-color4:hover, .btn-badge-color5:hover, .btn-badge-color6:hover,
        .btn-badge-color1:focus, .btn-badge-color2:focus, .btn-badge-color3:focus, .btn-badge-color4:focus, .btn-badge-color5:focus, .btn-badge-color6:focus {
            color: #fff;
            opacity: 0.8;
        }

        .btn-badge-color1 {
            /*background:chocolate;
            color:#fff;*/
            background: #d9c015;
            color: #fff;
        }

        .btn-badge-color2 {
            background: #30cb57;
            color: #fff;
        }

        .btn-badge-color3 {
            background: #ff6a00;
            color: #fff;
        }

        .btn-badge-color4 {
            background: #7a6fd6;
            color: #fff;
        }

        .btn-badge-color5 {
            background: #6272e2;
            color: #fff;
        }

        .btn-badge-color6 {
            background: #3366FF;
            color: #fff;
        }

        .nocursor {
            cursor: default;
        }

        .auto-style1 {
            height: 145px;
        }

        .m0 {
            margin-top: 0 !important;
        }

        .mBot0 {
            margin-bottom: 0 !important;
        }


    </style>
 <script>
     $(function () {

         $('input[id$="endTime"]').inputmask(
           "hh:mm:ss", {
               placeholder: "HH:MM:SS",
               insertMode: false,
               showMaskOnHover: false,
               hourFormat: 24
           }
         );


     });
  </script>
    <script type="text/javascript">


        function btn_TodayFollowUp(e) {
            $("#hfActivityFilter").val("TodayFollowup");
            Grid.Refresh();
        }
        function btn_YesterdayFollowUp(e) {
            $("#hfActivityFilter").val("YesterdayFollowup");
            Grid.Refresh();
        }
        function btn_AllFollowUp(e)
        {
            $("#hfActivityFilter").val("Y");
            cCallbackPanel.PerformCallback('Load~' + $("#hfActivityFilter").val());
        }

        
        function ClearGridLookup() {
            var grid = gridquotationLookup.GetGridView();
            grid.UnselectRows();
        }


        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }
        function CallbackPanelEndCall(s, e) {
            Grid.Refresh();
        }


        function OnAddActivitiesbuttonClick() {
            cdtActivityDate.SetDate(new Date());
            //$("#cmbContactType").val("");
            ctxtContact.SetText("")
            
            //$("#cmbActivity").val("");
            $("#txtSubject").val("");
            $("#txtDetails").val("");

            

            //$("#ddlPriority").val(0);
            //cDue_dt.SetDate(new Date());


            $("#cmbActivity").val(0);
            $("#txtSubject").text('');
            $("#cmbType").val(0);
            $("#txtDetails").text('');
            $("#cmbSalesActivityAssignTo").val(0);
            $("#cmbDuration").val(0);
            $("#cmbPriority").val(0);
            $("#cmbContactType").val(0);
            $("#btnClear").addClass('hide');
            $("#btnSave").removeClass('hide');
            // Rev Mantis Issue 22801
            $("#timepicker1").val("00:00:00");
            // End of Rev Mantis Issue 22801
        }


    </script>
    <%--//For Product Button on Lead Acticity--%>
     <script type='text/javascript' >

         function CountDocument()
         {
             $.ajax({
                 type: "POST",
                 url: "Activities.aspx/ButtonCountShow",
                 data: JSON.stringify(),
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: false,
                 success: function (msg) {

                     var status = msg.d;
                     //debugger;
                     //if (msg.d.All != null) {
                     $("#spTodayFollowUp").html(status.split("~")[0]);
                     $("#spYesterFollowUp").html(status.split("~")[1]);
                     $("#spAllFollowUp").html(status.split("~")[2]);

                 }
             });

         }


         $(document).ready(function () {

             CountDocument();
             btn_TodayFollowUp();
           
         })


         var ActivityProduct = [];
         var ActivityProductProductId = "";
    </script>
    <%--//For Product Button on Lead Acticity--%>

    <script type="text/javascript">
       
        //for Esc
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && cpopupApproval.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                popupHide();
            }
        }
        function popupHide(s, e) {
            cpopupApproval.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
        }

        function CloseGridQuotationLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            gridquotationLookup.Focus();
        }

        function CloseLookup() {
            clookupClass.ConfirmCurrentSelection();
            clookupClass.HideDropDown();
            clookupClass.Focus();
        }

        function _CloseLookup() {
            clookupBrand.ConfirmCurrentSelection();
            clookupBrand.HideDropDown();
            clookupBrand.Focus();
        }

        function popupHide(s, e) {

            cpopupApproval.Hide();
        }

        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
        }


    </script>
    <script type="text/javascript">

        function SaveActivity() {
            if (validations()) {
                var contactname = $('#hdnSelectedContact').val();
                
                // Rev Mantis Issue 22801 [ control timepicker1 added in PerformCallback() parameter]
                //cCallbackPanelActivity.PerformCallback('Save~' + contactname + '~' + $("#cmbActivity").val() + '~' + $("#cmbType").val() + '~' + $("#txtSubject").val() + '~' + $("#txtDetails").val() + '~' + $("#cmbSalesActivityAssignTo").val() + '~' + $("#cmbDuration").val() + '~' + $("#cmbPriority").val() + '~' + $('#cmbContactType').val() + '~' + $('#hdnActivityEmailid').val() + '~' + $('#hdnSMSid').val() );
                var duration_new = $('#timepicker1').val();
                var duration = 0;
                cCallbackPanelActivity.PerformCallback('Save~' + contactname + '~' + $("#cmbActivity").val() + '~' + $("#cmbType").val() + '~' + $("#txtSubject").val() + '~' + $("#txtDetails").val() + '~' + $("#cmbSalesActivityAssignTo").val() + '~' + duration + '~' + $("#cmbPriority").val() + '~' + $('#cmbContactType').val() + '~' + $('#hdnActivityEmailid').val() + '~' + $('#hdnSMSid').val() + '~' + duration_new);
            }
        }
        function validations() {
            var ismandatory = false;

            if (cdtActivityDate.date != null) {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Activity Date');
                return;
            }

            if ($('#cmbActivity').val() != "0") {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Activity');
                return;
            }

            if ($('#cmbType').val() != "0") {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Type');
                return;
            }

            if ($('#txtSubject').val() != "") {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Subject');
                return;
            }

            if ($('#txtDetails').val() != "") {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Details');
                return;
            }

            if ($('#cmbSalesActivityAssignTo').val() != "0") {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Assign To');
                return;
            }

            // Rev Mantis Issue 22801
            //if ($('#cmbDuration').val() != "0") {
            //    ismandatory = true;
            //}
            //else {
            //    ismandatory = false;
            //    jAlert('Select Duration');
            //    return;
            //}

            if ($('#timepicker1').val() != "00:00:00" && $('#timepicker1').val() != "" && $('#timepicker1').val() != "0" && $('#timepicker1').val() != "0:00:00") {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Duration');
                return;
            }
            // End of Rev Mantis Issue 22801

            if ($('#cmbPriority').val() != "0") {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Priority');
                return;
            }

            if (cDtxtDue.date != null) {
                ismandatory = true;
            }
            else {
                ismandatory = false;
                jAlert('Select Due Date');
                return;
            }

            return ismandatory;
        }
        function CancelActivity() {
            $("#cmbActivity").val(0);
            $("#txtSubject").text('');
            $("#cmbType").val(0);
            $("#txtDetails").text('');
            $("#cmbSalesActivityAssignTo").val(0);
            $("#cmbDuration").val(0);
            $("#cmbPriority").val(0);
            $("#cmbContactType").val(0);
            // Rev Mantis Issue 22801
            $("#timepicker1").val("00:00:00");
            // End of Rev Mantis Issue 22801
            $("#btnClear").removeClass('hide');
            $("#btnSave").addClass('hide');
            cActivityPopup.Hide();
        }
        var keyvalueforassign = '';
        function ClickOnEdit(keyValue) {
            console.log("hi")
            keyvalueforassign = keyValue;
            $("#hdnEntityID").val(keyValue);
            $("#btnSection").addClass('hide');
            $("#btnSave").addClass('hide');
            $("#btnClear").addClass('hide');
            cCallbackPanelActivity.PerformCallback('Edit~' + keyvalueforassign );
            cActivityPopup.Show();

            // Rev Mantis Issue 22801_Sushanta
            setTimeout(function () {
                
                $('#timepicker1').timepicker({
                    minuteStep: 1,
                    showSeconds: true,
                    showMeridian: false,
                    defaultTime: false,
                    explicitMode: true,
                    setTime: new Date()
                });
            }, 1500)
            // End of Rev Mantis Issue 22801_Sushanta
        }

        function ClickOnCancel(keyValue) {
            keyvalueforassign = keyValue;
            $("#hdnEntityID").val(keyValue);
            cCallbackPanelActivity.PerformCallback('Cancel~' + keyvalueforassign);
        }
       

        function OnDelete(keyValue) {
            keyvalueforassign = keyValue;
            $("#hdnEntityID").val(keyValue);
            cCallbackPanelActivity.PerformCallback('Delete~' + keyvalueforassign);
        }
    </script>
    <%-- For Single selection when click on ok button--%>
          <script type="text/javascript">

              function ValueSelected(e, indexName) {

                          if (e.code == "Enter" || e.code == "NumpadEnter") {
                              if (indexName == "ContactIndex") {
                                  var Id = e.target.parentElement.parentElement.cells[0].innerText;
                                  var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                                  if (Id) {
                                      SetContact(Id, name);
                                  }
                              }
                              else if (indexName == "ProductIndex") {
                                  var Id = e.target.parentElement.parentElement.cells[0].innerText;
                                  var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                                  if (Id) {
                                      SetProduct(Id, name);
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
                                  $('#txtContactSearch').focus();
                              }
                          }

              }

          </Script>
    <%-- For Single selection when click on ok button--%>
   <%--For Contact Single Selection--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ContactModel').on('shown.bs.modal', function () {
                $('#txtContactSearch').focus();
            })
        })
        function ContactButnClick(s, e) {
            clear_PreviousCustdata();
            $('#ContactModel').modal('show');
        }

        function Contact_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                clear_PreviousCustdata();
                $('#ContactModel').modal('show');
            }
        }
        function clear_PreviousCustdata() {
            ctxtContact.SetText("");
            var OtherDetails = {}
            OtherDetails.SearchKey = 'abc123zzxx';
            OtherDetails.ContactType = $('#cmbContactType').val();

            var HeaderCaption = [];
            HeaderCaption.push("Name");
            callonServer("CrmServices/CRMService.asmx/GetContact", OtherDetails, "ContactTable", HeaderCaption, "ContactIndex", "SetContact");
        }

        function Contactkeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtContactSearch").val()) == "" || $.trim($("#txtContactSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtContactSearch").val();
            OtherDetails.ContactType = $('#cmbContactType').val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Name");
                if ($("#txtContactSearch").val() != "") {
                    callonServer("CrmServices/CRMService.asmx/GetContact", OtherDetails, "ContactTable", HeaderCaption, "ContactIndex", "SetContact");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ContactIndex=0]"))
                    $("input[ContactIndex=0]").focus();
            }
        }

        function SetContact(Id, Name) {
            //debugger;
            var key = Id;
            if (key != null && key != '') {
                $('#ContactModel').modal('hide');
                ctxtContact.SetText(Name);
                GetObjectID('hdnSelectedContact').value = key;
                ctxtContact.Focus();
            }
            else {
                ctxtContact.SetText('');
                GetObjectID('hdnSelectedContact').value = '';
            }
        }

    </Script>
    <%--For Contact Single Selection--%>

    <%--For Product Single Selection--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ProductModel').on('shown.bs.modal', function () {
                $('#txtProductSearch').focus();
            })
        })
        function ProductButnClick(s, e) {
            $('#ProductModel').modal('show');
        }

        function Product_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#ProductModel').modal('show');
            }
        }

        function Productkeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtProductSearch").val()) == "" || $.trim($("#txtProductSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtProductSearch").val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Product Description");
                HeaderCaption.push("HSN/SAC");

                if ($("#txtProductSearch").val() != "") {
                    callonServer("CrmServices/CRMService.asmx/GetPosProduct", OtherDetails, "ProductTable", HeaderCaption, "ProductIndex", "SetProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProductIndex=0]"))
                    $("input[ProductIndex=0]").focus();
            }
        }

        function SetProduct(Id, Name) {
            //debugger;
            var key = Id;
            if (key != null && key != '') {
                $('#ProductModel').modal('hide');
                ctxtProduct.SetText(Name);
                GetObjectID('hdfProductID').value = key;
                ctxtProduct.Focus();
            }
            else {
                ctxtProduct.SetText('');
                GetObjectID('hdfProductID').value = '';
            }
        }

    </Script>
    <%--For Product Single Selection--%>




    <%--//For Add--%>
    <script type="text/javascript">
        $(document).ready(function () {
         
            $("#cmbPriority").change(function () {

                var value = $(this).val();
                if (value == '1') {
                    $("#cmbPriority").css({ 'background': '#66c19b', 'border-color': '#56a886', 'color': '#fff' }); // High
                } else if (value == '2') {
                    $("#cmbPriority").css({ 'background': '#35d667', 'border-color': '#2cc35b', 'color': '#555' });//Low
                }
                else if (value == '3') {
                    $("#cmbPriority").css({ 'background': '#f94747', 'border-color': '#f23c3c', 'color': '#fff' });  //Moderate
                }
                else if (value == '4') {
                    $("#cmbPriority").css({ 'background': '#f5dfc3', 'border-color': '#b8b8b8', 'color': '#555' });//Normal
                }

            });
        });

        function ddlPriorityChange() {
            var value = $("#cmbPriority").val();
            if (value == '1') {
                $("#cmbPriority").css({ 'background': '#66c19b', 'border-color': '#56a886', 'color': '#fff' }); // High
            } else if (value == '2') {
                $("#cmbPriority").css({ 'background': '#35d667', 'border-color': '#2cc35b', 'color': '#555' });//Low
            }
            else if (value == '3') {
                $("#cmbPriority").css({ 'background': '#f5dfc3', 'border-color': '#b8b8b8', 'color': '#555' });  //Moderate
            }
            else if (value == '4') {
                $("#cmbPriority").css({ 'background': '#f94747', 'border-color': '#f23c3c', 'color': '#fff' }); //Normal
            }
        }
        var olds = "";
        function ddlICRChange() {
            var s = $("#<%=cmbActivity.ClientID%> option:selected").val();
             //if (olds != s) {
                 cCallbackPanelActivity.PerformCallback('Activity_Type~' + s);
                 olds = s;
                
                // Rev Mantis Issue 22801_Sushanta
                setTimeout(function () {
                    // alert("hi")
                    $('#timepicker1').timepicker({
                        minuteStep: 1,
                        showSeconds: true,
                        showMeridian: false,
                        defaultTime: false,
                        explicitMode: true,
                        setTime: new Date()
                    });
                }, 1000)
                // End of Rev Mantis Issue 22801_Sushanta
             //}

        }
      
        function btn_Add(e) {
            OnAddActivitiesbuttonClick();
            $("#btnSection").removeClass('hide');
              cCallbackPanelActivity.PerformCallback('Add');
              cActivityPopup.Show();

            // Rev Mantis Issue 22801_Sushanta
            setTimeout(function () {

            $('#timepicker1').timepicker({
                minuteStep: 1,
                showSeconds: true,
                showMeridian: false,
                defaultTime: false,
                explicitMode: true,
                setTime: new Date()
            });
              }, 1500)
            // End of Rev Mantis Issue 22801_Sushanta

          }
          function CallbackPanelActivityEndCall(s, e) {
               //$('#LeadActivityModel').modal('show');
              if (cCallbackPanelActivity.cpStatusActivity == "Save") {
                  cActivityPopup.Hide();
                  //$("#hfActivityFilter").val("Y");
                  //cCallbackPanel.PerformCallback('Load~' + $("#hfActivityFilter").val());
                  //Grid.Refresh();
                  btn_TodayFollowUp();
                  CountDocument();
          }
              else if (cCallbackPanelActivity.cpStatusActivity == "Add") {
                  cdtActivityDate.SetValue(new Date());
                  $("#cmbContactType").val(cCallbackPanelActivity.cpActivityFor);
                  ctxtContact.SetText(cCallbackPanelActivity.cpContactName);
                  $("#hdnSelectedContact").val(cCallbackPanelActivity.cpContactID);
                  olds = "";
                 // $("#cmbActivity").val(cCallbackPanelActivity.cpActivity);
                 // $("#cmbType").val(cCallbackPanelActivity.cpType);
                  $("#txtSubject").val(cCallbackPanelActivity.cpSubject);
                  $("#txtDetails").val(cCallbackPanelActivity.cpDetails);
                  $("#cmbSalesActivityAssignTo").val(cCallbackPanelActivity.cpAssignto);
                  $("#cmbDuration").val(cCallbackPanelActivity.cpDuration);
                  $("#cmbPriority").val(cCallbackPanelActivity.cpPriority);
                  // Rev Mantis Issue 22801
                  $("#timepicker1").val(cCallbackPanelActivity.cpDuration_New);
                  // End of Rev Mantis Issue 22801
                  cDtxtDue.SetValue(new Date());
                  //if (cCallbackPanelActivity.cpProductDetails!="")
                  ActivityProduct = cCallbackPanelActivity.cpProductDetails;
                  //$("#hfActivityFilter").val("Y");
                  //cCallbackPanel.PerformCallback('Load~' + $("#hfActivityFilter").val());
                  //Grid.Refresh();
                  btn_TodayFollowUp();
                  CountDocument();
              }
              else if (cCallbackPanelActivity.cpStatusActivity == "Edit") {
                  cdtActivityDate.SetValue(new Date(cCallbackPanelActivity.cpActivityDate));
                  $("#cmbContactType").val(cCallbackPanelActivity.cpActivityFor);
                  ctxtContact.SetText(cCallbackPanelActivity.cpContactName);
                  $("#hdnSelectedContact").val(cCallbackPanelActivity.cpContactID);

                  $("#cmbActivity").val(cCallbackPanelActivity.cpActivity);
                  $("#cmbType").val(cCallbackPanelActivity.cpType);
                  $("#txtSubject").val(cCallbackPanelActivity.cpSubject);
                  $("#txtDetails").val(cCallbackPanelActivity.cpDetails);
                  $("#cmbSalesActivityAssignTo").val(cCallbackPanelActivity.cpAssignto);
                  $("#cmbDuration").val(cCallbackPanelActivity.cpDuration);
                  $("#cmbPriority").val(cCallbackPanelActivity.cpPriority);
                  // Rev Mantis Issue 22801
                  $("#timepicker1").val(cCallbackPanelActivity.cpDuration_New)
                  // End of Rev Mantis Issue 22801
                  cDtxtDue.SetValue(new Date(cCallbackPanelActivity.cpDueDate));
                  //cDtxtDue.SetText(cCallbackPanelActivity.cpDueDate);
                  //if (cCallbackPanelActivity.cpProductDetails!="")
                  ActivityProduct = cCallbackPanelActivity.cpProductDetails;
                  //$("#hfActivityFilter").val("Y");
                  //cCallbackPanel.PerformCallback('Load~' + $("#hfActivityFilter").val());
                  //Grid.Refresh();
                  btn_TodayFollowUp();
                  CountDocument();
              }
              else if (cCallbackPanelActivity.cpStatusActivity == "Delete") {
                  //Grid.Refresh();
                  jAlert("Successfully Deleted");
                  cCallbackPanelActivity.cpStatusActivity = null;
                  btn_TodayFollowUp();
                  CountDocument();
              }
              else if (cCallbackPanelActivity.cpStatusActivity == "Cancel") {
                  Grid.Refresh();
                  jAlert("Successfully Canceled");
                  cCallbackPanelActivity.cpStatusActivity = null;

              }

          }

   </script>

     <%--For Lead Activity Email--%>
         <script type="text/javascript">
             function ddlTypeChange() {
                 var typetext = $("#cmbType option:selected").text();
                 if (typetext == 'Email Sent') {
                     cCallbackPanelEmail.PerformCallback('Load~' + $('#hdnSelectedContact').val());
                 }
                 else if (typetext == 'SMS') {
                     cCallbackPanelSMS.PerformCallback('Load~' + $('#hdnSelectedContact').val());
                 }

             }
             function SentEmail() {
                 if ($("#txtEmailTo").val() != '')
                 {
                     SaveEmail();
                 }
                 else
                 {
                     jAlert("Please specify at least one recipient.");
                 }
                
             }
             function GetFileSize() {
                <%-- var maxFileSize = '<%=fileSize %>'; // 2MB

                    if ($("#FileUpload1")[0].files[0].size < maxFileSize) {
                    } else {
                        $("#FileUpload1").val('');
                        return false;
                    }


                    var files = $('#FileUpload1')[0].files;
                    var len = $('#FileUpload1').get(0).files.length;

                    for (var i = 0; i < len; i++) {

                        f = files[i];

                        var ext = f.name.split('.').pop().toLowerCase();
                        if ($.inArray(ext, ['exe']) == 1) {
                            return false;
                        }
                    }--%>
             }
             function SaveEmail() {
                 var cmb = CKEDITOR.instances['txtEmailBody'].getData();

                 //var fileData = new FormData();
                 //for (var j = 0; j < 10; j++) {
                 //    if ($("#FileUpload1").get(j) != null && $("#FileUpload1").get(j) != "") {
                 //        var fileUpload = $("#FileUpload1").get(j);
                 //        var files = fileUpload.files;
                 //        // Create FormData object
                 //        // Looping over all files and add it to FormData object
                 //        for (var i = 0; i < files.length; i++) {
                 //            fileData.append(files[i].name, files[i]);
                 //        }
                 //    }
                 //}
                 //// Adding one more key to FormData object
                 //fileData.append('ActionType', 'SAVE');
                 //fileData.append('Contactid', $('#hdnSelectedContact').val());
                 //fileData.append('ToEmail', $("#txtEmailTo").val());

                 //fileData.append('CCEmail', $("#txtEmailcc").val());
                 //fileData.append('BCCEmail', CKEDITOR.instances['bodyInput'].getData());
                 //fileData.append('module_name', $("#hdnModule_Name").val());
                 //fileData.append('module_id', $("#hdnModule_Id").val());

                 var url = 'Activities.aspx/SaveEmails';
                 $.ajax({
                     type: "POST",
                     url: url,
                     data: JSON.stringify({ 'ActionType': 'SAVE', 'Contactid': $('#hdnSelectedContact').val(), 'ToEmail': $("#txtEmailTo").val(), 'CCEmail': $("#txtEmailcc").val(), 'BCCEmail': $("#txtEmailbcc").val(), 'Subject': $("#txtEmailSubject").val(), 'EmailBody': cmb }),
                     async: false,
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (response) {
                         console.log(response);
                         jAlert("Email Sent Successfully");
                         $("#hdnActivityEmailid").val(response.d);
                         cEmailPopup.Hide();
                         $("#txtSubject").val($("#txtEmailSubject").val());
                         $("#txtDetails").val(cmb);
                         
                     },
                     error: function (response) {
                         console.log(response);
                     }
                 });
             }
             function CancelEmail() {
                 $("#txtEmailTo").val('');
                 $("#txtEmailcc").val('');
                 $("#txtEmailbcc").val('');
                 $("#txtEmailSubject").val('');
                 $("#txtEmailBody").val('');
                 $("#cmbContactType").val(0);
                 $("#drpTemplate").val(0);
                 cEmailPopup.Hide();
             }

             function EmailPopupEndCall(s, e) {
                 if (cCallbackPanelEmail.cpStatusActivity == "Load")
                 {
                    
                     $("#txtEmailTo").val(cCallbackPanelEmail.cpEmail);
                     cEmailPopup.Show();
                     var editor = CKEDITOR.instances['txtEmailBody'];
                     if (editor) { editor.destroy(true); }
                     CKEDITOR.replace('txtEmailBody');

                 }
                 
             }
         </script>
     <%--End of For Lead Activity Email--%>
      <%--For Activity SMS--%>
      <script type="text/javascript">
          function SMSPopupEndCall(s, e) {
             if (cCallbackPanelSMS.cpStatusActivity == "Load") {
                 $("#txtSMSMobileNo").val(cCallbackPanelSMS.cpSMS);
                 cSMSPopup.Show();
              
             }
         }
         function CancelSMS() {
             $("#txtSMSMobileNo").val('');
             $("#txtSMSContent").val('');
             cSMSPopup.Hide();
         }
         function SentSMS() {
             if ($("#txtSMSMobileNo").val() != '') {
                 SaveSMS();
             }
             else {
                 jAlert("Please specify at least one recipient.");
             }
         }
         function SaveSMS() {
             
             var url = 'Activities.aspx/SaveSMS';
             $.ajax({
                 type: "POST",
                 url: url,
                 data: JSON.stringify({ 'ActionType': 'SAVE', 'Contactid': $('#hdnSelectedContact').val(), 'MobileNo': $("#txtSMSMobileNo").val(), 'SmsContent': $("#txtSMSContent").val() }),
                 async: false,
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (response) {
                     console.log(response);
                     jAlert("SMS Sent Successfully");
                     $("#hdnSMSid").val(response.d);
                     cSMSPopup.Hide();
                 },
                 error: function (response) {
                     console.log(response);
                 }
             });
         }
         function gridRowclick(s, e) {
             $('#ShowGrid').find('tr').removeClass('rowActive');
             $('.floatedBtnArea').removeClass('insideGrid');
             //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
             $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
             $(s.GetRow(e.visibleIndex)).addClass('rowActive');
             setTimeout(function () {
                 //alert('delay');
                 var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                 //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                 //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                 //    setTimeout(function () {
                 //        $(this).fadeIn();
                 //    }, 100);
                 //});    
                 $.each(lists, function (index, value) {
                     //console.log(index);
                     //console.log(value);
                     setTimeout(function () {
                         $(value).css({ 'opacity': '1' });
                     }, 100);
                 });
             }, 200);
         }
      </script>
       
      <%--End of For Activity SMS--%>
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
        .fa-plus-circle {
            z-index:9
        }
        .plhead a.collapsed .fa-minus-circle{
            display:none;
        }
        .pdr20 {
            padding-right:14px;
        }
        .verticaltTBL>tbody>tr>td{
            padding-right:15px;
            padding-bottom:5px;
        }
    </style>

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px !important;
            /*-webkit-appearance: none;
            position: relative;
            z-index: 1;*/
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
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

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
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
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        select.btn
        {
            padding-right: 10px !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        /*.panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }*/

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
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #SalesDetailsGrid, #ShowGrid
        {
            max-width: 99% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
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
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
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
                margin-top: 7px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-27
        {
            margin-top: 27px !important;
        }

        .col-md-3 , .col-md-2
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        .btn.btn-xs
        {
                font-size: 14px !important;
        }

        .dxpc-content table
        {
             width: 100%;
        }

        input[type="text"], input[type="password"], textarea
        {
            margin-bottom: 0 !important;
        }
        #FromDate , #ToDate , #ASPxFromDate , #ASPxToDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FromDate_B-1 , #ToDate_B-1 , #ASPxFromDate_B-1 , #ASPxToDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FromDate_B-1 #FromDate_B-1Img , #ToDate_B-1 #ToDate_B-1Img , #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img
        {
            display: none;
        }

        #lblToDate
        {
            padding-left: 10px;
        }

        .dxtc-activeTab:after {
            content: '';
            width: 0;
            height: 0;
            border-left: 8px solid transparent;
            border-right: 8px solid transparent;
            border-top: 9px solid #3e5395;
            position: absolute;
            /* left: 50%; */
            z-index: 3;
            /* bottom: -15px; */
            margin-left: -9px;
        }
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div id="" class="panel-title">
            
            <h3>
                <span id="lblHeadTitle">Activities</span>
            </h3>
        </div>
        
    </div>
        <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        
        <div class="clearfix">
            <button id="btnShow" class="btn btn-success btn-radius" type="button" onclick="btn_Add(this);"><span class="btn-icon"><i class="fa fa-plus" ></i></span> Add</button>
                     <% if (rights.CanExport)
                           { %>

                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}" TabIndex="6">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                     <% } %>

            <button id="btnTodayFollowup" data-toggle="tooltip" title="Click to see the today's followup details" class="btn btn-badge-color2 btn-radius" type="button" onclick="btn_TodayFollowUp(this);">Activities Due today<span class="badge" id="spTodayFollowUp">0</span></button>
            <button id="btnYesterFollowup" data-toggle="tooltip" title="Click to see upto yesterday's followup details" class="btn btn-badge-color3 btn-radius" type="button" onclick="btn_YesterdayFollowUp(this);">Overdue Activities<span class="badge" id="spYesterFollowUp" >0</span></button>
            <button id="btnAllFollowup" data-toggle="tooltip" title="Click to see all followup details" class="btn btn-badge-color6 btn-radius" type="button" onclick="btn_AllFollowUp(this);">All<span class="badge" id="spAllFollowUp">0</span></button>

        </div>
        <table class="TableMain100">


            <tr>

                <td colspan="2">
                    <div class="relative">
                         <dxe:ASPxGridView ID="ShowGrid" runat="server" AutoGenerateColumns="False"
                            Width="100%" ClientInstanceName="Grid" KeyFieldName="RID" 
                            DataSourceID="EntityServerModeDataSource" OnHtmlRowPrepared="ShowGrid_HtmlRowPrepared"  
                             OnSummaryDisplayText="ShowGrid_SummaryDisplayText" TabIndex="7" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="220">
                            <Columns>
                              
                                  <dxe:GridViewDataTextColumn Caption="Name" FieldName="Name" width="15%"
                                    VisibleIndex="1" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn Caption="Activity Name" FieldName="ActivityName" width="10%"
                                    VisibleIndex="2" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                             
                              
                                <dxe:GridViewDataTextColumn Caption="Activity Type" FieldName="ActivityTypeName" width="10%"
                                    VisibleIndex="3" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="Phone Number" FieldName="phf_Alt_phoneNumber" width="18%"
                                    VisibleIndex="4" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Subject" FieldName="Leadsubject" width="15%"
                                    VisibleIndex="5" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn Caption="Details" FieldName="Leaddetails" width="20%"
                                    VisibleIndex="6" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Priority" FieldName="PriorityName" width="5%"
                                    VisibleIndex="7" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                               
                                <dxe:GridViewDataTextColumn Caption="Duration" FieldName="DurationName" width="8%"
                                    VisibleIndex="8" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                               
                                 <dxe:GridViewDataTextColumn Caption="Assign To" FieldName="AssignTo_Name" width="12%"
                                    VisibleIndex="9" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Due Date" Width="8%" FieldName="Duedate" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy"
                                    VisibleIndex="10" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                               <dxe:GridViewDataTextColumn Caption="Cancel?" Width="8%" FieldName="isCancel" 
                                    VisibleIndex="11" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                   <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="16" CellStyle-HorizontalAlign="Center" Width="100px">

                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AllowFilterBySearchPanel="True" />
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                      <% if (rights.CanView)
                                       {%>
                                            <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Eval("SalesActivityId") %>')" title="" class="" style="text-decoration: none;">
                                                <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>View</span>
                                            </a>
                                        <% }%>
                                         <a href="javascript:void(0);" onclick="ClickOnCancel('<%# Eval("SalesActivityId") %>')" title="" class="" style="text-decoration: none;">
                                                <span class='ico ColorFour'><i class="fa fa-ban" aria-hidden="true"></i></span><span class='hidden-xs'>Cancel</span>
                                            </a>

                                    <% if (rights.CanDelete)
                                        { %>
                                            <a href="javascript:void(0);" onclick="OnDelete('<%# Eval("SalesActivityId") %>')" title="" class="">
                                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                    <% }%>
                                    </div>
    

                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>

                            </Columns>

                             <ClientSideEvents RowClick="gridRowclick" />
                            <SettingsBehavior AllowFocusedRow="true" AllowGroup="true" />
                            <SettingsEditing Mode="Inline">
                            </SettingsEditing>
                            <SettingsSearchPanel Visible="True" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsLoadingPanel Text="Please Wait..." />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />






                             <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                <SettingsEditing Mode="EditForm" />
                <SettingsContextMenu Enabled="true" />
                <SettingsBehavior AutoExpandAllGroups="true" />
                <Settings   ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" /> 
                <SettingsPager PageSize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                </SettingsPager>



                            <TotalSummary>

                            </TotalSummary>

                        </dxe:ASPxGridView>
                          <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                            ContextTypeName="ERPDataClassesDataContext" TableName="v_ActivityList" />


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
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="BudgetAfterHide" />
    </dxe:ASPxPopupControl>
     
     
<asp:HiddenField ID="hdnUserId" runat="server" />

<dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfActivityFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
      <ClientSideEvents EndCallback="CallbackPanelEndCall" />
</dxe:ASPxCallbackPanel>


 <dxe:ASPxPopupControl ID="ActivityPopup" runat="server" ClientInstanceName="cActivityPopup"
            Width="650px" HeaderText="Activity" PopupHorizontalAlign="WindowCenter" Height="500px"
            PopupVerticalAlign="WindowCenter" CssClass="DevPopTypeNew" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentStyle VerticalAlign="Top"></ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelActivity" ClientInstanceName="cCallbackPanelActivity" OnCallback="CallbackPanelActivity_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                              
                              
                                <div class="clear"></div>
                                <div class="clearfix">


                                   <div class="col-md-4">
                                         <div class="visF">
                                            <div id="ltd_ActivityDate" class="labelt">
                                                <div class="visF">
                                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Activity Date">
                                                    </dxe:ASPxLabel>
                                                    <span style="color: red;">*</span>
                                                </div>
                                            </div>
                                            <div id="td_ActivityDate">
                                                <div class="visF">
                                                  <dxe:ASPxDateEdit ID="dtActivityDate" TabIndex="1" runat="server" EditFormatString="dd-MM-yyyy" Date="" Width="100%"  ClientInstanceName="cdtActivityDate">
                                                    <TimeSectionProperties>
                                                        <TimeEditProperties EditFormatString="hh:mm tt" />
                                                    </TimeSectionProperties>
                                                  </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                         <div class="visF">
                                            <div id="td_Activity" class="labelt">
                                                <div class="visF">
                                                    <dxe:ASPxLabel ID="lblContactType" runat="server" Text="Activity For">
                                                    </dxe:ASPxLabel>
                                                    <span style="color: red;">*</span>
                                                </div>
                                            </div>
                                            <div id="td_Type">
                                                <div class="visF">
                                                    <asp:DropDownList ID="cmbContactType"  runat="server" TabIndex="2" Width="100%">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    
                                      <div class="col-md-4">
                                         <div class="visF">
                                            <div id="td_ConactName" class="labelt">
                                                <div class="visF">
                                                    <dxe:ASPxLabel ID="lblContactname" runat="server" Text="Contact Name">
                                                    </dxe:ASPxLabel>
                                                    <span style="color: red;">*</span>
                                                </div>
                                            </div>
                                            <div id="td_ContactName">
                                                <div class="visF">
                                                    <dxe:ASPxButtonEdit ID="txtContact" runat="server" ReadOnly="true" ClientInstanceName="ctxtContact" Width="100%" TabIndex="2">
                                                        <Buttons>
                                                            <dxe:EditButton>
                                                            </dxe:EditButton>
                                                        </Buttons>
                                                        <ClientSideEvents ButtonClick="function(s,e){ContactButnClick();}" KeyDown="function(s,e){Contact_KeyDown(s,e);}" />
                                                    </dxe:ASPxButtonEdit>
                                                    <asp:HiddenField ID="hdnSelectedContact" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-4">
                                         <div class="visF">
                                            <div id="td_Activity" class="labelt">
                                                <div class="visF">
                                                    <dxe:ASPxLabel ID="lblActivity" runat="server" Text="Activity">
                                                    </dxe:ASPxLabel>
                                                    <span style="color: red;">*</span>
                                                </div>
                                            </div>
                                            <div id="td_Type">
                                                <div class="visF">
                                                    <asp:DropDownList ID="cmbActivity"  runat="server" TabIndex="3" Width="100%" AutoPostBack="false">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="visF">
                                            <div class="labelt">
                                                <label>
                                                    <dxe:ASPxLabel ID="blnActivityType" runat="server" Text="Type" CssClass="pdl8"></dxe:ASPxLabel>
                                                    <span style="color: red;">*</span>

                                                </label>
                                                <div id="td_Type">
                                                    <div class="">
                                                        <asp:DropDownList ID="cmbType" runat="server" TabIndex="4" Width="100%">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                         <label>
                                          <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="" CssClass="pdl8"></dxe:ASPxLabel>
                                         </label>
                                        <button type="button" class="btn btn-product btn-block" onclick="Products('ACPRD')">Product</button>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-12">
                                        <div class="">
                                        <label>
                                            <dxe:ASPxLabel ID="lblSubject" runat="server" Text="Subject" CssClass="pdl8"></dxe:ASPxLabel>
                                            <span style="color: red;">*</span>

                                        </label>
                                        <div id="td_Type">
                                            <div class="">
                                                <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" TabIndex="5" MaxLength="500" Width="100%">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-12">
                                        <div class="">
                                        <label>
                                            <dxe:ASPxLabel ID="lblDetails" runat="server" Text="Details" CssClass="pdl8"></dxe:ASPxLabel>
                                            <span style="color: red;">*</span>

                                        </label>
                                        <div id="td_Details">
                                            <div class="">
                                                <asp:TextBox ID="txtDetails" runat="server" TextMode="MultiLine" Columns="20" Rows="6" CssClass="form-control" TabIndex="5" MaxLength="500" Width="100%">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-12">
                                        <div class="">
                                            <div id="td_lAssignto" class="labelt">
                                                <dxe:ASPxLabel ID="lblSalesActivityAssignTo" runat="server" Text="Assign To">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                            </div>
                                            <div id="td_dAssignto">
                                                <asp:DropDownList ID="cmbSalesActivityAssignTo" runat="server" TabIndex="7" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                    <div class="col-md-6">
                                        <div class="">
                                            <div id="td_lDuration" class="labelt">
                                                <dxe:ASPxLabel ID="lblDuration" runat="server" Text="Duration">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                            </div>
                                           <%--Rev Mantis Issue 22801_Sushanta [New Tinme control "timepicker1" control for Duration used  and the olkd class "td_dDuration" hidden--%> 
                                            <div>
                                                <div class="input-group bootstrap-timepicker timepicker">
                                                    <input id="timepicker1" type="text" class="form-control input-small"  />
                                                    <span class="input-group-addon" style="padding: 2px 5px;"><i class="fa fa-clock-o"></i></span>
                                                </div>
                                            </div>
                                             <div id="td_dDuration" class="hide">
                                                <%--<asp:DropDownList ID="cmbDuration" runat="server" TabIndex="8" Width="100%">
                                                </asp:DropDownList>--%>
                                                <input type="text" id="cmbDuration">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="">
                                            <div id="td_lPriority" class="labelt">
                                                <dxe:ASPxLabel ID="lblPriority" runat="server" Text="Priority">
                                                </dxe:ASPxLabel>
                                                <span style="color: red;">*</span>
                                            </div>
                                            <div id="td_dPriority">
                                                <asp:DropDownList ID="cmbPriority" runat="server" TabIndex="9" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="">
                                            <div id="td_lDue" class="labelt">
                                                <dxe:ASPxLabel ID="lblDue" runat="server" Text="Due" CssClass="pdl8"></dxe:ASPxLabel>
                                            </div>
                                            <div>
                                               <%-- <dxe:ASPxDateEdit ID="DtxtDue" runat="server" EditFormatString="dd-MM-yyyy hh:mm:ss"  ClientInstanceName="cDtxtDue" EditFormat="Custom" UseMaskBehavior="True" TabIndex="20" Width="100%">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <TimeSectionProperties>
                                                        <TimeEditProperties EditFormatString="hh:mm tt" />
                                                    </TimeSectionProperties>
                                                </dxe:ASPxDateEdit>--%>

                                                <dxe:ASPxDateEdit ID="DtxtDue" TabIndex="10" runat="server"  Date="" Width="100%"  ClientInstanceName="cDtxtDue">
                                                <TimeSectionProperties>
                                                    <TimeEditProperties EditFormatString="hh:mm tt" />
                                                </TimeSectionProperties>
                                                  <%-- <ClientSideEvents DateChanged="Enddate" />--%>
                                        </dxe:ASPxDateEdit>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                               
                                <div class="clearfix">
                                    <asp:HiddenField ID="hdnEntityID" runat="server" />
                                </div>


              
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="CallbackPanelActivityEndCall" />
                    </dxe:ASPxCallbackPanel>

                             <div class="modal-footer" id="btnSection">
                            <button class="btn btn-success btn-radius" id="btnClear" type="button" data-keyboard="true" onclick="OnAddActivitiesbuttonClick();"><span class="btn-icon"><i class="fa fa-plus"></i></span>Activity</button>
                            <button type="button" id= "btnSave" class="btnOkformultiselection btn btn-success hide" onclick="SaveActivity()">Save</button>
                            <button type="button" class="btnOkformultiselection btn btn-danger" onclick="CancelActivity()">Cancel</button>
                       </div>


                </dxe:PopupControlContentControl>
            </ContentCollection>

        </dxe:ASPxPopupControl>

<dxe:ASPxPopupControl ID="AspxDirectCustomerViewPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectCustomerViewPopup" Height="650px"
        Width="1020px" HeaderText="Customer View" Modal="true" AllowResize="false">
         
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
</dxe:ASPxPopupControl>


<dxe:ASPxPopupControl ID="AspxDirectProductViewPopup" runat="server"
    CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectProductViewPopup" Height="650px"
    Width="1020px" HeaderText="Product View" Modal="true" AllowResize="false">
         
    <ContentCollection>
        <dxe:PopupControlContentControl runat="server">
        </dxe:PopupControlContentControl>
    </ContentCollection>
</dxe:ASPxPopupControl>

   <!--Contact Modal -->
    <div class="modal fade pmsModal w40" id="ContactModel" role="dialog">
        <div class="modal-dialog">
            <!-- Contact content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Contact Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Contactkeydown(event)" id="txtContactSearch" autofocus width="100%" placeholder="Search By ContactName" />
                    <div id="ContactTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Name</th>
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
 <!--Contact Modal -->


 <!--Product Modal -->
    <div class="modal fade pmsModal w40" id="ProductModel" role="dialog">
        <div class="modal-dialog">
            <!-- Contact content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Productkeydown(event)" id="txtProductSearch" autofocus width="100%" placeholder="Search By Product" />
                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Name</th>
                                <th>Product Description</th>
                                <th>HSN/SAC</th>
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

      <dxe:ASPxPopupControl ID="ActivityProductpopup" runat="server" ClientInstanceName="cActivityProduct" ShowCloseButton="true"
        Width="1200px" HeaderText="Activity Products" PopupHorizontalAlign="WindowCenter" Height="200px"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                  <div class="clearfix boxStyle">
                    <div class="col-md-3">
                        <label>Product</label>
                      <%--  <dxe:ASPxTextBox runat="server" ID="txtProduct" ClientInstanceName="ctxtProduct">
                             <ClientSideEvents LostFocus="SizeLostFocus" />
                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                        </dxe:ASPxTextBox>--%>
                         <dxe:ASPxButtonEdit ID="txtProduct" runat="server" ReadOnly="true" ClientInstanceName="ctxtProduct" Width="100%" TabIndex="2">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="ProductButnClick" KeyDown="Product_KeyDown" />
                        </dxe:ASPxButtonEdit>
                        <asp:HiddenField ID="hdfProductID" runat="server" />
                    </div>
                    <div class="col-md-2 lblmTop8">
                        <label>Quantity</label>
                        <dxe:ASPxTextBox runat="server" ID="txtQuantity" ClientInstanceName="ctxtQuantity">
                            <%-- <ClientSideEvents LostFocus="SizeLostFocus" />--%>
                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />     
                        </dxe:ASPxTextBox>
                    </div>
                    <div class="col-md-2 lblmTop8">
                        <label>Rate</label>
                        <dxe:ASPxTextBox runat="server" ID="txtRate" ClientInstanceName="ctxtRate">
                             <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                        </dxe:ASPxTextBox>
                    </div>
                   <%-- <div class="col-md-3">
                        <label>Total</label>
                        <dxe:ASPxTextBox runat="server" ID="txtTotal" ClientEnabled="false" ClientInstanceName="ctxtTotal">
                            <MaskSettings Mask="&lt;0..99999999999&gt;.&lt;00..99&gt;" />
                        </dxe:ASPxTextBox>
                    </div>--%>
                     <div class="col-md-4 lblmTop8 ">
                        <label>Remarks</label>
                        <dxe:ASPxTextBox runat="server" ID="ASPxTextBox1" ClientInstanceName="ctxtRemarks" Width="100%" >
                        </dxe:ASPxTextBox>
                    </div>

                    <div class="col-md-1  pdLeft0">
                        <label>&nbsp;</label>
                        <div class="mtop5">
                        <button type="button" onclick="AddActivityProductDetails();" class="btn btn-primary">Add</button>
                        </div>
                    </div>
                </div>
                <div class="clearfix"></div>
                <div class="col-md-12">
                    <table id="dataTbl" class="display nowrap" style="width:100%" >
                        <thead>
                            <tr>
                            <th class="hide">GUID</th>
                            <th class="hide">ProductId</th>
                            <th>SL</th>
                            <th>Product</th>
                            <th>Quantity</th>
                            <th>Rate</th>
                            <th>Remarks</th>
                            <th>Action</th>
                            </tr>
                        </thead>
                        <tbody id="tbodyActivityProduct">

                        </tbody>
                    </table>
                </div>
                <div class="col-md-12 text-right pdTop15">
                    <button class="btn btn-success" type="button" onclick="SaveActivityProductDetails('Activity');">OK</button>
                    <button class="btn btn-danger hide" type="button" onclick="return cActivityProduct.Hide();">Cancel</button>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>

    </dxe:ASPxPopupControl>

    <%--Mail--%>
      <dxe:ASPxPopupControl ID="EmailPopup" runat="server" ClientInstanceName="cEmailPopup"
            Width="650px" HeaderText="Email" PopupHorizontalAlign="WindowCenter" Height="500px"
            PopupVerticalAlign="WindowCenter" CssClass="DevPopTypeNew" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentStyle VerticalAlign="Top"></ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelEmail" ClientInstanceName="cCallbackPanelEmail" OnCallback="CallbackPanelEmail_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                              
                              
                                <div class="clear"></div>
                                <div class="clearfix">
                                    <div class="col-md-12">
                                        <div class="">
                                        <label>
                                            <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="To" CssClass="pdl8"></dxe:ASPxLabel>
                                            <span style="color: red;">*</span>

                                        </label>
                                        <div id="td_To">
                                            <div class="">
                                                <asp:TextBox ID="txtEmailTo" runat="server" CssClass="form-control" TabIndex="1" MaxLength="500" Width="100%">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                 </div>

                                <div class="clear"></div>
                                    <div class="col-md-12">
                                        <div class="">
                                        <label>
                                            <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="cc" CssClass="pdl8"></dxe:ASPxLabel>
                                        </label>
                                        <div id="td_cc">
                                            <div class="">
                                                <asp:TextBox ID="txtEmailcc" runat="server" CssClass="form-control" TabIndex="2" MaxLength="500" Width="100%">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                      </div>
                                    </div>

                                    <div class="clear"></div>
                                        <div class="col-md-12">
                                            <div class="">
                                            <label>
                                                <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="bcc" CssClass="pdl8"></dxe:ASPxLabel>
                                            </label>
                                            <div id="td_Type">
                                                <div class="">
                                                    <asp:TextBox ID="txtEmailbcc" runat="server" CssClass="form-control" TabIndex="3" MaxLength="500" Width="100%">
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        </div>

                                    <div class="clear"></div>
                                    <div class="col-md-12">
                                        <div class="">
                                        <label>
                                            <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Subject" CssClass="pdl8"></dxe:ASPxLabel>
                                        </label>
                                        <div id="td_Type">
                                            <div class="">
                                                <asp:TextBox ID="txtEmailSubject" runat="server" CssClass="form-control" TabIndex="4" MaxLength="500" Width="100%">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    </div>

                                    <div class="clear"></div>
                                    <div class="col-md-12">
                                        <div class="">
                                        <div id="td_Details">
                                            <div class="">
                                                <asp:TextBox ID="txtEmailBody" runat="server" TextMode="MultiLine" Columns="20" Rows="5" CssClass="form-control" TabIndex="5" MaxLength="500" Width="100%">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    </div>

                                    <div class="clear"></div>
                                    <div class="col-md-12">
                                        <div class="">
                                            <div  class="labelt">
                                                <dxe:ASPxLabel ID="ASPxLabel11" runat="server" Text="Template">
                                                </dxe:ASPxLabel>
                                            </div>
                                            <div >
                                                <asp:DropDownList ID="drpTemplate" runat="server" TabIndex="6" Width="100%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                 <%--   <div class="clear"></div>
                                    <div class="col-md-12">
                                        <div class="">
                                            <div  class="labelt">
                                                <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="Attachment">
                                                </dxe:ASPxLabel>
                                            </div>
                                            <div >
                                                <asp:FileUpload ID="FileUpload1" runat="server" Width="250px" TabIndex="7" onchange="GetFileSize()" />
                                            </div>
                                        </div>
                                    </div>--%>

                                    
                                </div>
                               
                                <div class="clearfix">
                                    <asp:HiddenField ID="hdnemailuniqueid" runat="server" />
                                </div>

                        <div class="modal-footer">
                            <button type="button" class="btnOkformultiselection btn btn-success" onclick="SentEmail()">Sent Email</button>
                            <button type="button" class="btnOkformultiselection btn btn-danger" onclick="CancelEmail()">Cancel</button>
                       </div>
              
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="EmailPopupEndCall" />
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>

        </dxe:ASPxPopupControl>
     <asp:HiddenField ID="hdnActivityEmailid" runat="server" />
    <%--Mail--%>
    <%--SMS--%>
        <dxe:ASPxPopupControl ID="SMSPopup" runat="server" ClientInstanceName="cSMSPopup"
            Width="650px" HeaderText="SMS" PopupHorizontalAlign="WindowCenter" Height="200px"
            PopupVerticalAlign="WindowCenter" CssClass="DevPopTypeNew" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentStyle VerticalAlign="Top"></ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelSMS" ClientInstanceName="cCallbackPanelSMS" OnCallback="CallbackPanelSMS_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                              
                              
                                <div class="clear"></div>
                                <div class="clearfix">
                                    <div class="col-md-12">
                                        <div class="">
                                        <label>
                                            <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="Mobile No" CssClass="pdl8"></dxe:ASPxLabel>
                                            <span style="color: red;">*</span>

                                        </label>
                                        <div id="td_To">
                                            <div class="">
                                                <asp:TextBox ID="txtSMSMobileNo" runat="server" CssClass="form-control" TabIndex="1"  Width="100%">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                 </div>

                                <div class="clear"></div>
                                      <div class="col-md-12">
                                        <div class="">
                                        <div id="td_Type">
                                            <div class="">
                                               <asp:TextBox ID="txtSMSContent" runat="server" CssClass="form-control" TabIndex="2" Width="100%">
                                                </asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    </div>
                                </div>
                               
                        <div class="modal-footer">
                            <button type="button" class="btnOkformultiselection btn btn-success" onclick="SentSMS()">Sent SMS</button>
                            <button type="button" class="btnOkformultiselection btn btn-danger" onclick="CancelSMS()">Cancel</button>
                       </div>
              
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="SMSPopupEndCall" />
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>

        </dxe:ASPxPopupControl>
     <asp:HiddenField ID="hdnSMSid" runat="server" />
     <%--SMS--%>
    <asp:HiddenField ID="hdnGridRefresh" runat="server" />
</asp:Content>