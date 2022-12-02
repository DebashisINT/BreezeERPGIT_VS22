<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_ExEmployeeDetails" Codebehind="frmReport_ExEmployeeDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  

<%--    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/init.js"></script>--%>

    <%--   <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>--%>

    <script type="text/javascript" src="/assests/js/ajaxList_rootfile.js"></script>


    <script language="javascript" type="text/javascript">
        //Global Variable
        FieldName = 'none';
        //End

        function OnEditButtonClick(keyValue) {
            var url = '../management/master/Employee_EmployeeCTC.aspx?id=' + keyValue;
            OnMoreInfoClick(url, "Add New CTC Details", '980px', '500px', "Y");
        }

      

        function OnMoreInfoClick(keyValue) {
            var url = '../management/master/employee_general.aspx?id=' + keyValue;
            OnMoreInfoClick(url, "Modify Employee Details", '980px', '500px', "Y");
        }

        function Show(keyValue) {
            var url = "../management/master/frmEmployeeCTC.aspx?link=../Reports/frmReport_ExEmployeeDetails.aspx&id=ADD&ContID=" + keyValue;
            // var url="frmEmployeeCTC.aspx?id=ADD";
            popup.SetContentUrl(url);
            popup.Show();

        }
        function OnPageNo_Click(obj) {
            var i = document.getElementById(obj).innerText;
            grid.PerformCallback("SearchByNavigation~" + i + "~All");
            //alert(i);
        }
        function OnLeftNav_Click() {
            var i = document.getElementById("A1").innerText;
            if (parseInt(i) > 1) {
                i = parseInt(i) - 10;
                for (l = 1; l < 11; l++) {
                    var obj = "A" + l;
                    document.getElementById(obj).innerText = i++;
                }
                grid.PerformCallback("SearchByNavigation~" + document.getElementById("A1").innerText + "~All");
            }
            else {
                alert('You are on the Beginning');
            }
        }
        function OnRightNav_Click() {
            var TestEnd = document.getElementById("A10").innerText;
            var TotalPage = document.getElementById("B_TotalPage").innerText;
            if (TestEnd == "" || TestEnd == TotalPage) {
                alert('You are at the End');
                return;
            }
            var i = document.getElementById("A1").innerText;
            if (parseInt(i) < TotalPage) {
                i = parseInt(i) + 10;
                var n = parseInt(TotalPage) - parseInt(i) > 10 ? parseInt(11) : parseInt(TotalPage) - parseInt(i) + 2;
                for (r = 1; r < n; r++) {
                    var obj = "A" + r;
                    document.getElementById(obj).innerText = i++;
                }
                for (r = n; r < 11; r++) {
                    var obj = "A" + r;
                    document.getElementById(obj).innerText = "";
                }
                grid.PerformCallback("SearchByNavigation~" + document.getElementById("A1").innerText + "~All");
            }
            else {
                alert('You are at the End');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <script type="text/javascript" language="javascript">

            function CallList(obj1, obj2, obj3) {
                var obj5 = '';
                ajax_showOptions(obj1, obj2, obj3, obj5);
            }

            FieldName = 'Headermain1_cmbSegment';
            function ShowHidebtn() {
                FieldName = 'Headermain1_cmbSegment';
                grid.PerformCallback("SearchByNavigation~1~All");

            }


            function ShowHideFilter(obj) {
                FieldName = 'Headermain1_cmbSegment';
                grid.PerformCallback("SearchByNavigation~1~" + obj);
                //        grid.PerformCallback(obj);
            }
            function OnRejoinClick(e, obj) {
                FieldName = 'Headermain1_cmbSegment';
                RowID = obj;
                popup.ShowAtElement(e);
                popup.Show();
            }
            function btnRejoinClick() {
                if (RejoinDate.GetDate() != null) {
                    //alert(RejoinDate.GetDate());
                    popPanel.PerformCallback(RowID);
                }
                else
                    alert('Please Select Join Date!');
            }

            function pageload() {
                FieldName = 'Headermain1_cmbSegment';
            }
            function EndCallBack() {
                popup.Hide();
                grid.PerformCallback();
            }

            function aspxExEmpGrid_EndCallback() {
                var strUndefined = new String(grid.cpIsEmptyDsSearch);
                if (strUndefined != "undefined" && strUndefined != "NoRecord") {
                    document.getElementById("B_PageNo").innerText = strUndefined.split('~')[1];
                    document.getElementById("B_TotalPage").innerText = strUndefined.split('~')[2];
                    document.getElementById("B_TotalRows").innerText = strUndefined.split('~')[3];
                    // document.getElementById("B_SearchBy").innerText=strUndefined.split('~')[4];
                    var i = document.getElementById("A1").innerText;
                    var TotalPage = strUndefined.split('~')[2];
                    if (parseInt(i) <= TotalPage && parseInt(i) == 1) {
                        n = (parseInt(TotalPage) - parseInt(i) > 10) ? parseInt(11) : parseInt(TotalPage) - parseInt(i) + 2;
                        for (a = 1; a < n; a++) {
                            var obj = "A" + a;
                            document.getElementById(obj).innerText = a;
                        }
                        for (a = n; a < 11; a++) {
                            var obj = "A" + a;
                            document.getElementById(obj).innerText = "";
                        }
                    }


                }
                else if (strUndefined == "NoRecord") {
                    alert('No Record Found');
                }

            }

            function keyVal(obj) {
               
                popup.Show();
            }


        </script>
     <div class="panel-heading">
       <div class="panel-title">
           <h3>Ex-Employee's Details Repor</h3>
       </div>

   </div> 
      <div class="form_main">
        <table class="TableMain100">
           
            <tr>
                <td class="gridcellleft" colspan="0">
                    <table>
                        <tr>
                            <td style="padding-right:15px">
                                <dxe:ASPxDateEdit ID="fromDate" runat="server" DropDownButton-Width="100%" Font-Size="12px"
                                    Width="150px" EditFormat="Custom" UseMaskBehavior="True">
                                    <buttonstyle width="13px">
                                </buttonstyle>
                                    <dropdownbutton text="FromDate" >
                                </dropdownbutton>
                                    <validationsettings display="Dynamic" errortextposition="Bottom">
                                    <RequiredField ErrorText="Invalid Date" IsRequired="True" />
                                </validationsettings>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td style="padding-right:15px">
                                <dxe:ASPxDateEdit ID="toDate" runat="server" DropDownButton-Width="100%" Font-Size="12px"
                                    Width="150px" EditFormat="Custom" UseMaskBehavior="True">
                                    <buttonstyle width="13px">
                                </buttonstyle>
                                    <dropdownbutton text="ToDate" >
                                </dropdownbutton>
                                    <validationsettings display="Dynamic" errortextposition="Bottom">
                                    <RequiredField ErrorText="Invalid Date" IsRequired="True" />
                                </validationsettings>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td style="padding-right:15px">
                                <dxe:ASPxComboBox ID="cmbCompany" runat="server" EnableIncrementalFiltering="true"
                                    ValueType="System.String" DropDownButton-Text="Company" Font-Size="12px" Width="150px">
                                    <buttonstyle width="13px">
                                </buttonstyle>
                                    <dropdownbutton text="Company" >
                                </dropdownbutton>
                                </dxe:ASPxComboBox>
                            </td>
                            <td style="padding-right:15px">
                                <dxe:ASPxComboBox ID="cmbBranch" runat="server" EnableIncrementalFiltering="true"
                                    ValueType="System.String" DropDownButton-Text="Branch" Font-Size="12px" Width="150px">
                                    <buttonstyle width="13px">
                                </buttonstyle>
                                    <dropdownbutton text="Branch" >
                                </dropdownbutton>
                                </dxe:ASPxComboBox>
                            </td>
                            <td style="padding-right:15px">
                                <dxe:ASPxComboBox ID="cmbDepartment" runat="server" EnableIncrementalFiltering="true"
                                    ValueType="System.String" DropDownButton-Text="Department" Font-Size="12px" Width="150px">
                                    <buttonstyle width="13px">
                                </buttonstyle>
                                    <dropdownbutton text="Department" >
                                </dropdownbutton>
                                </dxe:ASPxComboBox>
                                <asp:SqlDataSource ID="Department" runat="server"
                                    SelectCommand="Select cost_id,cost_description from tbl_master_costCenter where cost_costCenterType = 'department' ORDER BY [cost_description]">
                                </asp:SqlDataSource>
                            </td>
                            <td style="padding-right:15px" >
                                <input id="bnSubmit" type="button" value="GO"  class="btn btn-primary" onclick="ShowHidebtn();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table class="hide">
                        <tr>
                            <td id="Td2">
                                <a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span >
                                    Show Filter</span></a>
                            </td>
                            <td id="Td3">
                                <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>
                                    All Records</span></a>
                            </td>
                        </tr>
                    </table>
                     
                    <table border="1" style="width:320px" class="hide">
                        <tr>
                            
                            
                            <td valign="top" style="padding-top: 8px; ">
                                <table width="100%">
                                    <tr>
                                        <td style="vertical-align: top;  text-align: left;"
                                valign="top">
                                &nbsp;Page of (records)</td>
                                        <%-- <td valign="top">
                                <a id="A_StartNav" runat="server" href="javascript:void(0);" onclick="OnStartNav_Click()">
                                    <img src="/assests/images/LeftNav.gif" width="10"/>
                                  </a>
                               </td>--%>
                                        <td style="vertical-align: top; height: 11px;  text-align: left"
                                            valign="top">
                                            <a id="A_LeftNav" runat="server" href="javascript:void(0);" onclick="OnLeftNav_Click()">
                                                <img src="/assests/images/LeftNav.gif" width="10" />
                                            </a>
                                        </td>
                                        <td style="vertical-align: top; height: 11px;  text-align: left"
                                            valign="top">
                                            <a id="A1" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A1')">
                                                1</a>
                                        </td>
                                        <td style="vertical-align: top; height: 11px;  text-align: left"
                                            valign="top">
                                            <a id="A2" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A2')">
                                                2</a>
                                        </td>
                                        <td style="vertical-align: top; height: 11px;  text-align: left"
                                            valign="top">
                                            <a id="A3" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A3')">
                                                3</a>
                                        </td>
                                        <td style="vertical-align: top; height: 11px;  text-align: left"
                                            valign="top">
                                            <a id="A4" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A4')">
                                                4</a>
                                        </td>
                                        <td style="vertical-align: top; height: 11px;  text-align: left"
                                            valign="top">
                                            <a id="A5" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A5')">
                                                5</a>
                                        </td>
                                        <td style="vertical-align: top; height: 11px;  text-align: left"
                                            valign="top">
                                            <a id="A6" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A6')">
                                                6</a>
                                        </td>
                                        <td style="vertical-align: top; height: 11px;  text-align: left"
                                            valign="top">
                                            <a id="A7" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A7')">
                                                7</a>
                                        </td>
                                        <td style="vertical-align: top; height: 11px;  text-align: left"
                                            valign="top">
                                            <a id="A8" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A8')">
                                                8</a>
                                        </td>
                                        <td style="vertical-align: top; height: 11px;  text-align: left"
                                            valign="top">
                                            <a id="A9" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A9')">
                                                9</a>
                                        </td>
                                        <td style="vertical-align: top; height: 11px;text-align: left"
                                            valign="top">
                                            <a id="A10" runat="server" href="javascript:void(0);" onclick="OnPageNo_Click('A10')">
                                                10</a>
                                        </td>
                                        <td style="vertical-align: top; height: 11px;  text-align: right"
                                            valign="top">
                                            <a id="A_RightNav" runat="server" href="javascript:void(0);" onclick="OnRightNav_Click()">
                                                <img src="../images/RightNav.gif" width="10" />
                                            </a>
                                        </td>
                                        
                                        <%--<td style="text-align: right" valign="top">
                               <a id="A_LastNav" runat="server" href="javascript:void(0);" onclick="OnLastNav_Click()">
                                    <img src="/assests/images/LeftNav.gif" width="10"/>
                                  </a>
                               </td>--%>
                                    </tr>
                                </table>
                            </td>
                          
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="aspxExEmpGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                        Width="100%" OnCustomCallback="aspxExEmpGrid_CustomCallback" KeyFieldName="emp_id"
                        Settings-ShowHorizontalScrollBar="false">
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <SettingsPager Mode="ShowAllRecords">
                        </SettingsPager>
                        <ClientSideEvents EndCallback="aspxExEmpGrid_EndCallback" />
                        <Columns>
                            <dxe:GridViewDataTextColumn  VisibleIndex="0">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <DataItemTemplate>
                                    <%--<a href="javascript:void(0);" onclick="OnRejoinClick(this,'<%# Container.KeyValue %>')">
                                        <u>Rejoin</u> </a>--%>
                                    <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')">
                                        More Info</a>
                                </DataItemTemplate>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn  VisibleIndex="1">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <DataItemTemplate>
                                    <%--<a href="javascript:void(0);" onclick="OnRejoinClick(this,'<%# Container.KeyValue %>')">
                                        <u>Rejoin</u> </a>--%>
                                    <a href="javascript:void(0);" onclick="Show('<%# Container.KeyValue %>')">Rejoin</a>
                                </DataItemTemplate>
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataDateColumn FieldName="joindate" UnboundType="DateTime" Width="150px"
                                Caption="Joining Date" VisibleIndex="2">
                                <PropertiesDateEdit DisplayFormatString="{0:dd MMM yyyy}">
                                </PropertiesDateEdit>
                                <Settings SortMode="Value"></Settings>
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataDateColumn FieldName="Leavedate" UnboundType="DateTime" Width="150px"
                                Caption="Leaving Date" VisibleIndex="3">
                                <PropertiesDateEdit DisplayFormatString="{0:dd MMM yyyy}">
                                </PropertiesDateEdit>
                                <Settings SortMode="Value"></Settings>
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn FieldName="Name"  Caption="EmpName[code]"
                                VisibleIndex="4">
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="designation"  Caption="Designation"
                                VisibleIndex="5">
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="company"  Caption="Company"
                                VisibleIndex="6">
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="branch"  Caption="Branch" VisibleIndex="7">
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="department"  Caption="Department"
                                VisibleIndex="8">
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="reportTo"  Caption="Reporting Head"
                                VisibleIndex="9">
                                <CellStyle HorizontalAlign="Left" Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <Settings ShowHorizontalScrollBar="false"></Settings>
                    </dxe:ASPxGridView>
                    &nbsp;
                        
                <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" 
                    CloseAction="CloseButton" Top="100" Left="400" ClientInstanceName="popup" Height="530px"
                    Width="930px" HeaderText="Add Employee CTC">
                    <ContentCollection>
                        <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                </dxe:ASPxPopupControl>
                    <asp:SqlDataSource ID="SqlExEmployeeDetails" runat="server" >
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
          
   </div> 
</asp:Content>
