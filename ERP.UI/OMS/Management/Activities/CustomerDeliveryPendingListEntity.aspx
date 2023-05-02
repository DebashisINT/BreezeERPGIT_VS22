<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                11-04-2023        2.0.37           Pallab              25985: Customer Delivery - OD module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerDeliveryPendingListEntity.aspx.cs" Inherits="ERP.OMS.Management.Activities.CustomerDeliveryPendingListEntity"
    MasterPageFile="~/OMS/MasterPage/ERP.Master" %>


<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

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

        function updateGrid() {
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
                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                cGrdOrder.Refresh();
                //cGrdOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
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

        //function OnAddButtonClick() {
        //    var url = 'SalesChallanAdd.aspx?key=' + 'ADD';
        //    window.location.href = url;
        //}
        //});

        function gridRowclick(s, e) {
            $('#GrdOrder').find('tr').removeClass('rowActive');
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
                cGrdOrder.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrdOrder.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdOrder.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdOrder.SetWidth(cntWidth);
                }

            });
        });
    </script>
    <link href="CSS/CustomerDeliveryPendingListEntity.css" rel="stylesheet" />

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        .TableMain100 #ShowGrid, .TableMain100 #ShowGridList, .TableMain100 #ShowGridRet, .TableMain100 #ShowGridLocationwiseStockStatus, 
        #downpaygrid {
            max-width: 98% !important;
        }
        #FormDate, #toDate, #dtTDate, #dt_PLQuote, #dt_PlQuoteExpiry {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        select
        {
            -webkit-appearance: auto;
        }

        /*.calendar-icon
        {
                right: 10px;
        }*/
    </style>
    <%--Rev end 1.0--%>
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
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>
                <asp:Label ID="lblHeadertext" runat="server"></asp:Label></h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px">
            <tr>
                <td>
                    <label>From Date</label></td>
                <%--Rev 1.0: "for-cust-icon" class add --%>
                <td class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"   EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>
                <td>
                    <label>To Date</label>
                </td>
                <%--Rev 1.0: "for-cust-icon" class add --%>
                <td class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" UseMaskBehavior="True" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 1.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 1.0--%>
                </td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                </td>
                <td>
                    <% if (rights.CanExport)
                       { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                        AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                    <% } %>
                </td>

            </tr>

        </table>
    </div>
        <div class="form_main">
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary" style="display: none;"><span><u>A</u>dd New</span> </a>
            <% } %>
        </div>



    </div>
        <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="SlNo" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGrdOrder" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  OnCustomCallback="GrdOrder_CustomCallback" OnHtmlRowPrepared="AvailableStockgrid_HtmlRowPrepared"
            SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true"
            SettingsBehavior-AllowFocusedRow="true" OnSummaryDisplayText="ShowGrid_SummaryDisplayText" HorizontalScrollBarMode="Auto">
           <SettingsSearchPanel Visible="true" Delay="5000" />
             <columns>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Bill_Number"
                    VisibleIndex="0"  Width="120px" FixedStyle="Left" Settings-ShowFilterRowMenu="True"  >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Bill_CheckDate"
                    VisibleIndex="1"  Width="90px" Settings-ShowFilterRowMenu="True" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                     <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                    VisibleIndex="2" Width="160px" Settings-ShowFilterRowMenu="True" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BranchName"
                    VisibleIndex="3" Settings-ShowFilterRowMenu="True" Width="250px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                     <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Assigned Unit" FieldName="AssignedBranch"
                    VisibleIndex="4" Settings-ShowFilterRowMenu="True" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                
                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                    VisibleIndex="5" >
                    <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                    <PropertiesTextEdit>
                    <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                        
                   </PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Dlv type" FieldName="DeliveryType"
                    VisibleIndex="6" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Dlv Date" FieldName="DeliveryDate"
                    VisibleIndex="7"  Width="80px" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status"
                    VisibleIndex="8"  Width="65px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Challan No." FieldName="ChallanNo"
                    VisibleIndex="9" Width="120px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="ChallanDate"
                    VisibleIndex="10" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="BRS?" FieldName="BRS"
                    VisibleIndex="11"  Width="80px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="BRS Date" FieldName="BRSDate"
                    VisibleIndex="12"  Width="120px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Assigned By" FieldName="AssignedBy"
                    VisibleIndex="13" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Assigned Date" FieldName="AssignedDate"
                    VisibleIndex="14" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="ChallanDocNo" FieldName="ChallanDocNo"
                    VisibleIndex="16"  Width="0" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                    <CellStyle CssClass="hide" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Customer_Id"
                    VisibleIndex="17"  Width="0" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                    <CellStyle CssClass="hide" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="EntryType" FieldName="EntryType"
                    VisibleIndex="18"  Width="0" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                    <CellStyle CssClass="hide" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="IsSRN" FieldName="isSRN"
                    VisibleIndex="19"  Width="0" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                    <CellStyle CssClass="hide" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="15" Width="0">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                        <% if (rights.CanAdd)
                           { %>
                        
                        <a href="javascript:void(0);" onclick="OnMoreInfoClickOur('<%#Eval("Bill_Id")%>',<%# Container.VisibleIndex %>)" class="pad" title="">
                            
                            <span class='ico ColorFour'><i class="fa fa-truck out" ></i></span><span class='hidden-xs'>Make Delivery</span></a>  <% } %>

                      
                        
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
                               <a href="javascript:void(0);" onclick="onPrintJv('<%#Eval("Bill_Id")%>', '<%# Eval("Bill_Id") %>', '<%# Eval("ChallanDocNo") %>')" class="" title="">
                               <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                         </a><%} %>
                            </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
            </columns>
               <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <clientsideevents endcallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
            <%--<settingspager numericbuttoncount="20" pagesize="10" showseparators="True">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </settingspager>--%>

            <settingspager pagesize="10">
                           <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                             </settingspager>
            <%--<settingssearchpanel visible="True" />--%>
            <settings showgrouppanel="True" showfooter="true" showgroupfooter="VisibleIfExpanded" showstatusbar="Hidden" showhorizontalscrollbar="true" showfilterrow="true" showfilterrowmenu="false" />
            <settingsloadingpanel text="Please Wait..." />
            <totalsummary>
                                       <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" /> 
                                 </totalsummary>
        </dxe:ASPxGridView>
        <asp:HiddenField ID="hiddenedit" runat="server" />
        <asp:HiddenField ID="hddnTypeIdd" runat="server" />
        <asp:HiddenField ID="hddnBRSConfigSettings" runat="server" />

        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_CustomerDeliveryPendingList" />
    </div>
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

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
    </div>

</asp:Content>
