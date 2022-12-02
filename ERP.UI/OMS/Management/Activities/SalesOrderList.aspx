<%@ Page Language="C#" Title="Sales Order" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="SalesOrderList.aspx.cs" Inherits="ERP.OMS.Management.Activities.SalesOrderList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.1.0/jquery-confirm.min.css">
    <%--Code Added By Sandip For Approval Detail Section Start--%>
     <script>
          <%-- Code block By Debashis Talukder--%>
         //function onPrintJv(id) {
         //    window.location.href = "../../reports/XtraReports/Viewer/OrderReportViewer.aspx?id=" + id;
         //}
         <%-- Code block By Debashis Talukder--%>

         //This function is called to show the Status of All Sales Order Created By Login User Start
         function OpenPopUPUserWiseQuotaion() {
             cgridUserWiseQuotation.PerformCallback();
             cPopupUserWiseQuotation.Show();
         }
         // function above  End

         //This function is called to show all Pending Approval of Sales Order whose Userid has been set LevelWise using Approval Configuration Module 
         function OpenPopUPApprovalStatus() {
             cgridPendingApproval.PerformCallback();
             cpopupApproval.Show();
         }
         // function above  End


         // Status 2 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
         function GetApprovedQuoteId(s, e, itemIndex) {
             var rowvalue = cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);
             //cgridPendingApproval.PerformCallback('Status~' + rowvalue);
             //cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetApprovedRowValues);

         }
         function OnGetApprovedRowValues(obj) {
             uri = "SalesOrderAdd.aspx?key=" + obj + "&status=2" + '&type=SO';
             popup.SetContentUrl(uri);
             popup.Show();
         }
         // function above  End For Approved

         // Status 3 is passed If Approved Check box is checked by User Both Below function is called and used to show in POPUP,  the Add Page of Respective Segment(like Page for Adding Quotation ,Sale Order ,Challan)
         function GetRejectedQuoteId(s, e, itemIndex) {
             debugger;
             cgridPendingApproval.GetRowValues(itemIndex, 'ID', OnGetRejectedRowValues);

         }
         function OnGetRejectedRowValues(obj) {
             uri = "SalesOrderAdd.aspx?key=" + obj + "&status=3" + '&type=SO';
             popup.SetContentUrl(uri);
             popup.Show();
         }
         // function above  End For Rejected

         // To Reflect the Data in Pending Waiting Grid and Pending Waiting Counting if the user approve or Rejecte the Order and Saved 

         function OnApprovalEndCall(s, e) {
             $.ajax({
                 type: "POST",
                 url: "SalesOrderList.aspx/GetPendingCase",
                 contentType: "application/json; charset=utf-8",
                 success: function (data) {
                     $('#<%= lblWaiting.ClientID %>').text(data.d);
                }
            });
         }

         // function above  End 

     </script>



   <%-- Code Added By Sandip For Approval Detail Section End--%>

     <script>
         <%-- Code Added By Debashis Talukder For Document Printing End--%>
         var SOrderId = 0;
         function onPrintJv(id) {
             //window.location.href = "../../reports/XtraReports/Viewer/PurchaseOrderReportViewer.aspx?id=" + id;
             debugger;
             SOrderId = id;
             cDocumentsPopup.Show();
             cCmbDesignName.SetSelectedIndex(0);
             cSelectPanel.PerformCallback('Bindalldesignes');
             $('#btnOK').focus();
         }

         function PerformCallToGridBind() {
             cSelectPanel.PerformCallback('Bindsingledesign');
             cDocumentsPopup.Hide();
             return false;
         }

         var isFirstTime = true;
         function AllControlInitilize() {
             debugger;
             if (isFirstTime) {

                 if (localStorage.getItem('FromDateSalesOrder')) {
                     var fromdatearray = localStorage.getItem('FromDateSalesOrder').split('-');
                     var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                     cFormDate.SetDate(fromdate);
                 }

                 if (localStorage.getItem('ToDateSalesOrder')) {
                     var todatearray = localStorage.getItem('ToDateSalesOrder').split('-');
                     var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                     ctoDate.SetDate(todate);
                 }
                 if (localStorage.getItem('OrderBranch')) {
                     if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('OrderBranch'))) {
                         ccmbBranchfilter.SetValue(localStorage.getItem('OrderBranch'));
                     }

                 }
                 updateGridByDate();

                 isFirstTime = false;
             }
         }

         //Function for Date wise filteration
         function updateGridByDate() {
             debugger;
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
                 cGrdOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

             }
         }
         //End

         function cSelectPanelEndCall(s, e) {
             debugger;
             if (cSelectPanel.cpSuccess != null) {
                 var TotDocument = cSelectPanel.cpSuccess.split(',');
                 var reportName = cCmbDesignName.GetValue();
                 var module = 'Sorder';
                 window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + SOrderId, '_blank')
             }
             cSelectPanel.cpSuccess = null
             if (cSelectPanel.cpSuccess == null) {
                 cCmbDesignName.SetSelectedIndex(0);
             }
         }
         <%-- Code Added By Debashis Talukder For Document Printing End--%>
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
             debugger;
             $.ajax({
                 type: "POST",
                 url: "SalesOrderList.aspx/GetSalesInvoiceIsExistInSalesInvoice",
                 data: "{'keyValue':'" + keyValue + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: false,//Added By:Subhabrata
                 success: function (msg) {
                     debugger;
                     var status = msg.d;
                     if (status == "1") {
                         jAlert('Used in other module(s). Cannot Delete.', 'Confirmation Dialog', function (r) {
                             if (r == true) {
                                 return false;
                             }
                         });
                     }
                     else {
                         jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                             if (r == true) {
                                 cGrdOrder.PerformCallback('Delete~' + keyValue);
                             }
                         });
                     }
                 }
             });
         }

         function OnclickViewAttachment(obj) {
             //var URL = '/OMS/Management/Activities/SalesOrder_Document.aspx?idbldng=' + obj + '&type=SalesOrder';
             var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=SalesOrder';
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
                 var pro_status = cGrdOrder.cpEdit.split('~')[2]
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

         function OnMoreInfoClick(keyValue) {
            debugger;
             var ActiveUser = '<%=Session["userid"]%>'
                 if (ActiveUser != null) {
                     $.ajax({
                         type: "POST",
                         url: "SalesOrderList.aspx/GetEditablePermission",
                         data: "{'ActiveUser':'" + ActiveUser + "'}",
                         contentType: "application/json; charset=utf-8",
                         dataType: "json",
                         async:false,//Added By:Subhabrata
                         success: function (msg) {
                             debugger;
                             var status = msg.d;
                             var url = 'SalesOrderAdd.aspx?key=' + keyValue + '&Permission=' + status + '&type=SO';
                             window.location.href = url;
                         }
                     });
                 }
         }

         ////##### coded by Samrat Roy - 04/05/2017  
         ////Add an another param to define request type 
         function OnViewClick(keyValue) {
             var url = 'SalesOrderAdd.aspx?key=' + keyValue + '&req=V';
             window.location.href = url;
         }

             function OnAddButtonClick() {
                 var url = 'SalesOrderAdd.aspx?key=' + 'ADD';
                 window.location.href = url;
             }
         //});
             $(document).ready(function () {
                 
             });
             
    </script>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxPopupControl ID="Popup_OrderStatus" runat="server" ClientInstanceName="cOrderStatus"
                Width="500px" HeaderText="Approvers Configuration" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad">
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-12">
                                <table width="100%">
                                    <tr>
                                        <td style="padding-right:20px">
                                            <label style="margin-bottom:5px">Proforma</label> 
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
            </ContentCollection>
        </dxe:ASPxPopupControl>
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Sales Order</h3>
        </div>
         <table class="padTab pull-right" style="margin-top:7px">
            <tr>
                <td>
                    <label>From </label></td>
                <td style="width:150px">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <label>To </label>
                </td>
                <td style="width:150px">
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
    <div class="form_main">
        <div class="clearfix">
             <% if (rights.CanAdd)
                                   { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span><u>A</u>dd New</span> </a>
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

            <%--Sandip Section for Approval Section in Design Start --%>

            <span id="spanStatus" runat="server">
            <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary">
                    <span>My Sales Order Status</span>
                    <%--<asp:Label ID="Label1" runat="server" Text=""></asp:Label>--%>                   
                </a>
                </span>
            <span id="divPendingWaiting" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPApprovalStatus()" class="btn btn-primary">
                    <span>Approval Waiting</span>

                    <asp:Label ID="lblWaiting" runat="server" Text=""></asp:Label>                   
                </a>
                 <i class="fa fa-reply blink" style="font-size: 20px;margin-right: 10px;" aria-hidden="true"></i>
                
            </span>

            <%--Sandip Section for Approval Section in Design End --%>

        </div>
    </div>
    <div class="GridViewArea">
        <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="Order_Id" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGrdOrder" OnCustomCallback="GrdOrder_CustomCallback"  SettingsBehavior-AllowFocusedRow="true"
            HorizontalScrollBarMode="Auto"
             OnSummaryDisplayText="ShowGrid_SummaryDisplayText">
            <Columns>
                <dxe:GridViewDataTextColumn Caption="Doc No." FieldName="OrderNo"
                    VisibleIndex="1" FixedStyle="Left"  Width="140px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Date" FieldName="Order_Date"
                    VisibleIndex="2"  Width="150px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                    VisibleIndex="3" Width="300px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn> 
                <dxe:GridViewDataTextColumn Caption="Branch" FieldName="BranchName"
                    VisibleIndex="4" Width="250px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                    VisibleIndex="5"  Width="100">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>                    
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>

                <%--<dxe:GridViewDataTextColumn Caption="Challan No" FieldName="ChallanNo"
                    VisibleIndex="6"  Width="150">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="Challan_Date"
                    VisibleIndex="7"  Width="150" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>--%>

                <dxe:GridViewDataTextColumn Caption="Vehicle No" FieldName="VehicleNos"
                    VisibleIndex="6"  Width="120">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Vehicle Out Date" FieldName="VehicleOutDateTime"
                    VisibleIndex="7"  Width="120">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Invoice No" FieldName="InvoiceNo"
                    VisibleIndex="8"  Width="150">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Invoice Date" FieldName="InvoiceDate"
                    VisibleIndex="9"  Width="150">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                    VisibleIndex="10"  Width="80">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Last Modified On" FieldName="LastModifiedOn"
                    VisibleIndex="11"  Width="70">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
               <%-- <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="UpdatedBy"
                    VisibleIndex="12" FixedStyle="Left" Width="80">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>--%>
                <dxe:GridViewDataTextColumn Caption="Created From" FieldName="CreatedFrom"
                    VisibleIndex="12"  Width="120">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                
                
                <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status"
                    VisibleIndex="111"  Visible="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="150px">
                    <DataItemTemplate>
                         <% if (rights.CanView)
                            { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="pad" title="View">
                            <img src="../../../assests/images/doc.png" /></a>
                           <% } %>
                        <% if (rights.CanEdit)
                                        { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="pad" title="Modify">
                            
                            <img src="../../../assests/images/info.png" /></a>  <% } %>
                          <% if (rights.CanDelete)
                                       { %>
                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete">
                            <img src="../../../assests/images/Delete.png" /></a> 
                          <% } %>
                        <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title="Copy " style="display:none">
                          <i class="fa fa-copy"></i>  </a>
                        <a href="javascript:void(0);" onclick="OnClickStatus('<%# Container.KeyValue %>')" class="pad" title="Status" style="display:none">
                            <img src="../../../assests/images/verified.png" /></a>
                         <% if (rights.CanView)
                                       { %>
                        <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="pad" title="View Attachment">
                            <img src="../../../assests/images/attachment.png" />  </a>
                         <% } %>
                        <% if (rights.CanPrint)
                                       { %>
                         <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="pad" title="print">
                            <img src="../../../assests/images/Print.png" />
                        </a><%} %>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
             <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" />
              <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
            <%--<SettingsSearchPanel Visible="True" />--%>
            <Settings ShowGroupPanel="True" ShowFooter="true"  ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="false" />
            <SettingsLoadingPanel Text="Please Wait..." />
            <TotalSummary>
                                       <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" /> 
                                 </TotalSummary>
        </dxe:ASPxGridView>
        <asp:HiddenField ID="hiddenedit" runat="server" />
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
      <%-- Sandip Approval Dtl Section Start--%>
    <div class="PopUpArea"> 
        <dxe:ASPxPopupControl ID="popupApproval" runat="server" ClientInstanceName="cpopupApproval"
            Width="900px" HeaderText="Pending Approvals" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridPendingApproval" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"  OnPageIndexChanged="gridPendingApproval_PageIndexChanged" 
                                Width="100%" ClientInstanceName="cgridPendingApproval"   OnCustomCallback="gridPendingApproval_CustomCallback">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Sales Order No." FieldName="Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Party Name" FieldName="customer"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Date" FieldName="CreateDate"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Branch" FieldName="branch_description"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="craetedby"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Approved">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkapprove" runat="server" AllowGrayed="false" OnInit="chkapprove_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                <%--<ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                    </dxe:GridViewDataCheckColumn>

                                    <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Rejected">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkreject" runat="server" AllowGrayed="false" OnInit="chkreject_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                             <%--<ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                    </dxe:GridViewDataCheckColumn>
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" />
                                <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <SettingsEditing Mode="Inline">
                                </SettingsEditing>
                                <SettingsSearchPanel Visible="True" />
                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />
                                <ClientSideEvents EndCallback="OnApprovalEndCall" />
                            </dxe:ASPxGridView>
                        </div>
                        <div class="clear"></div>


                        <%--<div class="col-md-12" style="padding-top: 10px;">
                            <dxe:ASPxButton ID="ASPxButton1" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                                AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                <ClientSideEvents Click="function (s, e) {SaveApprovalStatus();}" />
                            </dxe:ASPxButton>
                        </div>--%>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl> 
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
            Width="1200px" HeaderText="Quotation Approval" Modal="true" AllowResize="true" ResizingMode="Postponed">
            <HeaderTemplate>
                <span>User Approval</span> 
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl> 
        <dxe:ASPxPopupControl ID="PopupUserWiseQuotation" runat="server" ClientInstanceName="cPopupUserWiseQuotation"
            Width="900px" HeaderText="User Wise Sales Order Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad"> 
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridUserWiseQuotation" runat="server" KeyFieldName="ID" AutoGenerateColumns="False" 
                                Width="100%" ClientInstanceName="cgridUserWiseQuotation"  OnCustomCallback="gridUserWiseQuotation_CustomCallback" OnPageIndexChanged="gridUserWiseQuotation_PageIndexChanged">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Sale Order No." FieldName="number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Approval User" FieldName="approvedby"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn> 
                                   
                                    <dxe:GridViewDataTextColumn Caption="User Level" FieldName="UserLevel"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                     <dxe:GridViewDataTextColumn Caption="Status" FieldName="status"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="true" />
                                <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <SettingsEditing Mode="Inline">
                                </SettingsEditing>
                                <SettingsSearchPanel Visible="True" />
                                <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsLoadingPanel Text="Please Wait..." />
                                 
                            </dxe:ASPxGridView>
                        </div>
                        <div class="clear"></div> 
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl> 
    </div>    <%-- Sandip Approval Dtl Section End--%>
   
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
</asp:Content>

