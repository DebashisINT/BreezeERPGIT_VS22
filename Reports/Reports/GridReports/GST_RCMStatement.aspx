<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="GST_RCMStatement.aspx.cs" Inherits="Reports.Reports.GridReports.GST_RCMStatement" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        function updateGridByDate() {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            $("#hfIsRCMStatementFilter").val("Y");
            if (cxdeToDate.GetDate() < cxdeFromDate.GetDate()) {
                jAlert('From date shoulkd not be grater than Todate');
            }
            else {
                //cRcmGrid.PerformCallback('BindRCMGrid');
                //cRcmGrid.PerformCallback('BindRCMGrid' + '~' + $("#ddlgstn").val());
                //cCallbackPanel.PerformCallback('BindRCMGrid' + '~' + $("#ddlgstn").val());
                if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                    jAlert('Please select atleast one branch for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback('BindRCMGrid' + '~' + $("#ddlgstn").val());
                }
            }
        }

        function CloseGridBranchLookup() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }


        function selectAll_branch() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll_branch() {
            gridbranchLookup.gridView.UnselectRows();
        }

        //$(function () {
        //    cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);
        //});


        $(function () {
            $('body').on('change', '#ddlgstn', function () {
                if ($("#ddlgstn").val()) {
                    cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlgstn").val());
                }
                else {
                    cProductbranchComponentPanel.PerformCallback('BindComponentGrid' + '~' + 0);
                }
            });
        });


        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            e.preventDefault;
            //var data = "OnDateChanged";
            var v = $("#ddlgstn").val();
            $("#hfIsRCMStatementFilter").val("Y");
            //cCallbackPanel.PerformCallback('ListData~' + v);
            //Grid.PerformCallback('ListData~' + v);
            //Grid.PerformCallback(data + '~' + $("#ddlgstn").val());
            //Gridreturn.PerformCallback('ListData~' + v);

            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback('ListData~' + v);
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

        function CallbackPanelEndCall(s, e) {
           <%--Rev Subhra 20-12-2018   0017670--%>
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
           //End of Subhra
            cRcmGrid.Refresh();
        }

    </script>
    <style>
        .mrtable, .padtbl {
            margin-left: 15px;
        }

            .mrtable > tbody > tr > td {
                padding-right: 25px;
            }

            .padtbl > tbody > tr > td {
                padding-right: 15px;
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
        /*rev Pallab*/
        .branch-selection-box .dxlpLoadingPanel_PlasticBlue tbody, .branch-selection-box .dxlpLoadingPanelWithContent_PlasticBlue tbody, #divProj .dxlpLoadingPanel_PlasticBlue tbody, #divProj .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            bottom: 0 !important;
        }
        .dxlpLoadingPanel_PlasticBlue tbody, .dxlpLoadingPanelWithContent_PlasticBlue tbody
        {
            position: absolute;
            bottom: 200px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 55%;
        }
        /*rev end Pallab*/
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cRcmGrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cRcmGrid.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cRcmGrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cRcmGrid.SetWidth(cntWidth);
                }

            });
        });
    </script>
    <div class="panel-heading">
        <%--<div class="panel-title">
            <h3>RCM Statement</h3>
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

        <%--<div class="SearchArea">--%>
        <div class="clear"></div>
        <div class="clearfix row">

            <div class="col-md-2">
                <%--<label>GSTIN: </label>--%>
                <%-- <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label6" runat="Server" Text="GSTIN : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>

                    <dxe:ASPxComboBox ID="cmbGstinlist" ClientInstanceName="ccmbGstinlist" runat="server" SelectedIndex="0"
                        ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                    </dxe:ASPxComboBox>
                </div>--%>

                <td>
                    <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                        <asp:Label ID="Label6" runat="Server" Text="GSTIN : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>
                <td>
                    <asp:DropDownList ID="ddlgstn" runat="server" Width="150px"></asp:DropDownList>
                </td>
            </div>

            <div class="col-md-2 branch-selection-box">
                <td style="">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label5" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                </td>


                <td style="width: 254px">

                    <asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <span id="MandatoryBranch" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EII" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="HiddenField3" runat="server" />

                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchComponentPanel" OnCallback="Componentbranch_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                    OnDataBinding="lookup_branch_DataBinding"
                                    KeyFieldName="branch_id" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
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
                                                            <div class="hide">
                                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_branch" />
                                                            </div>
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_branch" />                                                            
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
                        </PanelCollection>

                    </dxe:ASPxCallbackPanel>
                </td>

            </div>

            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="Document Type: " CssClass="mylabel1"></asp:Label>
                </label>
                <asp:DropDownList ID="ddlisdocument" runat="server" Width="100%">
                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                    <asp:ListItem Text="Cash/Bank" Value="Cash/Bank"></asp:ListItem>
                    <asp:ListItem Text="Purchases" Value="Purchases"></asp:ListItem>
                    <asp:ListItem Text="Journal" Value="Journal"></asp:ListItem>

                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="RCM Type: " CssClass="mylabel1"></asp:Label>
                </label>
                <asp:DropDownList ID="ddlistRCM" runat="server" Width="100%">
                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="ITC Type: " CssClass="mylabel1"></asp:Label>
                </label>
                <asp:DropDownList ID="ddlistITC" runat="server" Width="100%">
                    <asp:ListItem Text="All" Value="All"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="clear"></div>
            <table class="padtbl">
                <tr>
                    <td>
                        <asp:CheckBox ID="chkdet" runat="server" Checked="true"></asp:CheckBox>
                        <asp:Label ID="Label1" runat="Server" Text="Show Details " CssClass="mylabel1"></asp:Label></td>

                    <td>
                        <asp:CheckBox ID="chkPartyInvDt" runat="server" Checked="true"></asp:CheckBox>
                        <asp:Label ID="Label7" runat="Server" Text="Search by Party Invoice Date " CssClass="mylabel1"></asp:Label></td>

                    <td>
                        <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                            <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                        </label>
                        <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dxe:ASPxDateEdit>
                    </td>

                    <td>
                        <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                            <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                                Width="92px"></asp:Label>
                        </label>
                        <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>

                        </dxe:ASPxDateEdit>
                    </td>

                    <td style="width: 200px; padding-top: 18px;">
                        <input type="button" style="margin-top: 2px;" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                        <asp:DropDownList ID="drdExport" runat="server" Style="margin-top: 2px;" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLSX</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>

            <%--<div class="col-md-3">
                <label>From: </label>
                <div>

                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
            </div>--%>





            <%--<div class="col-md-3">
                <label>To: </label>
                <div>

                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>

                </div>
            </div>--%>
            <div class="col-md-2">
            </div>

            <div class="col-md-2">

                <table>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                </table>



            </div>


        </div>
        <%--  </div>--%>


        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" ClientInstanceName="page"
            Font-Size="12px" Width="100%">
            <TabPages>
                <dxe:TabPage Name="RCM" Text="Reverse Charged Documents">
                    <ContentCollection>
                        <dxe:ContentControl runat="server">


                            <div class="GridViewArea">
                                <dxe:ASPxGridView ID="RcmGrid" runat="server" KeyFieldName="SL" AutoGenerateColumns="False"
                                    Width="100%" ClientInstanceName="cRcmGrid" SettingsBehavior-AllowFocusedRow="true" OnSummaryDisplayText="RcmGrid_SummaryDisplayText"
                                    SettingsBehavior-AllowSelectSingleRowOnly="false" DataSourceID="GenerateEntityServerModeDataSource"
                                    SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto" 
                                    SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true"
                                    SettingsCookies-StoreGroupingAndSorting="true" SettingsBehavior-ColumnResizeMode="Control"
                                    Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                                    <%--OnCustomCallback="RcmGrid_CustomCallback" OnCustomSummaryCalculate="ASPxGridView1_CustomSummaryCalculate" OnDataBinding="RcmGrid_DataBinding" --%>
                                    <Columns>

                                        <dxe:GridViewDataTextColumn Caption="Sl. No." FieldName="SL"
                                            VisibleIndex="0" FixedStyle="Left">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Unit " FieldName="BRANCH"
                                            VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Document Date" FieldName="DOC_DATE"
                                            VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Document No." FieldName="DOC_NO"
                                            VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Party Invoice Date" FieldName="Party_Invoice_Date"
                                            VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Party Invoice No" FieldName="Party_Invoice_No" Width="140px"
                                            VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                         <dxe:GridViewDataTextColumn Caption="Party Name" FieldName="PARTY_NAME" Width="140px"
                                            VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                         <dxe:GridViewDataTextColumn Caption="Supply" FieldName="SUPPLY" Width="140px"
                                            VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Document Type" FieldName="DOC_TYPE" Width="170px"
                                            VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Cash/Bank" FieldName="CASH_BANK" Width="140px"
                                            VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Account Head" FieldName="ACC_HEAD"
                                            VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Taxable Amount" FieldName="Taxable_Amount"
                                            VisibleIndex="0">
                                            <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            </PropertiesTextEdit>
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="CGST Rate" FieldName="CGST_Rate"
                                            VisibleIndex="0">
                                            <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            </PropertiesTextEdit>
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="SGST Rate" FieldName="SGST_Rate"
                                            VisibleIndex="0">
                                            <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            </PropertiesTextEdit>
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="IGST Rate" FieldName="IGST_Rate"
                                            VisibleIndex="0">
                                            <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            </PropertiesTextEdit>
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>


                                        <dxe:GridViewDataTextColumn Caption="CGST Amt." FieldName="CGST_Amount"
                                            VisibleIndex="0">
                                            <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            </PropertiesTextEdit>
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="SGST Amt." FieldName="SGST_Amount"
                                            VisibleIndex="0">
                                            <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            </PropertiesTextEdit>
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="IGST Amt." FieldName="IGST_Amount"
                                            VisibleIndex="0">
                                            <PropertiesTextEdit Style-HorizontalAlign="Right" DisplayFormatString="0.00">
                                            </PropertiesTextEdit>
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Tax Type" FieldName="TAX_TYPE"
                                            VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="RCM" FieldName="RCM"
                                            VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="ITC" FieldName="ITC"
                                            VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                            </CellStyle>
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>

                                    <SettingsPager PageSize="10">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100" />
                                    </SettingsPager>

                                    <SettingsSearchPanel Visible="false" />
                                    <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <SettingsLoadingPanel Text="Please Wait..." />


                                    <TotalSummary>
                                        <dxe:ASPxSummaryItem FieldName="Taxable_Amount" SummaryType="Sum" />
                                        <dxe:ASPxSummaryItem FieldName="CGST_Amount" SummaryType="Sum" />
                                        <dxe:ASPxSummaryItem FieldName="SGST_Amount" SummaryType="Sum" />
                                        <dxe:ASPxSummaryItem FieldName="IGST_Amount" SummaryType="Sum" />
                                        <dxe:ASPxSummaryItem FieldName="DOC_NO" SummaryType="Custom" DisplayFormat="Count" />
                                    </TotalSummary>


                                </dxe:ASPxGridView>
                                <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                                ContextTypeName="ReportSourceDataContext" TableName="RCMSTATEMENTDETSUM_REPORT" ></dx:LinqServerModeDataSource>
                                <asp:HiddenField ID="hiddenedit" runat="server" />
                            </div>
                            <div style="display: none">
                                <dxe:ASPxGridViewExporter ID="exporter" GridViewID="RcmGrid" runat="server" Landscape="true" PaperKind="Letter" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" MaxColumnWidth="75">
                                </dxe:ASPxGridViewExporter>
                            </div>

                        </dxe:ContentControl>
                    </ContentCollection>
                </dxe:TabPage>

            </TabPages>
        </dxe:ASPxPageControl>
    </div>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsRCMStatementFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>

