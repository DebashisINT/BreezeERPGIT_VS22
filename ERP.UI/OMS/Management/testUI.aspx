<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="testUI.aspx.cs" Inherits="ERP.OMS.Management.testUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function showMsg() {
            //jAlert('Select Duration');
            showalertTable("data", "Table type Alert");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>
                <span id="HeaderName"> Add Money Receipt </span>
            </h3>
        </div>
        <div id="divcross" class="crossBtn"><a href="#"><i class="fa fa-times"></i></a></div>
    </div>
    <div class="form_main">
        <div></div>
        <button type="button" class="btn btn-success" onclick="showMsg()">Alert</button>
    </div>
</asp:Content>
