<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="STBSchemeReceivedAdd.aspx.cs" Inherits="ServiceManagement.STBManagement.STBSchemeReceived.STBSchemeReceivedAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,800;0,900;1,600&display=swap" rel="stylesheet" />
    <link href="/assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />

    <script src="/assests/pluggins/datePicker/bootstrap-datepicker.js"></script>
    <link href="/assests/pluggins/Select2/select2.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/Select2/select2.min.js"></script>
    <link href="/assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <script src="/assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>

    <link href="../../ServiceManagement/Transaction/CSS/ReceiptChallan.css" rel="stylesheet" />
    <script src="../JS/STBSchemeReceivedAddJS.js?v=2.0"></script>
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
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3><%--Receipt Challan--%>
                <asp:Label runat="server" ID="HeaderName"></asp:Label>
            </h3>
        </div>
        <div id="divcross" runat="server" class="crossBtn"><a href="STBSchemeReceivedList.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <div class="pmsForm slick boxModel clearfix">
            <div class="row" style="margin-top: 5px">
                <div class="col-md-2" id="divNumberingScheme" runat="server">
                    <label>Numbering Scheme <span class="red">*</span></label>
                    <div id="divScheme" class="dropDev">
                        <dxe:ASPxComboBox ID="CmbScheme" runat="server" ClientInstanceName="cCmbScheme" Width="100%">
                            <clientsideevents selectedindexchanged="CmbScheme_ValueChange" />
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-3 noLabelmargin">
                    <label>Document Number  <span class="red">*</span></label>
                    <div>
                        <asp:TextBox ID="txtDocumentNumber" runat="server" Width="95%" MaxLength="30" CssClass="form-control">                             
                        </asp:TextBox>
                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                </div>
                <div class="col-md-2 noLabelmargin">
                    <label>Location  <span class="red">*</span></label>
                    <div class="dropDev">
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
                        <input type="text" class="form-control" id="txtEntityCode" maxlength="300" runat="server" disabled />
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
                        <dxe:ASPxComboBox ID="DiviceTyp" runat="server" ClientInstanceName="cDiviceTyp" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Model <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" maxlength="100" id="txtModel" disabled />
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Rate </label>
                    <div class="dropDev">
                        <dxe:ASPxTextBox ID="txtRate" ClientInstanceName="ctxtRate" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
                            <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                            <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                        </dxe:ASPxTextBox>
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
                            <input type="checkbox" value="" id="chkRemote" onchange="getRate();" checked>Remote
                        </label>
                        <label class="checkbox-inline">
                            <input type="checkbox" value="" id="chkCardAdaptor" onchange="getRate();">Cord/Adaptor</label>
                    </div>
                </div>
            </div>
            <div class="pdTop15">
                <div class="">
                    <button type="button" id="btnAdd" class="btn btn-success" onclick="AddDevice()"><i class="fa fa-plus-circle mr-2"></i>Add</button>
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

                    <dxe:GridViewDataTextColumn Caption="Device Type" FieldName="DeviceType"
                        VisibleIndex="2" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

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

                    <dxe:GridViewDataTextColumn Caption="Rate" FieldName="Rate"
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
                    <settingscontextmenu enabled="true"></settingscontextmenu>
                    <clientsideevents endcallback="grid_EndCallBack" />
                    <settingspager numericbuttoncount="10" pagesize="10" showseparators="True" mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                    <FirstPageButton Visible="True">
                    </FirstPageButton>
                    <LastPageButton Visible="True">
                    </LastPageButton>
                </settingspager>

                    <settings showgrouppanel="True" showstatusbar="Hidden" showhorizontalscrollbar="False" showfilterrow="true" showfilterrowmenu="true" />
                    <settingsloadingpanel text="Please Wait..." />
                </dxe:ASPxGridView>
            </div>
        </div>
    </div>

    <div class="headerPy" id="divApprovalSectionhdr" runat="server">Approval Details</div>
    <div class="pmsForm slick boxModel clearfix" style="border-radius: 0 0 5px 5px" id="divApprovalSectiondtls" runat="server">
        <div>
            <table class="pRightTbl">
                <tr>
                    <td>
                        <div>Approval Action</div>
                    </td>
                    <td>
                        <div id="tdddlApprovalAction">
                            <select id="ddlApprovalAction" class="form-control">
                                <option value="0">Select</option>
                                <option value="1">Approve</option>
                                <option value="2">Hold</option>
                                <option value="3">Cancel</option>
                            </select>
                        </div>
                    </td>
                    <td class="hide" id="divEmployeeIn">
                        <div class="dropDev">
                            <dxe:ASPxComboBox ID="dddlApprovalEmployee" runat="server" ClientInstanceName="cdddlApprovalEmployee" Width="100%">
                            </dxe:ASPxComboBox>
                        </div>
                    </td>
                    <td>
                        <div>Remarks</div>
                    </td>
                    <td style="width: 300px">
                        <div class="">
                            <input type="text" class="form-control" id="txtApprovalRemarks" runat="server" />

                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="margin-top: 15px;" id="divsave" runat="server">
        <div class="col-md-12">
            <button type="button" onclick="SaveButtonClick('new');" id="btnSaveNew" class="btn btn-success">Save & New</button>
            <button type="button" onclick="SaveButtonClick('Exit');" id="btnSaveExit" class="btn btn-primary">Save & Exit</button>
            <button type="button" class="btn btn-danger" onclick="cancel();">Cancel</button>
            <label class="checkbox-inline">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
            <%--  <label class="checkbox-inline">
                <input type="checkbox" value="" id="chkSendSMS" disabled checked />Send SMS
            </label>--%>
        </div>
    </div>
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divsave"
        Modal="True">
    </dxe:ASPxLoadingPanel>

    <asp:HiddenField ID="hdnSchemaType" runat="server" />
    <asp:HiddenField runat="server" ID="hdnGuid" />
    <asp:HiddenField ID="hdnSchemaID" runat="server" />
    <asp:HiddenField runat="server" ID="Hidden_add_edit" />
    <asp:HiddenField runat="server" ID="hdnSchemeReceivedID" />
    <asp:HiddenField runat="server" ID="hdnEntryTypeID" />
    <asp:HiddenField runat="server" ID="hdnEntityCode" />

    <asp:HiddenField runat="server" ID="hdnOnlinePrint" />
    <asp:HiddenField runat="server" ID="hdnIsAproval" />
    <asp:HiddenField runat="server" ID="hdnServiceHistoryValidation" Value="0" />

     <asp:HiddenField runat="server" ID="hdnIsEntityInformationEditableInSTBSchemeReceived" Value="0" />
    <asp:HiddenField runat="server" ID="hdnIsDuplicateSerialNoAllowedinSTBSchemeRec" />
</asp:Content>
