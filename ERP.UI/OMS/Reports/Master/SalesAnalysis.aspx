<%@ Page Title="Sales Analysis" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SalesAnalysis.aspx.cs" Inherits="ERP.OMS.Reports.Master.SalesAnalysis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function CloseLookup() {
            clookupClass.ConfirmCurrentSelection();
            clookupClass.HideDropDown();
            clookupClass.Focus();
        }

        function _CloseLookup() {
            clookupBrand.ConfirmCurrentSelection();
            clookupBrand.HideDropDown();
            clookupBrand.Focus();
        }

        function btn_ShowRecordsClick(e) {
            $("#drdExport").val(0);
            $("#ddldetails").val(0);
            cShowGrid.PerformCallback();
        }

        function selectAll() {
            clookupClass.gridView.SelectRows();
            document.getElementById("hflookupClassAllFlag").value = "ALL";
        }

        function unselectAll() {
            clookupClass.gridView.UnselectRows();
            document.getElementById("hflookupClassAllFlag").value = "";
        }

        function _selectAll() {
            clookupBrand.gridView.SelectRows();
            document.getElementById("hflookupBrandAllFlag").value = "ALL";

        }

        function _unselectAll() {
            clookupBrand.gridView.UnselectRows();
            document.getElementById("hflookupClassAllFlag").value = "";
        }

        function OpenAnalysisDetails(brandID,classID, productID, Rate) {
            $("#drdExport").val(0);
            $("#ddldetails").val(0);
            $('#<%=hdnCategoryID.ClientID %>').val(brandID);
            $('#<%=hdnClassID.ClientID %>').val(classID);
            $('#<%=hdnProductID.ClientID %>').val(productID);
            $('#<%=hdnRate.ClientID %>').val(Rate);

            cCallbackPanel.PerformCallback(productID);
            cShowGridDetails.PerformCallback();
            cpopup.Show();
        }

        function popupHide(s, e) {
            cpopup.Hide();
        }

        function CallbackPanelEndCall(s, e) {
            if (cCallbackPanel.cpProductValue != null) {
                var ProductValue = cCallbackPanel.cpProductValue;
                cCallbackPanel.cpProductValue = null;

                var productCode = ProductValue.split('||@||')[0].replace("squot", "'").replace("squot", "'").replace("coma", ",").replace("slash", "/");
                var productName = ProductValue.split('||@||')[1].replace("squot", "'").replace("squot", "'").replace("coma", ",").replace("slash", "/");

                ctxtProductCode.SetValue(productCode);
                ctxtProductName.SetValue(productName);
            }
        }

        function closePopup(s, e) {
            e.cancel = false;

            $("#drdExport").val(0);
            $("#ddldetails").val(0);
            ctxtProductCode.SetValue("");
            ctxtProductName.SetValue("");
            $('#<%=hdnCategoryID.ClientID %>').val("");
            $('#<%=hdnClassID.ClientID %>').val("");
            $('#<%=hdnProductID.ClientID %>').val("");
            $('#<%=hdnRate.ClientID %>').val("");

             cShowGridDetails.PerformCallback();
         }

         $(document).keydown(function (e) {
             if (e.keyCode == 27) {
                 cpopup.Hide();
                 cShowGrid.Focus();
             }
         });

         function OnWaitingGridKeyPress(e) {

             if (e.code == "Enter") {
                 cShowGrid.GetRowValues(cShowGrid.GetFocusedRowIndex(), 'CategoryID;ClassID;sProducts_ID;Pu_Rate', OnGetRowValues);
             }

         }
         function OnGetRowValues(values) {
             OpenAnalysisDetails(values[0], values[1], values[2], values[3])
         }

         function Endgridanalysis() {

             cShowGrid.Focus();
         }

    </script>




    <style>
        .pdbot > tbody > tr > td {
            padding-bottom: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Sales Analysis Report </h3>
        </div>
    </div>
    <div class="form_main">
        <div class="row">
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label1" runat="Server" Text="Class : " CssClass="mylabel1"></asp:Label></label>
                <asp:HiddenField ID="hflookupClassAllFlag" runat="server" Value="" />
                <dxe:ASPxGridLookup ID="lookupClass" ClientInstanceName="clookupClass" SelectionMode="Multiple" runat="server" 
                    OnDataBinding="lookupClass_DataBinding" KeyFieldName="ProductClass_ID" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                        <dxe:GridViewDataColumn FieldName="ProductClass_Name" Visible="true" VisibleIndex="1" Caption="Class Name" Settings-AutoFilterCondition="Contains">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="ProductClass_ID" Visible="true" VisibleIndex="2" Caption="Class ID" Settings-AutoFilterCondition="Contains" Width="0">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                    </Columns>
                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                        <Templates>
                            <StatusBar>
                                <table class="OptionsTable" style="float: right">
                                    <tr>
                                        <td>
                                            <%--<div class="hide">--%>
                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" />
                                            <%--</div>--%>
                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" />
                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLookup" UseSubmitBehavior="False" />
                                        </td>
                                    </tr>
                                </table>
                            </StatusBar>
                        </Templates>
                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                        <SettingsPager Mode="ShowPager">
                        </SettingsPager>
                        <SettingsPager PageSize="20">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                        </SettingsPager>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                    </GridViewProperties>
                    
                </dxe:ASPxGridLookup>
                <span id="MandatorClass" style="display: none" class="validclass" />
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label3" runat="Server" Text="Brand : " CssClass="mylabel1"></asp:Label></label>
                <asp:HiddenField ID="hflookupBrandAllFlag" runat="server" Value="" />
                <dxe:ASPxGridLookup ID="lookupBrand" ClientInstanceName="clookupBrand" SelectionMode="Multiple" runat="server"
                    OnDataBinding="lookupBrand_DataBinding" KeyFieldName="Brand_Id" Width="100%" TextFormatString="{0}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                        <dxe:GridViewDataColumn FieldName="Brand_Name" Visible="true" VisibleIndex="1" Caption="Brand Name" Settings-AutoFilterCondition="Contains">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                        <dxe:GridViewDataColumn FieldName="Brand_Id" Visible="true" VisibleIndex="1" Caption="Brand ID" Settings-AutoFilterCondition="Contains" Width="0">
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataColumn>
                    </Columns>
                    <GridViewProperties Settings-VerticalScrollBarMode="Auto" SettingsPager-Mode="ShowAllRecords">
                        <Templates>
                            <StatusBar>
                                <table class="OptionsTable" style="float: right">
                                    <tr>
                                        <td>
                                            <%--<div class="hide">--%>
                                                <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="_selectAll" />
                                            <%--</div>--%>
                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="_unselectAll" />                                            
                                            <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="_CloseLookup" UseSubmitBehavior="False" />
                                        </td>
                                    </tr>
                                </table>
                            </StatusBar>
                        </Templates>
                        <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" />
                        <SettingsPager Mode="ShowPager">
                        </SettingsPager>
                        <SettingsPager PageSize="20">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                        </SettingsPager>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                    </GridViewProperties>
                </dxe:ASPxGridLookup>
                <span id="MandatoryActivityType" style="display: none" class="validclass" />
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="Label2" runat="Server" Text="Rate Criteria : " CssClass="mylabel1"></asp:Label>
                </label>
                <asp:DropDownList ID="ddlRateCriteria" runat="server" Width="100%">
                    <asp:ListItem Text="With GST" Value="True"></asp:ListItem>
                    <asp:ListItem Text="Without GST" Value="False"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsFrom">
                    <asp:Label ID="lblFromDate" runat="Server" Text="From Date : " CssClass="mylabel1"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxFromDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeFromDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>
            <div class="col-md-2">
                <label style="color: #b5285f; font-weight: bold;" class="clsTo">
                    <asp:Label ID="lblToDate" runat="Server" Text="To Date : " CssClass="mylabel1"
                        Width="92px"></asp:Label>
                </label>
                <dxe:ASPxDateEdit ID="ASPxToDate" runat="server" EditFormat="custom" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"
                    UseMaskBehavior="True" Width="100%" ClientInstanceName="cxdeToDate">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>

                </dxe:ASPxDateEdit>
            </div>
             <div class="col-md-2">
                <label style="margin-bottom: 0">&nbsp</label>
                <div class="">
                    <button id="btnShow" class="btn btn-primary" type="button" onclick="btn_ShowRecordsClick(this);">Show</button>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="drdExport_SelectedIndexChanged"
                        AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>

        <div class="clearfix" onkeypress="OnWaitingGridKeyPress(event)">
            <dxe:ASPxGridView runat="server" ID="ShowGrid" ClientInstanceName="cShowGrid" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False" KeyFieldName="Sl."
                OnCustomCallback="ShowGrid_CustomCallback" OnDataBinding="ShowGrid_DataBinding" OnSummaryDisplayText="ShowGrid_SummaryDisplayText" KeyboardSupport="true" ClientSideEvents-EndCallback="Endgridanalysis">
                <Columns>
                    <dxe:GridViewDataTextColumn FieldName="Sl." Caption="Sl." Width="4%" VisibleIndex="0" CellStyle-HorizontalAlign="Left">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Category" Caption="Category" Width="12%" VisibleIndex="1">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Class" Caption="Class" Width="12%" VisibleIndex="2">
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn FieldName="Particulars" Caption="Particulars" Width="24%" VisibleIndex="2">
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="sProducts_ID" Caption="Net Sale" Width="8%" Visible="false">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="Pu_Rate" Caption="Net Sale" Width="8%" Visible="false">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn FieldName="Net Sale" Caption="Net Sale" Width="8%" VisibleIndex="4">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" Width="8%" VisibleIndex="5">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Sale Value" Caption="Sale Value" Width="8%" VisibleIndex="6">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Pu_Rate" Caption="Pur.Rate" Width="9%" VisibleIndex="7">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Pu.Value" Caption="Pur.Value" Width="8%" VisibleIndex="8">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Dif Value" Caption="Diff. Value" Width="8%" VisibleIndex="9">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="Diff %" Caption="Diff %" Width="8%" VisibleIndex="10" CellStyle-HorizontalAlign="Right">
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                <SettingsEditing Mode="EditForm" />
                <SettingsContextMenu Enabled="true" />
                <SettingsBehavior AutoExpandAllGroups="true" />
                <Settings   ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" /> 
                <SettingsPager PageSize="10">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </SettingsPager>
                <TotalSummary>
                    <dxe:ASPxSummaryItem FieldName="Net Sale" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Rate" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Sale Value" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Pu.Rate" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Pu.Value" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Dif Value" SummaryType="Sum" />
                    <dxe:ASPxSummaryItem FieldName="Diff %" SummaryType="Sum" />
                </TotalSummary>
            </dxe:ASPxGridView>
        </div>
    </div>

    <dxe:ASPxPopupControl ID="popup" runat="server" ClientInstanceName="cpopup"
        Width="1000px" Height="600px" ScrollBars="Vertical" HeaderText="Sales Analysis Report Details" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents Closing="function(s, e) {
	closePopup(s, e);}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="col-md-12">
                    <div class="row clearfix">
                        <table class="pdbot" style="margin: 4px 0 16px 10px; float: left;">
                            <tr>
                                <td>
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="Label5" runat="Server" Text="Product Code : " CssClass="mylabel1"></asp:Label>
                                    </label>
                                </td>
                                <td>
                                    <dxe:ASPxTextBox ID="txtProductCode" ClientInstanceName="ctxtProductCode" runat="server" ReadOnly="true" Width="600px"></dxe:ASPxTextBox>
                                </td>

                            </tr>
                            <tr>
                                <td>
                                    <label style="color: #b5285f; font-weight: bold; margin-top: 5px;" class="clsTo">
                                        <asp:Label ID="Label4" runat="Server" Text="Product Name : " CssClass="mylabel1"></asp:Label>
                                    </label>
                                </td>
                                <td>
                                    <dxe:ASPxTextBox ID="txtProductName" ClientInstanceName="ctxtProductName" runat="server" ReadOnly="true" Width="600px"></dxe:ASPxTextBox>
                                </td>

                            </tr>
                        </table>
                        <div class="pull-right" style="padding-top: 26px;">
                            <td style="float: right">Press Esc to Close</td>
                            <asp:DropDownList ID="ddldetails" runat="server" CssClass="btn btn-sm btn-primary" AutoPostBack="true" OnSelectedIndexChanged="ddldetails_SelectedIndexChanged">
                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                <asp:ListItem Value="4">CSV</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class=" clearfix">
                    <dxe:ASPxGridView runat="server" ID="ShowGridDetails" ClientInstanceName="cShowGridDetails" Width="100%" EnableRowsCache="false" AutoGenerateColumns="False"
                        OnCustomCallback="ShowGridDetails_CustomCallback" OnDataBinding="ShowGridDetails_DataBinding" OnSummaryDisplayText="ShowGridDetails_SummaryDisplayText">
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="Branch" Caption="Branch" Width="19%" VisibleIndex="0">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Sale" Caption="Sale" Width="9%" VisibleIndex="1">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Return" Caption="Return" Width="9%" VisibleIndex="2">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Net Sale" Caption="Net Sale" Width="9%" VisibleIndex="3">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Rate" Caption="Rate" Width="9%" VisibleIndex="4">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Sale Value" Caption="Sale Value" Width="9%" VisibleIndex="5">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Pu.Rate" Caption="Pur.Rate" Width="9%" VisibleIndex="6">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Pu.Value" Caption="Pur.Value" Width="9%" VisibleIndex="7">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Dif Value" Caption="Diff. Value" Width="9%" VisibleIndex="8">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Diff %" Caption="Diff. %" Width="9%" VisibleIndex="9" CellStyle-HorizontalAlign="Right">
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsBehavior ConfirmDelete="true" EnableCustomizationWindow="true" EnableRowHotTrack="true" ColumnResizeMode="Control" />
                        <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />
                        <SettingsEditing Mode="EditForm" />
                        <SettingsContextMenu Enabled="true" />
                        <SettingsBehavior AutoExpandAllGroups="true" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsSearchPanel Visible="false" />
                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>



                        <TotalSummary>
                            <dxe:ASPxSummaryItem FieldName="Sale" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Return" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Net Sale" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Rate" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Sale Value" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Pu.Rate" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Pu.Value" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Dif Value" SummaryType="Sum" />
                            <dxe:ASPxSummaryItem FieldName="Diff %" SummaryType="Sum" />
                        </TotalSummary>
                    </dxe:ASPxGridView>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="popupHide" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField ID="hdnCategoryID" runat="server" />
    <asp:HiddenField ID="hdnClassID" runat="server" />
    <asp:HiddenField ID="hdnProductID" runat="server" />
    <asp:HiddenField ID="hdnRate" runat="server" />
    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <dxe:ASPxGridViewExporter ID="exporterDetails" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    
     
 
</asp:Content>
