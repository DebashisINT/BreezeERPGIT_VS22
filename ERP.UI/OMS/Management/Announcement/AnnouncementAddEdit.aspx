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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Js/AnnouncementAddEdit.js?v=0.9"></script>

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

            <div class="col-md-2">
                <label>From</label>
                <dxe:ASPxDateEdit ID="FromDate" runat="server" ClientInstanceName="cFromDate"
                    Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false">
                </dxe:ASPxDateEdit>
            </div>

            <div class="col-md-2">
                <label>To</label>
                <dxe:ASPxDateEdit ID="ToDate" runat="server" ClientInstanceName="cToDate"
                    Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false">
                </dxe:ASPxDateEdit>
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

    <asp:HiddenField ID="hdnServicemanagement" runat="server" />
    <asp:HiddenField ID="onedit" runat="server" />
    <asp:HiddenField ID="hdnSTBManagementMasterSettings" runat="server" />





    <asp:SqlDataSource ID="userDataSource" runat="server"
        SelectCommand="select user_id,user_name,user_loginId  from tbl_master_user where user_inactive='N'"></asp:SqlDataSource>

</asp:Content>
