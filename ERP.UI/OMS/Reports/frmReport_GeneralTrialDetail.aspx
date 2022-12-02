<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/PopUp.Master" Inherits="ERP.OMS.Reports.Reports_frmReport_GeneralTrialDetail" Codebehind="frmReport_GeneralTrialDetail.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="../CSS/style.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="/assests/js/init.js"></script>
    
     
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />

    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>

    <link rel="stylesheet" href="../modalfiles/modal.css" type="text/css" />

    <script type="text/javascript" src="../modalfiles/modal.js"></script>



     <script language="javascript" type="text/javascript">
         function ShowHideFilter(obj) {
             grid.PerformCallback(obj);
         }

         function callback() {
             grid.PerformCallback();
             height();
         }

         function ShowLedger(objMainID, objSubID, objSegmentID, objMainAcc, objSubAcc, objUcc, objDate) {
             var URL = 'frmReport_IFrameLedgerView.aspx?MainID=' + objMainID + ' &SubID=' + objSubID + ' &SegmentID=' + objSegmentID + ' &date=' + objDate;
             //	    OnMoreInfoClick(URL,""+objMainAcc+"  -  "+objSubAcc+" ["+objUcc+"]",'1000px','500px',"N");
             //	    
             //	    
             //	      var URL='../management/ObligationStatementCM.aspx?date='+objDate +' &SettNo='+objSettno+' &Compid='+objComp+' &SegID='+objSegment+' &ClientID='+objCliID;	        
             editwin = dhtmlmodal.open("Editbox", "iframe", URL, "" + objSubAcc + "", "width=940px,height=450px,center=1,resize=1,top=500", "recal");
         }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
 <%--   <table width="100%"><tr><td>
    <div id="display" runat="server" >
                                </div></td></tr></table>--%>
                                
                                
                                <table class="TableMain100">
            <%-- <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">Email Accounts Setup</span></strong>
                    </td>
                </tr>--%>
                <tr>
                <td>
                <table width="100%" style="display:none">
                  <tr>
                                                    <td id="Td1" align="left">
                                                        <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                                            Show Filter</span></a> || <a href="javascript:ShowHideFilter('All');"><span style="color: #000099;
                                                                text-decoration: underline">All Records</span></a>
                                                    </td>
                                                    <td align="right">
                                                        <asp:DropDownList ID="ddlExport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExport_SelectedIndexChanged1">
                            <asp:ListItem Value="Ex" Selected="True">Export</asp:ListItem>
                            <asp:ListItem Value="E">Excel</asp:ListItem>                           
                        </asp:DropDownList>
                                                    </td>
                                                   
                                                </tr></table>
                    <% if (rights.CanExport)
                                               { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary"   AutoPostBack="true" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" >
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                    <% } %>

                    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A3" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>

                </td>
                
                </tr>
                <tr>
                    <td>
                 
                        <dxe:ASPxGridView ID="gridStatus" ClientInstanceName="grid"  Width="100%"
                            KeyFieldName="ClosingID"  runat="server" 
                            AutoGenerateColumns="False" OnCustomCallback="gridStatus_CustomCallback" OnDataBound="gridStatus_DataBound" OnDataBinding="gridStatus_DataBinding" OnHtmlRowCreated="gridStatus_HtmlRowCreated" OnHtmlRowPrepared="gridStatus_HtmlRowPrepared">
                            <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                               <Settings ShowStatusBar="Hidden" ShowFilterRow="true"  ShowFilterRowMenu="true"   />
                            <Styles>
                               <%-- <FocusedRow CssClass="gridselectrow" BackColor="#FCA977">
                                </FocusedRow>
                                <FocusedGroupRow CssClass="gridselectrow" BackColor="#FCA977">
                                </FocusedGroupRow>--%>
                                <Footer   Font-Bold="True" HorizontalAlign="Right">
                                </Footer>
                            </Styles>
                            <SettingsPager NumericButtonCount="20" PageSize="15" ShowSeparators="True" AlwaysShowPager="True">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            <Columns>
                                <dxe:GridViewDataTextColumn Visible="False" FieldName="ClosingID" Caption="ID" >
                                    <CellStyle CssClass="gridcellleft">
                                    </CellStyle>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                            
                                <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="accountsledger_subaccountName"  width="10%"
                                    Caption="Name">
                                    <CellStyle CssClass="gridcellleft" Wrap="True">
                                    </CellStyle>
                                    <FooterTemplate>
                                             Total :
                                                </FooterTemplate>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="BRANCH" width="30%"
                                    Caption="Branch">
                                    <CellStyle CssClass="gridcellleft" Wrap="True">
                                    </CellStyle>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                                
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="UCC" width="30%"
                                    Caption="Code">
                                    <CellStyle CssClass="gridcellleft" Wrap="True">
                                    </CellStyle>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                                
                                    <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="OpeningDr" width="15%"
                                    Caption="Opening Dr" >
                                    <CellStyle CssClass="gridcellright" >
                                    </CellStyle>
                                     <FooterTemplate>
                                              <%# GetTotalOPDr() %>
                                                </FooterTemplate>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                                
                                 
                                  
                                    <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="OpeningCr" width="15%"
                                    Caption="OpeningCr">
                                    <CellStyle CssClass="gridcellright">
                                    </CellStyle>
                                     <FooterTemplate>
                                               <%# GetTotalOPCr() %>
                                                </FooterTemplate>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                                 
                                    <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="AmountDr"
                                    Caption="AmountDr">
                                    <CellStyle CssClass="gridcellright">
                                    </CellStyle>
                                    <FooterTemplate>
                                              <%# GetTotalAmDr() %>
                                                </FooterTemplate>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                                
                                    <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="AmountCr"
                                    Caption="AmountCr">
                                    <CellStyle CssClass="gridcellright">
                                    </CellStyle>
                                    <FooterTemplate>
                                    
                                              <%# GetTotalAmCr() %>
                                                </FooterTemplate>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>
                                

                                    <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="ClosingDr"
                                    Caption="ClosingDr">
                                    <CellStyle CssClass="gridcellright">
                                    </CellStyle>
                                    <FooterTemplate>
                                              <%# GetTotalCLDr() %>
                                                </FooterTemplate>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                </dxe:GridViewDataTextColumn>

                                    <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="ClosingCr"
                                    Caption="ClosingCr">
                                    <CellStyle CssClass="gridcellright">
                                    </CellStyle>
                                    <FooterTemplate>
                                              <%# GetTotalCLCr() %>
                                                </FooterTemplate>
                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                                    </dxe:GridViewDataTextColumn>
                                
                                                     
                            </Columns>
                                    <StylesEditors>
                                <ProgressBar Height="25px">
                                </ProgressBar>
                            </StylesEditors>
                             <TotalSummary>
                                            <dxe:ASPxSummaryItem FieldName="TotOPDr" ShowInColumn="OpeningDr" ShowInGroupFooterColumn="OpeningDr" Tag="Total Opening Debit" DisplayFormat="#,##,###.00" />
                                            <dxe:ASPxSummaryItem FieldName="TotOPCr" ShowInColumn="OpeningCr" ShowInGroupFooterColumn="OpeningCr" Tag="Total Opening Debit" DisplayFormat="#,##,###.00" />
                                            <dxe:ASPxSummaryItem FieldName="TotAmtDr" ShowInColumn="AmountDr" ShowInGroupFooterColumn="AmountDr" Tag="Total Opening Debit" DisplayFormat="#,##,###.00" />
                                            <dxe:ASPxSummaryItem FieldName="TotAmtCr" ShowInColumn="AmountCr" ShowInGroupFooterColumn="AmountCr" Tag="Total Opening Debit" DisplayFormat="#,##,###.00" />
                                            <dxe:ASPxSummaryItem FieldName="TotCLDr" ShowInColumn="ClosingDr" ShowInGroupFooterColumn="ClosingDr" Tag="Total Opening Debit" DisplayFormat="#,##,###.00" />
                                            <dxe:ASPxSummaryItem FieldName="TotCLCr" ShowInColumn="ClosingCr" ShowInGroupFooterColumn="ClosingCr" Tag="Total Opening Debit" DisplayFormat="#,##,###.00" />
                                        </TotalSummary>
                            <Settings ShowFooter="True" />
                        </dxe:ASPxGridView>
                                  
           
                    </td>
                </tr>
            </table>
    </div>
</asp:Content>
