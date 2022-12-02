<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="DailyAttRegister.aspx.cs" Inherits="ERP.OMS.Management.Attendance.DailyAttRegister" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .mailBody {
            width: 800px;
        }

        .padTop10 {
            padding-top: 10px;
        }

        .EmailBody {
            background-color: #ffffff !important;
        }


        .xxxx {
            color: #7d0a0a;
            position: relative;
            background: #ffffff;
            margin-right: 5px;
            padding: 5px;
            padding-right: 15px;
            display: inline-block;
            margin-bottom: 3px;
            border: .5px solid;
            border-color: #dadcde;
        }

        #listOfEmail {
            margin-left: 35px;
        }

        .xxxx .close {
            right: 2px;
            top: 4px;
            font-size: 8px !important;
        }

            .xxxx .close:hover {
                opacity: 0.7;
            }

        .addE {
            font-size: 22px;
            float: left;
            margin-right: 15px;
            color: #4fa74f;
        }





        .switch {
            position: relative;
            display: inline-block;
            width: 46px;
            height: 20px;
        }

            .switch input {
                display: none;
            }

        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                position: absolute;
                content: "";
                height: 13px;
                width: 13px;
                left: 4px;
                bottom: 4px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        /* Rounded sliders */
        .slider.round {
            border-radius: 34px;
        }

            .slider.round:before {
                border-radius: 50%;
            }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                cgridAttendance.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                cgridAttendance.SetWidth(cntWidth);
            }

            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cgridAttendance.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cgridAttendance.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../Activities/JS/SearchPopup.js"></script>
    <script src="Js/DailyAttRegister.js?v=0.02"></script>
    <link href="Css/Attendance.css" rel="stylesheet" />


    <div class="panel-heading">
        <div class="panel-title">

            <h3>Daily Attendance Sheet</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="row">
            <div class="col-md-2">
                <table>
                    <tr>
                        <td>
                             <label>Consider Payroll Branch</label> 
                        </td>
                        </tr>
                        <tr>

                        <td>
                             <dxe:ASPxCheckBox ID="chkPayrollBranch" runat="server"></dxe:ASPxCheckBox>
                        </td>
                    </tr>
                </table>
                
                </div>
            <div class="col-md-2">
                <label>Main Unit</label>
                <dxe:ASPxComboBox ID="cmbMainUnit" runat="server" ClientInstanceName="ccmbMainUnit" Width="100%" ClientSideEvents-SelectedIndexChanged="cmbMainBranchChange">
                </dxe:ASPxComboBox>
            </div>

            <div class="col-md-2">
                <label>Sub Unit</label>
                <dxe:ASPxComboBox ID="cmbSubunit" runat="server" ClientInstanceName="ccmbSubunit" EnableCallbackMode="false" Width="100%">
                </dxe:ASPxComboBox>
            </div>

            <div class="col-md-2">
                <label>Date</label>
                <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" AllowNull="false"
                    ClientInstanceName="ctoDate" Width="100%" DisplayFormatString="dd-MM-yyyy" UseMaskBehavior="True">
                    <ButtonStyle Width="13px">
                    </ButtonStyle>
                </dxe:ASPxDateEdit>
            </div>

            <div class="col-md-3">
                <label id="lblexclude">Include Inactive Employee(s)</label>
                <div>
                    <label class="switch">
                        <input type="checkbox" checked onchange="ExEmpchkChange()" id="chkExcludeEmp" />
                        <span class="slider round"></span>
                    </label>
                </div>
                <asp:HiddenField ID="hdShowInactive" runat="server" />
            </div>
            <div class="clear" />
            <div class="col-md-4">
                <button type="button" class="btn btn-success" id="BtnShow" onclick="ShowAttendance()">Show</button>


                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary  " OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                    <asp:ListItem Value="0">Export to</asp:ListItem>
                    <asp:ListItem Value="1">PDF</asp:ListItem>
                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                    <asp:ListItem Value="3">RTF</asp:ListItem>
                    <asp:ListItem Value="4">CSV</asp:ListItem>
                </asp:DropDownList>

                <button type="button" class="btn btn-success" id="btnSendMail" onclick="OpenMailPopup()"><span><i class="fa fa-envelope"></i></span>Mail</button>


            </div>
        </div>






        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridAttendance" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>


        <dxe:ASPxGridView ID="gridAttendance" runat="server" KeyFieldName="slno"
            Width="100%" ClientInstanceName="cgridAttendance"
            OnDataBinding="gridAttendance_DataBinding"
            SettingsBehavior-AllowFocusedRow="true"
            OnCustomCallback="gridAttendance_CustomCallback"
            Settings-HorizontalScrollBarMode="Auto"
            Settings-VerticalScrollableHeight="300"
            SettingsBehavior-ColumnResizeMode="Control">



            <Columns>


                <dxe:GridViewDataTextColumn Caption="Employee Code" FieldName="EmpCode" Width="15%"
                    VisibleIndex="0">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>

                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Name" FieldName="EmpName" Width="25%"
                    VisibleIndex="0">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>

                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTimeEditColumn Caption="In-Time(First)" FieldName="Intime" Width="12%"
                    VisibleIndex="0" PropertiesTimeEdit-DisplayFormatString="h:mm:ss tt"
                    PropertiesTimeEdit-EditFormatString="h:mm:ss tt">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTimeEditColumn>

                <dxe:GridViewDataTimeEditColumn Caption="Out-Time(Last)" FieldName="Outtime" Width="12%"
                    VisibleIndex="0" PropertiesTimeEdit-DisplayFormatString="h:mm:ss tt"
                    PropertiesTimeEdit-EditFormatString="h:mm:ss tt">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTimeEditColumn>

                <dxe:GridViewDataTextColumn Caption="Duration" FieldName="WorkingHour" Width="12%"
                    VisibleIndex="0">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Late Hour(s)" FieldName="LateHour" Width="12%"
                    VisibleIndex="0">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn Caption="Status" FieldName="AttStatusName" Width="12%"
                    VisibleIndex="0">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>






            </Columns>
            <SettingsPager PageSize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,200" />
            </SettingsPager>
            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
            <ClientSideEvents EndCallback="gridEndCallBack" BeginCallback="onBeginCallBack" />
        </dxe:ASPxGridView>

        <asp:HiddenField ID="hdBranchList" runat="server" />
        <asp:HiddenField ID="MailTO" runat="server" />
        <asp:HiddenField ID="CCMail" runat="server" />



    </div>




    <!--Time Update Modal -->
    <div class="modal fade" id="MailIt" role="dialog">
        <div class="modal-dialog mailBody">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Email</h4>
                </div>
                <div class="modal-body EmailBody">
                    <label class="col-lg-2 control-label">To</label>
                    <div class="col-lg-10 clearfix">
                        <span class="addE" onclick="EmployeeSelect('E')"><i class="fa fa-plus-circle"></i></span>
                        <div class="emails" id="listOfEmail">
                        </div>
                    </div>

                    <label class="col-lg-2 control-label">CC</label>
                    <div class="col-lg-10 clearfix">
                        <span class="addE" onclick="EmployeeSelect('C')"><i class="fa fa-plus-circle"></i></span>
                        <div class="emails" id="listOfCCmail">
                        </div>
                    </div>


                    <div class="clear"></div>
                    <label class="col-lg-2 control-label">Subject</label>
                    <div class="col-lg-10">
                        <dxe:ASPxTextBox ID="mailSubject" ClientInstanceName="cmailSubject"
                            runat="server" Width="100%">
                        </dxe:ASPxTextBox>
                    </div>
                    <div class="clear"></div>
                    <label class="col-lg-2 control-label"></label>
                    <div class="col-lg-10">
                    </div>
                    <div class="clear"></div>
                    <label class="col-lg-2 control-label">Body</label>
                    <div class="col-lg-10">
                        <dxe:ASPxMemo ID="MailBody" ClientInstanceName="cMailBody" runat="server" Height="300px" Width="100%"></dxe:ASPxMemo>
                    </div>
                    <div class="clear"></div>

                    <div class="col-md-8 col-md-offset-2 padTop10">
                        <button type="button" class="btn btn-success" onclick="SendMailNow()">Send</button>
                    </div>
                    <div class="clear"></div>

                </div>



            </div>

        </div>
    </div>



    <!--Employee Modal -->
    <div class="modal fade" id="EmailModel" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Email Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="EmailKeyDown(event)" id="emailtxt" autofocus width="100%" autocomplete="off" placeholder="Search for Email" />

                    <div id="EmployeeTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Email</th>
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


</asp:Content>
