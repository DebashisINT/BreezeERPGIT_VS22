<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_frmReport_salesActivityAnalysis" Codebehind="frmReport_salesActivityAnalysis.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
    <style>
        #RBReportType>tbody>tr>td>table>tbody>tr>td {
            padding-top:0px !important;
            padding-bottom:0px !important;
        }
    </style>
    <script language="javascript" type="text/javascript">
        FieldName = 'TxtStartDate';
        function frmOpenNewWindow1(location, v_height, v_weight) {
            var y = (screen.availHeight - v_height) / 2;
            var x = (screen.availWidth - v_weight) / 2;
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + y + ",left=" + x + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=no,resizable=no,dependent=no");
        }
        function loadNotes(Obj) {
           
            var str = "../management/ShowHistory_Phonecall.aspx?id1=" + Obj;
            window.location.href = str;
            //frmOpenNewWindow1(str, 400, 900)
        }
</script>
   <div class="panel-heading">
        <div class="panel-title">
            <h3>Sales Activity Analysis</h3>
        </div>

    </div>
         <div class="crossBtn"><a href="../Management/ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main" style="padding-top:0;">
<table class="TableMain100">
    
    <tr>
        <td>
            <table width="100%">
                <tr>
                        <td>
                            <asp:Panel ID="Panel2" runat="server" Width="100%">
                                <div class="col-md-6" style="padding-left:0;">
                                    <table>
                                        <tr>
                                            <td style="padding-right:15px">
                                                 <label>Start Date:</label>
                                                <dxe:ASPxDateEdit ID="TxtStartDate" runat="server" UseMaskBehavior="True" EditFormat="Custom" EditFormatString="dd MMMM yyyy">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                    <DropDownButton Text="From Date">
                                                    </DropDownButton>
                                                </dxe:ASPxDateEdit>
                                        
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtStartDate"
                                        Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator></td>
                                            <td style="padding-right:15px">
                                                <label>End Date:</label>
                                                <dxe:ASPxDateEdit ID="TxtEndDate" runat="server" UseMaskBehavior="True" EditFormat="Custom"  EditFormatString="dd MMMM yyyy">
                                                        <ButtonStyle Width="13px">
                                                        </ButtonStyle>
                                                        <DropDownButton Text="To Date">
                                                        </DropDownButton>
                                                    </dxe:ASPxDateEdit>
                                        
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="TxtEndDate"
                                            Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!"></asp:RequiredFieldValidator></td>
                                            <td style="padding-right:15px">
                                                <label>Report Type:</label>
                                                <dxe:ASPxRadioButtonList ID="RBReportType" runat="server" CssPostfix="BlackGlass" RepeatDirection="Horizontal" Width="168px" Height="2px" SelectedIndex="0" TextSpacing="3px">
                                                    <Items>
                                                            <dxe:ListEditItem Text="Screen" Value="Screen" />
                                                            <dxe:ListEditItem Text="Print" Value="Print" />
                                                    </Items>
                                                    <ValidationSettings ErrorText="Error has occurred">
                                                        <ErrorImage Height="14px" Width="14px" />
                                                    </ValidationSettings>
                                                    </dxe:ASPxRadioButtonList>
                                            </td>
                                            <td style="padding-top:26px;">
                                                <dxe:ASPxButton ID="btnShowReport" runat="server" Text="Show"  OnClick="BtnReport_Click" CssClass="btn btn-success">
                                                </dxe:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                        
                                    
                                        
                                    
                                    <div>
                                        
                                    </div>
                                     
                                </div>
                            </asp:Panel>
                        </td>
                </tr>
                <%--<tr id="CollapseAll" runat="server">
                     <td style="text-align: right; vertical-align:bottom">
                        <table id="tblExport" runat="server">
                             <tr>
                                <td style="text-align: right; vertical-align:bottom">
                                    <dxe:ASPxButton ID="btnExpandAll" runat="server" Text="Expand All"  AutoPostBack="false" CssClass="btn btn-primary" >
                                         <ClientSideEvents Click="function(s, e) {
                                                                  List.ExpandAll();
                                                                  }" />
                                    </dxe:ASPxButton>
                               </td>
                               <td style="text-align: center; vertical-align:bottom">
                                    <dxe:ASPxButton ID="btnCollapsAll" runat="server" Text="Collaps All" AutoPostBack="false"  CssClass="btn btn-primary">
                                         <ClientSideEvents Click="function(s, e) {
                                                                  List.CollapseAll();
                                                                  }" />
                                    </dxe:ASPxButton>
                               </td>
                               <td style=" vertical-align:bottom">
                                    <dxe:ASPxComboBox ID="cmbExport" runat="server" ValueType="System.String" Height="17px" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" SelectedIndex="0" AutoPostBack="true" Font-Overline="False" Font-Size="12px">
                                         <ButtonStyle Width="13px">
                                         </ButtonStyle>
                                         <Items>
                                            
 				<dxe:ListEditItem Text="Select" Value="0" />
                            <dxe:ListEditItem Text="PDF" Value="1" />
                            <dxe:ListEditItem Text="XLS" Value="2" />
                            <dxe:ListEditItem Text="RTF" Value="3" />
                            <dxe:ListEditItem Text="CSV" Value="4" />
                                         </Items>
                                         <DropDownButton Text="Export" ToolTip="Export File">
                                         </DropDownButton>
                                    </dxe:ASPxComboBox>
                               </td>
                            </tr>
                        </table>
                    </td>
                </tr>--%>
                <tr id="GridTree" runat="server">
                    <td>
                        <asp:Panel ID="Panel1" runat="server" Width="100%">                        
                        <dxeTreeList:ASPxTreeList ID="TvSalesActivityAnalyst" runat="server" ClientInstanceName="List" Width="100%" KeyFieldName="ID" ParentFieldName="ParentID">
                            <Styles>
                            </Styles>
                            <Settings SuppressOuterGridLines="True" GridLines="None" ShowFooter="false" ShowGroupFooter="false" />
                            <Images>
                                <ExpandedButton Height="11px" Width="11px" />
                                <CustomizationWindowClose  Width="13px" />
                                <CollapsedButton  Width="11px" />
                            </Images>
                            <Columns>
                                <dxeTreeList:TreeListTextColumn Caption="Name" FieldName="LeadName" VisibleIndex="0">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="PhoneNo" FieldName="PhoneNo" VisibleIndex="1">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="Visited" FieldName="Visited" VisibleIndex="2">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="Phone FollowUp" FieldName="Phone FollowUp" VisibleIndex="3">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="Outcome" FieldName="Outcome" VisibleIndex="4">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn Caption="LeadId" FieldName="LeadId" VisibleIndex="5" Visible="false">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                </dxeTreeList:TreeListTextColumn>
                                <dxeTreeList:TreeListTextColumn  VisibleIndex="6" Width="50px" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                     <DataCellTemplate >
                                         <a href="#" onclick="loadNotes('<%# Eval("LeadId") %>')" title="History">
                                             <img src="../images/history.png"
                                         </a>
                                     </DataCellTemplate>
                                    <HeaderCaptionTemplate>Actions</HeaderCaptionTemplate>
                                </dxeTreeList:TreeListTextColumn>
                            </Columns>
                        </dxeTreeList:ASPxTreeList>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <dxeTreeList:ASPxTreeListExporter ID="ASPxTreeListExporter1" runat="server">
            </dxeTreeList:ASPxTreeListExporter>
        </td>
    </tr>
    <tr>
        <td>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Start Date Required" ControlToValidate="TxtStartDate" Display="None" ValidationGroup="a"></asp:RequiredFieldValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="End Date Required" ControlToValidate="TxtEndDate" Display="None" ValidationGroup="a"></asp:RequiredFieldValidator>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="a" />
        </td>
    </tr>
</table>
    </div>
</asp:Content>

