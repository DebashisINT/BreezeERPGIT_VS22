<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StbMangement.aspx.cs" Inherits="DashBoard.DashBoard.STBMng.StbMangement" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Poppins:300,400,400i,500,600,700,800,900&display=swap" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.3.1/jquery.js"></script>
    <script src="../Js/bootstrap.min.js"></script>
    <link href="../css/newtheme.css" rel="stylesheet" />
    <link href="/assests/css/custom/PMSStyles.css" rel="stylesheet" />

     <link href="/assests/pluggins/DataTable/jquery.dataTables.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/DataTable/jquery.dataTables.min.js"></script>
    <script src="/assests/pluggins/DataTable/dataTables.fixedColumns.min.js"></script>

    <style>
        .jumbrton, .jumbrton > h1, .jumbrton > h3, .infoitm > h5, .nType, .itm-menu > h4 {
            font-family: 'Poppins', sans-serif !important;
        }

        .jumbrton {
            background: #323eda;
            text-align: center;
            padding: 70px 0;
        }

            .jumbrton > h1 {
                color: #fff;
                font-size: 24px;
            }

        .mt-5, .my-5 {
            margin-top: 3rem !important;
        }

        .wrapFlex {
            flex-wrap: wrap;
            align-items: center;
        }

        .justify-content-center {
            justify-content: center;
        }

        .itmWidth {
            min-width: 128px;
            margin: 10px;
            cursor: pointer;
        }

        .itm-menu {
            padding: 15px;
            border-radius: 4px;
            text-align: center;
            font-family: 'Poppins', sans-serif;
        }

            .itm-menu > h4 {
                font-size: 15px;
                font-family: 'Poppins', sans-serif !important;
                color: #7e7979;
                font-weight: 500;
            }

        .icon-style {
            display: flex;
            margin: 0 auto;
            width: 70px;
            height: 70px;
            border-radius: 8px;
            background: #5345d8;
            justify-content: center;
            align-items: center;
            margin: 10px auto 20px auto;
            -webkit-transition: all 0.3s ease-in;
            transition: all 0.16s cubic-bezier(0.57, 1.87, 1, 1);
        }

        .c1 .icon-style {
            background: #5345d8;
            /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#635bd3+1,5345d8+100 */
            background: #635bd3; /* Old browsers */
        }

        .c2 .icon-style {
            background: #40d0b0;
        }

        .c3 .icon-style {
            /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#635bd3+1,a345d8+100 */
            background: #37b35d; /* Old browsers */
        }

        .c4 .icon-style {
            /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#a85fd8+1,f06838+100 */
            background: #8235b5; /* Old browsers */
        }

        .c5 .icon-style {
            /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#bbcea1+1,9ea369+100 */
            background: #7d9957; /* Old browsers */
        }

        .c6 .icon-style {
            /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#5f5687+0,3a3941+100 */
            background: #3a2a7a; /* Old browsers */
        }

        .c7 .icon-style {
            /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#22a57c+0,391c8b+99 */
            background: #3d55ad; /* Old browsers */
        }

        .c8 .icon-style {
            /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#d3949e+0,d63653+100 */
            background: #da203e; /* Old browsers */
        }

        .c9 .icon-style {
            /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#646edb+1,5345d8+100 */
            background: #5345d8; /* Old browsers */
        }

        .c10 .icon-style {
            /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#d6a062+2,133f96+100 */
            background: #d6a062; /* Old browsers */
        }

        .itmWidth:hover .itm-menu .icon-style {
            transform: scale(1.3);
            color: #fff;
            box-shadow: 0 3px 5px rgba(0,0,0,0.15);
        }

        .icon-style > img {
            max-width: 40px;
        }

        .clear {
            clear: both;
        }

        .info-item > .infoitm {
            background: #fff;
            border-radius: 9px;
            margin: 15px 15px 0 15px;
            padding: 10px 25px;
            border-bottom: 4px solid #d1d6d1;
        }

            .info-item > .infoitm > h5 {
                font-size: 15px;
            }

        .plBadge {
            background: #fff;
            border-radius: 10px;
            padding: 0 12px;
            color: #e33333;
            font-weight: 600;
            font-family: 'Poppins', sans-serif;
        }

        .totalG {
            border-bottom: 4px solid #24bc24 !important;
        }

        .totalR {
            border-bottom: 4px solid #e14f23 !important;
        }

        .nType {
            font-size: 24px;
            font-weight: 600;
            color: #4c4b4b;
        }
    </style>
    <style>
        .d-flex {
            display: -ms-flexbox !important;
            display: flex !important;
        }

        .d-inline-flex {
            display: -ms-inline-flexbox !important;
            display: inline-flex !important;
        }


        .mainDashBoxes {
            flex-wrap: wrap;
        }

            .mainDashBoxes .flex-itm {
                position: relative;
            }

                .mainDashBoxes .flex-itm:not(:last-child) {
                    margin-right: 16px;
                }

        @media only screen and (min-width: 889px) {
            .mainDashBoxes .flex-itm:not(:last-child) {
                margin-right: 16px;
            }
        }

        .widgBox {
            background: #fff;
            border-radius: 40px;
            padding: 20px;
            -webkit-transition: transform 0.3s ease-in-out;
            -moz-transition: transform 0.3s ease-in-out;
            transition: transform 0.3s ease-in-out;
            box-shadow: 0px 5px 20px rgba(0,0,0,0.23);
            cursor: pointer !important;
            margin-bottom: 15px;
            padding-bottom: 40px;
        }

            .widgBox .icon {
                width: 60px;
                height: 60px;
                display: flex;
                justify-content: center;
                align-items: center;
                background: #fff;
                border-radius: 50%;
                -webkit-box-shadow: 7px 8px 10px rgba(0, 0, 0, 0.22);
                box-shadow: 7px 8px 10px rgba(0, 0, 0, 0.22);
                font-size: 24px;
            }

            .widgBox .txt {
                font-size: 15px;
                padding-left: 10px;
                max-width: 114px;
                font-weight: 600;
                color: #8e8787;
                padding-top: 16px;
            }

            .widgBox .Numb {
                text-align: center;
                font-size: 4.9em;
                font-weight: 200;
                font-family: 'Roboto', sans-serif;
            }

                .widgBox .Numb + div {
                    font-size: 15px;
                }

            .widgBox .dwn {
                font-size: 26px;
                position: absolute;
                right: 18px;
                bottom: 40px;
            }

            .widgBox.active {
                background: #ee0067;
                -webkit-transform: scale(1.14);
                -moz-transform: scale(1.14);
                transform: scale(1.14);
                color: #fff;
            }

                .widgBox.active .txt {
                    color: #fff;
                }

            .widgBox .icon {
                color: #ffffff;
                background: #ee0067;
            }

            .widgBox.active .icon {
                background: #ee0067;
            }

            .widgBox.active.c2 {
                background: #0068bd;
                color: #fff;
            }

            .widgBox.c2 .icon {
                color: #ffffff;
                background: #0068bd;
            }

            .widgBox.active.c2 .icon {
                background: #0068bd;
            }

            .widgBox.active.c3 {
                background: #9c3dec;
                color: #fff;
            }

            .widgBox.c3 .icon {
                color: #ffffff;
                background: #9c3dec;
            }

            .widgBox.active.c3 .icon {
                background: #3eac3f;
            }

            .widgBox.active.c4 {
                background: #5b4a4a;
                color: #fff;
            }

            .widgBox.c4 .icon {
                color: #ffffff;
                background: #5b4a4a;
            }

            .widgBox.c5 .icon {
                color: #ffffff;
                background: #27bd24;
            }

            .widgBox.c6 .icon {
                color: #ffffff;
                background: #f14b2f;
            }

            .widgBox.active.c4 .icon {
                background: #5b4a4a;
            }

            .widgBox:not(.active):hover {
                -webkit-transform: scale(1.06);
                -moz-transform: scale(1.06);
                transform: scale(1.06);
            }

                .widgBox:not(.active):hover .arrD {
                    -moz-animation: bounce 1s infinite;
                    -webkit-animation: bounce 1s infinite;
                    animation: bounce 1s infinite;
                }

        .dpvsl {
            float: right;
        }

        .translate-y {
            -webkit-transform: translateY(4px);
            -moz-transform: translateY(4px);
            transform: translateY(4px);
            display: inline-block;
        }

        .translate-y-big {
            -webkit-transform: translateY(11px);
            -moz-transform: translateY(11px);
            transform: translateY(11px);
            display: inline-block;
        }

        .navRightDr > ul {
            display: inline-block;
            position: absolute;
            right: 25px;
            top: 18px;
        }

            .navRightDr > ul > li {
                float: left;
            }

                .navRightDr > ul > li .pl {
                    background: #fff;
                    border-radius: 4px;
                    padding: 5px 14px;
                    cursor: pointer;
                }

                    .navRightDr > ul > li .pl:hover {
                        background: #ebee21;
                    }

            .navRightDr > ul .dropdown > a {
                color: #fff;
                padding: 0;
                padding-left: 15px;
            }

                .navRightDr > ul .dropdown > a:hover,
                .navRightDr > ul .dropdown > a:focus,
                .navRightDr > ul .dropdown > a:active,
                .navRightDr > ul .dropdown > a:visited {
                    background: transparent !important;
                    background-color: transparent !important;
                }

        .navRightDr .dropdown-menu.dropdown-menu-right {
            left: auto;
            right: 0;
            border: none;
            min-width: 350px;
            top: 130%;
        }

        .navRightDr .dropdown-menu .dropdown-header {
            background: #FFF;
            padding: 15px;
            position: relative;
            text-align: center;
            color: #747F8B;
            font-weight: bold;
            border-radius: 10px 10px 0 0;
            border: 0px;
            border-style: solid;
            border-bottom-width: 1px;
            -moz-border-image: -moz-linear-gradient(right, white, #cedae0, #cedae0, white) 1 20%;
            -o-border-image: -o-linear-gradient(right, white, #cedae0, #cedae0, white) 1 20%;
            border-image: linear-gradient(to right, white 0%, #cedae0 40%, #cedae0 60%, white 100%) 1 20%;
            box-shadow: 0px 2px 10px -2px #cedae0;
        }

            .navRightDr .dropdown-menu .dropdown-header .triangle {
                position: absolute;
                top: -10px;
                right: 7px;
                height: 18px;
                width: 18px;
                border-radius: 4px 0px 0px 0px;
                transform: rotate(45deg);
                background: #FFF;
            }

        .navRightDr .dropdown-menu .dropdown-body {
            padding-top: 12px;
        }

        /* for pop */
        .popupWraper {
            position: fixed;
            top: 0;
            left: 0;
            height: 100vh;
            width: 100%;
            background: rgba(0,0,0,0.85);
            z-index: 10;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .popBox {
            width: 670px;
            background: #fff;
            padding: 35px;
            text-align: center;
            min-height: 350px;
            display: flex;
            align-items: center;
            flex-direction: column;
            justify-content: center;
            background: #fff url("../../images/popupBack.png") no-repeat top left;
            box-shadow: 0px 14px 14px rgba(0,0,0,0.56);
        }

            .popBox h1, .popBox p {
                font-family: 'Poppins', sans-serif !important;
                margin-bottom: 20px !important;
            }

            .popBox p {
                font-size: 15px;
            }

        .btn-sign {
            background: #3680fb;
            color: #fff;
            padding: 10px 25px;
            box-shadow: 0px 5px 5px rgba(0,0,0,0.22);
        }

            .btn-sign:hover {
                background: #2e71e1;
                color: #fff;
            }
    </style>
    <style>
        .fontPp {
            font-family: Poppins, sans-serif;
        }

        .stbDbBox {
            background: #fff;
            border-radius: 5px;
            width:22%;
        }

        .mr-5 {
            margin-right: 20px;
        }

        .bx-footer {
            padding: 8px 15px;
            font-weight: 500;
            font-size: 14px;
            border-top: 1px dashed #ddd;
            margin-top: 5px;
        }

        .bx-cont {
            min-height: 55px;
            font-family: Poppins, sans-serif;
        }

        .bqBox {
            background: #afaf29;
            display: flex;
            width: 45px;
            height: 45px;
            text-align: center;
            justify-content: center;
            align-items: center;
            margin: 11px 10px 0 15px;
            border-radius: 50%;
        }

            .bqBox.c1 {
                background: #27bd24;
            }

            .bqBox.c2 {
                background: #0068bd;
            }

            .bqBox.c3 {
                background: #9c3dec;
            }

            .bqBox.c4 {
                background: #5b4a4a;
            }

            .bqBox.c5 {
                background: #f14b2f;
            }

            .bqBox.c6 {
                background: #ee0067;
            }

            .bqBox.c7 {
                background: #22D195;
            }

            .bqBox.c8 {
                background: #3F3771;
            }

            .bqBox.c9 {
                background: #E7AA18;
            }

        .bx-amt {
            font-size: 32px;
        }

        .bx-muted {
            font-size: 11px;
            text-align: left;
            margin: 5px 0 0 0;
        }

        .linkBoxes > a {
            margin-right: 2.8rem;
            justify-content: center;
            display: flex;
            flex-direction: column;
        }

            .linkBoxes > a:focus {
                text-decoration: none;
            }

        .linkBox {
            background: #323eda;
            display: flex;
            margin: 10px auto;
            width: 65px;
            height: 65px;
            justify-content: center;
            align-items: center;
            border-radius: 50%;
            transition: all 0.3s ease;
            box-shadow: 0 0 0 4px #e6e6e6, 0 0 0 5px #929292;
        }

        .linkBoxes > a:hover .linkBox {
            transform: scale(1.2);
        }

        .linkBox.c1 {
            background: #7da042;
        }

        .linkBox.c2 {
            background: #db6400;
        }

        .linkBox.c3 {
            background: #3b9c5b;
        }

        .linkBox.c4 {
            background: #944e6c;
        }

        .linkBox.c5 {
            background: #433d3c;
        }

        .linkBox.c6 {
            background: #583d72;
        }

        .linkBox.c7 {
            background: #e8643f;
        }

        .linkBox.c8 {
            background: #ff4646;
        }

        .linkBox.c9 {
            background: #045762;
        }

        .linkBox.c10 {
            background: #16697a;
        }

        .linkText {
            font-family: 'Poppins';
            color: #333;
            font-size: 14px;
            text-align: center;
        }
        svg path{
          fill:red
        }
    </style>
    <script>
        $(document).ready(function () {

            //$('.itmWidth').mouseover(function () {
            //    var iconTag = $(this).find('.icon-style img');
            //    var dataTag = $(this).find('.icon-style>img').attr('data-in');
            //    //alert(dataTag)
            //    $(this).find('.icon-style img').attr('src', dataTag);
            //})
            //$('.itmWidth').mouseout(function () {
            //    var iconTag = $(this).find('.icon-style img');
            //    var dataTag = $(this).find('.icon-style>img').attr('data-out');
            //    //alert(dataTag)
            //    $(this).find('.icon-style img').attr('src', dataTag);
            //})
            $('#dataTable').DataTable({
                scrollX: true,
                fixedColumns: {
                    rightColumns: 2
                }
            });
        });

        function SwitchtoERP() {
            var OtherDetails = {}
            OtherDetails.comment = "ERP";
            $.ajax({
                type: "POST",
                url: "StbMangement.aspx/STBSession",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    parent.document.location.href = '/oms/management/projectmainpage.aspx'
                }
            });


        }

        function PopupOk() {
            $('#divPopHead').addClass("hide");
        }

        function ShowOpup() {
            $.ajax({
                type: "POST",
                url: "StbMangement.aspx/AnnouncementDetails",
                data: JSON.stringify({ reqStr: "" }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list != null && list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var head = '';
                            var body = '';
                            head = list[i].split('|')[0];
                            body = list[i].split('|')[1];
                        }
                        $("#h1heading").text(head);
                        $("#pParagraph").text(body);
                        $("#divPopHead").removeClass("hide");
                    }
                    else {
                        $("#h1heading").text('');
                        $("#pParagraph").text('No notification for today.');
                        $('#divPopHead').removeClass("hide");
                    }
                }
            });
        }

        //function GOtoERP() {
        //    parent.document.location.href = '/OMS/management/ProjectMainPage.aspx';
        //}


        function ReceiptClick() {
            parent.document.location.href = '/STBManagement/MoneyReceipt/index.aspx';
        }

        function WalletRechargeClick() {
            parent.document.location.href = '/STBManagement/WalletRecharge/index.aspx';
        }

        function STBRequisitionClick() {
            parent.document.location.href = '/STBManagement/Requisition/STBRequisition.aspx';
        }

        function STBReqReturnClick() {
            parent.document.location.href = '/STBManagement/ReturnRequisition/ReturnRequisitionList.aspx';
        }

        function ApprovalClick() {
            parent.document.location.href = '/STBManagement/Approval/index.aspx';
        }

        function InventoryClick() {
            parent.document.location.href = '/STBManagement/STBInventory/STBInventory.aspx';
        }

        function SearchClick() {
            parent.document.location.href = '/STBManagement/Search/search.aspx';
        }

        function ReportsClick() {
            parent.document.location.href = '/Reports/GridReports/stbregister.aspx';
        }

        function ReturnDispatchClick() {
            parent.document.location.href = '/STBManagement/ReturnDispatch/ReturnDispatch.aspx';
        }

        //STB Scheme 

        function SchemeReceivedClick() {
            parent.document.location.href = '/STBManagement/STBSchemeReceived/STBSchemeReceivedList.aspx';
        }

        function SchemeDirApprovalClick() {
            parent.document.location.href = '/STBManagement/STBSchemeDirectorApproval/SchemeDirectorApproval.aspx';
        }

        function SchemeReqCloseClick() {
            parent.document.location.href = '/STBManagement/STBSchemeRequisition/STBSchemeRequisitionList.aspx';
        }

        function SchemeSearchClick() {
            parent.document.location.href = '/STBManagement/STBSchemeSearch/STBSchemeSearch.aspx';
        }

        function SchemeRegisterClick() {
            parent.document.location.href = '/Reports/GridReports/STBSchemeRegister.aspx';
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('body').click(function (e) {

                $('.sidenav ul.dropdown-menu').hide();
                $('.sidenav>ul>li a').removeClass('active');
            });
        })
    </script>
    <style>
       .dTable {
            width: 100%;
            border: 1px solid #442ebd;
        }
        .dTable>tbody>tr>td, .dTable>thead>tr>td {
            padding:3px 5px;
        }
        .dTable>thead>tr>td{
            background: #442ebd;
            color: #fff;
            border-bottom: 1px solid #cedae0;
            padding: 8px 5px;
        }
    </style>

    <script>
        function STBRequisitionFinPending_Click() {
            $("#lblheaderId").text('TB Requisition (Fin Pending)');
            var status = "<table class='table table-striped table-bordered tableStyle' id='dataTable'>";
            status = status + " <thead><tr>";
            status = status + " <td>Req. No</td><td>Req. Date</td><td>Location </td>";
            status = status + " <td>Entity Code</td><td>Qty</td><td>Hold By</td><td>Director</td><td>Model</td></tr></thead>";
            status = status + " </table>";
            $('#DivHistoryTable').html(status);
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: "StbMangement.aspx/ShowDashboardDetails",
                    data: JSON.stringify({ report: "STBRequisitionFinPending" }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $('#dataTable').DataTable().destroy();
                        $("#rhModal").modal('show');
                        setTimeout(function () {
                            $('#dataTable').DataTable({
                                bDestroy: true,
                                data: msg.d,
                                columns: [
                                   { 'data': 'ReqNo' },
                                   { 'data': 'ReqDate' },
                                   { 'data': 'Location' },
                                   { 'data': 'EntityCode' },
                                   { 'data': 'Qty' },
                                   { 'data': 'HoldBy' },
                                   { 'data': 'Director' },
                                   { 'data': 'Model' }
                                ],
                                error: function (error) {
                                    alert(error);
                                }
                            });
                        }, 500);
                    }
                });
            }, 1000);
        }

        function STBRequisitionDirPending_Click() {
            $("#lblheaderId").text('STB Requisition (Dir Pending)');
            var status = "<table class='table table-striped table-bordered tableStyle' id='dataTable'>";
            status = status + " <thead><tr>";
            status = status + " <td>Req. No</td><td>Req. Date</td><td>Location </td>";
            status = status + " <td>Entity Code</td><td>Qty</td><td>Hold By</td><td>Director</td><td>Model</td></tr></thead>";
            status = status + " </table>";
            $('#DivHistoryTable').html(status);
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: "StbMangement.aspx/ShowDashboardDetails",
                    data: JSON.stringify({ report: "STBRequisitionDirPending" }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $('#dataTable').DataTable().destroy();
                        $("#rhModal").modal('show');
                        setTimeout(function () {
                            $('#dataTable').DataTable({
                                bDestroy: true,
                                data: msg.d,
                                columns: [
                                   { 'data': 'ReqNo' },
                                   { 'data': 'ReqDate' },
                                   { 'data': 'Location' },
                                   { 'data': 'EntityCode' },
                                   { 'data': 'Qty' },
                                   { 'data': 'HoldBy' },
                                   { 'data': 'Director' },
                                   { 'data': 'Model' }
                                ],
                                error: function (error) {
                                    alert(error);
                                }
                            });
                        }, 500);
                    }
                });
            }, 1000);
        }

        function STBRequisitionOnHold_Click() {
            $("#lblheaderId").text('STB Requisition (On Hold)');
            var status = "<table class='table table-striped table-bordered tableStyle' id='dataTable'>";
            status = status + " <thead><tr>";
            status = status + " <td>Req. No</td><td>Req. Date</td><td>Location </td>";
            status = status + " <td>Entity Code</td><td>Qty</td><td>Hold By</td><td>Director</td><td>Model</td></tr></thead>";
            status = status + " </table>";
            $('#DivHistoryTable').html(status);
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: "StbMangement.aspx/ShowDashboardDetails",
                    data: JSON.stringify({ report: "STBRequisitionOnHold" }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $('#dataTable').DataTable().destroy();
                        $("#rhModal").modal('show');
                        setTimeout(function () {
                            $('#dataTable').DataTable({
                                bDestroy: true,
                                data: msg.d,
                                columns: [
                                   { 'data': 'ReqNo' },
                                   { 'data': 'ReqDate' },
                                   { 'data': 'Location' },
                                   { 'data': 'EntityCode' },
                                   { 'data': 'Qty' },
                                   { 'data': 'HoldBy' },
                                   { 'data': 'Director' },
                                   { 'data': 'Model' }
                                ],
                                error: function (error) {
                                    alert(error);
                                }
                            });
                        }, 500);
                    }
                });
            }, 1000);
        }

        function InventoryPending_Click() {
            $("#lblheaderId").text('Inventory Pending');
            var status = "<table class='table table-striped table-bordered tableStyle' id='dataTable'>";
            status = status + " <thead><tr>";
            status = status + " <td>Req. No</td><td>Req. Date</td><td>Location </td>";
            status = status + " <td>Entity Code</td><td>Qty</td><td>Hold By</td><td>Director</td><td>Model</td></tr></thead>";
            status = status + " </table>";
            $('#DivHistoryTable').html(status);
            setTimeout(function () {
                $.ajax({
                    type: "POST",
                    url: "StbMangement.aspx/ShowDashboardDetails",
                    data: JSON.stringify({ report: "InventoryPending" }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $('#dataTable').DataTable().destroy();
                        $("#rhModal").modal('show');
                        setTimeout(function () {
                            $('#dataTable').DataTable({
                                bDestroy: true,
                                data: msg.d,
                                columns: [
                                   { 'data': 'ReqNo' },
                                   { 'data': 'ReqDate' },
                                   { 'data': 'Location' },
                                   { 'data': 'EntityCode' },
                                   { 'data': 'Qty' },
                                   { 'data': 'HoldBy' },
                                   { 'data': 'Director' },
                                   { 'data': 'Model' }
                                ],
                                error: function (error) {
                                    alert(error);
                                }
                            });
                        }, 500);
                    }
                });
            }, 1000);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Modal -->
        <div id="rhModal" class="modal pmsModal fade" role="dialog">
          <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal">&times;</button>
                <h4 class="modal-title"><label id="lblheaderId"></label> <%--Header--%></h4>
              </div>
              <div class="modal-body" id="DivHistoryTable">
                <table class="dTable">
                    <thead>
                        <tr>
                            <td>Req. No</td>
                            <td>Req. Date</td>
                            <td>Location </td>
                            <td>Entity Code</td>
                            <td>Qty</td>
                            <td>Hold By</td>
                            <td>Director</td>
                            <td>Model</td>
                        </tr>
                    </thead>
                   <%-- <tbody>
                        <tr>
                            <td>a</td>
                            <td>b</td>
                            <td>c </td>
                            <td>d</td>
                            <td>e</td>
                        </tr>
                    </tbody>--%>
                </table>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
              </div>
            </div>
          </div>
        </div>
        <div class="dashboardWraper">
            <div class="jumbrton" style="padding-bottom: 20px; padding-top: 8px; border-radius: 15px">
                <div class="navRightDr">
                    <ul class="nav">
                        <li class="">
                            <div class="pl" onclick="SwitchtoERP()">
                                <img src="../images/arrow.png" style="width: 16px; margin-right: 5px" />Switch to ERP</div>
                        </li>
                        <li class="nav-item dropdown relative">
                            <a class="nav-link text-light " href="javascript:void(0)" id="navbarDropdown" role="button" onclick="ShowOpup()">
                                <img src="../images/notification.png" style="width: 26px" />
                            </a>
                        </li>
                    </ul>
                </div>
                <h1 style="margin-bottom: 22px !important; text-transform: uppercase">STB Management</h1>
                <h3 style="color: #c4c4c4; margin-bottom: 20px !important"></h3>
                <div class="clearfix">
                    <div class="d-flex justify-content-center stbBoxes">
                        <div class="flex-itm stbDbBox mr-5" onclick="STBRequisitionFinPending_Click();" style="cursor:pointer">
                            <div class="">
                                <div class="bx-cont">
                                    <div class="media d-flex">
                                        <div class="media-left " style="display: flex; justify-content: center; align-items: center;">
                                            <div class="bqBox c5">
                                                <img src="../images/server.svg" class="media-object" style="max-width: 25px" /></div>
                                        </div>
                                        <div class="media-body">
                                            <div class="bx-muted">Total </div>
                                            <div class="bx-amt" runat="server" id="DivSTBRequisitionFinPending">0</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="bx-footer">STB Requisition(Fin Pending)
                                </div>
                            </div>
                        </div>
                        <div class="flex-itm stbDbBox mr-5" onclick="STBRequisitionDirPending_Click();" style="cursor:pointer">
                            <div class="">
                                <div class="bx-cont">
                                    <div class="media d-flex">
                                        <div class="media-left" style="display: flex; justify-content: center; align-items: center;">
                                            <div class="bqBox c1">
                                                <img src="../images/server.svg" class="media-object" style="max-width: 25px" /></div>
                                        </div>
                                        <div class="media-body">
                                            <div class="bx-muted">Total </div>
                                            <div class="bx-amt" runat="server" id="DivSTBRequisitionDirPending">0</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="bx-footer">STB Requisition
                                   (Dir Pending)
                                </div>
                            </div>
                        </div>
                        <div class="flex-itm stbDbBox mr-5" onclick="STBRequisitionOnHold_Click();" style="cursor:pointer">
                            <div class="">
                                <div class="bx-cont">
                                    <div class="media d-flex">
                                        <div class="media-left" style="display: flex; justify-content: center; align-items: center;">
                                            <div class="bqBox c2">
                                                <img src="../images/server.svg" class="media-object" style="max-width: 25px" /></div>
                                        </div>
                                        <div class="media-body">
                                            <div class="bx-muted">Total </div>
                                            <div class="bx-amt" runat="server" id="DivSTBRequisitionOnHold">0</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="bx-footer">STB Requisition
                                    (On Hold)
                                </div>
                            </div>
                        </div>
                        <div class="flex-itm stbDbBox" onclick="InventoryPending_Click();" style="cursor:pointer">
                            <div class="">
                                <div class="bx-cont">
                                    <div class="media d-flex">
                                        <div class="media-left" style="display: flex; justify-content: center; align-items: center;">
                                            <div class="bqBox c3">
                                                <img src="../images/006-storage.png" class="media-object" style="max-width: 25px" /></div>
                                        </div>
                                        <div class="media-body">
                                            <div class="bx-muted">Total </div>
                                            <div class="bx-amt" runat="server" id="DivInventoryPending">0</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="bx-footer">Inventory Pending</div>
                            </div>
                        </div>
                        
                    </div>
                    <div class="d-flex justify-content-center stbBoxes" style="margin-top: 15px">
                        <div class="flex-itm stbDbBox mr-5">
                            <div class=" ">
                                <div class="bx-cont">
                                    <div class="media d-flex">
                                        <div class="media-left" style="display: flex; justify-content: center; align-items: center;">
                                            <div class="bqBox c4">
                                                <img src="../images/004-return.png" class="media-object" style="max-width: 25px" /></div>
                                        </div>
                                        <div class="media-body">
                                            <div class="bx-muted">Total </div>
                                            <div class="bx-amt" runat="server" id="DivReturnReqPendingBranch">0</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="bx-footer">Return Req. Pending (Branch)</div>
                            </div>
                        </div>
                        <div class="flex-itm stbDbBox mr-5">
                            <div class="">
                                <div class="bx-cont">
                                    <div class="media d-flex">
                                        <div class="media-left " style="display: flex; justify-content: center; align-items: center;">
                                            <div class="bqBox c6">
                                                <img src="../images/server.svg" class="media-object" style="max-width: 25px" /></div>
                                        </div>
                                        <div class="media-body">
                                            <div class="bx-muted">Total </div>
                                            <div class="bx-amt" runat="server" id="DivReturnReqPendingHO">0</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="bx-footer">Return Req. Pending (HO)</div>
                            </div>
                        </div>
                        <div class="flex-itm stbDbBox mr-5 hide">
                            <div class="">
                                <div class="bx-cont">
                                    <div class="media d-flex">
                                        <div class="media-left" style="display: flex; justify-content: center; align-items: center;">
                                            <div class="bqBox c7">
                                                <img src="../images/002-wallet.png" class="media-object" style="max-width: 25px" /></div>
                                        </div>
                                        <div class="media-body">
                                            <div class="bx-muted">Total </div>
                                            <div class="bx-amt" runat="server" id="DivWalletRechargeOpenCash">0</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="bx-footer">Wallet Recharge open (Cash)</div>
                            </div>
                        </div>
                        <div class="flex-itm stbDbBox mr-5 hide">
                            <div class="">
                                <div class="bx-cont">
                                    <div class="media d-flex">
                                        <div class="media-left" style="display: flex; justify-content: center; align-items: center;">
                                            <div class="bqBox c8">
                                                <img src="../images/002-wallet.png" class="media-object" style="max-width: 25px" /></div>
                                        </div>
                                        <div class="media-body">
                                            <div class="bx-muted">Total </div>
                                            <div class="bx-amt" runat="server" id="DivWalletRechargeOpenCheque">0</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="bx-footer">Wallet Recharge open (Cheque)</div>
                            </div>
                        </div>
                        <div class="flex-itm stbDbBox mr-5">
                            <div class="">
                                <div class="bx-cont">
                                    <div class="media d-flex">
                                        <div class="media-left" style="display: flex; justify-content: center; align-items: center;">
                                            <div class="bqBox c9">
                                                <img src="../images/002-wallet.png" class="media-object" style="max-width: 25px" /></div>
                                        </div>
                                        <div class="media-body">
                                            <div class="bx-muted">Total </div>
                                            <div class="bx-amt" runat="server" id="DivWalletRechargeCancelReq">0</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="bx-footer">Wallet Recharge cancel req.</div>
                            </div>
                        </div>
                        <div class="flex-itm stbDbBox">
                            <div class=" ">
                                <div class="bx-cont">
                                    <div class="media d-flex">
                                        <div class="media-left" style="display: flex; justify-content: center; align-items: center;">
                                            <div class="bqBox c10">
                                                <img src="../images/paperW.png" class="media-object" style="max-width: 25px" />
                                            </div>
                                        </div>
                                        <div class="media-body">
                                            <div class="bx-muted">Total </div>
                                            <div class="bx-amt" runat="server" id="DivReceiptCancelReq">250</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="bx-footer">Receipt cancel req.</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="container mt-5 " style="margin-top: 20px;padding-bottom:55px">
            <div class="d-flex wrapFlex justify-content-center linkBoxes">
                <a href="#" id="DivReceipt" runat="server" onclick="ReceiptClick();">
                    <div class="linkBox c1">
                        <img src="../images/paperW.png" style="width: 26px" /></div>
                    <div class="linkText">Money Receipt</div>
                </a>
                <a href="#" id="DivWalletRecharge" runat="server" onclick="WalletRechargeClick();">
                    <div class="linkBox c9">
                        <img src="../images/002-wallet.png" style="width: 26px" /></div>
                    <div class="linkText">Wallet Recharge</div>
                </a>
                <a href="#" id="DivSTBRequisition" runat="server" onclick="STBRequisitionClick();">
                    <div class="linkBox c3">
                        <img src="../images/005-smart-tv.png" style="width: 26px" /></div>
                    <div class="linkText">STB Requisition</div>
                </a>
                <a href="#" id="DivSTBReqReturn" runat="server" onclick="STBReqReturnClick();">
                    <div class="linkBox c4">
                        <img src="../images/004-return.png" style="width: 26px" /></div>
                    <div class="linkText">STB Req. Return</div>
                </a>
                 <a href="#" id="DivReturnDispatch" runat="server" onclick="ReturnDispatchClick();">
                    <div class="linkBox c4">
                        <img src="../images/stock.png" style="width: 26px" /></div>
                    <div class="linkText">Ret. Dispatch</div>
                </a>
                <a href="#" id="DivApproval" runat="server" onclick="ApprovalClick();">
                    <div class="linkBox c5">
                        <img src="../images/003-approval.png" style="width: 26px" /></div>
                    <div class="linkText">Approval</div>
                </a>
                <a href="#" id="DivInventory" runat="server" onclick="InventoryClick();">
                    <div class="linkBox c6">
                        <img src="../images/006-storage.png" style="width: 26px" /></div>
                    <div class="linkText">Inventory</div>
                </a>
                <a href="#" id="DivSearch" runat="server" onclick="SearchClick();">
                    <div class="linkBox c7">
                        <img src="../images/searchW.png" style="width: 26px" /></div>
                    <div class="linkText">Search</div>
                </a>
                <a href="#" id="DivReports" runat="server" onclick="ReportsClick();">
                    <div class="linkBox c8">
                        <img src="../images/reportW.png" style="width: 26px" /></div>
                    <div class="linkText">Reports</div>
                </a>

                <%-- STB Scheme --%>
                 <a href="#" id="DivSchemeReceived" runat="server" onclick="SchemeReceivedClick();">
                    <div class="linkBox c4">
                        <img src="../images/paperW.png" style="width: 26px" /></div>
                    <div class="linkText">Scheme - Received</div>
                </a>
                <a href="#" id="DivSchemeDirApproval" runat="server" onclick="SchemeDirApprovalClick();">
                    <div class="linkBox c5">
                        <img src="../images/003-approval.png" style="width: 26px" /></div>
                    <div class="linkText">Scheme - Dir. Approval</div>
                </a>
                <a href="#" id="DivSchemeReqClose" runat="server" onclick="SchemeReqCloseClick();">
                    <div class="linkBox c6">
                        <img src="../images/006-storage.png" style="width: 26px" /></div>
                    <div class="linkText">Scheme - Req. Close</div>
                </a>
                <a href="#" id="DivSchemeSearch" runat="server" onclick="SchemeSearchClick();">
                    <div class="linkBox c7">
                        <img src="../images/searchW.png" style="width: 26px" /></div>
                    <div class="linkText">Scheme - Search</div>
                </a>
                <a href="#" id="DivSchemeRegister" runat="server" onclick="SchemeRegisterClick();">
                    <div class="linkBox c8">
                        <img src="../images/reportW.png" style="width: 26px" /></div>
                    <div class="linkText">STB Scheme - Register</div>
                </a>
            </div>
        </div>


        <div class="popupWraper hide" id="divPopHead" runat="server">
            <div class="popBox">
                <img src="/assests/images/pop_notification.png" class="mBot10" style="width: 50px;" />
                <h1 id="h1heading" runat="server"><%--Discussion heading--%></h1>
                <p id="pParagraph" runat="server">
                   <%-- Then system moves to ‘Dashboard’ or landing page where the entire control and
                        activity component will be available for service management.--%>
                </p>
                <button type="button" class="btn btn-sign" onclick="return PopupOk()">OK</button>
            </div>
        </div>

    </form>
</body>
</html>