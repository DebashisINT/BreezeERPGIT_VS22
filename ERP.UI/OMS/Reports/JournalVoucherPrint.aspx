<%@ Page Title="Journal Voucher Print" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_JournalVoucherPrint" Codebehind="JournalVoucherPrint.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        $(function () {
                
        });
            function DateChangeForFrom()
            {
                var sessionVal ="<%=Session["LastFinYear"]%>";
                var objsession=sessionVal.split('-');
                var MonthDate=dtDate.GetDate().getMonth()+1;
                var DayDate=dtDate.GetDate().getDate();
                var YearDate=dtDate.GetDate().getYear();
                if(YearDate>=objsession[0])
                {
                    if(MonthDate<4 && YearDate==objsession[0])
                    {
                        alert('Enter Date Is Outside Of Financial Year !!');
                        var datePost=(4+'-'+1+'-'+objsession[0]);
                        dtDate.SetDate(new Date(datePost));
                    }
                    else if(MonthDate>3 && YearDate==objsession[1])
                    {
                        alert('Enter Date Is Outside Of Financial Year !!');
                        var datePost=(4+'-'+1+'-'+objsession[0]);
                        dtDate.SetDate(new Date(datePost));
                    }
                    else if(YearDate!=objsession[0] && YearDate!=objsession[1])
                    {
                        alert('Enter Date Is Outside Of Financial Year !!');
                        var datePost=(4+'-'+1+'-'+objsession[0]);
                        dtDate.SetDate(new Date(datePost));
                    }
                }
                else
                {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost=(4+'-'+1+'-'+objsession[0]);
                    dtDate.SetDate(new Date(datePost));
                }
            }
            function DateChangeForTo()
            {
                var sessionVal ="<%=Session["LastFinYear"]%>";
                var objsession=sessionVal.split('-');
                var MonthDate=dtToDate.GetDate().getMonth()+1;
                var DayDate=dtToDate.GetDate().getDate();
                var YearDate=dtToDate.GetDate().getYear();
            
                if(YearDate<=objsession[1])
                {
                    if(MonthDate<4 && YearDate==objsession[0])
                    {
                        alert('Enter Date Is Outside Of Financial Year !!');
                        var datePost=(3+'-'+31+'-'+objsession[1]);
                        dtToDate.SetDate(new Date(datePost));
                    }
                    else if(MonthDate>3 && YearDate==objsession[1])
                    {
                        alert('Enter Date Is Outside Of Financial Year !!');
                        var datePost=(3+'-'+31+'-'+objsession[1]);
                        dtToDate.SetDate(new Date(datePost));
                    }
                    else if(YearDate!=objsession[0] && YearDate!=objsession[1])
                    {
                        alert('Enter Date Is Outside Of Financial Year !!');
                        var datePost=(3+'-'+31+'-'+objsession[1]);
                        dtToDate.SetDate(new Date(datePost));
                    }
                }
                else
                {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost=(3+'-'+31+'-'+objsession[1]);
                    dtToDate.SetDate(new Date(datePost));
                }
            }
            function ForSpecific(obj)
            {
                if(obj=='A')
                    Hide('TdSpecific');
                else
                    Show('TdSpecific');
            }
            function Show(obj)
            {
                document.getElementById(obj).style.display='inline';
            }
            function Hide(obj)
            {
                document.getElementById(obj).style.display='none';
            }
            function Page_Load()
            {
                Hide('TdSpecific');
                height();
            }
            // function height()
            //{
            //    if(document.body.scrollHeight>=300)
            //        window.frameElement.height = document.body.scrollHeight;
            //    else
            //        window.frameElement.height = '300px';

            //    window.frameElement.Width = document.body.scrollWidth;
            //}
        
            function updateEditorText() 
            {
                var code=txtAccountCode.GetText().toUpperCase();
                if(code=='X' || code=='Y' || code=='Z')
                {
                    alert('You Can not Enter This Code');
                    txtAccountCode.SetText('');
                }
            }
            function OnAllCheckedChanged(s, e) {
                debugger;
                if (s.GetChecked()) {
                    gridJournalVouchar.SelectRows();
                    //Grid.SelectAllRowsOnPage();-- This method throws a js error
                }
                else {
                    gridJournalVouchar.UnselectRows();
                    //Grid.UnselectAllRowsOnPage(); ---- this also throws a js error

                }
            }

            function onSelectAll(s, e) {
                debugger;
                $('[id*="cbAll"]').each(function () {
                    try {
                        eval($(this)[0].id).SetChecked(s.GetChecked());
                    }

                    catch (err) { }
                });

                return false;
            }



            
    </script>
    <style>
        #gridJournalVouchar_DXMainTable tr:first-child td {
            padding:2px  10px!important;
            color:#fff !important;
            font-weight:600 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="panel-heading">
            <div class="panel-title">
                <h3>Journal Voucher Print</h3>
            
               <%-- <div id="btncross" class="crossBtn" style="display:none;margin-left:50px;"><a href="Sales_List.aspx"><i class="fa fa-times"></i></a></div>--%>
           
            </div>
        </div>
        <div class="form_main">
            <table class="TableMain100" style="width: 100%">
                <tr>
                    <td class="gridcellleft" style="width: 77px">
                        Date
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td style="width: 196px">
                                    <dxe:ASPxDateEdit ID="dtDate" runat="server" EditFormat="Custom" ClientInstanceName="dtDate"
                                        UseMaskBehavior="True">
                                        <DropDownButton Text="From ">
                                        </DropDownButton>
                                        <ClientSideEvents DateChanged="function(s,e){DateChangeForFrom();}" />
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtToDate" runat="server" EditFormat="Custom" ClientInstanceName="dtToDate"
                                        UseMaskBehavior="True">
                                        <DropDownButton Text="To">
                                        </DropDownButton>
                                        <ClientSideEvents DateChanged="function(s,e){DateChangeForTo();}" />
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="gridcellleft" style="width: 77px">
                        Voucher Type
                    </td>
                    <td>
                        <table width="">
                            <tr>
                                <td>
                                    <asp:RadioButton ID="radAll" Checked="true" runat="server" GroupName="a" onclick="ForSpecific('A')" />
                                </td>
                                <td style="padding-right:25px">
                                    All
                                </td>
                                <td>
                                    <asp:RadioButton ID="radSpecific" runat="server" GroupName="a" onclick="ForSpecific('B')" />
                                </td>
                                <td>
                                    Specific
                                </td>
                                <td id="TdSpecific">
                                    <dxe:ASPxTextBox ID="txtAccountCode" ClientInstanceName="txtAccountCode"
                                        runat="server" Width="100%" MaxLength="2">
                                        <ValidationSettings>
                                            <RequiredField IsRequired="True" ErrorText="Select Account Code" />
                                        </ValidationSettings>
                                        <ClientSideEvents KeyPress="function(s,e){window.setTimeout('updateEditorText()', 10);}" />
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                 <tr>
                    <td class="gridcellleft" style="width: 89px">
                        Print Option :
                    </td>
                    <td>
                        <table width="300px">
                            <tr>
                                <td id="td_user">
                                    <asp:CheckBox ID="user" runat="server"   />
                                </td>
                                <td>
                                    Print Entered By User
                                </td>
                                <td id="Td1">
                                    <asp:CheckBox ID="time" runat="server"  />
                                </td>
                                <td>
                                    Print Entry Date Time
                                </td>
                                
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btn btn-primary" OnClick="btnShow_Click" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; text-align: left" colspan="2">
                         <dxe:ASPxGridView ID="gridJournalVouchar" runat="server" width="100%" AutoGenerateColumns="False"
                            ClientInstanceName="cgridJournalVouchar" Font-Size="12px"  KeyFieldName="AccountsLedger_TransactionReferenceID">
                          <clientsideevents endcallback="function(s, e) {
	  EndCall(s.cpEND);
}" />
                            <Columns>
                            
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="50px" VisibleIndex="0" SelectAllCheckboxMode="Page">
                                                <HeaderTemplate>
                                                    <dxe:ASPxCheckBox ID="cbAll"  runat="server" ClientSideEvents-CheckedChanged="onSelectAll" ClientInstanceName="cbAll" ToolTip="Select all rows"
                                                         >
                                                        <%-- <ClientSideEvents CheckedChanged="OnAllCheckedChanged" />--%>
                                                    </dxe:ASPxCheckBox>
                                                 <%--<dxe:ASPxCheckBox ID="cbPage" runat="server" ClientInstanceName="cbPage" ToolTip="Select all rows within the page"
                                                        OnInit="cbPage_Init">
                                                        <ClientSideEvents CheckedChanged="OnPageCheckedChanged" />
                                                    </dxe:ASPxCheckBox>--%>
                                                </HeaderTemplate>
                                            </dxe:GridViewCommandColumn>
                            
                             <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="AccountsLedger_TransactionReferenceID"
                                    Caption="Voucher No.">
                                  
                                </dxe:GridViewDataTextColumn>
                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="TranDate" 
                                    Caption="Voucher Date">
                                   
                                </dxe:GridViewDataTextColumn>
                                
                                 
                              
                              
                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="MainNarration" 
                                    Caption="Narration">
                                  
                                </dxe:GridViewDataTextColumn>
                                  <dxe:GridViewDataTextColumn VisibleIndex="4" CellStyle-HorizontalAlign="Right" FieldName="TotAmtDr"
                                     Caption="Amount">
                                   
                                </dxe:GridViewDataTextColumn>
                               <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="BranchName" 
                                    Caption="BranchName">
                                   
                                </dxe:GridViewDataTextColumn>
                                                            
                            </Columns>
                            <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                            <SettingsPager NumericButtonCount="30" ShowSeparators="True" Mode="ShowAllRecords"
                                PageSize="20">
                                <FirstPageButton Visible="True">
                                </FirstPageButton>
                                <LastPageButton Visible="True">
                                </LastPageButton>
                            </SettingsPager>
                            
                            <Styles>
                                <FocusedRow BackColor="#FFC080" Font-Bold="False">
                                </FocusedRow>
                                <Header BackColor="ControlLight" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center">
                                </Header>
                            </Styles>
                              <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu = "True" />
                        </dxe:ASPxGridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                         <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary" OnClick="btnPrint_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Content>
