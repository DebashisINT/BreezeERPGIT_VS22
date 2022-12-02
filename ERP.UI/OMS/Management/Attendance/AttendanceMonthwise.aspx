<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="AttendanceMonthwise.aspx.cs" Inherits="ERP.OMS.Management.Attendance.AttendanceMonthwise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .topTableri {
            margin-top: 10px;
        }

            .topTableri > tbody > tr > td {
                padding-right: 15px;
                padding-bottom: 10px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../Activities/JS/SearchMultiPopup.js"></script>
    <script src="Js/AttendanceMonthwise.js?v=0.07"></script>

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Attendance Register</h3>
        </div>
    </div>

    <div class="form_main">
        <div class="clearfix">
            <div>
                <table class="topTableri">

                    <tr>
                        <td style="color: #006ac5">Consider Payroll Branch
                        </td>
                        <td colspan="2">
                            <dxe:ASPxCheckBox ID="chkPayrollBranch" ClientInstanceName="cchkPayrollBranch" runat="server"></dxe:ASPxCheckBox>
                        </td>
                    </tr>



                    <tr>
                        <td style="color: #006ac5">Branch
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cmbBranch" ClientInstanceName="ccmbBranch"
                                runat="server" ValueType="System.String">
                                <ClientSideEvents SelectedIndexChanged="cmbBranchChange" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="color: #006ac5">Select All Employee(s)
                        </td>
                        <td colspan="2">
                            <dxe:ASPxCheckBox ID="chkAllEmp" runat="server" ClientInstanceName="chkAllEmp" Checked="true">
                                <ClientSideEvents CheckedChanged="allEmpChange" />
                            </dxe:ASPxCheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="color: #006ac5">Employee</td>
                        <td colspan="2">
                            <dxe:ASPxButtonEdit ID="empButtonEdit" ReadOnly="true" runat="server" ClientInstanceName="cempButtonEdit" ClientEnabled="false">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){EmployeeSelect();}" KeyDown="function(s,e){EmployeebtnKeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                        </td>
                    </tr>
                    <tr>
                        <%--  <td style="color:#006ac5">Month / Year</td>
                     <td>
                           <dxe:ASPxComboBox ID="cmbMonthYear" ClientInstanceName="ccmbMonthYear"
                                runat="server" ValueType="System.String">
                            </dxe:ASPxComboBox>
                     </td>--%>

                        <td style="color: #006ac5">From Date</td>
                        <td>
                            <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                                ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                                <ClientSideEvents DateChanged="FromDateChange" />
                            </dxe:ASPxDateEdit>
                        </td>
                    </tr>

                    <tr>
                        <td style="color: #006ac5">To Date</td>
                        <td>
                            <dxe:ASPxDateEdit ID="ToDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                                ClientInstanceName="cToDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </td>

                    </tr>

                       <tr>
                        <td style="color: #006ac5">Show Inactive</td>
                        <td>
                             <dxe:ASPxCheckBox ID="chkInactive" runat="server" ClientInstanceName="cchkInactive" Checked="true"> 
                            </dxe:ASPxCheckBox>
                        </td>

                    </tr>


                    <tr>

                        <td class="setWtd" colspan="3">
                            <button type="button" class="btn btn-success" id="BtnShow" onclick="ShowReport()">Generate</button></td>
                    </tr>

                </table>

            </div>
        </div>







        <asp:HiddenField ID="EmpId" runat="server" />


    </div>

    <!--Employee Modal -->
    <div class="modal fade" id="EmployeeModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Employee Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Employeekeydown(event)" id="txtEmpSearch" autocomplete="off" autofocus width="100%" placeholder="Search By Employee Name or Unique Id" />

                    <div id="EmployeeTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Employee Name</th>
                                <th>Employee Id</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger btn-sm" style="margin-bottom: 0px !important;" onclick="DeSelectAll('EmpSource')">Deselect All</button>
                    <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal" onclick="OKPopup('EmpSource')">OK</button>
                </div>
            </div>

        </div>
    </div>


</asp:Content>
