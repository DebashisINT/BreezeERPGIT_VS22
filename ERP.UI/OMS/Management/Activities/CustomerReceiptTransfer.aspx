<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="CustomerReceiptTransfer.aspx.cs" Inherits="ERP.OMS.Management.Activities.CustomerReceiptTransfer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
         .iconBranch {
            position: absolute;
            right: -1px;
            top: 36px;
        }
          .iconCashBank{
            position: absolute;
            right: -1px;
            top: 29px;
        }
      </style>
    <script type="text/javascript">
        //function ddlBranch_SelectedIndexChanged() {
        //    var branch = $("#ddlBranch").val();
        //    cddlCashBank.PerformCallback(branch);
        //}
        var jsonBranch = [];
        var oldBranchId = "";
        $(document).ready(function () {

            jsonBranch= JSON.parse($('#hdBranchDetails').val());

        });
        
        function BranchGotFocus() {
            oldBranchId = $('#ddlBranch').val();
        }

        function onAssignBranchChange() {
            var selectedBranch = $('#ddlBranch').val();
            for (var i = 0; i < jsonBranch.length; i++)
            {
                if (jsonBranch[i].branch_id == selectedBranch) {
                    if (jsonBranch[i].branch_MainAccount.trim() == "") {
                        jAlert(" You must map ledger for the Selected Branch to post data from Branch Master. It is not mapped. Cannot proceed.");
                        $('#ddlBranch').val(oldBranchId);
                    }
                }
            }
            oldBranchId = $('#ddlBranch').val();
        }

        function OnTransferClick(keyValue) {
            cPopup_BranchTransfer.Show();
          
            cBranchTransferCallBackPanel.PerformCallback('Edit~'+keyValue);
        }
        function SaveButtonClick()
        {
            var branch = $("#ddlBranch").val();
            if (branch == "") {
                $("#MandatoryBranch").show();
                return false;
            }
            var fromBranchId = parseFloat( $('#hdFromBranchID').val());
            var findObj = $.grep(jsonBranch, function (e) { return e.branch_id == fromBranchId; })
            if (findObj[0].branch_MainAccount.trim() == "") {
                jAlert("You must map ledger for the Selected Branch to post data from Branch Master. It is not mapped. Cannot proceed.");
                return false;
            }
            //var CashBank = cddlCashBank.GetValue();
            //if (CashBank == null) {
            //    $("#MandatoryCashBank").show();
            //    return false;
            //}
            var EditID = document.getElementById('hdnEditID').value;
            //var branch = $("#ddlBranch").val();
            cBranchTransferCallBackPanel.PerformCallback('Save~' + EditID+'~'+ branch);
        }
        function BranchTransferEndCallBack()
        {
            if (cBranchTransferCallBackPanel.cpEdit != null)
            {               
                document.getElementById('hdnEditID').value = cBranchTransferCallBackPanel.cpEdit;
                cBranchTransferCallBackPanel.cpEdit = null;
            }
            if (cBranchTransferCallBackPanel.cpTransfer =="YES")
            {
                cPopup_BranchTransfer.Hide();
                window.location.href = 'CustomerReceiptTransfer.aspx';
                cBranchTransferCallBackPanel.cpTransfer = null;
            }
            if (cBranchTransferCallBackPanel.cpBtnVisible != null && cBranchTransferCallBackPanel.cpBtnVisible != "") {
                cBranchTransferCallBackPanel.cpBtnVisible = null;
                document.getElementById('btnSaveNew').style.display = 'none'
                document.getElementById('tagged').style.display = 'block';
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title" id="td_contact1" runat="server">
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Customer Advance Transfer"></asp:Label>
            </h3>
        </div>

    </div>
    
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="Popup_BranchTransfer" runat="server" ClientInstanceName="cPopup_BranchTransfer"
            Width="450px" HeaderText="Transfer to Branch" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="200px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="BranchTransferCallBackPanel" ClientInstanceName="cBranchTransferCallBackPanel" OnCallback="BranchTransferCallBackPanel_Callback">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <div style="background: #f5f4f3; padding: 5px 0 5px 0; margin-bottom: 10px; border-radius: 4px; border: 1px solid #ccc;">
                                    <div class="Top clearfix">

                                         <div class="col-md-12">
                                            <label>Transfer From : </label>
                                             <strong>
                                            <asp:Label ID="lblTransferFrom" runat="server" Text="" />

                                             </strong>
                                        </div>
                                        <div class="clear"></div>

                                        <div class="col-md-12">
                                            <label>Transfer To<span style="color: red">*</span></label>
                                            <div>
                                                <asp:DropDownList ID="ddlBranch" runat="server" DataSourceID="dsBranch" onChange="onAssignBranchChange()" onfocus="BranchGotFocus()"
                                                    DataTextField="BANKBRANCH_NAME" DataValueField="BANKBRANCH_ID" Width="100%" >
                                                </asp:DropDownList>
                                                <span id="MandatoryBranch" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none"
                                                    title="Mandatory"></span>
                                            </div>
                                        </div>
                                        <%--<div class="col-md-6" id="tdCashBankLabel">
                                            <label>Cash/Bank <span style="color: red">*</span></label>
                                            <dxe:ASPxComboBox ID="ddlCashBank" runat="server" ClientIDMode="Static" ClientInstanceName="cddlCashBank" Width="100%" OnCallback="ddlCashBank_Callback">
                                                
                                            </dxe:ASPxComboBox>
                                            <span id="MandatoryCashBank" class="iconCashBank pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none" title="Mandatory"></span>
                                        </div>--%>
                                        <div class="clear"></div>
                                        <div class="col-md-12 lblmTop8">
                                            <label>Remarks </label>
                                            <div>
                                                <asp:TextBox ID="txtNarration" runat="server" MaxLength="500" onkeydown="checkTextAreaMaxLength(this,event,'500');"
                                                    TextMode="MultiLine"
                                                    Width="100%" Height="80px" onchange="txtNarrationTextChanged()"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <table style="width: 100%;">
                                    <tr>
                                        <td style="padding: 5px 0;">
                                            <div class="text-center">
                                                <dxe:ASPxButton ID="btnSaveNew" ClientInstanceName="cbtnSaveNew" runat="server"
                                                    AutoPostBack="false" CssClass="btn btn-primary" TabIndex="0" Text="Save & C&#818;lose"
                                                    UseSubmitBehavior="False">
                                                    <ClientSideEvents Click="function(s, e) {SaveButtonClick();}" />
                                                </dxe:ASPxButton>
                                                  <b><span id="tagged" style="display:none;color: red">Advance Already Adjusted. Cannot Modify data</span></b>
                                            </div>

                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="hdFromBranchID" runat="server" />
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents EndCallback="BranchTransferEndCallBack" />
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>

    </div>
    <div id="DivDataSource">
        <asp:SqlDataSource ID="dsBranch" runat="server" 
            ConflictDetection="CompareAllValues"
            SelectCommand=""></asp:SqlDataSource>
    </div>
    <dxe:ASPxGridView ID="Grid_CustomerReceiptTransfer" runat="server" AutoGenerateColumns="False" KeyFieldName="ReceiptPayment_ID"
        ClientInstanceName="CgvCustomerReceiptTransfer" Width="100%" SettingsBehavior-AllowFocusedRow="true"
        SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true"
        SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true">
        <SettingsSearchPanel Visible="True" />
        <ClientSideEvents />
        <Columns>
            <dxe:GridViewDataCheckColumn VisibleIndex="0" Visible="false">
                <EditFormSettings Visible="True" />
                <EditItemTemplate>
                    <dxe:ASPxCheckBox ID="ASPxCheckBox1" Text="" runat="server"></dxe:ASPxCheckBox>
                </EditItemTemplate>
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataCheckColumn>
            <dxe:GridViewDataTextColumn FieldName="ReceiptPayment_ID" Visible="false">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="Type" FieldName="ReceiptPayment_TransactionType">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="2" Caption="Date" FieldName="ReceiptPayment_TransactionDate">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="3" Caption="Voucher Number" FieldName="ReceiptPayment_VoucherNumber">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Branch" FieldName="branch">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="5" Caption="Transfer To Branch" FieldName="branchToBranch">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <%--  <dxe:GridViewDataTextColumn VisibleIndex="4" Caption="Currency" FieldName="ReceiptPayment_Currency">
                <CellStyle Wrap="False" CssClass="gridcellleft" HorizontalAlign="left"></CellStyle>
            </dxe:GridViewDataTextColumn>--%>
            <dxe:GridViewDataTextColumn VisibleIndex="6" Caption="Customer(s)" FieldName="Customer" Width="20%">
                <CellStyle CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Amount" FieldName="Amount">
                <CellStyle CssClass="gridcellleft" HorizontalAlign="right"></CellStyle>
            </dxe:GridViewDataTextColumn>
          <%--  <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Cash/Bank" FieldName="CashBankID" Width="15%">
                <CellStyle CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn>--%>
            <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="ReceiptPayment_CreateUser"
                Caption="Entered By">
                <CellStyle Wrap="False" HorizontalAlign="left">
                </CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="ReceiptPayment_CreateDateTime"
                Caption="Last Update On">
                <CellStyle Wrap="False" HorizontalAlign="Right">
                </CellStyle>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="ReceiptPayment_ModifyUser"
                Caption="Updated By">
                <CellStyle Wrap="False" HorizontalAlign="left">
                </CellStyle>
            </dxe:GridViewDataTextColumn>

          <%--  <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="12">
                <DataItemTemplate>
                    <%if(rights.CanAssignbranch){ %>
                    <a href="javascript:void(0);" onclick="OnTransferClick('<%# Container.KeyValue %>')" class="pad" title="Transfer">
                        <img src="../../../assests/images/transfer.png" /></a>
                    <%} %>
                </DataItemTemplate>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>--%>

        </Columns>
        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
        <ClientSideEvents EndCallback="function(s, e) {
	                                        ShowMsgLastCall();
                                        }" />
        <SettingsBehavior ConfirmDelete="True" />
        <Styles>
            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
            <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
            <LoadingPanel ImageSpacing="10px"></LoadingPanel>
            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
            <Footer CssClass="gridfooter"></Footer>
        </Styles>
        <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
        </SettingsPager>


    </dxe:ASPxGridView>

     <asp:HiddenField ID="hdnEditID" runat="server" />
    <asp:HiddenField ID="hdBranchDetails" runat="server" />
</asp:Content>
