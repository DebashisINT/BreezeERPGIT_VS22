<%--====================================================Revision History=========================================================================
 1.0   v2.0.37	Priti	13-03-2023	0025686:Eway Bill Cancel not working for Transit Sales Invoice & Credit Note
====================================================End Revision History=====================================================================--%>


<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="eInvoice.aspx.cs" Inherits="ERP.OMS.Management.eInvoice" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="csrf-token" content="XYZ123" />
    <script src="Activities/JS/eInvoice.js?v5.2"></script>    
    <link href="CSS/einvoiceStyle.css" rel="stylesheet" />
    <style type="text/css">
        .new-box{
            background:#efebeb;
            border-radius:10px;
        }
        .rightC{
            background:#f34f5a;
            border-radius:10px 0px 0px 10px;
            position:relative;
            min-height:86px;
            justify-content:center;
            align-items:center;
            color:#fff
        }
        .rightC:after{
            content:"";
            border-left:20px solid transparent;
            border-right:25px solid transparent;
            border-top: 86px solid #f34f5a;
            position:absolute;
            right:-25px;
            top:0;
        }
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <!-- Modal -->
<div id="homeFilter" class="modal pmsModal fade w30" role="dialog">
  <div class="modal-dialog">
    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Invoice Filter</h4>
      </div>
      <div class="modal-body">
        <div class="clearfix">
            <label>From</label>
            <div>
                <div>
                    <dxe:ASPxDateEdit ID="cFormDateFilter2" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDateFilter1" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
            </div>
        </div>
        <div class="clearfix">
            <label>To</label>
            <div>
                <div>
                    <dxe:ASPxDateEdit ID="cToDateFilter2" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cToDateFilter1" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
            </div>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-primary" id="filterOneok">Ok</button>
        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>
<%--Modal filter 2--%>
    <div id="profileFilter" class="modal pmsModal fade w30" role="dialog">
  <div class="modal-dialog">
    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Transit Sales Filter</h4>
      </div>
      <div class="modal-body">
        <div class="clearfix">
            <label>From</label>
            <div>
                <div>
                    <dxe:ASPxDateEdit ID="ASPxDateEdit29" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDateFilter2" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
            </div>
        </div>
        <div class="clearfix">
            <label>To</label>
            <div>
                <div>
                    <dxe:ASPxDateEdit ID="ASPxDateEdit30" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cToDateFilter2" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
            </div>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-primary" id="filterTwook">Ok</button>
        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>

    <%--Modal filter 2--%>
    <div id="messagesFilter" class="modal pmsModal fade w30" role="dialog">
  <div class="modal-dialog">
    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Credit Note Filter</h4>
      </div>
      <div class="modal-body">
        <div class="clearfix">
            <label>From</label>
            <div>
                <div>
                    <dxe:ASPxDateEdit ID="ASPxDateEdit31" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDateFilter3" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
            </div>
        </div>
        <div class="clearfix">
            <label>To</label>
            <div>
                <div>
                    <dxe:ASPxDateEdit ID="ASPxDateEdit32" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cToDateFilter3" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </div>
            </div>
        </div>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-primary" id="filterThreeok">Ok</button>
        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>


    <a id="downloadAnchorElem" style="display: none"></a>
    <div class="panel-title clearfix">
        <h3 class="pull-left">
            <%--<asp:Label ID="lblHeadTitle" Text="" runat="server"></asp:Label>--%>
            <label>
                E-Invoice 
            </label>
        </h3>
    </div>
    <div class="form_main">
        <div  class="clearfix Vtabs">  
            <div class="row">   
            <div class="col-sm-2 col-md-2 col-lg-1">
                <!-- required for floating -->
                <!-- Nav tabs -->
                <ul class="nav nav-pills nav-stacked flex-column" style="margin-top:20px">
                    <li class="active"><a href="#home" data-toggle="tab">Invoice</a></li>
                    <li id="TsiTabLoad"><a href="#profile" data-toggle="tab">Transit Sales</a></li>
                    <li id="SrTabLoad"><a href="#messages" data-toggle="tab">Credit Note</a></li>
                    <%--<li><a href="#settings" data-toggle="tab">Debit Note</a></li>--%>
                </ul>
            </div>
            <div class="col-sm-10 col-md-10 col-lg-11">
                <!-- Tab panes -->
                <div class="tab-content ttCont">
                    <div class="tab-pane active" id="home">
                        <div class="filterItem homeFilter" data-toggle="modal" data-target="#homeFilter"><i class="fa fa-filter" data-toggle="tooltip" data-placement="top" title="Filter"></i></div>
                        <div class="">
                            <div class="row pt-3" style="margin:0">
                                <div class="col-md-12">
                                    <div class="new-box">
                                        <div class="row">
                                            <div class="col-md-3 rightC">
                                                <div class=" ">
			                                        <h4>Invoice Details</h4>
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
                                            <div class="col-md-3 " style="display: flex; justify-content: center;padding-left: 40px; flex-direction: column;">
                                                <div class=" boxHover clickTrig" data-triggreid="#pills-home-tab">
			                                        <h4>IRN Generated</h4>
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
                                            <div class="col-md-3" style="display: flex; justify-content: center; flex-direction: column;">
                                                <div class=" boxHover clickTrig" data-triggreid="#pills-profile-tab">
			                                        <h4>IRN Pending</h4>
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
                                            <div class="col-md-3" style="display: flex; justify-content: center; flex-direction: column;">
                                                <div class=" boxHover clickTrig" data-triggreid="#pills-cancel-tab">
			                                        <h4>IRN Cancelled</h4>
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
                                    </div>
                                </div>
                            </div>
                       
                        </div>
                        
                        <div class="row">
                            <div class="col-md-12 horiTab">
                                <ul class="nav nav-tabs mb-3" id="pills-tab" role="tablist">
                                  <li class="nav-item active">
                                    <a class="nav-link " id="pills-home-tab" data-toggle="pill" href="#pills-home" role="tab" aria-controls="pills-home" aria-selected="true">IRN Generated</a>
                                  </li>
                                  <li class="nav-item">
                                    <a class="nav-link" id="pills-profile-tab" data-toggle="pill" href="#pills-profile" role="tab" aria-controls="pills-profile" aria-selected="false">IRN Pending</a>
                                  </li>
                                 <li class="nav-item">
                                    <a class="nav-link" id="pills-cancel-tab" data-toggle="pill" href="#pills-cancel" role="tab" aria-controls="pills-cancel" aria-selected="false">IRN Cancelled</a>
                                  </li>
                                  <li class="nav-item">
                                    <a class="nav-link" id="pills-ewaybill-tab" data-toggle="pill" href="#pills-ewaybill" role="tab" aria-controls="pills-ewaybill" aria-selected="false">E-Way bill Generated</a>
                                  </li>
                                <li class="nav-item hide">
                                    <a class="nav-link" id="pills-Cancelewaybill-tab" data-toggle="pill" href="#pills-Cancelewaybill" role="tab" aria-controls="pills-Cancelewaybill" aria-selected="false">E-Way bill Cancelled</a>
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
                                                <% if (rights.CanIRN)
                                                { %>
                                                <button type="button" class="btn btn-dark fontPp hide" data-toggle="modal" onclick="CancelBulkIRN()">Cancel Bulk IRN</button>
                                                 <% } %>
                                                <% if (rights.CanIRN)
                                                { %>
                                                <button type="button" class="btn btn-Crimson fontPp" onclick="DownloadBulkJSONSI()">Download Bulk IRN</button>
                                                <% } %>
                                                <% if (rights.CanEWayBill)
                                                { %>
                                              <button type="button" class="btn btn-OrangeRed fontPp hide " onclick="UploadEwaybillSI()">Upload Bulk E-Way Bill</button>
                                                <% } %>
                                                <%--  <button type="button" class="btn btn-SeaGreen fontPp">Upload Bulk IRN</button>--%>
                                            </div>
                                             
                                            <div class="row">
                                                 <div class="col-md-12">
                                                <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />
                                                <asp:HiddenField ID="hfBranchID" runat="server" />
                                                <asp:HiddenField ID="hfFromDate" runat="server" />
                                                <asp:HiddenField ID="hfIsFilter" runat="server" />
                                                <asp:HiddenField ID="hfToDate" runat="server" />
                                                <asp:HiddenField ID="HiddenField5" runat="server" />
                                                <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                    Width="100%" ClientInstanceName="cGrdQuotation" SettingsBehavior-AllowFocusedRow="true" DataSourceID="EntityServerModeDataSource"
                                                    SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                    SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" OnCustomCallback="GrdQuotation_CustomCallback"
                                                    SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" ClientSideEvents-EndCallback="grdEndcallback" Styles-SearchPanel-CssClass="searchBoxSmall">
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

                                                                         <% if (rights.CanIRN)
                                                                            { %>
                                                                    <a href="javascript:void(0);" onclick="DoownLoadJson('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Download JSON</span></a>
                                                                    <% } %>

                                                                     <% if (rights.CanIRN)
                                                                            { %>
                                                                  <a href="javascript:void(0);" onclick="CancelIRN('<%# Eval("Irn") %>')" id="a_CancelIRN" class="" style='<%#Eval("Can_IRN")%>' title="">
                                                                            <span class='ico deleteColor'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel IRN</span></a>
                                                                     <% } %>

                                                                     <% if (rights.CanEWayBill)
                                                                            { %>
                                                                  <a href="javascript:void(0);" onclick="UpdatePinIRN('<%# Container.KeyValue %>')" id="a_UpdatePin" class="" style='<%#Eval("Can_IRN")%>' title="">
                                                                            <span class='ico editColor'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Update Pin</span></a>
                                                                     <% } %>

                                                                     <% if (rights.CanEWayBill)
                                                                            { %>
                                                             <a href="javascript:void(0);" onclick="genEwaybill('<%# Eval("Irn") %>','<%# Container.KeyValue %>')" id="a_genEwaybill" class="" title="">
                                                                            <span class='ico ColorSix'><i class='fa fa-truck' aria-hidden='true'></i></span><span class='hidden-xs'>Generate E-Way bill</span></a>
                                                                    <% } %>
                                                                     <% if (rights.CanIRN)
                                                                            { %>
                                                                     <a href="javascript:void(0);" 
                                                                          onclick="ShowInfoN('<%# Container.KeyValue %>', 'SI', 'IRN_CANCEL','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="InfoIRN" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info</span></a>
                                                                     <% } %>
                                                                    <% if (rights.CanEWayBill)
                                                                            { %>
                                                                    <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SI', 'EWAY_GEN','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_ebillInfoIRN" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>E way bill Info</span></a>
                                                                     <% } %>
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
                                            <% if (rights.CanEWayBill) { %>
                                            <button type="button" class="btn btn-Crimson fontPp"  onclick="DownloadBulkJSONPendingSI()">Donload Bulk JSON</button>
                                            <% } %>
                                            <% if (rights.CanIRN) { %>
                                            <button type="button" class="btn btn-OrangeRed fontPp" onclick="UploadBulkIRNSI()">Upload Bulk IRN</button>
                                            <% } %>
                                        </div>
                                          <div class="row mTop5">
                                                <div class="col-md-12">
                                                    <dx:LinqServerModeDataSource ID="LinqServerModeDataSourcePendinSI" runat="server" OnSelecting="LinqServerModeDataSourcePendinSI_Selecting"
                                                        ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />
                                                    <dxe:ASPxGridView ID="GrdQuotationPendinSI" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                        Width="100%" ClientInstanceName="cGrdQuotationPendinSI" SettingsBehavior-AllowFocusedRow="true" DataSourceID="LinqServerModeDataSourcePendinSI"
                                                        SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                        SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" OnCustomCallback="ASPxGridView2_CustomCallback" 
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
                                                                          <% if (rights.CanIRN)
                                                                             { %>
                                                                        <a href="javascript:void(0);" onclick="DoownLoadJson('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Download JSON</span></a>
                                                                        <% } %>
                                                                          <% if (rights.CanIRN)
                                                                                { %> 
                                                                        <a href="javascript:void(0);" onclick="UploadIRNSI('<%# Container.KeyValue %>')" id="a_uploadIRNsi" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Upload IRN</span></a>
                                                                        <% } %>
                                                                        <% if (rights.CanIRN)
                                                                                { %>
                                                                        <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SI', 'IRN_GEN','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRN" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info</span></a>
                                                                        <% } %>
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
                                     <div class="tab-pane" id="pills-cancel" role="tabpanel" aria-labelledby="pills-cancel-tab">
                                         <div>
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <label>From Date</label>
                                                    <div>
                                                        <dxe:ASPxDateEdit ID="ASPxDateEdit15" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDateCancelledSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <label>To Date</label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="ASPxDateEdit16" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDateCancelledSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>

                                                </div>
                                                    </div>
                                                <div class="col-md-3">
                                                    <label>Unit</label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="brCancelledSI" runat="server" ClientInstanceName="ccmbBranchfilterCancelledSI" Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </div>
                                                    </div>
                                                <div class="col-md-3" style="padding-top:22px">
                                                    <input type="button" value="Show" class="btn btn-success" id="SIcancelBut" onclick="updateGridByDateCancelledSI()" />
                                                </div>
                                            </div>
                                        </div>
<%--                                        <div style="margin-top:14px">
                                            <button type="button" class="btn btn-dark fontPp" data-toggle="modal" data-target="#modal1">Upload Bulk IRN</button>
                                            <button type="button" class="btn btn-Crimson fontPp">Upload Bulk IRN</button>
                                            <button type="button" class="btn btn-OrangeRed fontPp">Upload Bulk IRN</button>
                                            <button type="button" class="btn btn-SeaGreen fontPp">Upload Bulk IRN</button>
                                        </div>--%>
                                                                                   <div class="row mTop5">
                                                <div class="col-md-12">
                                                    <dx:LinqServerModeDataSource ID="LinqServerModeDataSourceCancelSI" runat="server" OnSelecting="LinqServerModeDataSourceCancelSI_Selecting"
                                                        ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />
                                                        <dxe:ASPxGridView ID="ASPxGridView1" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                        Width="100%" ClientInstanceName="cGrdQuotationCancelledSI" SettingsBehavior-AllowFocusedRow="true" DataSourceID="LinqServerModeDataSourceCancelSI"
                                                        SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                        SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" 
                                                        SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" >
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
                                                       <dxe:GridViewDataTextColumn Caption="Ack. Date" FieldName="AckDt"
                                                            VisibleIndex="20">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>

                                                       <dxe:GridViewDataTextColumn Caption="Ack. No." FieldName="AckNo" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy hh:mm:ss"
                                                            VisibleIndex="21">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="IRN Cancel Dt." FieldName="IRN_Cancell_Date" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy hh:mm:ss"
                                                            VisibleIndex="22">
                                                            <CellStyle CssClass="gridcellleft" Wrap="true">
                                                            </CellStyle>
                                                            <Settings AllowAutoFilterTextInputTimer="False" />
                                                            <Settings AutoFilterCondition="Contains" />
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
                                     <div class="tab-pane" id="pills-ewaybill" role="tabpanel" aria-labelledby="pills-ewaybill-tab">
                                            <div>
                                                                          <div class="row">
                                                                                <div class="col-md-3">
                                                                                    <label>From Date</label>
                                                                                    <div>
                                                                                        <dxe:ASPxDateEdit ID="ASPxDateEdit21" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDateewaybillSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-3">
                                                                                    <label>To Date</label>
                                                                                <div>
                                                                                    <dxe:ASPxDateEdit ID="ASPxDateEdit22" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDateewaybillSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                        <ButtonStyle Width="13px">
                                                                                        </ButtonStyle>
                                                                                    </dxe:ASPxDateEdit>

                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3">
                                                                                    <label>Unit</label>
                                                                                <div>
                                                                                    <dxe:ASPxComboBox ID="brewaybillSI" runat="server" ClientInstanceName="ccmbBranchfilterewaybillSI" Width="100%">
                                                                                    </dxe:ASPxComboBox>
                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3" style="padding-top:22px">
                                                                                    <input type="button" value="Show" class="btn btn-success" onclick="updateGridByDateewaybillSI()" />
                                                                                </div>
                                                                            </div>
                                                                      </div>
                                            <div style="margin-top:14px">
                                                <%--<button type="button" class="btn btn-dark fontPp" data-toggle="modal" onclick="CancelBulkewaybillSI()">Cancel Bulk E-Way bill</button>  --%>                                              
                                                <%--<button type="button" class="btn btn-Crimson fontPp" onclick="DownloadBulkJSONSI()">Download Bulk E-Way bill</button>--%>
<%--                                                <button type="button" class="btn btn-OrangeRed fontPp">Upload Bulk IRN</button>
                                                <button type="button" class="btn btn-SeaGreen fontPp">Upload Bulk IRN</button>--%>
                                            </div>
                                             
                                            <div class="row">
                                                 <div class="col-md-12">
                                                <dx:LinqServerModeDataSource ID="EntityServerModeDataSourceewaybillSI" runat="server" OnSelecting="LinqServerModeDataSourceewaybillSI_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />

                                                <dxe:ASPxGridView ID="GrdQuotationewaybillSI" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                    Width="100%" ClientInstanceName="cGrdQuotationewaybillSI" SettingsBehavior-AllowFocusedRow="true" DataSourceID="EntityServerModeDataSourceewaybillSI"
                                                    SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                    SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" OnCustomCallback="GrdQuotationewaybillSI_CustomCallback"
                                                    SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" ClientSideEvents-EndCallback="grdEndcallbackewaybillSI" Styles-SearchPanel-CssClass="searchBoxSmall">
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




                                                       <dxe:GridViewDataTextColumn Caption="IRN" FieldName="Irn" Width="250"
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



                                                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="24" Width="0">
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


                                                        
                                                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="25" Width="0">
                                                            <DataItemTemplate>
                                                                <div class='floatedBtnArea'>

                                                                        <% if (rights.CanEWayBill)
                                                                             { %>
                                                         <a href="javascript:void(0);" onclick="DownloadEwayBillSI('<%# Eval("EWayBillNumber") %>')" id="a_DownloadEwaybillSI" class="" title="">
                                                        <span class='ico editColor'><i class='fa fa-download' aria-hidden='true'></i></span><span class='hidden-xs'>Download E-Way Bill</span></a>
                                                                     <% } %>

                                                                        <% if (rights.CanEWayBill)
                                                                             { %>
                                                                  <a href="javascript:void(0);" onclick="CancelEwayBillSI('<%# Eval("EWayBillNumber") %>')" id="a_CancelEwaybillSI" class="" title="">
                                                                            <span class='ico deleteColor'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel E-Way Bill</span></a>
                                                                     <% } %>
                                                                      <%--  <% if (rights.CanEWayBill)
                                                                             { %>
                                        <a href="javascript:void(0);" onclick="UpdateEwayBillSI('<%# Eval("EWayBillNumber") %>')" id="a_UpdateEwaybillSI" class="" title="">
                                                                            <span class='ico ColorSeven'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Update E-Way Bill</span></a>
                                                                     <% } %>--%>
                                                                        <% if (rights.CanEWayBill)
                                                                             { %>
                                           <a href="javascript:void(0);" onclick="UpdateTransporterEwayBillSI('<%# Eval("EWayBillNumber") %>')" id="a_UpdateTransporterEwaybillSI" class="" title="">
                                                                            <span class='ico ColorSeven'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Update Transporter</span></a>
                                                                     <% } %>
                                                                        <% if (rights.CanEWayBill)
                                                                             { %>
                                                                     <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SI', 'EWAYBILL_CANCEL','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRNeaycancelSI" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info (Cancel)</span></a>
                                                                     <% } %>

                                                                       <%-- <% if (rights.CanEWayBill)
                                                                             { %>
                                            <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SI', 'EWAYBILL_UPDATE','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRNEwayUpdateSI" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info (Update)</span></a>
                                                                     <% } %>--%>
                                                                        <% if (rights.CanEWayBill)
                                                                             { %>
                                            <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SI', 'EWAYBILL_UPDATETR','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRNEwayUpdateTrSI" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info (Update Transporter)</span></a>
                                                                     <% } %>

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
                                     <div class="tab-pane" id="pills-Cancelewaybill" role="tabpanel" aria-labelledby="pills-Cancelewaybill-tab">
                                            <div>
                                                                          <div class="row">
                                                                                <div class="col-md-3">
                                                                                    <label>From Date</label>
                                                                                    <div>
                                                                                        <dxe:ASPxDateEdit ID="ASPxDateEdit11" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDateCancelewaybillSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-3">
                                                                                    <label>To Date</label>
                                                                                <div>
                                                                                    <dxe:ASPxDateEdit ID="ASPxDateEdit12" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDateCancelewaybillSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                        <ButtonStyle Width="13px">
                                                                                        </ButtonStyle>
                                                                                    </dxe:ASPxDateEdit>

                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3">
                                                                                    <label>Unit</label>
                                                                                <div>
                                                                                    <dxe:ASPxComboBox ID="brCancelEwaybill" runat="server" ClientInstanceName="ccmbBranchfilterCancelewaybillSI" Width="100%">
                                                                                    </dxe:ASPxComboBox>
                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3" style="padding-top:22px">
                                                                                     <% if (rights.CanEWayBill) { %>
                                                                                    <input type="button" value="Show" class="btn btn-success" onclick="updateGridByDateCancelewaybill()" />
                                                                                    <% } %>
                                                                                </div>
                                                                            </div>
                                                                      </div>
                                            <div style="margin-top:14px">
                                               
                                            </div>
                                             
                                            <div class="row">
                                                 <div class="col-md-12">
                                                <dx:LinqServerModeDataSource ID="LinqServerModeDataSourceCancelewaybill" runat="server" OnSelecting="LinqServerModeDataSourceCancelewaybill_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />

                                                <dxe:ASPxGridView ID="GrdQuotationCancelewaybill" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                    Width="100%" ClientInstanceName="cGrdQuotationCancelewaybill" SettingsBehavior-AllowFocusedRow="true" DataSourceID="LinqServerModeDataSourceCancelewaybill"
                                                    SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                    SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" OnCustomCallback="GrdQuotationewaybillSI_CustomCallback"
                                                    SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" ClientSideEvents-EndCallback="grdEndcallbackEwaybillSI" Styles-SearchPanel-CssClass="searchBoxSmall">
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




                                                       <dxe:GridViewDataTextColumn Caption="IRN" FieldName="Irn" Width="250"
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
                        <div class="filterItem homeFilter" data-toggle="modal" data-target="#profileFilter"><i class="fa fa-filter" data-toggle="tooltip" data-placement="top" title="Filter"></i></div>
                         <div class="row">
                            <div class="col-md-3">
                                <div class="holderBox fontPp c3">
                                    <h4>Transit Invoice Details</h4>
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
                                    <h4>IRN Generated</h4>
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
                                    <h4>IRN Pending</h4>
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
                                    <h4>IRN Cancelled</h4>
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
                                    <a class="nav-link " id="pills-homeTSI-tab" data-toggle="pill" href="#pills-homeTSI" role="tab" aria-controls="pills-home" aria-selected="true">IRN Generated</a>
                                  </li>
                                  <li class="nav-item">
                                    <a class="nav-link" id="pills-profileTSI-tab" data-toggle="pill" href="#pills-profileTSI" role="tab" aria-controls="pills-profile" aria-selected="false">IRN Pending</a>
                                  </li>
                                 <li class="nav-item">
                                    <a class="nav-link" id="pills-CancelTSI-tab" data-toggle="pill" href="#pills-CancelTSI" role="tab" aria-controls="pills-cancel" aria-selected="false">IRN Cancelled</a>
                                  </li>
                                    <li class="nav-item">
                                    <a class="nav-link" id="pills-ewaybillTSI-tab" data-toggle="pill" href="#pills-ewaybillTSI" role="tab" aria-controls="pills-ewaybillTSI" aria-selected="false">E-Way bill Generated</a>
                                  </li>
                                <li class="nav-item hide">
                                    <a class="nav-link" id="pills-CancelewaybillTSI-tab" data-toggle="pill" href="#pills-CancelewaybillTSI" role="tab" aria-controls="pills-CancelewaybillTSI" aria-selected="false">E-Way bill Cancelled</a>
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
                                                <% if (rights.CanIRN) { %>
                                                <button type="button" class="btn btn-dark fontPp hide" data-toggle="modal" onclick="CancelBulkIRNTSI()">Cancel Bulk IRN</button>
                                                <button type="button" class="btn btn-Crimson fontPp" onclick="DownloadBulkJSONTSI()">Download Bulk IRN</button>
                                                <% } %>
                                                 <% if (rights.CanEWayBill) { %>
                                                <button type="button" class="btn btn-OrangeRed fontPp hide" onclick="UploadEwaybillTSI()">Upload Bulk E-Way bill</button>
                                                <% } %>
                 <%--                               <button type="button" class="btn btn-SeaGreen fontPp">Upload Bulk IRN</button>--%>
                                            </div>
                                             
                                            <div class="row">
                                                 <div class="col-md-12">
                                                <dx:LinqServerModeDataSource ID="LinqServerModeDataSourceTSI" runat="server" OnSelecting="LinqServerModeDataSourceTSI_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />
 
                                                <dxe:ASPxGridView ID="GrdQuotationTSI" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                    Width="100%" ClientInstanceName="cGrdQuotationTSI" SettingsBehavior-AllowFocusedRow="true" DataSourceID="LinqServerModeDataSourceTSI"
                                                    SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                    SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" OnCustomCallback="GrdQuotationTSI_CustomCallback"
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
                                                                     <% if (rights.CanIRN)
                                                                             { %>
                                                                      <a href="javascript:void(0);" onclick="DoownLoadJsonTSI('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                                                        <span class='ico deleteColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Download JSON</span></a>
                                                                    <% } %>
                                                                     <% if (rights.CanIRN)
                                                                             { %>
                                                                  <a href="javascript:void(0);" onclick="CancelIRNTSI('<%# Eval("Irn") %>')" id="a_CancelIRN" class="" style='<%#Eval("Can_IRN")%>' title="">
                                                                            <span class='ico editColor'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel IRN</span></a>
                                                                    <% } %>
                                                                      <% if (rights.CanEWayBill)
                                                                            { %>
                                                                  <a href="javascript:void(0);" onclick="UpdatePinIRNTSI('<%# Container.KeyValue %>')" id="a_UpdatePin" class="" style='<%#Eval("Can_IRN")%>' title="">
                                                                            <span class='ico editColor'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Update Pin</span></a>
                                                                     <% } %>
                                                                     <% if (rights.CanEWayBill)
                                                                             { %>
                                         <a href="javascript:void(0);" onclick="genEwaybillTSI('<%# Eval("Irn") %>','<%# Container.KeyValue %>')" id="a_genEwaybillTSI" class="" title="">
                                                                            <span class='ico ColorSix'><i class='fa fa-truck' aria-hidden='true'></i></span><span class='hidden-xs'>Generate E-way bill</span></a>                          
                                                                    <% } %>
                                                                     <% if (rights.CanIRN)
                                                                             { %>
                                                                      <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'TSI', 'IRN_CANCEL','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRN" class="" title="">
                                   
                                                                               <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info</span></a>
                                                                    <% } %>
                                                                     <% if (rights.CanEWayBill)
                                                                             { %>
                                                 <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'TSI', 'EWAY_GEN','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRNTSI" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>E way bill Info</span></a>
                                                                    <% } %>
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
                                           <%--   <% if (rights.CanIRN) { %>--%>
                                            <button type="button" class="btn btn-Crimson fontPp"  onclick="DownloadBulkJSONPendingTSI()">Download Bulk JSON</button>
                                          <%--  <% } %>--%>
                                            <% if (rights.CanIRN) { %>
                                                <button type="button" class="btn btn-OrangeRed fontPp" onclick="UploadBulkIRNTSI()">Upload Bulk IRN</button>
                                            <% } %>
                                        </div>
                                          <div class="row mTop5">
                                                <div class="col-md-12">
                                                    <dx:LinqServerModeDataSource ID="LinqServerModeDataSourcePendingTSI" runat="server" OnSelecting="LinqServerModeDataSourcePendingTSI_Selecting"
                                                        ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />
                                                    <dxe:ASPxGridView ID="GrdQuotationPendingTSI" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                        Width="100%" ClientInstanceName="cGrdQuotationPendingTSI" SettingsBehavior-AllowFocusedRow="true" DataSourceID="LinqServerModeDataSourcePendingTSI"
                                                        SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                        SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" OnCustomCallback="GrdQuotationPendingTSI_CustomCallback"
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
                                                                         <% if (rights.CanIRN)
                                                                             { %>
                                                                        <a href="javascript:void(0);" onclick="DoownLoadJsonTSI('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Download JSON</span></a>
                                                                        <% } %>
                                                                         <% if (rights.CanIRN)
                                                                             { %>
                                                                        <a href="javascript:void(0);" onclick="UploadIRNTSI('<%# Container.KeyValue %>')" id="a_uploadIRNsi" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Upload IRN</span></a>
                                                                         <% } %>
                                                                         <% if (rights.CanIRN)
                                                                             { %>
                                                                        <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'TSI', 'IRN_GEN','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRN" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info</span></a>
                                                                         <% } %>

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
                                        <div class="tab-pane" id="pills-CancelTSI" role="tabpanel" aria-labelledby="pills-CancelTSI-tab">
                                         <div>
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <label>From Date</label>
                                                    <div>
                                                        <dxe:ASPxDateEdit ID="ASPxDateEdit17" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDateCancelTSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <label>To Date</label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="ASPxDateEdit18" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDateCancelTSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>

                                                </div>
                                                    </div>
                                                <div class="col-md-3">
                                                    <label>Unit</label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="brCancelTSI" runat="server" ClientInstanceName="ccmbBranchfilterCancelTSI" Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </div>
                                                    </div>
                                                <div class="col-md-3" style="padding-top:22px">
                                                    <input type="button" value="Show" class="btn btn-success" id="TSIcancelBut" onclick="updateGridByDateCancelTSI()" />
                                                </div>
                                            </div>
                                        </div>
                                          <div class="row mTop5">
                                                <div class="col-md-12">
                                                    <dx:LinqServerModeDataSource ID="LinqServerModeDataSourceCancelTSI" runat="server" OnSelecting="LinqServerModeDataSourceCancelTSI_Selecting"
                                                        ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />
                                                    <dxe:ASPxGridView ID="GrdQuotationCancelTSI" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                        Width="100%" ClientInstanceName="cGrdQuotationCancelTSI" SettingsBehavior-AllowFocusedRow="true" DataSourceID="LinqServerModeDataSourceCancelTSI"
                                                        SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                        SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" OnCustomCallback="GrdQuotation_CustomCallback"
                                                        SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" ClientSideEvents-EndCallback="grdEndcallback">
                                                        <SettingsSearchPanel Visible="True" Delay="5000" />
                                                        <Columns>
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
                                                                        <% if(rights.CanIRN)
                                                                            { %>
                                                                        <a href="javascript:void(0);" onclick="DoownLoadJson('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Download JSON</span></a>
                                                                        <% } %>
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
                                        <div class="tab-pane" id="pills-ewaybillTSI" role="tabpanel" aria-labelledby="pills-ewaybillTSI-tab">
                                            <div>
                                                                          <div class="row">
                                                                                <div class="col-md-3">
                                                                                    <label>From Date</label>
                                                                                    <div>
                                                                                        <dxe:ASPxDateEdit ID="ASPxDateEdit13" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDateewaybillTSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-3">
                                                                                    <label>To Date</label>
                                                                                <div>
                                                                                    <dxe:ASPxDateEdit ID="ASPxDateEdit14" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDateewaybillTSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                        <ButtonStyle Width="13px">
                                                                                        </ButtonStyle>
                                                                                    </dxe:ASPxDateEdit>

                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3">
                                                                                    <label>Unit</label>
                                                                                <div>
                                                                                    <dxe:ASPxComboBox ID="brewaybillTSI" runat="server" ClientInstanceName="ccmbBranchfilterewaybillTSI" Width="100%">
                                                                                    </dxe:ASPxComboBox>
                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3" style="padding-top:22px">
                                                                                    <input type="button" value="Show" class="btn btn-success" onclick="updateGridByDateewaybillTSI()" />
                                                                                </div>
                                                                            </div>
                                                                      </div>
                                            <div style="margin-top:14px">
                                                <%--<button type="button" class="btn btn-dark fontPp" data-toggle="modal" onclick="CancelBulkewaybillSI()">Cancel Bulk E-Way bill</button>  --%>                                              
                                                <%--<button type="button" class="btn btn-Crimson fontPp" onclick="DownloadBulkJSONSI()">Download Bulk E-Way bill</button>--%>
<%--                                                <button type="button" class="btn btn-OrangeRed fontPp">Upload Bulk IRN</button>
                                                <button type="button" class="btn btn-SeaGreen fontPp">Upload Bulk IRN</button>--%>
                                            </div>
                                             
                                            <div class="row">
                                                 <div class="col-md-12">
                                                <dx:LinqServerModeDataSource ID="LinqServerModeDataSourceewaybillTSI" runat="server" OnSelecting="LinqServerModeDataSourceewaybillTSI_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />

                                                <dxe:ASPxGridView ID="GrdQuotationewaybillTSI" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                    Width="100%" ClientInstanceName="cGrdQuotationewaybillTSI" SettingsBehavior-AllowFocusedRow="true" DataSourceID="LinqServerModeDataSourceewaybillTSI"
                                                    SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                    SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" OnCustomCallback="GrdQuotationewaybillTSI_CustomCallback"
                                                    SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" ClientSideEvents-EndCallback="grdEndcallbackewaybillTSI" Styles-SearchPanel-CssClass="searchBoxSmall">
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




                                                       <dxe:GridViewDataTextColumn Caption="IRN" FieldName="Irn" Width="250"
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



                                                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="24" Width="0">
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



                                                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="25" Width="0">
                                                            <DataItemTemplate>
                                                                <div class='floatedBtnArea'>
                                                                    <%--Mantis Issue 24237--%>
                                                                    <% if (rights.CanEWayBill)
                                                                             { %>
                                                                             <a href="javascript:void(0);" onclick="DownloadEwayBillTSI('<%# Eval("EWayBillNumber") %>')" id="a_DownloadEwaybillTSI" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-download' aria-hidden='true'></i></span><span class='hidden-xs'>Download E-Way Bill</span></a>
                                                                     <% } %>
                                                                    <%--End of Mantis Issue 24237--%>
                                                                     <% if (rights.CanEWayBill)
                                                                             { %>
                                                                  <a href="javascript:void(0);" onclick="CancelEwayBillTSI('<%# Eval("EWayBillNumber") %>')" id="a_CancelEwaybillTSI" class="" title="">
                                                                            <span class='ico deleteColor'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel E-Way Bill</span></a>
                                                                    <% } %>
                                                                     <% if (rights.CanEWayBill)
                                                                             { %>
                                        <a href="javascript:void(0);" onclick="UpdateEwayBillTSI('<%# Eval("EWayBillNumber") %>')" id="a_UpdateEwaybillTSI" class="" title="">
                                                                            <span class='ico deleteColor'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Update E-Way Bill</span></a>
                                                                    <% } %>
                                                                     <% if (rights.CanEWayBill)
                                                                             { %>
                                           <a href="javascript:void(0);" onclick="UpdateTransporterEwayBillTSI('<%# Eval("EWayBillNumber") %>')" id="a_UpdateTransporterEwaybillTSI" class="" title="">
                                                                            <span class='ico deleteColor'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Update Transporter</span></a>
                                                                    <% } %>
                                                                     <% if (rights.CanEWayBill)
                                                                             { %>
                                                                     <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'TSI', 'EWAYBILL_CANCEL','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRNeaycancelSI" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info (Cancel)</span></a>
                                                                    <% } %>
                                                                     <% if (rights.CanEWayBill)
                                                                             { %>
                                            <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'TSI', 'EWAYBILL_UPDATE','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRNEwayUpdateSI" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info (Update)</span></a>
                                                                    <% } %>
                                                                     <% if (rights.CanEWayBill)
                                                                             { %>
                                            <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'TSI', 'EWAYBILL_UPDATETR','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRNEwayUpdateTrSI" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info (Update Transporter)</span></a>
                                                                    <% } %>
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
                                        <div class="tab-pane" id="pills-CancelewaybillTSI" role="tabpanel" aria-labelledby="pills-CancelewaybillTSI-tab">
                                            <div>
                                                                          <div class="row">
                                                                                <div class="col-md-3">
                                                                                    <label>From Date</label>
                                                                                    <div>
                                                                                        <dxe:ASPxDateEdit ID="ASPxDateEdit23" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDateCancelewaybillTSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-3">
                                                                                    <label>To Date</label>
                                                                                <div>
                                                                                    <dxe:ASPxDateEdit ID="ASPxDateEdit24" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDateCancelewaybillTSI" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                        <ButtonStyle Width="13px">
                                                                                        </ButtonStyle>
                                                                                    </dxe:ASPxDateEdit>

                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3">
                                                                                    <label>Unit</label>
                                                                                <div>
                                                                                    <dxe:ASPxComboBox ID="brCancelewaybillTSI" runat="server" ClientInstanceName="ccmbBranchfilterCancelewaybillTSI" Width="100%">
                                                                                    </dxe:ASPxComboBox>
                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3" style="padding-top:22px">
                                                                                    <% if (rights.CanEWayBill) { %>
                                                                                        <input type="button" value="Show" class="btn btn-success" onclick="updateGridByDateCancelewaybillTSI()" />
                                                                                    <% } %>
                                                                                </div>
                                                                            </div>
                                                                      </div>
                                            <div style="margin-top:14px">
                                               
                                            </div>
                                             
                                            <div class="row">
                                                 <div class="col-md-12">
                                                <dx:LinqServerModeDataSource ID="LinqServerModeDataSourceCancelewaybillTSI" runat="server" OnSelecting="LinqServerModeDataSourceCancelewaybillTSI_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />

                                                <dxe:ASPxGridView ID="ASPxGridView3" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                    Width="100%" ClientInstanceName="cGrdQuotationCancelewaybillTSI" SettingsBehavior-AllowFocusedRow="true" DataSourceID="LinqServerModeDataSourceCancelewaybillTSI"
                                                    SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                    SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
                                                    SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  Styles-SearchPanel-CssClass="searchBoxSmall">
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




                                                       <dxe:GridViewDataTextColumn Caption="IRN" FieldName="Irn" Width="250"
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
                        <div class="filterItem homeFilter" data-toggle="modal" data-target="#messagesFilter"><i class="fa fa-filter" data-toggle="tooltip" data-placement="top" title="Filter"></i></div>
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
                                    <h4>IRN Generated</h4>
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
                                    <h4>IRN Pending</h4>
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
                                    <h4>IRN Cancelled</h4>
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
                                    <a class="nav-link " id="pills-homeSR-tab" data-toggle="pill" href="#pills-homeSR" role="tab" aria-controls="pills-homeSR" aria-selected="true">IRN Generated</a>
                                  </li>
                                  <li class="nav-item">
                                    <a class="nav-link" id="pills-profileSR-tab" data-toggle="pill" href="#pills-profileSR" role="tab" aria-controls="pills-profileSR" aria-selected="false">IRN Pending</a>
                                  </li>
                                  <li class="nav-item">
                                    <a class="nav-link" id="pills-cancelSR-tab" data-toggle="pill" href="#pills-cancelSR" role="tab" aria-controls="pills-cancelSR" aria-selected="false">IRN Cancelled</a>
                                  </li>  
                                     <li class="nav-item">
                                    <a class="nav-link" id="pills-ewaybillSR-tab" data-toggle="pill" href="#pills-ewaybillSR" role="tab" aria-controls="pills-ewaybillSR" aria-selected="false">E-Way bill Generated</a>
                                  </li>
                                <li class="nav-item hide">
                                    <a class="nav-link" id="pills-CancelewaybillSR-tab" data-toggle="pill" href="#pills-CancelewaybillSR" role="tab" aria-controls="pills-CancelewaybillTSI" aria-selected="false">E-Way bill Cancelled</a>
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
                                                <% if (rights.CanIRN) { %>
                                               <button type="button" class="btn btn-dark fontPp hide" data-toggle="modal" onclick="CancelBulkIRNSR()">Cancel Bulk IRN</button>
                                                <button type="button" class="btn btn-Crimson fontPp" onclick="DownloadBulkJSONSR()">Download Bulk IRN</button>
                                                <% } %>
                                                <% if (rights.CanEWayBill) { %>
                                                    <button type="button" class="btn btn-OrangeRed fontPp hide" onclick="UploadEwaybillSR()">Upload Bulk E-Way bill</button>
                                                <% } %>

                                            </div>
                                             
                                            <div class="row">
                                                 <div class="col-md-12">
                                                <dx:LinqServerModeDataSource ID="LinqServerModeDataSourceCR" runat="server" OnSelecting="LinqServerModeDataSourceCR_Selecting"
                                                 ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />
                                                <dxe:ASPxGridView ID="GrdQuotationCR" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                    Width="100%" ClientInstanceName="cGrdQuotationCR" SettingsBehavior-AllowFocusedRow="true" DataSourceID="LinqServerModeDataSourceCR"
                                                    SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                    SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" OnCustomCallback="GrdQuotationCR_CustomCallback"
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
                                                                    <% if (rights.CanIRN)
                                                                             { %>
                                                                     <a href="javascript:void(0);" onclick="DoownLoadJsonSR('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Download JSON</span></a>
                                                                    <% } %>
                                                                    <% if (rights.CanIRN)
                                                                             { %>
                                                                  <a href="javascript:void(0);" onclick="CancelIRNSR('<%# Eval("Irn") %>')" id="a_CancelIRN" class=""   title="">
                                                                            <span class='ico deleteColor'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel IRN</span></a>
                                                                    <% } %>
                                                                    <% if (rights.CanEWayBill)
                                                                             { %>
                                             <a href="javascript:void(0);" onclick="genEwaybillSR('<%# Eval("Irn") %>','<%# Container.KeyValue %>')" id="a_genEwaybillSR" class="" title="">
                                                                            <span class='ico ColorSix'><i class='fa fa-truck' aria-hidden='true'></i></span><span class='hidden-xs'>Generate E-Way Bill</span></a>
                                                                    <% } %>

                                                                    <% if (rights.CanIRN)
                                                                             { %>
                                                                    <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SR', 'IRN_CANCEL','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="CancelIRN" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info</span></a>
                                                                    <% } %>
                                                  <% if (rights.CanEWayBill)
                                                                             { %>
                                            <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SR', 'EWAY_GEN','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRN" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>E way bill Info</span></a>
                                                                    <% } %>


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
                                            <button type="button" class="btn btn-Crimson fontPp" onclick="DownloadBulkJSONPendingSR()">Donload Bulk JSON</button>
                                            <% if (rights.CanIRN) { %>
                                                <button type="button" class="btn btn-OrangeRed fontPp" onclick="UploadBulkIRNTSR()">Upload Bulk IRN</button>
                                            <% } %>
                                        </div>
                                          <div class="row mTop5">
                                                <div class="col-md-12">
                                                    <dx:LinqServerModeDataSource ID="LinqServerModeDataSourcePendingCR" runat="server" OnSelecting="LinqServerModeDataSourcePendingCR_Selecting"
                                                        ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />
                                                    <dxe:ASPxGridView ID="GrdQuotationPendingCR" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                        Width="100%" ClientInstanceName="cGrdQuotationPendingCR" SettingsBehavior-AllowFocusedRow="true" DataSourceID="LinqServerModeDataSourcePendingCR"
                                                        SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                        SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" OnCustomCallback="GrdQuotationPendingCR_CustomCallback"
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
                                                                         <% if (rights.CanIRN)
                                                                             { %>
                                                                         <a href="javascript:void(0);" onclick="DoownLoadJsonSR('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Download JSON</span></a>
                                                                        <% } %>
                                                                         <% if (rights.CanIRN)
                                                                             { %>
                                                                        <a href="javascript:void(0);" onclick="UploadIRNSR('<%# Container.KeyValue %>')" id="a_uploadIRNsi" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Upload IRN</span></a>
                                                                        <% } %>
                                                                         <% if (rights.CanIRN)
                                                                             { %>
                                                                        <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SR', 'IRN_GEN','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="uploadIRNsi" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info</span></a>
                                                                        <% } %>
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
                                     <div class="tab-pane" id="pills-cancelSR" role="tabpanel" aria-labelledby="pills-cancelSR-tab">
                                         <div>
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <label>From Date</label>
                                                    <div>
                                                        <dxe:ASPxDateEdit ID="ASPxDateEdit19" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDateCancelCR" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <label>To Date</label>
                                                <div>
                                                    <dxe:ASPxDateEdit ID="ASPxDateEdit20" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDateCancelCR" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>

                                                </div>
                                                    </div>
                                                <div class="col-md-3">
                                                    <label>Unit</label>
                                                <div>
                                                    <dxe:ASPxComboBox ID="brCancelBR" runat="server" ClientInstanceName="ccmbBranchfilterCancelCR" Width="100%">
                                                    </dxe:ASPxComboBox>
                                                </div>
                                                    </div>
                                                <div class="col-md-3" style="padding-top:22px">
                                                    <input type="button" value="Show" class="btn btn-success" id="SRcancelBut" onclick="updateGridByDateCancelCR()" />
                                                </div>
                                            </div>
                                        </div>

                                          <div class="row mTop5">
                                                <div class="col-md-12">
                                                    <dx:LinqServerModeDataSource ID="LinqServerModeDataSourceCancelCR" runat="server" OnSelecting="LinqServerModeDataSourceCancelCR_Selecting"
                                                        ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />

                                                    <dxe:ASPxGridView ID="GrdQuotationCancelCR" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                        Width="100%" ClientInstanceName="cGrdQuotationCancelCR" SettingsBehavior-AllowFocusedRow="true" DataSourceID="LinqServerModeDataSourceCancelCR"
                                                        SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                        SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
                                                        SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" >
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
                                                                         <% if (rights.CanIRN)
                                                                             { %>
                                                                        <a href="javascript:void(0);" onclick="DoownLoadJson('<%# Container.KeyValue %>')" id="a_editInvoice" class="" title="">
                                                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Download JSON</span></a>
                                                                        <%} %>
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
                                     <div class="tab-pane" id="pills-ewaybillSR" role="tabpanel" aria-labelledby="pills-ewaybillSR-tab">
                                            <div>
                                                                          <div class="row">
                                                                                <div class="col-md-3">
                                                                                    <label>From Date</label>
                                                                                    <div>
                                                                                        <dxe:ASPxDateEdit ID="ASPxDateEdit25" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDateewaybillSR" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-3">
                                                                                    <label>To Date</label>
                                                                                <div>
                                                                                    <dxe:ASPxDateEdit ID="ASPxDateEdit26" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDateewaybillSR" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                        <ButtonStyle Width="13px">
                                                                                        </ButtonStyle>
                                                                                    </dxe:ASPxDateEdit>

                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3">
                                                                                    <label>Unit</label>
                                                                                <div>
                                                                                    <dxe:ASPxComboBox ID="brewaybillSR" runat="server" ClientInstanceName="ccmbBranchfilterewaybillSR" Width="100%">
                                                                                    </dxe:ASPxComboBox>
                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3" style="padding-top:22px">
                                                                                    <input type="button" value="Show" class="btn btn-success" onclick="updateGridByDateewaybillSR()" />
                                                                                </div>
                                                                            </div>
                                                                      </div>
                                            <div style="margin-top:14px">
                                                <%--<button type="button" class="btn btn-dark fontPp" data-toggle="modal" onclick="CancelBulkewaybillSI()">Cancel Bulk E-Way bill</button>  --%>                                              
                                                <%--<button type="button" class="btn btn-Crimson fontPp" onclick="DownloadBulkJSONSI()">Download Bulk E-Way bill</button>--%>
<%--                                                <button type="button" class="btn btn-OrangeRed fontPp">Upload Bulk IRN</button>
                                                <button type="button" class="btn btn-SeaGreen fontPp">Upload Bulk IRN</button>--%>
                                            </div>
                                             
                                            <div class="row">
                                                 <div class="col-md-12">
                                                <dx:LinqServerModeDataSource ID="LinqServerModeDataSourceewaybillSR" runat="server" OnSelecting="LinqServerModeDataSourceewaybillSR_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />

                                                <dxe:ASPxGridView ID="GrdQuotationewaybillSR" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                    Width="100%" ClientInstanceName="cGrdQuotationewaybillSR" SettingsBehavior-AllowFocusedRow="true" DataSourceID="LinqServerModeDataSourceewaybillSR"
                                                    SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                    SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" OnCustomCallback="GrdQuotationewaybillSR_CustomCallback"
                                                    SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" ClientSideEvents-EndCallback="grdEndcallbackewaybillSR" Styles-SearchPanel-CssClass="searchBoxSmall">
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

                                                            <%--<dxe:GridViewDataTextColumn Caption="Type" FieldName="Pos_EntryType" Width="70"
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
                                                            </dxe:GridViewDataTextColumn>--%>

                                                           <%-- <dxe:GridViewDataTextColumn Caption="Delv. Date" FieldName="Pos_DeliveryDate"
                                                                VisibleIndex="8">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>--%>

                                                           <%-- <dxe:GridViewDataTextColumn Caption="Delv. Status" FieldName="DelvStatus"
                                                                VisibleIndex="9">
                                                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                                                </CellStyle>
                                                                <Settings AllowAutoFilterTextInputTimer="False" />
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dxe:GridViewDataTextColumn>--%>

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




                                                       <dxe:GridViewDataTextColumn Caption="IRN" FieldName="Irn" Width="250"
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



                                                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="24" Width="0">
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



                                                        <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="25" Width="0">
                                                            <DataItemTemplate>
                                                                <div class='floatedBtnArea'>
                                                                  <%--  Rev 1.0--%>
                                                                     <% if (rights.CanEWayBill)
                                                                       { %>
                                                                  <a href="javascript:void(0);" onclick="CancelEwayBillSR('<%# Eval("EWayBillNumber") %>')" id="a_CancelEwaybillSI" class="" title="">
                                                                            <span class='ico deleteColor'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel E-Way Bill</span></a>
                                                                    <% } %>
                                                                     <%--  Rev 1.0 End--%>
                                                                     <% if (rights.CanEWayBill)
                                                                       { %>
                                        <a href="javascript:void(0);" onclick="UpdateEwayBillSI('<%# Eval("EWayBillNumber") %>')" id="a_UpdateEwaybillSI" class="" title="">
                                                                            <span class='ico deleteColor'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Update E-Way Bill</span></a>
                                                                     <% } %>
                                                                     <% if (rights.CanEWayBill)
                                                                       { %>
                                           <a href="javascript:void(0);" onclick="UpdateTransporterEwayBillSI('<%# Eval("EWayBillNumber") %>')" id="a_UpdateTransporterEwaybillSI" class="" title="">
                                                                            <span class='ico deleteColor'><i class='fa fa-ban' aria-hidden='true'></i></span><span class='hidden-xs'>Update Transporter</span></a>
                                                                     <% } %>
                                                                     <% if (rights.CanEWayBill)
                                                                       { %>
                                                                     <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SI', 'EWAYBILL_CANCEL','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRNeaycancelSI" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info (Cancel)</span></a>
                                                                     <% } %>
                                                                     <% if (rights.CanEWayBill)
                                                                       { %>
                                            <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SI', 'EWAYBILL_UPDATE','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRNEwayUpdateSI" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info (Update)</span></a>
                                                                     <% } %>
                                                                     <% if (rights.CanEWayBill)
                                                                       { %>
                                            <a href="javascript:void(0);" onclick="ShowInfoN('<%# Container.KeyValue %>', 'SI', 'EWAYBILL_UPDATETR','<%# Eval("Irn") %>', '<%# Eval("InvoiceNo") %>', '<%# Eval("CustomerName") %>',
                                                                          '<%# Eval("InvoiceNo") %>', '<%# Eval("NetAmount") %>', '<%# Eval("AckDt") %>', <%# Eval("AckNo") %>)" id="a_InfoIRNEwayUpdateTrSI" class="" title="">
                                                                            <span class='ico ColorFour'><i class='fa fa-exclamation' aria-hidden='true'></i></span><span class='hidden-xs'>Info (Update Transporter)</span></a>
                                                                     <% } %>
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
                                     <div class="tab-pane" id="pills-CancelewaybillSR" role="tabpanel" aria-labelledby="pills-CancelewaybillSR-tab">
                                            <div>
                                                                          <div class="row">
                                                                                <div class="col-md-3">
                                                                                    <label>From Date</label>
                                                                                    <div>
                                                                                        <dxe:ASPxDateEdit ID="ASPxDateEdit27" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDateCancelewaybillSR" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                            <ButtonStyle Width="13px">
                                                                                            </ButtonStyle>
                                                                                        </dxe:ASPxDateEdit>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-3">
                                                                                    <label>To Date</label>
                                                                                <div>
                                                                                    <dxe:ASPxDateEdit ID="ASPxDateEdit28" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDateCancelewaybillSR" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                                                                        <ButtonStyle Width="13px">
                                                                                        </ButtonStyle>
                                                                                    </dxe:ASPxDateEdit>

                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3">
                                                                                    <label>Unit</label>
                                                                                <div>
                                                                                    <dxe:ASPxComboBox ID="brCancelewaybillSR" runat="server" ClientInstanceName="ccmbBranchfilterCancelewaybillSR" Width="100%">
                                                                                    </dxe:ASPxComboBox>
                                                                                </div>
                                                                                    </div>
                                                                                <div class="col-md-3" style="padding-top:22px">
                                                                                    <% if (rights.CanEWayBill) { %>
                                                                                    <input type="button" value="Show" class="btn btn-success" onclick="updateGridByDateCancelewaybillSR()" />
                                                                                    <% } %>
                                                                                </div>
                                                                            </div>
                                                                      </div>
                                            <div style="margin-top:14px">
                                               
                                            </div>
                                             
                                            <div class="row">
                                                 <div class="col-md-12">
                                                <dx:LinqServerModeDataSource ID="LinqServerModeDataSourceCancelewaybillSR" runat="server" OnSelecting="LinqServerModeDataSourceCancelewaybillSR_Selecting"
                                                    ContextTypeName="ERPDataClassesDataContext" TableName="v_SalesInvoice" />

                                                <dxe:ASPxGridView ID="GrdQuotationCancelewaybillSR" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
                                                    Width="100%" ClientInstanceName="cGrdQuotationCancelewaybillSR" SettingsBehavior-AllowFocusedRow="true" DataSourceID="LinqServerModeDataSourceCancelewaybillSR"
                                                    SettingsBehavior-AllowSelectSingleRowOnly="false" SettingsBehavior-AllowSelectByRowClick="true" Settings-HorizontalScrollBarMode="Auto"
                                                    SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto"
                                                    SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  Styles-SearchPanel-CssClass="searchBoxSmall">
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




                                                       <dxe:GridViewDataTextColumn Caption="IRN" FieldName="Irn" Width="250"
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
        <div class="clearfix"></div>
    </div>

        
        <div class="clearfix"></div>
        <div class="row hide">
            <% if (rights.CanIRN) { %>
            <div class="col-md-3">
                <div>
                    <input type="file" id="flEcel"   />

                    <button type="button" onclick="" class="btn btn-green">Upload Bulk IRN</button>
                </div>
            </div>
            <% } %>
        </div>

        
    </div>

    

    <!-- Modal -->
    <div class="modal pmsModal w30 fade" id="modal1" tabindex="-1" role="dialog" aria-labelledby="modal1Lbl" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="modal1Lbl">Cancel IRN</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">          
              <div class="row">
                  <div class="col-md-12">
                      <label>Cancel Reason</label>
                      <div>
                          <select class="form-control" id="ddlCancelReason">
                              <option value="1">Duplicate</option>
                              <option value="2">Data Entry Mistake</option>

                          </select>
                      </div>
                  </div>
                  <div class="col-md-12">
                      <label>Cancel Remarks</label>
                      <div>
                         <input type="text" class="form-control" placeholder="Enter Remarks" maxlength="100" id="txtCancelRemarks" />
                      </div>
                  </div>
              </div>

          </div>
          <div class="modal-footer">
              <% if(rights.CanIRN) 
                 { %>
            <button type="button" class="btn btn-dark" data-dismiss="modal">Cancel</button>
              <% } %>
              <% if(rights.CanIRN) 
                 { %>
              <button type="button" class="btn btn-SeaGreen" onclick="CancelIRNSubmit();">Submit</button>
              <% } %>
            <%--<button type="button" class="btn btn-SeaGreen" onclick="UploadExcel();">Upload</button>--%>
          </div>
        </div>
      </div>
    </div>

    <asp:HiddenField ID="hdnCancelIRNType" runat="server" />
    <asp:HiddenField ID="hdnCancelIRNNo" runat="server" />
      <div class="modal pmsModal w30 fade" id="modalUpdatePin" tabindex="-1" role="dialog" aria-labelledby="modalUpdatePinLbl" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="modalUpdatePinLbl">PIN to PIN Distance</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">          
              <div class="row">                 
                  <div class="col-md-12">
                      <label>Update Pin</label>
                      <div>
                         <%--<input type="number" class="form-control" placeholder="PIN to PIN Distance" maxlength="10" id="txtPINtoPINDistance" />--%>
                            <dxe:ASPxTextBox ID="txtPINtoPINDistance" runat="server" Width="100%" ClientInstanceName="ctxtPINtoPINDistance">
                            <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />                           
                            </dxe:ASPxTextBox>
                      </div>
                  </div>
              </div>

          </div>
          <div class="modal-footer">
              <% if(rights.CanIRN) 
                 { %>
            <button type="button" class="btn btn-dark" data-dismiss="modal">Cancel</button>
              <% } %>
              <% if(rights.CanIRN) 
                 { %>
              <button type="button" class="btn btn-SeaGreen" onclick="PINtoPINDistanceSubmit();">Submit</button>
              <% } %>
           
          </div>
        </div>
      </div>
    </div>

     <div class="modal pmsModal w30 fade" id="modalUpdatePinTSI" tabindex="-1" role="dialog" aria-labelledby="modalUpdatePinLblTSI" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="modalUpdatePinLblTSI">PIN to PIN Distance</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">          
              <div class="row">                 
                  <div class="col-md-12">
                      <label>Update Pin</label>
                      <div>
                         <%--<input type="number" class="form-control" placeholder="PIN to PIN Distance" maxlength="10" id="txtPINtoPINDistance" />--%>
                            <dxe:ASPxTextBox ID="txtPINtoPINDistanceTSI" runat="server" Width="100%" ClientInstanceName="ctxtPINtoPINDistanceTSI">
                            <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />                           
                            </dxe:ASPxTextBox>
                      </div>
                  </div>
              </div>

          </div>
          <div class="modal-footer">
              <% if(rights.CanIRN) 
                 { %>
            <button type="button" class="btn btn-dark" data-dismiss="modal">Cancel</button>
              <% } %>
              <% if(rights.CanIRN) 
                 { %>
              <button type="button" class="btn btn-SeaGreen" onclick="PINtoPINDistanceSubmitTSI();">Submit</button>
              <% } %>
           
          </div>
        </div>
      </div>
    </div>
    <asp:HiddenField ID="hdnInvoiceId" runat="server" />
 <div class="bcShad "></div> 
 <div class="popupSuc ">
     <div style="background: #467bbd;
    color: #fff;
    text-align: center;
    padding: 7px;font-size: 14px;">Hi <span id="SetLogedUser"></span></div>
     <div class="text-center">
         <span class="cnIcon"><i class="fa fa-check" aria-hidden="true"></i></span>
     </div>
     <div class="bInfoIt">
         <p style="font-size: 15px;color: #e68710;margin-bottom: 10px;">Document has been uploaded successfully to GSTN server.</p>
         <div style="font-size: 14px;color: blue;">
             <table style="width: 90%;margin: auto;">
                 <tr>
                     <td style="width:40px"><b>IRN :</b></td>
                     <td><input type="text" id="IrnNumber" class="noStyle" readonly /></td>
                     <td style="width:40px">
                         <span class="ttip">
                            <button type="button" class="btn" onclick="myFunction()" onmouseout="outFunc()">
                              <span class="tooltiptext" id="myTooltip">Copy to memory</span>
                                <i class="fa fa-clone"></i>
                              </button>
                          </span>
                     </td>
                 </tr>
             </table>  
         </div>
     </div>
     <table class="ppTabl fontSmall">
        <tr>
            <td>Document Number :</td>
            <td><b id="IrnlblInvNUmber"></b></td>
        </tr>
        <tr>
            <td>Customer : </td>
            <td><b id="IrnlblInvDate"></b> </td>
        </tr>
        
        <tr>
            <td>Amount : </td>
            <td><b id="IrnlblAmount"></b></td>
        </tr>
         <tr>
            <td>Ack Number : </td>
            <td><b id="AckNum"></b></td>
        </tr>
         <tr>
            <td>Ack Date : </td>
            <td><b id="AckDate"></b></td>
        </tr>
    </table>
     <div style="text-align: center;padding: 14px;background: antiquewhite;">
         <button class="okbtn btn btn-success" type="button" onclick="rmPop()" style="border-radius: 16px;padding-right: 18px;"><i class="fa fa-check"></i> OK</button>
     </div>
 </div>
 


<div class="modal pmsModal w30 fade" id="EwayBillcancelModal" tabindex="-1" role="dialog" aria-labelledby="modal1Lbl" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="modal1Lbl1">Cancel E-way Bill</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
              <div class="row">
                  <div class="col-md-12">
                      <label>Cancel Reason</label>
                      <div>
                          <select class="form-control" id="ddlEwaybillCancelReason">
                              <option value="1">Duplicate</option>
                              <option value="2">Order Cancelled</option>
                              <option value="3">Data Entry mistake</option>
                              <option value="4">Other</option>
                          </select>
                      </div>
                  </div>
                  <div class="col-md-12">
                      <label>Cancel Remarks</label>
                      <div>
                         <input type="text" class="form-control" placeholder="Enter Remarks" maxlength="100" id="txtEwayCancelRemarks" />
                      </div>
                  </div>
              </div>
                    
          </div>
          <div class="modal-footer">
              <% if(rights.CanEWayBill) 
                 { %>
            <button type="button" class="btn btn-dark" data-dismiss="modal">Cancel</button>
              <% } %>
              <% if (rights.CanEWayBill) 
                 { %>
              <button type="button" class="btn btn-SeaGreen" onclick="CancelEwaSubmit();">Submit</button>
              <% } %>
            <%--<button type="button" class="btn btn-SeaGreen" onclick="UploadExcel();">Upload</button>--%>
          </div>
        </div>
      </div>
    </div>

    <asp:HiddenField ID="hdnEwayBillType" runat="server" />
    <asp:HiddenField ID="hdnEwayBillNo" runat="server" />

</asp:Content>
