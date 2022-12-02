<%@ Page Title="Payslip" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ViewPayslip.aspx.cs" Inherits="ERP.OMS.Management.Payroll.ViewPayslip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/SearchPopup.css" rel="stylesheet" />
    <script src="JS/SearchPopup.js"></script>

    <style>
        .panel-body {
            border: 1px solid #ccc;
            border-top: 3px solid #3D5294;
        }
    </style>

    <script type="text/javascript">
        function txtPayStructure_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }

        function txtPayStructure_Click(s, e) {
            document.getElementById('hdnInputType').value = "PayStructure";
            document.getElementById("txtSearchBox").value = "";

            var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th class=\"hide\">id</th><th>>Pay Structure Name</th></tr><table>";
            document.getElementById("TableStructure").innerHTML = txt;

            BindModalGrid('');
            setTimeout(function () { $("#txtSearchBox").focus(); }, 500);
            $('#ModelPopup').modal('show');
        }

        function txtEmployeeNmae_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }

        function txtEmployeeNmae_Click(s, e) {
            document.getElementById('hdnInputType').value = "Employee";
            document.getElementById("txtSearchBox").value = "";

            var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Employee Code</th><th>Employee Name</th></tr><table>";
            document.getElementById("TableStructure").innerHTML = txt;

            BindModalGrid('');
            setTimeout(function () { $("#txtSearchBox").focus(); }, 500);
            $('#ModelPopup').find('.modal-title').text('Pay Structure');
            $('#ModelPopup').modal('show');
        }

        function txtPeriod_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter") {
                s.OnButtonClick(0);
            }
        }

        function txtPeriod_Click(s, e) {
            document.getElementById('hdnInputType').value = "Period";
            document.getElementById("txtSearchBox").value = "";

            var txt = "<table border='1' width=\"100%\" class=\"dynamicPopupTbl\"><tr class=\"HeaderStyle\"><th>Period</th><th>Period From</th><th>Period To</th></tr><table>";
            document.getElementById("TableStructure").innerHTML = txt;

            BindModalGrid('Period');
            setTimeout(function () { $("#txtSearchBox").focus(); }, 500);
            $('#ModelPopup').find('.modal-title').text('Period Generation');
            $('#ModelPopup').modal('show');

        }

        function txtSearchBoxkeydown(e) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                if ($("#txtSearchBox").val() != '') {
                    BindModalGrid($("#txtSearchBox").val());
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[searchIndex=0]"))
                    $("input[searchIndex=0]").focus();
            }
        }
        // Rev Sanchita
        function PayStructure_TextChange() {
            document.getElementById('hdnInputType').value = "EmployeeTable";
           
            BindModalGrid('EmployeeTable');
            
        }
        // End of Rev Sanchita
    </script>
    <script>
        function BindModalGrid(SearchKey) {
            var SearchType = document.getElementById('hdnInputType').value;

            if (SearchType == "PayStructure") {
                var OtherDetails = {}
                OtherDetails.SearchKey = SearchKey;

                var HeaderCaption = [];
                HeaderCaption.push("Pay Structure Name");

                callonServerScroll("Services/Payroll_Master.asmx/GetPayStructureList", OtherDetails, "TableStructure", HeaderCaption, "searchIndex", "SetDropdownValue");
            }
            else if (SearchType == "Employee") {
                var OtherDetails = {};
                OtherDetails.SearchKey = SearchKey;
                OtherDetails.PayStructureID = document.getElementById('hdnPayStructureID').value;

                var HeaderCaption = [];
                HeaderCaption.push("Employee Code");
                HeaderCaption.push("Employee Name");

                callonServerScroll("Services/Payroll_Master.asmx/GetPayrollEmployeeList", OtherDetails, "TableStructure", HeaderCaption, "searchIndex", "SetDropdownValue");
            }
            else if (SearchType == "Period") {
                var OtherDetails = {};
                OtherDetails.SearchKey = SearchKey;
                OtherDetails.PayStructureID = document.getElementById('hdnPayStructureID').value;

                var HeaderCaption = [];
                HeaderCaption.push("Period");
                HeaderCaption.push("Period From");
                HeaderCaption.push("Period To");

                callonServerScroll("Services/Payroll_Master.asmx/GetPayrollSalaryPeriodList", OtherDetails, "TableStructure", HeaderCaption, "searchIndex", "SetDropdownValue");
            }
                // Rev Sanchita
            else if (SearchType == "EmployeeTable") {
                var OtherDetails = {};
                OtherDetails.SearchKey = SearchKey;
                OtherDetails.PayStructureID = document.getElementById('hdnPayStructureID').value;

                $.ajax({
                    type: "POST",
                    url: 'Services/Payroll_Master.asmx/GetPayrollEmployeeTable',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(OtherDetails),
                    success: function (type) {
                        grid.Refresh();
                        cCmbDesignName.PerformCallback();
                    }
                })
            }
            // End of Rev Sanchita
        }

        function SetDropdownValue(Id, Name, e) {
            var SearchType = document.getElementById('hdnInputType').value;

            if (SearchType == "PayStructure") {
                var PayStructureID = Id;
                var PayStructureName = Name;

                if (PayStructureID != "") {
                    $('#ModelPopup').modal('hide');

                    txtPayStructure.SetText(PayStructureName);
                    document.getElementById('hdnPayStructureID').value = Id;
                    txtEmployeeNmae.SetFocus(true);


                    txtEmployeeNmae.SetText("");
                    txtPeriod.SetText("");
                    document.getElementById('hdnEmployeeID').value = "";
                    document.getElementById('hdnPeriod').value = "";
                    // Rev Sanchita
                    PayStructure_TextChange();

                   // Get_SelectedDesigns();
                    
                    // End of Rev Sanchita
                }
            }
            else if (SearchType == "Employee") {
                if (Id != "") {
                    $('#ModelPopup').modal('hide');

                    txtEmployeeNmae.SetText(e.parentElement.cells[2].innerText);
                    document.getElementById('hdnEmployeeID').value = Id;
                    txtPeriod.SetFocus(true);
                }
            }
            else if (SearchType == "Period") {
                if (Id != "") {
                    $('#ModelPopup').modal('hide');

                    txtPeriod.SetText(Name);
                    document.getElementById('hdnPeriod').value = Id;
                    document.getElementById("btnViewPayslip").focus();
                }
            }
        }

        function ValueSelected(e, indexName) {
            if (e.code == "Enter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "searchIndex") {
                        SetDropdownValue(Id, name, e.target.parentElement);
                    }
                }
            }

            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                //if (thisindex < 10)
                $("input[" + indexName + "=" + thisindex + "]").focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {
                    if (indexName == "searchIndex")
                        $('#txtSearchBox').focus();
                }
            }
        }
    </script>
    <script>
        function btnViewPayslip_Click() {

            $.ajax({
                type: "POST",
                url: "ViewPayslip.aspx/InitSessionVariables",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    //
                    }
                });

            // Rev Sanchita
            if (document.getElementById('chkSelectAll').checked == true)
            {
                var Period = document.getElementById('hdnPeriod').value;
                grid.PerformCallback('ViewPayslip_SelectAll' + '|' + Period);
            }
            else
            {
            // End of Rev Sanchita
                var PayStructureID = document.getElementById('hdnPayStructureID').value;
                //var EmployeeID = document.getElementById('hdnEmployeeID').value;
                var Period = document.getElementById('hdnPeriod').value;
                var DesignName = cCmbDesignName.GetValue();

                grid.PerformCallback('ViewPayslip' + '|' + PayStructureID + '|' + Period + '|' + DesignName);

                //if (PayStructureID != "" && EmployeeID != "" && Period != "") {
                //    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + cCmbDesignName.GetValue() + "&modulename=PAYSLIP&structureid=" + PayStructureID + "&employeeid=" + EmployeeID + "&yymm=" + Period + '', '_blank');
                //}
            }
            
        }
        function btnSaveAsPDF_Click() {
            $.ajax({
                type: "POST",
                url: "ViewPayslip.aspx/InitSessionVariables",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    //
                }
            });

            if (document.getElementById('chkSelectAll').checked == true) {
                var Period = document.getElementById('hdnPeriod').value;
                grid.PerformCallback('GeneratePDF_SelectAll' + '|' + Period);
            }
            else {
                // End of Rev Sanchita
                var PayStructureID = document.getElementById('hdnPayStructureID').value;
                //var EmployeeID = document.getElementById('hdnEmployeeID').value;
                var Period = document.getElementById('hdnPeriod').value;
                var DesignName = cCmbDesignName.GetValue();

                grid.PerformCallback('GeneratePDF'+'|' + PayStructureID + '|' + Period + '|' + DesignName);

                
            }

            //grid.PerformCallback('GeneratePDF');
        }
        // End of Rev Sanchita

        function onEndcallback(s,e)
        {
            // Rev Sanchita
            if (s.cpPayslip == "SelectAllGenerate") {
                var Period = document.getElementById('hdnPeriod').value;

                $.ajax({
                    type: "POST",
                    url: "ViewPayslip.aspx/GetPayslipConfig",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (listPayslipConfig) {
                        
                        for (var i = 0 ; i < listPayslipConfig.d.length; i++) {

                            var liID = listPayslipConfig.d[i]["liID"];
                            var liEmpList = listPayslipConfig.d[i]["liEmpList"];
                            var liPayStructureCode = listPayslipConfig.d[i]["liPayStructureCode"];
                            var liDefaultDesign = listPayslipConfig.d[i]["liDefaultDesign"];


                            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + liDefaultDesign + "&modulename=PAYSLIP&structureid=" + liPayStructureCode + "&yymm=" + Period + '', '_blank');

                            //if (liEmpList != "" && liPayStructureCode != "" && liDefaultDesign != "") {
                            //    s.cpPayslip = null;
                            //    var objEmp = {};
                            //    objEmp.liID = liID;
                            //    objEmp.structureid = liPayStructureCode;
                            //    $.ajax({
                            //        type: "POST",
                            //        url: "ViewPayslip.aspx/GetEmployeesForSlip",
                            //        contentType: "application/json; charset=utf-8",
                            //        dataType: "json",
                            //        data: JSON.stringify(objEmp),
                            //        success: function (msg) {
                            //            //
                            //            window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + liDefaultDesign + "&modulename=PAYSLIP&structureid=" + liPayStructureCode + "&yymm=" + Period + '', '_blank');

                            //            var message = msg.d;

                            //            }
                            //        });

                            //   // window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + liDefaultDesign + "&modulename=PAYSLIP&structureid=" + liPayStructureCode + "&yymm=" + Period + '', '_blank');
                            //}
                        }



                    }
                });

                

                
            }
            else if(s.cpPayslip == "Generate") {
                // End of Rev Sanchita
                var PayStructureID = document.getElementById('hdnPayStructureID').value;
                var EmployeeID = document.getElementById('hdnEmployeeID').value;
                var Period = document.getElementById('hdnPeriod').value;

                if (PayStructureID != "" && s.cpPayslip == "Generate" && Period != "") {
                    s.cpPayslip = null;
                    window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + cCmbDesignName.GetValue() + "&modulename=PAYSLIP&structureid=" + PayStructureID + "&yymm=" + Period + '', '_blank');
                }
            }
            else if (s.cpPayslip == "NoEmployeeSelected") {
                jAlert("Select at least one Employee.");
            }
            else if (s.cpPayslip == "BlankPeriod") {
                jAlert("Select Period.");
            }
            else if (s.cpPayslip == "NoPayStructureSelected") {
                jAlert("Select a Pay Structure.");
            }
            else if (s.cpPayslip == "NoDesignNameSelected") {
                jAlert("Select a Design.");
            }
            else if (s.cpPayslip == "PDFGenerateSuccess") {
                jAlert("Payslip Generated and Sent Successfully.");
            }
            s.cpPayslip = null;
        }

        

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title clearfix">
            <h3 class="pull-left">View Payslip</h3>
        </div>
    </div>
    <div class="panel-body">
        <div class="row">
            <div class="col-md-3">
                <label>Pay Structure</label>
                <label class="checkbox-inline" style="margin-left:15px; margin-top:-4px;">
                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" ></asp:CheckBox>
                    <span style="margin: 0px 0; margin-top:-2px; display: block">
                        <dxe:ASPxLabel ID="ASPxLabel9" runat="server" Text="Select All">
                        </dxe:ASPxLabel>
                    </span>
                </label>
                <dxe:ASPxButtonEdit ID="txtPayStructure" runat="server" ReadOnly="true" ClientInstanceName="txtPayStructure" Width="100%" AutoPostBack="false">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){txtPayStructure_Click();}" KeyDown="function(s,e){txtPayStructure_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </div>
            <div class="col-md-3 hide">
                <label>Employee</label>
                <dxe:ASPxButtonEdit ID="txtEmployeeNmae" runat="server" ReadOnly="true" ClientInstanceName="txtEmployeeNmae" Width="100%" AutoPostBack="false">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){txtEmployeeNmae_Click();}" KeyDown="function(s,e){txtEmployeeNmae_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </div>
            <div class="col-md-3">
                <label>Period</label>
                <dxe:ASPxButtonEdit ID="txtPeriod" runat="server" ReadOnly="true" ClientInstanceName="txtPeriod" Width="100%" AutoPostBack="false">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){txtPeriod_Click();}" KeyDown="function(s,e){txtPeriod_KeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </div>
            <div class="col-md-3">
                <label id="lblSelectDesign" runat="server">Select Design</label>
                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" ValueField="FullDefaultDesign" TextField="DesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True"
                        OnCallback="CmbDesignName_Callback"  >
                </dxe:ASPxComboBox>
            </div>

            <div class="col-md-3">
                <label></label>
                <button type="button" onclick="btnViewPayslip_Click()" class="btn btn-primary">View Payslip</button>
            </div>
            <div class="col-md-3">
                <label></label>
                <button type="button" onclick="btnSaveAsPDF_Click()" class="btn btn-primary">Generate Payslip</button>
            </div>

            </div>
            <div class="clearfix"></div>
            <div class="row">
                <%--Rev Sanchita--%>
                <div class="col-md-12" style="padding-top: 27px;">
                    <dxe:ASPxGridView ID="EmployeeGrid" runat="server" KeyFieldName="Employee_Code" AutoGenerateColumns="False"
                        OnDataBound="EmployeeGrid_DataBound" OnDataBinding="EmployeeGrid_DataBinding"
                        Width="100%" ClientInstanceName="grid" OnCustomCallback="EmployeeGrid_CustomCallback"
                        Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        SettingsBehavior-AllowFocusedRow="true" Settings-HorizontalScrollBarMode="Visible"
                        SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" ClientSideEvents-EndCallback="onEndcallback">
                        <SettingsSearchPanel Visible="True" Delay="5000" />


                        <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />
                        <Columns>
                            <dxe:GridViewCommandColumn SelectAllCheckboxMode="AllPages" VisibleIndex="0" ShowSelectCheckbox="true" Width="5%"> </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn Visible="True" ReadOnly="True" VisibleIndex="1" FieldName="EmployeeUniqueCode" Caption="Employee Code" Width="45%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="True" ReadOnly="True" VisibleIndex="2" FieldName="Employee_Name" Caption="Employee Name" Width="50%">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        
                    </dxe:ASPxGridView>

                </div>
                <%--End of Rev Sanchita--%>
            </div>
        </div>
    </div>
    </div>
    <!-- Modal PopUp -->
    <div class="modal fade" id="ModelPopup" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Pay structure</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="txtSearchBoxkeydown(event)" id="txtSearchBox" autofocus width="100%" placeholder="Search By Pay Structure Name or Short Name" />
                    <div id="TableStructure">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal PopUp -->

    <div>
        <asp:HiddenField ID="hdnInputType" runat="server" />
        <asp:HiddenField ID="hdnPayStructureID" runat="server" />
        <asp:HiddenField ID="hdnEmployeeID" runat="server" />
        <asp:HiddenField ID="hdnPeriod" runat="server" />
    </div>


</asp:Content>
