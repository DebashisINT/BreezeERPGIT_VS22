<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesDb.aspx.cs" Inherits="DashBoard.DashBoard.Sales.SalesDb" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" />

    
    <script src="../Js/jquery.3.3.1.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <script src="../Js/moment.min.js"></script>
    <script src="../Js/SalesDb.js"></script>
    <script src="../Js/amC3/amcharts.js"></script>
    <script src="../Js/amC3/serial.js"></script>
    <script src="../Js/amC3/pie.js"></script>
    <script src="../Js/amC3/export.min.js"></script>
    <link href="../Js/amC3/export.css" rel="stylesheet" />
    <script src="../Js/amC3/light.js"></script>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link href="../css/dashboard.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <%--<iframe style="width:100%;height:700px" 
            src="https://www.google.co.in/maps/dir/Dankuni,+West+Bengal/Jagadishpur+Bazaar,+Benaras+Rd,+Chamrail,+Howrah,+West+Bengal+711114/@22.6654678,88.2699903,14z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x39f8830efd94c899:0x4304d214888b1699!2m2!1d88.2970142!2d22.6809296!1m5!1m1!1s0x39f882571af592d7:0xcd65e7343fc12bee!2m2!1d88.2818533!2d22.6488084?hl=en" title=""></iframe>--%>
        <div class="col-md-12 clearfix padding bdBot">
            <h3 class="pull-left fontPop">Sales Dashboard</h3>
            <table class="pull-right flLeftTbl fontPop">
                <tr>
                    <td>From </td>
                    <td>

                        <dx:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>
                    <td>To </td>
                    <td>

                        <dx:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="ctoDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>
                    <td>
                        <input type="button" class="btn btn-success" value="Refresh" onclick="RefreshAll()" /></td>
                </tr>
            </table>

        </div>
        <div class="container-fluid" id="displayDiv">

            <div class="padding">
                <div class="row">
                    <div class="col-md-3" runat="server" id="TotSale" visible="false">
                        <div class="widget c1">
                            <div class="iconBox"><img src="../images/bx1.png" /></div>
                            <div class="textInbox">
                                <div class="wdgLabel">Total Sale</div>
                                <div class="wdgNumber" id="totSale">0.00</div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3" runat="server" id="TotSaleDue" visible="false">
                        <div class="widget c2">
                            <div class="iconBox"><img src="../images/bx1.png" /></div>
                            <div class="textInbox">
                                <div class="wdgLabel">Total Due</div>
                                <div class="wdgNumber" id="totDue">0.00</div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3" runat="server" id="TotAdvRcv" visible="false">
                        <div class="widget c3">
                            <div class="iconBox"><img src="../images/bx1.png" /></div>
                            <div class="textInbox">
                                <div class="wdgLabel">Total Advance Received</div>
                                <div class="wdgNumber" id="totAdvance">0.00</div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3" runat="server" id="TotOrder" visible="false">
                        <div class="widget c4">
                            <div class="iconBox"><img src="../images/bx1.png" /></div>
                            <div class="textInbox">
                                <div class="wdgLabel">Total Order</div>
                                <div class="wdgNumber" id="totOrder">0.00</div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6" runat="server" id="TopNSalesman" visible="false">
                        <div class="box"  id="topzSalesman">
                            <div class="clearfix">
                                <div class="col-md-8"><h4 class="fontPop colorlightDark">Top 10 Salesman</h4></div>
                                <div class="pull-right calltoActionicons" >
                                    <a href="#" onclick="LoadTopSaleman()" class=""><i class="fa fa-refresh"></i></a>
                                    <a href="#" onclick="changeViewTopProd()"  class="ful"> <i class="fa fa-arrows-alt"></i></a>
                                </div>
                            </div>
                            


                            <div class="boxcontent" id="SalesManPanel" style="width: 100%; height: 350px">
                                <%--content goes here--%>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6" runat="server" id="TopNCustomer" visible="false">
                        <div class="box" id="topzCustomer">
                            <div class="clearfix">
                                <div class="col-md-8 "><h4 class="fontPop colorlightDark">Top 10 Customer</h4></div>
                                <div class="pull-right calltoActionicons" >
                                    <a href="#" onclick="LoadCustomer()" class=""><i class="fa fa-refresh"></i></a>
                                    <a href="#" onclick="changeViewcustZoom()"  class="ful"> <i class="fa fa-arrows-alt"></i></a>
                                </div>
                            </div>
                            

                            <div class="boxcontent" id="topNCustomer" style="width: 100%; height: 350px">
                                <%--content goes here--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
