<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_TelePerformance_Target" CodeBehind="frmReport_TelePerformance_Target.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>

        <script type="text/javascript">
           
            function height() {
                if (document.body.scrollHeight >= 500)
                    window.frameElement.height = document.body.scrollHeight;
                else
                    window.frameElement.height = '500px';
                window.frameElement.Width = document.body.scrollWidth;
            }
            function client_OnTreeNodeChecked() {
                var obj = window.event.srcElement;
                var treeNodeFound = false;
                var checkedState;
                if (obj.tagName == "INPUT" && obj.type == "checkbox") {
                    var treeNode = obj;
                    checkedState = treeNode.checked;
                    do {
                        obj = obj.parentElement;
                    } while (obj.tagName != "TABLE")
                    var parentTreeLevel = obj.rows[0].cells.length;
                    var parentTreeNode = obj.rows[0].cells[0];
                    var tables = obj.parentElement.getElementsByTagName("TABLE");
                    var numTables = tables.length
                    if (numTables >= 1) {
                        for (i = 0; i < numTables; i++) {
                            if (tables[i] == obj) {
                                treeNodeFound = true;
                                i++;
                                if (i == numTables) {
                                    return;
                                }
                            }
                            if (treeNodeFound == true) {
                                var childTreeLevel = tables[i].rows[0].cells.length;
                                if (childTreeLevel > parentTreeLevel) {
                                    var cell = tables[i].rows[0].cells[childTreeLevel - 1];
                                    var inputs = cell.getElementsByTagName("INPUT");
                                    inputs[0].checked = checkedState;
                                }
                                else {
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            function All_CheckedChanged() {
                document.getElementById("TrTreeExport").style.display = 'inline';
                document.getElementById("TdTree").style.display = 'none';
                document.getElementById("TdGrid").style.display = 'none';
                document.getElementById("TdExport").style.display = 'none';
                document.getElementById("TdButtonShow").style.display = 'inline';
            }
            function Specific_CheckedChanged() {
                document.getElementById("TrTreeExport").style.display = 'inline';
                document.getElementById("TdTree").style.display = 'inline';
                document.getElementById("TdGrid").style.display = 'none';
                document.getElementById("TdExport").style.display = 'none';
                document.getElementById("TdButtonShow").style.display = 'none';
            }
            function page_load() {
                document.getElementById("TrTreeExport").style.display = 'none';
                document.getElementById("TdGrid").style.display = 'none';
                document.getElementById("TdExport").style.display = 'none';
            }
            FieldName = 'ctl00$ContentPlaceHolder3$TxtFromDate';
        </script>

        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="4" style="text-align: center;">
                    <strong><span style="color: #000099">Target Vs Achivement</span></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PnlDate" runat="server" BorderWidth="1px" BorderColor="white" Width="100%">
                                                <table width="70%">
                                                    <tr>
                                                        <td class="gridcellleft">
                                                            <dxe:ASPxDateEdit ID="TxtFromDate" runat="server" UseMaskBehavior="True" EditFormat="Custom">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                                <DropDownButton Text="From Date">
                                                                </DropDownButton>
                                                            </dxe:ASPxDateEdit>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtFromDate"
                                                                Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="gridcellleft">
                                                            <dxe:ASPxDateEdit ID="TxtToDate" runat="server" UseMaskBehavior="True" EditFormat="Custom">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                                <DropDownButton Text="To Date">
                                                                </DropDownButton>
                                                            </dxe:ASPxDateEdit>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtToDate"
                                                                Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td class="gridcellright" style="width: 190px;">
                                                            <span style="color: #000099">Select User:</span></td>
                                                        <td class="gridcellleft" style="width: 280px;">
                                                            <asp:RadioButton ID="All" runat="server" GroupName="a" Width="21px" Checked="True" /><asp:Label
                                                                ID="lblAll" runat="server" Text="All"></asp:Label>
                                                            <asp:RadioButton ID="Specific" runat="server" GroupName="a" Width="18px" /><asp:Label
                                                                ID="lblSelected" runat="server" Text="Specific"></asp:Label>
                                                        </td>
                                                        <td class="gridcellright" style="width: 150px;">
                                                            <span style="color: #000099">Report Type:</span></td>
                                                        <td class="gridcellleft" style="">
                                                            <dxe:ASPxRadioButtonList ID="RBReportType" runat="server" CssPostfix="BlackGlass"
                                                                RepeatDirection="Horizontal" Height="2px" SelectedIndex="0" TextSpacing="3px">
                                                                <Items>
                                                                    <dxe:ListEditItem Text="Screen" Value="Screen" />
                                                                    <dxe:ListEditItem Text="Print" Value="Print" />
                                                                </Items>
                                                                <ValidationSettings ErrorText="Error has occurred">
                                                                </ValidationSettings>
                                                            </dxe:ASPxRadioButtonList>
                                                        </td>
                                                        <td class="gridcellleft" id="TdButtonShow">
                                                            <dxe:ASPxButton ID="btnShowReport" runat="server" Text="Show" Height="5px" Width="82px"
                                                                OnClick="btnShowReport_Click" ValidationGroup="a">
                                                                <Border BorderColor="White" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; padding: 0px">
                    <table>
                        <tr>
                            <td style="text-align: right; vertical-align: bottom;" id="TdExport">
                                <table id="Table1" runat="server" style="">
                                    <tr>
                                        <td>
                                            <table>
                                                <tr id="TrExpndCollapse">
                                                    <td style="text-align: right;">
                                                        <dxe:ASPxButton ID="btnExpandAll" runat="server" Text="Expand All" Height="20px"
                                                            AutoPostBack="false" Font-Size="12px">
                                                            <ClientSideEvents Click="function(s, e) {
                                                                                    List.ExpandAll();
                                                                                    }" />
                                                        </dxe:ASPxButton>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <dxe:ASPxButton ID="btnCollapsAll" runat="server" Text="Collaps All" Height="11px"
                                                            AutoPostBack="false" Font-Size="12px">
                                                            <ClientSideEvents Click="function(s, e) {
                                                                                     List.CollapseAll();
                                                                                     }" />
                                                        </dxe:ASPxButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="vertical-align: middle">
                                            <dxe:ASPxComboBox ID="cmbExport" runat="server" ValueType="System.String" Height="17px"
                                                OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" SelectedIndex="0" AutoPostBack="true"
                                                Font-Overline="False" Font-Size="12px" Width="91px">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <Items>
                                                    <dxe:ListEditItem Text="Select" Value="" />
                                                    <dxe:ListEditItem Text="Pdf" Value="Pdf" />
                                                    <dxe:ListEditItem Text="Xls" Value="Xls" />
                                                    <dxe:ListEditItem Text="Rtf" Value="Rtf" />
                                                </Items>
                                                <DropDownButton Text="Export" ToolTip="Export File">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="vertical-align: top">
                                <asp:Panel ID="PnlGrid" runat="server" BorderColor="blue" BorderWidth="0px" Width="100%">
                                    <table width="100%">
                                        <tr id="TrTreeExport">
                                            <td style="vertical-align: top; text-align: left; width: 39%" id="TdTree">

                                                <table>
                                                    <tr>
                                                        <td>
                                                            <strong><span style="color: blue">Select User : </span></strong>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlTree" runat="server" BorderColor="white" BorderWidth="1px" Height="275px" ScrollBars="Vertical">
                                                                <asp:TreeView ID="TreeAccessebility" runat="server" onclick="client_OnTreeNodeChecked();"
                                                                    NodeIndent="3" ExpandDepth="1" EnableTheming="True" ShowLines="True">
                                                                </asp:TreeView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxButton ID="BtnShowSelectReport" runat="server" Text="Show" Height="5px"
                                                                Width="82px" OnClick="btnShowReport_Click" ValidationGroup="a">
                                                                <Border BorderColor="White" />
                                                            </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" id="TdGrid">
                                                <dxeTreeList:ASPxTreeList ID="GridActivitytarget" runat="server" ClientInstanceName="List"
                                                    Width="100%" KeyFieldName="ID" ParentFieldName="ParentID">
                                                    <columns>
                                                                <dxeTreeList:TreeListTextColumn Caption="Name" FieldName="Assign To" VisibleIndex="0">
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxeTreeList:TreeListTextColumn>
                                                                <dxeTreeList:TreeListTextColumn Caption="Total Calls" FieldName="Total Calls" VisibleIndex="1">
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxeTreeList:TreeListTextColumn>
                                                                <dxeTreeList:TreeListTextColumn Caption="Assigned Calls" FieldName="Assigned Calls" VisibleIndex="2">
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                </dxeTreeList:TreeListTextColumn>
                                                                <dxeTreeList:TreeListTextColumn Caption="Activity Calls" FieldName="Activity Calls" VisibleIndex="4">
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                    <HeaderStyle Wrap="True" />
                                                                </dxeTreeList:TreeListTextColumn>
                                                                <dxeTreeList:TreeListTextColumn Caption="Performance Vs Target(%)" FieldName="Performance Vs Target(%)"
                                                                    VisibleIndex="5">
                                                                    <CellStyle CssClass="gridcellleft">
                                                                    </CellStyle>
                                                                    <HeaderStyle Wrap="True" />
                                                                </dxeTreeList:TreeListTextColumn>
                                                            </columns>
                                                    <styles>
                                                            </styles>
                                                    <settings suppressoutergridlines="True" gridlines="Both" />
                                                    <images>
                                                                <ExpandedButton Height="11px" Width="11px" />
                                                                <CustomizationWindowClose Height="12px" Width="13px" />
                                                                <CollapsedButton Height="11px" Width="11px" />
                                                            </images>
                                                </dxeTreeList:ASPxTreeList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <dxeTreeList:ASPxTreeListExporter ID="ASPxTreeListExporter1" runat="server">
                                </dxeTreeList:ASPxTreeListExporter>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
