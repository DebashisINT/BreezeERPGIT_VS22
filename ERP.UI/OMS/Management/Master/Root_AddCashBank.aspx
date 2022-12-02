<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Root_AddCashBank.aspx.cs" Inherits="ERP.OMS.Management.Master.Root_AddCashBank" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        //var Userid = "";
        //$(document).ready(function () {
        //    var urlKeys = getUrlVars();
        //    Userid = urlKeys.id;
        //});

        function CloseCashBankLookup() {
            cCashBankLookUp.ConfirmCurrentSelection();
            cCashBankLookUp.HideDropDown();
            cCashBankLookUp.Focus();
        }
        function DeleteRow(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cCashBankMapping.PerformCallback(keyValue);
                }
            });
        }
        function setvalue() {
            document.getElementById("txtContact_hidden").value = document.getElementById("lstContact").value;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">
                <asp:Label ID="lblHeadTitle" Text="Add Cash/Bank Rights" runat="server"></asp:Label>
            </h3>
            <div id="CashBankCross" runat="server">
                <div id="divcross" class="crossBtn" style="margin-left: 100px;"><a href="root_user.aspx"><i class="fa fa-times"></i></a></div>
            </div>
        </div>
    </div>
    <div class="form_main  clearfix">
        <div class="row">
            <div class="col-md-6" style="margin-top: 7px; margin-bottom: 5px;" id="CashBankSection" runat="server">
                <div style="height: auto; margin-bottom: 5px;">
                  Select Cash/Bank :
                </div>
                <div class="Left_Content">
                    <dxe:ASPxCallbackPanel runat="server" ID="ComponentPanel" ClientInstanceName="cComponentPanel">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">
                                <dxe:ASPxGridLookup ID="CashBankLookUp" runat="server" SelectionMode="Single" DataSourceID="CashBankDataSource" ClientInstanceName="cCashBankLookUp"
                                    KeyFieldName="MainAccount_ReferenceID" Width="100%" TextFormatString="{0}">
                                    <Columns>
                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="0" Caption=" " />
                                        <dxe:GridViewDataColumn FieldName="MainAccountName" Caption="Cash/Bank Name" Width="220">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dxe:GridViewDataColumn>
                                    </Columns>
                                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                        <Templates>
                                            <StatusBar>
                                                <table class="OptionsTable" style="float: right">
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseCashBankLookup" UseSubmitBehavior="False" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </StatusBar>
                                        </Templates>
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                                    </GridViewProperties>
                                </dxe:ASPxGridLookup>
                                <asp:HiddenField ID="hfHSN_CODE" runat="server" />
                            </dxe:PanelContent>
                        </PanelCollection>
                        <ClientSideEvents />
                    </dxe:ASPxCallbackPanel>

                </div>
                <div id="divSubmitButton" runat="server">
                    <table style="width: 100%;">
                        <tr>
                            <td style="padding: 5px 0;">
                                <span>
                                    <asp:Button ID="BtnSave" runat="server" Text="Allow Cash/Bank" CssClass="btnUpdate btn btn-primary" OnClick="BtnSave_Click" OnClientClick="setvalue()" />
                                    <%--<asp:Button ID="BtnCancel" runat="server" CssClass="btnUpdate btn btn-danger" Text="Cancel" OnClientClick="Cancel_Click()" />--%>
                                </span>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:SqlDataSource runat="server" ID="CashBankDataSource" 
                    SelectCommand="prc_UserDetail" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:Parameter Type="String" Name="action" DefaultValue="GetUserCashBankIDS" />
                         <asp:QueryStringParameter Name="userid" DbType ="Int32" Direction = "Input" QueryStringField="id" DefaultValue="" ConvertEmptyStringToNull="True" />
                    </SelectParameters>
                </asp:SqlDataSource>


            </div>
        </div>
        <div>
            <dxe:ASPxGridView ID="CashBankMapping" runat="server" KeyFieldName="CashBankMapID" AutoGenerateColumns="False" OnDataBinding="CashBankMapping_DataBinding"
                ClientInstanceName="cCashBankMapping" Width="100%" OnCustomCallback="CashBankMapping_CustomCallback">
                <Columns>
                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="0"
                        Width="0">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="MainAccountName" ReadOnly="True" Caption="Cash/Bank Name" VisibleIndex="1">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="user_name" ReadOnly="True" Caption="User Name" VisibleIndex="2">
                        <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="3" Width="60px" Caption="Details" CellStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                        <DataItemTemplate>
                            <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')">

                                <img src="/assests/images/Delete.png" />
                            </a>
                        </DataItemTemplate>
                        <CellStyle Wrap="False">
                        </CellStyle>
                        <HeaderTemplate>
                            Delete                               
                        </HeaderTemplate>
                        <EditFormSettings Visible="False"></EditFormSettings>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="UserID" ReadOnly="True" Caption="UserID" VisibleIndex="4" Width="0">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn FieldName="EntityID" ReadOnly="True" Caption="EntityID" VisibleIndex="5" Width="0">
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <Styles>
                    <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>
                    <FocusedRow HorizontalAlign="Left" VerticalAlign="Top" CssClass="gridselectrow"></FocusedRow>
                    <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                    <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                    <Footer CssClass="gridfooter"></Footer>
                </Styles>
                <SettingsText ConfirmDelete="Are You Sure To Delete This Record ???" />
                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                <Settings ShowGroupPanel="True"  ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" ShowFooter="true"
                     />
                <SettingsPager NumericButtonCount="10" PageSize="10" ShowSeparators="True" Mode="ShowPager">
                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                </SettingsPager>
            </dxe:ASPxGridView>
        </div>
        <div id="hiddenField">
             <asp:HiddenField runat="server" ID="hdnUserID" />
        </div>
    </div>
</asp:Content>
