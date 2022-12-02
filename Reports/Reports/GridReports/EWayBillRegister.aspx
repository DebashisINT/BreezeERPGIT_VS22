<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="EWayBillRegister.aspx.cs" Inherits="Reports.Reports.GridReports.EWayBillRegister"
    EnableEventValidation="false" EnableViewState="false" %>
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
            right: 55%;
        }
        /*rev end Pallab*/  
    </style>

    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
        function SetSelectedValues(Id, Name, ArrName) {
           if (ArrName == 'CustomerVendorSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#CustVendModel').modal('hide');
                    ctxtCustVendName.SetText(Name);
                    GetObjectID('hdnSelectedCustomerVendor').value = key;
                }
                else {
                    ctxtCustVendName.SetText('');
                    GetObjectID('hdnSelectedCustomerVendor').value = '';
                }
            }

        }

    </script>
    <%-- For multiselection when click on ok button--%>

 
      <%-- For Customer/Vendor multiselection--%>
    <script type="text/javascript">
        $(document).ready(function () {
            //GetObjectID('hdnSelectedDocType').value = 'SI';
            GetObjectID('hdnSelectedDocType').value = 'SALL';

            $('#CustVendModel').on('shown.bs.modal', function () {
                $('#txtCustVendSearch').focus();
            })

            
            var ddl = document.getElementById("ddlDocType");
            var opt = document.createElement("option")

            ddl.add(opt, ddl[0]);
            opt.text = "All";
            opt.value = "SALL";

            opt = document.createElement("option");
            ddl.add(opt, ddl[1]);
            opt.text = "Sales Invoice";
            opt.value = "SI";

            opt = document.createElement("option");
            ddl.add(opt, ddl[2]);
            opt.text = "Sales Invoice for Vendor";
            opt.value = "SIV";

            opt = document.createElement("option");
            ddl.add(opt, ddl[3]);
            opt.text = "Sales Invoice for Transporter";
            opt.value = "SIT";

            opt = document.createElement("option");
            ddl.add(opt, ddl[4]);
            opt.text = "Transit Sales Invoice";
            opt.value = "TSI";

            opt = document.createElement("option");
            ddl.add(opt, ddl[5]);
            opt.text = "Sales Challan";
            opt.value = "SC";


        })

        var CustVendArr = new Array();
        $(document).ready(function () {
            var CustVendObj = new Object();
            CustVendObj.Name = "CustomerVendorSource";
            CustVendObj.ArraySource = CustVendArr;
            arrMultiPopup.push(CustVendObj);
        })
        function CustomerVendorButnClick(s, e) {
            $('#CustVendModel').modal('show');
        }

        function CustomerVendor_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#CustVendModel').modal('show');
            }
        }

        function CustomerVendorkeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtCustVendSearch").val()) == "" || $.trim($("#txtCustVendSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtCustVendSearch").val();
            
            if (e.code == "Enter" || e.code == "NumpadEnter") {

                var HeaderCaption = [];
                HeaderCaption.push("Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Party Type");

             if (drp_partytype.value == 'C') {
                    OtherDetails.Type = "8";
                    callonServerM("Services/Master.asmx/GetCustomerSalesInvVendTransporter", OtherDetails, "CustomerVendorTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "CustomerVendorSource");
                }
                else if (drp_partytype.value == 'V') {
                    OtherDetails.Type = "9";
                    callonServerM("Services/Master.asmx/GetVendorPurinvCustTransporter", OtherDetails, "CustomerVendorTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "CustomerVendorSource");
                }

            }
            else if (e.code == "ArrowDown") {
                if ($("input[dPropertyIndex=0]"))
                    $("input[dPropertyIndex=0]").focus();
            }

        }

        function SetfocusOnseach(indexName) {
            if (indexName == "dPropertyIndex")
                $('#txtCustVendSearch').focus();
            else
                $('#txtCustVendSearch').focus();
        }
    </script>
    <%-- For Customer/Vendor multiselection--%>


    <script type="text/javascript">

        function ClearGridLookup() {
            var grid = gridquotationLookup.GetGridView();
            grid.UnselectRows();
        }
        function CallbackPanelEndCall(s, e) {
            <%--Rev Subhra 11-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }
        $(function () {

          
            cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());

           
            $("#drp_partytype").change(function () {
                var end = $("#drp_partytype").val();
                $("#ckpar").attr('style', 'display:inline-block; padding-left: 15px; padding-top: 3px;color: #b5285f; font-weight: bold;" class="clsTo;');

                if (end == 'C') {
                    //=================For clear data=======================
                    //GetObjectID('hdnSelectedDocType').value = 'SI';
                    GetObjectID('hdnSelectedDocType').value = 'SALL';

                    var OtherDetailsClear = {}
                    var HeaderCaptionClear = [];
                    HeaderCaptionClear.push("Name");
                    HeaderCaptionClear.push("Unique Id");
                    HeaderCaptionClear.push("Party Type");

                    document.getElementById("txtCustVendSearch").value = ""
                    OtherDetailsClear.SearchKey = 'xxxxxxxxxxaaaaaabbbbbbbbbxxxxxxxxxxxxxxxx';
                    OtherDetailsClear.Type = "8";
                    callonServerM("Services/Master.asmx/GetCustomerSalesInvVendTransporter", OtherDetailsClear, "CustomerVendorTable", HeaderCaptionClear, "dPropertyIndex", "SetSelectedValues", "CustomerVendorSource");

                    //======================================================
                    $("#Label3").text('Customer');
                    $("#lblAllCustVend").text('All Customer');
                    
                    $("#ckpar").attr('style', 'display:none; padding-left: 15px; padding-top: 3px;color: #b5285f; font-weight: bold;" class="clsTo;');
                   
                   $('#<%=hdfVendor.ClientID %>').val('2');

                    var ddl = document.getElementById("ddlDocType");
                    var opt = document.createElement("option")
                  
                    if (ddl.length > 0) {
                        var len = ddl.length;
                        for(i=0;i<len; i++){
                            ddl.remove(0);
                        }
                    }

                    ddl.add(opt, ddl[0]);
                    opt.text = "All";
                    opt.value = "SALL";

                    opt = document.createElement("option");
                    ddl.add(opt, ddl[1]);
                    opt.text = "Sales Invoice";
                    opt.value = "SI";

                    opt = document.createElement("option");
                    ddl.add(opt, ddl[2]);
                    opt.text = "Sales Invoice for Vendor";
                    opt.value = "SIV";

                    opt = document.createElement("option");
                    ddl.add(opt, ddl[3]);
                    opt.text = "Sales Invoice for Transporter";
                    opt.value = "SIT";

                    opt = document.createElement("option");
                    ddl.add(opt, ddl[4]);
                    opt.text = "Transit Sales Invoice";
                    opt.value = "TSI";

                    opt = document.createElement("option");
                    ddl.add(opt, ddl[5]);
                    opt.text = "Sales Challan";
                    opt.value = "SC";
               

                }
                else if (end == 'V') {
                    //GetObjectID('hdnSelectedDocType').value = 'PB';
                    GetObjectID('hdnSelectedDocType').value = 'PALL';
                    //=================For clear data=======================
                    var OtherDetailsClear = {}
                    var HeaderCaptionClear = [];
                    HeaderCaptionClear.push("Name");
                    HeaderCaptionClear.push("Unique Id");
                    HeaderCaptionClear.push("Party Type");

                    document.getElementById("txtCustVendSearch").value = ""
                    OtherDetailsClear.SearchKey = 'xxxxxxxxxxaaaaaabbbbbbbbbxxxxxxxxxxxxxxxx';
                    OtherDetailsClear.Type = "9";
                    callonServerM("Services/Master.asmx/GetCustomerSalesInvVendTransporter", OtherDetailsClear, "CustomerVendorTable", HeaderCaptionClear, "dPropertyIndex", "SetSelectedValues", "CustomerVendorSource");

                    //======================================================

                    $("#Label3").text('Vendor');
                    $("#lblAllCustVend").text('All Vendor');
                    $('#<%=hdfVendor.ClientID %>').val('1');
                    
                    var ddl = document.getElementById("ddlDocType");
                    var opt = document.createElement("option")

                    if (ddl.length > 0) {
                        var len = ddl.length;
                        for (i = 0; i < len; i++) {
                            ddl.remove(0);
                        }
                    }

                    ddl.add(opt, ddl[1]);
                    opt.text = "All";
                    opt.value = "PALL";

                    opt = document.createElement("option");
                    ddl.add(opt, ddl[1]);
                    opt.text = "Purchase Invoice";
                    opt.value = "PB";

                    opt = document.createElement("option");
                    ddl.add(opt, ddl[2]);
                    opt.text = "Purchase Invoice for Customer";
                    opt.value = "PBC";

                    opt = document.createElement("option");
                    ddl.add(opt, ddl[3]);
                    opt.text = "Purchase Invoice for Transporter";
                    opt.value = "PBT";

                    opt = document.createElement("option");
                    ddl.add(opt, ddl[4]);
                    opt.text = "Transit Purchase Invoice";
                    opt.value = "TPB";

                    opt = document.createElement("option");
                    ddl.add(opt, ddl[5]);
                    opt.text = "Purchase Challan";
                    opt.value = "PC";


                }
               
            });

            $("#ddlDocType").change(function () {
                GetObjectID('hdnSelectedDocType').value = $("#ddlDocType").val();
            })
            
            function OnWaitingGridKeyPress(e) {

                if (e.code == "Enter") {

                }

            }


        });




    $(document).ready(function () {
        $("#ListBoxBranches").chosen().change(function () {
            var Ids = $(this).val();
            // BindLedgerType(Ids);                    
            $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })
            
            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                $('#MandatoryActivityType').attr('style', 'display:none');
                $("#hdnSelectedBranches").val('');
                cBranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })

            $("#ListBoxGroup").chosen().change(function () {   //Suvankar
                var Ids = $(this).val();
             
            })


            $("#ListBoxCustomerVendor").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedCustomerVendor.ClientID %>').val(Ids);
                $('#MandatoryCustomerType').attr('style', 'display:none');

            })
        })

    </script>


    <script type="text/javascript">


        function cxdeToDate_OnChaged(s, e) {

            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();

        }
        function CustAll(obj) {
            if (obj == 'allcust') {
                if (chkallcust.checked == true) {
                    ctxtCustVendName.SetText('');
                    GetObjectID('hdnSelectedCustomerVendor').value = '';
                    document.getElementById("txtCustVendSearch").value = ""
                    ctxtCustVendName.SetEnabled(false);
                }
                else {
                    ctxtCustVendName.SetEnabled(true);
                }
            }
        }
        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            e.preventDefault;
            var data = "OnDateChanged";
            $("#drdExport").val(0);
            $("#hfIsEWayBillFilter").val("Y");

            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();

            //Rev Subhra v1.0.99  0019105   07-12-2018
            //if (ctxtCustVendName.GetValue() == null & chkallcust.checked == false) {
               // jAlert('Please select atleast one Party.');
           // }
           // else {

                //Grid.PerformCallback(data);
               // if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                  //  jAlert('Please select atleast one branch for generate the report.');
             //   }
               // else {
                   // cCallbackPanel.PerformCallback();
               // }

            //}

            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                //Rev Subhra 12-12-2018  19105 : Enhancement required(mailed by Bhaskar da)
                //jAlert('Please select atleast one branch for generate the report.');
                jAlert('Select a Branch to generate the report.');
                //End of Rev
            }
            else {

                //Grid.PerformCallback(data);
                if (ctxtCustVendName.GetValue() == null & chkallcust.checked == false) {
                    //Rev Subhra 12-12-2018  19105 : Enhancement required(mailed by Bhaskar da)
                    //jAlert('Please select atleast one Party.');
                    if(drp_partytype.value == 'C')
                    {
                        jAlert('Select a Customer to generate the report.');
                    }
                    else {
                        jAlert('Select a Vendor to generate the report.');
                    }
                    //End of Rev
                     
                }
                else {
                    cCallbackPanel.PerformCallback();
                }

            }

            //End of Rev
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
     
     
        function BudgetAfterHide(s, e) {
            popupbudget.Hide();
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


        function selectAll() {
            gridquotationLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridquotationLookup.gridView.UnselectRows();
        }
     
        function selectAll_branch() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll_branch() {
            gridbranchLookup.gridView.UnselectRows();
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
        .plast {
            padding: 15px 0 6px 0;
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
            <h3>Consolidated Party Ledger</h3>
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
    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row">
            <div class="col-md-2 col-lg-2">
                 <div style="color: #b5285f; font-weight: bold;">
                    <span style="color: #b5285f; font-weight: bold;">Head Branch:</span>
                </div>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
           <div class="col-md-2 col-lg-2">
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
                                                            <%--<div class="hide"></div>--%>
                                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_branch" UseSubmitBehavior="False" />
                                                            
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_branch" UseSubmitBehavior="False" />
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
                    <%--<dx:LinqServerModeDataSource ID="BranchEntityServerModeDataSource" runat="server" OnSelecting="BranchEntityServerModeDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="v_BranchList" />--%>
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="Party Type : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </div>
                <div>
                    <asp:DropDownList ID="drp_partytype" runat="server" Width="100%">
                        <asp:ListItem Text="Customer" Value="C"></asp:ListItem>
                        <asp:ListItem Text="Vendor" Value="V"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
             <div class="col-md-2">
               <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                   <span style="color: #b5285f; font-weight: bold;" class="clsTo">Document Type</span>
                </div>

                
                <div>
                    <asp:DropDownList ID="ddlDocType" runat="server" Width="100%" >
                       <%-- <asp:ListItem Value="SI" Text="Sales Invoice"></asp:ListItem>
                        <asp:ListItem Value="SIV" Text="Sales Invoice for Vendor"></asp:ListItem>
                        <asp:ListItem Value="SIT" Text="Sales Invoice for Transporter"></asp:ListItem>
                        <asp:ListItem Value="TSI" Text="Transit Sales Invoice"></asp:ListItem>
                        <asp:ListItem Value="SC" Text="Sales Challan"></asp:ListItem>--%>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <%--Rev 1.0  v1.0.98   0018714 Subhra comment start--%>
                <%--<span style="color: #b5285f; font-weight: bold;" class="clsTo">Eway-Bill</span>--%>
                <span style="color: #b5285f; font-weight: bold;" class="clsTo">E-Way Bill</span>
                <%--end--%>
                <div>
                    <asp:DropDownList ID="ddlEwaybill" runat="server" Width="100%">
                        <asp:ListItem Value="All" Text="All"></asp:ListItem>
                        <asp:ListItem Value="Pend" Text="Pending"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="Customer: " CssClass="mylabel1"
                        Width="110px"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxButtonEdit ID="txtCustVendName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustVendName" Width="100%" TabIndex="6">
                        <buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </buttons>
                        <clientsideevents buttonclick="function(s,e){CustomerVendorButnClick();}" keydown="function(s,e){CustomerVendor_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>

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
                        <buttonstyle width="13px">
                        </buttonstyle>
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
                        <buttonstyle width="13px">
                        </buttonstyle>

                    </dxe:ASPxDateEdit>
                </div>
            </div>
           
            <div class="col-md-4"     style="padding: 11px;">
                <%--<div class="plast">
                    <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                </div>--%>
                  <table class="paddingTbl marginTop10">
                    <tr>
                        <td> <asp:CheckBox runat="server" ID="chkallcust" Checked="false" Text="" />
                            <asp:Label ID="lblAllCustVend" runat="Server" Text="All Customer" CssClass="mylabel1"
                        Width="110px"></asp:Label>
                        </td>
                         
                        <td>
                            <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>

                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                                OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" >
                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLSX</asp:ListItem>
                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                <asp:ListItem Value="4">CSV</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="pull-right">
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div onkeypress="OnWaitingGridKeyPress(event)">
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SLNO"
                            ClientSideEvents-BeginCallback="Callback2_EndCallback"
                             Settings-HorizontalScrollBarMode="Visible" DataSourceID="GenerateEntityServerModeDataSource" >
                            <columns>
                                <dxe:GridViewDataTextColumn FieldName="BRANCH_DESCRIPTION" Caption="Unit" Width="25%" VisibleIndex="1" />
                                <dxe:GridViewDataTextColumn FieldName="DOC_NUMBER" Caption="Doc No." Width="15%" VisibleIndex="2" />
                                <dxe:GridViewDataTextColumn FieldName="DOCTYPE" Caption="Doc Type" Width="20%" VisibleIndex="3" />
                                <dxe:GridViewDataTextColumn FieldName="DOC_DATE" Caption="Date" Width="13%" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />
                                <dxe:GridViewDataTextColumn FieldName="CUSTVEND_NAME" Caption="Customer" Width="25%" VisibleIndex="5" />
                                <dxe:GridViewDataTextColumn FieldName="SALESMANNAME" Caption="Salesmen" Width="25%" VisibleIndex="6" />
                                <%--Rev 1.0  v1.0.98   0018714 Subhra comment start--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="EWAYBILLNUMBER" Caption="Eway-Bill No" Width="15%" VisibleIndex="7" />--%>
                                <%--<dxe:GridViewDataTextColumn FieldName="EWAYBILLVALUE" Caption="Eway-Bill Value" Width="15%" VisibleIndex="8" />--%>
                               <%-- <dxe:GridViewDataTextColumn FieldName="EWAYBILLDATE" Caption="Eway-Bill Date" Width="15%" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />--%>

                                <dxe:GridViewDataTextColumn FieldName="EWAYBILLNUMBER" Caption="E-Way Bill No" Width="15%" VisibleIndex="7" />
                                <dxe:GridViewDataTextColumn FieldName="EWAYBILLVALUE" Caption="E-Way Bill Value" Width="15%" VisibleIndex="8" />
                                <dxe:GridViewDataTextColumn FieldName="EWAYBILLDATE" Caption="E-Way Bill Date" Width="15%" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" />
                                 <%--end--%>
                            </columns>
                            
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" AllowSort ="false" />
                            <Settings ShowFooter="true" ShowGroupPanel="false" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" />
                            <Settings ShowGroupPanel="false" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </SettingsPager>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                            <totalsummary>
                               
                            </totalsummary>
                        </dxe:ASPxGridView>
                         <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="EWAY_BILL_REGISTER_REPORT" />
                    </div>
                </td>
            </tr>
        </table>
    </div>

     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsEWayBillFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

   <!--Customer/Vendor Modal -->
    <div class="modal fade" id="CustVendModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer/Vendor Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="CustomerVendorkeydown(event)" id="txtCustVendSearch" width="100%" placeholder="Search By Customer/Vendor Name" />
                    <div id="CustomerVendorTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">id</th>
                                <th>Name</th>
                                <th>Unique Id</th>
                                <th>Party Type</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default" onclick="DeSelectAll('CustomerVendorSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('CustomerVendorSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
   <!--Customer/Vendor Modal -->

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

    <asp:HiddenField ID="hdfVendor" runat="server" />
    <asp:HiddenField ID="hdnSelectedCustomerVendor" runat="server" />
     <asp:HiddenField ID="hdnSelectedDocType" runat="server" />
    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>
