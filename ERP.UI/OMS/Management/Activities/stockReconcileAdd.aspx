<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="stockReconcileAdd.aspx.cs" Inherits="ERP.OMS.Management.Activities.stockReconcileAdd" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="JS/SearchPopup.js"></script>
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <style>
        .py-3 {
            padding: 10px 0;
        }

        a.dxbButtonSys > span {
            display: none !important;
        }
       #MandatoryRemarks {
            position: absolute;
    right: -20px;
    top: 0;
        }
    </style>

    <script>


        $(document).ready(function () {
            setTimeout(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                }
            }, 1000);
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                }

            });
        });

        var globalRowIndex;


        $(document).ready(function () {

            if ($("#hdnPageStatus").val() == "Edit") {
                $("#drdNumSchema").hide();
                ctxt_CustDocNo.SetEnabled(false);

                //cCmbWarehouse.SetValue($("#hdnWarehouseValueF").val());
                cCmbWarehouse.SetText($("#hdnWarehouseTextF").val());
                cCmbWarehouse.SetEnabled(false);
                $("#btn_SaveRecords").hide();
                $("#dvShow").hide();
            }
            else {
                $("#btn_SaveRecords").show();
                $("#drdNumSchema").show();
            }

            $('#ddl_numberingScheme').change(function () {

                var NoSchemeTypedtl = $(this).val();
                var NumSchemaId = NoSchemeTypedtl.toString().split('~')[0];

                $("#NumId").val(NumSchemaId);

                var NoSchemeType = NoSchemeTypedtl.toString().split('~')[1];
                var quotelength = NoSchemeTypedtl.toString().split('~')[2];

                // var branchID = (NoSchemeTypedtl.toString().split('~')[3] != null) ? NoSchemeTypedtl.toString().split('~')[3] : "";
                // if (branchID != "") document.getElementById('ddl_Branch').value = branchID;




                var fromdate = NoSchemeTypedtl.toString().split('~')[4];
                var todate = NoSchemeTypedtl.toString().split('~')[5];

                var dt = new Date();
                tstartdate.SetDate(dt);


                if (dt < new Date(fromdate)) {
                    tstartdate.SetDate(new Date(fromdate));
                }

                if (dt > new Date(todate)) {
                    tstartdate.SetDate(new Date(todate));
                }
                //tstartdate.SetMinDate(new Date(fromdate));
                //tstartdate.SetMaxDate(new Date(todate));





                if (NoSchemeType == '1') {
                    ctxt_CustDocNo.SetText('Auto');
                    ctxt_CustDocNo.SetEnabled(false);


                    tstartdate.Focus();
                }
                else if (NoSchemeType == '0') {
                    ctxt_CustDocNo.SetEnabled(true);
                    ctxt_CustDocNo.GetInputElement().maxLength = quotelength;

                    ctxt_CustDocNo.SetText('');
                    ctxt_CustDocNo.Focus();
                }


            });


        });

        function OnEndCallback(s, e) {


            if (grid.cpSuccess == "Success") {

                $("#hdfIsDelete").val('');
                $('#<%=hdfIsDelete.ClientID %>').val('');
                LoadingPanel.Hide();
                grid.cpSuccess = null;
                jAlert("Reconcile and save  successfully.");

                window.location.assign("stockReconcileList.aspx");
                //if (grid.cpDocId != "") {
                //    var keyValue = grid.cpDocId
                //var url = 'OMS/Management/Activities/stockReconcileAdd.aspx?key=' + keyValue;
                //window.location.href = url;
                // }
            }
            //else if()
        }

        function Save_PostToStkAdj() {
            cGrdQuotation.Refresh();
            jConfirm('Are you sure to create Stock Adjustments for all the items with updated physical stock Qty? ', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cPopup_PostStlAdj.Show();
                }
            });


        }

        function StockAdjustComplete()
        {
            var Quote_Msg = "Stock adjustment successfully done.";
            jAlert(Quote_Msg, 'Alert Dialog: [StockReconcile]', function (r) {
                if (r == true) {
                   

                   
                   
                    window.location.assign("stockReconcileList.aspx");
                    
                }
            });

            //jAlert("Stock adjustment successfully done.");
            //var URL = '/OMS/Management/Activities/stockReconcileList.aspx';
            //window.location.href = URL;
        }


        function StkAdj_CancelClick() {
            var URL = '/OMS/Management/Activities/stockReconcileList.aspx';
            window.location.href = URL;
        }

        function StkAdj_CloseClick() {
            cPopup_PostStlAdj.Hide();
        }

        function StkAdj_SaveClick() {

        }



        function Save_ButtonClick() {
            LoadingPanel.Show();

            flag = true;
            grid.batchEditApi.EndEdit();
            var doc_no = ctxt_CustDocNo.GetText();
            var warehouseId = cCmbWarehouse.GetValue();
            var NumberingId = $("#ddl_numberingScheme").val();
            var Remarks = $("#txtRemarks").val();
            if (NumberingId == "0" && $("#hdnPageStatus").val() == "ADD") {
                LoadingPanel.Hide();
                jAlert("Plaese Select Numberiung Schema.");
                flag = false;
                return false;
            }
            if (NumberingId == "" && $("#hdnPageStatus").val() == "ADD") {
                LoadingPanel.Hide();
                jAlert("Plaese Select Numberiung Schema.");
                flag = false;
                return false;
            }
            if (doc_no == "") {
                LoadingPanel.Hide();
                jAlert("Plaese enter document no.");
                flag = false;
                return false;
            }

            if (warehouseId == "" || warehouseId == null) {
                LoadingPanel.Hide();
                jAlert("Plaese select Warehouse.");
                flag = false;
                return false;
            }
            if (Remarks == null || Remarks == "") {
                LoadingPanel.Hide();
                $("#MandatoryRemarks").show();
                flag = false;
                return false;
            }

            if (grid.GetVisibleRowsOnPage() == 0 || grid.GetVisibleRowsOnPage() < 0) {
                LoadingPanel.Hide();
                flag = false;
                return false;
            }


            $('#<%=hdfIsDelete.ClientID %>').val('I');

            grid.AddNewRow();
            grid.UpdateEdit();

        }


        function OnCustomButtonClick(s, e) {



        }


        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
        }
        function gridFocusedRowChanged(s, e) {
            globalRowIndex = e.visibleIndex;


        }

        function PerformCallToGridBind() {
            flag = true;            
            var doc_no = ctxt_CustDocNo.GetText();
            var warehouseId = cCmbWarehouse.GetValue();
            var NumberingId = $("#ddl_numberingScheme").val();
          
            if (NumberingId == "0" && $("#hdnPageStatus").val() == "ADD") {
                LoadingPanel.Hide();
                jAlert("Plaese Select Numberiung Schema.");
                flag = false;
                return false;
            }
            if (NumberingId == "" && $("#hdnPageStatus").val() == "ADD") {
                LoadingPanel.Hide();
                jAlert("Plaese Select Numberiung Schema.");
                flag = false;
                return false;
            }
            if (doc_no == "") {
                LoadingPanel.Hide();
                jAlert("Plaese enter document no.");
                flag = false;
                return false;
            }

            if (warehouseId == "" || warehouseId == null) {
                LoadingPanel.Hide();
                jAlert("Select Warehouse and proceed.");
                flag = false;
                return false;
            }            
            
            grid.PerformCallback('BindGrid');
        }


    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <%--  Stock Reconcile Add--%>
                <asp:Label ID="lblHeadTitle" Text="" runat="server"></asp:Label>

            </h3>

            <div class="crossBtn"><a href="stockReconcileList.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <div class=" clearfix py-3">
            <div class="col-md-3" id="drdNumSchema" runat="server">
                <label>Numbering Scheme <span class="red">*</span></label>
                <div>
                    <%--   <select class="form-control">
                    <option>Select</option>
                </select>--%>
                    <asp:DropDownList ID="ddl_numberingScheme" runat="server" Width="100%">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-3">
                <label>Document Number <span class="red">*</span></label>
                <div>
                    <dxe:ASPxTextBox ID="txt_CustDocNo" runat="server" ClientInstanceName="ctxt_CustDocNo" Width="100%" MaxLength="50">
                        <%--    <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" />--%>
                    </dxe:ASPxTextBox>
                </div>
            </div>
            <div class="col-md-3">
                <label>Posting date <span class="red">*</span></label>
                <div>
                    <dxe:ASPxDateEdit ID="dt_PLQuote" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="tstartdate" TabIndex="3" Width="100%" UseMaskBehavior="True">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <ClientSideEvents GotFocus="function(s,e){tstartdate.ShowDropDown();}" />
                    </dxe:ASPxDateEdit>
                </div>
            </div>
            <div class="col-md-3">
                <label>Warehouse Name <span class="red">*</span></label>
                <div>
                    <dxe:ASPxComboBox ID="CmbWarehouse" EnableIncrementalFiltering="True" ClientInstanceName="cCmbWarehouse" SelectedIndex="0"
                        TextField="WarehouseName" ValueField="WarehouseID" runat="server" Width="100%">

                        <%--   <ClientSideEvents SelectedIndexChanged="CmbWarehouse_ValueChange" GotFocus="CmbWarehouse_GotFocus" />--%>
                    </dxe:ASPxComboBox>
                </div>
            </div>

            <div class="clear"></div>
            <div class="col-md-6">
                <label>Remarks</label>
                <div class="relative">
                    <textarea class="form-control" style="height: 70px;" id="txtRemarks" runat="server"></textarea>
                    <span id="MandatoryRemarks" class="voucherno  pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>

                </div>
            </div>
            <div class="col-sm-2" style="padding-top: 24px;" runat="server" id="dvShow">
                <asp:Button ID="Button3" runat="server" Text="Show" CssClass="btn btn-success  mLeft mTop" OnClientClick="return PerformCallToGridBind();" UseSubmitBehavior="false" />
            </div>
        </div>

        <div class="py-3">

            <div class="gridArea">
                <dxe:ASPxGridView runat="server"
                    ClientInstanceName="grid" ID="grid" KeyFieldName="sProducts_ID" AutoGenerateColumns="false"
                    Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                    OnBatchUpdate="grid_BatchUpdate"
                    OnCustomCallback="grid_CustomCallback"
                    OnDataBinding="grid_DataBinding"
                    OnCellEditorInitialize="grid_CellEditorInitialize"
                    OnRowInserting="Grid_RowInserting"
                    OnRowUpdating="Grid_RowUpdating"
                    OnRowDeleting="Grid_RowDeleting"
                    SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="300" Settings-HorizontalScrollBarMode="Visible">
                    <SettingsPager Visible="true"></SettingsPager>
                    <Columns>
                        <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="3%" VisibleIndex="0" Caption="#" Visible="false">
                            <CustomButtons>
                                <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                                    <Image Url="/assests/images/crs.png">
                                    </Image>
                                </dxe:GridViewCommandColumnCustomButton>
                            </CustomButtons>
                            <%-- <HeaderCaptionTemplate>
                                                            <dxe:ASPxHyperLink ID="btnNew" runat="server" Text="New" ForeColor="White">
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}" />
                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>--%>
                        </dxe:GridViewCommandColumn>
                        <dxe:GridViewDataTextColumn FieldName="SrlNo" Caption="Sl" ReadOnly="true" VisibleIndex="1" Width="2%" Settings-AllowAutoFilter="False">
                            <PropertiesTextEdit>
                            </PropertiesTextEdit>
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataButtonEditColumn FieldName="Product" Caption="Product" VisibleIndex="2" Width="14%" ReadOnly="true">
                        </dxe:GridViewDataButtonEditColumn>
                        <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="3" ReadOnly="True" Width="18%">
                            <CellStyle Wrap="True"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                         <dxe:GridViewDataTextColumn FieldName="sProduct_Status" Caption="Status" VisibleIndex="4" ReadOnly="True" Width="18%">
                            <CellStyle Wrap="True"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="Class" Caption="Class" VisibleIndex="5" ReadOnly="true" Width="180px">
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="StockUnit" Caption="Stock Unit" VisibleIndex="6" Width="120px" ReadOnly="true">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="AltUnit" Caption="Alt. Unit" VisibleIndex="7" ReadOnly="true" Width="120px">
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="CLOSE_QTY" Caption="Stock Unit Qty" VisibleIndex="8" Width="9%" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                            <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">


                                <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..9999&gt;" />
                                <Style HorizontalAlign="Right">
                                                            </Style>
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="ALTCLOSE_QTY" Caption="Alt. Unit Qty" VisibleIndex="9" Width="9%" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                            <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">

                                <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..9999&gt;" />
                                <Style HorizontalAlign="Right">
                                                            </Style>
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn FieldName="StockUnitQnty" Caption="Physical(Stk Unit)" VisibleIndex="10" Width="9%" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                            <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">


                                <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..9999&gt;" />
                                <Style HorizontalAlign="Right">
                                                            </Style>
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="AltUnitQnty" Caption="Physical(Alt. Unit)" VisibleIndex="11" Width="9%" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                            <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">

                                <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..9999&gt;" />
                                <Style HorizontalAlign="Right">
                                                            </Style>
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn FieldName="DiffStockUnitQnty" Caption="Diff(Main Unit)" VisibleIndex="12" Width="9%" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                            <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                <%-- <ClientSideEvents LostFocus="PopulateMultiUomAltQuantity" GotFocus="StockQuantityGotFocus" />--%>

                                <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..9999&gt;" />
                                <Style HorizontalAlign="Right">
                                                            </Style>
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn FieldName="DiffAltUnitQnty" Caption="Diff(Alt. Unit)" VisibleIndex="13" Width="9%" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                            <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                <%-- <ClientSideEvents LostFocus="PopulateMultiUomAltQuantity" GotFocus="StockQuantityGotFocus" />--%>

                                <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..9999&gt;" />
                                <Style HorizontalAlign="Right">
                                                            </Style>
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn FieldName="Price" Caption="Price" VisibleIndex="14" Width="9%" HeaderStyle-HorizontalAlign="Right" ReadOnly="true">
                            <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right">
                                <%-- <ClientSideEvents LostFocus="PopulateMultiUomAltQuantity" GotFocus="StockQuantityGotFocus" />--%>

                                <MaskSettings AllowMouseWheel="False" Mask="&lt;-999999999..999999999&gt;.&lt;00..9999&gt;" />
                                <Style HorizontalAlign="Right">
                                                            </Style>
                            </PropertiesTextEdit>
                            <HeaderStyle HorizontalAlign="Right" />
                            <CellStyle HorizontalAlign="Right"></CellStyle>
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn FieldName="ProductID" PropertiesTextEdit-ValidationSettings-ErrorImage-IconID="ghg" Caption="hidden Field Id" VisibleIndex="15" ReadOnly="True" Width="0" PropertiesTextEdit-Height="15px" PropertiesTextEdit-Style-CssClass="abcd">
                            <PropertiesTextEdit Height="15px">
                                <ValidationSettings>
                                    <ErrorImage IconID="ghg">
                                    </ErrorImage>
                                </ValidationSettings>
                                <Style CssClass="abcd">
                                                            </Style>
                            </PropertiesTextEdit>
                            <CellStyle Wrap="True" CssClass="abcd"></CellStyle>
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn FieldName="ClassId" Caption="hidden Field Id" VisibleIndex="16" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                            <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                        </dxe:GridViewDataTextColumn>


                        <dxe:GridViewDataTextColumn FieldName="BrandId" Caption="hidden Field Id" VisibleIndex="17" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                            <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="StockUnitId" Caption="hidden Field Id" VisibleIndex="18" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                            <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="AltUnitId" Caption="hidden Field Id" VisibleIndex="19" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide" PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide" PropertiesTextEdit-Height="15px">
                            <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                        </dxe:GridViewDataTextColumn>


                    </Columns>
                    <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />

                    <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" Mode="ShowPager">
                        <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="20,50,100,200,300,400,500,600,700,800,900,1000" />
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <SettingsDataSecurity AllowEdit="true" />
                    <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                        <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
                    </SettingsEditing>
                    <Settings VerticalScrollBarMode="Auto" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsBehavior ColumnResizeMode="Disabled" />
                </dxe:ASPxGridView>
            </div>

            <div class="clear">
                <br />
            </div>
            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Reconcile & New" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
            </dxe:ASPxButton>

            <dxe:ASPxButton ID="ASPxButton4" ClientInstanceName="cbtn_StkAdjcancel" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                <%--      <a href="stockReconcileList.aspx"></a>--%>
                <ClientSideEvents Click="function(s, e) {StkAdj_CancelClick();}" />
            </dxe:ASPxButton>

            <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_SaveRecordsPostToStkAdj" runat="server" AutoPostBack="False" Text="Make Stock Adj" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                <ClientSideEvents Click="function(s, e) {Save_PostToStkAdj();}" />
            </dxe:ASPxButton>

        </div>
    </div>
    <asp:HiddenField ID="hdfIsDelete" runat="server" />
    <asp:HiddenField ID="NumId" runat="server" />
    <dxe:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel" ContainerElementID="divSubmitButton"
        Modal="True">
    </dxe:ASPxLoadingPanel>

    <dxe:ASPxPopupControl ID="Popup_PostStlAdj" runat="server" ClientInstanceName="cPopup_PostStlAdj"
        Width="1200px" Height="300px" HeaderText="Stock Adjustment List" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">

                    <asp:DropDownList ID="drdStkAdj" runat="server" CssClass="btn btn-primary btn-radius " OnSelectedIndexChanged="ReconcileStkAdjustmentExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>

                    <div class="clear">
                    </div>

                    <div class="GridViewArea relative">
                        <dxe:ASPxGridView ID="GrdQuotation" runat="server" KeyFieldName="sProducts_ID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                            Width="100%" ClientInstanceName="cGrdQuotation"
                            Settings-HorizontalScrollBarMode="Auto" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false"
                            SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" SettingsBehavior-ColumnResizeMode="Control" Settings-VerticalScrollableHeight="350" Settings-VerticalScrollBarMode="Auto">
                            <SettingsSearchPanel Visible="True" Delay="5000" />
                            <Columns>

                                <dxe:GridViewDataTextColumn FieldName="sProducts_ID" Visible="false" Width="0" VisibleIndex="0">
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>



                                <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="DocNum" SortOrder="Descending" VisibleIndex="1" Width="100px" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Product" FieldName="Product" VisibleIndex="1" Width="100px" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Description" FieldName="Description" VisibleIndex="2" Width="100px" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="Class" FieldName="Class" VisibleIndex="3" Width="100px" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>




                                <dxe:GridViewDataTextColumn FieldName="StockUnit" Caption="Stock Unit" VisibleIndex="4" Width="100px" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="AltUnit" Caption="Alt. Unit" VisibleIndex="5" Width="100px" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="CLOSE_QTY" Caption="Stock Unit Qty" VisibleIndex="6" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="ALTCLOSE_QTY" Caption="Alt. Unit Qty" VisibleIndex="7" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="StockUnitQnty" Caption="Physical(Stk Unit)" VisibleIndex="8" Width="100px" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="AltUnitQnty" Caption="Physical(Alt. Unit)" VisibleIndex="9" Width="100px" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn FieldName="DiffStockUnitQnty" Caption="Diff(Main Unit)" VisibleIndex="10" Width="100px" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="DiffAltUnitQnty" Caption="Diff(Alt. Unit)" VisibleIndex="11" Width="100px" >
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Created By" FieldName="EnteredByName"
                                    VisibleIndex="12" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Created On" FieldName="EnteredByDate"
                                    VisibleIndex="13" Width="150px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="ModifiedByName"
                                    VisibleIndex="14" Width="100px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="ModifiedDate"
                                    VisibleIndex="15" Width="150px">
                                    <CellStyle CssClass="gridcellleft" Wrap="true">
                                    </CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                    <Settings AutoFilterCondition="Contains" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn FieldName="Reconcile_Id" Visible="false" Width="16" VisibleIndex="0">
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                    <Settings AllowAutoFilterTextInputTimer="False" />
                                </dxe:GridViewDataTextColumn>


                            </Columns>
                            <SettingsContextMenu Enabled="true"></SettingsContextMenu>

                            <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                           
                            <GroupSummary>
                            </GroupSummary>

                            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsLoadingPanel Text="Please Wait..." />
                        </dxe:ASPxGridView>

                        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                            ContextTypeName="ERPDataClassesDataContext" TableName="V_ReconcileAdjustmentList" />

                    </div>
                    <div class="clear">
                        <br />
                    </div>



                    <div class="col-sm-12 text-right">


                        <dxe:ASPxButton ID="ASPxButton2" ClientInstanceName="cbtn_StkAdjSave" OnClick="ReconcileStockAdjusted" runat="server" AutoPostBack="False" Text="Submit" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                        </dxe:ASPxButton>

                        <dxe:ASPxButton ID="ASPxButton3" ClientInstanceName="cbtn_close" runat="server" AutoPostBack="False" Text="Close" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                            <ClientSideEvents Click="function(s, e) {StkAdj_CloseClick();}" />
                        </dxe:ASPxButton>

                    </div>

                    <div style="display: none">
                        <dxe:ASPxGridViewExporter ID="Calculateexport" GridViewID="GrdQuotation" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                        </dxe:ASPxGridViewExporter>
                    </div>

                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>


    <asp:HiddenField ID="hdnNumSchema" runat="server" />
    <asp:HiddenField ID="hdnPageStatus" runat="server" />
    <asp:HiddenField ID="hdnPostedToStockCheck" runat="server" />
    <asp:HiddenField ID="hdnWarehouseValueF" runat="server" />
    <asp:HiddenField ID="hdnWarehouseTextF" runat="server" />
    <asp:HiddenField ID="hdnReconcileId" runat="server" />
</asp:Content>
