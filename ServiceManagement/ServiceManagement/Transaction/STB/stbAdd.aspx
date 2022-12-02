<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="stbAdd.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.STB.stbAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../JS/SearchMultiPopup.js"></script>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,800;0,900;1,600&display=swap" rel="stylesheet" />
    <link href="/assests/pluggins/datePicker/datepicker.css" rel="stylesheet" />
    <script src="/assests/pluggins/datePicker/bootstrap-datepicker.js"></script>
    <link href="/assests/pluggins/Select2/select2.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/Select2/select2.min.js"></script>
    <link href="/assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <script src="/assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>
    <%-- <link href="../../../assests/css/custom/commonService.css" rel="stylesheet" />--%>
    <link href="../CSS/jobsheetEntry.css" rel="stylesheet" />
    <script src="../JS/STBReceiving.js?V=1.6"></script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <span class="hddd font-pp" id="HeaderName" runat="server">Add Receipt Challan - Token</span>
    <div id="divcross" runat="server" class="crossBtn pull-right"><a href="stbList.aspx"><i class="fa fa-times"></i></a></div>

    <div class="clearfix">
        <div class=" slick boxModel clearfix mTop5 font-pp" style="margin-top: 15px;">
            <div class="row mTop5" style="">
                <div class="col-md-2" id="divNumberingScheme" runat="server">
                    <label>Numbering Scheme <span class="red">*</span></label>
                    <div id="divScheme" class="dropDev">
                        <dxe:ASPxComboBox ID="CmbScheme" runat="server" ClientInstanceName="cCmbScheme" Width="100%">
                            <clientsideevents selectedindexchanged="CmbScheme_ValueChange" />
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Document Number <span class="red">*</span></label>
                    <div>
                        <asp:TextBox ID="txtDocumentNumber" runat="server" Width="95%" MaxLength="30" CssClass="form-control">   </asp:TextBox>
                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Unit</label>
                    <div>
                        <dxe:ASPxComboBox ID="ddlBranch" runat="server" ClientInstanceName="cddlBranch" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>

                </div>
                <div class="col-md-2">
                    <label>Date <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <buttonstyle width="13px"></buttonstyle>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-4">
                    <label>For Location <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlLocation" runat="server" ClientInstanceName="cddlLocation" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>

                <div class="clear"></div>
                <div class="col-md-6">
                    <label>Remarks</label>
                    <div>
                        <input type="text" id="txtHeaderRemarks" class="form-control" runat="server" />
                    </div>
                </div>
                <div class="clear"></div>
            </div>
            <div class="row mTop5" style="">
                <div class="col-md-2" id="div1" runat="server">
                    <label>Challan Date <span class="red">*</span></label>
                    <div id="divScheme" class="dropDev">
                        <dxe:ASPxDateEdit ID="dt_ChallanDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cChallanDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <buttonstyle width="13px"></buttonstyle>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Challan Number <span class="red">*</span></label>
                    <div>
                        <asp:TextBox ID="txtChallanNumber" runat="server" Width="95%" MaxLength="30" CssClass="form-control">   </asp:TextBox>
                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>LCO Code <span class="red">*</span></label>
                    <div>
                        <input type="text" id="txtLCOCode" class="form-control" runat="server" maxlength="20" onblur="Devicenumber_change();" />
                    </div>

                </div>
                <div class="col-md-2">
                    <label>LCO Name </label>
                    <div class="dropDev">
                        <%--<asp:TextBox ID="txtLCOName" runat="server" Width="95%" MaxLength="30" CssClass="form-control"></asp:TextBox>--%>
                        <input type="text" class="form-control" maxlength="200" id="txtLCOName" runat="server" disabled />
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Vendor <span class="red">*</span></label>
                    <div class="dropDev">
                        <%--<asp:TextBox ID="txtMSO" runat="server" Width="95%" MaxLength="30" CssClass="form-control">   </asp:TextBox>--%>
                         <dxe:ASPxComboBox ID="ddlMSO" runat="server" ClientInstanceName="cddlMSO" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>STB Model <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="STBModel" runat="server" ClientInstanceName="cSTBModel" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="col-md-1">
                    <label>Type <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="DiviceTyp" runat="server" ClientInstanceName="cDiviceTyp" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                
                <div class="col-md-1">
                    <label>Quantity <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxTextBox ID="txtQuantity" ClientInstanceName="ctxtQuantity" runat="server" Width="100%">
                            <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                            <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                
                <div class="col-md-6">
                    <label>Remarks</label>
                    <div>
                        <input type="text" id="txtRemarks" class="form-control" runat="server" />
                    </div>
                </div>
                <div class="col-md-6">
                    <label>&nbsp;</label>
                    <div>
                        <button type="button" id="btnAdd" class="btn btn-success" onclick="AddDevice();"><i class="fa fa-plus-circle"></i>Add</button>
                    </div>
                </div>
                <div class="clear"></div>
            </div>
        </div>

        <div style="margin-top: 10px">
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

                     <dxe:GridViewDataTextColumn Caption="LocationId" FieldName="LocationId"
                        VisibleIndex="1" FixedStyle="Left" Width="200px" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="MSOId" FieldName="MSOId"
                        VisibleIndex="1" FixedStyle="Left" Width="200px" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="STBModelID" FieldName="STBModelID"
                        VisibleIndex="1" FixedStyle="Left" Width="200px" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Location" FieldName="Location" VisibleIndex="2" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Challan Date" FieldName="ChallanDate" Width="100px" VisibleIndex="3">
                        <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Challan Number" FieldName="ChallanNumber" VisibleIndex="4" Width="190px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="LCO Code" FieldName="LCOCode" VisibleIndex="5" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="LCO Name" FieldName="LCOName" VisibleIndex="6" Width="170px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                   
                    <dxe:GridViewDataTextColumn Caption="Vendor" FieldName="MSO" VisibleIndex="7" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="STB Model" FieldName="STBModel" VisibleIndex="7" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Type" FieldName="DeviceType" VisibleIndex="8" Width="100px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity" VisibleIndex="9" Width="80px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks" VisibleIndex="10" Width="150px">
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
                <%--<clientsideevents endcallback="grid_EndCallBack" />--%>
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

        <div style="margin-top: 15px;" id="divsave" runat="server">
            <div class="col-md-12">
                <button type="button" onclick="SaveButtonClick('new');" id="btnSaveNew" class="btn btn-success">Save & New</button>
                <button type="button" onclick="SaveButtonClick('Exit');" id="btnSaveExit" class="btn btn-primary">Save & Exit</button>
                <button type="button" class="btn btn-danger" onclick="cancel();">Cancel</button>
                <%-- <label class="checkbox-inline"> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</label>
            <label class="checkbox-inline">
                <input type="checkbox" value="" id="chkSendSMS" disabled checked />Send SMS
            </label>--%>
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
    <asp:HiddenField runat="server" ID="hdnSTBReceivedID" />
    <asp:HiddenField runat="server" ID="hdnEntryTypeID" />
    <asp:HiddenField runat="server" ID="hdnEntityCode" />

    <asp:HiddenField runat="server" ID="hdnOnlinePrint" />

    <asp:HiddenField runat="server" ID="hdnServiceHistoryValidation" Value="0" />

</asp:Content>
