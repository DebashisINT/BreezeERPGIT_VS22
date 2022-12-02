<%@ Page Title="GSTR-1 All Report" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" AutoEventWireup="true"
    CodeBehind="Gstr-1Report.aspx.cs" Inherits="Reports.Reports.GridReports.Gstr_1Report" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" >

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

        /*#ShowGrid, #ShowGrid .dxgvCSD {
            width: 100% !important;
        }*/
    </style>

    <script type="text/javascript">

        function fn_OpenDetails(keyValue) {
            //cPopup_Empcitys.SetHeaderText('Modify Products');
            Grid.PerformCallback('Edit~' + keyValue);

        }

        function Tabchange() {
            $("#drdExport").val(0);

        }

        $(function () {

            ///   BindBranches(null);

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

        })



    </script>

    <script type="text/javascript">


        $("#drdExport").val(0);
        function btn_ShowRecordsClick(e) {
            e.preventDefault;

            var v = $("#ddlgstn").val();



            var activeTab = page.GetActiveTab();
            if (activeTab.name == 'B2B') {

                Gridb2b.PerformCallback('ListData~' + v);

            }
            else if (activeTab.name == 'B2CL') {

                b2clGrid.PerformCallback('ListData~' + v);

            }

            else if (activeTab.name == 'B2CS') {

                b2csGrid.PerformCallback('ListData~' + v);

            }

            else if (activeTab.name == 'CDNR9B') {

                cdnrGrid.PerformCallback('ListData~' + v);

            }


            else if (activeTab.name == 'CDNUR9B') {

                cdnurGrid.PerformCallback('ListData~' + v);

            }


            else if (activeTab.name == 'EXP') {

                expGrid.PerformCallback('ListData~' + v);

            }

            else if (activeTab.name == 'AT') {

                atGrid.PerformCallback('ListData~' + v);
            }

            else if (activeTab.name == 'ADJ') {

                adjGrid.PerformCallback('ListData~' + v);
            }

            else if (activeTab.name == 'EXEMP') {

                exempGrid.PerformCallback('ListData~' + v);
            }


            else if (activeTab.name == 'HSN') {

                hsnGrid.PerformCallback('ListData~' + v);
            }


            else if (activeTab.name == 'HSNDET') {

                hsndetailsGrid.PerformCallback('ListData~' + v);
            }


            else if (activeTab.name == 'GSTINDocumentCount') {

                GSTNInDocumentCount.PerformCallback('ListData~' + v);
            }

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
        }


        function btn_ShowRecordsClickAll(e) {
            e.preventDefault;

            var v = $("#ddlgstn").val();
                Gridb2b.PerformCallback('ListData~' + v);
                b2clGrid.PerformCallback('ListData~' + v);
                b2csGrid.PerformCallback('ListData~' + v);
                cdnrGrid.PerformCallback('ListData~' + v);
                cdnurGrid.PerformCallback('ListData~' + v);
                expGrid.PerformCallback('ListData~' + v);
                atGrid.PerformCallback('ListData~' + v);
                adjGrid.PerformCallback('ListData~' + v);
                exempGrid.PerformCallback('ListData~' + v);
                hsnGrid.PerformCallback('ListData~' + v);
                hsndetailsGrid.PerformCallback('ListData~' + v);
                GSTNInDocumentCount.PerformCallback('ListData~' + v);
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


        function OpenBillDetails(branch) {


            cgridPendingApproval.PerformCallback('BndPopupgrid~' + branch);
            cpopupApproval.Show();
            return true;
        }

        function popupHide(s, e) {

            cpopupApproval.Hide();
        }


        function Callback_BeginCallback() {


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
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                Gridb2b.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                Gridb2b.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    Gridb2b.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    Gridb2b.SetWidth(cntWidth);
                }

            });
        });
    </script>
    <link type="text/css" href="../Content/ComponentList/Styles.css" rel="Stylesheet" />
    <style type="text/css">
        
        .dxtcFixed td.dxdvItem {
            width: auto !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
       <%-- <div class="panel-title">
            <h3>GSTR-1 All</h3>
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
            <div class="hide">
                <asp:HiddenField ID="hdnActivityType" runat="server" />
                <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="GSTIN : " CssClass="mylabel1"
                        Width="100%"></asp:Label>
                </div>
                <div>
                    <asp:DropDownList ID="ddlgstn" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="Label1" runat="Server" Text="From Date : " CssClass="mylabel1"
                        Width="100%"></asp:Label>
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
                        Width="100%"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </div>
            </div>
            <div class="col-md-6" style="padding-top: 13px;margin-bottom: 6px;">
                 <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Generate</button>
                 <button id="btnShowAlltab" runat="server" class="btn btn-warning" type="button"  onclick="btn_ShowRecordsClickAll(this);">Generate All Tab(s)</button>
                 <asp:Button id="btnShowAll" runat="server" class="btn btn-info" type="button"  Text="Export all to Excel" OnClick="btnShow_Click"></asp:Button>

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
        <div class="clear"></div>

        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
            Width="100%" Height="350px" CssClass="dxtcFixed dxtcAligned horizontal-center-aligned" >
            <TabStyle Paddings-PaddingLeft="10px" Paddings-PaddingRight="10px"/>
            <ContentStyle>
                <Paddings PaddingLeft="2px"/>
            </ContentStyle>
            <TabPages>
                <dxe:TabPage Name="B2B" Text="GSTR-1 B2B">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <table class="TableMain100">
                                <tr>
                                    <td colspan="2">
                                        <div onkeypress="OnWaitingGridKeyPress(event)">         
                                                                              
                                            <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Gridb2b" Width="100%" EnableRowsCache="false"
                                                 ClientSideEvents-BeginCallback="Callback_BeginCallback"
                                                OnSummaryDisplayText="ShowGrid_SummaryDisplayText" OnDataBound="Showgrid_Datarepared" Settings-HorizontalScrollBarMode="Visible" OnCustomSummaryCalculate="ASPxGridView1_CustomSummaryCalculate"
                                                OnCustomCallback="Grid_CustomCallback" OnDataBinding="grid_DataBinding"  
                                                Settings-VerticalScrollableHeight="180" Settings-VerticalScrollBarMode="Auto">

                                                <Columns>

                                                    <dxe:GridViewDataTextColumn FieldName="GSTINUIN" Caption="GSTIN/UIN of Recipient" VisibleIndex="1" Width="220px">
                                                    </dxe:GridViewDataTextColumn>
                                                    
                                                    <dxe:GridViewDataTextColumn FieldName="Customer_Name" Caption="Receiver Name" VisibleIndex="2" Width="220px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="InvoiceNo" Caption="Invoice Number" VisibleIndex="3" Width="220px">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Date" Caption="Invoice Date" VisibleIndex="4" Width="220px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Value" Caption="Invoice Value" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00" Width="100px">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="POS" Caption="Place of Supply" VisibleIndex="6" Width="120px">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="Reverse Charge" Caption="Reverse Charge" VisibleIndex="7" Width="120px">
                                                    </dxe:GridViewDataTextColumn>

                                                   <%-- REV SAYANTANI--%>
                                                     <dxe:GridViewDataTextColumn  FieldName="Applicable_of_TaxRate" Caption="Applicable % of Tax Rate" VisibleIndex="8" Width="150px">
                                                    </dxe:GridViewDataTextColumn>
                                                 <%--   END OF REV SAYANTANI--%>
                                                

                                                    <dxe:GridViewDataTextColumn FieldName="Type" Caption="Invoice Type" VisibleIndex="9" Width="100px">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="GSTIN E-Commerce" Caption="E-Commerce GSTIN" VisibleIndex="10" Width="120px">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="11" Width="120px" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <%-- <dxe:GridViewDataTextColumn  Caption="Applicable % of Tax Rate" VisibleIndex="12" Width="150px">
                                                    </dxe:GridViewDataTextColumn>--%>



                                                    <dxe:GridViewDataTextColumn FieldName="Taxable value" Caption="Taxable value" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00" Width="120px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="cesss" Caption="Cess Amount" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00" Width="120px">
                                                    </dxe:GridViewDataTextColumn>


                                                </Columns>

                                                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                                                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                                                <SettingsEditing Mode="EditForm" />
                                                <SettingsContextMenu Enabled="true" />
                                                <SettingsBehavior AutoExpandAllGroups="true" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                <SettingsSearchPanel Visible="false" />
                                                <SettingsPager PageSize="10">
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>

                                                  <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="true" />

                                                <TotalSummary>

                                                    <dxe:ASPxSummaryItem FieldName="Value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Taxable value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="cesss" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="InvoiceNo" SummaryType="Custom" DisplayFormat="Count" />
                                                    <dxe:ASPxSummaryItem FieldName="GSTINUIN" SummaryType="Custom" DisplayFormat="Count" />

                                                </TotalSummary>

                                            </dxe:ASPxGridView>
                                            <dxe:ASPxGridViewExporter ID="ShowGridExporter"  runat="server" GridViewID="ShowGrid" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="B2CL" Text="GSTR-1 B2CL">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <table class="TableMain100">

                                <tr>
                                    <td colspan="2">
                                        <div>
                                            <dxe:ASPxGridView runat="server" ID="grid_b2cl" ClientInstanceName="b2clGrid" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="Grid_b2cl_SummaryDisplayText"
                                                OnCustomSummaryCalculate="GridView_b2cl_CustomSummaryCalculate" ClientSideEvents-BeginCallback="Callback_BeginCallback"
                                                OnCustomCallback="Grid_b2cl__CustomCallback" OnDataBinding="grid_b2cl_DataBinding">

                                                <Columns>

                                                    <dxe:GridViewDataTextColumn FieldName="InvoiceNo" Caption="Invoice No." Width="15%" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Date" Caption="Invoice Date" VisibleIndex="3" Width="150px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Value" Caption="Invoice Value" VisibleIndex="4" Width="20%" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="POS" Caption="Place of Supply" VisibleIndex="5"  Width="150px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--Rev Sayantani--%>
                                                    <dxe:GridViewDataTextColumn  FieldName="Applicable_of_TaxRate" Caption="Applicable % of Tax Rate" VisibleIndex="6" Width="150px">
                                                    </dxe:GridViewDataTextColumn>
                                                   <%-- End of Rev Sayantani--%>


                                                    <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00" Width="150px">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Taxable value" Caption="Taxable value" Width="20%" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="cesss" Caption="Cess Amount" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00"  Width="150px">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="GSTIN E-Commerce" Caption="E-Commerce GSTIN" VisibleIndex="10"  Width="150px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--Rev Sayantani--%>
                                                    <dxe:GridViewDataTextColumn  FieldName="Sale_from_Bonded_WH" Caption="Sale from Bonded WH" VisibleIndex="11" Width="150px">
                                                    </dxe:GridViewDataTextColumn>
                                                   <%-- End of Rev Sayantani--%>
                                                </Columns>

                                                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                                                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                                                <SettingsEditing Mode="EditForm" />
                                                <SettingsContextMenu Enabled="true" />
                                                <SettingsBehavior AutoExpandAllGroups="true" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                <SettingsSearchPanel Visible="false" />
                                                <SettingsPager PageSize="10">
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>

                                                  <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />

                                                <TotalSummary>
                                                    <dxe:ASPxSummaryItem FieldName="Value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Taxable value" SummaryType="Sum" />

                                                    <dxe:ASPxSummaryItem FieldName="InvoiceNo" SummaryType="Custom" DisplayFormat="Count" />



                                                </TotalSummary>

                                            </dxe:ASPxGridView>
                                            <dxe:ASPxGridViewExporter ID="grid_b2clExporter" runat="server" GridViewID="grid_b2cl" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="B2CS" Text="GSTR-1 B2CS">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <table class="TableMain100">

                                <tr>
                                    <td colspan="2">
                                        <div>
                                            <dxe:ASPxGridView runat="server" ID="grid_b2cs" ClientInstanceName="b2csGrid" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="Grid_b2cs_SummaryDisplayText" 
                                                ClientSideEvents-BeginCallback="Callback_BeginCallback"
                                                OnCustomCallback="Grid_b2cs__CustomCallback" OnDataBinding="grid_b2cs_DataBinding">

                                                <Columns>


                                                    <dxe:GridViewDataTextColumn FieldName="Type" Caption="Type" VisibleIndex="1">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="POS" Caption="Place of Supply" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--rev Pallab 25380: column position alternate--%>
                                                    <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Applicable_of_TaxRate" Caption="Applicable % of Tax Rate" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--rev end Pallab 25380--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Taxable value" Caption="Taxable value" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="00.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="cesss" Caption="Cess Amount" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="GSTIN E-Commerce" Caption="E-Commerce GSTIN" VisibleIndex="7">
                                                    </dxe:GridViewDataTextColumn>


                                                </Columns>

                                                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                                                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                                                <SettingsEditing Mode="EditForm" />
                                                <SettingsContextMenu Enabled="true" />
                                                <SettingsBehavior AutoExpandAllGroups="true" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                <SettingsSearchPanel Visible="false" />
                                                <SettingsPager PageSize="10">
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>

                                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />


                                                <TotalSummary>

                                                    <dxe:ASPxSummaryItem FieldName="Taxable value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="cesss" SummaryType="Sum" />




                                                </TotalSummary>

                                            </dxe:ASPxGridView>
                                            <dxe:ASPxGridViewExporter ID="grid_b2csExporter" runat="server" GridViewID="grid_b2cs" />

                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="CDNR9B" Text="GSTR-1 CDNR">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <table class="TableMain100">
                                <tr>
                                    <td colspan="2">
                                        <div class="GridViewArea">
                                            <dxe:ASPxGridView runat="server" ID="grid_cdnr" ClientInstanceName="cdnrGrid" Width="100%"
                                                EnableRowsCache="false" ClientSideEvents-BeginCallback="Callback_BeginCallback"
                                                OnSummaryDisplayText="Grid_cdnr_SummaryDisplayText" Settings-HorizontalScrollBarMode="Visible"
                                                OnCustomSummaryCalculate="GridView_cdnr_CustomSummaryCalculate"  OnCustomColumnDisplayText="GrdQuotation_CustomColumnDisplayText"
                                                OnCustomCallback="Grid_cdnr_CustomCallback" OnDataBinding="grid_cdnr_DataBinding">

                                                <Columns>
                                                    <dxe:GridViewDataTextColumn FieldName="GSTINUIN" Caption="GSTIN/UIN of Recipient" VisibleIndex="1" Width="140px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Customer_Name" Caption="Receiver Name" VisibleIndex="2" Width="140px">
                                                    </dxe:GridViewDataTextColumn>

                                                  <%--  <dxe:GridViewDataTextColumn FieldName="Invoice" Caption="Invoice/Advance Receipt Number" VisibleIndex="3" Width="190px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Invoice_Date" Caption="Invoice/Advance Receipt date" VisibleIndex="4" Width="170px">
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Return_Number" Caption="Note Number" VisibleIndex="5" Width="190px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Return_Date" Caption="Note Date" VisibleIndex="6" Width="170px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="type" Caption="Note Type" VisibleIndex="7">
                                                    </dxe:GridViewDataTextColumn>

<%--                                                <dxe:GridViewDataTextColumn FieldName="reason" Caption="Reason For Issuing document" VisibleIndex="8" Width="190px">
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    <dxe:GridViewDataTextColumn FieldName="POS" Caption="Place Of Supply" VisibleIndex="9">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="ReverseMechanism" Caption="Reverse Charge" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                     <dxe:GridViewDataTextColumn FieldName="note_supply_type" Caption="Note Supply Type" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Value" Caption="Note Value" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="N2" Width="190px">                                                    
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--Rev Debashis 0024232--%>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Applicable_of_TaxRate" Caption="Applicable % of Tax Rate" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00" Width="170px">
                                                    </dxe:GridViewDataTextColumn>--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Applicable_of_TaxRate" Caption="Applicable % of Tax Rate" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00" Width="170px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--End of Rev Debashis 0024232--%>

                                                    <dxe:GridViewDataTextColumn FieldName="Taxable value" Caption="Taxable Value" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="cess" Caption="Cess Amount" VisibleIndex="16" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

<%--                                                <dxe:GridViewDataTextColumn FieldName="pregst" Caption="Pre GST" VisibleIndex="15" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>--%>
                                                </Columns>

                                                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                                                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                                                <SettingsEditing Mode="EditForm" />
                                                <SettingsContextMenu Enabled="true" />
                                                <SettingsBehavior AutoExpandAllGroups="true" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                <SettingsSearchPanel Visible="false" />
                                                <SettingsPager PageSize="10">
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>

                                                  <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />


                                                <TotalSummary>
                                                    <dxe:ASPxSummaryItem FieldName="Value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Taxable value" SummaryType="Sum" />


                                                    <dxe:ASPxSummaryItem FieldName="Invoice" SummaryType="Custom" DisplayFormat="Count" />

                                                    <dxe:ASPxSummaryItem FieldName="GSTINUIN" SummaryType="Custom" DisplayFormat="Count" />

                                                    <dxe:ASPxSummaryItem FieldName="Return_Number" SummaryType="Custom" DisplayFormat="Count" />

                                                </TotalSummary>

                                            </dxe:ASPxGridView>
                                            <dxe:ASPxGridViewExporter ID="grid_cdnrExporter" runat="server" GridViewID="grid_cdnr" />

                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="CDNUR9B" Text="GSTR-1 CDNUR">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <table class="TableMain100">

                                <tr>
                                    <td colspan="2">
                                        <div class="GridView,Area">

                                            <dxe:ASPxGridView runat="server" ID="grid_cdnur" ClientInstanceName="cdnurGrid" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="Grid_cdnur_SummaryDisplayText"
                                                BeginCallback="Callback_BeginCallback"
                                                Settings-HorizontalScrollBarMode="Visible" OnCustomSummaryCalculate="GridView_cdnur_CustomSummaryCalculate"
                                                OnCustomCallback="Grid_cdnur_CustomCallback" OnDataBinding="grid_cdnur_DataBinding">

                                                <Columns>


                                                    <dxe:GridViewDataTextColumn FieldName="urltype" Caption="UR Type" VisibleIndex="1" Width="140px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Return_Number" Caption="Note Number" VisibleIndex="2" Width="190px">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Return_Date" Caption="Note date" VisibleIndex="3" Width="170px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="type" Caption="Note Type" VisibleIndex="4">
                                                    </dxe:GridViewDataTextColumn>


<%--                                                    <dxe:GridViewDataTextColumn FieldName="Invoice" Caption="Invoice/Advance Receipt Number" VisibleIndex="5" Width="190px">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Invoice_Date" Caption="Invoice/Advance Receipt date" VisibleIndex="6" Width="170px">
                                                    </dxe:GridViewDataTextColumn>--%>


<%--                                                <dxe:GridViewDataTextColumn FieldName="reason" Caption="Reason For Issuing document" VisibleIndex="7" Width="190px">
                                                    </dxe:GridViewDataTextColumn>--%>


                                                    <dxe:GridViewDataTextColumn FieldName="POS" Caption="Place Of Supply" VisibleIndex="8">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Value" Caption="Note Value" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00" Width="190px">
                                                    </dxe:GridViewDataTextColumn>

                                                    <%--Rev Sayantani--%>
                                                    <dxe:GridViewDataTextColumn FieldName="Applicable_of_TaxRate" Caption="Applicable % of Tax Rate" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00" Width="170px">
                                                    </dxe:GridViewDataTextColumn>
                                                   <%-- End of Rev Sayantani--%>
                                                    <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Taxable value" Caption="Taxable Value" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="cess" Caption="Cess Amount" VisibleIndex="13" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                   <%-- <dxe:GridViewDataTextColumn FieldName="pregst" Caption="Pre GST" VisibleIndex="14" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>--%>

                                                </Columns>

                                                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                                                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                                                <SettingsEditing Mode="EditForm" />
                                                <SettingsContextMenu Enabled="true" />
                                                <SettingsBehavior AutoExpandAllGroups="true" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                <SettingsSearchPanel Visible="false" />
                                                <SettingsPager PageSize="10">
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>


                                                  <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />


                                                <TotalSummary>
                                                    <dxe:ASPxSummaryItem FieldName="Value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Taxable value" SummaryType="Sum" />


                                                    <dxe:ASPxSummaryItem FieldName="Invoice" SummaryType="Custom" DisplayFormat="Count" />

                                                    <dxe:ASPxSummaryItem FieldName="GSTINUIN" SummaryType="Custom" DisplayFormat="Count" />

                                                    <dxe:ASPxSummaryItem FieldName="Return_Number" SummaryType="Custom" DisplayFormat="Count" />

                                                </TotalSummary>

                                            </dxe:ASPxGridView>
                                            <dxe:ASPxGridViewExporter ID="grid_cdnurExporter" runat="server" GridViewID="grid_cdnur" />

                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="EXP" Text="GSTR-1 EXP">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <table class="TableMain100">

                                <tr>
                                    <td colspan="2">
                                        <div>
                                            <dxe:ASPxGridView runat="server" ID="grid_exp" ClientInstanceName="expGrid" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="Grid_exp_SummaryDisplayText" BeginCallback="Callback_BeginCallback"
                                                Settings-HorizontalScrollBarMode="Auto" OnCustomSummaryCalculate="GridView_exp_CustomSummaryCalculate"
                                                OnCustomCallback="Grid_exp_CustomCallback" OnDataBinding="grid_exp_DataBinding">
                                                <Columns>
                                                    <dxe:GridViewDataTextColumn FieldName="exptype" Caption="Export Type" VisibleIndex="1" Width="20%">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="InvoiceNo" Caption="Invoice Number" VisibleIndex="2" Width="18%">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Date" Caption="Invoice Date" VisibleIndex="3" Width="15%">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Value" Caption="Invoice Value" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00" Width="30%">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="portcode" Caption="Port Code" VisibleIndex="5" Width="20%">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="shipbill" Caption="Shipping Bill Number" VisibleIndex="6" Width="20%">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="shipdate" Caption="Shipping Bill Date" VisibleIndex="7" Width="16%">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="8" Width="10%" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <%-- Rev Sayantani--%>
                                                    <%--Rev Debashis 0024232--%>
                                                   <%-- <dxe:GridViewDataTextColumn FieldName="Applicable_of_TaxRate" Caption="Applicable % of Tax Rate" VisibleIndex="8" Width="150px">
                                                    </dxe:GridViewDataTextColumn>--%>
                                                    <%--Rev Debashis 0024232--%>
                                                    <%-- End of Rev Sayantani--%>  
                                                   
                                                    <dxe:GridViewDataTextColumn FieldName="Taxable value" Caption="Taxable value" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00" Width="30%">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn  Caption="Cess" VisibleIndex="10" Width="150px">
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>

                                                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                                                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                                                <SettingsEditing Mode="EditForm" />
                                                <SettingsContextMenu Enabled="true" />
                                                <SettingsBehavior AutoExpandAllGroups="true" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                <SettingsSearchPanel Visible="false" />
                                                <SettingsPager PageSize="10">
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>


                                                  <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />

                                                <TotalSummary>
                                                    <dxe:ASPxSummaryItem FieldName="Value" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Taxable value" SummaryType="Sum" />

                                                    <dxe:ASPxSummaryItem FieldName="InvoiceNo" SummaryType="Custom" DisplayFormat="Count" />



                                                </TotalSummary>

                                            </dxe:ASPxGridView>
                                            <dxe:ASPxGridViewExporter ID="grid_expExporter" runat="server" GridViewID="grid_exp" />

                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="AT" Text="GSTR-1 AT">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <table class="TableMain100">

                                <tr>

                                    <td colspan="2">
                                        <div>
                                            <dxe:ASPxGridView runat="server" ID="grid_at" ClientInstanceName="atGrid" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="Grid_at_SummaryDisplayText" BeginCallback="Callback_BeginCallback"
                                                OnCustomCallback="Grid_at_CustomCallback" OnDataBinding="grid_at_DataBinding">

                                                <Columns>



                                                    <dxe:GridViewDataTextColumn FieldName="POS" Caption="Place of Supply" VisibleIndex="1">
                                                    </dxe:GridViewDataTextColumn>

                                                 <%--   Rev Sayantani--%>
                                                    <dxe:GridViewDataTextColumn FieldName="Applicable_of_TaxRate" Caption="Applicable % of Tax Rate" VisibleIndex="2" Width="150px">
                                                    </dxe:GridViewDataTextColumn>
                                                   <%-- End of Rev Sayantani--%>
                                                    <dxe:GridViewDataTextColumn FieldName="Rate2" Caption="Rate" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Value" Caption="Gross Advance Received" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="00.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="cesss" Caption="Cess Amount" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>



                                                </Columns>

                                                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                                                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                                                <SettingsEditing Mode="EditForm" />
                                                <SettingsContextMenu Enabled="true" />
                                                <SettingsBehavior AutoExpandAllGroups="true" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                <SettingsSearchPanel Visible="false" />
                                                <SettingsPager PageSize="10">
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>

                                                  <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                <TotalSummary>

                                                    <dxe:ASPxSummaryItem FieldName="Value" SummaryType="Sum" />

                                                       <dxe:ASPxSummaryItem FieldName="cesss" SummaryType="Sum" />


                                                </TotalSummary>

                                            </dxe:ASPxGridView>
                                            <dxe:ASPxGridViewExporter ID="grid_atExporter" runat="server" GridViewID="grid_at" />

                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="ADJ" Text="GSTR-1 ADVANCED  ADJUSTED">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <table class="TableMain100">

                                <tr>
                                    <td colspan="2">
                                        <div>
                                            <dxe:ASPxGridView runat="server" ID="grid_adj" ClientInstanceName="adjGrid" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="Grid_adj_SummaryDisplayText" BeginCallback="Callback_BeginCallback"
                                                OnCustomCallback="Grid_adj_CustomCallback" OnDataBinding="grid_adj_DataBinding">

                                                <Columns>



                                                    <dxe:GridViewDataTextColumn FieldName="POS" Caption="Place of Supply" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>

                                                   <%--   Rev Sayantani--%>
                                                    <dxe:GridViewDataTextColumn FieldName="Applicable_of_TaxRate" Caption="Applicable % of Tax Rate" VisibleIndex="3" Width="150px">
                                                    </dxe:GridViewDataTextColumn>
                                                   <%-- End of Rev Sayantani--%>


                                                    <dxe:GridViewDataTextColumn FieldName="Rate2" Caption="Rate" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Value" Caption="Gross Advance Adjusted" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="cesss" Caption="Cess Amount" VisibleIndex="6 " PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>





                                                </Columns>

                                                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                                                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                                                <SettingsEditing Mode="EditForm" />
                                                <SettingsContextMenu Enabled="true" />
                                                <SettingsBehavior AutoExpandAllGroups="true" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                <SettingsSearchPanel Visible="false" />
                                                <SettingsPager PageSize="10">
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>
                                                  <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                <TotalSummary>

                                                    <dxe:ASPxSummaryItem FieldName="Value" SummaryType="Sum" />
                                                     <dxe:ASPxSummaryItem FieldName="cesss" SummaryType="Sum" />



                                                </TotalSummary>

                                            </dxe:ASPxGridView>
                                            <dxe:ASPxGridViewExporter ID="grid_adjExporter" runat="server" GridViewID="grid_adj" />

                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="EXEMP" Text="GSTR-1 EXEMP">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <table class="TableMain100">

                                <tr>
                                    <td colspan="2">
                                        <div>
                                            <dxe:ASPxGridView runat="server" ID="grid_exemp" ClientInstanceName="exempGrid" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="Grid_exemp_SummaryDisplayText" BeginCallback="Callback_BeginCallback"
                                                OnCustomCallback="Grid_exemp_CustomCallback" OnDataBinding="grid_exemp_DataBinding">

                                                <Columns>


                                                    <dxe:GridViewDataTextColumn FieldName="name" Caption="Description" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>




                                                      <dxe:GridViewDataTextColumn FieldName="nilrated" Caption="Nil Rated Supplies" VisibleIndex="3" PropertiesTextEdit-DisplayFormatString="00.00">
                                                    </dxe:GridViewDataTextColumn>



                                                     <dxe:GridViewDataTextColumn FieldName="exempted" Caption="Exempted (other than nil rated/non GST supply )" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="00.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Amount" Caption="Non-GST Supplies" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="00.00">
                                                    </dxe:GridViewDataTextColumn>




                                                </Columns>

                                                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                                                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                                                <SettingsEditing Mode="EditForm" />
                                                <SettingsContextMenu Enabled="true" />
                                                <SettingsBehavior AutoExpandAllGroups="true" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                <SettingsSearchPanel Visible="false" />
                                                <SettingsPager PageSize="10">
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>
                                                  <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                              
                                                <TotalSummary>
 
                                                       <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
                                                       <dxe:ASPxSummaryItem FieldName="exempted" SummaryType="Sum" />
                                                       <dxe:ASPxSummaryItem FieldName="nilrated" SummaryType="Sum" />

                                                </TotalSummary>

                                            </dxe:ASPxGridView>
                                            <dxe:ASPxGridViewExporter ID="grid_exempExporter" runat="server" GridViewID="grid_exemp" />

                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>


                <dxe:TabPage Name="HSN" Text="GSTR-1 HSN(12)">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <table class="TableMain100">

                                <tr>
                                    <td colspan="2">
                                        <div>
                                            <dxe:ASPxGridView runat="server" ID="grid_hsn" ClientInstanceName="hsnGrid" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="Grid_hsn_SummaryDisplayText"  BeginCallback="Callback_BeginCallback"
                                                OnCustomCallback="Grid_hsn_CustomCallback" OnDataBinding="grid_hsn_DataBinding">

                                                <Columns>


                                                    <dxe:GridViewDataTextColumn FieldName="sProducts_HsnCode" Caption="HSN" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="sProducts_Description" Caption="Description" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UQC" VisibleIndex="3">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Qty" Caption="Total Quantity" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Net" Caption="Total Value" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="TAxAmt" Caption="Taxable Value" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="IGSTRate" Caption="Integrated Tax Amount" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="CGSTRate" Caption="Central tax Amount" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="SGSTRate" Caption="State/UT tax Amount" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="cess" Caption="Cess Amount" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                </Columns>

                                                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                                                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                                                <SettingsEditing Mode="EditForm" />
                                                <SettingsContextMenu Enabled="true" />
                                                <SettingsBehavior AutoExpandAllGroups="true" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                <SettingsSearchPanel Visible="false" />
                                                <SettingsPager PageSize="10">
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>
                                                  <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                <TotalSummary>

                                                    <dxe:ASPxSummaryItem FieldName="Qty" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Net" SummaryType="Sum" />

                                                    <dxe:ASPxSummaryItem FieldName="TAxAmt" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="IGSTRate" SummaryType="Sum" />


                                                    <dxe:ASPxSummaryItem FieldName="CGSTRate" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="SGSTRate" SummaryType="Sum" />

                                                </TotalSummary>

                                            </dxe:ASPxGridView>
                                            <dxe:ASPxGridViewExporter ID="grid_hsnExporter" runat="server" GridViewID="grid_hsn" />

                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="HSNDET" Text="GSTR-1 HSN Summary(12)">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <table class="TableMain100">

                                <tr>
                                    <td colspan="2">
                                        <div>

                                            <dxe:ASPxGridView runat="server" ID="grid_hsndetails" ClientInstanceName="hsndetailsGrid" Width="100%" EnableRowsCache="false"
                                                OnSummaryDisplayText="Grid_hsndetietails_SummaryDisplayText"  BeginCallback="Callback_BeginCallback"
                                                OnCustomCallback="Grid_hsndetials_CustomCallback" OnDataBinding="grid_hsndetails_DataBinding">

                                                <Columns>


                                                    <dxe:GridViewDataTextColumn FieldName="sProducts_HsnCode" Caption="HSN" VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="HSN_Desc"  Caption="Description"  VisibleIndex="3">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="UOM" Caption="UQC" VisibleIndex="4">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Qty" Caption="Total Quantity" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Net" Caption="Total Value" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>



                                                    <dxe:GridViewDataTextColumn FieldName="TAxAmt" Caption="Taxable Value" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="IGSTRate" Caption="Integrated Tax Amount" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="CGSTRate" Caption="Central tax Amount" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="SGSTRate" Caption="State/UT tax Amount" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="cess" Caption="Cess Amount" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--REV PALLAB 25380--%>
                                                    <dxe:GridViewDataTextColumn FieldName="IGST_Rate" Caption="Rate" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" VisibleIndex="11" Width="120px" PropertiesTextEdit-DisplayFormatString="0.00">
                                                    </dxe:GridViewDataTextColumn>--%>
                                                    <%--REV end PALLAB 25380--%>

                                                </Columns>

                                                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                                                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                                                <SettingsEditing Mode="EditForm" />
                                                <SettingsContextMenu Enabled="true" />
                                                <SettingsBehavior AutoExpandAllGroups="true" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                <SettingsSearchPanel Visible="false" />
                                                <SettingsPager PageSize="10">
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>
                                                  <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                <TotalSummary>

                                                    <dxe:ASPxSummaryItem FieldName="Qty" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="Net" SummaryType="Sum" />

                                                    <dxe:ASPxSummaryItem FieldName="TAxAmt" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="IGSTRate" SummaryType="Sum" />


                                                    <dxe:ASPxSummaryItem FieldName="CGSTRate" SummaryType="Sum" />
                                                    <dxe:ASPxSummaryItem FieldName="SGSTRate" SummaryType="Sum" />

                                                </TotalSummary>

                                            </dxe:ASPxGridView>
                                            <dxe:ASPxGridViewExporter ID="grid_hsndetailsExporter" runat="server" GridViewID="grid_hsndetails" />

                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
                <dxe:TabPage Name="GSTINDocumentCount" Text="GSTR-1 Document Count">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">
                            <table class="TableMain100">

                                <tr>
                                    <td colspan="2">
                                        <div>
                                            <dxe:ASPxGridView runat="server" ID="GST_DocumentCount" ClientInstanceName="GSTNInDocumentCount" Width="100%" EnableRowsCache="false"
                                               OnSummaryDisplayText="Grid_Document_SummaryDisplayText"  BeginCallback="Callback_BeginCallback" 
                                                OnCustomCallback="Grid_DocumentCount_CustomCallback" OnDataBinding="grid_DocumentCount_DataBinding">

                                                <Columns>


                                                    <dxe:GridViewDataTextColumn FieldName="Nature_of_Document" Caption="Document Nature" VisibleIndex="1">
                                                    </dxe:GridViewDataTextColumn>

                                                    <dxe:GridViewDataTextColumn FieldName="Start_Documnet_number" Caption="Start Doc No." VisibleIndex="2">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="End_Documnet_number" Caption="End Doc No." VisibleIndex="3">
                                                    </dxe:GridViewDataTextColumn>


                                                    <dxe:GridViewDataTextColumn FieldName="Total_Number" Caption="Total Number" VisibleIndex="4" >
                                                    </dxe:GridViewDataTextColumn>

                                                   <%--Rev Sayantani--%>
                                                    <%--<dxe:GridViewDataTextColumn FieldName="Cancelled" Caption="Cancelled" VisibleIndex="5" >
                                                    </dxe:GridViewDataTextColumn>--%>
                                                  <%--End of Rev Sayantani--%>
                                             


                                                </Columns>

                                                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                                                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                                                <SettingsEditing Mode="EditForm" />
                                                <SettingsContextMenu Enabled="true" />
                                                <SettingsBehavior AutoExpandAllGroups="true" />
                                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                                                <SettingsSearchPanel Visible="false" />
                                                <SettingsPager PageSize="10">
                                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                                </SettingsPager>
                                                  <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                <TotalSummary>

                                                    <dxe:ASPxSummaryItem FieldName="Total_Number" SummaryType="Sum" />
                                                    

                                                </TotalSummary>

                                            </dxe:ASPxGridView>
                                            <dxe:ASPxGridViewExporter ID="GST_DocumentCountExporter" runat="server" GridViewID="GST_DocumentCount" />

                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>
            </TabPages>
            <clientsideevents activetabchanged="Tabchange" />
        </dxe:ASPxPageControl>




    </div>

    <div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>




</asp:Content>

