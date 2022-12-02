<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OldUnitAssign.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.OldUnitAssign" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .disableClick {
            pointer-events: none;
        }

        #txtOldUnitqty_ET .dxeErrorFrameSys.dxeErrorCellSys,
        #txtoldUnitValue_ET .dxeErrorFrameSys.dxeErrorCellSys {
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">

        function OnBranchAssignButtonClick(Invoice_Id, current_status, assign_to_branch, receiver_remark, assignee_remark) {

            $("#hidden_Invoice_Id").val(Invoice_Id);
            $("#hfInvoice_Id").val(Invoice_Id);
            $('#MandatoryAssignBranch').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');

            if (current_status == 0) {
                cPopup_AssignBranch.Show();
                CtxtAssignBranchremark.SetValue();
                cCmbBranch.SetValue();

                CtxtAssignBranchremark.SetEnabled(true);
                cCmbBranch.SetEnabled(true);
                $("#btnAssignBranchSave").attr("disabled", false);
            }
            else if (current_status == 1 || current_status == 2) {
                cPopup_AssignBranch.Show();
                CtxtAssignBranchremark.SetValue(assignee_remark);
                cCmbBranch.SetValue(assign_to_branch);

                CtxtAssignBranchremark.SetEnabled(false);
                cCmbBranch.SetEnabled(false);
                $("#btnAssignBranchSave").attr("disabled", true);
            }
            else {
                //do nothing
            }
        }
        function OldUnitAddButtonClick(Invoice_Id, current_status) {

            //if (current_status == 1 || current_status == 2) {
            //    CtxtAssignBranchremark.SetEnabled(false);
            //    cCmbBranch.SetEnabled(false);
            //    $("#btnAssignBranchSave").attr("disabled", true);
            //    $("#hdAddOrEdit").val("View");

            //    coldunitPopupSaveAndClickClick.SetVisible(false);
            //    coldUnitGridAdd.SetVisible(false);
            //    coldUnitGridClear.SetVisible(false);
            //    cOldUnitGrid.SetEnabled(false);
            //}
            //else {
            //    coldunitPopupSaveAndClickClick.SetVisible(true);
            //    coldUnitGridAdd.SetVisible(true);
            //    coldUnitGridClear.SetVisible(true);
            //    cOldUnitGrid.SetEnabled(true);

            //}

            $("#hfInvoice_Id").val(Invoice_Id)
            OldUnitButtonOnClick();
            coldUnitProductLookUp.Focus();
        }
        function CallAssignBranch_save() {

            var flag = true;
            var Remarks = CtxtAssignBranchremark.GetValue();
            var branch = cCmbBranch.GetValue();

            if (branch == "" || branch == null || branch == 0) {
                $('#MandatoryAssignBranch').attr('style', 'display:block;position: absolute;right: 13px;top: 35px;');
                flag = false;
            }
            else {
                $('#MandatoryAssignBranch').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                cPopup_AssignBranch.Hide();

                var tbl_trans_oldunit = {
                    assign_to_branch: branch,
                    assignee_remark: Remarks,
                    Invoice_Id: $("#hidden_Invoice_Id").val(),
                    current_status: 1
                }

                $.ajax({
                    type: "POST",
                    url: "OldUnitAssign.aspx/SaveAssignedBranch",
                    data: JSON.stringify({ 'tbl_trans_oldunit': tbl_trans_oldunit }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        CtxtAssignBranchremark.SetValue();
                        cCmbBranch.SetValue()
                        grid.Refresh();
                        jAlert(data.d, "Alert!!", function () {
                            grid.Refresh();
                        });

                    },
                    failure: function (response) {
                        jAlert("Error");
                    }
                });
            }
            return flag;
        }
        function IconChange() {
            $(function () {

                var $tr = $("#gridStatus_DXMainTable > tbody > tr:gt(1)");

                $tr.each(function (index, value) {
                    var $a = $(this).find("td").eq(9).find("a");
                    var $current_status = $(this).find("td").eq(9).find("a").attr("onclick");
                    var params = $current_status.split("OnBranchAssignButtonClick(")[1].split(',');
                    var $current_status = params[1].replace("'", "").replace("'", "");

                    if ($current_status == 2) {
                        var Received_Img_Src = "../../../assests/images/verified.png";
                        $a.attr("title", "Old unit received");
                        $a.find("img").eq(0).attr('src', Received_Img_Src);
                    }

                });


            });
        }
        function DisabledBranchAssign() {
            $(function () {

                var $tr = $("#gridStatus_DXMainTable > tbody > tr:gt(1)");

                $tr.each(function (index, value) {

                    var $a = $(this).find("td").eq(10).find("a");
                    var $IsForm = $(this).find("td").eq(9).text();

                    if ($IsForm != null && $IsForm == "Advice") {
                        $a.eq(0).show();
                    }
                    else {
                        $a.eq(0).hide();
                    }

                });


            });
        }
        $(document).ready(function () {
            //IconChange();
            DisabledBranchAssign();
        });
        function OnBeginAfterCallback(s, e) {
            //IconChange();
            DisabledBranchAssign();
        }
        function CmbScheme_ValueChange() {

            var val = document.getElementById("CmbScheme").value;
            $('#<%=hdnSchemaID.ClientID %>').val(val);
            $("#MandatoryBillNo").hide();

            if (val != "0") {
                $.ajax({
                    type: "POST",
                    url: 'OldUnitAssign.aspx/getSchemeType',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{sel_scheme_id:\"" + val + "\"}",
                    success: function (type) {
                        console.log(type);

                        var schemetypeValue = type.d;
                        var schemetype = schemetypeValue.toString().split('~')[0];
                        var schemelength = schemetypeValue.toString().split('~')[1];
                        $('#txtBillNo').attr('maxLength', schemelength);

                        if (schemetype == '0') {
                            $('#<%=hdnSchemaType.ClientID %>').val('0');
                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = false;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                            document.getElementById("txtBillNo").focus();
                        }
                        else if (schemetype == '1') {
                            $('#<%=hdnSchemaType.ClientID %>').val('1');
                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "Auto";
                            //tDate.SetValue(new Date());
                            tDate.Focus();
                        }
                        else if (schemetype == '2') {
                            $('#<%=hdnSchemaType.ClientID %>').val('2');
                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "Datewise";
                        }
                        else {
                            document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                            document.getElementById('<%= txtBillNo.ClientID %>').value = "";
                        }
                    }
                });
    }
    else {
        document.getElementById('<%= txtBillNo.ClientID %>').disabled = true;
                document.getElementById('<%= txtBillNo.ClientID %>').value = "";
            }
        }
        function txtBillNo_TextChanged() {
            var VoucherNo = document.getElementById("txtBillNo").value;
            <%--var type = $('#<%=hdnMode.ClientID %>').val();--%>
            var type = "1";

            if (VoucherNo != "") {
                $("#MandatoryBillNo").hide();
            }

            $.ajax({
                type: "POST",
                url: "OldUnitAssign.aspx/CheckUniqueName",
                data: JSON.stringify({ VoucherNo: VoucherNo, Type: type }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        $("#duplicateMandatoryBillNo").show();
                        document.getElementById("txtBillNo").value = '';
                        document.getElementById("<%=txtBillNo.ClientID%>").focus();
                    }
                    else {
                        $("#duplicateMandatoryBillNo").hide();
                    }
                }
            });
        }
    </script>
    <%--Old Unit Script Start--%>
    <script type="text/javascript">
        function oldUnitGridClearClick() {
            coldUnitUpdatePanel.PerformCallback('Clear');

        }
        function oldunitPopupSaveAndEXitClick() {

            var VoucherNo = document.getElementById("txtBillNo").value;
            var CmbScheme = $("#CmbScheme").val();
            var VoucherDate = tDate.GetDate();

            if (($("#CmbScheme").is(":visible")) && CmbScheme != null && CmbScheme == '0') {
                jAlert("Numbering Scheme is mandatory.", "Alert!", function () {
                    document.getElementById("CmbScheme").focus();
                });
            }

            else if (VoucherNo != null && VoucherNo == '') {
                jAlert("Document No is mandatory.", "Alert!", function () {
                    document.getElementById("txtBillNo").focus();
                });
            }

            else if (VoucherDate != null && VoucherDate == '') {
                jAlert("Date is mandatory.", "Alert!", function () {
                    tDate.focus();
                });
            }
            else {
                $("#MandatoryBillNo").hide();
                var Invoice_Id = $("#hfInvoice_Id").val();
                cOldUnitPopUpControl.Hide();
                coldUnitUpdatePanel.PerformCallback('SaveOldUnit~' + Invoice_Id);
            }
        }
        function oldUnitGridRowChange() {
            if (cOldUnitGrid.GetVisibleRowsOnPage() > 0) {
                if (document.getElementById('hdAddOrEdit').value != "Edit") {
                    coldunitPopupSaveAndClickClick.SetVisible(true);
                }
            } else {
                coldunitPopupSaveAndClickClick.SetVisible(false);
            }
        }

        function OldUnitGridEndCallback() {

            if (coldUnitUpdatePanel.cpReturnString) {
                if (coldUnitUpdatePanel.cpReturnString != "") {
                    if (coldUnitUpdatePanel.cpReturnString == 'AddDataToTable') {
                        ClearOldUnitData();
                        //coldUnitProductLookUp.Focus();
                        coldUnitUpdatePanel.cpReturnString = null;
                    }
                }
            }
            if (coldUnitUpdatePanel.cpSave != null && coldUnitUpdatePanel.cpSave != "") {
                jAlert(coldUnitUpdatePanel.cpSave, "Alert!!", function () {
                    grid.Refresh();
                });
            }
            if (coldUnitUpdatePanel.cpDuplicateProduct != null && coldUnitUpdatePanel.cpDuplicateProduct != "") {
                jAlert("Duplicate product can not be added.", "Alert!!", function () {
                   // coldUnitProductLookUp.Focus();
                });
            }
            if (coldUnitUpdatePanel.cpClear != null && coldUnitUpdatePanel.cpClear != "") {
                ClearOldUnitData();
                //coldUnitProductLookUp.Focus();
                $('#HdDiscountAmount').val('0');
            }
            if (coldUnitUpdatePanel.cpDisplay != null && coldUnitUpdatePanel.cpDisplay != "") {
                cOldUnitPopUpControl.SetHeaderText(coldUnitUpdatePanel.cpDisplay);
                cOldUnitPopUpControl.Show();

                //Clear Fields while display
                ClearOldUnitData();
                $('#HdDiscountAmount').val('0');
            }

            oldUnitGridRowChange();
        }

        function ClearOldUnitData() {
            coldUnitProductLookUp.Clear();
            ctxtOldUnitUom.SetText('');
            ctxtOldUnitqty.SetText('');
            ctxtoldUnitValue.SetText('');
        }

        function OldUnitButtonOnClick() {

            //cOldUnitPopUpControl.Show();
            ClearOldUnitData();
            $('#HdDiscountAmount').val('0');
            coldUnitUpdatePanel.PerformCallback('DisplayOldUnit');

        }

        function oldUnitProductTextChanged(s, e) {
            var key = coldUnitProductLookUp.GetGridView().GetRowKey(coldUnitProductLookUp.GetGridView().GetFocusedRowIndex());
            ctxtOldUnitUom.SetText(key.split('|@|')[1]);
        }
        function oldUnitProductGotFocus(s, e) {
            coldUnitProductLookUp.ShowDropDown();
        }
        function fn_EditOldUnit(keyVal) {
            coldUnitUpdatePanel.PerformCallback("Update~" + keyVal);
        }

        function fn_removeOldUnit(keyVal) {
            coldUnitUpdatePanel.PerformCallback("DeleteFromTable~" + keyVal);
        }

        function oldUnitGridAddClick() {
            debugger;
            $('#mandetoryOldUnit').attr('style', 'display:none');
            var focusedRow = coldUnitProductLookUp.gridView.GetFocusedRowIndex();

            var MRP = 0;
            if (focusedRow >= 0) {
                MRP = parseFloat(coldUnitProductLookUp.gridView.GetRow(focusedRow).children[5].innerText);
            }

            if (coldUnitProductLookUp.GetValue() == null) {
                $('#mandetoryOldUnit').attr('style', 'display:block');
            }
            else if (ctxtoldUnitValue.GetValue() == null || ctxtoldUnitValue.GetValue() == '0.00') {
                jAlert("Old Unit Value cannot be zero.", "Alert", function () { ctxtoldUnitValue.Focus(); });
            }
            else if (MRP != 0 && ctxtoldUnitValue.GetValue() > MRP) {
                var roundOfValue = parseFloat(Math.round(Math.abs(MRP) * 100) / 100).toFixed(2);
                jAlert("Old Unit Value cannot be Greater then MRP defined.", "Alert", function () { ctxtoldUnitValue.Focus(); });
            }
            else {
                coldUnitUpdatePanel.PerformCallback("AddDataToTable");
            }
        }
    </script>
    <%--Old Unit Script End--%>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Old Unit Advice & Branch Assign</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100" style="width: 100%">
            <tr>
                <td style="vertical-align: top; text-align: left" colspan="2">
                    <dxe:ASPxGridView ID="gridStatus" ClientInstanceName="grid" Width="100%"
                        KeyFieldName="Invoice_Id" runat="server"
                        AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true" DataSourceID="EntityServerModeDataSource"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  >
                        <ClientSideEvents BeginCallback="OnBeginAfterCallback" EndCallback="OnBeginAfterCallback" />
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" FieldName="Invoice_Id" Caption="Invoice_Id" SortOrder="Descending">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Invoice_Number"
                                Caption="Document No.">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Invoice_Date"
                                Caption="Posting Date" >
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="True"></EditFormSettings>
                                <%--<PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"  DisplayFormatInEditMode="True">  </PropertiesTextEdit>--%>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="CustomerName"
                                Caption="Customer">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Invoice_TotalAmount"
                                Caption="Net Amount" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="EnteredBy"
                                Caption="Entered By" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="assigned_on"
                                Caption="Last updated on" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="UpdatedBy"
                                Caption="Updated By" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="branch_code"
                                Caption="Assigned Unit" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Assigned"
                                Caption="Assigned" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn
                                Caption="Old Unit" Visible="false">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <%--<dxe:GridViewDataTextColumn
                                Caption="Type" Visible="true" FieldName="IsForm" Width="0">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>
                            <dxe:GridViewDataColumn FieldName="IsForm" Caption="Type" Width="0px" CellStyle-CssClass="hide" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataColumn>

                            <dxe:GridViewDataTextColumn CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="120px">
                                <DataItemTemplate>

                                    <% if (rights.CanAdd)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnBranchAssignButtonClick('<%# Container.KeyValue %>', '<%# Eval("current_status") %>', '<%# Eval("assign_to_branch") %>', '<%# Eval("receiver_remark") %>', '<%# Eval("assignee_remark") %>')" title="Assigned Branch" class="pad">
                                        <span class="fa fa-truck out"></span>
                                    </a>
                                    <a href="javascript:void(0);" title="Add Old Unit Advice" onclick="OldUnitAddButtonClick('<%# Container.KeyValue %>', '<%# Eval("current_status") %>')">
                                        <img src="../../../assests/images/Add.png" />
                                    </a>
                                    <a href="javascript:void(0);" title="Print" style="display: none;">
                                        <img src="../../../assests/images/print.png" />
                                    </a>
                                    <% } %>
                                </DataItemTemplate>
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <Cell CssClass="gridcellleft">
                            </Cell>
                        </Styles>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="True" />
                        <%--<SettingsSearchPanel Visible="True" />--%>
                    </dxe:ASPxGridView>
                    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="V_SalesChallan_Details_OldUnitAssignList" />
                    <input type="hidden" id="hidden_Invoice_Id" />
                </td>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>
    <dxe:ASPxPopupControl ID="Popup_AssignBranch" runat="server" ClientInstanceName="cPopup_AssignBranch"
        Width="400px" HeaderText="Assign Branch" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <div>
                        <label>Assign Branch:<span style="color: red">*</span></label>
                        <div>
                            <dxe:ASPxComboBox ID="CmbBranch" EnableIncrementalFiltering="True" ClientInstanceName="cCmbBranch"
                                TextField="branch_code" ValueField="branch_id"
                                runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                            </dxe:ASPxComboBox>
                            <span id="MandatoryAssignBranch" style="display: none">
                                <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                        </div>
                    </div>
                    <div>
                        <label style="margin-top: 8px">Remark:</label>
                        <div>
                            <dxe:ASPxMemo ID="txtAssignBranchremark" runat="server" Width="100%" Height="50px" ClientInstanceName="CtxtAssignBranchremark"></dxe:ASPxMemo>
                        </div>
                    </div>
                    <div style="padding-top: 15px; text-align: center;">
                        <input id="btnAssignBranchSave" class="btn btn-primary" onclick="CallAssignBranch_save()" type="button" value="Assign" />
                        <input id="btnAssignBranchCancel" class="btn btn-danger" onclick='cPopup_AssignBranch.Hide();' type="button" value="Cancel" />
                    </div>



                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />


    </dxe:ASPxPopupControl>
    <dxe:ASPxPopupControl ID="OldUnitPopUpControl" runat="server" Width="1100" Height="500"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cOldUnitPopUpControl"
        HeaderText="Old Unit Details" AllowResize="false" ResizingMode="Postponed" Modal="true">
        <ContentCollection>

            <dxe:PopupControlContentControl runat="server">

                <dxe:ASPxCallbackPanel runat="server" ID="oldUnitUpdatePanel" ClientInstanceName="coldUnitUpdatePanel" OnCallback="oldUnitUpdatePanel_Callback">
                    <PanelCollection>
                        <dxe:PanelContent runat="server">
                            <asp:HiddenField ID="hdAddOrEdit" runat="server" />
                            <asp:HiddenField runat="server" ID="uniqueId" />
                            <asp:HiddenField runat="server" ID="hfInvoice_Id" />
                            <asp:HiddenField ID="hdnSchemaID" runat="server" Value="0" />
                            <asp:HiddenField ID="hdnSchemaType" runat="server" />
                            <asp:HiddenField ID="hdnMode" runat="server" />

                            <div class="row">
                                <asp:Panel ID="pnlschema" runat="server">
                                    <div class="col-md-3" id="div_Edit">
                                        <label>Select Numbering Scheme</label>
                                        <div>
                                            <asp:DropDownList ID="CmbScheme" runat="server" TabIndex="2"
                                                Width="100%"
                                                onchange="CmbScheme_ValueChange()">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="col-md-3">
                                    <label>Document No.</label>
                                    <div>
                                        <asp:TextBox ID="txtBillNo" runat="server" Width="95%" meta:resourcekey="txtBillNoResource1" TabIndex="3" MaxLength="30" onchange="txtBillNo_TextChanged()"></asp:TextBox>
                                        <span id="MandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        <span id="duplicateMandatoryBillNo" class="pullleftClass fa fa-exclamation-circle iconRed" style="color: red; position: absolute; display: none" title="Duplicate Journal No."></span>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <label style="">Date</label>
                                    <div>
                                        <dxe:ASPxDateEdit ID="tDate" runat="server" EditFormatString="dd-MM-yyyy" ClientInstanceName="tDate" TabIndex="4"
                                            UseMaskBehavior="True" Width="100%" meta:resourcekey="tDateResource1" DisplayFormatString="dd-MM-yyyy" >
                                            <%--<ClientSideEvents DateChanged="function(s,e){DateChange()}" />--%>
                                        </dxe:ASPxDateEdit>
                                    </div>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <div class="row">
                                <div class="col-md-3">
                                    <label style="margin-top: 0px !important">Select Old Unit</label>

                                    <dxe:ASPxGridLookup ID="oldUnitProductLookUp" runat="server" DataSourceID="oldUnitDataSource" ClientInstanceName="coldUnitProductLookUp"
                                        KeyFieldName="sProducts_ID" Width="100%" TextFormatString="{0}+{1}" MultiTextSeparator=", ">
                                        <Columns>

                                            <dxe:GridViewDataColumn FieldName="sProducts_Code" Caption="Name" Width="100">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                            <dxe:GridViewDataColumn FieldName="sProducts_Name" Caption="Description" Width="180">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>

                                            <dxe:GridViewDataColumn FieldName="sProduct_IsInventory" Caption="Inventory" Width="50">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="hsnCode" Caption="HSN/SAC" Width="80">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="ProductClass_Code" Caption="Class" Width="200">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="MRP" Caption="MRP" Width="0">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                            <dxe:GridViewDataColumn FieldName="sProducts_Code" Caption="sProducts_Code" Width="0">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dxe:GridViewDataColumn>
                                        </Columns>
                                        <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                            <Templates>
                                                <StatusBar>
                                                    <table class="OptionsTable" style="float: right">
                                                        <tr>
                                                            <td>
                                                                <%--  <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </StatusBar>
                                            </Templates>
                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                        </GridViewProperties>
                                        <ClientSideEvents TextChanged="oldUnitProductTextChanged" GotFocus="oldUnitProductGotFocus" />
                                    </dxe:ASPxGridLookup>
                                    <span id="mandetoryOldUnit" style="display: none; top: -18px; left: -95px;">
                                        <img id="mandetoryOldUnitimg" style="position: absolute; right: -2px; top: 24px;" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory">
                                    </span>
                                </div>
                                <div class="col-md-2">
                                    <label>UOM</label>
                                    <dxe:ASPxTextBox ID="txtOldUnitUom" runat="server" ClientInstanceName="ctxtOldUnitUom" Width="100%" ClientEnabled="False">
                                    </dxe:ASPxTextBox>
                                </div>
                                <div class="col-md-1">
                                    <label>Quantity</label>
                                    <dxe:ASPxTextBox ID="txtOldUnitqty" runat="server" ClientInstanceName="ctxtOldUnitqty" Width="100%">
                                        <MaskSettings Mask="<1..9999999>" AllowMouseWheel="false" />
                                    </dxe:ASPxTextBox>
                                </div>
                                <div class="col-md-1">
                                    <label>Value</label>
                                    <dxe:ASPxTextBox ID="txtoldUnitValue" runat="server" ClientInstanceName="ctxtoldUnitValue" Width="100%">
                                        <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" />
                                    </dxe:ASPxTextBox>
                                </div>

                                <div class="col-md-3 pdTop15">
                                    <dxe:ASPxButton ID="oldUnitGridAdd" ClientInstanceName="coldUnitGridAdd" runat="server" Text="Add" AutoPostBack="false" CssClass="btn btn-primary mTop16" UseSubmitBehavior="false">
                                        <ClientSideEvents Click="oldUnitGridAddClick" />
                                    </dxe:ASPxButton>

                                    <dxe:ASPxButton ID="oldUnitGridClear" ClientInstanceName="coldUnitGridClear" runat="server" Text="Clear" AutoPostBack="false" CssClass="btn btn-danger mTop16">
                                        <ClientSideEvents Click="oldUnitGridClearClick" />
                                    </dxe:ASPxButton>
                                </div>
                            </div>
                            <div class="clear"></div>
                            <asp:SqlDataSource runat="server" ID="oldUnitDataSource" 
                                SelectCommand="prc_PosSalesInvoice" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:Parameter Type="String" Name="action" DefaultValue="OldUnitProductDetails" />
                                </SelectParameters>
                            </asp:SqlDataSource>

                            <div class="GridViewArea">
                                <dxe:ASPxGridView ID="OldUnitGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="cOldUnitGrid"
                                    Width="100%" OnCustomCallback="OldUnitGrid_CustomCallback" CssClass="pull-left" KeyFieldName="oldUnit_id">
                                    <Columns>

                                        <dxe:GridViewDataTextColumn Caption="Product Details" FieldName="Product_Des" ReadOnly="True"
                                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                                            <EditFormSettings Visible="True" />
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="UOM" FieldName="oldUnit_Uom" ReadOnly="True"
                                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                                            <EditFormSettings Visible="True" />
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="oldUnit_qty" ReadOnly="True"
                                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                                            <EditFormSettings Visible="True" />
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn Caption="Value" FieldName="oldUnit_value" ReadOnly="True"
                                            Visible="True" FixedStyle="Left" VisibleIndex="0">
                                            <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                <MaskSettings Mask="<0..999999999999>.<0..99>" AllowMouseWheel="false" />
                                            </PropertiesTextEdit>
                                            <EditFormSettings Visible="True" />
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataTextColumn>

                                        <dxe:GridViewDataTextColumn ReadOnly="False" Width="12%" CellStyle-HorizontalAlign="Center">
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <CellStyle HorizontalAlign="Center"></CellStyle>
                                            <HeaderTemplate>
                                                Actions
                                            </HeaderTemplate>
                                            <DataItemTemplate>

                                                <%if (hdAddOrEdit.Value != "Edit")
                                                  { %>
                                                <a href="javascript:void(0);" onclick="fn_EditOldUnit('<%# Container.KeyValue %>')" title="Edit" class="pad">
                                                    <img src="/assests/images/Edit.png" /></a>

                                                <a href="javascript:void(0);" onclick="fn_removeOldUnit('<%# Container.KeyValue %>')" title="Delete" class="pad">
                                                    <img src="/assests/images/Delete.png" /></a>
                                                <%} %>
                                            </DataItemTemplate>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <%--<ClientSideEvents EndCallback="OldUnitGridEndCallback"></ClientSideEvents>--%>
                                </dxe:ASPxGridView>
                            </div>
                            <div class="clear"></div>
                            <div style="padding-top: 5px;">
                                <dxe:ASPxButton ID="oldunitPopupSaveAndClickClick" ClientInstanceName="coldunitPopupSaveAndClickClick" runat="server" Text="Save" AutoPostBack="false" CssClass="btn btn-primary mTop16">
                                    <ClientSideEvents Click="oldunitPopupSaveAndEXitClick" />
                                </dxe:ASPxButton>
                            </div>
                        </dxe:PanelContent>
                    </PanelCollection>
                    <ClientSideEvents EndCallback="OldUnitGridEndCallback" />
                </dxe:ASPxCallbackPanel>

            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
</asp:Content>
