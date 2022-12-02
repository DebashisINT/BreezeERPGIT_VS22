<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ReceiptChallan.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.ReceiptChallan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">




    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,800;0,900;1,600&display=swap" rel="stylesheet" />
    <link href="/assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />

    <script src="/assests/pluggins/datePicker/bootstrap-datepicker.js"></script>
    <link href="/assests/pluggins/Select2/select2.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/Select2/select2.min.js"></script>
    <link href="/assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <script src="/assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>

    <link href="CSS/ReceiptChallan.css?1.0.1" rel="stylesheet" />
    <script src="JS/ReceiptChallan.js?v=2.0.0.05"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3><%--Receipt Challan--%>
                <asp:Label runat="server" ID="HeaderName"></asp:Label>
            </h3>
        </div>
        <div id="divcross" runat="server" class="crossBtn"><a href="ReceiptChallanList.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <div class="pmsForm slick boxModel clearfix">
            <div class="row" style="margin-top: 5px">
                <div class="col-md-2 hide">
                    <label>Entry Type</label>
                    <div>
                        <%-- <dxe:ASPxComboBox ID="ddlEntryType" runat="server" ClientInstanceName="cddlEntryType" Width="100%">
                            <ClientSideEvents ValueChanged="function(s,e){ddlEntryType_SelectedIndexChanged()}"></ClientSideEvents>
                       </dxe:ASPxComboBox>--%>
                        <%--<select class="js-example-basic-single" name="" id="ddlEntryType" runat="server" onchange="ddlEntryType_SelectedIndexChanged()">
                            <option value="0">Select</option>
                            <option value="1">Token</option>
                            <option value="2">Challan</option>
                            <option value="3">Worksheet</option>
                        </select>--%>
                        <asp:DropDownList ID="ddlEntryType" runat="server" Width="100%" CssClass="js-example-basic-single hide" onchange="ddlEntryType_SelectedIndexChanged()">
                            <asp:ListItem Value="0">Select</asp:ListItem>
                            <asp:ListItem Value="1">Token</asp:ListItem>
                            <asp:ListItem Value="2">Challan</asp:ListItem>
                            <asp:ListItem Value="3">Worksheet</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-2" id="divNumberingScheme" runat="server">
                    <label>Numbering Scheme <span class="red">*</span></label>
                    <div id="divScheme" class="dropDev">
                        <%--  <select class="js-example-basic-single" name="">
                            <option value="AL">Select</option>
                        </select>--%>
                        <%-- <dxe:ASPxComboBox ID="ddlNumberingScheme" runat="server" ClientInstanceName="cddlNumberingScheme" Width="100%">
                       </dxe:ASPxComboBox>    ValueChanged="function(s,e){CmbScheme_ValueChange()}"--%>
                        <%-- <dxe:ASPxComboBox ID="ddlNumberingScheme" EnableIncrementalFiltering="True" ClientInstanceName="cddlNumberingScheme" class="js-example-basic-single"
                            SelectedIndex="0" EnableCallbackMode="false"
                            TextField="SchemaName" ValueField="ID"
                            runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True" OnCallback="ddlNumberingScheme_Callback">
                            <ClientSideEvents  EndCallback="ddlNumberingSchemeEndCallback"></ClientSideEvents>
                        </dxe:ASPxComboBox>--%>

                        <dxe:ASPxComboBox ID="CmbScheme" runat="server" ClientInstanceName="cCmbScheme"
                            Width="100%">
                            <clientsideevents selectedindexchanged="CmbScheme_ValueChange" />
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-3 noLabelmargin">
                    <label>Document Number  <span class="red">*</span></label>
                    <div>
                        <%--<input type="text" class="form-control" id="txtDocumentNumber" />  onchange="txtBillNo_TextChanged()"--%>
                        <asp:TextBox ID="txtDocumentNumber" runat="server" Width="95%" MaxLength="30" CssClass="form-control">                             
                        </asp:TextBox>
                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                </div>
                <div class="col-md-2 noLabelmargin">
                    <label>Location  <span class="red">*</span></label>
                    <div class="dropDev">
                        <%-- <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                        </dxe:ASPxComboBox>--%>
                        <%--<asp:DropDownList ID="ddlBranch" runat="server" CssClass="js-example-basic-single"  Width="100%">
                            <%--onchange="ddlBranch_ChangeIndex()"  DataTextField="branch_description" DataValueField="branch_id"--%>
                        <%--</asp:DropDownList>--%>
                        <dxe:ASPxComboBox ID="ddlBranch" runat="server" ClientInstanceName="cddlBranch" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Date  <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <buttonstyle width="13px"></buttonstyle>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>

                <div class="clear"></div>
                <hr class="hrBoder" />
                <div style="padding-top: 5px"></div>
                <div class="col-md-3">
                    <label>Entity Code <span class="red">*</span></label>
                    <div>
                        <%--Mantis Issue 24412--%>
                        <%--<input type="text" class="form-control" id="txtEntityCode" maxlength="300" runat="server" disabled />--%>
                        <input type="text" class="form-control" id="txtEntityCode" maxlength="300" runat="server" onblur="get_NetworkName();" disabled />
                        <%--End of Mantis Issue 24412--%>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Network Name <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" id="txtNetworkName" maxlength="300" runat="server" disabled />
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Contact Person <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" maxlength="200" id="txtContactPerson" runat="server" disabled />
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Contact Number <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" id="txtContactNo" maxlength="15" runat="server" disabled />
                    </div>
                </div>
            </div>

            <div class="clear"></div>
            <div style="padding-top: 5px"></div>
            <div class="row">
                <div class="col-md-2">
                    <label>Serial Number <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" id="txtDeviceNumber" maxlength="16" onblur="Devicenumber_change();" />
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Device Type <span class="red">*</span></label>
                    <div class="dropDev">
                        <%--<select class="js-example-basic-single" name="EntCode" id="DiviceTyp">
                            <option value="AL">Select</option>
                            <option value="STB">STB</option>
                            <option value="WY">MOBILE</option>
                        </select>--%>
                        <dxe:ASPxComboBox ID="DiviceTyp" runat="server" ClientInstanceName="cDiviceTyp" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Model <span class="red">*</span></label>
                    <div class="relative">
                        <input type="text" class="form-control" maxlength="100" id="txtModel" disabled style="z-index: 4;position: relative;" />
                        <%--Mantis Issue 24413  [ class="relative" added in above div AND style="z-index: 4;position: relative;"  added in txtModel ] --%>
                         <div class="dropDev" 
                             style="position: absolute;
                                top: 0;
                                z-index: 1;
                                right: -19px;">

                            <dxe:ASPxComboBox ID="selModel" runat="server" ClientInstanceName="cselModel" Width="100%" >
                                <clientsideevents selectedindexchanged="selModel_change" />
                            </dxe:ASPxComboBox>
                        </div>
                         <%--End of Mantis Issue 24413--%>
                    </div>
                    
                </div>

                <div class="col-md-2">
                    <label>Warranty:</label>
                    <div>
                        <input type="text" class="form-control" id="txtWarranty" disabled />
                    </div>
                </div>
                <div class="col-md-4">
                    <label>Problem <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlProblem" runat="server" ClientInstanceName="cddlProblem" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
            </div>
            <div class="clear"></div>
            <div style="padding-top: 5px"></div>
            <div class="row">
                <div class="col-md-6">
                    <label>Remarks</label>
                    <div>
                        <textarea class="form-control textareaBig" id="txtRemarks" maxlength="500"></textarea>
                    </div>
                </div>

                <div class="col-md-6">

                    <label style="padding-top: 40px"></label>
                    <div class="grpCheckboxDiv">
                        <label class="checkbox-inline">
                            <input type="checkbox" value="" id="chkRemote">Remote
                        </label>
                        <label class="checkbox-inline">
                            <input type="checkbox" value="" id="chkCardAdaptor">Cord/Adaptor</label>
                    </div>

                </div>

            </div>
            <div class="pdTop15">
                <div class="">
                    <button type="button" id="btnAdd" class="btn btn-success" onclick="AddDevice()"><i class="fa fa-plus-circle mr-2"></i>Add</button>
                    <button type="button" class="btn btn-info" onclick="ShowRepeatHistory()"><i class="fa fa-history mr-2"></i>Repeat History</button>
                    <button type="button" class="btn btn-warning" data-toggle="modal" data-target="#srrHist" onclick="BindServiceEntryHistory();"><i class="fa fa-wrench mr-2"></i>Service History</button>
                </div>
            </div>
        </div>


    </div>
    <div style="margin-top: 15px;">
        <div class="row">
            <div class="col-md-12" style="padding-left: 20px;">
                <dxe:ASPxGridView ID="GrdDevice" runat="server" KeyFieldName="HIddenID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                    SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" OnDataBinding="grid_DataBinding" OnCustomCallback="GrdDevice_CustomCallback"
                    Width="100%" ClientInstanceName="cGrdDevice" Settings-HorizontalScrollBarMode="auto">
                    <settingssearchpanel visible="false" delay="5000" />
                    <columns>
                    <dxe:GridViewDataTextColumn Caption="ID" FieldName="HIddenID"
                        VisibleIndex="1" FixedStyle="Left" Width="200px" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="DeviceTypeId" FieldName="DeviceTypeId"
                        VisibleIndex="1" FixedStyle="Left" Width="200px" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="ProblemID" FieldName="ProblemID"
                        VisibleIndex="1" FixedStyle="Left" Width="200px" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Device Type" FieldName="DeviceType"
                        VisibleIndex="2" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <%--<dxe:GridViewDataTextColumn Caption="Holiday Start Date" Width="150px" FieldName="FrmDate" VisibleIndex="3">
                        <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Holiday End Date" FieldName="ToDate" Width="150px" VisibleIndex="4">
                        <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>--%>

                    <dxe:GridViewDataTextColumn Caption="Model" FieldName="Model"
                        VisibleIndex="3" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="DeviceNumber"
                        VisibleIndex="4" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Warranty" FieldName="Warranty" Width="150px" VisibleIndex="5">
                        <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Problem" FieldName="Problem"
                        VisibleIndex="6" Width="200px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks"
                        VisibleIndex="7" Width="200px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Remote" FieldName="Remote"
                        VisibleIndex="8" Width="100px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Cord/Adaptor" FieldName="CardAdaptor"
                        VisibleIndex="9" Width="100px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="15" Width="150px">
                        <DataItemTemplate>

                            <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Container.KeyValue %>')" id="a_editInvoice" class="pad" title="Edit">
                                <img src="../../../assests/images/info.png" /></a>

                            <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="pad" title="Delete" id="a_delete">
                                <img src="../../../assests/images/Delete.png" /></a>
                        </DataItemTemplate>
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <CellStyle HorizontalAlign="Center"></CellStyle>
                        <HeaderTemplate><span>Actions</span></HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                </columns>
                    <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                    <clientsideevents endcallback="grid_EndCallBack" />
                    <settingspager numericbuttoncount="10" pagesize="10" showseparators="True" mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </settingspager>

                    <settings ShowGroupPanel="True" ShowStatusbar="Hidden" showhorizontalscrollbar="False" showfilterrow="true" showfilterrowmenu="true" />
                    <settingsloadingpanel text="Please Wait..." />
                </dxe:ASPxGridView>
            </div>
        </div>
    </div>
    <div style="margin-top: 15px;" id="divsave" runat="server">
        <div class="col-md-12">
            <button type="button" onclick="SaveButtonClick('new');" id="btnSaveNew" class="btn btn-success">Save & New</button>
            <button type="button" onclick="SaveButtonClick('Exit');" id="btnSaveExit" class="btn btn-primary">Save & Exit</button>
            <button type="button" class="btn btn-danger" onclick="cancel();">Cancel</button>
            <label class="checkbox-inline"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
            <label class="checkbox-inline">
                <input type="checkbox" value="" id="chkSendSMS" disabled checked />Send SMS
            </label>
        </div>
    </div>


    
    <div class="modal fade pmsModal w30" id="RepeatHistory" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Repeat History </h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="Close()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="pmsForm">
                        <div class="form-group row">
                            <span><<span id="idRepaired">0</span>> times repaired within 30 days.</span>
                        </div>
                        <div class="formLine"></div>
                        <div class="form-group row">
                            <span><<span id="idExchange">0</span>> times exchanged within 90 days.</span>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger btn-radius" data-dismiss="modal" onclick="Close()">Close</button>
                </div>
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
                                  
                                </table>
                            </div>
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

        <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divsave"
        Modal="True">
    </dxe:ASPxLoadingPanel>

    <asp:HiddenField ID="hdnSchemaType" runat="server" />
    <asp:HiddenField runat="server" ID="hdnGuid" />
    <asp:HiddenField ID="hdnSchemaID" runat="server" />
    <asp:HiddenField runat="server" ID="Hidden_add_edit" />
    <asp:HiddenField runat="server" ID="hdnReceiptChalanID" />
    <asp:HiddenField runat="server" ID="hdnEntryTypeID" />
    <asp:HiddenField runat="server" ID="hdnEntityCode" />

    <asp:HiddenField runat="server" ID="hdnOnlinePrint" />

     <asp:HiddenField runat="server" ID="hdnServiceHistoryValidation" Value="0" />
    <asp:HiddenField runat="server" ID="hdnIsEntityInformationEdiableinReceiptChallan" />
</asp:Content>
