<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BranchTransferInList.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master"
     Inherits="ERP.OMS.Management.Activities.BranchTrasferInList" %>


<asp:content id="Content1" contentplaceholderid="head" runat="server">
    

     <script>
         document.onkeydown = function (e) {
             if (event.keyCode == 18) isCtrl = true;


             if (event.keyCode == 65 && isCtrl == true) { //run code for alt+a -- ie, Add
                 StopDefaultAction(e);
                 OnAddButtonClick();
             }

         }


         var isFirstTime = true;
         function AllControlInitilize() {
             debugger;
             if (isFirstTime) {

                 if (localStorage.getItem('FromDateBTI')) {
                     var fromdatearray = localStorage.getItem('FromDateBTI').split('-');
                     var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                     cFormDate.SetDate(fromdate);
                 }

                 if (localStorage.getItem('ToDateBTI')) {
                     var todatearray = localStorage.getItem('ToDateBTI').split('-');
                     var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                     ctoDate.SetDate(todate);
                 }
                 if (localStorage.getItem('BranchBTI')) {
                     if (ccmbBranchfilter.FindItemByValue(localStorage.getItem('BranchBTI'))) {
                         ccmbBranchfilter.SetValue(localStorage.getItem('BranchBTI'));
                     }

                 }
                 //updateGridByDate();

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
                 localStorage.setItem("FromDateBTI", cFormDate.GetDate().format('yyyy-MM-dd'));
                 localStorage.setItem("ToDateBTI", ctoDate.GetDate().format('yyyy-MM-dd'));
                 localStorage.setItem("BranchBTI", ccmbBranchfilter.GetValue());
                 cGrdOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());

             }
         }
         //End

         function StopDefaultAction(e) {
             if (e.preventDefault) { e.preventDefault() }
             else { e.stop() };

             e.returnValue = false;
             e.stopPropagation();
         }
         function OnClickDelete(keyValue) {
             debugger;
             jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                 if (r == true) {
                     cGrdOrder.PerformCallback('Delete~' + keyValue);
                 }
             });
         }

         function OnclickViewAttachment(obj) {
             var URL = '/OMS/Management/Activities/EntriesDocuments.aspx?idbldng=' + obj + '&type=BTIN';
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

         function OnMoreInfoClick(keyValue) {
             debugger;
             var ActiveUser = '<%=Session["userid"]%>'
             if (ActiveUser != null) {
                 $.ajax({
                     type: "POST",
                     url: "BranchTransferInList.aspx/GetEditablePermission",
                     data: "{'ActiveUser':'" + ActiveUser + "'}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: false,//Added By:Subhabrata
                     success: function (msg) {
                         debugger;
                         var status = msg.d;
                         var url = 'BranchTransferIn.aspx?key=' + keyValue + '&Permission=' + status + '&type=BI';
                         window.location.href = url;
                     }
                 });
             }
         }

         ////##### coded by Samrat Roy - 04/05/2017  
         ////Add an another param to define request type 
         function OnViewClick(keyValue) {
             var url = 'BranchTransferIn.aspx?key=' + keyValue + '&req=V';
             window.location.href = url;
         }

         function OnAddButtonClick() {
             var url = 'BranchTransferIn.aspx?key=' + 'ADD';
             window.location.href = url;
         }
         //});

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
</asp:content>

<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
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
                                            <label style="margin-bottom:5px">Branch Transfer In</label> 
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
            <h3>Branch Transfer In</h3>
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
    <div class="form_main">
        <div class="clearfix">
             <% if (rights.CanAdd)
                { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary"><span><u>A</u>dd New</span> </a>
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
        <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="Stk_Id" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGrdOrder" OnCustomCallback="GrdOrder_CustomCallback" OnDataBinding="GrdOrder_DataBinding" OnSummaryDisplayText="ShowGrid_SummaryDisplayText"
             SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true" 
            SettingsBehavior-AllowFocusedRow="true" 
            >
            <Columns>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Out_No"
                    VisibleIndex="0" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Date" FieldName="Out_Date"
                    VisibleIndex="0" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="From Branch" FieldName="From_Branch"
                    VisibleIndex="1" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="To Branch" FieldName="To_Branch"
                    VisibleIndex="2" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                  <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                    VisibleIndex="3" FixedStyle="Left">
                    <PropertiesTextEdit DisplayFormatString="0.000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                    <PropertiesTextEdit>
                    <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                        
                   </PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
             <dxe:GridViewDataTextColumn Caption="Purpose" FieldName="Purpose"
                    VisibleIndex="4" FixedStyle="Left" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                
                <dxe:GridViewDataTextColumn Caption="Status" FieldName="Purpose"
                    VisibleIndex="5" FixedStyle="Left" Visible="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="15%">
                    <DataItemTemplate>
                        <% if (rights.CanView)
                            { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="pad" title="View">
                            <img src="../../../assests/images/doc.png" /></a>
                           <% } %>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="pad" title="Edit">
                            
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
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
             <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" />
            <%--<SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>--%>
               <settingspager pagesize="10">
                           <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                             </settingspager>
            <SettingsSearchPanel Visible="True" />
            <Settings ShowGroupPanel="True" ShowFooter="true"  ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
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
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
</asp:content>