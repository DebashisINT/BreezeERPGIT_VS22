<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="InOutRegister.aspx.cs" Inherits="ERP.OMS.Management.Attendance.InOutRegister" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .topTableri {
            margin-top: 10px;
        }

            .topTableri > tbody > tr > td {
                padding-right: 15px;
                padding-bottom: 10px;
            }


        .paddingright0 {
          padding-right: 2px !important;
        }
             
        .bandedHeader {
            background: #54749D;
            font-size: medium;
        }

        .gridHeader {
            background: #54749D;
        }

        .dxgvSelectedRow_PlasticBlue {
            background: #54749D;
        }

        .exprtClass {
            background: #54749D;
            font-size: 16px;
            margin-top: 10px;
            color: white;
        } 
        .rightStaticNav {
                position: fixed;
                top: 0;
                height: 100vh;
                width: 250px;
                right: 0;
                background: #bfe3ec;
                padding: 15px;
                -webkit-transition:all 0.3s ease-in-out;
                transition:all 0.3s ease-in-out;
                z-index:9999;
        }  
        .rightStaticNav.off {
            right:-260px;
            opacity:0;
        }
        .rightStaticNav.on {
            right:0px;
            opacity:1;
        }


        #HierchyDiv table {
        width :100%
        }

    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cGridDetail.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cGridDetail.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGridDetail.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGridDetail.SetWidth(cntWidth);
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../Activities/JS/SearchMultiPopup.js"></script>
    <script src="Js/InOutRegister.js?v=0.1"></script>

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Employee(s) In-Out Register</h3>
        </div>
    </div>


    <div class="form_main">
        <div class="clearfix">
            <div>
                <table class="topTableri">
                    <tr>
                        <td>Consider <br />Payroll Branch</td>
                        <td> 
                            <dxe:ASPxCheckBox ID="chkPayrollBranch" runat="server"></dxe:ASPxCheckBox>
                        </td>


                        <td style="color: #006ac5" class="paddingright0">Branch
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cmbBranch" ClientInstanceName="ccmbBranch"
                                runat="server" ValueType="System.String">
                                <ClientSideEvents SelectedIndexChanged="cmbBranchChange" />
                            </dxe:ASPxComboBox>
                        </td>

                         <td style="color: #006ac5" class="paddingright0">Employee</td>
                        <td colspan="2">
                            <dxe:ASPxButtonEdit ID="empButtonEdit" ReadOnly="true" runat="server" ClientInstanceName="cempButtonEdit" ClientEnabled="true">
                                <Buttons>
                                    <dxe:EditButton>
                                    </dxe:EditButton>
                                </Buttons>
                                <ClientSideEvents ButtonClick="function(s,e){EmployeeSelect();}" KeyDown="function(s,e){EmployeebtnKeyDown(s,e);}" />
                            </dxe:ASPxButtonEdit>
                        </td>



                        <td style="color: #006ac5" class="paddingright0">All
                        </td>
                        <td colspan="2">
                            <dxe:ASPxCheckBox ID="chkAllEmp" runat="server" ClientInstanceName="chkAllEmp" >
                                <ClientSideEvents CheckedChanged="allEmpChange" />
                            </dxe:ASPxCheckBox>
                        </td>

                       


                        <td style="color: #006ac5" class="paddingright0">Date</td>
                        <td>
                            <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                                ClientInstanceName="cFormDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </td>
                   

                    
                         
                           <td style="color: #006ac5" class="paddingright0">Show Inactive</td>
                           <td>
                               <dxe:ASPxCheckBox ID="chkInactive" runat="server" ClientInstanceName="cchkInactive" Checked="true">
                               </dxe:ASPxCheckBox>
                           </td>

                         <tr>

                           <td class="setWtd" >
                               <button type="button" class="btn btn-success" id="BtnShow" onclick="ShowReport()">Generate</button></td>

                             <td style="padding-bottom: 21px;">
                                  <asp:DropDownList ID="drdExport" runat="server" CssClass="exprtClass" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0">Export to</asp:ListItem>
                                        <asp:ListItem Value="2">XLSX</asp:ListItem> 
                                    </asp:DropDownList>
                             </td>


                       </tr>

                </table>

            </div>


             <dxe:ASPxGridViewExporter ID="exporter" GridViewID="GridDetail" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
            </dxe:ASPxGridViewExporter>


            <dxe:ASPxGridView ID="GridDetail" runat="server" ClientInstanceName="cGridDetail" KeyFieldName="internalId"
                Width="100%" Settings-HorizontalScrollBarMode="Auto" DataSourceID="EntityServerModeDataSource"
                SettingsBehavior-ColumnResizeMode="Control" OnCustomCallback="GridDetail_CustomCallback"
                Settings-VerticalScrollableHeight="275" SettingsBehavior-AllowSelectByRowClick="true"
                Settings-VerticalScrollBarMode="Auto"
                Settings-ShowFilterRow="true" Settings-ShowFilterRowMenu="true">

                <Columns>

                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="Employee Details" Name="MainBandedHeader" FixedStyle="Left" HeaderStyle-HorizontalAlign="Center">
                        <Columns>

                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Employee code" Width="150" FieldName="Empcode">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Employee Name" Width="250" FieldName="EmpName">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                    </dxe:GridViewBandColumn>



                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Intime1" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>
                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Outime1" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>


                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Intime2" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>
                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Outime2" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>


                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Intime3" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>
                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Outime3" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>


                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Intime4" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>
                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Outime4" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>


                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Intime5" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>
                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Outime5" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>


                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Intime6" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>
                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Outime6" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>


                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Intime7" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>
                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Outime7" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>


                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Intime8" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>
                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Outime8" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>


                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Intime9" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>
                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Outime9" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>


                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Intime10" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>
                    <dxe:GridViewDataTimeEditColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" CellStyle-HorizontalAlign="Center"
                        FieldName="Outime10" PropertiesTimeEdit-DisplayFormatString="h:mm tt" PropertiesTimeEdit-EditFormatString="h:mm tt">
                    </dxe:GridViewDataTimeEditColumn>



                    <dxe:GridViewBandColumn HeaderStyle-CssClass="bandedHeader" Caption="In-Out Count" Name="MainBandedHeader" FixedStyle="Left" HeaderStyle-HorizontalAlign="Center">
                        <Columns>

                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="In-Time" Width="70" FieldName="inCount" CellStyle-HorizontalAlign="Center">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn HeaderStyle-CssClass="gridHeader" Caption="Out-Time" Width="70" FieldName="outCount" CellStyle-HorizontalAlign="Center">
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                    </dxe:GridViewBandColumn>

                </Columns>
                <ClientSideEvents BeginCallback="gridBeginCallBack" RowDblClick="rowDbClick" />

            </dxe:ASPxGridView>




            <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                ContextTypeName="ERPDataClassesDataContext" TableName="tbl_EmpAttendanceRecord_reports" />


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



    <div style="display: none">
        <dxe:ASPxComboBox ID="ErrorCheck" ClientInstanceName="cErrorCheck"
            runat="server" ValueType="System.String">
        </dxe:ASPxComboBox>
    </div>



    <nav class="rightStaticNav off" id="rightNav">
        <span onclick="navOnOff()" style="cursor:pointer"> Close(X)</span>
        <div id="HierchyDiv"> </div>
    </nav>






</asp:Content>
