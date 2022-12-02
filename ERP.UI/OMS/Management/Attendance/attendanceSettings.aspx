<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="attendanceSettings.aspx.cs" Inherits="ERP.OMS.Management.Attendance.attendanceSettings" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script>
        function onBioIdClick(empid) {
        
            $('#EmpId').val(empid);
            SettingsPopup.Show();
            cbioGrid.Refresh();
            $('#MapId').val(0);
        }

        function onpayrollBranchClick(empid) {
            $('#EmpId').val(empid);
            cPayrollBranchPopup.Show();
            cpayBranch.PerformCallback('');
        }


        function Edit(id,bioId) {
            $('#MapId').val(id);
            $('#txtbioid').val(bioId);
        }

        function ondelete(id){
            deleteId = id;
            jConfirm("Confirm Delete?", 'Alert', function (ret) {
                if (ret) {
                    var OtherDetails = {}
                    OtherDetails.BioId = deleteId;
                    $.ajax({
                        type: "POST",
                        url: "attendanceSettings.aspx/BioIdDelete",
                        data: JSON.stringify(OtherDetails),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            jAlert(msg.d.split('~')[1]);
                            if (msg.d.split('~')[0] == '0') {
                                $('#txtbioid').val('');
                                $('#MapId').val(0);
                                cbioGrid.Refresh();
                            }

                        }
                    });
                }
            });
        }

        

        function AddBioid() {

            if ($('#txtbioid').val().trim() == '') {
                jAlert("You must enter Biometric ID to Proceed Further.");
                return;
            }

            var OtherDetails = {}
            OtherDetails.EmpId = $('#EmpId').val();
            OtherDetails.Id = $('#MapId').val();
            OtherDetails.BioId = $('#txtbioid').val().trim();
            $.ajax({
                type: "POST",
                url: "attendanceSettings.aspx/BioIdUpdateedit",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    jAlert(msg.d.split('~')[1]);
                    if (msg.d.split('~')[0] == '0') {
                        $('#txtbioid').val('');
                        $('#MapId').val(0);
                        cbioGrid.Refresh();
                    }

                }
            });
        }

        function savePayBranch() {
            if (cpayBranch.GetValue())
                cpayBranch.PerformCallback('Save~' + cpayBranch.GetValue());
            else
                jAlert('Please select Payroll Branch first.', 'Alert', function () {
                    cpayBranch.Focus();
                })
        }

        function paybranchEndCallback() {
            if (cpayBranch.cpSave && cpayBranch.cpSave == 'Yes') {
                jAlert('Payroll Branch Updated Successfully.', 'Alert', function () {
                    cPayrollBranchPopup.Hide();
                })
            }
        }

        function closePaybranch() {
            cPayrollBranchPopup.Hide();
        }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGrid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGrid.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrid.SetWidth(cntWidth);
                }
            });
        });
    </script>


    <style>
        .m10 {
        margin: 10px;
        }
          .m20 {
        margin: 20px;
        }
        .padico {
            padding-right: 4px !important;
        font-size: 18px;}
    </style>


    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">Attendance Settings</h3>
        </div>
    </div>
    <div class="form_main">



        <dxe:ASPxGridView ID="Grid" runat="server" ClientInstanceName="cGrid" KeyFieldName="cnt_internalId"
            Width="100%" Settings-HorizontalScrollBarMode="Auto"
            SettingsBehavior-ColumnResizeMode="Control" DataSourceID="EntityServerModeDataSource"
            Settings-VerticalScrollableHeight="275" SettingsBehavior-AllowSelectByRowClick="true"
            Settings-VerticalScrollBarMode="Auto"
            Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">

            <Columns>

                <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Employee Code" FieldName="cnt_UCC" Width="20%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Name" FieldName="Name" Width="30%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Designation" FieldName="deg_designation" Width="20%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Role" FieldName="job_responsibility" Width="20%">
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" Width="10%" FixedStyle="Left">
                    <DataItemTemplate>

                        <a href="javascript:void(0);" onclick="onBioIdClick('<%# Container.KeyValue %>')" id="a_editInvoice" class="padico" title="Add/Edit Biometric ID">
                            <i class="fa fa-tablet"></i>
                        </a>

                           <a href="javascript:void(0);" onclick="onpayrollBranchClick('<%# Container.KeyValue %>')"  class="padico" title="Set Payroll Branch">
                            <i class="fa fa-home"></i>
                        </a>


                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" CssClass="gridHeader"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />

                </dxe:GridViewDataTextColumn>


            </Columns>

            <SettingsPager PageSize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>

        </dxe:ASPxGridView>



        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_employee_details" />

    </div>






    <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="SettingsPopup"
        Width="500px" HeaderText="Settings" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">



                <table>
                    <tr>
                        <td>Biometric ID</td>
                        <td colspan="2"><input type="text" class="m10" maxlength="100" id="txtbioid"/></td>
                        <td><input type="button" value="Save" class="btn btn-primary m20" onclick="AddBioid()"/> </td>

                    </tr>
                </table>




                <dxe:ASPxGridView ID="bioGrid" runat="server" ClientInstanceName="cbioGrid" KeyFieldName="id"
                    Width="100%" Settings-HorizontalScrollBarMode="Auto"
                    SettingsBehavior-ColumnResizeMode="Control"  OnDataBinding="bioGrid_DataBinding" OnCustomCallback="bioGrid_CustomCallback"
                    Settings-VerticalScrollableHeight="275" SettingsBehavior-AllowSelectByRowClick="true"
                    Settings-VerticalScrollBarMode="Auto"
                    Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">
                    <Columns>

                        <dxe:GridViewDataTextColumn Caption="Biometric ID" FieldName="BiometricId" Width="70%"
                            VisibleIndex="0">
                            <CellStyle CssClass="gridcellleft" Wrap="true">
                            </CellStyle>
                            <Settings AllowAutoFilterTextInputTimer="False" />
                            <Settings AutoFilterCondition="Contains" />
                        </dxe:GridViewDataTextColumn>



                          <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" Width="30%" FixedStyle="Left">
                    <DataItemTemplate>

                        <a href="javascript:void(0);" onclick="Edit('<%# Container.KeyValue %>','<%#Eval("BiometricId") %>')" id="a_editInvoice" class="pad" title="Edit">
                            <img src="../../../assests/images/Edit.png" /></a>

                         <a href="javascript:void(0);" onclick="ondelete('<%# Container.KeyValue %>')" id="a_editInvoice" class="pad" title="Edit">
                            <img src="../../../assests/images/delete.png" /></a>

                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" CssClass="gridHeader"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />

                </dxe:GridViewDataTextColumn>

                    </Columns>


                </dxe:ASPxGridView>






                <asp:HiddenField ID="MapId" runat="server" />
                <asp:HiddenField ID="EmpId" runat="server" />

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>









    <dxe:ASPxPopupControl ID="PayrollBranchPopup" runat="server" ClientInstanceName="cPayrollBranchPopup"
        Width="500px" HeaderText="Payroll Branch" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">



                <table>
                    <tr>
                        <td>Payroll Branch</td>
                        <td>

                            <dxe:ASPxComboBox ID="payBranch" ClientInstanceName="cpayBranch" ClientSideEvents-EndCallback="paybranchEndCallback"
                                OnCallback="payBranch_Callback"  runat="server" ValueType="System.String"></dxe:ASPxComboBox>
                        </td>

                    </tr>
                    <tr>
                        <td colspan="2">
                            <input type="button" value="Save" class="btn btn-primary mTop5" onclick="savePayBranch()"/>
                            <input type="button" value="Cancel" class="btn btn-danger mTop5" onclick="closePaybranch()"/>
                        </td>
                    </tr>
                </table>


                 
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>


</asp:Content>
