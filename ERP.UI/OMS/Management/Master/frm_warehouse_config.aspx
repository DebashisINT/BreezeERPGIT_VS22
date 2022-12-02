<%@ Page Title="Inventory Configuration" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" CodeBehind="frm_warehouse_config.aspx.cs" Inherits="ERP.OMS.Management.Master.frm_warehouse_config" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    

    <script language="javascript" type="text/javascript">
        var globalBatchChecked;
        var GlobalBarCodeChecked;
        var globalSerialChecked;
        var globalStockBlockChecked;
        var GlobalChecked;
        var is_existswarehouse;
        var is_existsbatch;
        var is_existsserial;
        var is_existsstockblock;
        var is_type;
        var batchchekhedset = 0;
        var serialchkset = 0; 
        var stockblockchkset = 0;
        var Barcodechkset = 0;

        function checkWarehouseTranscationExists(s, e) {
            is_existswarehouse = s.GetChecked();
            is_type = 'warehouse';
            var prod_id=''
            chkTranscationExists(is_existswarehouse, is_type, prod_id,s);
        }
        
        function chkTranscationExists(is_exiwarhse, is_type, prod_id, sender) {
            debugger;
            var ret = false;
            if (prod_id=='') {
                prod_id = 0;
            }
                GlobalChecked = sender;
                batchchekhedset = 0;
                serialchkset = 0;
                stockblockchkset = 0;
                Barcodechkset = 0;
                $.ajax({
                    type: "POST",
                    url: "frm_warehouse_config.aspx/GetTransactionExistscheck",
                    data: '{prodid: "' + prod_id + '",is_type: "' + is_type + '" ,is_checked: "' + is_exiwarhse + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    //success: OnSuccess,
                    success: function (response) {
                        if (response.d == 'Exists') { 
                            ret = false;
                            jAlert('Transaction Exists.Can Not Activate.');
                            if (is_type == 'warehouse') {
                                //GlobalChecked.SetChecked(true);
                                if (is_exiwarhse==false) {
                                    GlobalChecked.SetChecked(true);
                                }
                                else {
                                    GlobalChecked.SetChecked(false);
                                }
                               
                            }
                            else if (is_type == 'batch') {
                                if (is_exiwarhse == false) {
                                    GlobalChecked.SetChecked(true);
                                }
                                else {
                                    GlobalChecked.SetChecked(false);
                                }
                                batchchekhedset = 1;
                            }

                            else if (is_type == 'serial') {
                                if (is_exiwarhse == false) {
                                    GlobalChecked.SetChecked(true);
                                }
                                else {
                                    GlobalChecked.SetChecked(false);
                                }
                                serialchkset = 1;
                            }
                            //else if (is_type == 'BarCode') {
                            //    if (is_exiwarhse == false) {
                            //        GlobalChecked.SetChecked(true);
                            //    }
                            //    else {
                            //        GlobalChecked.SetChecked(false);
                            //    }
                            //    Barcodechkset = 1;
                            //}
                            else if (is_type == 'stockblock') {
                                if (is_exiwarhse == false) {
                                    GlobalChecked.SetChecked(true);
                                }
                                else {
                                    GlobalChecked.SetChecked(false);
                                }
                                stockblockchkset = 1;
                            }
                        }
                        //else if (response.d == 'QuotationExists') {
                        //    jAlert('Transaction Exists....Cant Change..');
                        //}
                        else {
                            batchchekhedset = 0;
                            //ret = true;
                            serialchkset = 0;
                            Barcodechkset=0
                            stockblockchkset = 0;
                            if (is_type == 'serial') {
                                checkserialno(sender, prod_id);
                            }
                        }
                        //else {


                        //    if (document.getElementById("hdnSerialNo").value == '') {
                        //        document.getElementById("hdnSerialNo").value = globalSerialChecked + '~' + obj;
                        //    }
                        //    else {
                        //        document.getElementById("hdnSerialNo").value = document.getElementById("hdnSerialNo").value + ',' + globalSerialChecked + '~' + obj;
                        //    }

                        //}
                      
                    },
                    failure: function (response) {
                        //alert(response.d);
                    }
                });
                return ret;
        }

        function SavedSuccessfully(msg) {
            debugger;
            jAlert(msg, "Saved", function () {
                window.location.href = "frm_warehouse_config.aspx";
            })
        }
      
        document.onkeydown = function (e) {
            if (event.keyCode == 17) isCtrl = true;
            if (event.keyCode == 83 && isCtrl == true) {
                //run code for CTRL+S -- ie, save!            
               // $('#<%=Button1.ClientID %>').click();
                document.getElementById('<%= Button1.ClientID %>').click();
                return false;

            }
       <%--     else if ((event.keyCode == 120 || event.keyCode == 88) && isCtrl == true) {

                //run code for CTRL+X -- ie, discard!           

                $('#<%=btnSubmitExit.ClientID %>').click();

                    return false;

                }--%>



        }
       
        function storevalue(s, e) {
            //$('#loading').fadeOut();
            debugger;
            if (grid.cpBatch) {
                if (grid.cpBatch != '') {
                    document.getElementById('hdnActiveBranch').value = grid.cpBatch;

                    grid.cpBatch = null;
                }
            }
            //else {
            //    document.getElementById('hdnActiveBranch').value = grid.cpBatch; grid.cpBatch = null;

            //}



            if (grid.cpSerial) {

                if (grid.cpSerial != '') {
                    document.getElementById('hdnSerialNo').value = grid.cpSerial;

                    grid.cpSerial = null;
                }
            }
            //else {
            //    document.getElementById('hdnSerialNo').value = grid.cpSerial; grid.cpSerial = null;

            //}



            if (grid.cpStock) {

                if (grid.cpStock != '') {
                    document.getElementById('hdnStockBlock').value = grid.cpStock;

                    grid.cpStock = null;
                }
            }
            //else {
            //    document.getElementById('hdnStockBlock').value = grid.cpStock; grid.cpStock = null;

            //}

            if (grid.cpBarCode) {

                if (grid.cpBarCode != '') {
                    document.getElementById('hdnBarCode').value = grid.cpBarCode;

                    grid.cpBarCode = null;
                }
            }

        }
        function GetCheckStockBlock(s, e, itemIndex) {
            debugger;
            //is_existsstockblock = s.GetChecked();
            //is_type = 'stockblock';
            //var prod_id = ''
           
            var obj = grid.GetRowKey(itemIndex);
            //prod_id = obj;
            //chkTranscationExists(is_existsstockblock, is_type, prod_id, s);

            //if (stockblockchkset==0){
            globalStockBlockChecked = s.GetChecked();

            if (document.getElementById("hdnStockBlock").value == '') {
                document.getElementById("hdnStockBlock").value = globalStockBlockChecked + '~' + obj;
            }
            else {
                document.getElementById("hdnStockBlock").value = document.getElementById("hdnStockBlock").value + ',' + globalStockBlockChecked + '~' + obj;
            }
           //}
        }

        //var globalbatchtrue = false;

        function GetCheckBarCode(s, e, itemIndex) {

            debugger;
            is_type = 'BarCode';
            var prod_id = ''
            var obj = grid.GetRowKey(itemIndex);
            prod_id = obj;
            GlobalBarCodeChecked = s.GetChecked();

           // alert('Hi');
            // var xx = chkTranscationExists(is_existsserial, is_type, prod_id, s);

            if (Barcodechkset == 0) {
                if (document.getElementById("hdnBarCode").value == '') {
                    document.getElementById("hdnBarCode").value = GlobalBarCodeChecked + '~' + obj;
                }
                else {
                    document.getElementById("hdnBarCode").value = document.getElementById("hdnBarCode").value + ',' + GlobalBarCodeChecked + '~' + obj;
                }
            }
            //alert(document.getElementById("hdnBarCode").value);
        }
        
        function GetCheckActivateBatch(s, e, itemIndex) {
            debugger;
            is_existsbatch = s.GetChecked();
            is_type = 'batch';
            var prod_id = ''
           
            //grid.GetRowValues(itemIndex, 'sproducts_id', GetFieldvalue1);
            var obj = grid.GetRowKey(itemIndex);
            prod_id = obj;
            chkTranscationExists(is_existsbatch, is_type, prod_id, s);
            globalBatchChecked = s.GetChecked();
            if (batchchekhedset==0){
            if (document.getElementById("hdnActiveBranch").value == '') {
                document.getElementById("hdnActiveBranch").value = globalBatchChecked + '~' + obj;
            }
            else {
                document.getElementById("hdnActiveBranch").value = document.getElementById("hdnActiveBranch").value + ',' + globalBatchChecked + '~' + obj;
            }
           }
        }
      
        function checkserialno(s, prod_id) {
            GlobalChecked = s;
            globalSerialChecked = s.GetChecked();
            isGetCellRowCall = false;
            //For check wheather stock,sale purchase uom are different or not.............if so then can't check on activate serial//
            debugger;
            $.ajax({
                type: "POST",
                url: "frm_warehouse_config.aspx/GetUOMcheck",
                data: '{prodid: "' + prod_id + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                //success: OnSuccess,
                success: function (response) {

                    $("#loading").fadeOut();
                    if (response.d == '0') {
                        jAlert('Stock,Sale & purchase UOM are different!......Cannot Activate.');
                        GlobalChecked.SetChecked(false);
                    }
                    else {


                        if (document.getElementById("hdnSerialNo").value == '') {
                            document.getElementById("hdnSerialNo").value = globalSerialChecked + '~' + prod_id;
                        }
                        else {
                            document.getElementById("hdnSerialNo").value = document.getElementById("hdnSerialNo").value + ',' + globalSerialChecked + '~' + prod_id;
                        }

                    }
                },
                failure: function (response) {
                    //alert(response.d);
                }
            });
        }
        function GetCheckSerialNo(s, e, itemIndex) {
          
            debugger;
            is_type = 'serial';
            var prod_id = ''
            var obj = grid.GetRowKey(itemIndex);
            prod_id = obj;
            is_existsserial = s.GetChecked();
            //chkTranscationExists(is_existsserial, is_type, prod_id, s);
            var xx = chkTranscationExists(is_existsserial, is_type, prod_id, s);
        }
       

        function ChangeState(value) {
            grid.PerformCallback(value);
        }
        function GetFieldvalue(obj)
        {
            debugger;
            if(document.getElementById("hdnprodrowid").value=='')
            {
                document.getElementById("hdnprodrowid").value = obj;
            }
            else
            {
                document.getElementById("hdnprodrowid").value = document.getElementById("hdnprodrowid").value +'~'+ obj;
            }
            //hdnprodrowid.value = obj;

        }
        function GetFieldvalue1(obj) {

            $("#loading").fadeOut();
            debugger;
            if (document.getElementById("hdnActiveBranch").value == '') {
                document.getElementById("hdnActiveBranch").value = globalBatchChecked + '~' + obj;
            }
            else {
                document.getElementById("hdnActiveBranch").value = document.getElementById("hdnActiveBranch").value + ',' + globalBatchChecked + '~' + obj;
            }

        }
        function GetFieldvalue2(obj) {
            isGetCellRowCall = false;
            debugger;
            $.ajax({
                type: "POST",
                url: "frm_warehouse_config.aspx/GetUOMcheck",
                data: '{prodid: "' + obj + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                //success: OnSuccess,
                success: function (response) {

                    $("#loading").fadeOut();
                    if (response.d == '0') {
                        jAlert('Stock,Sale & purchase UOM are different!......Cannot Activate.'); 
                        GlobalChecked.SetChecked(false);
                    }
                    else {


                        if (document.getElementById("hdnSerialNo").value == '') {
                            document.getElementById("hdnSerialNo").value = globalSerialChecked + '~' + obj;
                        }
                        else {
                            document.getElementById("hdnSerialNo").value = document.getElementById("hdnSerialNo").value + ',' + globalSerialChecked + '~' + obj;
                        }

                    }
                },
               failure: function (response) {
               //alert(response.d);
              }
            });

            //function OnSuccess(response) {
            //    alert(response.d);
            //}



        }

        
        function GetFieldvalueStock(obj) {

            $("#loading").fadeOut();
            debugger;
            if (document.getElementById("hdnStockBlock").value == '') {
                document.getElementById("hdnStockBlock").value = globalStockBlockChecked + '~' + obj;
            }
            else {
                document.getElementById("hdnStockBlock").value = document.getElementById("hdnStockBlock").value + ',' + globalStockBlockChecked + '~' + obj;
            }

        }

        function OnEditButtonClick(keyValue) {
            debugger;
            var url = 'frm_currency_master.aspx?id=' + keyValue;
            window.location.href = url;
        }

        function EndCall(obj) {
            if (grid.cpDelmsg != null) {
                jAlert(grid.cpDelmsg);
                grid.cpDelmsg = null;
            }
        }

        function getProductId(s,e)
        {
            debugger;
            //grid.PerformCallback(itemIndex);
        }

        function OnSelectedIndexChanged(s, e, itemIndex) {
            debugger;
            if (document.getElementById("hdnwarehouseid").value == '') {
                document.getElementById("hdnwarehouseid").value = s.GetValue();
            }
            else {
                document.getElementById("hdnwarehouseid").value = document.getElementById("hdnwarehouseid").value + '~' + s.GetValue();
            }

            grid.GetRowValues(itemIndex, 'sproducts_id', GetFieldvalue);
            
            //grid.PerformCallback(itemIndex + '^' + 'warehouse');
          
        }

        function grid_SelectionChanged(s, e) {
            debugger;
            if (e.isSelected) {
                var key = s.GetRowKey(e.visibleIndex);
                jAlert('Last Key = ' + key);
            }
        }
        function OnAddButtonClick() {
            var url = 'frm_currency_master.aspx?id=ADD';
            window.location.href = url;
        }

        function DeleteRow(keyValue) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });

        }

    </script>
        <style>
        #loading {
            position: fixed;
            width:100%;
            top: 0;
              left: 0;
              bottom: 0;
              right: 0;
              z-index:99999;
              display:none;
        }
        #loadingcont {
            
              z-index: 999;
              height: 2em;
              width: 2em;
              overflow: show;
              margin: auto;
              margin-top:250px;
              
        }
        #gridWarehouseConfig td.dxgv {
            padding-bottom:0 !important;
            padding-top:0 !important;
        } 
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3> Inventory Configuration</h3>
            <div id="btncross" class="crossBtn" style="margin-left:50px;"><a href="../ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
     <div id="loading" style="">
            <div id="loadingcont" >
                <p id="loadingspinr">
                    <img src="/assests/images/loader.gif"
                </p>
            </div>
        </div>
    <div class="form_main clearfix">
        <table class="TableMain100" style="width: 100%">
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                 <asp:HiddenField runat="server" ID="hdnstoreheaderbatch" />
                                 <asp:HiddenField runat="server" ID="hdnActiveBranch" />
                                 <asp:HiddenField runat="server" ID="hdnSerialNo" />
                                <asp:HiddenField runat="server" ID="hdnStockBlock" />
                                <asp:HiddenField runat="server" ID="hdnBarCode" />
                                   <asp:HiddenField runat="server" ID="hdnprodrowid" />
                                 <asp:HiddenField runat="server" ID="hdnwarehouseid" />
                                <table>
                                    <tr>
                                        <td><dxe:ASPxCheckBox ID="checkIsactive" runat="server" Text="Activate Warehouse" OnCheckedChanged="checkIsactive_CheckedChanged" >
                                            <ClientSideEvents CheckedChanged="checkWarehouseTranscationExists" />
                                            </dxe:ASPxCheckBox></td>
                                       <%-- <td><dxe:ASPxCheckBox ID="checkIsActivateSerialno" runat="server" Text="Activate Serial No"></dxe:ASPxCheckBox></td>--%>
                                    </tr>
                                </table>
                                

                               
                                  <%--<% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();" class="btn btn-primary">Add New</a>
                                 <% } %>--%>
                               <%-- <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                     <asp:ListItem Value="2">XLS</asp:ListItem>
                                     <asp:ListItem Value="3">RTF</asp:ListItem>
                                     <asp:ListItem Value="4">CSV</asp:ListItem>
                        
                                </asp:DropDownList> --%>
                            </td>
                            <td>
                                <div class="col-md-12">
                                    <asp:Button ID="Button1" runat="server" Text="Save" CssClass="btn btn-primary" OnClientClick="return BatchUpdate();" OnClick="Button1_Click" />
                                    <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="Button2_Click" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="gridcellright" style="float: right; vertical-align: top">
              
                </td>
            </tr>
            <tr>

            <%--   SettingsPager-Mode="ShowAllRecords"  Settings-ShowFooter="false"   Settings-VerticalScrollBarMode="Auto"  Settings-VerticalScrollableHeight="360"--%><%-- For off to show all record--%>
                <td style="vertical-align: top; text-align: left" colspan="2">
                    <dxe:ASPxGridView ID="gridWarehouseConfig" ClientInstanceName="grid" Width="100%"    
                        KeyFieldName="sproducts_id" DataSourceID="gridWarehouseConfigDataSource" OnAutoFilterCellEditorInitialize="gridWarehouseConfig_CellEditorInitialize" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" runat="server" 
                        AutoGenerateColumns="False" OnInitNewRow="gridWarehouseConfig_InitNewRow" SettingsBehavior-AllowFocusedRow="true" OnCellEditorInitialize="gridWarehouseConfig_CellEditorInitialize"   OnCustomCallback="grid_CustomCallback" OnDataBinding="gridWarehouseConfig_PageIndexChanged"  >
                                    <clientsideevents endcallback="storevalue"  SelectionChanged="grid_SelectionChanged" FocusedRowChanged="grid_SelectionChanged" 
                       />
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true"/>

                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="True" FieldName="sProducts_Code" Caption="Product Code">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Visible="True" FieldName="sProducts_Name" Caption="Product Name" VisibleIndex="1">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>


                        <%--    <dxe:GridViewDataTextColumn Visible="False" FieldName="cmp_id" Caption="comp_id" VisibleIndex="4">
                            </dxe:GridViewDataTextColumn>--%>

                          <%--   <dxe:GridViewDataTextColumn Visible="True" Caption="Warehouse" VisibleIndex="2">
                                 <DataItemTemplate>
                                     <dxe:ASPxComboBox ID="ASPxComboBox2" runat="server" ValueType="System.String">
                                     </dxe:ASPxComboBox>
                                 </DataItemTemplate>
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>


                            <dxe:GridViewDataComboBoxColumn Caption="Warehouse" FieldName="warehouse_id" VisibleIndex="2">
                                  <DataItemTemplate>
                                    <dxe:ASPxComboBox ID="ASPxComboBox2" ClientInstanceName="cASPxComboBox2" runat="server"  ValueType="System.String" DataSourceID="sqlwarehouse" TextField="bui_name" ValueField="bui_id" Text='<%# Bind("warehouse_name") %>' OnInit="cb_Init" Width="100%" >
                                 <%--  <ClientSideEvents SelectedIndexChanged="function(s,e){getProductId(s,e);}" />--%>
                                         
                                         </dxe:ASPxComboBox>
                                   
                                </DataItemTemplate>
                              <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                          
                          <dxe:GridViewDataTextColumn Caption="#" FieldName="Is_active_Batch" Settings-AllowSort="False">
                             <DataItemTemplate>
                                 <div style="text-align: center">           
                                    <dxe:ASPxCheckBox ID="chkActivateBatch" ClientInstanceName="CchkActivateBatch" Checked='<%# GetChecked(Eval("Is_active_Batch").ToString()) %>' runat="server" OnInit="chkActivateBatch_Init">
                                    </dxe:ASPxCheckBox>
                                     </div>
                             </DataItemTemplate>
                           <HeaderTemplate>
                              <dxe:ASPxCheckBox ID="selectAllCheckBox" Text="Activate Batch" runat="server" ToolTip="Select/Unselect all rows on the page"
                                   ClientInstanceName="SelectAllCheckBox"  ClientSideEvents-CheckedChanged="function(s, e) { 
                                  grid.PerformCallback(s.GetChecked()+'^'+'batch'); 
                                  
                                  }">
                              </dxe:ASPxCheckBox>
                           </HeaderTemplate>
                           <HeaderStyle HorizontalAlign="Center" />
                              <Settings AllowAutoFilterTextInputTimer="False" />
                          </dxe:GridViewDataTextColumn>


                           <dxe:GridViewDataTextColumn Caption="Activate Serial" FieldName="Is_active_serialno" VisibleIndex="4">
                                <DataItemTemplate>
                                     <div style="text-align: center">   
                                    <dxe:ASPxCheckBox ID="chkActivateSerialno" ClientInstanceName="CchkActivateSerialno" runat="server" Checked='<%# GetChecked(Eval("Is_active_serialno").ToString()) %>' OnInit="chkActivateSerialno_Init">
                                    </dxe:ASPxCheckBox>
                                          </div>
                                </DataItemTemplate>
                               <HeaderTemplate>
                              <dxe:ASPxCheckBox ID="selectAllSerial" Text="Activate Serial" runat="server" ToolTip="Select/Unselect all rows on the page"
                                   ClientInstanceName="SelectAllSerial"  ClientSideEvents-CheckedChanged="function(s, e) {
                                  grid.PerformCallback(s.GetChecked()+'^'+'serial'); }">
                              </dxe:ASPxCheckBox>
                           </HeaderTemplate>
                               <Settings AllowAutoFilterTextInputTimer="False" />
                         </dxe:GridViewDataTextColumn>

                          <dxe:GridViewDataTextColumn Caption="Stock Block" FieldName="Is_stock_Block" VisibleIndex="5">
                                <DataItemTemplate>
                                     <div style="text-align: center">   
                                    <dxe:ASPxCheckBox ID="chkStkBlck" ClientInstanceName="CchkStkBlck" runat="server" Checked='<%# GetChecked(Eval("Is_stock_Block").ToString()) %>' OnInit="chkStkBlck_Init">
                                    </dxe:ASPxCheckBox>
                                           </div>
                                </DataItemTemplate>
                                <HeaderTemplate>
                                  <dxe:ASPxCheckBox ID="selectAllstock" Text="Activate Stock Block[SO]" runat="server" ToolTip="Select/Unselect all rows on the page"
                                    ClientInstanceName="SelectAllstock"  ClientSideEvents-CheckedChanged="function(s, e) { 
                                      grid.PerformCallback(s.GetChecked()+'^'+'stock'); }">
                                  </dxe:ASPxCheckBox>
                           </HeaderTemplate>
                              <Settings AllowAutoFilterTextInputTimer="False" />
                           </dxe:GridViewDataTextColumn>
                         
                             <dxe:GridViewDataTextColumn Caption="Active Barcode" FieldName="Is_BarCode_Active" VisibleIndex="6">
                                <DataItemTemplate>
                                     <div style="text-align: center">   
                                    <dxe:ASPxCheckBox ID="chkActivateBarCode" ClientInstanceName="CchkActivateBarCode" runat="server" Checked='<%# GetChecked(Eval("Is_BarCode_Active").ToString()) %>' OnInit="chkActivateBarCode_Init">
                                    </dxe:ASPxCheckBox>
                                          </div>
                                </DataItemTemplate>
                               <HeaderTemplate>
                              <dxe:ASPxCheckBox ID="selectAllBarCode" Text="Activate Barcode" runat="server" ToolTip="Select/Unselect all rows on the page"
                                   ClientInstanceName="cSelectAllBarCode"  ClientSideEvents-CheckedChanged="function(s, e) {
                                  grid.PerformCallback(s.GetChecked()+'^'+'BarCode'); }">
                              </dxe:ASPxCheckBox>
                           </HeaderTemplate>
                                 <Settings AllowAutoFilterTextInputTimer="False" />
                         </dxe:GridViewDataTextColumn>

                        </Columns>
                         <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <%--  <settingsediting mode="Batch">
                    </settingsediting>--%>
                      
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <%--<SettingsSearchPanel Visible="True" />--%>
                <%--        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu = "True" /> --%>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden"/> 
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>


         


        <asp:SqlDataSource ID="gridWarehouseConfigDataSource" runat="server" 
            SelectCommand="select sproducts_id,sProducts_Name,sProducts_Code,warehouse_id,warehouse_name=(
select bui_name from tbl_master_building where bui_id=warehouse_id),Is_active_Batch,Is_active_serialno,Is_stock_Block,Is_BarCode_Active from Master_sProducts where sProduct_IsInventory=1"> 
        </asp:SqlDataSource>

         <asp:SqlDataSource ID="sqlwarehouse" runat="server">
           
        </asp:SqlDataSource>
       
        
    </div>
</asp:Content>