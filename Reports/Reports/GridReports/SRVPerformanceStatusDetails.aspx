<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SRVPerformanceStatusDetails.aspx.cs" Inherits="Reports.Reports.GridReports.SRVPerformanceStatusDetails" %>

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

          ctxtTechName.SetEnabled(false);
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
            $("#drdExport").val(0);
            $("#hfIsSrvPerformanceStatDetFilter").val("Y");
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
        
        function selectAll() {
            gridbranchLookup.gridView.SelectRows();
        }
        function unselectAll() {
            gridbranchLookup.gridView.UnselectRows();
        }

        function CloseGridBranchLookup() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }

        function Callback_EndCallback() {
            $("#drdExport").val(0);
        }

        function CheckIsBranchWise(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CchkIsTechnicianWise.SetCheckState('UnChecked');
                ctxtTechName.SetEnabled(false);
                ctxtTechName.SetText('');
                GetObjectID('hdnTechnicianId').value = '';
            }
            else {
                CchkIsTechnicianWise.SetEnabled(true);
                ctxtTechName.SetEnabled(true);
            }
        }

        function CheckIsTechnicianWise(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CchkIsBranchWise.SetCheckState('UnChecked');
                ctxtTechName.SetEnabled(true);
            }
            else {
                CchkIsBranchWise.SetEnabled(true);
                ctxtTechName.SetEnabled(false);
                ctxtTechName.SetText('');
                GetObjectID('hdnTechnicianId').value = '';
            }
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
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" UseSubmitBehavior="False"/>                                                        
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

                <span id="MandatoryActivityType" style="display: none" class="validclass">
                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
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

            <div class="col-md-2" style="margin-top:17px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkIsBranchWise" Checked="true" Text="Branch wise Performance" ClientInstanceName="CchkIsBranchWise">
                    <ClientSideEvents CheckedChanged="CheckIsBranchWise" />
                </dxe:ASPxCheckBox>
            </div>

            <div class="col-md-2" style="margin-top:17px;color: #b5285f; font-weight: bold;">
                <dxe:ASPxCheckBox runat="server" ID="chkIsTechnicianWise" Checked="false" Text="Technician wise Performance" ClientInstanceName="CchkIsTechnicianWise">
                    <ClientSideEvents CheckedChanged="CheckIsTechnicianWise" />
                </dxe:ASPxCheckBox>
            </div>
           
            <div class="clear"></div>
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
                        
            <div class="col-md-2" style="padding:0;padding-top: 12px;">
            <table>
                <tr>
                    <td  style="padding-left:15px;">
                            <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <% if (rights.CanExport)
                    { %> 
                         <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                             <asp:ListItem Value="1">EXCEL</asp:ListItem>
                             <asp:ListItem Value="2">PDF</asp:ListItem>
                             <asp:ListItem Value="3">CSV</asp:ListItem>
                        </asp:DropDownList>
                    <% } %>
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
                            DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Visible"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback"
                            Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BRANCHDESC" Width="220px" VisibleIndex="1" HeaderStyle-CssClass="colDisable">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Technician" Width="250px" FieldName="PARTY" VisibleIndex="2" HeaderStyle-CssClass="colDisable">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CHALLAN_NO" Width="130px" Caption="Challan No." VisibleIndex="3" HeaderStyle-CssClass="colDisable" >
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Received Date" Width="100px" FieldName="RCVDT" VisibleIndex="4" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Received Time" Width="100px" FieldName="RCVTIME" VisibleIndex="5" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Serviced Date" Width="100px" FieldName="SRVDT" VisibleIndex="6" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Serviced Time" Width="100px" FieldName="SRVTIME" VisibleIndex="7" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Time Taken (< 1) Hour" Width="140px" FieldName="TAKEMINUTELESS1HR" VisibleIndex="8" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Time Taken (>= 1) Hour" Width="140px" FieldName="TAKEMINUTEGREATER1HR" VisibleIndex="9" HeaderStyle-CssClass="colDisable">
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Days Taken" Width="100px" FieldName="DAYSTAKEN" VisibleIndex="10" HeaderStyle-CssClass="colDisable">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <CellStyle HorizontalAlign="Right"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                
                            </Columns>
                            <SettingsDataSecurity AllowDelete="false" AllowEdit="false" AllowInsert="false" />
                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" AllowSort="False"/>
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />
                        </dxe:ASPxGridView>
                    <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                            ContextTypeName="ReportSourceDataContext" TableName="SRVPERFORMANCESTATDET_REPORT"></dx:LinqServerModeDataSource>

            <asp:HiddenField ID="hfIsSrvPerformanceStatDetFilter" runat="server" />
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

    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
</asp:Content>