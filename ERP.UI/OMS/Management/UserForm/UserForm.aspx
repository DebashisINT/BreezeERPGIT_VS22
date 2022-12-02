<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="UserForm.aspx.cs" Inherits="ERP.OMS.Management.UserForm.UserForm" %>
<%@ Register Src="~/OMS/Management/UserForm/UserControl/CustomerSelection.ascx" TagPrefix="uc1" TagName="ucCustomerSelection" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
    .newlineClas {
            height: 15px;
        }

     .col-md-3 > label, .col-md-3 > span {
            margin-top: auto; 
        }
</style>
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../Activities/JS/SearchPopup.js"></script>
    <script src="JS/UserForm.js?v=0.03"></script>

    <div class="panel-title clearfix" id="myDiv">
        <h3 class="pull-left">
            <asp:Label ID="titleHeader" runat="server" Text="Label"></asp:Label>
        </h3>

        <div id="divcross" runat="server" class="crossBtn"><a href="#" onclick="onBack()"><i class="fa fa-times"></i></a></div>
    </div>

    <div class="form_main">

    <asp:Panel ID="HeaderControlDetails" CssClass="row" runat="server"></asp:Panel>


    <div class="clear" />
    <input type="button" onclick="onSaveClick()" value="Save" class="btn btn-sm btn-primary mt-5"/>

    </div>

    <div id="HeaderJson" runat="server" style="display: none" />
    <asp:HiddenField ID="hdModuleName" runat="server" />
    <asp:HiddenField ID="hdtagId" runat="server" />
</asp:Content>
