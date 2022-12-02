<%@ Page Language="C#" EnableViewState="false"  MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CustomerAgeingWithDaysInterval.aspx.cs" Inherits="Reports.Reports.GridReports.CustomerAgeingWithDaysInterval" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

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

        .btnOkformultiselection {
            border-width: 1px;
            padding: 4px 10px;
            font-size: 13px !important;
            margin-right: 6px;
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
        function ctxt_NoofDays_TextChanged() {
            GetObjectID('hdndyintrvl').value = 'Y'
        }
        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            e.preventDefault;
            var data = "OnDateChanged";
            if (ctxt_NoofDays.GetValue() == null || ctxt_DaysInterval.GetValue() == null) {
                jAlert('No of Days & Days Interval can not be Blank');
            }
            else {
                if (parseInt(ctxt_DaysInterval.GetValue()) > parseInt(ctxt_NoofDays.GetValue())) {
                    jAlert('Days Interval can not be greater than No of Days');
                }
                else {

                    if ((parseInt(ctxt_NoofDays.GetValue()) / parseInt(ctxt_DaysInterval.GetValue())) > 20) {
                        //jAlert('Days Interval should not be greater than Twenty');
                        jAlert('Interval Columns should be within Twenty');
                    }
                    else {
                        //if (ctxtCustName.GetValue() == null & chkallcust.checked == false) {
                        if (ctxtCustName.GetValue() == null & Cchkallcust.GetChecked() == false) {
                            jAlert('Please select Customer for generate the report.');
                        }
                        else {
                            //cShowGridCustAgeingWithDaysInterval.PerformCallback(data + '~' + $("#ddlbranchHO").val());
                            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                                jAlert('Please select atleast one branch for generate the report.');
                            }
                            else {
                                    GetObjectID('hdnNoCaption').value = '';
                                    GetObjectID('hdnNoFields').value = '';
                                    cShowGridCustAgeingWithDaysInterval.PerformCallback(data + '~' + $("#ddlbranchHO").val());
                            }
                        }
                    }
                }
                var ToDate = (cxdeAsOnDate.GetValue() != null) ? cxdeAsOnDate.GetValue() : "";

                ToDate = GetDateFormat(ToDate);
                document.getElementById('<%=DateRange.ClientID %>').innerHTML = "As On: " + ToDate;
            }
        }
        //Rev Subhra 24-12-2018  0017670
        function ShowGridCustAgeingWithDaysIntervalEndCall() {
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cShowGridCustAgeingWithDaysInterval.cpBranchNames;
        }
        //End of Rev 
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

        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
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

        function CheckConsAllCust(s, e) {
            if (s.GetCheckState() == 'Checked') {
                ctxtCustName.SetEnabled(false);
                ctxtCustName.SetText('');
                GetObjectID('hdnCustomerId').value = '';
            }
            else {
                ctxtCustName.SetEnabled(true);
            }
        }

        function cShowGridCustAgeingWithDaysInterval_EndCallback() {

            $("#drdExport").val(0);
        }

        function EndCallback() {

        }

        function CloseGridBranchLookup() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

    </script>
     <style>
         .pl-10{
                padding-left: 10px;
        }

        .plhead a {
            font-size:16px;
            padding-left:10px;
            position:relative;
            width:100%;
            display:block;
            padding:9px 10px 5px 10px;
            text-decoration:none;
        }
        .plhead a>i {
            position:absolute;
            top:11px;
            right:15px;
        }
        #accordion {
            margin-bottom:10px;
        }
        .companyName>span {
            font-size:18px;
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
            margin-top:10px;
        }
        table[errorframe="errorFrame"]>tbody>tr>td.dxeErrorCellSys
        {
            display:none;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cShowGridCustAgeingWithDaysInterval.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cShowGridCustAgeingWithDaysInterval.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cShowGridCustAgeingWithDaysInterval.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cShowGridCustAgeingWithDaysInterval.SetWidth(cntWidth);
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
                  <asp:Label ID="RptHeading" runat="Server" Text=""  Style="font-weight: bold;"></asp:Label>
                    <i class="fa fa-plus-circle" ></i>
                    <i class="fa fa-minus-circle"></i>
                </a>
              </h4>
            </div>
            <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
              <div class="panel-body">
                        
                    <div  class="companyName">
                        <asp:Label ID="CompName" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
                  <%--Rev Subhra 18-12-2018   0017670--%>
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

        <div>
            
        </div>
        
    </div>

    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row">
            <div class="col-md-2 col-lg-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">Head Branch:</label>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2 col-lg-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"></asp:Label></label>
                <asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="60px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                <asp:HiddenField ID="hdnActivityType" runat="server" />

                <dxe:ASPxCallbackPanel runat="server" ID="ComponentBranchPanel" ClientInstanceName="cBranchComponentPanel" OnCallback="Componentbranch_Callback">
                    <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                OnDataBinding="lookup_branch_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60px" Caption=" " />
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
                    </panelcollection>
                </dxe:ASPxCallbackPanel>

                <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
            </div>

            <div class="col-md-2 col-lg-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="As On Date : " CssClass="mylabel1"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxAsOnDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeAsOnDate">
                    <buttonstyle width="13px">
                    </buttonstyle>
                </dxe:ASPxDateEdit>
            </div>

           <div class="col-md-1">
                <label style="color: #b5285f; font-weight: bold;">
                    <dxe:ASPxLabel ID="lbl_NoofDays" runat="server" Text="No. of Days : " Width="120px">
                    </dxe:ASPxLabel>
                </label>
              
                 <dxe:ASPxTextBox ID="txt_NoofDays" runat="server" Width="100%" MaxLength="100" ClientInstanceName="ctxt_NoofDays" > 
                     <MaskSettings Mask="<0..999>" AllowMouseWheel="false" />
                 </dxe:ASPxTextBox>
            </div>
            
           <div class="col-md-1">
                <label style="color: #b5285f; font-weight: bold;">
                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Days Interval : " Width="120px">
                    </dxe:ASPxLabel>
                </label>
                 <dxe:ASPxTextBox ID="txt_DaysInterval" runat="server" Width="100%" MaxLength="100" ClientInstanceName="ctxt_DaysInterval"> 
                     <MaskSettings Mask="<0..999>" AllowMouseWheel="false" />
                 </dxe:ASPxTextBox>
            </div>

            <div class="col-md-2 col-lg-2 ">
                <span style="margin-bottom: 5px; display: inline-block;">
                    <dxe:ASPxLabel ID="lbl_Customer" style="color: #b5285f;" runat="server" Text="Customer :">
                    </dxe:ASPxLabel></span>
                <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%" TabIndex="5">
                    <buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </buttons>
                    <clientsideevents buttonclick="function(s,e){CustomerButnClick();}" keydown="function(s,e){Customer_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
                <span id="MandatorysCustomer" style="display: none" class="validclass">
                    <img id="1gridHistory_DXPEForm_efnew_DXEFL_DXEditor2_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
            </div>
            <div class="col-md-1" style="padding:0;padding-top: 25px;">
                <dxe:ASPxCheckBox ID="chkallcust" runat="server" Checked="false" Text="All Customers" ClientInstanceName="Cchkallcust">
                    <ClientSideEvents CheckedChanged="CheckConsAllCust" />
                </dxe:ASPxCheckBox> 
            </div>
            
            <div class="clear"></div>
            <div class="col-md-2 pl-10">
                <dxe:ASPxCheckBox ID="chkcb" runat="server" Checked="false" Text="Include Cash/Bank"></dxe:ASPxCheckBox>
            </div>
            <div class="col-md-2 pl-10">    
                <dxe:ASPxCheckBox ID="chkjv" runat="server" Checked="false" Text="Include Journal"></dxe:ASPxCheckBox>
            </div>
            <div class="col-md-2 pl-10">
                <dxe:ASPxCheckBox ID="chkdncn" runat="server" Checked="false" Text="Exclude Debit/Credit Note"></dxe:ASPxCheckBox>
            </div>
           <%-- <div class="col-md-12">
                <table class="paddingTbl marginTop10">
                    <tr>
                        <td> <asp:CheckBox runat="server" ID="chkallcust" Checked="false" Text="All Customers" /></td>
                        <td><asp:CheckBox runat="server" ID="chkcb" Checked="false" Text="Include Cash/Bank" /></td>
                        <td><asp:CheckBox runat="server" ID="chkjv" Checked="false" Text="Include Journal" /></td>
                        <td><asp:CheckBox runat="server" ID="chkdncn" Checked="false" Text="Exclude Debit/Credit Note" /></td>
                        <td>
                            <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                            <% if (rights.CanExport)
                                { %> 
                                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" >
                                        <asp:ListItem Value="0">Export to</asp:ListItem>
                                        <asp:ListItem Value="1">PDF</asp:ListItem>
                                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                                        <asp:ListItem Value="3">RTF</asp:ListItem>
                                        <asp:ListItem Value="4">CSV</asp:ListItem>
                                    </asp:DropDownList>
                            <% } %>
                        </td>
                    </tr>
                </table>
            </div>--%>

            <div class="col-md-2 pb-5">
                <%--<label style="margin-bottom: 0">&nbsp</label>--%>
                    <div class="">
                        <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                        <% if (rights.CanExport)
                            { %> 
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLSX</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                        <% } %>
                    </div>
            </div>
            
            </div>  
            
            <div class="clear"></div>


            <div class="col-md-2">
                <label style="margin-bottom: 0">&nbsp</label>
                <div class="">

                    <%-- <% } %>--%>
                </div>
            </div>
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2">

                    <div>
                       
                      <%--  <dx:aspxgridview ID="ShowGridCustAgeingWithDaysInterval" runat="server" ClientInstanceName="cShowGridCustAgeingWithDaysInterval" 
                            Width="100%" Settings-HorizontalScrollBarMode="Auto" KeyFieldName="SEQ"
                            SettingsBehavior-ColumnResizeMode="Control" ClientSideEvents-BeginCallback="cShowGridCustAgeingWithDaysInterval_EndCallback"
                            Settings-VerticalScrollableHeight="237" SettingsBehavior-AllowSelectByRowClick="true" 
                            Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue" OnCustomCallback="ShowGridCustAgeingWithDaysInterval_CustomCallback" OnDataBinding="ShowGridCustAgeingWithDaysInterval_DataBinding"
                            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true" >
                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <SettingsBehavior EnableCustomizationWindow="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true"/>
                            <SettingsContextMenu Enabled="true" />
                                <Columns>

                                </Columns>

                                <SettingsPager PageSize="10" NumericButtonCount="4">
                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="50,100" />
                                </SettingsPager>
                        </dx:aspxgridview>--%>

                         <dxe:ASPxGridView runat="server" ID="ShowGridCustAgeingWithDaysInterval" ClientInstanceName="cShowGridCustAgeingWithDaysInterval" Width="100%" EnableRowsCache="false" 
                             AutoGenerateColumns="False" KeyFieldName="SEQ"
                            Settings-HorizontalScrollBarMode="Visible" AllowSort="False"
                             ClientSideEvents-BeginCallback="cShowGridCustAgeingWithDaysInterval_EndCallback" OnCustomCallback="ShowGridCustAgeingWithDaysInterval_CustomCallback" 
                             OnDataBinding="ShowGridCustAgeingWithDaysInterval_DataBinding" OnHtmlDataCellPrepared="ShowGridCustAgeingWithDaysInterval_HtmlDataCellPrepared"
                             OnHtmlFooterCellPrepared="ShowGridCustAgeingWithDaysInterval_HtmlFooterCellPrepared" >
                            <columns>
                              
                            </columns>
                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" columnresizemode="Control" />
                            <settings showfooter="true" showgrouppanel="true" showgroupfooter="VisibleIfExpanded" />
                              <%--Rev Subhra 24.12.2018  0017670 --%> 
                                <ClientSideEvents EndCallback="ShowGridCustAgeingWithDaysIntervalEndCall" />
                              <%--End of Rev--%>
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="false" />
                            <settingsbehavior autoexpandallgroups="true" AllowSort="False"/>
                            <settings showgrouppanel="true" showstatusbar="Visible" showfilterrow="true" />
                            <settingssearchpanel visible="false" />
                            <settingspager pagesize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="50,100" />
                            </settingspager>

                        </dxe:ASPxGridView>

                    </div>


                </td>
            </tr>
        </table>
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
             
                </div>
            </div>
        </div>
    </div>
    <!--Customer Modal -->

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <asp:HiddenField ID="hdnCustomerId" runat="server" />
    <asp:HiddenField ID="hdndyintrvl" runat="server" />
    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
    <asp:HiddenField ID="hdnNoCaption" runat="server" />
    <asp:HiddenField ID="hdnNoFields" runat="server" />
    
</asp:Content>




