<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PendingDelivery.aspx.cs"
    Inherits="ERP.OMS.Management.Activities.PendingDelivery" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" %>

<%@ Register Src="~/OMS/Management/Activities/UserControls/VehicleDetailsControl.ascx" TagPrefix="uc1" TagName="VehicleDetailsControl" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(function () {
            //added By:Subhabrata
            $("#btntransporter").hide();
            $("#vehicleTransporter").hide();
            $("#PendingDlvTransporter").show();
            //End
        });
    </script>
    <script>

        var isFirstTime = true;
        function AllControlInitilize() {

            if (isFirstTime) {

                if (localStorage.getItem('FromDatePendingDelivery')) {
                    var fromdatearray = localStorage.getItem('FromDatePendingDelivery').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }

                if (localStorage.getItem('ToDatePendingDelivery')) {
                    var todatearray = localStorage.getItem('ToDatePendingDelivery').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('BranchPendingDelivery')) {
                    if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('BranchPendingDelivery'))) {
                        ccmbBranchfilter.SetValue(localStorage.getItem('BranchPendingDelivery'));
                    }

                }
                //updateGridByDate();

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
                localStorage.setItem("FromDatePendingDelivery", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDatePendingDelivery", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("BranchPendingDelivery", ccmbBranchfilter.GetValue());
                // cGrdOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                cGrdOrder.Refresh();

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
                window.location.reload();
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
        function onPrintJv(id, ChallanDocNo) {

            PDChallan_id = ChallanDocNo;
            cSelectPanel.cpSuccess = "";
            if (ChallanDocNo != "") {
                cDocumentsPopup.Show();
                CselectDuplicate.SetEnabled(false);
                CselectTriplicate.SetEnabled(false);
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
                cSelectPanel.PerformCallback('Bindalldesignes');
                $('#btnOK').focus();
            }
            else {
                jAlert('This challan not yet delivered.Nothing to Print.');
            }
        }

        //Subhabrata on 13-07-2017
        function onClickTransporter(id, ChallanDocNo) {
            debugger;
            $("#<%=hddnChallanDocNo.ClientID%>").val(ChallanDocNo);

            var isExitsChallanIdInTrns = false;
            if (ChallanDocNo == "") {
                jAlert('Delivery not done for this Sale Challan. Cannot proceed.');
            }
            else {
               
                $.ajax({
                    type: "POST",
                    url: "PendingDeliveryList.aspx/GetChallanTransporterDetails",
                    data: JSON.stringify({ Challan_Id: ChallanDocNo }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        debugger;
                        var ObjData = msg.d;
                        if (ObjData > 0) {
                           
                            isExitsChallanIdInTrns = true;

                            callTransporterControlFromUndelivered(ChallanDocNo, 'SC', 'PendingDelivery');
                           
                            $('#exampleModal').modal({
                                show: 'true'
                            });
                        }

                    }

                });
                if (isExitsChallanIdInTrns == false) {
                    //$("#DivFreight").show();
                    //$("#Div_VehicleOutDate").show();
                    $("#callBackuserControlPanel").hide();
                    $("#DivFreight").hide();
                    $("#FreghtField").hide();


                    

                    

                    $("#Div_VehicleOutDate").show();
                    $("#cAddress").hide();
                    $("#cCNBilityLRNo").hide();
                    $("#cTotalTaxes").hide();
                    $("#cTolltax").hide();
                    $("#cWeightChanrges").hide();
                    $("#cUnloading").hide();
                    $("#cLoading").hide();
                    $("#cPoint").hide();
                    $("#FreghtField").hide();
                    $("#divGSTIN").hide();

                    $("#cParking").hide();
                    $("#cPhoneNo").hide();
                    $("#cArea").hide();
                    $("#cPin").hide();
                    $("#cDistrict").hide();
                    $("#cState").hide();
                    $("#cCountry").hide();
                    $("#td_FinalTransporter").css('display', 'block');
                    
                    $('#exampleModal').modal({
                        show: 'true'
                    });

                    //$('#exampleModal').show();
                }
                else {
                    //cGrdOrder.PerformCallback('BindTransPorter~' + ChallanDocNo);

                }

            }
            $("#cServiceTaxes").hide(); /// added mantis issue 17756

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
                CselectDuplicate.SetEnabled(false);
                CselectTriplicate.SetEnabled(false);
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
            }
        }

        function OnMoreInfoClickOur(keyValue, vissibleInx) {
            debugger;
            cGrdOrder.SetFocusedRowIndex(vissibleInx);
            var ChallanDocNo = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[9].innerText;
            if (ChallanDocNo.trim() != "") {
                jAlert('Already Delivered.You can only view !', 'Confirmation Dialog', function (r) {
                    if (r == true) {
                        //debugger;
                        var url = 'SalesChallanAdd.aspx?key=' + ChallanDocNo + '&type=SC' + '&DlvTyeId=' + '2';
                        window.location.href = url;
                    }
                });
            }
            else {
                //Cahnged 12.09.2017--- For Doc Type
                var url = 'CustomerPendingDelivery.aspx?key=' + keyValue + '&Flag=' + 'PendingDeliveryFlag' + '&type=SI';
                window.location.href = url;
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

                                var status1 = msg.d;
                                $.ajax({
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
                                        var ChallanDocNo = cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[9].innerText;
                                        if (status1 == "1") {
                                            jAlert('Already Delivered.You can only view !', 'Confirmation Dialog', function (r) {
                                                if (r == true) {
                                                    //debugger;
                                                    var url = 'SalesChallanAdd.aspx?key=' + ChallanDocNo + '&type=SC' + '&DlvTyeId=' + '2';
                                                    window.location.href = url;
                                                }
                                            });
                                        }
                                        else {
                                            var url = 'SalesChallanAdd.aspx?key=' + keyValue + '&Permission=' + status + '&CustID=' + Customer_Id + '&Flag=' + 'PendingDeliveryFlag' + '&BillDate=' + BillDate;
                                            window.location.href = url;
                                        }



                                    }
                                });
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
    <link href="CSS/PendingDelivery.css" rel="stylesheet" />
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
                                            <label style="margin-bottom:5px">Pending Delivery</label> 
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
            <h3>Undelivered List</h3>
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
                <td>
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
                </td>

            </tr>

        </table>
    </div>

    <div class="form_main" style="display: none;">
        <div class="clearfix">


            <% if (rights.CanExport)
               { %>
            <%-- <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>--%>
            <% } %>
        </div>
    </div>
    <div class="clear"></div>
    <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="Bill_Id" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGrdOrder" DataSourceID="EntityServerModeDataSource"  OnCustomCallback="GrdOrder_CustomCallback" SettingsBehavior-AllowFocusedRow="true"
            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" HorizontalScrollBarMode="Auto">
            <columns>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Bill_Number"
                    VisibleIndex="0" Width="120px" FixedStyle="Left" Settings-AutoFilterCondition="Contains">
                     <Settings AutoFilterCondition="Contains" />
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Bill_Date"
                    VisibleIndex="1" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                    VisibleIndex="2" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Unit" FieldName="BranchName"
                    VisibleIndex="3" Width="200px" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                    VisibleIndex="4" >
                    <PropertiesTextEdit DisplayFormatString="0.000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                    <PropertiesTextEdit>
                    <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                        
                   </PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                
                <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status"
                    VisibleIndex="5"  >
                    <CellStyle CssClass="gridcellleft Pending" Wrap="true" >
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Challan No." FieldName="ChallanNo"
                    VisibleIndex="6" Width="120px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="ChallanDate"
                    VisibleIndex="7" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Transporter Name" FieldName="TransporterName"
                    VisibleIndex="8" Width="220px" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Transporter Type" FieldName="TransporterType"
                    VisibleIndex="9" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Resgistered" FieldName="IsRegistered"
                    VisibleIndex="10" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Vehicle No" FieldName="VehicleNo"
                    VisibleIndex="11" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Vehicle Out Date" FieldName="VehicleOutDateTime" width="150"
                    VisibleIndex="12" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="ChallanDocNo" FieldName="ChallanDocNo"
                    VisibleIndex="13"  Width="0" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                    <CellStyle CssClass="hide" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Customer_Id"
                    VisibleIndex="14"  Width="0" Visible="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center"
                    VisibleIndex="15"  >
                    
                    <DataItemTemplate>
                         <a href="javascript:void(0);" onclick="onClickTransporter('<%# Container.KeyValue %>', '<%# Eval("ChallanDocNo") %>')" class="pad" title="Delivery Receipt">
                            <img src="../../../assests/images/tow-truck.png" width="16px" />
                        </a>
                        </DataItemTemplate>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="16" Width="0">
                    <DataItemTemplate>
                          
                        <div class='floatedBtnArea'>
                        <% if (rights.CanEdit)
                           { %>

                       

                        <a href="javascript:void(0);" onclick="OnMoreInfoClickOur('<%# Container.KeyValue %>',<%# Container.VisibleIndex %>)" style="display:none" class="pad" title="">
                            
                             <span class='ico ColorFour'><i class="fa fa-truck out" ></i></span><span class='hidden-xs'>Create Delivery Challan</span></a>  <% } %>
                        
                        <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title=" " style="display:none">
                          <span class='ico ColorSix'><i class='fa fa-copy'></i></span><span class='hidden-xs'>Copy</span> </a>
                        <a href="javascript:void(0);" onclick="OnClickStatus('<%# Container.KeyValue %>')" class="pad" title="" style="display:none">
                            <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Status</span></a>
                         <% if (rights.CanView)
                            { %>
                        <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" style="display:none" class="pad" title="">
                            <span class='ico ColorSix'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>View Attachment</span> </a>
                         <% } %>
                        <% if (rights.CanPrint)
                           { %>
                        <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>', '<%# Eval("ChallanDocNo") %>')" class="pad" title="">
                           <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                        </a><%} %>
                        
                        <a href="javascript:void(0);" onclick="onClickTransporter('<%# Container.KeyValue %>', '<%# Eval("ChallanDocNo") %>')" class="pad" title="">
                            <span class='ico ColorFour'><i class="fa fa-truck out" ></i></span><span class='hidden-xs'>Delivery Receipt</span>
                        </a>
                            </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span></span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </columns>

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
            <settings showgrouppanel="True" showfooter="true" showgroupfooter="VisibleIfExpanded" showstatusbar="Hidden"
                showhorizontalscrollbar="true" showfilterrow="true" showfilterrowmenu="true" />
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

         <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_GetUndeliveredList" />
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
                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" ClientSideEvents-CheckedChanged="OrginalCheckChange"
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" ClientSideEvents-CheckedChanged="DuplicateCheckChange"
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>
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
