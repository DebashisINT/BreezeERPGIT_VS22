<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/PopUp.Master" AutoEventWireup="true" CodeBehind="FollowupEntry.aspx.cs" Inherits="ERP.OMS.Management.Followup.FollowupEntry" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
     
    </script>
    <style>
        body {
            background: #fff;
        }

        .ContentPlaceHolder1Class {
            background: #fff !important;
        }

        .gridHeader {
            background: #54749D;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Js/FollowupEntry.js?v=0.10"></script>

    <dxe:ASPxCallbackPanel runat="server" ID="ComponentPanel" ClientInstanceName="cComponentPanel" OnCallback="ComponentPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">

                <div>

                    <div class="col-md-12">
                        <h4>Follow-up by :
                            <asp:Label ID="lblFollowupBy" runat="server" Text="User"></asp:Label></h4>
                    </div>

                    <div>
                        <div class="col-md-2">
                            <label>Date</label>
                            <dxe:ASPxDateEdit ID="dtFollowDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                                ClientInstanceName="cdtFollowDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" ClientEnabled="false">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </div>
                        <div class="col-md-2">
                            <label>Document</label>


                            <dxe:ASPxDropDownEdit ClientInstanceName="checkComboBox" ID="cmbDocumentList" Width="100%" runat="server" AnimationType="None">
                                <DropDownWindowStyle BackColor="#EDEDED" />
                                <DropDownWindowTemplate>
                                    <dxe:ASPxListBox Width="100%" ID="documentList" ClientInstanceName="checkListBox" SelectionMode="CheckColumn"
                                        runat="server" Height="200" EnableSelectAll="true">
                                        <Border BorderStyle="None" />
                                        <BorderBottom BorderStyle="Solid" BorderWidth="1px" BorderColor="#DCDCDC" />
                                        <ClientSideEvents SelectedIndexChanged="updateText" />
                                    </dxe:ASPxListBox>




                                    <table style="width: 100%">
                                        <tr>
                                            <td style="padding: 4px">
                                                <input type="button" onclick="SelectAllDocument()" value="Select All" />
                                            </td>

                                            <td style="padding: 4px">
                                                <dxe:ASPxButton ID="ASPxButton1" AutoPostBack="False" runat="server" Text="Close" Style="float: right">
                                                    <ClientSideEvents Click="function(s, e){ checkComboBox.HideDropDown(); }" />
                                                </dxe:ASPxButton>
                                            </td>

                                        </tr>
                                    </table>
                                </DropDownWindowTemplate>
                                <ClientSideEvents TextChanged="synchronizeListBoxValues" DropDown="synchronizeListBoxValues" />
                            </dxe:ASPxDropDownEdit>
                        </div>
                        <div class="col-md-2">
                            <label>Follow-up using</label>
                            <dxe:ASPxComboBox ID="cmbFollowUp" runat="server" ClientInstanceName="ccmbFollowUp" ValueType="System.String" Width="100%" SelectedIndex="0">
                                <Items>
                                    <dxe:ListEditItem Text="Phone" Value="Phone" />
                                    <dxe:ListEditItem Text="Email" Value="Email" />
                                    <dxe:ListEditItem Text="SMS" Value="SMS" />
                                    <dxe:ListEditItem Text="Visit" Value="Visit" />
                                    <dxe:ListEditItem Text="Social Network" Value="Social Network" />

                                </Items>
                            </dxe:ASPxComboBox>
                        </div>

                        <div class="col-md-2">
                            <label>Open</label>
                            <dxe:ASPxComboBox ID="cmbOpenClose" runat="server" ClientInstanceName="ccmbOpenClose" ValueType="System.String" Width="100%" SelectedIndex="0">
                                <Items>
                                    <dxe:ListEditItem Text="Open" Value="O" />
                                    <dxe:ListEditItem Text="Close" Value="C" />
                                    <dxe:ListEditItem Text="Disputed" Value="D" />
                                    <dxe:ListEditItem Text="Critical" Value="CR" />
                                </Items>
                                <ClientSideEvents SelectedIndexChanged="onStatusChange" />
                            </dxe:ASPxComboBox>
                        </div>
                        <div class="col-md-2">
                            <label>Next Follow-up date</label>
                            <dxe:ASPxDateEdit ID="dtNextFollowupdate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                                ClientInstanceName="cdtNextFollowupdate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-6 lblmTop8">
                            <label>Remarks<span style="color: red; font-weight: 600; padding-left: 10px;">*</span></label>
                            <dxe:ASPxMemo ID="memoRemarks" runat="server" Height="50px" Width="100%" MaxLength="3000" ClientInstanceName="cmemoRemarks"></dxe:ASPxMemo>
                        </div>



                        <div class="col-md-6">
                            <div class="alert alert-success fade in alert-dismissible hide" style="margin-top: 18px;" id="alertdiv">
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close" id="closeAlert">
                                    <span aria-hidden="true" style="font-size: 20px">×</span>
                                </button>
                                <div id="alertId" style="font-weight: 800; color: #b90707;"></div>
                            </div>
                        </div>


                        <div class="clear"></div>

                        <div class="col-md-4" style="padding-top: 10px;">
                            <input type="button" class="btn btn-primary" value="Save" onclick="clickOnSave()" />
                            <input type="button" class="btn btn-danger" value="Clear" onclick="clearAll()" />
                        </div>






                        <asp:HiddenField ID="hdFollowId" runat="server" />
                        <asp:HiddenField ID="hdAction" runat="server" />
                        <asp:HiddenField ID="hdlastStatus" runat="server" />
                        <asp:HiddenField ID="hdStatusNewCust" runat="server" />
                    </div>
                </div>


            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="PanelEndCallback" />
    </dxe:ASPxCallbackPanel>


    <asp:HiddenField ID="hdNoDays" runat="server" />

    <dxe:ASPxGridView ID="GridDetail" runat="server" ClientInstanceName="cGridDetail" KeyFieldName="id"
        Width="100%" Settings-HorizontalScrollBarMode="Auto"
        SettingsBehavior-ColumnResizeMode="Control" DataSourceID="EntityServerModeDataSource"
        Settings-VerticalScrollableHeight="105" SettingsBehavior-AllowSelectByRowClick="true"
        Settings-VerticalScrollBarMode="Auto"
        Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">

        <Columns>


            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" Width="75" FixedStyle="Left">
                <DataItemTemplate>
                    <% if (rights.CanEdit)
                       { %>
                    <a href="javascript:void(0);" onclick="OnView('<%# Container.KeyValue %>')" id="a_editInvoice" class="pad" title="Edit">
                        <img src="../../../assests/images/Edit.png" /></a>
                    <% } %>
                    <% if (rights.CanDelete)
                       { %>
                    <a href="javascript:void(0);" onclick="onDelete('<%# Container.KeyValue %>')" class="pad" title="Delete">
                        <img src="../../../assests/images/Delete.png" /></a>
                    <% } %>
                </DataItemTemplate>
                <HeaderStyle HorizontalAlign="Center" CssClass="gridHeader"></HeaderStyle>
                <CellStyle HorizontalAlign="Center"></CellStyle>
                <HeaderTemplate><span>Actions</span></HeaderTemplate>
                <EditFormSettings Visible="False"></EditFormSettings>
                <Settings AllowAutoFilterTextInputTimer="False" />

            </dxe:GridViewDataTextColumn>



            <dxe:GridViewDataDateColumn HeaderStyle-CssClass="gridHeader" Caption="Follow-up Date" FieldName="FollowDate" Width="150"
                PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy" FixedStyle="Left">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Equals" />
            </dxe:GridViewDataDateColumn>


            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Documents" FieldName="Documents" Width="300">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Using" FieldName="FollowUsing" Width="100">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Status" FieldName="openClsoe" Width="100">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataDateColumn HeaderStyle-CssClass="gridHeader" Caption="Next Date" FieldName="NextFollowDate" Width="100"
                PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Equals" />
            </dxe:GridViewDataDateColumn>

            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Remarks" FieldName="Remarks" Width="500">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>


            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Follow-up By" FieldName="followedByname" Width="100">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataDateColumn HeaderStyle-CssClass="gridHeader" Caption="Follow-up On" FieldName="FollowedOn" Width="100" SortIndex="1" SortOrder="Descending"
                PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Equals" />
            </dxe:GridViewDataDateColumn>

            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Modify By" FieldName="ModifyByName" Width="100">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataDateColumn HeaderStyle-CssClass="gridHeader" Caption="Modify On" FieldName="ModiFyedOn" Width="150"
                PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy hh:mm:ss" PropertiesDateEdit-EditFormatString="dd-MM-yyyy hh:mm:ss">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Equals" />
            </dxe:GridViewDataDateColumn>





        </Columns>
    </dxe:ASPxGridView>

    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
        ContextTypeName="ERPDataClassesDataContext" TableName="tbl_trans_FollowupHeaders" DefaultSorting="id" />



    <h4>Document Details
    
    <asp:DropDownList ID="drdExport" runat="server" Style="background: #54749d; color: white;" AutoPostBack="true" OnSelectedIndexChanged="drdExport_SelectedIndexChanged">
        <asp:ListItem Value="0">Export to</asp:ListItem>
        <asp:ListItem Value="1">PDF</asp:ListItem>
        <asp:ListItem Value="2">XLSX</asp:ListItem>
        <asp:ListItem Value="3">RTF</asp:ListItem>
        <asp:ListItem Value="4">CSV</asp:ListItem>
    </asp:DropDownList>
    </h4>



    <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridDocZoom" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>

    <dxe:ASPxGridView ID="gridDocZoom" runat="server" ClientInstanceName="cgridDocZoom" KeyFieldName="Slno"
        Width="100%" Settings-HorizontalScrollBarMode="Auto"
        SettingsBehavior-ColumnResizeMode="Control" DataSourceID="LinqServerModeDataSource1"
        Settings-VerticalScrollableHeight="105" SettingsBehavior-AllowSelectByRowClick="true"
        Settings-VerticalScrollBarMode="Auto"
        Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">

        <Columns>

            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Unit" Width="10%" FieldName="branch_description">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Document No." Width="25%" FieldName="DocNo">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataDateColumn HeaderStyle-CssClass="gridHeader" Caption="Document Date" Width="10%" FieldName="DocumentDate"
                PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Equals" />
            </dxe:GridViewDataDateColumn>

            <dxe:GridViewDataDateColumn HeaderStyle-CssClass="gridHeader" Caption="Due Date" Width="10%" FieldName="DueDate"
                PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Equals" />
            </dxe:GridViewDataDateColumn>


            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Saleman" Width="15%" FieldName="SalesMan">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Contains" />
            </dxe:GridViewDataTextColumn>


            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Total Amount" Width="10%" FieldName="Invoice_TotalAmount"
                PropertiesTextEdit-DisplayFormatString="0.00">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Equals" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Adjusted Amount" Width="10%" FieldName="adjustedAmount"
                PropertiesTextEdit-DisplayFormatString="0.00">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Equals" />
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Outstanding Amount " Width="10%" FieldName="UnPaidAmount"
                PropertiesTextEdit-DisplayFormatString="0.00">
                <Settings AllowAutoFilterTextInputTimer="False" />
                <Settings AutoFilterCondition="Equals" />
            </dxe:GridViewDataTextColumn>



        </Columns>
    </dxe:ASPxGridView>




    <dx:LinqServerModeDataSource ID="LinqServerModeDataSource1" runat="server" OnSelecting="LinqServerModeDataSource1_Selecting"
        ContextTypeName="ERPDataClassesDataContext" TableName="tbl_EmpAttendanceRecord_reports" />




</asp:Content>
