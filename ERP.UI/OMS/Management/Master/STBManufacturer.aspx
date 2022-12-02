<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="STBManufacturer.aspx.cs" Inherits="ERP.OMS.Management.Master.STBManufacturer" %>

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
                $.each(lists, function (index, value) {
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
                ctxtManufacturerName.SetText('');
                cPopupManufacturer.SetHeaderText('Add Manufacturer');
                cPopupManufacturer.Show();
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function fn_EditManufacturer(keyValue) {
            WorkingRoster();
            if (rosterstatus) {
                grid.PerformCallback('Edit~' + keyValue);
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }

        function fn_DeleteManufacturer(keyValue) {
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
            cPopupManufacturer.Hide();
        }
        function btnSave_Manufacturer() {
            WorkingRoster();
            if (rosterstatus) {
                var Manufacturernm = ctxtManufacturerName.GetText();
                if (Manufacturernm.trim() == '') {
                    $('#valid').attr('style', 'display:block;position: absolute;right: 32px;top: 17px;');
                    ctxtManufacturerName.Focus();
                }
                else {
                    var id = chfID.Get('hfID');
                    if (id == '')
                        grid.PerformCallback('saveManufacturer~' + ctxtManufacturerName.GetText());
                    else
                        grid.PerformCallback('updateManufacturer~' + chfID.Get('hfID'));
                }
            }
            else {
                $("#divPopHead").removeClass('hide');
            }
        }


        function grid_EndCallBack() {
            if (grid.cpEdit != null) {
                ctxtManufacturerName.SetText(grid.cpEdit.split('~')[0]);
                var hfid = grid.cpEdit.split('~')[1];
                cPopupManufacturer.SetHeaderText('Modify Manufacturer');
                chfID.Set("hfID", hfid);
                cPopupManufacturer.Show();
            }

            if (grid.cpinsert != null) {
                if (grid.cpinsert == 'Success') {
                    jAlert('Saved successfully');
                    cPopupManufacturer.Hide();
                }
                else {
                    jAlert("Error On Insertion\n'Please Try Again!!'");
                }
            }

            if (grid.cpExists != null) {
                if (grid.cpExists == 'Exists') {
                    jAlert('Duplicate value');
                    cPopupManufacturer.Hide();
                }

            }

            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Success') {
                    jAlert('Updated successfully');
                    grid.cpUpdate = null;
                    cPopupManufacturer.Hide();
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
        }

        var rosterstatus = false;
        function WorkingRoster() {
            $.ajax({
                type: "POST",
                url: 'STBManufacturer.aspx/CheckWorkingRoster',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify({ module_ID: '10' }),
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
                background:#fff url("/assests/images/popupBack.png") no-repeat top left;
                box-shadow: 0px 14px 14px rgba(0,0,0,0.56);
        
            }
        .popBox  h1, .popBox p{
            font-family: 'Poppins', sans-serif !important;
            margin-bottom:20px !important;
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
            <h3>Manufacturer</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="Main">

            <div class="SearchArea clearfix">
                <div class="FilterSide">
                    <div style="float: left; padding-right: 5px;">
                        <% if (rights.CanAdd)
                           { %>
                        <a href="javascript:void(0);" onclick="fn_PopUpOpen()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span>Add New </a>
                        <% } %>
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
                    </div>
                </div>

            </div>

            <div class="GridViewArea relative">
                <dxe:ASPxGridView ID="GridManufacturer" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                    KeyFieldName="Manufacturer_Id" Width="100%" OnHtmlEditFormCreated="GridManufacturer_HtmlEditFormCreated" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                    OnCustomCallback="GridManufacturer_CustomCallback" SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false">
                    <SettingsSearchPanel Visible="True" Delay="5000" />
                    <Columns>
                        <dxe:GridViewDataTextColumn Caption="Manufacturer ID" FieldName="Manufacturer_Id" ReadOnly="True"
                            Visible="False" VisibleIndex="0">
                            <EditCellStyle HorizontalAlign="Left">
                            </EditCellStyle>
                            <EditFormSettings Visible="False" VisibleIndex="1" />
                            <Settings AllowAutoFilterTextInputTimer="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Manufacturer Name" FieldName="Manufacturer_Name" VisibleIndex="1"
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
                                    <a href="javascript:void(0);" onclick="fn_EditManufacturer('<%#Container.KeyValue %>')" class="" title="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                    <% } %>
                                    <% if (rights.CanDelete)
                                       { %>
                                    <a href="javascript:void(0);" onclick="fn_DeleteManufacturer('<%#Container.KeyValue %>')" title="">
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
                <dxe:ASPxPopupControl ID="PopupManufacturer" runat="server" ClientInstanceName="cPopupManufacturer"
                    Width="400px" Height="100px" HeaderText="Add/Modify Manufacturer" PopupHorizontalAlign="Windowcenter"
                    PopupVerticalAlign="WindowCenter" CloseAction="closeButton" Modal="true">
                    <ContentCollection>
                        <dxe:PopupControlContentControl ID="ManufacturerPopup" runat="server">
                            <div class="Top clearfix">
                                <div style="padding-top: 5px;" class="col-md-12">
                                    <div class="stateDiv col-md-3" style="padding-top: 5px;">Manufacturer:<span style="color: red;">*</span></div>
                                    <div style="padding-top: 5px;" class="col-md-8">
                                        <dxe:ASPxTextBox ID="txtManufacturerName" ClientInstanceName="ctxtManufacturerName" ClientEnabled="true"
                                            runat="server" Width="236px" MaxLength="50">
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
                                        <dxe:ASPxButton ID="btnSave_Manufacturer" ClientInstanceName="cbtnSave_States" runat="server"
                                            AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                            <ClientSideEvents Click="function (s, e) {btnSave_Manufacturer();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                    <div style="">
                                        <dxe:ASPxButton ID="btnCancel_Manufacturer" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
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

            <div class="HiddenFieldArea" style="display: none;">
                <dxe:ASPxHiddenField runat="server" ClientInstanceName="chfID" ID="hfID">
                </dxe:ASPxHiddenField>
            </div>
        </div>
    </div>
</asp:Content>
