<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                01-03-2023        2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CustomerAnalysis.aspx.cs" Inherits="Reports.Reports.GridReports.CustomerAnalysis" %>

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

        #ListBoxProjects{
            width: 200px;
        }

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
            bottom: 200px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/
    </style>
    
   


    <script type="text/javascript">

        //for Esc
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && popupdetails.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                popupHide();
            }
        }
        function popupHide(s, e) {
            popupdetails.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }

        function fn_OpenDetails(keyValue) {
            Grid.PerformCallback('Edit~' + keyValue);
        }

        $(function () {
            cProjectPanel.PerformCallback('BindProjectGrid');
        });

        $(function () {
            cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());

        });


        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
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

            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                $('#MandatoryActivityType').attr('style', 'display:none');
                $("#hdnSelectedBranches").val('');
                cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })

        })
    </script>
 <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'CustomerSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#CustModel').modal('hide');
                    ctxtCustName.SetText(Name);
                    GetObjectID('hdnCustomerId').value = key;
                }
                else {
                    ctxtCustName.SetText('');
                    GetObjectID('hdnCustomerId').value = '';
                }
            }

        }

    </script>
  <%-- For multiselection when click on ok button--%>
   <%-- For multiselection--%>
     <script type="text/javascript">
         $(document).ready(function () {
             $('#CustModel').on('shown.bs.modal', function () {
                 $('#txtCustSearch').focus();
             })
         })
         var CustArr = new Array();
         $(document).ready(function () {
             var CustObj = new Object();
             CustObj.Name = "CustomerSource";
             CustObj.ArraySource = CustArr;
             arrMultiPopup.push(CustObj);
         })
         function CustomerButnClick(s, e) {
             $('#CustModel').modal('show');
         }

         function Customer_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#CustModel').modal('show');
             }
         }

         function Customerkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtCustSearch").val()) == "" || $.trim($("#txtCustSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtCustSearch").val();
             // OtherDetails.BranchID = $('#ddl_Branch').val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Customer Name");
                 HeaderCaption.push("Unique ID");
                 HeaderCaption.push("Alternate No.");
                 HeaderCaption.push("Address");


                 if ($("#txtCustSearch").val() != "") {
                     //callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues");
                     callonServerM("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "CustomerSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }


         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtCustSearch').focus();
             else
                 $('#txtCustSearch').focus();
         }
   </script>
      <%-- For multiselection--%>


    <script type="text/javascript">

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            var ProjectSelectionInReport = document.getElementById('hdnProjectSelectionInReport').value;
            e.preventDefault;
            var data = "OnDateChanged";
            $("#drdExport").val(0);
            $("#hfIsCustAnalysisDetFilter").val("Y");

            //if (ctxtCustName.GetValue() == null & chkallcust.checked == false) {
            if (ctxtCustName.GetValue() == null & Cchkallcust.GetValue() == false) {
                jAlert('Please select Customer for generate the report.');
            }
            else if (ProjectSelection == "1") {
                if (ProjectSelectionInReport == "Yes" && gridprojectLookup.GetValue() == null) {
                    jAlert('Please select atleast one Project for generate the report.');
                }
                else if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val());
                }
            }
            else {
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val());
                }
            }

            var ToDate = (cxdeAsOnDate.GetValue() != null) ? cxdeAsOnDate.GetValue() : "";

            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "As On: " + ToDate;
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

        function CallbackPanelEndCall(s, e) {
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            GridCustAnalysis.Refresh();
        }

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
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

    </script>
    <script>
        //function CustAll(obj) {
        //    if (obj == 'allcust') {
        //        if (chkallcust.checked == true) {
        //            ctxtCustName.SetText('');
        //            GetObjectID('hdnCustomerId').value = '';
        //            document.getElementById("txtCustSearch").value = ""
        //            ctxtCustName.SetEnabled(false);
        //        }
        //        else {
        //            ctxtCustName.SetEnabled(true);
        //        }
        //    }
        //}

        function CheckAllCust(s, e) {
            if (s.GetCheckState() == 'Checked') {
                ctxtCustName.SetText('');
                GetObjectID('hdnCustomerId').value = '';
                document.getElementById("txtCustSearch").value = ""
                ctxtCustName.SetEnabled(false);
            }
            else {
                ctxtCustName.SetEnabled(true);
            }
        }

        function CallbackCustAnalysis_BeginCallback() {
            $("#drdExport").val(0);
        }

        function CloseGridBranchLookup() {
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
        .paddingTbl>tbody>tr>td {
            padding-right:20px;
        }
        .marginTop10 {
            margin-top:20px;
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
            bottom: 7px;
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
            line-height: 18px;
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridCustOut , .TableMain100 #ShowGridCustAnalysis
        {
            max-width: 98% !important;
        }

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


        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/

        .custom-position
        {
            display: inline-block;
    float: left;
        }

        .mt-20
        {
            margin-top: 10px;
            margin-bottom: 10px;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                GridCustAnalysis.SetWidth(cntWidth);
                console.log(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                GridCustAnalysis.SetWidth(cntWidth);
                console.log(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    GridCustAnalysis.SetWidth(cntWidth);
                    console.log(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    GridCustAnalysis.SetWidth(cntWidth);
                    console.log(cntWidth);
                }

            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <asp:HiddenField ID="hdnexpid" runat="server" />
    </div>

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

        <div>
            
        </div>
        
    </div>
    <%--Rev 1.0: "outer-div-main" class add: --%>
    <div class="outer-div-main">
        <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row">
            <div class="col-md-3 col-lg-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">Head Branch:</label>
                <%--Rev 1.0 : "simple-select" class add--%>
                <div class="simple-select">
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3 col-lg-2 branch-selection-box">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"></asp:Label></label>
                <asp:HiddenField ID="hdnActivityType" runat="server" />

                <dxe:ASPxCallbackPanel runat="server" ID="ComponentBranchPanel" ClientInstanceName="cBranchComponentPanel" OnCallback="Componentbranch_Callback">
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
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" UseSubmitBehavior="False" />
                                                      <%--  </div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" UseSubmitBehavior="False" />                                                        
                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridBranchLookup" UseSubmitBehavior="False" />
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

                <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
            </div>

            <div class="col-md-2 col-lg-2">
                <label style="color: #b5285f; font-weight: bold; margin-top: 8px;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="As On Date : " CssClass="mylabel1"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxAsOnDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeAsOnDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>

            <div class="col-md-2 col-lg-1 lblmTop8">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="On Date: " CssClass="mylabel1"></asp:Label>
                </label>
                <%--Rev 1.0 : "simple-select" class add--%>
                <div class="simple-select">
                    <asp:DropDownList ID="ddlondate" runat="server" Width="100%">
                        <asp:ListItem Text="Bill Date" Value="B"></asp:ListItem>
                        <asp:ListItem Text="Due Date" Value="D"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3 col-lg-2">
                <span style="display: inline-block;"><dxe:ASPxLabel ID="lbl_Customer" style="color: #b5285f;" runat="server" Text="Customer:">
                </dxe:ASPxLabel></span>                        
                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
                <span id="MandatorysCustomer" style="display: none" class="validclass">
                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
            </div>
            <div class="col-md-2 col-lg-1" style="padding:0;padding-top: 30px; width: 110px;">
                <%--<asp:CheckBox runat="server" ID="chkallcust" Checked="false" Text="All Customer" />--%>
                <dxe:ASPxCheckBox runat="server" ID="chkallcust" Checked="false" Text="All Customer" ClientInstanceName="Cchkallcust">
                     <ClientSideEvents CheckedChanged="CheckAllCust" />
                </dxe:ASPxCheckBox>
            </div>
            <div class="col-md-2 col-lg-2" style="padding-top: 30px; width: 155px;">
                <%--<asp:CheckBox runat="server" ID="chkduedate" Checked="false" Text="Show Due Date" />--%>
                <dxe:ASPxCheckBox runat="server" ID="chkduedate" Checked="false" Text="Show Due Date">
                </dxe:ASPxCheckBox>
            </div>
            <div class="clear"></div>

            

            <div class="col-md-12">
                <table class="paddingTbl marginTop10">
                <tr>
                   <%-- <td><asp:CheckBox runat="server" ID="chkprintdays" Checked="false" Text="Show Days" /></td>
                    <td><asp:CheckBox runat="server" ID="chksalesman" Checked="false" Text="Show Salesman" /></td>
                    <td> <asp:CheckBox runat="server" ID="chkcb" Checked="false" Text="Include Cash/Bank" /></td>
                    <td><asp:CheckBox runat="server" ID="chkjv" Checked="false" Text="Include Journal" /></td>
                    <td><asp:CheckBox runat="server" ID="chkdncn" Checked="false" Text="Exclude Debit/Credit Note" /></td>--%>
                    <td><dxe:ASPxCheckBox runat="server" ID="chkprintdays" Checked="false" Text="Show Days">
                    </dxe:ASPxCheckBox></td>
                    <td><dxe:ASPxCheckBox runat="server" ID="chksalesman" Checked="false" Text="Show Salesman">
                    </dxe:ASPxCheckBox></td>
                    <td><dxe:ASPxCheckBox runat="server" ID="chkcb" Checked="false" Text="Include Cash/Bank">
                    </dxe:ASPxCheckBox></td>
                    <td><dxe:ASPxCheckBox runat="server" ID="chkjv" Checked="false" Text="Include Journal">
                    </dxe:ASPxCheckBox></td>
                    <td><dxe:ASPxCheckBox runat="server" ID="chkdncn" Checked="false" Text="Exclude Debit/Credit Note">
                    </dxe:ASPxCheckBox></td>

                    <td><dxe:ASPxCheckBox runat="server" ID="chkSoNoDt" Checked="false" Text="Show Order No & Date">
                    </dxe:ASPxCheckBox></td>
                    
                                       
                </tr>
            </table>
            </div>
            <div class="clear"></div>
            <div class="col-md-12  mt-20">
                <table class="paddingTbl marginTop10 custom-position">
                    <tr>
                         <td><dxe:ASPxCheckBox runat="server" ID="chkPartyOrdNoDt" Checked="false" Text="Show Party Order No & Date">
                    </dxe:ASPxCheckBox></td>
                        
                    </tr>
                </table>

                <div class="col-md-2 lblmTop8" style="" id="divProj">
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

                <table class="paddingTbl marginTop10  custom-position">
                    <tr>
                        <td >
                            <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                        <% if (rights.CanExport)
                        { %> 
                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                                OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                <%--<asp:ListItem Value="1">XLSX</asp:ListItem>
                                <asp:ListItem Value="2">PDF</asp:ListItem>
                                <asp:ListItem Value="3">CSV</asp:ListItem>
                                <asp:ListItem Value="4">RTF</asp:ListItem>--%>
                                <asp:ListItem Value="1">EXCEL</asp:ListItem>
                                <asp:ListItem Value="2">PDF</asp:ListItem>
                                <asp:ListItem Value="3">CSV</asp:ListItem>
                            </asp:DropDownList>
                        <% } %>
                    </td>
                    </tr>
                </table>
                <div class="clear"></div>
            </div>

            <div class="clear"></div>
            
            <%--<div class="col-md-2">
                <label style="margin-bottom: 0">&nbsp</label>
                <div class="">
                </div>
            </div>--%>
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2">
                  
                       <div>
                           <dxe:ASPxGridView runat="server" ID="ShowGridCustAnalysis" ClientInstanceName="GridCustAnalysis" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                               OnSummaryDisplayText="ShowGridCustAnalysis_SummaryDisplayText" Settings-VerticalScrollBarMode="Auto" Settings-HorizontalScrollBarMode="Auto"
                               DataSourceID="GenerateEntityServerModeDataSource" ClientSideEvents-BeginCallback="CallbackCustAnalysis_BeginCallback" Settings-VerticalScrollableHeight="300"
                               OnHtmlFooterCellPrepared="ShowGridCustAnalysis_HtmlFooterCellPrepared" OnHtmlDataCellPrepared="ShowGridCustAnalysis_HtmlDataCellPrepared">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Customer Name"  FieldName="PARTYNAME" GroupIndex="0" VisibleIndex="0">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BRANCH_DESCRIPTION" Width="210px" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                            
                                 <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DOC_TYPE" Width="190px" Caption="Doc. Type" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                     <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ISOPENING" Width="60px" Caption="Opening?" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="PROJ_NAME" Width="200px" Caption="Project Name" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DOC_NO" Width="120px" Caption="Doc. No." HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Doc. Date" Width="80px" FieldName="DOC_DATE" VisibleIndex="6" HeaderStyle-CssClass="colDisable">
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Due Date" Width="80px" FieldName="DUE_DATE" VisibleIndex="7" HeaderStyle-CssClass="colDisable">
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="SONO" Width="120px" Caption="Order No." HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Order Date" Width="120px" FieldName="SODATE" VisibleIndex="9" HeaderStyle-CssClass="colDisable">
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="PARTYORD_NO" Width="120px" Caption="Party Order No." HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Party Order Date" Width="120px" FieldName="PARTYORD_DATE" VisibleIndex="11" HeaderStyle-CssClass="colDisable">
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Salesman" FieldName="SALESMAN" Width="120px" VisibleIndex="12" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                            
                                <dxe:GridViewDataTextColumn FieldName="DOC_AMOUNT" Caption="Doc. Amt." Width="110px" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                                     <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <%--<PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>--%>
                                     <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="ADJ_DOC_TYPE" Width="180px" Caption="Adj. Doc. Type" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="ADJ_DOC_NO" Width="120px" Caption="Adj. Doc. No." HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Adj. Doc. Date" Width="90px" FieldName="ADJ_DOC_DATE" VisibleIndex="16" HeaderStyle-CssClass="colDisable">
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ADJ_AMOUNT" Caption="Adj. Amt." Width="90px" VisibleIndex="17" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                                     <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <%--<PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>--%>
                                     <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BAL_AMOUNT" Caption="Balance Amount" Width="110px" VisibleIndex="18" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" HeaderStyle-CssClass="colDisable">
                                     <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <%--<PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>--%>
                                     <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DAYS" Caption="Days" Width="50px" VisibleIndex="19" HeaderStyle-CssClass="colDisable">
                                      <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                               
                            </Columns>
                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <SettingsBehavior ConfirmDelete="false" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" AllowSort="False" />
                            <Settings ShowGroupPanel="true" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="50">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="DOC_TYPE" SummaryType="Custom" Tag="Item_DocType"/>
                                <dxe:ASPxSummaryItem FieldName="BAL_AMOUNT" SummaryType="Custom" Tag="Item_BalAmt" >                                   
                                </dxe:ASPxSummaryItem>
                            </TotalSummary>

                        </dxe:ASPxGridView>

                      <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PARTYWISEANALYSISDET_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>
    <div>
    </div>

    <!--Customer Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" width="100%" placeholder="Search By Customer Name or Unique ID" />
                    <div id="CustomerTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Customer Name</th>
                                 <th>Unique ID</th>
                                <th>Alternate No.</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('CustomerSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('CustomerSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <!--Customer Modal -->

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <asp:HiddenField ID="hdnCustomerId" runat="server" />

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsCustAnalysisDetFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
       <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectionInReport" runat="server" />
</asp:Content>