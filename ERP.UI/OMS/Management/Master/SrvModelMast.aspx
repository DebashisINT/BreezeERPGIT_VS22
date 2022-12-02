<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SrvModelMast.aspx.cs" Inherits="ERP.OMS.Management.Master.SrvModelMast" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style type="text/css">
        .stateDiv {
            height: 25px;
            width: 68px;
            float: left;
        }

        .dxpc-headerContent {
            color: white;
        }

        .dxgvHeader {
            border: 1px solid #2c4182 !important;
            background-color: #415698 !important;
        }

            .dxgvHeader, .dxgvHeader table {
                color: #fff !important;
            }
    </style>
    <script type="text/javascript" src="../../CentralData/JSScript/GenericJScript.js"></script>

    <script language="javascript" type="text/javascript">

        function gridRowclick(s, e) {
            //alert('hi');
            $('#gridcrmCampaign').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                }

            });
        });
    </script>

    <script type="text/javascript">
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function fn_PopUpOpen() {
            WorkingRoster();
            if (rosterstatus) {
                $('#valid').attr('style', 'display:none;');
                chfID.Set("hfID", '');
                ctxtModelDesc.SetText('');
                cPopupModel.SetHeaderText('Add Model');

                cPopupModel.Show();
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function fn_EditModel(keyValue) {
            WorkingRoster();
            if (rosterstatus) {
                grid.PerformCallback('Edit~' + keyValue);
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function fn_DeleteModel(keyValue) {
            WorkingRoster();
            if (rosterstatus) {
                jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        grid.PerformCallback('Delete~' + keyValue);
                    }
                    else {
                        return false;
                    }
                });
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function fn_btnCancel() {
            cPopupModel.Hide();
        }
        function btnSave_Model() {
            WorkingRoster();
            if (rosterstatus) {
                var Modelnm = ctxtModelDesc.GetText();
                if (Modelnm.trim() == '') {
                    $('#valid').attr('style', 'display:block;position: absolute;right: 32px;top: 17px;');
                    // alert('Please Enter Model Name');
                    ctxtModelDesc.Focus();
                }
                else {
                    var id = chfID.Get('hfID');
                    if (id == '')
                        grid.PerformCallback('saveModel~' + ctxtModelDesc.GetText());
                    else
                        grid.PerformCallback('updateModel~' + chfID.Get('hfID'));
                }
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }


        function grid_EndCallBack() {
            if (grid.cpEdit != null) {
                ctxtModelDesc.SetText(grid.cpEdit.split('~')[0]);
                var hfid = grid.cpEdit.split('~')[1];
                cPopupModel.SetHeaderText('Modify Model');
                chfID.Set("hfID", hfid);
                cPopupModel.Show();
            }

            if (grid.cpinsert != null) {
                if (grid.cpinsert == 'Success') {
                    jAlert('Saved successfully');
                    cPopupModel.Hide();
                }
                else {
                    jAlert("Error On Insertion\n'Please Try Again!!'");
                }
            }

            if (grid.cpExists != null) {
                if (grid.cpExists == 'Exists') {
                    jAlert('Duplicate value');
                    cPopupModel.Hide();
                }

            }

            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Success') {
                    jAlert('Updated successfully');
                    grid.cpUpdate = null;
                    cPopupModel.Hide();
                }
                else {
                    jAlert("Error on Updation\n'Please Try again!!'")
                    grid.cpUpdate = null;
                }
            }


            if (grid.cpDelete != null) {
                if (grid.cpDelete == 'Success') {
                    jAlert(grid.cpDelete);
                    grid.cpDelete = null;
                    grid.PerformCallback();
                }
                else {
                    jAlert(grid.cpDelete)
                    grid.cpDelete = null;
                    grid.PerformCallback();
                }
            }

            if (grid.cpImportModel != null) {
                if (grid.cpImportModel == 'Success') {
                    jAlert('Import successfully');
                    cImportPopupModel.Hide();
                    grid.PerformCallback();
                }
                else {
                    jAlert("No data found!");
                    cImportPopupModel.Hide();
                }
            }
        }

        //$(document).ready(function () {
        //    cCompanyComponentPanel.PerformCallback('BindCompanyGrid');
        //});

        function fn_ImportPopUpOpen() {
            WorkingRoster();
            if (rosterstatus) {
                $('#valid').attr('style', 'display:none;');
                cImportPopupModel.Show();
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function selectAll() {
            gridCompanyLookup.gridView.SelectRows();
        }

        function unselectAll() {
            gridCompanyLookup.gridView.UnselectRows();
        }

        function CloseGridBranchLookup() {
            gridCompanyLookup.ConfirmCurrentSelection();
            gridCompanyLookup.HideDropDown();
            gridCompanyLookup.Focus();
        }

        function btnImport_Model() {
            //if (gridCompanyLookup.GetValue() == null) {
            if ($("#ddlCompany").val() == "") {
                jAlert('Please select company');
            }
            else {
                grid.PerformCallback('ImportModel~' + $("#ddlCompany").val());
            }
        }

        var rosterstatus = false;
        function WorkingRoster() {
            $.ajax({
                type: "POST",
                url: 'SrvModelMast.aspx/CheckWorkingRoster',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify({ module_ID: '11' }),
                success: function (response) {
                    if (response.d.split('~')[0] == "true") {
                        rosterstatus = true;
                    }
                    else if (response.d.split('~')[0] == "false") {
                        rosterstatus = false;
                        $("#spnbegin").text(response.d.split('~')[1]);
                        $("#spnEnd").text(response.d.split('~')[2]);
                    }
                },
            });
        }

        function WorkingRosterClick() {
            $("#divPopHead").addClass('hide');
        }
    </script>
    <style>
        /* for pop */
        .popupWraper {
            position: fixed;
            top: 0;
            left: 0;
            height: 100vh;
            width: 100%;
            background: rgba(0,0,0,0.85);
            z-index: 10;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .popBox {
            width: 670px;
            background: #fff;
            padding: 35px;
            text-align: center;
            min-height: 350px;
            display: flex;
            align-items: center;
            flex-direction: column;
            justify-content: center;
            background: #fff url("/assests/images/popupBack.png") no-repeat top left;
            box-shadow: 0px 14px 14px rgba(0,0,0,0.56);
        }

            .popBox h1, .popBox p {
                font-family: 'Poppins', sans-serif !important;
                margin-bottom: 20px !important;
            }

            .popBox p {
                font-size: 15px;
            }

        .btn-sign {
            background: #3680fb;
            color: #fff;
            padding: 10px 25px;
            box-shadow: 0px 5px 5px rgba(0,0,0,0.22);
        }

            .btn-sign:hover {
                background: #2e71e1;
                color: #fff;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="popupWraper hide" id="divPopHead" runat="server">
        <div class="popBox">
            <img src="/assests/images/warningAlert.png" class="mBot10" style="width: 70px;" />
            <h1 id="h1heading" class="red">Your Access is Denied</h1>
            <p id="pParagraph" class="red">
                You can access this section starting from <span id="spnbegin"></span>upto <span id="spnEnd"></span>
            </p>
            <button type="button" class="btn btn-sign" onclick="WorkingRosterClick()">OK</button>
        </div>
    </div>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Model</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="Main">

            <div class="SearchArea clearfix">
                <div class="FilterSide">
                    <div style="float: left; padding-right: 5px;">
                        <% if (rights.CanAdd)
                           { %>
                        <a href="javascript:void(0);" onclick="fn_PopUpOpen()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span>Model </a>
                        <% } %>
                        <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                        <% if (rights.CanExport)
                           { %>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                        <% } %>

                        <% if (IsImport)
                           { %>
                        <a href="javascript:void(0);" onclick="fn_ImportPopUpOpen()" class="btn btn-success btn-radius"><span class="btn-icon"></span>Add Import </a>
                        <% } %>
                    </div>

                </div>

            </div>

            <div class="GridViewArea relative">
                <dxe:ASPxGridView ID="GridModel" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                    KeyFieldName="ModelID" Width="100%" OnHtmlEditFormCreated="GridModel_HtmlEditFormCreated" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                    OnCustomCallback="GridModel_CustomCallback" SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                    <%--OnHtmlRowCreated="GridModel_HtmlRowCreated"--%>
                    <SettingsSearchPanel Visible="True" Delay="5000" />
                    <Columns>
                        <dxe:GridViewDataTextColumn Caption="Model ID" FieldName="ModelID" ReadOnly="True"
                            Visible="False" VisibleIndex="0">
                            <EditCellStyle HorizontalAlign="Left">
                            </EditCellStyle>
                            <EditFormSettings Visible="False" VisibleIndex="1" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Model Description" FieldName="ModelDesc" VisibleIndex="1"
                            Width="100%">
                            <EditFormSettings Visible="True" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="1px">
                            <HeaderTemplate>
                                <span></span>
                            </HeaderTemplate>
                            <DataItemTemplate>
                                <div class='floatedBtnArea'>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="fn_EditModel('<%#Container.KeyValue %>')" class="" title="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                    <% } %>
                                    <% if (rights.CanDelete)
                                       { %>
                                    <a href="javascript:void(0);" onclick="fn_DeleteModel('<%#Container.KeyValue %>')" title="">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                    <% } %>
                                </div>
                            </DataItemTemplate>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <SettingsSearchPanel Visible="True" />
                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />

                    <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
                </dxe:ASPxGridView>
            </div>

            <div class="PopUpArea">
                <dxe:ASPxPopupControl ID="PopupModel" runat="server" ClientInstanceName="cPopupModel"
                    Width="750px" Height="100px" HeaderText="Add/Modify Model" PopupHorizontalAlign="Windowcenter"
                    PopupVerticalAlign="WindowCenter" CloseAction="closeButton" Modal="true">
                    <ContentCollection>
                        <dxe:PopupControlContentControl ID="ModelPopup" runat="server">
                            <div class="Top clearfix">
                                <div style="padding-top: 5px;" class="col-md-12">
                                    <div class="stateDiv" style="padding-top: 5px; width: 100px;">Model Desc.:<span style="color: red;">*</span></div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtModelDesc" ClientInstanceName="ctxtModelDesc" ClientEnabled="true"
                                            runat="server" Width="600px" Height="30px" MaxLength="500">
                                        </dxe:ASPxTextBox>

                                        <div id="valid" style="display: none; position: absolute; right: -4px; top: 30px;">
                                            <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="ContentDiv">
                                <div class="ScrollDiv"></div>
                                <br style="clear: both;" />
                                <div class="Footer" style="padding-left: 84px;">
                                    <div style="float: left;">
                                        <dxe:ASPxButton ID="btnSave_Model" ClientInstanceName="cbtnSave_States" runat="server"
                                            AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                            <ClientSideEvents Click="function (s, e) {btnSave_Model();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <div style="">
                                        <dxe:ASPxButton ID="btnCancel_Model" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                            <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <br style="clear: both;" />
                                </div>
                                <br style="clear: both;" />
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
                <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>
            </div>

            <div class="PopUpArea">
                <dxe:ASPxPopupControl ID="ImportPopupModel" runat="server" ClientInstanceName="cImportPopupModel"
                    Width="471px" Height="100px" HeaderText="Import Model" PopupHorizontalAlign="Windowcenter"
                    PopupVerticalAlign="WindowCenter" CloseAction="closeButton" Modal="true">
                    <ContentCollection>
                        <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <div class="Top clearfix">
                                <div style="padding-top: 5px;" class="col-md-12">
                                    <div class="stateDiv" style="padding-top: 5px; width: 100px;">From Company.:<span style="color: red;">*</span></div>
                                    <div style="padding-top: 5px;">

                                        <%--<dxe:ASPxCallbackPanel runat="server" ID="ComponentCompanyPanel" ClientInstanceName="cCompanyComponentPanel" OnCallback="ComponentCompany_Callback">
                                            <PanelCollection>
                                                <dxe:PanelContent runat="server">
                                                    <dxe:ASPxGridLookup ID="lookup_company" SelectionMode="Single" runat="server" ClientInstanceName="gridCompanyLookup"
                                                        OnDataBinding="lookup_company_DataBinding"
                                                        KeyFieldName="Company_Code" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                                        <Columns>
                                                            <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60px" Caption=" " />


                                                            <dxe:GridViewDataColumn FieldName="Company_Name" Visible="true" VisibleIndex="1" Width="200px" Caption="Company Name">
                                                            </dxe:GridViewDataColumn>
                                                            <dxe:GridViewDataColumn FieldName="DbName" Visible="true" VisibleIndex="2" Width="0" Caption="Data Base Name">
                                                            </dxe:GridViewDataColumn>
                                                            <dxe:GridViewDataColumn FieldName="Company_Code" Visible="true" VisibleIndex="3" Width="0" Caption="Company Name">
                                                            </dxe:GridViewDataColumn>
                                                        </Columns>
                                                        <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                                                            <Templates>
                                                                <StatusBar>
                                                                    <table class="OptionsTable" style="float: right">
                                                                        <tr>
                                                                            <td>
                                                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" UseSubmitBehavior="False" />
                                                                                <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" UseSubmitBehavior="False" />
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
                                        </dxe:ASPxCallbackPanel>--%>
                                        <%--Rev Sanchita--%>
                                        <%--<select id="ddlCompany" class="form-control">
                                            <option value="GTPL_INV">GTPL_INV</option>
                                            <option value="GTPL_SRV">GTPL_SRV</option>
                                            <option value="GTPL_STB">GTPL_STB</option>
                                        </select>--%>
                                        <select id="ddlCompany" class="form-control">
                                            <option value="BRZ_GTPLINV">GTPL_INV</option>
                                            <option value="GTPL">GTPL_SRV</option>
                                            <option value="BRZ_GTPLSTB">GTPL_STB</option>
                                        </select>
                                        <%--End of Rev Sanchita--%>

                                        <%--  <div id="valid" style="display: none; position: absolute; right: -4px; top: 30px;">
                                            <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required" /></div>--%>
                                    </div>
                                </div>
                            </div>
                            <div class="ContentDiv">
                                <div class="ScrollDiv"></div>
                                <br style="clear: both;" />
                                <div class="Footer" style="padding-left: 84px;">
                                    <div style="float: left;">
                                        <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtnSave_States" runat="server"
                                            AutoPostBack="False" Text="Import" CssClass="btn btn-primary">
                                            <ClientSideEvents Click="function (s, e) {btnImport_Model();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <div style="">
                                        <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                            <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <br style="clear: both;" />
                                </div>
                                <br style="clear: both;" />
                            </div>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
                <dxe:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>
            </div>
            <div class="HiddenFieldArea" style="display: none;">
                <dxe:ASPxHiddenField runat="server" ClientInstanceName="chfID" ID="hfID">
                </dxe:ASPxHiddenField>
            </div>
        </div>
    </div>
</asp:Content>
