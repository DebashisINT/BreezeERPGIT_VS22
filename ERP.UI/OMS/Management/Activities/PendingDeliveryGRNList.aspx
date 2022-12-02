<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="PendingDeliveryGRNList.aspx.cs" Inherits="ERP.OMS.Management.Activities.PendingDeliveryGRNList" 
    EnableEventValidation="false"%>

<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(function () {
            $("#btntransporter").hide();
            $("#vehicleTransporter").hide();
            $("#PendingDlvTransporter").show();
        });
    </script>
    <script>

        var isFirstTime = true;
        function AllControlInitilize() {

            if (isFirstTime) {

                if (localStorage.getItem('FromDatePendingDeliveryGRN')) {
                    var fromdatearray = localStorage.getItem('FromDatePendingDeliveryGRN').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('ToDatePendingDeliveryGRN')) {
                    var todatearray = localStorage.getItem('ToDatePendingDeliveryGRN').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('BranchPendingDeliveryGRN')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('BranchPendingDeliveryGRN'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('BranchPendingDeliveryGRN'));
                    }

                }
                updateGridByDate();

                isFirstTime = false;
            }
        }


        //Function for Date wise filteration
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
                localStorage.setItem("FromDatePendingDeliveryGRN", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDatePendingDeliveryGRN", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("BranchPendingDeliveryGRN", ccmbBranchfilter.GetValue());
                cGrdOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

            }
        }
        //End

        function callbackData(data) {

        }

        document.onkeydown = function (e) {
            //if (event.keyCode == 18) isCtrl = true;


            //if (event.keyCode == 65 && isCtrl == true) { //run code for alt+a -- ie, Add
            //    StopDefaultAction(e);
            //    OnAddButtonClick();
            //}

        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function OnClickDelete(keyValue) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cGrdOrder.PerformCallback('Delete~' + keyValue);
                }
            });
        }

        function OnclickViewAttachment(obj) {
            var URL = '/OMS/Management/Activities/SalesOrder_Document.aspx?idbldng=' + obj + '&type=SalesOrder';
            window.location.href = URL;
        }
        function OnClickStatus(keyValue) {
            GetObjectID('hiddenedit').value = keyValue;
            cGrdOrder.PerformCallback('Edit~' + keyValue);
        }
        function grid_EndCallBack() {

            if (cGrdOrder.cpEdit != null) {
                GetObjectID('hiddenedit').value = cGrdOrder.cpEdit.split('~')[0];
                cProforma.SetText(cGrdOrder.cpEdit.split('~')[1]);
                cCustomer.SetText(cGrdOrder.cpEdit.split('~')[4]);
                var pro_status = cGrdOrder.cpEdit.split('~')[2];
                if (pro_status != null) {
                    var radio = $("[id*=rbl_OrderStatus] label:contains('" + pro_status + "')").closest("td").find("input");
                    radio.attr("checked", "checked");
                    //return false;
                    //$('#rbl_QuoteStatus[type=radio][value=' + pro_status + ']').prop('checked', true); 
                    cOrderRemarks.SetText(cGrdOrder.cpEdit.split('~')[3]);
                    cOrderStatus.Show();
                }
            }
            if (cGrdOrder.cpDelete != null) {
                jAlert(cGrdOrder.cpDelete);
                cGrdOrder.cpDelete = null;
            }

            if (cGrdOrder.cpBindTransport == "UpdateTransport") {
                cGrdOrder.cpBindTransport = null;
                $('#exampleModal').modal('toggle');
            }

        }
        function SavePrpformaStatus() {

            if (document.getElementById('hiddenedit').value == '') {
                cGrdOrder.PerformCallback('save~');
            }
            else {
                cGrdOrder.PerformCallback('update~' + GetObjectID('hiddenedit').value);
            }

        }

        var PDChallan_id = 0;
        function onPrintJv(id, InvoiceID) {
            debugger;
            PDChallan_id = InvoiceID;
            cSelectPanel.cpSuccess = "";
            if (InvoiceID != "") {
                cDocumentsPopup.Show();
                //CselectDuplicate.SetEnabled(false);
                //CselectTriplicate.SetEnabled(false);
                //CselectOriginal.SetCheckState('UnChecked');
                //CselectDuplicate.SetCheckState('UnChecked');
                //CselectTriplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
                cSelectPanel.PerformCallback('Bindalldesignes');
                $('#btnOK').focus();
            }
            else {
                jAlert('This GRN not yet delivered. Nothing to Print.');
            }
        }

        //Subhabrata on 13-07-2017
        function onClickTransporter(id) {
            $("#<%=hddnChallanDocNo.ClientID%>").val(id);
            var isExitsChallanIdInTrns = false;
                $.ajax({
                    type: "POST",
                    url: "PendingDeliveryGRNList.aspx/GetTransporterDetailsForInvoice",
                    data: JSON.stringify({ GRN_Id: id }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        var ObjData = msg.d;
                        if (ObjData > 0) {

                            isExitsChallanIdInTrns = true;

                            callTransporterControlFromPending(id, 'PC', 'PendingDelivery');
                            $('#exampleModal').modal({
                                show: 'true'
                            });

                            $("#DivFreight").show();
                            $("#Div_VehicleOutDate").show();
                        }
                        else {
                            clearTransporter();
                        }

                    }

                });
                if (isExitsChallanIdInTrns == false) {
                    $("#DivFreight").show();
                    $("#Div_VehicleOutDate").show();
                    $('#exampleModal').modal({
                        show: 'true'
                    });

                    //$('#exampleModal').show();
                }
                else {
                    //cGrdOrder.PerformCallback('BindTransPorter~' + ChallanDocNo);
                }
        }
        //End

        function SaveTransporterWithChallanDocId() {

            $('#<%=hfControlData.ClientID %>').val($('#hfControlSaveData').val());
            var Challan_Id = $("#<%=hddnChallanDocNo.ClientID%>").val();
            cGrdOrder.PerformCallback('SaveTransporter~' + Challan_Id);

        }

        function OrginalCheckChange(s, e) {

            if (s.GetCheckState() == 'Checked') {
                CselectDuplicate.SetEnabled(true);
            }
            else {
                CselectDuplicate.SetCheckState('UnChecked');
                CselectDuplicate.SetEnabled(false);
                CselectTriplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetEnabled(false);
            }

        }
        function DuplicateCheckChange(s, e) {
            if (s.GetCheckState() == 'Checked') {
                CselectTriplicate.SetEnabled(true);
            }
            else {
                CselectTriplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetEnabled(false);
            }

        }

        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }


        function cSelectPanelEndCall(s, e) {

            if (cSelectPanel.cpSuccess != null) {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'PDChallan';
                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + PDChallan_id + '&PrintOption=' + TotDocument[i], '_blank')
                        }
                    }
                }
            }
            //cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == "") {
                if (cSelectPanel.cpChecked != "") {
                    jAlert('Please check Original For Recipient and proceed.');
                }
                //CselectDuplicate.SetEnabled(false);
                //CselectTriplicate.SetEnabled(false);
                //CselectOriginal.SetCheckState('UnChecked');
                //CselectDuplicate.SetCheckState('UnChecked');
                //CselectTriplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
            }
        }



        function OnMoreInfoClick(keyValue, vissibleInx, PBInvoiceID) {

            //var Customer_Id = cGrdOrder.GetEditor("Customer_Id").GetValue();
            //var Billdate = cGrdOrder.GetEditor("Bill_Date").GetValue();
            //cGrdOrder.GetRowValues(1, 'Customer_Id', callbackData);
            cGrdOrder.SetFocusedRowIndex(vissibleInx);
            var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "CustomerDeliveryPendingList.aspx/GetEditablePermission",
                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,//Added By:Subhabrata
                    success: function (msg) {

                        var status = msg.d;
                        $.ajax({
                            type: "POST",
                            url: "CustomerDeliveryPendingList.aspx/GetChallanIdIsExistInSalesInvoice",
                            data: "{'keyValue':'" + keyValue + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,//Added By:Subhabrata
                            success: function (msg) {
                                if (PBInvoiceID != "") {
                                    alert(PBInvoiceID);
                                    jAlert('Already Delivered.You can only view !', 'Confirmation Dialog', function (r) {
                                        if (r == true) {
                                            //debugger;
                                            var url = 'SalesChallanAdd.aspx?key=' + ChallanDocNo + '&type=SC' + '&DlvTyeId=' + '2';
                                            window.location.href = url;
                                        }
                                    });
                                }
                                else {
                                    alert(PBInvoiceID, "Key Value:" + keyValue);
                                    //var url = 'SalesChallanAdd.aspx?key=' + keyValue + '&Permission=' + status + '&CustID=' + Customer_Id + '&Flag=' + 'PendingDeliveryFlag' + '&BillDate=' + BillDate;
                                    //window.location.href = url;
                                }

                                //var status1 = msg.d;
                                //$.ajax({
                                //    type: "POST",
                                //    url: "CustomerDeliveryPendingList.aspx/GetCustomerId",
                                //    data: "{'KeyVal':'" + keyValue + "'}",
                                //    contentType: "application/json; charset=utf-8",
                                //    dataType: "json",
                                //    async: false,//Added By:Subhabrata
                                //    success: function (msg) {
                                //        //debugger;
                                //        var ID = msg.d;
                                //        var Customer_Id = ID.split('~')[0];
                                //        var BillDate = ID.split('~')[1];
                                //        var ChallanDocNo = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[8].innerText;
                                //        if (status1 == "1") {
                                //            jAlert('Already Delivered.You can only view !', 'Confirmation Dialog', function (r) {
                                //                if (r == true) {
                                //                    //debugger;
                                //                    var url = 'SalesChallanAdd.aspx?key=' + ChallanDocNo + '&type=SC' + '&DlvTyeId=' + '2';
                                //                    window.location.href = url;
                                //                }
                                //            });
                                //        }
                                //        else {
                                //            var url = 'SalesChallanAdd.aspx?key=' + keyValue + '&Permission=' + status + '&CustID=' + Customer_Id + '&Flag=' + 'PendingDeliveryFlag' + '&BillDate=' + BillDate;
                                //            window.location.href = url;
                                //        }



                                //    }
                                //});
                            }
                        });


                    }
                });
            }
        }

        function OnAddButtonClick() {
            var url = 'SalesChallanAdd.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        //});

    </script>
    <style>
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
    <dxe:ASPxPopupControl ID="Popup_OrderStatus" runat="server" ClientInstanceName="cOrderStatus"
        Width="500px" HeaderText="Approvers Configuration" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <contentcollection>
            <dxe:PopupControlContentControl runat="server">
                <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-12">
                            <table width="100%">
                                <tr>
                                    <td style="padding-right: 20px">
                                        <label style="margin-bottom: 5px">Pending Delivery GRN List</label>
                                    </td>
                                    <td>
                                        <%--<dxe:ASPxTextBox ID="txt_Proforma" MaxLength="80" ClientInstanceName="cProforma" TabIndex="1" 
                                                runat="server" Width="100%"> 
                                            </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxLabel ID="lbl_Proforma" runat="server" ClientInstanceName="cProforma" Text="ASPxLabel"></dxe:ASPxLabel>
                                    </td>
                                    <td style="padding-right: 20px; padding-left: 8px">
                                        <label style="margin-bottom: 5px">Customer</label>
                                    </td>
                                    <td>
                                        <%-- <dxe:ASPxTextBox ID="txt_Customer" ClientInstanceName="cCustomer"  runat="server" MaxLength="100" TabIndex="2"
                                            Width="100%"> 
                                        </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxLabel ID="lbl_Customer" runat="server" ClientInstanceName="cCustomer" Text="ASPxLabel"></dxe:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-md-6">
                        </div>
                        <div class="col-md-6">
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="col-md-12">
                    <table>
                        <tr>
                            <td style="width: 70px; padding: 13px 0;">Status </td>
                            <td>
                                <asp:RadioButtonList ID="rbl_OrderStatus" runat="server" Width="250px" CssClass="mTop5" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Pending" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Accepted" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="3"></asp:ListItem>
                                </asp:RadioButtonList></td>
                        </tr>
                    </table>





                </div>
                <div class="clear"></div>
                <div class="col-md-12">

                    <div class="" style="margin-bottom: 5px;">
                        Reason 
                    </div>

                    <div>
                        <dxe:ASPxMemo ID="txt_OrderRemarks" runat="server" ClientInstanceName="cOrderRemarks" Height="71px" Width="100%"></dxe:ASPxMemo>
                    </div>
                </div>

                <div class="col-md-12" style="padding-top: 10px;">
                    <dxe:ASPxButton ID="btn_PrpformaStatus" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                        AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                        <ClientSideEvents Click="function (s, e) {SavePrpformaStatus();}" />
                    </dxe:ASPxButton>
                </div>
            </dxe:PopupControlContentControl>
        </contentcollection>
    </dxe:ASPxPopupControl>
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Pending Delivery GRN List</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px">
            <tr>
                <td>
                    <label>From Date</label></td>
                <td>
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <label>To Date</label>
                </td>
                <td>
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
    <div class="form_main" style="display: none;">
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary" style="display: none;"><span><u>A</u>dd New</span> </a>
            <% } %>

            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>
        </div>
    </div>
    <div class="GridViewArea">
        <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="PCId" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGrdOrder" OnCustomCallback="GrdOrder_CustomCallback" SettingsBehavior-AllowFocusedRow="true"
            OnSummaryDisplayText="ShowGrid_SummaryDisplayText">
            <columns>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="PCNo"
                    VisibleIndex="0" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="PCDate"
                    VisibleIndex="1" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Vendor" FieldName="PCVendorName"
                    VisibleIndex="2" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="MBBranchDesc"
                    VisibleIndex="3" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="PCAmount"
                    VisibleIndex="4" FixedStyle="Left">
                    <PropertiesTextEdit DisplayFormatString="0.000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                    <PropertiesTextEdit>
                        <MaskSettings Mask="&lt;0..999999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />

                    </PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status"
                    VisibleIndex="5" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft Pending" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                
                <dxe:GridViewDataTextColumn Caption="Vehicle Out Date" FieldName="VehicleOutDateTime" Width="150"
                    VisibleIndex="6" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Invoice No." FieldName="PBInvoiceNo"
                    VisibleIndex="6" FixedStyle="Left" Width="0">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Invoice Date" FieldName="PBInvoiceDate"
                    VisibleIndex="7" FixedStyle="Left" Width="0">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="GRN ID" FieldName="PCId"
                    VisibleIndex="8" FixedStyle="Left" Width="0" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                    <CellStyle CssClass="hide" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Vendor" FieldName="MCVendorID"
                    VisibleIndex="9" FixedStyle="Left" Width="0" Visible="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="10" Width="15%">
                    <DataItemTemplate>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>',<%# Container.VisibleIndex %>,'<%# Eval("PBInvoiceID") %>')" class="pad" title="Create Delivery Invoice" style="display: none">

                            <i class="fa fa-truck out"></i></a><% } %>

                        <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title="Copy " style="display: none">
                            <i class="fa fa-copy"></i></a>
                        <a href="javascript:void(0);" onclick="OnClickStatus('<%# Container.KeyValue %>')" class="pad" title="Status" style="display: none">
                            <img src="../../../assests/images/verified.png" /></a>
                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" style="display: none" class="pad" title="View Attachment">
                            <img src="../../../assests/images/attachment.png" />
                        </a>
                        <% } %>
                        <% if (rights.CanPrint)
                           { %>
                        <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>', '<%# Eval("PBInvoiceID") %>')" class="pad" title="print" style="display: none">
                            <img src="../../../assests/images/Print.png" />
                        </a><%} %>

                        <a href="javascript:void(0);" onclick="onClickTransporter('<%# Container.KeyValue %>', '<%# Eval("PBInvoiceID") %>')" class="pad" title="Delivery Receipt">
                            <img src="../../../assests/images/tow-truck.png" width="16px" />
                        </a>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </columns>

            <clientsideevents endcallback="function (s, e) {grid_EndCallBack();}" />
            <%--<settingspager numericbuttoncount="20" pagesize="10" showseparators="True">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </settingspager>--%>
            <settingspager pagesize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </settingspager>
            <settingssearchpanel visible="True" />
            <settings showgrouppanel="True" showfooter="true" showgroupfooter="VisibleIfExpanded" showstatusbar="Hidden" showhorizontalscrollbar="False" showfilterrow="true" showfilterrowmenu="true" />
            <settingsloadingpanel text="Please Wait..." />
            <totalsummary>
                <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
            </totalsummary>
        </dxe:ASPxGridView>
        <asp:HiddenField ID="hiddenedit" runat="server" />
        <asp:HiddenField ID="hddnChallanDocNo" runat="server" />
        <asp:HiddenField ID="hfControlData" runat="server" />
        <asp:HiddenField ID="hddnFromDate" runat="server" />
        <asp:HiddenField ID="hddnToDate" runat="server" />
        <asp:HiddenField ID="hddnBranch" runat="server" />
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <div>
        <uc1:VehicleDetailsControl runat="server" ID="VehicleDetailsControl" />
    </div>
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

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <clientsideevents controlsinitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
</asp:Content>



