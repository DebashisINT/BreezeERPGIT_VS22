<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseDb.aspx.cs" Inherits="DashBoard.DashBoard.Purchase.PurchaseDb" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" />

    <script src="../Js/jquery.3.3.1.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <script src="../Js/moment.min.js"></script>
    <script src="../Js/PurchaseDb.js?1.0.5"></script>


    <script src="../Js/amC3/amcharts.js"></script>
    <script src="../Js/amC3/serial.js"></script>
    <script src="../Js/amC3/pie.js"></script>
    <script src="../Js/amC3/export.min.js"></script>
    <link href="../Js/amC3/export.css" rel="stylesheet" />
    <script src="../Js/amC3/light.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
       
    <link href="../css/dashboard.css" rel="stylesheet" />

     <link href="../css/SearchPopup.css" rel="stylesheet" />
    <script src="../js/SearchMultiPopup.js"></script>
    <script src="../../assests/pluggins/choosen/choosen.min.js"></script>
    <script>
        function reloadParent() {
            parent.document.location.href = '/oms/management/projectmainpage.aspx'
        }

      
    </script>
</head>
<body>

     <!--Product Modal -->
    <div class="modal fade" id="ProdModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Product Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Productkeydown(event)" id="txtProdSearch" width="100%" placeholder="Search By Product Name" />
                    <div id="ProductTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Code</th>
                                 <th>Name</th>
                                <th>HSN</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('ProductSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ProductSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>



    <form id="form1" runat="server">
        <div class="col-md-12 clearfix padding bdBot">
            <h3 class="pull-left fontPop">Purchase Analytics</h3>
            <span class="pull-right"><a href="#" onclick="reloadParent()" class="pageClose"><i class="fa fa-times"></i></a></span>
            <table class="pull-right flLeftTbl fontPop resposive_table">
                <tr>
                    <td>Branch</td>
                    <td>
                        
                        <dxe:ASPxComboBox ID="ddlBranch" runat="server" ClientInstanceName="cddlBranch" Width="100%">
                        </dxe:ASPxComboBox>             
                    </td>
                    <td style="width:55px"><div style="color: #b5285f;" class="clsFrom">
                            <asp:Label ID="Label5" runat="Server" Text="Class : " CssClass="mylabel1"></asp:Label>
                        </div></td>
                     <td>
                        
                        <div>
                            <dxe:ASPxButtonEdit ID="txtClass" runat="server" ReadOnly="true" ClientInstanceName="ctxtClass" Width="100%" TabIndex="5">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){ClassButnClick();}" KeyDown="function(s,e){Class_KeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                        </div>
                    </td>
                     <td style="width:60px">Product</td>
                    <td>
                       
                        <div>
                            <dxe:ASPxButtonEdit ID="txtProdName" runat="server" ReadOnly="true" ClientInstanceName="ctxtProdName" Width="100%" TabIndex="5">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){ProductButnClick();}" KeyDown="function(s,e){Product_KeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                        </div>
                    </td>      
                    <td>From </td>
                    <td style="width:110px">
                        <dx:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>
                    <td>To </td>
                    <td style="width:110px">
                        <dx:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                            ClientInstanceName="ctoDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                            <ButtonStyle Width="13px">
                            </ButtonStyle>
                        </dx:ASPxDateEdit>
                    </td>
                    <td>
                        <select class="form-control" id="dynamiDate" style="margin-bottom: 1px;">
                            <option value="0">Custom</option>
                            <option value="YTD">YTD</option>
                            <option value="QTD">QTD</option>
                            <option value="MTD">MTD</option>
                            <option value="WTD">WTD</option>
                        </select>
                    </td>
                    <td>
                        <input type="button" class="btn btn-success" value="Refresh" onclick="RefreshAll()" /></td>
                </tr>
            </table>

        </div>
        <div class="container-fluid">

            <div class="padding">

                <div class="row">
                    <div class="col-md-3" runat="server" id="TotPurchase" visible="false">
                        <div class="widget c1">
                            <div class="iconBox"><img src="../images/bx1.png" /></div>
                            <div class="textInbox">
                                <div class="wdgLabel">Total Purchase</div>
                                <div class="wdgNumber" id="totSale">0.00</div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3" runat="server" id="TotDue" visible="false">
                        <div class="widget c2">
                            <div class="iconBox"><img src="../images/bx9.png" /></div>
                            <div class="textInbox">
                                <div class="wdgLabel">Total Due</div>
                                <div class="wdgNumber" id="totDue">0.00</div>
                            </div>
                        </div> 
                    </div>
                    <div class="col-md-3" runat="server" id="TotPayment" visible="false">
                        <div class="widget c3">
                            <div class="iconBox"><img src="../images/bx6.png" /></div>
                            <div class="textInbox">
                                <div class="wdgLabel">Total Payment</div>
                                <div class="wdgNumber" id="totAdvance">0.00</div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3" runat="server" id="TotReturn" visible="false">
                        <div class="widget c4">
                            <div class="iconBox"><img src="../images/bx10.png" width="32px" /></div>
                            <div class="textInbox">
                                <div class="wdgLabel">Total Return</div>
                                <div class="wdgNumber" id="totOrder">0.00</div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6" runat="server" id="TopNItemByPurchase" visible="false">
                        <div class="box" id="topProd">
                            <div class="clearfix">
                                <div class="col-md-8 "><h4 class="fontPop colorlightDark">Top 10 Item by Purchase</h4></div>
                                <div class="pull-right calltoActionicons" >
                                    <a href="#" onclick="LoadTopSaleman()" class=""><i class="fa fa-refresh"></i></a>
                                    <a href="#" onclick="changeViewTopProd()"  class="ful"> <i class="fa fa-arrows-alt"></i></a>
                                </div>
                            </div>

                            <div class="boxcontent chartboxes"  id="PurchaseManPanel" style="width: 100%; height: 350px">
                                <%--content goes here--%>
                                <div class="nochart">
                                    <h3 class="fontPop">No Data to Display</h3>
                                    <p class="fontPop">Please select time lenght to refresh data</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6" runat="server" id="TopNVendor" visible="false">
                        <div class="box" id="topVend">
                            <div class="clearfix">
                                <div class="col-md-8 "><h4 class="fontPop colorlightDark">Top 10 Vendor</h4></div>
                                <div class="pull-right calltoActionicons" >
                                    <a href="#" onclick="LoadCustomer()" class=""><i class="fa fa-refresh"></i></a>
                                    <a href="#" onclick="changeViewTopVend()"  class="ful"> <i class="fa fa-arrows-alt"></i></a>
                                </div>
                            </div>

                            <div class="boxcontent chartboxes" id="topNCustomer" style="width: 100%; height: 350px">
                                <%--content goes here--%>
                                <div class="nochart">
                                    <h3 class="fontPop">No Data to Display</h3>
                                    <p class="fontPop">Please select time lenght to refresh data</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--Class Modal -->
    <div class="modal fade" id="ClassModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Class Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Classkeydown(event)" id="txtClassSearch" width="100%" placeholder="Search By Class Name" />
                    <div id="ClassTable">
                        <table border='1' width="100%">

                            <tr class="HeaderStyle" style="font-size:small">
                                <th>Select</th>
                                 <th class="hide">id</th>
                                <th>Class Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btnOkformultiselection btn-default"  onclick="DeSelectAll('ClassSource')">Deselect All</button>
                    <button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal" onclick="OKPopup('ClassSource')">OK</button>
                    <%--<button type="button" class="btnOkformultiselection btn-default" data-dismiss="modal">Close</button>--%>
                </div>
            </div>
        </div>
    </div>
      <asp:HiddenField ID="hdnClassId" runat="server" />
      <asp:HiddenField ID="hdnProductId" runat="server" />   
    <!--Class Modal -->
    </form>

    
    

</body>
</html>
