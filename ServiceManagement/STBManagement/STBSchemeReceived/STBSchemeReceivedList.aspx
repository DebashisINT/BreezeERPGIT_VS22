<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="STBSchemeReceivedList.aspx.cs" Inherits="ServiceManagement.STBManagement.STBSchemeReceived.STBSchemeReceivedList" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script>
        <%-- Code Added By Debashis Talukder For Document Printing End--%>
        var RCId = 0;
        function onPrintJv(id) {
            RCId = id;
            cDocumentsPopup.Show();
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }

        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }

        function cSelectPanelEndCall(s, e) {
            if (cSelectPanel.cpSuccess != null) {
                var TotDocument = cSelectPanel.cpSuccess.split(',');
                var reportName = cCmbDesignName.GetValue();
                var module = 'RECPTCHALLAN';
                window.open("../../OMS/Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + RCId, '_blank')
            }
            cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == null) {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }
        <%-- Code Added By Debashis Talukder For Document Printing End--%>

        function OnClickDelete(val) {
            jConfirm('Confirm Delete?', 'Alert', function (r) {
                if (r) {
                    $.ajax({
                        type: "POST",
                        url: "STBSchemeReceivedList.aspx/DeleteSchemeReceived",
                        data: JSON.stringify({ SchemeReceived_ID: val }),
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            // console.log(response);
                            if (response.d) {
                                if (response.d == "true") {
                                    jAlert("STB Scheme Received delete sucessfully.");
                                    var url = 'STBSchemeReceivedList.aspx';
                                    // window.location.href = url;
                                    cGrdSchemeReceivedList.Refresh();
                                }
                                else if (response.d == "Logout") {
                                    location.href = "../../OMS/SignOff.aspx";
                                }
                                else {
                                    alert(response.d);
                                }
                            }
                        },
                        error: function (response) {
                            console.log(response);
                        }
                    });
                }
            });
        }

        function SendSMS(val) {
            jConfirm('Confirm Send SMS?', 'Alert', function (r) {
                if (r) {
                    $.ajax({
                        type: "POST",
                        url: "STBSchemeReceivedList.aspx/SendSMS",
                        data: JSON.stringify({ SchemeReceived_ID: val }),
                        async: false,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            // console.log(response);
                            if (response.d) {
                                if (response.d == "Logout") {
                                    location.href = "../../OMS/SignOff.aspx";
                                }
                            }
                        },
                        error: function (response) {
                            console.log(response);
                        }
                    });
                }
            });
        }

        function OnAddButtonClick() {
            var url = "STBSchemeReceivedAdd.aspx?Key=ADD";
            window.location.href = url;
        }

        function ClickOnEdit(id) {
            location.href = "STBSchemeReceivedAdd.aspx?id=" + id + "&Key=edit";
        }

        function ClickOnView(id) {
            location.href = "STBSchemeReceivedAdd.aspx?id=" + id + "&Key=view";
        }

        function gridRowclick(s, e) {
            //alert('hi');
            $('#GrdSchemeReceived').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        //console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            setTimeout(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdSchemeReceivedList.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdSchemeReceivedList.SetWidth(cntWidth);
                }
                cGrdSchemeReceivedList.Refresh();
            }, 1000);


            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
                    cGrdSchemeReceivedList.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    cGrdSchemeReceivedList.SetWidth(cntWidth);
                }

            });
        });
    </script>

    <style>
        .drHover {
            display: inline-block;
            margin-bottom: 5px;
        }

            .drHover .dropdown-menu > li > a:hover {
                background: #1987c6;
            }

            .drHover > button.ft {
                border-radius: 15px 0 0 15px;
            }

            .drHover > button.sd {
                border-radius: 0 15px 15px 0;
                height: 31px;
            }

        .padTab > tbody > tr > td {
            padding-right: 15px;
            vertical-align: middle;
        }

            .padTab > tbody > tr > td > label {
                margin-bottom: 0 !important;
            }

            .padTab > tbody > tr > td > .btn {
                margin-top: 0 !important;
            }
    </style>

    <script>
        function updateGridByDate() {
            //debugger;
            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else if (ccmbBranchfilter.GetValue() == null) {
                jAlert('Please select Branch.', 'Alert', function () { ccmbBranchfilter.Focus(); });
            }
            else {
                localStorage.setItem("FromDateSalesOrder", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDateSalesOrder", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("OrderBranch", ccmbBranchfilter.GetValue());
                //cGrdOrder.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBranchfilter.GetValue());
                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBranchfilter.GetValue());
                $("#hfIsFilter").val("Y");
                cGrdSchemeReceivedList.Refresh();
            }
        }
    </script>

    <script>

        function OnClickApprove(values) {
            $("#hdnSchemeReceived_ID").val(values);
            $("#assignpop").modal('toggle');
        }

        function Approval_Click() {
          
            var Remarks = $("#txtRemarks").val();
            var SchemeReceived_id = $("#hdnSchemeReceived_ID").val();

            if (cdddlApprovalEmployee.GetValue() == "0") {
                jAlert("Please Select Approval Employee.", "Alert", function () {
                    setTimeout(function () {
                        LoadingPanel.Hide();
                        cdddlApprovalEmployee.Focus();
                        return
                    }, 200);
                });
                return
            }
          var  ApprovalEmployee = cdddlApprovalEmployee.GetValue();

            $.ajax({
                type: "POST",
                url: "STBSchemeReceivedList.aspx/DircetorApproval",
                data: JSON.stringify({ ApprovalEmployee: ApprovalEmployee, SchemeReceived_id: SchemeReceived_id, Remarks: Remarks }),
                async: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    //console.log(response);
                    if (response.d) {
                        if (response.d == "true") {
                            jAlert("Approved Successfully.", "Alert", function () {
                                cdddlApprovalEmployee.SetValue(0);
                                $("#txtRemarks").val("");
                                $("#hdnSchemeReceived_ID").val("");
                                $("#assignpop").toggle();
                                cGrdSchemeReceivedList.Refresh();
                            });
                        }
                        else if (response.d == "Logout") {
                            location.href = "../../OMS/SignOff.aspx";
                        }
                        else {
                            jAlert(response.d);
                            return
                        }
                    }
                },
                error: function (response) {
                    console.log(response);
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>STB Scheme - Received</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px">
            <tr>
                <td>
                    <label>From </label>
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <label>To </label>
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>Unit</td>
                <td>
                    <dxe:ASPxComboBox ID="cmbBranchfilter" runat="server" ClientInstanceName="ccmbBranchfilter" Width="100%">
                    </dxe:ASPxComboBox>
                </td>
                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                </td>

            </tr>

        </table>
    </div>
    <div class="form_main">
        <table class="TableMain100" cellpadding="0px" width="100%">
            <tr>
                <td>
                    <% if (rights.CanAdd)
                       { %>
                    <span class="btn-group drHover">
                        <button type="button" class="btn btn-success btn-radius" id="Team" onclick="OnAddButtonClick();">Add STB Scheme - Received</button>
                    </span>
                    <%} %>

                    <% if (rights.CanExport)
                       { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" AutoPostBack="true" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                    <% } %>
                </td>
            </tr>
            <tr>
                <td class="gridcellcenter relative" colspan="2">
                    <dxe:ASPxGridView ID="GrdSchemeReceivedList" runat="server" KeyFieldName="SchemeReceived_ID" AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true"
                        SettingsPager-Mode="ShowAllRecords" Settings-VerticalScrollBarMode="auto" Settings-VerticalScrollableHeight="280" Settings-HorizontalScrollBarMode="Auto"
                        DataSourceID="EntityServerModeDataSource"
                        Width="100%" ClientInstanceName="cGrdSchemeReceivedList">
                        <settingssearchpanel visible="True" delay="5000" />
                        <columns>
                         
                            <dxe:GridViewDataTextColumn Caption="Document No" FieldName="DocumentNumber"
                                VisibleIndex="2" Width="20%">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn Caption="Date" Width="10%" FieldName="DocumentDate" VisibleIndex="3">
                                <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Location" FieldName="branch_description" VisibleIndex="4" Width="15%">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Caption="Entity Code" FieldName="EntityCode" VisibleIndex="5" Width="15%">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Caption="Network Name" FieldName="NetworkName" VisibleIndex="6" Width="15%">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                              <dxe:GridViewDataTextColumn Caption="Contact Person" FieldName="ContactPerson" VisibleIndex="7" Width="15%">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                               <dxe:GridViewDataTextColumn Caption="Contact Number" FieldName="ContactNo" VisibleIndex="8" Width="15%">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                             <dxe:GridViewDataTextColumn Caption="Status" FieldName="Status" VisibleIndex="9" Width="15%">
                                <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Entered By" FieldName="CREATE_BY" Width="10%" VisibleIndex="10">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                              <dxe:GridViewDataTextColumn Caption="Entered On" FieldName="Create_date" Width="10%" VisibleIndex="11">
                               <%--<PropertiesTextEdit DisplayFormatString="dd-MM-yyyy HH:mm:ss"></PropertiesTextEdit>--%>
                                     <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                              <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="UPDATE_BY" Width="10%" VisibleIndex="12">
                                <CellStyle CssClass="gridcellleft" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>
                              <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="Update_date" Width="10%" VisibleIndex="13">
                                <%--<PropertiesTextEdit DisplayFormatString="dd-MM-yyyy HH:mm:ss"></PropertiesTextEdit>--%>
                                     <CellStyle CssClass="gridcellright" Wrap="true">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <Settings AutoFilterCondition="Contains" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="14" Width="0">
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                         <% if (rights.CanView)
                                            { %>
                                        <a href="javascript:void(0);" onclick="ClickOnView('<%# Container.KeyValue %>')" id="a_viewInvoice" class="" title="View">
                                            <span class='ico editColor'><i class='fa fa-eye' aria-hidden='true'></i></span><span class='hidden-xs'>View</span></a>
                                        <%} %>
                                        <% if (rights.CanEdit)
                                           { %>
                                        <a href="javascript:void(0);" onclick="ClickOnEdit('<%# Container.KeyValue %>')" id="a_editInvoice" style='<%#Eval("EditStaus")%>' class="" title="Edit">
                                            <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>
                                        <%} %>
                                        <% if (rights.CanDelete)
                                           { %>
                                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" class="" style='<%#Eval("EditStaus")%>' title="Delete" id="a_delete">
                                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                                        <%} %>                                           
                                         <% if (rights.CanApproved)
                                            { %>
                                         <a href="javascript:void(0);" onclick="OnClickApprove('<%# Container.KeyValue %>')" class="" style='<%#Eval("EditStaus")%>' title="Approve" id="a_Assign"  data-toggle='tooltip' data-placement='left'  >
                                            <span class='ico printColor'><i class='fa fa-user-plus assig' aria-hidden='true'></i>
                                            </span><span class='hidden-xs'>Approve</span></a>
                                        <%} %>
                                        <% if (rights.CanPrint)
                                              { %>
                                        <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>')" class="" title="Print" id="a_Print">
                                            <span class='ico printColor'><i class='fa fa-print det' aria-hidden='true'></i></span><span class='hidden-xs'>Print</span></a>
                                        <%} %>
                                    </div>
                                </DataItemTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate><span></span></HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                        </columns>
                        <settingscontextmenu enabled="true"></settingscontextmenu>
                        <clientsideevents rowclick="gridRowclick" />
                        <settingspager numericbuttoncount="10" pagesize="10" showseparators="True" mode="ShowPager">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </settingspager>
                        <settings showgrouppanel="True" showstatusbar="Hidden" showhorizontalscrollbar="False" showfilterrow="true" showfilterrowmenu="true" />
                        <settingsloadingpanel text="Please Wait..." />
                    </dxe:ASPxGridView>
                    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                        ContextTypeName="ServicveManagementDataClassesDataContext" TableName="V_SchemeReceivedReportList" />
                    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                    </dxe:ASPxGridViewExporter>
                </td>
            </tr>

        </table>
    </div>

     <asp:HiddenField ID="hfIsFilter" runat="server" />
    <asp:HiddenField ID="hfFromDate" runat="server" />
    <asp:HiddenField ID="hfToDate" runat="server" />
    <asp:HiddenField ID="hfBranchID" runat="server" />
    <asp:HiddenField ID="hdnIsUserwiseFilter" runat="server" />
    <asp:HiddenField ID="hddnKeyValue" runat="server" />
    <asp:HiddenField ID="hddnCancelCloseFlag" runat="server" />



    
    <div class="modal fade pmsModal w30" id="assignpop" role="dialog" aria-labelledby="assignpop" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Director Approval</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row ">
                        <div class="col-md-6">
                            <label class="deep">Director</label>
                            <div class="dropDev" id="DivTechnician">
                                    <dxe:ASPxComboBox ID="dddlApprovalEmployee" runat="server" ClientInstanceName="cdddlApprovalEmployee" Width="100%">
                            </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <div class="clear"></div>
                        <div class="col-md-12 mTop5">
                            <label class="deep">Remarks </label>
                            <div class="fullWidth">
                                <textarea class="form-control" id="txtRemarks" maxlength="500" rows="5"></textarea>
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField ID="hdnSchemeReceived_ID" runat="server" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
                    <% if (rights.CanApproved)
                       { %>
                    <button type="button" class="btn btn-success" onclick="Approval_Click();">Confirm</button>
                    <%} %>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
