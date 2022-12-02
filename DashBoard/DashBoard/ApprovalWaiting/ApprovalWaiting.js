function reloadParent() {
    parent.document.location.href = '/oms/management/projectmainpage.aspx'
}

$(document).ready(function () {
    $('#detalsTable').hide();
    $('.relative').click(function (e) {
        $('#detalsTable').show()
    });
    getAllData();
});

function getAllData() {
    var dt = {};
    dt.action = "ALLCount";
    $.ajax({
        type: "POST",
        url: "ApprovalWaiting.aspx/GetAllCountData",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {

            var data = data.d[0];
            console.log(data.BranchRequisitonCount);
            $('#BranchRequisitonCou').html(data.BranchRequisitonCount);
            $('#PurchaseIndentCou').html(data.PurchaseIndentCount);
            $('#ProjectIndentCou').html(data.ProjectIndentCount);
            $('#PurchaseOrderCou').html(data.PurchaseOrderCount);
            $('#ProjectPurchaseOrderCou').html(data.ProjectPurchaseOrderCount);
            $('#SalesOrderCou').html(data.SalesOrderCount);
            $('#ProjectSalesOrderCou').html(data.ProjectSalesOrderCount);

            $('#BranchRequisitonCou_F').html(data.BranchRequisitonCount);
            $('#PurchaseIndentCou_F').html(data.PurchaseIndentCount);
            $('#ProjectIndentCou_F').html(data.ProjectIndentCount);
            $('#PurchaseOrderCou_F').html(data.PurchaseOrderCount);
            $('#ProjectPurchaseOrderCou_F').html(data.ProjectPurchaseOrderCount);
            $('#SalesOrderCou_F').html(data.SalesOrderCount);
            $('#ProjectSalesOrderCou_F').html(data.ProjectSalesOrderCount);
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function GetBranchRequisiton() {
    //$('.itemType').removeClass('active');
    //$('.one').addClass('active');
    var dt = {};
    dt.action = "PopulateERPDocApprovalPendingListByUserLevel";
    $.ajax({
        type: "POST",
        url: "ApprovalWaiting.aspx/GetBranchRequisiton",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var data = data.d;
            console.log('data', data)
            var html = '';
            if (data.length != 0) {

                for (i = 0; i < data.length; i++) {
                    //var email = data[i].EMAIL;
                    html += '<tr><td>' + data[i].Branch + '</td>'
                    html += '<td>' + data[i].DocumentNo + '</td>'
                    html += '<td>' + data[i].Date + '</td>'
                    html += '<td>' + data[i].RequestedBy + '</td>'
                  //  html += '<td><button class="btn btn-success" id="btnApprove" onclick="BRApproveClick(' + data[i].DocumnetId + ');" >Approve</button></td>'
                   // html += '<td><button class="btn btn-danger" id="btnReject" onclick="BRRejectClick(' + data[i].DocumnetId + ');" >Reject</button></td></tr>'
                }

            } else {
                html += '<tr><td colspan="5" class="text-center">No Data to Display</td></tr>'
            }
            $('#showTbleInfo').html(html);
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function GetPurchaseIndent() {
    //$('.itemType').removeClass('active');
    //$('.one').addClass('active');
    var dt = {};
    dt.action = "PopulateERPDocApprovalPendingListByUserLevel";
    $.ajax({
        type: "POST",
        url: "ApprovalWaiting.aspx/GetPurchaseIndent",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var data = data.d;
            console.log('data', data)
            var html = '';
            if (data.length != 0) {

                for (i = 0; i < data.length; i++) {
                    //var email = data[i].EMAIL;
                    html += '<tr><td>' + data[i].Branch + '</td>'
                    html += '<td>' + data[i].DocumentNo + '</td>'
                    html += '<td>' + data[i].Date + '</td>'
                    html += '<td>' + data[i].RequestedBy + '</td>'
                    //html += '<td><button class="btn btn-success" id="btnApprove" onclick="PIApproveClick(' + data[i].DocumnetId + ');" >Approve</button></td>'
                    //html += '<td><button class="btn btn-danger" id="btnReject" onclick="PIRejectClick(' + data[i].DocumnetId + ');" >Reject</button></td></tr>'
                }

            } else {
                html += '<tr><td colspan="5" class="text-center">No Data to Display</td></tr>'
            }
            $('#showTbleInfo').html(html);
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function GetProjectIndent() {
    //$('.itemType').removeClass('active');
    //$('.one').addClass('active');
    var dt = {};
    dt.action = "ProjectPopulateERPDocApprovalPendingListByUserLevel";
    $.ajax({
        type: "POST",
        url: "ApprovalWaiting.aspx/GetProjectIndent",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var data = data.d;
            console.log('data', data)
            var html = '';
            if (data.length != 0) {

                for (i = 0; i < data.length; i++) {
                    //var email = data[i].EMAIL;
                    html += '<tr><td>' + data[i].Branch + '</td>'
                    html += '<td>' + data[i].DocumentNo + '</td>'
                    html += '<td>' + data[i].Date + '</td>'
                    html += '<td>' + data[i].RequestedBy + '</td>'
                    //html += '<td><button class="btn btn-success" id="btnApprove" onclick="PPIApproveClick(' + data[i].DocumnetId + ');" >Approve</button></td>'
                    //html += '<td><button class="btn btn-danger" id="btnReject" onclick="PPIRejectClick(' + data[i].DocumnetId + ');" >Reject</button></td></tr>'
                }

            } else {
                html += '<tr><td colspan="5" class="text-center">No Data to Display</td></tr>'
            }
            $('#showTbleInfo').html(html);
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function GetPurchaseOrder() {
    //$('.itemType').removeClass('active');
    //$('.one').addClass('active');
    var dt = {};
    dt.action = "PopulateERPDocApprovalPendingListByUserLevel";
    $.ajax({
        type: "POST",
        url: "ApprovalWaiting.aspx/GetPurchaseOrder",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var data = data.d;
            console.log('data', data)
            var html = '';
            if (data.length != 0) {

                for (i = 0; i < data.length; i++) {
                    //var email = data[i].EMAIL;
                    html += '<tr><td>' + data[i].Branch + '</td>'
                    html += '<td>' + data[i].DocumentNo + '</td>'
                    html += '<td>' + data[i].Date + '</td>'
                    html += '<td>' + data[i].RequestedBy + '</td>'
                    //html += '<td><button class="btn btn-success" id="btnApprove" onclick="POApproveClick(' + data[i].DocumnetId + ');" >Approve</button></td>'
                    //html += '<td><button class="btn btn-danger" id="btnReject" onclick="PORejectClick(' + data[i].DocumnetId + ');" >Reject</button></td></tr>'
                }

            } else {
                html += '<tr><td colspan="5" class="text-center">No Data to Display</td></tr>'
            }
            $('#showTbleInfo').html(html);
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function GetProjectPurchaseOrder() {
    //$('.itemType').removeClass('active');
    //$('.one').addClass('active');
    var dt = {};
    dt.action = "ProjectPopulateERPDocApprovalPendingListByUserLevel";
    $.ajax({
        type: "POST",
        url: "ApprovalWaiting.aspx/GetProjectPurchaseOrder",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var data = data.d;
            console.log('data', data)
            var html = '';
            if (data.length != 0) {

                for (i = 0; i < data.length; i++) {
                    //var email = data[i].EMAIL;
                    html += '<tr><td>' + data[i].Branch + '</td>'
                    html += '<td>' + data[i].DocumentNo + '</td>'
                    html += '<td>' + data[i].Date + '</td>'
                    html += '<td>' + data[i].RequestedBy + '</td>'
                    //html += '<td><button class="btn btn-success" id="btnApprove" onclick="PPOApproveClick(' + data[i].DocumnetId + ');" >Approve</button></td>'
                    //html += '<td><button class="btn btn-danger" id="btnReject" onclick="PPORejectClick(' + data[i].DocumnetId + ');" >Reject</button></td></tr>'
                }

            } else {
                html += '<tr><td colspan="5" class="text-center">No Data to Display</td></tr>'
            }
            $('#showTbleInfo').html(html);
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function GetSalesOrder() {
    //$('.itemType').removeClass('active');
    //$('.one').addClass('active');
    var dt = {};
    dt.action = "PopulateERPDocApprovalPendingListByUserLevel";
    $.ajax({
        type: "POST",
        url: "ApprovalWaiting.aspx/GetSalesOrder",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var data = data.d;
            console.log('data', data)
            var html = '';
            if (data.length != 0) {

                for (i = 0; i < data.length; i++) {
                    //var email = data[i].EMAIL;
                    html += '<tr><td>' + data[i].Branch + '</td>'
                    html += '<td>' + data[i].DocumentNo + '</td>'
                    html += '<td>' + data[i].Date + '</td>'
                    html += '<td>' + data[i].RequestedBy + '</td>'
                    //html += '<td><button class="btn btn-success" id="btnApprove" onclick="SOApproveClick(' + data[i].DocumnetId + ');" >Approve</button></td>'
                    //html += '<td><button class="btn btn-danger" id="btnReject" onclick="SORejectClick(' + data[i].DocumnetId + ');" >Reject</button></td></tr>'
                }

            } else {
                html += '<tr><td colspan="5" class="text-center">No Data to Display</td></tr>'
            }
            $('#showTbleInfo').html(html);
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function GetProjectSalesOrder() {
    //$('.itemType').removeClass('active');
    //$('.one').addClass('active');
    var dt = {};
    dt.action = "ProjectPopulateERPDocApprovalPendingListByUserLevel";
    $.ajax({
        type: "POST",
        url: "ApprovalWaiting.aspx/GetProjectSalesOrder",
        data: JSON.stringify(dt),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            var data = data.d;
            console.log('data', data)
            var html = '';
            if (data.length != 0) {

                for (i = 0; i < data.length; i++) {
                    //var email = data[i].EMAIL;
                    html += '<tr><td>' + data[i].Branch + '</td>'
                    html += '<td>' + data[i].DocumentNo + '</td>'
                    html += '<td>' + data[i].Date + '</td>'
                    html += '<td>' + data[i].RequestedBy + '</td>'
                    //html += '<td><button class="btn btn-success" id="btnApprove" onclick="PSOApproveClick(' + data[i].DocumnetId + ');" >Approve</button></td>'
                    //html += '<td><button class="btn btn-danger" id="btnReject" onclick="PSORejectClick(' + data[i].DocumnetId + ');" >Reject</button></td></tr>'
                }

            } else {
                html += '<tr><td colspan="5" class="text-center">No Data to Display</td></tr>'
            }
            $('#showTbleInfo').html(html);
        },
        error: function (data) {
            console.log(data);
        }
    });
}

function BRApproveClick(obj) {
    var url = '';
    uri = '/OMS/Management/Activities/BranchRequisition.aspx?key=' + obj + '&status=2';
    popupdetails.SetContentUrl(uri);
    popupdetails.Show();
}

function BRRejectClick(obj) {
    var url = '';
    uri = '/OMS/Management/Activities/BranchRequisition.aspx?key=' + obj + '&status=3';
    popupdetails.SetContentUrl(uri);
    popupdetails.Show();
}

function PIApproveClick(obj) {
    var url = '';
    uri = '/OMS/Management/Activities/PurchaseIndent.aspx?key=' + obj + '&status=2';
    popupdetails.SetContentUrl(uri);
    popupdetails.Show();
}

function PIRejectClick(obj) {
    var url = '';
    uri = '/OMS/Management/Activities/PurchaseIndent.aspx?key=' + obj + '&status=3';
    popupdetails.SetContentUrl(uri);
    popupdetails.Show();
}

function PPIApproveClick(obj) {
    var url = '';
    uri = '/OMS/Management/Activities/BranchRequisition.aspx?key=' + obj + '&status=2';
    popupdetails.SetContentUrl(uri);
    popupdetails.Show();
}

function PPIRejectClick(obj) {
    var url = '';
    uri = '/OMS/Management/Activities/BranchRequisition.aspx?key=' + obj + '&status=3';
    popupdetails.SetContentUrl(uri);
    popupdetails.Show();
}

function POApproveClick(obj) {
    var url = '';
    uri = '/OMS/Management/Activities/PurchaseOrder.aspx?key=' + obj + '&status=2' + '&type=PO';
    popupdetails.SetContentUrl(uri);
    popupdetails.Show();
}

function PORejectClick(obj) {
    var url = '';
    uri = '/OMS/Management/Activities/PurchaseOrder.aspx?key=' + obj + '&status=3' + '&type=PO';
    popupdetails.SetContentUrl(uri);
    popupdetails.Show();
}

function PPOApproveClick(obj) {
    var url = '';
    uri = '/OMS/Management/Activities/ProjectPurchaseOrder.aspx?key=' + obj + '&type=PO&AppStat=ProjApprove';
    popupdetails.SetContentUrl(uri);
    popupdetails.Show();
}

function PPORejectClick(obj) {
    var url = '';
    uri = '/OMS/Management/Activities/ProjectPurchaseOrder.aspx?key=' + obj + '&type=PO&AppStat=ProjApprove';
    popupdetails.SetContentUrl(uri);
    popupdetails.Show();
}

function SOApproveClick(obj) {
    var url = '';
    uri = '/OMS/Management/Activities/SalesOrderAdd.aspx?key=' + obj + '&status=2' + '&type=SO' + '&isformApprove=YES';
    popupdetails.SetContentUrl(uri);
    popupdetails.Show();
}

function SORejectClick(obj) {
    var url = '';
    uri = '/OMS/Management/Activities/SalesOrderAdd.aspx?key=' + obj + '&status=3' + '&type=SO';
    popupdetails.SetContentUrl(uri);
    popupdetails.Show();
}

function PSOApproveClick(obj) {
    var url = '';
    uri = '/OMS/Management/Activities/ProjectOrder.aspx?key=' + obj + '&Permission=3&type=SO&type1=PO';
    popupdetails.SetContentUrl(uri);
    popupdetails.Show();
}

function PSORejectClick(obj) {
    var url = '';
    uri = '/OMS/Management/Activities/ProjectOrder.aspx?key=' + obj + '&Permission=3&type=SO&type1=PO';
    popupdetails.SetContentUrl(uri);
    popupdetails.Show();
}


function DetailsAfterHide(s, e) {
    popupdetails.Hide();
}