<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CustomerMasterList.aspx.cs" Inherits="ERP.OMS.Management.Master.CustomerMasterList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        #EmployeeGrid_DXPagerBottom {
            min-width: 100% !important;
        }

        .nogap {
            padding: 0px;
        }

        label {
            display: inline-block;
            margin-bottom: 5px;
            font-weight: bold !important;
        }
    </style>
    <%--Rev work start 03.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
    <style>
        .VerySmall {
            width: 320px;
        }
    </style>
    <%--Rev work close 03.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
    <link href="/assests/css/custom/PMSStyles.css" rel="stylesheet" />
    <script src="Js/CustomerMasterList.js?v=70.4"></script>
    <%--Rev work start 03.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
    <script type="text/javascript">
        function ImportUpdatePopOpenCustomerTarget(e) {

            $("#modalimport").modal('show');
        }
        function ViewLogData() {
            cGvJvSearch.Refresh();
        }
        function ShowLogData(haslog) {
            debugger;
            $('#btnViewLog').click();
        }
    </script>
    <%--Rev work close 03.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title" id="td_contact1" runat="server">
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Customers / Clients"></asp:Label>
            </h3>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100">

            <tr>
                <td>

                    <div class="SearchArea">
                        <div class="FilterSide">
                            <div style="float: left; padding-right: 5px;">
                                <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span><span>Add New</span> </a>

                                <% } %>
                            </div>

                            <div class="pull-left">

                                <% if (rights.CanExport)
                                   { %>
                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                    <asp:ListItem Value="5">XLSX</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>

                                </asp:DropDownList>
                                <% } %>
                            </div>
                            

                        </div>
                        <div class="ExportSide pull-right">
                            <div>
                            </div>
                        </div>
                        <%--Rev work start 03.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
                         <div class="pull-left">
                            <asp:LinkButton ID="lnlDownloaderexcel" runat="server" OnClick="lnlDownloaderexcel_Click" CssClass="btn btn-info btn-radius pull-right mBot0">Download Format</asp:LinkButton>
                        </div>
                        <div class="pull-left">
                            <button type="button" onclick="ImportUpdatePopOpenCustomerTarget();" class="btn btn-primary btn-radius">Import(Add/Update)</button>
                            <button type="button" class="btn btn-warning btn-radius" data-toggle="modal" data-target="#modalSS" id="btnViewLog" onclick="ViewLogData();">View Log</button>
                        </div>
                        <%--Rev work close 03.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
                    </div>
                </td>
            </tr>
            <tr id="TrFilter" style="display: none">
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" CssClass="water" Text="Name" ToolTip="Name"
                                    Font-Size="12px" Width="119px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBranchName" runat="server" CssClass="water" Text="Branch Name"
                                    ToolTip="Branch Name" Font-Size="12px" Width="100px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCode" runat="server" CssClass="water" Text="Code" ToolTip="Code"
                                    Font-Size="12px" Width="54px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TxtTCODE" runat="server" CssClass="water" Text="Trade.Code" ToolTip="Trade.Code"
                                    Font-Size="12px" Width="79px"></asp:TextBox>
                                <asp:HiddenField ID="TxtSeg" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPAN" runat="server" CssClass="water" Text="PAN No." ToolTip="PAN No."
                                    Font-Size="12px" Width="79px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRelationManager" runat="server" CssClass="water" Text="R. Manager"
                                    ToolTip="R. Manager" Font-Size="12px" Width="85px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferedBy" runat="server" CssClass="water" Text="Email" ToolTip="Email"
                                    Font-Size="12px" Width="92px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhNumber" runat="server" CssClass="water" Text="Ph. Number" ToolTip="Ph. Number"
                                    Font-Size="12px" Width="90px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactStatus" runat="server" CssClass="water" Text="Contact Status"
                                    ToolTip="Contact Status" Font-Size="12px" Width="79px"></asp:TextBox>
                            </td>

                            <td>
                                <input id="btnSearch" type="button" value="Search" class="btnUpdate" style="height: 21px"
                                    onclick="btnSearch_click()" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="relative">
                    <dxe:ASPxGridView ID="EmployeeGrid" runat="server" KeyFieldName="cnt_Id" AutoGenerateColumns="False" OnDataBound="EmployeeGrid_DataBound"
                        DataSourceID="EntityServerModeDataSource" Width="100%" ClientInstanceName="grid" OnCustomJSProperties="EmployeeGrid_CustomJSProperties" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        OnCustomCallback="EmployeeGrid_CustomCallback" OnHtmlRowCreated="EmployeeGrid_HtmlRowCreated" SettingsBehavior-AllowFocusedRow="true" Settings-HorizontalScrollBarMode="Visible" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <ClientSideEvents EndCallback="function(s,e) { ShowError(s.cpInsertError);
                                                                                                 }"
                            RowClick="gridcrmCampaignclick" />


                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" EditFormColumnCount="3" />
                        <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />
                        <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" FilterRowMode="Auto" />
                        <SettingsText PopupEditFormCaption="Add/ Modify Employee" ConfirmDelete="Confirm delete?" />
                        <StylesPager>
                            <Summary Width="100%">
                            </Summary>
                        </StylesPager>
                        <Columns>

                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="Id">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="cnt_Id" SortOrder="Descending">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="2" FieldName="Name" Width="150px">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="BranchName" Width="140px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Code" Caption="Unique ID" Width="120px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="phf_phoneNumber" Caption="Phone Number" Width="110px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="phf_Alt_phoneNumber" Caption="Alt. Ph. No.(s)" Width="190px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="PanNumber" Caption="PAN Number" Width="110px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="city_name" Caption="District" Width="150px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <%-- <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="Status" Caption="Contact Status">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>



                            <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="Activetype" Caption="Status" Width="100px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>



                            <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="gstin" Caption="GSTIN" Width="140px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="TaxEntityType" Caption="Tax Entity Type" Width="180px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="Assign_To" Caption="Assign To" Width="120px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="EnterBy" Caption="Created By" Width="120px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="CreatedOn" Caption="Created On" Width="120px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="ModifyUser" Caption="Last Modified By" Width="120px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="ModifyDateTime" Caption="Last Modified On" Width="120px">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" AutoFilterCondition="Contains" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="16" CellStyle-HorizontalAlign="Center" Width="0">

                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <HeaderTemplate></HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                        <%  if (rights.CanCreateActivity)
                                            {%>

                                        <a href="javascript:void(0);" onclick="OnCreateActivityClick('<%# Eval("Id") %>','<%# Eval("cnt_id") %>','<%# Eval("Status") %>')" title="Create Activity" class="pad" style="text-decoration: none;">
                                            <span class='ico ColorThree'><i class='fa fa-wrench'></i></span><span class='hidden-xs'>Create Activity</span>
                                        </a>
                                        <% }
                                            if (rights.CanEdit)
                                            {%>
                                        <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Eval("cnt_id") %>')" title=" More Info" class="pad" style="text-decoration: none;">
                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span>
                                        </a>

                                        <% }
                                            if (rights.CanView)
                                            {%>
                                        <a href="javascript:void(0);" onclick="ClickVIewInfo('<%# Eval("Id") %>')" title="View" class="pad" style="text-decoration: none;">
                                            <span class='ico ColorFour'><i class='fa fa-eye' aria-hidden='true'></i></span><span class='hidden-xs'>View</span>
                                        </a>


                                        <% }
                                            if (rights.CanContactPerson)
                                            {%>

                                        <a href="javascript:void(0);" onclick="OnContactInfoClick('<%#Eval("Id") %>','<%#Eval("Name") %>')" title="Add Contact Person" class="pad" style="text-decoration: none;">
                                            <%--<img src="../../../assests/images/show.png" />--%>
                                            <span class='ico ColorSix'><i class='fa fa-users' aria-hidden='true'></i></span><span class='hidden-xs'>View</span>
                                        </a>
                                        <% }
                                           if (rights.CanIndustry)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnAddBusinessClick('<%#Eval("Id") %>','<%#Eval("Name") %>')" title="Map Industry" class="pad" style="text-decoration: none;">
                                            <%--<img src="../../../assests/images/icoaccts.gif" />--%><span class='ico ColorSix'><i class='fa fa-map' aria-hidden='true'></i></span><span class='hidden-xs'>Map Industry</span>  </a>

                                        <%     }
                                           if (rights.CanDelete)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnDelete('<%# Eval("Id") %>')" title="Delete" class="pad">
                                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                        <%   }%>

                                        <%  if (rights.CanBudget)
                                            { %>

                                        <a href="javascript:void(0);" onclick="OnBudgetopen('<%# Eval("cnt_Id") %>')" title="Budget" class="pad">
                                            <span class='ico ColorSeven'><i class='fa fa-briefcase' aria-hidden='true'></i></span><span class='hidden-xs'>Budget</span></a>

                                        <%   }%>

                                        <%  if (rights.CanAssignTo)
                                            { %>

                                        <a href="javascript:void(0);" onclick="OnAssignTo('<%# Eval("Id") %>','<%#Eval("Name") %>','<%#Eval("EnterBy") %>')" title="Assign To" class="pad">
                                            <span class='ico editColor'><i class='fa fa-user' aria-hidden='true'></i></span><span class='hidden-xs'>Assign To</span></a>

                                        <%   }%>
                                        <%  if (rights.CanAdd)
                                            { %>

                                        <a href="javascript:void(0);" onclick="CopyToCustomer('<%# Eval("cnt_id") %>')" title="Assign To" class="pad">
                                            <span class='ico editColor'><i class='fa fa-files-o' aria-hidden='true'></i></span><span class='hidden-xs'>Copy</span></a>
                                        <%   }%>
                                        <% if(ShowSegment.ToUpper()=="1")  {%>
                                        <a href="javascript:void(0);" onclick="ClickOnSegments('<%# Eval("cnt_id") %>')" title="Segment Codes" class="pad" style="text-decoration: none;">
                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Segment Codes</span>
                                        </a>
                                        <%   }%>
                                    </div>
                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="16" FieldName="user_name"
                                Caption="Created User">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <SettingsCookies Enabled="true" StorePaging="true" Version="1.0" />
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>

                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>

        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_CustomerMasterList" />

        <asp:SqlDataSource ID="EmployeeDataSource" runat="server" SelectCommand="">
            <SelectParameters>
                <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
            </SelectParameters>


        </asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>


        <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupbudget" Height="500px"
            Width="1310px" HeaderText="Budget" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>

            <ClientSideEvents CloseUp="BudgetAfterHide" />
        </dxe:ASPxPopupControl>


        <%--------Assign to Subhra 16-05-2019------------------%>
        <div class="modal fade pmsModal w30" id="AssignToModel" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Assign To</h4>
                    </div>
                    <div class="modal-body">
                        <div>
                            <dxe:ASPxLabel ID="lblenteredby" runat="server" Text="">
                            </dxe:ASPxLabel>
                        </div>
                        <div class="clearfix">
                            <div class="visF">
                                <div id="td_lAssignto" class="labelt">
                                    <div class="visF">
                                        <dxe:ASPxLabel ID="lblAssignTo" runat="server" Text="Assign To">
                                        </dxe:ASPxLabel>
                                    </div>

                                </div>
                                <div id="td_dAssignto">
                                    <div class="visF">
                                        <asp:DropDownList ID="cmbAssignTo" runat="server" TabIndex="1" Width="100%">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="">
                                <label>
                                    <dxe:ASPxLabel ID="lblRemarks" runat="server" Text="Remarks" CssClass="pdl8"></dxe:ASPxLabel>
                                    <span style="color: red;">*</span>

                                </label>
                                <div style="position: relative;">
                                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Columns="20" Rows="4" CssClass="form-control" TabIndex="2" MaxLength="500" Width="100%">
                                    </asp:TextBox>
                                    <asp:HiddenField ID="hdncopytoproduct" runat="server" />
                                    <asp:HiddenField ID="hdnanniversarydate" runat="server" />
                                    <asp:HiddenField ID="hdndob" runat="server" />
                                    <asp:HiddenField ID="hdnsex" runat="server" />
                                    <asp:HiddenField ID="hdncustomerucc" runat="server" />
                                </div>
                            </div>
                        </div>


                    </div>
                    <div class="modal-footer">
                        <%--<button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('CustomerSource')">Assign</button>--%>
                        <button type="button" id="btnAssign" class="btnOkformultiselection btn btn-success" onclick="Assign()">Assign</button>
                        <button type="button" id="btnUnAssign" class="btnOkformultiselection btn btn-danger" onclick="UnAssign()">Unassign</button>
                    </div>
                </div>
            </div>
        </div>

        <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanelAssign" ClientInstanceName="cCallbackPanelAssign" OnCallback="PopupAssign_Callback">
            <PanelCollection>
                <dxe:PanelContent runat="server">
                </dxe:PanelContent>
            </PanelCollection>
            <ClientSideEvents EndCallback="CallbackPanelEndCall" />
        </dxe:ASPxCallbackPanel>

        <%--------------Assign to------------------%>

        <dxe:ASPxPopupControl ID="DirectAddCustPopup" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="AspxDirectAddCustPopup" Height="680px"
            Width="1100px" HeaderText="Add New Customer" Modal="true" AllowResize="false" ResizingMode="Postponed">

            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

        <asp:HiddenField ID="hidIsLigherContactPage" runat="server" />
    </div>
    <dxe:ASPxPopupControl ID="AspxDirectCustomerViewPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CAspxDirectCustomerViewPopup" Height="650px"
        Width="1020px" HeaderText="Customer View" Modal="true" AllowResize="false">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <%--rev rajdip--%>
    <dxe:ASPxPopupControl ID="ASPxDirectCustomerCopyToProductPopup" runat="server"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="CASPxDirectCustomerCopyToProductPopup" Height="650px"
        Width="1020px" HeaderText="Customer Copy" Modal="true" AllowResize="false">

        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="panel panel-default">
                    <div class="panel-body">

                        <div class="row">

                            <%--  Chinmoy start 02-04-2020--%>
                            <%--Chinmoy added 02-04-2020 start--%>
                            <div class="col-md-2 " id="ddl_Num" runat="server" style="display: none">
                                <label>
                                    <dxe:ASPxLabel ID="lbl_NumberingScheme" Width="120px" runat="server" Text="Numbering Scheme">
                                    </dxe:ASPxLabel>
                                </label>
                                <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%">
                                </asp:DropDownList>
                            </div>




                            <div class="col-md-2 " runat="server" id="dvCustDocNo" style="display: none">
                                <label>
                                    <dxe:ASPxLabel ID="lbl_CustDocNo" runat="server" Text="Unique ID" Width="">
                                    </dxe:ASPxLabel>
                                    <span style="color: red">*</span>
                                </label>

                                <dxe:ASPxTextBox ID="txt_CustDocNo" runat="server" ClientInstanceName="ctxt_CustDocNo" Width="100%" MaxLength="50">

                                    <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />
                                </dxe:ASPxTextBox>

                            </div>
                            <%-- End--%>
                            <%--End--%>

                            <div class="col-md-4" runat="server" id="dvIdType">
                                <label>ID Type</label><label style="color: red">*</label>
                                <select name="copytocustomerIdentityType" class="form-control input-sm" id="copytocustomerIdentityTypeIdType">
                                    <option value="0" selected="selected">Select</option>
                                    <option value="1">Phone</option>
                                    <option value="2">PAN No.</option>
                                    <option value="3">Aadhar No.</option>

                                </select>
                            </div>
                            <div class="col-md-4" runat="server" id="dvUniqueId">


                                <label>Unique Id</label>
                                <label style="color: red">*</label>
                                <input class="form-control input-sm" placeholder="Unique ID " maxlength="50" id="copytocustomeruniqueId" type="text" data-toggle="tooltip" title="Unique ID" />
                            </div>
                            <div class="col-md-4">
                                <label>Name</label><label style="color: red">*</label>
                                <input class="form-control input-sm" placeholder="Name" id="copytocustomerName" type="text" maxlength="50" data-toggle="tooltip" title="Name" />
                            </div>
                          <%-- REV SRIJEETA mantis issue 0024515 --%>
                            </div>
                            <div class="col-md-4" runat="server" id="Div1">


                                <label>Alternative Code</label>
                                <input class="form-control input-sm" placeholder="Alternative Code " maxlength="100" id="copytocustomeruniqueIdALTCODE" type="text" data-toggle="tooltip" title="Alternative Code" />
                            </div>
                           
                            <%-- End OF REV SRIJEETA mantis issue 0024515--%>

                            <!--<div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-12">
                                        <label>GSTIN </label>
                                    </div>
                                    <div class="clear" />
                                    <div class="col-md-3">
                                        <input class="form-control input-sm case-upp gfdgdfg" id="GSTIN1" type="text" maxlength="2" data-toggle="tooltip" title="GSTIN">
                                    </div>
                                    <div class="col-md-5">
                                        <input class="form-control input-sm case-upp" id="GSTIN2" type="text" maxlength="10" data-toggle="tooltip" title="GSTIN">
                                    </div>
                                    <div class="col-md-4">
                                        <input class="form-control input-sm case-upp" id="GSTIN3" type="text" maxlength="3" data-toggle="tooltip" title="GSTIN">
                                    </div>
                                </div>
                            </div>-->
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-12">
                                        <label>GSTIN </label>
                                    </div>

                                    <div class="col-md-3">
                                        <input class="form-control input-sm case-upp gfdgdfg" id="copytocustomerGSTIN1" type="text" maxlength="2" data-toggle="tooltip" title="GSTIN" />
                                    </div>
                                    <div class="col-md-5">
                                        <input class="form-control input-sm case-upp" id="copytocustomerGSTIN2" type="text" maxlength="10" data-toggle="tooltip" title="GSTIN" />
                                    </div>
                                    <div class="col-md-4">
                                        <input class="form-control input-sm case-upp" id="copytocustomerGSTIN3" type="text" maxlength="3" data-toggle="tooltip" title="GSTIN" />
                                    </div>
                                </div>
                            </div>

                            <div style="margin-bottom: 10px">
                                <input type="checkbox" id="chkTCSAppli" />
                                <label id="lblTCSApplicable">TCS Applicable</label>

                            </div>

                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="panel panel-default">
                                    <div class="panel-header">
                                        <label style="padding-left: 10px; padding-top: 6px;">Billing Details</label>
                                    </div>
                                    <div class="panel-body">
                                        <div class="col-md-6 nogap">
                                            <label>PIN</label><label style="color: red">*</label>
                                            <input class="form-control input-sm" placeholder="Pin Code" id="copytocustomerbillingpinCode" maxlength="6" onblur="BillingPinChange()" type="text" data-toggle="tooltip" title="Pin Code">
                                        </div>
                                        <div class="col-md-6">
                                            <label>Alternative Phone</label>
                                            <input class="form-control input-sm" placeholder="Alternative Phone" id="copytobillingphnocustomer" maxlength="12" type="text" data-toggle="tooltip" title="Alternative Phone">
                                        </div>
                                        <div class="clear"></div>


                                        <div class="col-md-12 nogap">
                                            <label>Address </label>
                                            <label style="color: red">*</label>
                                            <input class="form-control input-sm" placeholder="Address 1" id="copytocustomerBillingAddress1" maxlength="50" type="text" data-toggle="tooltip" title="Address 1">
                                        </div>
                                        <div class="col-md-12 nogap">
                                            <input class="form-control input-sm" placeholder="Address 2" id="copytocustomerBillingAddress2" maxlength="50" type="text" data-toggle="tooltip" title="Address 2">
                                        </div>

                                        <div class="col-md-4 nogap">
                                            <label>Country</label>
                                            <input class="form-control input-sm " placeholder="Country" disabled id="copytocustomerbillingCountry" type="text" data-toggle="tooltip" title="Country">
                                        </div>

                                        <div class="col-md-4">
                                            <label>State</label>
                                            <input class="form-control input-sm" placeholder="State" disabled id="copytocustomerbillingState" type="text" data-toggle="tooltip" title="State">
                                        </div>

                                        <div class="col-md-4">
                                            <label>City/District</label>
                                            <input class="form-control input-sm" placeholder="City/District" disabled id="copytocustomerCity" type="text" data-toggle="tooltip" title="City/District" />
                                        </div>
                                        <!-- <a href="#" onclick="billingCopyToshipping()">-->
                                        <a class="[ form-group ]" href="#" onclick="javascript: BillingCheckChangeCopytoproduct()">
                                            <input type="checkbox" name="fancy-checkbox-success" id="copytocustomerfancy-checkbox-successforcopy" style="display: none;" autocomplete="off" />
                                            <div class="[ btn-group ]">
                                                <label for="fancy-checkbox-success" class="[ btn btn-success ]">
                                                    <span class="[ glyphicon glyphicon-ok ]"></span>
                                                    <span></span>
                                                </label>
                                                <label for="fancy-checkbox-success" class="[ btn btn-default active ]">
                                                    Ship To Same Address
                                                </label>
                                            </div>
                                        </a>
                                        <!--</a>-->
                                    </div>
                                </div>
                                <div style="margin-bottom: 10px">

                                    <label id="lblgrpName">Group</label>
                                    <select class="form-control" id="copytocustomerGroupCust">
                                        <option value="0" selected="selected">Select</option>
                                    </select>
                                </div>

                                <div style="margin-bottom: 10px">

                                    <%--         <label id="lblCopyAll">Copy All</label>--%>
                                    <%--       <a class="[ form-group ]" href="#" onclick="javascript: Copyall()">
                                            <input type="checkbox" name="fancy-checkbox-success" id="chkCopyalltocustomer" style="display:none;" autocomplete="off" />
                                            <div class="[ btn-group ]">
                                                <label for="fancy-checkbox-success" class="[ btn btn-success ]">
                                                    <span class="[ glyphicon glyphicon-ok ]"></span>
                                                    <span> </span>
                                                </label>
                                                <label for="fancy-checkbox-success" class="[ btn btn-default active ]">
                                                  Copy All
                                                </label>
                                            </div>
                                        </a>--%>
                                    <label id="lblCopyAll">Copy All To Customer</label>
                                    <input type="checkbox" id="chkcopy" />
                                </div>
                            </div>


                            <div class="col-md-6" id="shippingId">
                                <div class="panel panel-default">
                                    <div class="panel-header">
                                        <label style="padding-left: 10px; padding-top: 6px;">Shipping Details</label>
                                    </div>
                                    <div class="panel-body">
                                        <div class="col-md-12 nogap">
                                            <label>Contact Person</label>
                                            <input class="form-control input-sm" placeholder="Contact Person" id="copytocustomershippingcontactperson" maxlength="40" type="text" data-toggle="tooltip" title="Contact Person" />
                                        </div>

                                        <div class="col-md-6 nogap">
                                            <label>PIN</label><label style="color: red">*</label>
                                            <input class="form-control input-sm" placeholder="Pin Code" id="copytocustomershippingpinCode" maxlength="6" onblur="shippingPinChange()" id="Text4" type="text" data-toggle="tooltip" title="Pin Code" />
                                        </div>
                                        <div class="col-md-6">
                                            <label>Alternative Phone</label>
                                            <input class="form-control input-sm" placeholder="Alternative Phone" id="copytocustomerShippingPhone" maxlength="12" type="text" data-toggle="tooltip" title="Alternative Phone" />
                                        </div>
                                        <div class="clear"></div>



                                        <div class="col-md-12 nogap">
                                            <label>Address 1</label><label style="color: red">*</label>
                                            <input class="form-control input-sm" placeholder="Address 1" id="copytocustomershippingAddress1" maxlength="50" type="text" data-toggle="tooltip" title="Address 1" />
                                        </div>
                                        <div class="col-md-12 nogap">
                                            <input class="form-control input-sm" placeholder="Address 2" id="copytocustomershippingAddress2" maxlength="50" type="text" data-toggle="tooltip" title="Address 2" />
                                        </div>

                                        <div class="col-md-4 nogap">
                                            <label>Country</label>
                                            <input class="form-control input-sm " placeholder="Country" disabled id="copytocustomershippingCountry" type="text" data-toggle="tooltip" title="Country" />
                                        </div>

                                        <div class="col-md-4">
                                            <label>State</label>
                                            <input class="form-control input-sm" placeholder="State" disabled id="copytocustomershippingState" type="text" data-toggle="tooltip" title="State" />
                                        </div>

                                        <div class="col-md-4">
                                            <label>City/District</label>
                                            <input class="form-control input-sm" placeholder="City/District" disabled id="copytocustomershippingCity" type="text" data-toggle="tooltip" title="City/District" />
                                        </div>



                                        <a class="[ form-group ]" href="#" onclick="javascript: ShippingCheckChangeforcopy()">
                                            <input type="checkbox" style="display: none;" name="fancy-checkbox-successShippingcopytoproductforcopy" id="fancy-checkbox-successShippingcopytoproductforcopy" autocomplete="off" />
                                            <div class="[ btn-group ]">
                                                <label for="fancy-checkbox-successShippingcopytoproductforcopy" class="[ btn btn-success ]">
                                                    <span class="[ glyphicon glyphicon-ok ]"></span>
                                                    <span></span>
                                                </label>
                                                <label for="fancy-checkbox-successShippingcopytoproductforcopy" class="[ btn btn-default active ]">
                                                    Bill To Same Address
                                                </label>
                                            </div>
                                        </a>


                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="clear"></div>
                        <div class="col-md-12 pull-left nogap">
                            <input type="button" class="btn btn-primary" value="Save" onclick="SaveCustomercopy()" />
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>

                <asp:HiddenField ID="hdnAutoNumStg" runat="server" />
                <asp:HiddenField ID="hdnTransactionType" runat="server" />
                <asp:HiddenField ID="hddnDocNo" runat="server" />
                <asp:HiddenField ID="hdnNumberingId" runat="server" />

                <asp:HiddenField ID="hdnDuplicateGSTN" runat="server" />
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <%--end rev rajdip--%>
    <%--Rev work start 03.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
    <div class="modal fade" id="modalimport" role="dialog">
        <div class="modal-dialog VerySmall">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Select File to Import (Add/Update)</h4>
                </div>
                <div class="modal-body">

                    <div class="col-md-12">
                        <div id="divproduct">

                            <div>
                                <asp:FileUpload ID="OFDBankSelect" accept=".xls,.xlsx" runat="server" Width="100%" />
                                <div class="pTop10  mTop5">
                                    <asp:Button ID="BtnSaveexcel" runat="server" Text="Import(Add/Update)"  OnClick="BtnSaveexcel_Click1" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </div>
     <div class="modal fade" id="modalSS" role="dialog">
        <div class="modal-dialog fullWidth">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Log</h4>
                </div>
                <div class="modal-body">

                    <dxe:ASPxGridView ID="GvJvSearch" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                        ClientInstanceName="cGvJvSearch" KeyFieldName="CustLogId" Width="100%" OnDataBinding="GvJvSearch_DataBinding" Settings-VerticalScrollBarMode="Auto" Settings-VerticalScrollableHeight="400">

                        <SettingsBehavior ConfirmDelete="false" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                            <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                            <Footer CssClass="gridfooter"></Footer>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="CustLogId" Caption="LogID" SortOrder="Descending">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="CreatedDatetime" Caption="Date" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="CustomerCode" Caption="Customer Code" Width="10%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="LoopNumber" Caption="Row Number" Width="13%">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="CustName" Width="8%" Caption="Customer Name">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="FileName" Width="14%" Caption="File Name">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Description" Caption="Description" Width="10%" Settings-AllowAutoFilter="False">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="Status" Caption="Status" Width="14%" Settings-AllowAutoFilter="False">
                                <CellStyle Wrap="True" CssClass="gridcellleft"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsSearchPanel Visible="false" />
                        <SettingsPager NumericButtonCount="200" PageSize="200" ShowSeparators="True" Mode="ShowPager">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="200,400,600" />
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                    </dxe:ASPxGridView>
                </div>
            </div>
        </div>
    </div>
    <%--Rev work close 03.06.2022 Mantise issue:0024783: Customer & Product master import required for ERP--%>
    <asp:HiddenField runat="server" ID="hdnDocumentSegmentSettings" />
   
</asp:Content>
