<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_CallUserInformation" CodeBehind="CallUserInformation.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        function frmOpenNewWindow1(location, v_height, v_weight) {
            var x = (screen.availHeight - v_height) / 2;
            var y = (screen.availWidth - v_weight) / 2
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + x + ",left=" + y + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=yes,resizable=no,dependent=no'");
        }
        function CallParent(val, val1) {
            parent.callDhtmlFormsParent(val, val1);
        }
        function saveAddressString(str) {
            var sel = window.parent.document.getElementById('drpvisitplace');
            var i = 0;
            while (sel.options.length != 0) {
                sel.options[0] = null;
                i = i + 1;
            }
            var txt;
            var addOption;
            var st = str.split('||');
            addOption123(sel, "Select Address", 0)
            for (j = 0; j < st.length - 1; j++) {
                var s = st[j].split("@@@@")
                addOption123(sel, s[0], s[1])
            }
        }
        function FillValues(id) {
            parent.FillValues(id)

        }
        function saveAddressString123(str) {
            var sel = window.parent.document.getElementById('drpNextVisitPlace');
            var i = 0;
            while (sel.options.length != 0) {
                sel.options[0] = null;
                i = i + 1;
            }
            var txt;
            var addOption;
            var st = str.split('||');
            addOption123(sel, "Select Address", 0)
            for (j = 0; j < st.length - 1; j++) {
                var s = st[j].split("@@@@")
                addOption123(sel, s[0], s[1])
            }
        }
        function addOption123(selectbox, text, value) {
            var optn = document.createElement("OPTION");
            optn.text = text;
            optn.value = value;
            selectbox.options.add(optn);
        }
        function height() {
            //alert('a');
            window.frameElement.height = document.body.scrollHeight;
            window.frameElement.width = document.body.scrollWidth;
            window.parent.height();

            //alert(document.body.scrollHeight );
        }

    </script>

    <script type="text/ecmascript">
        function valueChange() {
            var obj = document.getElementById('drpRating');
            var aa = obj.value
            CallServer(aa, "");

        }
        function ReceiveServerData(rValue) {
            //alert(rValue);
            var DATA = rValue.split('~');

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="TableMain100">
        <tr>
            <td valign="top" style="width: 48%;">
                <table style="width: 100%; vertical-align: top;">
                    <tr>
                        <td>
                            <table class="TableMain100">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" CssClass="mylabel1" Width="111px">Name :</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblLeadName" runat="server" Text="Lead Name" CssClass="Mytext" Width="111px"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="Label2" runat="server" CssClass="mylabel1" Width="57px">Rating:</asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpRating" runat="Server" Width="120px" CssClass="EcoheadCon">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" CssClass="mylabel1" Width="117px">Age & Profession :</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblAge" runat="Server" Width="49px"></asp:Label><b></b><asp:Label
                                            ID="lblProfession" runat="Server" Width="78px"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" CssClass="mylabel1" Width="56px" Height="13px">Office No :</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOffice" runat="server" CssClass="EcoheadCon" Height="15px" Width="126px"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" CssClass="mylabel1" Width="114px" Height="17px">Mobile No :</asp:Label></td>
                                    <td>
                                        <asp:Label CssClass="EcoheadCon" ID="lblMobile" runat="server" Height="17px" Width="142px"></asp:Label></td>
                                    <td style="width: 72px">
                                        <asp:Label ID="Label10" runat="server" CssClass="mylabel1" Width="56px">Email Id:</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblemailid" runat="server" CssClass="EcoheadCon"></asp:Label></td>
                                </tr>
                            </table>
                            <table class="TableMain100">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" CssClass="mylabel1" Width="108px">Residential  Address:</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblRes1" runat="server" CssClass="EcoheadCon" Width="346px"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Label ID="lblRes2" runat="server" CssClass="EcoheadCon"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Label ID="lblRes3" runat="server" CssClass="EcoheadCon"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" CssClass="mylabel1" Width="90px">Phone No(Res):</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblRes" runat="server" CssClass="EcoheadCon"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" CssClass="mylabel1" Width="90px">Office Address:</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblOffice1" runat="server" CssClass="EcoheadCon"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Label ID="lblOffice2" runat="server" CssClass="EcoheadCon"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" CssClass="mylabel1" Width="90px">Fax No:</asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblFax" runat="server" CssClass="EcoheadCon"></asp:Label></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table class="TableMain100" style="border: solid 1px blue;">
                    <tr>
                        <td valign="top">
                            <iframe id="iFrmInformation" src="frmProductDetails.aspx" style="vertical-align: top; overflow: auto;"
                                width="100%" height="200px" frameborder="0" marginheight="0"
                                marginwidth="0" scrolling="yes"></iframe>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
