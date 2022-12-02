<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ListingOfMaster_Vendor.aspx.cs" Inherits="Reports.Reports.GridReports.ListingOfMaster_Vendor" %>

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
            bottom: 80px;
            background: #fff;
            box-shadow: 1px 1px 10px #0000001c;
            right: 50%;
        }
        /*rev end Pallab*/
    </style>
    
   


    <script type="text/javascript">
        //for Esc
        document.onkeyup = function (e) {
            if (event.keyCode == 27 && popupdetails.GetVisible() == true) { //run code for alt+N -- ie, Save & New  
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

      
    </script>
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
                     callonServerM("Services/Master.asmx/GetVendor", OtherDetails, "VendorTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "VendorSource");
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
        function btn_ShowRecordsClick(e) {
            e.preventDefault;
            var data = "OnDateChanged";
            $("#hfIsListMastFilter").val("Y");

            cCallbackPanel.PerformCallback();
        }

        function CallbackPanelEndCall(s, e) {
            GridList.Refresh();
        }

        function OnContextMenuItemClick(sender, args) {
            if (args.item.name == "ExportToPDF" || args.item.name == "ExportToXLS") {
                args.processOnServer = true;
                args.usePostBack = true;
            } else if (args.item.name == "SumSelected")
                args.processOnServer = true;
        }

    </script>
    <script>
        function CallbackListofMaster_BeginCallback() {
            $("#drdExport").val(0);
        }
    </script>
     <script type="text/javascript">
         function ClickVIewInfo(keyValue) {
             var url = '/OMS/management/master/View/ViewVendor.html?id=' + keyValue;
             CAspxDirectVendorViewPopup.SetWidth(window.screen.width-50);
             CAspxDirectVendorViewPopup.SetHeight(window.innerHeight - 70);
             CAspxDirectVendorViewPopup.SetContentUrl(url);
             //AspxDirectAddCustPopup.ClearVerticalAlignedElementsCache();

             CAspxDirectVendorViewPopup.RefreshContentUrl();
             CAspxDirectVendorViewPopup.Show();
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

        <div>
            
        </div>
        
    </div>

    <div class="form_main">
        <asp:HiddenField runat="server" ID="hdndaily" />
        <asp:HiddenField runat="server" ID="hdtid" />
        <div class="row">
            <div class="col-md-4 col-lg-2">
             <dxe:ASPxLabel ID="lbl_Vendor" style="color: #b5285f;" runat="server" Text="Vendor:">
                </dxe:ASPxLabel>                   
                <dxe:ASPxButtonEdit ID="txtVendName" runat="server" ReadOnly="true" ClientInstanceName="ctxtVendName" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){Vendor_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </div>

            
            <div class="col-md-6" style="padding-top: 15px;">
            <table>
                <tr>
                  <td style="padding-right:20px">
                        <asp:CheckBox ID="chkshowdormant" runat="server" Checked="true"></asp:CheckBox>
                        <asp:Label ID="Label7" runat="Server" Text="Show Dormant " CssClass="mylabel1"></asp:Label>
                  </td>
                    <td >
                     <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                  <% if (rights.CanExport)
                    { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"
                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
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
                    
                    <%-- <% } %>--%>
                </div>
            </div>
        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2">
                  
                       <div>
                           <dxe:ASPxGridView runat="server" ID="ShowGridList" ClientInstanceName="GridList" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                               Settings-HorizontalScrollBarMode="Visible" KeyFieldName="SLNO"
                               DataSourceID="GenerateEntityServerModeDataSource" ClientSideEvents-BeginCallback="CallbackListofMaster_BeginCallback" >
                            <Columns>

                                <dxe:GridViewDataTextColumn Caption="Unique Code"  FieldName="Unique_Code" VisibleIndex="1" Width="250px">
                                </dxe:GridViewDataTextColumn>
                                 <dxe:GridViewDataTextColumn Caption="Name" FieldName="Name" Width="250px"
                                    VisibleIndex="2" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                     <HeaderStyle HorizontalAlign="Center" />
                                    <DataItemTemplate>
                                        <a href="javascript:void(0)" onclick="ClickVIewInfo('<%#Eval("CODE") %>')" class="pad">
                                            <%#Eval("Name")%>
                                        </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>
                            
                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Branch_DESCRIPTION" Width="200px" Caption="Branch" >
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

<%--                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="credit_Days" Width="80px" Caption="Cr. Days" >
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="credit_limit" Width="80px" Caption="Cr. Limit" >
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>--%>
                               
                                <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status" Width="60px" VisibleIndex="4" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                            
                                
                                <dxe:GridViewDataTextColumn Caption="Ledger" FieldName="Map_Ledger" Width="120px"
                                    VisibleIndex="5" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn Caption="GSTIN" FieldName="GSTIN_NO" Width="120px"
                                    VisibleIndex="6" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="GST Registered" FieldName="GST_Registered" Width="100px"
                                    VisibleIndex="7" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn Caption="Pan Card" FieldName="PanCrdNo" Width="100px"
                                    VisibleIndex="8" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn Caption="Aadhar Card" FieldName="AdharCrdNo" Width="100px"
                                    VisibleIndex="9" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                               <dxe:GridViewDataTextColumn Caption="Billing Address" FieldName="BILLING_ADDRESS" Width="320px"
                                    VisibleIndex="10" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                               </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Bill. Pin" FieldName="Billing_Pincode" Width="90px"
                                    VisibleIndex="11" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn Caption="Bill. Mob#" FieldName="BILLING_MOBILE_NUMBER" Width="130px"
                                    VisibleIndex="12" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                              <%--  <dxe:GridViewDataTextColumn Caption="Bill. Alternate No" FieldName="BILLING_ALTERNATE_NUMBER" Width="130px"
                                    VisibleIndex="13" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>--%>

                                 <dxe:GridViewDataTextColumn Caption="Billing Email" FieldName="Billing_EmailId" Width="110px"
                                    VisibleIndex="13" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn Caption="Shipping Address" FieldName="SHIPPING_ADDRESS" Width="320px"
                                    VisibleIndex="14" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Ship. Pin" FieldName="SHIPPING_Pincode" Width="100px"
                                    VisibleIndex="15" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Ship. Mob#" FieldName="SHIPPING_MOBILE_NUMBER" Width="130px"
                                    VisibleIndex="16" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                               <%-- <dxe:GridViewDataTextColumn Caption="Ship Alternate No" FieldName="SHIPPING_ALTERNATE_NUMBER" Width="130px"
                                    VisibleIndex="18" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>--%>

                                 <dxe:GridViewDataTextColumn Caption="Shipping Email" FieldName="Shipping_EmailId" Width="110px"
                                    VisibleIndex="17" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="ADDED_BY" Width="90px" VisibleIndex="18" >
                                    <CellStyle HorizontalAlign="Left"  Wrap="true">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Entered On" FieldName="ADDED_ON" Width="130px" VisibleIndex="19" >
                                    <CellStyle HorizontalAlign="Left"  Wrap="true">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Modified By" FieldName="EDITED_BY" Width="90px" VisibleIndex="20" >
                                    <CellStyle HorizontalAlign="Left"  Wrap="true">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Modified On" FieldName="EDITED_ON" Width="130px" VisibleIndex="21" >
                                    <CellStyle HorizontalAlign="Left"  Wrap="true">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                               
                            </Columns>

                            <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsContextMenu Enabled="true" />
                            <SettingsBehavior AutoExpandAllGroups="true" AllowSort="True" />
                            <Settings ShowGroupPanel="true" ShowStatusBar="Visible" ShowFilterRow="true" />
                            <SettingsSearchPanel Visible="false" />
                            <SettingsPager PageSize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </SettingsPager>

                        </dxe:ASPxGridView>

                      <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                        ContextTypeName="ReportSourceDataContext" TableName="LISTINGOFMASTER_REPORT" ></dx:LinqServerModeDataSource>
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
        <asp:HiddenField ID="hfIsListMastFilter" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
      <ClientSideEvents EndCallback="CallbackPanelEndCall" />
</dxe:ASPxCallbackPanel>

     <dxe:ASPxPopupControl ID="AspxDirectVendorViewPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectVendorViewPopup" Height="650px"
        Width="1020px" HeaderText="Vendor View" Modal="true" AllowResize="false">
         
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

</asp:Content>

