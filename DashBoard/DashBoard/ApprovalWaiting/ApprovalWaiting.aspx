<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApprovalWaiting.aspx.cs" Inherits="DashBoard.DashBoard.ApprovalWaiting.ApprovalWaiting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" />

    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:ital,wght@0,200;0,500;0,600;0,700;0,800;0,900;1,400&display=swap" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />
    <link href="../css/jquery.alerts.css" rel="stylesheet" />
    <script src="../Js/jquery.alerts.js"></script>
    <link href="../css/projectDB.css" rel="stylesheet" />
    <style>
        .itemType {
            margin-right: 20px;
        }

            .itemType.active {
                background: #fbffea;
                box-shadow: 0px 0px 5px 0px rgb(51 50 50 / 12%);
                transform: scale(1.1) translateY(2px);
            }

        .closeBtn.whd > a {
            color: #ccc;
        }

        .shwDet {
            cursor: pointer;
        }

        #popup_title {
            color: #fff !important;
        }

        .wrapTrue {
            flex-wrap: wrap;
        }

        .space-arround {
            justify-content: space-around;
        }
    </style>

    <script src="ApprovalWaiting.js?V=1.4"></script>


</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h3 class="kpiHeading">Approval Waiting
            <span class="pull-right closeBtn whd"><a href="#" onclick="reloadParent()"><i class="fa fa-times"></i></a></span>
            </h3>
            <div class="container-fluid">
                <div class="col-md-12">
                    <div class="BoxTypeGrey">
                        <div class="clearfix">
                            <div class="flex-row  align-items-center wrapTrue">
                                <div class="flex-item itemType  relative" onclick="GetBranchRequisiton();" id="divBranchRequisiton" runat="server">
                                    <div class="">
                                        <div class="valRound c10">
                                            <div class="showFullInfo" id="BranchRequisitonCou_F">0</div>
                                            <div id="BranchRequisitonCou">0</div>
                                        </div>
                                        <div class="hdTag">Branch Requisiton</div>
                                        <div class="smallmuted">Today</div>
                                    </div>
                                </div>
                                <div class="flex-item itemType  relative" onclick="GetPurchaseIndent();" id="divPurchaseIndent" runat="server">
                                    <div class="">
                                        <div class="valRound c4">
                                            <div class="showFullInfo" id="PurchaseIndentCou_F">0</div>
                                            <div id="PurchaseIndentCou">0</div>
                                        </div>
                                        <div class="hdTag">Purchase Indent</div>
                                        <div class="smallmuted">Today</div>
                                    </div>
                                </div>
                                <div class="flex-item itemType  relative" onclick="GetProjectIndent();" id="divProjectIndent" runat="server">
                                    <div class="">
                                        <div class="valRound c5">
                                            <div class="showFullInfo" id="ProjectIndentCou_F">0</div>
                                            <div id="ProjectIndentCou">0</div>
                                        </div>
                                        <div class="hdTag">Project Indent</div>
                                        <div class="smallmuted">Today</div>
                                    </div>
                                </div>
                                <div class="flex-item itemType  relative" onclick="GetPurchaseOrder();" id="divPurchaseOrder" runat="server">
                                    <div class="">
                                        <div class="valRound c6">
                                            <div class="showFullInfo" id="PurchaseOrderCou_F">0</div>
                                            <div id="PurchaseOrderCou">0</div>
                                        </div>
                                        <div class="hdTag">Purchase Order</div>
                                        <div class="smallmuted">Today</div>
                                    </div>
                                </div>
                                <div class="flex-item itemType  relative" onclick="GetProjectPurchaseOrder();" id="divProjectPurchaseOrder" runat="server">
                                    <div class="">
                                        <div class="valRound c8">
                                            <div class="showFullInfo" id="ProjectPurchaseOrderCou_F">0</div>
                                            <div id="ProjectPurchaseOrderCou">0</div>
                                        </div>
                                        <div class="hdTag">Project Purchase Order</div>
                                        <div class="smallmuted">Today</div>
                                    </div>
                                </div>
                                <div class="flex-item itemType  relative" onclick="GetSalesOrder();" id="divSalesOrder" runat="server">
                                    <div class="">
                                        <div class="valRound c11">
                                            <div class="showFullInfo" id="SalesOrderCou_F">0</div>
                                            <div id="SalesOrderCou">0</div>
                                        </div>
                                        <div class="hdTag">Sales Order</div>
                                        <div class="smallmuted">Today</div>

                                    </div>
                                </div>
                                <div class="flex-item itemType  relative" onclick="GetProjectSalesOrder();" id="divProjectSalesOrder" runat="server">
                                    <div class="">
                                        <div class="valRound">
                                            <div class="showFullInfo" id="ProjectSalesOrderCou_F">0</div>
                                            <div id="ProjectSalesOrderCou">0</div>
                                        </div>
                                        <div class="hdTag">Project Sales Order</div>
                                        <div class="smallmuted">Today</div>

                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="clearfix" id="detalsTable">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="shadowBox">
                                        <div class="bigHeading">Details</div>
                                        <div class="table-responsive">
                                            <table class="table styledTble rightGap">
                                                <thead>
                                                    <tr>
                                                        <th scope="col">Branch</th>
                                                        <th scope="col">Document No.</th>
                                                        <th scope="col">Date</th>
                                                        <th scope="col">Requested By</th>
                                                       <%-- <th scope="col">Approve</th>
                                                        <th scope="col">Reject</th>--%>
                                                    </tr>
                                                </thead>
                                                <tbody id="showTbleInfo">
                                                    <tr>
                                                        <td colspan="6" class="text-center">No Data Found</td>
                                                        <%--<td>
                                                            <button class="btn btn-success" id="btnApprove" onclick="ApproveClick();" >Approve</button>
                                                            <button class="btn btn-danger" id="btnReject" onclick="RejectClick();" >Reject</button>
                                                        </td>--%>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>





                        <%--                        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server"
                            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup" Height="630px"
                            Width="1200px" HeaderText="Quotation Approval" Modal="true" AllowResize="true" ResizingMode="Postponed">
                            <headertemplate>
                    <span>User Approval</span>
                </headertemplate>
                            <contentcollection>
                    <dxe:PopupControlContentControl runat="server">
                    </dxe:PopupControlContentControl>
                </contentcollection>
                        </dxe:ASPxPopupControl>--%>

                        <dxe:ASPxPopupControl ID="ASPXPopupControl2" runat="server"
                            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupdetails" Height="500px"
                            Width="1200px" HeaderText="Details" Modal="true" AllowResize="true" ResizingMode="Postponed">
                            <contentcollection>
                                <dxe:PopupControlContentControl runat="server">
                                </dxe:PopupControlContentControl>
                              </contentcollection>

                            <clientsideevents closeup="DetailsAfterHide" />
                        </dxe:ASPxPopupControl>


                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
