<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="MultiCompanyStockSummary.aspx.cs" Inherits="Reports.Reports.GridReports.MultiCompanyStockSummary" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchMultiPopup.js"></script>
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <style>
        .colDisable {
            cursor: default !important;
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

        table[errorframe="errorFrame"] > tbody > tr > td.dxeErrorCellSys {
            display: none;
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
            text-decoration: none;
        }

            .plhead a > i {
                position: absolute;
                top: 11px;
                right: 15px;
            }

        #accordion {
            margin-bottom: 10px;
        }

        .companyName > span {
            font-size: 18px;
            font-weight: bold;
            margin-bottom: 15px;
        }

       

        .plhead a.collapsed .fa-minus-circle {
            display: none;
        }

        .paddingTbl > tbody > tr > td {
            padding-right: 20px;
        }

        .marginTop10 {
            margin-top: 10px;
        }
    </style>
    <script>
        $(function () {
            cCompanyComponentPanel.PerformCallback('BindCompanyGrid');

        });
        $(document).ready(function () {
            cCompanyComponentPanel.PerformCallback('BindCompanyGrid');
        });
        $(document).ready(function () {
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
        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'ProductSource') {
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
            if (indexName == "dPropertyIndex")
                $('#txtProdSearch').focus();
            else
                $('#txtProdSearch').focus();
        }
        function selectAll() {
            gridCompanyLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridCompanyLookup.gridView.UnselectRows();
        }
        function CloseGridBranchLookup() {
            gridCompanyLookup.ConfirmCurrentSelection();
            gridCompanyLookup.HideDropDown();
            gridCompanyLookup.Focus();
        }
        function btn_ShowRecordsClick(e) {
            if (gridCompanyLookup.GetValue() == null) {
                jAlert('Please select atleast one company');
            }
            else {
                e.preventDefault;
                var data = "OnDateChanged";
                cShowGridCompanyStockSummary.PerformCallback(data);
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
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cShowGridCompanyStockSummary.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cShowGridCompanyStockSummary.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cShowGridCompanyStockSummary.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cShowGridCompanyStockSummary.SetWidth(cntWidth);
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
                            <asp:Label ID="RptHeading" runat="Server" Text="" Style="font-weight: bold;"></asp:Label>
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

        <div>
        </div>

    </div>

    <div class="form_main">
        <div class="row">
            <div class="col-md-2 ">
                <dxe:ASPxLabel ID="ASPxLabel1" style="color: #b5285f; font-weight: bold;" runat="server" Text="Company :">
                </dxe:ASPxLabel>
                            

                <dxe:ASPxCallbackPanel runat="server" ID="ComponentCompanyPanel" ClientInstanceName="cCompanyComponentPanel" OnCallback="ComponentCompany_Callback">
                    <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_company" SelectionMode="Multiple" runat="server" ClientInstanceName="gridCompanyLookup"
                                OnDataBinding="lookup_company_DataBinding"
                                KeyFieldName="Company_Code" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60px" Caption=" " />


                                    <dxe:GridViewDataColumn FieldName="Company_Name" Visible="true" VisibleIndex="1" width="200px" Caption="Company Name">                                        
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="DbName" Visible="true" VisibleIndex="2" width="0" Caption="Data Base Name" >                                       
                                    </dxe:GridViewDataColumn>
                                    <dxe:GridViewDataColumn FieldName="Company_Code" Visible="true" VisibleIndex="3" width="0" Caption="Company Name">                                        
                                    </dxe:GridViewDataColumn>
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>                                                      
                                                        <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" UseSubmitBehavior="False" />                                                     
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
                                
                <span id="MandatoryCompany" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
               
            </div>
            <div class="col-md-2">
                <dxe:ASPxLabel ID="lbl_Product" style="color: #b5285f; font-weight: bold;" runat="server" Text="Product :">
                </dxe:ASPxLabel>
                <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%" TabIndex="5">
                    <buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </buttons>
                    <clientsideevents buttonclick="function(s,e){ProductButnClick();}" keydown="function(s,e){Product_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
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
            <div class="col-md-2">
                <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);" style="margin-top: 14px;">Show</button>

                <% if (rights.CanExport)
                    { %> 
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" style="margin-top: 14px;"
                            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" >
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">XLSX</asp:ListItem>
                            <asp:ListItem Value="2">PDF</asp:ListItem>
                            <asp:ListItem Value="3">CSV</asp:ListItem>
                            <asp:ListItem Value="4">RTF</asp:ListItem>
                        </asp:DropDownList>
                <% } %>
            </div>
        </div>
        <br />
    </div>
    <table class="TableMain100">
        <tr>
            <td colspan="2">
                <div>

                    <dx:ASPxGridView ID="ShowGridCompanyStockSummary" runat="server" ClientInstanceName="cShowGridCompanyStockSummary"
                        Width="100%" Settings-HorizontalScrollBarMode="Auto" KeyFieldName="SEQ"
                        SettingsBehavior-ColumnResizeMode="Control"
                        Settings-VerticalScrollableHeight="400" SettingsBehavior-AllowSelectByRowClick="true"
                        Settings-VerticalScrollBarMode="Auto" Theme="PlasticBlue" OnCustomCallback="ShowGridCompanyStockSummary_CustomCallback" OnDataBinding="ShowGridCompanyStockSummary_DataBinding"
                        Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true" OnHtmlFooterCellPrepared="ShowGridCompanyStockSummary_HtmlFooterCellPrepared">
                        <Columns>
                        </Columns>                        
                        <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                        <SettingsBehavior EnableCustomizationWindow="false" />                       
                        <Settings ShowFooter="true" />
                        <SettingsContextMenu Enabled="true" />
                        <SettingsPager PageSize="100" NumericButtonCount="4">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                        </SettingsPager>
                    </dx:ASPxGridView>

                </div>


            </td>
        </tr>
    </table>
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
     <asp:HiddenField ID="hdnNoCaption" runat="server" />
    <asp:HiddenField ID="hdnNoFields" runat="server" />
    <asp:HiddenField ID="hdnNoBandedColumn" runat="server" />
</asp:Content>
