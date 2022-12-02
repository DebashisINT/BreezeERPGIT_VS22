<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="WarrantyEntry.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.Warranty.WarrantyEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/css/select2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/select2@4.0.13/dist/js/select2.min.js"></script>


    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,800;0,900;1,600&display=swap" rel="stylesheet" />
    <link href="/assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />
    <script src="/assests/pluggins/datePicker/bootstrap-datepicker.js"></script>
    <link href="/assests/pluggins/Select2/select2.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/Select2/select2.min.js"></script>
    <link href="/assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <script src="/assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>

    <link href="../CSS/WarrantyEntry.css" rel="stylesheet" />
    <script src="../JS/WarrantyEntry.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3 id="DivHeader" runat="server"><%--Receipt Challan--%>
                Add Warranty
            </h3>
        </div>
        <div id="divcross" class="crossBtn"><a href="WarrantyList.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <div class="pmsForm slick boxModel clearfix">
            <div class="row" style="margin-top: 5px">
                <div class="col-md-2 ">
                    <label>Serial No</label>
                    <div>
                        <input type="text" id="txtSerialNo" class="form-control" maxlength="16" runat="server" />
                    </div>
                </div>
                <div class="col-md-2">
                    <div style="padding-top: 19px">
                        <button type="button" class="btn btn-success" id="btnSearchSerial" runat="server" onclick="Search_Click();">Search</button>
                    </div>
                </div>
            </div>
            <div style="border-top: 1px solid #afaaaa; margin-top: 5px;">
                <div class="row" style="margin-top: 5px">
                    <div class="col-md-3 col-lg-2">
                        <label>Receipt Challan No</label>
                        <div>
                            <input type="text" id="txtReceiptChallanNo" class="form-control" disabled runat="server" />
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-2">
                        <label>Date</label>
                        <div>
                            <input type="text" id="txtDate" class="form-control" disabled runat="server" />
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-2">
                        <label>Entity Code</label>
                        <div>
                            <input type="text" id="txtEntityCode" class="form-control" disabled runat="server" />
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-4">
                        <label>Network Name</label>
                        <div>
                            <input type="text" id="txtNetworkName" class="form-control" disabled runat="server" />
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-2">
                        <label>Serial No</label>
                        <div>
                            <input type="text" id="txtdtlsSerialNo" class="form-control" disabled runat="server" />
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-2">
                        <label>New Serial No</label>
                        <div>
                            <input type="text" id="txtNewSerialNo" class="form-control" disabled runat="server" />
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-2">
                        <label>Warranty Status</label>
                        <div>
                            <input type="text" id="txtWarrantyStatus" class="form-control" disabled runat="server" />
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-2">
                        <label>Warranty Date</label>
                        <div>
                            <input type="text" id="txtWarrantyDate" class="form-control" disabled runat="server" />
                        </div>
                    </div>
                    <div class="col-md-6">
                        <label>Problem Found</label>
                        <div>
                            <textarea class="form-control" id="txtProblemFound" rows="3" style="height: 45px !important" disabled runat="server"></textarea>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="pmsForm slick boxModel clearfix" style="margin-top: 10px">
            <div class="row" style="margin-top: 5px">
                <div class="col-md-2">
                    <label>Update Warranty <span class="red">*</span></label>
                    <div class="relative">
                        <div class="input-group date">
                            <%--Mantis Issue 24290  [ onchange="WarrantyChange();" onblur="WarrantyChange();"  added  ] --%>
                            <input type="text" id="dtUpdateWarranty" class="form-control" style="height: 28px !important" runat="server" onchange="WarrantyChange();" onblur="WarrantyChange();" />
                            <div class="input-group-addon">
                                <span class="fa fa-calendar-check-o"></span>
                            </div>
                        </div>
                    </div>
                    <%-- <div>
                        <input type="text" class="form-control"id="dtUpdateWarranty" />
                    </div>--%>
                </div>
                <%--Mantis Issue 24290--%>
                <div class="col-md-2">
                    <label>Warranty Status <span class="red">*</span></label>
                    <div>
                        <select class="form-control" id="ddlWarrentyStatus" name="ddlWarrentyStatus" runat="server" >
                            <option value="0">Select</option>
                            <option value="1">In Warranty</option>
                            <option value="2">Out warranty</option>
                        </select>
                    </div>
                </div>
                <%--End of Mantis Issue 24290--%>
                <div class="col-md-6">
                    <label>Remarks <span class="red">*</span></label>
                    <div>
                        <textarea class="form-control" rows="3" style="height: 75px !important;" id="txtRemarks" runat="server"></textarea>
                    </div>
                </div>

            </div>
        </div>
        <div class="clearfix " style="margin-top: 10px">
            <% if (rights.CanAdd)
               { %>
            <button type="button" id="btnUpdateWarranty" runat="server" class="btn btn-primary" onclick="UpdateWarranty();">Update</button>
            <%} %>
            <button type="button" class="btn btn-danger">Cancel</button>
        </div>
        <input type="hidden" id="hdnReceiptChallanID" runat="server" />
        <input type="hidden" id="hdnEntryID" runat="server" />
        <input type="hidden" id="hdnEntryDtlsId" runat="server" />
        <input type="hidden" id="hdnWarrantyUpdateId" runat="server" />
        <input type="hidden" id="hdnAddEditAction" runat="server" />
        <input type="hidden" id="hdnWarrantyForEdit" runat="server" />
    </div>
</asp:Content>
