<%--====================================================== Revision History ============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                13-02-2023        2.0.36           Pallab              25575 : Report pages modification
====================================================== Revision History ================================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="DayBook.aspx.cs" Inherits="Reports.Reports.GridReports.DayBook" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            right: 50%;
        }
        /*rev end Pallab*/
    </style>


    <script type="text/javascript">

        $(function () {
            cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            function OnWaitingGridKeyPress(e) {
                if (e.code == "Enter") {
                }
            }

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
                cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        })


        function BindActivityType(noteTilte) {
            var lBox = $('select[id$=ListBoxBranches]');
            var listItems = [];
            var selectedNoteId = '';
            if (noteTilte) {

                selectedNoteId = noteTilte;
            }
            lBox.empty();


            $.ajax({
                type: "POST",
                url: 'DayBook.aspx/GetBranchesList',
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
                }
            });


        }

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
       
        function OnGetRowValuesCallback(values) {
            alert(values);
        }

        function Callback2_EndCallback() {
            $("#drdExport").val(0);
        }
        function CallbackPanelEndCall(s, e) {
            <%--Rev Subhra 20-12-2018   0017670--%>
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
        }

        function btn_ShowRecordsClick(e) {
            //e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsDayBook").val("Y");
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            //Rev work start 01.07.2022
            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            FromDate = FromDate.split('-')[1] + '-' + FromDate.split('-')[0] + '-' + FromDate.split('-')[2];
            ToDate = ToDate.split('-')[1] + '-' + ToDate.split('-')[0] + '-' + ToDate.split('-')[2];

            var dayse = Math.round((Date.parse(ToDate) - Date.parse(FromDate)) / (1000 * 60 * 60 * 24));
            //Rev work close 01.07.2022 mantise no:0025003
            var data = "OnDateChanged";
            if (gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.', "Alert", function () {
                });
                //rev work start mantise no:0025003
                return false;
                //Rev work close 01.07.2022 mantise no:0025003
            }
            //Rev work start 01.07.2022 mantise no:0025003
            //else {
            //    cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val());
            //}
            if (dayse >=30) {
                jAlert('Searching days must be within 30 days', "Alert", function () {
                });                
                return false;               
            }

            cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val());

           
           /* var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);*/
            //Rev work close 01.07.2022 mantise no:0025003
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

        function OnGetRowValues(types) {
            var data = "OnDateChanged";
            if (gridbranchLookup.GetValue() == null) {
                jAlert('Please select at least one Branch', "Alert", function () {
                });
            }
            else {
                cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val());
            }
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

    
        function BudgetAfterHide(s, e) {
            popupldgr.Hide();
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

        //function CloseGridQuotationLookupledger() {
        //    gridledgerLookup.ConfirmCurrentSelection();
        //    gridledgerLookup.HideDropDown();
        //    gridledgerLookup.Focus();
        //}

        function CloseGridLookupbranch() {
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

        //function selectAll() {
        //    gridledgerLookup.gridView.SelectRows();
        //}

        //function unselectAll() {
        //    gridledgerLookup.gridView.UnselectRows();
        //}


        //function selectAll_Party() {
        //    gridquotationLookup.gridView.SelectRows();
        //}

        //function unselect_AllParty() {
        //    gridquotationLookup.gridView.UnselectRows();
        //}

        //function selectAll_Employee() {
        //    gridemployeeLookup.gridView.SelectRows();
        //}

        //function unselect_Employee() {
        //    gridemployeeLookup.gridView.UnselectRows();
        //}

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

        /*table
        {
                max-width: 99%  !important;
        }*/
        /*Rev end 1.0*/
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
            <h3>Day Book </h3>
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
                    <%--Rev Subhra 20-12-2018   0017670--%>
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

                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchPanel" OnCallback="Componentbranch_Callback">
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
                                                                <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Branch" />
                                                            <%--</div>--%>
                                                            <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Branch" />                                                            
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
                                   <%-- <ClientSideEvents ValueChanged="BranchValuewiseledger" />--%>
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>

                    </dxe:ASPxCallbackPanel>

                </div>
            </div>
           
            <div class="col-md-2">
               
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                        <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                
                <div>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
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
                        <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>

                        </dxe:ASPxDateEdit>
                    </div>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>
            
        <%--   <div class="hide">--%>
                <div class="col-md-2">
                    <div id="ckpar" style="padding-top: 20px;">
                        <asp:CheckBox runat="server" ID="chkConsiderOpening" Checked="false" Text="Consider Opening" />
                    </div>
                </div>
         <%--  </div>--%>
            <div class="col-md-2" style="padding-top: 15px;">
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
              
            </tr>
            <tr>            
            </tr>
        </table>

        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyboardSupport="true" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerModeDataSource" OnHtmlDataCellPrepared="ShowGrid_HtmlDataCellPrepared" ClientSideEvents-BeginCallback="Callback2_EndCallback"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" 
                            SettingsBehavior-AllowFocusedRow="true" SettingsBehavior-AllowSelectSingleRowOnly="true" Settings-HorizontalScrollBarMode="Visible">
                            <Columns>
                               <%-- <dxe:GridViewDataTextColumn FieldName="CASHBANKNAME" Caption="Cash Bank" VisibleIndex="1" Width="200px" />
                                <dxe:GridViewDataTextColumn FieldName="GROUP_NAME" Caption="Group Name" VisibleIndex="2" Width="100px"/>
                                <dxe:GridViewDataTextColumn FieldName="SUBGRP_NAME" Caption="Sub Group Name" VisibleIndex="3" Width="120px"/>--%>
                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" VisibleIndex="0" Width="170px"/>
                                <dxe:GridViewDataTextColumn FieldName="MAINACCOUNT" Caption="Ledger Description" VisibleIndex="1" Width="180px"/>
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="DOCUMENT_NO" Caption="Voucher No" Width="120px">
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="TRAN_DATE1" Caption="Voucher Date" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" Width="100px"/>
                                <dxe:GridViewDataTextColumn FieldName="INSTRUMENT_NO" Caption="Cheque No" VisibleIndex="4" Width="100px" Settings-AllowAutoFilter="False"/>
                                <dxe:GridViewDataTextColumn FieldName="INSTRUMENT_DATE" Caption="Cheque Date"  VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" Width="100px" Settings-AllowAutoFilter="False"/>
                                <dxe:GridViewDataTextColumn FieldName="DRAWEE_BANK" Caption="Cheque On Bank" VisibleIndex="6" Width="100px" Settings-AllowAutoFilter="False"/>
                                <dxe:GridViewDataTextColumn FieldName="TRAN_TYPE" Caption="Doc. Type" VisibleIndex="7" Width="110px"/>
                                <dxe:GridViewDataTextColumn FieldName="PARTICULARS" Caption="Particulars" VisibleIndex="8" Width="200px" Settings-AllowAutoFilter="False"/>
                                <dxe:GridViewDataTextColumn FieldName="HEADER_NARRATION" Caption="Header Narration" VisibleIndex="9" Width="200px" Settings-AllowAutoFilter="False"/>
                                <dxe:GridViewDataTextColumn FieldName="DEBIT" Caption="Debit" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00" Width="100px" Settings-AllowAutoFilter="False">
                                      <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CREDIT" Caption="Credit" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00" Width="100px" Settings-AllowAutoFilter="False">
                                      <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CLOSING_BALANCE" Caption="Balance"  VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00" Width="100px" Settings-AllowAutoFilter="False">
                                      <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CLOSE_TYPE" Caption="DbCr." VisibleIndex="13" Width="80px" Settings-AllowAutoFilter="False"/>
                                
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
                                <dxe:ASPxSummaryItem FieldName="MAINACCOUNT" SummaryType="Custom" />
                                <dxe:ASPxSummaryItem FieldName="DEBIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CREDIT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CLOSING_BALANCE" SummaryType="Custom" />
                                <dxe:ASPxSummaryItem FieldName="CLOSE_TYPE" SummaryType="Custom" />
                            </TotalSummary>
                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="DAYBOOKRUNNINGBAL_REPORT"></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>
    
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupldgr" Height="500px"
        Width="1310px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="BudgetAfterHide" />
    </dxe:ASPxPopupControl>
  <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsDayBook" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
 </dxe:ASPxCallbackPanel>

</asp:Content>