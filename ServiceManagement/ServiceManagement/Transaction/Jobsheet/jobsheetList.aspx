<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="jobsheetList.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.Jobsheet.jobsheetList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,800;0,900;1,600&display=swap" rel="stylesheet" />
    <link href="/assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />

    <script src="/assests/pluggins/datePicker/bootstrap-datepicker.js"></script>
    <link href="/assests/pluggins/Select2/select2.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/Select2/select2.min.js"></script>
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
    <link href="../CSS/jobsheetList.css" rel="stylesheet" />
    <script src="../JS/jobsheetList.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">

            <h3>Jobsheet</h3>
        </div>
    </div>
    <div class="clearfix form_main mTop5">
        <% if (rights.CanAdd)
           { %>
        <button class="btn btn-success btn-radius" type="button" onclick="JobSheetBtnClick();">
            <span class="btn-icon"><i class="fa fa-plus"></i></span>
            <span>Jobsheet</span>
        </button>
        <%} %>

        <% if (rights.CanExport)
           { %>
        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" onchange="ExportChange();">
            <asp:ListItem Value="0">Export to</asp:ListItem>
            <%--<asp:ListItem Value="1">PDF</asp:ListItem>--%>
            <asp:ListItem Value="2">XLS</asp:ListItem>
            <%--<asp:ListItem Value="4">CSV</asp:ListItem>--%>
        </asp:DropDownList>
        <% } %>
    </div>
    <div class="clearfix">
        <div id="filterToggle" class=" boxModel clearfix mBot10 mTop5 font-pp" style="background: #f9f9f9">
            <%--   <span class="togglerSlidecut"><i class="fa fa-times-circle"></i></span>--%>
            <div class="row">
                <div class="col-md-2">
                    <label>From Date</label>
                    <div class="relative">
                        <div class="input-group date" data-provide="datepicker">
                            <input type="text" class="form-control" id="FromDate" style="height: 28px;" />
                            <div class="input-group-addon">
                                <span class="fa fa-calendar-check-o"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>To Date</label>
                    <div class="relative">
                        <div class="input-group date" data-provide="datepicker">
                            <input type="text" class="form-control" id="ToDate" style="height: 28px;" />
                            <div class="input-group-addon">
                                <span class="fa fa-calendar-check-o"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Location</label>
                    <div class="relative">
                       <%-- <asp:DropDownList ID="ddlBranch" runat="server" CssClass="js-example-basic-single" DataTextField="branch_description" DataValueField="branch_id" Width="100%">
                        </asp:DropDownList>--%>
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
                    </div>
                </div>

                <div class="col-md-2">
                    <label>Technician name</label>
                    <div>
                       <%-- <asp:DropDownList ID="ddlTechnician" runat="server" CssClass="js-example-basic-single" DataTextField="cnt_firstName" DataValueField="cnt_id" Width="100%">
                        </asp:DropDownList>--%>
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
                    <label>&nbsp</label>
                    <div>
                        <button class="btn btn-success" type="button" style="margin-top: -2px;" onclick="SearchClick();">Search</button>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <div class="" id="LodingId">
        <div class="clearfix">
            <div class="relative">
                <%--<span class="togglerSlide btn btn-warning" style="position: absolute; right: 243px; z-index: 10;" data-toggle="tooltip" data-placement="top" title="Filter"><i class="fa fa-filter"></i></span>--%>
                <div id="divListData">
                    <table id="dataTable" class="table table-striped table-bordered display nowrap" style="width: 100%">
                        <thead>
                            <tr>
                                <th>Challan Number</th>
                                <th>Ref Jobsheet</th>
                                <th>Assign To </th>
                                <th>Work done on</th>
                                <th>Location</th>
                                <th>Entity Code</th>
                                <th>Network Name </th>
                                <th>Contact Person </th>
                                <th>Contact Number</th>
                                <th>Entered By</th>
                                <th>Entered On</th>
                                <th>Status</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <%-- <tbody>
                        <tr>
                            <td>154610</td>
                                   
                            <td>Technician name</td>
                            <td>asfasf </td>
                            <td>asfasf </td>
                            <td>asfasf </td>
                            <td>asfasf </td>
                            <td>10.2.21</td>
                            <td>asfasf </td>
                            <td>asfasf </td>
                            <td>10.2.21</td>
                            <td>Assigned by</td>
                            <td>Assigned on</td>       
                            <td>Assigned on</td>
                            <td>Assigned on</td>
                            <td class="actionInput text-center">
                               <span><i class="fa fa-plus assig" data-toggle="tooltip" data-placement="bottom" title="Add" ></i> </span>
                                <span><i class="fa fa-eye assig" data-toggle="tooltip" data-placement="bottom" title="View" ></i> </span>
                                <span><i class="fa fa-pencil-square-o assig" data-toggle="tooltip" data-placement="bottom" title="Edit" ></i> </span>
                                <span><i class="fa fa-print assig" data-toggle="tooltip" data-placement="bottom" title="Print" ></i> </span>
                            </td>
                        </tr>
                    </tbody>--%>
                    </table>
                </div>

            </div>
        </div>
    </div>

    <%--DEBASHIS--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">                    
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
    <%--DEBASHIS--%>

    <asp:HiddenField ID="hdnUserType" runat="server" />

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="LodingId"
        Modal="True">
        </dxe:ASPxLoadingPanel>
</asp:Content>
