<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Team.aspx.cs" Inherits="ERP.OMS.Management.Master.Team" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="google" content="notranslate">
    <%-- <script src="js/Dashboard.js"></script>
    <script src="../Activities/JS/SearchPopup.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />--%>
    <script>
        $(function () {
            BindListBoxUser();

            $('#btnAdd').click(
                 function (e) {
                     $('#list1 > option:selected').appendTo('#list2');
                     e.preventDefault();
                 });

            $('#btnAddAll').click(
            function (e) {
                $('#list1 > option').appendTo('#list2');
                e.preventDefault();
            });

            $('#btnRemove').click(
            function (e) {
                $('#list2 > option:selected').appendTo('#list1');
                e.preventDefault();
            });

            $('#btnRemoveAll').click(
            function (e) {
                $('#list2 > option').appendTo('#list1');
                e.preventDefault();
            });
        });

        function BindListBoxUser() {
            $.ajax({
                type: "POST",
                url: "Team.aspx/GetAllUser",
                data: JSON.stringify({ id: "0" }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    // console.log(response);
                    if (response.d) {
                        if (response.d.msg == "true") {

                            var luser = $('select[id$=list1]');
                            luser.empty();
                            var listItems = [];

                            var ddlArray = new Array();
                            var ddl = document.getElementById('list2');
                            for (i = 0; i < ddl.options.length; i++) {
                                ddlArray[i] = ddl.options[i].value;
                            }

                            $.each(response.d._usergroup, function (index, e) {
                                if (ddlArray.indexOf(e.id) == -1) {
                                    listItems.push("<option value=" + e.id + "> " + e.name + "</option>");
                                }
                            });
                            $(luser).append(listItems.join(''));
                        }
                        else {
                            alert(response.d.msg);
                        }
                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });
        }


        function apply() {
            if (ctxt_Team_nm.GetText() == "") {
                $("#Team_nm").show();
                return
            }
            else {
                $("#Team_nm").hide();
            }

            if ($('#list2 option').length > 0) {
                var _header = {
                    Team_name: ctxt_Team_nm.GetText(),
                    branchID: ccmbBranchfilter.GetValue(),
                    Description: $("#txtDescription").val(),
                    action: $('#Hidden_add_edit').val(),
                    teamid: $('#Hidn_team_id').val()
                }

                //var rights_dtls = [];

                //var jsonobj = JSON.parse($("#jsonlistdiv").text());
                //var ParentObj = $.grep(jsonobj, function (e) { return e.parent_id != "0" })
                ////console.log(ParentObj);
                //for (var pid = 0; pid < ParentObj.length; pid++) {
                //    var rights = {};

                //    rights['column_name'] = ParentObj[pid].column_name;
                //    rights['status'] = document.getElementById('chk' + ParentObj[pid].id).checked;
                //    rights_dtls.push(rights);

                //}
                var listItems = [];
                var listBox = document.getElementById("list2");

                for (var i = 0; i < listBox.options.length; i++) {
                    var Items = {};

                    Items['value'] = listBox.options[i].value;
                    Items['text'] = listBox.options[i].innerHTML;
                    listItems.push(Items);
                }

                var apply = {
                    header: _header,
                    users_dtls: listItems
                }

                $.ajax({
                    type: "POST",
                    url: "Team.aspx/save",
                    data: "{apply:" + JSON.stringify(apply) + "}",//JSON.stringify(apply),
                    async: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        console.log(response);
                        if (response.d) {
                            if (response.d == "true") {
                                jAlert("Saved Successfully.", "Alert", function () {
                                    window.location.href = 'Team.aspx?id=ADD';
                                });
                            }
                            else {
                                jAlert(response.d);
                                $("#Team_nm").show();
                                return
                            }

                        }

                    },
                    error: function (response) {

                        console.log(response);
                    }

                });
            }
            else {
                jAlert("Select Atleast One User!");
                return;
            }

        }
        function cancel() {
            location.href = "erpTeamList.aspx"
        }

    </script>
    <style>
        .clear {
            clear: both;
        }
        /*#list2 option {
     background:#333;
     color:#fff;
 }*/ ul.inline-list {
            padding-left: 10px;
        }

        .inline-list > li {
            display: inline-block;
            list-style-type: none;
            margin-right: 20px;
            margin-bottom: 8px;
        }

            .inline-list > li > input {
                -webkit-transform: translateY(3px);
                -moz-transform: translateY(3px);
                transform: translateY(3px);
                margin-right: 4px;
            }

        .panel-title > a {
            font-size: 16px !important;
        }

        #list2 option,
        #list1 option {
            padding: 5px 3px;
        }

        .padTbl > tbody > tr > td {
            padding-right: 20px;
            vertical-align: central;
        }

            .padTbl > tbody > tr > td > label {
                margin-bottom: 0px !important;
            }

        #DynamicAccordian .panel-title > input[type="checkbox"] {
            margin-left: 15px;
        }

        #DynamicAccordian .panel-title {
            position: relative;
        }

            #DynamicAccordian .panel-title > a:after {
                content: '\f056';
                font-family: FontAwesome;
                font-size: 18px;
                position: absolute;
                right: 10px;
                top: 6px;
            }

            #DynamicAccordian .panel-title > a.collapsed:after {
                content: '\f055';
            }

        .errorField {
            position: absolute;
            right: -17px;
            top: 3px;
        }
        .mutiWraper {
            overflow: hidden;
            border: 1px solid #5869de;
            border-radius: 8px;
            padding: 0;
        }
        .mutiWraper>div.hdr {
            background: #5869de;
            padding: 5px;
            color: #fff;
        }
        .mutiWraper .multiSelect {
            margin: 0;
            border: none;
            width: 100%
        }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="pageOverlay"></div>
    <div class="">
        <div class=" clearfix formBox mtop80">
            <div id="ApprovalCross" runat="server" class="crossBtn pageTypepop"><a href="erpTeamList.aspx"><i class="fa fa-times"></i></a></div>
            <label class="pagePopLabl">Add Team</label>
            <div class="row">
                <div class="col-md-6">
                    <div class="form-group ">
                        <label for="" class="col-form-label">Team Name<span style="color: red">*</span> </label>
                        <div >
                            <div class="relative">
                                <dxe:ASPxTextBox ID="txt_Team_nm" runat="server" ClientInstanceName="ctxt_Team_nm" MaxLength="50" Width="100%">
                                </dxe:ASPxTextBox>
                                <span id="Team_nm" style="display: none;" class="errorField">
                                    <img id="mandetorydelivery" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory" />
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group ">
                        <label for="" class="col-form-label">Unit<span style="color: red">*</span> </label>
                        <div>
                            <div class=" relative">
                                <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="form-group">
                    <label for="" class="col-form-label">Description </label>
                    <div>
                        <div class="relative">
                            <textarea id="txtDescription" runat="server" class="form-control" rows="6" cols="50"></textarea>
                        </div>
                    </div>
                </div>
                </div>
            </div>
            <div class="row mBot11">
                <div class="col-md-12">
                    <label for="" class="col-form-label">User </label>
                    <table>
                        <tr>
                            <td style="width:40%">
                                <div class="mutiWraper">
                                    <div class="hdr">Available User(s)</div>
                                    <asp:ListBox ID="list1" runat="server" Style="height: 160px !important; " CssClass="multiSelect" SelectionMode="Multiple"></asp:ListBox>
                                </div>
                            </td>
                            <td class="text-center">
                                <input type="button" class="btn btn-info btn-radius" id="btnAdd" value=">" style="width: 50px;" /><br />
                                <input type="button" class="btn btn-warning btn-radius" id="btnAddAll" value=">>" style="width: 50px;" /><br />
                                <input type="button" class="btn btn-info btn-radius" id="btnRemove" value="<" style="width: 50px;" /><br />
                                <input type="button" class="btn btn-warning btn-radius" id="btnRemoveAll" value="<<" style="width: 50px;" />
                            </td>
                            <td style="width:40%">
                                <div class="mutiWraper">
                                    <div class="hdr">Selected User(s)</div>
                                    <asp:ListBox ID="list2" runat="server" CssClass="multiSelect" Style="height: 160px !important;" SelectionMode="Multiple"></asp:ListBox>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="row mTop5">
                <div class="col-md-12 mTop5">
                    <button type="button" class="btn btn-success btn-radius" onclick="apply()">Save & New</button>
                    <button type="button" class="btn btn-success btn-radius" onclick="">Save & Exit</button>
                    <button type="button" class="btn btn-danger btn-radius" onclick="cancel()">Cancel</button>
                </div>
            </div>
        </div>
    </div>

    <div class="clear"></div>
    

    <asp:HiddenField runat="server" ID="UserId" />
    <asp:HiddenField runat="server" ID="Hidden_add_edit" />
    <asp:HiddenField runat="server" ID="Hidn_team_id" />

    <div runat="server" id="jsonlistdiv" style="display: none"></div>
    <div runat="server" id="jsonlisteditdiv" style="display: none"></div>
</asp:Content>
