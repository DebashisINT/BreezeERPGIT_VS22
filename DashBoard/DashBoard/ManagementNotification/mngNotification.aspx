<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mngNotification.aspx.cs" Inherits="DashBoard.DashBoard.ManagementNotification.mngNotification" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
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

    <link href="../js/ckeditor/contents.css" rel="stylesheet" />
    <script src="../js/ckeditor/ckeditor.js"></script>
    
    <style>
        .itemType {
            margin-right:20px
        }
        .itemType.active{
            background: #fbffea;
            box-shadow: 0px 0px 5px 0px rgb(51 50 50 / 12%);
            transform: scale(1.1) translateY(2px);
        }
        
        .closeBtn.whd>a {
            color: #ccc;
        }
        .shwDet {
            cursor:pointer;
        }
        #popup_title {
            color:#fff !important;
        }
    </style> 
    <script src="mngNotification.js"></script>
   
</head>
<body>

    <!-- Modal -->
<div class="modal fade pmsModal w40" id="emailModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">Send Email</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <form>
              <div class="form-group">
                <label for="exampleInputEmail1">Email address</label>
                <input type="email" class="form-control" id="toEmail" aria-describedby="emailHelp" placeholder="" style="height:23px" />
              </div>
                <div class="form-group">
                <label for="">Subject Line</label>
                <input type="text" class="form-control" id="sub" aria-describedby="" value="" placeholder="" />
                
              </div>
              <div class="form-group">
                <label for="exampleInputPassword1">Message</label>
                <textarea class="form-control" height="100px"  name="msgbody" id="msgbody"></textarea>
              </div>   
         </form>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-primary" onclick="sendMail()" style="margin-bottom:0">Send Mail</button>
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
      </div>
    </div>
  </div>
</div>
    <form id="form1" runat="server">
        <h3 class="kpiHeading">
            Management Notification
            <span class="pull-right closeBtn whd"><a href="#" onclick="reloadParent()"><i class="fa fa-times"></i></a></span>
        </h3>
        <div class="container-fluid">
              <div class="col-md-12">
                  <div class="BoxTypeGrey">
                      <div class="clearfix">
                              <div class="flex-row  align-items-center">
                                  <div class="flex-item itemType relative shwDet one" onclick="getCustomer()" id="divCustomer" runat="server">
                                      <div class="">
                                          <div class="valRound">
                                              <span id="totalCust">0</span>
                                          </div>
                                          <div class="smallmuted">Today</div>
                                          <div class="hdTag">Customer </div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative shwDet two" onclick="GetAllVendor()" id="divVendor" runat="server">
                                      <div class="">
                                          <div class="valRound c1">
                                              <span id="totalVend">0</span>
                                          </div>
                                          <div class="smallmuted">Today</div>
                                          <div class="hdTag">Vendor </div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative shwDet three" onclick="GetAllEmployee()" id="divEmployee" runat="server">
                                      <div class="">
                                          <div class="valRound c2">
                                              <span id="totalEmp">0</span>
                                          </div>
                                          <div class="smallmuted">Today</div>
                                          <div class="hdTag">Employee</div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative shwDet four" onclick="GetAllInfluencer()" id="divInfluencer" runat="server">
                                      <div class="">
                                          <div class="valRound c3">
                                              <span id="totalInfl">0</span>
                                          </div>
                                          <div class="smallmuted">Today</div>
                                          <div class="hdTag">Influencer</div>
                                      </div>
                                  </div>
                                  <div class="flex-item itemType relative shwDet five" onclick="GetAllTransporter()" id="divTransporter" runat="server">
                                      <div class="">
                                          <div class="valRound c4">
                                              <span id="totalTrans">0</span>
                                          </div>
                                          <div class="smallmuted ">Today</div>
                                          <div class="hdTag">Transporter </div>
                                      </div>
                                  </div>
                              </div>
                          </div>

                      <br />
                      <div class="clearfix" id="detalsTable">
                        <div class="shadowBox">
                            <div class="bigHeading">Details</div>
                            <div class="table-responsive">
                                <table class="table styledTble">
                                    <thead>
                                    <tr>
                                        <th scope="col">Name </th>
                                        <th scope="col">Company </th>
                                        <th scope="col">Event Type</th>
                                        
                                        <th scope="col">SMS </th>
                                        <th scope="col">Email</th>
                                    </tr>
                                    </thead>
                                    <tbody id="datinTable">
                                        
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                  </div>
            </div>
            
            
        </div>
    </form>
</body>
</html>
