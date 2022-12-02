<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_mtest3" CodeBehind="mtest3.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Button2" />
                <asp:AsyncPostBackTrigger ControlID="Button3" />
            </Triggers>
            <ContentTemplate>

                <asp:Label ID="Label1" runat="server"></asp:Label><%=DateTime .Now %><br />
                <asp:Button ID="Button1" runat="server" Text="Button1" OnClick="Button1_Click" />
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        Processing...
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:Button ID="Button4" runat="server" Text="Button4" OnClick="Button1_Click" />

            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="Button2" runat="server" Text="Button" OnClick="Button1_Click" />
        <asp:Button ID="Button3" runat="server" Text="Button" OnClick="Button1_Click" />
    </div>
</asp:Content>
