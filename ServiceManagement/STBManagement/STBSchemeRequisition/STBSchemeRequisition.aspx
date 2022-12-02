<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="STBSchemeRequisition.aspx.cs" Inherits="ServiceManagement.STBManagement.STBSchemeRequisition.STBSchemeRequisition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,100;0,200;0,300;0,400;0,500;0,800;0,900;1,600&display=swap" rel="stylesheet" />

    <link href="/assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <script src="/assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>

    <link href="../../ServiceManagement/Transaction/CSS/ReceiptChallan.css" rel="stylesheet" />
    <script src="../JS/STBRequisition.js?v=2.0"></script>
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
    <style>
        #dataTable > tbody > tr > td {
            white-space: nowrap;
        }
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

        .pRightTbl > tbody > tr > td {
            padding-right: 20px;
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
            <h3>
                <%-- Add STB Requisition--%>
                <span id="spnHeaderName">
                    <asp:Label runat="server" ID="HeaderName"></asp:Label></span>
            </h3>
        </div>
        <div id="divcross" runat="server" class="crossBtn"><a href="STBSchemeRequisitionList.aspx"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main clearfix">
        <div class="pmsForm slick boxModel clearfix">
            <div class="row" style="margin-top: 5px">

                <div class="col-md-2" id="divNumberingScheme" runat="server">
                    <label>Numbering Scheme <span class="red">*</span></label>
                    <div id="divScheme" class="dropDev">
                        <dxe:ASPxComboBox ID="CmbScheme" runat="server" ClientInstanceName="cCmbScheme"
                            Width="100%">
                            <clientsideevents selectedindexchanged="CmbScheme_ValueChange" />
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-2 noLabelmargin">
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
                <div class="col-md-2 noLabelmargin">
                    <label>Requisition Type <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlRequisitionType" runat="server" ClientInstanceName="cddlRequisitionType" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-2 noLabelmargin">
                    <label>Requisition For  <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlRequisitionFor" runat="server" ClientInstanceName="cddlRequisitionFor" Width="100%">
                            <clientsideevents selectedindexchanged="ddlRequisitionFor_ValueChange" />
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="clear"></div>
                <div class="col-md-3">
                    <label>STB Scheme Received <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" id="txtSTBSchemeReceived" maxlength="300" runat="server" disabled />

                    </div>
                </div>
                <hr class="hrBoder" />

            </div>

            <div class="row">
                <div class="col-md-3">
                    <label>Entity Code <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" id="txtEntityCode" maxlength="300" runat="server" onblur="Devicenumber_change()" />

                    </div>
                </div>
                <div class="col-md-3">
                    <label>Network Name <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" id="txtNetworkName" maxlength="300" runat="server" disabled />
                    </div>
                </div>
                <div class="col-md-2 mtop8">
                    <label>Contact Person <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" maxlength="200" id="txtContactPerson" runat="server" disabled />
                    </div>
                </div>
                <div class="col-md-2 mtop8">
                    <label>Contact Number <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" id="txtContactNo" maxlength="15" runat="server" disabled />
                    </div>
                </div>
                <div class="col-md-2 mtop8">
                    <label>DAS <span class="red">*</span></label>
                    <div>
                        <input type="text" class="form-control" id="txtDAS" maxlength="15" runat="server" disabled />
                    </div>
                </div>
            </div>
        </div>
        <div class="headerPy">STB Details</div>
        <div class="pmsForm slick boxModel clearfix" style="border-radius: 0 0 5px 5px" id="divDetails1EntryLeven" runat="server">
            <div class="row">
                <div class="col-md-2">
                    <label>Model  <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlModel" runat="server" ClientInstanceName="cddlModel" Width="100%">
                            <clientsideevents selectedindexchanged="ddlModel_ValueChange" />
                        </dxe:ASPxComboBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Unit Price  </label>
                    <div class="dropDev">
                        <dxe:ASPxTextBox ID="txtUnitPrice" ClientInstanceName="ctxtUnitPrice" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
                            <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                            <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                            <clientsideevents lostfocus="OnUnitPriceLostFocus" />
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Quantity  <span class="red">*</span></label>
                    <div class="dropDev">
                        <dxe:ASPxTextBox ID="txtQuantity" ClientInstanceName="ctxtQuantity" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
                            <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                            <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                            <clientsideevents lostfocus="OnQuantityLostFocus" />
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Amount</label>
                    <div class="dropDev">
                        <dxe:ASPxTextBox ID="txtAmount" ClientInstanceName="ctxtAmount" runat="server" Width="100%" DisplayFormatString="{0:0.00}">
                            <masksettings mask="<0..999999999>.<0..99>" allowmousewheel="false" />
                            <validationsettings requiredfield-isrequired="false" errordisplaymode="None"></validationsettings>
                        </dxe:ASPxTextBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <label>Vendor</label>
                    <div class="dropDev">
                        <dxe:ASPxComboBox ID="ddlOSTBVendor" runat="server" ClientInstanceName="cddlOSTBVendor" Width="100%">
                        </dxe:ASPxComboBox>
                    </div>
                </div>
            </div>
            <div class="row ">
                <div class="col-md-6">
                    <label>Remarks</label>
                    <div>
                        <textarea class="form-control textareaBig" id="txtRemarks" maxlength="500"></textarea>
                    </div>
                </div>
                <div class="col-md-6" style="margin-top: 23px;">
                    <button type="button" id="btnAdd" class="btn btn-success" onclick="AddDevice()"><i class="fa fa-plus-circle mr-2"></i>Add</button>
                </div>
            </div>
        </div>
        <div style="margin-top: 2px;">
            <div class="row">
                <div class="col-md-12" style="padding-left: 15px;">
                    <dxe:ASPxGridView ID="GrdDevice" runat="server" KeyFieldName="HIddenID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                        SettingsPager-Mode="ShowAllRecords" OnDataBinding="grid_DataBinding" OnCustomCallback="GrdDevice_CustomCallback"
                        Width="100%" ClientInstanceName="cGrdDevice">
                        <%--  Settings-HorizontalScrollBarMode="auto"--%>
                        <settingssearchpanel visible="false" delay="5000" />
                        <columns>
                    <dxe:GridViewDataTextColumn Caption="ID" FieldName="HIddenID"
                        VisibleIndex="0" FixedStyle="Left" Width="0%" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Model" FieldName="Model"
                        VisibleIndex="1" FixedStyle="Left" Width="20%" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Unit Price" FieldName="UnitPrice"
                        VisibleIndex="2" FixedStyle="Left" Width="15%" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                        VisibleIndex="3" FixedStyle="Left" Width="10%" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="Amount " FieldName="Amount"
                        VisibleIndex="4" FixedStyle="Left" Width="15%" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                     <dxe:GridViewDataTextColumn Caption="OSTBVendor" FieldName="OSTBVendor"
                        VisibleIndex="5" FixedStyle="Left" Width="20%" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Remarks" FieldName="Remarks"
                        VisibleIndex="6" FixedStyle="Left" Width="20%" >
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="Model_ID" FieldName="Model_ID"
                        VisibleIndex="12" FixedStyle="Left" Width="0%" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>                        
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataTextColumn Caption="OSTBVendor_ID" FieldName="OSTBVendor_ID"
                        VisibleIndex="13" FixedStyle="Left" Width="0%" Visible="false">
                        <CellStyle CssClass="gridcellleft" Wrap="true">
                        </CellStyle>                        
                    </dxe:GridViewDataTextColumn>

                   <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="9" Width="20%">
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

                        <settings showgrouppanel="False" showfooter="false" showstatusbar="Hidden" showhorizontalscrollbar="False" showfilterrow="true" showfilterrowmenu="true" />
                        <settingsloadingpanel text="Please Wait..." />
                        <totalsummary>
                <%--<dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />--%>
            </totalsummary>
                    </dxe:ASPxGridView>
                </div>
            </div>
        </div>


        <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divsave"
            Modal="True">
        </dxe:ASPxLoadingPanel>

        <div class="row">
            <div class="col-md-12 mtop8">
                <button type="button" onclick="SaveButtonClick('new');" id="btnSaveNew" class="btn btn-success">Save & New</button>
                <button type="button" onclick="SaveButtonClick('Exit');" id="btnSaveExit" class="btn btn-primary">Save & Exit</button>
                <button type="button" class="btn btn-danger" onclick="cancel();">Cancel</button>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdAddEdit" runat="server" />
    <asp:HiddenField runat="server" ID="hdnSTBRequisitionID" />
    <asp:HiddenField runat="server" ID="hdnOnlinePrint" />
    <asp:HiddenField runat="server" ID="hdnGuid" />
    <asp:HiddenField runat="server" ID="hdnGuid2" />
    <asp:HiddenField runat="server" ID="hdnIsCancel" />
    <asp:HiddenField runat="server" ID="hdnIsAproval" />

    <asp:HiddenField runat="server" ID="hdnLastFiveRequisitionValidation" Value="0" />
    <asp:HiddenField runat="server" ID="hdnIsInventory" />

    <asp:HiddenField runat="server" ID="hdnSTBSchemeReceived" />

</asp:Content>
