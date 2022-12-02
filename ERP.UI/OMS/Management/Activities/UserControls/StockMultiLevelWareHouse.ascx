<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StockMultiLevelWareHouse.ascx.cs" Inherits="ERP.OMS.Management.Activities.UserControls.StockMultiLevelWareHouse" %>



    <script>
        function ucddlWarehouse_ValueChange() {
            var WarehouseID = $('#ucddlWarehouse').val();

            var List = $.grep(ucwarehouserateList, function (e) { return e.WarehouseID == defaultWarehouse; })
            if (List.length > 0) {
                var Rate = List[0].Rate;
                cuctxtRate.SetValue(Rate);
            }
            else {
                cuctxtRate.SetValue("0");
            }
        }

        function ucCheckProductStockdetails(ProductID, ProductName, UOM, VisibleIndex, DefaultWarehouse, Warehousetype, OpeningStock,branchid) {
            GetObjectID('uchdnIsPopUp').value = "Y";

            var Branch = branchid;
            var GetserviceURL = "../Activities/Services/Master.asmx/GetOpeningStockDetails";
            var serviceURL = "../Activities/Services/Master.asmx/CheckDuplicateSerial";

            GetObjectID('uchdfProductSrlNo').value = ProductID;
            GetObjectID('uchdfProductID').value = ProductID;
            GetObjectID('uchdfWarehousetype').value = Warehousetype;
            GetObjectID('uchdndefaultWarehouse').value = DefaultWarehouse;
            GetObjectID('uchdfUOM').value = UOM;
            GetObjectID('uchdfServiceURL').value = serviceURL;
            GetObjectID('uchdfBranch').value = Branch;
            GetObjectID('uchdfIsRateExists').value = "Y";

            OpeningStock = parseFloat(OpeningStock).toFixed(4);

            document.getElementById('<%=uclblProductName.ClientID %>').innerHTML = ProductName.replace("squot", "'").replace("coma", ",").replace("slash", "/");
            document.getElementById('<%=uctxt_EnteredSalesAmount.ClientID %>').innerHTML = "0.0000";
            document.getElementById('<%=uctxt_EnteredSalesUOM.ClientID %>').innerHTML = UOM;
            document.getElementById('<%=uclblAvailableStock.ClientID %>').innerHTML = OpeningStock;
            document.getElementById('<%=uclblAvailableStockUOM.ClientID %>').innerHTML = UOM;

            var objectToPass = {}
            objectToPass.ProductID = ProductID;
            objectToPass.BranchID = Branch;

            $.ajax({
                type: "POST",
                url: GetserviceURL,
                data: JSON.stringify(objectToPass),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;
                    var myObj = returnObject.ProductStockDetails;
                    var RateList = returnObject.WarehouseRate;
                    var BlockList = returnObject.StockBlock;

                    StockOfProduct = [];
                    warehouserateList = [];

                    if (BlockList.length > 0) {
                        for (x in BlockList) {
                            GetObjectID('ucIsStockBlock').value = BlockList[x]["IsStockBlock"];
                            GetObjectID('ucAvailableQty').value = parseFloat(BlockList[x]["AvailableQty"]).toFixed(4);
                            GetObjectID('ucCurrentQty').value = parseFloat(OpeningStock).toFixed(4);
                        }
                    }

                    if (RateList.length > 0) {
                        for (x in RateList) {
                            var Rates = { WarehouseID: RateList[x]["WarehouseID"], Rate: parseFloat(RateList[x]["Rate"]).toFixed(2) }
                            ucwarehouserateList.push(Rates);
                        }
                    }

                    if (myObj.length > 0) {
                        for (x in myObj) {
                            var ProductStock = {
                                Product_SrlNo: myObj[x]["Product_SrlNo"], SrlNo: parseInt(myObj[x]["SrlNo"]), WarehouseID: myObj[x]["WarehouseID"], WarehouseName: myObj[x]["WarehouseName"],
                                Quantity: myObj[x]["Quantity"], SalesQuantity: myObj[x]["SalesQuantity"], Batch: myObj[x]["Batch"], MfgDate: myObj[x]["MfgDate"], ExpiryDate: myObj[x]["ExpiryDate"],
                                Rate: myObj[x]["Rate"], SerialNo: myObj[x]["SerialNo"], Barcode: myObj[x]["Barcode"], ViewBatch: myObj[x]["Batch"],
                                ViewMfgDate: myObj[x]["MfgDate"], ViewExpiryDate: myObj[x]["ExpiryDate"], ViewRate: myObj[x]["Rate"],
                                IsOutStatus: myObj[x]["IsOutStatus"], IsOutStatusMsg: myObj[x]["IsOutStatusMsg"], LoopID: parseInt(myObj[x]["LoopID"]), Status: myObj[x]["Status"]
                            }
                            StockOfProduct.push(ProductStock);
                        }

                        ucStockOfProduct.sort(ucsortByMultipleKey(['LoopID', 'SrlNo']));
                        ucStockOfProduct = ucReGenerateStock(StockOfProduct);

                        ucCreateStock();
                        cucPopup_Warehouse.Show();
                    }
                    else {
                        ucCreateStock();
                        cucPopup_Warehouse.Show();
                    }
                }
            });
        }

        function uccloseWarehouse(s, e) {
            GetObjectID('uchdnIsPopUp').value = "";
            e.cancel = false;
        }

        function ucReGenerateStock(myObj) {
            var Previous_LoopID = "";
            for (x in myObj) {
                var Current_LoopID = myObj[x]["LoopID"];

                if (Current_LoopID == Previous_LoopID) {
                    myObj[x]["WarehouseName"] = "";
                    myObj[x]["SalesQuantity"] = "";
                    myObj[x]["ViewBatch"] = "";
                    myObj[x]["ViewMfgDate"] = "";
                    myObj[x]["ViewExpiryDate"] = "";
                }

                Previous_LoopID = myObj[x]["LoopID"];
            }

            return myObj;
        }

        function ucFullnFinalSave() {
            debugger;
            GetObjectID('uchdnIsPopUp').value = "";
            var JsonProductStock = JSON.stringify(ucStockOfProduct);
            GetObjectID('uchdnJsonProductStock').value = JsonProductStock;

            //OpeningGrid.PerformCallback('FinalSubmit');
        }
    </script>
    <script>
        document.onkeydown = function (e) {
            if (event.keyCode == 88 && event.altKey == true && GetObjectID('uchdnIsPopUp').value == "Y") { //run code for ALT+X -- ie, Save & Exit!     
                ucStopDefaultAction(e);
                ucFullnFinalSave();
            }
        }

        function ucStopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }

        
    </script>


<dxe:ASPxPopupControl ID="ucPopup_Warehouse" runat="server" ClientInstanceName="cucPopup_Warehouse"
                        Width="850px" HeaderText="Warehouse Details" PopupHorizontalAlign="WindowCenter"
                        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                        ContentStyle-CssClass="pad">
                        <ClientSideEvents Closing="function(s, e) {
	uccloseWarehouse(s, e);}" />
                        <ContentStyle VerticalAlign="Top" CssClass="pad">
                        </ContentStyle>
                        <ContentCollection>
                            <dxe:PopupControlContentControl runat="server">
                                <div id="content-5" class="pull-right wrapHolder content horizontal-images" style="width: 100%; margin-right: 0px;">
                                    <ul>
                                        <li>
                                            <div class="lblHolder" style="display: none;">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Entered Quantity </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="uctxt_EnteredSalesAmount" runat="server" Font-Bold="true"></asp:Label>
                                                                <asp:Label ID="uctxt_EnteredSalesUOM" runat="server" Font-Bold="true"></asp:Label>

                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Opening Stock</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="uclblAvailableStock" runat="server"></asp:Label>
                                                                <asp:Label ID="uclblAvailableStockUOM" runat="server"></asp:Label>

                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="lblHolder">
                                                <table>
                                                    <tbody>
                                                        <tr>
                                                            <td>Selected Product</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="uclblProductName" runat="server"></asp:Label></td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                                <div class="clear">
                                </div>
                                <div class="clearfix  modal-body" style="padding: 8px 0 8px 0; margin-bottom: 15px; margin-top: 15px; border-radius: 4px; border: 1px solid #ccc;">
                                    <div class="col-md-12">
                                        <div class="clearfix  row">
                                            <div class="col-md-3" id="uc_div_Warehouse">
                                                <div>
                                                    Warehouse
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <asp:DropDownList ID="ucddlWarehouse" runat="server" Width="100%" DataTextField="WarehouseName" DataValueField="WarehouseID" onchange="ucddlWarehouse_ValueChange()">
                                                    </asp:DropDownList>
                                                    <span id="ucrfvWarehouse" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="uc_div_Batch">
                                                <div>
                                                    Batch/Lot
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <input type="text" id="uctxtBatch" placeholder="Batch" />
                                                    <span id="ucrfvBatch" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="uc_div_Manufacture">
                                                <div>
                                                    Mfg Date
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <%-- <input type="text" id="txtMfgDate" placeholder="Mfg Date" />--%>
                                                    <dxe:ASPxDateEdit ID="uctxtMfgDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True"
                                                        ClientInstanceName="cuctxtMfgDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="uc_div_Expiry">
                                                <div>
                                                    Expiry Date
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <%-- <input type="text" id="txtExprieyDate" placeholder="Expiry Date" />--%>
                                                    <dxe:ASPxDateEdit ID="uctxtExprieyDate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True"
                                                        ClientInstanceName="cuctxtExprieyDate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                    </dxe:ASPxDateEdit>
                                                </div>
                                            </div>
                                            <div class="clear" id="uc_div_Break"></div>
                                            <div class="col-md-3" id="uc_div_Rate">
                                                <div>
                                                    Rate
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxTextBox ID="uctxtRate" runat="server" ClientInstanceName="cuctxtRate" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="uc_div_Quantity">
                                                <div>
                                                    Quantity
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <dxe:ASPxTextBox ID="uctxtQty" runat="server" ClientInstanceName="cuctxtQty" HorizontalAlign="Right" Font-Size="12px" Width="100%" Height="15px">
                                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..9999&gt;" IncludeLiterals="DecimalSymbol" />
                                                        <ValidationSettings Display="None"></ValidationSettings>
                                                    </dxe:ASPxTextBox>
                                                    <span id="rfvQuantity" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="uc_div_Serial">
                                                <div>
                                                    Serial No
                                                </div>
                                                <div class="Left_Content" style="">
                                                    <input type="text" id="uctxtSerial" placeholder="Serial No" onkeyup="Serialkeydown(event)" />
                                                    <span id="ucrfvSerial" title="Mandatory" class="tp2 fa fa-exclamation-circle iconRed" style="display: none;"></span>
                                                </div>
                                            </div>
                                            <div class="col-md-3" id="uc_div_Upload">
                                                <div class="col-md-3">
                                                    <div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div>
                                                </div>
                                                <div class="Left_Content" style="padding-top: 7px">
                                                    <input type="button" onclick="ucSaveStock()" value="Add" class="btn btn-primary" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div id="ucshowData" class="gridStatic">
                                </div>
                                <div class="clearfix  row">
                                    <div class="col-md-3">
                                        <div>
                                        </div>
                                        <div class="Left_Content" style="padding-top: 14px">
                                            <input type="button" onclick="ucFullnFinalSave()" value="Save & Ex&#818;it" class="btn btn-primary" />
                                        </div>
                                    </div>
                                </div>
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                        <HeaderStyle BackColor="LightGray" ForeColor="Black" />






                    </dxe:ASPxPopupControl>

    <div>
        <asp:HiddenField runat="server" ID="uchdfWarehousetype" />
        <asp:HiddenField runat="server" ID="uchdfProductSrlNo" />
        <asp:HiddenField runat="server" ID="uchdfProductID" />
        <asp:HiddenField runat="server" ID="uchdndefaultWarehouse" />
        <asp:HiddenField runat="server" ID="uchdfUOM" />
        <asp:HiddenField runat="server" ID="uchdfServiceURL" />
        <asp:HiddenField runat="server" ID="uchdfBranch" />
        <asp:HiddenField runat="server" ID="uchdfIsRateExists" />
        <asp:HiddenField runat="server" ID="uchdnJsonProductStock" />
        <asp:HiddenField runat="server" ID="uchdnIsPopUp" />
        <asp:HiddenField runat="server" ID="ucIsStockBlock" />
        <asp:HiddenField runat="server" ID="ucAvailableQty" />
        <asp:HiddenField runat="server" ID="ucCurrentQty" />
        <asp:HiddenField runat="server" ID="uchdfIsBarcodeActive" />
        <asp:HiddenField runat="server" ID="uchdfIsBarcodeGenerator" />
    </div>