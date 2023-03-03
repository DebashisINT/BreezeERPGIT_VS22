<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                03-03-2023        2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableViewState="false" AutoEventWireup="true" CodeBehind="frm_saleouttaxreg.aspx.cs" Inherits="Reports.Reports.GridReports.frm_saleouttaxreg"  %>

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

        #ListBoxBranches {
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
            bottom: 80px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 55%;
        }
        /*rev end Pallab*/
    </style>

   <%-- For Single selection when click on ok button--%>
          <script type="text/javascript">

              function ValueSelected(e, indexName) {
                  if (e.code == "Enter" || e.code == "NumpadEnter") {
                      if (indexName == "CustomerIndex") {
                          var Id = e.target.parentElement.parentElement.cells[0].innerText;
                          var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                          if (Id) {
                              SetCustomer(Id, name);
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
                          $('#txtCustomerSearch').focus();
                      }
                  }
              }

          </Script>
    <%-- For Single selection when click on ok button--%>

      <%--For Customer Single Selection--%>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#CustModel').on('shown.bs.modal', function () {
                $('#txtCustomerSearch').focus();
            })
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

            if ($.trim($("#txtCustomerSearch").val()) == "" || $.trim($("#txtCustomerSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtCustomerSearch").val();
            OtherDetails.Type = "2";

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");

                if ($("#txtCustomerSearch").val() != "") {
                    callonServer("Services/Master.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "CustomerIndex", "SetCustomer");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[CustomerIndex=0]"))
                    $("input[CustomerIndex=0]").focus();
            }
        }

        function SetCustomer(Id, Name) {
            var key = Id;
            if (key != null && key != '') {
                $('#CustModel').modal('hide');
                ctxtCustName.SetText(Name);
                GetObjectID('hdnCustomerId').value = key;
                ctxtCustName.Focus();
            }
            else {
                ctxtCustName.SetText('');
                GetObjectID('hdnCustomerId').value = '';
            }
        }

    </Script>
    <%--For Customer Single Selection--%>
    <script type="text/javascript">
        $(function () {
            $('body').on('change', '#ddlgstn', function () {
                if ($("#ddlgstn").val()) {
                    cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlgstn").val());
                }
                else {
                    cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);
                }
            });
        });

        function fn_OpenDetails(keyValue) {
            Grid.PerformCallback('Edit~' + keyValue);
        }

        function CloseGridQuotationLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function selectAll() {
            cBranchGridLookup.gridView.SelectRows();
        }
        function unselectAll() {
            cBranchGridLookup.gridView.UnselectRows();
        }

        function selectAll_branch() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll_branch() {
            gridbranchLookup.gridView.UnselectRows();
        }

        function CloseGridDocTypeLookup() {
            griddoctypeLookup.ConfirmCurrentSelection();
            griddoctypeLookup.HideDropDown();
            griddoctypeLookup.Focus();
        }

        function selectAll_doctype() {
            griddoctypeLookup.gridView.SelectRows();
        }
        function unselectAll_doctype() {
            griddoctypeLookup.gridView.UnselectRows();
        }


        $(function () {
            cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);
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

            function OnWaitingGridKeyPress(e) {
                alert('1Hi');
                if (e.code == "Enter") {

                }

            }


        });

    </script>

    <script type="text/javascript">



        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }



        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            $("#drdExport").val(0);
            $("#hfIsSaleOutTaxFilter").val("Y");
            if (ddlgstn.value == '') {
                jAlert('Please select GSTIN');
            }
            else {
                if (griddoctypeLookup.GetValue() == null) {
                    jAlert('Please select atleast one Document Type');
                }
                else {
                    //var sdate = $('#FromDate').datepicker("getDate");
                    //var edate = $('#ToDate').datepicker("getDate");

                    if (cxdeToDate.GetDate() < cxdeFromDate.GetDate()) {
                        //if (edate < sdate) {
                        jAlert('From Date should not be grater than To Date');
                    }
                    else {
                        if ($("#ddlgstn").val()) {
                            //cCallbackPanel.PerformCallback('BindOPTREGGrid' + '~' + $("#ddlgstn").val());
                            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                                jAlert('Please select atleast one branch for generate the report.');
                            }
                            else {
                                cCallbackPanel.PerformCallback('BindOPTREGGrid' + '~' + $("#ddlgstn").val());
                            }
                        }
                        else {
                            //cCallbackPanel.PerformCallback('BindOPTREGGrid' + '~' + 0);
                            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                                jAlert('Please select atleast one branch for generate the report.');
                            }
                            else {
                                cCallbackPanel.PerformCallback('BindOPTREGGrid' + '~' + 0);
                            }
                        }

                    }
                }
            }
            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
        }
        function CallbackPanelEndCall(s, e) {
               <%--Rev Subhra 18-12-2018   0017670--%>
                document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
                //End of Subhra
            cOPTREGGrid.Refresh();
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


        function OpenBillDetails(branch) {

            cgridPendingApproval.PerformCallback('BndPopupgrid~' + branch);
            cpopupApproval.Show();
            return true;

        }

        function Callback_EndCallback() {
            $("#drdExport").val(0);
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
            right: 19px;
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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridCustOut , #RcmGrid
        {
            max-width: 97% !important;
            /*width: 99% !important;*/
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

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}


        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cOPTREGGrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cOPTREGGrid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cOPTREGGrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cOPTREGGrid.SetWidth(cntWidth);
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
        <div class="clear"></div>
         <div class="clearfix row">
                <%--Rev 1.0 : "simple-select" class add--%>
                 <div class="col-md-2 simple-select">
                     <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label6" runat="Server" Text="GSTIN : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                    <asp:DropDownList ID="ddlgstn" runat="server" Width="100%"></asp:DropDownList>
                </div>

                 <div class="col-md-2 branch-selection-box">
                        <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                            <asp:Label ID="Label2" runat="Server" Text="Branch : " CssClass="mylabel1"
                                Width="92px"></asp:Label>
                        </div>
             
                        <%--<asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>--%>
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                        <asp:HiddenField ID="HiddenField2" runat="server" />
                        <span id="MandatoryBranch" style="display: none" class="validclass">
                            <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EII" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                        <asp:HiddenField ID="HiddenField3" runat="server" />


                            <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchComponentPanel" OnCallback="ASPxCallbackPanel1_Callback">
                            <PanelCollection>
                                <dxe:PanelContent runat="server">
                                    <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                        OnDataBinding="lookup_branch_DataBinding"
                                        KeyFieldName="branch_id" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
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
                                                                <div class="hide">
                                                                    <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_branch" UseSubmitBehavior="False" />
                                                                </div>
                                                                <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_branch" UseSubmitBehavior="False" />
                                                                <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookupbranch" UseSubmitBehavior="False" />
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

                 <div class="col-md-2">
                      <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Document Type: " CssClass="mylabel1"></asp:Label></div>
              
                                    <dxe:ASPxGridLookup ID="lookup_doctype" SelectionMode="Multiple" runat="server" ClientInstanceName="griddoctypeLookup"
                                        OnDataBinding="lookup_doctype_DataBinding"
                                        KeyFieldName="doctype_code" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                        <Columns>
                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                            <dxe:GridViewDataColumn FieldName="doctype_code" Visible="false" VisibleIndex="1" Caption="Document Type code" Settings-AutoFilterCondition="Contains">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                            <dxe:GridViewDataColumn FieldName="doctype_description" Visible="true" VisibleIndex="2" Caption="Document Type Name" Settings-AutoFilterCondition="Contains">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                        </Columns>
                                        <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                            <Templates>
                                                <StatusBar>
                                                    <table class="OptionsTable" style="float: right">
                                                        <tr>
                                                            <td>
                                                               <%-- <div class="hide">--%>
                                                                    <dxe:ASPxButton ID="ASPxselect" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_doctype"  UseSubmitBehavior="False" />
                                                               <%-- </div>--%>
                                                                <dxe:ASPxButton ID="ASPxdselect" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_doctype" UseSubmitBehavior="False" />                                                            
                                                                <dxe:ASPxButton ID="Closedoc" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridDocTypeLookup" UseSubmitBehavior="False" />
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
                          
                </div>
          
                 <div class="col-md-2">
                    
                    <dxe:ASPxLabel ID="lbl_Customer" style="color: #b5285f; font-weight: bold;" runat="server" Text="Customer:">
                    </dxe:ASPxLabel>                    
                    <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%" TabIndex="5">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>

                 </div>
                  
                 <div class="col-md-2">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                    </div>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                     <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                 </div>
                 <div class="col-md-2">
                   <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                   </div>
                    <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                     <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                  </div>  

               <div class="clear"></div>
            <div class="col-md-4 pt-10 pb-10 pb-10">
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
            <div class="GridViewArea">
                         <dxe:ASPxGridView runat="server" ID="OPTREGGrid" ClientInstanceName="cOPTREGGrid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                 DataSourceID="GenerateEntityServerModeDataSource" OnCustomSummaryCalculate="OPTREGGrid_CustomSummaryCalculate" KeyFieldName="SEQ"
                OnSummaryDisplayText="OPTREGGrid_SummaryDisplayText" Settings-HorizontalScrollBarMode="Visible">

                 <%--    OnCustomCallback="OPTREGGrid_CustomCallback" OnDataBinding="OPTREGGrid_DataBinding"--%>
                                    <Columns>

                                     <dxe:GridViewDataTextColumn FieldName="DOCUMENT_TYPE" Caption="Document Type" Width="200px" VisibleIndex="0" GroupIndex="0">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="BRANCH_NAME" Caption="Location" Width="200px" VisibleIndex="1" >
                                     </dxe:GridViewDataTextColumn>

                                      <dxe:GridViewDataTextColumn FieldName="DOCUMENT_NO" Caption="Document No" Width="200px" VisibleIndex="2">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="DOCUMENT_DATE" Caption="Document Date" Width="200px" VisibleIndex="3">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="HSNCODE" Caption="HSN/SAC code" Width="140px" VisibleIndex="4">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="CUSTOMER_NAME" Caption="Customer Name" Width="140px" VisibleIndex="5">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="GSTINID" Caption="Party GSTIN" Width="140px" VisibleIndex="6">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="CUST_TYPE" Caption="Customer Type" Width="140px" VisibleIndex="7">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="PLACE_OF_SUPPLY" Caption="Place of supply" Width="170px" VisibleIndex="8">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="TYPE_OF_MOVEMENT" Caption="Type of Movement(Inter/Intra/Import)" Width="250px" VisibleIndex="9">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="DOC_TYPE_DESCRIPTION" Caption="Doc Type Description" Width="200px" VisibleIndex="10">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="INVOICE_VALUE" Caption="Total Amount" Width="150px" VisibleIndex="11">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="TAXABLE_AMOUNT" Caption="Taxable Value" Width="150px" VisibleIndex="12">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="TAXABLEEXEMPT" Caption="Taxable/Exempt" Width="150px" VisibleIndex="13">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="CGST_RATE" Caption="Central Tax Rate" Width="140px" VisibleIndex="14">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="SGST_RATE" Caption="State Tax Rate" Width="140px" VisibleIndex="15">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="IGST_RATE" Caption="Integrated Tax Rate" Width="140px" VisibleIndex="16">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="CGST_AMOUNT" Caption="Central Tax Amount" Width="140px" VisibleIndex="17">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="SGST_AMOUNT" Caption="State Tax Amount" Width="140px" VisibleIndex="18">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="IGST_AMOUNT" Caption="Integrated Tax Amount" Width="140px" VisibleIndex="19">
                                     </dxe:GridViewDataTextColumn>
                                      
                                     <dxe:GridViewDataTextColumn FieldName="CGSTRATEREV" Caption="Central REV Tax Rate" Width="140px" VisibleIndex="20">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="SGSTRATEREV" Caption="State REV Tax Rate" Width="140px" VisibleIndex="21">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="IGSTRATEREV" Caption="Integrated REV Tax Rate" Width="140px" VisibleIndex="22">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="TOTAL_CGSTREV" Caption="Central REV Tax Amount" Width="140px" VisibleIndex="23">
                                     </dxe:GridViewDataTextColumn>

                                      <dxe:GridViewDataTextColumn FieldName="TOTAL_SGSTREV" Caption="State REV Tax Amount" Width="140px" VisibleIndex="24">
                                     </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn FieldName="TOTAL_IGSTREV" Caption="Integrated REV Tax Amount" Width="160px" VisibleIndex="25">
                                     </dxe:GridViewDataTextColumn>

                                      <dxe:GridViewDataTextColumn FieldName="ITC" Caption="ITC Eligible?" Width="100px" VisibleIndex="26">
                                      </dxe:GridViewDataTextColumn>

                                      <dxe:GridViewDataTextColumn FieldName="REVERSE_CHARGE" Caption="RCM" Width="100px" VisibleIndex="27">
                                      </dxe:GridViewDataTextColumn>

                                    </Columns>

                                     <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                                     <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                                     <SettingsEditing Mode="EditForm" />
                                     <SettingsContextMenu Enabled="true" />
                                     <SettingsBehavior AutoExpandAllGroups="true" />
                                     <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                     <SettingsSearchPanel Visible="false" />
                                    <SettingsLoadingPanel Text="Please Wait..." />
                                     <SettingsPager PageSize="10">
                                       <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                     </SettingsPager>

                                    <TotalSummary>
                                       <%--  <dxe:ASPxSummaryItem FieldName="Document No" SummaryType="Custom" />--%>
                                        <dxe:ASPxSummaryItem FieldName="INVOICE_VALUE" SummaryType="Sum" />
                                        <dxe:ASPxSummaryItem FieldName="TAXABLE_AMOUNT" SummaryType="Sum" />
                                        <dxe:ASPxSummaryItem FieldName="CGST_AMOUNT" SummaryType="Sum" />
                                        <dxe:ASPxSummaryItem FieldName="SGST_AMOUNT" SummaryType="Sum" />
                                        <dxe:ASPxSummaryItem FieldName="IGST_AMOUNT" SummaryType="Sum" />
                                        <dxe:ASPxSummaryItem FieldName="TOTAL_CGSTREV" SummaryType="Sum" />
                                        <dxe:ASPxSummaryItem FieldName="TOTAL_SGSTREV" SummaryType="Sum" />
                                        <dxe:ASPxSummaryItem FieldName="TOTAL_IGSTREV" SummaryType="Sum" />
                                    </TotalSummary>
                                    
                                </dxe:ASPxGridView>
                          <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                                ContextTypeName="ReportSourceDataContext" TableName="SALE_GST_OUTPUTTAXREG_REPORT"></dx:LinqServerModeDataSource>
                                <asp:HiddenField ID="hiddenedit" runat="server" />
                    </div>
                            <div style="display: none">
                     
                            </div>
  
    </div>
    </div>

    <div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" GridViewID="ShowGrid" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

   
 <!--Customer Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">
            <!-- Customer content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustomerSearch" autofocus width="100%" placeholder="Search By Customer Name" />
                    <div id="CustomerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                  <th>Customer Name</th>
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
    <asp:HiddenField ID="hdnCustomerId" runat="server" />
 <!--Customer Modal -->

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsSaleOutTaxFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
</dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>

