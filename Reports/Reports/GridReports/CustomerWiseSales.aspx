<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CustomerWiseSales.aspx.cs" Inherits="Reports.Reports.GridReports.CustomerWiseSales" %>

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
            bottom: 200px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/
    </style>


    <script type="text/javascript">
        $(function () {
            cBranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
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
                cBranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        })

        function CallbackPanelDetEndCall(s, e) {
            cShowGridDet.Refresh();
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
  <%-- For Customer multiselection--%>
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
   <%-- For Customer multiselection--%>    

    <script type="text/javascript">
        //for Esc
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && cpopupDetails.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
                popupHide();
            }
        }
        function popupHide(s, e) {
            cpopupDetails.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }

        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }

        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsCustwiseSaleSumFilter").val("Y");
            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one Branch for generate the report.');
            }
            else {
                cCallbackPanel.PerformCallback(data + '~' + $("#ddlbranchHO").val());
            }
            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
        }
        function CallbackPanelEndCall(s, e) {
            document.getElementById('<%=CompBranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            Grid.Refresh();
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

        function OpenPartyDetails(partyid) {
            $("#hfIsCustwiseSaleDetFilter").val("Y");
            cCallbackPanelDetail.PerformCallback('BndPopupgrid~' + partyid);
            cpopupDetails.Show();
            return true;
        }
        function popupHide(s, e) {
            cpopupDetails.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }

        function OpenDocDetails(docid, doctype) {
            var url = '';
            if (doctype == 'POS') {
                url = '/OMS/Management/Activities/posSalesInvoice.aspx?key=' + docid + '&IsTagged=1&Viemode=1';
            }
            else if (doctype == 'SI') {
                url = '/OMS/Management/Activities/SalesInvoice.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + doctype;
            }
            else if (doctype == 'TSI') {
                url = '/OMS/Management/Activities/TSalesInvoice.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + doctype;
            }
            else if (doctype == 'SR') {
                url = '/OMS/Management/Activities/SalesReturn.aspx?key=' + docid + '&IsTagged=1&req=V&type=' + doctype;
            }
            else if (doctype == 'CR' || doctype == 'DR') {
                url = '/OMS/Management/Activities/CustomerNote.aspx?key=' + docid + '&IsTagged=1&req=V&type=CDCN';
            }

            popupdocdetails.SetContentUrl(url);
            popupdocdetails.Show();
        }

        function DetailsAfterHide(s, e) {
            popupdocdetails.Hide();
        }

        function selectAllBranch() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAllBranch() {
            gridbranchLookup.gridView.UnselectRows();
        }
        function CloseBranchLookup() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

    </script>
    <script>
        function Callback_EndCallback() {
            $("#drdExport").val(0);
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
            <div class="col-md-2 branch-selection-box">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"></asp:Label></label>
                <asp:ListBox ID="ListBoxBranches" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="60px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                <asp:HiddenField ID="hdnActivityType" runat="server" />

                <dxe:ASPxCallbackPanel runat="server" ID="BranchPanel" ClientInstanceName="cBranchPanel" OnCallback="Componentbranch_Callback">
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
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllBranch" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllBranch" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseBranchLookup" UseSubmitBehavior="False" />
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

            <%--<div class="clear"></div>--%>
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
                       
            <div class="col-md-2">
                <div style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Customer : " CssClass="mylabel1" Width="110px"></asp:Label>
                </div>                                   
                <div>
                 <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
                </div>
            </div>

             <div class="clear"></div>
            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold; padding-top: 20px" class="clsTo">
                    <asp:CheckBox ID="chkDrCrNote" runat="server" Checked="true" Text="Include Debit/Credit Note"/>
                </div>
            </div>

            <div class="col-md-2  pb-5">
                <label style="margin-bottom: 0">&nbsp</label>
                    <div class="">
                        <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">XLSX</asp:ListItem>
                            <asp:ListItem Value="2">PDF</asp:ListItem>
                            <asp:ListItem Value="3">CSV</asp:ListItem>
                            <asp:ListItem Value="4">RTF</asp:ListItem>
                        </asp:DropDownList>
                    </div>
            </div>
            <div class="clear"></div>
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2">
                    <div>
                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback" 
                             DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight ="300">
                            <Columns>
                                <dxe:GridViewDataTextColumn FieldName="SEQ" Caption="Sl. No." Width="50px" VisibleIndex="1" FixedStyle="Left">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                     <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PARTYNAME" Width="250px" VisibleIndex="2" Caption="Customer Name" FixedStyle="Left">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains"/>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenPartyDetails('<%#Eval("PARTYID") %>')" class="pad">
                                            <%#Eval("PARTYNAME")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="PARTYADDRESS" Width="250px" Caption="Office Address" VisibleIndex="3" >
                                    <Settings AutoFilterCondition="Contains" />
                                 </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="PHNO" Width="100px" Caption="Mobile/Ph. No." VisibleIndex="4" >
                                    <Settings AutoFilterCondition="Contains" />
                                 </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="PARTYPAN" Width="100px" Caption="PAN" VisibleIndex="5" >
                                    <Settings AutoFilterCondition="Contains" />
                                 </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="CNT_GSTIN" Width="130px" Caption="GSTIN" VisibleIndex="6" >
                                    <Settings AutoFilterCondition="Contains" />
                                 </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TAXABLEAMT" Width="120px" Caption="Net Sale Value" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IGSTAMT" Width="120px" Caption="IGST" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CGSTAMT" Width="120px" Caption="CGST" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="SGSTAMT" Width="120px" Caption="SGST" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OTHER_TAX" Width="120px" Caption="Chrg After GST" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TOTAL" Width="120px" Caption="Total Invoice Value" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="TAXABLEAMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IGSTAMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CGSTAMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="SGSTAMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OTHER_TAX" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="TOTAL" SummaryType="Sum" />
                            </TotalSummary>
                        </dxe:ASPxGridView>
                            <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PARTYWISESALESPURCHASE_REPORT" ></dx:LinqServerModeDataSource>

                    </div>
                </td>
            </tr>
        </table>
    </div>

    <div>
    </div>

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxPopupControl ID="popupDetails" runat="server" ClientInstanceName="cpopupDetails"
        Width="1200px" Height="600px" ScrollBars="Vertical" HeaderText="Customer Details" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="TopSides" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <asp:DropDownList ID="ddldetails" runat="server" CssClass="btn btn-sm btn-primary"
                    AutoPostBack="true" OnSelectedIndexChanged="cmbExport1_SelectedIndexChanged">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">XLSX</asp:ListItem>
                    <asp:ListItem Value="2">PDF</asp:ListItem>
                    <asp:ListItem Value="3">CSV</asp:ListItem>
                    <asp:ListItem Value="4">RTF</asp:ListItem>

                </asp:DropDownList>

                <div class="row">
                    <div>
                    </div>

                    <div class="col-md-12">
                        <dxe:ASPxGridView ID="ShowGridDet" runat="server" AutoGenerateColumns="False" Width="100%" ClientInstanceName="cShowGridDet" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerDetailsModeDataSource"  OnSummaryDisplayText="ShowGridDet_SummaryDisplayText" 
                            Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight ="300">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BRANCHDESC" Width="150px" VisibleIndex="1" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Customer Name" Width="200px" FieldName="PARTYNAME" VisibleIndex="2" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Salesman Name" Width="200px" FieldName="SALESMAN_NAME" VisibleIndex="3" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Document No" FieldName="DOCNO" VisibleIndex="4" Width="120px">
                                   <CellStyle HorizontalAlign="Center">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>

                                        <a href="javascript:void(0)" onclick="OpenDocDetails('<%#Eval("DOCID") %>','<%#Eval("DOCTYPE") %>')">
                                            <%#Eval("DOCNO")%>
                                        </a>
                                    </DataItemTemplate>

                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn Caption="Document Date" FieldName="DOCDATE" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" VisibleIndex="5" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>                                

                                <dxe:GridViewDataTextColumn Caption="Document Type" FieldName="TRAN_TYPE" VisibleIndex="6" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TAXABLEAMT" Width="100px" Caption="Net Sale Value" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="IGSTAMT" Width="100px" Caption="IGST" VisibleIndex="8" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CGSTAMT" Width="100px" Caption="CGST" VisibleIndex="9" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="SGSTAMT" Width="100px" Caption="SGST" VisibleIndex="10" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="OTHER_TAX" Width="100px" Caption="Chrg After GST" VisibleIndex="11" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="TOTAL" Width="100px" Caption="Total Invoice Value" VisibleIndex="12" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;">
                                     <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dxe:GridViewDataTextColumn>

                            </Columns>

                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" ColumnResizeMode="Control" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>

                            <TotalSummary>
                               <dxe:ASPxSummaryItem FieldName="TAXABLEAMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="IGSTAMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="CGSTAMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="SGSTAMT" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="OTHER_TAX" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="TOTAL" SummaryType="Sum" />
                            </TotalSummary>

                        </dxe:ASPxGridView>
                          <dx:LinqServerModeDataSource ID="GenerateEntityServerDetailsModeDataSource" runat="server" OnSelecting="GenerateEntityServerDetailsModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PARTYWISESALESPURCHASE_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                    <div class="clear"></div>

                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="popupHide" />

    </dxe:ASPxPopupControl>


    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdocdetails" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>

        <ClientSideEvents CloseUp="DetailsAfterHide" />
    </dxe:ASPxPopupControl>
    
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

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsCustwiseSaleSumFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
          <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelDetail" ClientInstanceName="cCallbackPanelDetail" OnCallback="CallbackPanelDetail_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            <asp:HiddenField ID="hfIsCustwiseSaleDetFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
          <ClientSideEvents EndCallback="CallbackPanelDetEndCall" />
    </dxe:ASPxCallbackPanel>

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>
