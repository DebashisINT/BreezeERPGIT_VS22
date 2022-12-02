<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ApprovedSalesOrderNew.aspx.cs" Inherits="ERP.OMS.Management.Activities.ApprovedSalesOrderNew" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../../assests/css/custom/jquery.confirm.css" rel="stylesheet" />
    <%--<script src="JS/ApproveSaleasOrder.js?v=1.8"></script>--%>
    <link href="CSS/SalesOrderEntityList.css" rel="stylesheet" />

    <style>
        .plhead a {
            font-size: 16px;
            padding-left: 10px;
            position: relative;
            width: 100%;
            display: block;
            padding: 9px 10px 5px 10px;
        }

            .plhead a > i {
                position: absolute;
                top: 11px;
                right: 15px;
            }

        #accordion {
            margin-bottom: 10px;
        }

        .companyName {
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 15px;
        }

        .plhead a.collapsed .fa-minus-circle {
            display: none;
        }

        .blocklableTbl > tbody > tr > td > label {
            display: block;
            margin-bottom: 4px;
        }

            .blocklableTbl > tbody > tr > td > label > span {
                font-weight: 600;
            }

        .blocklableTbl > tbody > tr > td > select {
            margin: 0 !important;
        }

        .blocklableTbl > tbody > tr > td:not(:first-child) {
            padding-left: 25px;
        }

        .btnTD {
            min-width: 200px;
            padding-top: 19px;
        }

            .btnTD > .btn {
                margin: 0 !important;
            }

        .badge {
            display: inline-block;
            min-width: 10px;
            padding: 5px 8px;
            font-size: 12px;
            font-weight: bold;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            background-color: #929ef0;
            border-radius: 10px;
        }

        .btn .badge {
            background: #fff;
            color: #333;
            margin-left: 4px;
            font-size: 14px;
        }

        .btn-badge-color1:hover, .btn-badge-color2:hover, .btn-badge-color3:hover, .btn-badge-color4:hover, .btn-badge-color5:hover, .btn-badge-color6:hover,
        .btn-badge-color1:focus, .btn-badge-color2:focus, .btn-badge-color3:focus, .btn-badge-color4:focus, .btn-badge-color5:focus, .btn-badge-color6:focus {
            color: #fff;
            opacity: 0.8;
        }

        .btn-badge-color1 {
            /*background:chocolate;
            color:#fff;*/
            background: #d9c015;
            color: #fff;
        }

        .btn-badge-color2 {
            background: #30cb57;
            color: #fff;
        }

        .btn-badge-color3 {
            background: #ff6a00;
            color: #fff;
        }

        .btn-badge-color4 {
            background: #7a6fd6;
            color: #fff;
        }

        .btn-badge-color5 {
            background: #6272e2;
            color: #fff;
        }

        .btn-badge-color6 {
            background: #3366FF;
            color: #fff;
        }

        .nocursor {
            cursor: default;
        }

        .auto-style1 {
            height: 145px;
        }

        .m0 {
            margin-top: 0 !important;
        }

        .mBot0 {
            margin-bottom: 0 !important;
        }
    </style>


    <script>

        function gridRowclick(s, e) {
            $('#GrdOrder').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                $.each(lists, function (index, value) {
                    setTimeout(function () {
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

        function updateGridByDate() {
            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else if (ccmbBranchfilter.GetValue() == null) {
                jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
            }
            else {
                localStorage.setItem("FromDateSalesOrder", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDateSalesOrder", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("OrderBranch", ccmbBranchfilter.GetValue());
                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                $("#hFilterType").val("");

                cGridApprovedSalesOrder.Refresh();
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Approved Sales Order</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px">
            <tr>
                <td>
                    <label>From </label>
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <label>To </label>
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
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
        <div class="clearfix">


            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="drdExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>

        </div>
    </div>

    <div class="GridViewArea relative">
        <div class="makeFullscreen ">
            <span class="fullScreenTitle">Approved Sales Order</span>
            <dxe:ASPxGridView ID="GridApprovedSalesOrder" runat="server" KeyFieldName="SlNo" AutoGenerateColumns="false" DataSourceID="EntityServerModeDataSource"
                SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" Width="100%" ClientInstanceName="cGridApprovedSalesOrder"
                SettingsBehavior-AllowFocusedRow="true">
                <Columns>
                    <dxe:GridViewDataTextColumn Caption="Document No." FieldName="OrderNo" VisibleIndex="1" Width="140px"
                        FixedStyle="Left" Settings-ShowFilterRowMenu="True">
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Order_Date" VisibleIndex="2" Width="150px"
                        FixedStyle="Left" Settings-ShowFilterRowMenu="True">
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName" VisibleIndex="3" Width="300px"
                        FixedStyle="Left" Settings-ShowFilterRowMenu="True">
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BranchName" VisibleIndex="4" Width="250px"
                        FixedStyle="Left" Settings-ShowFilterRowMenu="True">
                    </dxe:GridViewDataTextColumn>

                </Columns>
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>
            </dxe:ASPxGridView>

            <asp:HiddenField ID="hiddenedit" runat="server" />
            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="V_GetApproveSalesOrderEntityList" />
        </div>
    </div>

    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hdnIsUserwiseFilter" runat="server" />
        <asp:HiddenField ID="hddnKeyValue" runat="server" />
        <asp:HiddenField ID="hddnCancelCloseFlag" runat="server" />
        <asp:HiddenField ID="hddnPrintButton" runat="server" />

        <asp:HiddenField ID="hFilterType" runat="server" />
    </div>

    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cReadyToInvoicePopup"
            Width="500px" HeaderText="Update Stage" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="Top clearfix">
                        <table style="width: 94%">
                            <tr>
                                <td>Stage<span style="color: red">*</span></td>
                                <td class="relative">
                                    <dxe:ASPxComboBox ID="cmbStage" runat="server" ClientInstanceName="ccmbStage" Width="100%">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Remarks<span style="color: red">*</span></td>
                                <td class="relative" colspan="2" style="padding-top: 10px;">
                                    <dxe:ASPxMemo ID="txtRemarks" runat="server" Width="100%" Height="50px" ClientInstanceName="ctxtRemarks"></dxe:ASPxMemo>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="2" style="padding-top: 10px;">
                                    <input id="btnFeedbackSaves" class="btn btn-primary" onclick="ReadyToInvoice_save()" type="button" value="Save" />&nbsp;&nbsp;
                                    <input id="btnFeedbackCancels" class="btn btn-danger" onclick="CancelReadyToInvoice()" type="button" value="Cancel" />

                                    <asp:HiddenField ID="hdnOrderID" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>

</asp:Content>
