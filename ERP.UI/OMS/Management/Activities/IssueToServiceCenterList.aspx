<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IssueToServiceCenterList.aspx.cs"  MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.IssueToServiceCenterList" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:content id="Content1" contentplaceholderid="head" runat="server">
    

     <script>
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
                 url: "IssueToServiceCenterList.aspx/GetChallanIdIsExistInSalesInvoice",
                 data: "{'keyValue':'" + keyValue + "'}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: false,//Added By:Subhabrata
                 success: function (msg) {
                     debugger;
                     var status = msg.d;
                     if (status == "1") {
                         jAlert('Sorry!Document tagged in Received From Service Centre. Cannot Delete.?', 'Confirmation Dialog', function (r) {
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
                 cGrdOrder.Refresh();
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
                     url: "IssueToServiceCenterList.aspx/GetEditablePermission",
                     data: "{'ActiveUser':'" + ActiveUser + "'}",
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     async: false,//Added By:Subhabrata
                     success: function (msg) {
                         debugger;
                         var status = msg.d;
                         $.ajax({
                             type: "POST",
                             url: "IssueToServiceCenterList.aspx/GetChallanIdIsExistInSalesInvoice",
                             data: "{'keyValue':'" + keyValue + "'}",
                             contentType: "application/json; charset=utf-8",
                             dataType: "json",
                             async: false,//Added By:Subhabrata
                             success: function (msg) {
                                 debugger;
                                 var status = msg.d;
                                 if (status == "1") {
                                     jConfirm('Tagged in Received From Service Centre. Cannot Moodfy.?', 'Confirmation Dialog', function (r) {
                                         if (r == true) {
                                             var url = 'IssueToServiceCenter.aspx?key=' + keyValue;
                                             window.location.href = url;
                                         }
                                     });
                                 }
                                 else
                                 {
                                     var url = 'IssueToServiceCenter.aspx?key=' + keyValue + '&Permission=' + status;
                                     window.location.href = url;
                                 }

                                 //return false;
                             }
                         });
                        
                     }
                 });
             }
         }

         ////##### coded by Samrat Roy - 04/05/2017  
         ////Add an another param to define request type 
         function OnViewClick(keyValue) {
             var url = 'IssueToServiceCenter.aspx?key=' + keyValue + '&req=V';
             window.location.href = url;
         }

         function OnAddButtonClick() {
             var url = 'IssueToServiceCenter.aspx?key=' + 'ADD';
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
                         //console.log(value);
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
</asp:content>

<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <dxe:ASPxPopupControl ID="Popup_OrderStatus" runat="server" ClientInstanceName="cOrderStatus"
                Width="500px" HeaderText="Approvers Configuration" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
                PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad" >
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-12">
                                <table width="100%">
                                    <tr>
                                        <td style="padding-right:20px">
                                            <label style="margin-bottom:5px">Issue Service Center</label> 
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
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Issue To Service Center</h3>
        </div>
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
        </div>
    </div>
    <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="Service_Id" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
            Width="100%" ClientInstanceName="cGrdOrder" OnCustomCallback="GrdOrder_CustomCallback" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" >
           <SettingsSearchPanel Visible="True" Delay="5000" />
             <Columns>
               <dxe:GridViewDataTextColumn Visible="False" FieldName="Service_Id" Caption="Service_Id" SortOrder="Descending">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                   <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Service_No"
                    VisibleIndex="0" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Service_Date"  
                    VisibleIndex="0" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                <%--<PropertiesTextEdit DisplayFormatString="dd-MM-yyyy" DisplayFormatInEditMode="True"></PropertiesTextEdit>--%>
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Service Center" FieldName="Service_Center"
                    VisibleIndex="1" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Call No" FieldName="Call_No"
                    VisibleIndex="2" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                  <dxe:GridViewDataTextColumn Caption="Issue Qty" FieldName="Issue_Qty"
                    VisibleIndex="3" FixedStyle="Left">
                    <PropertiesTextEdit DisplayFormatString="0.000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                    <PropertiesTextEdit>
                    <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                        
                   </PropertiesTextEdit>
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                      <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
             <dxe:GridViewDataTextColumn Caption="Narration" FieldName="Purpose"
                    VisibleIndex="4" FixedStyle="Left" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                 <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                
                <dxe:GridViewDataTextColumn Caption="Status" FieldName="Purpose"
                    VisibleIndex="5" FixedStyle="Left" Visible="false">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                         <% if (rights.CanView)
                            { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                           <% } %>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="" title="">
                            
                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>  <% } %>
                          <% if (rights.CanDelete)
                             { %>
                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a> 
                          <% } %>
                        <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="" title="" style="display:none">
                          <span class='ico ColorSix'><i class='fa fa-copy'></i></span><span class='hidden-xs'>Copy</span> </a>
                        <a href="javascript:void(0);" onclick="OnClickStatus('<%# Container.KeyValue %>')" class="" title="" style="display:none">
                            <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Status</span></a>
                         <% if (rights.CanView)
                            { %>
                        <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico ColorSix'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>Add/View Attachment</span></a>
                         <% } %>

                        </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <HeaderTemplate><span></span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
             <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
            <%--<SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>--%>

            <settingspager pagesize="10">
                           <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                             </settingspager>
          
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>
         <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="V_IssueToServiceCenterList" />
        <asp:HiddenField ID="hiddenedit" runat="server" />
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:content>

