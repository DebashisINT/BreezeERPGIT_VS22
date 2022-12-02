<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SRVTechnicianProductivity.aspx.cs" Inherits="Reports.Reports.GridReports.SRVTechnicianProductivity" %>

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
       
        .hide {
            display: none;
        }

        .cPoint{
            cursor:pointer;
        }

        .cPoint1{
            cursor:pointer;
        }
        .dxtc-activeTab .dxtc-link {
            color: #fff !important;
        }
    </style>

    <%-- For multiselection when click on ok button--%>
    <script type="text/javascript">
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

    <script type="text/javascript">
        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            $("#hfIsSrvTechProdFilter").val("Y");
            $("#drdExport").val(0);
            cCallbackPanel.PerformCallback();
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
    </script>
    <script>
        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }
        $(function () {
            $('[data-toggle="popover"]').popover({
                trigger: "hover"
            })
        })
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
            <div class="col-md-2" style="padding:0;padding-top: 17px;">
            <table>
                <tr>
                    <td  style="padding-left:15px;">
                    <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">EXCEL</asp:ListItem>
                        <asp:ListItem Value="2">PDF</asp:ListItem>
                        <asp:ListItem Value="3">CSV</asp:ListItem>
                    </asp:DropDownList>
                    </td>
                </tr>
            </table>                
            </div>
            <div class="clear"></div>
            <div class="col-md-2 pTop5">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <label>Daily No. of Box Checked:<span class="cPoint" data-container="body" data-toggle="popover" data-placement="right" data-content="LEVEL A+>35.00;LEVEL A<=35.00;LEVEL B<=25.00;LEVEL C<20.00"><i class="fa fa-question-circle" aria-hidden="true"></i></span></label>
                </div>
            </div>
            <div class="col-md-4 pTop5">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <label>Performance Grade by Daily No. of Box Repaired:<span class="cPoint1" data-container="body" data-toggle="popover" data-placement="right" data-content="LEVEL A+>20.00;LEVEL A<=20.00;LEVEL B<=15.00;LEVEL C<10.00"><i class="fa fa-question-circle" aria-hidden="true"></i></span></label>
                </div>
            </div> 
            <div class="clear"></div>
            <div class="pTop5"></div>
        </div>
    </div>

    <div>
    </div>

     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">

                 <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                    DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Visible" OnHtmlDataCellPrepared="ShowGrid_HtmlDataCellPrepared"
                    OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback"
                    Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                    <Columns>
                        <dxe:GridViewDataTextColumn Caption="Repaired By" Width="200px" FieldName="PARTYNAME" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>
                               
                        <dxe:GridViewDataTextColumn FieldName="TOTALRUNNINGSTBALLOCATED" Caption="Total Running STB Allocated" Width="180px" VisibleIndex="2" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                            <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="TOTALRUNNINGSTBREPAIRED" Caption="Total Running STB Repaired" Width="180px" VisibleIndex="3" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                            <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="TOTALBACKLOGSTBALLOCATED" Caption="Total Backlog STB Allocated" Width="180px" VisibleIndex="4" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                            <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="TOTALBACKLOGSTBREPAIRED" Caption="Total Backlog STB Repaired" Width="180px" VisibleIndex="5" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                            <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="OVERALLSTBALLOCATED" Caption="Overall STB Allocated" Width="140px" VisibleIndex="6" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                            <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="OVERALLREPAIRED" Caption="Overall Repaired" Width="100px" VisibleIndex="7" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                            <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="PERCNTOFREP" Caption="% of Repairing" Width="100px" VisibleIndex="8" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                            <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="WORKINGDAYS" Caption="WorkIng Days" Width="100px" VisibleIndex="9" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                            <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="DAILYAVGBOXCHECKED" Caption="Daily Avg. Box Checked" Width="150px" VisibleIndex="10" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                            <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Box Checking Grade" Width="200px" FieldName="BOXCHECKINGGRADE" VisibleIndex="11" HeaderStyle-CssClass="colDisable">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="DAILYAVGBOXREPAIRED" Caption="Daily Avg. Box Repaired" Width="150px" VisibleIndex="12" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                            <HeaderStyle HorizontalAlign="Right" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Performance Grade" Width="200px" FieldName="PERFORMANCEGRADE" VisibleIndex="13" HeaderStyle-CssClass="colDisable">
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
                        <dxe:ASPxSummaryItem FieldName="PARTYNAME" SummaryType="Custom" Tag="TagParty"/>
                        <dxe:ASPxSummaryItem FieldName="TOTALRUNNINGSTBALLOCATED" SummaryType="Custom" Tag="TagTRSA"/>
                        <dxe:ASPxSummaryItem FieldName="TOTALRUNNINGSTBREPAIRED" SummaryType="Custom" Tag="TagTRSR"/>
                        <dxe:ASPxSummaryItem FieldName="TOTALBACKLOGSTBALLOCATED" SummaryType="Custom" Tag="TagTBSA"/>
                        <dxe:ASPxSummaryItem FieldName="TOTALBACKLOGSTBREPAIRED" SummaryType="Custom" Tag="TagTBSR"/>
                        <dxe:ASPxSummaryItem FieldName="OVERALLSTBALLOCATED" SummaryType="Custom" Tag="TagOA"/>
                        <dxe:ASPxSummaryItem FieldName="OVERALLREPAIRED" SummaryType="Custom" Tag="TagOR"/>
                        <dxe:ASPxSummaryItem FieldName="WORKINGDAYS" SummaryType="Custom" Tag="TagWD"/>
                    </TotalSummary>
                 </dxe:ASPxGridView>
                 <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                     ContextTypeName="ReportSourceDataContext" TableName="TECHNICIANPRODUCTIVITY_REPORT"></dx:LinqServerModeDataSource>

            <asp:HiddenField ID="hfIsSrvTechProdFilter" runat="server" />
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

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
</asp:Content>