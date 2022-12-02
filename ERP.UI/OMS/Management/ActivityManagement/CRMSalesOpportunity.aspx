


<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CRMSalesOpportunity.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.CRMSalesOpportunity" EnableEventValidation="false" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <%--<link href="../CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../JS/SearchMultiPopup.js"></script>
      <script src="../assests/pluggins/choosen/choosen.min.js"></script>--%>

    <script src="/assests/pluggins/amchart4/core.js"></script>
    
    <script src="/assests/pluggins/amchart4/charts.js"></script>
    <script src="/assests/pluggins/amchart4/animated.js"></script>
        <style>
         .btnOkformultiselection {
            border-width: 1px;
            padding: 4px 10px;
            font-size: 13px !important;
            margin-right: 6px;
            }

    </style>
    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function gridRowclick(s, e) {
            $('#ShowGrid').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                $.each(lists, function (index, value) {
                    setTimeout(function () {
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'ClassSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#ClassModel').modal('hide');
                    ctxtClass.SetText(Name);
                    GetObjectID('hdnClassId').value = key;

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

         //function txtIndicatorA_lostFocus()
         //{
         //    if (parseInt(CtxtIndicatorA.GetValue()) > 0) {

         //        var AVal = parseInt(CtxtIndicatorA.GetValue()) + parseInt(CtxtIndicatorB.GetValue());
         //        AVal = (100 - AVal);
         //        if ((AVal > 100) || (AVal < 0)) {
         //            CtxtIndicatorA.SetValue('0');
         //            jAlert("Value Should Not be greater than 100");
         //        }
         //        else {
         //            CtxtIndicatorC.SetValue(AVal)
         //        }
         //       // CtxtIndicatorC.SetText(100 - (parseInt(CtxtIndicatorA.GetValue()) + parseInt(CtxtIndicatorB.GetValue())))
         //    }
         //}

         //function txtIndicatorB_lostFocus() {
         //    if (parseInt(CtxtIndicatorB.GetValue()) > 0) {

         //        var BVal = parseInt(CtxtIndicatorA.GetValue()) + parseInt(CtxtIndicatorB.GetValue());
         //        BVal = (100 - BVal);

         //        if ((BVal > 100) || (BVal < 0)) {
         //            CtxtIndicatorB.SetValue('0');
         //            jAlert("Value Should Not be greater than 100");
         //        }
         //        else {
         //            CtxtIndicatorC.SetValue(BVal)
         //        }
         //        // CtxtIndicatorC.SetText(100 - (parseInt(CtxtIndicatorA.GetValue()) + parseInt(CtxtIndicatorB.GetValue())))
         //    }
         //}

         function OnEndCallback(s, e) {
             //$("#spntotalproduct").text(cShowGrid.cpTotalProduct);
             //$("#spntotalvalueA").text(cShowGrid.cpTotalA);
             //$("#spntotalvalueB").text(cShowGrid.cpTotalB);
             //$("#spntotalvalueC").text(cShowGrid.cpTotalC);

             //if (s.cpCalledFrom == "ALL" || s.cpCalledFrom == "OPEN" || s.cpCalledFrom == "CLOSED")
             //{
             //    $("#btnOpenSpan").text(s.cpOpenCnt);
             //    $("#btnCloseSpan").text(s.cpClosedCnt);
             //    cShowGrid.Refresh();
             //}
             //s.cpCalledFrom = null;
             
             if (s.cpReturnMessage == "AutoCloseSuccess")
             {
                 $("#autoCloseModal").modal('hide');
                 jAlert(s.cpReturnMessageAutoClose);
                 s.cpReturnMessage = null;
                 GetObjectID('hdnOpenClosedClicked').value = 'All';
                 cCallbackPanel.PerformCallback();
             }
             else if (s.cpReturnMessage == "AutoCloseFail")
             {
                 $("#autoCloseModal").modal('hide');
                 jAlert("Autoclosed Failed");
                 s.cpReturnMessage = null;
             }

             else if (s.cpReturnMessage == "AlreadyClosed") {
                 $("#Closed").modal('hide');
                 jAlert("Opportunity Already Closed");
                 s.cpReturnMessage = null;
             }
             else if (s.cpReturnMessage == "ClosedSuccess") {
                 $("#Closed").modal('hide');
                 jAlert("Closed Successfully");
                 s.cpReturnMessage = null;
                 GetObjectID('hdnOpenClosedClicked').value = 'All';
                 cCallbackPanel.PerformCallback();
             }
             else if (s.cpReturnMessage == "ClosedFailed") {
                 $("#Closed").modal('hide');
                 jAlert("Close Failed");
                 s.cpReturnMessage = null;
             }


             else if (s.cpReturnMessage == "AlreadyOpen") {
                 $("#Reopen").modal('hide');
                 jAlert("Opportunity Already Open");
                 s.cpReturnMessage = null;
             }
             else if (s.cpReturnMessage == "ReopenSuccess") {
                 $("#Reopen").modal('hide');
                 jAlert("Reopened Successfully");
                 s.cpReturnMessage = null;
                 GetObjectID('hdnOpenClosedClicked').value = 'All';
                 cCallbackPanel.PerformCallback();
             }
             else if (s.cpReturnMessage == "ReopneFailed") {
                 $("#Reopen").modal('hide');
                 jAlert("Reopne Failed");
                 s.cpReturnMessage = null;
             }
             

             else if (s.cpLoad == "LOAD") {
                 $("#btnOpenSpan").text(s.cpOpenCnt);
                 $("#btnCloseSpan").text(s.cpClosedCnt);
                 s.cpLoad = null;
                 s.cpOpenCnt = null;
                 s.cpClosedCnt = null;

                 //cShowGrid.Refresh();

             }
             
         }

         $(document).ready(function () {
            // if (CtxtIndicatorA.GetValue())
             //CtxtIndicatorA.SetValue('80');
             //CtxtIndicatorB.SetValue('15');
             //CtxtIndicatorC.SetValue('5');

             if ($("#ddlOnCriteria").val() == "S") {
                 $("#ddlValTech").prop('disabled', true);
             }
             $('#ddlOnCriteria').change(function () {
                 
                 if($("#ddlOnCriteria").val()=="S")
                 {
                     $("#ddlValTech").prop('disabled', true);
                 }
                 else
                 {
                     $("#ddlValTech").prop('disabled', false);
                 }


             });


             $('#ClassModel').on('shown.bs.modal', function () {
                 $('#txtClassSearch').focus();
             })
             
         });
         var ClassArr = new Array();
         $(document).ready(function () {
             var ClassObj = new Object();
             ClassObj.Name = "ClassSource";
             ClassObj.ArraySource = ClassArr;
             //arrMultiPopup.push(ClassObj);
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
            // arrMultiPopup.push(BrandObj);
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
    <script>
        function btn_ShowRecordsClick(e) {
            //cShowGrid.Refresh() ;
            $("#hfIsCRMSalesOpportunity").val("Y");
            GetObjectID('hdnOpenClosedClicked').value = 'All';
            cCallbackPanel.PerformCallback();
            //cShowGrid.Refresh()
            
            
        }

        function btn_OpenClick(e) {
            //cShowGrid.Refresh() ;
            GetObjectID('hdnOpenClosedClicked').value = 'OPEN';
            //cCallbackPanel.PerformCallback();
            cShowGrid.Refresh()
            

        }

        function btn_CloseClick(e) {
            //cShowGrid.Refresh() ;
            GetObjectID('hdnOpenClosedClicked').value = 'CLOSED';
            //cCallbackPanel.PerformCallback();
            cShowGrid.Refresh()
          
        }

        function SaveAutoClose(e) {
            hdnAutoCloseRem.value = txtAutoCloseRem.value;
            cShowGrid.PerformCallback('AUTOCLOSE');
        }

        function selectValue(e)
        {
            var checked_radio = $("[id*=Order_Status] input:checked");
            var value = checked_radio.val();
            //console.log(value)
            if (value == "0")
            {
                $('#grpCloseQty').show();
                $('#grpReason').hide();
            }
            else if (value == "1")
            {
                $('#grpCloseQty').hide();
                $('#grpReason').show();
            }

        }

        function SaveClosedInfo(e)
        {
            var checked_radio = $("[id*=Order_Status] input:checked");
            var value = checked_radio.val();

            if (value!=undefined && value != "")
            {
                hdnClosedStatus.value = value;

                if (value == "0") {
                    hdnCloseReason.value = "";
                    hdnCloseQty.value = CtxtCloseQty.GetValue();
                    hdnCloseRemark.value = txtCloseRemarks.value;
                }
                else if (value == "1") {
                    hdnCloseReason.value = cCmbCloseReason.GetValue();
                    hdnCloseQty.value = 0;
                    hdnCloseRemark.value = txtCloseRemarks.value;
                }



                if (value == "1" && hdnCloseReason.value == "") {
                    jAlert("Select Close Reason");
                }
                else if (hdnCloseRemark.value == "") {
                    jAlert("Enter Close Remarks");
                }
                else {
                    cShowGrid.PerformCallback('CLOSE');
                }
            }
            else {
                jAlert("Select Order Status (Won / Lost)");
            }
            
            
        }

        function SaveReopenInfo(e)
        {
            hdnReopenFeedback.value = txtReopenFeedback.value;
            if (hdnReopenFeedback.value == "") {
                jAlert("Enter Reopen Feedback")
            }
            else {
                cShowGrid.PerformCallback('REOPEN');
            }
            
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

        $(document).keydown(function (e) {
            if (e.keyCode == 27) {
                cpopup.Hide();
                cShowGrid.Focus();
            }
        });

        function CallbackPanelEndCall(s, e) {
            //$("#btnOpen").val(hdnOpenCnt.value());
            //$("#btnClose").val(hdnClosedCnt.value());
            cShowGrid.Refresh();
        }
    </script>

    <style>
        .pdbot > tbody > tr > td {
            padding-bottom: 10px;
        }
    </style>

    <style>
        .colDisable {
        cursor:default !important;
        }
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
        .pb-5 {
            padding-bottom:5px
        }
        .padTableR{
            width:auto;
            margin-top:5px;
        }
        .padTableR>tbody>tr>td:not(:last-child){
            padding-right:10px;
            vertical-align:middle;
        }
        .popover{
            max-width:50%;
        }
        .cPoint{
            cursor:pointer;
        }
        /*btn badge items*/
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
            background:#fff;
            color:#333;
            margin-left:4px;
            font-size: 14px;
        }
        .btn-badge-color1:hover, .btn-badge-color2:hover, .btn-badge-color3:hover,.btn-badge-color4:hover,.btn-badge-color5:hover,.btn-badge-color6:hover,
        .btn-badge-color1:focus, .btn-badge-color2:focus, .btn-badge-color3:focus,.btn-badge-color4:focus,.btn-badge-color5:focus,.btn-badge-color6:focus {
            color:#fff;
            opacity:0.8;
        }
        .btn-badge-color1 {
            /*background:chocolate;
            color:#fff;*/
            /*background:#d9c015;*/
            background:#ecd21dcc;
            color:#fff;
        }
        .btn-badge-color2 {
            background:#30cb57;
            color:#fff;
        }
        .btn-badge-color3 {
            background:#ff6a00;
            color:#fff;
        }
        .btn-badge-color4 {
            background:#83bcdd;
            color:#fff;
        }
        .btn-badge-color5 {
            background:#7cdfcc;
            color:#fff;
        }
         .btn-badge-color6 {
            background:#3366FF;
            color:#fff;
        }
        .nocursor{
            cursor:default;
        }
        .auto-style1 {
            height: 145px;
        }
        .tableSpaced>tbody>tr>td:not(:last-child) {
            padding-right:10px
        }
        .filterParent {
            position:relative;
            display:inline-block;
        }
        .filterDrop {
            background: #fbfbfb;
            position: absolute;
            top: 85%;
            left: 0px;
            padding: 15px;
            border-radius: 5px;
            min-width: 400px;
            box-shadow: 1px 6px 10px 0px rgb(0 0 0 / 18%), 1px 0px 10px 0px rgb(0 0 0 / 13%);
            display:none;
            opacity:0;
            -webkit-transition: top 0.3s ease-out, opacity 0.3s ease 0.5s;
            -moz-transition: top 0.3s ease-out, opacity 0.3s ease 0.5s;
            -o-transition: top 0.3s ease-out, opacity 0.3s ease 0.5s;
            transition: top 0.3s ease-out, opacity 0.3s ease 0.5s;   
            z-index: 9;  
        }
        .filterDrop.active {
            display:block;
            opacity:1;
            top:92%
        }
        .mainFilterList {
            list-style-type:none;
            padding:0
        }
        .mainFilterList>li {
            padding: 7px 5px;
        }
        .mainFilterList>li.hdType {
            background:#ddd
        }
        .mainFilterList>li:not(:last-child) {
            border-bottom: 1px solid #ececec;
        }
        .mainFilterList>li .badge {
            float:right;
            display:inline-block
        }
        .mainFilterList>li .txts {
            max-width: 80%;
            display: inline-block;
            padding-left: 8px;
        }
        .filterBadgeColor {
            background: #ffe5c5;
            color: #333;
            border: 1px solid #efa74e;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cShowGrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cShowGrid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cShowGrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cShowGrid.SetWidth(cntWidth);
                }

            });
            $(".filterToggle").on("click", function (e) {
                e.preventDefault();
                $(this).parent(".filterParent").find(".filterDrop").toggleClass("active")
                
            })

        });
        $(function () {
            $('[data-toggle="popover"]').popover()
        })
        function CloseModalHandler(obj) {
            hdnOpportunityID.value = obj;

            var data = obj.split('~');
            if (data[1] == "True") {
                jAlert("Opportunity Already Closed");
            }
            else {
                cCmbCloseReason.SetValue("");
                CtxtCloseQty.SetValue(0);
                txtCloseRemarks.value = "";
                $("[id*=Order_Status] input").prop("checked", false);

                $('#grpCloseQty').show();
                $('#grpReason').show();

                $("#Closed").modal("show");
            }
        }
        function ReopenModalHandler(obj) {
            hdnOpportunityID.value = obj;

            var data = obj.split('~');
            if (data[1] == "False") {
                jAlert("Opportunity Already Open");
            }
            else if (data[2] == "True") {
                jAlert("Opportunity Cannot be Re-Opened");
            }
            else {
                txtReopenFeedback.value = "";

                $("#Reopen").modal("show");
            }

            
        }

        function getListFilter() {
            $.ajax({
                type: "POST",
                url: "CRMSalesOpportunity.aspx/GetSaleOpportunitiesCloseReason",
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (responseFromServer) {
                    var html = '<li class="hdType"><input type="checkbox" onclick="setAllselection()" value="" /> <span class="txts">SELECT ALL</span></li>'
                    for (i = 0; i < responseFromServer.d.length; i++) {
                        html += "<li><input type='checkbox' id=" + responseFromServer.d[i].ID + "  class='txts' onclick=CheckParticular($(this).is(':checked')) value=" + responseFromServer.d[i].ID + "  /><span class='txts'>" + responseFromServer.d[i].Close_Reason + "</span> <span class='badge filterBadgeColor'>" + responseFromServer.d[i].Close_Reason_Count + " </span> </a></li>";
                    }
                    $("#slItems").html(html);
                }
            });
           
        }
        function itemClick(){
            var ar = [];
           
            $("#slItems").find(":checkbox:checked").each(function () {
                var values = $(this).val();
                if(values =='') {

                }else {
                    ar.push($(this).val());
                }
               
            });
            var strng = ar.toString();
            $(".filterDrop").removeClass("active");
            GetObjectID('hdnOpenClosedClicked').value = 'FILTERED_VALUE'
            GetObjectID('hdnFilteredValue').value = strng;
            cShowGrid.Refresh()
            
        }
        function setAllselection(e) {
            $("#slItems").find(":checkbox.txts").attr("checked", true);

        }
        function openPop() {
            $("#Analytics").modal('show')
        }
        function refreshChart(){
            var f = cmbfrmDatean.GetDate().toISOString().substr(0, 19).replace('T', ' ').split(" ")[0];
            var t = cmbtoDatean.GetDate().toISOString().substr(0, 19).replace('T', ' ').split(" ")[0];
           
            var salesmaId = $("#salesmanList").val();

            GetDataforChart(f, t, salesmaId)
            GencloseReasonChart(f, t, salesmaId)

        }
        function GencloseReasonChart(frmDat, ttDat, salesmaId) {
            $.ajax({
                type: "POST",
                url: "CRMSalesOpportunity.aspx/GetChartDataForCloseReason",
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                data: JSON.stringify({ 'frmDate': frmDat, 'toDate': ttDat, "salesmaId": salesmaId }),
                success: function (data) {
                    console.log("GetChartDataForCloseReason", data)
                    //var withColor = data.d.map(function (item) {
                    //    var element = {};
                    //    if (item.Close_Reason == "Order Won") {
                    //        element.title = item.Close_Reason,
                    //        element.value = item.Count,
                    //        element.color = "#53d593"
                    //    } else if (item.Close_Reason == "Order Lost") {
                    //        element.title = item.Close_Reason,
                    //        element.value = item.Count,
                    //        element.color = "#f94b4b"
                    //    } else if (item.Close_Reason == "Open") {
                    //        element.title = item.Close_Reason,
                    //        element.value = item.Count,
                    //        element.color = "#e9d335"
                    //    }
                    //    return element
                    //})
                    am4core.useTheme(am4themes_animated);
                    var chart = am4core.create("closeReasonDiv", am4charts.PieChart);

                    // Add data
                    chart.data = data.d;

                    // Add and configure Series
                    var pieSeries = chart.series.push(new am4charts.PieSeries());
                    pieSeries.dataFields.value = "Count";
                    pieSeries.dataFields.category = "Close_Reason";
                    pieSeries.slices.template.propertyFields.fill = "color"

                    pieSeries.labels.template.maxWidth = 130;
                    pieSeries.labels.template.wrap = true;

                    chart.legend = new am4charts.Legend();
                    chart.legend.position = "right";
                    
                }
            });
        }
        
        
        function GetDataforChart(frmDat, ttDat, salesmaId) {
           
            $.ajax({
                type: "POST",
                url: "CRMSalesOpportunity.aspx/GetChartData",
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                data: JSON.stringify({ 'frmDate': frmDat, 'toDate': ttDat, "salesmaId": salesmaId }),
                success: function (data) {
                    var withColor = data.d.map(function (item) {
                        var element = {};
                        if (item.Close_Reason == "Order Won") {
                            element.title = item.Close_Reason,
                            element.value = item.Count,
                            element.color = "#53d593"
                        } else if (item.Close_Reason == "Order Lost") {
                            element.title = item.Close_Reason,
                            element.value = item.Count,
                            element.color = "#f94b4b"
                        } else if (item.Close_Reason == "Open") {
                            element.title = item.Close_Reason,
                            element.value = item.Count,
                            element.color = "#e9d335"
                        }
                        return element
                    })
                    
                    GenchartPie(withColor)
                }
            });
        }

        function GenchartPie(data) {
            console.log("bdata", data)
            //$("#chartdiv").html("");
            am4core.useTheme(am4themes_animated);
            var chart = am4core.create("chartdiv", am4charts.PieChart);

            // Add data
            chart.data = data;

            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            pieSeries.dataFields.value = "value";
            pieSeries.dataFields.category = "title";
            pieSeries.slices.template.propertyFields.fill = "color"

            pieSeries.labels.template.maxWidth = 130;
            pieSeries.labels.template.wrap = true;

            chart.legend = new am4charts.Legend();
            chart.legend.position = "right";

            //am4core.useTheme(am4themes_animated);
            //var chart = am4core.create("chartdiv", am4charts.PieChart);
            //chart.data = data;
            //var pieSeries = chart.series.push(new am4charts.PieSeries());
            //pieSeries.dataFields.value = "title";
            //pieSeries.dataFields.category = "value";
            //pieSeries.slices.template.propertyFields.fill = "color";
            //// this creates initial animation
            //pieSeries.hiddenState.properties.opacity = 1;
            //pieSeries.hiddenState.properties.endAngle = -90;
            //pieSeries.hiddenState.properties.startAngle = -90;
            //chart.legend = new am4charts.Legend();
            //chart.legend.position = "right";

        }
        function getSalesman() {
            
            $.ajax({
                type: "POST",
                url: "CRMSalesOpportunity.aspx/GetSalemanHierarchy",
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                data: JSON.stringify({}),
                success: function (data) {
                    var response = data.d
                    var d = "";
                    for (let i = 0; i < response.length; i++) {
                        d += "<option value='" + response[i].Salesman_Id + "'>" + response[i].Salesman_Name + "</option>";
                    }
                    $("#salesmanList").html(d);
                }
            });
        }
        getSalesman()

    </script>
    <style>
        .chartTab.nav-tabs > li a{
        font-weight: 600;
        font-size: 14px;
        text-transform: uppercase;
        color:#ddd
    }
        .chartTab.nav-tabs > li a:hover {
            background-color: transparent !important;
            border: none;
            color: #7e7b7b;
        } 
    .chartTab.nav-tabs > li.active > a, .chartTab.nav-tabs > li.active > a:hover, .chartTab.nav-tabs > li.active > a:focus {
        border: none;
        color: #1477bd;
        cursor: default;
        background-color: transparent !important;
        border-bottom: 2px solid #1477bd;
        border-bottom-color: #1477bd;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div class="modal fade pmsModal " id="Analytics" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document" style="margin-top:0 !important">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Opportunity Close</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-3">
                            <div>
                                <label>Select Salesman</label>
                                    <select class="selectpicker form-control" data-live-search="true" data-size="8"  id="salesmanList"></select>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div>
                                <label>From Date</label>
                                <dxe:ASPxDateEdit ID="frmDatean" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cmbfrmDatean">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div>
                                    <label>To Date </label>
                                <dxe:ASPxDateEdit ID="toDatean" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cmbtoDatean">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>

                                </dxe:ASPxDateEdit>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div>
                                <label style="display: block; margin: 0;">&nbsp</label>
                                <button class="btn btn-success" type="button" onclick="refreshChart()">Generate</button>
                            </div>
                        </div>
                                <%--Rev Sanchita--%>                       
                                <%--End of Rev Sanchita--%>

                    </div>
                    <div class="clear"></div>
                     <ul class="nav nav-tabs chartTab" role="tablist">
                        <li role="presentation" class="active"><a href="#home" aria-controls="home" role="tab" data-toggle="tab">Opportunity Close</a></li>
                         <li role="presentation" class=""><a href="#closeReason" aria-controls="home" role="tab" data-toggle="tab">Close Reason Wise</a></li>
                    </ul>
                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="home">
                            <div id="chartdiv" style="min-height:60vh"></div>
                        </div>
                        <div role="tabpanel" class="tab-pane " id="closeReason">
                            <div id="closeReasonDiv" style="min-height:60vh"></div>
                        </div>
                    </div>    
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>



    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3 class="">
                <asp:Label id="lblSalesActivity" runat="server" Text="Opportunities" />
            </h3>   
        </div>
        <div class="pull-right">
                <table class="tableSpaced">
                    <tr>
                        <td>From </td>
                        <td>
                            <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td>To </td>
                        <td>
                            <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                                UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>

                            </dxe:ASPxDateEdit>
                        </td>
                        <td style="width:180px">
                            <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>

                            <% if (rights.CanExport) { %>
                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="drdExport_SelectedIndexChanged"
                                    AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">XLSX</asp:ListItem>
                                    <asp:ListItem Value="2">PDF</asp:ListItem>
                                    <asp:ListItem Value="3">CSV</asp:ListItem>
                                    <asp:ListItem Value="4">RTF</asp:ListItem>
                                </asp:DropDownList>
                            <% } %>
                        </td>
                    </tr>
                </table>
                <div id="btncross" class="crossBtn" style="display:none;margin-left:50px;"><a href="Sales_List.aspx"><i class="fa fa-times"></i></a></div>
            </div>
    </div>
    <div class="form_main">
        <div class="row">
            <div class="col-md-8 relative ">
                <button id="btnOpen" type="button" onclick="btn_OpenClick(this);" class="btn btn-success">Open <span id="btnOpenSpan" class="badge">0</span></button>
                 <button id="btnClose" type="button" onclick="btn_CloseClick(this);" class="btn btn-warning">Closed <span id="btnCloseSpan" class="badge">0</span></button>
                    <div class="filterParent">
                    <button class="btn btn-success  filterToggle" type="button" onclick="getListFilter()">Filter <span class="badge" style="padding: 5px 2px; margin-left: 8px;"><i class="fa fa-filter"></i></span></button>
                    <div class="filterDrop">
                        <div class="row">
                            <div class="col-sm-12">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <ul class="mainFilterList" id="slItems">
                                            
                                            
                                            
                                        </ul>
                                    </div>
                               
                                </div>
                            </div>
                            <div class="col-md-12 text-right"><button type="button" onclick="itemClick()" class="btn btn-default">OK</button></div>
                        </div>
                    </div>
                    </div>
                <% if (rights.CanAutoCloseOpportunities)
                   { %>
                    <button class="btn btn-primary" type="button"  data-toggle="modal" data-target="#autoCloseModal" style="padding: 6px 10px;">Auto Close </button>
                <% } %>
                <button class="btn btn-info" type="button"  onclick="openPop()" style="padding: 6px 10px;">Analytics </button>
            </div>
        </div>
        <div class="clear"></div>
        <div class="clearfix relative">
           
            <%--OnCustomCallback="GrdOrder_CustomCallback"
                OnHtmlRowPrepared="AvailableStockgrid_HtmlRowPrepared"
                --%>

            <dxe:ASPxGridView ID="ShowGrid" runat="server" KeyFieldName="ID" AutoGenerateColumns="False" DataSourceID="GenerateEntityServerModeDataSource" 
                SettingsDataSecurity-AllowEdit="false"
                SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"
                Width="100%" ClientInstanceName="cShowGrid" OnCustomCallback="ShowGrid_CustomCallback"
                SettingsBehavior-AllowFocusedRow="true" 
                HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto">
                <SettingsSearchPanel Visible="True" Delay="5000" />

                <Columns>
                    <dxe:GridViewDataTextColumn FieldName="Assigned_To" Caption="Salesman" Width="120"
                        VisibleIndex="1">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="Industry" Caption="Industry"
                        VisibleIndex="2" Width="150px" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="Name" Caption="Customer/Lead" Width="120"
                        VisibleIndex="3">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="ProductClasName" Caption="Product Class"
                        VisibleIndex="4" Width="150px" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="Closed_Date" Caption="Date of Closing" Width="120"
                        VisibleIndex="5">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="budget" Caption="Product:Budget"
                        VisibleIndex="6" Width="150px" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="budget_Monthwise" Caption="Product:Budget/Month" Width="150"
                        VisibleIndex="7">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="Close_Reason" Caption="Close Reason"
                        VisibleIndex="8" Width="150px" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="Close_Qty" Caption="Qty" Width="120"
                        VisibleIndex="9">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="Close_Remarks" Caption="Close Remarks"
                        VisibleIndex="10" Width="150px" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="Reopen_Feedback" Caption="Reopen Feedback"
                        VisibleIndex="11" Width="150px" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="Closed_Status" Caption="Order Outcome"
                        VisibleIndex="12" Width="150px" Settings-ShowFilterRowMenu="True">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    

                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="13" Width="0">
                        <DataItemTemplate>
                            <div class="floatedIcons">
                                <div class='floatedBtnArea'>
                                    <% if (rights.CanCloseOpportunities)
                                       { %>
                                    <a href="javascript:void(0);"  class="" title="" onclick="CloseModalHandler('<%# Eval("ID") %>'+'~'+'<%# Eval("closed") %>'+'~'+'<%# Eval("Locked") %>')">
                                        <span class='ico ColorSix'><i class='fa fa-eye'></i></span><span class='hidden-xs'>Close</span></a>
                                    <% } %>
                                    <% if (rights.CanReopenOpportunities)
                                       { %>
                                    <a href="javascript:void(0);"  class="" title="" onclick="ReopenModalHandler('<%# Eval("ID") %>'+'~'+'<%# Eval("closed") %>'+'~'+'<%# Eval("Locked") %>')">

                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Reopen</span></a>  
                                    <% } %>


                                    
                                    
                                </div>
                            </div>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    
                </Columns>
               

               <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                <SettingsCookies Enabled="true" StorePaging="true" StoreColumnsVisiblePosition="true" Version="3.6" />
               <SettingsContextMenu Enabled="true"></SettingsContextMenu> 
                 <ClientSideEvents EndCallback="OnEndCallback" RowClick="gridRowclick" /> 
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>
                <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="false" />
                <SettingsLoadingPanel Text="Please Wait..." />
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
                </TotalSummary>
            </dxe:ASPxGridView>
            <asp:HiddenField ID="hiddenedit" runat="server" />
            <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="CRM_SalesOpportunities_Temp" />
        </div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

  <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsCRMSalesOpportunity" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
 </dxe:ASPxCallbackPanel>

<!-- autoCloseModal  Modal -->
<div class="modal fade pmsModal w40" id="Closed" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" id="">Reason for Close</h4>
      </div>
      <div class="modal-body">
          
          <div class="form-group">
              <asp:RadioButtonList ID="Order_Status" ClientInstanceName="cOrder_Status" runat="server" RepeatDirection="Horizontal" onchange="selectValue();" Width="190px">
                    <asp:ListItem Text="Order Won" Value="0" ></asp:ListItem>
                    <asp:ListItem Text="Order Lost" Value="1"></asp:ListItem>
                </asp:RadioButtonList>
         </div>

          <div class="form-group" id="grpReason">
               <label for="CmbCloseReason">Reason <span class="red">*</span></label>

                <dxe:ASPxComboBox ID="CmbCloseReason" ClientInstanceName="cCmbCloseReason" ValueField="ID" TextField="Close_Reason" runat="server" ValueType="System.String" Width="100%" 
                    EnableSynchronization="True" >
                </dxe:ASPxComboBox>
         </div>
          <div class="form-group" id="grpCloseQty">
               <label for="txtCloseQty">Qty <span class="red">*</span></label>
               <dxe:ASPxTextBox ID="txtCloseQty" runat="server" TextMode="Number" Width="100%" ClientInstanceName="CtxtCloseQty">
                    <MaskSettings Mask="<0..99999999>" AllowMouseWheel="false" />
                </dxe:ASPxTextBox>
            </div>
         <div class="form-group">
               <label for="txtCloseRemarks">Remarks <span class="red">*</span></label>
                <textarea id="txtCloseRemarks" class="form-control" rows="5"></textarea>
          </div>
          
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-success" onclick="SaveClosedInfo()" >Save </button>
      </div>
    </div>
  </div>
</div>
  <!-- autoCloseModal  Modal -->
<div class="modal fade pmsModal w40" id="Reopen" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" id="">Reason for Reopen</h4>
      </div>
      <div class="modal-body">
         <div class="form-group">
            <label for="AutoCloseText">Feedback <span class="red">*</span></label>
           <textarea id="txtReopenFeedback" class="form-control" rows="5"></textarea>
          </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-success" onclick="SaveReopenInfo()" >Save </button>
      </div>
    </div>
  </div>
</div>   
<!-- autoCloseModal  Modal -->
<div class="modal fade pmsModal w40" id="autoCloseModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" id="myModalLabel">Reason for Close</h4>
      </div>
      <div class="modal-body">
         <div class="form-group">
            <label for="AutoCloseText">Remarks <span class="red">*</span></label>
           <textarea id="txtAutoCloseRem" class="form-control" rows="5"></textarea>
          </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
        <button type="button" class="btn btn-success" onclick="SaveAutoClose()" >Save </button>
      </div>
    </div>
  </div>
</div>
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

    <asp:HiddenField ID="hdnOpenClosedClicked" runat="server" />
    <asp:HiddenField ID="hdnAutoCloseRem" runat="server" />
    <asp:HiddenField ID="hdnOpenCnt" runat="server" />
    <asp:HiddenField ID="hdnClosedCnt" runat="server" />

    <asp:HiddenField ID="hdnCloseReason" runat="server" />
    <asp:HiddenField ID="hdnCloseQty" runat="server" />
    <asp:HiddenField ID="hdnCloseRemark" runat="server" />
    <asp:HiddenField ID="hdnOpportunityID" runat="server" />
    <asp:HiddenField ID="hdnReopenFeedback" runat="server" />
    <asp:HiddenField ID="hdnClosed" runat="server" />
    <asp:HiddenField ID="hdnClosedStatus" runat="server" />

    <asp:HiddenField ID="hdnFilteredValue" runat="server" />


    <!--Class Modal -->
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
</asp:Content>
