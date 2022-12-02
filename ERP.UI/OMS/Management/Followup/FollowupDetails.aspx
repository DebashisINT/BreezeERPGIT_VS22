<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FollowupDetails.aspx.cs" Inherits="ERP.OMS.Management.Followup.FollowupDetails" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style>
         .gridHeader {
        background: #54749D;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>

             <asp:DropDownList ID="drdExport" runat="server" style="background: #54749d;color: white;" OnSelectedIndexChanged="drdExport_SelectedIndexChanged" AutoPostBack="true" >
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLSX</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
            
    <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GridDetail" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
    </dxe:ASPxGridViewExporter>
            <dxe:ASPxGridView ID="GridDetail" runat="server" ClientInstanceName="cGridDetail" KeyFieldName="Slno"
                Width="100%" Settings-HorizontalScrollBarMode="Auto"
                SettingsBehavior-ColumnResizeMode="Control" DataSourceID="EntityServerModeDataSource"
                Settings-VerticalScrollableHeight="275" SettingsBehavior-AllowSelectByRowClick="true"
                Settings-VerticalScrollBarMode="Auto"
                Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">

                <Columns>

                    <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Unit" Width="10%" FieldName="branch_description">
                         <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>
                    
                    <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Document No." Width="30%" FieldName="DocNo">
                         <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                    </dxe:GridViewDataTextColumn>

                    <dxe:GridViewDataDateColumn HeaderStyle-CssClass="gridHeader" Caption="Document Date" Width="10%" FieldName="DocDate"
                        PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                         <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Equals" />
                    </dxe:GridViewDataDateColumn>

                    <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Saleman" Width="20%" FieldName="SalesMan">
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


        </div>


        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="tbl_EmpAttendanceRecord_reports" />

    </form>
</body>
</html>
