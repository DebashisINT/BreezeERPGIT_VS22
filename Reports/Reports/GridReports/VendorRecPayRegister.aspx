<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="VendorRecPayRegister.aspx.cs" 
    Inherits="Reports.Reports.GridReports.VendorRecPayRegister" %>

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
            right: 50%;
        }
        /*rev end Pallab*/
    </style>

     <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'VendorSource') {
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

   <%-- For multiselection--%>
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

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Vendor Name");

                 if ($("#txtVendSearch").val() != "") {
                     //callonServerM("Services/Master.asmx/GetVendor", OtherDetails, "VendorTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "VendorSource");
                     callonServerM("Services/Master.asmx/GetOnlyVendor", OtherDetails, "VendorTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "VendorSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }


         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtVendSearch').focus();
             else
                 $('#txtVendSearch').focus();
         }
   </script>
      <%-- For multiselection--%>

    <script type="text/javascript">
        function VendAll(obj) {
            if (obj == 'allvend') {
                if (chkallvendors.checked == true) {
                    ctxtVendName.SetText('');
                    GetObjectID('hdnVendorId').value = '';
                    document.getElementById("txtVendSearch").value = ""
                    ctxtVendName.SetEnabled(false);
                }
                else {
                    ctxtVendName.SetEnabled(true);
                }
            }
        }

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

        function ClearGridLookup() {
            var grid = gridquotationLookup.GetGridView();
            grid.UnselectRows();
        }

        //function GetChecked() {
        //    if ($("#chkallvendors").is(":checked") == true) {
        //        gridquotationLookup.SetEnabled(false);
        //        gridquotationLookup.SetValue(null);
        //    }
        //    else {
        //        gridquotationLookup.SetEnabled(true);
        //    }

        //}
        $(function () {
            cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);

            //cVendorComponentPanel.PerformCallback('BindComponentGrid' + '~' + 2);

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

        $(function () {
            cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
        });

        // REV SAYANTANI
        $(function () {
            cProjectPanel.PerformCallback('BindProjectGrid');
        });
        // END OF REV SAYANTANI  
        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })

            // REV SAYANTANI
            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            if (ProjectSelection == "0") {
                $('#divProj').addClass('hidden');
            }
            else {
                $('#divProj').removeClass('hidden');
            }
            // END OF REV SAYANTANI
            $("#ListBoxCustomerVendor").chosen().change(function () {
                var Ids = $(this).val();

             <%--   $('#<%=hdnSelectedCustomerVendor.ClientID %>').val(Ids);
                $('#MandatoryVendorType').attr('style', 'display:none');--%>

            })

            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                $("#hdnSelectedBranches").val('');
                cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })

        })

        function BindCustomerVendor(type) {
            cVendorComponentPanel.PerformCallback('BindComponentGrid' + '~' + 'DV');
        }


    </script>


    <script type="text/javascript">
        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            var ProjectSelectionInReport = document.getElementById('hdnProjectSelectionInReport').value;
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsVendRecPay").val("Y");
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
            data += '~' + $("#ddlbranchHO").val();
            if ($("#chkallvendors").is(":checked") == false) {
                if (ctxtVendName.GetValue() == null & chkallvendors.checked == false) {
                    jAlert('Please select atleast one Vendor.');
                }
                else {
                    //cCallbackPanel.PerformCallback(data);
                    if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                        jAlert('Please select atleast one branch for generate the report.');
                    }
                    else if (ProjectSelection == "1") {
                        if (ProjectSelectionInReport == "Yes" && gridprojectLookup.GetValue() == null) {
                            jAlert('Please select atleast one Project for generate the report.');
                        }
                        else {
                            cCallbackPanel.PerformCallback(data);
                        }
                    }
                    else {
                        cCallbackPanel.PerformCallback(data);
                    }
                }
            }
            else if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else if (ProjectSelection == "1") {
                if (ProjectSelectionInReport == "Yes" && gridprojectLookup.GetValue() == null) {
                    jAlert('Please select atleast one Project for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback(data);
                }
            }
            else {
                //cCallbackPanel.PerformCallback(data);
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback(data);
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

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }


        function OpenPOSDetails(Uniqueid, type) {
            var url = '';

            if (type == 'P' || type == 'R') {
               url = '/OMS/Management/Activities/VendorPaymentReceipt.aspx?key=' + Uniqueid + '&IsTagged=1&req=V&type=VPR';
            }
            popupdetails.SetContentUrl(url);
            popupdetails.Show();
        }
        function detailsAfterHide(s, e) {
            popupdetails.Hide();
        }

        function CloseGridVendorLookup() {
            gridquotationLookup.ConfirmCurrentSelection();
            gridquotationLookup.HideDropDown();
            gridquotationLookup.Focus();
        }

        function CloseGridLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function Callback2_EndCallback() {
            // alert('');
            $("#drdExport").val(0);
        }
        function CallbackPanelEndCall(s, e) {
             <%--Rev Subhra 20-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }
        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
        }
         
        // REV SAYANTANI
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
        // END OF REV SAYANTANI
        //function selectAll_vendor() {
        //    gridquotationLookup.gridView.SelectRows();
        //}
        //function unselectAll_vendor() {
        //    gridquotationLookup.gridView.UnselectRows();
        //}

    </script>
    <style>
        #ShowGrid, #ShowGrid .dxgvCSD {
            /*width: 100% !important;*/
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
            <h3>Vendor Payment/Receipt Register</h3>
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
    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row">
            <div class="col-md-2">
                
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                      <asp:Label ID="Label2" runat="Server" Text="Head Branch : " CssClass="mylabel1" Width="92px"></asp:Label>
                </div>
               
                 <div>
                      <div>
                        <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                    </div>
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
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedBranches" runat="server" />

                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchComponentPanel" OnCallback="Componentbranch_Callback">
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
                        </PanelCollection>

                    </dxe:ASPxCallbackPanel>

                </div>
            </div>
            <div class="col-md-2">
                
                     <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label6" runat="Server" Text="Transaction Type: " CssClass="mylabel1"  Width="110px"></asp:Label>
                     </div>
                

                 <div>
                      <asp:DropDownList ID="ddltranstype" runat="server" Width="100%">
                        <asp:ListItem Text="All" Value="A"></asp:ListItem>
                        <asp:ListItem Text="Receipt" Value="R"></asp:ListItem>
                      <asp:ListItem Text="Payment" Value="P"></asp:ListItem>
                </asp:DropDownList>
                 </div>
            </div>
            <div class="col-md-2">
                
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="Vendor : " CssClass="mylabel1"
                        Width="110px"></asp:Label>
                </div>
                
                <div>
                  <%--  <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cVendorComponentPanel" OnCallback="ComponentVendor_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_vendor" SelectionMode="Multiple" runat="server" TabIndex="7" ClientInstanceName="gridquotationLookup"
                                    OnDataBinding="lookup_vendor_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
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
                                                            <div class="hide">
                                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_vendor" />
                                                            </div>
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_vendor" />
                                                            <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridVendorLookup" UseSubmitBehavior="False" />
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





                    <asp:HiddenField ID="hdnSelectedCustomerVendor" runat="server" />


                    <span id="MandatoryVendorType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>--%>
                    <dxe:ASPxButtonEdit ID="txtVendName" runat="server" ReadOnly="true" ClientInstanceName="ctxtVendName" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){Vendor_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>

                </div>
            </div>
            <div class="col-md-2">
                <div style="padding-left: 15px;padding-top:20px;">
                    <asp:CheckBox ID="chkallvendors" Text="Select all Vendors" type="checkbox" runat="server" Checked="false" ClientInstanceName="cchkallvendors"></asp:CheckBox>
                </div>
            </div>
             <div class="clear"></div>
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
            </div>
           
           
            
            <%-- REV SAYANTANI--%>
              <div class="col-md-2" id="divProj">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lbl_Project" runat="Server" Text="Project : " CssClass="mylabel1"></asp:Label>
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
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Project" UseSubmitBehavior="False"/>
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Project" UseSubmitBehavior="False"/>                                                        
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
           <%-- END OF REV SAYANTANI--%>
            
            <div class="col-md-4" style=" padding-top: 13px">
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
        <table class="pull-left">
            <tr> 
                <td>
                </td>

               <%-- <td>
                    <asp:CheckBox ID="chkcash" runat="server" Checked="true"></asp:CheckBox>
                </td>--%>
               
            </tr>
            <tr>
                <td></td>
                
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
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SrlNo" 
                            DataSourceID="GenerateEntityServerModeDataSource"  OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback2_EndCallback"
                             Settings-HorizontalScrollBarMode="Visible">
                            <%--Settings-VerticalScrollableHeight="180" Settings-VerticalScrollBarMode="Auto"--%>

                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Serial" Width="50px" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn FieldName="BranchName" Caption="Unit" VisibleIndex="2" Width="100px"/>
                                <dxe:GridViewDataTextColumn FieldName="ReceiptPayment_TransactionType" Caption="Voucher Type" Width="90px" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="ReceiptPayment_VoucherNumber" Caption="Voucher No." Width="120px" VisibleIndex="4">
                                     <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                      <HeaderStyle HorizontalAlign="Center" />
                                      <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenPOSDetails('<%#Eval("ReceiptPayment_ID") %>','<%#Eval("ReceiptPayment_ModuleType") %>')" class="pad">
                                            <%#Eval("ReceiptPayment_VoucherNumber")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="ReceiptPayment_TransactionDate" Caption="Voucher Date" Width="100px" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />
                                <dxe:GridViewDataTextColumn FieldName="ReceiptDetail_DocumentTypeID" Caption="Doc. Type" VisibleIndex="6" Width="80px"/>
                                <dxe:GridViewDataTextColumn FieldName="CustomerVendor" Caption="Vendor(s)" VisibleIndex="7" Width="150px"/>
                                 <%--REV SAYANTANI--%>
                                 <dxe:GridViewDataTextColumn FieldName="PROJ_NAME" Caption="Project Name" VisibleIndex="8" Width="150px"/>
                                <%--END OF REV SAYANTANI--%>
                                <dxe:GridViewDataTextColumn FieldName="CashBank_Name" Caption="Cash/Bank" VisibleIndex="9" Width="100px"/>
                                <dxe:GridViewDataTextColumn FieldName="InstrumentType" Caption="Instrument Type" VisibleIndex="10" Width="100px"/>
                                <dxe:GridViewDataTextColumn FieldName="InstrumentNumber" Caption="Instrument No" VisibleIndex="11" Width="100px"/>
                                <dxe:GridViewDataTextColumn FieldName="CVRPTax_Amount" Caption="Taxable" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00" Width="80px">
                                      <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Total_CGST" Caption="CGST" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00" Width="80px">
                                      <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Total_SGST" Caption="SGST" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00" Width="80px" >
                                      <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Total_IGST" Caption="IGST" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00" Width="80px">
                                      <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Total_UTGST" Caption="UTGST" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="0.00" Width="80px">
                                      <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Voucher Amt." VisibleIndex="17" PropertiesTextEdit-DisplayFormatString="0.00" Width="100px">
                                      <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                            
                            
                            </Columns>
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="CVRPTax_Amount" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total_CGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total_SGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total_IGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Total_UTGST" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
                            </TotalSummary>
                        </dxe:ASPxGridView>
                         <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="CUSTVENDRECPAYREGISTER_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdetails" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="detailsAfterHide" />
    </dxe:ASPxPopupControl>

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

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Vendor Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('VendorSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('VendorSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <!--Vendor Modal -->
         <asp:HiddenField ID="hdnVendorId" runat="server" />

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsVendRecPay" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
   </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
  <%--  Rev Sayantani--%>
     <asp:HiddenField ID="hdnProjectSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectionInReport" runat="server" />
  <%--  End of Rev Sayantani--%>
</asp:Content>


