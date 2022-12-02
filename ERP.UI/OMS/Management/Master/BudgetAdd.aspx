<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/PopUp.Master" 
    CodeBehind="BudgetAdd.aspx.cs" Inherits="ERP.OMS.Management.Master.BudgetAdd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style type="text/css">
        .inline {
            display: inline-block !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }


        .abs {
            position: absolute;
            right: -20px;
            top: 10px;
        }

        .fa.fa-exclamation-circle:before {
            font-family: FontAwesome;
        }

        .tp2 {
            right: -18px;
            top: 7px;
            position: absolute;
        }

        #txtCreditLimit_EC {
            position: absolute;
        }

        #grid1_DXStatus span > a {
            display: none;
        }

        #aspxGridTax_DXEditingErrorRow0 {
            display: none;
        }

        .horizontal-images.content li {
            float: left;
        }

        #grid1_DXMainTable > tbody > tr > td:last-child {
            display: none !important;
        }

        #taxroundedOf, #chargesRoundOf {
            font-weight: 600;
            font-size: 15px;
            color: #7f0826;
        }
    </style>
    <script>


       


        function ProductButnClick(s, e) {
            if (e.buttonIndex == 0) {

                if (cproductLookUp.Clear()) {
                    cProductpopUp.Show();
                    cproductLookUp.Focus();
                    cproductLookUp.ShowDropDown();
                }
            }
        }
        function ProductlookUpKeyDown(s, e) {
            if (e.htmlEvent.key == "Escape") {
                cProductpopUp.Hide();
                grid1.batchEditApi.StartEdit(globalRowIndex, 5);
            }
        }

        function ProductSelected(s, e) {
            if (cproductLookUp.GetGridView().GetFocusedRowIndex() == -1) {
                cProductpopUp.Hide();
                grid1.batchEditApi.StartEdit(globalRowIndex, 5);
                return;
            }
            var LookUpData = cproductLookUp.GetGridView().GetRowKey(cproductLookUp.GetGridView().GetFocusedRowIndex());
            var ProductCode = cproductLookUp.GetValue();
            if (!ProductCode) {
                LookUpData = null;
            }
            console.log(LookUpData);
            cProductpopUp.Hide();



            //alert(s.GetSelectedFieldValues("ProductID", GetSelectedFieldValuesCallback));


            grid1.batchEditApi.StartEdit(globalRowIndex);
            grid1.GetEditor("ProductID").SetText(LookUpData);
            grid1.GetEditor("ProductName").SetText(ProductCode);

            ////  grid1.GetRowValues(grid1.GetFocusedRowIndex(), 'ProductName', OnGetRowValues);

            //  pageheaderContent.style.display = "block";

            var tbDescription = grid1.GetEditor("Description");
            var tbUOM = grid1.GetEditor("UOM");
            var tbclass = grid1.GetEditor("Productclass");
            var tbIndustry = grid1.GetEditor("Industry");

            //  var ProductID = (grid1.GetEditor('ProductID').GetText() != null) ? grid1.GetEditor('ProductID').GetText() : "0";
            var ProductID = LookUpData;
            var SpliteDetails = ProductID.split("||@||");
            var strProductID = SpliteDetails[0];
            var strDescription = SpliteDetails[1];
            var strUOM = SpliteDetails[2];
            var strStkUOM = SpliteDetails[4];
            var strclasscode = SpliteDetails[17];
            var strIndustry = SpliteDetails[18];

            //  var QuantityValue = (grid1.GetEditor('Quantity').GetValue() != null) ? grid1.GetEditor('Quantity').GetValue() : "0";


            tbDescription.SetValue(strDescription);
            tbUOM.SetValue(strUOM);
            tbclass.SetValue(strclasscode);
            tbIndustry.SetValue(strIndustry);
            //   grid1.GetEditor("Quantity").SetValue("0.00");

        }



        //function OnGetRowValues(values) {
        //    alert(values[0]);
        //}

        //$(function () {  
        //    debugger;
        //    OnAddNewClick();
        //});


        function OnAddNewClick() {
            //debugger;
            grid1.PerformCallback();
            //grid1.AddNewRow();

            //$("#hdnchkgridbatch").val('AddNew');
            //grid1.AddNewRow();
            //$("#hdnchkgridbatch").val('AddNew');
            //var noofvisiblerows = grid1.GetVisibleRowsOnPage(); // all newly created rows have -ve index -1 , -2 etc
            //var i;
            //var cnt = 1;
            //for (i = -1 ; cnt <= noofvisiblerows ; i--) {
            //    var tbQuotation = grid1.GetEditor("SrlNo");
            //    tbQuotation.SetValue(cnt);
            //    cnt++;
            //}
        }

        function On_AddNewClick() {
            grid1.AddNewRow();

        }


        function gridFocusedRowChanged(s, e) {
            globalRowIndex = e.visibleIndex;

        }
        var globalRowIndex;
        var globalTaxRowIndex;
        function GetVisibleIndex(s, e) {
            globalRowIndex = e.visibleIndex;
        }


        function OnCustomButtonClick(s, e) {

            if (e.buttonID == 'CustomDelete') {

                grid1.batchEditApi.EndEdit();
                grid1.DeleteRow(e.visibleIndex);

            }

            else if (e.buttonID == 'AddNew') {

                /// debugger;
                var index = e.visibleIndex;
                grid1.batchEditApi.StartEdit(index, 2);
                var ProductIDValue = (grid1.GetEditor('ProductID').GetText() != null) ? grid1.GetEditor('ProductID').GetText() : "0";
                if (ProductIDValue != "") {
                    //grid1.PerformCallback('Display');
                    grid1.AddNewRow();
                }
                else {
                    // grid1.batchEditApi.StartEdit(e.visibleIndex, 2);
                }


            }
        }

        function OnEndCallback(s, e) {
            debugger;
            if ($("#hdnchkgridbatch").val() == 'New') {
                grid1.AddNewRow();
                $("#hdnchkgridbatch").val('');
            }
            else if (grid1.cpSaveSuccessOrFail == "Success") {
                window.close();
                jAlert("Data saved successfully");
                window.parent.popupCbudget.Hide();

            }
            else if (grid1.cpSaveSuccessOrFail == "Error") {
                window.close();
                jAlert("Error occured");
            }
            else if (grid1.cpSaveSuccessOrFail == "Duplicate") {
                //  window.close();
                jAlert("Duplicate Product");
                grid1.AddNewRow();
            }
        }

        function PermonthCalculation() {
            //  debugger;
            var Permonth = grid1.GetEditor("Qty_Permonth");

            var current = grid1.GetEditor('Qty_CurrentFY').GetValue()

            if (current != '') {
                var cal = parseInt(current) / 12
                Permonth.SetValue(cal);
            }
            else {
                var cal = 0;
                Permonth.SetValue(cal);
            }


        }

        function Save_ButtonClick() {
            //  alert('hi');
            //    debugger;
            //grid1.PerformCallback('Display');
            //  OnAddNewClick();
            //   alert(grid1.GetEditor('Qty_CurrentFY').GetValue());

            //if (grid1.GetEditor('Qty_CurrentFY').GetValue() != null || grid1.GetEditor('Qty_CurrentFY').GetValue() != '0.0') {

            //    grid1.batchEditApi.EndEdit();
            //    grid1.UpdateEdit();
            //}
            //else {

            //    jAlert('Please Add Quantity');
            //    //   grid1.AddNewRow();
            //}
            debugger;

            var classVal = $("#gridproductclass").val();
            if (classVal != null ) {
                cacpCrossBtn.PerformCallback();
            }
            else if(classVal != "")
            {
                cacpCrossBtn.PerformCallback();
            }
            else if(classVal != "0")
            {
                cacpCrossBtn.PerformCallback();
            }
            else if(classVal != undefined)
            {

                cacpCrossBtn.PerformCallback();
            }
          else  if (classVal != "" ) {
                cacpCrossBtn.PerformCallback();
            }
        else if (classVal != "0" ) {
            cacpCrossBtn.PerformCallback();
        }
        else if (classVal != undefined) {
            cacpCrossBtn.PerformCallback();
        }
            else
            {
                jAlert("Please select Product Class.");
                return false;
            }
        }
        function acpCrossBtnEndCall() {
            debugger;
            window.close();
            window.parent.popupbudget.Hide();
        }
        function GridCallBack() {
            grid1.PerformCallback('Display');
            OnAddNewClick();
        }



        function ProductclassChanged(s, e) {
            //  debugger;

            debugger;
            gridcomboproductclass.GetValue();
            //  alert(gridcomboproductclass.GetValue());
            var quote_Id = gridcomboproductclass.GetValue();

            //  alert(quote_Id);
            if (quote_Id != null) {

                var arr = quote_Id.split(',');

            }

            else {
                jAlert('Select Class');
            }

            if (quote_Id != null) {
                //cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
                //cProductsPopup.Show();


                // cgridproducts.PerformCallback('BindComponentGridInvoiceOnSelection');
            }

        }



        function ChangeState(value) {

            cgridproducts.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
        }


        function Cggid_EndCallBack(value) {


        }

        function PerformCallToGridBind() {

            grid1.PerformCallback('BindGridOnQuotation' + '~' + '@');

            //   grid1.PerformCallback('SS' + '~' + '@');
            ///    $('#hdnPageStatus').val('Quoteupdate');
            ///  var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();
            cProductsPopup.Hide();

            On_AddNewClick();
            return false;
        }


        ////
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <div class="row">
            <br />
            <dxe:ASPxCallbackPanel runat="server" ID="acpCrossBtn" ClientInstanceName="cacpCrossBtn" OnCallback="acpCrossBtn_Callback">
                <PanelCollection>
                    <dxe:PanelContent runat="server">
                        <div id="divbudget" runat="server">
                            <div class="col-sm-12">
                                <h5>Criteria : Customer's Industrywise, All Products </h5>
                            </div>
                            <div class="col-sm-12">
                                <label>Product Class</label>
                                <dxe:ASPxComboBox ClientInstanceName="gridcomboproductclass" runat="server"  ID="gridproductclass" Width="100%">
                                    <ClientSideEvents ValueChanged="ProductclassChanged" />
                                </dxe:ASPxComboBox>
                            </div><br />
                            <div class="col-sm-12">
                                <label>Input Quantity(Current Financial Year)</label>
                                <dxe:ASPxTextBox ClientInstanceName="gridtxtproductclass" runat="server" ID="txt_qtyfinyr" Width="100%">
                                    <MaskSettings Mask="<0..999999999>.<0..99>" />
                                </dxe:ASPxTextBox>
                            </div>
                            <div style="padding-top: 8px" class="col-md-12">
                                <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AutoPostBack="False" Text="Save" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                    <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                                </dxe:ASPxButton>

                                <asp:HiddenField runat="server" ID="hdnchkgridbatch" />
                            </div>
                        </div>
                        <div id="divmsg" runat="server" style="display:none">
                           <div class="col-md-12"> No class is mapped for the selected Customer. Cannot enter budget values.</div>
                        </div>
                    </dxe:PanelContent>
                </PanelCollection>
                <ClientSideEvents EndCallback="acpCrossBtnEndCall" />
            </dxe:ASPxCallbackPanel>


        </div>
        <br />

        <%--Abhisek  --%>
        <%--<dxe:ASPxGridView runat="server" KeyFieldName="BudgetId"
            ClientInstanceName="grid1" ID="grid1"
            Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
            SettingsPager-Mode="ShowAllRecords"
            Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="200"
            OnCustomCallback="grid1_CustomCallback"
            OnDataBinding="grid1_DataBinding"
            OnBatchUpdate="grid1_BatchUpdate"
            OnRowInserting="Grid1_RowInserting"
            OnRowUpdating="Grid1_RowUpdating"
            OnRowDeleting="Grid1_RowDeleting"
            ViewStateMode="Disabled">
            <SettingsPager Visible="false"></SettingsPager>
            <Columns>

                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="true" Width="50" VisibleIndex="0" Caption="Action" HeaderStyle-HorizontalAlign="Center">
                    <CustomButtons>
                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="CustomDelete" Image-Url="/assests/images/crs.png">
                        </dxe:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxe:GridViewCommandColumn>

                <dxe:GridViewDataButtonEditColumn FieldName="ProductName" Caption="Product" VisibleIndex="1" Width="50">
                    <PropertiesButtonEdit>
                        <ClientSideEvents ButtonClick="ProductButnClick" />
                        <Buttons>
                            <dxe:EditButton Text="..." Width="20px">
                            </dxe:EditButton>
                        </Buttons>
                    </PropertiesButtonEdit>
                </dxe:GridViewDataButtonEditColumn>




                <dxe:GridViewDataTextColumn FieldName="Qty_CurrentFY" Caption="Qty(Current Fiscal Year)" VisibleIndex="2" Width="100">
                    <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Left" Style-VerticalAlign="Middle">
                        <ClientSideEvents TextChanged="function(s, e) {PermonthCalculation();}" KeyUp="function(s, e) {PermonthCalculation();}" />
                        <MaskSettings Mask="<0..999999999>.<0..99>" />
                    </PropertiesTextEdit>

                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn FieldName="Qty_PreviousFY" Caption="Qty(Previous Fiscal Year)" ReadOnly="True" VisibleIndex="3" Width="100">
                    <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Left" Style-VerticalAlign="Middle">
                    </PropertiesTextEdit>

                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn FieldName="Qty_Permonth" Caption="Per month" VisibleIndex="4" ReadOnly="True" PropertiesTextEdit-Height="15px" Width="50">
                    <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right" Style-VerticalAlign="Middle"></PropertiesTextEdit>

                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn FieldName="Description" Caption="Description" VisibleIndex="5" ReadOnly="True" Width="100">
                    <CellStyle Wrap="True"></CellStyle>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="UOM" Caption="Stock UOM" VisibleIndex="6" ReadOnly="True" Width="7%">
                    <CellStyle Wrap="True"></CellStyle>
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn FieldName="Industry" Caption="Mapped Industry" VisibleIndex="7" ReadOnly="true" Width="70">
                    <CellStyle HorizontalAlign="Right"></CellStyle>
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn FieldName="Productclass" Caption="Mapped Class" VisibleIndex="8" ReadOnly="true" Width="50">
                    <PropertiesTextEdit DisplayFormatString="0.0000" Style-HorizontalAlign="Right"></PropertiesTextEdit>
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn FieldName="ProductID" Caption="hidden Field Id" VisibleIndex="15" ReadOnly="True" Width="0" EditCellStyle-CssClass="hide"
                    PropertiesTextEdit-FocusedStyle-CssClass="hide" PropertiesTextEdit-Style-CssClass="hide">
                    <CellStyle Wrap="True" CssClass="hide"></CellStyle>
                </dxe:GridViewDataTextColumn>




                <dxe:GridViewCommandColumn ShowDeleteButton="false" ShowNewButtonInHeader="false" Width="15" VisibleIndex="14" Caption=" ">
                    <CustomButtons>
                        <dxe:GridViewCommandColumnCustomButton Text=" " ID="AddNew" Image-Url="/assests/images/add.png">
                        </dxe:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxe:GridViewCommandColumn>


            </Columns>
            <ClientSideEvents CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" EndCallback="OnEndCallback" BatchEditStartEditing="gridFocusedRowChanged" />

            <SettingsDataSecurity AllowEdit="true" />
            <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
            </SettingsEditing>
            <SettingsBehavior ColumnResizeMode="Disabled" />
        </dxe:ASPxGridView>--%>
    </div>
    <div>
        <%--Abhisek  --%>
        <%--<dxe:ASPxPopupControl ID="ProductpopUp" runat="server" ClientInstanceName="cProductpopUp"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="900"
            Width="2000px" HeaderText="Select Product" AllowResize="true" ResizingMode="Postponed" Modal="true">
            <HeaderTemplate>
                <span>Select Product</span>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <label><strong>Search By Product Name</strong></label>
                    <span style="color: red;">[Press ESC key to Cancel]</span>
                    <dxe:ASPxGridLookup ID="productLookUp" runat="server" DataSourceID="ProductDataSource" ClientInstanceName="cproductLookUp"
                        KeyFieldName="Products_ID" Width="800" TextFormatString="{0}" MultiTextSeparator=", " ClientSideEvents-TextChanged="ProductSelected" ClientSideEvents-KeyDown="ProductlookUpKeyDown">
                        <Columns>
                            <dxe:GridViewDataColumn FieldName="Products_Name" Caption="Name" Width="220">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="IsInventory" Caption="Inventory" Width="60">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="HSNSAC" Caption="HSN/SAC" Width="80">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="ClassCode" Caption="Class" Width="200">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="BrandName" Caption="Brand" Width="100">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>
                            <dxe:GridViewDataColumn FieldName="sProducts_isInstall" Caption="Installation Reqd." Width="120">
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>

                        </Columns>
                        <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                            <Templates>
                                <StatusBar>
                                    <table class="OptionsTable" style="float: right">
                                        <tr>
                                            /*<td>
                                                 <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />
                                            </td>*/
                                        </tr>
                                    </table>
                                </StatusBar>
                            </Templates>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                        </GridViewProperties>
                    </dxe:ASPxGridLookup>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />

        </dxe:ASPxPopupControl>--%>
    </div>


    <asp:SqlDataSource runat="server" ID="ProductDataSource"
        SelectCommand="Sp_GetbudgetProductData" SelectCommandType="StoredProcedure">

        <SelectParameters>

            <asp:SessionParameter Name="CustomerID" SessionField="CustomerID" Type="String" />

        </SelectParameters>
    </asp:SqlDataSource>



    <div>
        <%--Abhisek  --%>
        <%--<dxe:ASPxPopupControl ID="ASPxProductsPopup" runat="server" ClientInstanceName="cProductsPopup"
            Width="900px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <HeaderTemplate>
                <strong><span style="color: #fff">Select Products</span></strong>
                <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                    <ClientSideEvents Click="function(s, e){ 
                                                            cProductsPopup.Hide();
                                                        }" />
                </dxe:ASPxImage>
            </HeaderTemplate>
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <div style="padding: 7px 0;">
                        <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                        <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                        <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>
                    </div>

                    <dxe:ASPxGridView runat="server" KeyFieldName="ProductID" ClientInstanceName="cgridproducts" ID="grid_Products1"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" SettingsPager-Mode="ShowAllRecords" OnCustomCallback="cgridProducts_CustomCallback"
                        Settings-ShowFooter="false" AutoGenerateColumns="False" Settings-VerticalScrollableHeight="300" Settings-VerticalScrollBarMode="Visible">
                        <SettingsBehavior AllowDragDrop="False" AllowSort="False"></SettingsBehavior>
                        <SettingsPager Visible="false"></SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />

                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ProductsName" ReadOnly="true" Width="100" Caption="Product Name">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ProductDescription" Width="200" ReadOnly="true" Caption="Product Description">
                            </dxe:GridViewDataTextColumn>




                        </Columns>
                        <SettingsDataSecurity AllowEdit="true" />
                        <ClientSideEvents EndCallback="Cggid_EndCallBack" />
                    </dxe:ASPxGridView>
                    <div class="text-center">
                        <asp:Button ID="Button3" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" OnClientClick="return PerformCallToGridBind();" />
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>--%>
    </div>

<asp:HiddenField ID="hdnUrlVal" runat="server"/>


</asp:Content>
