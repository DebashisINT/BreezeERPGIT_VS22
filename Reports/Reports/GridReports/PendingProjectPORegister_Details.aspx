<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PendingProjectPORegister_Details.aspx.cs" Inherits="Reports.Reports.GridReports.PendingProjectPORegister_Details" %>

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

        #ListBoxProjects{
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
            if (ArrName == 'VendorSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#VendModel').modal('hide');
                    ctxtVendName.SetText(Name);
                    GetObjectID('hdnVendorId').value = key;
                }
                else {
                    ctxtVendName.SetText('');
                    GetObjectID('hdnVendorId').value = '';
                }
            }

        }

    </script>
   <%-- For multiselection when click on ok button--%>


   <%-- For multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#VendModel').on('shown.bs.modal', function () {
                 $('#txtVendSearch').focus();
             })
         })
         var VendArr = new Array();
         $(document).ready(function () {
             var VendObj = new Object();
             VendObj.Name = "VendorSource";
             VendObj.ArraySource = VendArr;
             arrMultiPopup.push(VendObj);
         })
         function VendorButnClick(s, e) {
             $('#VendModel').modal('show');
         }

         function Vendor_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#VendModel').modal('show');
             }
         }

         function Vendorkeydown(e) {
             var OtherDetails = {}

             if ($.trim($("#txtVendSearch").val()) == "" || $.trim($("#txtVendSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtVendSearch").val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {

                 var HeaderCaption = [];
                 HeaderCaption.push("Vendor Name");

                 if ($("#txtVendSearch").val() != "") {
                     callonServerM("Services/Master.asmx/GetOnlyVendor", OtherDetails, "VendorTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "VendorSource");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }


         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtVendSearch').focus();
             else
                 $('#txtVendSearch').focus();
         }
   </script>
      <%-- For multiselection--%>

    <script type="text/javascript">
        $(function () {
            cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            function OnWaitingGridKeyPress(e) {
                if (e.code == "Enter") {
                }
            }
        });

        $(function () {
            cProjectPanel.PerformCallback('BindProjectGrid');
        });

        $(document).ready(function () {
            $("#ListBoxBranches").chosen().change(function () {
                var Ids = $(this).val();
                $('#<%=hdnSelectedBranch.ClientID %>').val(Ids);
            })

            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            if (ProjectSelection == "0") {
                $('#divProj').addClass('hidden');
            }
            else {
                $('#divProj').removeClass('hidden');
            }

            $("#ddlbranchHO").change(function () {
                var Ids = $(this).val();
                $("#hdnSelectedBranch").val('');
                cProductbranchPanel.PerformCallback('BindComponentGrid' + '~' + $("#ddlbranchHO").val());
            })
        })
        function CloseGridLookupbranch() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }
        function selectAll_Branch() {
            gridbranchLookup.gridView.SelectRows();
        }

        function unselectAll_Branch() {
            gridbranchLookup.gridView.UnselectRows();
        }

        function selectAll_Project() {
            gridprojectLookup.gridView.SelectRows();
        }
        function unselectAll_Project() {
            gridprojectLookup.gridView.UnselectRows();
        }
        function CloseGridProjectLookup() {
            gridprojectLookup.ConfirmCurrentSelection();
            gridprojectLookup.HideDropDown();
            gridprojectLookup.Focus();
        }

        document.onkeyup = function (e) {
            if (event.keyCode == 27 && popupdetails.GetVisible() == true) { 
                popupHide();
            }
        }
        function popupHide(s, e) {
            popupdetails.Hide();
            Grid.Focus();
            $("#drdExport").val(0);
        }

        function fn_OpenDetails(keyValue) {
            Grid.PerformCallback('Edit~' + keyValue);
        }

        function popupPendSalesAfterHide(s, e) {
            popupPendSales.Hide();
        }
    </script>


    <script type="text/javascript">
        function btn_ShowRecordsClick(e) {
            var BranchSelection = document.getElementById('hdnBranchSelection').value;
            var ProjectSelection = document.getElementById('hdnProjectSelection').value;
            var ProjectSelectionInReport = document.getElementById('hdnProjectSelectionInReport').value;
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsPendProjectPORegDetFilter").val("Y");
            if (BranchSelection == "Yes" && gridbranchLookup.GetValue() == null) {
                jAlert('Please select atleast one branch for generate the report.');
            }
            else if (ProjectSelection == "1") {
                if (ProjectSelectionInReport == "Yes" && gridprojectLookup.GetValue() == null) {
                    jAlert('Please select atleast one Project for generate the report.');
                }
                else {
                    cCallbackPanel.PerformCallback();
                }
            }
            else {
                cCallbackPanel.PerformCallback();
            }
            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=Daterange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
        }

        function GetDateFormat(today) {
            if (today != "") {
                var dd = today.getDate();
                var mm = today.getMonth() + 1; 

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
            document.getElementById('<%=Compbranch.ClientID %>').innerHTML = cCallbackPanel.cpBranchNames
            GridList.Refresh();
        }

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

        function OpenPendPurchaseOrdDetails(invoice, type) {
            if (type == 'PPO') {
                url = '/OMS/Management/Activities/ProjectPurchaseOrder.aspx?key=' + invoice + '&IsTagged=1&req=V&type=PO';
            }
            popupdetails.SetContentUrl(url);
            popupdetails.Show();
        }

        function DetailsAfterHide(s, e) {
            popupdetails.Hide();
        }

        //Rev Debashis
        function CheckZeroBal(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CchkCloseOrder.SetCheckState('UnChecked');
            }
            else {
                CchkCloseOrder.SetEnabled(true);
            }
        }

        function CheckCloseOrder(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CchkZeroBal.SetCheckState('UnChecked');
            }
            else {
                CchkZeroBal.SetEnabled(true);
            }
        }
        //End of Rev Debashis
    </script>
    <script>
        function CallbackListofMaster_BeginCallback() {
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
                GridList.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                GridList.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    GridList.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    GridList.SetWidth(cntWidth);
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
                        <asp:Label ID="Compbranch" runat="Server" Text="" Width="470px" ></asp:Label>
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
                        <asp:Label ID="Daterange" runat="Server" Text="" Width="470px" ></asp:Label>
                    </div>
              </div>
            </div>
          </div>
        </div>

        <div>
            
        </div>
        
    </div>

    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row">
             <div class="col-md-2">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label3" runat="Server" Text="Head Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>
                    <asp:DropDownList ID="ddlbranchHO" runat="server" Width="100%"></asp:DropDownList>
                </div>
            </div>
             <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="Label1" runat="Server" Text="Branch : " CssClass="mylabel1"
                            Width="92px"></asp:Label>
                    </div>
                <div>

                    <asp:HiddenField ID="hdnActivityType" runat="server" />
                    <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                    <asp:HiddenField ID="hdnSelectedBranch" runat="server" />
                    <dxe:ASPxCallbackPanel runat="server" ID="ASPxCallbackPanel1" ClientInstanceName="cProductbranchPanel" OnCallback="Componentbranch_Callback">
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
                                                                <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Branch" UseSubmitBehavior="False"/>
                                                            <%--</div>--%>
                                                            <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Branch" UseSubmitBehavior="False"/>                                                            
                                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookupbranch" UseSubmitBehavior="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                                        <SettingsPager Mode="ShowPager">
                                        </SettingsPager>

                                        <SettingsPager PageSize="20">
                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100" />
                                        </SettingsPager>

                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>
                               
                                </dxe:ASPxGridLookup>
                            </dxe:PanelContent>
                        </PanelCollection>

                    </dxe:ASPxCallbackPanel>

                </div>
            </div>


            <div class="col-md-2 col-lg-2">
            <dxe:ASPxLabel ID="lbl_Vendor" style="color: #b5285f;font-weight: bold;" runat="server" Text="Vendor:">
                </dxe:ASPxLabel>                   
                <dxe:ASPxButtonEdit ID="txtVendName" runat="server" ReadOnly="true" ClientInstanceName="ctxtVendName" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){Vendor_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </div>

            <div class="col-md-2">
                <div style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                </div>
                <div>
                    <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                        UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
            </div>
            <div class="col-md-2">
                    <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                        <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"></asp:Label>
                    </div>              
                    <div>
                        <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                            UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>

                        </dxe:ASPxDateEdit>
                    </div>
            </div>
            <%--<div class="clear"></div>--%>
             <div class="col-md-2" style="padding-top:20px">
                <%--<div id="ckpar" style="padding-left: 5px">
                    <asp:CheckBox runat="server" ID="chkZeroBal" style="color: #b5285f" Text="Show Zero Balance" />
                </div>--%>
                 <dxe:ASPxCheckBox runat="server" ID="chkZeroBal" Checked="false" Text="Show Zero Balance" ClientInstanceName="CchkZeroBal">
                    <ClientSideEvents CheckedChanged="CheckZeroBal" />
                </dxe:ASPxCheckBox>
            </div>
            <div class="clear"></div>

            <div class="col-md-2 " id="divProj" style="padding-bottom: 13px;">
                <div style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="Project : " CssClass="mylabel1"></asp:Label>
                </div>
                <asp:ListBox ID="ListBoxProjects" Visible="false" runat="server" SelectionMode="Multiple" Font-Size="12px" Height="60px" Width="100%" CssClass="mb0 chsnProduct  hide" data-placeholder="--- ALL ---"></asp:ListBox>
                <dxe:ASPxCallbackPanel runat="server" ID="ProjectPanel" ClientInstanceName="cProjectPanel" OnCallback="Project_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_project" SelectionMode="Multiple" runat="server" ClientInstanceName="gridprojectLookup"
                                OnDataBinding="lookup_project_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="project_code" Visible="true" VisibleIndex="1" width="200px" Caption="Project code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="project_name" Visible="true" VisibleIndex="2" width="200px" Caption="Project Name" Settings-AutoFilterCondition="Contains">
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
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll_Project" UseSubmitBehavior="False"/>
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll_Project" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridProjectLookup" UseSubmitBehavior="False" />
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
                <asp:HiddenField ID="hdnSelectedProjects" runat="server" />
            </div>

            <div class="col-md-2" style="padding-top:15px">
                <dxe:ASPxCheckBox runat="server" ID="chkCloseOrder" Checked="false" Text="Show Close Order" ClientInstanceName="CchkCloseOrder">
                    <ClientSideEvents CheckedChanged="CheckCloseOrder" />
                </dxe:ASPxCheckBox>
            </div>
             
            <div class="col-md-2" style="padding-top: 14px;">
            <table>
                <tr>                     
                    <td >
                     <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                         <% if (rights.CanExport)
                     { %> 
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                            OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">XLSX</asp:ListItem>
                            <asp:ListItem Value="2">PDF</asp:ListItem>
                            <asp:ListItem Value="3">CSV</asp:ListItem>
                            <asp:ListItem Value="4">RTF</asp:ListItem>
                        </asp:DropDownList>
                        <% } %>
                    </td>
                </tr>
            </table>
            
            </div>
            <div class="clear"></div>
           
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2">
                  
                       <div>
                           <dxe:ASPxGridView runat="server" ID="ShowGridList" ClientInstanceName="GridList" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                               Settings-HorizontalScrollBarMode="Visible" KeyFieldName="SLNO" OnSummaryDisplayText="ShowGridList_SummaryDisplayText"
                               DataSourceID="GenerateEntityServerModeDataSource" ClientSideEvents-BeginCallback="CallbackListofMaster_BeginCallback" >
                            <Columns>

                                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="BRANCH_DESCRIPTION" Width="110px" Caption="Unit" >
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ORDER_NUMBER" Width="130px" Caption="Order Number" >
                                    <CellStyle HorizontalAlign="Center"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="OpenPendPurchaseOrdDetails('<%#Eval("ORDER_ID") %>','PPO')">
                                            <%#Eval("ORDER_NUMBER")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="ORDER_DATE" Caption="Order Date" Width="100px">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="REVISION_NUMBER" Caption="Revision No." Width="100px">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="REVISION_DATE" Caption="Revision date" Width="100px">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                               
                                <dxe:GridViewDataTextColumn Caption="Vendor Name" FieldName="PARTYNAME" Width="310px" VisibleIndex="6" >
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                     <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="PROJ_NAME" Width="170px"
                                    VisibleIndex="7">
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Product Code" FieldName="SPRODUCTS_CODE" Width="200px"
                                    VisibleIndex="8" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn Caption="Description" FieldName="SPRODUCTS_NAME" Width="200px"
                                    VisibleIndex="9" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="UOM" FieldName="UOM_NAME" Width="80px"
                                    VisibleIndex="10" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Rate" FieldName="ORDDETRATE" Width="110px"
                                    VisibleIndex="11" >
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Right" />
                               </dxe:GridViewDataTextColumn>

                               <dxe:GridViewDataTextColumn Caption="Actual Quantity" FieldName="ACTUAL_QUANTITY" Width="110px"
                                    VisibleIndex="12" >
                                    <CellStyle HorizontalAlign="Right">
                                    </CellStyle>
                                   <HeaderStyle HorizontalAlign="Right" />
                               </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn Caption="Mature Quantity" FieldName="MATURE_QUANTITY" Width="110px"
                                VisibleIndex="13" >
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                             </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Balance Quantity" FieldName="BALANCE_QUANTITY" Width="110px"
                                VisibleIndex="14" >
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn Caption="Actual Values" FieldName="ACTUAL_VALUES" Width="110px"
                                VisibleIndex="15" >
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Mature Values" FieldName="MATURE_VALUES" Width="110px"
                                VisibleIndex="16" >
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                             </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Balance Values" FieldName="BALANCE_VALUES" Width="110px"
                                VisibleIndex="17" >
                                <CellStyle HorizontalAlign="Right">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                            </dxe:GridViewDataTextColumn>

                            </Columns>

                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" AllowSort="true" />
                            <Settings ShowGroupPanel="true" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                            </SettingsPager>
                            <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />
                               
                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="ACTUAL_QUANTITY" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="MATURE_QUANTITY" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="BALANCE_QUANTITY" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="ACTUAL_VALUES" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="MATURE_VALUES" SummaryType="Sum" />
                                <dxe:ASPxSummaryItem FieldName="BALANCE_VALUES" SummaryType="Sum" />
                            </TotalSummary>
                        </dxe:ASPxGridView>

                      <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="PENDING_SALEPURCHASEPROJECTORDER_REPORT" ></dx:LinqServerModeDataSource>
                    </div>
                          
             
                </td>
            </tr>
        </table>
    </div>

    <div>
    </div>

    <!--Vendor Modal -->
    <div class="modal fade" id="VendModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Vendor Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Vendorkeydown(event)" id="txtVendSearch" width="100%" placeholder="Search By Vendor Name" />
                    <div id="VendorTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Vendor Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('VendorSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('VendorSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <!--Vendor Modal -->

    
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
        <asp:HiddenField ID="hdnVendorId" runat="server" />
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
          
            <asp:HiddenField ID="hfIsPendProjectPORegDetFilter" runat="server" />
          
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>

    <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
    CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupPendSales" Height="500px"
    Width="1310px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
    <ContentCollection>
        <dxe:PopupControlContentControl runat="server">
        </dxe:PopupControlContentControl>
    </ContentCollection>

    <ClientSideEvents CloseUp="popupPendSalesAfterHide" />
    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="ASPXPopupControl1" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdetails" Height="500px"
        Width="1200px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="DetailsAfterHide" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField ID="hdnBranchSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelection" runat="server" />
    <asp:HiddenField ID="hdnProjectSelectionInReport" runat="server" />
</asp:Content>

