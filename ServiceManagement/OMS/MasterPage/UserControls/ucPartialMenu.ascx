<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPartialMenu.ascx.cs" Inherits="ServiceManagement.OMS.MasterPage.UserControls.ucPartialMenu" %>

<%--<%@ OutputCache Duration="120" VaryByParam="None" VaryByCustom="userid" %>--%>

 



   <nav class="sidenav">
                    <% ServiceManagement.OMS.MVCUtility.RenderAction("Common", "_PartialMenu", new { }); %>
                   
</nav>