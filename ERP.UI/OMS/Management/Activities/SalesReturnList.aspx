<%--==========================================================Revision History ============================================================================================   
 1.0   Priti    V2.0.36   18-01-2023     	0025311: Views to be converted to Procedures in the Listing Page of Transaction / Return-Sales / Sales Return
 2.0   Pallab   V2.0.37   12-04-2023     	Transactions pages design modification
========================================== End Revision History =======================================================================================================--%>


<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="SalesReturnList.aspx.cs" Inherits="ERP.OMS.Management.Activities.SalesReturnList" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function OnMoreInfoClick(keyValue) {

            var statusmode = "";
            var ActiveEInvoice = $('#hdnActiveEInvoice').val();
            if (ActiveEInvoice == "1") {
                $.ajax({
                    type: "POST",
                    url: "SalesReturnList.aspx/Prc_EInvoiceChecking_details",
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
                                    url: "SalesReturnList.aspx/GetEditablePermission",
                                    data: "{'ActiveUser':'" + ActiveUser + "'}",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (msg) {
                                        var status = msg.d;
                                        var url = 'SalesReturn.aspx?key=' + keyValue + '&Permission=' + status + '&type=SR';
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
            else {
                var ActiveUser = '<%=Session["userid"]%>'
                if (ActiveUser != null) {
                    $.ajax({
                        type: "POST",
                        url: "SalesReturnList.aspx/GetEditablePermission",
                        data: "{'ActiveUser':'" + ActiveUser + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            var status = msg.d;
                            var url = 'SalesReturn.aspx?key=' + keyValue + '&Permission=' + status + '&type=SR';
                            window.location.href = url;
                        }
                    });
                }
            }           
        }
    </script>
    <link href="CSS/SalesReturnList.css" rel="stylesheet" />
    <script src="JS/SalesReturnList.js?v=2.2"></script>

    <style>
        /*Rev 2.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px !important;
            /*-webkit-appearance: none;
            position: relative;
            z-index: 1;*/
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 7px;
            right: 10px;
            z-index: 0;
            cursor: pointer;
        }

        #FormDate , #toDate , #ASPxDateEditFrom , #ASPxDateEditTo
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #FormDate_B-1 , #toDate_B-1 , #ASPxDateEditFrom_B-1 , #ASPxDateEditTo_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #ASPxDateEditFrom_B-1 #ASPxDateEditFrom_B-1Img , #ASPxDateEditTo_B-1 #ASPxDateEditTo_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        select.btn
        {
            padding-right: 10px !important;
        }

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .mb-10{
            margin-bottom: 10px !important;
        }

        /*.btn
        {
            padding: 5px 10px;
        }*/


        /*Rev end 2.0*/

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 2.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Sales Return</h3>
        </div>
        <table class="padTabtype2 pull-right" id="gridFilter">
            <tr>
                <td>
                    <label>From Date</label></td>
                <%--Rev 2.0: "for-cust-icon" class add --%>
                <td class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 2.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 2.0--%>
                </td>
                <td>
                    <label>To Date</label>
                </td>
                <%--Rev 2.0: "for-cust-icon" class add --%>
                <td class="for-cust-icon">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                    <%--Rev 2.0--%>
                    <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                    <%--Rev end 2.0--%>
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
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>A</u>dd New</span> </a><%} %>

            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>
        </div>
    </div>
    
        <div id="spnEditLock" runat="server" style="display: none; color: red; text-align: center"></div>
        <div id="spnDeleteLock" runat="server" style="display: none; color: red; text-align: center"></div>

        <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--   <dxe:ASPxGridView runat="server" KeyFieldName="ID" ClientInstanceName="cgriddocuments" ID="grid_Documents"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridDocuments_CustomCallback" ClientSideEvents-EndCallback="cgridDocumentsEndCall"
                        Settings-ShowFooter="false" AutoGenerateColumns="False"
                        Settings-VerticalScrollableHeight="100" Settings-VerticalScrollBarMode="Hidden">
                                                      
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="10" Caption=" "  />




                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="ID" Width="0" ReadOnly="true" Caption="No." CellStyle-CssClass="hide" HeaderStyle-CssClass="hide">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="NAME" Width="" ReadOnly="true" Caption="Design(s)">
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                      
                    </dxe:ASPxGridView>--%>
                    <%-- <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
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
                    </dxe:ASPxCallbackPanel>--%>
                </dxe:PopupControlContentControl>

            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
        <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdSalesReturn" runat="server" KeyFieldName="SrlNo" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGrdSalesReturn" OnCustomCallback="GrdSalesReturn_CustomCallback" SettingsBehavior-AllowFocusedRow="true"
            DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"
             Settings-HorizontalScrollBarMode="Auto" >
            <SettingsSearchPanel Visible="true" Delay="5000" />
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
                <%--<dxe:GridViewDataTextColumn Caption="Date" FieldName="Return_Date"
                    VisibleIndex="1" FixedStyle="Left" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>--%>

                <dxe:GridViewDataTextColumn Caption="Invoice Number(s)" FieldName="Invoice"
                    VisibleIndex="2" Width="130px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Date(s)" FieldName="InvoiceDate"
                    VisibleIndex="3">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Customer" FieldName="CustomerName"
                    VisibleIndex="4" FixedStyle="Left" Width="210px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount"
                    VisibleIndex="5" FixedStyle="Left" Width="110px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="E-Way Bill No" FieldName="EWayBillNumber" VisibleIndex="6" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Place of Supply[GST]" FieldName="PlaceOfSupply" VisibleIndex="7" Width="100px">

                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name" Width="150px" VisibleIndex="8" Settings-AllowAutoFilter="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="true" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="IRN ?" FieldName="IsIRN" VisibleIndex="9" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                 <dxe:GridViewDataTextColumn Caption="IRN" FieldName="IRN" VisibleIndex="10" Width="200px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Ack No" FieldName="AckNo" VisibleIndex="11" Width="200px">
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
               
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="16" Width="0">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>

                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Return_Id")%>')" class="" title="">
                                <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                            <% } %>
                            <% if (rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("Return_Id")%>')" class="" title="" style='<%#Eval("Editlock")%>'>
                                <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>
                            <% if (rights.CanDelete)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("Return_Id")%>')" class="" title="" style='<%#Eval("Deletelock")%>'>
                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a><%} %>
                            <%-- <a href="javascript:void(0);" onclick="OnClickCopy('<%# Container.KeyValue %>')" class="pad" title="Copy ">
                            <i class="fa fa-copy"></i></a>--%>
                            <%--   <% if (rights.CanEdit)
                                       { %>
                        <a href="javascript:void(0);" onclick="OnClickStatus('<%# Container.KeyValue %>')" class="pad" title="Status">
                            <img src="../../../assests/images/verified.png" /></a><%} %>--%>
                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%#Eval("Return_Id")%>')" class="" title="">
                                <span class='ico ColorSix'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>View Attachment</span>
                            </a><%} %>
                            <a href="javascript:void(0);" onclick="OnEWayBillClick('<%#Eval("Return_Id")%>','<%#Eval("EWayBillNumber") %>','<%#Eval("EWayBillDate") %>','<%#Eval("EWayBillValue") %>')" class="" title="">
                                <span class='ico ColorFour'><i class='fa fa-file-text-o'></i></span><span class='hidden-xs'>Update E-Way Bill</span>
                                <% if (rights.CanPrint)
                                   { %>
                                <a href="javascript:void(0);" onclick="onPrintJv('<%#Eval("Return_Id")%>')" class="" title="">
                                    <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                                </a><%} %>

                                <a href="javascript:void(0);" onclick="onInfluencerCommissionReturn('<%#Eval("Return_Id")%>')" class="" title="">
                                    <span class='ico editSeven'><i class='fa fa-user-plus' aria-hidden='true'></i></span><span class='hidden-xs'>Influecer Return</span></a>
                        </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span></span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
            </Columns>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents EndCallback="OnEndCallback" RowClick="gridRowclick" />
            <%-- <SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>--%>
            <SettingsPager PageSize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
            <%-- <SettingsSearchPanel Visible="True" />--%>
            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
        </dxe:ASPxGridView>


        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesReturnList" />
        <asp:HiddenField ID="hiddenedit" runat="server" />
    </div>
    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GrdSalesReturn" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>




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
        <asp:HiddenField ID="hdnActiveEInvoice" runat="server" />
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
                                <dxe:ASPxDateEdit ID="dt_EWayBill" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_EWayBill" Width="100%" UseMaskBehavior="True">
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





















    <%-------Influencer Return------%>


    <div class="modal fade pmsModal w80" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document" style="width: 95%;">
            <div class="modal-content">
                <div class="modal-header">

                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Influencer</h4>
                </div>
                <div class="modal-body" style="overflow-y: scroll; max-height: 450px; overflow-x: hidden;">

                    <div class="clearfix">
                        <div class="Top clearfix">
                            <ul class="list-inline">
                                <li>
                                    <div class="lblHolder" id="">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>Invoice Number </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="width: 100%;">
                                                            <b id="txtInvoiceNumber" style="text-align: center">0.00</b>
                                                        </div>

                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </li>
                                <li>
                                    <div class="lblHolder">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>Amount</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="width: 100%;">
                                                            <span id="txtInvoiceAmount">Peekay Group</span>
                                                        </div>

                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </li>
                                <li id="liJV" class="hide">
                                    <div class="lblHolder">
                                        <table>
                                            <tbody>
                                                <tr>
                                                    <td>Auto Journal Number</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div style="width: 100%;">
                                                            <span id="txtAutoJV">Peekay Group</span>
                                                        </div>

                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="clearfix">
                        <ul class="smallList  hide">
                            <li>On Qty - Commission calculated on the Qty of the selected Item</li>
                            <li>Amount before GST - Commission calculated as percentage of the Amount before GST is charged.</li>
                            <li>Amount after GST - Commission calculated as percentage of the Amount with GST charged.</li>
                            <li>Flat Value - Flat commission amount irrespective of Qty and Value</li>
                        </ul>
                    </div>


                    <div class="clearfix padTopBot mBot10">
                        <table id="tableProduct" class="table table-striped table-bordered display " style="width: 100%">
                        </table>
                    </div>


                    <div style="background: #f5f4f3; padding: 8px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc; margin-bottom: 10px;" class="clearfix padTopBot">
                        <div class="col-md-3" id="div_Edit">
                            <label>Select Numbering Scheme For Journal</label>
                            <div>
                                <asp:DropDownList ID="CmbScheme" runat="server" DataSourceID="SqlSchematype"
                                    DataTextField="SchemaName" DataValueField="ID" Width="100%"
                                    onchange="CmbScheme_ValueChange()">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlSchematype" runat="server"
                                    SelectCommand="Select * From ((Select '0' as ID,'Select' as SchemaName) Union (Select ID,SchemaName  + 
                                (Case When (SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch) Is Null Then '' 
                                Else ' ('+(SELECT Isnull(branch_description,'') FROM tbl_master_branch where branch_id=Branch)+')' End) From tbl_master_Idschema  Where TYPE_ID='1' AND IsActive=1 AND Isnull(Branch,'') in (select s FROM dbo.GetSplit(',',@userbranchHierarchy)) AND Isnull(comapanyInt,'')=@LastCompany AND financial_year_id=(Select Top 1 FinYear_ID FROM Master_FinYear WHERE FinYear_Code=@LastFinYear))) as x Order By ID asc">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="LastCompany" SessionField="LastCompany" />
                                        <asp:SessionParameter Name="LastFinYear" SessionField="LastFinYear" />
                                        <asp:SessionParameter Name="userbranchHierarchy" SessionField="userbranchHierarchy" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Document No.</label>
                            <div>
                                <asp:TextBox ID="txtBillNo" runat="server" Width="95%" meta:resourcekey="txtBillNoResource1" MaxLength="30" onchange="txtBillNo_TextChanged()"></asp:TextBox>
                                <span id="MandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                <span id="duplicateMandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Duplicate Journal No."></span>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label style="">Posting Date</label>
                            <div>
                                <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy" ClientInstanceName="tDate"
                                    UseMaskBehavior="True" Width="100%" meta:resourcekey="tDateResource1">
                                </dxe:ASPxDateEdit>
                            </div>
                        </div>
                        <div class="col-md-3 hide">
                            <div class="mroonText">Something goes here</div>
                        </div>

                    </div>

                    <div class="borderTopBottom clear padTopBot mBot10 ">

                        <div class="row mBot10 ">
                            <div class="col-md-2" style="padding-top: 8px;">
                                <label>Commission Expense Ledger</label>
                                <dxe:ASPxButtonEdit runat="server" ID="btnMAdr" ClientInstanceName="cbtnMAdr" class="form-control" Width="100%">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                </dxe:ASPxButtonEdit>
                            </div>
                            <div class="col-md-2 hide" style="padding-top: 8px;">
                                <label>Posting Ledger Cr.</label>
                                <dxe:ASPxButtonEdit runat="server" ID="btnMainAccount" ClientEnabled="false" ClientInstanceName="cbtnMainAccount" class="form-control hide" Width="100%">

                                    <Buttons>

                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                </dxe:ASPxButtonEdit>
                            </div>
                            <div class="col-md-3 hide">
                                <label>Influencer</label>
                                <dxe:ASPxButtonEdit runat="server" ID="txtInfluencer" ClientInstanceName="ctxtInfluencer" class="form-control" Width="100%">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>

                                </dxe:ASPxButtonEdit>
                            </div>
                            <div class="col-md-2 padTop25 hide">
                                <dxe:ASPxButton runat="server" AutoPostBack="False" UseSubmitBehavior="false" ID="btnSaveInfluencer" ClientInstanceName="cbtnSaveInfluencer" CssClass="btn btn-primary onDxe" Text="+">
                                </dxe:ASPxButton>
                            </div>
                            <div class="col-md-5">
                                <ul class="list-inline pull-right" style="margin-top: 13px; margin-bottom: 0;">
                                    <li>
                                        <div class="lblHolder" id="">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <td>Total Amount </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div style="width: 100%;">
                                                                <b id="txtCommAmt" style="text-align: center">0.00</b>
                                                            </div>

                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </li>

                                </ul>
                            </div>
                        </div>
                    </div>
                    <table id="influencerGrid" class="table table-striped table-bordered display ">
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" id="divDeleteinf" onclick="DeleteInfluencerdetails();" class="btn btn-danger">Delete</button>
                    <button type="button" id="divSaveinf" onclick="SaveInfluencerdetails();" class="btn btn-success">Save and Exit</button>

                    <label class="red hide" id="divmsg">Payment for this commission has already been made. Edit/Delete is not allowed</label>

                </div>
            </div>
        </div>
    </div>



    <input type="hidden" id="ddlBranch" />
    <input type="hidden" id="invid" />
    <input type="hidden" id="infid" />
    <input type="hidden" id="mainaccrid" />
    <input type="hidden" id="mainacdrid" />


    <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
    <asp:HiddenField ID="hdnLockToDateedit" runat="server" />

    <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />

    <asp:HiddenField ID="hdnLockFromDateeditDataFreeze" runat="server" />
    <asp:HiddenField ID="hdnLockToDateeditDataFreeze" runat="server" />

    <asp:HiddenField ID="hdnLockFromDatedeleteDataFreeze" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedeleteDataFreeze" runat="server" />
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
