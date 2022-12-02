<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="qcReports.aspx.cs" Inherits="DashBoard.DashBoard.QcReportsLinks.qcReports" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quick reports</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" /> 
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,400i,500,600,700,800,900&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" />
   
    <script src="../Js/jquery3.3.1.min.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />
    <link href="../css/dashboard.css" rel="stylesheet" />
    <link href="../css/PMSStyles.css" rel="stylesheet" />
    
    <link href="qcReports.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            //JS script
            $('.mo-backdrop, .mpopup').hide();

            $('body').on('click', '.nav-link', function (e) {
                e.preventDefault();
                var pageTitle = $(this).find('.mName').text();
                $('.mo-backdrop, .mpopup').show();
                var link = $(this).data("remote");
                
                var button = $(e.relatedTarget);
                var modal = $(this);
                console.log(link);
                console.log(modal);
                // load content from HTML string
                //modal.find('.modal-body').html("Nice modal body baby...");

                // or, load content from value of data-remote url
                $('#addCnt').attr("src", link);
                $('.mpopup .mpopup-header .mTitle').text(pageTitle);
                
            });
            getReportsLinks();
            $('.mo-close').click(function (e) {
                $('.mo-backdrop, .mpopup').hide();
                $('#addCnt').html('Please load Reports');
            }); 
        });



        function getReportsLinks() {  
            $.ajax({
                type: "POST",
                url: "qcReports.aspx/GetReportsLink",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var retObj = msg.d;
                    console.log(retObj)

                    var toDoItem = "";
                    for (var i = 0; i < retObj.length; i++) {
                        toDoItem += "<li >"
                        toDoItem += "<a href='#' class='nav-link ' data-remote='../../" + retObj[i].Link + "?dashboard=1'>"
                        toDoItem += "<i class='fa fa-arrow-right' aria-hidden='true'></i><span class='mName' > " + retObj[i].Name + "</span>"
                        toDoItem +="</a></li>"      
                    }
                    console.log(toDoItem)
                    $('#getQuick').html(toDoItem);

                }
            });
        }
    </script>
     <script>
         function reloadParent() {
             parent.document.location.href = '/oms/management/projectmainpage.aspx'
         }
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
        <div class="mo-backdrop"></div>
        <div class="mpopup">
            <div class="mpopup-header">
                <span class="mTitle">Report Header</span>
                <span class="mo-close"><i class="fa fa-times"></i></span>
            </div>
            <div class="mpopup-content">
                <iframe id="addCnt" src="">
                    Reports Loading
                </iframe>
            </div>
        </div>


        <div class="clearfix">
            <div class="col-md-12 clearfix padding  bdBot">
                <h3 class="pull-left HeaderStyleCRM fontPop">Quick Reports </h3>
                    <span class="pull-right closeBtn">  <a href="#" onclick="reloadParent()"><i class="fa fa-times"></i></a></span>
            </div>
        </div>
        <div class="clearfix relative">
            <div class="gdt"></div>
            <div id="HelpDivid" class="helpDivClass" style="height: 90vh;">
                <span class="close-it pull-right" onclick="CloseHelpPopup()"><i class="fa fa-close"></i></span>
                <div class="flinkWrap" > 
                    <table> 
                        <tbody>
                            <tr> 
                                <td style="width:75%"> 
                                    <div class="pdRight20">
                                        An ERP dashboard is an easy-to-understand, visually intuitive graphical representation of key business
                                    performance metrics, easily accessed by users from a single screen. Color coded bar charts,
                                    graphs and other visual depictions of data give users a fast look at key metrics important to their functional area.
                                    </div>
                                    <table style="margin-top:15px">
                                        <tr>
                                            <td>
                                                
                                            </td>
                                            <td>
                                                
                                            </td>
                                        </tr>
                                    </table>
                                       
                                </td>
                                <td> 
                                    <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQhCfIVqGtr_4NtoF2KPnNL6zLhnuAyfXp6_Hu0rItBYb4VcxCh" /> 
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="listjs">
                    
                    <ul class="qclistInfo" id="getQuick">
                       
                    </ul>
                </div>
            </div>
        </div>

        <!-- Modal -->
        <div class="modal fade pmsModal w90" id="theModal"  role="dialog" aria-labelledby="theModallabel" aria-hidden="true">
          <div class="modal-dialog" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title" id="exampleModalLabel">Report</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div class="modal-body">
                 <iframe id="ltd"></iframe>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                <button type="button" class="btn btn-success">Save changes</button>
              </div>
            </div>
          </div>
        </div>
        
    </form>
</body>
</html>
