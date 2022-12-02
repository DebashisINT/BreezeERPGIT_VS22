<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Management_SendErrorReport" CodeBehind="SendErrorReport.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function Page_Load()///Call Into Page Load
        {
            Hide('TabGrid');
            Hide('Tab_DisplayNoAction');
            height();
        }
        function height() {
            if (document.body.scrollHeight >= 350) {
                window.frameElement.height = document.body.scrollHeight;
            }
            else {
                window.frameElement.height = '350px';
            }
            window.frameElement.width = document.body.scrollwidth;
        }
        function Hide(obj) {
            document.getElementById(obj).style.display = 'none';
        }
        function Show(obj) {
            document.getElementById(obj).style.display = 'inline';
        }
        function FnFetchSettlement(objID, objEvent, ObjType) {
            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';

            var exchangesegmnet = "<%=Session["ExchangeSegmentID"]%>";
            var TypeSuffix = new String("<%=Session["LastSettNo"]%>");

            TypeSuffix = TypeSuffix.substring(7, 8);

            var finyear = '<%=Session["LastFinYear"]%>';
            var valyr = finyear.split('-');

            strQuery_Table = "Master_Settlements";
            strQuery_FieldName = "distinct top 10 RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)+'  ' + REPLACE(CONVERT(VARCHAR(9), settlements_StartDateTime, 6), ' ', '-') AS [DD-Mon-YY],RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)";
            strQuery_WhereClause = " settlements_exchangesegmentid='<%=Session["ExchangeSegmentID"]%>' and  ((RTRIM(settlements_Number)+RTRIM(settlements_TypeSuffix)) like (\'%RequestLetter%') and ((settlements_StartDateTime between '" + valyr[0] + "-04-01 00:00:00' and '" + valyr[1] + "-03-31 11:59:59') or (Settlements_FundsPayin between '" + valyr[0] + "-04-01 00:00:00' and '" + valyr[1] + "-03-31 11:59:59'))) ";

        if (ObjType == "TransferTo") {
            if (exchangesegmnet == "1") {
                if (TypeSuffix == "W")
                    strQuery_WhereClause = " settlements_TypeSuffix='X' and " + strQuery_WhereClause;
                else
                    strQuery_WhereClause = " settlements_TypeSuffix='A' and " + strQuery_WhereClause;
            }
            if (exchangesegmnet == "4") {
                if (TypeSuffix == "Z")
                    strQuery_WhereClause = " settlements_TypeSuffix='Y' and " + strQuery_WhereClause;
                else
                    strQuery_WhereClause = " settlements_TypeSuffix='D' and " + strQuery_WhereClause;
            }

        }

        CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
        ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery));
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
    function RecordDisplay(obj) {

        if (obj == "1") {
            alert('No Record Found');
            Hide('TabGrid');
            Hide('Tab_DisplayNoAction');
        }
        else if (obj == "3") {
            alert('Please Select Settlement Number !! ');
            Hide('TabGrid');
            Hide('Tab_DisplayNoAction');
        }
        else if (obj == "4") {
            Hide('TabGrid');
            Hide('Tab_DisplayNoAction');
            alert('Generate Successfully !!');
        }
        else if (obj == "5") {
            Hide('TabGrid');
            Show('Tab_DisplayNoAction');
            alert('Generate Successfully !!');
        }
        else {
            Show('TabGrid');
            Hide('Tab_DisplayNoAction');
        }

        height();

    }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" colspan="0" style="text-align: center; height: 20px;">
                    <strong><span id="SpanHeader" style="color: #000099">Error Report</span></strong></td>
            </tr>
        </table>
        <table class="TableMain100">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <table border="10" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">To Email Id:
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:TextBox ID="txtToEmailId" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtToEmailId" runat="server" ErrorMessage="Enter a valid Email Id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Style="color: #FF0000"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="gridcellleft" bgcolor="#B7CEEC"></td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC">
                                            <asp:Button ID="btnSendMail" runat="server" Text="Send Mail" CssClass="btnUpdate" OnClick="btnSendMail_Click" /></td>
                                        <td class="gridcellleft" bgcolor="#B7CEEC"></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
