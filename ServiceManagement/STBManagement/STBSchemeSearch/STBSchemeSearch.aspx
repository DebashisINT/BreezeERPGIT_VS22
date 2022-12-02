<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="STBSchemeSearch.aspx.cs" Inherits="ServiceManagement.STBManagement.STBSchemeSearch.STBSchemeSearch" %>
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
    <link href="../CSS/Stbsearch.css" rel="stylesheet" />
    <script src="../JS/STBSchemeSearch.js"></script>
    <style>
        /* for pop */
        .popupWraper {
            position: fixed;
            top: 0;
            left: 0;
            height: 100vh;
            width: 100%;
            background: rgba(0,0,0,0.85);
            z-index: 10;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .popBox {
            width: 670px;
            background: #fff;
            padding: 35px;
            text-align: center;
            min-height: 350px;
            display: flex;
            align-items: center;
            flex-direction: column;
            justify-content: center;
            background: #fff url("/assests/images/popupBack.png") no-repeat top left;
            box-shadow: 0px 14px 14px rgba(0,0,0,0.56);
        }

            .popBox h1, .popBox p {
                font-family: 'Poppins', sans-serif !important;
                margin-bottom: 20px !important;
            }

            .popBox p {
                font-size: 15px;
            }

        .btn-sign {
            background: #3680fb;
            color: #fff;
            padding: 10px 25px;
            box-shadow: 0px 5px 5px rgba(0,0,0,0.22);
        }

            .btn-sign:hover {
                background: #2e71e1;
                color: #fff;
            }
    </style>
     <style>
        .mtop8 {
            margin-top: 8px;
        }

        .ptTbl > tbody > tr > td {
            padding-right: 10px;
            padding-bottom: 8px;
        }

        .headerPy {
            background: #66b1c7;
            /* display: inline-block; */
            padding: 4px 10px;
            /* transform: translate(-4px); */
            border-radius: 5px 5px 0 0;
            /* border: 1px solid #858eb7; */
            font-weight: 500;
            color: #f1f1f1;
            margin-top: 2px;
            margin-left: 19px; 
            margin-right: 19px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="popupWraper hide" id="divPopHead" runat="server">
        <div class="popBox">
            <img src="/assests/images/warningAlert.png" class="mBot10" style="width: 70px;" />
            <h1 id="h1heading" class="red">Your Access is Denied</h1>
            <p id="pParagraph" class="red">
                You can access this section starting from <span id="spnbegin"></span>upto <span id="spnEnd"></span>
            </p>
            <button type="button" class="btn btn-sign" onclick="WorkingRosterClick()">OK</button>
        </div>
    </div>

        
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Search</h3>
        </div>
    </div>
    <div class="form_main">

        <div class="clearfix">
            <div class="row">
                <div class="col-md-12">
                    <div class="backBox clearfix font-pp " style="padding-top: 8px;">
                        <div class="col-md-2">
                            <label>Type</label>
                            <div class="relative">
                                <select id="idTypes" class="form-control">
                                    <option value="0">Select</option>
                                    <option value="1">Document No</option>
                                    <option value="2">Entity</option>
                                    <option value="3">Location</option>
                                    <option value="4">Date</option>
                                    <option value="5">Serial No</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2" id="rcn">
                            <label>Document No</label>
                            <div class="relative">
                                <input type="text" id="DocumentsNo" class="form-control" />
                            </div>
                        </div>
                        <div class="col-md-2" id="sln">
                            <label>Entity</label>
                            <div class="relative">
                                <input type="text" id="txtEntityNo" class="form-control" />
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
                        <div id="slDate">
                            <div class="col-md-2">
                                <label>From Date</label>
                                <div class="relative">
                                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                                        <buttonstyle width="13px">
                                        </buttonstyle>
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <label>To Date</label>
                                <div class="relative">
                                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                                        <buttonstyle width="13px">
                                        </buttonstyle>
                                    </dxe:ASPxDateEdit>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2" id="rsrl">
                            <label>Serial No</label>
                            <div class="relative">
                                <input type="text" id="txtSerialNo" class="form-control" />
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
                        <label>Document No</label>
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
                    <%-- <% if (rights.CanExport)
                       { %>--%>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-export" onchange="ExportChange();" Style="position: absolute; right: 243px; z-index: 10;">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <%--<asp:ListItem Value="1">PDF</asp:ListItem>--%>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <%--<asp:ListItem Value="4">CSV</asp:ListItem>--%>
                    </asp:DropDownList>
                    <%--  <%} %>--%>
                    <div id="DivDeliveryDetails">
                        <table id="dataTable" class="table table-striped table-bordered display nowrap" style="width: 100%">
                            <thead>
                                <tr>
                                    <th>Document No.</th>
                                    <th>Document Date</th>
                                    <th>Type</th>
                                    <th>Entity Code </th>
                                    <th>Network Name</th>
                                    <th>Contact Person</th>
                                    <th>Contact No </th>
                                    <th>Location </th>
                                    <th>Entered By</th>
                                    <th>Entered On</th>
                                    <th>Status</th>
                                    <th>Cancel Reason</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                          
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
        <asp:SqlDataSource ID="gridStatusDataSource" runat="server"
            SelectCommand="">
            <%--  <SelectParameters>
                <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
            </SelectParameters>--%>
        </asp:SqlDataSource>
        <asp:HiddenField ID="hdnUserType" runat="server" />
        <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="LodingId"
            Modal="True">
        </dxe:ASPxLoadingPanel>
        <asp:HiddenField runat="server" ID="hdnIsCancelReceipt" />
        <asp:HiddenField runat="server" ID="hdnIsCancelWallet" />
    </div>

     <div class="modal fade pmsModal w90" id="detailsSTBReqModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleSTBReqModalLabel">View STB Requisition</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="clearfix row">
                        <div class="col-md-2">
                            <label>Document Number </label>
                            <div>
                                <input type="text" id="txtDocumentNumberSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Location </label>
                            <div>
                                <input type="text" id="txtLocationSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Date </label>
                            <div>
                                <input type="text" id="txtDateSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Requisition Type</label>
                            <div>
                                <input type="text" id="txtRequisitionTypeSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Requisition For</label>
                            <div>
                                <input type="text" id="txtRequisitionForSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="clearfix row">
                        <div class="col-md-2">
                            <label>Entity Code </label>
                            <div>
                                <input type="text" id="txtEntityCodeSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3" style="margin-top:-9px;">
                            <label>Network Name </label>
                            <div>
                                <input type="text" id="txtNetworkNameSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3" style="margin-top:-9px;">
                            <label>Contact Person </label>
                            <div>
                                <input type="text" id="txtContactPersonSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Contact Number </label>
                            <div>
                                <input type="text" id="txtContactNumberSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>DAS </label>
                            <div>
                                <input type="text" id="txtDASSTBReq" class="form-control" disabled />
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="clearfix ">
                        <div id="divSTBReqRechargeDtls">
                        </div>
                    </div>
                    <br />
                    <div class="clearfix ">
                        <div id="divSTBReqRechargeDtls2">
                        </div>
                    </div>
                </div>
                <div class="headerPy" id="DivSTBReqCancelDetails">Payment Details</div>
                <div id="DivSTBReqCancelData" class="pmsForm slick boxModel clearfix" style="border-radius: 0 0 5px 5px; margin-left: 19px; margin-right: 19px; margin-bottom:2px;">
                    <div class="row ">
                        <div class="col-md-3">
                            <label class="">Pay using on Acount Balance: </label>
                            <span id="spnPayUsingAcountBalance"></span>
                        </div>

                        <div class="col-md-3">
                            <label class="">No Payment (Due): </label><span id="spnNoPayment"></span>
                        </div>
                        <div class="col-md-6 trY">
                            <label class="">Payment related remarks/notes:</label>
                            <div>
                                <input type="text" class="form-control" id="txtPaymentRelatedRemarks" maxlength="500" runat="server" disabled />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
        
      <asp:HiddenField ID="hdnmodule_ID" runat="server" />

    <div class="modal fade pmsModal w40" id="CloseRequisitionpop" role="dialog" aria-labelledby="assignpop" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Requisition Close</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row ">
                        <div class="col-md-12 mTop5">
                            <label class="deep">Req. No </label>
                            <div class="fullWidth">
                                <input type="text" class="form-control" id="txtRequisitionNo1" maxlength="30" />
                            </div>
                        </div>
                    </div>
                    <div class="row ">
                        <div class="col-md-12 mTop5">
                            <label class="deep">Req. No </label>
                            <div class="fullWidth">
                                <input type="text" class="form-control" id="txtRequisitionNo2" maxlength="30" />
                            </div>
                        </div>
                    </div>
                    <div class="row ">
                        <div class="col-md-12 mTop5">
                            <label class="deep">Req. No </label>
                            <div class="fullWidth">
                                <input type="text" class="form-control" id="txtRequisitionNo3" maxlength="30" />
                            </div>
                        </div>
                    </div>
                    <div class="row ">
                        <div class="col-md-12 mTop5">
                            <label class="deep">Req. No </label>
                            <div class="fullWidth">
                                <input type="text" class="form-control" id="txtRequisitionNo4" maxlength="30" />
                            </div>
                        </div>
                    </div>
                    <div class="row ">
                        <div class="col-md-12 mTop5">
                            <label class="deep">Req. No </label>
                            <div class="fullWidth">
                                <input type="text" class="form-control" id="txtRequisitionNo5" maxlength="30" />
                            </div>
                        </div>
                    </div>
                    <div class="row ">
                        <div class="col-md-12 mTop5">
                            <label class="deep">Remarks </label>
                            <div class="fullWidth">
                                <textarea class="form-control" id="txtRemarks" maxlength="500" rows="3"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                    <button type="button" class="btn btn-success" onclick="CloseRequisition();">Confirm</button>
                </div>
            </div>
        </div>
    </div>

    
     <!-- Modal -->
    <div class="modal fade pmsModal w90" id="detailsModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">View STB Scheme - Received</h5>
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
                            <label>Date <span class="red">*</span></label>
                            <div>
                                <input type="text" id="txtDate" class="form-control" disabled />
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Location <span class="red">*</span></label>
                            <div>
                                <input type="text" id="txtLocation" class="form-control" disabled />
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
                           
                        </div>
                    </div>
                    <br />
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
