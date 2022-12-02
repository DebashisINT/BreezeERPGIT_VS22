<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="StockReval.aspx.cs" Inherits="ERP.OMS.Management.Activities.StockReval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function doReval()
        {
            $.ajax({
                type: "POST",
                url: "StockReval.aspx/doReval",
                //data: "{'ProductName':'" + ProductName + "'}",
                //data: JSON.stringify(otherdet),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    console.log(msg.d);
                    downloadObjectAsJson(msg.d, "Invoice_JSON");
                    var json = JSON.stringify(msg.d);
                    console.log(json);
                    // WriteToFile(json);
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <button type="button" id="btnReval" class="btn btn-primary" onclick="doReval()">Reval</button>
</asp:Content>
