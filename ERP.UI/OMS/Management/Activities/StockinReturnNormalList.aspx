<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                13-04-2023        2.0.37           Pallab              25815 :Receive Against Normal Return page design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="StockinReturnNormalList.aspx.cs" Inherits="ERP.OMS.Management.Activities.StockinReturnNormalList" %>


<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
     Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>

        .fwidth{width:350px !important;}
           .padTabtype2 > tbody > tr > td {
            padding: 0px 5px;
        }

            .padTabtype2 > tbody > tr > td > label {
                margin-bottom: 0 !important;
                margin-right: 15px;
            }
    </style>
     <%--Subhra--%>
    <script>
        function OnMoreInfoClick(keyValue, TransferBranch) {
            var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {
                $.ajax({
                    type: "POST",
                    url: "StockinReturnNormalList.aspx/GetEditablePermission",
                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var status = msg.d;
                        var url = 'StockinReturnNormal.aspx?key=' + keyValue + '&Permission=' + status + '&type=SRN' + '&TBID=' + TransferBranch;
                        window.location.href = url;
                    }
                });
            }
        }
    </script>
    
<script src="JS/StockinReturnNormalList.js"></script>


    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 0;
        }

        #GrdSalesReturn {
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

        .calendar-icon
        {
                right: 10px;
        }
    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Receive Against Normal Return</h3>
        </div>
    </div>
        <div class="form_main">
        <div class="clearfix">
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

             <table class="padTabtype2 pull-right" id="gridFilter">
                        <tr>
                            <td>
                                <label>From Date</label></td>
                            <%--Rev 1.0: "for-cust-icon" class add --%>
                            <td class="for-cust-icon">
                                <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
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
                                <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
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

                        </tr>

                    </table>
        </div>
    </div>
        <div class="GridViewArea">
        <dxe:ASPxGridView ID="GrdSalesReturn" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False" 
            Width="100%" ClientInstanceName="cGrdSalesReturn" OnCustomCallback="GrdSalesReturn_CustomCallback" SettingsBehavior-AllowFocusedRow="true" 
           
         DataSourceID="EntityServerModeDataSource"   SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  Setting-HorizontalScrollBarMode="Auto" >
           <SettingsSearchPanel Visible="True" Delay="5000" />
             <Columns>
                 <dxe:GridViewDataTextColumn Caption="SI" FieldName="SrlNo"
                    VisibleIndex="0" FixedStyle="Left"  Width="50px"  CellStyle-HorizontalAlign="center" >
                    <CellStyle CssClass="gridcellleft" Wrap="true" >
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="SalesReturnNo"
                    VisibleIndex="1" FixedStyle="Left"  Width="140px" >
                    <CellStyle CssClass="gridcellleft" Wrap="true" >
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                  <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Return_Date" Caption="Posting Date" SortOrder="Descending">
                                               <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                      <Settings AllowAutoFilterTextInputTimer="False" />
                                                <EditFormSettings Visible="True"></EditFormSettings>
                                           </dxe:GridViewDataTextColumn>
                <%-- <dxe:GridViewDataTextColumn Caption="Date" FieldName="Return_Date"
                    VisibleIndex="2" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>--%>

                   <dxe:GridViewDataTextColumn Caption="Invoice Number(s)" FieldName="Invoice"
                    VisibleIndex="3"  Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                       <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                   <dxe:GridViewDataTextColumn Caption="Date(s)" FieldName="InvoiceDate"
                    VisibleIndex="4" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                       <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                    VisibleIndex="5" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                    VisibleIndex="6" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                     <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>               
               
                 <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Branch"
                    VisibleIndex="7" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>   
                 <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Assigned To Unit" FieldName="branchToBranch" Width="130px">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
            </dxe:GridViewDataTextColumn> 

           <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Project Name" FieldName="Proj_Name" CellStyle-HorizontalAlign="center">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                      <Settings AllowAutoFilterTextInputTimer="False" />
            </dxe:GridViewDataTextColumn>

                  <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="Return_CreateUser"
                    VisibleIndex="9" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                      <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>   

                  <dxe:GridViewDataTextColumn Caption="Last Update On" FieldName="Return_CreateDateTime"
                    VisibleIndex="10" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                      <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>   
                  <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="Return_ModifyUser"
                    VisibleIndex="11"  >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                      <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>   
                  <dxe:GridViewDataTextColumn VisibleIndex="12" Caption="Transfer Status" FieldName="PStatus" >
                <CellStyle Wrap="False" CssClass="gridcellleft" ></CellStyle>
                      <Settings AllowAutoFilterTextInputTimer="False" />
            </dxe:GridViewDataTextColumn>
                       <dxe:GridViewDataTextColumn VisibleIndex="13" Caption="Received Status" FieldName="RStatus" CellStyle-HorizontalAlign="center">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                           <Settings AllowAutoFilterTextInputTimer="False" />
            </dxe:GridViewDataTextColumn>


                 <dxe:GridViewDataTextColumn Caption="Assigned By" FieldName="Return_AssignedUser"
                    VisibleIndex="14"  >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>    
                 <dxe:GridViewDataTextColumn Caption="Assigned Date" FieldName="Return_AssignedDateTime"
                    VisibleIndex="15"  >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>    
                <dxe:GridViewDataTextColumn Caption="Received By" FieldName="Return_ReceivedUser"    
                    VisibleIndex="16"  >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn> 
                  <dxe:GridViewDataTextColumn Caption="Received Date" FieldName="Return_ReceivedDateTime"
                    VisibleIndex="17"  >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                      <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>       

                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="18"  Width="130px">
                    <DataItemTemplate>
                         <% if (rights.CanView)
                            { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Return_Id")%>')" class="pad" title="View">
                            <img src="../../../assests/images/doc.png" /></a>
                           <% } %>
                        <% if (rights.CanEdit)
                                       { %>
                       <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("Return_Id")%>','<%#Eval("BranchID")%>')" class="pad" title="StockIn">
                            <img src="../../../assests/images/warehouse.png" /></a><%} %>
                     
                   <% if (rights.CanPrint)
                           { %>
                        <a href="javascript:void(0);" onclick="onPrintJv('<%#Eval("Return_Id")%>')" class="pad" title="print">
                            <img src="../../../assests/images/Print.png" />
                        </a><%} %>
             
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
             <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents EndCallback="OnEndCallback" />
           <%-- <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>--%>
            <settingspager pagesize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                              </settingspager>
           <%-- <SettingsSearchPanel Visible="True" />--%>
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden"  HorizontalScrollBarMode="Visible"  ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>
            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_StockInReturnNormalList" />
        <asp:HiddenField ID="hiddenedit" runat="server" />
    </div>
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdSalesReturn" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>

    

      <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="Popup_BranchTransfer" runat="server" ClientInstanceName="cPopup_BranchTransfer"
            Width="450px" HeaderText="Branch Assignment" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="200px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="BranchTransferCallBackPanel" ClientInstanceName="cBranchTransferCallBackPanel" OnCallback="BranchTransferCallBackPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <div style="background: #f5f4f3; padding: 5px 0 5px 0; margin-bottom: 10px; border-radius: 4px; border: 1px solid #ccc;">
                                    <div class="Top clearfix">
                                        <div class="col-md-6">
                                            <label>Assigned To Unit<span style="color: red">*</span></label>
                                            <div>
                                                <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch" 
                                                    DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%">
                                                </asp:DropDownList>
                                                <span id="MandatoryBranch" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none"
                                                    title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <%--<div class="col-md-6" id="tdCashBankLabel">
                                            <label>Cash/Bank <span style="color: red">*</span></label>
                                            <dxe:ASPxComboBox ID="ddlCashBank" runat="server" ClientIDMode="Static" ClientInstanceName="cddlCashBank" Width="100%" OnCallback="ddlCashBank_Callback">
                                                
                                            </dxe:ASPxComboBox>
                                            <span id="MandatoryCashBank" class="iconCashBank pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>--%>
                                        <div class="clear"></div>
                                        <div class="col-md-12 lblmTop8">
                                            <label>Remarks </label>
                                            <div>
                                                <asp:TextBox ID="txtNarration" runat="server" MaxLength="500"
                                                    TextMode="MultiLine"
                                                    Width="100%" Height="80px" ></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <table style="width: 100%;">
                                    <tr>
                                        <td style="padding: 5px 0;">
                                            <div class="text-center">
                                                <dxe:ASPxButton ID="btnSaveNew" ClientInstanceName="cbtnSaveNew" runat="server"
                                                    AutoPostBack="false" CssClass="btn btn-primary" TabIndex="0" Text="Save & C&#818;lose"
                                                    UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                                </dxe:ASPxButton>
                                                  <b><span id="tagged" style="display:none;color: red">Advance Already Adjusted. Cannot Modify data</span></b>
                                            </div>

                                        </td>
                                    </tr>
                                </table>
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="BranchTransferEndCallBack" />
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>

    </div>
      <div id="DivDataSource">
        <asp:SqlDataSource ID="dsBranch" runat="server" 
            ConflictDetection="CompareAllValues"
            SelectCommand=""></asp:SqlDataSource>
    </div>
      <asp:HiddenField ID="hdnEditID" runat="server" />

      <%--SUBHRA--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" 
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" 
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
    </div>
</asp:Content>

