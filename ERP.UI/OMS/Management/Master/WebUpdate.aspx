<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="WebUpdate.aspx.cs" Inherits="ERP.OMS.Management.Master.WebUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function loadUdfGroup(obj) {
            cComboUdfGroup.ClearItems();
            if (combo.GetSelectedItem()) {
                var appLicable = combo.GetSelectedItem().value


                $.ajax({
                    type: "POST",
                    url: "category.aspx/GetUdfGroup",
                    data: JSON.stringify({ AppliFor: appLicable }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var list = msg.d;
                        var listItems = [];
                        cComboUdfGroup.AddItem('-Select-', '0');
                        cComboUdfGroup.SetValue(0);
                        if (list.length > 0) {

                            for (var i = 0; i < list.length; i++) {
                                var id = '';
                                var name = '';
                                id = list[i].split('|')[1];
                                name = list[i].split('|')[0];
                                cComboUdfGroup.AddItem(name, id);
                            }

                            if (obj) {
                                cComboUdfGroup.SetValue(obj);
                            }

                        }
                    }
                });
            }

        }

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function LastCall(obj) {
            // height();
            if (Action == '') {
                MakeRowInVisible();
            }

            if (grid.cpDelmsg != null) {
                if (grid.cpDelmsg.trim() != '') {
                    jAlert(grid.cpDelmsg);
                }
            }

            if (grid.cpEditJson != null) {

                var jsonData = JSON.parse(grid.cpEditJson);

                // document.getElementById('txtVar_Name').value = jsonData.Variable_Name;
                document.getElementById('txtVal_Desc').value = jsonData.Variable_Description;

                document.getElementById('txtModuleName').value = jsonData.ModuleName;

                if (jsonData.FieldType == 'Text') {
                    $("#ComboValue").attr("style", "display:none");
                    $("#txtVal_Value").attr("style", "display:block");
                    document.getElementById('txtVal_Value').value = jsonData.Variable_Value;
                }
                else if (jsonData.FieldType == 'Dropdown') {
                    $("#txtVal_Value").attr("style", "display:none");
                    $("#ComboValue").attr("style", "display:block");
                    cComboUdfGroup.SetValue(jsonData.Variable_Value);
                }



                if (jsonData.IsActive == 'False') {
                    document.getElementById("chkIsMandatory").checked = false;
                } else {
                    document.getElementById("chkIsMandatory").checked = true;
                }


            }

            if (grid.cpSave != null) {
                if (grid.cpSave == 'Y') {
                    cPopup_Empcitys.Hide();
                    if (grid.cpSaveMsg != null) {
                        if (grid.cpSaveMsg != '') {
                            jAlert(grid.cpSaveMsg);
                        }
                    }
                }
            }
            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Y')
                    cPopup_Empcitys.Hide();
            }
        }



        function MakeRowVisible() {
            Action = 'add';
            Status = 'SAVE_NEW';
            document.getElementById('txtcat_desc').value = '';

        }

        function Call_save() {

            grid.PerformCallback(Status);

        }



        function Call_edit() {
            grid.PerformCallback('edit');
        }

        function DeleteRow(keyValue) {

            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
        }

        function PEndCallBack(obj) {
            if (obj == 'Y') {
                Action = '';
                ShowHideFilter('All');

            }
            if (obj.length > 1) {
                alert(obj);
                grid.PerformCallback();

            }
        }
        function OnEdit(obj) {
            Action = 'edit';
            Status = obj;
            grid.PerformCallback('BEFORE_' + obj);
            cPopup_Empcitys.SetHeaderText('Modify System Settings');
            cPopup_Empcitys.Show();
        }

        function callback() {
            grid.PerformCallback();
        }
    </script>
    <style>
        .pullleftClass {
            position: absolute;
            right: -5px;
            top: 32px;
        }

        .col-md-10 label {
            margin-top: 8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Web Update</h3>
        </div>
    </div>
    <div>
        <div id="divproduct">
            <div>
                <asp:FileUpload ID="OFDBankSelect" accept=".xls,.xlsx" runat="server" Width="100%" />
                <div class="pTop10  mTop5">
                    <asp:Button ID="BtnSaveexcel" runat="server" Text="Web Update"  OnClick="BtnSaveexcel_Click" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>

