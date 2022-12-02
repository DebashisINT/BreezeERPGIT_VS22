<%@ Page Title="Inventory Control Centre" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" EnableEventValidation="false"
    Inherits="ERP.OMS.Management.DailyTask.Management_DailyTask_InvControlCentre" CodeBehind="InvControlCentre.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- <%@ Register TagPrefix="uc" TagName="ucpOrdPop" Src="~/Management/DailyTask/uc_invSearchPopup.ascx" %>--%>
    <%@ Register TagPrefix="uc" TagName="TransPopup" Src="~/OMS/Management/DailyTask/InvTransactions.ascx" %>
    <%--<%@ Register Assembly="DevExpress.Web.v9.1.Export, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1" Namespace="DevExpress.Web"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>--%>

    <%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
        Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
    <%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
        Namespace="DevExpress.Web" TagPrefix="dxe" %>

    <style type="text/css">
        #Popup_IsEdit_PWST-1 {
            left: 120px !important;
            top: -155px !important;
        }

        #Popup_IsEdit_PW-1 {
            left: 56px !important;
            width: 1024px !important;
        }

        .dxpcControl {
            width: 100% !important;
        }

        #Popup_IsEdit_CLW-1 {
            width: 100% !important;
        }

        .grdCommon {
           
            font-size: 12px;
            width: 100%;
            margin-top: 5px;
            border: 1px solid #4d74a8;
            background-color: #fff;
        }

        table.openingStkTable {
            
            font-size: 12px;
            width: 100%;
            margin-top: 15px;
        }

            table.openingStkTable, table.openingStkTable th, table.openingStkTable td {
                border-collapse: collapse;
                border: #4d74a8 1px solid;
            }

                table.openingStkTable th {
                    padding: 5px 10px;
                    background-color: #d3ddeb;
                }

                table.openingStkTable td {
                    padding: 5px 10px;
                }

        .on {
            background-color: #99B2DB;
        }

        .vsplitter :nth-child(2), .hsplitter {
            display: none;
        }

        .splitter_panel {
            height: 320px !important;
        }

        .PointerSet {
            cursor: pointer;
        }
    </style>

    <script type="text/javascript" src="http://code.jquery.com/jquery-1.8.3.min.js"></script>

    <script type="text/javascript">
        var jq183 = jQuery.noConflict();
    </script>

    <script src="/assests/js/jquery.splitter-0.14.0.js" type="text/javascript"></script>

    <link href="../../css/jquery.splitter.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        jQuery(function (jQuery) {
            jq183('#widget').width(1700).height(500).split({ orientation: 'vertical', limit: 1350 });
            jq183('#foo').width(1325).split({ orientation: 'horizontal', limit: 10 });
            jq183('#a').split({ orientation: 'vertical', limit: 10 });
            jq183('#spliter2').css({ width: 800, height: 300 }).split({ orientation: 'horizontal', limit: 20 });
        });
    </script>

    <script type="text/javascript" src="/assests/js/jquery-1.3.2.min.js"></script>

    <script type="text/javascript">
        var jq132 = jQuery.noConflict();
    </script>

    <script type="text/javascript">
        jq132(document).ready(function () {

            PositionGridOnClick();
            EnterTransactionView();
            document.getElementById("hdnPopupsCheck").value = "";
        });
        function CloseTransactionPopup() {
            Trans_Popup.Hide();
        }

        function EnterTransactionView() {
            var OrderpositopnId = "";
            OrderpositopnId = document.getElementById("hdnOrderPositon").value;
            if (OrderpositopnId == "" || OrderpositopnId == null) {
                document.getElementById("dvEnterTransaction").style.display = "none";
            }
            else {
                document.getElementById("dvEnterTransaction").style.display = "block";
            }
        }
        function OpenEditPopup() {
            Trans_Popup.Show();
        }
        function PopupOpen() {

            SearchPopup.Show();
        }
        function TransactionPopupOpen() {
            var OrderpositopnId = "";
            var CheckPoint = false;
            OrderpositopnId = document.getElementById("hdnOrderPositon").value;
            document.getElementById("hdnTransactionEdit").value = "insert";

            //Condition check for open transaction popup open.  
            jq132.ajax({
                type: "POST",
                url: "InvControlCentre.aspx/TransactionPopupCondition",
                data: "{'OrderpositopnId':'" + OrderpositopnId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    CheckPoint = data.d;
                    if (CheckPoint == true) {
                        if (OrderpositopnId == "" || OrderpositopnId == null) {
                            alert("Please select a position record!")
                        }
                        else {
                            Trans_Popup.Show();
                        }
                    }
                    else {
                        alert("No more action required");
                    }
                }

            });
        }


        jq132(document).ready(function () {
            jq132(function () {
                jq132('tr').click(function () {
                    jq132(this).parents('table').find('tr').each(function (index, element) {
                        jq132(element).removeClass('on');
                    });
                    jq132(this).addClass('on');
                });
            });

            //            jq132("#grdPosition").click(function(e) {
            //                alert("click");
            //            });
        });
        function PositionGridOnClick() {
            var JStockGridData = "";
            jq132("#grdPosition").click(function (e) {
                var row = null;
                var rowId = null;
                row = jq132(e.target || e.srcElement).parent();
                rowId = row.find('td:first').andSelf().find("input:hidden").val();
                if (rowId == null || rowId === undefined) {
                    row = jq132(e.target || e.srcElement).parent().parent();
                    rowId = row.find('td:first').andSelf().find("input:hidden").val();
                }
                document.getElementById("hdnOrderPositon").value = rowId;
                generalGridbind(rowId);
                EnterTransactionView();
            });
        }
        function generalGridbind(rowId) {
            jq132.ajax({
                type: "POST",
                url: "InvControlCentre.aspx/GetStockGridData",
                data: "{'id':'" + rowId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $("#grdStock").empty();
                    if (data.d.length > 0) {
                        $("#grdStock").append("<tr>" +
                    "<th>Product Desc</th>" +
                    "<th>Stk In</th>" +
                    "<th>Stk Out</th>" +
                    "<th>Stk In Hand</th>" +
                    "<th>QntyUnt</th>" +
                    "<th>Location</th>" +
                    "</tr>");
                        for (var i = 0; i < data.d.length; i++) {
                            $("#grdStock").append("<tr> <td>" +
                        data.d[i].ProductDescription + "</td> <td>" +
                        data.d[i].Stock_In + "</td> <td>" +
                        data.d[i].Stock_Out + "</td> <td>" +
                        data.d[i].Stock_In_Hand + "</td> <td>" +
                        data.d[i].QuantityUnit + "</td> <td>" +
                        data.d[i].Location_Name + "</td>" +
                        "</tr>");
                        }
                    }
                    else {
                        jq132("#grdStock").append(
                            "<tr> <td colspan='5'> No data in the data source.</td> </tr>")
                    }
                }
            });
            jq132.ajax({
                type: "POST",
                url: "InvControlCentre.aspx/GetTransactionGridData",
                data: "{'id':'" + rowId + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    jq132("#grdTransaction").empty();
                    if (data.d.length > 0) {
                        jq132("#grdTransaction").append("<tr>" +
                            "<th>piece No.</th>" +
                            "<th>Date</th>" +
                            "<th>Type</th>" +
                            "<th>Product Details</th>" +
                            "<th>Batch No.</th>" +
                            "<th>QntyRcvd</th>" +
                            "<th>QntyDlvd</th>" +
                            "<th>UOM</th>" +
                            "<th>RcvdAt</th>" +
                            "<th>DlvdFrom</th>" +
                            "<th>Action</th>" +
                            "<th>Action</th>" +
                            "</tr>");
                        for (var i = 0; i < data.d.length; i++) {
                            jq132("#grdTransaction").append("<tr> <td>" +
                                data.d[i].Inventory_PieceNo + "</td> <td>" +
                                data.d[i].Inventory_Date + "</td> <td>" +
                                data.d[i].Inventory_Type + "</td> <td>" +
                                data.d[i].Inventory_ProductDetails + "</td> <td>" +
                                data.d[i].Inventory_BatchNumber + "</td> <td>" +
                                data.d[i].Inventory_QuantityIn + "</td> <td>" +
                                data.d[i].Inventory_QuantityOut + "</td> <td>" +
                                data.d[i].UOM_Name + "</td> <td>" +
                                data.d[i].RecievedAt + "</td> <td>" +
                                data.d[i].DeliveredFrom + "</td> <td>" +
                                "<a href ='javascript:void(0)' id='" + data.d[i].InventoryId + "' onclick='fn_EditTransaction(" + data.d[i].StockPosition_ID + "," + data.d[i].InventoryId + ")'>Edit</a>" + "<input type='hidden' id='" + data.d[i].InventoryId + "'/>" + "</td> <td>" +
                                "<a href ='javascript:void(0)' onclick='fn_DeleteTransaction(" + data.d[i].InventoryId + "," + data.d[i].StockPosition_ID + ")'>Delete</a>" + "</td>" +
                                "</tr>");
                        }
                        document.getElementById("dvPrintAll").style.display = "block";
                    }
                    else {
                        jq132("#grdTransaction").append(
                            "<tr> <td colspan='10'> No data in the data source.</td> </tr>"
                            )
                        document.getElementById("dvPrintAll").style.display = "none";
                    }
                }
            });
        }
        function fn_EditTransaction(id, InventoryId) {
            document.getElementById("hdnOrderPositon").value = InventoryId;
            document.getElementById("hdnTransactionEdit").value = "edit";
            document.getElementById("hdnEditInventoryId").value = InventoryId;
            Trans_Popup.Show();
        }
        function fn_DeleteTransaction(id, stockid) {
            if (confirm("Are you sure delete the record?")) {
                jq132.ajax({
                    type: "POST",
                    url: "InvControlCentre.aspx/DeleteTransaction",
                    data: "{'id':'" + id + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d == true) {
                            alert("Record deleted successfully!");
                            generalGridbind(stockid);
                        }
                    }
                });
            }
            return false;
        }
        function fn_PrintForm() {

            jq132("#customConfirmMain").fadeIn(100);

            //            var IsContactnamePrint = false;
            //            var StockPositionId = document.getElementById("hdnOrderPositon").value;
            //            if (confirm("Are you want to print contact name into the report?")) {
            //                IsContactnamePrint = true;
            //                alert(IsContactnamePrint);
            //                url = "InvTransactionPrintForm.aspx?StockPositionId=" + StockPositionId + "&IsContactnamePrint=" + IsContactnamePrint;
            //                newwindow = window.open(url, 'liveMatches', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=720,height=800');
            //                if (window.focus) {
            //                    newwindow.focus()
            //                }
            //            }
            //            else {
            //                IsContactnamePrint = false;
            //                alert(IsContactnamePrint);
            //                url = "InvTransactionPrintForm.aspx?StockPositionId=" + StockPositionId + "&IsContactnamePrint=" + IsContactnamePrint;
            //                newwindow = window.open(url, 'liveMatches', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=720,height=800');
            //                if (window.focus) {
            //                    newwindow.focus()
            //                }
            //            }
        }


        function YesBtnClick() {

            jq132("#customConfirmMain").fadeOut(100);

            var IsContactnamePrint = false;
            var StockPositionId = document.getElementById("hdnOrderPositon").value;
            var SelectedPageOpt = '0';

            if (document.getElementById("radbtnPageRange_1").checked == true) {
                SelectedPageOpt = 1;
            }

            var pages = '0';
            if (SelectedPageOpt == 1) {
                if (document.getElementById("txtpageno").value == '') {
                    alert('Please put the required piece no');
                    return false;
                }
                else {
                    pages = document.getElementById("txtpageno").value;
                    //alert(pages);
                }
            }
            IsContactnamePrint = true;
            var invNo = document.getElementById("txtInvNo").value;
            var SelectedlblOpt = '0';
            if (document.getElementById("radbtnlabel_1").checked == true) {
                SelectedlblOpt = 1;
            }

            url = "InvTransactionPrintForm.aspx?StockPositionId=" + StockPositionId + "&IsContactnamePrint=" + IsContactnamePrint + "&invNo=" + invNo + "&pages=" + pages + "&lbl=" + SelectedlblOpt;
            newwindow = window.open(url, 'liveMatches', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=720,height=800');
            if (window.focus) {
                newwindow.focus()
            }

        }

        function NoBtnClick() {

            jq132("#customConfirmMain").fadeOut(100);

            /*var IsContactnamePrint = false;
            var StockPositionId = document.getElementById("hdnOrderPositon").value;

            IsContactnamePrint = false;
            //alert(IsContactnamePrint);
            url = "InvTransactionPrintForm.aspx?StockPositionId=" + StockPositionId + "&IsContactnamePrint=" + IsContactnamePrint;
            newwindow = window.open(url, 'liveMatches', 'directories=no,titlebar=no,toolbar=no,location=no,status=no,menubar=no,scrollbars=no,resizable=no,width=720,height=800');
            if (window.focus) {
            newwindow.focus()
            }
            */
        }

    </script>

    <style type="text/css">
        .customConfirmMain {
            height: 330px;
            width: 500px;
            margin: 0px auto;
            position: absolute;
            z-index: 1000;
            margin-left: 487px;
            margin-top: 50px;
            background-color: #fff;
            border-radius: 5px 5px 5px 5px;
            -moz-border-radius: 5px 5px 5px 5px;
            -webkit-border-radius: 5px 5px 5px 5px;
            border: 0px solid #000000;
            -webkit-box-shadow: 2px 2px 2px 2px #B3B3B3;
            box-shadow: 2px 2px 2px 2px #B3B3B3;
            display: none;
        }

        .messageText {
            margin: 26px 11px;
            position: absolute;
        }

        .pagerange {
            float: left;
            margin-left: 11px;
            margin-top: 50px;
            width: 480px;
        }

        .lblSize {
            float: left;
            margin-left: 11px;
            margin-top: 20px;
            width: 480px;
        }

        .ButtonHolder {
            width: 189px;
            float: left;
            margin-left: 180px;
            margin-top: 30px;
        }

        .BtnYes {
            float: left;
            width: 72px;
            cursor: pointer;
        }

        .BtnNo {
            float: left;
            margin-left: 43px;
            width: 74px;
            cursor: pointer;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>INVENTORY CONTROL CENTRE</h3>
        </div>

    </div>
    <div class="form_main">
        <div class="customConfirmMain" id="customConfirmMain">
            <div class="messageText">
                Are you want to print contact name into the report?
            </div>
            <div class="pagerange">
                <asp:Panel GroupingText="Page Range" runat="server">
                    <asp:RadioButtonList runat="server" RepeatDirection="Vertical" ID="radbtnPageRange">
                        <asp:ListItem Value="0" Text="All" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Piece no"></asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:TextBox ID="txtpageno" runat="server"></asp:TextBox>
                    <br />
                    <asp:Label runat="server" Text="Type piece no separated by commas. e.g. 1,2,3" Style="color: Red"></asp:Label>
                    <br />
                    <asp:Label runat="server" Text="INVOICE NO"></asp:Label>
                    :
                <asp:TextBox ID="txtInvNo" runat="server"></asp:TextBox>
                </asp:Panel>
            </div>
            <div class="lblSize">
                <asp:Panel ID="pnlLabel" GroupingText="LABEL SIZE" runat="server">
                    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="radbtnlabel">
                        <asp:ListItem Value="0" Text="10.5 cm X 7 cm" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="1" Text="7.5CM X 3cm"></asp:ListItem>
                    </asp:RadioButtonList>
                </asp:Panel>
            </div>
            <div class="ButtonHolder">
                <input type="button" value="Yes" id="BtnYes" class="BtnYes" onclick="YesBtnClick()" />
                <input type="button" value="No" id="BtnNo" class="BtnNo" onclick="NoBtnClick()" />
            </div>
        </div>
        <asp:Button Visible="false" OnClick="Hidden_Click" ID="btnSearchClick" runat="server" />
        <%--<div class="PopUpArea" id="Parea">
        <uc:ucpOrdPop ID="ucpOrdPopCon" runat="server" />
    </div>--%>
        <div class="PopUpArea" id="Parea_Trans">
            <uc:TransPopup ID="ucTrans_Popup" runat="server" />
        </div>
        <div style="background-color: #d3ddeb; overflow: hidden">
            <%-- <div style="background-color: #4d74a8">
            <h3 style="color: #fff; margin: 0; font-size: 12px; text-transform: uppercase; font-family: Arial, Helvetica, sans-serif; padding: 10px 0; text-align: center">INVENTORY CONTROL CENTRE
            </h3>
        </div>--%>
            <div id="widget" style="width: 1024px">
                <div id="foo">
                    <div id="a">
                        <div id="x">
                            <div style="padding: 0.5em; text-align: justify">
                                <div style="padding: 10px; width: 100%; display: inline-block; vertical-align: top; background-color: #EDEDED; margin-top: 10px; border: 2px solid #1A59AF;">
                                    <%--<h3 style="color: #4D74A8; margin: 0; font-size: 12px; text-transform: uppercase;
                                    font-family: Arial, Helvetica, sans-serif; padding: 0px 0; text-align: left">
                                    POSITION</h3>--%>
                                    <asp:Label ID="LblPosition" runat="server" Text="POSITION" CssClass="newLbl" Style="color: #4D74A8; margin: 0; font-size: 12px; text-transform: uppercase; font-family: Arial, Helvetica, sans-serif; padding: 0px 0; text-align: left"></asp:Label>
                                    <div id="dvEnterTransaction" style="background-color: #EDEDED">
                                        <h3 style="color: #4D74A8; margin: 0; text-transform: uppercase;padding: 10px 0; text-align: left">TRANSACTIONS</h3>
                                        <a href="#" style="color: #4D74A8; margin: 0; padding: 10px 0; text-align: right"
                                            onclick="TransactionPopupOpen();">Enter Transaction</a>
                                    </div>
                                    <%--Position grid start--%>
                                    <div style="width: 100%; overflow: auto">
                                        <asp:GridView ID="grdPosition" runat="server" AutoGenerateColumns="False" EmptyDataText="No data in the data source."
                                            class="grdCommon">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Customer/Vendor" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="#000000">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="pOrder_ID" runat="server" Value='<%#Eval("StockPosition_ID")%>' />
                                                        <asp:Label ID="lblCustomer" runat="server" Text='<%#Eval("StockPosition_ContactID")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Order No" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-ForeColor="#000000" ControlStyle-CssClass="PointerSet">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblpOrderRefNo" runat="server" Text='<%#Eval("pOrderRefNo")%>' ToolTip=' <%#"Order No: " + Eval("pOrderRefNo")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Product" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-ForeColor="#000000" ControlStyle-CssClass="PointerSet">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblpOrderProduct" runat="server" Text='<%#Eval("StockPosition_ProductName")%>'
                                                            ToolTip=' <%#"Order No: " + Eval("StockPosition_OrderNumber")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="QntyToDlv" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-ForeColor="#000000">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblToDeliver" runat="server" Text='<%#Eval("StockPosition_ToDeliver")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="QntDlvd" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-ForeColor="#000000">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDelivered" runat="server" Text='<%#Eval("StockPosition_Delivered")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="QtyToRecv" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-ForeColor="#000000">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblToReceive" runat="server" Text='<%#Eval("StockPosition_ToReceive")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="QtyRcvd" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-ForeColor="#000000">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReceived" runat="server" Text='<%#Eval("StockPosition_Received")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PndgIn" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-ForeColor="#000000">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPndgIn" runat="server" Text='<%#Eval("PndgIn")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="PndgOut" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-ForeColor="#000000">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPndgOut" runat="server" Text='<%#Eval("PndgOut")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="QntyUnt" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    HeaderStyle-ForeColor="#000000">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPndgOut1" runat="server" Text='<%#Eval("StockPosition_QuantityUnitName")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <%--Position grid end--%>
                                </div>
                            </div>
                        </div>
                        <div id="y">
                            <div style="padding: 10px; width: 96%; display: inline-block; background-color: #EDEDED; margin-top: 10px; border: 2px solid #1A59AF;">
                                <h3 style="color: #4D74A8; margin: 0; font-size: 12px; text-transform: uppercase; padding: 10px 0; text-align: left">STOCK</h3>
                                <%--STOCK grid start--%>
                                <asp:GridView ID="grdStock" runat="server" AutoGenerateColumns="False" EmptyDataText="No data in the data source."
                                    class="grdCommon">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Product Desc" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-ForeColor="#000000">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("ProductDescription")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stk In" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-ForeColor="#000000">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("Stock_In")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stk Out" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-ForeColor="#000000">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("Stock_Out")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stk In Hand" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-ForeColor="#000000">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("Stock_In_Hand")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-ForeColor="#000000">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("Location_Name")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%--STOCK grid end--%>
                            </div>
                        </div>
                    </div>
                    <!-- #a -->
                    <!-- end of #foo -->
                </div>
            </div>
            <div style="padding: 10px; width: 98%; display: inline-block; vertical-align: top; background-color: #EDEDED; margin-top: 10px; border: 2px solid #1A59AF;">
                <h3 style="color: #4D74A8; margin: 0; font-size: 12px; text-transform: uppercase;  padding: 10px 0; text-align: left">TRANSACTION</h3>
                <div>
                    <div id="dvPrintAll" style="display: none">
                        <div style="display: inline-block">
                            <div>
                                <a href="javascript:void(0)" id="lnkPrintAllFromOpen" onclick="fn_PrintForm();">Print</a>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:GridView ID="grdTransaction" runat="server" AutoGenerateColumns="False" EmptyDataText="No data in the data source."
                    class="grdCommon">
                    <Columns>
                        <asp:TemplateField HeaderText="Id" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-ForeColor="#000000">
                            <ItemTemplate>
                                <asp:Label ID="lblInventory_ID" runat="server" Text='<%#Eval("Inventory_ID")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-ForeColor="#000000">
                            <ItemTemplate>
                                <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("Inventory_Date")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-ForeColor="#000000">
                            <ItemTemplate>
                                <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("Inventory_Type")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Details" HeaderStyle-HorizontalAlign="Left"
                            ItemStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="#000000">
                            <ItemTemplate>
                                <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("Inventory_ProductDetails")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Batch No." HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-ForeColor="#000000">
                            <ItemTemplate>
                                <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("Inventory_BatchNumber")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="QntyRcvd" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-ForeColor="#000000">
                            <ItemTemplate>
                                <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("Inventory_QuantityIn")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="QntyDlvd" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-ForeColor="#000000">
                            <ItemTemplate>
                                <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("Inventory_QuantityOut")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UOM" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-ForeColor="#000000">
                            <ItemTemplate>
                                <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("UOM_Name")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RcvdAt" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-ForeColor="#000000">
                            <ItemTemplate>
                                <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("RecievedAt")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DlvdFrom" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-ForeColor="#000000">
                            <ItemTemplate>
                                <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("DeliveredFrom")%>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblAction" runat="server" Text="Action"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <a href="javascript:void(0)" id="linkbtn_Edit" runat="server" onclick="OpenEditPopup();">Edit</a>
                                <asp:LinkButton ID="linkbtn_Delete" runat="server" Text="Delete"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="hdnOrderPositon" runat="server" />
                <asp:HiddenField ID="hdnPopupsCheck" runat="server" />
                <asp:HiddenField ID="hdnTransactionEdit" runat="server" />
                <asp:HiddenField ID="hdnEditInventoryId" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>


