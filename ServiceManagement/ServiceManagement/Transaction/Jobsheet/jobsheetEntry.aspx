<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="jobsheetEntry.aspx.cs" Inherits="ServiceManagement.ServiceManagement.Transaction.Jobsheet.jobsheetEntry" %>

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
    <script src="../JS/jobsheetEntry.js?v=1.0.0.12"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <span class="hddd font-pp" id="HeaderName" runat="server">Add Jobsheet</span>
    <div id="divcross" runat="server" class="crossBtn pull-right"><a href="jobsheetList.aspx"><i class="fa fa-times"></i></a></div>
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
                    <label>Challan Number <span class="red">*</span></label>
                    <div>
                        <asp:TextBox ID="txtDocumentNumber" runat="server" Width="95%" MaxLength="30" CssClass="form-control">   </asp:TextBox>
                        <span id="MandatoryBillNo" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                    </div>
                </div>
                 <div class="col-md-2">
                    <label>Posting Date</label>
                    <div class="dropDev">
                        <dxe:ASPxDateEdit ID="PostingDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cPostingDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <buttonstyle width="13px"></buttonstyle>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Ref Jobsheet <span class="red">*</span></label>
                    <div>
                        <input type="text" id="txtRefJobsheet" class="form-control" runat="server" maxlength="20" />
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Assign To <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlTechnician" runat="server" ClientInstanceName="cddlTechnician" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>

                <div class="col-md-2">
                    <label>Work done on</label>
                    <div class="dropDev">
                        <%-- <div class="input-group date" data-provide="datepicker">
                            <input type="text" class="form-control" />
                            <div class="input-group-addon">
                                <span class="fa fa-calendar-check-o"></span>
                            </div>
                        </div>--%>
                        <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <buttonstyle width="13px"></buttonstyle>
                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                 <div class="clear"></div>
                <div class="col-md-2">
                    <label>Location</label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlBranch" runat="server" ClientInstanceName="cddlBranch" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
               
                <div class="col-md-4">
                    <label>Remarks</label>
                    <div>
                        <input type="text" id="txtHeaderRemarks" class="form-control" runat="server" />
                    </div>
                </div>
                <div class="clear"></div>
                <hr class="hrBoder" />

            </div>
            <div class="row mTop5">
                <div class="col-md-3">
                    <label>Entity Code <span class="red">*</span></label>
                    <div>
                        <%--Mantis Issue 24412--%>
                        <%--<input type="text" id="txtEntityCode" class="form-control" disabled maxlength="100" runat="server" />--%>
                        <input type="text" id="txtEntityCode" class="form-control" disabled maxlength="100" onblur="get_NetworkName();" runat="server" />
                        <%--End of Mantis Issue 24412--%>
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Network Name <span class="red">*</span></label>
                    <div>
                        <input type="text" id="txtNetworkName" class="form-control" disabled maxlength="200" runat="server" />
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Contact Person <span class="red">*</span></label>
                    <div>
                        <input type="text" id="txtContactPerson" class="form-control" maxlength="200" disabled runat="server" />
                    </div>
                </div>
                <div class="col-md-3">
                    <label>Contact Number <span class="red">*</span></label>
                    <div>
                        <input type="text" id="txtContactNumber" class="form-control" maxlength="15" disabled runat="server" />
                    </div>
                </div>

            </div>
            <div class="row mTop5">
                <div class="col-md-2">
                    <label>Serial Number <span class="red">*</span></label>
                    <div>
                        <input type="text" id="txtDeviceNumber" class="form-control" maxlength="16" runat="server" onblur="Devicenumber_change();" />
                    </div>
                </div>

                <div class="col-md-2">
                    <label>Device Type <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="DiviceTyp" runat="server" ClientInstanceName="cDiviceTyp" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>

                <div class="col-md-2"  >
                    <label>Model <span class="red">*</span></label>
                    <div style="position: relative;">
                        <input type="text" id="txtDeviceModel" class="form-control" maxlength="20" disabled runat="server" style="z-index: 4;position: relative;" />

                        <%--Mantis Issue 24413/24417  [ style="position: relative;" added in above div AND style="z-index: 4;position: relative;"  added in txtModel ] --%>
                         <div class="dropDev" 
                             style="position: absolute;
                                top: 0;
                                z-index: 1;
                                right: -19px;">

                            <dxe:ASPxComboBox ID="selModel" runat="server" ClientInstanceName="cselModel" Width="100%" >
                                <clientsideevents selectedindexchanged="selModel_change" />
                            </dxe:ASPxComboBox>
                        </div>
                         <%--End of Mantis Issue 24413/24417--%>
                    </div>
                </div>
                <div class="col-md-3" style="margin-top: -8px;">
                    <label>Problem Found</label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlProblem" runat="server" ClientInstanceName="cddlProblem" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-3" style="margin-top: -8px;">
                    <label>Other</label>
                    <div>
                        <input type="text" id="txtOtherProblem" class="form-control" runat="server" maxlength="500" runat="server" />
                    </div>
                </div>

            </div>
            <div class="row mTop5">
                <div class="col-md-2">
                    <label>Service Action <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlServiceAction" runat="server" ClientInstanceName="cddlServiceAction" Width="100%">
                            <clientsideevents selectedindexchanged="ddlServiceAction_change" />
                        </dxe:ASPxComboBox>
                    </div>
                </div>

                <div class="col-md-3" style="margin-top: -8px;">
                    <label>Components</label>
                    <div class="dropDev">
                        <%-- <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%">
                            <buttons><dxe:EditButton></dxe:EditButton></buttons>
                            <clientsideevents buttonclick="function(s,e){ProductButnClick();}" keydown="function(s,e){Product_KeyDown(s,e);}" />
                        </dxe:ASPxButtonEdit>--%>

                        <dxe:ASPxCallbackPanel runat="server" ID="ComponentPanel" ClientInstanceName="cComponentPanel" OnCallback="Component_Callback">
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
                                 <ClientSideEvents GotFocus="Component_GotFocus"  />
                            </dxe:ASPxGridLookup>
                        </dxe:PanelContent>
                    </panelcollection>
                        </dxe:ASPxCallbackPanel>
                        <asp:HiddenField ID="hdnSelectedBranches" runat="server" />
                    </div>
                </div>

                <div class="col-md-2">
                    <label>Warranty</label>
                    <div>
                        <div class="input-group date" data-provide="datepicker" id="dtwrnty">
                            <input type="text" class="form-control" id="txtWarranty" runat="server" style="height: 28px;" />
                            <div class="input-group-addon">
                                <span class="fa fa-calendar-check-o"></span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-2 dsl">
                    <label>Return Reason</label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlReturnReason" Height="23px" runat="server" cssClass="" ClientInstanceName="cddlReturnReason" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>

                <div class="col-md-2 hide">
                    <label>New Model</label>
                    <div>
                        <asp:DropDownList ID="ddlModel" runat="server" CssClass="js-example-basic-single" DataTextField="ModelDesc" DataValueField="ModelID" Width="100%">
                        </asp:DropDownList>
                    </div>
                </div>


            </div>
            <div class="row mTop5">

                <div class="col-md-5">
                    <label>Remarks</label>
                    <div>
                        <textarea class="form-control textareaBig" id="txtDetailsRemarks" runat="server" maxlength="500"></textarea>
                    </div>
                </div>
                <div class="col-md-2" style="padding-top: 19px">
                    <div class="checkbox">
                        <label>
                            <input type="checkbox" id="chkBillable" runat="server" />
                            Billable

                        </label>
                    </div>
                </div>
            </div>
             <div class="pdTop15">
                <div class="">
                   <button type="button" id="btnAdd" class="btn btn-success" onclick="JobSheetEntryAdd()"><i class="fa fa-plus-circle mr-2"></i>Add</button>
                    <button type="button" class="btn btn-info" onclick="ShowRepeatHistory()"><i class="fa fa-history" style="margin: 0"></i>&nbsp;Repeat History</button>
                    <button type="button" class="btn btn-warning" data-toggle="modal" data-target="#srrHist" onclick="BindServiceEntryHistory();"><i class="fa fa-wrench" style="margin: 0"></i>&nbsp;Service History</button>
                </div>
            </div>
            <div class="row mTop5">
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

                     <dxe:GridViewDataTextColumn Caption="DeviceTypeID" FieldName="DeviceTypeId"
                        VisibleIndex="1" FixedStyle="Left" Width="200px" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="DetailsID" FieldName="DetailsID"
                        VisibleIndex="1" FixedStyle="Left" Width="200px" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="ServiceActionID" FieldName="ServiceActionID"
                        VisibleIndex="1" FixedStyle="Left" Width="200px" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="ComponentsID" FieldName="ComponentsID"
                        VisibleIndex="1" FixedStyle="Left" Width="200px" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="ReturnReasonID" FieldName="ReturnReasonID"
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

                    <dxe:GridViewDataTextColumn Caption="Serial Number" FieldName="SerialNumber"
                        VisibleIndex="2" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Device Type" FieldName="DeviceType"
                        VisibleIndex="3" Width="100px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Model" FieldName="Model"
                        VisibleIndex="4" Width="100px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Problem Found" FieldName="Problem"
                        VisibleIndex="5" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                   
                    <dxe:GridViewDataTextColumn Caption="Other" FieldName="Other"
                        VisibleIndex="6" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Service Action" FieldName="ServiceAction"
                        VisibleIndex="7" Width="150px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Components" FieldName="Components"
                        VisibleIndex="8" Width="200px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Warranty" FieldName="Warranty" Width="100px" VisibleIndex="9">
                        <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>  
                        
                     <dxe:GridViewDataTextColumn Caption="Return Reason" FieldName="ReturnReason"
                        VisibleIndex="10" Width="100px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>                  

                    <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks"
                        VisibleIndex="11" Width="200px">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Billable" FieldName="Billable"
                        VisibleIndex="12" Width="100px">
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
            <div class="pdTop15" id="divsave" runat="server">
                <div class="">
                    <button type="button" class="btn btn-success" id="btnSaveNew" onclick="SaveJob('New');">Save & New</button>
                    <button type="button" class="btn btn-primary" id="btnSaveExit" onclick="SaveJob('Exit');">Save & Exit</button>
                    <button type="button" class="btn btn-danger" onclick="">Cancel</button>
                    <%--<button type="button" class="btn btn-info" onclick="ShowRepeatHistory()"><i class="fa fa-history" style="margin: 0"></i>&nbsp;Repeat History</button>--%>
                    <%--<button type="button" class="btn btn-warning" data-toggle="modal" data-target="#srrHist" onclick="BindServiceEntryHistory();"><i class="fa fa-wrench" style="margin: 0"></i>&nbsp;Service History</button>--%>
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
                            <%--<tbody>
                                    <tr>
                                        <td>sasgasg</td>
                                        <td>sasgasg</td>
                                        <td>sasgasg</td>
                                        <td>sasgasg</td>
                                    </tr>
                                </tbody>--%>
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

    <asp:HiddenField ID="hdnSchemaType" runat="server" />
    <asp:HiddenField runat="server" ID="hdnGuid" />
    <asp:HiddenField ID="hdnSchemaID" runat="server" />
    <asp:HiddenField runat="server" ID="Hidden_add_edit" />
    <asp:HiddenField runat="server" ID="hdnJobSheetID" />
    <asp:HiddenField runat="server" ID="hdnEntryTypeID" />
    <asp:HiddenField runat="server" ID="hdnEntityCode" />
    <asp:HiddenField runat="server" ID="hdnDetailsID" />
    <asp:HiddenField runat="server" ID="hdnOnlinePrint" />

    <asp:HiddenField ID="hdnEntryMode" runat="server" />
      <asp:HiddenField ID="hdnComponentQty" runat="server" />
    <asp:HiddenField runat="server" ID="hdnIsEntityInformationEditableInJobsheet" />

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
