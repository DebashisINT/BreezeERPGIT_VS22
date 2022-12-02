<%@ Page Title="Tile Calculator" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="TilesCalculator.aspx.cs" Inherits="ERP.OMS.Management.Others.TilesCalculator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>
    
    <style>
        .margin {
            margin: 5px 10px 10px 0;
        }

        .mtop {
            margin-top: 9px;
            margin-right: 10px;
        }

        .mleft15 {
            margin-left: 15px;
        }

        .tblPadding > tbody > tr > td {
            padding: 5px 15px;
        }
        .spanTop {
                display: inline-block;
    padding-top: 9px;
        }
    </style>
    <script>
        function changeTileUnit(TileType) {
            var e = document.getElementById(TileType);
            var strValue = e.options[e.selectedIndex].value;

            if (TileType == "ddlTileWidthUnit") document.all('<%=ddlTileLongUnit.ClientID %>').value = strValue;
            else if (TileType == "ddlTileLongUnit") document.all('<%=ddlTileWidthUnit.ClientID %>').value = strValue;
        }

        function changeAreaUnit(AreaType) {
            var e = document.getElementById(AreaType);
            var strValue = e.options[e.selectedIndex].value;

            if (AreaType == "ddlTotalAreaUnit") {
                document.all('<%=ddlAreaWideUnit.ClientID %>').value = strValue.charAt(0);
                document.all('<%=ddlAreaLongUnit.ClientID %>').value = strValue.charAt(0);
            }
            else if (AreaType == "ddlAreaWideUnit") {
                document.all('<%=ddlTotalAreaUnit.ClientID %>').value = strValue + '' + strValue;
                document.all('<%=ddlAreaLongUnit.ClientID %>').value = strValue;
            }
            else if (AreaType == "ddlAreaLongUnit") {
                document.all('<%=ddlTotalAreaUnit.ClientID %>').value = strValue + '' + strValue;
                document.all('<%=ddlAreaWideUnit.ClientID %>').value = strValue;
            }
}
    </script>
    <script>
        $(document).ready(function () {
            ctxtProduct.Focus();
        });

        function ProductKeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }

        function ProductButnClick(s, e) {
            if (e.buttonIndex == 0) {
                setTimeout(function () { $("#txtProdSearch").focus(); }, 500);

                $('#txtProdSearch').val('');
                $('#ProductModel').modal('show');
            }
        }

        function prodkeydown(e) {
            var OtherDetails = {}
            OtherDetails.SearchKey = $("#txtProdSearch").val();
            OtherDetails.InventoryType = "B";

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Product Code");
                HeaderCaption.push("Product Name");
                HeaderCaption.push("Inventory");
                HeaderCaption.push("HSN/SAC");
                HeaderCaption.push("Class");
                HeaderCaption.push("Brand");
                if ($("#txtProdSearch").val() != '') {
                    callonServer("../Activities/Services/Master.asmx/GetPurchaseProduct", OtherDetails, "ProductTable", HeaderCaption, "ProdIndex", "SetProduct");
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[ProdIndex=0]"))
                    $("input[ProdIndex=0]").focus();
            }
        }

        function ValueSelected(e, indexName) {
            if (e.code == "Enter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "ProdIndex")
                        SetProduct(Id, name);
                }
            }

            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                if (thisindex < 10)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {
                    if (indexName == "ProdIndex")
                        $('#txtProdSearch').focus();
                }
            }
        }

        function SetProduct(Id, Name) {
            if (Id != "") {
                $('#ProductModel').modal('hide');
                ctxtProduct.SetText(Name);
                ctxtTileWidth.SetText("16.00");
                ctxtTileLong.SetText("16.00");
                ctxtTotalArea.SetText("5000.00");
                ctxtTileWidth.Focus();
                
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="clearfix pull-left">
                <span id="lblHeadTitle">Tile Calculator</span>
            </h3>
        </div>
    </div>
    <div id="ApprovalCross" runat="server" class="crossBtn"><a href="../ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main clearfix">
        <div class="col-md-12">
            <table>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </div>
        <div>
            <br />
        </div>
        <table cellpadding="5" style="border-spacing: 4px !important; border-collapse: separate;" align="center" width="100%" class="tblPadding">
            <tbody>
                <tr bgcolor="#FBEBB2">
                    <td align="right"><b>Product:</b></td>
                    <td>
                        <dxe:ASPxButtonEdit ID="txtProduct" runat="server" ReadOnly="true" ClientInstanceName="ctxtProduct" Width="310px">
                            <Buttons>
                                <dxe:EditButton>
                                </dxe:EditButton>
                            </Buttons>
                            <ClientSideEvents ButtonClick="function(s,e){ProductButnClick(s,e);}" KeyDown="function(s,e){ProductKeyDown(s,e);}" />
                        </dxe:ASPxButtonEdit>
                    </td>
                </tr>
                <tr bgcolor="#b2d8fb">
                    <td align="right"><b>Tile Size:</b></td>
                    <td>
                        <dxe:ASPxTextBox ID="txtTileWidth" runat="server" ClientInstanceName="ctxtTileWidth" HorizontalAlign="Right" Width="150px" CssClass="pull-left margin ">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                            <ValidationSettings Display="None"></ValidationSettings>
                        </dxe:ASPxTextBox>


                        <asp:DropDownList ID="ddlTileWidthUnit" runat="server" DataTextField="Value" DataValueField="ID" Width="150px" CssClass="pull-left margin" onchange="changeTileUnit('ddlTileWidthUnit')">
                        </asp:DropDownList>
                        <span class="pull-left mtop">wide</span>



                        <dxe:ASPxTextBox ID="txtTileLong" runat="server" ClientInstanceName="ctxtTileLong" HorizontalAlign="Right" Width="150px" CssClass="pull-left margin">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                            <ValidationSettings Display="None"></ValidationSettings>
                        </dxe:ASPxTextBox>


                        <asp:DropDownList ID="ddlTileLongUnit" runat="server" DataTextField="Value" DataValueField="ID" Width="150px" CssClass="pull-left margin" onchange="changeTileUnit('ddlTileLongUnit')">
                            <asp:ListItem Text="inches" Value="I"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="pull-left mtop">long</span>
                    </td>
                </tr>
                <tr bgcolor="#cefab0">
                    <td align="right"><b>Total Area to Cover:</b></td>
                    <td>
                        <dxe:ASPxTextBox ID="txtTotalArea" runat="server" ClientInstanceName="ctxtTotalArea" HorizontalAlign="Right" Width="150px" CssClass="pull-left margin">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                            <ValidationSettings Display="None"></ValidationSettings>
                        </dxe:ASPxTextBox>
                        <asp:DropDownList ID="ddlTotalAreaUnit" runat="server" DataTextField="Value" DataValueField="ID" CssClass="pull-left margin" onchange="changeAreaUnit('ddlTotalAreaUnit')">
                            <asp:ListItem Text="square feet" Value="SF"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="pull-left mtop bigtext"><b>OR</b></span>

                        <dxe:ASPxTextBox ID="txtAreaWidth" runat="server" ClientInstanceName="ctxtAreaWidth" HorizontalAlign="Right" Width="150px" CssClass="pull-left margin">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                            <ValidationSettings Display="None"></ValidationSettings>
                        </dxe:ASPxTextBox>
                        <asp:DropDownList ID="ddlAreaWideUnit" runat="server" DataTextField="Value" DataValueField="ID" CssClass="pull-left margin" onchange="changeAreaUnit('ddlAreaWideUnit')">
                            <asp:ListItem Text="inches" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="pull-left mtop">wide</span>

                        <dxe:ASPxTextBox ID="txtAreaLong" runat="server" ClientInstanceName="ctxtAreaLong" HorizontalAlign="Right" Width="150px" CssClass="pull-left margin">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                            <ValidationSettings Display="None"></ValidationSettings>
                        </dxe:ASPxTextBox>
                        <asp:DropDownList ID="ddlAreaLongUnit" runat="server" DataTextField="Value" DataValueField="ID" CssClass="pull-left margin" onchange="changeAreaUnit('ddlAreaLongUnit')">
                            <asp:ListItem Text="inches" Value="I"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="pull-left mtop">long</span>
                    </td>
                </tr>
                <tr bgcolor="#fee8ba">
                    <td align="right"><b>Gap Size:</b></td>
                    <td>
                        <dxe:ASPxTextBox ID="txtGapWidth" runat="server" ClientInstanceName="ctxtGapWidth" HorizontalAlign="Right" Width="150px" CssClass="pull-left margin">
                            <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                            <ValidationSettings Display="None"></ValidationSettings>
                        </dxe:ASPxTextBox>
                        <asp:DropDownList ID="ddlGapWidthUnit" runat="server" DataTextField="Value" DataValueField="ID" CssClass="pull-left margin">
                            <asp:ListItem Text="inches" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="smalltext spanTop">tile grout spacing, use negative value if overlaps.</span>
                    </td>
                </tr>
                <tr bgcolor="#eeeeee">
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click" CssClass="btn btn-primary" UseSubmitBehavior="false" />
                        <asp:Button ID="btnProcedure" runat="server" Text="Procedure for Billing" CssClass="btn btn-primary" UseSubmitBehavior="false" />
                    </td>
                </tr>
            </tbody>
        </table>
        <br />
        <asp:Label ID="lblMessage" runat="server" Font-Size="Medium"></asp:Label>
    </div>

    <!--Product Modal -->
    <div class="modal fade" id="ProductModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="prodkeydown(event)" id="txtProdSearch" autofocus width="100%" placeholder="Search By Product Code or Name" />

                    <div id="ProductTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Product Code</th>
                                <th>Product Name</th>
                                <th>Inventory</th>
                                <th>HSN/SAC</th>
                                <th>Class</th>
                                <th>Brand</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--Product Modal -->
</asp:Content>
