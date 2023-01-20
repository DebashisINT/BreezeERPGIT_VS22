<%--==========================================================Revision History ============================================================================================   
 1.0   Priti   V2.0.36   19-01-2023    	0025313: Views to be converted to Procedures in the Listing Page of Transaction / Return-Sales / Sale Return-Manual
========================================== End Revision History =======================================================================================================--%>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReturnManualList.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Activities.ReturnManualList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .fwidth {
            width: 350px !important;
        }

        .padTabtype2 > tbody > tr > td {
            padding: 0px 5px;
        }

            .padTabtype2 > tbody > tr > td > label {
                margin-bottom: 0 !important;
                margin-right: 15px;
            }

        .dxeErrorFrameWithoutError_PlasticBlue .dxeControlsCell_PlasticBlue, .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue {
            padding: 0px !important;
        }
    </style>
    <script src="JS/ReturnManualList.js?2.0"></script>
    <%--Subhra--%>
    <script>
        function OnMoreInfoClick(keyValue) {
           
            var ActiveEInvoice = $('#hdnActiveEInvoice').val();
            if (ActiveEInvoice == "1") {
                $.ajax({
                    type: "POST",
                    url: "ReturnManualList.aspx/Prc_EInvoiceChecking_details",
                    data: "{'returnid':'" + keyValue + "','mode':'Edit'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function (msg) {
                        var status = msg.d;
                        if (status == "Yes") {
                            var ActiveUser = '<%=Session["userid"]%>'
                            if (ActiveUser != null) {
                                $.ajax({
                                    type: "POST",
                                    url: "ReturnManualList.aspx/GetEditablePermission",
                                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (msg) {
                                        var status = msg.d;
                                        var url = 'ReturnManual.aspx?key=' + keyValue + '&Permission=' + status + '&type=SRM';
                                        window.location.href = url;
                                    }
                                });
                            }
                        }
                        else {
                            jAlert("IRN generated can not edit.");
                        }
                    }
                });
            }
            else
            {
                var ActiveUser = '<%=Session["userid"]%>'
                if (ActiveUser != null) {
                    $.ajax({
                        type: "POST",
                        url: "ReturnManualList.aspx/GetEditablePermission",
                        data: "{'ActiveUser':'" + ActiveUser + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var status = msg.d;
                            var url = 'ReturnManual.aspx?key=' + keyValue + '&Permission=' + status + '&type=SRM';
                            window.location.href = url;
                        }
                    });
                }
            }
        }
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Sale Return Manual</h3>
        </div>
        <table class="padTabtype2 pull-right" id="gridFilter">
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
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span><u>A</u>dd New</span> </a><%} %>

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

      <div id="spnEditLock" runat="server" style="display:none; color:red;text-align:center"></div>
     <div id="spnDeleteLock" runat="server" style="display:none; color:red;text-align:center"></div>

    <div class="GridViewArea">
        <dxe:ASPxGridView ID="GrdSalesReturn" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGrdSalesReturn" OnCustomCallback="GrdSalesReturn_CustomCallback" SettingsBehavior-AllowFocusedRow="true"
            DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
            <SettingsSearchPanel Visible="True" Delay="5000" />
            <Styles>
                <Cell Wrap="False"></Cell>
            </Styles>
            <Columns>
                <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="SalesReturnNo"
                    VisibleIndex="0" FixedStyle="Left" Width="140px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="Return_Date" Caption="Posting Date" SortOrder="Descending" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy">
                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <EditFormSettings Visible="True"></EditFormSettings>
                </dxe:GridViewDataTextColumn>

                <%--<dxe:GridViewDataTextColumn Caption="Sales Invoice Number(s)" FieldName="Invoice"
                    VisibleIndex="2" FixedStyle="Left" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>--%>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                    VisibleIndex="2" FixedStyle="Left" Width="210px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                    VisibleIndex="3" FixedStyle="Left" Width="110px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Branch"
                    VisibleIndex="4" FixedStyle="Left" Width="150px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Assigned To Unit" FieldName="branchToBranch">
                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Status" FieldName="PStatus">
                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="E-Way Bill No" FieldName="EWayBillNumber" VisibleIndex="7" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                  <dxe:GridViewDataTextColumn Caption="Place of Supply[GST]" FieldName="PlaceOfSupply" VisibleIndex="8" Width="100px">
                  
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="IRN ?" FieldName="IsIRN" VisibleIndex="9" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="IRN" FieldName="IRN" VisibleIndex="10" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Ack No" FieldName="AckNo" VisibleIndex="11" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Ack Date" FieldName="AckDt" VisibleIndex="12" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="Return_CreateUser"
                    VisibleIndex="13">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Last Update On" FieldName="Return_CreateDateTime"
                    VisibleIndex="14">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="Return_ModifyUser"
                    VisibleIndex="15">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name" Width="150px"  VisibleIndex="16" Settings-AllowAutoFilter="True">
                           <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="true" />
                           <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="200px">
                    <DataItemTemplate>
                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Return_Id")%>')" class="pad" title="View">
                            <img src="../../../assests/images/doc.png" /></a>
                        <% } %>
                        <% if (rights.CanEdit)
                           { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("Return_Id")%>')" class="pad" title="Edit" style='<%#Eval("AssignStatus")%>'>
                            <img src="../../../assests/images/info.png" /></a><%} %>
                        <% if (rights.CanDelete)
                           { %>
                        <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("Return_Id")%>')" class="pad" title="Delete" style='<%#Eval("Deletelock")%>'>
                            <img src="../../../assests/images/Delete.png" /></a><%} %>

                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%#Eval("Return_Id")%>')" class="pad" title="View Attachment">
                            <img src="../../../assests/images/attachment.png" />
                        </a><%} %>
                        <a href="javascript:void(0);" onclick="OnEWayBillClick('<%#Eval("Return_Id")%>','<%#Eval("EWayBillNumber") %>','<%#Eval("EWayBillDate") %>','<%#Eval("EWayBillValue") %>')" class="pad" title="Update E-Way Bill">
                            <img src="../../../assests/images/bill.png" />
                            <% if (rights.CanPrint)
                               { %>
                            <a href="javascript:void(0);" onclick="onPrintJv('<%#Eval("Return_Id")%>')" class="pad" title="print">
                                <img src="../../../assests/images/Print.png" />
                            </a><%} %>

                            <% if (rights.CanAssignbranch)
                               {
                          
                            %>

                            <a href="javascript:void(0);" class="pad" title="Branch Assignment" id="a_Assignment" onclick="OnTransferClick('<%#Eval("Return_Id")%>')" style='<%#Eval("AssignStatus")%>'>
                                <span class="fa fa-truck out"></span>
                            </a>

                            <a href="javascript:void(0);" class="pad" title="Cancel Branch Assignment" id="Cancel_Assignment" onclick="onCancelBranchAssignment('<%#Eval("Return_Id")%>')" style='<%#Eval("UnAssignStatus")%>'>
                                <img src="../../../assests/images/crossT.png" />
                            </a><%
                               } %>
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
            <SettingsPager PageSize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
            <%--<SettingsSearchPanel Visible="True" />--%>
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>


        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesReturnManualList" />
        <asp:HiddenField ID="hiddenedit" runat="server" />
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
                                                    Width="100%" Height="80px"></asp:TextBox>
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
                                                <b><span id="tagged" style="display: none; color: red">Advance Already Adjusted. Cannot Modify data</span></b>
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
        <asp:HiddenField ID="hddnSalesReturnID" runat="server" />
    </div>
    <dxe:ASPxPopupControl ID="Popup_EWayBill" runat="server" ClientInstanceName="cPopup_EWayBill"
        Width="400px" HeaderText="Update E-Way Bill" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="150px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div class="Top clearfix">

                    <table style="width: 100%; margin: 0 auto; margin-top: 5px;">

                        <tr>
                            <label>
                                <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="E-Way Bill Number">
                                </dxe:ASPxLabel>
                            </label>

                            <dxe:ASPxTextBox ID="txtEWayBillNumber" MaxLength="12" ClientInstanceName="ctxtEWayBillNumber"
                                runat="server" Width="100%">
                                <MaskSettings Mask="<0..999999999999>" AllowMouseWheel="false" />
                                <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                            </dxe:ASPxTextBox>
                        </tr>

                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="E-Way Bill Date">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxDateEdit ID="dt_EWayBill" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_EWayBill" Width="100%">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>


                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="E-Way Bill Value">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxTextBox ID="txtEWayBillValue" ClientInstanceName="ctxtEWayBillValue"
                                    runat="server" Width="100%">
                                    <MaskSettings Mask="<0..999999999>.<0..99>" AllowMouseWheel="false" />
                                    <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                                </dxe:ASPxTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="margin-top: 10px;">
                        <input id="btnEWayBillSave" class="btn btn-primary" onclick="CallEWayBill_save()" type="button" value="Save" />
                        <input id="btnEWayBillCancel" class="btn btn-danger" onclick="CancelEWayBill_save()" type="button" value="Cancel" />
                        <dxe:ASPxLabel ID="lblEwayBillStatus" runat="server" Text="" style="color: red; font-size:large"></dxe:ASPxLabel>
                    </div>

                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />


    </dxe:ASPxPopupControl>

    <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
    <asp:HiddenField ID="hdnLockToDateedit" runat="server" />
 
    <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />

    <asp:HiddenField ID="hdnLockFromDateeditDataFreeze" runat="server" />
    <asp:HiddenField ID="hdnLockToDateeditDataFreeze" runat="server" />
 
    <asp:HiddenField ID="hdnLockFromDatedeleteDataFreeze" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedeleteDataFreeze" runat="server" />
    <asp:HiddenField ID="hdnActiveEInvoice" runat="server" />

     <%-- REV 1.0--%>
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
    <PanelCollection>
    <dxe:PanelContent runat="server">           
    </dxe:PanelContent>
    </PanelCollection>
    <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <asp:HiddenField ID="hFilterType" runat="server" />
    <%--END REV 1.0--%>
</asp:Content>
