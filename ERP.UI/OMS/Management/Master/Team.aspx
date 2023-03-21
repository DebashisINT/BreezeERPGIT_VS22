<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                21-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

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

        /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

        /*select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }*/

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 5px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto , #FormDate , #toDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #txtcstVdate_B-1 ,
        #txtLocalVdate_B-1 , #txtCINVdate_B-1 , #txtincorporateDate_B-1 , #txtErpValidFrom_B-1 , #txtErpValidUpto_B-1 , #txtESICValidFrom_B-1 ,
        #txtESICValidUpto_B-1 , #FormDate_B-1 , #toDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img ,
        #txtcstVdate_B-1 #txtcstVdate_B-1Img ,
        #txtLocalVdate_B-1 #txtLocalVdate_B-1Img , #txtCINVdate_B-1 #txtCINVdate_B-1Img , #txtincorporateDate_B-1 #txtincorporateDate_B-1Img ,
        #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img , #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img , #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img ,
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img , #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
        }
        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #GrdHolidays , #GrdTeam
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 3px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-24
        {
            margin-top: 24px;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        .chosen-container-single .chosen-single span
        {
            min-height: 30px;
            line-height: 30px;
        }

        .chosen-container-single .chosen-single div {
        background: #094e8c;
        color: #fff;
        border-radius: 4px;
        height: 26px;
        top: 1px;
        right: 1px;
        /*position:relative;*/
    }

        .chosen-container-single .chosen-single div b {
            display: none;
        }

        .chosen-container-single .chosen-single div::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 2px;
            right: 5px;
            font-size: 18px;
            transform: rotate(269deg);
            font-weight: 500;
        }

    .chosen-container-active.chosen-with-drop .chosen-single div {
        background: #094e8c;
        color: #fff;
    }

        .chosen-container-active.chosen-with-drop .chosen-single div::after {
            transform: rotate(90deg);
            right: 5px;
        }

        .formBox
        {
                max-width: 600px;
        }

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 0.4 !important;
            color: #ffffff !important;
        }*/
                /*.padTopbutton {
            padding-top: 27px;
        }*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/

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
                                <input type="button" class="btn btn-info btn-radius mb-10" id="btnAdd" value=">" style="width: 50px;" /><br />
                                <input type="button" class="btn btn-warning btn-radius mb-10" id="btnAddAll" value=">>" style="width: 50px;" /><br />
                                <input type="button" class="btn btn-info btn-radius mb-10" id="btnRemove" value="<" style="width: 50px;" /><br />
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
                    <button type="button" class="btn btn-primary btn-radius" onclick="">Save & Exit</button>
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
