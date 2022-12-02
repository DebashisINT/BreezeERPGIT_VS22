<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SRVMaterialInOutRegisterSummary.aspx.cs" Inherits="Reports.Reports.GridReports.SRVMaterialInOutRegisterSummary" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

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

        #ListBoxWarehouse {
            width: 200px;
        }

        .hide {
            display: none;
        }

        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
    </style>

    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">

        $(function () {
            cWarehouseComponentPanel.PerformCallback('BindWarehouseGrid');
        });

        function SetSelectedValues(Id, Name, ArrName) {
            if (ArrName == 'TechnicianSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#TechModel').modal('hide');
                    ctxtTechName.SetText(Name);
                    GetObjectID('hdnTechnicianId').value = key;
                }
                else {
                    ctxtTechName.SetText('');
                    GetObjectID('hdnTechnicianId').value = '';
                }
            }
            else if (ArrName == 'ProductSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#ProdModel').modal('hide');
                    ctxtProdName.SetText(Name);
                    GetObjectID('hdnProductId').value = key;
                }
                else {
                    ctxtProdName.SetText('');
                    GetObjectID('hdnProductId').value = '';
                }
            }
        }
  </script>
  <%-- For multiselection when click on ok button--%>
  <%-- For Technician multiselection--%>
  <script type="text/javascript">
      $(document).ready(function () {
          $('#TechModel').on('shown.bs.modal', function () {
              $('#txtTechSearch').focus();
          })
      })
      var TechArr = new Array();
      $(document).ready(function () {
          var TechObj = new Object();
          TechObj.Name = "TechnicianSource";
          TechObj.ArraySource = TechArr;
          arrMultiPopup.push(TechObj);
      })
      function TechnicianButnClick(s, e) {
          $('#TechModel').modal('show');
      }

      function Technician_KeyDown(s, e) {
          if (e.htmlEvent.key == "Enter") {
              $('#TechModel').modal('show');
          }
      }

      function Techniciankeydown(e) {
          var OtherDetails = {}

          if ($.trim($("#txtTechSearch").val()) == "" || $.trim($("#txtTechSearch").val()) == null) {
              return false;
          }
          OtherDetails.SearchKey = $("#txtTechSearch").val();

          if (e.code == "Enter" || e.code == "NumpadEnter") {

              var HeaderCaption = [];
              HeaderCaption.push("Technician Name");
              HeaderCaption.push("Unique ID");
              HeaderCaption.push("Alternate No.");
              HeaderCaption.push("Address");

              if ($("#txtTechSearch").val() != "") {
                  callonServerM("Services/Master.asmx/GetTechnician", OtherDetails, "TechnicianTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "TechnicianSource");
              }
          }
          else if (e.code == "ArrowDown") {
              if ($("input[dPropertyIndex=0]"))
                  $("input[dPropertyIndex=0]").focus();
          }
      }

      function SetfocusOnseach(indexName) {
          if (indexName == "dPropertyIndex")
              $('#txtTechSearch').focus();
          else
              $('#txtTechSearch').focus();
      }
   </script>
   <%-- For Technician multiselection--%>

    <%-- For Product multiselection--%> 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#ProdModel').on('shown.bs.modal', function () {
                 $('#txtProdSearch').focus();
             })
         })
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

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Code");
                 HeaderCaption.push("Name");
                 HeaderCaption.push("Hsn");

                 if ($("#txtProdSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetComponentProduct", OtherDetails, "ProductTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "ProductSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }

         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtProdSearch').focus();
             else
                 $('#txtProdSearch').focus();
         }
   </script>
   <%-- For Product multiselection--%>

    <script type="text/javascript">

        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }

        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsSrvMatIORegSumFilter").val("Y");
            cCallbackPanel.PerformCallback(data);
            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
        }

        function CallbackPanelEndCall(s, e) {
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

        $(document).ready(function () {
            $("#ListBoxWarehouse").chosen().change(function () {
                var Ids = $(this).val();

                $('#<%=hdnSelectedWarehouse.ClientID %>').val(Ids);
                $('#MandatoryActivityType').attr('style', 'display:none');

            })
        });

        function selectAllWH() {
            gridwarehouseLookup.gridView.SelectRows();
        }
        function unselectAllWH() {
            gridwarehouseLookup.gridView.UnselectRows();
        }

        function CloseLookupwarehouse() {
            gridwarehouseLookup.ConfirmCurrentSelection();
            gridwarehouseLookup.HideDropDown();
            gridwarehouseLookup.Focus();
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
        <div class="row">
            <div class="col-md-2">
                <label style="color: #b5285f;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f;" class="clsTo">
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
                 <label style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="Warehouse : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <div>
                    <asp:ListBox ID="ListBoxWarehouse" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                    <span id="MandatoryActivityType" style="display: none" class="validclass">
                        <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                    <asp:HiddenField ID="hdnSelectedWarehouse" runat="server" />
                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel2" ClientInstanceName="cWarehouseComponentPanel" OnCallback="Componentwarehouse_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="lookup_warehouse" SelectionMode="Multiple" runat="server" ClientInstanceName="gridwarehouseLookup"
                                    OnDataBinding="lookup_warehouse_DataBinding"
                                    KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="Description" Visible="true" VisibleIndex="1" width="200px" Caption="Warehouse Name" Settings-AutoFilterCondition="Contains">
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
                                                                <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllWH"  UseSubmitBehavior="False" />
                                                            <%--</div>--%>
                                                            <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllWH"  UseSubmitBehavior="False" />
                                                            <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLookupwarehouse" UseSubmitBehavior="False" />
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
                <label style="color: #b5285f;" class="clsTo">
                    <asp:Label ID="Label4" runat="Server" Text="Technician : " CssClass="mylabel1" Width="110px"></asp:Label>
                </label>                                   
                <div>
                 <dxe:ASPxButtonEdit ID="txtTechName" runat="server" ReadOnly="true" ClientInstanceName="ctxtTechName" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){TechnicianButnClick();}" KeyDown="function(s,e){Technician_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
                </div>
            </div>

            <div class="col-md-2">
                 <label style="color: #b5285f;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Product : " CssClass="mylabel1"
                            Width="110px"></asp:Label>
                    </label>
                <div>
                    <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%" TabIndex="6">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){Product_KeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                </div>
            </div>
            <%--<div class="clear"></div>--%>
            <div class="col-md-2" style="padding:0;padding-top: 17px;">
            <table>
                <tr>
                    <td  style="padding-left:15px;">
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">XLSX</asp:ListItem>
                        <asp:ListItem Value="2">PDF</asp:ListItem>
                        <asp:ListItem Value="3">CSV</asp:ListItem>
                        <asp:ListItem Value="4">RTF</asp:ListItem>
                    </asp:DropDownList>
                    </td>
                </tr>
            </table>
                
            </div>
            <div class="clear"></div>
            
            
            <div class="col-md-2">
                <label style="margin-bottom: 0">&nbsp</label>
                <div class="">
                </div>
            </div>
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2"> 
                    <div>
                       
                    </div>
                            
                </td>
            </tr>
        </table>
    </div>

    <div>
    </div>

     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">

                 <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                    DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Visible" OnHtmlDataCellPrepared="ShowGrid_HtmlDataCellPrepared"
                    OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback" OnHtmlFooterCellPrepared="ShowGrid_HtmlFooterCellPrepared"
                    Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                    <Columns>
                        <dxe:GridViewDataTextColumn FieldName="WHDESC" Width="250px" Caption="Warehouse" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Technician" Width="250px" FieldName="PARTY" VisibleIndex="2" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Components" Width="300px" FieldName="PRODDESC" VisibleIndex="3" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                               
                        <dxe:GridViewDataTextColumn FieldName="OP_QTY" Caption="Opening Qty." Width="130px" VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="IN_QTY" Caption="Received Qty." Width="130px" VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="OUT_QTY" Caption="Issued Qty" Width="130px" VisibleIndex="6" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="BAL_QTY" Caption="Balance Qty" Width="130px" VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                    <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                    <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                    <SettingsEditing Mode="EditForm" />
                    <SettingsContextMenu Enabled="true" />
                    <SettingsBehavior AutoExpandAllGroups="true" AllowSort="False" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                    <SettingsSearchPanel Visible="false" />
                    <SettingsPager PageSize="10">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                    </SettingsPager>
                    <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />

                    <TotalSummary>
                        <dxe:ASPxSummaryItem FieldName="PRODDESC" SummaryType="Custom" Tag="Sum_ProdDesc"/>
                        <dxe:ASPxSummaryItem FieldName="OP_QTY" SummaryType="Custom" Tag="Sum_OpQty"/>
                        <dxe:ASPxSummaryItem FieldName="IN_QTY" SummaryType="Custom" Tag="Sum_InQty"/>
                        <dxe:ASPxSummaryItem FieldName="OUT_QTY" SummaryType="Custom" Tag="Sum_OutQty"/>
                        <dxe:ASPxSummaryItem FieldName="BAL_QTY" SummaryType="Custom" Tag="Sum_BalQty"/>
                    </TotalSummary>
                 </dxe:ASPxGridView>
                 <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                     ContextTypeName="ReportSourceDataContext" TableName="SRVMATERIALIOREGISTER_REPORT"></dx:LinqServerModeDataSource>

            <asp:HiddenField ID="hfIsSrvMatIORegSumFilter" runat="server" />
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <!--Technician Modal -->
    <div class="modal fade" id="TechModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Technician Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Techniciankeydown(event)" id="txtTechSearch" width="100%" placeholder="Search By Technician Name or Unique ID" />
                    <div id="TechnicianTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Technician Name</th>
                                 <th>Unique ID</th>
                                <th>Alternate No.</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('TechnicianSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('TechnicianSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnTechnicianId" runat="server" />
    <!--Technician Modal -->

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

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Code</th>
                                 <th>Name</th>
                                <th>HSN</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('ProductSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ProductSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnProductId" runat="server" />    
    <!--Product Modal -->

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
</asp:Content>
