<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="frmChangeVendorPassword.aspx.cs" Inherits="ERP.OMS.Management.ToolsUtilities.frmChangeVendorPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function BtnCancel_Click() {
            var newP = document.getElementById("TxtNewPassword");
            var confirmP = document.getElementById("TxtConfirmPassword");
            newP.value = "";
            confirmP.value = "";
        }

    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Change Password</h3>
            <div class="crossBtn"><a href="../master/Root_VendorUser.aspx"><i class="fa fa-times"></i></a></div>
        </div>

    </div>
    <div class="form_main">
        <table class="TableMain100" style="text-align: left;">

            <tr>
                <td style="text-align: left;">
                    <br />
                    <asp:Panel ID="panel1" BorderColor="white" BorderWidth="1px" runat="server" Width="400px">
                        <table>                            
                            <tr>
                                <td class="Ecoheadtxt" width="200px">New Password :&nbsp;<em style="color: red">*</em> 
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TxtNewPassword" runat="server" CssClass="EcoheadCon" TextMode="Password" MaxLength="50"
                                        Width="160px"></asp:TextBox>
                                    
                                </td>
                                <td><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtNewPassword"
                                        ErrorMessage="" SetFocusOnError="True"
                                        ValidationGroup="a"  CssClass="spl fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td class="Ecoheadtxt">Confirm Password :&nbsp;<em style="color: red">*</em> 
                                </td>
                                <td style="text-align: left">
                                    <asp:TextBox ID="TxtConfirmPassword" runat="server" CssClass="EcoheadCon" TextMode="Password"
                                        Width="160px" MaxLength="50"></asp:TextBox>
                                     
                                </td>
                                <td> <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtConfirmPassword"
                                         ErrorMessage=""  SetFocusOnError="True"
                                        ValidationGroup="a"   CssClass="spl fa fa-exclamation-circle iconRed" ToolTip="Mandatory"></asp:RequiredFieldValidator>


                                      <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="TxtNewPassword"
                                        ControlToValidate="TxtConfirmPassword" ErrorMessage=""
                                        SetFocusOnError="True" ValidationGroup="a" Display="Dynamic" ToolTip="New password and confirm password must be same"  CssClass="spl fa fa-exclamation-circle iconRed" ></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="BtnSave" runat="server" Text="Update" class="btn btn-primary" OnClick="BtnSave_Click"
                                        ValidationGroup="a" />
                                    <input id="BtnCancel" type="button" style="display:none;" value="Cancel" class="btn btn-danger" onclick="BtnCancel_Click()" /></td>
                                 <td></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                   
                                    
                                  
                                  
                                   <%-- <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                                        ShowSummary="False" ValidationGroup="a" />--%>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>