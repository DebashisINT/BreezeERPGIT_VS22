<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/PopUp.Master"
    CodeBehind="PosMassBranch.aspx.cs" Inherits="ERP.OMS.Management.Activities.PosMassBranch" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <script type="text/javascript">
        //$(document).ready(function () {
        //    cmassBranch.Refresh();
        //   });
        function AssignmentGridEndCallback() {
            if (cAssignmentGrid.cpMsg) {
                if (cAssignmentGrid.cpMsg != '') {
                    jAlert(cAssignmentGrid.cpMsg, 'Alert', function () {
                        cAssignmentPopUp.Hide();
                        if (page.activeTabIndex == 0) {
                            //cGrdQuotation.PerformCallback('RefreshGrid');
                            cGrdQuotation.Refresh();
                        }
                        else {
                            cIstGrid.PerformCallback('RefreshGrid');
                        }
                    });
                    cAssignmentGrid.cpMsg = null;
                }
            }
        }
        function MassBranchAssignSaveClick() {
            //  var jSondata = JSON.stringify(BranchMassListByKeyValue);
            cmassBranch.PerformCallback('Save');
        }
        function gridFocusedRowChanged(s, e) {
            GlobalRowIndex = e.visibleIndex;
        }
        
        var BranchMassListByKeyValue = [];
        function massBranchOnEndCallBack() {
            if (cmassBranch.cpMsg) {
                alert(cmassBranch.cpMsg);
                window.parent.cmassBranchPopup.Hide();
                cmassBranch.Refresh();
                if( window.parent.location.pathname == "/OMS/Management/Activities/PosSalesInvoiceList.aspx")
                {
                    window.parent.cGrdQuotation.Refresh();
                }
                else if( window.parent.location.pathname == "/OMS/Management/Activities/PosInvoiceList.aspx")
                {
                    window.parent.cIstGrid.Refresh();
                }
                
                cmassBranch.cpMsg = null;
            } else {

                var pageNo = cmassBranch.GetPageIndex();
                pageNo = pageNo * 15;
                for (vInd = pageNo; vInd < pageNo + cmassBranch.GetVisibleRowsOnPage() ; vInd++) {
                    var findObj = $.grep(BranchMassListByKeyValue, function (e) { return e.InvoiceId == cmassBranch.GetRowKey(vInd); })
                    if (findObj.length > 0) {
                        cmassBranch.batchEditApi.StartEdit(vInd);
                        cmassBranch.GetEditor('pos_assignBranch').SetValue(findObj[0].BranchId);
                    }
                }
                cmassBranch.batchEditApi.StartEdit(0);
            }
        }
        function MassBranchCustomButtonClick(s, e) {
            if (e.buttonID == 'CancelAssignment') {
                GlobalRowIndex = e.visibleIndex;
                cmassBranch.batchEditApi.StartEdit(GlobalRowIndex);
                cmassBranch.GetEditor('pos_assignBranch').SetValue(0);
                BranchChangeOnMassChange(cmassBranch.GetEditor('pos_assignBranch'));
            }
            else if (e.buttonID == 'ShowStock') {
                debugger;
                cAssignmentGrid.PerformCallback('0~0');
                GlobalRowIndex = e.visibleIndex;
                SelectedInvoiceId = cmassBranch.GetRowKey(GlobalRowIndex);
                $('#BranchAssignmentHeader').hide();
                cAssignmentPopUp.SetHeaderText('Show Stock');
                cAssignmentPopUp.Show();
            }
        }
        function BranchAssignmentBranchSelectedIndexChanged() {
            //cAssignedBranch.SetValue(cBranchAssignmentBranch.GetValue());
            //    AssignedBranchSelectedIndexChanged(cBranchAssignmentBranch);
            //cAssignedWareHouse.PerformCallback(cAssignedBranch.GetValue());
        }
        function updateAssignmentGrid() {
            cAssignmentGrid.PerformCallback(SelectedInvoiceId + '~' + cBranchAssignmentBranch.GetValue());
        }
        

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
        <tr>
            <td width="150px">
                <span>
                    <dxe:ASPxComboBox ID="MassBranchId" runat="server" ClientInstanceName="cMassBranchId" Width="100%">
                    </dxe:ASPxComboBox>
                </span>

            </td>
            <td style="padding-left: 20px; padding-bottom: 10px;"><%-- <input type="button" value="Assign" class="btn btn-primary" onclick="updateMassBranchAssign()" />--%>
                <a href="javascript:void(0);" onclick="MassBranchAssignSaveClick()" style="margin-top: 10px;" class="btn btn-success"><span>A<u>s</u>sign & Exit</span> </a>
            </td>
            <td>


                <% if (rights.CanExport)
                   { %>
                <asp:DropDownList ID="DropDownList1" runat="server" CssClass="btn btn-sm btn-primary pull-right"
                    OnSelectedIndexChanged="BranchAssigncmbExport_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLS</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>
                <% } %>

            </td>

        </tr>

    </table>
    <%--<dxe:ASPxGridView ID="massBranch" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
        Width="100%" ClientInstanceName="cmassBranch" OnCustomCallback="massBranch_CustomCallback" SelectionMode="Multiple"
        OnDataBinding="massBranch_DataBinding" SettingsBehavior-AllowFocusedRow="true">--%>
    <dxe:ASPxGridView ID="massBranch" runat="server" KeyFieldName="Invoice_Id" AutoGenerateColumns="False"
        Width="100%" ClientInstanceName="cmassBranch" OnCustomCallback="massBranch_CustomCallback" SelectionMode="Multiple"
         SettingsBehavior-AllowFocusedRow="true" DataSourceID="EntityServerModeDataSource">
        <%--   <SettingsEditing Mode="Batch">
                                  <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row" />
               </SettingsEditing>--%>
        <Columns>

            <dxe:GridViewCommandColumn ShowSelectCheckbox="true" Width="50" Caption="Select" FixedStyle="Left" VisibleIndex="0" />

            <dxe:GridViewDataTextColumn Caption="Invoice No." FieldName="InvoiceNo" ReadOnly="True"
                VisibleIndex="0" FixedStyle="Left">
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn Caption="Invoice Date" FieldName="Invoice_Date" ReadOnly="True"
                VisibleIndex="0" FixedStyle="Left">
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn Caption="Customer Name" FieldName="CustomerName" ReadOnly="True"
                VisibleIndex="0" FixedStyle="Left">
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn Caption="Amount" FieldName="NetAmount" ReadOnly="True"
                VisibleIndex="0" FixedStyle="Left">
                <PropertiesTextEdit DisplayFormatString="0.00"></PropertiesTextEdit>
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn Caption="Type" FieldName="Pos_EntryType" ReadOnly="True" Width="50px"
                VisibleIndex="0" FixedStyle="Left">
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn Caption="Delivery Date" FieldName="Pos_DeliveryDate" ReadOnly="True"
                VisibleIndex="0" FixedStyle="Left">
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="EnteredBy" ReadOnly="True"
                VisibleIndex="0" FixedStyle="Left">
                <CellStyle CssClass="gridcellleft" Wrap="true">
                </CellStyle>
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>


            <%-- <dxe:GridViewDataComboBoxColumn Caption="Assign" FieldName="pos_assignBranch"  Width="200px" >
                                  <PropertiesComboBox ValueField="branch_id" TextField="branch_description"  > 
                                     <ClientSideEvents SelectedIndexChanged="BranchChangeOnMassChange"  />
                                  </PropertiesComboBox>
                             </dxe:GridViewDataComboBoxColumn>--%>

            <dxe:GridViewCommandColumn VisibleIndex="7" Caption="Action" Width="80px">
                <CustomButtons>
                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="ShowStock" Image-Url="/assests/images/warehouse.png" Image-ToolTip="Show Stock">
                        <Image ToolTip="Show Stock" Url="/assests/images/warehouse.png">
                        </Image>
                    </dxe:GridViewCommandColumnCustomButton>
                </CustomButtons>

                <%-- <CustomButtons>
                                    <dxe:GridViewCommandColumnCustomButton Text=" " ID="CancelAssignment" Image-Url="/assests/images/warehouse.png" Image-ToolTip="Cancel">
                                        <image tooltip="Cancel" url="/assests/images/crs.png">
                                        </image>
                                    </dxe:GridViewCommandColumnCustomButton>
                                </CustomButtons>--%>
            </dxe:GridViewCommandColumn>

        </Columns>

        <SettingsSearchPanel Visible="True" />
        <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsLoadingPanel Text="Please Wait..." />
        <SettingsPager PageSize="15"></SettingsPager>
        <ClientSideEvents BatchEditStartEditing="gridFocusedRowChanged" EndCallback="massBranchOnEndCallBack" CustomButtonClick="MassBranchCustomButtonClick" />
    </dxe:ASPxGridView>
    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_posList" />
    <dxe:ASPxPopupControl ID="AssignmentPopUp" runat="server" Width="900"
        CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="cAssignmentPopUp" Height="500"
        HeaderText="Branch Assignment" AllowResize="false" ResizingMode="Postponed">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                
                <table class="smllpad" style="margin-top: 15px;">
                    <tr>

                        <td style="width: 110px">Select Branch To View Stock </td>
                        <td>
                            <dxe:ASPxComboBox ID="BranchAssignmentBranch" runat="server" ClientInstanceName="cBranchAssignmentBranch" Width="100%">
                                <ClientSideEvents SelectedIndexChanged="BranchAssignmentBranchSelectedIndexChanged"></ClientSideEvents>
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <a href="#" onclick="updateAssignmentGrid()">
                                <button type="button" class="btn btn-primary "><i class="fa fa-search" style="" aria-hidden="true"></i>View Stock</button></a>
                            <%--   <input type="button" value="Show Stock" class="btn btn-primary" onclick="updateAssignmentGrid()" />--%>
                        </td>

                    </tr>

                </table>


                <dxe:ASPxGridView ID="AssignmentGrid" runat="server" KeyFieldName="InvoiceDetails_Id" AutoGenerateColumns="False"
                    Width="100%" ClientInstanceName="cAssignmentGrid" OnCustomCallback="AssignmentGrid_CustomCallback" KeyboardSupport="true"
                    SettingsBehavior-AllowSelectSingleRowOnly="true" OnDataBinding="AssignmentGrid_DataBinding" SettingsBehavior-AllowFocusedRow="true">
                    <Columns>


                        <dxe:GridViewDataTextColumn Caption="Code" FieldName="sProducts_Code"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Name" FieldName="InvoiceDetails_ProductDescription"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Available Stock" FieldName="availableQty"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Curently Invoiced" FieldName="InvoicedBalance"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                        <dxe:GridViewDataTextColumn Caption="Actual Balance" FieldName="Actual_Balance"
                            VisibleIndex="0" FixedStyle="Left">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>

                    </Columns>
                    <ClientSideEvents EndCallback="AssignmentGridEndCallback" />


                    <SettingsSearchPanel Visible="True" />
                    <Settings ShowGroupPanel="False" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsLoadingPanel Text="Please Wait..." />
                </dxe:ASPxGridView>




            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="massBranch" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
