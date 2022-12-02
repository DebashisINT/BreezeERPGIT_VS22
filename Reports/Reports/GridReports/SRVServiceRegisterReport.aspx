<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SRVServiceRegisterReport.aspx.cs" Inherits="Reports.Reports.GridReports.SRVServiceRegisterReport" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <link href="/assests/pluggins/bootstrap-multiselect/multiselect.css" rel="stylesheet" />
    <script src="/assests/pluggins/bootstrap-multiselect/bootstrap-multiselect.min.js"></script>
    <link href="/assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />
    <script src="/assests/pluggins/datePicker/bootstrap-datepicker.js"></script>--%>
    <link href="../../assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />
    <script src="../../assests/pluggins/datePicker/bootstrap-datepicker.js"></script>
    <script src="../../assests/pluggins/bootstrap-multiselect/bootstrap-multiselect.min.js"></script>
    <link href="../../assests/pluggins/bootstrap-multiselect/multiselect.css" rel="stylesheet" />

    <script src="JS/ServiceRegister.js?v=1.0.0.05"></script>
    <link href="CSS/ServiceRegister.css" rel="stylesheet" />


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Service Register
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

        <input type="hidden" id="hdnGridTytpe" runat="server" />
        <div class="pt-3" style="margin-top: 15px;">
            <div class="clearfix font-pp">
                <div class="relative">


                    <div id="DivServiceRegisterReportSummary">

                        <dxe:ASPxGridView runat="server" ID="ShowGridHeader" ClientInstanceName="GridHeader" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Visible"
                            ClientSideEvents-BeginCallback="HeaderCallback_EndCallback"
                            Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                            <columns>
                                 <dxe:GridViewDataTextColumn Caption="Receipt Challan" FieldName="RECEIPTCHALLAN" Width="120px" VisibleIndex="0" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Date" Width="100px" FieldName="DATE" VisibleIndex="1">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Entry Type" Width="120px" FieldName="ENTRYTYPE" VisibleIndex="2" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Location" Width="120px" FieldName="LOCATION" VisibleIndex="3" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Entity Code" Width="150px" FieldName="ENTITYCODE" VisibleIndex="4" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>                               

                                <dxe:GridViewDataTextColumn Caption="Network Name" Width="200px" FieldName="NETWORKNAME" VisibleIndex="5" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Contact Name" Width="200px" FieldName="CONTACTNAME" VisibleIndex="6" >
                                   <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn Caption="Contact No" Width="150px" FieldName="CONTACTNO" VisibleIndex="7" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                   <dxe:GridViewDataTextColumn Caption="Received By" Width="150px" FieldName="RECEIVEDBY" VisibleIndex="8">
                                     <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="RECEIVEDON" Width="100px" Caption="Received On" >
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Technician" Width="200px" FieldName="TECHNICIAN" VisibleIndex="10">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Assigned By" FieldName="ASSIGNEDBY" Width="150px" VisibleIndex="11">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Assigned On" FieldName="ASSIGNEDON" Width="100px" VisibleIndex="12">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--mantis Issue 24780--%>
                                <dxe:GridViewDataTextColumn Caption="Repairing Status" FieldName="RepairStatus" Width="100px" VisibleIndex="13">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Rep. Status Date" FieldName="Repair_Update_date" Width="100px" VisibleIndex="14">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--end of mantis Issue 24780--%>
                                 <dxe:GridViewDataTextColumn FieldName="SERVICEDBY" Caption="Serviced By" Width="150px" VisibleIndex="15">
                                  
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="SERVICEDON" Caption="Serviced On" Width="100px" VisibleIndex="16">
                                   
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CONFIRMDELIVERY" Caption="Confirm Delivered" Width="100px" VisibleIndex="17">
                                    
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DELIVEREDTO" Caption="Delivered To" Width="200px" VisibleIndex="18">
                                    
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                 <%--Mantis Issue 24496--%>
                                <dxe:GridViewDataTextColumn FieldName="DEL_CONTACTNO" Caption="Delivery Contact No" Width="200px" VisibleIndex="19">
                                    
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24496--%>
                                <dxe:GridViewDataTextColumn FieldName="DELIVEREDBY" Caption="Delivered By" Width="150px" VisibleIndex="20">
                                    
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DELIVEREDON" Caption="Delivered On" Width="100px" VisibleIndex="21">                                   
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                  <dxe:GridViewDataTextColumn FieldName="CONFIRMDELIVERYDATE" Caption="Confirm Delivery Date" Width="100px" VisibleIndex="22">                                   
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="STATUS" Caption="Status" Width="100px" VisibleIndex="23">
                                    
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <%--Mantis Issue 25213--%>
                                <dxe:GridViewDataTextColumn FieldName="LEVELDESC" Caption="Level" Width="100px" VisibleIndex="24">
                                    
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 25213--%>
                               

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


                    <div id="DivServiceRegisterReportDetails" class="hide">

                        <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="Grid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Visible"
                            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" ClientSideEvents-BeginCallback="Callback_EndCallback"
                            Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                            <columns>
                                <dxe:GridViewDataTextColumn Caption="Receipt Challan" FieldName="RECEIPTCHALLAN" Width="120px" VisibleIndex="0" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Date" Width="100px" FieldName="DATE" VisibleIndex="1">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Entry Type" Width="120px" FieldName="ENTRYTYPE" VisibleIndex="2" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Location" Width="150px" FieldName="LOCATION" VisibleIndex="3" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Entity Code" Width="150px" FieldName="ENTITYCODE" VisibleIndex="4" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>                               

                                <dxe:GridViewDataTextColumn Caption="Network Name" Width="200px" FieldName="NETWORKNAME" VisibleIndex="5" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Contact Name" Width="200px" FieldName="CONTACTNAME" VisibleIndex="6" >
                                   <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn Caption="Contact No" Width="150px" FieldName="CONTACTNO" VisibleIndex="7" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Model No" Width="150px" FieldName="MODELNO" VisibleIndex="8" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Serial No" Width="150px" FieldName="SERIALNO" VisibleIndex="9" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="PROBLEMREPORTED" Width="200px" Caption="Problem Reported" >
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Received By" Width="150px" FieldName="RECEIVEDBY" VisibleIndex="11">
                                     <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="RECEIVEDON" Width="100px" Caption="Received On" >
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Technician" Width="200px" FieldName="TECHNICIAN" VisibleIndex="13">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Assigned By" FieldName="ASSIGNEDBY" Width="150px" VisibleIndex="14">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Assigned On" FieldName="ASSIGNEDON" Width="100px" VisibleIndex="15">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--mantis Issue 24780--%>
                                <dxe:GridViewDataTextColumn Caption="Repairing Status" FieldName="RepairStatus" Width="100px" VisibleIndex="16">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Rep. Status Date" FieldName="Repair_Update_date" Width="100px" VisibleIndex="17">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--end of mantis Issue 24780--%>
                                <dxe:GridViewDataTextColumn FieldName="SERVICEACTION" Caption="Service Action" Width="100px" VisibleIndex="18">
                                      <HeaderStyle HorizontalAlign="Right" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn Caption="Component(s)" FieldName="COMPONENT" Width="300px" VisibleIndex="19">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Problem Found" FieldName="PROBLEMFOUND" Width="200px" VisibleIndex="20">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <%--add new column Warranty Status--%>
                                 <dxe:GridViewDataTextColumn Caption="Warranty Status" Width="150px" FieldName="WARRANTYSTATUS" VisibleIndex="21">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--add new column Warranty Status--%>

                                 <dxe:GridViewDataTextColumn FieldName="PROBLEMREMARKS" Caption="Problem Remarks" Width="200px" VisibleIndex="22">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="BILLABLE" Caption="Billable" Width="100px" VisibleIndex="23">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="REASON" Caption="Reason" Width="150px" VisibleIndex="24">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="SERVICEDBY" Caption="Serviced By" Width="150px" VisibleIndex="25">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="SERVICEDON" Caption="Serviced On" Width="100px" VisibleIndex="26">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CONFIRMDELIVERY" Caption="Confirm Delivered" Width="100px" VisibleIndex="27">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DELIVEREDTO" Caption="Delivered To" Width="200px" VisibleIndex="28">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--Mantis Issue 24496--%>
                                <dxe:GridViewDataTextColumn FieldName="DEL_CONTACTNO" Caption="Delivery Contact No" Width="200px" VisibleIndex="29">
                                    
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24496--%>
                                <dxe:GridViewDataTextColumn FieldName="DELIVEREDBY" Caption="Delivered By" Width="150px" VisibleIndex="30">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DELIVEREDON" Caption="Delivered On" Width="100px" VisibleIndex="31">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="CONFIRMDELIVERYDATE" Caption="Confirm Delivery Date" Width="100px" VisibleIndex="32">                                   
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="STATUS" Caption="Status" Width="130px" VisibleIndex="33">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--Mantis Issue 25213--%>
                                <dxe:GridViewDataTextColumn FieldName="LEVELDESC" Caption="Level" Width="100px" VisibleIndex="34">
                                    
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 25213--%>
                                

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


                    <div id="DivServiceRegisterReportEntry" class="hide">

                        <dxe:ASPxGridView runat="server" ID="ShowGridEntry" ClientInstanceName="Grid1" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="SEQ"
                            DataSourceID="GenerateEntityServerModeDataSource" Settings-HorizontalScrollBarMode="Visible"
                            ClientSideEvents-BeginCallback="Grid1Callback_EndCallback"
                            Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Auto">
                            <columns>
                                <dxe:GridViewDataTextColumn Caption="Receipt Challan" FieldName="RECEIPTCHALLAN" Width="120px" VisibleIndex="0" FixedStyle="Left">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Date" Width="100px" FieldName="DATE" VisibleIndex="1">
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Entry Type" Width="120px" FieldName="ENTRYTYPE" VisibleIndex="2" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Location" Width="120px" FieldName="LOCATION" VisibleIndex="3" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Entity Code" Width="150px" FieldName="ENTITYCODE" VisibleIndex="4" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>                               

                                <dxe:GridViewDataTextColumn Caption="Network Name" Width="250px" FieldName="NETWORKNAME" VisibleIndex="5" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Technician" Width="200px" FieldName="TECHNICIAN" VisibleIndex="6">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn Caption="Model" Width="150px" FieldName="MODEL" VisibleIndex="7" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Serial No" Width="150px" FieldName="SERIALNO" VisibleIndex="8" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Problem Reported" Width="250px" FieldName="PROBLEMREPORTED" VisibleIndex="9" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true"></CellStyle>
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="SERVICEACTION" Width="130px" Caption="Service Action" >
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Component(s)" Width="300px" FieldName="COMPONENT" VisibleIndex="11">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="WARRANTY" Width="100px" Caption="Warranty" >
                                    <CellStyle HorizontalAlign="Left"></CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <EditFormSettings Visible="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Warranty Status" Width="150px" FieldName="WARRANTYSTATUS" VisibleIndex="13">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Stock Entry" FieldName="STOCKENTRY" Width="150px" VisibleIndex="14">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="New Model" FieldName="NEWMODEL" Width="100px" VisibleIndex="15">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="NEWSERIALNO" Caption="New Serial No" Width="100px" VisibleIndex="16">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="NEWWARRANTY" Caption="New Warranty" Width="100px" VisibleIndex="17">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn Caption="Billable" FieldName="BILLABLE" Width="100px" VisibleIndex="18">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Return Reason" FieldName="RETURNREASON" Width="200px" VisibleIndex="19">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                
                                 <dxe:GridViewDataTextColumn FieldName="PROBLEMFOUND" Caption="Problem Found" Width="200px" VisibleIndex="20">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="REMARKS" Caption="Remarks" Width="200px" VisibleIndex="21">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                  <dxe:GridViewDataTextColumn FieldName="REASON" Caption="Reason" Width="150px" VisibleIndex="22">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="RECEIVEDBY" Caption="Received By" Width="150px" VisibleIndex="23">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="RECEIVEDON" Caption="Received On" Width="100px" VisibleIndex="24">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ASSIGNEDBY" Caption="Assign By" Width="150px" VisibleIndex="25">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ASSIGNEDON" Caption="Assign On" Width="100px" VisibleIndex="26">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--mantis Issue 24780--%>
                                <dxe:GridViewDataTextColumn Caption="Repairing Status" FieldName="RepairStatus" Width="100px" VisibleIndex="27">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Rep. Status Date" FieldName="Repair_Update_date" Width="100px" VisibleIndex="28">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--end of mantis Issue 24780--%>
                                <dxe:GridViewDataTextColumn FieldName="SERVICEDBY" Caption="Serviced By" Width="150px" VisibleIndex="29">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="SERVICEDON" Caption="Serviced On" Width="100px" VisibleIndex="30">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CONFIRMDELIVERY" Caption="Confirm Delivered" Width="100px" VisibleIndex="31">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DELIVEREDBY" Caption="Delivered By" Width="150px" VisibleIndex="32">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                               <%--Mantis Issue 24496--%>
                                <dxe:GridViewDataTextColumn FieldName="DEL_CONTACTNO" Caption="Delivery Contact No" Width="200px" VisibleIndex="33">
                                    
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 24496--%>
                                <dxe:GridViewDataTextColumn FieldName="DELIVEREDON" Caption="Delivered On" Width="100px" VisibleIndex="34">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="CONFIRMDELIVERYDATE" Caption="Confirm Delivery Date" Width="100px" VisibleIndex="35">                                   
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                 <dxe:GridViewDataTextColumn FieldName="STATUS" Caption="Status" Width="100px" VisibleIndex="36">
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--Mantis Issue 25213--%>
                                <dxe:GridViewDataTextColumn FieldName="LEVELDESC" Caption="Level" Width="100px" VisibleIndex="37">
                                    
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <%--End of Mantis Issue 25213--%>
                                 
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
                        ContextTypeName="ReportSourceDataContext" TableName="SRV_ServiceRegisterReport"></dx:LinqServerModeDataSource>

                    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                    </dxe:ASPxGridViewExporter>

                    <dxe:ASPxGridViewExporter ID="exporter1" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                    </dxe:ASPxGridViewExporter>

                    <dxe:ASPxGridViewExporter ID="exporter2" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                    </dxe:ASPxGridViewExporter>
                </div>
            </div>
        </div>
    </div>

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divGenerateReport"
        Modal="True">
    </dxe:ASPxLoadingPanel>
</asp:Content>
