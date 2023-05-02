<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                11-04-2023        2.0.37           Pallab              25982: Sale Rate Lock module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CustSaleRateLock.aspx.cs" Inherits="ERP.OMS.Management.Activities.CustSaleRateLock" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../Activities/JS/SearchPopup.js"></script>
    <script src="JS/CustSaleRateLock.js"></script>


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
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <label id="lblheading">Sale Rate Lock</label>
            </h3>
        </div>
        <div id="divcross" runat="server" class="crossBtn" style="display: none; margin-left: 50px;"><a href="#" onclick="cancel()"><i class="fa fa-times"></i></a></div>
    </div>
        <div class="form_main">
        <div id="TblSearch" class="rgth  full">
            <div class="clearfix mb-10">
                <div style="padding-right: 5px;">
                    <span id="divAddButton">
                        <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span><u>A</u>dd New</span> </a>
                    </span>
                </div>
            </div>

        </div>
        <div class="clear"></div>
        <div id="view" class="relative">
            <dxe:ASPxGridView ID="GridSaleRate" runat="server" AutoGenerateColumns="False" SettingsBehavior-AllowSort="true"
                ClientInstanceName="cGridSaleRate" KeyFieldName="SaleRateLockID" Width="100%" Settings-HorizontalScrollBarMode="Auto"
                DataSourceID="EntityServerModeDataSource"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"    OnCustomCallback="GridSaleRate_CustomCallback">
               
                <SettingsSearchPanel Visible="True" Delay="5000" />
                <ClientSideEvents CustomButtonClick="SaleRateCustomButtonClick" RowClick="gridRowclick" />
                <SettingsBehavior ConfirmDelete="True" />
                <Columns>
                    <dxe:GridViewDataTextColumn Visible="False" FieldName="SaleRateLockID" SortOrder="Descending">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="CustName" Caption="Customer Name" Width="150px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="Products_Name" Caption="Product Name" Width="150px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="MinSalePrice" Caption="Min Sale Price">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="Disc" Caption="Discount">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DiscSalesPrice" Caption="Amount">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="ValidFrom" Caption="Valid From Date" Width="200px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ValidUpto" Caption="Valid upto Date" Width="200px">
                        <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewCommandColumn VisibleIndex="8" Width="130px" ButtonType="Image" Caption="Actions" HeaderStyle-HorizontalAlign="Center">
                        <CustomButtons>
                            <dxe:GridViewCommandColumnCustomButton ID="CustomBtnEdit" meta:resourcekey="GridViewCommandColumnCustomButtonResource1" Image-ToolTip="Edit" Styles-Style-CssClass="pad">
                                <Image Url="/assests/images/Edit.png"></Image>
                            </dxe:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                        <CustomButtons>
                            <dxe:GridViewCommandColumnCustomButton ID="CustomBtnDelete" meta:resourcekey="GridViewCommandColumnCustomButtonResource1" Image-ToolTip="Delete" Styles-Style-CssClass="pad">
                                <Image Url="/assests/images/Delete.png" ToolTip="Delete"></Image>
                            </dxe:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                    </dxe:GridViewCommandColumn>

                </Columns>
                 <SettingsContextMenu Enabled="true"></SettingsContextMenu>
               
                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" HorizontalScrollBarMode="Visible" ShowFooter="true" />
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </SettingsPager>
            </dxe:ASPxGridView>
            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="v_SaleRateLockList" />
        </div>
        <div id="entry" style="display: none">
            <div style="background: #f5f4f3; padding: 17px 0; margin-bottom: 0px; border-radius: 4px; border: 1px solid #ccc;" class="clearfix">
                <div class="col-md-2">
                    <label>Customer</label>
                    <dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" TabIndex="1">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){CustomerKeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                    <span id="MandatorysCustName" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -11px; top: 24px;"
                        title="Mandatory"></span>
                </div>
                <div class="col-md-2">
                    <label>Product</label>
                    <dxe:ASPxButtonEdit ID="txtProductName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProductName" TabIndex="2">
                        <Buttons>
                            <dxe:EditButton>
                            </dxe:EditButton>
                        </Buttons>
                        <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){ProductKeyDown(s,e);}" />
                    </dxe:ASPxButtonEdit>
                    <span id="MandatorysProductName" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -11px; top: 24px;"
                        title="Mandatory"></span>
                </div>
                <div class="col-md-2">
                    <label>Min Sale Price</label>
                    <dxe:ASPxTextBox ID="txtMinSalePrice" ClientInstanceName="ctxtMinSalePrice" runat="server" ReadOnly="true">
                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                    </dxe:ASPxTextBox>
                </div>
                <div class="col-md-2">
                    <label>Discount (%)</label>
                    <dxe:ASPxSpinEdit ID="txtDiscount" ClientInstanceName="ctxtDiscount"  runat="server"  ShowOutOfRangeWarning="false" SpinButtons-ClientVisible="false" 
                        TabIndex="3" MaxValue="100" AllowMouseWheel="false" EditFormatString="0.00" DisplayFormatString="0.00" DecimalPlaces="2" NumberType="Float" MinValue="0.00">                        
                         
                       <%-- <MaskSettings Mask="&lt;0..999&gt;.&lt;00..99&gt;" AllowMouseWheel="false"  />--%>
                        <ValidationSettings Display="None"></ValidationSettings>
                        <ClientSideEvents LostFocus="AmountCalculate" />
                    </dxe:ASPxSpinEdit>
                    <span id="MandatorysDiscount" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -13px; top: 24px;"
                        title="Mandatory"></span>
                </div>
                <div class="col-md-2">
                    <label>Amount</label>
                    <dxe:ASPxTextBox ID="txtAmount" ClientInstanceName="ctxtAmount" runat="server" TabIndex="4">
                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" AllowMouseWheel="false" />
                        <ClientSideEvents LostFocus="PercentageCalculate" />
                    </dxe:ASPxTextBox>
                    <span id="MandatorysAmount" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -11px; top: 24px;"
                        title="Mandatory"></span>
                </div>
                <div class="clearfix"></div>
                <div class="col-md-2 lblmTop8">
                    <label>From Date</label>
                    <dxe:ASPxDateEdit ID="Fromdt" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy HH:mm" ClientInstanceName="cFormDate"
                        Width="100%" TabIndex="5" DisplayFormatString="dd-MM-yyyy HH:mm" UseMaskBehavior="True">
                        <TimeSectionProperties Visible="true"></TimeSectionProperties>
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <ClientSideEvents GotFocus="function(s,e){cFormDate.ShowDropDown();}" />
                    </dxe:ASPxDateEdit>
                    <span id="MandatorysFromdt" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -11px; top: 24px;"
                        title="Mandatory"></span>
                </div>
                <div class="col-md-2 lblmTop8">
                    <label>To Date</label>
                    <dxe:ASPxDateEdit ID="ToDate" runat="server" EditFormat="DateTime" EditFormatString="dd-MM-yyyy HH:mm" ClientInstanceName="cToDate"
                        Width="100%" TabIndex="6" DisplayFormatString="dd-MM-yyyy HH:mm" UseMaskBehavior="True">
                        <TimeSectionProperties Visible="true"></TimeSectionProperties>
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <ClientSideEvents GotFocus="function(s,e){cToDate.ShowDropDown();}" />
                    </dxe:ASPxDateEdit>
                    <span id="MandatorysTodt" class="fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none; right: -11px; top: 24px;"
                        title="Mandatory"></span>
                </div>
            </div>
            <div class="clearfix"></div>
            <div style="padding: 15px 10px 10px 0px;">
                <dxe:ASPxButton ID="btnSaveRecords" TabIndex="7" ClientInstanceName="cbtnSaveRecords" runat="server" AutoPostBack="False" Text="S&#818;ave" CssClass="btn btn-success" UseSubmitBehavior="False">
                    <ClientSideEvents Click="function(s, e) {SaveButtonClick('Insert');}" />
                </dxe:ASPxButton>
                <dxe:ASPxButton ID="btncancel" TabIndex="8" ClientInstanceName="cbtncancel" runat="server" AutoPostBack="False" Text="C&#818;ancel" CssClass="btn btn-primary" UseSubmitBehavior="False">
                    <ClientSideEvents Click="function(s, e) {cancel();}" />
                </dxe:ASPxButton>

            </div>
        </div>
    </div>
    </div>


    <asp:HiddenField ID="hdnCustId" runat="server" />
    <asp:HiddenField ID="hdnProdId" runat="server" />
    <asp:HiddenField ID="HiddenSaleRateLockID" runat="server" />
    <asp:HiddenField ID="Hiddenvalidfrom" runat="server" />
    <asp:HiddenField ID="Hiddenvalidupto" runat="server" />
    <!--Customer Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name,Unique Id and Phone No." />

                    <div id="CustomerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Customer Name</th>
                                <th>Unique Id</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
    <!--Product Modal -->
    <div class="modal fade" id="ProductModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Name or Description" />

                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Name</th>
                                <th>Product Description</th>
                                <th>Min Sale Price</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>



</asp:Content>
