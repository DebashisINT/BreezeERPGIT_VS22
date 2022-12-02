<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPartialMenu.ascx.cs" Inherits="ERP.OMS.MasterPage.UserControls.ucPartialMenu" %>

<%--<%@ OutputCache Duration="120" VaryByParam="None" VaryByCustom="userid" %>--%>

 



   <nav class="sidenav">
                    <% ERP.OMS.MVCUtility.RenderAction("Common", "_PartialMenu", new { }); %>
                   
</nav>