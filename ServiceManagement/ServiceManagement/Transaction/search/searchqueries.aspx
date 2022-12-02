<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="searchqueries.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.search.searchqueries" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,800;0,900;1,600&display=swap" rel="stylesheet" />
    <link href="/assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />

    <script src="/assests/pluggins/datePicker/bootstrap-datepicker.js"></script>
    <link href="/assests/pluggins/Select2/select2.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/Select2/select2.min.js"></script>
    <link href="/assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <script src="/assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>
    <script src="/assests/pluggins/bootstrap-multiselect/bootstrap-multiselect.min.js"></script>

    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/dataTables.buttons.min.js"></script>
    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/buttons.flash.min.js"></script>
    <script src="/assests/pluggins/DataTable/JSZip-2.5.0/jszip.min.js"></script>
    <script src="/assests/pluggins/DataTable/pdfmake-0.1.36/pdfmake.min.js"></script>
    <script src="/assests/pluggins/DataTable/pdfmake-0.1.36/vfs_fonts.js"></script>
    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/buttons.html5.min.js"></script>
    <script src="/assests/pluggins/DataTable/Buttons-1.6.2/js/buttons.print.min.js"></script>

    <link href="../CSS/search.css" rel="stylesheet" />
    <script src="../JS/search.js?v=1.0.0.27"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%-- <div class="clearfix">
        <button type="button" class="btn btn-success" data-toggle="modal" data-target="#detailsModalJobsheet">View Jobsheet</button>
        <button type="button" class="btn btn-success" data-toggle="modal" data-target="#detailsModalDelivery">View Delivery</button>
        <button type="button" class="btn btn-success" data-toggle="modal" data-target="#detailsModalServiceEntry">View Service Entry</button>
    </div>--%>
    <div class="modal fade pmsModal w90" id="detailsModalServiceEntry" tabindex="-1" role="dialog" aria-labelledby="detailsModalJobsheet" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="">View Service Entry</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="clearfix row">
                        <div class="col-md-2">
                            <label>Challan No </label>
                            <div>
                                <input type="text" id="SrvEntryChallanNo" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Entity Code </label>
                            <div>
                                <input type="text" id="SrvEntryEntityCode" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Network Name</label>
                            <div>
                                <input type="text" id="SrvEntryNetworkName" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Contact Person</label>
                            <div>
                                <input type="text" id="SrvEntryContactPerson" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Received On</label>
                            <div>
                                <input type="text" id="SrvEntryReceivedOn" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Received By</label>
                            <div>
                                <input type="text" id="SrvEntryReceivedBy" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix row">
                        <div class="col-md-2">
                            <label>Assigned To</label>
                            <div>
                                <input type="text" id="SrvEntryAssignedTo" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Assigned By</label>
                            <div>
                                <input type="text" id="SrvEntryAssignedBy" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Assigned On</label>
                            <div>
                                <input type="text" id="SrvEntryAssignedOn" class="form-control" disabled />
                            </div>
                        </div>

                    </div>

                    <hr />
                    <div class="clearfix ">
                        <div id="SrvEntryTable">
                            <table id="dataTable-ServiceEntry" class="table table-striped table-bordered display nowrap" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>Model No</th>
                                        <th>Serial No</th>
                                        <th>Problem Reported</th>
                                        <th>Service Action</th>
                                        <th>Components</th>
                                        <th>Warranty</th>
                                        <th>Stock Entry</th>
                                        <th>New Model</th>

                                        <th>New Serial No</th>
                                        <th>Problem Found</th>
                                        <th>Remarks </th>
                                        <th>Warranty Status</th>
                                        <th>Return Reason</th>
                                        <th>Billable </th>

                                    </tr>
                                </thead>
                                <%-- <tbody>
                                    <tr>
                                        <td>154610</td>
                                        <td>Technician name</td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>10.2.21</td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                    </tr>

                                </tbody>--%>
                            </table>
                        </div>
                    </div>
                    <br />
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade pmsModal w90" id="detailsModalDelivery" tabindex="-1" role="dialog" aria-labelledby="detailsModalJobsheet" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="">View Delivery</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="clearfix row">
                        <div class="col-md-2">
                            <label>Challan No </label>
                            <div>
                                <input type="text" id="DelChallanNo" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Entity Code </label>
                            <div>
                                <input type="text" id="DelEntityCode" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Network Name</label>
                            <div>
                                <input type="text" id="DelNetworkName" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Contact Person</label>
                            <div>
                                <input type="text" id="DelContactPerson" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Received On</label>
                            <div>
                                <input type="text" id="DelReceivedOn" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Received By</label>
                            <div>
                                <input type="text" id="DelReceivedBy" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix row">
                        <div class="col-md-2">
                            <label>Assigned To</label>
                            <div>
                                <input type="text" id="DelAssignedTo" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Assigned By</label>
                            <div>
                                <input type="text" id="DelAssignedBy" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Assigned On</label>
                            <div>
                                <input type="text" id="DelAssignedOn" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Delivered To</label>
                            <div>
                                <input type="text" id="DelDeliveredTo" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Phone No</label>
                            <div>
                                <input type="text" id="DelPhoneNo" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Remarks</label>
                            <div>
                                <input type="text" id="DelRemarks" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="clearfix ">
                        <div id="DeliveryTable">
                            <table id="dataTable-Delivery" class="table table-striped table-bordered display nowrap" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>Device Type</th>
                                        <th>Model No</th>
                                        <th>Serial No</th>
                                        <th>Problem found</th>
                                        <th>Service Action</th>

                                        <th>Warranty</th>
                                        <th>AC Cord / Adapter	</th>
                                        <th>Remote </th>


                                    </tr>
                                </thead>
                                <%--<tbody>
                                    <tr>
                                        <td>154610</td>
                                        <td>Technician name</td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>10.2.21</td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                    </tr>

                                </tbody>--%>
                            </table>
                        </div>
                    </div>
                    <br />
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade pmsModal w90" id="detailsModalJobsheet" tabindex="-1" role="dialog" aria-labelledby="detailsModalJobsheet" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="">View Jobsheet</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="clearfix row">
                        <div class="col-md-2">
                            <label>Challan Number </label>
                            <div>
                                <input type="text" id="jobChallanNumber" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Posting Date </label>
                            <div>
                                <input type="text" id="jobPostingDate" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Ref Jobsheet</label>
                            <div>
                                <input type="text" id="jobRefJobsheet" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Assign To</label>
                            <div>
                                <input type="text" id="jobAssignTo" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Work done on</label>
                            <div>
                                <input type="text" id="jobWorkDoneOn" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Location</label>
                            <div>
                                <input type="text" id="jobLocation" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix row">
                        <div class="col-md-4">
                            <label>Remarks</label>
                            <div>
                                <input type="text" id="jobRemarks" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Entity Code </label>
                            <div>
                                <input type="text" id="jobEntityCode" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Network Name </label>
                            <div>
                                <input type="text" id="jobNetworkName" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Contact Person </label>
                            <div>
                                <input type="text" id="jobContactPerson" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Contact Number </label>
                            <div>
                                <input type="text" id="jobContactNumber" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix row">
                    </div>
                    <hr />
                    <div class="clearfix ">
                        <div id="JobSheetTable">
                            <table id="dataTable-jobsheetView" class="table table-striped table-bordered display nowrap" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>Serial Number</th>
                                        <th>Device Type</th>
                                        <th>Model</th>
                                        <th>Problem found</th>
                                        <th>Other</th>
                                        <th>Service Action</th>
                                        <th>Components</th>
                                        <th>Warranty</th>
                                        <th>Return reason</th>
                                        <th>Remarks </th>
                                        <th>Billable </th>

                                    </tr>
                                </thead>
                                <%-- <tbody>
                                    <tr>
                                        <td>154610</td>
                                        <td>Technician name</td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>10.2.21</td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                    </tr>

                                </tbody>--%>
                            </table>
                        </div>
                    </div>
                    <br />
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <%--Mantis Issue 24781--%>
    <div class="modal fade pmsModal w30" id="assignPhnNo" role="dialog" aria-labelledby="assignpop" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    
                    <h5 class="modal-title">Enter Phone Number</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    
                </div>
                <div class="modal-body">
                    <div class="row ">
                        <%--<div class="col-md-12">
                            <label class="deep">Choose Technician</label>
                            <div class="fullWidth" id="DivTechnician">

                            </div>
                        </div>--%>
                        <div class="col-md-12 mTop5">
                            <label class="deep">Phone No </label>
                            <div class="fullWidth">
                                <%--<textarea class="form-control" id="txtRemarks" maxlength="10" ></textarea>--%>
                                <input type="text" id="txtPhoneNo" maxlength="10" class="form-control" placeholder="Enter Phone No" onkeypress="return onlyNumbers(event)" ondrop="return false;" onpaste="return false;" />
                                <input type="hidden" id="hdnReceiptChallan_ID" />
                                <input type="hidden" id="hdnModule_ID" />
                            </div>
                        </div>
                    </div>
                    <%--<asp:HiddenField ID="hdnReceipt_ID" runat="server" />--%>
                </div>
                <div class="modal-footer" id="divsave">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                   
                    <button type="button" class="btn btn-success" onclick="PhoneNoSend();">Confirm</button>
                    
                </div>
            </div>
        </div>
    </div>
    <%--End of Mantis Issue 24781--%>
    <!-- Modal -->
    <div class="modal fade pmsModal w90" id="detailsModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">View Receipt Challan</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="clearfix row">
                        <div class="col-md-3">
                            <label>Document Number <span class="red">*</span></label>
                            <div>
                                <input type="text" id="txtDocumentNumber" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Location <span class="red">*</span></label>
                            <div>
                                <input type="text" id="txtLocation" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Date <span class="red">*</span></label>
                            <div>
                                <input type="text" id="txtDate" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Technician</label>
                            <div>
                                <input type="text" id="txtTechnician" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix row">
                        <div class="col-md-3">
                            <label>Entity Code <span class="red">*</span></label>
                            <div>
                                <input type="text" id="EntityCode" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Network Name <span class="red">*</span></label>
                            <div>
                                <input type="text" id="NetworkName" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Contact Person <span class="red">*</span></label>
                            <div>
                                <input type="text" id="ContactPerson" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Contact Number <span class="red">*</span></label>
                            <div>
                                <input type="text" id="ContactNumber" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="clearfix ">
                        <div id="divReceiptChallanDtls">
                            <%-- <table id="dataTable2" class="table table-striped table-bordered display nowrap" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>Device Type</th>
                                        <th>Model</th>
                                        <th>Serial Number</th>
                                        <th>Warranty</th>
                                        <th>Problem</th>
                                        <th>Remarks </th>
                                        <th>Remote </th>
                                        <th>Card/Adaptor</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>154610</td>
                                        <td>Technician name</td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                        <td>10.2.21</td>
                                        <td>asfasf </td>
                                        <td>asfasf </td>
                                    </tr>

                                </tbody>
                            </table>--%>
                        </div>
                    </div>
                    <br />
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Search</h3>
        </div>
    </div>
    <div class="form_main pTop5">
        <div class="clearfix">
            <div class="row">
                <div class="col-md-12">
                    <div class="backBox clearfix font-pp " style="padding-top: 8px;">
                        <div class="col-md-2">
                            <label>Type</label>
                            <div class="relative">
                                <select id="idType" class="form-control">
                                    <option value="0">Select</option>
                                    <option value="1">Receipt challan</option>
                                    <option value="2">Serial No</option>
                                    <option value="3">Location</option>
                                    <option value="4">Serial Tracking</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2" id="rcn">
                            <label>Receipt challan No</label>
                            <div class="relative">
                                <input type="text" id="ReceiptchallanNo" class="form-control" />
                            </div>
                        </div>
                        <div class="col-md-2" id="sln">
                            <label>Serial No</label>
                            <div class="relative">
                                <input type="text" id="txtSerialNo" class="form-control" />
                            </div>
                        </div>
                        <div class="col-md-2" id="lct">
                            <label>Location</label>
                            <div class="relative" id="locationMulti">

                                <dxe:ASPxCallbackPanel runat="server" ID="BranchPanel" ClientInstanceName="cBranchPanel" OnCallback="Componentbranch_Callback">
                                    <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_branch" SelectionMode="Multiple" runat="server" ClientInstanceName="gridbranchLookup"
                                OnDataBinding="lookup_branch_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" width="200px" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="2" width="200px" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
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
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllBranch" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllBranch" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseBranchLookup" UseSubmitBehavior="False" />
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
                        </dxe:PanelContent>
                    </panelcollection>
                                </dxe:ASPxCallbackPanel>

                                <asp:HiddenField ID="hdnActivityTypeText" runat="server" />
                                <span id="MandatoryActivityType" style="display: none" class="validclass">
                                    <img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                            </div>
                        </div>
                        <div class="col-md-2" id="slnTrack">
                            <label>Serial No</label>
                            <div class="relative">
                                <input type="text" id="txtSerialTrack" class="form-control" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>&nbsp</label>
                            <div>
                                <button class="btn btn-success" type="button" onclick="SearchSingleClick();" style="margin-top: -1px; margin-bottom: 8px;">Search</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="clearfix">
            <div id="filterToggle" class=" boxModel clearfix mBot10 mTop5 font-pp" style="background: #f9f9f9">
                <span class="togglerSlidecut"><i class="fa fa-times-circle"></i></span>
                <div class="row">
                    <div class="col-md-3">
                        <label>Receipt challan No</label>
                        <div class="relative">
                            <input type="text" id="txtReceiptchallanNo" class="form-control" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>Entity Code</label>
                        <div class="relative">
                            <input type="text" id="txtEntityCode" class="form-control" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>Location</label>
                        <div class="relative">
                            <%--  <asp:DropDownList ID="ddlBranch" runat="server" CssClass="js-example-basic-single" DataTextField="branch_description" DataValueField="branch_id" Width="100%"
                                meta:resourcekey="ddlBranchResource1">
                            </asp:DropDownList>--%>
                            <dxe:ASPxCallbackPanel runat="server" ID="LocationPanel" ClientInstanceName="cLocationPanel" OnCallback="Location_Callback">
                                <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_Location" SelectionMode="Multiple" runat="server" ClientInstanceName="gridLocationLookup"
                                OnDataBinding="lookup_Location_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="branch_code" Visible="true" VisibleIndex="1" width="200px" Caption="Branch code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="branch_description" Visible="true" VisibleIndex="2" width="200px" Caption="Branch Name" Settings-AutoFilterCondition="Contains">
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
                                                            <dxe:ASPxButton ID="ASPxButton9" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllLocation" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton10" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllLocation" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="ASPxButton11" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseLoactionLookup" UseSubmitBehavior="False" />
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
                        </dxe:PanelContent>
                    </panelcollection>
                            </dxe:ASPxCallbackPanel>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <label>&nbsp</label>
                        <div>
                            <button class="btn btn-success" style="margin-top: 7px;" type="button" onclick="SearchClick();">Search</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="" id="LodingId">
            <div class="clearfix font-pp">
                <div class="relative">
                    <span class="togglerSlide btn btn-warning" style="position: absolute; right: 337px; z-index: 10;" data-toggle="tooltip" data-placement="top" title="Filter" id="spnfilter"><i class="fa fa-filter"></i></span>
                    <% if (rights.CanExport)
                       { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-export" onchange="ExportChange();" Style="position: absolute; right: 243px; z-index: 10;">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <%--<asp:ListItem Value="1">PDF</asp:ListItem>--%>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <%--<asp:ListItem Value="4">CSV</asp:ListItem>--%>
                    </asp:DropDownList>
                    <%} %>
                    <div id="DivDeliveryDetails">
                        <table id="dataTable" class="table table-striped table-bordered display nowrap" style="width: 100%">
                            <thead>
                                <tr>
                                    <th>Receipt Challan</th>
                                    <th>Type</th>
                                    <th>Entity Code </th>
                                    <th>Network Name</th>
                                    <th>Contact Person</th>
                                    <th>Technician </th>
                                    <th>Location </th>
                                    <th>Received by</th>
                                    <th>Received on</th>
                                    <th>Assigned by</th>
                                    <th>Assigned on</th>
                                    <th>Serv. Entered By</th>
                                    <th>Serv. Entered On</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <%--<tbody>
                        <tr>
                            <td>154610</td>
                                   
                            <td>Technician name</td>
                            <td>asfasf </td>
                            <td>asfasf </td>
                            <td>asfasf </td>
                            <td>10.2.21</td>
                            <td>asfasf </td>
                            <td>asfasf </td>
                            <td>10.2.21</td>
                            <td>Assigned by</td>
                            <td>Assigned on</td>       
                            <td>Assigned on</td>
                            <td>Assigned on</td>
                            <td class="actionInput text-center">
                                <span><i class="fa fa-pencil-square-o assig" data-toggle="tooltip" data-placement="bottom" title="Edit" ></i> </span>
                                <span><i class="fa fa-print det" data-toggle="tooltip" data-placement="bottom" title="Print" ></i> </span>
                            </td>
                        </tr>
                       
                    </tbody>--%>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnUserType" runat="server" />

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="LodingId"
        Modal="True">
    </dxe:ASPxLoadingPanel>
</asp:Content>
