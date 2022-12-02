<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_rptdailysales" CodeBehind="rptdailysales.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


   
    <script language="javascript" type="text/javascript">

       
        //     function ShowGridFilter()
        //    {
        //        var date1 = TxtStartDate.GetDate();
        //             
        //        if (date1== null )
        //                {
        //            alert(" Please select Date.");
        //            return false;
        //                }
        //        else
        //               {
        //              
        //           GridDailyActivity.PerformCallback();
        //               }
        //  } 
        //  function Callback()
        //        {
        //         alert("this")
        //            GridDailyActivity.PerformCallback();
        //        }


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
        FieldName = 'ctl00$ContentPlaceHolder3$TxtStartDate';
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <div class="panel-heading">
        <div class="panel-title">
            <h3>Daily Activity</h3>
        </div>

    </div>
     <div class="crossBtn"><a href="../Management/ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main" style="">

        <asp:Panel ID="PnlDate" runat="server" Style="width: 100%">
            <table style="width: 100%">
                
                    <tr>
                        <td style="width:150px">
                            <dxe:ASPxDateEdit ID="TxtStartDate" runat="server" UseMaskBehavior="True" EditFormat="Custom">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                                <DropDownButton Text="From Date">
                                </DropDownButton>
                            </dxe:ASPxDateEdit>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtStartDate"
                                Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 98px;">
                            <span style="padding-left:15px">Select User:</span></td>
                        <td  style="text-align: left; width: 124px;">
                            <asp:RadioButton ID="All" runat="server" GroupName="a" Width="21px" Checked="True" /><asp:Label
                                ID="lblAll" runat="server" Text="All"></asp:Label>
                            <asp:RadioButton ID="Specific" runat="server" GroupName="a" Width="18px" /><asp:Label
                                ID="lblSelected" runat="server" Text="Specific"></asp:Label>
                        </td>
                        <td  style="width:94px;">
                            <span style="color: #000099">Report Type:</span></td>
                        <td style="width:134px">
                            <dxe:ASPxRadioButtonList ID="RBReportType" runat="server" RepeatDirection="Horizontal"
                                Height="2px" SelectedIndex="0" TextSpacing="3px" CssPostfix="BlackGlass">
                                <Items>
                                    <dxe:ListEditItem Text="Screen" Value="Screen" />
                                    <dxe:ListEditItem Text="Print" Value="Print" />
                                </Items>
                                <ValidationSettings ErrorText="Error has occurred">
                                    <ErrorImage Height="14px" Width="14px" />
                                </ValidationSettings>
                            </dxe:ASPxRadioButtonList>
                        </td>
                        <td class="gridcellleft" id="TdButtonShow" style="padding-left:10px">
                            <dxe:ASPxButton ID="btnShowReport" runat="server" Text="Show" 
                                OnClick="btnShowReport_Click" CssClass="btn btn-success tn-xs">
                                <Border BorderColor="White" />
                                <%-- <ClientSideEvents Click="function(s,e) {ShowGridFilter();}" />--%>
                            </dxe:ASPxButton>
                        </td>
                        <%--<td style="text-align: right; vertical-align: bottom;" id="TdExport">
                            <table id="Table1" runat="server">
                                <tr style="vertical-align: bottom;">
                                    <td style="vertical-align: bottom">
                                        <table>
                                            <tr id="TrExpndCollapse">
                                                <td style="text-align: right;">
                                                    <dxe:ASPxButton ID="btnExpandAll" runat="server" Text="Expand All" 
                                                        AutoPostBack="false"  CssClass="btn btn-warning">
                                                        <ClientSideEvents Click="function(s, e) {
                                        List.ExpandAll();
                                        }" />
                                                    </dxe:ASPxButton>
                                                </td>
                                                <td style="text-align: center;">
                                                    <dxe:ASPxButton ID="btnCollapsAll" runat="server" Text="Collaps All" 
                                                        AutoPostBack="false"  CssClass="btn btn-warning">
                                                        <ClientSideEvents Click="function(s, e) {
                                            List.CollapseAll();
                                            }" />
                                                    </dxe:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    
                                </tr>
                            </table>
                        </td>--%>
                        <%--<td>
                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary pull-right" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                            </asp:DropDownList>
                        </td>--%>
                    </tr>
                       
            </table>
        </asp:Panel>
                                     

        <table class="TableMain100">
            <%--<tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Daily Activity</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td>
                    
                </td>
            </tr>
            <tr style="vertical-align: bottom;">
                <td style="text-align: left; padding: 0px"></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <table width="100%">
                        <tr>
                            <td style="vertical-align: top">
                                <asp:Panel ID="PnlGrid" runat="server" BorderColor="blue" BorderWidth="0px" Width="100%">
                                    <table width="100%">
                                        <tr id="TrTreeExport">
                                            <td style="vertical-align: top; text-align: left;" id="TdTree" colspan="2">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <strong><span >Select User : </span></strong>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="pnlTree" runat="server" BorderColor="white" BorderWidth="0px" >
                                                                <asp:TreeView ID="TreeAccessebility" runat="server" onclick="client_OnTreeNodeChecked();"
                                                                    NodeIndent="3" ExpandDepth="1" EnableTheming="True" ShowLines="True">
                                                                </asp:TreeView>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxButton ID="BtnShowSelectReport" runat="server" Text="Show"  CssClass="btn btn-success"
                                                                OnClick="btnShowReport_Click">
                                                            </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" id="TdGrid">
                                                <dxeTreeList:aspxtreelist id="GridDailyActivity" runat="server" clientinstancename="List"
                                                    width="100%" keyfieldname="ID" parentfieldname="ParentID">
                                                    <columns>
                                                            <dxeTreeList:TreeListTextColumn Caption="Assign To" FieldName="Assign To" VisibleIndex="0">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                            </dxeTreeList:TreeListTextColumn>
                                                            <dxeTreeList:TreeListTextColumn Caption="Assign By" FieldName="Assign By" VisibleIndex="1">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                            </dxeTreeList:TreeListTextColumn>
                                                            <dxeTreeList:TreeListTextColumn Caption="Fresh Visits" FieldName="Fresh Visits" VisibleIndex="2">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                            </dxeTreeList:TreeListTextColumn>
                                                            <dxeTreeList:TreeListTextColumn Caption="Follow Ups/Old Visits" FieldName="Follow Ups/Old Visits"
                                                                VisibleIndex="3">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <HeaderStyle Wrap="True" />
                                                            </dxeTreeList:TreeListTextColumn>
                                                            <dxeTreeList:TreeListTextColumn Caption="Total Vists" FieldName="Total Vists" VisibleIndex="4">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                            </dxeTreeList:TreeListTextColumn>
                                                            <dxeTreeList:TreeListTextColumn Caption="Pending Visits" FieldName="Pending Visits" VisibleIndex="5"
                                                                Visible="false">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                            </dxeTreeList:TreeListTextColumn>
                                                            <dxeTreeList:TreeListTextColumn Caption="Followup/Rescheduled" FieldName="Followup/Rescheduled"
                                                                VisibleIndex="2">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <HeaderStyle Wrap="True" />
                                                            </dxeTreeList:TreeListTextColumn>
                                                            <dxeTreeList:TreeListTextColumn Caption="Confirm Sales" FieldName="Confirm Sales" VisibleIndex="3">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                            </dxeTreeList:TreeListTextColumn>
                                                            <dxeTreeList:TreeListTextColumn Caption="Not Interested/Lost" FieldName="Not Interested/Lost"
                                                                VisibleIndex="4">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <HeaderStyle Wrap="True" />
                                                            </dxeTreeList:TreeListTextColumn>
                                                            <dxeTreeList:TreeListTextColumn Caption="Not Contactable" FieldName="Not Contactable" VisibleIndex="5"
                                                                Visible="false">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                                <HeaderStyle Wrap="True" />
                                                            </dxeTreeList:TreeListTextColumn>
                                                            <dxeTreeList:TreeListTextColumn Caption="Not Useable" FieldName="Not Useable" VisibleIndex="2">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                            </dxeTreeList:TreeListTextColumn>
                                                            <dxeTreeList:TreeListTextColumn Caption="Effectiveness" FieldName="Effectiveness" VisibleIndex="3">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                            </dxeTreeList:TreeListTextColumn>
                                                            <dxeTreeList:TreeListTextColumn Caption="assignedtoid" FieldName="assignedtoid" VisibleIndex="4">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                            </dxeTreeList:TreeListTextColumn>
                                                            <dxeTreeList:TreeListTextColumn Caption="assignedByid" FieldName="assignedByid" VisibleIndex="5"
                                                                Visible="false">
                                                                <CellStyle CssClass="gridcellleft">
                                                                </CellStyle>
                                                            </dxeTreeList:TreeListTextColumn>
                                                        </columns>
                                                    <styles>
                                                        </styles>
                                                    <settings suppressoutergridlines="True" />
                                                    <images>
                                                            <ExpandedButton Height="11px" Width="11px" />
                                                            <CustomizationWindowClose Height="12px" Width="13px" />
                                                            <CollapsedButton Height="11px" Width="11px" />
                                                        </images>
                                                </dxeTreeList:aspxtreelist>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <dxeTreeList:aspxtreelistexporter id="ASPxTreeListExporter1" runat="server">
                                </dxeTreeList:aspxtreelistexporter>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
