<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="E-waybill.aspx.cs" Inherits="ERP.OMS.Management.E_waybill" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="csrf-token" content="XYZ123">
    <script src="Activities/JS/ewaybill.js?v0.0.5"></script>
    <style>
        .tabs-left, .tabs-right {
            border-bottom: none;
            padding-top: 2px;
        }

        .tabs-left {
            border-right: 1px solid #ddd;
        }

        .tabs-right {
            border-left: 1px solid #ddd;
        }

            .tabs-left > li, .tabs-right > li {
                float: none;
                margin-bottom: 2px;
            }

        .tabs-left > li {
            margin-right: -1px;
        }

        .tabs-right > li {
            margin-left: -1px;
        }

        .tabs-left > li.active > a,
        .tabs-left > li.active > a:hover,
        .tabs-left > li.active > a:focus {
            border-bottom-color: #ddd;
            border-right-color: transparent;
        }

        .tabs-right > li.active > a,
        .tabs-right > li.active > a:hover,
        .tabs-right > li.active > a:focus {
            border-bottom: 1px solid #ddd;
            border-left-color: transparent;
        }

        .tabs-left > li > a {
            border-radius: 21px 0 0 21px;
            margin-right: 0;
            display: block !important;
        }

        .tabs-right > li > a {
            border-radius: 0 4px 4px 0;
            margin-right: 0;
        }

        .vertical-text {
            margin-top: 50px;
            border: none;
            position: relative;
        }

            .vertical-text > li {
                height: 20px;
                width: 120px;
                margin-bottom: 100px;
            }

                .vertical-text > li > a {
                    border-bottom: 1px solid #ddd;
                    border-right-color: transparent;
                    text-align: center;
                    border-radius: 4px 4px 0px 0px;
                }

                .vertical-text > li.active > a,
                .vertical-text > li.active > a:hover,
                .vertical-text > li.active > a:focus {
                    border-bottom-color: transparent;
                    border-right-color: #ddd;
                    border-left-color: #ddd;
                }

            .vertical-text.tabs-left {
                left: -50px;
            }

            .vertical-text.tabs-right {
                right: -50px;
            }

                .vertical-text.tabs-right > li {
                    -webkit-transform: rotate(90deg);
                    -moz-transform: rotate(90deg);
                    -ms-transform: rotate(90deg);
                    -o-transform: rotate(90deg);
                    transform: rotate(90deg);
                }

            .vertical-text.tabs-left > li {
                -webkit-transform: rotate(-90deg);
                -moz-transform: rotate(-90deg);
                -ms-transform: rotate(-90deg);
                -o-transform: rotate(-90deg);
                transform: rotate(-90deg);
            }

        /*.tabs-left > li > a, .fontPp {
            font-family: Poppins !important;
        }*/

        .tabs-left > li > a {
            background-color: #f9f9f9 !important;
            margin-bottom: 10px;
        }

            .tabs-left > li > a:hover {
                background-color: #efefff !important;
                border-color: #e1e5ff;
            }

        .tabs-left > li.active > a, .tabs-left > li.active > a:hover, .tabs-left > li.active > a:focus {
            background: #432ADB !important;
            color: #fff !important;
            font-size: 14px !important;
            border-color: #432adb;
        }

        .no-gutters {
            padding: 0;
        }

        .ttCont {
            background: #ffffff;
            padding: 15px;
            border-radius: 18px;
         
            min-height: 300px;
        }

        .holderBox {
            background: #ff5808;
            border-radius: 10px;
            padding: 1px 15px 8px 15px;
            color: #ffffff;
        }

            .holderBox.c1 {
                background: #0f78fb;
            }

            .holderBox.c2 {
                background: #7208ff;
            }

            .holderBox.c3 {
                background: #fb0f87;
            }

        .bDashed-right {
            border-right: 1px dashed;
        }
        /*horizontal tab*/

        .horiTab .nav-tabs>li>a {
            padding: 7px 7px;
            height: auto;
            white-space:nowrap;
            width: auto !important;
        }
        .horiTab .nav-tabs>li>a:before {
            display:none;
        }
        .horiTab {
            margin-top: 15px;
        }

            .horiTab .nav-tabs > li > a {
                background: transparent;
                border: none;
            }

                .horiTab .nav-tabs > li > a:hover {
                    border: none;
                    background-color: transparent !important;
                }

            .horiTab .nav-tabs > li.active > a, .horiTab .nav-tabs > li.active > a:hover, .horiTab .nav-tabs > li.active > a:focus {
                border: none;
                background: none !important;
                border-bottom: 3px solid #0f78fb;
                font-size: 14px;
                font-weight: 500;
                color: #565353;
            }

        .dx-header-row {
            background: #545dc1 !important;
            color: #fff;
        }

            .dx-header-row > td {
                border-color: #545dc1 !important;
            }

        .dx-texteditor-input-container input {
            border: transparent;
            margin: 0;
        }

        .dropzone {
            background: white;
            border-radius: 5px;
            border: 2px dashed rgb(0, 135, 247);
            border-image: none;
            max-width: 500px;
            margin-left: auto;
            margin-right: auto;
        }
        .ui-widget.ui-widget-content {
            max-width: 900px !important;
        }
        

        
    </style>     
    <style>
        .Vtabs .tab-pane {
            margin-left: 0;
            border-left: none;
            padding-left: 0;
        }
        .Vtabs .ttCont>.tab-pane {
            margin-left: 15px;
            border-left: 1px solid #e8e8e8;
            padding-left: 11px;
        }
        .rightSide  {
            padding:0;
        }
        .Vtabs {
            border-top:none;
            padding-top:0;
        }
        .Vtabs li a {
            padding-left: 5px;
            padding-right: 5px;
        }
        .floatedBtnArea  {
            right: 22px;
        }
        .floatedBtnArea.insideGrid>a {
            padding: 0 !important;
            font-size:13px !important;
        }
    </style>
    <style>
        .cap {
            font-size:34px;
            color:red;
        }
        .dropify-wrapper{
            border: 2px dashed #E5E5E5;
        }
        .ppTabl {
            margin:0 auto;
            
        }
        .ppTabl>tbody>tr>td:first-child{
            text-align:right;
            padding-right:15px;
        }
        .ppTabl>tbody>tr>td {
            padding:4px 0;
            font-size:15px;
            text-align:left;
        }
        .empht {
            font-size: 18px;
            color: #d68f0d;
            margin: 6px;
        }
        .poppins {
            font-family: 'Poppins', sans-serif;
        }
        .bcShad {
            position: fixed;
            width: 100%;
            background: rgba(0,0,0,0.75);
            height: 100%;
            left: 0;
            z-index: 120;
            top: 0;
            display:none;
        }
        .popupSuc {
            position: absolute;
            z-index: 123;
            background: #fff;
            padding: 3px;
            min-width: 650px;
            left: 50%;
            top: 50%;
            transform: translate(-50%, -50%);
            display:none;
        }
        .bcShad.in , .popupSuc.in {
            display:block;
        }
        .bInfoIt{
            text-align: center;
            border-top: 1px solid #ccc;
            border-bottom: 1px solid #ccc;
            padding: 12px;
        }
        .bInfoIt p {
            margin:0;
        }
        .fontSmall>tbody>tr>td {
            font-size: 13px !important;
        }
        .cnIcon {
            display: flex;
            background: #4ec34e;
            border-radius: 50%;
            width: 80px;
            height: 80px;
            margin: 15px auto;
            justify-content: center;
            align-items: center;
            font-size: 32px;
            color: #fff;
        }
    </style>
    <style>
        .boxHover {
            -webkit-transition:all 0.2s ease-in-out;
            transition:all 0.2s ease-in-out;
        }
        .boxHover:hover {
            box-shadow: 0px 7px 5px rgba(0,0,0,0.12);
            cursor: pointer;
            -webkit-transform: translateY(-4px);
            -ms-transform: translateY(-4px);
            -moz-transform: translateY(-4px);
            transform: translateY(-4px); 
        }
        .strong{
            font-weight:bold;
        }
         .searchBoxSmall>table>tbody>tr>td>table{
            max-width:250px !important
        }
         .noStyle {
             border:none !important;
             color:blue !important
         }
.ttip {
  position: relative;
  display: inline-block;
}

.ttip .tooltiptext {
  visibility: hidden;
  width: 140px;
  background-color: #555;
  color: #fff;
  text-align: center;
  border-radius: 6px;
  padding: 5px;
  position: absolute;
  z-index: 1;
  bottom: 150%;
  left: 50%;
  margin-left: -75px;
  opacity: 0;
  transition: opacity 0.3s;
}

.ttip .tooltiptext::after {
  content: "";
  position: absolute;
  top: 100%;
  left: 50%;
  margin-left: -5px;
  border-width: 5px;
  border-style: solid;
  border-color: #555 transparent transparent transparent;
}

.ttip:hover .tooltiptext {
  visibility: visible;
  opacity: 1;
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-title clearfix">
        <h3 class="pull-left">
            
            <label>
                E-Way Bill  (Without IRN)
            </label>
        </h3>
    </div>
    <div class="form_main clearfix">
        <div class="clearfix Vtabs">
            <div class="row">   
            <div class="col-sm-2 col-md-2 col-lg-1">
                <!-- required for floating -->
                <!-- Nav tabs -->
                <ul class="nav nav-pills nav-stacked flex-column" style="margin-top:20px">
                    <li class="active"><a href="#home" data-toggle="tab">Invoice</a></li>
                    <li onclick="tsiBoxRefresh()"><a href="#profile" data-toggle="tab">Transit Sales</a></li>
                    <li onclick="SRBoxRefresh()"><a href="#messages" data-toggle="tab">Credit Note</a></li>
                    <%--<li><a href="#settings" data-toggle="tab">Debit Note</a></li>--%>
                </ul>
            </div>
            <div class="col-sm-10 col-md-10 col-lg-11">
                <!-- Tab panes -->
                <div class="tab-content ttCont">
                    <div class="tab-pane active" id="home">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="holderBox fontPp c3">
                                    <h4>Total Invoice</h4>
                                    <div class="row">
                                        <div class="col-sm-4 bDashed-right">
                                            <div>Count</div>
                                            <div id="SITOTAL_COUNTS" class="strong">0</div>
                                        </div>
                                        <div class="col-sm-8">
                                            <div>Amount</div>
                                            <div id="SITOTAL_AMOUNT" class="strong">0</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="holderBox fontPp c1 boxHover clickTrig" data-triggreid="#pills-home-tab">
                                    <h4>e - Way Bill Generated</h4>
                                    <div class="row">
                                        <div class="col-sm-4 bDashed-right">
                                            <div>Count</div>
                                            <div id="SITOTAL_GENERATED" class="strong">0</div>
                                        </div>
                                        <div class="col-sm-8">
                                            <div>Amount</div>
                                            <div id="SITOTAL_GENERATED_AMOUNT" class="strong">0</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="holderBox fontPp c2 boxHover clickTrig" data-triggreid="#pills-profile-tab">
                                    <h4>e - Way Bill Pending</h4>
                                    <div class="row">
                                        <div class="col-sm-4 bDashed-right">
                                            <div>Count</div>
                                            <div id="SITOTAL_PENDING" class="strong">0</div>
                                        </div>
                                        <div class="col-sm-8">
                                            <div>Amount</div>
                                            <div id="SITOTAL_PENDING_AMOUNT" class="strong">0</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="holderBox fontPp boxHover clickTrig" data-triggreid="#pills-cancel-tab">
                                    <h4>e - Way Bill Cancelled</h4>
                                    <div class="row">
                                        <div class="col-sm-4 bDashed-right">
                                            <div>Count</div>
                                            <div id="SITOTAL_CANCEL" class="strong">0</div>
                                        </div>
                                        <div class="col-sm-8">
                                            <div>Amount</div>
                                            <div id="SITOTAL_CANCEL_AMOUNT" class="strong">0</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 horiTab">
                                <ul class="nav nav-tabs mb-3" id="pills-tab" role="tablist">
                                  <li class="nav-item active">
                                    <a class="nav-link " id="pills-home-tab" data-toggle="pill" href="#pills-home" role="tab" aria-controls="pills-home" aria-selected="true">e - Way Bill Generated</a>
                                  </li>
                                  <li class="nav-item">
                                    <a class="nav-link" id="pills-profile-tab" data-toggle="pill" href="#pills-profile" role="tab" aria-controls="pills-profile" aria-selected="false">e - Way Bill Pending</a>
                                  </li>
                                 
                                </ul>
                                <div class="tab-content" id="pills-tabContent">
                                     <div class="tab-pane active" id="pills-home" role="tabpanel" aria-labelledby="pills-home-tab">
                                            <div>
                                                                          <div class="row">
                                                                                <div class="col-md-3">
                                                                                    <label>From Date</label>
                                                                                    <div>
                                                                                        <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-3">
                                                                                    <label>To Date</label>
                                                                                <div>
                                                                                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                        <ButtonStyle Width="13px">
                                                                                        </ButtonStyle>
                                                                                    </dxe:ASPxDateEdit>

                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3">
                                                                                    <label>Unit</label>
                                                                                    <div>
                                                                                        <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                                                                                        </dxe:ASPxComboBox>
                                                                                    </div>
                                                                                 </div>
                                                                                <div class="col-md-3" style="padding-top:22px">
                                                                                    <input type="button" value="Show" class="btn btn-success" id="SIgenBut" onclick="updateGridByDate()" />
                                                                                </div>
                                                                            </div>
                                                                      </div>
                                            <div style="margin-top:14px">
                                                <%--<button type="button" class="btn btn-dark fontPp" data-toggle="modal" onclick="CancelBulkIRN()">Cancel Bulk IRN</button>
                                                <button type="button" class="btn btn-Crimson fontPp" onclick="DownloadBulkJSONSI()">Download Bulk IRN</button>
                                              <button type="button" class="btn btn-OrangeRed fontPp hide " onclick="UploadEwaybillSI()">Upload Bulk E-Way Bill</button>--%>
 <%--                                                 <button type="button" class="btn btn-SeaGreen fontPp">Upload Bulk IRN</button>--%>
                                            </div>
                                             
                                            <div class="row">
                                                 <div class="col-md-12">
                                              
                                                <asp:HiddenField ID="hfBranchID" runat="server" />
                                                <asp:HiddenField ID="hfFromDate" runat="server" />
                                                <asp:HiddenField ID="hfIsFilter" runat="server" />
                                                <asp:HiddenField ID="hfToDate" runat="server" />
                                                <asp:HiddenField ID="HiddenField5" runat="server" />
                                                      <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"                    
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />
                                                <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                    Width="100%" ClientInstanceName="cGrdQuotation" SettingsBehavior-AllowFocusedRow="true" DataSourceID="EntityServerModeDataSource"
                                                    SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                    SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" OnCustomCallback="GrdQuotation_CustomCallback"
                                                    SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" ClientSideEvents-EndCallback="grdEndcallback" Styles-SearchPanel-CssClass="searchBoxSmall">
                                                    <SettingsSearchPanel Visible="True" Delay="5000" />
                                                    <Columns>
                                                       
                                                            <dxe:GridViewDataTextColumn Caption="Sl No." FieldName="SlNo" Width="50" Visible="false" SortOrder="Descending"
                                                                VisibleIndex="1" FixedStyle="Left">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Document No." FieldName="InvoiceNo" Width="200"
                                                                VisibleIndex="2" FixedStyle="Left">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Invoice_Date"
                                                                VisibleIndex="3">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="CustomerName" Caption="Customer" VisibleIndex="4" Width="300">
                                                                <DataItemTemplate>
                                                                    <a href="javascript:void(0);" onclick="CustomerClick(this,'<%# Eval("Customer_Id") %>')">
                                                                        <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("CustomerName")%>'
                                                                            ToolTip="Customer Outstanding">
                                                                        </dxe:ASPxLabel>
                                                                    </a>

                                                                </DataItemTemplate>
                                                                <EditFormSettings Visible="False" />
                                                                <CellStyle Wrap="False" CssClass="text-center">
                                                                </CellStyle>
                                                                <Settings AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <HeaderStyle Wrap="False" CssClass="text-center" />

                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="NetAmount"
                                                                VisibleIndex="5">
                                                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Type" FieldName="Pos_EntryType" Width="70"
                                                                VisibleIndex="6">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>



                                                            <dxe:GridViewDataTextColumn Caption="Delv. Type" FieldName="Pos_DeliveryType"
                                                                VisibleIndex="7" Width="80">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Delv. Date" FieldName="Pos_DeliveryDate"
                                                                VisibleIndex="8">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Delv. Status" FieldName="DelvStatus"
                                                                VisibleIndex="9">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Challan No" FieldName="ChallanNo"
                                                                VisibleIndex="10" Width="180">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="ChallanDate"
                                                                VisibleIndex="11">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="SRN?" FieldName="isSRN"
                                                                VisibleIndex="12" Width="50">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="SRN No." FieldName="SRNno"
                                                                VisibleIndex="13">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="SRN Date" FieldName="SrnDate"
                                                                VisibleIndex="14">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="CD Date" FieldName="cdDate"
                                                                VisibleIndex="15">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Assigned Unit" FieldName="pos_assignBranch"
                                                                VisibleIndex="16" Width="350">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Sales Person" FieldName="salesman" Width="150"
                                                                VisibleIndex="17">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>


                                                            <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                                                                VisibleIndex="18">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>




                                                       <dxe:GridViewDataTextColumn Caption="IRN" FieldName="Irn"
                                                            VisibleIndex="19">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>
                                                       <dxe:GridViewDataTextColumn Caption="Ack. Date" FieldName="AckDt" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy hh:mm:ss"
                                                            VisibleIndex="20">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                       <dxe:GridViewDataTextColumn Caption="Ack. No." FieldName="AckNo"
                                                            VisibleIndex="21">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>


                                                        <dxe:GridViewDataTextColumn Caption="E-Way Bill Number" FieldName="EWayBillNumber" Width="250"
                                                            VisibleIndex="22">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>
                                                       <dxe:GridViewDataTextColumn Caption="E-Way Bill Date" FieldName="EWayBillDate" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy hh:mm:ss"
                                                            VisibleIndex="23">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="22" Width="0">
                                                            <DataItemTemplate>
                                                                <div class='floatedBtnArea'>
                                                                </div>
                                                            </DataItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />

                                                        </dxe:GridViewDataTextColumn>



                                                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="23" Width="0">
                                                            <DataItemTemplate>
                                                                <div class='floatedBtnArea'>

                                                                    <a href="javascript:void(0);" onclick="DoownLoadEwaybill('<%# Eval("EWayBillNumber") %>')" id="a_editInvoice" class="" title="">
                                                                        <span class='ico editColor'><i class="fas fa-cloud-download-alt"></i>' aria-hidden='true'></i></span><span class='hidden-xs'>Download</span></a>
                                                                  
                                                                </div>

                                                            </DataItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />

                                                        </dxe:GridViewDataTextColumn>

                                                    </Columns>
                                                    <SettingsPager PageSize="10">
                                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                                    </SettingsPager>
                                                    <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                    <SettingsLoadingPanel Text="Please Wait..." />
                                                    <ClientSideEvents RowClick="gridcrmCampaignclick" />
                                                    <TotalSummary>
                                                        <dxe:ASPxSummaryItem FieldName="NetAmount" SummaryType="Sum" />
                                                    </TotalSummary>


                                                </dxe:ASPxGridView>
                                            </div>
                                            </div>
                                           
                                     </div>
                                     <div class="tab-pane" id="pills-profile" role="tabpanel" aria-labelledby="pills-profile-tab">
                                         <div>
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <label>From Date</label>
                                                    <div>
                                                        <dxe:ASPxDateEdit ID="ASPxDateEdit1" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDatePendinSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <label>To Date</label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="ASPxDateEdit2" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDatePendinSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>

                                                </div>
                                                    </div>
                                                <div class="col-md-3">
                                                    <label>Unit</label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="brPendinSI" runat="server" ClientInstanceName="ccmbBranchfilterPendinSI" Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </div>
                                                    </div>
                                                <div class="col-md-3" style="padding-top:22px">
                                                    <input type="button" value="Show" class="btn btn-success" id="SIpendindBut" onclick="updateGridByDatePendinSI()" />
                                                </div>
                                            </div>
                                        </div>
                                        <div style="margin-top:14px">
                                            
                                        <%--    <button type="button" class="btn btn-Crimson fontPp"  onclick="DownloadBulkJSONPendingSI()">Donload Bulk JSON</button>
                                            <button type="button" class="btn btn-OrangeRed fontPp" onclick="UploadBulkIRNSI()">Upload Bulk IRN</button>
                                            --%>
                                        </div>
                                          <div class="row mTop5">
                                                <div class="col-md-12">
                                                    <dx:LinqServerModeDataSource ID="LinqServerModeDataSourcePendingewaybill" runat="server" OnSelecting="LinqServerModeDataSourcePendingewaybill_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />
                                                   
                                                    <dxe:ASPxGridView ID="GrdQuotationPendinSI" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                        Width="100%" ClientInstanceName="cGrdQuotationPendinSI" SettingsBehavior-AllowFocusedRow="true" DataSourceID="LinqServerModeDataSourcePendingewaybill"
                                                        SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                        SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" OnCustomCallback="GrdQuotationPendinSI_CustomCallback"
                                                        SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" ClientSideEvents-EndCallback="grdEndcallbackPendingSI" Styles-SearchPanel-CssClass="searchBoxSmall">
                                                        <SettingsSearchPanel Visible="True" Delay="5000" />
                                                        <Columns>
                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" />
                                                            <dxe:GridViewDataTextColumn Caption="Sl No." FieldName="SlNo" Width="50" Visible="false" SortOrder="Descending"
                                                                VisibleIndex="1" FixedStyle="Left">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Document No." FieldName="InvoiceNo" Width="200"
                                                                VisibleIndex="2" FixedStyle="Left">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Invoice_Date"
                                                                VisibleIndex="3">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="CustomerName" Caption="Customer" VisibleIndex="4" Width="300">
                                                                <DataItemTemplate>
                                                                    <a href="javascript:void(0);" onclick="CustomerClick(this,'<%# Eval("Customer_Id") %>')">
                                                                        <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("CustomerName")%>'
                                                                            ToolTip="Customer Outstanding">
                                                                        </dxe:ASPxLabel>
                                                                    </a>


                                                                </DataItemTemplate>
                                                                <EditFormSettings Visible="False" />
                                                                <CellStyle Wrap="False" CssClass="text-center">
                                                                </CellStyle>
                                                                <Settings AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <HeaderStyle Wrap="False" CssClass="text-center" />

                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="NetAmount"
                                                                VisibleIndex="5">
                                                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Type" FieldName="Pos_EntryType" Width="70"
                                                                VisibleIndex="6">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>



                                                            <dxe:GridViewDataTextColumn Caption="Delv. Type" FieldName="Pos_DeliveryType"
                                                                VisibleIndex="7" Width="80">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Delv. Date" FieldName="Pos_DeliveryDate"
                                                                VisibleIndex="8">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Delv. Status" FieldName="DelvStatus"
                                                                VisibleIndex="9">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Challan No" FieldName="ChallanNo"
                                                                VisibleIndex="10" Width="180">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="ChallanDate"
                                                                VisibleIndex="11">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="SRN?" FieldName="isSRN"
                                                                VisibleIndex="12" Width="50">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="SRN No." FieldName="SRNno"
                                                                VisibleIndex="13">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="SRN Date" FieldName="SrnDate"
                                                                VisibleIndex="14">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="CD Date" FieldName="cdDate"
                                                                VisibleIndex="15">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Assigned Unit" FieldName="pos_assignBranch"
                                                                VisibleIndex="16" Width="350">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Sales Person" FieldName="salesman" Width="150"
                                                                VisibleIndex="17">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>


                                                            <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                                                                VisibleIndex="18">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>




                                                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                                                                <DataItemTemplate>
                                                                    <div class='floatedBtnArea'>
                                                                    </div>
                                                                </DataItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />

                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                                                                <DataItemTemplate>
                                                                    <div class='floatedBtnArea'>

                                                                        <a href="javascript:void(0);" onclick="UploadEwayBill('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-cloud-upload-alt' aria-hidden='true'></i></span><span class='hidden-xs'>Upload e Way Bill</span></a>
                                                                       <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SI', 'EWAY_GEN','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRN" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info</span></a>
                                                                    </div>

                                                                </DataItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />

                                                            </dxe:GridViewDataTextColumn>

                                                        </Columns>
                                                        <SettingsPager PageSize="10">
                                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                                        </SettingsPager>
                                                        <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                        <SettingsLoadingPanel Text="Please Wait..." />
                                                        <ClientSideEvents RowClick="gridcrmCampaignclick" />
                                                        <TotalSummary>
                                                            <dxe:ASPxSummaryItem FieldName="NetAmount" SummaryType="Sum" />
                                                        </TotalSummary>


                                                    </dxe:ASPxGridView>
                                                </div>
                                            </div>
                                     </div> 
                                </div>
                        </div>
                    </div>
                </div>

                    <div class="tab-pane" id="profile"> 
                         <div class="row">
                            <div class="col-md-3">
                                <div class="holderBox fontPp c3">
                                    <h4>Transit Total Invoice</h4>
                                    <div class="row">
                                        <div class="col-sm-4 bDashed-right">
                                            <div>Count</div>
                                            <div id="TSTOTAL_COUNTS" class="strong">0</div>
                                        </div>
                                        <div class="col-sm-8">
                                            <div>Amount</div>
                                            <div id="TSTOTAL_AMOUNT" class="strong">0</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="holderBox fontPp c1 boxHover clickTrig" data-triggreid="#pills-homeTSI-tab">
                                    <h4>e - Way Bill Generated</h4>
                                    <div class="row">
                                        <div class="col-sm-4 bDashed-right">
                                            <div>Count</div>
                                            <div id="TSTOTAL_GENERATED" class="strong">0</div>
                                        </div>
                                        <div class="col-sm-8">
                                            <div>Amount</div>
                                            <div id="TSTOTAL_GENERATED_AMOUNT" class="strong">0</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="holderBox fontPp c2 boxHover clickTrig" data-triggreid="#pills-profileTSI-tab">
                                    <h4>e - Way Bill Pending</h4>
                                    <div class="row">
                                        <div class="col-sm-4 bDashed-right">
                                            <div>Count</div>
                                            <div id="TSTOTAL_PENDING" class="strong">0</div>
                                        </div>
                                        <div class="col-sm-8">
                                            <div>Amount</div>
                                            <div id="TSTOTAL_PENDING_AMOUNT" class="strong">0</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="holderBox fontPp boxHover clickTrig" data-triggreid="#pills-CancelTSI-tab">
                                    <h4>e - Way Bill Cancelled</h4>
                                    <div class="row">
                                        <div class="col-sm-4 bDashed-right">
                                            <div>Count</div>
                                            <div id="TSTOTAL_CANCEL" class="strong">0</div>
                                        </div>
                                        <div class="col-sm-8">
                                            <div>Amount</div>
                                            <div id="TSTOTAL_CANCEL_AMOUNT" class="strong">0</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 horiTab">
                                <ul class="nav nav-tabs mb-3" id="pills-tabTSI" role="tablist">
                                  <li class="nav-item active">
                                    <a class="nav-link " id="pills-homeTSI-tab" data-toggle="pill" href="#pills-homeTSI" role="tab" aria-controls="pills-home" aria-selected="true">e - Way Bill Generated</a>
                                  </li>
                                  <li class="nav-item">
                                    <a class="nav-link" id="pills-profileTSI-tab" data-toggle="pill" href="#pills-profileTSI" role="tab" aria-controls="pills-profile" aria-selected="false">e - Way Bill Pending</a>
                                  </li>
                                </ul>
                                <div class="tab-content" id="pills-tabContentTSI">
                                        <div class="tab-pane active" id="pills-homeTSI" role="tabpanel" aria-labelledby="pills-homeTSI-tab">
                                            <div>
                                                                          <div class="row">
                                                                                <div class="col-md-3">
                                                                                    <label>From Date</label>
                                                                                    <div>
                                                                                        <dxe:ASPxDateEdit ID="ASPxDateEdit3" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDateTSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-3">
                                                                                    <label>To Date</label>
                                                                                <div>
                                                                                    <dxe:ASPxDateEdit ID="ASPxDateEdit4" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDateTSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                        <ButtonStyle Width="13px">
                                                                                        </ButtonStyle>
                                                                                    </dxe:ASPxDateEdit>

                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3">
                                                                                    <label>Unit</label>
                                                                                <div>
                                                                                    <dxe:ASPxComboBox ID="brTSI" runat="server" ClientInstanceName="ccmbBranchfilterTSI" Width="100%">
                                                                                    </dxe:ASPxComboBox>
                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3" style="padding-top:22px">
                                                                                    <input type="button" value="Show" class="btn btn-success" id="TSIgenBut" onclick="updateGridByDateTSI()" />
                                                                                </div>
                                                                            </div>
                                                                      </div>
                                            <div style="margin-top:14px">
                                              <%--  <button type="button" class="btn btn-dark fontPp" data-toggle="modal" onclick="CancelBulkIRNTSI()">Cancel Bulk IRN</button>
                                                <button type="button" class="btn btn-Crimson fontPp" onclick="DownloadBulkJSONTSI()">Download Bulk IRN</button>
                                                <button type="button" class="btn btn-OrangeRed fontPp hide" onclick="UploadEwaybillTSI()">Upload Bulk E-Way bill</button>--%>
                 <%--                               <button type="button" class="btn btn-SeaGreen fontPp">Upload Bulk IRN</button>--%>
                                            </div>
                                             
                                            <div class="row">
                                                 <div class="col-md-12">
                                               
 
                                                <dxe:ASPxGridView ID="GrdQuotationTSI" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                    Width="100%" ClientInstanceName="cGrdQuotationTSI" SettingsBehavior-AllowFocusedRow="true" 
                                                    SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                    SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
                                                    SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" ClientSideEvents-EndCallback="grdEndcallbackTSI" Styles-SearchPanel-CssClass="searchBoxSmall">
                                                    <SettingsSearchPanel Visible="True" Delay="5000" />
                                                    <Columns>
                                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" />
                                                        <dxe:GridViewDataTextColumn Caption="Sl No." FieldName="SlNo" Width="50" Visible="false" SortOrder="Descending"
                                                            VisibleIndex="0" FixedStyle="Left">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Document No." FieldName="InvoiceNo" Width="200"
                                                            VisibleIndex="0" FixedStyle="Left">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Invoice_Date"
                                                            VisibleIndex="1">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="CustomerName" Caption="Customer" VisibleIndex="2" Width="300">
                                                            <DataItemTemplate>
                                                                <a href="javascript:void(0);" onclick="CustomerClick(this,'<%# Eval("Customer_Id") %>')">
                                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("CustomerName")%>'
                                                                        ToolTip="Customer Outstanding">
                                                                    </dxe:ASPxLabel>
                                                                </a>

                                                            </DataItemTemplate>
                                                            <EditFormSettings Visible="False" />
                                                            <CellStyle Wrap="False" CssClass="text-center">
                                                            </CellStyle>
                                                            <Settings AutoFilterCondition="Contains" />
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <HeaderStyle Wrap="False" CssClass="text-center" />

                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="NetAmount"
                                                            VisibleIndex="3">
                                                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Caption="Type" FieldName="Pos_EntryType" Width="70"
                                                            VisibleIndex="4">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>



                                                        <dxe:GridViewDataTextColumn Caption="Delv. Type" FieldName="Pos_DeliveryType"
                                                            VisibleIndex="4" Width="80">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Caption="Delv. Date" FieldName="Pos_DeliveryDate"
                                                            VisibleIndex="4">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                        

                                                        <dxe:GridViewDataTextColumn Caption="SRN?" FieldName="isSRN"
                                                            VisibleIndex="4" Width="50">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Caption="SRN No." FieldName="SRNno"
                                                            VisibleIndex="4">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Caption="SRN Date" FieldName="SrnDate"
                                                            VisibleIndex="4">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Caption="CD Date" FieldName="cdDate"
                                                            VisibleIndex="4">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Caption="Assigned Unit" FieldName="pos_assignBranch"
                                                            VisibleIndex="4" Width="350">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Caption="Sales Person" FieldName="salesman" Width="150"
                                                            VisibleIndex="4">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>


                                                        <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                                                            VisibleIndex="4">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                        
                                                       <dxe:GridViewDataTextColumn Caption="IRN" FieldName="Irn"
                                                            VisibleIndex="19">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>
                                                       <dxe:GridViewDataTextColumn Caption="Ack. Date" FieldName="AckDt" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy hh:mm:ss"
                                                            VisibleIndex="20">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                       <dxe:GridViewDataTextColumn Caption="Ack. No." FieldName="AckNo"
                                                            VisibleIndex="21">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                                                            <DataItemTemplate>
                                                                <div class='floatedBtnArea'>
                                                                </div>
                                                            </DataItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />

                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                                                            <DataItemTemplate>
                                                                <div class='floatedBtnArea'>

                                                                      <a href="javascript:void(0);" onclick="DoownLoadJsonTSI('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                                                        <span class='ico deleteColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Download JSON</span></a>
                                                                  <a href="javascript:void(0);" onclick="CancelIRNTSI('<%# Eval("Irn") %>')" id="a_CancelIRN" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel IRN</span></a>
                                         <a href="javascript:void(0);" onclick="genEwaybillTSI('<%# Eval("Irn") %>','<%# Container.KeyValue %>')" id="a_genEwaybillTSI" class="" title="">
                                                                            <span class='ico ColorSix'><i class='fa fa-truck' aria-hidden='true'></i></span><span class='hidden-xs'>Generate E-way bill</span></a>                          
                                                                    
                                                                      <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'TSI', 'IRN_CANCEL','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRN" class="" title="">
                                   

                                                                           <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info</span></a>
                                        <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'TSI', 'EWAY_GEN','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRNTSI" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>E way bill Info</span></a>

                                                                    </div>

                                                            </DataItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />

                                                        </dxe:GridViewDataTextColumn>

                                                    </Columns>
                                                    <SettingsPager PageSize="10">
                                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                                    </SettingsPager>
                                                    <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                    <SettingsLoadingPanel Text="Please Wait..." />
                                                    <ClientSideEvents RowClick="gridcrmCampaignclick" />
                                                    <TotalSummary>
                                                        <dxe:ASPxSummaryItem FieldName="NetAmount" SummaryType="Sum" />
                                                    </TotalSummary>


                                                </dxe:ASPxGridView>
                                            </div>
                                            </div>
                                           
                                     </div>
                                        <div class="tab-pane" id="pills-profileTSI" role="tabpanel" aria-labelledby="pills-profile-tab">
                                         <div>
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <label>From Date</label>
                                                    <div>
                                                        <dxe:ASPxDateEdit ID="ASPxDateEdit5" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDatePendingTSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <label>To Date</label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="ASPxDateEdit6" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDatePendingTSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>

                                                </div>
                                                    </div>
                                                <div class="col-md-3">
                                                    <label>Unit</label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="brPendingTSI" runat="server" ClientInstanceName="ccmbBranchfilterPendingTSI" Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </div>
                                                    </div>
                                                <div class="col-md-3" style="padding-top:22px">
                                                    <input type="button" value="Show" class="btn btn-success" id="TSIpendingBut" onclick="updateGridByDatePendingTSI()" />
                                                </div>
                                            </div>
                                        </div>
                                        <div style="margin-top:14px">
                                          <%--  <button type="button" class="btn btn-Crimson fontPp"  onclick="DownloadBulkJSONPendingTSI()">Donload Bulk JSON</button>
                                            <button type="button" class="btn btn-OrangeRed fontPp" onclick="UploadBulkIRNTSI()">Upload Bulk IRN</button>--%>
                                           
                                        </div>
                                          <div class="row mTop5">
                                                <div class="col-md-12">
                                                   
                                                    <dxe:ASPxGridView ID="GrdQuotationPendingTSI" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                        Width="100%" ClientInstanceName="cGrdQuotationPendingTSI" SettingsBehavior-AllowFocusedRow="true" 
                                                        SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                        SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
                                                        SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" Styles-SearchPanel-CssClass="searchBoxSmall" ClientSideEvents-EndCallback="grdEndcallbackPendingTSI">
                                                        <SettingsSearchPanel Visible="True" Delay="5000" />
                                                        <Columns>
                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" />
                                                            <dxe:GridViewDataTextColumn Caption="Sl No." FieldName="SlNo" Width="50" Visible="false" SortOrder="Descending"
                                                                VisibleIndex="0" FixedStyle="Left">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Document No." FieldName="InvoiceNo" Width="200"
                                                                VisibleIndex="0" FixedStyle="Left">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Invoice_Date"
                                                                VisibleIndex="1">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="CustomerName" Caption="Customer" VisibleIndex="2" Width="300">
                                                                <DataItemTemplate>
                                                                    <a href="javascript:void(0);" onclick="CustomerClick(this,'<%# Eval("Customer_Id") %>')">
                                                                        <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("CustomerName")%>'
                                                                            ToolTip="Customer Outstanding">
                                                                        </dxe:ASPxLabel>
                                                                    </a>

                                                                </DataItemTemplate>
                                                                <EditFormSettings Visible="False" />
                                                                <CellStyle Wrap="False" CssClass="text-center">
                                                                </CellStyle>
                                                                <Settings AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <HeaderStyle Wrap="False" CssClass="text-center" />

                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="NetAmount"
                                                                VisibleIndex="3">
                                                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Type" FieldName="Pos_EntryType" Width="70"
                                                                VisibleIndex="4">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>



                                                            <dxe:GridViewDataTextColumn Caption="Delv. Type" FieldName="Pos_DeliveryType"
                                                                VisibleIndex="4" Width="80">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Delv. Date" FieldName="Pos_DeliveryDate"
                                                                VisibleIndex="4">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            

                                                            <dxe:GridViewDataTextColumn Caption="SRN?" FieldName="isSRN"
                                                                VisibleIndex="4" Width="50">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="SRN No." FieldName="SRNno"
                                                                VisibleIndex="4">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="SRN Date" FieldName="SrnDate"
                                                                VisibleIndex="4">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="CD Date" FieldName="cdDate"
                                                                VisibleIndex="4">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Assigned Unit" FieldName="pos_assignBranch"
                                                                VisibleIndex="4" Width="350">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn Caption="Sales Person" FieldName="salesman" Width="150"
                                                                VisibleIndex="4">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>


                                                            <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                                                                VisibleIndex="4">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                                                                <DataItemTemplate>
                                                                    <div class='floatedBtnArea'>
                                                                    </div>
                                                                </DataItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />

                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                                                                <DataItemTemplate>
                                                                    <div class='floatedBtnArea'>

                                                                        <a href="javascript:void(0);" onclick="DoownLoadJsonTSI('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Download JSON</span></a>
                                                                        <a href="javascript:void(0);" onclick="UploadIRNTSI('<%# Container.KeyValue %>')" id="a_uploadIRNsi" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Upload IRN</span></a>
                                                                        <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'TSI', 'IRN_GEN','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRN" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info</span></a>
                                                                    </div>

                                                                </DataItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />

                                                            </dxe:GridViewDataTextColumn>

                                                        </Columns>
                                                        <SettingsPager PageSize="10">
                                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                                        </SettingsPager>
                                                        <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                        <SettingsLoadingPanel Text="Please Wait..." />
                                                        <ClientSideEvents RowClick="gridcrmCampaignclick" />
                                                        <TotalSummary>
                                                            <dxe:ASPxSummaryItem FieldName="NetAmount" SummaryType="Sum" />
                                                        </TotalSummary>


                                                    </dxe:ASPxGridView>
                                                </div>
                                            </div>
                                     </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane" id="messages">
                         <div class="row">
                            <div class="col-md-3">
                                <div class="holderBox fontPp c3">
                                    <h4>Credit Note Details</h4>
                                    <div class="row">
                                        <div class="col-sm-4 bDashed-right">
                                            <div>Count</div>
                                            <div id="SRTOTAL_COUNTS" class="strong">0</div>
                                        </div>
                                        <div class="col-sm-8">
                                            <div>Amount</div>
                                            <div id="SRTOTAL_AMOUNT" class="strong">0</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="holderBox fontPp c1 boxHover clickTrig" data-triggreid="#pills-homeSR-tab">
                                    <h4>e - Way Bill Generated</h4>
                                    <div class="row">
                                        <div class="col-sm-4 bDashed-right">
                                            <div>Count</div>
                                            <div id="SRTOTAL_GENERATED" class="strong">0</div>
                                        </div>
                                        <div class="col-sm-8">
                                            <div>Amount</div>
                                            <div id="SRTOTAL_GENERATED_AMOUNT" class="strong">0</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="holderBox fontPp c2 boxHover clickTrig" data-triggreid="#pills-profileSR-tab">
                                    <h4>e - Way Bill Pending</h4>
                                    <div class="row">
                                        <div class="col-sm-4 bDashed-right">
                                            <div>Count</div>
                                            <div id="SRTOTAL_PENDING" class="strong">0</div>
                                        </div>
                                        <div class="col-sm-8">
                                            <div>Amount</div>
                                            <div id="SRTOTAL_PENDING_AMOUNT" class="strong">0</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="holderBox fontPp boxHover clickTrig" data-triggreid="#pills-cancelSR-tab">
                                    <h4>e - Way Bill Cancelled</h4>
                                    <div class="row">
                                        <div class="col-sm-4 bDashed-right">
                                            <div>Count</div>
                                            <div id="SRTOTAL_CANCEL" class="strong">0</div>
                                        </div>
                                        <div class="col-sm-8">
                                            <div>Amount</div>
                                            <div id="SRTOTAL_CANCEL_AMOUNT" class="strong">0</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 horiTab">
                                <ul class="nav nav-tabs mb-3" id="pillsSR-tab" role="tablist">
                                  <li class="nav-item active">
                                    <a class="nav-link " id="pills-homeSR-tab" data-toggle="pill" href="#pills-homeSR" role="tab" aria-controls="pills-homeSR" aria-selected="true">e - Way Bill Generated</a>
                                  </li>
                                  <li class="nav-item">
                                    <a class="nav-link" id="pills-profileSR-tab" data-toggle="pill" href="#pills-profileSR" role="tab" aria-controls="pills-profileSR" aria-selected="false">e - Way Bill Pending</a>
                                  </li>                  
                                </ul>
                                <div class="tab-content" id="pillsSR-tabContent">
                                     <div class="tab-pane active" id="pills-homeSR" role="tabpanel" aria-labelledby="pills-homeSR-tab">
                                            <div>
                                                                          <div class="row">
                                                                                <div class="col-md-3">
                                                                                    <label>From Date</label>
                                                                                    <div>
                                                                                        <dxe:ASPxDateEdit ID="ASPxDateEdit7" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDateCR" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-3">
                                                                                    <label>To Date</label>
                                                                                <div>
                                                                                    <dxe:ASPxDateEdit ID="ASPxDateEdit8" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDateCR" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                        <ButtonStyle Width="13px">
                                                                                        </ButtonStyle>
                                                                                    </dxe:ASPxDateEdit>

                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3">
                                                                                    <label>Unit</label>
                                                                                <div>
                                                                                    <dxe:ASPxComboBox ID="crBR" runat="server" ClientInstanceName="ccmbBranchfilterCR" Width="100%">
                                                                                    </dxe:ASPxComboBox>
                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3" style="padding-top:22px">
                                                                                    <input type="button" value="Show" class="btn btn-success" id="SRgenBut" onclick="updateGridByDateCR()" />
                                                                                </div>
                                                                            </div>
                                                                      </div>
                                            <div style="margin-top:14px">
                                              <%-- <button type="button" class="btn btn-dark fontPp" data-toggle="modal" onclick="CancelBulkIRNSR()">Cancel Bulk IRN</button>
                                                <button type="button" class="btn btn-Crimson fontPp" onclick="DownloadBulkJSONSR()">Download Bulk IRN</button>
                                                <button type="button" class="btn btn-OrangeRed fontPp hide" onclick="UploadEwaybillSR()">Upload Bulk E-Way bill</button>--%>

                                            </div>
                                             
                                            <div class="row">
                                                 <div class="col-md-12">
                                                
                                                <dxe:ASPxGridView ID="GrdQuotationCR" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                    Width="100%" ClientInstanceName="cGrdQuotationCR" SettingsBehavior-AllowFocusedRow="true"
                                                    SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                    SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
                                                    SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" Styles-SearchPanel-CssClass="searchBoxSmall" ClientSideEvents-EndCallback="grdEndcallbackCR">
                                                    <SettingsSearchPanel Visible="True" Delay="5000" />
                                                    <Columns>
                                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" />
                                                        <dxe:GridViewDataTextColumn Caption="Sl No." FieldName="SlNo" Width="50" Visible="false" SortOrder="Descending"
                                                            VisibleIndex="0" FixedStyle="Left">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Document No." FieldName="InvoiceNo" Width="200"
                                                            VisibleIndex="0" FixedStyle="Left">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Invoice_Date"
                                                            VisibleIndex="1">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn FieldName="CustomerName" Caption="Customer" VisibleIndex="2" Width="300">
                                                            <DataItemTemplate>
                                                                <a href="javascript:void(0);" onclick="CustomerClick(this,'<%# Eval("CustomerName") %>')">
                                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("CustomerName")%>'
                                                                        ToolTip="Customer Outstanding">
                                                                    </dxe:ASPxLabel>
                                                                </a>

                                                            </DataItemTemplate>
                                                            <EditFormSettings Visible="False" />
                                                            <CellStyle Wrap="False" CssClass="text-center">
                                                            </CellStyle>
                                                            <Settings AutoFilterCondition="Contains" />
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <HeaderStyle Wrap="False" CssClass="text-center" />

                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="NetAmount"
                                                            VisibleIndex="3">
                                                            <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>                                                                                                        

                                                        <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                                                            VisibleIndex="4">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn Caption="IRN" FieldName="Irn"
                                                            VisibleIndex="19">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>
                                                       <dxe:GridViewDataTextColumn Caption="Ack. Date" FieldName="AckDt" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy hh:mm:ss"
                                                            VisibleIndex="20">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                       <dxe:GridViewDataTextColumn Caption="Ack. No." FieldName="AckNo"
                                                            VisibleIndex="21">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>


                                                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                                                            <DataItemTemplate>
                                                                <div class='floatedBtnArea'>
                                                                </div>
                                                            </DataItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />

                                                        </dxe:GridViewDataTextColumn>

                                                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                                                            <DataItemTemplate>
                                                                <div class='floatedBtnArea'>

                                                                     <a href="javascript:void(0);" onclick="DoownLoadJsonSR('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Download JSON</span></a>
                                                                  <a href="javascript:void(0);" onclick="CancelIRNSR('<%# Eval("Irn") %>')" id="a_CancelIRN" class="" title="">
                                                                            <span class='ico deleteColor'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel IRN</span></a>
                                             <a href="javascript:void(0);" onclick="genEwaybillSR('<%# Eval("Irn") %>','<%# Container.KeyValue %>')" id="a_genEwaybillSR" class="" title="">
                                                                            <span class='ico ColorSix'><i class='fa fa-truck' aria-hidden='true'></i></span><span class='hidden-xs'>Generate E-Way Bill</span></a>


                                                                    <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SR', 'IRN_CANCEL','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="CancelIRN" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info</span></a>

                                            <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SR', 'EWAY_GEN','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRN" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>E way bill Info</span></a>



                                                                 </div>

                                                            </DataItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                                            <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                                            <EditFormSettings Visible="False"></EditFormSettings>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />

                                                        </dxe:GridViewDataTextColumn>

                                                    </Columns>
                                                    <SettingsPager PageSize="10">
                                                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                                    </SettingsPager>
                                                    <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                    <SettingsLoadingPanel Text="Please Wait..." />
                                                    <ClientSideEvents RowClick="gridcrmCampaignclick" />
                                                    <TotalSummary>
                                                        <dxe:ASPxSummaryItem FieldName="NetAmount" SummaryType="Sum" />
                                                    </TotalSummary>


                                                </dxe:ASPxGridView>
                                            </div>
                                            </div>
                                           
                                     </div>
                                     <div class="tab-pane" id="pills-profileSR" role="tabpanel" aria-labelledby="pills-profileSR-tab">
                                         <div>
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <label>From Date</label>
                                                    <div>
                                                        <dxe:ASPxDateEdit ID="ASPxDateEdit9" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDatePendingCR" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <label>To Date</label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="ASPxDateEdit10" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDatePendingCR" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>

                                                </div>
                                                    </div>
                                                <div class="col-md-3">
                                                    <label>Unit</label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="brPendingCR" runat="server" ClientInstanceName="ccmbBranchfilterPendingCR" Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </div>
                                                    </div>
                                                <div class="col-md-3" style="padding-top:22px">
                                                    <input type="button" value="Show" class="btn btn-success" id="SRpendingBut" onclick="updateGridByDatePendingCR()" />
                                                </div>
                                            </div>
                                        </div>
                                        <div style="margin-top:14px">
                                          <%--  <button type="button" class="btn btn-Crimson fontPp" onclick="DownloadBulkJSONPendingSR()">Donload Bulk JSON</button>
                                            <button type="button" class="btn btn-OrangeRed fontPp" onclick="UploadBulkIRNTSR()">Upload Bulk IRN</button>
                                           --%>
                                        </div>
                                          <div class="row mTop5">
                                                <div class="col-md-12">
                                                    
                                                    <dxe:ASPxGridView ID="GrdQuotationPendingCR" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                        Width="100%" ClientInstanceName="cGrdQuotationPendingCR" SettingsBehavior-AllowFocusedRow="true" 
                                                        SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                        SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
                                                        SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" Styles-SearchPanel-CssClass="searchBoxSmall" ClientSideEvents-EndCallback="grdEndcallbackPendingCR">
                                                        <SettingsSearchPanel Visible="True" Delay="5000"  />
                                                        <Columns>
                                                            <dxe:GridViewCommandColumn ShowSelectCheckbox="true" VisibleIndex="0" />
                                                            <dxe:GridViewDataTextColumn Caption="Sl No." FieldName="SlNo" Width="50" Visible="false" SortOrder="Descending"
                                                                VisibleIndex="0" FixedStyle="Left">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Document No." FieldName="InvoiceNo" Width="200"
                                                                VisibleIndex="0" FixedStyle="Left">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Invoice_Date"
                                                                VisibleIndex="1">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn FieldName="CustomerName" Caption="Customer" VisibleIndex="2" Width="300">
                                                                <DataItemTemplate>
                                                                    <a href="javascript:void(0);" onclick="CustomerClick(this,'<%# Eval("CustomerName") %>')">
                                                                        <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("CustomerName")%>'
                                                                            ToolTip="Customer Outstanding">
                                                                        </dxe:ASPxLabel>
                                                                    </a>

                                                                </DataItemTemplate>
                                                                <EditFormSettings Visible="False" />
                                                                <CellStyle Wrap="False" CssClass="text-center">
                                                                </CellStyle>
                                                                <Settings AutoFilterCondition="Contains" />
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <HeaderStyle Wrap="False" CssClass="text-center" />

                                                            </dxe:GridViewDataTextColumn>
                                                            <dxe:GridViewDataTextColumn Caption="Net Amount" FieldName="NetAmount"
                                                                VisibleIndex="3">
                                                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                           
                                                            <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy"
                                                                VisibleIndex="4">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                                                                <DataItemTemplate>
                                                                    <div class='floatedBtnArea'>
                                                                    </div>
                                                                </DataItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />

                                                            </dxe:GridViewDataTextColumn>

                                                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                                                                <DataItemTemplate>
                                                                    <div class='floatedBtnArea'>

                                                                         <a href="javascript:void(0);" onclick="DoownLoadJsonSR('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Download JSON</span></a>
                                                                        <a href="javascript:void(0);" onclick="UploadIRNSR('<%# Container.KeyValue %>')" id="a_uploadIRNsi" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Upload IRN</span></a>
                                                                        <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SR', 'IRN_GEN','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="uploadIRNsi" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info</span></a>
                                                                   </div>

                                                                </DataItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />

                                                            </dxe:GridViewDataTextColumn>

                                                        </Columns>
                                                        <SettingsPager PageSize="10">
                                                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,20,50,100,150,200" />
                                                        </SettingsPager>
                                                        <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="true" ShowFilterRow="true" ShowFilterRowMenu="true" />
                                                        <SettingsLoadingPanel Text="Please Wait..." />
                                                        <ClientSideEvents RowClick="gridcrmCampaignclick" />
                                                        <TotalSummary>
                                                            <dxe:ASPxSummaryItem FieldName="NetAmount" SummaryType="Sum" />
                                                        </TotalSummary>


                                                    </dxe:ASPxGridView>
                                                </div>
                                            </div>
                                     </div>                                    
                                </div>
                            </div>
                        </div>
                    </div>
           </div>
        </div>
    </div>
        </div>
</asp:Content>
