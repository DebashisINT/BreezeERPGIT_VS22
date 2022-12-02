<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_chargemaster" CodeBehind="chargemaster.aspx.cs" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!--

    

    ___________________These files are for List Items__________________________

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>

    ___________________________________________________________________________-->

    <script language="javascript" type="text/javascript">
        function OnTypeChanged() {
            var text = ddltype.GetText().toString();
            if (text == 'Brokerage')
                document.getElementById("txtcode").value = 'GRPBR';
            else
                if (text == 'Charges')
                    document.getElementById("txtcode").value = 'GRPCH';
                else
                    document.getElementById("txtcode").value = 'GRPDP';
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="TableMain100">
        <tr>
            <td class="gridcellcenter">
                <table style="border: solid 1px white; margin: 0 auto">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td class="gridcellleft">
                                        <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Type" Width="58px">
                                        </dxe:ASPxLabel>
                                    </td>
                                    <td class="gridcellleft">
                                        <dxe:ASPxComboBox runat="server" Width="225px" ID="ddltype" ClientInstanceName="ddltype" EnableSynchronization="False" EnableIncrementalFiltering="True"
                                            ValueType="System.String" Height="22px">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnTypeChanged(s); }"></ClientSideEvents>
                                            <ValidationSettings Display="Dynamic" ValidationGroup="a">
                                                <RequiredField IsRequired="True"></RequiredField>
                                            </ValidationSettings>
                                        </dxe:ASPxComboBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddltype"
                                            Display="Dynamic" ErrorMessage="Required!." ValidationGroup="a"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td class="gridcellleft">
                                        <dxe:ASPxLabel ID="ASPxLabel5" runat="server" Text="Code">
                                        </dxe:ASPxLabel>
                                    </td>
                                    <td class="Ecoheadtxt" style="text-align: left;">&nbsp;
                                            <asp:TextBox ID="txtcode" runat="server" MaxLength="10" Height="21px" Width="225px" ValidationGroup="a"></asp:TextBox>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="gridcellleft">
                                        <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text="Name">
                                        </dxe:ASPxLabel>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:TextBox ID="txtname" runat="server" ValidationGroup="a" Height="21px" Width="225px"></asp:TextBox>

                                    </td>
                                </tr>
                                <tr>
                                    <td class="gridcellleft"></td>
                                    <td class="gridcellleft">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtname"
                                            ErrorMessage="Required!." Display="Dynamic" ValidationGroup="a"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td class="gridcellleft">
                                        <dxe:ASPxLabel ID="ASPxLabel6" runat="server" Text="Is Default Group" Height="17px" Width="99px">
                                        </dxe:ASPxLabel>
                                    </td>
                                    <td class="gridcellleft">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="rdno" runat="server" Checked="True" GroupName="g1" />No</td>
                                                <td>
                                                    <asp:RadioButton ID="rdyes" runat="server" GroupName="g1" />Yes</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td style="width: 238px">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="4" class="gridcellcenter">
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdReferenceBy" runat="server" />
                                                                <asp:Button ID="btnSave" CssClass="btnUpdate" runat="server" Text="Save" OnClick="btnSave_Click" ValidationGroup="a" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

