<%@ Page Title="Encode/Decode" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="utilitiesEncryption.aspx.cs" Inherits="ERP.OMS.Management.Others.utilitiesEncryption" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function btnRefresh() {
            document.getElementById('<%= txtFromArea.ClientID %>').value = "";
            document.getElementById('<%= txtToArea.ClientID %>').value = "";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-title clearfix">
        <h3 class="pull-left">
            <label>Encode/Decode</label>
        </h3>
    </div>
    <div class="form_main">
        <div class="clearfix row" style="padding-top: 6px">
            <div class="col-md-6">
                <asp:TextBox ID="txtFromArea" TextMode="multiline" Columns="50" Rows="10" Height="250px" runat="server" placeholder="Enter the text that you wish to encode or decode." />
            </div>
            <div class="col-md-6">
                <asp:TextBox ID="txtToArea" TextMode="multiline" Columns="50" Rows="5" Height="250px" runat="server" placeholder="Your results will appear here." Enabled="false" />
            </div>
            <div style="clear: both"></div>
            <div class="col-md-12" style="padding-top: 6px">
                <asp:LinkButton ID="btnEncode" runat="server" CssClass="btn btn-primary" OnClick="btnEncode_Click">
                    <span class="glyphicon glyphicon-arrow-right"> Encode</span>
                </asp:LinkButton>
                 <asp:LinkButton ID="btnDecode" runat="server" CssClass="btn btn-primary"  OnClick="btnDecode_Click">
                    <span class="glyphicon glyphicon-arrow-left"> Decode</span>
                </asp:LinkButton>
                <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn btn-primary"  OnClientClick="btnRefresh();">
                     <span aria-hidden="true" class="glyphicon glyphicon-refresh"> Clear</span>
                </asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
