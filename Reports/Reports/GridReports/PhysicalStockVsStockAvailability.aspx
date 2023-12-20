<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                13-03-2023        2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PhysicalStockVsStockAvailability.aspx.cs" Inherits="Reports.Reports.GridReports.PhysicalStockVsStockAvailability" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        .colDisable {
        cursor:default !important;
        }

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
    </style>

    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'ClassSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#ClassModel').modal('hide');
                    ctxtClass.SetText(Name);
                    GetObjectID('hdnClassId').value = key;


                    ctxtProdName.SetText('');
                    $('#txtProdSearch').val('')

                    var OtherDetailsProd = {}
                    OtherDetailsProd.SearchKey = 'undefined text';
                    OtherDetailsProd.ClassID = '';
                    var HeaderCaption = [];
                    HeaderCaption.push("Code");
                    HeaderCaption.push("Name");
                    HeaderCaption.push("Hsn");

                    callonServerM("Services/Master.asmx/GetClassWiseProduct", OtherDetailsProd, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");

                }
                else {
                    ctxtClass.SetText('');
                    GetObjectID('hdnClassId').value = '';
                }
            }
            else if (ArrName == 'ProductSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#ProdModel').modal('hide');
                    ctxtProdName.SetText(Name);
                    GetObjectID('hdncWiseProductId').value = key;
                }
                else {
                    ctxtProdName.SetText('');
                    GetObjectID('hdncWiseProductId').value = '';
                }
            }
            else if (ArrName == 'BrandSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#BrandModel').modal('hide');
                    ctxtBrandName.SetText(Name);
                    GetObjectID('hdnBranndId').value = key;
                }
                else {
                    ctxtBrandName.SetText('');
                    GetObjectID('hdnBranndId').value = '';
                }
            }

        }

    </script>
   <%-- For multiselection when click on ok button--%>

   <%-- For Class multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#ClassModel').on('shown.bs.modal', function () {
                 $('#txtClassSearch').focus();
             })

         })

         var ClassArr = new Array();
         $(document).ready(function () {
             var ClassObj = new Object();
             ClassObj.Name = "ClassSource";
             ClassObj.ArraySource = ClassArr;
             arrMultiPopup.push(ClassObj);
         })
         function ClassButnClick(s, e) {
             $('#ClassModel').modal('show');
         }

         function Class_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#ClassModel').modal('show');
             }
         }

         function Classkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtClassSearch").val()) == "" || $.trim($("#txtClassSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtClassSearch").val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Class Name");


                 if ($("#txtClassSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetClass", OtherDetails, "ClassTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ClassSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtClassSearch').focus();
             else
                 $('#txtClassSearch').focus();
         }
   </script>
 <%-- For Class multiselection--%>

    <%-- For Product multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#ProdModel').on('shown.bs.modal', function () {
                 $('#txtProdSearch').focus();
             })

         })
         var ProdArr = new Array();
         $(document).ready(function () {
             var ProdObj = new Object();
             ProdObj.Name = "ProductSource";
             ProdObj.ArraySource = ProdArr;
             arrMultiPopup.push(ProdObj);
         })
         function ProductButnClick(s, e) {
             $('#ProdModel').modal('show');
         }

         function Product_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#ProdModel').modal('show');
             }
         }

         function Productkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtProdSearch").val();
             OtherDetails.ClassID = hdnClassId.value;

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Code");
                 HeaderCaption.push("Name");
                 HeaderCaption.push("Hsn");


                 if ($("#txtProdSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetClassWiseProduct", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtProdSearch').focus();
             else
                 $('#txtProdSearch').focus();
         }
   </script>
      <%-- For Product multiselection--%>

   <%-- For Brand multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#BrandModel').on('shown.bs.modal', function () {
                 $('#txtBrandSearch').focus();
             })

         })
         var BrandArr = new Array();
         $(document).ready(function () {
             var BrandObj = new Object();
             BrandObj.Name = "BrandSource";
             BrandObj.ArraySource = BrandArr;
             arrMultiPopup.push(BrandObj);
         })
         function BrandButnClick(s, e) {
             $('#BrandModel').modal('show');
         }

         function Brand_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#BrandModel').modal('show');
             }
         }

         function Brandkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtBrandSearch").val()) == "" || $.trim($("#txtBrandSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtBrandSearch").val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Name");

                 if ($("#txtBrandSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetBrand", OtherDetails, "BrandTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "BrandSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtBrandSearch').focus();
             else
                 $('#txtBrandSearch').focus();
         }
   </script>
      <%-- For Brand multiselection--%>

    <script type="text/javascript">

        //$(function () {
        //    $("#ddlbranchHO").change(function () {
        //        var Ids = $(this).val();
        //        cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
        //    })
        //});

        $(function () {
            cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());

        });

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })

            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                $('#MandatoryActivityType').attr('style', 'display:none');
                $("#hdnSelectedBranches").val('');
                cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })

        })

        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }

        function CallbackPanelEndCall(s, e) {
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            Grid.Refresh();
        }
    </script>


    <script type="text/javascript">
        //for Esc
        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeToDate.GetDate();
        }

        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsPhyStockVsStockAvailSummaryFilter").val("Y");
            data += '~' + cxdeToDate.GetDate();

            if (gridbranchLookup.GetValue() == null && CchkIsConsolidated.GetChecked() == false) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(data);
            }
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "As On Date: " + ToDate;
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

        function CheckIsConsolidated(s, e) {
            if (s.GetCheckState() == 'Checked') {
                $(<%=ddlbranchHO.ClientID%>).prop("disabled", true)
                $("#ddlbranchHO").val("All");
                cBranchComponentPanel.SetEnabled(false);
                gridbranchLookup.Clear();
            }
            else {
                $(<%=ddlbranchHO.ClientID%>).prop("disabled", false)
                cBranchComponentPanel.SetEnabled(true);
                cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            }
        }

        function OpenSODetails(prodid,branchid)
        {
            $("#hfIsPhyStockVsStockAvailSOFilter").val("Y");
            cCallbackPanelSODetail.PerformCallback('BndSOPopupgrid~' + prodid + '~'+branchid);
            cpopupSODetails.Show();
            return true;
        }

        //function SOpopupHide(s, e) {
        //    cpopupSODetails.Hide();
        //}

        function CallbackPanelSODetEndCall(s, e) {
            cGridSODetails.Refresh();
        }

        //for Esc
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && cpopupSODetails.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                SOpopupHide();
            }
            else if (event.keyCode == 27 && cpopupPODetails.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                POpopupHide();
            }
        }
        function SOpopupHide(s, e) {
            cpopupSODetails.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }

        function OpenPODetails(prodid, branchid) {
            $("#hfIsPhyStockVsStockAvailPOFilter").val("Y");
            cCallbackPanelPODetail.PerformCallback('BndPOPopupgrid~' + prodid + '~' + branchid);
            cpopupPODetails.Show();
            return true;
        }

        //function POpopupHide(s, e) {
        //    cpopupPODetails.Hide();
        //}

        function CallbackPanelPODetEndCall(s, e) {
            cGridPODetails.Refresh();
        }

        //for Esc
        //document.onkeyup = function (e) {
        //    if (event.keyCode == 27 && cpopupPODetails.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
        //        POpopupHide();
        //    }
        //}
        function POpopupHide(s, e) {
            cpopupPODetails.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }

        function OpenDocumentsDetails(orderid, type) {
            if (type == 'SO') {
                url = '/OMS/Management/Activities/SalesOrderAdd.aspx?key=' + orderid + '&IsTagged=1&req=V&type=SO';
            }
            else if (type == 'PO') {
                url = '/OMS/Management/Activities/PurchaseOrder.aspx?key=' + orderid + '&IsTagged=1&req=V&type=PO';
            }
            popupdocument.SetContentUrl(url);
            popupdocument.Show();
        }

        function DocumentAfterHide(s, e) {
            popupdocument.Hide();
        }

        function selectAllBranch() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAllBranch() {
            gridbranchLookup.gridView.UnselectRows();
        }
        function CloseLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
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
        .padTopbutton {
                padding-top: 15px;
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

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
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

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
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
            top: 6px;
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
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridLocationwiseStockStatus 
        /*#B2B , 
        #grid_B2BUR , 
        #grid_IMPS , 
        #grid_IMPG ,
        #grid_CDNR ,
        #grid_CDNUR ,
        #grid_EXEMP ,
        #grid_ITCR ,
        #grid_HSNSUM*/
        {
            max-width: 98% !important;
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
                margin-top: 3px;
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

        .for-cust-icon {
            position: relative;
            z-index: 1;
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

        select.btn
        {
           position: relative;
           z-index: 0;
        }

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
        /*.padTopbutton {
    padding-top: 27px;
}*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/

        #popupSODetails_PW-1, #popupPODetails_PW-1
        {
            position: fixed !important;
            left: 30px !important;
            top: 12px !important;
        }
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
                    <div>
                        <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
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
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="clearfix row">
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <%--Rev 1.0: "simple-select" class add --%>
                <div class="simple-select">
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                 <div style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>
                    <asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                    <dxe:ASPxCallbackPanel runat="server" ID="gdcallBranch" ClientInstanceName="cBranchComponentPanel" OnCallback="Componentbranch_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                    OnDataBinding="lookup_branch_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
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
                                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllBranch"  UseSubmitBehavior="False" />
                                                            <%--</div>--%>
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllBranch"  UseSubmitBehavior="False" />
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLookupbranch" UseSubmitBehavior="False" />
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
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="Product : " CssClass="mylabel1"
                        Width="110px"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){Product_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsFrom">
                    <asp:Label ID="Label5" runat="Server" Text="Class : " CssClass="mylabel1"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxButtonEdit ID="txtClass" runat="server" ReadOnly="true" ClientInstanceName="ctxtClass" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){ClassButnClick();}" KeyDown="function(s,e){Class_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsFrom">
                    <asp:Label ID="Label6" runat="Server" Text="Brand : " CssClass="mylabel1"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxButtonEdit ID="txtBrandName" runat="server" ReadOnly="true" ClientInstanceName="ctxtBrandName" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){BrandButnClick();}" KeyDown="function(s,e){Brand_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </div>
            </div>
            <div class="clear"></div>
            <div class="col-md-2 mtc-10">
                <div style="color: #b5285f;" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="As On Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>
                    <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>

                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </div>
            </div>
            <div class="col-md-2" style="margin-top:30px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkIsConsolidated" Checked="false" Text="Consolidated" ClientInstanceName="CchkIsConsolidated">
                    <ClientSideEvents CheckedChanged="CheckIsConsolidated" />
                </dxe:ASPxCheckBox>
            </div>
            <%--<div class="clear"></div>--%>

            <div class="col-md-2 mt-30 pb-5">
                <%--<label style="margin-bottom: 0">&nbsp</label>--%>
                    <div class="">
                        <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                        <% if (rights.CanExport)
                            { %> 
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">XLSX</asp:ListItem>
                            <asp:ListItem Value="2">PDF</asp:ListItem>
                            <asp:ListItem Value="3">CSV</asp:ListItem>
                            <asp:ListItem Value="4">RTF</asp:ListItem>
                        </asp:DropDownList>
                         <% } %>
                    </div>
            </div>
        </div>
        
        <div class="pull-right">
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback" DataSourceID="GenerateEntityServerModeDataSource" 
                            Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight ="400">
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="PSBRANCHDESC" Width="200px" Caption="Unit" VisibleIndex="1" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn FieldName="PSPRODCODE" Width="220px" Caption="Item Code" VisibleIndex="2" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn FieldName="PSPRODNAME" Width="200px" Caption="Item Details" VisibleIndex="3" HeaderStyle-CssClass="colDisable"/>
                                                                 
                                <dxe:GridViewDataTextColumn FieldName="PSPRODCLASS" Width="150px" Caption="Class" VisibleIndex="4" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn FieldName="PSBRAND" Width="150px" Caption="Make" VisibleIndex="5" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewBandColumn Caption="Physical Stock" VisibleIndex="6" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="PCSMULTQTY" Caption="Pcs" Width="100px" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="METERMULTQTY" Caption="Meters" Width="100px" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="PSSTOCKUOMQTY" Caption="Wt" Width="100px" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewBandColumn Caption="Sales Order" VisibleIndex="10" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="SOPCSMULTQTY" Caption="Pcs" Width="100px" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="SOMETERMULTQTY" Caption="Meters" Width="100px" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="SOSTOCKQTY" Caption="Wt" Width="100px" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewDataTextColumn FieldName="SODETAILS" Width="150px" Caption="So Details" VisibleIndex="14" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenSODetails('<%#Eval("PSPRODID") %>','<%#Eval("PSBRANCH_ID") %>')" class="pad">
                                            <%#Eval("SODETAILS")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewBandColumn Caption="Sale Qty Available" VisibleIndex="15" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="SQAPCSSTOCKQTY" Caption="Pcs" Width="100px" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="SQAMETERSTOCKQTY" Caption="Meters" Width="100px" VisibleIndex="17" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="SQASTOCKQTY" Caption="Wt" Width="100px" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewBandColumn Caption="PO Placed" VisibleIndex="19" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="POPCSMULTQTY" Caption="Pcs" Width="100px" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="POMETERMULTQTY" Caption="Meters" Width="100px" VisibleIndex="21" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="POSTOCKQTY" Caption="Wt" Width="100px" VisibleIndex="22" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewDataTextColumn FieldName="PODETAILS" Width="150px" Caption="Po Details" VisibleIndex="23" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenPODetails('<%#Eval("PSPRODID") %>','<%#Eval("PSBRANCH_ID") %>')" class="pad">
                                            <%#Eval("PODETAILS")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="EDD" Width="100px" Caption="EDD" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" VisibleIndex="24" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" AllowSort="False" />
                            <Settings ShowFooter="true" ShowGroupPanel="false" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="false" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="50">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="PSPRODNAME" SummaryType="Custom" Tag="Item_Name"/>
                                <dxe:ASPxSummaryItem FieldName="PCSMULTQTY" SummaryType="Custom" Tag="PS_Pcs"/>
                                <dxe:ASPxSummaryItem FieldName="METERMULTQTY" SummaryType="Custom" Tag="PS_Meter"/>
                                <dxe:ASPxSummaryItem FieldName="PSSTOCKUOMQTY" SummaryType="Custom" Tag="PS_Wt"/>
                                <dxe:ASPxSummaryItem FieldName="SOPCSMULTQTY" SummaryType="Custom" Tag="SO_Pcs"/>
                                <dxe:ASPxSummaryItem FieldName="SOMETERMULTQTY" SummaryType="Custom" Tag="SO_Meter"/>
                                <dxe:ASPxSummaryItem FieldName="SOSTOCKQTY" SummaryType="Custom" Tag="SO_Wt"/>
                                <dxe:ASPxSummaryItem FieldName="SQAPCSSTOCKQTY" SummaryType="Custom" Tag="SQA_Pcs"/>
                                <dxe:ASPxSummaryItem FieldName="SQAMETERSTOCKQTY" SummaryType="Custom" Tag="SQA_Meter"/>
                                <dxe:ASPxSummaryItem FieldName="SQASTOCKQTY" SummaryType="Custom" Tag="SQA_Wt"/>
                                <dxe:ASPxSummaryItem FieldName="POPCSMULTQTY" SummaryType="Custom" Tag="PO_Pcs"/>
                                <dxe:ASPxSummaryItem FieldName="POMETERMULTQTY" SummaryType="Custom" Tag="PO_Meter"/>
                                <dxe:ASPxSummaryItem FieldName="POSTOCKQTY" SummaryType="Custom" Tag="PO_Wt"/>
                            </TotalSummary>
                        </dxe:ASPxGridView>
                            <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORT" ></dx:LinqServerModeDataSource>

                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxPopupControl ID="popupSODetails" runat="server" ClientInstanceName="cpopupSODetails"
        Width="1500px" Height="600px" ScrollBars="Vertical" HeaderText="Sales Order - Detail" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="TopSides" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <% if (rights.CanExport)
                    { %> 
                        <asp:DropDownList ID="ddlSOdetails" runat="server" CssClass="btn btn-sm btn-primary"
                            AutoPostBack="true" OnSelectedIndexChanged="cmbExportSODet_SelectedIndexChanged">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">XLSX</asp:ListItem>
                            <asp:ListItem Value="2">PDF</asp:ListItem>
                            <asp:ListItem Value="3">CSV</asp:ListItem>
                            <asp:ListItem Value="4">RTF</asp:ListItem>
                        </asp:DropDownList>
                <% } %>

                <div class="row">
                    <div>
                    </div>

                    <div class="col-md-12">
                        <dxe:ASPxGridView ID="GridSODetails" runat="server" AutoGenerateColumns="False" Width="100%" ClientInstanceName="cGridSODetails" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerSODetailsModeDataSource" OnSummaryDisplayText="GridSODetails_SummaryDisplayText" 
                            Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight ="300">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="PSBRANCHDESC" Width="150px" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Doc. Number" FieldName="ORDER_NUMBER" Width="130px" VisibleIndex="2" HeaderStyle-CssClass="colDisable">
                                   <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenDocumentsDetails('<%#Eval("ORDER_ID") %>','<%#Eval("TRANTYPE") %>')" class="pad">
                                            <%#Eval("ORDER_NUMBER")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Doc. Date" FieldName="ORDER_DATE" Width="100px" VisibleIndex="3" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft"></CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Customer Name" FieldName="CUSTVENDNAME" Width="220px" VisibleIndex="4" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"> </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="PROJ_NAME" Width="200px" Caption="Project Name" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Product Code" FieldName="PSPRODCODE" Width="220px" VisibleIndex="6" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Description" FieldName="PSPRODNAME" Width="220px" VisibleIndex="7" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewBandColumn Caption="UOM" VisibleIndex="8" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="PCSMULTIUOM" Caption="Pcs" Width="100px" VisibleIndex="9" HeaderStyle-CssClass="colDisable">                                        
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="METERMULTIUOM" Caption="Meter" Width="100px" VisibleIndex="10" HeaderStyle-CssClass="colDisable">                                        
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="STOCKUOM" Caption="Wt" Width="100px" VisibleIndex="11" HeaderStyle-CssClass="colDisable">                                        
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewDataTextColumn FieldName="RATE" Caption="Rate" Width="100px" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewBandColumn Caption="Actual Quantity" VisibleIndex="13" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="ACTUALPCSMULTQTY" Caption="Pcs" Width="100px" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="ACTUALMETERMULTQTY" Caption="Meter" Width="100px" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="ACTUALSTOCKQTY" Caption="Wt" Width="100px" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewBandColumn Caption="Mature Quantity" VisibleIndex="17" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="MATUREPCSQTY" Caption="Pcs" Width="100px" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="MATUREMETERQTY" Caption="Meter" Width="100px" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="MATURESTOCKQTY" Caption="Wt" Width="100px" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewBandColumn Caption="Balance Quantity" VisibleIndex="21" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="BALPCSQTY" Caption="Pcs" Width="100px" VisibleIndex="22" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="BALMETERQTY" Caption="Meter" Width="100px" VisibleIndex="23" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="BALSTOCKQTY" Caption="Wt" Width="100px" VisibleIndex="24" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>
                                 
                                <dxe:GridViewDataTextColumn FieldName="ACTUAL_VALUES" Caption="Actual Values" Width="100px" VisibleIndex="25" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="MATURE_VALUES" Caption="Mature Values" Width="100px" VisibleIndex="26" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BALANCE_VALUES" Caption="Balance Values" Width="100px" VisibleIndex="27" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>

                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" AllowSort="False" />
                            <Settings ShowFooter="true" ShowGroupPanel="false" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="false" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="50">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="PSPRODNAME" SummaryType="Custom" Tag="SOItem_Name"/>
                                <dxe:ASPxSummaryItem FieldName="ACTUALPCSMULTQTY" SummaryType="Custom" Tag="ASO_Pcs"/>
                                <dxe:ASPxSummaryItem FieldName="ACTUALMETERMULTQTY" SummaryType="Custom" Tag="ASO_Meter"/>
                                <dxe:ASPxSummaryItem FieldName="ACTUALSTOCKQTY" SummaryType="Custom" Tag="ASO_Wt"/>
                                <dxe:ASPxSummaryItem FieldName="MATUREPCSQTY" SummaryType="Custom" Tag="MSO_Pcs"/>
                                <dxe:ASPxSummaryItem FieldName="MATUREMETERQTY" SummaryType="Custom" Tag="MSO_Meter"/>
                                <dxe:ASPxSummaryItem FieldName="MATURESTOCKQTY" SummaryType="Custom" Tag="MSO_Wt"/>
                                <dxe:ASPxSummaryItem FieldName="BALPCSQTY" SummaryType="Custom" Tag="BSO_Pcs"/>
                                <dxe:ASPxSummaryItem FieldName="BALMETERQTY" SummaryType="Custom" Tag="BSO_Meter"/>
                                <dxe:ASPxSummaryItem FieldName="BALSTOCKQTY" SummaryType="Custom" Tag="BSO_Wt"/>
                                <dxe:ASPxSummaryItem FieldName="ACTUAL_VALUES" SummaryType="Custom" Tag="ASO_Value"/>
                                <dxe:ASPxSummaryItem FieldName="MATURE_VALUES" SummaryType="Custom" Tag="MSO_Value"/>
                                <dxe:ASPxSummaryItem FieldName="BALANCE_VALUES" SummaryType="Custom" Tag="BSO_Value"/>
                            </TotalSummary>

                        </dxe:ASPxGridView>
                          <dx:LinqServerModeDataSource ID="GenerateEntityServerSODetailsModeDataSource" runat="server" OnSelecting="GenerateEntityServerSODetailsModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                    <div class="clear"></div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="SOpopupHide" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="popupPODetails" runat="server" ClientInstanceName="cpopupPODetails"
        Width="1500px" Height="600px" ScrollBars="Vertical" HeaderText="Purchase Order - Detail" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="TopSides" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <% if (rights.CanExport)
                    { %> 
                        <asp:DropDownList ID="ddlPOdetails" runat="server" CssClass="btn btn-sm btn-primary"
                            AutoPostBack="true" OnSelectedIndexChanged="cmbExportPODet_SelectedIndexChanged">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">XLSX</asp:ListItem>
                            <asp:ListItem Value="2">PDF</asp:ListItem>
                            <asp:ListItem Value="3">CSV</asp:ListItem>
                            <asp:ListItem Value="4">RTF</asp:ListItem>
                        </asp:DropDownList>
                <% } %>

                <div class="row">
                    <div>
                    </div>

                    <div class="col-md-12">
                        <dxe:ASPxGridView ID="GridPODetails" runat="server" AutoGenerateColumns="False" Width="100%" ClientInstanceName="cGridPODetails" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerPODetailsModeDataSource" OnSummaryDisplayText="GridPODetails_SummaryDisplayText" 
                            Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight ="300">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="PSBRANCHDESC" Width="150px" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Doc. Number" FieldName="ORDER_NUMBER" Width="130px" VisibleIndex="2" HeaderStyle-CssClass="colDisable">
                                   <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenDocumentsDetails('<%#Eval("ORDER_ID") %>','<%#Eval("TRANTYPE") %>')" class="pad">
                                            <%#Eval("ORDER_NUMBER")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Doc. Date" FieldName="ORDER_DATE" Width="100px" VisibleIndex="3" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft"></CellStyle>
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Customer Name" FieldName="CUSTVENDNAME" Width="220px" VisibleIndex="4" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"> </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="PROJ_NAME" Width="200px" Caption="Project Name" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Product Code" FieldName="PSPRODCODE" Width="220px" VisibleIndex="6" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Description" FieldName="PSPRODNAME" Width="220px" VisibleIndex="7" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewBandColumn Caption="UOM" VisibleIndex="8" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="PCSMULTIUOM" Caption="Pcs" Width="100px" VisibleIndex="9" HeaderStyle-CssClass="colDisable">                                        
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="METERMULTIUOM" Caption="Meter" Width="100px" VisibleIndex="10" HeaderStyle-CssClass="colDisable">                                        
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="STOCKUOM" Caption="Wt" Width="100px" VisibleIndex="11" HeaderStyle-CssClass="colDisable">                                        
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewDataTextColumn FieldName="RATE" Caption="Rate" Width="100px" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewBandColumn Caption="Actual Quantity" VisibleIndex="13" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="ACTUALPCSMULTQTY" Caption="Pcs" Width="100px" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="ACTUALMETERMULTQTY" Caption="Meter" Width="100px" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="ACTUALSTOCKQTY" Caption="Wt" Width="100px" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewBandColumn Caption="Mature Quantity" VisibleIndex="17" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="MATUREPCSQTY" Caption="Pcs" Width="100px" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="MATUREMETERQTY" Caption="Meter" Width="100px" VisibleIndex="19" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="MATURESTOCKQTY" Caption="Wt" Width="100px" VisibleIndex="20" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>

                                <dxe:GridViewBandColumn Caption="Balance Quantity" VisibleIndex="21" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <dxe:GridViewDataTextColumn FieldName="BALPCSQTY" Caption="Pcs" Width="100px" VisibleIndex="22" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="BALMETERQTY" Caption="Meter" Width="100px" VisibleIndex="23" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn FieldName="BALSTOCKQTY" Caption="Wt" Width="100px" VisibleIndex="24" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">                                        
                                            <CellStyle HorizontalAlign="Right"></CellStyle>
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                </dxe:GridViewBandColumn>
                                 
                                <dxe:GridViewDataTextColumn FieldName="ACTUAL_VALUES" Caption="Actual Values" Width="100px" VisibleIndex="25" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="MATURE_VALUES" Caption="Mature Values" Width="100px" VisibleIndex="26" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BALANCE_VALUES" Caption="Balance Values" Width="100px" VisibleIndex="27" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                            </Columns>

                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" AllowSort="False" />
                            <Settings ShowFooter="true" ShowGroupPanel="false" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="false" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="50">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="PSPRODNAME" SummaryType="Custom" Tag="POItem_Name"/>
                                <dxe:ASPxSummaryItem FieldName="ACTUALPCSMULTQTY" SummaryType="Custom" Tag="APO_Pcs"/>
                                <dxe:ASPxSummaryItem FieldName="ACTUALMETERMULTQTY" SummaryType="Custom" Tag="APO_Meter"/>
                                <dxe:ASPxSummaryItem FieldName="ACTUALSTOCKQTY" SummaryType="Custom" Tag="APO_Wt"/>
                                <dxe:ASPxSummaryItem FieldName="MATUREPCSQTY" SummaryType="Custom" Tag="MPO_Pcs"/>
                                <dxe:ASPxSummaryItem FieldName="MATUREMETERQTY" SummaryType="Custom" Tag="MPO_Meter"/>
                                <dxe:ASPxSummaryItem FieldName="MATURESTOCKQTY" SummaryType="Custom" Tag="MPO_Wt"/>
                                <dxe:ASPxSummaryItem FieldName="BALPCSQTY" SummaryType="Custom" Tag="BPO_Pcs"/>
                                <dxe:ASPxSummaryItem FieldName="BALMETERQTY" SummaryType="Custom" Tag="BPO_Meter"/>
                                <dxe:ASPxSummaryItem FieldName="BALSTOCKQTY" SummaryType="Custom" Tag="BPO_Wt"/>
                                <dxe:ASPxSummaryItem FieldName="ACTUAL_VALUES" SummaryType="Custom" Tag="APO_Value"/>
                                <dxe:ASPxSummaryItem FieldName="MATURE_VALUES" SummaryType="Custom" Tag="MPO_Value"/>
                                <dxe:ASPxSummaryItem FieldName="BALANCE_VALUES" SummaryType="Custom" Tag="BPO_Value"/>
                            </TotalSummary>

                        </dxe:ASPxGridView>
                          <dx:LinqServerModeDataSource ID="GenerateEntityServerPODetailsModeDataSource" runat="server" OnSelecting="GenerateEntityServerPODetailsModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PHYSICALSTOCKVSSTOCKAVAILABILITY_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                    <div class="clear"></div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="POpopupHide" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdocument" Height="500px"
        Width="1200px" HeaderText="Document Detail" Modal="true" AllowResize="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="DocumentAfterHide" />
    </dxe:ASPxPopupControl>
       <!--Class Modal -->
    <div class="modal fade" id="ClassModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Class Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Classkeydown(event)" id="txtClassSearch" width="100%" placeholder="Search By Class Name" />
                    <div id="ClassTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Class Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('ClassSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ClassSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
      <asp:HiddenField ID="hdnClassId" runat="server" />
    <!--Class Modal -->

     <!--Product Modal -->
    <div class="modal fade" id="ProdModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Productkeydown(event)" id="txtProdSearch" width="100%" placeholder="Search By Product Name" />
                    <div id="ProductTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Code</th>
                                 <th>Name</th>
                                <th>Hsn</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('ProductSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ProductSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
         <asp:HiddenField ID="hdncWiseProductId" runat="server" />
    
    <!--Product Modal -->
  
     <!--Brand Modal -->
    <div class="modal fade" id="BrandModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Brand Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Brandkeydown(event)" id="txtBrandSearch" width="100%" placeholder="Search By Brand Name" />
                    <div id="BrandTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('BrandSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('BrandSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
      <asp:HiddenField ID="hdnBranndId" runat="server" />
    <!--Brand Modal -->

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsPhyStockVsStockAvailSummaryFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
          <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelSODetail" ClientInstanceName="cCallbackPanelSODetail" OnCallback="CallbackPanelSODetail_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsPhyStockVsStockAvailSOFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
      <ClientSideEvents EndCallback="CallbackPanelSODetEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelPODetail" ClientInstanceName="cCallbackPanelPODetail" OnCallback="CallbackPanelPODetail_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsPhyStockVsStockAvailPOFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
      <ClientSideEvents EndCallback="CallbackPanelPODetEndCall" />
    </dxe:ASPxCallbackPanel>
</asp:Content>
