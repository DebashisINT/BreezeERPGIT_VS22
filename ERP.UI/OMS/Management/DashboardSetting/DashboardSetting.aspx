<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="DashboardSetting.aspx.cs" Inherits="ERP.OMS.Management.DashboardSetting.DashboardSetting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="js/Dashboard.js"></script>
    <script src="../Activities/JS/SearchPopup.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
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
            padding:5px 3px;
        }
        .padTbl>tbody>tr>td {
            padding-right:20px;
            vertical-align:central;
        }.padTbl>tbody>tr>td>label {
             margin-bottom:0px !important;
         }
        #DynamicAccordian .panel-title>input[type="checkbox"] {
            margin-left:15px;
        }
        #DynamicAccordian .panel-title {
            position:relative;
        }
        #DynamicAccordian .panel-title>a:after {
            content: '\f056';
            font-family: FontAwesome;
            font-size: 18px;
            position: absolute;
            right: 10px;
            top: 6px;
        }
        #DynamicAccordian .panel-title>a.collapsed:after {
                content: '\f055';
        }
        .errorField {
            position:absolute;
                right: -17px;
            top: 3px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">DashBoard</h3>

            <div class="crossBtn"><a href="DashBoardSettingList.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <div class="row">

            <div class="col-md-12">
                <table class="padTbl">
                    <tr>
                        <td><label>Setting Name<span style="color: red">*</span></label></td>
                        <td>
                            <div class="relative">

                                <dxe:ASPxTextBox ID="txt_setting_nm" runat="server" ClientInstanceName="ctxt_setting_nm" MaxLength="50">
                                </dxe:ASPxTextBox>
                                <span id="setting_nm" style="display: none;" class="errorField">
                                                                <img id="mandetorydelivery" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"/>
                                                            </span>
                            </div>
                        </td>
                        <td> <label>User Group</label></td>
                        <td>
                            <div class="relative">
                                <dxe:ASPxButtonEdit ID="UserButtonEdit" ReadOnly="true" MaxLength="100" runat="server" ClientInstanceName="cUserButtonEdit">
                                    <Buttons>
                                        <dxe:EditButton>
                                        </dxe:EditButton>
                                    </Buttons>
                                    <ClientSideEvents ButtonClick="function(s,e){UserGroupSelect();}" KeyDown="function(s,e){UserGroupKeyDown(s,e);}" />
                                </dxe:ASPxButtonEdit>
                            </div>
                        </td>

                    </tr>
                </table>
                
                

            </div>
           
        </div>
        <div class="clear"></div>
        <div class="col-md-10" style="padding: 0; margin-top: 5px">
            <label>User</label>
            <table cellpadding="4" cellspacing="4" width="90%" align="center" style="border: solid 1px #d0c9c9; margin: 5px 0 15px 0;">

                <tr>
                    <td align="center" style="padding-top: 8px;">
                        <asp:ListBox ID="list1" runat="server" Style="height: 160px !important; width: 310px" SelectionMode="Multiple"></asp:ListBox>
                    </td>
                    <td align="center" style="padding-top: 5px;">
                        <input type="button" class="btn btn-primary" id="btnAdd" value=">" style="width: 50px;" /><br />
                        <input type="button" class="btn btn-primary" id="btnAddAll" value=">>" style="width: 50px;" /><br />
                        <input type="button" class="btn btn-primary" id="btnRemove" value="<" style="width: 50px;" /><br />
                        <input type="button" class="btn btn-primary" id="btnRemoveAll" value="<<" style="width: 50px;" />
                    </td>
                    <td align="center" style="padding-top: 8px;">
                        <asp:ListBox ID="list2" runat="server" Style="height: 160px !important; width: 310px" SelectionMode="Multiple"></asp:ListBox>
                    </td>
                </tr>

            </table>

        </div>
        <div class="clear"></div>
        <div class="clearfix" id="">
            <div class="panel-group" id="DynamicAccordian">

            </div>
        </div>
    </div>
    <div class="clear"></div>
    <div class="Row">
        <div class="col-md-12 " style="padding-top: 15px; padding-left: 5px">
            <div class="Left_Content">
                <button type="button" class="btn btn-primary"  onclick="apply()">Save</button>
                <button type="button" class="btn btn-danger" onclick="cancel()">Cancel</button>

            </div>
        </div>



    </div>

    <!--Employee Modal -->
    <div class="modal fade" id="UserGroupModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">User Group Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="LoadUserGroup(event)" id="txtEmpSearch" autofocus width="100%" autocomplete="off" placeholder="Search By User Group Name" />

                    <div id="UserTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th>User Group Name</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
    <asp:HiddenField runat="server" ID="UserId" />
    <asp:HiddenField runat="server" ID="Hidden_add_edit" />
    <asp:HiddenField runat="server" ID="Hidn_dash_board_id" />

    <div runat="server" id="jsonlistdiv" style="display: none"></div>
    <div runat="server" id="jsonlisteditdiv" style="display: none"></div>
</asp:Content>
