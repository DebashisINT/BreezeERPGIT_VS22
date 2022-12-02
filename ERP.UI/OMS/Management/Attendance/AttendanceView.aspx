<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="AttendanceView.aspx.cs" Inherits="ERP.OMS.Management.Attendance.AttendanceView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <style>
          .errorField {
                position: absolute;
                right: -1px;
                top: 23px;
        }
   </style>
</asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="Js/AttendanceView.js?v=0.9"></script>
    <script src="../Activities/JS/SearchPopup.js?v=0.4"></script>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <link href="Css/Attendance.css" rel="stylesheet" />

    <div class="panel-heading">
        <div class="panel-title">
            
            <h3>Employee Attendance Details</h3>
        </div>
    </div>
    <div class="form_main">
        <div class="row">
            <div class="col-md-2">
                <label>Month / Year</label>
                <dxe:ASPxComboBox ID="cmbMonthYear" ClientInstanceName="ccmbMonthYear"
                    runat="server" ValueType="System.String">
                </dxe:ASPxComboBox>
            </div>
            <div class="col-md-2">
                <label>Employee </label>
                <dxe:ASPxButtonEdit ID="empButtonEdit" ReadOnly="true" runat="server" ClientInstanceName="cempButtonEdit">
                    <Buttons>
                        <dxe:EditButton>
                        </dxe:EditButton>
                    </Buttons>
                    <ClientSideEvents ButtonClick="function(s,e){EmployeeSelect();}" KeyDown="function(s,e){EmployeeKeyDown(s,e);}" />
                </dxe:ASPxButtonEdit>
            </div>
            <div class="col-md-4 butnlaymargin">
                <button type="button" class="btn btn-success" id="BtnShow" onclick="ShowAttendance()">Show</button>
              
                
                   <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary  " OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLSX</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            </div>
        </div>
        

        

        

            <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridAttendance" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                            </dxe:ASPxGridViewExporter>


        <dxe:ASPxGridView ID="gridAttendance" runat="server" KeyFieldName="slno"
            Width="100%" ClientInstanceName="cgridAttendance"
            OnDataBinding="gridAttendance_DataBinding"
            SettingsBehavior-AllowFocusedRow="true" 
            OnCustomCallback="gridAttendance_CustomCallback"
            >

            <Columns>


                <dxe:GridViewDataDateColumn Caption="Date" FieldName="AttDate" Width="20%"
                    VisibleIndex="0" PropertiesDateEdit-DisplayFormatString="dd-MM-yyyy" PropertiesDateEdit-EditFormatString="dd-MM-yyyy">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <PropertiesDateEdit DisplayFormatString="dd-MM-yyyy"></PropertiesDateEdit>

                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataDateColumn>

                <dxe:GridViewDataTimeEditColumn Caption="In-Time(First)" FieldName="Intime" Width="20%"
                     VisibleIndex="0" PropertiesTimeEdit-DisplayFormatString="h:mm:ss tt" 
                    PropertiesTimeEdit-EditFormatString="h:mm:ss tt">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTimeEditColumn>
                
                <dxe:GridViewDataTimeEditColumn Caption="Out-Time(Last)" FieldName="Outtime" Width="20%"
                     VisibleIndex="0" PropertiesTimeEdit-DisplayFormatString="h:mm:ss tt" 
                    PropertiesTimeEdit-EditFormatString="h:mm:ss tt">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                     <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTimeEditColumn> 
                
                  <dxe:GridViewDataTextColumn Caption="Total Hour(s)" FieldName="WorkingHour" Width="15%"
                                        VisibleIndex="0">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                         <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                  </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn Caption="Status" FieldName="AttStatusName" Width="15%"
                                        VisibleIndex="0">
                                        <CellStyle CssClass="gridcellleft" Wrap="true">
                                        </CellStyle>
                                         <Settings AllowAutoFilterTextInputTimer="False" />
                                        <Settings AutoFilterCondition="Contains" />
                  </dxe:GridViewDataTextColumn>

                

                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center"
                    VisibleIndex="17" Width="10%">
                    <DataItemTemplate>
                        

                        <a href="javascript:void(0);"  style='<%#Eval("ShouldEditVissible") %>' class="pad" title="Edit" onclick="onEditClick('<%# Container.VisibleIndex %>','<%# Container.KeyValue %>', '<%#Eval("AttStatus") %>', '<%#Eval("Remarks") %>' )" >
                           <img src="../../../assests/images/info.png" /></a>
                       </a>

                       
                                              
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>

                </dxe:GridViewDataTextColumn>


            </Columns>
             <SettingsPager PageSize="10">
                                    <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,15,31" />
                                </SettingsPager>
            <Settings    ShowFilterRow="true" ShowFilterRowMenu="true" />
            <ClientSideEvents EndCallback="GridEndCallback" BeginCallback="onBeginCallBack"/>
        </dxe:ASPxGridView>

        <asp:HiddenField runat="server" ID="EmpId" />
        <asp:HiddenField runat="server" ID="KeyValue" />
        <asp:HiddenField runat="server" ID="hdAttDate" />



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
                    <input type="text" onkeydown="Employeekeydown(event)" id="txtEmpSearch" autofocus width="100%" autocomplete="off" placeholder="Search By Employee Name or Unique Id" />

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
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>


    <!--Time Update Modal -->
    <div class="modal fade" id="TimeUpdateModel" role="dialog"  >
        <div class="modal-dialog customSizeModal">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Update In Time/Out Time</h4>
                </div>
                <div class="modal-body">
                    <div class="bxCenter">

                        <div class="col-md-12">
                            <label>Status</label>
                            <dxe:ASPxComboBox ID="EmpStatus" ClientInstanceName="cEmpStatus"
                                 runat="server" ValueType="System.String" Width="100%">
                                <ClientSideEvents SelectedIndexChanged="StatusChange" />
                            </dxe:ASPxComboBox>
                        </div>

                        <div class="col-md-12">
                            <label>In Time</label>
                            <dxe:ASPxTimeEdit ID="IntimeEdit" runat="server" Width="190" EditFormat="Custom"
                                EditFormatString="h:mm:ss tt" DisplayFormatString="h:mm:ss tt" ClientInstanceName="cIntimeEdit">
                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                <ValidationSettings ErrorDisplayMode="None" />
                            </dxe:ASPxTimeEdit>
                        </div>
                        <div class="col-md-12">
                            <label>Out Time</label>
                            <dxe:ASPxTimeEdit ID="OutTimeEdit" runat="server" Width="190" EditFormat="Custom"
                                EditFormatString="h:mm:ss tt" DisplayFormatString="h:mm:ss tt" ClientInstanceName="cOutTimeEdit">
                                <ClearButton DisplayMode="OnHover"></ClearButton>
                                <ValidationSettings ErrorDisplayMode="None" />
                            </dxe:ASPxTimeEdit>
                        </div>

                        
                         <div class="col-md-12">
                            <label>Remarks<span style="color: red">*</span></label> 
                             <dxe:ASPxMemo ID="txtRewmarks" ClientInstanceName="ctxtRewmarks" runat="server" Height="71px" 
                                 Width="100%" MaxLength="800"></dxe:ASPxMemo>
                              <span id="txt_rmrks" style="display: none;" class="errorField">
                                                                <img id="mandetorydelivery" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"/>
                                                            </span>
                        </div>

                        <div class="col-md-12 mTop15">
                            <button type="button" class="btn btn-primary" onclick="UpdateTime()"><u>S</u>ave</button>
                            <button type="button" class="btn btn-danger" data-dismiss="modal"><u>C</u>lose</button>
                        </div>
                    </div>
                    

                    



                </div>
               
            </div>

        </div>
    </div>




</asp:Content>
