<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CustomerOutstanding.aspx.cs" 
    Inherits="Reports.Reports.GridReports.CustomerOutstanding" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="CSS/SearchPopup.css" rel="stylesheet" />
     <script src="JS/SearchMultiPopup.js"></script>

    <script>
        function BranchCloseLookup() {
            clookupBranch.ConfirmCurrentSelection();
            clookupBranch.HideDropDown();
            clookupBranch.Focus();
        }

        function CustomerCloseLookup() {
            clookupCustomer.ConfirmCurrentSelection();
            clookupCustomer.HideDropDown();
            clookupCustomer.Focus();
        }

        function SalesmanCloseLookup() {
            clookupSalesman.ConfirmCurrentSelection();
            clookupSalesman.HideDropDown();
            clookupSalesman.Focus();
        }

        function btn_ShowRecordsClick(e) {
            $("#drdExport").val(0);
            $("#ddldetails").val(0);

            if (clookupBranch.GetValue() == null) {
                jAlert('Please select atleast one branch');
            }
            else {
                cShowGrid.PerformCallback();
            }

            var ToDate = (cxdeAsOnDate.GetValue() != null) ? cxdeAsOnDate.GetValue() : "";

            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "As On: " + ToDate;
            //cShowGrid.PerformCallback();
        }
        //Rev Subhra 18-12-2018  0017670
        function ShowGridEndCall() {
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cShowGrid.cpBranchNames;
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

        function BranchselectAll() {
            clookupBranch.gridView.SelectRows();
            document.getElementById("hflookupClassAllFlag1").value = "ALL";
        }

        function BranchunselectAll() {
            clookupBranch.gridView.UnselectRows();
        }

        function CustomerselectAll() {
            clookupCustomer.gridView.SelectRows();
            document.getElementById("hflookupClassAllFlag2").value = "ALL";
        }

        function CustomerunselectAll() {
            clookupCustomer.gridView.UnselectRows();
        }

        function SalesmanselectAll() {
            clookupSalesman.gridView.SelectRows();
            document.getElementById("hflookupClassAllFlag3").value = "ALL";
        }

        function SalesmanunselectAll() {
            clookupSalesman.gridView.UnselectRows();
        }

        function OpenAnalysisDetails(productID) {
            $("#drdExport").val(0);
            $("#ddldetails").val(0);
            $('#<%=hdnProductID.ClientID %>').val(productID);

            cCallbackPanel.PerformCallback(productID);
            cShowGridDetails.PerformCallback();
            cpopup.Show();
        }

        function popupHide(s, e) {
            cpopup.Hide();
        }

        function CallbackPanelEndCall(s, e) {
            if (cCallbackPanel.cpProductValue != null) {
                var ProductValue = cCallbackPanel.cpProductValue;
                cCallbackPanel.cpProductValue = null;

                var productCode = ProductValue.split('||@||')[0].replace("squot", "'").replace("squot", "'").replace("coma", ",").replace("slash", "/");
                var productName = ProductValue.split('||@||')[1].replace("squot", "'").replace("squot", "'").replace("coma", ",").replace("slash", "/");

                ctxtProductCode.SetValue(productCode);
                ctxtProductName.SetValue(productName);
            }
        }
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

            if (e.code == "Enter" || e.code == "NumpadEnter") {

                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique ID");
                HeaderCaption.push("Alternate No.");
                HeaderCaption.push("Address");


                if ($("#txtCustSearch").val() != "") {
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

    <style>
        .pdbot > tbody > tr > td {
            padding-bottom: 10px;
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
        .btnOkformultiselection {
            border-width: 1px;
            padding: 4px 10px;
            font-size: 13px !important;
            margin-right: 6px;
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
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <%--<div class="panel-title">
            <h3>Outstanding Report-Invoice Wise </h3>
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
    </div>
    <div class="form_main">
        <div class="row">
            <div class="col-md-2">
                <asp:HiddenField ID="hflookupClassAllFlag1" runat="server" Value="" />
                <asp:HiddenField ID="hflookupClassAllFlag2" runat="server" Value="" />
                <asp:HiddenField ID="hflookupClassAllFlag3" runat="server" Value="" />
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch/Unit : " CssClass="mylabel1"></asp:Label></label>
                <dxe:ASPxGridLookup ID="lookupBranch" ClientInstanceName="clookupBranch" SelectionMode="Multiple" runat="server"
                    OnDataBinding="lookupBranch_DataBinding" KeyFieldName="Branch_ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                        <dxe:GridViewDataColumn FieldName="Branch_description" Visible="true" VisibleIndex="1" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="Branch_ID" Visible="true" VisibleIndex="2" Caption="Branch ID" Settings-AutoFilterCondition="Contains" Width="0">
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
                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="BranchselectAll" UseSubmitBehavior="False" />
                                            </div>
                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="BranchunselectAll" UseSubmitBehavior="False" />
                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="BranchCloseLookup" UseSubmitBehavior="False" />
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
                <span id="MandatorClass" style="display: none" class="validclass" />
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="Customer : " CssClass="mylabel1"></asp:Label></label>
                <%--<dxe:ASPxGridLookup ID="lookupCustomer" ClientInstanceName="clookupCustomer" SelectionMode="Multiple" runat="server"
                    OnDataBinding="lookupCustomer_DataBinding" KeyFieldName="cnt_internalId" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                        <dxe:GridViewDataColumn FieldName="Customer_Name" Visible="true" VisibleIndex="1" Caption="Customer Name" Settings-AutoFilterCondition="Contains">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="cnt_internalId" Visible="true" VisibleIndex="1" Caption="Customer ID" Settings-AutoFilterCondition="Contains" Width="0">
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
                                                <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="CustomerselectAll" />
                                            </div>
                                            <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="CustomerunselectAll" />                                            
                                            <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CustomerCloseLookup" UseSubmitBehavior="False" />
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
                </dxe:ASPxGridLookup>--%>

                 <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
                <span id="MandatoryActivityType" style="display: none" class="validclass" />
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label6" runat="Server" Text="Salesman : " CssClass="mylabel1"></asp:Label></label>
                <dxe:ASPxGridLookup ID="lookupSalesman" ClientInstanceName="clookupSalesman" SelectionMode="Multiple" runat="server"
                    OnDataBinding="lookupSalesman_DataBinding" KeyFieldName="cnt_internalId" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                        <dxe:GridViewDataColumn FieldName="Salesman_Name" Visible="true" VisibleIndex="1" Caption="Salesman Name" Settings-AutoFilterCondition="Contains">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="cnt_internalId" Visible="true" VisibleIndex="1" Caption="Salesman ID" Settings-AutoFilterCondition="Contains" Width="0">
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
                                                <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="SalesmanselectAll" UseSubmitBehavior="False" />
                                            </div>
                                            <dxe:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="SalesmanunselectAll" UseSubmitBehavior="False" />                                           
                                            <dxe:ASPxButton ID="ASPxButton8" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="SalesmanCloseLookup" UseSubmitBehavior="False" />
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
                <span id="MandatoryActivityType" style="display: none" class="validclass" />
            </div>
            <%--<div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>--%>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblAsOnDate" runat="Server" Text="As on Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxAsOnDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeAsOnDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>
            
            <div class="col-md-2" style="padding-top:20px">
                <div id="ckpar" style="padding-left: 15px">
                    <asp:CheckBox runat="server" ID="chkZeroBal" style="color: #b5285f" Text="Show Zero Balance" />
                </div>
            </div>

            <div class="col-md-2">
                <label style="margin-bottom: 0">&nbsp</label>
                <div class="">
                    <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="drdExport_SelectedIndexChanged"
                        AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="clearfix">
            <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="cShowGrid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                OnCustomCallback="ShowGrid_CustomCallback" OnDataBinding="ShowGrid_DataBinding" OnSummaryDisplayText="ShowGrid_SummaryDisplayText">
                <%--Settings-VerticalScrollableHeight="180" Settings-VerticalScrollBarMode="Auto"--%>
                <%--<Columns>
                    <dxe:GridViewDataTextColumn FieldName="Salesman" Caption="Salesman" Width="12%" VisibleIndex="0">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Branch" Caption="Branch/Unit" Width="12%" VisibleIndex="1">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Customer" Caption="Customer" Width="12%" VisibleIndex="2">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Bill_No" Caption="Bill No." Width="12%" VisibleIndex="3">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Bill_Date" Caption="Bill Date" Width="12%" VisibleIndex="4">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Bill_Amount" Caption="Bill Amount" Width="8%" VisibleIndex="5">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Outstanding_Amount" Caption="Outstanding Amount" Width="8%" VisibleIndex="6">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Due_Date" Caption="Due Date" Width="12%" VisibleIndex="7">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Overdue" Caption="Overdue" Width="8%" VisibleIndex="8">
                    </dxe:GridViewDataTextColumn>
                </Columns>--%>
                
                <Columns>
                    <dxe:GridViewDataTextColumn FieldName="BRANCH_DESCRIPTION" Caption="Branch/Unit" Width="8%" VisibleIndex="0">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="CUSTOMER" Caption="Customer" Width="12%" VisibleIndex="1">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="SALESMAN" Caption="Salesman" Width="12%" VisibleIndex="2">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="INVOICE_NUMBER" Caption="Bill No." Width="12%" VisibleIndex="3">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="INVOICE_DATE" Caption="Bill Date" Width="8%" VisibleIndex="4">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="INVOICE_TOTALAMOUNT" Caption="Bill Amount" Width="8%" VisibleIndex="5">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="ADJ_DOC_N0" Caption="Adjusted Doc No" Width="12%" VisibleIndex="6">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="ADJ_DOC_DATE" Caption="Adjusted Doc Date" Width="10%" VisibleIndex="7">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="DOC_TYPE" Caption="Adjusted Doc Type" Width="12%" VisibleIndex="8">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="ADJ_AMOUNT" Caption="Adjusted Amount" Width="10%" VisibleIndex="9">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="BALANCE" Caption="Outstanding Amount" Width="12%" VisibleIndex="10">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="DUE_DATE" Caption="Due Date" Width="8%" VisibleIndex="11">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="OVERDUE" Caption="Overdue" Width="8%" VisibleIndex="12">
                        <PropertiesTextEdit DisplayFormatString="0"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                </Columns>


                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" AllowSort ="false"/>
                <Settings ShowFooter="true" ShowGroupPanel="false" ShowGroupFooter="VisibleIfExpanded" />
                 <%--Rev Subhra 18.12.2018  0017670 --%> 
                    <ClientSideEvents EndCallback="ShowGridEndCall" />
                <%--End of Rev--%>
                <SettingsEditing Mode="EditForm" />
                <SettingsContextMenu Enabled="true" />
                <SettingsBehavior AutoExpandAllGroups="true" />
                <Settings ShowGroupPanel="false" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsSearchPanel Visible="false" />
                <SettingsPager PageSize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                </SettingsPager>
                
                <%--<TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="Bill_Amount" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Outstanding_Amount" SummaryType="Sum" />
                </TotalSummary>--%>

                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="INVOICE_TOTALAMOUNT" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="ADJ_AMOUNT" SummaryType="Sum" />
                </TotalSummary>

            </dxe:ASPxGridView>
        </div>
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
    <asp:HiddenField ID="hdnCustomerId" runat="server" />
    <!--Customer Modal -->


    <asp:HiddenField ID="hdnProductID" runat="server" />
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <dxe:ASPxGridViewExporter ID="exporterDetails" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
</asp:Content>

