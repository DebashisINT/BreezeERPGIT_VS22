<%@ Page Language="C#" Title="Sales Return Transfer" MasterPageFile="~/OMS/MasterPage/ERP.Master"  AutoEventWireup="true"  CodeBehind="SalesReturnBranchAssignment.aspx.cs" Inherits="ERP.OMS.Management.Activities.SalesReturnBranchAssignment" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript">
          var globalReturnId = 0;
          function OnTransferClick(obj) {
              cPopup_BranchTransfer.Show();
              globalReturnId = obj;
              cBranchTransferCallBackPanel.PerformCallback('PopulateAssignBranch~'+obj);
          }

          function BranchTransferEndCallBack() {
              if (cBranchTransferCallBackPanel.cpSave) {
                  if (cBranchTransferCallBackPanel.cpSave == "yes") {
                      jAlert("Updated Successfully.", "Alert", function () {
                          cPopup_BranchTransfer.Hide();
                      })

                      cBranchTransferCallBackPanel.cpSave = null;
                  }
              }
          }


          function SaveButtonClick() {

              cBranchTransferCallBackPanel.PerformCallback('SaveData~' + globalReturnId);
          }
      </script>
</asp:Content>

 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title" id="td_contact1" runat="server">
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Sales Return Transfer"></asp:Label>
            </h3>
        </div>

    </div> 
    <dxe:ASPxGridView ID="grid_salesReturnAssignment" runat="server" AutoGenerateColumns="False" KeyFieldName="Return_Id"
        ClientInstanceName="cgrid_salesReturnAssignment" Width="100%" SettingsBehavior-AllowFocusedRow="true"
        OnDataBinding="grid_salesReturnAssignment_OnDataBinding"
        SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true"
        SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true">
        <SettingsSearchPanel Visible="True" />
        <ClientSideEvents />
        <Columns>
     
            <dxe:GridViewDataTextColumn FieldName="Return_Number"  Caption="Return Number">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn> 

            <dxe:GridViewDataTextColumn FieldName="Return_Date"  Caption="Return Date">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn> 

            <dxe:GridViewDataTextColumn FieldName="customerName"  Caption="Customer">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn> 

            <dxe:GridViewDataTextColumn FieldName="amount"  Caption="Amount">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn> 

            <dxe:GridViewDataTextColumn FieldName="UserName"  Caption="Created By">
                <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
            </dxe:GridViewDataTextColumn> 

            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="12">
                <DataItemTemplate>
                    <a href="javascript:void(0);" onclick="OnTransferClick('<%# Container.KeyValue %>')" class="pad" title="Transfer">
                        <img src="../../../assests/images/transfer.png" /></a>
                </DataItemTemplate>
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>

        </Columns>
        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
        
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
                                        <div class="col-md-6">
                                            <label>Transfer To<span style="color: red">*</span></label>
                                            <div>
                                                 
                                                 <dxe:ASPxComboBox ID="ddlBranch" runat="server" ClientInstanceName="cddlBranch" Width="100%">
                                                  </dxe:ASPxComboBox>
                                                <span id="MandatoryBranch" class="iconBranch pullleftClass fa fa-exclamation-circle iconRed " style="color: red; position: absolute; display: none"
                                                    title="Mandatory"></span>
                                            </div>
                                        </div>
                                         
                                        <div class="clear"></div>
                                        <div class="col-md-12 lblmTop8">
                                            <label>Rmarks </label>
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
                                       </dxe:PanelContent>
                                    </PanelCollection>
                            <ClientSideEvents EndCallback="BranchTransferEndCallBack" />
                    </dxe:ASPxCallbackPanel>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
       </div>


</asp:Content>

