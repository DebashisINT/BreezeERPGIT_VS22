<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OldUnitReceived.aspx.cs" Inherits="ERP.OMS.Management.Activities.OldUnitReceived" MasterPageFile="~/OMS/MasterPage/ERP.Master" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .disableClick {
            pointer-events: none;
        }
        .padTab>tbody>tr>td {
            padding-right:15px;
        }
    </style>
    <%--Subhra--%>
    <script>
        var OldUnitRecvdId = 0;
        function onPrintJv(id, ReceivedNo) {
           
            OldUnitRecvdId = id;
            cSelectPanel.cpSuccess = "";
            cDocumentsPopup.Show();
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();

            //if (ReceivedNo != "") {
            //    cDocumentsPopup.Show();
            //    cCmbDesignName.SetSelectedIndex(0);
            //    cSelectPanel.PerformCallback('Bindalldesignes');
            //    $('#btnOK').focus();
            //}
            //else {
            //    jAlert('This Invoice not yet Received.Nothing to Print.');
            //}
        }



        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }
        function cSelectPanelEndCall(s, e) {
            
            if (cSelectPanel.cpSuccess != "") {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'OLDUNTRECVD';
                if (TotDocument.length > 0) {
                    for (var i = 0; i < TotDocument.length; i++) {
                        if (TotDocument[i] != "") {
                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + OldUnitRecvdId + '&PrintOption=' + TotDocument[i], '_blank')
                        }
                    }
                }
            }
            if (cSelectPanel.cpSuccess == "") {
                if (cSelectPanel.cpChecked != "") {
                    jAlert('Please check Original For Recipient and proceed.');
                }
                CselectOriginal.SetCheckState('UnChecked');
                CselectDuplicate.SetCheckState('UnChecked');
                CselectTriplicate.SetCheckState('UnChecked');
                cCmbDesignName.SetSelectedIndex(0);
            }
        }

        function updateGridByDate() {
            var sdate = cFormDate.GetValue();
            var edate = ctoDate.GetValue();

            var startDate = new Date(sdate);
            var endDate = new Date(edate);
            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else if (ccmbBranchfilter.GetValue() == null) {
                jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
            }
            else if (startDate > edate) {
                jAlert('From date can not be greater than To Date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else {
                $('#branchName').text(ccmbBranchfilter.GetText());
                //PopulateCurrentBankBalance(ccmbBranchfilter.GetValue());
                //if (page.activeTabIndex == 0) {
                //grid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
                //}
                //else if (page.activeTabIndex == 1) {
                //    cCustomerReceiptGrid.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue())
                //}
                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                grid.Refresh();
            }
        }
    </script>
    <%--Subhra--%>
    <script language="javascript" type="text/javascript">
        function OnStockClick(current_status, Invoice_Id, Invoice_Number, ViewMode, ReceivedNo) {
            //var url = 'OldUnitReceivedFromServiceCenter.aspx?key=' + Invoice_Id + '&Invoice_Number=' + Invoice_Number + '&ViewMode=' + ViewMode;
            var type = '';
            var rcv = ReceivedNo;
            var curr = current_status;
            if (rcv == '')
            {
                type="Add";
            }
            else
            {
                type = "Edit";
            }


            var url = 'OldUnitReceivedFromServiceCenter.aspx?key=' + Invoice_Id + '&ViewMode=' + ViewMode + '&rcv_no=' + ReceivedNo + '&Type=' + type;
            window.location.href = url;

        }
        function OnEditButtonClick(Invoice_Id, current_status, assign_to_branch, receiver_remark, assignee_remark) {

            //alert('Invoice_Id: ' + Invoice_Id);
            //alert('current_status: ' + current_status);
            //alert('assign_to_branch: ' + assign_to_branch);
            //alert('receiver_remark: ' + receiver_remark);
            //alert('assignee_remark: ' + assignee_remark);

            $("#hidden_Invoice_Id").val(Invoice_Id);

            if (current_status == 1) {
                cPopup_ReceivedBranch.Show();
                CtxtReceivedBranchremark.SetValue();
                $("#btnReceivedBranchSave").attr("disabled", false);
            }
            else if (current_status == 2) {
                cPopup_ReceivedBranch.Hide();
                CtxtReceivedBranchremark.SetValue(receiver_remark);
                CtxtReceivedBranchremark.SetEnabled(false);
                $("#btnReceivedBranchSave").attr("disabled", true);
            }
            else {
                //do nothing
            }
        }
        function CallReceivedBranch_save() {
            var flag = true;
            var Remarks = CtxtReceivedBranchremark.GetValue();

            if (Remarks == null || Remarks == '') {
                jAlert("Remarks is mandatory.");
            }
            else {
                $('#MandatoryAssignBranch').attr('style', 'display:none;position: absolute; right: -20px; top: 8px;');
                cPopup_ReceivedBranch.Hide();

                var tbl_trans_oldunit = {
                    receiver_remark: Remarks,
                    Invoice_Id: $("#hidden_Invoice_Id").val(),
                    current_status: 2
                }

                $.ajax({
                    type: "POST",
                    url: "OldUnitReceived.aspx/SaveReceivedBranch",
                    data: JSON.stringify({ 'tbl_trans_oldunit': tbl_trans_oldunit }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        CtxtReceivedBranchremark.SetValue();
                        grid.Refresh();
                        jAlert(data.d);
                        //IconChange();
                        location.reload();
                    },
                    failure: function (response) {
                        jAlert("Error");
                    }
                });
            }
        }
        function IconChange() {
            $(function () {

                var $tr = $("#gridStatus_DXMainTable > tbody > tr:gt(1)");

                $tr.each(function (index, value) {
                    
                    var $a = $(this).find("td").eq(10).find("a").eq(1);
                    var $current_status = $(this).find("td").eq(10).find("a").eq(1).attr("onclick");

                    if (typeof ($current_status) != "undefined") {
                        var params = $current_status.split("OnStockClick(")[1].split(',');
                        $current_status = params[0].replace("'", "").replace("'", "");
                    }

                    if ($current_status == 2) {
                        var Received_Img_Src = "../../../assests/images/verified.png";
                        $a.attr("title", "Old unit received");
                        $a.removeAttr("onclick");
                        $a.find("img").eq(0).attr('src', Received_Img_Src);
                    }

                });


            });
        }
        $(document).ready(function () {
            IconChange();
        });
        function OnBeginAfterCallback() {
            IconChange();
            $("#drdExport").val(0);
        }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            }
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
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Old Unit Received</h3>
        </div>
         <table class="padTab pull-right">
            <tr>
                <td>
                    From </td>
                <td style="width:150px">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" OnInit="FormDate_Init" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    To 
                </td>
                <td style="width:150px">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" OnInit="toDate_Init" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>

                </td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                    <%--<input type="button" value="Clear" class="btn btn-primary" onclick="ClearField()" />--%>
                </td>

            </tr>

        </table>
    </div>

    <div class="form_main">



        <div class="clearfix">
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
        </div>


        <table class="TableMain100" style="width: 100%">
            <tr>
                <td style="vertical-align: top; text-align: left" colspan="2">
                    <%--<dxe:ASPxGridView ID="gridStatus" ClientInstanceName="grid" Width="100%"
                        KeyFieldName="Invoice_Id" runat="server" OnCustomCallback="gridStatus_CustomCallback" 
                        AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true" OnDataBinding = "Grid_OldUnit_DataBinding" >--%>
                    <dxe:ASPxGridView ID="gridStatus" ClientInstanceName="grid" Width="100%" Settings-HorizontalScrollBarMode="Visible"
                            Settings-VerticalScrollableHeight="300"
                        KeyFieldName="Invoice_Id" runat="server" OnCustomCallback="gridStatus_CustomCallback" 
                        AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true" DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  >
                        <clientsideevents begincallback="OnBeginAfterCallback" endcallback="OnBeginAfterCallback" />
                       <SettingsSearchPanel Visible="True" Delay="5000" />
                         <columns>
                            <dxe:GridViewDataTextColumn Visible="False" FieldName="Invoice_Id" Caption="Invoice_Id" SortOrder="Descending">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Invoice_Number"
                                Caption="Document No." Width="150">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Invoice_Date"
                                Caption=" Posting Date" Width="100">
                                <CellStyle CssClass="gridcellleft" Wrap="True" >
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="True"></EditFormSettings>
                                 <%--<PropertiesTextEdit DisplayFormatString="dd-MM-yyyy" 
                                DisplayFormatInEditMode="True" ></PropertiesTextEdit> --%>
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
                                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <%--<dxe:GridViewDataTextColumn FieldName="EnteredBy"
                                Caption="Entered By" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>--%>

                            <dxe:GridViewDataTextColumn FieldName="UpdatedBy"
                                Caption="Assigned By" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="assigned_on"
                                Caption="Assigned on" Visible="True">
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

                            <dxe:GridViewDataTextColumn FieldName="Received"
                                Caption="Received" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="IsForm"
                                Caption="Type" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="ReceivedNo"
                                Caption="Received No" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>




                            <%--Suvankar 07092017--%>

                           <dxe:GridViewDataTextColumn FieldName="SHItem"
                               Caption="Second Hand Item" Visible="True">
                               <CellStyle CssClass="gridcellleft">
                               </CellStyle>
                               <Settings AllowAutoFilterTextInputTimer="False" />
                               <EditFormSettings Visible="False"></EditFormSettings>
                           </dxe:GridViewDataTextColumn>
                           <dxe:GridViewDataTextColumn FieldName="SHQty"
                               Caption="Qty" Visible="True">
                               <CellStyle CssClass="gridcellleft">
                               </CellStyle>
                               <Settings AllowAutoFilterTextInputTimer="False" />
                               <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                               <EditFormSettings Visible="False"></EditFormSettings>
                           </dxe:GridViewDataTextColumn>
                           <dxe:GridViewDataTextColumn FieldName="SHValue"
                               Caption="Value" Visible="True">
                               <CellStyle CssClass="gridcellleft">
                               </CellStyle>
                               <Settings AllowAutoFilterTextInputTimer="False" />
                               <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                               <EditFormSettings Visible="False"></EditFormSettings>
                           </dxe:GridViewDataTextColumn>


                          <dxe:GridViewDataTextColumn FieldName="Dnote_No"
                                Caption="Debit Note No." Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                              <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Dnote_Amount"
                                Caption="Debit Note Amt." Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>



                           <%--End--%>

                            <dxe:GridViewDataTextColumn CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="170">
                                <DataItemTemplate>

                                    <% if (rights.CanAdd)
                                       { %>
                                    <a href="javascript:void(0);" title="View" onclick="OnStockClick('<%# Eval("current_status") %>', '<%# Container.KeyValue%>', '<%# Eval("Invoice_Number") %>', 'yes') " >
                                        <img src="../../../assests/images/viewIcon.png" />
                                    </a>
                                    <a href="javascript:void(0);" onclick="OnStockClick('<%# Eval("current_status") %>', '<%# Container.KeyValue%>', '<%# Eval("Invoice_Number") %>', 'no','<%# Eval("ReceivedNo") %>')" title="Receive old unit" class="pad" style='<%#Eval("OldStyle")%>'>
                                        <img src="../../../assests/images/info.png" />
                                    </a>
                                    <%--<a href="javascript:void(0);" onclick="OnEditButtonClick('<%# Container.KeyValue %>', '<%# Eval("current_status") %>', '<%# Eval("assign_to_branch") %>', '<%# Eval("receiver_remark") %>', '<%# Eval("assignee_remark") %>')" title="More Info" class="pad">
                                        <img src="../../../assests/images/info.png" />
                                    </a>--%>
                                    <%--<% } %>
                                    <% else
                                       { %>
                                    <a href="javascript:void(0);" title="received" class="disableClick">
                                        <img src="../../../assests/images/verified.png" />
                                    </a>
                                    <% } %>--%>

                                    <%--<% if (rights.CanDelete)
                                       { %>--%>
                                    <a href="javascript:void(0);" title="Print" style="display: none;">
                                        <img src="../../../assests/images/print.png" />
                                    </a>
                                    <% } %>


                                     <% if (rights.CanPrint)
                                        { %>
                                        <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>','<%# Eval("ReceivedNo") %>')" class="pad" title="print">
                                            <img src="../../../assests/images/Print.png" />
                                        </a><%} %>
                                </DataItemTemplate>
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                        </columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <settingspager pagesize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </settingspager>
                        <styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <Cell CssClass="gridcellleft">
                            </Cell>
                        </styles>
                        <settings showgrouppanel="True"   showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="True" />
                     <%--   <settingssearchpanel visible="True" />--%>
                    </dxe:ASPxGridView>
                    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="V_OldUnitReceivedList" />
                    <asp:HiddenField ID="hfIsFilter" runat="server" />
                    <asp:HiddenField ID="hfFromDate" runat="server" />
                    <asp:HiddenField ID="hfToDate" runat="server" />
                    <asp:HiddenField ID="hfBranchID" runat="server" />
                    <input type="hidden" id="hidden_Invoice_Id" />
                </td>
            </tr>
        </table>
        <div style="display: none">
            <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridStatus" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>
        </div>
        <%--        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>--%>
    </div>
    <dxe:ASPxPopupControl ID="Popup_ReceivedBranch" runat="server" ClientInstanceName="cPopup_ReceivedBranch"
        Width="400px" HeaderText="Received Branch" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <contentcollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">

                    <table style="width: 94%">

                        <tr>
                            <td>Remark:<span style="color: red">*</span></td>
                            <td class="relative">
                                <dxe:ASPxMemo ID="txtReceivedBranchremark" runat="server" Width="100%" Height="50px" ClientInstanceName="CtxtReceivedBranchremark"></dxe:ASPxMemo>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 26px"></td>
                        </tr>
                        <tr>
                            <td colspan="3" style="padding-left: 121px;">
                                <input id="btnReceivedBranchSave" class="btn btn-primary" onclick="CallReceivedBranch_save()" type="button" value="Received" />
                                <input id="btnAssignBranchCancel" class="btn btn-danger" onclick='cPopup_ReceivedBranch.Hide();' type="button" value="Cancel" />
                            </td>

                        </tr>

                    </table>


                </div>

            </dxe:PopupControlContentControl>
        </contentcollection>
        <headerstyle backcolor="LightGray" forecolor="Black" />


    </dxe:ASPxPopupControl>

    <%--SUBHRA--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <contentcollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxCheckBox ID="selectOriginal" Text="Original For Recipient" runat="server" ToolTip="Select Original" 
                                    ClientInstanceName="CselectOriginal">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectDuplicate" Text="Duplicate For Transporter" runat="server" ToolTip="Select Duplicate" 
                                    ClientInstanceName="CselectDuplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectTriplicate" Text="Triplicate For Supplier" runat="server" ToolTip="Select Triplicate"
                                    ClientInstanceName="CselectTriplicate">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </contentcollection>
        </dxe:ASPxPopupControl>
    </div>
</asp:Content>
