<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ServiceRegister.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.ServiceRegister.ServiceRegister" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.15/css/bootstrap-multiselect.css" integrity="sha256-7stu7f6AB+1rx5IqD8I+XuIcK4gSnpeGeSjqsODU+Rk=" crossorigin="anonymous" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.15/js/bootstrap-multiselect.min.js" integrity="sha256-qoj3D1oB1r2TAdqKTYuWObh01rIVC1Gmw9vWp1+q5xw=" crossorigin="anonymous"></script>--%>
    <link href="/assests/pluggins/bootstrap-multiselect/multiselect.css" rel="stylesheet" />
    <script src="/assests/pluggins/bootstrap-multiselect/bootstrap-multiselect.min.js"></script>
    <link href="/assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />
    <script src="/assests/pluggins/datePicker/bootstrap-datepicker.js"></script>
    <link href="/assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <script src="/assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>

    
    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/dataTables.buttons.min.js"></script>
    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/buttons.flash.min.js"></script>
    <script src="/assests/pluggins/DataTable/JSZip-2.5.0/jszip.min.js"></script>
    <script src="/assests/pluggins/DataTable/pdfmake-0.1.36/pdfmake.min.js"></script>
    <script src="/assests/pluggins/DataTable/pdfmake-0.1.36/vfs_fonts.js"></script>
    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/buttons.html5.min.js"></script>
    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/buttons.print.min.js"></script>

    <link href="../CSS/ServiceRegister.css" rel="stylesheet" />
    <script src="../JS/ServiceRegister.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>
                Service Register
            </h3>
        </div>
    </div>
    <div class="form_main" id="divGenerateReport">
        <div class=" slick boxModel clearfix font-pp" id="locationMulti">
            <div class="row" style="margin-top: 5px">
                <div class="col-md-2">
                    <label>Type <span class="red">*</span></label>
                    <div>
                        <%-- example-getting-started--%>
                        <select id="ddlType" class="form-control " onchange="ddlType_change();">
                            <%--multi multiple="multiple"--%>
                            <option value="">Select</option>
                            <option value="rcptChallan">Receipt Challan</option>
                            <option value="Service">Service</option>
                            <option value="Delivery">Delivery</option>
                        </select>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Report <span class="red">*</span></label>
                    <div id="divReport">
                        <select id="ddlReport" class="multi" multiple="multiple">
                        </select>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>From date</label>
                    <div class="relative">
                        <div class="input-group date">
                            <input type="text" id="dtFromdate" class="form-control" style="height: 28px;" />
                            <div class="input-group-addon">
                                <span class="fa fa-calendar-check-o"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>To Date</label>
                    <div>
                        <div class="relative">
                            <div class="input-group date">
                                <input type="text" id="dtToDate" class="form-control" style="height: 28px;" />
                                <div class="input-group-addon">
                                    <span class="fa fa-calendar-check-o"></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Entity Code</label>
                    <div id="DivEntityCode">
                        <%-- <select id="ddlEntityCode" class="multi" multiple="multiple">
                        </select>--%>
                        <dxe:ASPxCallbackPanel runat="server" ID="EntityCodePanel" ClientInstanceName="cEntityCodePanel" OnCallback="EntityCode_Callback">
                            <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_EntityCode" SelectionMode="Multiple" runat="server" ClientInstanceName="gridEntityCodeLookup"
                                OnDataBinding="lookup_EntityCode_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="40" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="EntityCode" Visible="true" VisibleIndex="1" width="100px" Caption="Entity Code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                 <%--   <dxe:GridViewDataColumn FieldName="EntityCode" Visible="true" VisibleIndex="2" width="200px" Caption="Entity Code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>--%>
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                        <%--<div class="hide">--%>
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllEntityCode" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllEntityCode" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseEntityCodeLookup" UseSubmitBehavior="False" />
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
                    </panelcollection>
                        </dxe:ASPxCallbackPanel>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Entry Type</label>
                    <div id="DivEntityType">
                        <select id="ddlEntityType" class="multi" multiple="multiple">
                            <option value="1">Token</option>
                            <option value="2">Challan</option>
                            <option value="3">Worksheet</option>
                            <option value="4">JobSheet</option>
                        </select>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="col-md-2">
                    <label>Model</label>
                    <div id="DivModel">
                        <%--<select id="ddlModel" class="multi" multiple="multiple">
                        </select>--%>
                        <dxe:ASPxCallbackPanel runat="server" ID="ModelPanel" ClientInstanceName="cModelPanel" OnCallback="Model_Callback">
                            <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_Model" SelectionMode="Multiple" runat="server" ClientInstanceName="gridModelLookup"
                                OnDataBinding="lookup_Model_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="Model" Visible="true" VisibleIndex="1" width="200px" Caption="Model(s)" Settings-AutoFilterCondition="Contains">
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
                                                            <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllModel" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton4" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllModel" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseModelLookup" UseSubmitBehavior="False" />
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
                    </panelcollection>
                        </dxe:ASPxCallbackPanel>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Problem Found</label>
                    <div id="DivProblem">
                        <%-- <select id="ddlProblem" class="multi" multiple="multiple">
                        </select>--%>
                        <dxe:ASPxCallbackPanel runat="server" ID="ProblemFoundPanel" ClientInstanceName="cProblemFoundPanel" OnCallback="ProblemFound_Callback">
                            <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_ProblemFound" SelectionMode="Multiple" runat="server" ClientInstanceName="gridProblemFoundLookup"
                                OnDataBinding="lookup_ProblemFound_DataBinding"
                                KeyFieldName="ProblemID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="ProblemDesc" Visible="true" VisibleIndex="1" width="200px" Caption="Problem(s)" Settings-AutoFilterCondition="Contains">
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
                                                            <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllProblem" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton7" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllProblem" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton8" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseProblemLookup" UseSubmitBehavior="False" />
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
                    </panelcollection>
                        </dxe:ASPxCallbackPanel>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Technician</label>
                    <div id="DivTechnician">
                        <%-- <select id="ddlTechnician" class="multi" multiple="multiple">
                        </select>--%>
                        <dxe:ASPxCallbackPanel runat="server" ID="TechnicianPanel" ClientInstanceName="cTechnicianPanel" OnCallback="Technician_Callback">
                            <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_Technician" SelectionMode="Multiple" runat="server" ClientInstanceName="gridTechnicianLookup"
                                OnDataBinding="lookup_Technician_DataBinding"
                                KeyFieldName="cnt_internalId" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="cnt_firstName" Visible="true" VisibleIndex="1" width="200px" Caption="Technician(s)" Settings-AutoFilterCondition="Contains">
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
                                                            <dxe:ASPxButton ID="ASPxButton12" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllTechnician" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton13" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllTechnician" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton14" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseTechnicianLookup" UseSubmitBehavior="False" />
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
                    </panelcollection>
                        </dxe:ASPxCallbackPanel>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Location</label>
                    <div id="DivLocation">
                        <%-- <select id="ddlLocation" class="multi" multiple="multiple">
                        </select>--%>
                        <dxe:ASPxCallbackPanel runat="server" ID="LocationPanel" ClientInstanceName="cLocationPanel" OnCallback="Location_Callback">
                            <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_Location" SelectionMode="Multiple" runat="server" ClientInstanceName="gridLocationLookup"
                                OnDataBinding="lookup_Location_DataBinding"
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
                                                            <dxe:ASPxButton ID="ASPxButton9" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllLocation" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton10" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllLocation" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton11" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLoactionLookup" UseSubmitBehavior="False" />
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
                    </panelcollection>
                        </dxe:ASPxCallbackPanel>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Billable</label>
                    <div id="">
                        <select id="ddlBillable" class="form-control">
                            <option value="2">All</option>
                            <option value="1">Yes</option>
                            <option value="0">No</option>
                        </select>
                    </div>
                </div>

                <div class="col-md-2">
                    <label>&nbsp;</label>
                    <div id="">
                        <input type="checkbox" id="chkProblemReport" checked onchange="chkProblemReport_change();" />
                        Details
                    </div>
                </div>
                <div class="clear"></div>
                <%-- <div class="col-md-2">
                    <label>Problem Reported</label>
                    <div id="DivProblemReported">
                      
                        <dxe:ASPxCallbackPanel runat="server" ID="ProblemReportedPanel" ClientInstanceName="cProblemReportedPanel" OnCallback="ProblemReported_Callback">
                            <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_ProblemReported" SelectionMode="Multiple" runat="server" ClientInstanceName="gridProblemReportedLookup"
                                OnDataBinding="lookup_ProblemReported_DataBinding"
                                KeyFieldName="ProblemID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="ProblemDesc" Visible="true" VisibleIndex="1" width="200px" Caption="Problem(s)" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                        
                                                            <dxe:ASPxButton ID="ASPxButton15" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllProblemReported" UseSubmitBehavior="False" />
                                                     
                                                        <dxe:ASPxButton ID="ASPxButton16" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllProblemReported" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton17" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseProblemReportedLookup" UseSubmitBehavior="False" />
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
                    </panelcollection>
                        </dxe:ASPxCallbackPanel>
                    </div>
                </div>--%>
                <div class="clear"></div>
                 <div class="col-md-2">
                    <label>Confirm Delivered</label>
                    <div id="">
                        <select id="ddlConfirmDelivered" class="form-control">
                            <option value="2">All</option>
                            <option value="1">Yes</option>
                            <option value="0">No</option>
                        </select>
                    </div>
                </div>
                <div class="col-md-4">
                    <button class="btn btn-success" type="button" style="margin-top: 20px;" onclick="Generate_Report();">Generate</button>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius btn-export" onchange="ExportChange();">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <%--<asp:ListItem Value="1">PDF</asp:ListItem>--%>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <%--<asp:ListItem Value="4">CSV</asp:ListItem>--%>
                    </asp:DropDownList>
                </div>
            </div>
        </div>


        <div class="pt-3" style="margin-top: 15px;">
            <div class="clearfix font-pp">
                <div class="relative">

                    <div id="DivServiceRegisterReportDetails">
                        <%-- <table id="dataTable" class="table table-striped table-bordered display nowrap" style="width: 100%">
                            <thead>
                                <tr>
                                    <th>Receipt Challan</th>
                                    <th>Type</th>
                                    <th>Entity Code </th>
                                    <th>Network Name</th>
                                    <th>Contact Person</th>
                                    <th>Technician </th>
                                    <th>Location </th>
                                    <th>Received by</th>
                                    <th>Received on</th>
                                    <th>Assigned by</th>
                                    <th>Assigned on</th>
                                    <th>Serv. Entered By</th>
                                    <th>Serv. Entered On</th>
                                    <th>Action</th>
                                </tr>
                            </thead>

                        </table>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>

      <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divGenerateReport"
        Modal="True">
    </dxe:ASPxLoadingPanel>
</asp:Content>
