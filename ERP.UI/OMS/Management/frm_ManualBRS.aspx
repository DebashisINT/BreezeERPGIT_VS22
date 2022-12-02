<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frm_ManualBRS" CodeBehind="frm_ManualBRS.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <script type="text/javascript" src="/assests/js/init.js"></script>
    <script type="text/javascript" src="/assests/js/ajax-dynamic-list_wofolder.js"></script>
    <script type="text/javascript" src="/assests/js/jquery.meio.mask.js"></script>
    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>
    
    <style type="text/css">
        /* Big box with list of options */
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 32767;
        }

            #ajax_listOfOptions div { /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv { /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected { /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 3000;
        }

        form {
            display: inline;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

       $(".water").each(function () {
           if ($(this).val() == this.title) {
               $(this).addClass("opaque");
           }
       });

       $(".water").focus(function () {
           if ($(this).val() == this.title) {
               $(this).val("");
               $(this).removeClass("opaque");
           }
       });

       $(".water").blur(function () {
           if ($.trim($(this).val()) == "") {
               $(this).val(this.title);
               $(this).addClass("opaque");
           }
           else {
               $(this).removeClass("opaque");
           }
       });
   });

               </script>
    <style type="text/css">
        .demoheading {
            padding-bottom: 20px;
            color: #5377A9;
            font-family: Arial, Sans-Serif;
            font-weight: bold;
            font-size: 1.5em;
        }

        .water {
            font-family: Tahoma,Arial, Verdana, sans-serif;
            font-size: 100%;
        }

        .opaque {
            color: Gray;
        }
    </style>
    <script type="text/javascript">
        (function ($) {
       // call setMask function on the document.ready event
       $(function () {
           $('input:text').setMask();
       }
     );
   })(jQuery);
               </script>
    <script language="javascript" type="text/javascript">
        function height() {

       if (document.body.scrollHeight >= 300)
           window.frameElement.height = document.body.scrollHeight;
       else
           window.frameElement.height = '300px';
       window.frameElement.Width = document.body.scrollWidth;
   }
               function Disable(obj) {
                   if (obj == 'P') {
                       document.getElementById("FirstPage").style.display = 'none';
                       document.getElementById("PreviousPage").style.display = 'none';
                       document.getElementById("NextPage").style.display = 'inline';
                       document.getElementById("LastPage").style.display = 'inline';
                   }
                   else if (obj == 'O') {
                       document.getElementById("NextPage").style.display = 'none';
                       document.getElementById("LastPage").style.display = 'none';
                       document.getElementById("FirstPage").style.display = 'none';
                       document.getElementById("PreviousPage").style.display = 'none';
                   }
                   else if (obj == 'Nc') {
                       document.getElementById("NextPage").style.display = 'inline';
                       document.getElementById("LastPage").style.display = 'inline';
                       document.getElementById("FirstPage").style.display = 'inline';
                       document.getElementById("PreviousPage").style.display = 'inline';
                   }
                   else {
                       document.getElementById("NextPage").style.display = 'none';
                       document.getElementById("LastPage").style.display = 'none';
                       document.getElementById("FirstPage").style.display = 'inline';
                       document.getElementById("PreviousPage").style.display = 'inline';
                   }

               }
               function ShowBankName(obj1, obj2, obj3) {
                   ajax_showOptions(obj1, obj2, obj3);
               }
               FieldName = 'lblFieldName';
               function SearchVisible(obj) {
                   if (obj == 'N') {

                       document.getElementById("trsearch").style.display = 'none';
                   }
                   else {

                       document.getElementById("trsearch").style.display = 'inline';

                   }
               }
               function setvalue(obj1, obj2) {
                   //    alert('Gotfocus');
                   var a = parseInt(obj2) - 1;
                   var dtTo1 = Date.parse(document.getElementById(obj1 + a + '_I').value);
                   var dtfrom = Date.parse(document.getElementById("lbl1" + a).value);
                   var test = document.getElementById(obj1 + a + '_I').value.split('-');
                   var DateTest = new Date(test[2], test[1] - 1, test[0], 00, 00, 00);
                   if (DateTest < dtfrom) {
                       alert('Value Date can not be less than Transaction Date');
                       document.getElementById(obj1 + a).focus();
                       document.getElementById(obj1 + a + '_I').select();

                   }
                   else {

                       document.getElementById(obj1 + obj2 + '_I').value = document.getElementById(obj1 + a + '_I').value;
                       var lmonth = document.getElementById(obj1 + a + '_I').value.split('-');
                       document.getElementById(obj1 + obj2 + '_I').SetDate(new Date(lmonth[2], lmonth[1], lmonth[0], 00, 00, 00));
                       document.getElementById(obj1 + obj2).focus();
                       document.getElementById(obj1 + obj2 + '_I').select();
                   }

               }

               </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="TableMain100">
        <tr class="EHEADER">
            <td colspan="2" align="center">MANUAL BRS</td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" CssClass="mylabel1" runat="server" Text="Select Bank :" Width="77px" /></td>
                        <td colspan="3">
                            <asp:TextBox ID="txtBankName" runat="server" Font-Size="12px" Width="257px" Height="16px"></asp:TextBox>
                            <asp:HiddenField ID="txtBankName_hidden" runat="server"></asp:HiddenField>
                            <asp:Label ID="lblFieldName" runat="server" Text="Label" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <table style="border: solid 1px white">
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="RdUnCleared" runat="server" Text="" Checked="true" GroupName="R" /></td>
                                    <td class="mylabel1">Uncleared</td>
                                    <td>
                                        <asp:RadioButton ID="RdCleared" runat="server" Text="" GroupName="R" />
                                    </td>
                                    <td class="mylabel1">Cleared
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="RdAll" runat="server" Text="" GroupName="R" />
                                    </td>
                                    <td class="mylabel1">All
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table style="border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid">
                                <tr>
                                    <td class="rt">
                                        <asp:Label ID="lblFromdate" CssClass="mylabel1" runat="server" Text="From:" Width="28px"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="FromDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                            UseMaskBehavior="True" Width="130px">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td class="rt">
                                        <asp:Label ID="lblToDate" CssClass="mylabel1" runat="server" Text="To:"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="DateTo" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                            UseMaskBehavior="True" Width="118px">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnShow" runat="server" CssClass="btnUpdate" OnClick="btnShow_Click1"
                                            Text="Show" Height="22px" />
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
                <table>
                    <tr id="trhypertext" runat="server" visible="false">
                        <td>
                            <asp:LinkButton ID="lnAllRecords" runat="server" Font-Underline="True" ForeColor="#8080FF"
                                NavigateUrl="javascript:void(0)" CssClass="myhypertext" OnClick="lnAllRecords_Click">All Records</asp:LinkButton>
                        </td>
                        <td>
                            <asp:HyperLink ID="hpFilterRecords" runat="server" Font-Underline="True" ForeColor="#8080FF"
                                NavigateUrl="javascript:void(0)" CssClass="myhypertext" onclick="SearchVisible('Y')">Show Filter</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trsearch" visible="false">
            <td colspan="2">
                <table>
                    <tr>
                        <td valign="middle">
                            <dxe:ASPxDateEdit ID="AspTranDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                UseMaskBehavior="True" Width="90px" NullText="Tran Date">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td valign="middle">
                            <asp:TextBox ID="txtVoucherNo" runat="server" Width="74px" CssClass="water" Text="Voucher No"
                                ToolTip="Voucher No"></asp:TextBox>
                        </td>
                        <td valign="middle">
                            <dxe:ASPxDateEdit ID="AspInsDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                UseMaskBehavior="True" Width="110px" NullText="Ins Date">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInsNo" runat="server" CssClass="water" Text="Instrument No" ToolTip="Instrument No"
                                Width="74px"></asp:TextBox>
                        </td>
                        <td valign="middle">
                            <table>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtAccName" runat="server" Width="249px" CssClass="water" Text="Main Account"
                                            ToolTip="Main Account"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txtSubName" runat="server" Width="50px" CssClass="water" Text="SubAccount"
                                            ToolTip="SubAccount"></asp:TextBox></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="middle">
                            <asp:TextBox ID="txtPayAmt" runat="server" Width="78px" CssClass="water" Text="Payment Amount"
                                ToolTip="Payment Amount"></asp:TextBox>
                        </td>
                        <td valign="middle">
                            <asp:TextBox ID="txtReptAmt" runat="server" Width="80px" CssClass="water" Text="Receipt Amount"
                                ToolTip="Receipt Amount"></asp:TextBox>
                        </td>
                        <td valign="middle">
                            <dxe:ASPxDateEdit ID="AspValueDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy"
                                UseMaskBehavior="True" Width="84px" NullText="Value Date">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </td>
                        <td valign="middle">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" Height="23px" Width="53px"
                                CssClass="btnUpdate" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2" id="tdMainContent" runat="server" visible="false">
                <div class="grid_scrollNSEFO">
                    <asp:Table ID="tblDetails" runat="server" BorderStyle="Solid" BorderWidth="1px" CellPadding="2"
                        BorderColor="blue" CellSpacing="0" GridLines="Both">
                    </asp:Table>
                    &nbsp;
                </div>
            </td>
        </tr>
        <tr align="center">
            <td id="tdTable" runat="server" visible="false" colspan="4">
                <table>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="lbltotal" runat="server" Text="Label" ForeColor="blue"></asp:Label>
                            <asp:ImageButton ID="FirstPage" runat="server" CommandName="First" ImageUrl="~/images/pFirst.png"
                                OnClick="FirstPage_Click" />
                            <asp:ImageButton ID="PreviousPage" runat="server" CommandName="Prev" ImageUrl="~/images/pPrev.png"
                                OnClick="PreviousPage_Click" />
                            <asp:ImageButton ID="NextPage" runat="server" CommandName="Next" ImageUrl="~/images/pNext.png"
                                OnClick="NextPage_Click" />
                            <asp:ImageButton ID="LastPage" runat="server" CommandName="Last" ImageUrl="~/images/pLast.png"
                                OnClick="LastPage_Click" />
                            <asp:Label ID="lblPageNo" runat="server" Text="Label" Visible="false"></asp:Label>
                            <asp:HiddenField ID="hdCurrentPage" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hdTotalPages" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hdPageIndex" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hdPageSize" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hdActivity" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnUpdate" runat="server" Text="Save" OnClick="btnUpdate_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:SqlDataSource ID="SqlDetails" runat="server" ConflictDetection="CompareAllValues"
                   SelectCommand="select(convert(varchar(11),a.cashbank_transactionDate,113))as cashbank_transactionDate,a.cashbank_vouchernumber,(convert(varchar(11),b.cashbankdetail_instrumentdate,113))as cashbankdetail_instrumentdate,b.cashbankdetail_instrumentnumber,c.mainaccount_name +'['+ c.MAinaccount_accountcode +']' as AccountCode,b.cashbankdetail_paymentamount,b.cashbankdetail_receiptamount,b.cashbankdetail_bankvaluedate,b.cashbankdetail_id from Trans_CashBankVouchers as a inner join Trans_CashBankDetail as b on a.cashbank_id=b.cashbankdetail_voucherid inner join master_mainaccount as c on b.cashbankdetail_mainaccountid=c.MAinaccount_accountcode"></asp:SqlDataSource>
            </td>
        </tr>
    </table>
</asp:Content>
