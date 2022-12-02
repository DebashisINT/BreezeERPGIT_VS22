<%@ Page Title="Approve Orders" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Store.Master.Management_Store_Master_AppOrders" Codebehind="AppOrders.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <%--  <link type="text/css" href="../../../CSS/style.css" rel="Stylesheet" />--%>
    <%--<script type="text/javascript" src="/assests/js/loaddata1.js"></script>--%>

    <script type="text/javascript" src="/assests/js/jquery-1.3.2.js"></script>

    <script type="text/javascript">
        $(document).ready(function() {


        });

        function PopUpOpen(ID1) {

            var str = ID1;
            var res = str.split("_")[1];
            var ScheduleApplicationdate = "grdActive_" + res + "_hddOrdTlId";
            var OrdDtlValue = document.getElementById(ScheduleApplicationdate).value;

            $.ajax({
                type: "POST",
                url: "AppOrders.aspx/GetAutofillValue",
                data: "{'id':'" + OrdDtlValue + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(msg) {

                    var data = msg.d;
                    var SetVal = data.split(',');
                    if (SetVal[1] != "" && SetVal[1] != null) {
                        document.getElementById('Popup_Empcitys_txtOAppLog_OrderDetailID_I').value = SetVal[0];
                        document.getElementById('Popup_Empcitys_txtbxOAppLog_ID_I').value = SetVal[1];
                        document.getElementById('Popup_Empcitys_txtOAppLog_OrderNumber_I').value = SetVal[2];

                        document.getElementById('Popup_Empcitys_txtMarkets_Code_I').value = SetVal[6];
                        document.getElementById('Popup_Empcitys_txtMarkets_Name_I').value = SetVal[7];
                        document.getElementById('Popup_Empcitys_txtMarkets_Description_I').value = SetVal[8];

                    }
                    else {
                        document.getElementById('Popup_Empcitys_txtOAppLog_OrderDetailID_I').value = SetVal[0];

                        document.getElementById('Popup_Empcitys_txtMarkets_Code_I').value = "";
                        document.getElementById('Popup_Empcitys_txtMarkets_Name_I').value = "";
                        document.getElementById('Popup_Empcitys_txtMarkets_Description_I').value = "";
                        document.getElementById('Popup_Empcitys_txtbxOAppLog_ID_I').value = "";
                        document.getElementById('Popup_Empcitys_txtOAppLog_OrderNumber_I').value = "";
                    }

                },
                error: function(result) {
                    alert("Try Again");

                }

            });

            cPopup_Empcitys.Show();

        }
        function rejectPopUpOpen(ID1) {
            var str = ID1;
            var res = str.split("_")[1];
            var ScheduleApplicationdate = "grdActive_" + res + "_hddOrdTlId";
            var OrdDtlValue = document.getElementById(ScheduleApplicationdate).value;

            $.ajax({
                type: "POST",
                url: "AppOrders.aspx/GetAutofillValue",
                data: "{'id':'" + OrdDtlValue + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(msg) {

                    var data = msg.d;
                    var SetVal = data.split(',');
                    if (SetVal[1] != "" && SetVal[1] != null) {
                        document.getElementById('rejectpopup_txtOAppLog_Reject_OrderDetailID_I').value = SetVal[0];

                    }
                    else {
                        document.getElementById('rejectpopup_txtOAppLog_Reject_OrderDetailID_I').value = SetVal[0];
                        document.getElementById('rejectpopup_txtrejectRemarks_I').value = "";
                    }

                },
                error: function(result) {
                    alert("Try Again");

                }

            });

            crejectpopup.Show();
        }

        function DetailsPopUp(ID1) {
            var str = ID1;
            var res = str.split("_")[1];
            var ScheduleApplicationdate = "grdActive_" + res + "_hddOrdTlId";
            var OrdDtlValue = document.getElementById(ScheduleApplicationdate).value;
            var monthNames = ["", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC"];


            $("#orderDetails_lblprodDtl").text('');
            $("#orderDetails_lblOrdDtlBrand").text('');
            $("#orderDetails_lblsize").text('');
            $("#orderDetails_lblProdOrdDtlColor").text('');
            $("#orderDetails_lblBstBfrYr").text('');
            $("#orderDetails_lblYr").text('');
            $("#orderDetails_lblmnth").text('');
            $("#orderDetails_lblProdOrdDtlQuote").text('');
            $("#orderDetails_lblpUnit").text('');
            $("#orderDetails_lbllotUnit").text('');
            $("#orderDetails_lblUnit").text('');
            $("#orderDetails_lblQuantity").text('');
            $("#orderDetails_lblQntityunit").text('');
            $("#orderDetails_lblRemarks").text('');

            $.ajax({
                type: "POST",
                url: "AppOrders.aspx/GetDetailsofProduct",
                data: "{'id':'" + OrdDtlValue + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(msg) {

                    var data = msg.d;
                    var SetVal = data.split(',');

                    $("#orderDetails_lblprodDtl").text(SetVal[1]);
                    $("#orderDetails_lblOrdDtlBrand").text(SetVal[2]);
                    $("#orderDetails_lblsize").text(SetVal[3]);

                    $("#orderDetails_lblProdOrdDtlColor").text(SetVal[4]);
                    if (SetVal[5] != "" && SetVal[5] != "0") {
                        $("#orderDetails_lblBstBfrYr").text('Applicable');
                    }
                    else
                        $("#orderDetails_lblBstBfrYr").text('Not Applicable');

                    $("#orderDetails_lblYr").text(SetVal[5]);
                    $("#orderDetails_lblmnth").text(monthNames[SetVal[6]]);


                    $("#orderDetails_lblProdOrdDtlQuote").text(SetVal[7]);

                    $("#orderDetails_lblpUnit").text(SetVal[8]);
                    $("#orderDetails_lbllotUnit").text(SetVal[9]);
                    $("#orderDetails_lblUnit").text(SetVal[10]);
                    $("#orderDetails_lblQuantity").text(SetVal[11]);
                    $("#orderDetails_lblQntityunit").text(SetVal[12]);
                    $("#orderDetails_lblRemarks").text(SetVal[13]);

                },
                error: function(result) {
                    alert("Try Again");

                }

            });
            corderDetails.Show();

        }

        function fn_btnCancel() {
            corderDetails.Hide();
            document.location.reload();
        }

        function fn_btnRejectCancel() {

            crejectpopup.Hide();
            document.location.reload();
        }
        function OnAppQty_KeyPress() {

            var Quantity = document.getElementById('Popup_Empcitys_txtMarkets_Code_I').value;

            if (isNaN(Quantity)) {
                alert("Please enter proper value for Quantity");
                document.getElementById('Popup_Empcitys_txtMarkets_Code_I').value = "";
                document.getElementById("Popup_Empcitys_txtMarkets_Code_I").focus();
                return false;
            }
            //             if (Quantity == "" || Quantity == null || Quantity === undefined) {
            //                 alert("Please enter value for Quantity");
            //                 document.getElementById('Popup_Empcitys_txtMarkets_Code_I').value = "";
            //                 document.getElementById("Spinner1_Popup_Empcitys_txtQuantity").focus();
            //                 return false;
            //             }
            //             if (Quantity.indexOf('.') > -1) {
            //                 
            //             }
            //             else {
            //                 alert("Please enter only proper value for Quantity");
            //                 document.getElementById('Popup_Empcitys_txtMarkets_Code_I').value = "";
            //                 document.getElementById("Spinner1_Popup_Empcitys_txtQuantity").focus();
            //                 return false;
            //             }
        }
        function OnAppPrice_KeyPress() {


            var regex = /^\d+(\.\d{0,2})?$/g;

            var Quantity = document.getElementById('Popup_Empcitys_txtMarkets_Name_I').value;

            if (isNaN(Quantity)) {
                alert("Please enter proper value for price");
                document.getElementById('Popup_Empcitys_txtMarkets_Name_I').value = "";
                document.getElementById("Popup_Empcitys_txtMarkets_Name_I").focus();
                return false;
            }
            if (!regex.test(Quantity)) {
                alert('Please enter two decimal places price');

                document.getElementById('Popup_Empcitys_txtMarkets_Name_I').value = "";
                document.getElementById("Popup_Empcitys_txtMarkets_Name_I").focus();

                return false;
            }

        }
        function btnSave_citys() { 
            var Quantity = document.getElementById('Popup_Empcitys_txtMarkets_Code_I').value;
            if (Quantity == "") {
                alert("Please enter value for Quantity");
                document.getElementById('Popup_Empcitys_txtMarkets_Code_I').value = "";
                document.getElementById("Popup_Empcitys_txtMarkets_Code_I").focus();
                return false; 
            } 
            else {
                if (isNaN(Quantity)) {
                    alert("Please enter proper value for Quantity");
                    document.getElementById('Popup_Empcitys_txtMarkets_Code_I').value = "";
                    document.getElementById("Popup_Empcitys_txtMarkets_Code_I").focus();
                    return false;
                }
//                if (Quantity.indexOf('.') > -1) {
//                    alert("Please enter Integer value for Quantity");
//                    document.getElementById('Popup_Empcitys_txtMarkets_Code_I').value = "";
//                    document.getElementById("Popup_Empcitys_txtMarkets_Code_I").focus();
//                    return false;
//                }
            }


            var regex = /^\d+(\.\d{0,2})?$/g;

            var Price = document.getElementById('Popup_Empcitys_txtMarkets_Name_I').value;
            if (Price == "") {
                alert('Please enter proper two decimal places price');

                document.getElementById('Popup_Empcitys_txtMarkets_Name_I').value = "";
                document.getElementById("Popup_Empcitys_txtMarkets_Name_I").focus();

                return false;
            }
            else {
                if (isNaN(Price)) {
                    alert("Please enter proper value for price");
                    document.getElementById('Popup_Empcitys_txtMarkets_Name_I').value = "";
                    document.getElementById("Popup_Empcitys_txtMarkets_Name_I").focus();
                    return false;
                }
                if (!regex.test(Price)) {
                    alert('Please enter two decimal places price');

                    document.getElementById('Popup_Empcitys_txtMarkets_Name_I').value = "";
                    document.getElementById("Popup_Empcitys_txtMarkets_Name_I").focus();

                    return false;
                }
            }

        }
    </script>
    <style>
        #divChangable>table>tbody>tr>td {
            padding:5px ;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Approve Orders</h3>
        </div>
    </div>
    <div class="form_main">
        <div id="divChangable">
            <table>
                <tr>
                    <td>
                        Priod from Date
                    </td>
                    <td>
                        <dxe:ASPxDateEdit ID="txtpOrder_Date" runat="server" Width="180" ClientInstanceName="ctxtpOrder_Date"
                            EditFormat="Custom" EditFormatString="dd/MM/yyyy" UseMaskBehavior="true" />
                    </td>
                    <td>
                        Priod to Date
                    </td>
                    <td >
                        <dxe:ASPxDateEdit ID="txttoOrderDate" runat="server" Width="180" ClientInstanceName="ctxtpOrder_Date"
                            EditFormat="Custom" EditFormatString="dd/MM/yyyy" UseMaskBehavior="true" />
                    </td>
                    <%--<td>
                        Branch
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rdblist" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="All" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Selected"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>--%>
                </tr>
                <tr>
                    <td>
                        Order Type
                    </td>
                    <td >
                        <dxe:ASPxComboBox ID="ddlOrderType" ClientInstanceName="cddlOrderType" runat="server"
                            ValueType="System.String" Width="180px" EnableSynchronization="True" EnableIncrementalFiltering="True">
                        </dxe:ASPxComboBox>
                    </td>
                    <td>
                        Order Status
                    </td>
                    <td>
                        <dxe:ASPxComboBox ID="cmbOrder_Status" ClientInstanceName="cddlOrderType" runat="server"
                            ValueType="System.String" Width="180px" EnableSynchronization="True" EnableIncrementalFiltering="True">
                        </dxe:ASPxComboBox>
                    </td>
                    <%-- <td>
                        Customer/Vendor
                    </td>
                    <td>
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="All" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Selected"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>--%>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="3" style="padding:10px 5px">
                        <asp:Button ID="btnsubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnsubmit_OnClick" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="height:700px !important;overflow-y:auto;">
            <asp:GridView ID="grdActive" runat="server" Width="100%" height="100%"
                 BorderColor="CornflowerBlue" 
                ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
                BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" 
                OnSorting="grdActive_Sorting"
                OnRowCreated="grdActive_RowCreated"  PageSize="25" AllowPaging="true"
                OnRowDataBound="grdActive_RowDataBound" OnPageIndexChanged="grdActive_PageIndexChanged" 
                OnPageIndexChanging="grdActive_PageIndexChanging">
                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                <Columns>
                    <asp:TemplateField HeaderText="pOrder_ID" Visible="false">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblpOrder_ID" runat="server" Text='<%# Eval("pOrder_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="ORDER NO" SortExpression="pOrder_RefNumber">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblpOrder_Number" runat="server" Text='<%# Eval("pOrder_RefNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ORDER DATE" SortExpression="pOrder_Date">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblpOrder_Date" runat="server" Text='<%# Eval("pOrder_Date")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DETAILS" Visible="false">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <%-- <asp:LinkButton ID="lnkbtnView" runat="server" OnClientClick="DetailsPopUp(this.id)" >View</asp:LinkButton>  --%>
                            <a id="lnkbtnView" runat="server" href="#" onclick="DetailsPopUp(this.id)">View</a>
                            <%-- <asp:Label ID="lblpOrder_Date" runat="server" Text='<%# Eval("pOrder_Date")%>'></asp:Label>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CUSTOMER" SortExpression="Contactname">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblContactname" runat="server" Text='<%# Eval("Contactname")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PRODUCTS" SortExpression="sProducts_Name">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblproduct" runat="server" Text='<%# Eval("sProducts_Name")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="COLOR" SortExpression="Color">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblColor" runat="server" Text='<%# Eval("Color")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="WIDTH" SortExpression="Size">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblSize" runat="server" Text='<%# Eval("Size")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ORDER PRICE" SortExpression="pOrderDetail_UnitPrice">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblpOrderDetail_UnitPrice" runat="server" Text='<%# Eval("pOrderDetail_UnitPrice")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ORDER QTY" SortExpression="pOrderDetail_Quantity">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblpOrderDetail_Quantity" runat="server" Text='<%# Eval("pOrderDetail_Quantity")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Order UOM" SortExpression="pOrder_UOM" Visible="false">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblpOrder_UOM" runat="server" Text='<%# Eval("UOM_Name")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="APPROVE QTY" SortExpression="pOrderDetail_ApprovQuantity">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblpOrderDetail_ApprovQuantity" runat="server" Text='<%# Eval("pOrderDetail_ApprovQuantity")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="APPROVE PRICE" SortExpression="pOrderDetail_ApprovPrice">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblpOrderDetail_ApprovPrice" runat="server" Text='<%# Eval("pOrderDetail_ApprovPrice")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="APPROVE">
                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkApprove" runat="server" onclick="PopUpOpen(this.id);" />
                            <asp:HiddenField ID="hddOrdTlId" runat="server" Value='<%# Eval("pOrderDetail_ID")%>' />
                            <asp:HiddenField ID="hddpOrder_ApprovUser1" runat="server" Value='<%# Eval("pOrder_ApprovUser1")%>' />
                            <asp:HiddenField ID="hddpOrder_ApprovUser2" runat="server" Value='<%# Eval("pOrder_ApprovUser2")%>' />
                            <asp:HiddenField ID="hddpOrder_ApprovUser3" runat="server" Value='<%# Eval("pOrder_ApprovUser3")%>' />
                            <%-- <asp:Label ID="lblapp" runat="server" Text= ></asp:Label>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="REJECT">
                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="ChkReject" runat="server" onclick="rejectPopUpOpen(this.id);" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                    BorderWidth="1px"></RowStyle>
                <EditRowStyle BackColor="#E59930"></EditRowStyle>
                <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                <PagerStyle ForeColor="Blue" HorizontalAlign="Center"></PagerStyle>
                <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                    Font-Bold="False"></HeaderStyle>
            </asp:GridView>
        </div>
        <div class="PopUpArea" >
            <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
                Width="400px" HeaderText="Add Order Approval Details" PopupHorizontalAlign="WindowCenter"
                BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad">
                <ContentCollection>
                    <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div class="Top" style="background: #ccc; padding: 10px 30px;">
                            <div style="overflow: hidden; margin: 10px 0">
                                <div class="cityDiv" style="display: inline-block; vertical-align: top; float: left">
                                    Approve Quantity
                                </div>
                                <div class="Left_Content" style="display: inline-block; float: right">
                                    <dxe:ASPxTextBox ID="txtMarkets_Code" MaxLength="10" ClientInstanceName="ctxtMarkets_Code"
                                        runat="server" Width="180px">
                                        <%-- <ClientSideEvents KeyUp="function(s,e){OnAppQty_KeyPress()}"></ClientSideEvents>--%>
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div style="overflow: hidden; margin: 10px 0">
                                <div class="cityDiv" style="display: inline-block; vertical-align: top; float: left">
                                    Approve Price
                                </div>
                                <div class="Left_Content" style="display: inline-block; float: right">
                                    <dxe:ASPxTextBox ID="txtMarkets_Name" ClientInstanceName="ctxtMarkets_Name" runat="server"
                                        Width="180px">
                                        <%--   <ClientSideEvents KeyUp="function(s,e){OnAppPrice_KeyPress()}"></ClientSideEvents>--%>
                                    </dxe:ASPxTextBox>
                                </div>
                            </div>
                            <div style="overflow: hidden; margin: 10px 0">
                                <div class="cityDiv" style="display: inline-block; vertical-align: top; float: left">
                                    Remarks
                                </div>
                                <div class="Left_Content" style="display: inline-block; float: right">
                                    <dxe:ASPxMemo ID="txtMarkets_Description" ClientInstanceName="ctxtMarkets_Description"
                                        runat="server" Width="180px" Height="60px">
                                    </dxe:ASPxMemo>
                                </div>
                            </div>
                            <div style="display: none">
                                <dxe:ASPxTextBox ID="txtbxOAppLog_ID" ClientInstanceName="ctxtbxOAppLog_ID" runat="server"
                                    Width="180px">
                                </dxe:ASPxTextBox>
                                <dxe:ASPxTextBox ID="txtOAppLog_OrderDetailID" ClientInstanceName="ctxtOAppLog_OrderDetailID"
                                    runat="server" Width="180px">
                                </dxe:ASPxTextBox>
                                <dxe:ASPxTextBox ID="txtOAppLog_OrderNumber" ClientInstanceName="ctxtOAppLog_OrderNumber"
                                    runat="server" Width="180px">
                                </dxe:ASPxTextBox>
                            </div>
                        </div>
                        <div class="ContentDiv" style="height: auto; background: #ccc;">
                            <div style="display: none">
                            </div>
                            <br style="clear: both;" />
                            <div class="Footer">
                                <div style="margin-left: 130px; width: 70px; float: left;">
                                    <%-- <dxe:ASPxButton ID="" ClientInstanceName="cbtnSave_citys" runat="server"
                                        OnClick="" Text="Save">
                                       <ClientSideEvents Click="function (s, e) {javascript:return btnSave_citys();}" />
                                    </dxe:ASPxButton>--%>
                                    <asp:Button runat="server" ID="btnSave_OrderApprove" Style="padding: 3px" Text="Save"
                                        OnClick="btnSave_OrderApprove_OnClick" OnClientClick="javascript:return btnSave_citys()" />
                                </div>
                                <div style="">
                                    <dxe:ASPxButton ID="btnCancel_citys" runat="server" AutoPostBack="False" Text="Cancel">
                                        <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                    </dxe:ASPxButton>
                                </div>
                                <br style="clear: both;" />
                            </div>
                            <br style="clear: both;" />
                        </div>
                        <%-- </div>--%>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>
            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
            </dxe:ASPxGridViewExporter>
        </div>
        <div class="PopUpArea" >
            <dxe:ASPxPopupControl ID="rejectpopup" runat="server" ClientInstanceName="crejectpopup"
                Width="400px" HeaderText="Order Reject Details" PopupHorizontalAlign="WindowCenter"
                BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad">
                <ContentCollection>
                    <dxe:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div class="Top" style="background: #ccc; padding: 10px 30px;">
                            <div style="overflow: hidden; margin: 10px 0">
                                <div class="cityDiv" style="display: inline-block; vertical-align: top; float: left">
                                    Remarks
                                </div>
                                <div class="Left_Content" style="display: inline-block; float: right">
                                    <dxe:ASPxMemo ID="txtrejectRemarks" ClientInstanceName="ctxtrejectRemarks" runat="server"
                                        Width="180px" Height="60px">
                                    </dxe:ASPxMemo>
                                </div>
                            </div>
                            <div style="display: none">
                                <%-- <dxe:ASPxTextBox ID="ASPxTextBox3" ClientInstanceName="ctxtbxOAppLog_ID" runat="server"
                                    Width="180px">
                                </dxe:ASPxTextBox>--%>
                                <dxe:ASPxTextBox ID="txtOAppLog_Reject_OrderDetailID" ClientInstanceName="ctxtOAppLog_Reject_OrderDetailID"
                                    runat="server" Width="180px">
                                </dxe:ASPxTextBox>
                                <%-- <dxe:ASPxTextBox ID="ASPxTextBox5" ClientInstanceName="ctxtOAppLog_OrderNumber"
                                    runat="server" Width="180px">
                                </dxe:ASPxTextBox>--%>
                            </div>
                        </div>
                        <div class="ContentDiv" style="height: auto; background: #ccc;">
                            <div style="display: none">
                            </div>
                            <br style="clear: both;" />
                            <div class="Footer">
                                <div style="margin-left: 130px; width: 70px; float: left;">
                                    <dxe:ASPxButton ID="btnSave_Orderreject" ClientInstanceName="cbtnSave_citys" runat="server"
                                        OnClick="btnSave_Orderreject_OnClick" Text="Save">
                                    </dxe:ASPxButton>
                                </div>
                                <div style="">
                                    <dxe:ASPxButton ID="ASPxButton2" runat="server" AutoPostBack="False" Text="Cancel">
                                        <ClientSideEvents Click="function (s, e) {fn_btnRejectCancel();}" />
                                    </dxe:ASPxButton>
                                </div>
                                <br style="clear: both;" />
                            </div>
                            <br style="clear: both;" />
                        </div>
                        <%-- </div>--%>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>
            <dxe:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server">
            </dxe:ASPxGridViewExporter>
        </div>
        <div class="PopUpArea" >
            <dxe:ASPxPopupControl ID="orderDetails" runat="server" ClientInstanceName="corderDetails"
                Width="400px" HeaderText="View Order Details" PopupHorizontalAlign="WindowCenter"
                BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad">
                <ContentStyle VerticalAlign="Top" CssClass="pad">
                </ContentStyle>
                <ContentCollection>
                    <dxe:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                        <div style="width: 100%; background-color: #ccc; margin: 0px; overflow: hidden;">
                            <div class="Top">
                                <div style="margin: 10px 0; clear: both; overflow: hidden">
                                    <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                        float: left;">
                                        Product Details
                                    </div>
                                    <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                        <asp:Label ID="lblprodDtl" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div style="margin: 10px 0; clear: both; overflow: hidden">
                                    <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                        float: left;">
                                        Order Detail Brand
                                    </div>
                                    <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                        <asp:Label ID="lblOrdDtlBrand" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div style="margin: 10px 0; clear: both; overflow: hidden">
                                    <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                        float: left;">
                                        Size/Strength
                                    </div>
                                    <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                        <asp:Label ID="lblsize" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div style="margin: 10px 0; clear: both; overflow: hidden">
                                    <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                        float: left;">
                                        Product Order Detail Color
                                    </div>
                                    <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                        <asp:Label ID="lblProdOrdDtlColor" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div style="margin: 10px 0; clear: both; overflow: hidden">
                                    <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                        float: left;">
                                        BestBefore Mo/Yr
                                    </div>
                                    <div class="Left_Content" style="display: inline-block; float: right; margin-right: -33px;
                                        width: 106px">
                                        <asp:Label ID="lblBstBfrYr" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div style="margin: 10px 0; clear: both; overflow: hidden">
                                    <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                        float: left;">
                                        Product Order Detail
                                        <br>
                                        BestBeforeYear/Month
                                    </div>
                                    <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                        <div style="float: left; margin-right: 5px;">
                                            <asp:Label ID="lblmnth" runat="server"></asp:Label>
                                        </div>
                                        <div style="float: left">
                                            <asp:Label ID="lblYr" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div style="margin: 10px 0; clear: both; overflow: hidden">
                                    <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                        float: left;">
                                        Product Order Detail QuoteCurrency
                                    </div>
                                    <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                        <asp:Label ID="lblProdOrdDtlQuote" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div style="margin: 10px 0; clear: both; padding: 5px 0; overflow: hidden; overflow: hidden">
                                    <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                        float: left;">
                                        Unit Price
                                    </div>
                                    <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                        <div style="float: left; margin: 0 3px">
                                            <asp:Label ID="lblpUnit" runat="server"></asp:Label>
                                            &nbsp;/
                                        </div>
                                        <div style="float: left;">
                                            <asp:Label ID="lbllotUnit" runat="server"></asp:Label>
                                        </div>
                                        <div style="float: left; margin: 0 2px">
                                            <asp:Label ID="lblUnit" runat="server"></asp:Label>
                                            Unit
                                        </div>
                                    </div>
                                </div>
                                <div style="margin: 10px 0; clear: both; overflow: hidden">
                                    <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                        float: left;">
                                        Quantity
                                    </div>
                                    <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                        <asp:Label ID="lblQuantity" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div style="margin: 10px 0; clear: both; overflow: hidden">
                                    <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                        float: left;">
                                        Quantity Unit
                                    </div>
                                    <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                        <asp:Label ID="lblQntityunit" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div style="margin: 10px 0; clear: both; overflow: hidden">
                                    <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                        float: left;">
                                        Remarks
                                    </div>
                                    <div class="Left_Content txtA" style="display: inline-block; float: right; margin-right: 20px;">
                                        <asp:Label ID="lblRemarks" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div style="margin: 10px 0; clear: both; overflow: hidden">
                                    <div class="cityDiv dxbBtn" style="display: inline-block; height: auto; margin-left: 159px;
                                        float: left;">
                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="False" Text="Cancel">
                                            <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                        </dxe:ASPxButton>
                                    </div>
                                </div>
                            </div>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>
            &nbsp;<dxe:ASPxGridViewExporter ID="ASPxGridViewExporter2" runat="server">
            </dxe:ASPxGridViewExporter>
        </div>
    </div>
</asp:Content>
