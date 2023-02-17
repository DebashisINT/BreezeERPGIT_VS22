<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PurchaseQuotationList.aspx.cs" Inherits="ERP.OMS.Management.Activities.PurchaseQuotationList" %>



<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="JS/PurchaseQuotationList.js?v=2.0"></script>
    <style>
        strong label {
            font-weight: bold !important;
        }

        input[type="radio"] {
            webkit-transform: translateY(3px);
            -moz-transform: translateY(3px);
            transform: translateY(3px);
        }
    </style>
    <style>
        .smllpad > tbody > tr > td {
            padding-right: 25px;
        }

        .errorField {
            position: absolute;
            right: 5px;
            top: 9px;
        }

        .padTab {
            margin-bottom: 4px;
        }

            .padTab > tbody > tr > td {
                padding-right: 15px;
                vertical-align: middle;
            }

                .padTab > tbody > tr > td > label, .padTab > tbody > tr > td > input[type="button"] {
                    margin-bottom: 0 !important;
                }

        .backBranch {
            font-weight: 600;
            background: #75c1f5;
            padding: 5px;
        }
        .btn.typeNotificationBtn {
                position: relative;
            padding-right: 16px !important;
        }
        .typeNotification {
                 position: absolute;
                width: 22px;
                height: 22px;
                background: #ff5140;
                display: block;
                font-size: 12px;
                border-radius: 50%;
                right: -7px;
                top: -9px;
                line-height: 22px;
        }
    </style>
    <script>
        
        function OnApproveClick(keyValue, visibleIndex) {
            debugger;
            cGrdQuotation.SetFocusedRowIndex(visibleIndex);
            var IsBalMapQtyExists = cGrdQuotation.GetRow(cGrdQuotation.GetFocusedRowIndex()).children[7].innerHTML; //cGrdOrder.GetRow(cGrdOrder.GetFocusedRowIndex()).children[18].innerText;



            var IsCancelFlag = cGrdQuotation.GetRow(cGrdQuotation.GetFocusedRowIndex()).children[5].innerText;

            if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {


                var ActiveUser = '<%=Session["userid"]%>'
        if (ActiveUser != null) {
            $.ajax({
                type: "POST",
                url: "PurchaseQuotationList.aspx/GetEditablePermission",
                data: "{'ActiveUser':'" + ActiveUser + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,//Added By:Subhabrata
                success: function (msg) {
                    //debugger;
                    var status = msg.d;
                    var url = 'PurchaseQuotation.aspx?key=' + keyValue + '&Permission=' + status + '&type=PurQuote' + '&type1=AppPurQuote';
                    window.location.href = url;
                }
            });
        }
    }
    else {
                jAlert("Purchase Quotation  is " + IsCancelFlag.trim() + ". Approve is not allowed.");
    }
        }
        function OnMoreInfoClick(keyValue) {
            debugger;
            var ActiveUser = '<%=Session["userid"]%>'
            var IsCancelFlag = cGrdQuotation.GetRow(cGrdQuotation.GetFocusedRowIndex()).children[5].innerText;

            if (IsCancelFlag.trim() != 'Cancelled' && IsCancelFlag.trim() != 'Closed') {
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "PurchaseQuotationList.aspx/GetEditablePermissions",
                    //data: "{'ActiveUser':'" + ActiveUser + "'}",
                    //Rev Rajdip
                    data: "{'ActiveUser':'" + ActiveUser + "','SalesDocId':'" + keyValue + "'}",
                    //END Rev Rajdip
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var status = msg.d;
                        var url = 'PurchaseQuotation.aspx?key=' + keyValue + '&Permission=' + status + '&type=PurQuote';
                        window.location.href = url;
                    }
                });
             }
            }
            else {
                jAlert("Purchase Quotation is " + IsCancelFlag.trim() + ". Edit is not allowed.");
            }
        }
        //Rev Rajdip
        //Start For Copy
        function fn_CopySalesQuotation(keyValue) {
            var ActiveUser = '<%=Session["userid"]%>'
    if (ActiveUser != null) {
        $.ajax({
            type: "POST",
            url: "PurchaseQuotationList.aspx/GetEditablePermissions",
            //data: "{'ActiveUser':'" + ActiveUser + "'}",
            data: "{'ActiveUser':'" + ActiveUser + "','SalesDocId':'" + keyValue + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var status = msg.d;
                var url = 'PurchaseQuotation.aspx?key=' + keyValue + '&Permission=' + status + '&type=QO&Typenew=Copy';
                window.location.href = url;
            }
        });
    }

        }
        // Mantis Issue 25394
        function CallbackPanelEndCall(s, e) {
            cGrdQuotation.Refresh();
        }
        // End of Mantis Issue 25394
    </script>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
           <ClientSideEvents ControlsInitialized="AllControlInitilize" />
        </dxe:ASPxGlobalEvents>--%>
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Purchase Quotation</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px;">
            <tr>
                <td>
                    <label>From </label>
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <label>To </label>
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%" UseMaskBehavior="True">
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
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>New</span> </a><%} %>

            <%--<dxe:ASPxButton ID="btn_Approval" runat="server" class="btn btn-primary" Text="Pending Approval" ClientInstanceName="cbtn_Approval">
                <ClientSideEvents Click="function (s, e) {OpenPopUPApprovalStatus();}" />
            </dxe:ASPxButton>--%>
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
           <%-- <dxe:ASPxButton ID="btn_WaitQuote" ClientInstanceName="Cbtn_WaitQuote" runat="server" AutoPostBack="False" Text="Edit" CssClass="btn btn-primary" >
                                            <ClientSideEvents Click="function(s, e) {OpenWaitingQuote();}" />
            </dxe:ASPxButton>--%>
            <a href="javascript:void(0);" style="display:none" onclick="OpenWaitingQuote()" class="btn btn-warning typeNotificationBtn btn-radius"><span><u>Q</u>uotation Waiting </span>
                <span class="typeNotification"><dxe:ASPxLabel runat="server" Text="" ID="lblQuoteweatingCount" ClientInstanceName="ClblQuoteweatingCount"></dxe:ASPxLabel></span>
            </a>
            <span id="spanStatus" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPUserWiseQuotaion()" class="btn btn-primary btn-radius">
                    <span>My Quotation Status</span>
                    <%--<asp:Label ID="Label1" runat="server" Text=""></asp:Label>--%>                   
                </a>
            </span>
            <span id="divPendingWaiting" runat="server">
                <a href="javascript:void(0);" onclick="OpenPopUPApprovalStatus()" class="btn btn-info btn-radius">
                    <span>Approval Waiting</span>
                    <asp:Label ID="lblWaiting" runat="server" Text=""></asp:Label>
                </a>
                <i class="fa fa-reply blink" style="font-size: 20px; margin-right: 10px;" aria-hidden="true"></i>
            </span>
        </div>
    </div>
    <div class="GridViewArea relative">
        <div class="makeFullscreen ">
             <span class="fullScreenTitle">Purchase Quotation</span>
             <span class="makeFullscreen-icon half hovered " data-instance="cGrdQuotation" title="Maximize Grid" id="expandcGrdQuotation">
               <i class="fa fa-expand"></i>
             </span>

        <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Quote_Id" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
            Width="100%" ClientInstanceName="cGrdQuotation" OnCustomCallback="GrdQuotation_CustomCallback" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false"
             SettingsDataSecurity-AllowDelete="false" OnHtmlRowPrepared="AvailableStockgrid_HtmlRowPrepared" Settings-HorizontalScrollBarMode="Auto" >
           <SettingsSearchPanel Visible="True" Delay="5000" />
             <Columns>
                 <%--Rev Sayantani--%>
                <%--<dxe:GridViewDataTextColumn FieldName="Quote_Id" Visible="false" SortOrder="Descending">
                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>--%>
                 <dxe:GridViewDataTextColumn FieldName="Quote_Id" Visible="false" ShowInCustomizationForm="false" SortOrder="Descending">
                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                 <%--End of Rev Sayantani--%> 
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="QuotationNo"
                    VisibleIndex="0" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Quote_Date"
                    VisibleIndex="1" FixedStyle="Left" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Vendor" FieldName="CustomerName"
                    VisibleIndex="2" FixedStyle="Left">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataDateColumn Caption="Expiry Date" FieldName="Quote_Expiry"  PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy"
                    VisibleIndex="3" FixedStyle="Left" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <%--<PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>--%>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataDateColumn>
                <dxe:GridViewDataTextColumn Caption="Vendor Acceptance Status" FieldName="Status"
                    VisibleIndex="4" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount" HeaderStyle-HorizontalAlign="Right"
                    VisibleIndex="5" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>
                 <%-- Rev Rajdip --%>
                    <dxe:GridViewDataTextColumn Caption="Status" FieldName="IsClosed" Visible="True"
                    VisibleIndex="6" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                   <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Project Name" FieldName="Proj_Name" Width="120px">
                        <CellStyle CssClass="gridcellleft" Wrap="True"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                  <dxe:GridViewDataTextColumn Caption="Valid From" FieldName="ProjectValidFrom"
                    VisibleIndex="8" Width="150px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Valid Up To" FieldName="ProjectValidUpto"
                    VisibleIndex="8" Width="150px" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                   <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="RevNo"
                        Caption="Revision No." Width="120px">
                        <CellStyle HorizontalAlign="left" CssClass="gridcellleft">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="RevDate" Settings-AllowAutoFilter="False"
                        Caption="Revision Date" Width="180px">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                      <dxe:GridViewDataTextColumn Caption="Approval Status" FieldName="Project_ApproveStatus"
                    VisibleIndex="11" Width="150px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

                  <dxe:GridViewDataTextColumn Caption="Type" FieldName="Type" Visible="false"
                    VisibleIndex="12" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <%--  <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks" Visible="false"
                    VisibleIndex="7" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>--%>
                 <%-- End Rev Rajdip --%>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="13" Width="1">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico ColorFour'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                        <% } %>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" class="" title="" style='<%#Eval("PurchaseQuotationLastEntry")%>'>
                            
                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>
                        <% if (rights.CanDelete)
                           { %>
                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" title="" style='<%#Eval("PurchaseQuotationLastEntry")%>'>
                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a><%} %>
                        <%-- <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title="Copy ">
                            <i class="fa fa-copy"></i></a>--%>

                             <%-- Rev for approve reject--%>
                          <%--   <% if (rights.CanApproved)
                           { %>
                          <a href="javascript:void(0);" onclick="OnApproveClick('<%# Container.KeyValue %>')" class="" title="">
                            
                            <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Approve/Reject</span>

                        </a><% } %>--%>
                            <%-- Rev for approve reject--%>

                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="OnClickStatus('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico ColorSeven'><i class='fa fa-check'></i></span><span class='hidden-xs'>Status</span></a><%} %>




                             <% if (rights.CanApproved && IsApprove)
                           { %>
                          <a href="javascript:void(0);" onclick="OnApproveClick('<%#Eval("Quote_Id")%>',<%# Container.VisibleIndex %>)" class="" title="">
                            
                            <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Approve/Reject</span>

                        </a><% } %>


                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%# Container.KeyValue %>')" class="" title="">
                            <span class='ico ColorFive'><i class='fa fa-share-alt'></i></span><span class='hidden-xs'>Add/View Attachment</span>
                        </a><%} %>
                    
                      
                       
                      <%if (rights.CanClose)
                       { %>
                     <a href="javascript:void(0);" onclick="fn_CloseSalesOrder('<%# Container.KeyValue %>')" title="" class="">
                        <%--  <a href="javascript:void(0);" onclick="fn_Editcity(<%#Eval("sProducts_ID")%>)" title="" class="">--%>
                      <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Close</span>
          <%} %>

                           <% if (rights.CanCancel)
                           { %>
                          <a href="javascript:void(0);" onclick="OnCancelClick('<%# Container.KeyValue %>')" class="" title="">
                            
                            <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel</span>

                        </a><% } %>
                      
                            </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <%-- Rev RAJDIP --%>
                    <%--<HeaderTemplate><span>Actions</span></HeaderTemplate>--%>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <%--END Rev RAJDIP --%>
                    
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
            </Columns>
           <%-- --Rev Sayantani--%>
           <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" />
            <SettingsCookies Enabled="true" StorePaging="true"  Version="14.9" />
           <%-- -- End of Rev Sayantani --%>
             <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
            <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>
        
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>
        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="PurchaseQuotationList" />
        <asp:HiddenField ID="hiddenedit" runat="server" />
    </div></div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdQuotation" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <div class="PopUpArea">
        <%--Client Wise Quotation Status Section Start--%>

        <dxe:ASPxPopupControl ID="Popup_QuotationStatus" runat="server" ClientInstanceName="cQuotationStatus"
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
                                <table width="">
                                    <tr>
                                        <td style="padding-right: 8px; vertical-align: top">
                                            <strong>
                                                <label style="margin-bottom: 5px">Proforma:-</label></strong>
                                        </td>
                                        <td style="padding-top: 0; vertical-align: top; padding-right: 35px">
                                            <%--<dxe:ASPxTextBox ID="txt_Proforma" MaxLength="80" ClientInstanceName="cProforma" TabIndex="1" 
                                                runat="server" Width="100%"> 
                                            </dxe:ASPxTextBox>--%>
                                            <strong>
                                                <dxe:ASPxLabel ID="lbl_Proforma" runat="server" ClientInstanceName="cProforma" Text="ASPxLabel"></dxe:ASPxLabel>
                                            </strong>
                                        </td>
                                        <td style="padding-right: 8px; padding-left: 8px; vertical-align: top">
                                            <strong>
                                                <label style="margin-bottom: 5px">Customer:-</label></strong>
                                        </td>
                                        <td style="padding-top: 0; vertical-align: top">
                                            <%-- <dxe:ASPxTextBox ID="txt_Customer" ClientInstanceName="cCustomer"  runat="server" MaxLength="100" TabIndex="2"
                                            Width="100%"> 
                                        </dxe:ASPxTextBox>--%>
                                            <strong>
                                                <dxe:ASPxLabel ID="lbl_Customer" runat="server" ClientInstanceName="cCustomer" Text="ASPxLabel"></dxe:ASPxLabel>
                                            </strong>
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
                                <td style="width: 70px; padding: 13px 0;"><strong>Status </strong></td>
                                <td>
                                    <asp:RadioButtonList ID="rbl_QuoteStatus" runat="server" Width="172px" CssClass="mTop5" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="Accepted" Value="2" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Declined" Value="3"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="clear"></div>
                    <div class="col-md-12">
                        <div class="" style="margin-bottom: 5px;">
                            <strong>Reason </strong>
                        </div>
                        <div>
                            <dxe:ASPxMemo ID="txt_QuotationRemarks" runat="server" ClientInstanceName="cQuotationRemarks" Height="71px" Width="100%"></dxe:ASPxMemo>
                        </div>
                    </div>
                    <div class="col-md-12" style="padding-top: 10px;">
                        <dxe:ASPxButton ID="btn_PrpformaStatus" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                            AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                            <ClientSideEvents Click="function (s, e) {SavePrpformaStatus();}" />
                        </dxe:ASPxButton>
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>

        <%--Client Wise Quotation Status Section END--%>

        <%-- Sandip Approval Dtl Section Start--%>


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
                            <dxe:ASPxGridView ID="gridPendingApproval" runat="server" KeyFieldName="Quote_Id" AutoGenerateColumns="False" OnPageIndexChanged="gridPendingApproval_PageIndexChanged"
                                Width="100%" ClientInstanceName="cgridPendingApproval" OnCustomCallback="gridPendingApproval_CustomCallback">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Proforma/Quotation No." FieldName="Quote_Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Created On" FieldName="Quote_Date"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Caption="Unit" FieldName="branch_description"
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
                                                <%-- <ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <Settings ShowFilterRowMenu="False" AllowFilterBySearchPanel="False" AllowAutoFilter="False" />
                                    </dxe:GridViewDataCheckColumn>

                                    <dxe:GridViewDataCheckColumn UnboundType="Boolean" Caption="Rejected">
                                        <DataItemTemplate>
                                            <dxe:ASPxCheckBox ID="chkreject" runat="server" AllowGrayed="false" OnInit="chkreject_Init" ValueType="System.Boolean" ValueChecked="true" ValueUnchecked="false">
                                                <%-- <ClientSideEvents CheckedChanged="function (s, e) {ch_fnApproved();}" />--%>
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
            Width="900px" HeaderText="User Wise Quotation Status" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <dxe:ASPxGridView ID="gridUserWiseQuotation" runat="server" KeyFieldName="Quote_Id" AutoGenerateColumns="False"
                                Width="100%" ClientInstanceName="cgridUserWiseQuotation" OnCustomCallback="gridUserWiseQuotation_CustomCallback">
                                <Columns>
                                    <dxe:GridViewDataTextColumn Caption="Proforma/Quotation No." FieldName="Quote_Number"
                                        VisibleIndex="0" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Entered On" FieldName="createddate"
                                        VisibleIndex="1" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Approval User" FieldName="approvedby"
                                        VisibleIndex="2" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="User Level" FieldName="UserLevel"
                                        VisibleIndex="3" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Status" FieldName="status"
                                        VisibleIndex="4" FixedStyle="Left">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                    </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn Caption="Approved On" FieldName="ApprovedOn"
                                        VisibleIndex="5" FixedStyle="Left">
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

        <%-- Sandip Approval Dtl Section End--%>
    </div>
    <div class="PopUpArea">

<dxe:ASPxPopupControl ID="popup_QuoteWait" runat="server" Width="1200"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="Cpopup_QuoteWait"
        HeaderText="Quotation Waiting" AllowResize="false" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div onkeypress="OnWaitingGridKeyPress(event)">

                    <dxe:ASPxGridView ID="watingQuotegrid" runat="server" KeyFieldName="SBMain_Id" AutoGenerateColumns="False"
                        Width="100%" ClientInstanceName="CwatingQuotegrid" OnCustomCallback="watingQuotegrid_CustomCallback" KeyboardSupport="true"
                        SettingsBehavior-AllowSelectSingleRowOnly="true" OnDataBinding="watingQuotegrid_DataBinding" SettingsBehavior-AllowFocusedRow="true"
                        Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="320">
                        <Columns>
                            <%--<dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" Width="60" Caption="Select" />--%>

                            <dxe:GridViewDataTextColumn Caption="Salesman" FieldName="Salesman"
                                VisibleIndex="0" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Branch"
                                VisibleIndex="0" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="Customer" FieldName="Name"
                                VisibleIndex="1" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="Product List" FieldName="ProductList"
                                VisibleIndex="1" FixedStyle="Left" Width="180px">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>




                            <dxe:GridViewDataTextColumn Caption="Billing Amount" FieldName="finalAmount"
                                VisibleIndex="2" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="Billing Date" FieldName="Billingdate"
                                VisibleIndex="2" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Payment Type" FieldName="Paymenttype"
                                VisibleIndex="2" FixedStyle="Left">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="60">
                                <DataItemTemplate>
                                    <% if (rights.CanDelete)
                                       { %>
                                    <a href="javascript:void(0);" onclick="RemoveQuote('<%# Container.KeyValue %>')" class="pad" title="Remove">
                                        <%--   <img src="../../../assests/images/Delete.png" />--%>
                                        <i class="fa fa-close" aria-hidden="true" id="CloseRemoveWattingBtn" style="font-size: 19px; color: #f35248;"></i>
                                    </a>
                                    <%} %>
                                </DataItemTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                        </Columns>

                        <ClientSideEvents RowClick="ListRowClicked" EndCallback="watingQuotegridEndCallback" />

                        <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsLoadingPanel Text="Please Wait..." />
                    </dxe:ASPxGridView>
                </div>


                <dxe:ASPxButton ID="InvoiceWattingOk" runat="server" AutoPostBack="false" Text="O&#818;k" CssClass="btn btn-primary okClass"
                    ClientSideEvents-Click="QuotationWattingOkClick" UseSubmitBehavior="False" />
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    </div>
     <dxe:ASPxTimer runat="server" Interval="10000" ClientInstanceName="Timer1">
        <ClientSideEvents Tick="timerTick" />
    </dxe:ASPxTimer>
    <asp:HiddenField ID="waitingQuotationCount" runat="server" />
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

    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
        <asp:HiddenField ID="hfIsUserwise" runat="server" />
    </div>
    <%-- Rev Rajdip --%>
        <dxe:ASPxPopupControl ID="Popup_Closed" runat="server" ClientInstanceName="cPopup_Closed"
            Width="400px" HeaderText="Reason For Close" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">
                        <table style="width:94%">                           
                            <tr><td>Reason<span style="color: red">*</span></td>
                                <td class="relative">
                                     <dxe:ASPxMemo ID="ASPxMemo1" runat="server" Width="100%" Height="50px" ClientInstanceName="txtClosed"></dxe:ASPxMemo>
                                                                        <span id="MandatoryRemarksFeedback1" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                </td>
                            </tr>                           
                            <tr>
                                <td></td>
                                <td colspan="2" style="padding-top: 10px;">
                                    <input id="btnClosedSave" class="btn btn-primary" onclick="CallClosed_save()" type="button" value="Save" />&nbsp;&nbsp;
                                    <input id="btnClosedCancel" class="btn btn-danger" onclick="CancelClosed_save()" type="button" value="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />

          
        </dxe:ASPxPopupControl>
    <asp:HiddenField ID="hddnKeyValue" runat="server" />
    <%--End Rev Rajdip --%>

     <dxe:ASPxPopupControl ID="Popup_Feedback" runat="server" ClientInstanceName="cPopup_Feedback"
            Width="400px" HeaderText="Reason For Cancel" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">

                        <table style="width:94%">
                           
                            <tr><td>Reason<span style="color: red">*</span></td>
                                <td class="relative">
                                     <dxe:ASPxMemo ID="txtInstFeedback" runat="server" Width="100%" Height="50px" ClientInstanceName="txtFeedback"></dxe:ASPxMemo>
                                                                        <span id="MandatoryRemarksFeedback" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                </td></tr>
                             
                            <tr>
                                <td></td>
                                <td colspan="2" style="padding-top: 10px;">
                                    <input id="btnFeedbackSave" class="btn btn-primary" onclick="CallFeedback_save()" type="button" value="Save" />&nbsp;&nbsp;
                                    <input id="btnFeedbackCancel" class="btn btn-danger" onclick="CancelFeedback_save()" type="button" value="Cancel" />
                                </td>

                            </tr>
                        </table>


                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />

          
        </dxe:ASPxPopupControl>

    <%--Mantis Issue 25394--%>
     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">           
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <%--End of Mantis Issue 25394--%>

</asp:Content>

