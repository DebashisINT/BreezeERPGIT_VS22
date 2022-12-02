<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_frmContactMissingData" CodeBehind="frmContactMissingData.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td align="center">
                    <div>
                        <b>
                            <asp:Label ID="lbl_id" runat="server" Text="Created By : "> </asp:Label>
                            <%=createUser%><br />
                            <asp:Label ID="Label1" runat="server" Text="Create Date/Time : "> </asp:Label>
                            <%=createDate%>
                            <br />
                            <asp:Label ID="Label2" runat="server" Text="Modified By : "> </asp:Label>
                            <%=modifyuser%>
                            <br />
                            <asp:Label ID="Label3" runat="server" Text="Modify Date/Time: "> </asp:Label>
                            <%=modifydate%> </b>

                    </div>
                    <div id="display" runat="server">
                    </div>

                </td>
            </tr>

        </table>



    </div>
</asp:Content>
