<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="serviceDataEntry.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.ServiceData.serviceDataEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../JS/SearchMultiPopup.js"></script>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.15/css/bootstrap-multiselect.css" integrity="sha256-7stu7f6AB+1rx5IqD8I+XuIcK4gSnpeGeSjqsODU+Rk=" crossorigin="anonymous" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-multiselect/0.9.15/js/bootstrap-multiselect.min.js" integrity="sha256-qoj3D1oB1r2TAdqKTYuWObh01rIVC1Gmw9vWp1+q5xw=" crossorigin="anonymous"></script>


    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,800;0,900;1,600&display=swap" rel="stylesheet" />
    <link href="/assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />

    <script src="/assests/pluggins/datePicker/bootstrap-datepicker.js"></script>
    <link href="/assests/pluggins/Select2/select2.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/Select2/select2.min.js"></script>
    <link href="/assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <script src="/assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>

    <%--<link href="../../../assests/css/custom/commonService.css" rel="stylesheet" />--%>
    

    
    <link href="serviceData.css" rel="stylesheet" />
    <script src="serviceDataEntry.js"></script>
    <style>
       #FnAcceptRejectIframe {
            width:100%;
            border:none;
            min-height:350px
        }
    </style>
    <script>
        function BindRepairingDetails() {
           var challanNo = $("#tdChallanIDF").html();
           var TechId = 0;
            //Mantis Issue 25172
            //var UniqueKey = "GTPL_SRV";
           var UniqueKey = $("#hdnDbName").val();
            //End of Mantis Issue 25172
           var pageUrl = "TechnicianAssign.aspx?id=" + challanNo + "&&AU=" + TechId + "&&UniqueKey=" + UniqueKey;
         
           $("#FnAcceptReject").modal('show');
           $("#FnAcceptRejectIframe").attr('src', pageUrl)
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="modal fade pmsModal w40" id="FnAcceptReject" tabindex="-1" role="dialog" aria-labelledby="FnAcceptRejectLabel" aria-hidden="true">
      <div class="modal-dialog" role="document">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">Repairing Action</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body">
            <iframe id="FnAcceptRejectIframe"></iframe>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
          </div>
        </div>
      </div>
    </div>


    <span class="hddd font-pp" id="DivHeader" runat="server">Add Service Entry</span>
    <div id="divcross" runat="server" class="crossBtn pull-right"><a href="serviceDataList.aspx"><i class="fa fa-times"></i></a></div>
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
                        <td id="tdChallanIDF" runat="server" class="hide"></td>
                        <td id="tdChallanNo" runat="server"></td>
                        <td id="tdEntityCode" runat="server"></td>
                        <td id="tdNetworkName" runat="server"></td>
                        <td id="tdContactPerson" runat="server"></td>
                        <td id="tdReceivedOn" runat="server"></td>
                        <td id="tdReceivedBy" runat="server"></td>
                        <td id="tdAssignedTo" runat="server"></td>
                        <td id="tdAssignedBy" runat="server"></td>
                        <td id="tdAssignedOn" runat="server"></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="clearfix hide">
            <div class="">
                <ul class="list-inline listtype">
                    <li>
                        <div class="bugde">
                            <div class="deep">Entity Code </div>
                            <div class="fullWidth">
                                <b class="f15">1524dfgs523</b>
                            </div>
                        </div>
                    </li>
                    <li>
                        <div class="bugde">
                            <div class="deep">Network Name </div>
                            <div class="fullWidth">
                                <b class="f15">Somasf,fa asfb lasf</b>
                            </div>
                        </div>
                    </li>
                    <li>
                        <div class="bugde">
                            <div class="deep">Contact Person</div>
                            <div class="fullWidth">
                                <b class="f15">Susanta Kundu</b>
                            </div>
                        </div>
                    </li>
                    <li>
                        <div class="bugde">
                            <div class="deep">Received On</div>
                            <div class="fullWidth">
                                <b class="f15">03.02.2020</b>
                            </div>
                        </div>
                    </li>
                    <li>
                        <div class="bugde">
                            <div class="deep">Received By</div>
                            <div class="fullWidth">
                                <b class="f15">05.04.2020</b>
                            </div>
                        </div>
                    </li>
                    <li>
                        <div class="bugde">
                            <div class="deep">Assigned To</div>
                            <div class="fullWidth">
                                <b class="f15">Priti Ghosh</b>
                            </div>
                        </div>
                    </li>
                    <li>
                        <div class="bugde">
                            <div class="deep">Assigned By</div>
                            <div class="fullWidth">
                                <b class="f15">Sumon Das</b>
                            </div>
                        </div>
                    </li>
                    <li>
                        <div class="bugde">
                            <div class="deep">Assigned On</div>
                            <div class="fullWidth">
                                <b class="f15">05.04.2020</b>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
        </div>

        <div class="relative clearfix">
            <%-- <span class="togglerSlide btn btn-warning" style="position: absolute; right: 218px; z-index: 10;" data-toggle="tooltip" data-placement="top" title="Filter"><i class="fa fa-filter"></i></span>--%>
            <div id="divListData">
                <table id="dataTableList" class="table table-striped table-bordered display nowrap" style="width: 100%">
                    <thead>
                        <tr>
                            <th>Model</th>
                            <th>Serial No</th>
                            <th>Problem Reported </th>
                            <th>Service Action</th>
                            <th>Component</th>
                            <th>Warranty </th>
                            <th>Stock Entry </th>
                            <th>New Model</th>
                            <th>New Serial No</th>
                            <th>Problem Found</th>
                            <th>Remarks</th>
                            <th>Warranty Status</th>
                            <th>Return Reason</th>
                            <th>Billable</th>
                            <th>Level</th>
                            <th>Action</th>
                        </tr>
                    </thead>

                </table>
            </div>
        </div>

        <div class="repeatedDs">
            <div class="  newColorBox clearfix mBot10 mTop5 font-pp">
                <div class="clWraper ">
                    <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px 0; height: auto; border-radius: 0;">
                        <ul>
                            <li class="clsbnrLblTotalQty">
                                <div class="horizontallblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Model
                                                </td>
                                                <td id="tdModel"></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li class="clsbnrLblTotalQty">
                                <div class="horizontallblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Serial No
                                                </td>
                                                <td id="tdSerialNo"></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li class="clsbnrLblTotalQty">
                                <div class="horizontallblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Problem Reported
                                                </td>
                                                <td id="tdProblemReported"></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li>
                                <button type="button"  class="btn btn-warning" onclick="BindRepairingDetails()" ><i class="fa fa-wrench" style="margin: 0; margin-right: 5px"></i>Repairing Details</button>
                            </li>
                        </ul>
                    </div>
                    <div class="row hide">
                        <div class="col-md-2">
                            <label>Model</label>
                            <div class="relative">
                                <input type="text" class="form-control disabled" value="DX1254625" disabled="disabled" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Serial No</label>
                            <div class="relative">
                                <div class="relative">
                                    <input type="text" class="form-control disabled" value="DX1254625456" disabled="disabled" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8">
                            <label>Problem Reported</label>
                            <div class="relative">
                                <input type="text" class="form-control disabled" value="A wiki is a knowledge base website on which users collaboratively modify and structure content" disabled="disabled" />
                            </div>
                        </div>
                    </div>
                </div>

                <div style="padding: 15px">
                    <div class="row">
                        <div class="col-md-2 dsl">
                            <label>Service Action <span class="red">*</span></label>
                            <div class="dropDev">

                                <%--  <asp:DropDownList ID="ddlServiceAction" onChange="ddlServiceAction_change();" runat="server" CssClass="js-example-basic-single" DataTextField="SrvActionDesc" DataValueField="SrvActionID" Width="100%">
                            </asp:DropDownList>--%>
                                <dxe:ASPxComboBox ID="ddlServiceAction" Height="23px" runat="server" cssClass="" ClientInstanceName="cddlServiceAction" Width="100%">
                                    <clientsideevents selectedindexchanged="ddlServiceAction_change" />
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Component</label>
                            <div class="">
                                <%--<asp:DropDownList ID="ddlComponent" runat="server" CssClass="js-example-basic-single" DataTextField="sProducts_Description" DataValueField="sProducts_ID" Width="100%">
                                </asp:DropDownList>--%>
                                <dxe:ASPxCallbackPanel runat="server" ID="ComponentPanel" ClientEnabled="false" ClientInstanceName="cComponentPanel" OnCallback="Component_Callback" Height="23px">
                                    <panelcollection>
                        <dxe:PanelContent runat="server">
                            <dxe:ASPxGridLookup ID="lookup_Component" SelectionMode="Multiple" runat="server" ClientInstanceName="gridComponentLookup"
                                OnDataBinding="lookup_Component_DataBinding"
                                KeyFieldName="ID" Width="100%" TextFormatString="{1}" AutoGenerateColumns="False" MultiTextSeparator=", ">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="60" Caption=" " />
                                    <dxe:GridViewDataColumn FieldName="sProducts_Code" Visible="true" VisibleIndex="1" width="200px" Caption="Product code" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="sProducts_Name" Visible="true" VisibleIndex="2" width="200px" Caption="Product Name" Settings-AutoFilterCondition="Contains">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>
                                         <dxe:GridViewDataColumn FieldName="Replaceable" Visible="true" VisibleIndex="3" width="100px" Caption="Replaceable" Settings-AutoFilterCondition="Contains">
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
                                                            <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAllProduct" UseSubmitBehavior="False" />
                                                        <%--</div>--%>
                                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAllProduct" UseSubmitBehavior="False"/>                                                        
                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseProductLookup" UseSubmitBehavior="False" />
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
                                <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Warranty</label>
                            <div class="relative">
                                <div class="input-group date" onblur="WarrantyChange();">
                                    <input type="text" id="dtWarrenty" style="height: 28px;" class="form-control" onchange="WarrantyChange();" onblur="WarrantyChange();" />
                                    <div class="input-group-addon">
                                        <span class="fa fa-calendar-check-o" onblur="WarrantyChange();"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Stock Entry</label>
                            <div>
                                <select class="js-example-basic-single form-control" id="ddlStockEntry" name="EntCode">
                                    <option value="0">Select</option>
                                    <option value="1">Fresh</option>
                                    <option value="2">Refurbished</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>New Model</label>
                            <div>
                                <%--<select class="js-example-basic-single" name="EntCode">
                                    <option value="AL">Select</option>
                                    <option value="AL">15264555</option>
                                    <option value="WY">15456325</option>
                                    <option value="WY">45956654</option>
                                    <option value="WY">54865227</option>
                                    <option value="WY">58458962</option>
                                    <option value="WY">42541563</option>
                                    <option value="WY">85859456</option>
                                    <option value="WY">56326567</option>
                                </select>--%>
                                <asp:DropDownList ID="ddlModel" runat="server" CssClass="js-example-basic-single" DataTextField="ModelDesc" DataValueField="ModelID" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>New Serial No</label>
                            <div>
                                <input type="text" id="txtNewSerialNo" class="form-control" style="height: 28px;" maxlength="16" />
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-2">
                            <label>Problem Found <span class="red">*</span></label>
                            <div>
                                <%--<select class="js-example-basic-single" name="EntCode">
                                    <option value="AL">Select</option>
                                    <option value="AL">15264555</option>
                                    <option value="WY">15456325</option>
                                    <option value="WY">45956654</option>
                                    <option value="WY">54865227</option>
                                    <option value="WY">58458962</option>
                                    <option value="WY">42541563</option>
                                    <option value="WY">85859456</option>
                                    <option value="WY">56326567</option>
                                </select>--%>
                                <asp:DropDownList ID="ddlProblemFound" runat="server" CssClass="js-example-basic-single" DataTextField="ProblemDesc" DataValueField="ProblemID" Width="100%">
                                </asp:DropDownList>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <label>Remarks</label>
                            <div>
                                <input type="text" id="txtRemarks" style="height: 28px" class="form-control" />
                            </div>
                        </div>
                        <div class="col-md-2 dsl">
                            <label>Return Reason</label>
                            <div>

                                <dxe:ASPxComboBox ID="ddlReturnReason" Height="23px" runat="server" cssClass="" ClientInstanceName="cddlReturnReason" Width="100%">
                                </dxe:ASPxComboBox>
                            </div>
                         </div>
                           
                       <%-- Mantis Issue 25172--%>
                        <div class="col-md-2">
                            <label>Warranty Status <span class="red">*</span></label>
                            <div>
                                <select class="js-example-basic-single form-control" id="ddlWarrentyStatus" name="EntCode" onchange="ddlWarrentyStatus_Change();">
                                    <option value="0">Select</option>
                                    <option value="1">In Warranty</option>
                                    <option value="2">Out warranty</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="col-md-6" style="padding-top: 19px">
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" id="chkBillable" />
                                        Billable

                                    </label>
                                </div>
                            </div>
                        </div>

                        <%--<div class="col-md-4">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Warranty Status <span class="red">*</span></label>
                                    <div>
                                        <select class="js-example-basic-single form-control" id="ddlWarrentyStatus" name="EntCode" onchange="ddlWarrentyStatus_Change();">
                                            <option value="0">Select</option>
                                            <option value="1">In Warranty</option>
                                            <option value="2">Out warranty</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-6" style="padding-top: 19px">
                                    <div class="checkbox">
                                        <label>
                                            <input type="checkbox" id="chkBillable" />
                                            Billable

                                        </label>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <label>Unbillable Reason</label>
                                    <div>
                                        <input type="text" id="txtReason" style="height: 28px" class="form-control" disabled />
                                    </div>
                                </div>
                            </div>
                             
                        </div>--%>
                         
                        <div class="clear"></div>
                        
                        <div class="col-md-6">
                             <label>Unbillable Reason</label>
                            <div>
                                <input type="text" id="txtReason" style="height: 28px" class="form-control" disabled />
                            </div>
                        </div>
                       <div class="col-md-2">
                            <label>Level</label>
                            <div>
                                <select class="js-example-basic-single form-control" id="ddlLevel" name="Level" >
                                    <option value="0">Select</option>
                                    <option value="1">Level-1</option>
                                    <option value="2">Level-2</option>
                                    <option value="3">Level-3</option>
                                    <option value="4">Level-4</option>
                                </select>
                            </div>
                        </div>
                         <%--End of Mantis Issue 25172--%>

                        <div class="clear"></div>
                        
                       

                        <div class="clear"></div>
                        <div class="col-md-8 mTop5" >

                            <div id="divsave">

                                <% if (rights.CanAdd)
                                   { %>
                                <button type="button" id="btnAdd" runat="server" class="btn btn-success hide" onclick="ServiceEntryAdd();">Add</button>
                                <button type="button" id="btnSave" runat="server" class="btn btn-success hide" onclick="SaveServiceEntry();">Save & Exit</button>
                                <%} %>
                                <button type="button" id="btnCanlecl" runat="server" class="btn btn-danger" onclick="Cancel();">Cancel</button>
                                <button type="button" class="btn btn-warning" onclick="BindServiceEntryHistory()" data-toggle="modal" data-target="#srrHist"><i class="fa fa-wrench" style="margin: 0; margin-right: 5px"></i>Service History</button>

                                <label class="checkbox-inline">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
                                <label class="checkbox-inline">
                                    <input type="checkbox" value="" id="chkSendSMS" checked disabled />Send SMS
                                </label>
                            </div>
                           
                        </div>
                    </div>
                </div>
            </div>
            <%--   <div class="pmsForm  newColorBox clearfix mBot10 mTop5 font-pp">
                <div class="clWraper ">
                    <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px; height: auto; border-radius: 0;">
                        <ul>
                            <li class="clsbnrLblTotalQty">
                                <div class="horizontallblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Model
                                                </td>
                                                <td>adghgag45a4sg
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li class="clsbnrLblTotalQty">
                                <div class="horizontallblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Serial No
                                                </td>
                                                <td>DX1254625456
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li class="clsbnrLblTotalQty">
                                <div class="horizontallblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Problem Reported
                                                </td>
                                                <td>A wiki is a knowledge base website on
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <div class="row hide">
                        <div class="col-md-2">
                            <label>Model</label>
                            <div class="relative">
                                <input type="text" class="form-control disabled" value="DX1254625" disabled="disabled" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Serial No</label>
                            <div class="relative">
                                <div class="relative">
                                    <input type="text" class="form-control disabled" value="DX1254625456" disabled="disabled" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8">
                            <label>Problem Reported</label>
                            <div class="relative">
                                <input type="text" class="form-control disabled" value="A wiki is a knowledge base website on which users collaboratively modify and structure content" disabled="disabled" />
                            </div>
                        </div>
                    </div>
                </div>

                <div style="padding: 15px">
                    <div class="row">
                        <div class="col-md-2">
                            <label>Service Action</label>
                            <div>
                                <select class="js-example-basic-single" name="EntCode">
                                    <option value="AL">Select</option>
                                    <option value="AL">15264555</option>
                                    <option value="WY">15456325</option>
                                    <option value="WY">45956654</option>
                                    <option value="WY">54865227</option>
                                    <option value="WY">58458962</option>
                                    <option value="WY">42541563</option>
                                    <option value="WY">85859456</option>
                                    <option value="WY">56326567</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Component</label>
                            <div>
                                <select class="js-example-basic-single" name="EntCode">
                                    <option value="AL">Select</option>
                                    <option value="AL">15264555</option>
                                    <option value="WY">15456325</option>
                                    <option value="WY">45956654</option>
                                    <option value="WY">54865227</option>
                                    <option value="WY">58458962</option>
                                    <option value="WY">42541563</option>
                                    <option value="WY">85859456</option>
                                    <option value="WY">56326567</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Warranty</label>
                            <div class="relative">
                                <div class="input-group date">
                                    <input type="text" class="form-control" />
                                    <div class="input-group-addon">
                                        <span class="fa fa-calendar-check-o"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Stock Entry</label>
                            <div>
                                <select class="js-example-basic-single" name="EntCode">
                                    <option value="AL">Select</option>
                                    <option value="AL">Fresh</option>
                                    <option value="WY">Refurbished</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Model</label>
                            <div>
                                <select class="js-example-basic-single" name="EntCode">
                                    <option value="AL">Select</option>
                                    <option value="AL">15264555</option>
                                    <option value="WY">15456325</option>
                                    <option value="WY">45956654</option>
                                    <option value="WY">54865227</option>
                                    <option value="WY">58458962</option>
                                    <option value="WY">42541563</option>
                                    <option value="WY">85859456</option>
                                    <option value="WY">56326567</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>New Serial No</label>
                            <div>
                                <input type="text" class="form-control" />
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-2">
                            <label>Problem Found</label>
                            <div>
                                <select class="js-example-basic-single" name="EntCode">
                                    <option value="AL">Select</option>
                                    <option value="AL">15264555</option>
                                    <option value="WY">15456325</option>
                                    <option value="WY">45956654</option>
                                    <option value="WY">54865227</option>
                                    <option value="WY">58458962</option>
                                    <option value="WY">42541563</option>
                                    <option value="WY">85859456</option>
                                    <option value="WY">56326567</option>
                                </select>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <label>Remarks</label>
                            <div>
                                <input type="text" class="form-control" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Warranty Status</label>
                            <div>
                                <select class="js-example-basic-single" name="EntCode">
                                    <option value="AL">Select</option>
                                    <option value="AL">In Warranty</option>
                                    <option value="WY">15456325</option>
                                    <option value="WY">45956654</option>
                                    <option value="WY">54865227</option>
                                    <option value="WY">58458962</option>
                                    <option value="WY">42541563</option>
                                    <option value="WY">85859456</option>
                                    <option value="WY">56326567</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>&nbsp</label>
                            <div>
                                <button type="button" class="btn btn-warning" data-toggle="modal" data-target="#srrHist"><i class="fa fa-wrench" style="margin: 0"></i>Service History</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="pmsForm  newColorBox clearfix mBot10 mTop5 font-pp">
                <div class="clWraper ">
                    <div class="content reverse horizontal-images clearfix" style="width: 100%; margin-right: 0; padding: 8px; height: auto; border-radius: 0;">
                        <ul>
                            <li class="clsbnrLblTotalQty">
                                <div class="horizontallblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Model
                                                </td>
                                                <td>adghgag45a4sg
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li class="clsbnrLblTotalQty">
                                <div class="horizontallblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Serial No
                                                </td>
                                                <td>DX1254625456
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                            <li class="clsbnrLblTotalQty">
                                <div class="horizontallblHolder">
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>Problem Reported
                                                </td>
                                                <td>A wiki is a knowledge base website on
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <div class="row hide">
                        <div class="col-md-2">
                            <label>Model</label>
                            <div class="relative">
                                <input type="text" class="form-control disabled" value="DX1254625" disabled="disabled" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Serial No</label>
                            <div class="relative">
                                <div class="relative">
                                    <input type="text" class="form-control disabled" value="DX1254625456" disabled="disabled" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8">
                            <label>Problem Reported</label>
                            <div class="relative">
                                <input type="text" class="form-control disabled" value="A wiki is a knowledge base website on which users collaboratively modify and structure content" disabled="disabled" />
                            </div>
                        </div>
                    </div>
                </div>

                <div style="padding: 15px">
                    <div class="row">
                        <div class="col-md-2">
                            <label>Service Action</label>
                            <div>
                                <select class="js-example-basic-single" name="EntCode">
                                    <option value="AL">Select</option>
                                    <option value="AL">15264555</option>
                                    <option value="WY">15456325</option>
                                    <option value="WY">45956654</option>
                                    <option value="WY">54865227</option>
                                    <option value="WY">58458962</option>
                                    <option value="WY">42541563</option>
                                    <option value="WY">85859456</option>
                                    <option value="WY">56326567</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Component</label>
                            <div>
                                <select class="js-example-basic-single" name="EntCode">
                                    <option value="AL">Select</option>
                                    <option value="AL">15264555</option>
                                    <option value="WY">15456325</option>
                                    <option value="WY">45956654</option>
                                    <option value="WY">54865227</option>
                                    <option value="WY">58458962</option>
                                    <option value="WY">42541563</option>
                                    <option value="WY">85859456</option>
                                    <option value="WY">56326567</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Warranty</label>
                            <div class="relative">
                                <div class="input-group date">
                                    <input type="text" class="form-control" />
                                    <div class="input-group-addon">
                                        <span class="fa fa-calendar-check-o"></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Stock Entry</label>
                            <div>
                                <select class="js-example-basic-single" name="EntCode">
                                    <option value="AL">Select</option>
                                    <option value="AL">Fresh</option>
                                    <option value="WY">Refurbished</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Model</label>
                            <div>
                                <select class="js-example-basic-single" name="EntCode">
                                    <option value="AL">Select</option>
                                    <option value="AL">15264555</option>
                                    <option value="WY">15456325</option>
                                    <option value="WY">45956654</option>
                                    <option value="WY">54865227</option>
                                    <option value="WY">58458962</option>
                                    <option value="WY">42541563</option>
                                    <option value="WY">85859456</option>
                                    <option value="WY">56326567</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>New Serial No</label>
                            <div>
                                <input type="text" class="form-control" />
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-2">
                            <label>Problem Found</label>
                            <div>
                                <select class="js-example-basic-single" name="EntCode">
                                    <option value="AL">Select</option>
                                    <option value="AL">15264555</option>
                                    <option value="WY">15456325</option>
                                    <option value="WY">45956654</option>
                                    <option value="WY">54865227</option>
                                    <option value="WY">58458962</option>
                                    <option value="WY">42541563</option>
                                    <option value="WY">85859456</option>
                                    <option value="WY">56326567</option>
                                </select>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <label>Remarks</label>
                            <div>
                                <input type="text" class="form-control" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>Warranty Status</label>
                            <div>
                                <select class="js-example-basic-single" name="EntCode">
                                    <option value="AL">Select</option>
                                    <option value="AL">In Warranty</option>
                                    <option value="WY">15456325</option>
                                    <option value="WY">45956654</option>
                                    <option value="WY">54865227</option>
                                    <option value="WY">58458962</option>
                                    <option value="WY">42541563</option>
                                    <option value="WY">85859456</option>
                                    <option value="WY">56326567</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <label>&nbsp</label>
                            <div>
                                <button type="button" class="btn btn-warning" data-toggle="modal" data-target="#srrHist"><i class="fa fa-wrench" style="margin: 0"></i>Service History</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>--%>
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
                            <div id="DivHistoryTable">
                                <table class="table table-striped table-bordered tableStyle" id="dataTable">
                                    <thead>
                                        <tr>
                                            <th>Entity Code</th>
                                            <th>Ref. Receipt No.</th>
                                            <th>Service Action</th>
                                            <th>Remarks</th>
                                            <th>Billable</th>
                                        </tr>
                                    </thead>
                                    <%-- <tbody>
                                    <tr>
                                        <td>sasgasg</td>
                                        <td>sasgasg</td>
                                        <td>sasgasg</td>
                                        <td>sasgasg</td>
                                    </tr>
                                </tbody>--%>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                    <%--<button type="button" class="btn btn-success">OK</button>--%>
                </div>
            </div>
        </div>
    </div>

    <!--Product Modal -->
    <div class="modal fade" id="ProdModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Productkeydown(event)" id="txtProdSearch" width="100%" placeholder="Search By Product Name" />
                    <div id="ProductTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size: small">
                                <th>Select</th>
                                <th class="hide">id</th>
                                <th>Code</th>
                                <th>Name</th>
                                <th>Hsn</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn btn-info" onclick="DeSelectAll('ProductSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn btn-success" data-dismiss="modal" onclick="OKPopup('ProductSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdncWiseProductId" runat="server" />
    <asp:HiddenField ID="hdnClassId" runat="server" />
    <!--Product Modal -->

    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divsave"
        Modal="True">
    </dxe:ASPxLoadingPanel>

    <asp:HiddenField ID="hdnParentStatus" runat="server" />

    <asp:HiddenField ID="hdnReceiptChalanID" runat="server" />

    <asp:HiddenField ID="hdnReceiptChalanDtlsID" runat="server" />
    <asp:HiddenField ID="hdnEntryMode" runat="server" />

    <asp:HiddenField ID="hdnComponentQty" runat="server" />
    <%--Mantis Issue 25172--%>
    <asp:HiddenField ID="hdnDbName" runat="server" />
    <%--End of Mantis Issue 25172--%>


    <div class="modal fade pmsModal w50" id="detailsModalComponent" tabindex="-1" role="dialog" aria-labelledby="detailsModal" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="">Component List</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <div class="clearfix ">
                        <div id="ComponentTable">
                            <%-- <table id="dataTable-Component" class="table table-striped table-bordered display nowrap" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>Product Code</th>
                                        <th>Product Name</th>
                                        <th>Replaceable</th>
                                        <th>Quantity</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td>Product Code</td>
                                        <td>Product Name</td>
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
                    <button type="button" class="btn btn-success" data-dismiss="modal" onclick="ComponentQty_Submit();">Ok</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
