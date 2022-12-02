<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="DeliveryEntry.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.Delivery.DeliveryEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,800;0,900;1,600&display=swap" rel="stylesheet" />
    <link href="/assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />
    <script src="/assests/pluggins/datePicker/bootstrap-datepicker.js"></script>
    <link href="/assests/pluggins/Select2/select2.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/Select2/select2.min.js"></script>
    <link href="/assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <%--<script src="/assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>--%>
    <script src="//cdn.datatables.net/plug-ins/1.10.21/features/scrollResize/dataTables.scrollResize.min.js"></script>
    <link href="../CSS/DeliveryEntry.css" rel="stylesheet" />
<%--    <link href="../../../assests/css/custom/commonService.css" rel="stylesheet" />--%>
    <script src="../JS/DeliverEntry.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <span class="hddd font-pp" id="HeaderName" runat="server">Add Delivery </span>
    <div id="divcross" class="crossBtn pull-right" style="display:!inline-block;"><a href="DeliveryList.aspx"><i class="fa fa-times"></i></a></div>
    <div class="clearfix font-pp relative pTop10">
        <div class="colorHeaderType">
            <table>
                <thead>
                    <tr>
                        <th>Challan No</th>
                        <th>Entity Code</th>
                        <th>Network Name</th>
                        <th>Contact Person</th>
                        <th>Received On</th>
                        <th>Received By</th>
                        <th>Assigned To</th>
                        <th>Assigned By</th>
                        <th>Assigned On</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td id="tdChallanNo"></td>
                        <td id="tdEntityCode"></td>
                        <td id="tdNetworkName"></td>
                        <td id="tdContactPerson"></td>
                        <td id="tdReceivedOn"></td>
                        <td id="tdReceivedBy"></td>
                        <td id="tdAssignedTo"></td>
                        <td id="tdAssignedBy"></td>
                        <td id="tdAssignedOn"></td>
                    </tr>
                </tbody>
            </table>
        </div>
        
        <div class="repeatedDs " style="margin-top: 5px;">

            <div class="pmsForm  newColorBox clearfix mTop5 font-pp" style="background: rgb(245, 248, 255); margin-bottom: 5px">
                <div style="padding:0 15px;">
                    <div class="row">
                        <div class="col-md-3">
                            <label>Delivered To <span class="red">*</span></label>
                            <div>
                                <input type="text" maxlength="300" value="" id="txtDeliveredTo" class="form-control" />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Phone No  <span class="red">*</span></label>
                            <div>
                                <input type="text" maxlength="15" value="" id="txtPhoneNo" class="form-control" />
                            </div>
                        </div>
                        <div class="col-md-6 lblmTop8">
                            <label class="">Remarks <%--<span class="red">*</span>--%></label>
                            <div class="relative">
                                <input type="text" value="" maxlength="500" id="txtRemarks" class="form-control" />
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-3">
                            <div class="checkbox" style="margin-top: 20px">
                                <input type="checkbox" id="chkReceiptChallan" value="" style="width: 20px;height: 20px;" />
                                <label for="optinosCheckbox1">
                                    Receipt challan not received
                                </label>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <label >Receipt Challan Remarks</label>
                            <div>
                                <input type="text" value="" maxlength="500" id="txtChallanRemarks" class="form-control" />
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div style="margin-bottom: 6px;">Attach Document</div>
                            <div>
                                <input type="file" class="form-control-file" id="ReceiptChallanDoc" />
                                
                            </div>
                        </div>
                        <div class="col-md-2" style="padding-top:23px">
                            <button type="button" id="btnView" onclick="AttachmentView()" class="btn btn-default hide">View</button>
                        </div>
                        <div class="clear"></div>

                    </div>
                </div>
            </div>
            
            <div id="DivDetailsTable">
                
            </div>
        </div>
        <div>
            <div style="padding-top: 10px;" id="divsave" runat="server">
                <button class="btn btn-success" type="button" id="btnSaveExit" runat="server" onclick="OnSubmit();">Save & Exit</button>
                <button class="btn btn-danger" type="button" onclick="Cancel();">Cancel</button>
            </div>
        </div>
    </div>



    <div class="modal fade pmsModal w50" id="srrHist" tabindex="-1" role="dialog" aria-labelledby="srrHist" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Service History</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row ">
                        <div class="col-md-12">
                            <table class="table table-striped table-bordered tableStyle">
                                <thead>
                                    <tr>
                                        <th>Entity Code</th>
                                        <th>Ref. Receipt No.</th>
                                        <th>Service Action</th>
                                        <th>Remarks</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>sasgasg</td>
                                        <td>sasgasg</td>
                                        <td>sasgasg</td>
                                        <td>sasgasg</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-success">OK</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Attachment Modal-->
    <div class="modal fade" id="AttachmentFilesModal" role="dialog">
    <div class="modal-dialog">
        <div class="modal-content" id="">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title">View Document</h4>
            </div>
            <div class="modal-body" id="attachmentbody">
                <div class="imageViewerWrap text-center">
                    <img src="" alt="Image no found"  id="imageview" class="responsive-image" />
                </div>
                <%--@*<embed src="pdfFiles/interfaces.pdf" id="pdfview" width="600" height="500" alt="pdf" pluginspage="">*@--%>
                <div class="pdfViewerWrap">
                    <object data="" type="application/pdf" id="pdfview" height="600"></object>
                </div>
                
                
                

            </div>
        </div>
    </div>
</div>

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divsave"
        Modal="True">
    </dxe:ASPxLoadingPanel>
    <%--<input type="hidden" value="@Url.Action("AttachmentDocumentAddUpdate", "SRVFileuploadDelivery")" id="hdnAttachmentAddUpdate" />--%>
    <asp:HiddenField ID="hdnDocumentType" runat="server" />
    <asp:HiddenField ID="hdnUserType" runat="server" />
    <asp:HiddenField ID="hdnReceiptChallanID" runat="server" />
    <asp:HiddenField ID="hdnDeliveryId" runat="server" />
    <asp:HiddenField ID="hdnAttachmentFile" runat="server" />
    <asp:HiddenField runat="server" ID="hdnOnlinePrint" />
</asp:Content>
