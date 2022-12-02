<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PurchaseOrderRegister_Details.aspx.cs" Inherits="Reports.Reports.GridReports.PurchaseOrderRegister_Details" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">

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
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 220px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/
    </style>


    <script type="text/javascript">

        //function fn_OpenDetails(keyValue) {
        //    Grid.PerformCallback('Edit~' + keyValue);
        //}

        $(function () {
            cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
        });

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');
            })

            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
           <%--     $('#<%=hdnSelectedBranches.ClientID %>').val(Ids);--%>
                $('#MandatoryActivityType').attr('style', 'display:none');
                $("#hdnSelectedBranches").val('');
                cProductComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        })

        function CheckConsIPO(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CShowIPOTC.SetEnabled(true);
            }
            else {
                CShowIPOTC.SetCheckState('UnChecked');
                CShowIPOTC.SetEnabled(false);
            }
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
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsPurOrdRegDetFilter").val("Y");
            var branchid = $('#ddlbranchHO').val();
            //cCallbackPanel.PerformCallback(data + '~' + branchid);
            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(data + '~' + branchid);
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

        function abc() {
            $("#drdExport").val(0);
        }

        function OpenBillDetails(branch) {
            $("#drdExport").val(0);
            cgridPendingApproval.PerformCallback('BndPopupgrid~' + branch);
            cpopupApproval.Show();
            return true;
        }

        function popupHide(s, e) {
            cpopupApproval.Hide();
        }

        function OpenPOSDetails(invoice, type) {
            if (type == 'PO') {
                url = '/OMS/Management/Activities/PurchaseOrder.aspx?key=' + invoice + '&IsTagged=1&req=V&type=PO';
            }
            else if (type == 'IPO') {
                url = '/Import/Purchaseorder-Import.aspx?key=' + invoice + '&IsTagged=1&req=V';
            }
            popupdetails.SetContentUrl(url);
            popupdetails.Show();
        }

        function DetailsAfterHide(s, e) {
            popupdetails.Hide();
        }

        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
        }

        function Callback2_EndCallback() {
            if (Grid.cpErrorFinancial == 'ErrorFinancial') {
                jAlert('Date Range should be within Financial Year');
            }
            else {
                //alert(Grid.cpSummary);

                var Amount = parseFloat(Grid.cpSummary);
                ctxtdiffcalculation.SetText(Amount);
                ctxtdiffcalculationText.SetText('Mismatch Defeated');
                Grid.cpSummary = null;

                if (Amount != 0) {
                    loadCurrencyMassage.style.display = "block";
                }
                else {
                    loadCurrencyMassage.style.display = "none";
                }

                // alert('');
                $("#drdExport").val(0);
                Grid.Focus();
                Grid.SetFocusedRowIndex(2);
            }
            Grid.cpErrorFinancial = null;
        }

    </script>
    <script>

        function GethiddenSalesregister() {
            var value1 = '1';
            alert(value1);

            if (value1 != "0") {
                $("#hdnexpid").val(value1);
                return true;
            }
            else { }
        }

        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }
        function CallbackPanelEndCall(s, e) {
              <%--Rev Subhra 20-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            //End of Subhra
            Grid.Refresh();
        }
        function CloseGridQuotationLookup() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }


    </script>
    <style>
        
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
</asp:content>

<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">

    <div>
        <asp:HiddenField ID="hdnexpid" runat="server" />
    </div>
    <div class="panel-heading">
       <%-- <div class="panel-title">
            <h3>Purchase Order Register - Detail </h3>
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
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">Head Branch</label>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>

                </div>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"></asp:Label></label>
                <asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="60px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                <asp:HiddenField ID="hdnActivityType" runat="server" />

                <dxe:ASPxCallbackPanel runat="server" ID="ComponentQuotationPanel" ClientInstanceName="cProductComponentPanel" OnCallback="Componentbranch_Callback">
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
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" UseSubmitBehavior="False"/>
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridQuotationLookup" UseSubmitBehavior="False" />
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

                <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
            </div>

            <div class="col-md-2">
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
            </div>
           <div class="col-md-2" style="margin-top:27px">
             <%--<asp:CheckBox runat="server" ID="chkShowTermsnCondition" Checked="false" Text="Show Terms & Condition" />--%>
            <dxe:ASPxCheckBox runat="server" ID="ShowTermsnCondition" Checked="false" Text="Show Terms & Condition">
            </dxe:ASPxCheckBox>
            </div>
            <div class="col-md-2" style="margin-top:27px">
             <dxe:ASPxCheckBox runat="server" ID="ConsIPO" Checked="false" Text="Consider Import Purchase" >
                 <ClientSideEvents CheckedChanged="CheckConsIPO" />
             </dxe:ASPxCheckBox>
            </div>
             <div class="clear"></div>
            <div class="col-md-3" style="margin-top:15px">
                <dxe:ASPxCheckBox runat="server" ID="ShowIPOTC" Checked="false" ClientEnabled="false" Text="Show Import Purchase - Terms & Condition" ClientInstanceName="CShowIPOTC">
                </dxe:ASPxCheckBox>
            </div> 
            <%--Rev Maynak 28-10-2019  0019874--%>
            <div class="col-md-2">
                <label  style="color: #b5285f; font-weight: bold;" class="clsFrom">Rate in Decimal</label>
                <div class="">
                    <asp:DropDownList ID="ddlRateINDec" runat="server" Width="100%">
                        <asp:ListItem Value="2">2 Decimal</asp:ListItem>
                        <asp:ListItem Value="3">3 Decimal</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <%--End of Rev Maynak--%>
            <div class="col-md-2" style="margin-top:15px">
                <dxe:ASPxCheckBox runat="server" ID="chkCreateBy" Checked="false" Text="Show Created by" ></dxe:ASPxCheckBox>
            </div>
            <div class="col-md-2" style="padding-top: 15px;">
            <table>
                <tr>
                  
                    <td>
                        <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
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
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Visible"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback"
                            Settings-VerticalScrollableHeight="400" Settings-VerticalScrollBarMode="Auto" OnDataBound="ShowRateInTwoDecimal_DataBound"> <%--Rev Maynak Added OnDataBound For 0019874--%>
                            <Columns>
                                <%-- OnCustomCallback="Grid_CustomCallback" OnDataBinding="ShowGrid_DataBinding"--%>

                                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BRANCHNAME" Width="150px"
                                    VisibleIndex="1" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Vendor Name" Width="220px" FieldName="PARTYNAME"
                                    VisibleIndex="2" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="City" Width="120px" FieldName="city_name" VisibleIndex="3" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="State" Width="120px" FieldName="state" VisibleIndex="4" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Country" Width="100px" FieldName="cou_country" VisibleIndex="5" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Ship to Party Name" Width="200px" FieldName="SHIP_TO_PARTY" VisibleIndex="6" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="DOCONO" Width="130px" Caption="Order No." >
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <%--<a href="javascript:void(0)" target="_blank" onclick="OpenPOSDetails('<%#Eval("DOC_ID") %>','PO')">
                                            <%#Eval("DOCONO")%>
                                        </a>--%>
                                        <a href="javascript:void(0)" onclick="OpenPOSDetails('<%#Eval("DOC_ID") %>','<%#Eval("MODULETYPE") %>')">
                                            <%#Eval("DOCONO")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Order Date" Width="80px" FieldName="DOCDATE"
                                    VisibleIndex="8">
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="IMPORTPURCHASE" Width="80px" Caption="Imp. Purc." >
                                     <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <%--Rev Subhra 0019139  27-12-2018--%>
                                 <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="INVNO" Width="130px" Caption="Purchase Invoice No." Settings-AllowAutoFilter="False">
                                     <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Purchase Invoice Date" Width="130px" FieldName="INVDATE" Settings-AllowAutoFilter="False"
                                    VisibleIndex="11">
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="PARTYINVNO" Width="130px" Caption="Party Invoice No." Settings-AllowAutoFilter="False" >
                                     <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Party Invoice Date" Width="130px" FieldName="PARTYINVDATE" Settings-AllowAutoFilter="False"
                                    VisibleIndex="13">
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <%--End Rev--%> 
                                
                                <dxe:GridViewDataTextColumn Caption="Item Descprition" FieldName="PRODUCTDESC" Width="200px"
                                    VisibleIndex="14">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="QTY" Caption="Qty" Width="70px" VisibleIndex="15">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="COMMISSIONRCV" Caption="Comm. Receivable" Width="120px" VisibleIndex="16">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn FieldName="COMMISSIONRCVDTLS" Caption="Comm. Details" Width="110px" VisibleIndex="17">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                                  <dxe:GridViewDataTextColumn FieldName="COMMISSIONRATE" Caption="Comm. Rate" Width="110px" VisibleIndex="18">
                                      <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn FieldName="DISCNTRCV" Caption="Disc. Receivable" Width="110px" VisibleIndex="19">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DISCNTRCVDTLS" Caption="Disc. Details" Width="110px" VisibleIndex="20">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Delivery Schedule/Date" Width="130px" FieldName="DELIVERYDATE" Settings-AllowAutoFilter="False" VisibleIndex="21">
                                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DELREMARKS" Caption="Delivery Remarks" Width="200px" VisibleIndex="22">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="INSURANCECOVERAGE" Caption="Insurance Coverage" Width="120px" VisibleIndex="23">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="FREIGHTCHARGES" Caption="Freight Charge" Width="100px" VisibleIndex="24">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="FREIGHTREMARKS" Caption="Freight Remarks" Width="200px" VisibleIndex="25">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PERMITVALUE" Caption="E-Permit/Way bill/Road Permit" Width="180px" VisibleIndex="26">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OTHREMARKS" Caption="Other Remarks" Width="200px" VisibleIndex="27">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CERTREQ" Caption="Test Certificate Required?" Width="150px" VisibleIndex="28">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DELDETAILS" Caption="Delivery Details" Width="120px" VisibleIndex="29">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TC_PAYMENTTERMS" Caption="Payment Terms" Width="120px" VisibleIndex="30">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TRNSPRTRNAME" Caption="Transporter Name(General)" Width="180px" VisibleIndex="31">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BANK_NAME" Caption="Bank Name" Width="180px" VisibleIndex="32">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BANK_BRANCH" Caption="Bank Branch Name" Width="180px" VisibleIndex="33">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BANK_ADDRESS" Caption="Bank Branch Address" Width="180px" VisibleIndex="34">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BANK_LANDMARK" Caption="Bank Branch Landmark" Width="180px" VisibleIndex="35">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BANK_PIN" Caption="Bank Branch Pin" Width="110px" VisibleIndex="36">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BANK_ACNO" Caption="Account Number" Width="120px" VisibleIndex="37">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BANK_SWIFTCODE" Caption="SWIFT Code" Width="120px" VisibleIndex="38">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="RTGS" Caption="RTGS" Width="120px" VisibleIndex="39">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="NEFT" Caption="NEFT" Width="120px" VisibleIndex="40">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IFSC" Caption="IFSC Code" Width="120px" VisibleIndex="41">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="BANK_REMARKS" Caption="Remarks" Width="180px" VisibleIndex="42">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="TYPEOFIMPORT" Caption="Type of Import" Width="120px" VisibleIndex="43">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="INCODVTERMS" Caption="Incoterms" Width="120px" VisibleIndex="44">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="PAYMENTTRMREMARKS" Caption="Payment Remarks" Width="120px" VisibleIndex="45">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="INCODVTERMSREMARKS" Caption="Incoterms Remarks" Width="120px" VisibleIndex="46">
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="VALIDITYOFORDERDATE" Caption="Validity Date" Width="120px" VisibleIndex="47">
                                     <HeaderStyle HorizontalAlign="Left" />
                                     <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="RATE" Caption="Rate" Width="90px" VisibleIndex="48">
                                    <%--Rev Maynak 28-10-2019 0019874 --%>
                                    <%--<PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>--%>
                                    <%--End of Rev Maynak --%>
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="GROSAMOUNT" Caption="Purchase Value" Width="90px" VisibleIndex="49">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CGST_AMT" Caption="CGST" Width="90px" VisibleIndex="50">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="SGST_AMT" Caption="SGST" Width="90px" VisibleIndex="51">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IGST_AMT" Caption="IGST" Width="90px" VisibleIndex="52">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="UTGST_AMT" Caption="UTGST" Width="90px" VisibleIndex="53">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OTHER_AMT" Caption="Other Charges" Width="120px" VisibleIndex="54">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TAX_MISC" Caption="Tax Misc." Width="110px" VisibleIndex="55">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TOTALAMOUNT" Caption="Total Value" Width="110px" VisibleIndex="56">
                                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                     <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Created by" Width="160px" FieldName="CREATEDBY" VisibleIndex="57">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                </dxe:GridViewDataTextColumn>

                            </Columns>
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
                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="GROSAMOUNT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CGST_AMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="SGST_AMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IGST_AMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="UTGST_AMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="TAX_MISC" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OTHER_AMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="TOTALAMOUNT" SummaryType="Sum" />
                            </TotalSummary>
                            <%--<clientsideevents endcallback="Callback2_EndCallback" />--%>
                        </dxe:ASPxGridView>
                          <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="SALEPURCHASEORDERREGISTERDETAIL_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                </td>
            </tr>
        </table>

        <%--<%--<div class="text-center" style="display: none;">
            <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                <%--  <asp:Label ID="lbldiffcalculationText" runat="Server" Text="" CssClass="mylabel1"
                        Width="92px" Font-Bold="True" ForeColor="#cc0000"></asp:Label>--%>
                <%--<asp:Label ID="lbldiffcalculation" runat="Server" Text="" CssClass="mylabel1"
                        Width="92px" Font-Bold="True" ForeColor="#cc0000"></asp:Label>
                <dxe:ASPxTextBox ID="txtdiffcalculation" ClientInstanceName="ctxtdiffcalculation" runat="server" ReadOnly="true" Width="50px"></dxe:ASPxTextBox>
                <dxe:ASPxTextBox ID="txtdiffcalculationText" ClientInstanceName="ctxtdiffcalculationText" runat="server" ReadOnly="true" Width="100px"></dxe:ASPxTextBox>

            </label>
        </div>
        <%--<div id="loadCurrencyMassage" style="display: none;">
            <br />
            <label><span style="color: red; font-weight: bold; font-size: medium;">**  Trial Balance Mismatched.</span></label>
        </div>--%>

    </div>
    <div>
    </div>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
     <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdetails" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="DetailsAfterHide" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsPurOrdRegDetFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:content>
