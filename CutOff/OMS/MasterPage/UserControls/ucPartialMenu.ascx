<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPartialMenu.ascx.cs" Inherits="CutOff.OMS.MasterPage.UserControls.ucPartialMenu" %>

   <nav class="sidenav">
                    <% CutOff.OMS.MVCUtility.RenderAction("Common", "_PartialMenu", new { }); %>
                    <div class="text-center pwred"> Powered by BreezeERP  </div>
</nav>
