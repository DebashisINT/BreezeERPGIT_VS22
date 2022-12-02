<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SpecialEdit.aspx.cs" Inherits="ERP.OMS.Management.Master.SpecialEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../../assests/bootstrap/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <script src="../../../assests/bootstrap/js/bootstrap-datetimepicker.min.js"></script>

    
    <script src="../Activities/JS/SearchPopup.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />

    <style>
        .content-border {
            border:1px solid #ccc;
            border-top:none;
            padding:15px;
        }
        .nav-tabs>li>a {
            border:1px solid #ccc;
            border-radius: 5px 5px 0 0;
            margin-right: 4px;
            padding: 7px 20px;
        }
        .nav-tabs>li.active>a {
            background: #33a3e8;
            color: #fff;
            border: 1px solid #33a3e8;
        }
        .nav-tabs>li>a:hover, .nav-tabs>li>a:focus:hover, .nav-tabs>li.active>a:hover, .nav-tabs>li>a:focus{
            background:#4169E1;
            color:#fff !important;
        }
    </style>
    

    <script type="text/javascript">
        //SpecialEdit.aspx/
        $(document).ready(function () {
            $('#validatedView, #grnDateView, #grnNumberView, .scndSection').hide();
            $('#validate').click(function (e) {
                e.preventDefault();
               
            });
            $('#chooseChange').on('change', function () {
                
                checkDrop();
            });
            
            $('#chooseGrnDate').datepicker({});

            $('#submitDate').click(function (e) {

                e.preventDefault();
                //var newDate = $('#chooseGrnDate').val();

                var validateObj = {};
                validateObj.Grn_Number = $('#setgrnNumber').val();
                //validateObj.New_Grn_Date = $('#chooseGrnDate').val();
                validateObj.New_Grn_Date = cchooseGrnDate.GetText();
                $.ajax({
                    type: "POST",
                    url: "SpecialEdit.aspx/resetGrnDate",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(validateObj),
                    dataType: "json",
                    success: function (res) {
                        // watchlistDataLoop(data.d);
                        console.log(res.d)
                        if (res.d == '1') {
                            jAlert('GRN Date Update Successful.', 'Alert.');
                        } else {

                        }
                    }
                });
            });
        });

        function validateGrn() {
            // 'AM/AN/MRC0000015'
            //console.log();
            if ($('#grnInput').val() == '' || $('#grnInput').val() == undefined) {
                jAlert('Please Provide a valid GRN Number.', 'Alert.');
            } else {
                $('#validatedView').show();
                checkDrop()
                var validateObj = {};
                validateObj.grnNumber = $('#grnInput').val();
                validateObj.grnType = 'GRN';
                $.ajax({
                    type: "POST",
                    url: "SpecialEdit.aspx/ValidateGrn",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(validateObj),
                    dataType: "json",
                    success: function (res) {
                        // watchlistDataLoop(data.d);
                        console.log(res.d)
                        var data = res.d
                        console.log(res.d[0].Doc_Number);
                        $('#grnNo').text(res.d[0].Doc_Number);
                        $('#grnDate').text(res.d[0].Doc_Date);
                        $('#vendorName').text(res.d[0].cnt_firstName);

                        $('#setgrnNumber').val(res.d[0].Doc_Number);
                    }
                });
            }
            
        }
        function checkDrop() {
            var thisItem = $('#chooseChange');
            console.log('saf');
            if (thisItem.val() == '1') {
              
                $('#grnDateView').show();
                $('#grnNumberView').hide();
              
                
            } else if (thisItem.val() == '2') {
                
                $('#grnDateView').hide();
                $('#grnNumberView').show();
                
            } 
            else {
                $('#grnDateView, #grnNumberView').hide();
            }
        }


        
    </script>
    <script>
        function CustomerButnClick(s, e) {
            $('#CustModel').modal('show');
        }

        function Customer_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#CustModel').modal('show');
            }
        }

        function Customerkeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtCustSearch").val()) == "" || $.trim($("#txtCustSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtCustSearch").val();


            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Customer Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");

                if ($("#txtCustSearch").val() != "") {
                    callonServer("SpecialEdit.aspx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }
        }

        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "customerIndex") {
                        SetCustomer(Id, name);
                    }
                }
            }
            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                if (thisindex < 10)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {
                    if (indexName == "customerIndex")
                        $('#txtCustSearch').focus();
                }
            }
        }


        function SetCustomer(Id, Name) {


            var key = Id;
            if (key != null && key != '') {
                $('#CustModel').modal('hide');
                ctxtCustName.SetText(Name);
                $("#hdnCustomerId").val(Id);
            }
        }

        // Main account 
        function MainAccountButnClick(s, e) {
            $('#MainAccountModel').modal('show');
        }
        function MainAccountKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                $('#MainAccountModel').modal('show');
            }
        }
        function MainAccountNewkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtMainAccountSearch").val();
            //OtherDetails.branchId = $("#ddlBranch").val();
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtMainAccountSearch").val() == "")
                    return;
                var HeaderCaption = [];
                HeaderCaption.push("Main Account Name");
                HeaderCaption.push("Short Name");

                HeaderCaption.push("Subledger Type");

                callonServer("SpecialEdit.aspx/GetMainAccountJournal", OtherDetails, "MainAccountTable", HeaderCaption, "MainAccountIndex", "SetMainAccount");
            }
            else if (e.code == "ArrowDown") {
                if ($("input[MainAccountIndex=0]"))
                    $("input[MainAccountIndex=0]").focus();
            }
            else if (e.code == "Escape") {
             
                $('#MainAccountModel').modal('hide');
              
            }
        }

        function SetMainAccount(Id, name) {
            if (Id != null && Id != "") {
                $('#MainAccountModel').modal('hide');
                ctxtMainAccount.SetText(name);
                $('#hdnMainaccountId').val(Id);
            }
        }

        //$(function () {
        //    $('#MainAccountModel').modal('hide');
        //    //$('#MainAccountModel').modal({
        //    //    backdrop: 'static',
        //    //    keyboard: false
        //    //});
        //})
        function submitMasterCall() {
            var custId = $('#hdnCustomerId').val();
            var MainAccId = $('#hdnMainaccountId').val();
            var VendId = $('#hdnVendId').val();

            var typeValue = $('#chooseType').val();
            if (typeValue == 1) {
                var obj = {}
                obj.SearchKey = custId;
                obj.mainAcc = MainAccId;
                $.ajax({
                    type: "POST",
                    url: "SpecialEdit.aspx/MasterCall",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(obj),
                    dataType: "json",
                    success: function (res) {
                        // watchlistDataLoop(data.d);
                        console.log(res.d)
                        if (res.d == 1) {
                            jAlert('Update Successful.', 'Alert.');
                            ctxtCustName.SetText();
                            ctxtMainAccount.SetText();
                            $("#hdnCustomerId").val("");
                            $("#hdnMainaccountId").val("");
                        }
                    }
                });
            } else {
                var obj = {}
                obj.SearchKey = VendId;
                obj.mainAcc = MainAccId;
                $.ajax({
                    type: "POST",
                    url: "SpecialEdit.aspx/MasterCallWithVendor",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(obj),
                    dataType: "json",
                    success: function (res) {
                        // watchlistDataLoop(data.d);
                        console.log(res.d)
                        if (res.d == 1) {
                            jAlert('Update Successful.', 'Alert.');
                            ctxtVendorName.SetText();
                            ctxtMainAccount.SetText();
                            $("#hdnVendId").val("");
                            $("#hdnMainaccountId").val("");
                        }
                    }
                });
            }
        }
        function transformDate(x) {
            
            var day = x.getDate();
            var m = x.getMonth();
            var y = x.getFullYear();
            var a = [y, m, day]
            var y = a.join("-")
            return y
        }
        function submitDoc() {
            var docNum = $('#docNum').val();
            
            var docDate = transformDate(cdocDate.GetValue());
            console.log(docNum, docDate)
            if (docNum == '' && docDate == '') {
                jAlert('You must select Doc number and Date', 'Alert.');
            } else {
                var obj = {}
                obj.DocumentNo = docNum;
                obj.DocumentDate = docDate;
                $.ajax({
                    type: "POST",
                    url: "SpecialEdit.aspx/MasterDocSet",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(obj),
                    dataType: "json",
                    success: function (res) {
                        // watchlistDataLoop(data.d);
                        console.log(res.d)
                        if (res.d == 1) {
                            jAlert('Update Successful.', 'Alert.');
                        }
                    }
                });
            }
        }

        function closeModal() {
            $('#MainAccountModel').modal('hide');
        }
        // vendor button click
        function VendorButnClick(s, e) {
            $('#VendModel').modal('show');
            $('#txtVendSearch').focus();
        }
        function VendorKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#VendModel').modal('show');
                $('#txtVendSearch').focus();
            }
        }


        function Vendkeydown(e) {
            var OtherDetails = {};
            OtherDetails.SearchKey = $("#txtVendSearch").val();
            //OtherDetails.BranchID = $('#ddl_Branch').val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Vendor Name");
                HeaderCaption.push("Unique Id");
                if (OtherDetails.SearchKey != '') {
                    callonServer("SpecialEdit.aspx/GetVendorWithBranch", OtherDetails, "VendTable", HeaderCaption, "customerIndex", "SetVend");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }
        }

        function SetVend(Id, Name) {
            var key = Id;
            if (key != null && key != '') {
                $('#VendModel').modal('hide');
                ctxtVendorName.SetText(Name);
                $("#hdnVendId").val(Id);
            }
        }
        function showbyType() {
            var typeValue = $('#chooseType').val();
            if (typeValue == 1) {
                $('#byCust').show();
                $('#byVen').hide();
                $('.frstSection').show();
                $('.scndSection').hide();
            } else if (typeValue == 3) {
               
                $('.frstSection').hide();
                $('.scndSection').show();
            } else {
                $('#byCust').hide();
                $('#byVen').show();
                $('.frstSection').show();
                $('.scndSection').hide();
            }
        }
        $(document).ready(function () {
            $('#byVen').hide();
            showbyType();
            $('#chooseType').on('change', function () {
                showbyType();
            });
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Special Edit</h3>
        </div>
    </div>
    <div class="form_main">
        <ul class="nav nav-tabs">
          <li class="active"><a data-toggle="tab" href="#home">Master</a></li>
          <li class=""><a data-toggle="tab" href="#menu1" >GRN </a></li>
          <li class="hide"><a data-toggle="tab" href="#menu2">Purchase Return</a></li>
          <li class="hide"><a data-toggle="tab" href="#menu3">Purchase Invoice</a></li>
        </ul>

        <div class="tab-content content-border">
          <div id="home" class="tab-pane fade  in active">
                <div class="row">
                    <div class="col-md-3">
                        <label>Update By</label>
                        <div>
                            <select class="form-control" id="chooseType">
                                <option value="1">Customer</option>
                                <option value="2">Vendor</option>
                                <option value="3">Purchase Invoice</option>
                            </select>
                        </div>
                    </div>
                </div>
              <hr />
              <div class="frstSection">
                  <div class="row">
                        <div class="col-md-3" id="byCust">
                              <div class="form-group">
                                <label class="" for="">Customer</label>
                                <div><dxe:ASPxButtonEdit ID="txtCustName" runat="server" ReadOnly="true" ClientInstanceName="ctxtCustName" Width="100%" TabIndex="5">
                                             <Buttons>
                                              <dxe:EditButton>
                                             </dxe:EditButton>
                                               </Buttons>
                                        <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                                  </dxe:ASPxButtonEdit>
                              </div>
                              </div>
                        </div>
                        <div class="col-md-3" id="byVen">
                                  <div class="form-group">
                                    <label class="" for="">Vendor</label>
                                    <div>
                                        <dxe:ASPxButtonEdit ID="txtVendorName" runat="server" ReadOnly="true" ClientInstanceName="ctxtVendorName" Width="100%">
                                            <Buttons>
                                                <dxe:EditButton>
                                                </dxe:EditButton>
                                            </Buttons>
                                            <ClientSideEvents ButtonClick="function(s,e){VendorButnClick();}" KeyDown="function(s,e){VendorKeyDown(s,e);}" />
                                        </dxe:ASPxButtonEdit>
                                    </div>
                                  </div>
                            </div>
                        <div class="col-md-3">
                              <div class="form-group">
                                <label class="" for="">Main Account</label>
                                <div><dxe:ASPxButtonEdit ID="txtMainAccount" runat="server" ReadOnly="true" ClientInstanceName="ctxtMainAccount" Width="100%" TabIndex="5">
                                             <Buttons>
                                              <dxe:EditButton>
                                             </dxe:EditButton>
                                               </Buttons>
                                        <ClientSideEvents ButtonClick="function(s,e){MainAccountButnClick();}" KeyDown="function(s,e){MainAccountKeyDown(s,e);}" />
                                  </dxe:ASPxButtonEdit>
                              </div>
                              </div>
                        </div>
                        <div class="col-md-2" style="padding-top: 18px;"><button type="button"  onclick="submitMasterCall()" class="btn btn-success ">Submit</button></div>
                    </div>
              </div>
              <div class="scndSection">
                  <div class="row">
                      <div class="col-md-3">
                          <label>Doc Number</label>
                          <input type="text" id="docNum" placeholder="Type Doc Number" />
                      </div>
                      <div class="col-md-3">
                          <label>Select Date</label>
                          <div>
                              <dxe:ASPxDateEdit ID="docDate" TabIndex="1" runat="server" EditFormatString="dd-MM-yyyy" Date="" Width="100%"  ClientInstanceName="cdocDate">
                                        <TimeSectionProperties>
                                <TimeEditProperties EditFormatString="hh:mm tt" />
                            </TimeSectionProperties>
                            </dxe:ASPxDateEdit>
                          </div>
                      </div>
                      <div class="col-md-2" style="padding-top: 26px;"><button type="button"  onclick="submitDoc()" class="btn btn-success ">Submit</button></div>
                  </div>
              </div>
                
              <div class="clear"></div>
          </div>
          <div id="menu1" class="tab-pane fade">
            <div class="clearfix">
                    <div class="row">
                        <div class="col-md-3">
                              <div class="form-group">
                                <label class="" for="">GRN id</label>
                                <input type="text" class="form-control" id="grnInput" placeholder="" />
                              </div>
                        </div>
                        <div class="col-md-2" style="padding-top: 18px;"><button type="button" id="validate" onclick="validateGrn()" class="btn btn-primary ">Validate</button></div>
                    </div>
                    <div class="clear"></div>
                    
                    <div class="" id="validatedView">
                        <div class="row">
                            <div class="col-md-4">
                               <table class="table table-striped" style="margin-top:15px">
                                   <tbody>
                                       <tr>
                                           <th width="150">GRN No.</th>
                                           <td id="grnNo"></td>
                                       </tr>
                                       <tr>
                                           <th>GRN Date.</th>
                                           <td id="grnDate"></td>
                                       </tr>
                                       <tr>
                                           <th>Vendor Name</th>
                                           <td id="vendorName"></td>
                                       </tr>
                                   </tbody>
                                </table>
                           </div> 
                           <div class="clear"></div>
                        </div>
                        
                        <div class="styledBox">
                            <div class="row">
                                <div class="col-md-3">
                                      <div class="form-group">
                                        <label class="" for="">Modify GRN Date/Number</label>
                                        <select class="form-control" id="chooseChange">
                                            <option value="0">Option</option>
                                            <option value="1">GRN Date</option>
                                            <option value="2">GRN Number</option>
                                        </select>
                                      </div>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div id="grnDateView">
                                <div class="row">
                                    <div class="col-md-3">
                                            <div class="form-group">
                                                <label class="" for="">New Date</label>
                                                <input type="hidden" class="form-control" id="setgrnNumber"  />
                                               <%-- <input type="text" class="form-control" id="chooseGrnDate" placeholder="" />--%>
                                                <dxe:ASPxDateEdit ID="chooseGrnDate" TabIndex="1" runat="server" EditFormatString="dd-MM-yyyy" Date="" Width="100%"  ClientInstanceName="cchooseGrnDate">
                                                <TimeSectionProperties>
                                                    <TimeEditProperties EditFormatString="hh:mm tt" />
                                                </TimeSectionProperties>
                                                </dxe:ASPxDateEdit>
                                            </div>
                                    </div>
                                    <div class="col-md-2" style="padding-top: 18px;"><button type="button" class="btn btn-primary "  id="submitDate">Submit</button></div>
                                </div>
                                <div class="clear"></div>
                            </div>
                            <div id="grnNumberView">
                                <div class="row">
                                    <div class="col-md-3">
                                            <div class="form-group">
                                            <label class="" for="">New Number</label>
                                            <input type="text" class="form-control" id="" placeholder="" />
                                            </div>
                                    </div>
                                    <div class="col-md-2" style="padding-top: 18px;"><button type="button" class="btn btn-primary ">Submit</button></div>
                                </div>
                                <div class="clear"></div>
                            </div>
                        </div>
                    </div>
                </div>
          </div>
          <div id="menu2" class="tab-pane fade">
            
            <p>Some content in menu 2.</p>
          </div>
          <div id="menu3" class="tab-pane fade">
            
            <p>Some content in menu 2.</p>
          </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnCustomerId" runat="server" />
    <asp:HiddenField ID="hdnMainaccountId" runat="server" />
    <asp:HiddenField ID="hdnVendId" runat="server" />
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
    

    <%--/* main account */--%>
    <div class="modal fade" id="MainAccountModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <!-- Modal MainAccount-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" onclick="closeModal();">&times;</button>
                    <h4 class="modal-title">Main Account Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="MainAccountNewkeydown(event)" id="txtMainAccountSearch" autofocus width="100%" placeholder="Search by Main Account Name or Short Name" />

                    <div id="MainAccountTable">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th>Main Account Name</th>
                                <th>Short Name</th>
                                <th>Subledger Type</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" onclick="closeModal();">Close</button>
                </div>
            </div>
        </div>
    </div>

    <%--vendor modal--%>
    <div class="modal fade" id="VendModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Vendor Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Vendkeydown(event)" id="txtVendSearch" autofocus width="100%" placeholder="Search By Vendor Name or Unique Id" />
                    <div id="VendTable">
                        <table border='1' width="100%">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Vendor Name</th>
                                <th>Unique Id</th>
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
