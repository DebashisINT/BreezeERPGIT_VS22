<%@ Page Title="Cash Bank Voucher Printing" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_CashBankReport" CodeBehind="CashBankReport.aspx.cs" %>

<%--<%@ Register Assembly="CrystalDecisions.Web, Version=10.2.3600.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript" src="../../CentralData/JSScript/GenericJScript.js"></script>
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script src="../../assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript" src="/assests/js/ajaxList.js"></script>
    <script type="text/javascript">
        function DateChangeForFrom() {
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = dtDate.GetDate().getMonth() + 1;
            var DayDate = dtDate.GetDate().getDate();
            var YearDate = dtDate.GetDate().getYear();
            if (YearDate >= objsession[0]) {
                if (MonthDate < 4 && YearDate == objsession[0]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                    dtDate.SetDate(new Date(datePost));
                }
                else if (MonthDate > 3 && YearDate == objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                    dtDate.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                    dtDate.SetDate(new Date(datePost));
                }
            }
            else {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (4 + '-' + 1 + '-' + objsession[0]);
                dtDate.SetDate(new Date(datePost));
            }
        }
        function DateChangeForTo() {
            var sessionVal = "<%=Session["LastFinYear"]%>";
            var objsession = sessionVal.split('-');
            var MonthDate = dtToDate.GetDate().getMonth() + 1;
            var DayDate = dtToDate.GetDate().getDate();
            var YearDate = dtToDate.GetDate().getYear();

            if (YearDate <= objsession[1]) {
                if (MonthDate < 4 && YearDate == objsession[0]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (3 + '-' + 31 + '-' + objsession[1]);
                    dtToDate.SetDate(new Date(datePost));
                }
                else if (MonthDate > 3 && YearDate == objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (3 + '-' + 31 + '-' + objsession[1]);
                    dtToDate.SetDate(new Date(datePost));
                }
                else if (YearDate != objsession[0] && YearDate != objsession[1]) {
                    alert('Enter Date Is Outside Of Financial Year !!');
                    var datePost = (3 + '-' + 31 + '-' + objsession[1]);
                    dtToDate.SetDate(new Date(datePost));
                }
            }
            else {
                alert('Enter Date Is Outside Of Financial Year !!');
                var datePost = (3 + '-' + 31 + '-' + objsession[1]);
                dtToDate.SetDate(new Date(datePost));
            }
        }
        function ShowBankName(obj1, obj2, obj3) {
            ajax_showOptions(obj1, obj2, obj3);
        }

        $(document).ready(function () {
            debugger;
            ListBind();
            $('#txtBankName').trigger("chosen:updated");
            //SetBankNames(cntry);
        });

        function ListBind() {
            debugger;
            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }

        function GetAllCheck(obj) {
            if (obj == true) {
                document.getElementById('chkDebit').checked = true;
                document.getElementById('chkCredit').checked = true;
                document.getElementById('chkContra').checked = true;

                ForDisabled('chkDebit');
                ForDisabled('chkCredit');
                ForDisabled('chkContra');
            }
            else {

                ForEnabled('chkDebit');
                ForEnabled('chkCredit');
                ForEnabled('chkContra');
                document.getElementById('chkDebit').checked = false;
                document.getElementById('chkCredit').checked = false;
                document.getElementById('chkContra').checked = false;
            }


        }
        function ForEnabled(obj) {
            document.getElementById(obj).disabled = false;
        }
        function ForDisabled(obj) {
            document.getElementById(obj).disabled = true;
        }
        function Page_Load() {
            ForEnabled('chkAll');
            ForDisabled('chkDebit');
            ForDisabled('chkCredit');
            ForDisabled('chkContra');
            height();
        }
        //function height() {
        //    if (document.body.scrollHeight >= 500) {
        //        window.frameElement.height = document.body.scrollHeight;
        //    }
        //    else {
        //        window.frameElement.height = '500';
        //    }

        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        FieldName = 'btnReport';

        function Hide() {
            document.getElementById('tblPrint').style.display = 'none';
        }
        function Show() {
            document.getElementById('tblPrint').style.display = 'inline';
        }
        function CallBankAccount(obj1, obj2, obj3) {
            var CurrentSegment = '<%=Session["usersegid"]%>'
             if (CurrentSegment.length == 8)
                 CurrentSegment = document.getElementById("hdn_NsdlCdslExchangeSegment").value;
             var strPutSegment = " and (MainAccount_ExchangeSegment=" + CurrentSegment + " or MainAccount_ExchangeSegment=0)";
             var strQuery_Table = "(Select MainAccount_AccountCode+\'-\'+MainAccount_Name+\' [ \'+MainAccount_BankAcNumber+\' ]\' as IntegrateMainAccount,MainAccount_AccountCode as MainAccount_AccountCode,MainAccount_Name,MainAccount_BankAcNumber from Master_MainAccount where (MainAccount_BankCashType=\'Bank\' or MainAccount_BankCashType=\'Cash\') and (MainAccount_BankCompany=\'" + '<%=Session["LastCompany"] %>' + "\' Or IsNull(MainAccount_BankCompany,'')='')" + strPutSegment + ") as t1";
            var strQuery_FieldName = " Top 10 * ";
            var strQuery_WhereClause = "MainAccount_AccountCode like (\'%RequestLetter%\') or MainAccount_Name like (\'%RequestLetter%\') or MainAccount_BankAcNumber like (\'%RequestLetter%\')";
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(obj1, obj2, obj3, replaceChars(CombinedQuery), 'Main');
        }
        function replaceChars(entry) {
            out = "+"; // replace this
            add = "--"; // with this
            temp = "" + entry; // temporary holder

            while (temp.indexOf(out) > -1) {
                pos = temp.indexOf(out);
                temp = "" + (temp.substring(0, pos) + add +
                temp.substring((pos + out.length), temp.length));
            }
            return temp;

        }
        function OnAllCheckedChanged(s, e) {
            debugger;
            if (s.GetChecked())
                cgridCashBank.SelectRows();
            else
                cgridCashBank.UnselectRows();
        }
        function UnselectRow() {
            cgridCashBank.UnselectRows();

        }
        function CheckSelect() {
            var aa = cgridCashBank.GetSelectedKeysOnPage();
            alert(aa[0]);

        }

    </script>

    <%--<link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />--%>
    <style>
        #gridCashBank_DXMainTable #gridCashBank_DXHeadersRow0 td {
            background-color:none !important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cgridCashBank.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cgridCashBank.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cgridCashBank.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cgridCashBank.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
        <div class="panel-title">
            <h3>Cash Bank Voucher Printing</h3>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100 tableClass">
         <%--   <tr>
                <td class="EHEADER" colspan="2" style="text-align: center;">
                    <strong><span style="color: #000099">Cash Bank Voucher Printing</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td class="gridcellleft" style="width: 150px">Bank Name
                </td>
                <td colspan="3">
                    <%--<asp:TextBox ID="txtBankName" runat="server" Width="330px" onkeyup="CallBankAccount(this,'GenericAjaxList',event)"></asp:TextBox>--%>
                     <asp:ListBox ID="txtBankName" CssClass="chsn"   runat="server" Width="250px"   
                                        data-placeholder="Select..." ></asp:ListBox>
                    <asp:HiddenField ID="txtBankName_" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="gridcellleft" style="width: 89px">Type :
                </td>
                <td colspan="3">
                    <table width="280px" style="margin: 8px 0 2px 0;">
                        <tr>
                            <td id="TdAll">
                                <asp:CheckBox ID="chkAll" runat="server" Checked="true" onclick="GetAllCheck(this.checked);" />
                            </td>
                            <td width="30px">All
                            </td>
                            <td id="TdDebit">
                                <asp:CheckBox ID="chkDebit" runat="server" Checked="true" />
                            </td>
                            <td>Payment
                            </td>
                            <td id="TdCredit">
                                <asp:CheckBox ID="chkCredit" runat="server" Checked="true" />
                            </td>
                            <td>Receipt
                            </td>
                            <td id="TdContra">
                                <asp:CheckBox ID="chkContra" runat="server" Checked="true" />
                            </td>
                            <td>Contra
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft" style="width: 89px">Print Option:</td>
                <td colspan="3">
                    <table style="width:250px">
                        <tr>
                            <td style="width:15px">
                                <asp:RadioButton ID="rbtnSingle" runat="server" Width="" Checked="True" GroupName="a" ValidationGroup="a" />
                            </td>
                            <td>
                                <asp:Label ID="lblSingleSide" runat="server" Text="Single Copy"></asp:Label>
                            </td>
                            <td style="width:15px">
                                <asp:RadioButton ID="rbtnDouble" runat="server" Width="" GroupName="a" ValidationGroup="a" />
                            </td>
                            <td>
                                <asp:Label ID="lblDoubleSide" runat="server" Text="Double Copy"></asp:Label>
                            </td>
                            
                            <td style="display: none">
                                <asp:Label ID="lblTripleSide" runat="server" Text="Triple Copy"></asp:Label>
                            </td>
                            <td style="display: none">
                                <asp:RadioButton ID="rbtnTriple" runat="server" Width="74px" GroupName="a" ValidationGroup="a" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft" style="width: 89px">Cheque Number</td>
                
                <td style="width: 400px">
                    <label>From</label>
                    <asp:TextBox ID="txtFromCheque" runat="server" Width="250px"></asp:TextBox>
                    <label style="margin-left:0;">To</label>
                    <asp:TextBox ID="txtToCheque" runat="server" Width="250px"></asp:TextBox>
                </td>
                
                <td>
                    </td>
                <td></td>
            </tr>
            <tr>
                <td class="gridcellleft" style="width: 89px">Print Mode:</td>
                <td colspan="3">
                    <asp:DropDownList ID="ddlPrintMode" runat="server" Width="250px">
                        <asp:ListItem Value="1">Lazer Print</asp:ListItem>
                        <asp:ListItem Value="2">Dos Print</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="gridcellleft" style="width: 89px">Date
                </td>
                <td colspan="3">
                    <table>
                        <tr>
                            <td style="width: 265px">
                                <dxe:ASPxDateEdit ID="dtDate" runat="server" EditFormat="Custom" ClientInstanceName="dtDate"
                                    UseMaskBehavior="True" Width="250px">
                                    <DropDownButton Text="From ">
                                    </DropDownButton>
                                    <ClientSideEvents DateChanged="function(s,e){DateChangeForFrom();}" />
                                </dxe:ASPxDateEdit>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="dtToDate" runat="server" EditFormat="Custom" ClientInstanceName="dtToDate"
                                    UseMaskBehavior="True" Width="250px">
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
                <td class="gridcellleft" style="width: 89px">Print Option :
                </td>
                <td colspan="3">
                    <table width="347px">
                        <tr>
                            <td id="td_user">
                                <asp:CheckBox ID="user" runat="server" />
                            </td>
                            <td>Print Entered By User
                            </td>
                            <td id="Td1">
                                <asp:CheckBox ID="time" runat="server" />
                            </td>
                            <td>Print Entry Date Time
                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <table class="tableClass">
                        <tr>
                            <td style="width: 148px">
                                <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="btn btn-primary" OnClientClick="UnselectRow()" OnClick="btnShow_Click" />
                            </td>
                            <td style="width: 148px">
                                <asp:Button ID="btnReport" runat="server" Text="Print" CssClass="btn btn-primary"  OnClick="btnReport_Click" />
                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="display: none">
                    <asp:HiddenField ID="txtBankName_hidden" runat="server" />
                </td>
            </tr>
            
        </table>
        <table width="100%">
            <tr>
                <td>

                    <dxe:ASPxGridView ID="gridCashBank" runat="server" Width="100%" AutoGenerateColumns="False"
                        ClientInstanceName="cgridCashBank" Font-Size="12px" KeyFieldName="CashbankDetailsId">

                        <Columns>

                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="50px" VisibleIndex="0">
                                <HeaderTemplate>
                                    <dxe:ASPxCheckBox ID="cbAll" runat="server" ClientInstanceName="cbAll" ToolTip="Select all rows"
                                        >
                                        <ClientSideEvents CheckedChanged="OnAllCheckedChanged" />
                                    </dxe:ASPxCheckBox>

                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="TranRefID"
                                Caption="Voucher No.">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="TranDate"
                                Caption="Voucher Date">
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Narration1"
                                Caption="MainNarration">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="totalReceipt"
                                Caption="Receipt" CellStyle-HorizontalAlign="Right">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="totalPayment"
                                Caption="Payment" CellStyle-HorizontalAlign="Right">
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="BranchName"
                                Caption="BranchName">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="false" FieldName="CashbankDetailsId"
                                Caption="CashbankDetailsId">
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
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu = "True" />
                        <Styles>
                            <FocusedRow BackColor="#FFC080" Font-Bold="False">
                            </FocusedRow>
                            <Header BackColor="" Font-Bold="True" ForeColor="" HorizontalAlign="Center">
                            </Header>
                        </Styles>
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                    </dxe:ASPxGridView>

                </td>
            </tr>
        </table>
        <table id="tblPrint">
            <tr>
                <td>

                  <%--  <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
                        ReportSourceID="CrystalReportSource1" PrintMode="Pdf" BestFitPage="False" SeparatePages="False" Width="901px" />
                    <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
                        <Report FileName="CashBank.rpt">
                        </Report>
                    </CR:CrystalReportSource>--%>
                </td>
            </tr>

        </table>
        <asp:HiddenField ID="hdnPrint" runat="server" />
        <asp:HiddenField ID="hdn_NsdlCdslExchangeSegment" runat="server" />
    </div>
</asp:Content>
