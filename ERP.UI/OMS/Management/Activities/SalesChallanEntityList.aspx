<%--==========================================================Revision History ============================================================================================   
   1.0   Priti    V2.0.36     16-02-2023     Afer Listing view upgradation delete data show in list issue solved. 
   2.0	 PRITI    V2.0.38	  03/04/2023	    0025289: Views to be converted to Procedures in the Listing Page of Transaction / Sales / Challan
   3.0	 Pallab   V2.0.38	  06/04/2023	    0025855: Sales Challan module design modification
   4.0   Sanchita V2.0.39     25-07-2023     Attachment icon will be shown against the document number if there is any attachment - Sales Challan
                                             Mantis : 26609   
========================================== End Revision History =======================================================================================================--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesChallanEntityList.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.SalesChallanEntityList" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--Code Added By Sandip For Approval Detail Section Start--%>
    <script>

        function OnEWayBillClick(id, EWayBillNumber, EWayBillDate, EWayBillValue) {

            if (EWayBillNumber.trim() != "") {
                ctxtEWayBillNumber.SetText(EWayBillNumber);
            }
            else {
                ctxtEWayBillNumber.SetText("");
            }

            if (EWayBillDate.trim() != "" && EWayBillDate.trim() != "01-01-1970" && EWayBillDate.trim() != "01-01-1900") {
                var d = new Date(EWayBillDate.split('-')[2].trim(), EWayBillDate.split('-')[1].trim() - 1, EWayBillDate.split('-')[0].trim(), 0, 0, 0, 0);
                cdt_EWayBill.SetDate(d);
            }
            else {
                cdt_EWayBill.SetText("");
            }
            if (EWayBillValue.trim() != "0.00" && EWayBillValue.trim() != "") {
                ctxtEWayBillValue.SetText(EWayBillValue);
            }
            else {
                ctxtEWayBillValue.SetText("0.0");
            }

            ctxtTransporterGSTIN.SetText('');
            ctxtTransporterName.SetText('');
            $("#ddlTransportationMode").val('0');
            ctxtTransportationDistance.SetText('0');
            ctxtTransporterDocNo.SetText('');
            ctxtVehicleNo.SetText('');
            $("#ddlVehicleType").val('0');

            $.ajax({
                type: "POST",
                url: "SalesChallanEntityList.aspx/EditEWayBill",
                data: JSON.stringify({
                    DocID: id
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d[0];
                    console.log(msg);
                    if (status != "") {
                        console.log(status);
                        ctxtTransporterGSTIN.SetText(status.TransporterGSTIN);
                        ctxtTransporterName.SetText(status.TransporterName);
                        if (status.Transporter_Mode != '') {
                            $("#ddlTransportationMode").val(status.Transporter_Mode);
                        }
                        if (status.Transporter_Distance != '') {
                            ctxtTransportationDistance.SetText(status.Transporter_Distance);
                        }
                        if (status.Transporter_DocNo != '') {
                            ctxtTransporterDocNo.SetText(status.Transporter_DocNo);
                        }
                        if (status.Vehicle_No != '') {
                            ctxtVehicleNo.SetText(status.Vehicle_No);
                        }
                        if (status.Vehicle_type != '') {
                            $("#ddlVehicleType").val(status.Vehicle_type);
                        }
                    }
                }
            });

            $('#hddnChallanID').val(id);
            cPopup_EWayBill.Show();
            ctxtEWayBillNumber.Focus();
        }

        function GetEWayBillDateFormat(today) {
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
                today = yyyy + '-' + mm + '-' + dd;
            }

            return today;
        }
        function CallEWayBill_save() {

            var ChallanID = $("#<%=hddnChallanID.ClientID%>").val();
            //if (ctxtEWayBillNumber.GetValue() == null) {                
            //    jAlert("Please enter E-Way Bill Number.");
            //        return false;               
            //}
            //else
            //{
            var UpdateEWayBill = ctxtEWayBillNumber.GetValue();
            if (UpdateEWayBill == "0") {
                UpdateEWayBill = "";
            }
            if (cdt_EWayBill.GetValue() == "" && cdt_EWayBill.GetValue() == null) {
                var EWayBillDate = "1990-01-01";
            }
            else {
                var EWayBillDate = GetEWayBillDateFormat(new Date(cdt_EWayBill.GetValue()));
            }

            var EWayBillValue = ctxtEWayBillValue.GetValue();
            //Rev Extra column Tanmoy
            var TransporterGSTIN = ctxtTransporterGSTIN.GetText();
            var TransporterName = ctxtTransporterName.GetText();
            var TransportationMode = $("#ddlTransportationMode").val();
            var TransportationDistance = ctxtTransportationDistance.GetText();
            var TransporterDocNo = ctxtTransporterDocNo.GetText();
            var TransporterDocDate = null;
            if (cdt_TransporterDocDate.GetValue() == "" && cdt_TransporterDocDate.GetValue() == null) {
                var TransporterDocDate = "1990-01-01";
            }
            else {
                if (cdt_TransporterDocDate.GetValue() == null) {
                    var TransporterDocDate = null;
                }
                else {
                    var TransporterDocDate = GetEWayBillDateFormat(new Date(cdt_TransporterDocDate.GetValue()));
                }
            }
            var VehicleNo = ctxtVehicleNo.GetText();
            var VehicleType = $("#ddlVehicleType").val();
            //End of Rev

            $.ajax({
                type: "POST",
                url: "SalesChallanEntityList.aspx/UpdateEWayBillForChallan",
                data: JSON.stringify({
                    ChallanID: ChallanID, UpdateEWayBill: UpdateEWayBill, EWayBillDate: EWayBillDate, EWayBillValue: EWayBillValue, TransporterGSTIN: TransporterGSTIN
                    , TransporterName: TransporterName, TransportationMode: TransportationMode, TransportationDistance: TransportationDistance, TransporterDocNo: TransporterDocNo
                    , TransporterDocDate: TransporterDocDate, VehicleNo: VehicleNo, VehicleType: VehicleType
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d;
                    if (status == "1") {
                        jAlert("Saved successfully.");
                        cPopup_EWayBill.Hide();
                        cGrdOrder.Refresh();

                        ctxtTransporterGSTIN.SetText('');
                        ctxtTransporterName.SetText('');
                        $("#ddlTransportationMode").val('0');
                        ctxtTransportationDistance.SetText('0');
                        ctxtTransporterDocNo.SetText('');
                        ctxtVehicleNo.SetText('');
                        $("#ddlVehicleType").val('0');
                    }
                    else if (status == "-10") {
                        jAlert("Data not saved.");
                        cPopup_EWayBill.Hide();
                    }
                }
            });
            //}           
        }
        function CancelEWayBill_save() {
            cPopup_EWayBill.Hide();
        }



        //This function is called to show the Status of All Sales Order Created By Login User Start
        function OpenPopUPUserWiseQuotaion() {
            cgridUserWiseQuotation.PerformCallback();
            cPopupUserWiseQuotation.Show();
        }
        // function above  End

        //This function is called to show all Pending Approval of Sales Order whose Userid has been set LevelWise using Approval Configuration Module 
        function OpenPopUPApprovalStatus() {
            cgridPendingApproval.PerformCallback();
            cpopupApproval.Show();
        }
        // function above  End


        // Status 2 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
        function GetApprovedQuoteId(s, e, itemIndex) {
            var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);
        }
        function OnGetApprovedRowValues(obj) {
            uri = "SalesChallanAdd.aspx?key=" + obj + "&status=2" + '&type=SC';
            popup.SetContentUrl(uri);
            popup.Show();
        }
        // function above  End For Approved

        // Status 3 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
        function GetRejectedQuoteId(s, e, itemIndex) {
            debugger;
            cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

        }
        function OnGetRejectedRowValues(obj) {
            uri = "SalesChallanAdd.aspx?key=" + obj + "&status=3" + '&type=SC';
            popup.SetContentUrl(uri);
            popup.Show();
        }
        // function above  End For Rejected

        // To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 
        function OnApprovalEndCall(s, e) {
            $.ajax({
                type: "POST",
                url: "SalesChallan.aspx/GetPendingCase",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    $('#<%= lblWaiting.ClientID %>').text(data.d);
                }
            });
            }
            // function above  End 

    </script>



    <%-- Code Added By Sandip For Approval Detail Section End--%>

    <script>
        var SChallan_Id = 0;
        function onPrintJv(id) {
            debugger;
            SChallan_Id = id;
            cSelectPanel.cpSuccess = "";
            cDocumentsPopup.Show();
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }

        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }
        //Filteration
        var isFirstTime = true;
        function AllControlInitilize() {
            debugger;
            if (isFirstTime) {

                if (localStorage.getItem('FromDateSalesChallan')) {
                    var fromdatearray = localStorage.getItem('FromDateSalesChallan').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('ToDateSalesChallan')) {
                    var todatearray = localStorage.getItem('ToDateSalesChallan').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('SalesChallanBranch')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('SalesChallanBranch'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('SalesChallanBranch'));
                    }

                }
                //updateGridByDate();

                isFirstTime = false;
            }
        }

        //Function for Date wise filteration
        function updateGridByDate() {
           // debugger;
            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else if (ccmbBranchfilter.GetValue() == null) {
                jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
            }
            else {
                localStorage.setItem("FromDateSalesChallan", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDateSalesChallan", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("SalesChallanBranch", ccmbBranchfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                //REV 2.0
                //cGrdOrder.Refresh();
                $("#hFilterType").val("All");
                cCallbackPanel.PerformCallback("");
                 //REV 2.0 END
            }
        }
        //REV 2.0
        function CallbackPanelEndCall(s, e) {
            cGrdOrder.Refresh();
        }
         //REV 2.0 END
        function cSelectPanelEndCall(s, e) {
            if (cSelectPanel.cpSuccess != "") {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'SChallan';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + SChallan_Id, '_blank')
            }
            //cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == "") {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }
        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;


            if (event.keyCode == 65 && isCtrl == true) { //run code for alt+a -- ie, Add
                StopDefaultAction(e);
                OnAddButtonClick();
            }

        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function OnClickDelete(keyValue) {
            debugger;
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cGrdOrder.PerformCallback('Delete~' + keyValue);
                }
            });
        }

        function OnclickViewAttachment(obj) {
            //var URL = '/OMS/Management/Activities/SalesOrder_Document.aspx?idbldng=' + obj + '&type=SalesOrder';
            var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=SalesChallan';
            window.location.href = URL;
        }
        function OnClickStatus(keyValue) {
            GetObjectID('hiddenedit').value = keyValue;
            cGrdOrder.PerformCallback('Edit~' + keyValue);
        }
        function grid_EndCallBack() {
           // debugger;
            if (cGrdOrder.cpEdit != null) {
                GetObjectID('hiddenedit').value = cGrdOrder.cpEdit.split('~')[0];
                cProforma.SetText(cGrdOrder.cpEdit.split('~')[1]);
                cCustomer.SetText(cGrdOrder.cpEdit.split('~')[4]);
                var pro_status = cGrdOrder.cpEdit.split('~')[2];
                if (pro_status != null) {
                    var radio = $("[id*=rbl_OrderStatus] label:contains('" + pro_status + "')").closest("td").find("input");
                    radio.attr("checked", "checked");
                    //return false;
                    //$('#rbl_QuoteStatus[type=radio][value=' + pro_status + ']').prop('checked', true); 
                    cOrderRemarks.SetText(cGrdOrder.cpEdit.split('~')[3]);
                    cOrderStatus.Show();
                }
            }
            if (cGrdOrder.cpDelete != null) {
                jAlert(cGrdOrder.cpDelete);
               cGrdOrder.Refresh();
                /* Rev 1.0*/                
                //updateGridByDate();
                /* Rev 1.0 End*/
                cGrdOrder.cpDelete = null;
            }
        }
        function SavePrpformaStatus() {
            if (document.getElementById('hiddenedit').value == '') {
                cGrdOrder.PerformCallback('save~');
            }
            else {
                cGrdOrder.PerformCallback('update~' + GetObjectID('hiddenedit').value);
            }

        }

        function OnMoreInfoClick(keyValue) {
            debugger;
            var ActiveUser = '<%=Session["userid"]%>'

            if (ActiveUser != null) {
                //$.ajax({
                //    type: "POST",
                //    url: "SalesChallan.aspx/GetEditablePermission",
                //    data: "{'ActiveUser':'" + ActiveUser + "'}",
                //    contentType: "application/json; charset=utf-8",
                //    dataType: "json",
                //    async: false,//Added By:Subhabrata
                //    success: function (msg) {
                //        debugger;
                //        var status = msg.d;
                //        $.ajax({
                //            type: "POST",
                //            url: "SalesChallan.aspx/GetChallanIdIsExistInSalesInvoice",
                //            data: "{'keyValue':'" + keyValue + "'}",
                //            contentType: "application/json; charset=utf-8",
                //            dataType: "json",
                //            async: false,//Added By:Subhabrata
                //            success: function (msg) {
                //                debugger;
                //                var status = msg.d;
                //                if (status == "1") {
                //                    jConfirm('Tagged in Sale Invoice. Cannot Moodfy.?', 'Confirmation Dialog', function (r) {
                //                        if (r == true) {
                //                            var url = 'SalesChallanAdd.aspx?key=' + keyValue + '&type=SC';
                //                            window.location.href = url;
                //                        }
                //                    });
                //                }

                //                return false;
                //            }
                //        });
                //        var url = 'SalesChallanAdd.aspx?key=' + keyValue + '&Permission=' + status + '&type=SC';
                //        window.location.href = url;
                //    }
                //});

                var url = 'SalesChallanAdd.aspx?key=' + keyValue + '&Permission=' + status + '&type=SC';
                window.location.href = url;
            }
        }

        ////##### coded by Samrat Roy - 04/05/2017  
        ////Add an another param to define request type 
        function OnViewClick(keyValue) {
            var url = 'SalesChallanAdd.aspx?key=' + keyValue + '&req=V' + '&type=SC';
            window.location.href = url;
        }

        function OnAddButtonClick() {
            var url = 'SalesChallanAdd.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        //});
        function gridRowclick(s, e) {
            //alert('hi');
            $('#gridcrmCampaign').find('tr').removeClass('rowActive');
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
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>
    <style>
        .padTab > tbody > tr > td {
            padding-right: 15px;
            vertical-align: middle;
        }

            .padTab > tbody > tr > td > label {
                margin-bottom: 0 !important;
            }

            .padTab > tbody > tr > td > .btn {
                margin-top: 0 !important;
            }

        .dxeErrorFrameWithoutError_PlasticBlue .dxeControlsCell_PlasticBlue, .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue {
            padding: 0px !important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdOrder.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdOrder.SetWidth(cntWidth);
                }
            });
        });
    </script>

    <style>
        /*Rev 3.0*/

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

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_PlQuoteExpiry
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_PlQuoteExpiry_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_PlQuoteExpiry_B-1 #dt_PlQuoteExpiry_B-1Img
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
            top: 34px;
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
            line-height: 18px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
                z-index: 0;
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

        /*.col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }*/

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
        }

        select
        {
            margin-bottom: 0;
        }*/

        .form-control
        {
            background-color: transparent;
        }

        /*#massrecdt
        {
            width: 100%;
        }

        .col-sm-3 , .col-md-3{
            margin-bottom: 10px;
        }*/

        .crossBtn
        {
            top: 25px;
                right: 25px;
        }

        input[type="text"], input[type="password"], textarea
        {
                margin-bottom: 0;
        }

        .typeNotification span
        {
             color: #ffffff !important;
        }

        .makeFullscreen >table
        {
            z-index: 0;
        }
        .makeFullscreen .makeFullscreen-icon.half
        {
                z-index: 0;
        }

        #CallbackPanel_LPV
        {
            top: 420px !important;
        }

        #lblWaiting
        {
            background: #f8666f;
            padding: 1px 5px;
            border-radius: 2px;
            margin-left: 4px;
        }

        /*Rev end 3.0*/
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxPopupControl ID="Popup_OrderStatus" runat="server" ClientInstanceName="cOrderStatus"
        Width="500px" HeaderText="Approvers Configuration" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-12">
                            <table width="100%">
                                <tr>
                                    <td style="padding-right: 20px">
                                        <label style="margin-bottom: 5px">Sales Challan</label>
                                    </td>
                                    <td>
                                        <%--<dxe:ASPxTextBox ID="txt_Proforma" MaxLength="80" ClientInstanceName="cProforma" TabIndex="1" 
                                                runat="server" Width="100%"> 
                                            </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxLabel ID="lbl_Proforma" runat="server" ClientInstanceName="cProforma" Text="ASPxLabel"></dxe:ASPxLabel>
                                    </td>
                                    <td style="padding-right: 20px; padding-left: 8px">
                                        <label style="margin-bottom: 5px">Customer</label>
                                    </td>
                                    <td>
                                        <%-- <dxe:ASPxTextBox ID="txt_Customer" ClientInstanceName="cCustomer"  runat="server" MaxLength="100" TabIndex="2"
                                            Width="100%"> 
                                        </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxLabel ID="lbl_Customer" runat="server" ClientInstanceName="cCustomer" Text="ASPxLabel"></dxe:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-md-6">
                        </div>
                        <div class="col-md-6">
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="col-md-12">
                    <table>
                        <tr>
                            <td style="width: 70px; padding: 13px 0;">Status </td>
                            <td>
                                <asp:RadioButtonList ID="rbl_OrderStatus" runat="server" Width="250px" CssClass="mTop5" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Pending" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Accepted" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="3"></asp:ListItem>
                                </asp:RadioButtonList></td>
                        </tr>
                    </table>





                </div>
                <div class="clear"></div>
                <div class="col-md-12">

                    <div class="" style="margin-bottom: 5px;">
                        Reason 
                    </div>

                    <div>
                        <dxe:ASPxMemo ID="txt_OrderRemarks" runat="server" ClientInstanceName="cOrderRemarks" Height="71px" Width="100%"></dxe:ASPxMemo>
                    </div>
                </div>

                <div class="col-md-12" style="padding-top: 10px;">
                    <dxe:ASPxButton ID="btn_PrpformaStatus" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                        AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                        <ClientSideEvents Click="function (s, e) {SavePrpformaStatus();}" />
                    </dxe:ASPxButton>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <%--Rev 2.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Sales Challan</h3>
        </div>

        <table class="padTab pull-right" style="margin-top: 7px;">
            <tr>
                <td>
                    <label>From </label>
                </td>
                <%--Rev 3.0: "for-cust-icon" class add --%>
                <td style="width: 150px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 3.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 3.0--%>
                </td>
                <td>
                    <label>To</label>
                </td>
                <%--Rev 3.0: "for-cust-icon" class add --%>
                <td style="width: 150px" class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 3.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 3.0--%>
                </td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                </td>

            </tr>

        </table>
    </div>
        <div class="form_main">
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus"></i></span><span>New</span> </a>
            <% } %>

            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>
            <%--Sandip Section for Approval Section in Design Start --%>

            <span id="spanStatus" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-info btn-radius">
                    <span>My Sales Challan Status</span>
                    <%--<asp:Label ID="Label1" runat="server" Text=""></asp:Label>--%>                   
                </a>
            </span>
            <span id="divPendingWaiting" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPApprovalStatus()" class="btn btn-warning btn-radius">
                    <span>Approval Waiting</span>

                    <asp:Label ID="lblWaiting" runat="server" Text=""></asp:Label>
                </a>
                <i class="fa fa-reply blink" style="font-size: 20px; margin-right: 10px;" aria-hidden="true"></i>

            </span>

            <%--Sandip Section for Approval Section in Design End --%>
        </div>
    </div>
        <div class="GridViewArea relative">
        <%--rev pallab added selection settings--%>
        <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="SlNo" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
            DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"
            Width="100%" ClientInstanceName="cGrdOrder" OnCustomCallback="GrdOrder_CustomCallback" OnSummaryDisplayText="ShowGrid_SummaryDisplayText" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" Settings-HorizontalScrollBarMode="Visible">
            <SettingsSearchPanel Visible="true" Delay="5000" />
            <Columns>
                <%--Rev 4.0--%>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="0" Width="40px" FixedStyle="Left">
                    <DataItemTemplate>
                        <img src="../../../assests/images/attachment.png" style='<%#Eval("IsAttachmentDoc")%>' />
                        
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>

                    <HeaderTemplate><span></span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False"/>
                </dxe:GridViewDataTextColumn>
                <%--End of Rev 4.0--%>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Challan_No"
                    VisibleIndex="0" Settings-ShowFilterRowMenu="True" Width="170px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="false" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Challan_Date"
                    VisibleIndex="1" Width="130px" Settings-ShowFilterRowMenu="True" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Invoice No." FieldName="SIINVOICENO"
                    VisibleIndex="2" Settings-ShowFilterRowMenu="True" Width="170px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="false" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Invoice Date" FieldName="SIDATE"
                    VisibleIndex="3" Width="130px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Branch"
                    VisibleIndex="4" Width="120" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                    VisibleIndex="5" Width="150px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount in Local Curr." FieldName="Amount"
                    VisibleIndex="6" Width="130px">
                    <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                    <PropertiesTextEdit>
                        <MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />
                    </PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="E-Way Bill No" FieldName="EwayBillNo" VisibleIndex="7" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                    VisibleIndex="14" Width="150">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="LastModifiedOn"
                    VisibleIndex="15" Width="333" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="UpdatedBy"
                    VisibleIndex="16" Width="150">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <%--  Rev Sayantani--%>
                <%-- <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status"
                    VisibleIndex="11"  Visible="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>--%>
                <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name" Width="150px" VisibleIndex="12" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="true" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status"
                    VisibleIndex="13" Visible="false" ShowInCustomizationForm="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Transporter Name(Delivery)" FieldName="TransPorterDeliveryName" Width="150px" VisibleIndex="8" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="true" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Transporter Name(Invoice)" FieldName="InvTransPorterName" Width="150px" VisibleIndex="9" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="true" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="CN/Bilty/LR No" FieldName="CN_Bilty_LR_No" Width="150px" VisibleIndex="10" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="true" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="LR Date" FieldName="LRdate"
                    VisibleIndex="11" Width="130px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <%--End of Rev Sayantani--%>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="16" Width="0">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Challan_Id") %>')" class="" title="">
                                <span class='ico ColorSeven'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                            <% } %>
                            <% if (rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("Challan_Id") %>')" class="" title="">

                                <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>  <% } %>
                            <% if (rights.CanDelete)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("Challan_Id") %>')" class="" title="">
                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                            <% } %>
                            <a href="javascript:void(0);" onclick="OnClickCopy('<%#Eval("Challan_Id") %>')" class="" title=" " style="display: none">
                                <span class='ico ColorThree'><i class='fa fa-copy'></i></span><span class='hidden-xs'>Copy</span></a>
                            <a href="javascript:void(0);" onclick="OnClickStatus('<%#Eval("Challan_Id") %>')" class="" title="" style="display: none">
                                <span class='ico ColorFour'><i class='fa fa-check'></i></span><span class='hidden-xs'>Status</span></a>
                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%#Eval("Challan_Id") %>')" class="" title="">
                                <span class='ico ColorFive'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>Add/View Attachment</span>
                            </a>
                            <% } %>
                            <a href="javascript:void(0);" onclick="OnEWayBillClick('<%#Eval("Challan_Id") %>','<%#Eval("EwayBillNo") %>','<%#Eval("EWayBillDate") %>','<%#Eval("EWayBillValue") %>')" class="" title="">
                                <span class='ico ColorThree'><i class='fa fa-file-text'></i></span><span class='hidden-xs'>Update E-Way Bill</span>
                                <% if (rights.CanPrint)
                                   { %>
                                <a href="javascript:void(0);" onclick="onPrintJv('<%#Eval("Challan_Id") %>')" class="" title="">
                                    <span class='ico ColorSix'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                                </a><%} %>
                        </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span></span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

            </Columns>
            <%-- --Rev Sayantani--%>
            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
            <%-- <SettingsCookies Enabled="true" StorePaging="true" StoreColumnsVisiblePosition="false" />--%>
            <SettingsCookies Enabled="true" StorePaging="true" Version="2.16" />
            <%-- -- End of Rev Sayantani --%>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
            <%--  <settingspager numericbuttoncount="20" pagesize="10" showseparators="True">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </settingspager>--%>
            <SettingsPager PageSize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
            <%--<settingssearchpanel visible="True" />--%>
            <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="false" />
            <SettingsLoadingPanel Text="Please Wait..." />
            <TotalSummary>
                <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
            </TotalSummary>
        </dxe:ASPxGridView>
        <asp:HiddenField ID="hiddenedit" runat="server" />
        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesChallanList" />
    </div>
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <%--DEBASHIS--%>
    <%--<div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">                    
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" ClientSideEvents-CheckedChanged="OrginalCheckChange"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" ClientSideEvents-CheckedChanged="DuplicateCheckChange"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">                                   
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>

    <%-- Sandip Approval Dtl Section Start--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="popupApproval" runat="server" ClientInstanceName="cpopupApproval"
            Width="900px" HeaderText="Pending Approvals" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridPendingApproval" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridPendingApproval" OnCustomCallback="gridPendingApproval_CustomCallback">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="CreateDate"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Unit" FieldName="branch_description"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="craetedby"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Approved">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkapprove" runat="server" AllowGrayed="false" OnInit="chkapprove_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                <%--<ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                    </dxe:GridViewDataCheckColumn>

                                    <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Rejected">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkreject" runat="server" AllowGrayed="false" OnInit="chkreject_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                <%--<ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                    </dxe:GridViewDataCheckColumn>
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" />
                                <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <SettingsEditing Mode="Inline">
                                </SettingsEditing>
                                <SettingsSearchPanel Visible="True" />
                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />
                                <ClientSideEvents EndCallback="OnApprovalEndCall" />
                            </dxe:ASPxGridView>
                        </div>
                        <div class="clear"></div>


                        <%--<div class="col-md-12" style="padding-top: 10px;">
                            <dxe:ASPxButton ID="ASPxButton1" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                                AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function (s, e) {SaveApprovalStatus();}" />
                            </dxe:ASPxButton>
                        </div>--%>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="1200px" HeaderText="Quotation Approval" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <HeaderTemplate>
                <span>User Approval</span>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
        <dxe:ASPxPopupControl ID="PopupUserWiseQuotation" runat="server" ClientInstanceName="cPopupUserWiseQuotation"
            Width="900px" HeaderText="User Wise Sales Challan Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridUserWiseQuotation" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridUserWiseQuotation" OnCustomCallback="gridUserWiseQuotation_CustomCallback" OnPageIndexChanged="gridUserWiseQuotation_PageIndexChanged">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Sale Challan No." FieldName="number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Approval User" FieldName="approvedby"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="User Level" FieldName="UserLevel"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Status" FieldName="status"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" />
                                <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <SettingsEditing Mode="Inline">
                                </SettingsEditing>
                                <%--<SettingsSearchPanel Visible="True" />--%>
                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />

                            </dxe:ASPxGridView>
                        </div>
                        <div class="clear"></div>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>

    <%-- Sandip Approval Dtl Section End--%>


    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hddnChallanID" runat="server" />
    </div>
    <dxe:ASPxPopupControl ID="Popup_EWayBill" runat="server" ClientInstanceName="cPopup_EWayBill"
        Width="400px" HeaderText="Update E-Way Bill" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="150px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div class="Top clearfix">

                    <table style="width: 100%; margin: 0 auto; margin-top: 5px;">

                        <tr>
                            <label>
                                <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="E-Way Bill Number">
                                </dxe:ASPxLabel>
                            </label>

                            <dxe:ASPxTextBox ID="txtEWayBillNumber" MaxLength="12" ClientInstanceName="ctxtEWayBillNumber"
                                runat="server" Width="100%">
                                <MaskSettings Mask="<0..999999999999>" AllowMouseWheel="false" />
                                <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                            </dxe:ASPxTextBox>
                        </tr>

                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="E-Way Bill Date">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxDateEdit ID="dt_EWayBill" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_EWayBill" Width="100%" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>


                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="E-Way Bill Value">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtEWayBillValue" ClientInstanceName="ctxtEWayBillValue"
                                    runat="server" Width="100%">
                                    <MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                        <%-- add extra column --%>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel3" runat="server" Text="Transporter GSTIN">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtTransporterGSTIN" ClientInstanceName="ctxtTransporterGSTIN" runat="server" Width="100%">
                                    <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel4" runat="server" Text="Transporter Name">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtTransporterName" ClientInstanceName="ctxtTransporterName"
                                    runat="server" Width="100%">
                                    <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Mode of transportation">
                                    </dxe:ASPxLabel>
                                </label>
                                <asp:DropDownList ID="ddlTransportationMode" runat="server" Width="100%">
                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Road" Value="1" />
                                    <asp:ListItem Text="Rail" Value="2" />
                                    <asp:ListItem Text="Air" Value="3" />
                                    <asp:ListItem Text="Ship" Value="4" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="Distance of transportation">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtTransportationDistance" ClientInstanceName="ctxtTransportationDistance" runat="server" Width="100%">
                                    <MaskSettings Mask="<0..9999>" AllowMouseWheel="false" />
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel7" runat="server" Text="Transporter Doc No">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtTransporterDocNo" ClientInstanceName="ctxtTransporterDocNo" runat="server" Width="100%">
                                    <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel8" runat="server" Text="Transporter Doc Date">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxDateEdit ID="dt_TransporterDocDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_TransporterDocDate" Width="100%" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Vehicle No">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtVehicleNo" ClientInstanceName="ctxtVehicleNo" runat="server" Width="100%">
                                    <%--<MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />--%>
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel10" runat="server" Text="Vehicle Type">
                                    </dxe:ASPxLabel>
                                </label>
                                <asp:DropDownList ID="ddlVehicleType" runat="server" Width="100%">
                                    <asp:ListItem Selected="True" Text="Select" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="ODC" Value="O" />
                                    <asp:ListItem Text="Regular" Value="R" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div style="margin-top: 10px;">
                        <input id="btnEWayBillSave" class="btn btn-primary" onclick="CallEWayBill_save()" type="button" value="Save" />
                        <input id="btnEWayBillCancel" class="btn btn-danger" onclick="CancelEWayBill_save()" type="button" value="Cancel" />
                    </div>

                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />


    </dxe:ASPxPopupControl>
   <%-- REV 2.0--%>
     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">           
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <asp:HiddenField ID="hFilterType" runat="server" />
   <%--  REV 2.0 END--%>
</asp:Content>


