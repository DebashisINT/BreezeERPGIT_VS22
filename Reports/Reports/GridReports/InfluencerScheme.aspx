<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                02-03-2023        2.0.36           Pallab              25575 : Report pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="InfluencerScheme.aspx.cs" Inherits="Reports.Reports.GridReports.InfluencerScheme" %>

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
        /*rev Pallab*/
        .branch-selection-box .dxlpLoadingPanel_PlasticBlue tbody, .branch-selection-box .dxlpLoadingPanelWithContent_PlasticBlue tbody, #divProj .dxlpLoadingPanel_PlasticBlue tbody, #divProj .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            bottom: 0 !important;
        }
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 70px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/
    </style>

     <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'InfluencerSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#InfluencerModel').modal('hide');
                    ctxtInfluencer.SetText(Name);
                    GetObjectID('hdnInfluencerId').value = key;
                }
                else {
                    ctxtInfluencer.SetText('');
                    GetObjectID('hdnInfluencerId').value = '';
                }
            }
        }
    </script>
    <%-- For multiselection when click on ok button--%>

    <%-- For multiselection--%>
     <script type="text/javascript">
         $(document).ready(function () {
             $('#InfluencerModel').on('shown.bs.modal', function () {
                 $('#txtInfluencerSearch').focus();
             })
         });

         var InfluencerArr = new Array();
         $(document).ready(function () {
             var InfluencerObj = new Object();
             InfluencerObj.Name = "InfluencerSource";
             InfluencerObj.ArraySource = InfluencerArr;
             arrMultiPopup.push(InfluencerObj);
         })
         function InfluencerButnClick(s, e) {
             $('#InfluencerModel').modal('show');
         }

         function Influencer_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#InfluencerModel').modal('show');
             }
         }

         function Influencerkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtInfluencerSearch").val()) == "" || $.trim($("#txtInfluencerSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtInfluencerSearch").val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Influencer Name");
                 HeaderCaption.push("Unique ID");
                 HeaderCaption.push("Alternate No.");
                 HeaderCaption.push("Address");

                 if ($("#txtInfluencerSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetInfluencer", OtherDetails, "InfluencerTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "InfluencerSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }


         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtInfluencerSearch').focus();
             else
                 $('#txtInfluencerSearch').focus();
         }
   </script>
     <%-- For multiselection--%>   

    <script type="text/javascript">
        $(function () {
            cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);
        });

        function CallbackPanelEndCall(s, e) {
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
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

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })
        })

    </script>

    <script type="text/javascript">
        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            e.preventDefault;
            var data = "OnDateChanged";
            $("#drdExport").val(0);
            $("#hfIsInfluencerSchmFilter").val("Y");
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback();
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

        function CloseGridLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
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

    </script>
    <style>
        #ShowGrid{
            width: 100% !important;
        }
    </style>

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

        /*.TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #ShowGridCustOut , .TableMain100 #ShowGridVendAnalysis
        {
            max-width: 98% !important;
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


        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
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
                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentBranchPanel" ClientInstanceName="cProductbranchComponentPanel" OnCallback="Componentbranch_Callback">
                    <panelcollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                    OnDataBinding="lookup_branch_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" width="200px" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>

                                        <dxe:GridViewDataColumn FieldName="branch_description" SortOrder="Ascending" Visible="true" VisibleIndex="2" width="200px" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
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
                                                            <%--</div>--%>
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
                </div>
            </div>

            <div class="col-md-2">
                 <dxe:ASPxLabel ID="lbl_Influencer" style="color: #b5285f; font-weight: bold;" runat="server" Text="Influencer :">
                </dxe:ASPxLabel>                    
                <dxe:ASPxButtonEdit ID="txtInfluencer" runat="server" ReadOnly="true" ClientInstanceName="ctxtInfluencer" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){InfluencerButnClick();}" KeyDown="function(s,e){Influencer_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
                <span id="MandatorysInfluencer" style="display: none" class="validclass">
                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
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
          <%--  //debashis written here--%>
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold; padding-top: 20px" class="clsTo">
                    <dxe:ASPxCheckBox runat="server" ID="chkDet" Checked="false" Text="Show Detail" ClientInstanceName="CchkDet">
                    </dxe:ASPxCheckBox>
                </div>
            </div>

            <div class="col-md-2 hide">
                <div class="hide">
                    <div style="padding-left: 5px">
                        <div style="padding-left: 1px; vertical-align: middle; padding-top: 22px">
                            <asp:CheckBox ID="chkallInfluencers" runat="server" onChange="GetChecked()" />
                            Select all Influencers
                        </div>
                    </div>
                </div>
            </div>

            <%--<div class="clear"></div>--%>

            <div class="col-md-2" style="padding: 10px;">
                <div style="padding-top: 5px">
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
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback2_EndCallback"
                            DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Auto">
                            <columns>

                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESC" Caption="Unit" Width="250px" VisibleIndex="1" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn FieldName="DOC_DATE" Caption="Transaction Date" Width="120px" VisibleIndex="2" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="DOC_NO" Width="150px" Caption="Document No." HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="INFLUENCER" Caption="Influencer Name" Width="250px" VisibleIndex="4" HeaderStyle-CssClass="colDisable"/>
                                <dxe:GridViewDataTextColumn FieldName="PHONENO" Caption="Phone No" Width="120px" VisibleIndex="5" HeaderStyle-CssClass="colDisable"/>
                                <dxe:GridViewDataTextColumn FieldName="DOC_TYPE" Caption="Document Type" Width="120px" VisibleIndex="6" HeaderStyle-CssClass="colDisable"/>

                                <dxe:GridViewDataTextColumn FieldName="INVOICE_AMOUNT" Caption="Sale Value" Width="120px" VisibleIndex="7" HeaderStyle-CssClass="colDisable">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DET_AMOUNT_CR" Caption="Commission" Width="150px" VisibleIndex="8" HeaderStyle-CssClass="colDisable">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DOC_BAL" Caption="Scheme Balance" Width="120px" VisibleIndex="9" FooterCellStyle-HorizontalAlign="Right" HeaderStyle-CssClass="colDisable">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="NET_AMT" Caption="Net Balance" Width="120px" VisibleIndex="10" FooterCellStyle-HorizontalAlign="Right" HeaderStyle-CssClass="colDisable">
                                    <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00"></PropertiesTextEdit>
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                            </columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" AllowSort ="false" />
                            <settings showfooter="true" showgrouppanel="true" showgroupfooter="VisibleIfExpanded" />
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" AllowSort="False"/>
                            <settings showgrouppanel="false" showstatusbar="Visible" showfilterrow="true" />
                            <settingssearchpanel visible="false" />
                            <settingspager pagesize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </settingspager>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                            <totalsummary>
                                <dxe:ASPxSummaryItem FieldName="INVOICE_AMOUNT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="DET_AMOUNT_CR" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="NET_AMT" SummaryType="Custom" />
                            </totalsummary>
                        </dxe:ASPxGridView>

                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="INFLUENCERSCHEME_REPORT" />

                    </div>
                </td>
            </tr>
        </table>
    </div>
    </div>

     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsInfluencerSchmFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>


  <!--Influencer Modal -->
    <div class="modal fade" id="InfluencerModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Influencer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Influencerkeydown(event)" id="txtInfluencerSearch" width="100%" placeholder="Search By Influencer Name or Unique ID or Alternate No." />
                    <div id="InfluencerTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Influencer Name</th>
                                 <th>Unique ID</th>
                                <th>Alternate No.</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('InfluencerSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('InfluencerSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <!--Influencer Modal -->

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
     <asp:HiddenField ID="hdnInfluencerId" runat="server" />
     <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>