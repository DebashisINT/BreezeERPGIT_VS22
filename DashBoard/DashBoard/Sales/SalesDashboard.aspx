<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesDashboard.aspx.cs" Inherits="DashBoard.DashBoard.Sales.SalesDashboard" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales Analytics</title>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" /> 
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css"   crossorigin="anonymous" />

     
    <script src="../Js/jquery.3.3.1.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />
    <script src="../Js/SalesDb.js?v=0.9"></script>
     

    <script src="../Js/amC3/amcharts.js"></script>
    <script src="../Js/amC3/serial.js"></script>
    <script src="../Js/amC3/pie.js"></script>
    <script src="../Js/amC3/export.min.js"></script>
    <link href="../Js/amC3/export.css" rel="stylesheet" />
    <script src="../Js/amC3/light.js"></script>
    <link href="../Js/Swiper/swiper.min.css" rel="stylesheet" />
    <script src="../Js/Swiper/swiper.min.js"></script>
    

    <script>
        function reloadParent() {
            parent.document.location.href = '/oms/management/projectmainpage.aspx'
        }
    </script>

    <link href="SalesDashboard.css?1.0.1" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            $('a .white').click(function (e) {
                e.preventDefault();
            });

            $('[data-toggle="tooltip"]').tooltip();
            $('[data-toggle="popover"]').popover({ html: true });

            $('#TotSale').show().animate({ top: '0px', opacity: '1' });
            $('.zoom').click(function (e) {
                $('.arrowPointer').remove();
                $('.zoom').removeClass('DisableClass');
                var divid = $(this).attr('data-click');
                this.className = this.className + ' DisableClass';
                $('.panelClass').hide();
                $('.panelClass').css({ top: '50px', opacity: '0' });
                $('#' + divid).show();
                $('#' + divid).animate({ top: '0px', opacity: '1' });

                //this.children[0].children[0].innerHTML = this.children[0].children[0].innerHTML + '<i class="far fa-hand-point-left arrowPointer"></i>';
            });
            var swiper = new Swiper('.swiper-container', {
                slidesPerView: 'auto',
                centeredSlides: false,
                spaceBetween: 10,
                navigation: {
                    nextEl: '.snavNext',
                    prevEl: '.snavPrev',
                }
            });
            $('#TotSale').show().animate({ top: '0px', opacity: '1' });
        });


        function showCustomNewNotify(text) {
            var x = document.getElementById("CustomNewNotify");
            x.innerHTML = text;
            x.className = "show";
            setTimeout(function () { x.className = x.className.replace("show", ""); }, 6000);
        }


    </script>
    <link href="../css/dashboard.css" rel="stylesheet" />
    <link href="../css/SearchPopup.css" rel="stylesheet" />
    <script src="../js/SearchMultiPopup.js"></script>
    <script src="../../assests/pluggins/choosen/choosen.min.js"></script>
</head>
<body class="bodyBg">

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
        <script src="../Js/moment.min.js"></script>
        <%--<script src="../Js/SearchPopup.js"></script>--%>





        <div class="clearfix">


            <div class="col-md-12 clearfix padding  ">
                <h3 class="pull-left HeaderStyleCRM fontPop">Sales Analytics</h3>
                  <span class="pull-right closeBtn">  <a href="#" onclick="reloadParent()"><i class="fa fa-times"></i></a></span>
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
                            <td style="width:110px">

                                <dx:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                                    ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dx:ASPxDateEdit>
                            </td>
                    
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
                                <input type="button" class="btn btn-success" value="Show" onclick="RefreshWidget()" /></td>
                        </tr>
                    </table>
            </div>
        </div>
       
        <div id="CustomNewNotify">Some text some message..</div>

        <div class="clearfix form_main">
            <div class="col-md-12 relative">
            <div class="swNav snavPrev">
                <i class="fa fa-arrow-left"></i>
            </div>
           
            <div class="middl">
                <div class="swiper-container vTabWrap fontPop ">
                <div class="swiper-wrapper">
                <div class="swiper-slide">
                    <div class=" zoom DisableClass" runat="server" id="TotSalebtn" data-click="TotSale">
                    <div class="wdgLabel">Total Sale</div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom " runat="server" id="TotReceiptbtn" data-click="TotReceipt"  >
                        <div class="wdgLabel">Total Receipt</div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom " runat="server" id="TotDuebtn" data-click="TotDue">
                        <div class="wdgLabel">Total Due</div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom " runat="server" id="top10Smanbtn"  data-click="top10Sman"  >
                    <div class="wdgLabel">Top 10 Salesman</div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom " runat="server" id="top10Custbtn"   data-click="top10Cust" >
                    <div  class="wdgLabel">Top 10 Customer
                    </div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom " runat="server" id="newCustbtn"   data-click="newCust">
                    <div  class="wdgLabel">New Customer
                    </div>
                    </div>
                </div>
                <div class="swiper-slide">
                    <div class=" zoom " runat="server" id="TotOrdbtn"   data-click="TotOrd">
                    <div class="wdgLabel">Total Order
                    </div>
                    </div>
                </div>
            </div>
                </div>
            </div>
           
           <div class="swNav snavNext"><i class="fa fa-arrow-right"></i></div>
          </div>
           
         <div class="col-md-12" style="padding-top:25px;">

            <div class="col-md-12 panelClass " runat="server" id="TotSale" style="">
                <div class="card">

                      <div class="card-header clearfix">
                          <div style="width: 50%">
                              <span style="font-size:20px;cursor:pointer" onclick="ChangeviewtotSale()"  class="hidden-xs"> <i class="fa fa-arrows-alt"></i></span>
                          </div>
                          <div style="width: 50%">
                              <div class="pull-right mtop3">
                                  <label class="checkbox-label" title="Show Decimal">
                                    <input type="checkbox" id="chkSaleShowDecimal" onchange="totalSaleCheckChange()" />
                                    <span class="checkbox-custom rectangular"></span>
                                 </label>
                              </div>
                              <div class="pull-right glow salesAmt">
                                  <span>Total Sale</span>
                                  <span  id="TotsaleinInr"> 0</span> <span class="inrColor" id="lblCurrencySymbol" runat="server">(INR)</span>
                              </div>
                           </div> 
                    </div>
                     
                    <div class="card-body">
                        <div class="boxcontent chartboxes" id="TotSalePanel" style="width: 100%; height: 350px">
                            <div class="nochart">
                                    <h3 class="fontPop">No Data to Display</h3>
                                    <p class="fontPop">Please select time lenght to refresh data</p>
                                </div>
                            <div class="chartmsg " id="spanIdTotSale"></div>
                        </div>
                    </div>
                     <div class="card-header" style="min-height:3px"></div>
                </div>
                
                            
                
            </div>

            <div class="col-md-12 panelClass" runat="server" id="top10Sman" >
               <div class="card">

                    <div class="card-header">
                           <span style="font-size:20px;cursor:pointer" onclick="changeViewTopSman()"  > <i class="fa fa-arrows-alt"></i></span>
                       
                          <div class="pull-right glow salesAmt">
                              <span>Top 10 Salesman</span> 
                          </div>
                    </div>
                    
                   <div class="card-body"> 
                            <div class="boxcontent chartboxes" id="SalesManPanel" style="width: 100%; height: 350px"> 
                                <div class="nochart">
                                    <h3 class="fontPop">No Data to Display</h3>
                                    <p class="fontPop">Please enter Date and click on Show to generate the Chart.</p>
                                </div>
                                 <div class="chartmsg" id="spanidforSalesman"></div>
                            </div>
                       </div>
                   <div class="card-header" style="min-height:3px"></div>
                  </div> 
            </div>

            <div class="col-md-12 panelClass" runat="server" id="top10Cust" >
                <div class="card"> 
                <div class="card-header">
                           <span style="font-size:20px;cursor:pointer" onclick="changeViewNewcustZoom()"  > <i class="fa fa-arrows-alt"></i></span>
                            <div class="pull-right glow salesAmt">
                              <span>Top 10 Customer</span> 
                          </div>
                        
                    </div>
                 <div class="card-body">
                     <div class="boxcontent chartboxes" id="topNCustomer" style="width: 100%; height: 350px"> 
                             <div class="nochart">
                                    <h3 class="fontPop">No Data to Display</h3>
                                    <p class="fontPop">Enter Date and click on Show to generate the Chart.</p>
                                </div> 
                            <div class="chartmsg" id="spanidforcustomer">  </div>
                       </div> 
                    </div>
                     <div class="card-header" style="min-height:3px"></div>
                    </div>


                            
                   
            </div>

            <div class="col-md-12 panelClass" runat="server" id="TotOrd" >
                  <div class="card">
                    <div class="card-header">
                        <span style="font-size:20px;cursor:pointer" onclick="ChangeviewtotOrder()"  > <i class="fa fa-arrows-alt"></i></span>
                         <h4  class="pull-right glow">Total Order Value: 
                          <span  id="TotOrdinInr"> 0.00</span> <span class="inrColor" id="lblCurrencySymbol2" runat="server">(INR)</span> </h4>
                    </div>
                    <div class="card-body">
                        <div class="boxcontent chartboxes" id="TotOrdPanel" style="width: 100%; height: 350px">
                            <div class="nochart">
                                    <h3 class="fontPop">No Data to Display</h3>
                                    <p class="fontPop">Please enter Date and click on Show to generate the Chart.</p>
                                </div>
                          <div class="chartmsg" id="spanidfororderValue"> </div>
                        </div>
                    </div>
                      <div class="legrndBlock" id="ledgendbox" style="display:none">
                          <span class="squareLegend" style="background:#67b7dc"></span> 
                          <span >Order Value</span>
                           <span class="squareLegend" style="background:#fdd400"></span> 
                          <span >Invoice Value</span>
                      </div>
                     <div class="card-header" style="min-height:3px"></div>
                </div>
               
            </div>



            <div class="col-md-12 panelClass " runat="server" id="TotReceipt" style="">
                <div class="card">
                    <div class="card-header">
                           <span style="font-size:20px;cursor:pointer" onclick="ChangeviewTotReceipt()"  > <i class="fa fa-arrows-alt"></i></span>
                         <h4  class="pull-right glow">Total Receipt: 
                          <span  id="TotReceiptinInr"> 0</span> <span class="inrColor" id="lblCurrencySymbol3" runat="server" >(INR)</span> </h4> 
                    </div>
                    <div class="card-body">
                        <div class="boxcontent chartboxes" id="TotReceiptPanel" style="width: 100%; height: 350px">
                            <div class="nochart">
                                <h3 class="fontPop">No Data to Display</h3>
                                <p class="fontPop">Please enter Date and click on Show to generate the Chart.</p>
                            </div>
                            <div class="chartmsg" id="spanidfortotReceipt"> </div>
                        </div>
                    </div>
                     <div class="card-header" style="min-height:3px"></div>
                </div>
                
                            
                
            </div>




              <div class="col-md-12 panelClass " runat="server" id="TotDue" style="">
                <div class="card">
                    <div class="card-header" style="background:#a01f1f">
                           <span style="font-size:20px;cursor:pointer" onclick="ChangeviewTotDue()"  > <i class="fa fa-arrows-alt"></i></span>
                         <h4  class="pull-right glow">Total Due: 
                          <span  id="TotDueinInr"> 0.00</span> <span class="inrColor" id="lblCurrencySymbol4" runat="server">(INR)</span> </h4> 
                    </div>
                    <div class="card-body">
                        <div class="boxcontent chartboxes" id="TotDuePanel" style="width: 100%; height: 350px">
                            <div class="nochart">
                                <h3 class="fontPop">No Data to Display</h3>
                                <p class="fontPop">Please enter Date and click on Show to generate the Chart.</p>
                            </div>
                            <div class="chartmsg" id="spanidfortotaldue">  </div>
                        </div>
                    </div>
                     <div class="card-header" style="min-height:3px;background:#a01f1f"></div>
                </div>
                 
            </div>



            <div class="col-md-12 panelClass " runat="server" id="newCust" style="">
                <div class="card">
                    <div class="card-header" >
                           <span style="font-size:20px;cursor:pointer" onclick="ChangeviewTotnewCust()"  > <i class="fa fa-arrows-alt"></i></span>
                           <h4  class="pull-right glow">New Customer(s): 
                           <span  id="TotalCustCount"> 0</span> </h4> 
                    </div>
                    <div class="card-body">
                        <div class="boxcontent chartboxes" id="newCustPanel" style="width: 100%; height: 350px">
                            <div class="nochart">
                                <h3 class="fontPop">No Data to Display</h3>
                                <p class="fontPop">Please enter Date and click on Show to generate the Chart.</p>
                            </div>
                            <div class="chartmsg" id="spanidfornewCustomer"></div>
                        </div>
                    </div>
                     <div class="card-header" style="min-height:3px;"></div>
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


    </form>
</body>
</html>
