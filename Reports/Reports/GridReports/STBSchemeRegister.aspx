<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="STBSchemeRegister.aspx.cs" Inherits="Reports.Reports.GridReports.STBSchemeRegister" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />
    <script src="../../assests/pluggins/datePicker/bootstrap-datepicker.js"></script>
    <script src="../../assests/pluggins/bootstrap-multiselect/bootstrap-multiselect.min.js"></script>
    <link href="../../assests/pluggins/bootstrap-multiselect/multiselect.css" rel="stylesheet" />
    <link href="CSS/ServiceRegister.css" rel="stylesheet" />
    <script src="JS/STBSchemeRegisterJS.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>STB Scheme Register
            </h3>
        </div>
    </div>
    <div class="form_main">
        <div class="slick boxModel clearfix font-pp" id="locationMulti">
            <div class="row">
                <div class="col-md-2">
                    <label>From Date </label>
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
                    <label>To Date </label>
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
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="EntityCode" Visible="true" VisibleIndex="1" width="200px" Caption="Entity Code" Settings-AutoFilterCondition="Contains">
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
                    <label>Location </label>
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
                <div class="col-md-2" style="padding-top: 25px;">
                    <div class="custom-control custom-checkbox">
                        <input type="checkbox" class="custom-control-input" id="customCheck1" checked />
                        <label class="custom-control-label" for="customCheck1">Details</label>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <button class="btn btn-success" type="button" style="margin-top: 14px;" onclick="Generate_Report();">Generate</button>
                    <% if (rights.CanExport)
                       { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-export" AutoPostBack="true" OnSelectedIndexChanged="drdExport_SelectedIndexChanged">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                    <% } %>
                </div>
            </div>
        </div>

        <div class="clearfix" style="margin-top: 5px">
            <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
                <panelcollection>
                <dxe:PanelContent runat="server">
                    <div id="DivSTBRegisterReportSummary" class="hide">

                        <dxe:ASPxGridView runat="server" ID="ShowGridHeader" ClientInstanceName="GridHeader" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Visible"
                            Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                            <columns>
                                    <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="DocumentNumber" Width="150px"
                                        Caption="Document No" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="DocumentDate" Width="150px"
                                        Caption="Date" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Branch" Width="150px"
                                        Caption="Location" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="STATUS" Width="150px"
                                        Caption="Status" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="EntityCode" Width="150px"
                                        Caption="Entity code" Visible="true">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="NetworkName" Width="200px"
                                        Caption="Network name" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="ContactPerson" Width="200px"
                                        Caption="Contact name" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ContactNo" Width="100px"
                                        Caption="contact no" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="Director" Width="150px"
                                        Caption="Director" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="RequisitioNo" Width="150px"
                                        Caption="Requisition No" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="EnteredBy" Width="150px"
                                        Caption="Entered By" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="EnteredOn" Width="150px"
                                        Caption="Entered On" Visible="true">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="DirApprAssignedBy" Width="200px"
                                        Caption="Dir.Appr. Assigned By" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="ApprAssignedOn" Width="200px"
                                        Caption="Appr. Assigned On" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="ApprovedOn" Width="100px"
                                        Caption="Approved On" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                   
                                    </columns>
                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" columnresizemode="Control" />
                            <settings showfooter="true" showgrouppanel="true" showgroupfooter="VisibleIfExpanded" />
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior autoexpandallgroups="true" />
                            <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" />
                            <settingssearchpanel visible="false" />
                            <settingspager pagesize="10">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                                    </settingspager>
                            <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />

                        </dxe:ASPxGridView>

                    </div>


                    <div id="DivSTBRegisterReportDetails">

                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Visible"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback"
                            Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                            <columns>
                                       <dxe:GridViewDataTextColumn Visible="False" FieldName="DocumentID" Caption="ID">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="DocumentNumber" Width="150px"
                                        Caption="Document No" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="DocumentDate" Width="150px"
                                        Caption="Date" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="True">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Branch" Width="150px"
                                        Caption="Location" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="STATUS" Width="150px"
                                        Caption="Status" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="EntityCode" Width="150px"
                                        Caption="Entity code" Visible="true">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="NetworkName" Width="200px"
                                        Caption="Network name" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="ContactPerson" Width="200px"
                                        Caption="Contact name" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ContactNo" Width="100px"
                                        Caption="contact no" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="DeviceNumber" Width="100px"
                                        Caption="Serial No" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="Model" Width="150px"
                                        Caption="Model" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="Rate" Width="100px"
                                        Caption="Rate" Visible="True" PropertiesTextEdit-DisplayFormatString="#####,##,##,###0.00;" Settings-AllowAutoFilter="False" HeaderStyle-CssClass="colDisable">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                   
                                <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="Remote" Width="100px"
                                        Caption="Remote" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="CardAdaptor" Width="150px"
                                        Caption="Cord/Adaptor" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="Remarks" Width="200px"
                                    Caption="Remarks" Visible="True">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="Director" Width="150px"
                                        Caption="Director" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="RequisitioNo" Width="150px"
                                        Caption="Requisition No" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="16" FieldName="EnteredBy" Width="150px"
                                        Caption="Entered By" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="17" FieldName="EnteredOn" Width="150px"
                                        Caption="Entered On" Visible="true">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                
                                    <dxe:GridViewDataTextColumn VisibleIndex="18" FieldName="DirApprAssignedBy" Width="200px"
                                        Caption="Dir. Appr. Assigned By" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="19" FieldName="ApprAssignedOn" Width="200px"
                                        Caption="Appr. Assigned On" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="20" FieldName="ApprovedOn" Width="100px"
                                        Caption="Approved On" Visible="True">
                                        <CellStyle CssClass="gridcellleft">
                                        </CellStyle>
                                        <Settings AllowAutoFilterTextInputTimer="False" />
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                    </columns>
                            <settingsbehavior confirmdelete="true" enablecustomizationwindow="true" enablerowhottrack="true" columnresizemode="Control" />
                            <settings showfooter="true" showgrouppanel="true" showgroupfooter="VisibleIfExpanded" />
                            <settingsediting mode="EditForm" />
                            <settingscontextmenu enabled="true" />
                            <settingsbehavior autoexpandallgroups="true" />
                            <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" />
                            <settingssearchpanel visible="false" />
                            <settingspager pagesize="10">
                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100" />
                                    </settingspager>
                            <settings showfilterrow="True" showfilterrowmenu="true" showstatusbar="Visible" usefixedtablelayout="true" />

                        </dxe:ASPxGridView>

                    </div>

                      <dx:LinqServerModeDataSource ID="GenerateEntityServerModeDataSource" runat="server" OnSelecting="GenerateEntityServerModeDataSource_Selecting"
                     ContextTypeName="ReportSourceDataContext" TableName="SRVMATERIALIOREGISTER_REPORT"></dx:LinqServerModeDataSource>

                    <asp:HiddenField ID="hfIsSTBMatIORegDetFilter" runat="server" />
                 </dxe:PanelContent>
                 </panelcollection>
                <clientsideevents endcallback="CallbackPanelEndCall" />
            </dxe:ASPxCallbackPanel>

            <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>

            <dxe:ASPxGridViewExporter ID="exporter1" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>
        </div>

        <asp:HiddenField ID="hdnType" runat="server" />
        <asp:HiddenField ID="hdnReportType" runat="server" />
        <asp:HiddenField ID="hdnFromDate" runat="server" />
        <asp:HiddenField ID="hdnToDate" runat="server" />
        <asp:HiddenField ID="hdnIsDetails" runat="server" />
        <asp:HiddenField ID="hdnGridTytpe" runat="server" />
    </div>
</asp:Content>
