<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_cdslBill" CodeBehind="frmReport_cdslBill.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        function EndCall(obj) {
            height();
        }
        function SignOff() {
            window.parent.SignOff();
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        function PageLoad() {
            FieldName = 'exp';
            Dp = window.location.search.split('=')[1];

            spanHeader.innerHTML = Dp + ' Billing';

            tdHeader.style.display = 'none';
            tdFooter.style.display = 'none';
            document.getElementById('<%=txtHeader.ClientID %>').style.display = "none";
                 document.getElementById('<%=txtHeader_hidden.ClientID %>').style.display = "none";
               document.getElementById('<%=txtFooter.ClientID %>').style.display = "none";
               document.getElementById('<%=txtFooter_hidden.ClientID %>').style.display = "none";

               document.getElementById('<%=txtHeader.ClientID%>').value = '';
               document.getElementById('<%=txtHeader_hidden.ClientID %>').value = '';
               document.getElementById('<%=txtFooter.ClientID%>').value = '';
               document.getElementById('<%=txtFooter_hidden.ClientID %>').value = '';

               document.getElementById('f').style.display = 'none';
               document.getElementById('tblScreen').style.display = 'none';
               document.getElementById('Div1').style.display = 'none';
               document.getElementById('txtEmpName_hidden').style.display = 'none';
               document.getElementById('tdvalue').style.display = 'none';
               document.getElementById('tdempname').style.display = 'none';

               load();
           }
           function ShowHideFilter(obj) {
               grid.PerformCallback(obj);
           }

           function fetchResult(obj) {
               CheckMonthRange(obj);

               //              grid.PerformCallback(obj);

           }
           function CheckMonthRange(obj) {
               finYear = '<%=FinYear%>';
              var arrFinYear = finYear.split('-');
              monthFrom = MonthFromCombo.GetSelectedItem().value;
              monthTo = MonthToCombo.GetSelectedItem().value;
              if ((((monthFrom >= 1) && (monthFrom <= 3)) && ((monthTo >= monthFrom) && (monthTo <= 3)))
                   || (((monthFrom >= 4) && (monthFrom <= 12)) &&
                       (((monthTo >= monthFrom) && (monthTo <= 12)) || ((monthTo >= 1) && (monthTo <= 3)))
                      )
                 ) {
                  if ((monthFrom >= 1) && (monthFrom <= 3))
                      yearFrom = arrFinYear[1];
                  else if ((monthFrom >= 4) && (monthFrom <= 12))
                      yearFrom = arrFinYear[0];

                  if ((monthTo >= 1) && (monthTo <= 3))
                      yearTo = arrFinYear[1];
                  else if ((monthTo >= 4) && (monthTo <= 12))
                      yearTo = arrFinYear[0];

                  if (monthFrom == monthTo) {
                      if (confirm('Do you want to view bill for ' + MonthFromCombo.GetSelectedItem().text + ', ' + yearFrom + ' ?'))
                          MonthRange(obj);

                  }
                  else {
                      if (confirm('Do you want to view bill from ' + MonthFromCombo.GetSelectedItem().text + ', ' + yearFrom + ' to ' + MonthToCombo.GetSelectedItem().text + ', ' + yearTo + ' ?'))
                          MonthRange(obj);
                  }
              }
              else
                  alert('Invalid month range. Month range should be positive and within financial year ' + finYear);
          }

          function MonthRange(obj) {
              monthRng = '';
              if (monthFrom == monthTo)
                  monthRng = monthFrom;

              else if ((((monthFrom >= 1) && (monthFrom <= 3)) && ((monthTo >= monthFrom) && (monthTo <= 3)))
                  || (((monthFrom >= 4) && (monthFrom <= 12)) && ((monthTo >= monthFrom) && (monthTo <= 12)))
                 ) {
                  for (var i = monthFrom; i <= monthTo; i++) {
                      if (i == monthFrom)
                          monthRng = monthFrom;
                      else
                          monthRng = monthRng + ',' + i;
                  }

              }
              else if (((monthFrom >= 4) && (monthFrom <= 12)) && ((monthTo >= 1) && (monthTo <= 3))) {
                  for (i = monthFrom; i <= 12; i++) {
                      if (i == monthFrom)
                          monthRng = monthFrom;
                      else
                          monthRng = monthRng + ',' + i;
                  }
                  for (i = 1; i <= monthTo; i++) {
                      monthRng = monthRng + ',' + i;
                  }
              }
              //            alert(obj+'~'+monthRng);
              grid.PerformCallback(obj + '~' + monthRng);
          }

          function divscroll(obj) {
              document.getElementById('div1_grdBillRegister').className = "grid_scrollNSEFO";
          }

          function hideBillSummary() {

              document.getElementById('f').style.display = 'inline';
              document.getElementById('tblSummary').style.display = 'none';
              document.getElementById('tblScreen').style.display = 'inline';

          }
          function showBillSummary() {
              document.getElementById('f').style.display = 'none';
              document.getElementById('tblScreen').style.display = 'none';
              document.getElementById('tblSummary').style.display = 'inline';

          }

          function afterPartialPostBack() {

              hideBillSummary();
              document.getElementById('Div1').style.display = 'none';
          }

          function showProgress() {
              document.getElementById('Div1').style.display = 'inline';
          }

          function CallAjax(obj1, obj2, obj3, obj) {
              if ((obj == 'H') || (obj == 'F')) {
                  var obj4 = obj;
                  ajax_showOptions(obj1, obj2, obj3, obj4);
              }
              else
                  ajax_showOptions(obj1, obj2, obj3);
          }

          function checkChange() {
              var checkbox = document.getElementById('chkSignature')
              if (checkbox.checked) {
                  document.getElementById('tdvalue').style.display = 'inline';
                  document.getElementById('tdempname').style.display = 'inline';
              }
              else {
                  document.getElementById('tdvalue').style.display = 'none';
                  document.getElementById('tdempname').style.display = 'none';

              }

          }
          function HeaderFooter(obj) {
              if (obj.name == 'chbHeader') {
                  if (obj.checked) {
                      document.getElementById('<%=txtHeader.ClientID%>').style.display = 'inline';
                    tdHeader.style.display = 'inline';
                    document.getElementById('<%=txtHeader.ClientID%>').focus();
                }
                else {
                    document.getElementById('<%=txtHeader.ClientID%>').value = '';
                    document.getElementById('<%=txtHeader_hidden.ClientID%>').value = '';
                    tdHeader.style.display = 'none';
                }
            }
            else if (obj.name == 'chbFooter') {
                if (obj.checked) {
                    document.getElementById('<%=txtFooter.ClientID%>').style.display = 'inline';
                    tdFooter.style.display = 'inline';
                    document.getElementById('<%=txtFooter.ClientID%>').focus();
                }
                else {
                    document.getElementById('<%=txtFooter.ClientID%>').value = '';
                    document.getElementById('<%=txtFooter_hidden.ClientID%>').value = '';
                    tdFooter.style.display = 'none';
                }
            }

    }
    </script>
    <table class="TableMain100">
        <tr>
            <td class="EHEADER" colspan="0" style="text-align: center">
                <strong><span id="spanHeader" style="color: #000099"></span></strong>
            </td>
            <td class="EHEADER" id="f">
                <a href="javascript:void(0);" onclick="showBillSummary();"><span style="color: Blue; text-decoration: underline; font-size: 8pt; font-weight: bold">Show Summary</span></a>
            </td>
        </tr>
    </table>
    <table id="tblSummary" border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td>
                <table class="TableMain100">

                    <tr>
                        <td class="gridcellleft">
                            <span style="color: blue">Bill From : </span>
                            <dxe:ASPxComboBox ID="cmbMonthFrom" ClientInstanceName="MonthFromCombo" Width="160px" runat="server"
                                Font-Size="12px" ValueType="System.Int32" Font-Bold="False" SelectedIndex="0">
                                <Items>
                                    <dxe:ListEditItem Text="January" Value="1"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="February" Value="2"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="March" Value="3"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="April" Value="4"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="May" Value="5"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="June" Value="6"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="July" Value="7"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="August" Value="8"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="September" Value="9"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="October" Value="10"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="November" Value="11"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="December" Value="12"></dxe:ListEditItem>
                                </Items>
                                <ButtonStyle Width="60px">
                                </ButtonStyle>
                                <DropDownButton Text="MonthFrom">
                                </DropDownButton>

                            </dxe:ASPxComboBox>

                        </td>
                        <td class="gridcellleft">
                            <span style="color: blue">Bill To : </span>
                            <dxe:ASPxComboBox ID="cmbMonthTo" ClientInstanceName="MonthToCombo" Width="160px" runat="server"
                                Font-Size="12px" ValueType="System.Int32" Font-Bold="False" SelectedIndex="0">
                                <Items>
                                    <dxe:ListEditItem Text="January" Value="1"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="February" Value="2"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="March" Value="3"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="April" Value="4"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="May" Value="5"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="June" Value="6"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="July" Value="7"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="August" Value="8"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="September" Value="9"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="October" Value="10"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="November" Value="11"></dxe:ListEditItem>
                                    <dxe:ListEditItem Text="December" Value="12"></dxe:ListEditItem>
                                </Items>
                                <ButtonStyle Width="60px">
                                </ButtonStyle>
                                <DropDownButton Text="MonthTo">
                                </DropDownButton>

                            </dxe:ASPxComboBox>

                        </td>
                        <td class="gridcellright">
                            <span style="color: blue;">Show Bill Amount >= </span>
                        </td>
                        <td style="text-align: left">
                            <dxe:ASPxTextBox ID="txtbilAmt" ClientInstanceName="cwithdraw" runat="server" Width="120px"
                                TabIndex="18" HorizontalAlign="Right" Text="0.01">
                                <MaskSettings Mask="&lt;0..9999999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                <ValidationSettings ErrorDisplayMode="None">
                                </ValidationSettings>
                            </dxe:ASPxTextBox>
                        </td>
                        <td class="gridcellleft">
                            <dxe:ASPxButton ID="btnShow" runat="server" Text="Show" AutoPostBack="False">
                                <ClientSideEvents Click="function(s, e) { fetchResult('Show');}" />
                            </dxe:ASPxButton>
                        </td>
                        <td>
                            <table align="left">
                                <tr class="gridcellleft">
                                    <td>
                                        <span class="Ecoheadtxt" style="color: blue;">Add Signatory:</span></td>
                                    <td>
                                        <asp:CheckBox ID="chkSignature" runat="server" onclick="checkChange();" />
                                    </td>
                                    <td id="tdempname">
                                        <span class="Ecoheadtxt" style="color: blue;">Employee Name:</span>
                                    </td>
                                    <td id="tdvalue">
                                        <asp:TextBox ID="txtEmpName" runat="server"></asp:TextBox>
                                        <asp:TextBox ID="txtEmpName_hidden" runat="server"></asp:TextBox>
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
                <table class="TableMain100">
                    <tr>
                        <td>

                            <table>
                                <tr>
                                    <td id="ShowFilter">
                                        <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>
                                    </td>
                                    <td id="Td1">
                                        <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td colspan="5" style="text-align: right;">
                            <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" ClientInstanceName="exp"
                                BackColor="Navy" Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                ValueType="System.Int32" Width="130px" SelectedIndex="0">
                                <Items>
                                    <dxe:ListEditItem Text="Select" Value="0" />
                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                    <dxe:ListEditItem Text="CSV" Value="4" />
                                </Items>
                                <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                </ButtonStyle>
                                <ItemStyle BackColor="Navy" ForeColor="White">
                                    <HoverStyle BackColor="#8080FF" ForeColor="White">
                                    </HoverStyle>
                                </ItemStyle>
                                <Border BorderColor="White" />
                                <DropDownButton Text="Export">
                                </DropDownButton>
                            </dxe:ASPxComboBox>
                            <table>
                                <tr>
                                    <td colspan="2">
                                        <dxe:ASPxButton ID="btnView" runat="server" Text="View" AutoPostBack="True" OnClick="btnView_Click">
                                            <ClientSideEvents Click="function(s, e) {showProgress(); }" />
                                        </dxe:ASPxButton>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxButton ID="btnPrint" runat="server" Text="Print" AutoPostBack="True" OnClick="btnPrint_Click">
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"></td>
                                    <td>
                                        <asp:CheckBox ID="chbHeader" runat="server" onclick="HeaderFooter(this);" /></td>
                                    <td>Use Header</td>
                                    <td id="tdHeader">
                                        <asp:TextBox ID="txtHeader" runat="server" Font-Size="12px" Width="200px"
                                            onkeyup="CallAjax(this,'GetHeaderFooter',event,'H');"></asp:TextBox>
                                        <asp:TextBox ID="txtHeader_hidden" runat="server" Width="14px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"></td>
                                    <td>
                                        <asp:CheckBox ID="chbFooter" runat="server" onclick="HeaderFooter(this);" /></td>
                                    <td>Use Footer</td>
                                    <td id="tdFooter">
                                        <asp:TextBox ID="txtFooter" runat="server" Font-Size="12px" Width="200px"
                                            onkeyup="CallAjax(this,'GetHeaderFooter',event,'F');"></asp:TextBox>
                                        <asp:TextBox ID="txtFooter_hidden" runat="server" Width="14px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="text-align: center">
                <asp:Panel ID="Panel1" runat="server" Width="980px" ScrollBars="Horizontal" Style="margin: auto;">
                    <div id="div1_grdBillRegister">
                        <dxe:ASPxGridView ID="gridCdslBill" runat="server" ClientInstanceName="grid" KeyFieldName="DPBillSummary_BillNumber"
                            AutoGenerateColumns="False" OnCustomCallback="gridCdslBill_CustomCallback" OnHtmlRowCreated="gridCdslBill_HtmlRowCreated"
                            OnSummaryDisplayText="gridCdslBill_SummaryDisplayText2">

                            <ClientSideEvents EndCallback="function(s, e) {
	  EndCall(s.cpEND);
}"></ClientSideEvents>
                            <SettingsBehavior AllowFocusedRow="True" />
                            <Styles>
                                <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>

                                <FocusedRow CssClass="gridselectrow"></FocusedRow>

                                <LoadingPanel ImageSpacing="10px"></LoadingPanel>

                                <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
                            </Styles>
                            <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                <FirstPageButton Visible="True"></FirstPageButton>

                                <LastPageButton Visible="True"></LastPageButton>
                            </SettingsPager>
                            <Columns>
                                <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="DPBillSummary_BillNumber" Caption="Bill No.">
                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>
                                    <FooterTemplate>
                                        <b>Total:</b>

                                    </FooterTemplate>

                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="GroupName" Caption="Group Name">
                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText" SortMode="Value"></Settings>
                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="FirstHolderName" Caption="Client Name">
                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataDateColumn VisibleIndex="3" FieldName="AccountOpeningDate" Caption="A/C Opening Date">
                                    <HeaderStyle Wrap="True"></HeaderStyle>

                                    <PropertiesDateEdit DisplayFormatString="dd MMM yyyy"></PropertiesDateEdit>

                                    <Settings AutoFilterCondition="Contains" SortMode="Value" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataDateColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="AccountStatus" Caption="Status">
                                    <HeaderStyle Wrap="True"></HeaderStyle>

                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="DPBillSummary_AccountOpeningCharges" Caption="Doc &amp; A/C Opening Charges">
                                    <HeaderStyle Wrap="True"></HeaderStyle>

                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="DPBillSummary_AMC" Caption="AMC">
                                    <HeaderStyle Wrap="True"></HeaderStyle>

                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="DPBillSummary_TransactionCharges" Caption="Trans Charges">
                                    <HeaderStyle Wrap="True"></HeaderStyle>

                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="DPBillSummary_Demat" Caption="Demat Charges">
                                    <HeaderStyle Wrap="True"></HeaderStyle>

                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="DPBillSummary_Pledge" Caption="Pledge Charges">
                                    <HeaderStyle Wrap="True"></HeaderStyle>

                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="DPBillSummary_HoldingCharges" Caption="Holding Charges">
                                    <HeaderStyle Wrap="True"></HeaderStyle>

                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="11" FieldName="DPBillSummary_SettlementCharges" Caption="Settlement Fees">
                                    <HeaderStyle Wrap="True"></HeaderStyle>

                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="otherCharges" Caption="Other Charges">
                                    <HeaderStyle Wrap="True"></HeaderStyle>

                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="13" FieldName="total" Caption="Total">
                                    <HeaderStyle Wrap="True"></HeaderStyle>

                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="14" FieldName="allTaxes" Caption="Serv Taxes &amp; Cess">
                                    <HeaderStyle Wrap="True"></HeaderStyle>

                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="15" FieldName="DPBillSummary_RoundOffAmount" Caption="Round Off">
                                    <HeaderStyle Wrap="True"></HeaderStyle>

                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>

                                    <FooterCellStyle HorizontalAlign="Right"></FooterCellStyle>
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="16" FieldName="DPBillSummary_NetBillAmount" Caption="Net Bill Amt">
                                    <HeaderStyle Wrap="True"></HeaderStyle>

                                    <Settings AutoFilterCondition="Contains" FilterMode="DisplayText"></Settings>

                                    <CellStyle Wrap="False" HorizontalAlign="Right" CssClass="gridcellleft"></CellStyle>
                                </dxe:GridViewDataTextColumn>
                            </Columns>
                            <TotalSummary>
                                <dxe:ASPxSummaryItem FieldName="DPBillSummary_NetBillAmount" Tag="net" ShowInGroupFooterColumn="Net Bill Amt" ShowInColumn="Net Bill Amt" SummaryType="Sum"></dxe:ASPxSummaryItem>
                                <dxe:ASPxSummaryItem FieldName="DPBillSummary_AMC" Tag="net" ShowInGroupFooterColumn="AMC" ShowInColumn="AMC" SummaryType="Sum"></dxe:ASPxSummaryItem>
                                <dxe:ASPxSummaryItem FieldName="total" Tag="net" ShowInGroupFooterColumn="Total" ShowInColumn="Total" SummaryType="Sum"></dxe:ASPxSummaryItem>
                                <dxe:ASPxSummaryItem FieldName="otherCharges" Tag="net" ShowInGroupFooterColumn="Other Charges" ShowInColumn="Other Charges" SummaryType="Sum"></dxe:ASPxSummaryItem>
                                <dxe:ASPxSummaryItem FieldName="DPBillSummary_TransactionCharges" Tag="net" ShowInGroupFooterColumn="Trans Charges" ShowInColumn="Trans Charges" SummaryType="Sum"></dxe:ASPxSummaryItem>
                                <dxe:ASPxSummaryItem FieldName="allTaxes" Tag="net" ShowInGroupFooterColumn="Serv Taxes &amp; Cess" ShowInColumn="Serv Taxes &amp; Cess" SummaryType="Sum"></dxe:ASPxSummaryItem>
                                <dxe:ASPxSummaryItem FieldName="DPBillSummary_SettlementCharges" Tag="net" ShowInGroupFooterColumn="Settlement Fees" ShowInColumn="Settlement Fees" SummaryType="Sum"></dxe:ASPxSummaryItem>
                                <dxe:ASPxSummaryItem FieldName="DPBillSummary_RoundOffAmount" Tag="net" ShowInGroupFooterColumn="Round Off" ShowInColumn="Round Off" SummaryType="Sum"></dxe:ASPxSummaryItem>
                                <dxe:ASPxSummaryItem FieldName="DPBillSummary_HoldingCharges" Tag="net" ShowInGroupFooterColumn="Holding Charges" ShowInColumn="Holding Charges" SummaryType="Sum"></dxe:ASPxSummaryItem>
                                <dxe:ASPxSummaryItem FieldName="DPBillSummary_AccountOpeningCharges" Tag="net" ShowInGroupFooterColumn="Doc &amp; A/C Opening Charges" ShowInColumn="Doc &amp; A/C Opening Charges" SummaryType="Sum"></dxe:ASPxSummaryItem>
                                <dxe:ASPxSummaryItem FieldName="DPBillSummary_Demat" Tag="net" ShowInGroupFooterColumn="Demat Charges" ShowInColumn="Demat Charges" SummaryType="Sum"></dxe:ASPxSummaryItem>
                                <dxe:ASPxSummaryItem FieldName="DPBillSummary_Pledge" Tag="net" ShowInGroupFooterColumn="Pledge Charges" ShowInColumn="Pledge Charges" SummaryType="Sum"></dxe:ASPxSummaryItem>
                            </TotalSummary>
                            <Settings ShowFooter="True" ShowStatusBar="Visible" />
                            <StylesEditors>
                                <ProgressBar Height="25px"></ProgressBar>
                            </StylesEditors>
                        </dxe:ASPxGridView>
                    </div>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:ScriptManager runat="server" ID="s1" AsyncPostBackTimeout="360000">
    </asp:ScriptManager>

    <script language="javascript" type="text/javascript">
        function load() {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(afterPartialPostBack);
        }
    </script>

    <asp:UpdatePanel runat="server" ID="u1">
        <ContentTemplate>
            <table width="100%" border="1" bordercolor="blue" id="tblScreen" class="TableMain100">
                <tr>
                    <td align="center">
                        <table id="tblpage" cellspacing="0" cellpadding="0" runat="server" width="100%">
                            <tr>
                                <td width="20" style="padding: 5px; display: none">
                                    <asp:LinkButton ID="ASPxFirst" runat="server" Font-Bold="True" ForeColor="maroon"
                                        OnClick="btnFirst" OnClientClick="javascript:showProgress();">First</asp:LinkButton></td>
                                <td width="25" style="display: none">
                                    <asp:LinkButton ID="ASPxPrevious" runat="server" Font-Bold="True" ForeColor="Blue"
                                        OnClick="btnPrevious" OnClientClick="javascript:showProgress();">Previous</asp:LinkButton></td>
                                <td width="20" style="padding: 5px; display: none">
                                    <asp:LinkButton ID="ASPxNext" runat="server" Font-Bold="True" ForeColor="maroon"
                                        OnClientClick="javascript:showProgress();" OnClick="btnNext">Next</asp:LinkButton></td>
                                <td width="20" style="display: none">
                                    <asp:LinkButton ID="ASPxLast" runat="server" Font-Bold="True" ForeColor="Blue" OnClientClick="javascript:showProgress();"
                                        OnClick="btnLast">Last</asp:LinkButton></td>
                                <td align="right">
                                    <asp:Label ID="listRecord" runat="server" Font-Bold="True" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <span style="color: red; text-align: center; display: none" id="norecord" class="Ecoheadtxt"
                            runat="server"><strong>No Transaction Found</strong></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="0" cellpadding="0" width="100%" style="text-align: left">
                            <tr style="font-size: 12px; font-family: Calibri">
                                <td>Client Id :
                                </td>
                                <td>
                                    <asp:Label ID="boid" runat="server" Font-Bold="True"></asp:Label></td>
                                <td>Category:</td>
                                <td>
                                    <asp:Label ID="category" runat="server" Font-Bold="True"></asp:Label></td>
                                <td>Status:
                                </td>
                                <td>
                                    <asp:Label ID="status" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                                <td>Bill Number:
                                </td>
                                <td>
                                    <asp:Label ID="billNumber" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Name of Holders:</td>
                                <td colspan="7">
                                    <asp:Label ID="holders" runat="server" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="8" style="height: 19px">
                                    <table>
                                        <tr>
                                            <td id="tdlblTrans" runat="server">
                                                <asp:Label ID="lblTransction" runat="server" Text="0"></asp:Label>
                                                of
                                                    <asp:Label ID="lblTotalTransction" runat="server" Text="0"></asp:Label>
                                                Transactions
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnTransFirst" runat="server" OnClientClick="javascript:showProgress();"
                                                    Font-Bold="True" ForeColor="maroon" OnClick="btnTransFirst_Click">First</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnTransPrevious" runat="server" OnClientClick="javascript:showProgress();"
                                                    Font-Bold="True" ForeColor="Blue" OnClick="btnTransPrevious_Click">Previous</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnTransnNext" runat="server" Font-Bold="True" ForeColor="maroon"
                                                    OnClientClick="javascript:showProgress();" OnClick="btnTransnNext_Click">Next</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="btnTransnLast" runat="server" OnClientClick="javascript:showProgress();"
                                                    Font-Bold="True" ForeColor="Blue" OnClick="btnTransLast_Click">Last</asp:LinkButton>
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
                        <div id="display" runat="server">
                        </div>
                    </td>
                </tr>
                <tr id="tdsummary" runat="server">
                    <td>
                        <table width="100%">
                            <tr valign="top">
                                <td align="left" style="width: 30%">
                                    <div id="accounts" runat="server" style="border: solid 1px black;">
                                    </div>
                                </td>
                                <td align="right">
                                    <div id="summary" runat="server">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <table>
                            <tr>
                                <td id="tdlblTrans1" runat="server">
                                    <asp:Label ID="lblTransction1" runat="server" Text="0"></asp:Label>
                                    of
                                        <asp:Label ID="lblTotalTransction1" runat="server" Text="0"></asp:Label>
                                    Transactions
                                </td>
                                <td>
                                    <asp:LinkButton ID="btnTransFirst1" runat="server" OnClientClick="javascript:showProgress();"
                                        Font-Bold="True" ForeColor="maroon" OnClick="btnTransFirst_Click">First</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="btnTransPrevious1" OnClick="btnTransPrevious_Click" runat="server"
                                        Font-Bold="True" ForeColor="Blue">Previous</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="btnTransnNext1" OnClick="btnTransnNext_Click" runat="server"
                                        Font-Bold="True" ForeColor="maroon">Next</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="btnTransnLast1" runat="server" OnClientClick="javascript:showProgress();"
                                        Font-Bold="True" ForeColor="Blue" OnClick="btnTransLast_Click">Last</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnView" EventName="Click"></asp:AsyncPostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>
    <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 50%; height: 80; width: 150;'>
        <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td height='25' align='center' bgcolor='#FFFFFF'>
                                <img src='/assests/images/progress.gif' width='18' height='18'></td>
                            <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                <font size='2' face='Tahoma'><strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
