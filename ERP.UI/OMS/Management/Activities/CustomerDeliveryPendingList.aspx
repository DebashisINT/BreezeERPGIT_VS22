<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerDeliveryPendingList.aspx.cs"  MasterPageFile="~/OMS/MasterPage/ERP.Master"
     Inherits="ERP.OMS.Management.Activities.CustomerDeliveryPendingList" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style>
        .padTab>tbody>tr>td {
            padding-right:15px;
            vertical-align:middle;
        }
        .padTab>tbody>tr>td>label {
            margin-bottom:0 !important;
        }
        .padTab>tbody>tr>td>.btn {
            margin-top:0 !important
        }
    </style>

    <script>
        function GlobalBillingShippingEndCallBack() { };
        var isFirstTime = true;
        function AllControlInitilize() {
            //debugger;
            if (isFirstTime) {

                if (localStorage.getItem('FromDateODSD')) {
                    var fromdatearray = localStorage.getItem('FromDateODSD').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('ToDateODSD')) {
                    var todatearray = localStorage.getItem('ToDateODSD').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('BranchODSD')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('BranchODSD'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('BranchODSD'));
                    }

                }
                //updateGridByDate();
                updateGrid();
                isFirstTime = false;
            }
        }

        function updateGrid()
        {
            cGrdOrder.PerformCallback();
        }


        //var InvoiceId = 0;
        var ODSCChallanId = 0;
        function onPrintJv(id, Bill_Id, ChallanDocNo) {
            //window.location.href = "../../reports/XtraReports/Viewer/InvoiceReportViewer.aspx?id=" + id
            //  window.location.href = "../../reports/XtraReports/Viewer/TaxInvoiceReportViewer.aspx?id=" + id
            //debugger;
            //InvoiceId = id;
            ODSCChallanId = Bill_Id;
            cSelectPanel.cpSuccess = "";
            if (ChallanDocNo != "") {
                cDocumentsPopup.Show();
                cCmbDesignName.SetSelectedIndex(0);
                cSelectPanel.PerformCallback('Bindalldesignes');
                $('#btnOK').focus();
            }
            else {
                jAlert('This challan not yet delivered.Nothing to Print.');
            }
        }

        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
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
                localStorage.setItem("FromDateODSD", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDateODSD", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("BranchODSD", ccmbBranchfilter.GetValue());
                cGrdOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
            }
        }
        //End
        function cSelectPanelEndCall(s, e) {
            //debugger;
            if (cSelectPanel.cpSuccess != "") {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'ODSDChallan';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + ODSCChallanId, '_blank')
            }
            if (cSelectPanel.cpSuccess == "") {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }
        //--------------------------26-06-2017-------------------------------------
        function callbackData(data) {
           
        }

        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;


            if (event.keyCode == 65 && isCtrl == true) { //run code for alt+a -- ie, Add
                StopDefaultAction(e);
                OnAddButtonClick();
            }

        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function OnClickDelete(keyValue) {
            //debugger;
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
        }
        function SavePrpformaStatus() {
            if (document.getElementById('hiddenedit').value == '') {
                cGrdOrder.PerformCallback('save~');
            }
            else {
                cGrdOrder.PerformCallback('update~' + GetObjectID('hiddenedit').value);
            }

        }

        function OnMoreInfoClickOur(keyValue, vissibleInx) {
            debugger;
            cGrdOrder.SetFocusedRowIndex(vissibleInx);
            var ChallanDocNo = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[16].innerText;
            var BRS = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[11].innerText;
            var EntryType = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;
            var IsSRN = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[19].innerText;
            var ConfigSettings = $("#<%=hddnBRSConfigSettings.ClientID%>").val();
            if (IsSRN == "No") {
                if (BRS.trim() == "NOT-DONE" && ChallanDocNo.trim() == "" && EntryType.trim() == "Cheque" && ConfigSettings == "1") {
                    jAlert("Bank reconciliation is pending.Cannot proceed.");
                }
                else {
                    if (ChallanDocNo.trim() != "") {
                        jAlert('Already Delivered.You can only view !', 'Confirmation Dialog', function (r) {
                            if (r == true) {
                                var TypeIdDlv = $("#<%=hddnTypeIdd.ClientID%>").val();
                                //var url = 'SalesChallanAdd.aspx?key=' + keyValue + '&Permission=' + status + '&CustID=' + Customer_Id + '&DlvTyeId=' + TypeIdDlv + '&Flag=' + 'CustomerDeliveryFlag' + '&BillDate=' + BillDate;
                                var url = 'SalesChallanAdd.aspx?key=' + ChallanDocNo + '&type=SC' + '&DlvTyeId=' + TypeIdDlv;
                                window.location.href = url;
                            }
                        });

                    }
                    else {
                        var TypeIdDlv = $("#<%=hddnTypeIdd.ClientID%>").val();
                        var url = 'CustomerDeliveryPendingOurDelv.aspx?key=' + keyValue + '&DlvTyeId=' + TypeIdDlv + '&Flag=' + 'CustomerDeliveryFlag';

                        window.location.href = url;
                    }
                }
            }
            else {
                jAlert("Return already made for the selected Invoice. Cannot proceed.");
            }

        }

       

        function OnMoreInfoClick(keyValue, vissibleInx) {
            debugger;
            //var Customer_Id = cGrdOrder.GetEditor("Customer_Id").GetValue();
            //var Billdate = cGrdOrder.GetEditor("Bill_Date").GetValue();
            //cGrdOrder.GetRowValues(1, 'Customer_Id', callbackData);
            cGrdOrder.SetFocusedRowIndex(vissibleInx);
            var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                //$.ajax({
                //    type: "POST",
                //    url: "CustomerDeliveryPendingList.aspx/GetEditablePermission",
                //    data: "{'ActiveUser':'" + ActiveUser + "'}",
                //    contentType: "application/json; charset=utf-8",
                //    dataType: "json",
                //    async: false,//Added By:Subhabrata
                //    success: function (msg) {
                //        //debugger;
                //        var status = msg.d;
                        $.ajax({
                            type: "POST",
                            url: "CustomerDeliveryPendingList.aspx/GetChallanIdIsExistInSalesInvoice",
                            data: "{'keyValue':'" + keyValue + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                           // async: false,//Added By:Subhabrata
                            success: function (msg) {
                                debugger;
                                var status = msg.d;
                                if (status == "1") {
                                    jAlert('Already Delivered.You can only view !', 'Confirmation Dialog', function (r) {
                                        if (r == true) {
                                            //return false;
                                            //}

                                            //}
                                            //else {
                                            $.ajax({
                                                type: "POST",
                                                url: "CustomerDeliveryPendingList.aspx/GetCustomerId",
                                                data: "{'KeyVal':'" + keyValue + "'}",
                                                contentType: "application/json; charset=utf-8",
                                                dataType: "json",
                                                //async: false,//Added By:Subhabrata
                                                success: function (msg) {
                                                    debugger;
                                                    var ID = msg.d;
                                                    var Customer_Id = ID.split('~')[0];
                                                    var BillDate = ID.split('~')[1];
                                                    var ChallanDocNo = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[11].innerText;
                                                    var TypeIdDlv = $("#<%=hddnTypeIdd.ClientID%>").val();
                                                    //var url = 'SalesChallanAdd.aspx?key=' + keyValue + '&Permission=' + status + '&CustID=' + Customer_Id + '&DlvTyeId=' + TypeIdDlv + '&Flag=' + 'CustomerDeliveryFlag' + '&BillDate=' + BillDate;
                                                    var url = 'SalesChallanAdd.aspx?key=' + ChallanDocNo + '&type=SC' + '&DlvTyeId=' + TypeIdDlv;
                                                    window.location.href = url;
                                                }
                                            });
                                        }
                                    });
                                }
                                else {
                                    <%--   $.ajax({
                                        type: "POST",
                                        url: "CustomerDeliveryPendingList.aspx/GetCustomerId",
                                        data: "{'KeyVal':'" + keyValue + "'}",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        async: false,//Added By:Subhabrata
                                        success: function (msg) {
                                            //debugger;
                                            var ID = msg.d;
                                            var Customer_Id = ID.split('~')[0];
                                            var BillDate = ID.split('~')[1];
                                            var TypeIdDlv = $("#<%=hddnTypeIdd.ClientID%>").val();
                                            var url = 'CustomerDeliveryPendingOur.aspx?key=' + keyValue + '&Permission=' + status + '&CustID=' + Customer_Id + '&DlvTyeId=' + TypeIdDlv + '&Flag=' + 'CustomerDeliveryFlag' + '&BillDate=' + BillDate;

                                            window.location.href = url;
                                        }
                                    });--%>
                                    var TypeIdDlv = $("#<%=hddnTypeIdd.ClientID%>").val();
                                    var url = 'CustomerDeliveryPendingOurDelv.aspx?key=' + keyValue + '&DlvTyeId=' + TypeIdDlv + '&Flag=' + 'CustomerDeliveryFlag';

                                    window.location.href = url;
                                    //location.href = url;
                                }


                            }
                        });


                    }
                //});
            //}
        }

        function OnAddButtonClick() {
            var url = 'SalesChallanAdd.aspx?key=' + 'ADD';
            window.location.href = url;
        }
        //});

    </script>
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
                                        <td style="padding-right:20px">
                                            <label style="margin-bottom:5px">Customer Delivery - OD</label> 
                                            
                                           
                                        </td>
                                        <td>
                                            <%--<dxe:ASPxTextBox ID="txt_Proforma" MaxLength="80" ClientInstanceName="cProforma" TabIndex="1" 
                                                runat="server" Width="100%"> 
                                            </dxe:ASPxTextBox>--%>
                                            <dxe:ASPxLabel ID="lbl_Proforma" runat="server" ClientInstanceName="cProforma" Text="ASPxLabel"></dxe:ASPxLabel>
                                        </td>
                                        <td style="padding-right:20px;padding-left:8px">
                                            <label style="margin-bottom:5px">Customer</label> 
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
                                     <td style="width: 70px;padding: 13px 0;">Status </td>
                                     <td><asp:RadioButtonList ID="rbl_OrderStatus" runat="server" Width="250px" CssClass="mTop5" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Pending" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Accepted" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Pending" Value="3"></asp:ListItem>
                            </asp:RadioButtonList></td>
                                 </tr>
                             </table>
                            
                               
                               
                            
                                
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-12"> 
                                
                                    <div class="" style="margin-bottom:5px;">
                                        Reason 
                                    </div>
                                
                               <div>
                                   <dxe:ASPxMemo ID="txt_OrderRemarks" runat="server" ClientInstanceName="cOrderRemarks" Height="71px" Width="100%"></dxe:ASPxMemo>
                                </div>
                             </div>

                           <div class="col-md-12" style="padding-top:10px;"> 
                                <dxe:ASPxButton ID="btn_PrpformaStatus" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server" 
                                    AutoPostBack="False" Text="Save" CssClass="btn btn-primary" >
                                    <ClientSideEvents Click="function (s, e) {SavePrpformaStatus();}" /> 
                                </dxe:ASPxButton>
                            </div>
                        </dxe:PopupControlContentControl>
            </contentcollection>
    </dxe:ASPxPopupControl>
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3> <asp:Label ID="lblHeadertext" runat="server"></asp:Label></h3>
        </div>
        <table class="padTab pull-right" style="margin-top:7px">
            <tr>
                <td>
                    <label>From Date</label></td>
                <td>
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <label>To Date</label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>Branch</td>
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
    <div class="form_main" >
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary" style="display:none;"><span><u>A</u>dd New</span> </a>
            <% } %>

            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server"  CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" 
                AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
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
        <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="Bill_Id" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGrdOrder" OnCustomCallback="GrdOrder_CustomCallback" OnDataBinding="GrdOrder_DataBinding"  OnHtmlRowPrepared="AvailableStockgrid_HtmlRowPrepared" 
            SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" 
            SettingsBehavior-AllowFocusedRow="true" OnSummaryDisplayText="ShowGrid_SummaryDisplayText" HorizontalScrollBarMode="Auto">
            <columns>
                <dxe:GridViewDataTextColumn Caption="Bill No." FieldName="Bill_Number"
                    VisibleIndex="0"  Width="120px" FixedStyle="Left" Settings-ShowFilterRowMenu="True" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Date" FieldName="Bill_Date"
                    VisibleIndex="1"  Width="90px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                    VisibleIndex="2" Width="160px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Branch" FieldName="BranchName"
                    VisibleIndex="3" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Assigned Branch" FieldName="AssignedBranch"
                    VisibleIndex="4" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                    VisibleIndex="5" >
                    <PropertiesTextEdit DisplayFormatString="0.000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                    <PropertiesTextEdit>
                    <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                        
                   </PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Dlv type" FieldName="DeliveryType"
                    VisibleIndex="6" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Dlv Date" FieldName="DeliveryDate"
                    VisibleIndex="7"  Width="80px" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status"
                    VisibleIndex="8"  Width="65px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Challan No." FieldName="ChallanNo"
                    VisibleIndex="9" Width="120px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="ChallanDate"
                    VisibleIndex="10" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="BRS?" FieldName="BRS"
                    VisibleIndex="11"  Width="80px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="BRS Date" FieldName="BRSDate"
                    VisibleIndex="12"  Width="120px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Assigned By" FieldName="AssignedBy"
                    VisibleIndex="13" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Assigned Date" FieldName="AssignedDate"
                    VisibleIndex="14" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="ChallanDocNo" FieldName="ChallanDocNo"
                    VisibleIndex="16"  Width="0" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                    <CellStyle CssClass="hide" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Customer_Id"
                    VisibleIndex="17"  Width="0" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                    <CellStyle CssClass="hide" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="EntryType" FieldName="EntryType"
                    VisibleIndex="18"  Width="0" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                    <CellStyle CssClass="hide" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="IsSRN" FieldName="isSRN"
                    VisibleIndex="19"  Width="0" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                    <CellStyle CssClass="hide" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="15" Width="100px">
                    <DataItemTemplate>
                        <% if (rights.CanAdd)
                           { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClickOur('<%# Container.KeyValue %>',<%# Container.VisibleIndex %>)" class="pad" title="Make Delivery">
                            
                            <i class="fa fa-truck out" ></i></a>  <% } %>

                      
                        
                        <%--  <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title="Copy " style="display:none">
                          <i class="fa fa-copy"></i>  </a>
                        <a href="javascript:void(0);" onclick="OnClickStatus('<%# Container.KeyValue %>')" class="pad" title="Status" style="display:none">
                            <img src="../../../assests/images/verified.png" /></a>--%>
                         <% if (rights.CanView)
                            { %>
                       <%-- <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" style="display:none" class="pad" title="View Attachment">
                            <img src="../../../assests/images/attachment.png" />  </a>--%>
                         <% } %>
                         <% if (rights.CanPrint)
                            { %>
                               <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>', '<%# Eval("Bill_Id") %>', '<%# Eval("ChallanDocNo") %>')" class="pad" title="print">
                               <img src="../../../assests/images/Print.png" />
                         </a><%} %>
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
                           <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                             </settingspager>
           <%-- <settingssearchpanel visible="True" />--%>
            <settings showgrouppanel="True" ShowFooter="true"  ShowGroupFooter="VisibleIfExpanded"  showstatusbar="Hidden" showhorizontalscrollbar="true" showfilterrow="true" showfilterrowmenu="false" />
            <settingsloadingpanel text="Please Wait..." />
             <TotalSummary>
                                       <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" /> 
                                 </TotalSummary>
        </dxe:ASPxGridView>
        <asp:HiddenField ID="hiddenedit" runat="server" />
        <asp:HiddenField ID="hddnTypeIdd" runat="server" />
        <asp:HiddenField ID="hddnBRSConfigSettings" runat="server" />
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
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

     <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
</asp:Content>