<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="UserFormDesign.aspx.cs" Inherits="ERP.OMS.Management.UserForm.UserFormDesign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .controlGetFocus {
            background: rgb(53, 181, 210) !important;
        }

        .newlineClas {
            height: 15px;
            background: #95cdea;
        }

        .mTop10px {
            margin-top: 10px;
        }

        .mTop25px {
            margin-top: 25px;
        }



        .sidenavcontrol {
            height: 100%;
            width: 160px;
            position: fixed;
            z-index: 1;
            top: 0;
            left: 0;
            background-color: #111;
            overflow-x: hidden;
            padding-top: 20px;
            padding-left: 20px;
        }

            .sidenavcontrol a {
                padding: 6px 8px 6px 16px;
                text-decoration: none;
                font-size: 25px;
                color: #818181;
                display: block;
            }

                .sidenavcontrol a:hover {
                    color: #f1f1f1;
                }

        .main {
            margin-left: 160px; /* Same as the width of the sidenav */
            font-size: 28px; /* Increased text to enable scrolling */
            padding: 0px 10px;
        }

        @media screen and (max-height: 450px) {
            .sidenavcontrol {
                padding-top: 15px;
            }

                .sidenavcontrol a {
                    font-size: 18px;
                }
        }

        .col-md-3 > label, .col-md-3 > span {
            margin-top: auto;
        }

        .sidenavcontrol {
            -webkit-transition: all 0.3s ease-in;
            transition: all 0.3s ease-in;
            z-index: 9999;
        }

            .sidenavcontrol span {
                color: white;
            }

        .closed {
            left: -300px;
        }

        .mainWraper {
            -webkit-transition: all 0.3s ease-in;
            transition: all 0.3s ease-in;
        }

        .transFormIcon {
            -webkit-transform: rotate(90deg) translateX(2px);
            -moz-transform: rotate(90deg) translateX(2px);
            -ms-transform: rotate(90deg) translateX(2px);
            -o-transform: rotate(90deg) translateX(2px);
            transform: rotate(90deg) translateX(2px);
            margin-left: 10px;
            font-size: 22px;
            color: #138dcc;
        }

        .dragNewLine {
            border: 1px solid;
            border-color: red;
            display: inline-block;
                padding: 2px 6px 4px 5px;
        }

        .widthClass {
               border: 1px solid;
                padding: 2px 5px;
                border-color: #748998;
                display: inline-block;
        }
        .widthClass>span:first-child{
            top: -3px;
            position: relative;
        }
    </style>
    <script>
        $(document).ready(function () {

            -
            initiateDoubleClick();

            $('#idcontrolList').click(function () {
                if ($('.sidenavcontrol').hasClass('closed')) {
                    $('.sidenavcontrol').removeClass('closed');
                    //$('body').css({ 'overflow': 'hidden' });
                    //$('.mainWraper').css({ 'transform': 'translateX(100px)', '-webkit-transform': 'translateX(100px)' });
                } else {
                    $('.sidenavcontrol').addClass('closed');
                }
            });
            $('#clsM').click(function (e) {
                e.preventDefault();
                $('.sidenavcontrol').addClass('closed');
            });

        })
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="JS/UserFormDesigner.js"></script>

    <div class="panel-heading">
        <div class="panel-heading clearfix">
            <div class="panel-title pull-left">
                <h3>Module Designer
                    <span id="idcontrolList" class="btn btn-sm btn-success" style="margin-left: 25px;">Show Field(s)</span>

                    
                    <span draggable="true" ondragstart="drag(event)" style="    margin-left: -4px; font-size: 14px;" class="dragNewLine">Insert New Line<i class="fa fa-level-down transFormIcon"></i></span>


                    <span id="widthBlock" class="widthClass" style="display: none">
                        <span style="font-size: 14px;">Set width:</span>
                        <span onclick="plusClick()" id="plusid" style="display: none; color: #138dcc"><i class="fa fa-plus-circle"></i></span>
                        <span onclick="minusClick()" id="minusid" style="display: none; color: #138dcc"><i class="fa fa-minus-circle"></i></span>
                    </span>


                </h3>

            </div>
        </div>

        <div id="divcross" runat="server" class="crossBtn"><a href="CreateModuleList.aspx"><i class="fa fa-times"></i></a></div>
    </div>


    <div class="sidenavcontrol closed">
        <a href="" id="clsM">close(X)</a>
        <asp:Panel ID="HeaderControlDetails" CssClass="row" runat="server"></asp:Panel>
    </div>



    <div class="form_main" ondrop="drop(event)" ondragover="allowDrop(event)" id="divdropid" runat="server">
    </div>



    <input type="button" value="Save" class="btn btn-sm btn-primary mTop25px" onclick="saveData()" />
    <asp:HiddenField ID="hdMainId" runat="server" />
    <div class="modal fade" id="RenameModel" role="dialog">
        <div class="modal-dialog">

            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Rename Field</h4>
                </div>
                <div class="modal-body">
                    <div id="AddressTable">
                        <table class="mTop10px" width="100%">
                            <tr>
                                <td>
                                    <label>Display Text</label>
                                    <label id="TestMyinp" style="margin-left: 25px;"></label>
                                    <span class="pull-right">
                                        <input type="color" id="idcolor" onchange="colorchange()" />
                                        <input type="text" id="copiedbox" style="display: none" />
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <input type="text" id="txtRename" width="100%" maxlength="500" onkeyup="TestMyinput()" />
                                </td>
                            </tr>


                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <input type="button" class="btn btn-sm btn-primary" value="Save" onclick="updateDisplayText()" />
                    <button type="button" class="btn btn-sm btn-danger" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>









</asp:Content>
