<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/Erp.Master" Inherits="ERP.OMS.Management.ActivityManagement.management_activitymanagement_frmOfferedProduct_New" CodeBehind="frmOfferedProduct_New.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  

    <script language="javascript" type="text/javascript">
        function call_ajax(obj1, obj2, obj3) {
            var obj6 = document.getElementById("DDLProductype");
            if (obj6.value == 'HLO' || obj6.value == 'LAP' || obj6.value == 'PLO' || obj6.value == 'TLO' || obj6.value == 'BLO' || obj6.value == 'ELO' || obj6.value == 'ALO' || obj6.value == 'SLO' || obj6.value == 'LAS' || obj6.value == 'CRD') {
               
                ajax_showOptions(obj1, obj2, obj3, obj6.value);
            }
            else {
                var obj4 = document.getElementById("TxtComp_hidden");
                var obj5 = obj4.value;
        
             
                ajax_showOptions(obj1, obj2, obj3, obj5);
            }
        }
        function call_ajax1(obj1, obj2, obj3) {
            var set_value
            var obj4 = document.getElementById("DDLProductype")
            ajax_showOptions(obj1, obj2, obj3, obj4.value)

        }
        function height() {

            window.frameElement.height = document.body.scrollHeight;
            window.frameElement.width = document.body.scrollWidht;


        }

        FieldName = 'BtnCancel';
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="panel-heading">
        <div class="panel-title">
            <h3>Add Product</h3>
            <div class="crossBtn"><a href="frm_sales_Activity.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
          
            <tr>
                <td class="gridcellleft">
                    <table cellpadding="0px" cellspacing="0px">
                        <tr>
                            <td class="gridcellleft"></td>
                            <td class="gridcellleft"></td>
                            <td class="gridcellleft"></td>
                            <td class="gridcellleft">
                                <asp:Label ID="lblError1" runat="server" ForeColor="Red"></asp:Label></td>
                            <td class="gridcellleft"></td>
                            <td>
                                <asp:Label ID="lblError2" runat="server" ForeColor="Red"></asp:Label></td>
                            <td></td>
                            <td class="gridcellleft">
                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="mylabel1">Category</td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="DDLProductype" runat="server" Width="240px">
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
                            </tr>
                        <tr>
                            <td class="mylabel1">Company</td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="TxtComp" runat="server" Width="240px"></asp:TextBox>
                            </td>
                            </tr>
                        <tr>
                            <td class="mylabel1">Product</td>
                            <td>
                                <asp:TextBox ID="TxtProduct" runat="server" Width="240px"></asp:TextBox>
                            </td>
                            </tr>
                        <tr>
                            <td class="mylabel1">Amt.</td>
                            <td>
                                <asp:TextBox ID="TxtAmount" runat="server" Width="240px"></asp:TextBox>
                            </td>
                            <td class="mylabel1">
                                <asp:Button ID="BtnAdd" runat="server" Text="Add" OnClick="BtnAdd_Click" CssClass="btnUpdate btn btn-primary btn-xs"
                                    ValidationGroup="a" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="display: block">
                <td>
                    <asp:TextBox ID="TxtProduct_hidden" runat="server"></asp:TextBox>
                    <asp:TextBox ID="TxtComp_hidden" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr id="TRforHideInAjax">
                <td></td>
            </tr>
            <tr>
                <td class="gridcellcenter">
                    <asp:GridView CssClass="Ecoheadtxt" ID="GrdList" runat="server" CellPadding="4" ForeColor="#333333"
                        GridLines="None" BorderWidth="1px" BorderColor="#507CD1">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle Font-Bold="false" ForeColor="black" CssClass="EHEADER" BorderColor="AliceBlue"
                            BorderWidth="1px" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="gridcellcenter" style="padding-left:56px">
                    <asp:Button ID="BtnSave" runat="server" Text="Save" Enabled="False" OnClick="BtnSave_Click"
                        CssClass="btnUpdate btn btn-primary" />
                    <asp:Button ID="BtnCancel" runat="server" Text="Cancel" OnClick="BtnCancel_Click"
                        CssClass="btnUpdate btn btn-danger" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
