<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frmAddForms" CodeBehind="frmAddForms.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=btnDocumentDiscard.ClientID %>").click(function () {
                var url = '<%= Page.ResolveUrl("~/OMS/Management/ToolsUtilities/frmShowForm.aspx")%>';
                window.location.href = url;
                return false;
            });
        });
        $(document).ready(function () {            
            $("#<%=btnAddForm.ClientID %>").click(function () {
                if ($("#<%=txtName.ClientID %>").val() == "") {
                    alert("Please Enter File Name.");
                    $("#<%=txtName.ClientID %>").focus();
                    return false;
                }
                if ($("#<%=txtfilename.ClientID %>").val() == "") {
                    alert("Please Upload a File.");
                    $("#<%=txtfilename.ClientID %>").focus();
                    return false;
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Forms & Notices&nbsp;
            <asp:Label ID="lblP" runat="server"
                Text="Label"></asp:Label></h3>

        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%-- <tr>
            <td class="EHEADER" style="text-align: center;">
                <strong><span style="color: #000099">Forms & Notices
                        <asp:Label ID="lblP" runat="server"
                            Text="Label"></asp:Label></span></strong>
            </td>
        </tr>--%>
            <tr>
                <td style="vertical-align: top; padding-left: 30px;">
                    <div class="crossBtn"><a href="frmShowForm.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="500px" style="border: solid 1px white;">
                        <tr>
                            <td class="gridcellright">File Name</td>
                            <td style="height: 24px">:</td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtName" runat="server" Width="350px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="gridcellright">Form Description</td>
                            <td>:</td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtFile" runat="Server" Width="350px" Height="75px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="gridcellright">Upload File: </td>
                            <td>:</td>
                            <td class="gridcellleft">
                                <input type="file" runat="Server" id="txtfilename" name="txtfilename" /></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <br />
                            </td>
                        </tr>
                        <%--  <tr>
                <td valign="top">
                    Keep Personal</td><td valign="top">:</td>
                
                <td class="gridcellleft">
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                        <asp:ListItem Selected="True" Value="0" Text="No"></asp:ListItem>
                    </asp:RadioButtonList></td>
            </tr>--%>
                        <tr>
                            <td colspan="3" align="Center">
                                <asp:Button ID="btnAddForm" runat="Server" Text="Add" Width="100px" CssClass="btnUpdate btn btn-primary" OnClick="btnAddForm_Click" />
                                <asp:Button ID="btnDocumentDiscard" runat="Server" Text="Discard" Width="100px" CssClass="btnUpdate btn btn-danger"  /><%--OnClick="btnDocumentDiscard_Click"--%>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
