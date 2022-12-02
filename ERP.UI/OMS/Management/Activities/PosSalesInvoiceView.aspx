<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="PosSalesInvoiceView.aspx.cs" Inherits="ERP.OMS.Management.Activities.PosSalesInvoiceView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .inline {
            display: inline !important;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content {
            overflow: visible !important;
        }

        .labelClass {
            color: #ffffff;
            background-color: rgb(12, 135, 202);
            font-size: 15px;
        }

        strong .dxeBase_PlasticBlue {
            font-weight: 700 !important;
        }

        .abs {
            position: absolute;
            right: -20px;
            top: 10px;
        }

        .fa.fa-exclamation-circle:before {
            font-family: FontAwesome;
        }

        .tp2 {
            right: -18px;
            top: 7px;
            position: absolute;
        }

        #txtCreditLimit_EC {
            position: absolute;
        }

        #grid_DXStatus span > a {
            display: none;
        }

        #aspxGridTax_DXEditingErrorRow0 {
            display: none;
        }

        .horizontal-images.content li {
            float: left;
        }

        #rdl_SaleInvoice {
            margin-top: 3px;
        }

            #rdl_SaleInvoice > tbody > tr > td {
                padding-right: 5px;
            }

        .bod-table {
            width: 100%;
            border-radius: 5px;
        }

            .bod-table > tbody > tr > td {
                padding: 5px;
                background: #c2d8e6;
                font-weight: 500;
            }

            .bod-table.none > tbody > tr > td {
                background: none;
            }

        .bac {
            background: #c2d8e6;
            margin: 10px 0;
            padding: 2px 15px;
            border-radius: 5px;
        }

        .greyd {
            background: #ececec;
            margin: 10px 0;
            padding: 0px 15px;
            border-radius: 5px;
        }

        .newLbl .lblHolder table tr:first-child td {
            background: #2bb1bf;
        }

        table.pad > tbody > tr > td {
            padding: 0px 10px;
        }

        section.rds {
            margin-top: 25px;
            border: 1px solid #ccc;
            padding: 3px 15px;
        }

        span.fieldsettype {
            background: #1671b7;
            padding: 8px 10px;
            color: #fff;
            position: relative;
            top: -10px;
            z-index: 5;
        }

            span.fieldsettype::before {
                content: "";
                border-left: 9px solid transparent;
                border-right: 9px solid transparent;
                border-bottom: 13px solid #184d75;
                position: absolute;
                right: -9px;
                z-index: -1;
            }

        .horizontallblHolder {
            height: auto;
            border: 1px solid #12a79b;
            border-radius: 3px;
            overflow: hidden;
        }

            .horizontallblHolder > table > tbody > tr > td {
                padding: 8px 10px;
                background: #ffffff;
                background: -moz-linear-gradient(top, #ffffff 0%, #f3f3f3 50%, #ededed 51%, #ffffff 100%);
                background: -webkit-linear-gradient(top, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%);
                background: linear-gradient(to bottom, #ffffff 0%,#f3f3f3 50%,#ededed 51%,#ffffff 100%);
                filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#ffffff',GradientType=0 );
            }

                .horizontallblHolder > table > tbody > tr > td:first-child {
                    background: #12a79b;
                    color: #fff;
                }

                .horizontallblHolder > table > tbody > tr > td:last-child {
                    font-weight: 500;
                    text-transform: uppercase;
                    color: #121212;
                }

        .dxgvControl_PlasticBlue td.dxgvBatchEditModifiedCell_PlasticBlue {
            background: #fff !important;
        }

        .popover {
            z-index: 999999;
            max-width: 350px;
        }

            .popover .popover-title {
                margin-top: 0 !important;
                background: #465b9d;
                color: #fff;
            }

        .pdLeft15 {
            padding-left: 15px;
        }

        .mTop {
            margin-top: 10px;
        }

        .mLeft {
            margin-left: 15px;
        }

        .popover .popover-content {
            min-height: 60px;
        }
        /*#grid_DXEditingErrorRow-1 {
            display: none;
        }*/

        /*#grid_DXStatus span > a {
            display: none;
        }

        #gridTax_DXStatus span > a {
            display: none;
        }*/

        #grid_DXStatus {
            display: none;
        }

        #aspxGridTax_DXStatus {
            display: none;
        }

        #gridTax_DXStatus {
            display: none;
        }

        .hideCell {
            display: none;
        }

        .pullleftClass {
            position: absolute;
            right: -3px;
            top: 24px;
        }

        #myCheck {
            transform: translateY(2px);
            -webkit-transform: translateY(2px);
            -moz-transform: translateY(2px);
            margin-right: 5px;
        }

        .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }

        .dxeButtonEditClearButton_PlasticBlue {
            display: none;
        }

        .mbot5 .col-md-8 {
            margin-bottom: 5px;
        }

        .validclass {
            position: absolute;
            right: -4px;
            top: 20px;
        }

        .mandt {
            position: absolute;
            right: -18px;
            top: 4px;
        }

        #txtProductAmount, #txtProductTaxAmount, #txtProductDiscount {
            font-weight: bold;
        }

        /*#grid, #grid div {
            width: 100% !important;
        }*/
        .crossBtn {
            cursor: pointer;
        }

        #txtTaxTotAmt input, #txtprodBasicAmt input, #txtGstCstVat input {
            text-align: right;
        }

        #grid .dxgvHSDC > div, #grid .dxgvCSD {
            width: 100% !important;
        }

        #grid_DXMainTable > tbody > tr > td:last-child {
            display: none !important;
        }

        #taxroundedOf, #chargesRoundOf {
            font-weight: 600;
            font-size: 15px;
            color: #7f0826;
        }

        #openlink {
            font-size: 18px;
        }

        .errorField {
            position: absolute;
            right: 0;
            top: 20px;
        }

        .mTop10 {
            margin-top: 10px;
        }

        .backBranch {
            font-weight: 600;
            background: #75c1f5;
            padding: 5px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-title clearfix" id="myDiv">
        <h3 class="pull-left">
            <asp:Label ID="lblHeadTitle" Text="" runat="server"></asp:Label>
            <%--<label>Add Proforma Invoice/ Quotation</label>--%>
            <%--<div class="pull-right" style="font-size: 14px;"> Login into: <span class="backBranch"></span></div>--%>

        </h3>
        s


        <div id="pageheaderContent" class="scrollHorizontal pull-right wrapHolder content horizontal-images">
            <div class="Top clearfix">
                <ul>

                    <li>
                        <div class="lblHolder" id="divDues" style="display: none;">
                            <table>
                                <tr>
                                    <td>Customer Balance</td>
                                </tr>
                                <tr>
                                    <td class="lower">
                                        <asp:Label ID="lblTotalDues" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>


                    <li>
                        <div class="lblHolder" id="idCashbalanace">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Cash Balance </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="width: 100%;">
                                                <b style="text-align: center" id="B_BankBalance" runat="server">0.00</b>

                                            </div>

                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>

                    <li style="cursor: pointer">
                        <div class="lblHolder" id="divAvailableStk" onclick="AvailableStockClick()">
                            <table>
                                <tr>
                                    <td>Available Stock</td>
                                </tr>
                                <tr>
                                    <td style="color: blue">
                                        <asp:Label ID="lblAvailableStk" runat="server" Text="0.0"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>


                    <li>
                        <div class="lblHolder" id="divinvoiced">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Currently Invoiced</td>
                                    </tr>
                                    <tr>
                                        <td>

                                            <asp:Label ID="lblInvoiced" runat="server" Text="0.0"></asp:Label>

                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>

                    <li>
                        <div class="lblHolder">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>Actual Stock</td>
                                    </tr>
                                    <tr>
                                        <td>

                                            <asp:Label ID="lblActStock" runat="server" Text="0.0"></asp:Label>

                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </li>


                    <li class="hide">

                        <div class="lblHolder " id="divPacking" style="display: none;">
                            <table>
                                <tr>
                                    <td>Packing Quantity</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblPackingStk" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                </ul>
                <ul style="display: none;">
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tr>
                                    <td>Selected Branch</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblbranchName" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tr>
                                    <td>Selected Product</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblProduct" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                    <li>
                        <div class="lblHolder">
                            <table>
                                <tr>
                                    <td>Stock Quantity</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblStkQty" runat="server" Text="0.00"></asp:Label>
                                        <asp:Label ID="lblStkUOM" runat="server" Text=" "></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <div id="ApprovalCross" runat="server" class="crossBtn"><a href=""><i class="fa fa-times"></i></a></div>
        <div id="divcross" runat="server" class="crossBtn"><a href="PosSalesInvoiceList.aspx"><i class="fa fa-times"></i></a></div>

    </div>
    <div class="form_main">
        <asp:Panel ID="pnl_quotation" runat="server">
            <div class="">
                <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ClientInstanceName="page" Width="100%">
                    <TabPages>
                        <dxe:TabPage Name="General" Text="General">
                            
                        </dxe:TabPage>
                    </TabPages>
                </dxe:ASPxPageControl>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
