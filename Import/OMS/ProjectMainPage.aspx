<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master"  AutoEventWireup="true" 
    CodeBehind="ProjectMainPage.aspx.cs" Inherits="ImportModule.ProjectMainPage" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


  <ul>  

    <li style="font:50px"><a style="font-size: small;"" href="/Import/Document-Template.aspx">Document Template</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Import/Document-Attachmentlist.aspx">Document Attachment</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Import/Purchaseorderlist-Import.aspx">Purchase order</a></li>

    <li style="font:50px"><a style="font-size: small;"" href="/Import/PurchaseOrder-Acceptancelist.aspx">Purchase Order Acceptance</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Import/PurchaseorderBillofladingList.aspx">Bill Of Lading</a></li>


    <li style="font:50px"><a style="font-size: small;"" href="/Import/PurchaseInvoiceList-Import.aspx">Purchase Invoice</a></li>
    <li style="font:50px"><a style="font-size: small;"" href="/Import/PurchaseBillOfEntryList-Import.aspx">Bill Of Entry</a></li>

     <li style="font:50px"><a style="font-size: small;"" href="/Import/Import-LCOpeninglist.aspx">L/C Opening</a></li>
     <li style="font:50px"><a style="font-size: small;"" href="/Import/GoodsReceivedNoteList_Import.aspx">Stock Receipt</a></li>
     <li style="font:50px"><a style="font-size: small;"" href="/Import/HighSeaImportTransitList.aspx">High Sea Import (Transit)</a></li>

  </ul>



</asp:Content>
