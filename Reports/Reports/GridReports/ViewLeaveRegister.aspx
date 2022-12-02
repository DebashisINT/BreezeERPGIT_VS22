<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" EnableViewState="true" CodeBehind="ViewLeaveRegister.aspx.cs" Inherits="Reports.Reports.GridReports.ViewLeaveRegister" %>

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

        /*Rev Debashis*/
        #ListBoxProjects{
            width: 200px;
        }
        /*End of Rev Debashis*/

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
            if (ArrName == 'EmpSource') {
                var key = Id;
                if (key != null && key != '') {
                    $('#EmpModel').modal('hide');
                    ctxtEmpName.SetText(Name);
                    GetObjectID('hdnEmpId').value = key;
                  }
                else {
                    ctxtEmpName.SetText('');
                    GetObjectID('hdnEmpId').value = '';
                }
            }
           
        }

    </script>
   <%-- For multiselection when click on ok button--%>

    <%-- For multiselection--%>
 
     <script type="text/javascript">
         $(document).ready(function () {
             $('#EmpModel').on('shown.bs.modal', function () {
                 $('#txtEmpSearch').focus();
             })
         })
         var EmpArr = new Array();
         $(document).ready(function () {
             var EmpObj = new Object();
             EmpObj.Name = "EmpSource";
             EmpObj.ArraySource = EmpArr;
             arrMultiPopup.push(EmpObj);
         })
         function VendorButnClick(s, e) {
             
             $('#EmpModel').modal('show');
         }

         function Emp_KeyDown(s, e) {
             if (e.htmlEvent.key == "Enter") {
                 $('#EmpModel').modal('show');
             }
         }

         function Empkeydown(e) {
          
             var OtherDetails = {}

             if ($.trim($("#txtEmpSearch").val()) == "" || $.trim($("#txtEmpSearch").val()) == null) {
                 return false;
             }
             OtherDetails.SearchKey = $("#txtEmpSearch").val();

             if (e.code == "Enter" || e.code == "NumpadEnter") {
                
                 var HeaderCaption = [];
                 HeaderCaption.push("Employee Name");

                 if ($("#txtEmpSearch").val() != "") {
                     
                     callonServerM("Services/Master.asmx/GetEmpLeaveRegisterLists", OtherDetails, "EmpTable", HeaderCaption, "dPropertyIndex", "SetSelectedValues", "EmpSource");
                    //callonServerScroll("Services/Payroll_Master.asmx/GetPayStructureList", OtherDetails, "TableStructure", HeaderCaption, "searchIndex", "SetDropdownValue");
                 }
             }
             else if (e.code == "ArrowDown") {
                 if ($("input[dPropertyIndex=0]"))
                     $("input[dPropertyIndex=0]").focus();
             }
         }


         function SetfocusOnseach(indexName) {
             if (indexName == "dPropertyIndex")
                 $('#txtEmpSearch').focus();
             else
                 $('#txtEmpSearch').focus();
         }
   </script>
      <%-- For multiselection--%>

    <script type="text/javascript">

        function cxdeToDate_OnChaged(s, e) {
            var data = "OnDateChanged";
            data += '~' + cxdeFromDate.GetDate();
            data += '~' + cxdeToDate.GetDate();
        }

        function btn_ShowRecordsClick(e) {
            if (cxdeFromDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cxdeFromDate.Focus(); });
            }
            else if (cxdeToDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { cxdeToDate.Focus(); });
            }
            else
            {
                e.preventDefault;
                
                var data = "OnDateChanged";
                $("#hfIsLeaveRegFilter").val("Y");
                var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
                var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

                FromDate = GetDateFormat(FromDate);
                ToDate = GetDateFormat(ToDate);
                $("#hfFromDate").val(cxdeFromDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(cxdeToDate.GetDate().format('yyyy-MM-dd'));
              
                document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;

            $("#drdExport").val(0);

            var FromDate = (cxdeFromDate.GetValue() != null) ? cxdeFromDate.GetValue() : "";
            var ToDate = (cxdeToDate.GetValue() != null) ? cxdeToDate.GetValue() : "";

            FromDate = GetDateFormat(FromDate);
            ToDate = GetDateFormat(ToDate);
            document.getElementById('<%=DateRange.ClientID %>').innerHTML = "For the period: " + FromDate + " To " + ToDate;
            cCallbackPanel.PerformCallback();

            }
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
             function CallbackListofMaster_BeginCallback() {
                 $("#drdExport").val(0);
             }
    </script>
    <script>
        

        function CloseGridQuotationLookup() {
            gridbranchLookup.ConfirmCurrentSelection();
            gridbranchLookup.HideDropDown();
            gridbranchLookup.Focus();
        }


        function CallbackPanelEndCall(s, e) {
            GridList.Refresh();
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
                <label>
            <dxe:ASPxLabel ID="lbl_Vendor" style="color: #b5285f;" runat="server" Text="Employee:">
                </dxe:ASPxLabel> </label>
                <div>                 
                <dxe:ASPxButtonEdit ID="txtEmpName" runat="server" ReadOnly="true" ClientInstanceName="ctxtEmpName" Width="100%" TabIndex="5">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){Emp_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit></div> 
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
            
            <%--Rev Debashis--%>
            
            <%--End of Rev Debashis--%>
         
            <div class="col-md-2">
                <label style="margin-bottom: 0">&nbsp</label>
                    <div style="padding-top: 1px" class="clsTo">
                        <button id="btnShow" class="btn btn-success" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                          <%--<input type="button" value="Show" class="btn btn-success" onclick="updateGridByDate()" />--%>
                        <% if (rights.CanExport)
                        { %> 
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">EXCEL</asp:ListItem>
                            <asp:ListItem Value="3">CSV</asp:ListItem>
                        </asp:DropDownList>
                        <% } %>
                    </div>
            </div>
            <div class="clear"></div>

        </div>
        <table class="TableMain100">
            <tr>
                <td colspan="2">

                    <div>
                        <dxe:ASPxGridView runat="server" ID="ShowGridList" ClientInstanceName="GridList" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                            Settings-HorizontalScrollBarMode="Visible" DataSourceID="GenerateEntityServerModeDataSource"  KeyFieldName="SLNO" ClientSideEvents-BeginCallback="CallbackListofMaster_BeginCallback">
                            
                            <columns>
                                 <dxe:GridViewDataTextColumn Caption="Employee Code"  Width="100px" FieldName="EMPLOYEECODE"
                                    VisibleIndex="1" >
                                    <CellStyle HorizontalAlign="Left" Wrap="true">
                                    </CellStyle>
                                 
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                             
                                <dxe:GridViewDataTextColumn Caption="Employee Name"  Width="250px" FieldName="EMPLOYEENAME"
                                    VisibleIndex="2" >
                                    <CellStyle HorizontalAlign="Left" Wrap="true">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                            
                                <dxe:GridViewDataTextColumn Caption="Leave Type"  Width="80px" FieldName="LEAVENAME"
                                    VisibleIndex="3" >
                                    <CellStyle HorizontalAlign="Left" Wrap="true">
                                    </CellStyle>
                                   
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                              
                                <dxe:GridViewDataTextColumn Caption="Leave Start Date"  Width="250px" FieldName="LEV_DATE_FROM"
                                    VisibleIndex="4" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                                    <CellStyle HorizontalAlign="Left" Wrap="true">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Leave End Date"  Width="250px" FieldName="LEV_DATE_TO"
                                    VisibleIndex="5" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" >
                                    <CellStyle HorizontalAlign="Left" Wrap="true" >
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>

                               
                                <dxe:GridViewDataTextColumn Caption="Leave Days"  Width="80px" FieldName="LEAVEDAYS"
                                    VisibleIndex="6" >
                                    <CellStyle HorizontalAlign="Left" Wrap="true">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                               
                            
                                  <dxe:GridViewDataTextColumn Caption="Leave Applied on"  Width="250px" FieldName="LEAVEAPPLIEDON"
                                    VisibleIndex="7" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy" >
                                    <CellStyle HorizontalAlign="Left" Wrap="true">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                                
                                   <dxe:GridViewDataTextColumn Caption="Status"  Width="80px" FieldName="STATUS"
                                    VisibleIndex="8" >
                                    <CellStyle HorizontalAlign="Left" Wrap="true">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                             
                                <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="ENTEREDBY" Width="100px" VisibleIndex="9">
                                    <CellStyle HorizontalAlign="Left"  Wrap="true">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Left" />
                                </dxe:GridViewDataTextColumn>
                                 
                            </columns>

                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" columnresizemode="Control" />
                            <settings showfooter="true" showgrouppanel="true" showgroupfooter="VisibleIfExpanded" />
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior autoexpandallgroups="true" allowsort="True" />
                            <settings showgrouppanel="true" showstatusbar="Visible" showfilterrow="true" />
                            <settingssearchpanel visible="false" />
                            <settingspager pagesize="10">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            </settingspager>

                        </dxe:ASPxGridView>

                        <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                            ContextTypeName="ReportSourceDataContext" TableName="VIEWLEAVEREGISTER_REPORT"></dx:LinqServerModeDataSource>
                    </div>


                </td>
            </tr>
        </table>
        
    </div>

    <!--Vendor Modal -->
    <div class="modal fade" id="EmpModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Employee Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Empkeydown(event)" id="txtEmpSearch" width="100%" placeholder="Search By Employee Name" />
                    <div id="EmpTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Employee Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('EmpSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('EmpSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <!--Vendor Modal -->

    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

     <asp:HiddenField ID="hdnEmpId" runat="server" />
      <%-- <asp:HiddenField ID="hfIsFilter" runat="server" />--%>
     

    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
        <dxe:PanelContent runat="server">
        <asp:HiddenField ID="hfIsLeaveRegFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        </dxe:PanelContent>
    </PanelCollection>
      <ClientSideEvents EndCallback="CallbackPanelEndCall" />
</dxe:ASPxCallbackPanel>

</asp:Content>