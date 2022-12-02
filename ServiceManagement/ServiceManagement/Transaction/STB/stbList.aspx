<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="stbList.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.STB.stbList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

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
    <script src="../JS/STBReceivingList.js?V=1.09"></script>
    <script>
        function reBtnClick() {
            window.location.href = '/ServiceManagement/Transaction/STB/stbAdd.aspx?Key=ADD';
        }
    </script>
    <style>
        .dt-buttons {
            display: none !important;
        }
    </style>
    <style>
        .drHover {
            display: inline-block;
            margin-bottom: 5px;
        }

            .drHover .dropdown-menu > li > a:hover {
                background: #1987c6;
            }

            .drHover > button.ft {
                border-radius: 15px 0 0 15px;
            }

            .drHover > button.sd {
                border-radius: 0 15px 15px 0;
                height: 31px;
            }

        .padTab > tbody > tr > td {
            padding-right: 15px;
            vertical-align: middle;
        }

            .padTab > tbody > tr > td > label {
                margin-bottom: 0 !important;
            }

            .padTab > tbody > tr > td > .btn {
                margin-top: 0 !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>STB Receiving</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px">
            <tr>
                <td>
                    <label>From </label>
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <label>To </label>
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                </td>

            </tr>

        </table>
    </div>

    <div class="form_main">
        <table class="TableMain100" cellpadding="0px" width="100%">
            <tr>
                <td>
                    <% if (rights.CanAdd)
                       { %>
                        <button class="btn btn-success btn-radius" type="button" onclick="reBtnClick();">
                            <span class="btn-icon"><i class="fa fa-plus"></i></span>
                            <span>Receiving</span>
                        </button>
                    <%} %>

                    <% if (rights.CanExport)
                       { %>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" AutoPostBack="true" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                    <% } %>
                </td>
            </tr>
            <tr>
                <td class="gridcellcenter relative" colspan="2">
                    <dxe:ASPxGridView ID="GrdSTBListing" runat="server" KeyFieldName="STBReceived_ID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                        SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="280" Settings-HorizontalScrollBarMode="Auto"
                        DataSourceID="EntityServerModeDataSource"
                        Width="100%" ClientInstanceName="cGrdSTBListing">
                        <settingssearchpanel visible="True" delay="5000" />
                        <columns>
                          
                            <dxe:GridViewDataTextColumn Caption="Document No" FieldName="DocumentNumber"
                                VisibleIndex="1" Width="20%">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn Caption="Date" Width="10%" FieldName="DocumentDate" VisibleIndex="2">
                                <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Location" FieldName="branch_description" VisibleIndex="3" Width="15%">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks" VisibleIndex="4" Width="15%">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="CREATE_BY" Width="10%" VisibleIndex="5">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                              <dxe:GridViewDataTextColumn Caption="Entered On" FieldName="Create_date" Width="10%" VisibleIndex="6">
                                     <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                              <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="UPDATE_BY" Width="10%" VisibleIndex="7">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                              <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="Update_date" Width="10%" VisibleIndex="8">
                                     <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="9" Width="0">
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                         <% if (rights.CanView)
                                            { %>
                                        <a href="javascript:void(0);" onclick="View('<%# Container.KeyValue %>')" id="a_viewInvoice" class="" title="">
                                            <span class='ico editColor'><i class='fa fa-eye' aria-hidden='true'></i></span><span class='hidden-xs'>View</span></a>
                                        <%} %>
                                        <% if (rights.CanEdit)
                                           { %>
                                        <a href="javascript:void(0);" onclick="Edit('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                        <%} %>
                                        <% if (rights.CanDelete)
                                           { %>
                                        <a href="javascript:void(0);" onclick="Delete('<%# Container.KeyValue %>')" class="" title="" id="a_delete">
                                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                        <%} %>
                                           <% if (rights.CanPrint)
                                              { %>
                                        <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="" title="" id="a_Print">
                                            <span class='ico printColor'><i class='fa fa-print det' aria-hidden='true'></i></span><span class='hidden-xs'>Print</span></a>
                                        <%} %>
                                       
                                    </div>
                                </DataItemTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate><span></span></HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                        </columns>
                        <settingscontextmenu enabled="true"></settingscontextmenu>
                        <clientsideevents rowclick="gridRowclick" />
                        <settingspager numericbuttoncount="10" pagesize="10" showseparators="True" mode="ShowPager">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </settingspager>
                        <settings showgrouppanel="True" showstatusbar="Hidden" showhorizontalscrollbar="False" showfilterrow="true" showfilterrowmenu="true" />
                        <settingsloadingpanel text="Please Wait..." />
                    </dxe:ASPxGridView>
                    <dx:linqservermodedatasource id="EntityServerModeDataSource" runat="server" onselecting="EntityServerModeDataSource_Selecting"
                        contexttypename="ServicveManagementDataClassesDataContext" tablename="V_STBReceivingReportList" />
                    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                    </dxe:ASPxGridViewExporter>
                </td>
            </tr>

        </table>
    </div>

    <%--<div class="clearfix form_main mTop5">
        <button class="btn btn-success btn-radius" type="button" onclick="reBtnClick();">
            <span class="btn-icon"><i class="fa fa-plus"></i></span>
            <span>Receiving</span>
        </button>

        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" onchange="ExportChange();">
            <asp:ListItem Value="0">Export to</asp:ListItem>
            <asp:ListItem Value="2">XLS</asp:ListItem>
        </asp:DropDownList>
        <%--<div class="clearfix">
            <div id="filterToggle" class=" boxModel clearfix mBot10 mTop5 font-pp" style="background: #f9f9f9">
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
                                                      
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllBranch" UseSubmitBehavior="False" />
                                                     
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
                        <label>&nbsp</label>
                        <div>
                            <button class="btn btn-success" type="button" style="margin-top: -2px;" onclick="SearchClick();">Search</button>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <div class="relative mTop5">
            <table id="dataTable" class="table table-striped table-bordered display nowrap" style="width: 100%">
                <thead>
                    <tr>
                        <th>Document No</th>
                        <th>Date</th>
                        <th>Unit </th>
                        <th>Remarks</th>
                        <th>Entered by</th>
                        <th>Entered On </th>
                        <th>Action</th>
                    </tr>
                </thead>

            </table>
        </div>
    </div>--%>

    <%--DEBASHIS--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <contentcollection>
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
            </contentcollection>
        </dxe:ASPxPopupControl>
    </div>
    <%--DEBASHIS--%>

    <asp:HiddenField ID="hdnUserType" runat="server" />

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="LodingId"
        Modal="True">
    </dxe:ASPxLoadingPanel>

     <asp:HiddenField ID="hfIsFilter" runat="server" />
    <asp:HiddenField ID="hfFromDate" runat="server" />
    <asp:HiddenField ID="hfToDate" runat="server" />
    <asp:HiddenField ID="hfBranchID" runat="server" />
    <asp:HiddenField ID="hdnIsUserwiseFilter" runat="server" />
    <asp:HiddenField ID="hddnKeyValue" runat="server" />
    <asp:HiddenField ID="hddnCancelCloseFlag" runat="server" />

</asp:Content>
