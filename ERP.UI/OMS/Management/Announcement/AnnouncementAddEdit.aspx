<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                28-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="AnnouncementAddEdit.aspx.cs" Inherits="ERP.OMS.Management.Announcement.AnnouncementAddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link href="../../../ckeditor/contents.css" rel="stylesheet" />
    <script src="../../../ckeditor/ckeditor.js"></script>
    <script>
        function ServiseManagementChange(s, e) {
            if (cisServiceManagement.GetChecked() == false) {
                cuserLookUp.SetEnabled(true);
                $("#lblmsg").text('');
                ciscomment.SetEnabled(true);
            }
            else {
                cuserLookUp.SetEnabled(false);
                $("#lblmsg").text('  (All users are Selected.)');
                ciscomment.SetEnabled(false);
                cuserLookUp.gridView.UnselectRows();
            }
        }

        function STBManagementChange(s, e) {
            if (cisSTBManagement.GetChecked() == false) {
                cuserLookUp.SetEnabled(true);
                $("#lblmsg").text('');
                ciscomment.SetEnabled(true);
            }
            else {
                cuserLookUp.SetEnabled(false);
                $("#lblmsg").text('  (All users are Selected.)');
                ciscomment.SetEnabled(false);
                cuserLookUp.gridView.UnselectRows();
            }
        }
        // Susanta 27-02-1-2022
        $(document).ready(function () {
            var editor1 = CKEDITOR.instances['ancMemo'];
            if (editor1) { editor1.destroy(true); }
            CKEDITOR.replace('ancMemo', {});

            var onedit = $("#onedit").val();
            if (onedit != "" || onedit != null || onedit != undefined) {
                CKEDITOR.instances["ancMemo"].setData(onedit);
            }
        })
        

    </script>

    <style>
        /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

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

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
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
        select.btn
        {
            padding-right: 10px !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        /*.panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }*/

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
        .TableMain100 #GrdHolidays 
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
                margin-top: 7px;
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

        .mt-27
        {
            margin-top: 27px !important;
        }

        .col-md-3 , .col-md-2
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        .btn.btn-xs
        {
                font-size: 14px !important;
        }

        .dxpc-content table
        {
             width: 100%;
        }

        input[type="text"], input[type="password"], textarea
        {
            margin-bottom: 0 !important;
        }
        #FromDate , #ToDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FromDate_B-1 , #ToDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FromDate_B-1 #FromDate_B-1Img , #ToDate_B-1 #ToDate_B-1Img
        {
            display: none;
        }

        .calendar-icon
        {
            right: 18px !important;
        }
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Js/AnnouncementAddEdit.js?v=0.9"></script>
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">

            <h3>Announcement Add/Edit</h3>
            <div runat="server" class="crossBtn"><a href="AnnouncementList.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>


        <div class="form_main">
        <div class="row">
            <div class="col-md-3"  id="DivServiceManagement" runat="server">
                <label>Display in Service Management Dashboard</label>
                <dxe:ASPxCheckBox ID="isServiceManagement" ClientInstanceName="cisServiceManagement" runat="server">
                    <ClientSideEvents CheckedChanged="ServiseManagementChange" />
                </dxe:ASPxCheckBox>
            </div>

            <%--Add Rev for STB management Tanmoy --%>
             <div class="col-md-3" id="DivSTBManagement" runat="server">
                <label>Display in STB Management Dashboard</label>
                <dxe:ASPxCheckBox ID="isSTBManagement" ClientInstanceName="cisSTBManagement" runat="server">
                    <ClientSideEvents CheckedChanged="STBManagementChange" />
                </dxe:ASPxCheckBox>
            </div>
           <%--Add Rev for STB management Tanmoy --%>
            <div class="clear"></div>

            <div class="col-md-3" id="DivUserLookUp">

                <label>Users</label><label id="lblmsg" runat="server"></label>
                <dxe:ASPxGridLookup ID="userLookUp" runat="server" DataSourceID="userDataSource" MultiTextSeparator=", "
                    TextFormatString="{0}" KeyFieldName="user_id" SelectionMode="Multiple" ClientInstanceName="cuserLookUp" Width="100%">
                    <Columns>
                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" />
                        <dxe:GridViewDataColumn FieldName="user_name" Caption="User Name" />
                        <dxe:GridViewDataColumn FieldName="user_loginId" Caption="Login Id" />
                    </Columns>
                    <GridViewProperties Settings-VerticalScrollBarMode="Auto">
                        <Templates>
                            <StatusBar>
                                <table class="OptionsTable" style="float: right">
                                    <tr>
                                        <td>
                                            <input type="button" value="Select All" onclick="selectAlluser()" />
                                            <input type="button" value="Un-Select All" onclick="unselectAlluser()" />
                                        </td>
                                    </tr>
                                </table>
                            </StatusBar>
                        </Templates>
                        <SettingsBehavior AllowFocusedRow="True"></SettingsBehavior>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />
                    </GridViewProperties>
                </dxe:ASPxGridLookup>
            </div>

            <div class="col-md-3">
                <label>Title</label>
                <dxe:ASPxTextBox ID="txtTitle" ClientInstanceName="ctxtTitle" runat="server" Width="100%" MaxLength="499"></dxe:ASPxTextBox>
            </div>

            <div class="col-md-2 for-cust-icon">
                <label>From</label>
                <dxe:ASPxDateEdit ID="FromDate" runat="server" ClientInstanceName="cFromDate"
                    Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false">
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>

            <div class="col-md-2 for-cust-icon">
                <label>To</label>
                <dxe:ASPxDateEdit ID="ToDate" runat="server" ClientInstanceName="cToDate"
                    Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false">
                </dxe:ASPxDateEdit>
                <%--Rev 1.0--%>
                <img src="/assests/images/calendar-icon.png" class="calendar-icon"/>
                <%--Rev end 1.0--%>
            </div>

            <div class="col-md-2" style="padding-top: 23px;">
                <label>Allow to post comment</label>
                <dxe:ASPxCheckBox ID="iscomment" ClientInstanceName="ciscomment" runat="server"></dxe:ASPxCheckBox>
            </div>

            <div class="clear"></div>


            <div class="col-md-12" style="padding-top:5px">
                <label>Announcement</label>
                <asp:TextBox ID="ancMemo" runat="server" ClientInstanceName="cancMemo"></asp:TextBox>
                <%--<dxe:TextBox ID="ancMemo" ClientInstanceName="cancMemo" runat="server" Height="71px" Width="100%" MaxLength="1500"></dxe:TextBox>--%>

            </div>
            <div class="clear"></div>
            <div class="col-md-4 pTop10">
                
                <input type="hidden" ID="hdss" runat="server" />
                <input type="hidden" ID="hdssText" runat="server" />
                <asp:Button ID="btnsave" OnClick="btnsave_Click" runat="server" Text="Save" CssClass="btn btn-primary btn-sm " OnClientClick="return validate();" />
                <button type="button" class="btn btn-danger btn-sm">Close</button>
            </div>

        </div>
    </div>
    </div>
    <asp:HiddenField ID="hdnServicemanagement" runat="server" />
    <asp:HiddenField ID="onedit" runat="server" />
    <asp:HiddenField ID="hdnSTBManagementMasterSettings" runat="server" />





    <asp:SqlDataSource ID="userDataSource" runat="server"
        SelectCommand="select user_id,user_name,user_loginId  from tbl_master_user where user_inactive='N'"></asp:SqlDataSource>

</asp:Content>
