<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CRMSummary_Performance_F.aspx.cs" Inherits="Reports.Reports.GridReports.CRMSummary_Performance_F" %>

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
    </style>
    

    <style>
        .plhead a {
            font-size: 16px;
            padding-left: 10px;
            position: relative;
            width: 100%;
            display: block;
            padding: 9px 10px 5px 10px;
        }

            .plhead a > i {
                position: absolute;
                top: 11px;
                right: 15px;
            }

        #accordion {
            margin-bottom: 10px;
        }

        .companyName {
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 15px;
        }

        

        .plhead a.collapsed .fa-minus-circle {
            display: none;
        }
    </style>
    <script>
        function AllCustomer(obj) {
            if (obj == 'selectAllCustomer') {
                if (chkallCustomer.checked == true) {
                    ctxtCustName.SetText('');
                    GetObjectID('hdnCustomerId').value = '';
                    document.getElementById("txtCustSearch").value = ""
                    ctxtCustName.SetEnabled(false);
                }
                else {
                    ctxtCustName.SetEnabled(true);
                }
            }
        }
        function AllSalesman(obj) {
            if (obj == 'selectAllSalesman') {
                if (chkallSalesman.checked == true) {
                    ctxtSalesManAgent.SetText('');
                    GetObjectID('hdnSalesManAgentId').value = '';    
                    document.getElementById("txtSalesManSearch").value = ""
                    ctxtSalesManAgent.SetEnabled(false);
                }
                else {
                    ctxtSalesManAgent.SetEnabled(true);
                }
            }
        }
        function AllProduct(obj) {
            if (obj == 'selectAllProduct') {
                if (chkallProduct.checked == true) {
                    ctxtProdName.SetText('');
                    GetObjectID('hdncWiseProductId').value = '';
                    document.getElementById("txtProdSearch").value = ""
                    ctxtProdName.SetEnabled(false);
                }
                else {
                    ctxtProdName.SetEnabled(true);
                }
            }
        }

        $(document).ready(function () {
            $('#CustModel').on('shown.bs.modal', function () {
                $('#txtCustSearch').focus();
            })
            $('#ProdModel').on('shown.bs.modal', function () {
                $('#txtProdSearch').focus();
            })
        });
        var ProdArr = new Array();
        $(document).ready(function () {
            var ProdObj = new Object();
            ProdObj.Name = "ProductSource";
            ProdObj.ArraySource = ProdArr;
            arrMultiPopup.push(ProdObj);
        })
        function ProductButnClick(s, e) {
            $('#ProdModel').modal('show');
        }

        function Product_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#ProdModel').modal('show');
            }
        }

        function Productkeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtProdSearch").val()) == "" || $.trim($("#txtProdSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtProdSearch").val();
            OtherDetails.ClassID = hdnClassId.value;

            if (e.code == "Enter" || e.code == "NumpadEnter") {

                var HeaderCaption = [];
                HeaderCaption.push("Code");
                HeaderCaption.push("Name");
                HeaderCaption.push("Hsn");


                if ($("#txtProdSearch").val() != "") {
                    callonServerM("Services/Master.asmx/GetClassWiseProduct", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[dPropertyIndex=0]"))
                    $("input[dPropertyIndex=0]").focus();
            }
        }


        var CustArr = new Array();
        $(document).ready(function () {
            var CustObj = new Object();
            CustObj.Name = "CustomerSource";
            CustObj.ArraySource = CustArr;
            arrMultiPopup.push(CustObj);
        })

        var SalesArr = new Array();
        $(document).ready(function () {
            var CustObj = new Object();
            CustObj.Name = "SalesManSource";
            CustObj.ArraySource = SalesArr;
            arrMultiPopup.push(CustObj);
        })

        function SalesManButnClick(s, e) {
            $('#SalesManModel').modal('show');
            $("#txtSalesManSearch").focus();
        }

        function SalesManbtnKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#SalesManModel').modal('show');
                $("#txtSalesManSearch").focus();
            }
        }
        function SalesMankeydown(e) {

            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtSalesManSearch").val();
            if ($.trim($("#txtSalesManSearch").val()) == "" || $.trim($("#txtSalesManSearch").val()) == null) {
                return false;
            }
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Name");
                if ($("#txtSalesManSearch").val() != null && $("#txtSalesManSearch").val() != "") {

                    callonServerM("Services/Master.asmx/GetSalesManAgent", OtherDetails, "SalesManTable", HeaderCaption, "salesmanIndex", "SetSelectedValues", "SalesManSource");

                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[salesmanIndex=0]"))
                    $("input[salesmanIndex=0]").focus();
            }
        }

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
            else if (ArrName == 'SalesManSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#SalesManModel').modal('hide');
                    ctxtSalesManAgent.SetText(Name);
                    GetObjectID('hdnSalesManAgentId').value = key;
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
                    ctxtProdName.SetText('');
                    GetObjectID('hdncWiseProductId').value = '';
                }
            }

        }
        function SetfocusOnseach(indexName) {
            if (indexName == "dPropertyIndex") {
                $('#txtCustSearch').focus();
            }
            else
                $('#txtCustSearch').focus();
        }
    </script>

    <script type="text/javascript">
        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsCRMSummaryFilter").val("Y");
            if (ctxtSalesManAgent.GetValue() == null & chkallSalesman.checked == false) {
                jAlert('Please select Salesman/Agents for generate the report.');
            }
            else if (ctxtCustName.GetValue() == null & chkallCustomer.checked == false) {
                jAlert('Please select Customer for generate the report.');
            }
            else if (ctxtProdName.GetValue() == null & chkallProduct.checked == false) {
                jAlert('Please select Product for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback();
            }            
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

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
            GridList.Refresh();
        }

    </script>

    <script type="text/javascript">
        function CallbackListofCRM_BeginCallback() {
            $("#drdExport").val(0);
        }
    </script>
    <style>
        table.full {
            width:100%;
        }
        table.full>tbody>tr>td {
            padding:5px 10px;
            vertical-align:bottom;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">

        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
            <div class="panel panel-info">
                <div class="panel-heading" role="tab" id="headingOne">
                    <h4 class="panel-title plhead">
                        <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne" class="collapsed">
                            <asp:Label ID="RptHeading" runat="Server" Text="" Width="470px" Style="font-weight: bold;"></asp:Label>
                            <i class="fa fa-plus-circle"></i>
                            <i class="fa fa-minus-circle"></i>
                        </a>
                    </h4>
                </div>
                <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                    <div class="panel-body">
                        <div class="companyName">
                            <asp:Label ID="CompName" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompBranch" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompAdd" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompOth" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompPh" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="CompAccPrd" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                        <div>
                            <asp:Label ID="DateRange" runat="Server" Text="" Width="470px"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="form_main">
        <table class="full">
            <tr>
                <td>
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="As On Date : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                    <div>
                        <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                            <buttonstyle width="13px">
                                </buttonstyle>

                        </dxe:ASPxDateEdit>
                    </div>
                </td>
                <td>
                    <dxe:ASPxLabel ID="ASPxLabel3" runat="server" style="color: #b5285f; font-weight: bold;" Text="Salesman/Agents">
                    </dxe:ASPxLabel>
                    <dxe:ASPxButtonEdit ID="txtSalesManAgent" runat="server" ReadOnly="true" ClientInstanceName="ctxtSalesManAgent" TabIndex="8" Width="100%">
                        <buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </buttons>
                        <clientsideevents buttonclick="function(s,e){SalesManButnClick();}" keydown="SalesManbtnKeyDown" />
                    </dxe:ASPxButtonEdit>
                </td>
                <td><asp:CheckBox runat="server" Style="color: #b5285f; font-weight: bold;" ID="chkallSalesman" Checked="false" Text="All Salesman" /></td>
                <td>
                    <dxe:ASPxLabel ID="lbl_Customer" style="color: #b5285f; font-weight: bold;" runat="server" Text="Customer:">
                    </dxe:ASPxLabel>
                    <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%" TabIndex="5">
                        <buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </buttons>
                        <clientsideevents buttonclick="function(s,e){CustomerButnClick();}" keydown="function(s,e){Customer_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </td>
                <td> <asp:CheckBox runat="server" Style="color: #b5285f; font-weight: bold;" ID="chkallCustomer" Checked="false" Text="All Customer" /></td>
                <td>
                    <dxe:ASPxLabel ID="lbl_Product" style="color: #b5285f; font-weight: bold;" runat="server" Text="Product :">
                    </dxe:ASPxLabel>
                    <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%" TabIndex="5">
                        <buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </buttons>
                        <clientsideevents buttonclick="function(s,e){ProductButnClick();}" keydown="function(s,e){Product_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </td>
                <td><asp:CheckBox runat="server" Style="color: #b5285f; font-weight: bold;" ID="chkallProduct" Checked="false" Text="All Product" /></td>
                
            </tr>
            <tr>
                <td>
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                            <asp:Label ID="Label4" runat="Server" Text="Status : " CssClass="mylabel1"
                                Width="92px" Font-Bold="True"></asp:Label>
                        </label>
                    </div>
                    <asp:DropDownList ID="ddlstatus" runat="server" Width="100%">
                       <%-- <asp:ListItem Text="All" Value="A"></asp:ListItem>
                        <asp:ListItem Text="DOCUMENT COLLECTION" Value="DC"></asp:ListItem>
                        <asp:ListItem Text="CLOSED SALES" Value="CS"></asp:ListItem>
                        <asp:ListItem Text="FUTURE SALES" Value="FS"></asp:ListItem>
                        <asp:ListItem Text="OPEN ACTIVITIES" Value="OA"></asp:ListItem>
                        <asp:ListItem Text="CLARIFICATION REQUIRED" Value="CR"></asp:ListItem>--%>

                        <asp:ListItem Text="FUTURE SALES" Value="FS"></asp:ListItem>
                        <asp:ListItem Text="DOCUMENT COLLECTION" Value="DC"></asp:ListItem>
                        <asp:ListItem Text="CLARIFICATION REQUIRED" Value="CR"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td colspan="6">
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                         OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">XLSX</asp:ListItem>
                        <asp:ListItem Value="2">CSV</asp:ListItem>
                        <asp:ListItem Value="3">PDF</asp:ListItem>
                        <asp:ListItem Value="4">RTF</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="GridList" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            KeyFieldName="SEQ" OnSummaryDisplayText="ShowGrid_SummaryDisplayText"
                            DataSourceID="GenerateEntityServerModeDataSource" ClientSideEvents-BeginCallback="CallbackListofCRM_BeginCallback" 
                            Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto"  Settings-HorizontalScrollBarMode="Auto">
                            <columns>                                
                               <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="SALESMAN" Width="110px" Caption="Salesman" FixedStyle="Left">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CUSTOMER" Caption="Customer" Width="140px" VisibleIndex="2" FixedStyle="Left">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PRODUCTS_NAME" Caption="Product" Width="130px" VisibleIndex="3" FixedStyle="Left">
                                </dxe:GridViewDataTextColumn>
                                                             
                                <dxe:GridViewDataTextColumn Caption="Annual Budget" FieldName="QTY_CURRENTFY" Width="110px" VisibleIndex="4">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Monthly Budget" FieldName="MONTHLY_BUDGET" Width="90px" VisibleIndex="5">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn Caption="Cumulative Sales Till date" FieldName="CUMULATIVE_SALES" Width="145px" VisibleIndex="6">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>
                              
                                 <dxe:GridViewDataTextColumn Caption="Cumulative Sales % Till Date" FieldName="CUMULATIVESALES_PERCENTAGE" Width="160px" VisibleIndex="7">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="Weightage cumulative % Achd" FieldName="WEIGHTAGECUMULATIVE" Width="170px" VisibleIndex="8">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="April" FieldName="APRIL_CS" Width="70px" VisibleIndex="9">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="% April Sales" FieldName="APRIL_PS" Width="80px" VisibleIndex="10">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="Weightage % Achd" FieldName="APRIL_WP" Width="110px" VisibleIndex="11">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="May" FieldName="MAY_CS" Width="70px" VisibleIndex="12">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                 <dxe:GridViewDataTextColumn Caption="% May Sales" FieldName="MAY_PS" Width="80px" VisibleIndex="13">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="Weightage % Achd" FieldName="MAY_WP" Width="110px" VisibleIndex="14">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="June" FieldName="JUNE_CS" Width="70px" VisibleIndex="15">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                 <dxe:GridViewDataTextColumn Caption="% June Sales" FieldName="JUNE_PS" Width="80px" VisibleIndex="16">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="Weightage % Achd" FieldName="JUNE_WP" Width="110px" VisibleIndex="17">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="July" FieldName="JULY_CS" Width="70px" VisibleIndex="18">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                 <dxe:GridViewDataTextColumn Caption="% July Sales" FieldName="JULY_PS" Width="80px" VisibleIndex="19">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="Weightage % Achd" FieldName="JULY_WP" Width="110px" VisibleIndex="20">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="August" FieldName="AUGUST_CS" Width="70px" VisibleIndex="21">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                 <dxe:GridViewDataTextColumn Caption="% August Sales" FieldName="AUGUST_PS" Width="95px" VisibleIndex="22">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="Weightage % Achd" FieldName="AUGUST_WP" Width="110px" VisibleIndex="23">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="September" FieldName="SEPTEMBER_CS" Width="90px" VisibleIndex="24">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                 <dxe:GridViewDataTextColumn Caption="% September Sales" FieldName="SEPTEMBER_PS" Width="120px" VisibleIndex="25">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="Weightage % Achd" FieldName="SEPTEMBER_WP" Width="110px" VisibleIndex="26">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="October" FieldName="OCTOBER_CS" Width="90px" VisibleIndex="27">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                 <dxe:GridViewDataTextColumn Caption="% October Sales" FieldName="OCTOBER_PS" Width="100px" VisibleIndex="28">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="Weightage % Achd" FieldName="OCTOBER_WP" Width="110px" VisibleIndex="29">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                 <dxe:GridViewDataTextColumn Caption="November" FieldName="NOVEMBER_CS" Width="90px" VisibleIndex="30">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                 <dxe:GridViewDataTextColumn Caption="% November Sales" FieldName="NOVEMBER_PS" Width="110px" VisibleIndex="31">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="Weightage % Achd" FieldName="NOVEMBER_WP" Width="110px" VisibleIndex="32">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="December" FieldName="DECEMBER_CS" Width="90px" VisibleIndex="33">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                 <dxe:GridViewDataTextColumn Caption="% December Sales" FieldName="DECEMBER_PS" Width="110px" VisibleIndex="34">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="Weightage % Achd" FieldName="DECEMBER_WP" Width="110px" VisibleIndex="35">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="January" FieldName="JANUARY_CS" Width="90px" VisibleIndex="36">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                 <dxe:GridViewDataTextColumn Caption="% January Sales" FieldName="JANUARY_PS" Width="100px" VisibleIndex="37">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="Weightage % Achd" FieldName="JANUARY_WP" Width="110px" VisibleIndex="38">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                 <dxe:GridViewDataTextColumn Caption="February" FieldName="FEBRUARY_CS" Width="90px" VisibleIndex="39">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                 <dxe:GridViewDataTextColumn Caption="% February Sales" FieldName="FEBRUARY_PS" Width="110px" VisibleIndex="40">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="Weightage % Achd" FieldName="FEBRUARY_WP" Width="110px" VisibleIndex="41">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                 <dxe:GridViewDataTextColumn Caption="March" FieldName="MARCH_CS" Width="80px" VisibleIndex="42">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                 <dxe:GridViewDataTextColumn Caption="% March Sales" FieldName="MARCH_PS" Width="90px" VisibleIndex="43">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <dxe:GridViewDataTextColumn Caption="Weightage % Achd" FieldName="MARCH_WP" Width="110px" VisibleIndex="44">
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                 </dxe:GridViewDataTextColumn> 

                                <%--<dxe:GridViewDataTextColumn VisibleIndex="45" FieldName="ACTIVITY_STATUS" Width="100px" Caption="Status">
                                </dxe:GridViewDataTextColumn>--%>
                            </columns>
                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" />
                             <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" AllowSort="False" />
                            <Settings ShowGroupPanel="true" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />

                             <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="PRODUCTS_NAME" SummaryType="Custom"/>
                                <dxe:ASPxSummaryItem FieldName="QTY_CURRENTFY" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="MONTHLY_BUDGET" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="CUMULATIVE_SALES" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="CUMULATIVESALES_PERCENTAGE" SummaryType="Custom"/>
                                <dxe:ASPxSummaryItem FieldName="APRIL_CS" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="APRIL_PS" SummaryType="Custom"/>
                                <dxe:ASPxSummaryItem FieldName="MAY_CS" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="MAY_PS" SummaryType="Custom"/>
                                <dxe:ASPxSummaryItem FieldName="JUNE_CS" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="JUNE_PS" SummaryType="Custom"/>
                                <dxe:ASPxSummaryItem FieldName="JULY_CS" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="JULY_PS" SummaryType="Custom"/>
                                <dxe:ASPxSummaryItem FieldName="AUGUST_CS" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="AUGUST_PS" SummaryType="Custom"/>
                                <dxe:ASPxSummaryItem FieldName="SEPTEMBER_CS" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="SEPTEMBER_PS" SummaryType="Custom"/>
                                <dxe:ASPxSummaryItem FieldName="OCTOBER_CS" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="OCTOBER_PS" SummaryType="Custom"/>
                                <dxe:ASPxSummaryItem FieldName="NOVEMBER_CS" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="NOVEMBER_PS" SummaryType="Custom"/>
                                <dxe:ASPxSummaryItem FieldName="DECEMBER_CS" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="DECEMBER_PS" SummaryType="Custom"/>
                                <dxe:ASPxSummaryItem FieldName="JANUARY_CS" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="JANUARY_PS" SummaryType="Custom"/>
                                <dxe:ASPxSummaryItem FieldName="FEBRUARY_CS" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="FEBRUARY_PS" SummaryType="Custom"/>
                                <dxe:ASPxSummaryItem FieldName="MARCH_CS" SummaryType="Sum"/>
                                <dxe:ASPxSummaryItem FieldName="MARCH_PS" SummaryType="Custom"/>
                            </TotalSummary>
                            
                        </dxe:ASPxGridView>
                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="CRMSUMMARY_REPORT"></dx:LinqServerModeDataSource>

                    </div>
                </td>
            </tr>
        </table>
        

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

                            <tr class="HeaderStyle" style="font-size: small">
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
                    <button type="button" class="btnOkformultiselection btn-default" onclick="DeSelectAll('CustomerSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('CustomerSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <!--Customer Modal -->
    <asp:HiddenField ID="hdnCustomerId" runat="server" />

    <%--SalesMan/Agent--%>
    <div class="modal fade" id="SalesManModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">SalesMan/Agent Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="SalesMankeydown(event)" id="txtSalesManSearch" autofocus width="100%" placeholder="Search By SalesMan/Agent Name" />

                    <div id="SalesManTable">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">id</th>
                                <th>Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default" onclick="DeSelectAll('SalesManSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('SalesManSource')">OK</button>
                    <%--<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>

        </div>
    </div>
    <%--SalesMan/Agent--%>
    <asp:HiddenField ID="hdnSalesManAgentId" runat="server" />

    <!--Product Modal -->
    <div class="modal fade" id="ProdModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Productkeydown(event)" id="txtProdSearch" width="100%" placeholder="Search By Product Name" />
                    <div id="ProductTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
                                <th>Hsn</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default" onclick="DeSelectAll('ProductSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ProductSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdncWiseProductId" runat="server" />
    <asp:HiddenField ID="hdnClassId" runat="server" />
    <!--Product Modal -->

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsCRMSummaryFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
</asp:Content>
