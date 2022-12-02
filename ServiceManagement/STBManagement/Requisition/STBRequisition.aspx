<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="STBRequisition.aspx.cs" Inherits="ServiceManagement.STBManagement.Requisition.STBRequisition" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .padTab > tbody > tr > td {
            padding-right: 8px;
            vertical-align: middle;
        }
    </style>
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
    <script>
        function OnAddButtonClick() {
            WorkingRoster();
            if (rosterstatus) {
                var url = '/STBManagement/Requisition/STBRequisitionAdd.aspx?Key=Add';
                //OnMoreInfoClick(url, "Add New Accout", '920px', '500px', "Y");
                window.location.href = url;
            }
            else {
                // jAlert("Working period is over. Try in next working period.");
                $("#divPopHead").removeClass('hide');
            }
        }
        function ClickOnEdit(id) {
            WorkingRoster();
            if (rosterstatus) {
                location.href = "STBRequisitionAdd.aspx?id=" + id + "&Key=edit";
            }
            else {
                //jAlert("Working period is over. Try in next working period.");
                $("#divPopHead").removeClass('hide');
            }
        }

        function ClickOnView(id) {
            WorkingRoster();
            if (rosterstatus) {
                location.href = "STBRequisitionAdd.aspx?id=" + id + "&Key=view";
            }
            else {
                // jAlert("Working period is over. Try in next working period.");
                $("#divPopHead").removeClass('hide');
            }
        }
        var HearderId = 0;
        function onPrintJv(id) {
            WorkingRoster();
            if (rosterstatus) {
                $.ajax({
                    type: "POST",
                    url: 'STBRequisitionAdd.aspx/UpdatePrintCount',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    data: JSON.stringify({ STBRequisitionID: id }),
                    success: function (response) {

                    },
                    error: function (response) {
                        //jAlert(response);
                    }
                });


                HearderId = id;
                cSelectPanel.cpSuccess = "";
                cDocumentsPopup.Show();
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
                cSelectPanel.PerformCallback('Bindalldesignes');
                $('#btnOK').focus();
            }
            else {
                // jAlert("Working period is over. Try in next working period.");
                $("#divPopHead").removeClass('hide');
            }
        }

        function cSelectPanelEndCall(s, e) {

            if (cSelectPanel.cpSuccess != "") {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'STBRequisition';
                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            window.open("../../OMS/Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + HearderId + '&PrintOption=' + TotDocument[i], '_blank')
                        }
                    }
                }
            }

            if (cSelectPanel.cpSuccess == "") {
                if (cSelectPanel.cpChecked != "") {
                    jAlert('Please check Original For Recipient and proceed.');
                }
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
            }
        }

        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }

        function OnClickDelete(val) {
            WorkingRoster();
            if (rosterstatus) {
                jConfirm('Confirm Delete?', 'Alert', function (r) {
                    if (r) {
                        $.ajax({
                            type: "POST",
                            url: "STBRequisition.aspx/DeleteSTBRequisition",
                            data: JSON.stringify({ STBRequisitionID: val }),
                            async: false,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                if (response.d) {
                                    if (response.d == "true") {
                                        jAlert("STB Requisition delete sucessfully.");
                                        var url = 'STBRequisition.aspx';
                                        cGrdSTBRequisitionList.Refresh();
                                    }
                                    else if (response.d == "Logout") {
                                        location.href = "../../OMS/SignOff.aspx";
                                    }
                                    else {
                                        alert(response.d);
                                    }
                                }
                            },
                            error: function (response) {
                                console.log(response);
                            }
                        });
                    }
                });
            }
            else {
                //  jAlert("Working period is over. Try in next working period.");
                $("#divPopHead").removeClass('hide');
            }
        }

        function OnCancelClick(keyValue, visibleIndex) {
            WorkingRoster();
            if (rosterstatus) {
                $("#<%=hddnKeyValue.ClientID%>").val(keyValue);
                $("#<%=hddnCancelCloseFlag%>").val('CA');
                cGrdSTBRequisitionList.SetFocusedRowIndex(visibleIndex);
                jConfirm('Do you want to cancel the STB Requisition ?', 'Confirm Dialog', function (r) {
                    if (r == true) {
                        $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                        cPopup_Feedback.Show();
                    }
                    else {
                        return false;
                    }
                });
            }
            else {
                // jAlert("Working period is over. Try in next working period.");
                $("#divPopHead").removeClass('hide');
            }
        }

        function OnCloseClick(keyValue, visibleIndex) {
            $("#<%=hddnKeyValue.ClientID%>").val(keyValue);
            $("#<%=hddnCancelCloseFlag%>").val('CL');
            cGrdSTBRequisitionList.SetFocusedRowIndex(visibleIndex);
            jConfirm('Do you want to close the STB Requisition ?', 'Confirm Dialog', function (r) {
                if (r == true) {
                    $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                    cPopup_Feedback.Show();
                }
                else {
                    return false;
                }
            });
        }

        function CallFeedback_save() {
            var KeyVal = $("#<%=hddnKeyValue.ClientID%>").val();
            var flag = true;
            var Remarks = txtFeedback.GetValue();
            if (Remarks == "" || Remarks == null) {
                $('#MandatoryRemarksFeedback').attr('style', 'display:block;position: absolute; right: -20px; top: 8px;');
                flag = false;
            }
            else {
                $('#MandatoryRemarksFeedback').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                cPopup_Feedback.Hide();
                CancelSTBRequisition(KeyVal, Remarks);
                cGrdSTBRequisitionList.Refresh();
            }
            return flag;
        }

        function CancelSTBRequisition(keyValue, Reason) {

            $.ajax({
                type: "POST",
                url: "STBRequisition.aspx/CancelSTBRequisitionOnRequest",
                data: JSON.stringify({ keyValue: keyValue, Reason: Reason }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d;
                    if (status == "1") {
                        jAlert("Cancellation Requisition is generated Successfully.");
                    }
                    else if (status == "-1") {
                        jAlert("STB Requisition is not cancelled.Try again later");
                    }
                    else if (status == "-2") {
                        jAlert("Selected STB Requisition is tagged in other module. Cannot proceed.");
                    }
                    else if (status == "-3") {
                        jAlert("Cancellation Requisition is already generated.");
                    }
                    else if (status == "-4") {
                        jAlert("STB Requisition is already closed. Cannot proceed.");
                    }
                }
            });
        }
    </script>
    <script>
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
                cGrdSTBRequisitionList.Refresh();
            }
        }

        function gridRowclick(s, e) {
            $('#GrdReceiptChallan').find('tr').removeClass('rowActive');
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


        function OnclickViewAttachment(obj) {
            WorkingRoster();
            if (rosterstatus) {
                var URL = '../../../OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=StbRequisition';
                window.location.href = URL;
            }
            else {
                //jAlert("Working period is over. Try in next working period.");
                $("#divPopHead").removeClass('hide');
            }
        }

        $(document).ready(function () {
            setTimeout(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdSTBRequisitionList.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 217;
                    cGrdSTBRequisitionList.SetWidth(cntWidth);
                }
            }, 300);
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 217;
                    cGrdSTBRequisitionList.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdSTBRequisitionList.SetWidth(cntWidth);
                }
            });
        })

        var rosterstatus = false;
        function WorkingRoster() {
            $.ajax({
                type: "POST",
                url: 'STBRequisition.aspx/CheckWorkingRoster',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: JSON.stringify({ module_ID: '3' }),
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="popupWraper hide" id="divPopHead" runat="server">
        <div class="popBox">
            <img src="/assests/images/warningAlert.png" class="mBot10" style="width:70px;" />
            <h1 id="h1heading" class="red">Your Access is Denied</h1>
            <p id="pParagraph" class="red">
               You can access this section starting from <span id="spnbegin"></span> upto <span id="spnEnd"></span>
            </p>
            <button type="button" class="btn btn-sign" onclick="WorkingRosterClick()">OK</button>
        </div>
    </div>

    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>STB Requisition</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 8px;">
            <tr>
                <td>
                    <label>From Date</label></td>
                <td>&nbsp;</td>
                <td>
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>&nbsp;</td>
                <td>
                    <label>To Date</label>
                </td>
                <td>&nbsp;</td>
                <td>
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>&nbsp;</td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>&nbsp;</td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                </td>

            </tr>

        </table>
    </div>
    <div class="form_main">
        <table class="TableMain100" style="width: 100%">
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();" class="btn btn-success btn-radius">
                                    <span class="btn-icon"><i class="fa fa-plus"></i></span>STB</a>

                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" AutoPostBack="true" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>
                                <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span>Show Filter</span></a>--%>
                            </td>
                            <td id="Td1">
                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top; text-align: left" colspan="2" class="relative">
                    <dxe:ASPxGridView ID="GrdSTBRequisition" runat="server" KeyFieldName="STBRequisition_id" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                        SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200" Settings-HorizontalScrollBarMode="Auto"
                        DataSourceID="EntityServerModeDataSource"
                        Width="100%" ClientInstanceName="cGrdSTBRequisitionList">
                        <settingssearchpanel visible="True" delay="5000" />

                        <columns>
                            <dxe:GridViewDataTextColumn Visible="False" FieldName="STBRequisition_id" Caption="ID" VisibleIndex="0" SortOrder="Descending">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Document No" FieldName="DocumentNumber" FixedStyle="Left"
                                VisibleIndex="1" Width="200px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn Caption="Date" Width="100px" FieldName="DocumentDate" VisibleIndex="2" FixedStyle="Left">
                                <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Location" FieldName="branch_description" VisibleIndex="3" Width="150px">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Requisition Type" FieldName="RequisitionType" VisibleIndex="4" Width="150px">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Requisition For" FieldName="RequisitionFor" VisibleIndex="5" Width="150px">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="EntityCode"  Width="200px"
                                Caption="Entity Code" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="NetworkName"  Width="200px"
                                Caption="Network Name" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="ContactPerson" Width="200px"
                                Caption="Contact person" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="ContactNumber" Width="100px"
                                Caption="Contact Number" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="DAS" Width="200px"
                                Caption="DAS" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="ReqAmount" Width="100px"
                                Caption="Req. Amount" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="ReqStatus" Width="100px"
                                Caption="Req. Status" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="InvStatus" Width="100px"
                                Caption="Inv. Status" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="Director" Width="200px"
                                Caption="Director" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="RejectRemarks" Width="200px"
                                Caption="Reject Remarks" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="16" FieldName="HoldRemarks" Width="200px"
                                Caption="Hold Remarks" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="17" FieldName="CancelRemarks" Width="200px"
                                Caption="Cancel Remarks" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="18" FieldName="Create_by" Width="200px"
                                Caption="Entered by" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="19" FieldName="Create_date" Width="150px"
                                Caption="Entered on" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="20" FieldName="Update_by" Width="200px"
                                Caption="Updated by" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="21" FieldName="Update_date" Width="150px"
                                Caption="Updated on" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            
                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="22" Width="0">
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                         <% if (rights.CanView)
                                            { %>
                                        <a href="javascript:void(0);" onclick="ClickOnView('<%# Container.KeyValue %>')" id="a_viewInvoice" class="" title="" >
                                            <span class='ico editColor'><i class='fa fa-eye' aria-hidden='true'></i></span><span class='hidden-xs'>View</span></a>
                                        <%} %>
                                        <% if (rights.CanEdit)
                                           { %>
                                        <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Container.KeyValue %>')" id="a_editInvoice"  class="" title="" style="<%#Eval("EditStatus")%>" >
                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                        <%} %>
                                        <% if (rights.CanDelete)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class=""  title="" id="a_delete" style="<%#Eval("EditStatus")%>" >
                                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                        <%} %>
                                           <% if (rights.CanPrint)
                                              { %>
                                        <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="" title="" id="a_Print">
                                            <span class='ico printColor'><i class='fa fa-print det' aria-hidden='true'></i></span><span class='hidden-xs'>Print</span></a>
                                        <%} %>
                                       <%-- <% if (rights.CanCancel)
                                           { %>
                                          <a href="javascript:void(0);" onclick="OnCancelClick('<%# Container.KeyValue %>',<%# Container.VisibleIndex %>)" class="" title=""  style='<%#Eval("CancelApproveStatus")%>'>                            
                                            <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel </span>
                                        </a>
                                      <% } %>--%>
                                      <% if (rights.CanAddUpdateDocuments)
                                         { %>
                                    <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="" title="" style=''>
                                        <span class='ico ColorFive'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>Add/View Attachment</span>
                                    </a>
                                    <% } %>                               
                                           
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

                    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                        ContextTypeName="ServicveManagementDataClassesDataContext" TableName="V_STB_STBRequisitionList" />
                </td>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
        <asp:SqlDataSource ID="gridStatusDataSource" runat="server"
            SelectCommand="">
            <%--  <SelectParameters>
                <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
            </SelectParameters>--%>
        </asp:SqlDataSource>
    </div>

    <asp:HiddenField ID="hfIsFilter" runat="server" />
    <asp:HiddenField ID="hfFromDate" runat="server" />
    <asp:HiddenField ID="hfToDate" runat="server" />
    <asp:HiddenField ID="hfBranchID" runat="server" />
    <asp:HiddenField ID="hdnIsUserwiseFilter" runat="server" />
    <asp:HiddenField ID="hddnKeyValue" runat="server" />
    <asp:HiddenField ID="hddnCancelCloseFlag" runat="server" />
</asp:Content>
