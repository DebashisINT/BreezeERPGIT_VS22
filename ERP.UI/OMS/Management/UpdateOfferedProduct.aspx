<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.management_UpdateOfferedProduct" CodeBehind="UpdateOfferedProduct.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function call_ajax(obj1, obj2, obj3) {
            var obj6 = document.getElementById("DDLProductype");
            if (obj6.value == 'HLO' || obj6.value == 'LAP' || obj6.value == 'PLO' || obj6.value == 'TLO' || obj6.value == 'BLO' || obj6.value == 'ELO' || obj6.value == 'ALO' || obj6.value == 'SLO' || obj6.value == 'LAS' || obj6.value == 'CRD') {
                ajax_showOptions(obj1, obj2, obj3, obj6.value);
            }
            else {
                var obj4 = document.getElementById("TxtCompName_hidden");
                var obj5 = obj4.value;
                //alert(obj5);
                ajax_showOptions(obj1, obj2, obj3, obj5);
            }
        }
        function call_ajax1(obj1, obj2, obj3) {
            var set_value
            var obj4 = document.getElementById("DDLProductype")
            ajax_showOptions(obj1, obj2, obj3, obj4.value)

        }
        FieldName = 'BtnCancel';
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr class="EHEADER">
                <td class="Ecoheadtxt" colspan="8" style="text-align: center">
                    <b>Update Product</b>
                </td>
            </tr>
            <tr>
                <td class="gridcellleft"></td>
                <td class="gridcellleft"></td>
                <td class="gridcellleft"></td>
                <td class="gridcellleft"></td>
                <td class="gridcellleft"></td>
                <td class="gridcellleft">
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label></td>
                <td class="gridcellleft"></td>
                <td class="gridcellleft">
                    <asp:Label ID="lblError1" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td class="gridcellleft">Category</td>
                <td class="gridcellleft">
                    <asp:DropDownList ID="DDLProductype" runat="server" Enabled="false">
                        <asp:ListItem Text="Broking And DP Account" Value="Broking And DP Account"></asp:ListItem>
                        <asp:ListItem Text="Mutual Fund" Value="Mutual Fund"></asp:ListItem>
                        <asp:ListItem Text="Insurance-Life" Value="Insurance-Life"></asp:ListItem>
                        <asp:ListItem Text="Insurance-General" Value="Insurance-General"></asp:ListItem>
                        <asp:ListItem Text="Relationship Partner" Value="Relationship Partner"></asp:ListItem>
                        <asp:ListItem Text="Sub Broker" Value="Sub Broker"></asp:ListItem>
                        <asp:ListItem Text="Housing Loan" Value="HLO"></asp:ListItem>
                        <asp:ListItem Text="Loan Against Property" Value="LAP"></asp:ListItem>
                        <asp:ListItem Text="Personal Loan" Value="PLO"></asp:ListItem>
                        <asp:ListItem Text="Travel Loan" Value="TLO"></asp:ListItem>
                        <asp:ListItem Text="Business Loan" Value="BLO"></asp:ListItem>
                        <asp:ListItem Text="Education Loan" Value="ELO"></asp:ListItem>
                        <asp:ListItem Text="Auto Loan" Value="ALO"></asp:ListItem>
                        <asp:ListItem Text="SME Loan" Value="SLO"></asp:ListItem>
                        <asp:ListItem Text="Loan Against Securities" Value="LAS"></asp:ListItem>
                        <asp:ListItem Text="Credit Cards" Value="CRD"></asp:ListItem>
                    </asp:DropDownList></td>
                <td class="gridcellleft">
                    <asp:Label ID="lblComp" runat="server" Text="Company"></asp:Label></td>
                <td class="gridcellleft">
                    <asp:TextBox ID="TxtCompName" runat="server" Enabled="false"></asp:TextBox></td>
                <td class="gridcellleft">
                    <asp:Label ID="LblProduct" runat="server" Text="Product"></asp:Label></td>
                <td class="gridcellleft">
                    <asp:TextBox ID="TxtProduct" runat="server"></asp:TextBox></td>
                <td class="gridcellleft">
                    <asp:Label ID="Label1" runat="server" Text="Amount"></asp:Label></td>
                <td class="gridcellleft">
                    <asp:TextBox ID="TxtAmount" runat="server"></asp:TextBox></td>

            </tr>
            <tr>
                <td class="gridcellright" colspan="8">&nbsp;<asp:Button ID="BtnSave" runat="server" CssClass="btnUpdate" Text="Save" OnClick="BtnSave_Click" /><asp:Button ID="BtnCancel" runat="server" CssClass="btnUpdate" Text="Cancel" OnClick="BtnCancel_Click" />
                </td>
                <asp:TextBox ID="TxtCompName_hidden" runat="server" BackColor="Transparent" BorderColor="Transparent" BorderStyle="None" ForeColor="#DDECFE"></asp:TextBox><asp:TextBox ID="TxtProduct_hidden" runat="server" BackColor="Transparent" BorderColor="Transparent" BorderStyle="None" ForeColor="#DDECFE"></asp:TextBox>
            </tr>
        </table>
    </div>
</asp:Content>
