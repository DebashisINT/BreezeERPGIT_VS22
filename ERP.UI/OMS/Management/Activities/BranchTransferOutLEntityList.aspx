<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BranchTransferOutLEntityList.aspx.cs" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.BranchTransferOutLEntityList" EnableEventValidation="false" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <style>
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

    <script src="JS/BranchTransferOutLEntityList.js?v2.1"></script>
    <script>
        function OnMoreInfoClick(keyValue) {
            debugger;
            var IsExists = false;
            var ActiveUser = '<%=Session["userid"]%>'
            if (ActiveUser != null) {

                $.ajax({
                    type: "POST",
                    url: "BranchTransferOutList.aspx/GetBOIsExistInBI",
                    data: "{'keyValue':'" + keyValue + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,//Added By:Subhabrata
                    success: function (msg) {
                        debugger;
                        var status = msg.d;
                        if (status == "1") {
                            jAlert('Branch In is done.Now you can only view the document.', 'Alert Dialog', function (r) {
                                if (r == true) {
                                    $.ajax({
                                        type: "POST",
                                        url: "BranchTransferOutList.aspx/GetEditablePermission",
                                        data: "{'ActiveUser':'" + ActiveUser + "'}",
                                        contentType: "application/json; charset=utf-8",
                                        dataType: "json",
                                        async: false,//Added By:Subhabrata
                                        success: function (msg) {
                                            debugger;
                                            var status = msg.d;
                                            var url = 'BranchTransferOut.aspx?key=' + keyValue + '&Permission=' + status + '&type=BO';

                                            window.location.href = url;
                                        }
                                    });
                                }
                            });
                        }
                        else {
                            $.ajax({
                                type: "POST",
                                url: "BranchTransferOutList.aspx/GetEditablePermission",
                                data: "{'ActiveUser':'" + ActiveUser + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: false,//Added By:Subhabrata
                                success: function (msg) {
                                    debugger;
                                    var status = msg.d;
                                    var url = 'BranchTransferOut.aspx?key=' + keyValue + '&Permission=' + status + '&type=BO';

                                    window.location.href = url;
                                }
                            });
                        }


                    }
                });


            }
        }       
        //Rev work start 12.07.2022 mantise no :0025011: Update E-way Bill
        function OnEWayBillClick(id, VisibleIndex, EWayBillNumber, EWayBillDate) {
            debugger;
            cGrdOrder.SetFocusedRowIndex(VisibleIndex);
            if (EWayBillNumber.trim() != "") {
                ctxtEWayBillNumber.SetText(EWayBillNumber);
            }
            else {
                ctxtEWayBillNumber.SetText("");
            }
            if (EWayBillDate.trim() != "" && EWayBillDate.trim() != "01-01-1970" && EWayBillDate.trim() != "01-01-1900" && EWayBillDate.trim() != "01-01-0100") {
                var d = new Date(EWayBillDate.split('-')[2].trim(), EWayBillDate.split('-')[1].trim() - 1, EWayBillDate.split('-')[0].trim(), 0, 0, 0, 0);
                cdt_EWayBill.SetDate(d);
            }
            else {
                cdt_EWayBill.SetText("");
            } 
            $('#hddnInvoiceID').val(id);
            cPopup_EWayBill.Show();
            ctxtEWayBillNumber.Focus();
        }
        function CancelEWayBill_save() {
            cPopup_EWayBill.Hide();
        }
        function GetEWayBillDateFormat(today) {
            if (today != "") {
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!

                var yyyy = today.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                today = yyyy + '-' + mm + '-' + dd;
            }

            return today;
        }
        function CallEWayBill_save() {
            var Stk_Id = $("#<%=hddnInvoiceID.ClientID%>").val();           
            var UpdateEWayBill = ctxtEWayBillNumber.GetValue();
            if (UpdateEWayBill == "0") {
                UpdateEWayBill = "";
            }
            if (cdt_EWayBill.GetValue() == "" && cdt_EWayBill.GetValue() == null) {
                var EWayBillDate = "1990-01-01";
            }
            else {
                //var EWayBillDate = GetEWayBillDateFormat(new Date(cdt_EWayBill.GetValue()));
                //Rev Subhra  0019106  11/12/2018
                if (cdt_EWayBill.GetValue() == null) {
                    var EWayBillDate = null;
                }
                else {
                    var EWayBillDate = GetEWayBillDateFormat(new Date(cdt_EWayBill.GetValue()));
                }
                //End of Rev 
            }           
            $.ajax({
                type: "POST",
                url: "BranchTransferOutLEntityList.aspx/UpdateEWayBill",
                data: JSON.stringify({
                    Stk_Id: Stk_Id, UpdateEWayBill: UpdateEWayBill, EWayBillDate: EWayBillDate
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (msg) {
                    var status = msg.d;
                    if (status == "1") {
                        jAlert("Saved successfully.");
                        //ctxtEWayBillNumber.SetText("");
                        cPopup_EWayBill.Hide();
                        //Rev work start 03.08.2022 mantise no :0025011: Update E-way Bill
                        //cGrdQuotation.Refresh();
                        cGrdOrder.Refresh();
                        //Rev work close 03.08.2022 mantise no :0025011: Update E-way Bill
                    }
                    else if (status == "-10") {
                        jAlert("Data not saved.");
                        cPopup_EWayBill.Hide();
                    }
                }
            });
            //}           
        }
        //Rev work close 12.07.2022 mantise no :0025011: Update E-way Bill
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxPopupControl ID="Popup_OrderStatus" runat="server" ClientInstanceName="cOrderStatus"
        Width="500px" HeaderText="Approvers Configuration" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-12">
                            <table width="100%">
                                <tr>
                                    <td style="padding-right: 20px">
                                        <label style="margin-bottom: 5px">Branch Transfer Out</label>
                                    </td>
                                    <td>
                                        <%--<dxe:ASPxTextBox ID="txt_Proforma" MaxLength="80" ClientInstanceName="cProforma" TabIndex="1" 
                                                runat="server" Width="100%"> 
                                            </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxLabel ID="lbl_Proforma" runat="server" ClientInstanceName="cProforma" Text="ASPxLabel"></dxe:ASPxLabel>
                                    </td>
                                    <td style="padding-right: 20px; padding-left: 8px">
                                        <label style="margin-bottom: 5px">Customer</label>
                                    </td>
                                    <td>
                                        <%-- <dxe:ASPxTextBox ID="txt_Customer" ClientInstanceName="cCustomer"  runat="server" MaxLength="100" TabIndex="2"
                                            Width="100%"> 
                                        </dxe:ASPxTextBox>--%>
                                        <dxe:ASPxLabel ID="lbl_Customer" runat="server" ClientInstanceName="cCustomer" Text="ASPxLabel"></dxe:ASPxLabel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-md-6">
                        </div>
                        <div class="col-md-6">
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
                <div class="col-md-12">
                    <table>
                        <tr>
                            <td style="width: 70px; padding: 13px 0;">Status </td>
                            <td>
                                <asp:RadioButtonList ID="rbl_OrderStatus" runat="server" Width="250px" CssClass="mTop5" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Pending" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Accepted" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Pending" Value="3"></asp:ListItem>
                                </asp:RadioButtonList>

                            </td>
                        </tr>
                    </table>





                </div>
                <div class="clear"></div>
                <div class="col-md-12">

                    <div class="" style="margin-bottom: 5px;">
                        Reason 
                    </div>

                    <div>
                        <dxe:ASPxMemo ID="txt_OrderRemarks" runat="server" ClientInstanceName="cOrderRemarks" Height="71px" Width="100%"></dxe:ASPxMemo>
                    </div>
                </div>

                <div class="col-md-12" style="padding-top: 10px;">
                    <dxe:ASPxButton ID="btn_PrpformaStatus" CausesValidation="true" ClientInstanceName="cbtn_PrpformaStatus" runat="server"
                        AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                        <ClientSideEvents Click="function (s, e) {SavePrpformaStatus();}" />
                    </dxe:ASPxButton>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
    </dxe:ASPxPopupControl>

    <dxe:ASPxPopupControl ID="Popup_Feedback" runat="server" ClientInstanceName="cPopup_Feedback"
        Width="400px" HeaderText="Reason For Cancel" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                <div class="Top clearfix">

                    <table style="width: 94%">

                        <tr>
                            <td>Reason<span style="color: red">*</span></td>
                            <td class="relative">
                                <dxe:ASPxMemo ID="txtInstFeedback" runat="server" Width="100%" Height="50px" ClientInstanceName="txtFeedback"></dxe:ASPxMemo>
                                <span id="MandatoryRemarksFeedback" style="display: none">
                                    <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="3" style="padding-left: 121px;">
                                <input id="btnFeedbackSave" class="btn btn-primary" onclick="CallFeedback_save()" type="button" value="Save" />
                                <input id="btnFeedbackCancel" class="btn btn-danger" onclick="CancelFeedback_save()" type="button" value="Cancel" />
                            </td>

                        </tr>
                    </table>


                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />


    </dxe:ASPxPopupControl>
    <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Branch Transfer Out</h3>
        </div>
        <table class="padTab pull-right" style="margin-top: 7px">
            <tr>
                <td>
                    <label>From Date</label></td>
                <td>
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <label>To Date</label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>Branch</td>
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
        <div class="clearfix">
            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus"></i></span><span><u>A</u>dd New</span> </a>
            <% } %>

            <% if (rights.CanExport)
               { %>
            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>
            <% } %>
        </div>
    </div>

      <div id="spnEditLock" runat="server" style="display:none; color:red;text-align:center"></div>
     <div id="spnDeleteLock" runat="server" style="display:none; color:red;text-align:center"></div>

    <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="GrdOrder" runat="server" KeyFieldName="SlNo" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="cGrdOrder" OnCustomCallback="GrdOrder_CustomCallback" OnHtmlRowPrepared="AvailableStockgrid_HtmlRowPrepared"
            DataSourceID="EntityServerModeDataSource" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"
            SettingsCookies-Enabled="true" SettingsCookies-StorePaging="true" SettingsCookies-StoreFiltering="true" SettingsCookies-StoreGroupingAndSorting="true"
            SettingsBehavior-AllowFocusedRow="true" Settings-HorizontalScrollBarMode="Auto"
            OnSummaryDisplayText="ShowGrid_SummaryDisplayText" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto">
            <Columns>
                <dxe:GridViewDataTextColumn Caption="Document No." FieldName="Out_No" Width="180px"
                    VisibleIndex="0" FixedStyle="Left" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="false">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="Out_Date" Width="120px"
                    VisibleIndex="1" FixedStyle="Left" Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="false">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="From Branch" FieldName="From_Branch" Width="300px"
                    VisibleIndex="2"  Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="To Branch" FieldName="To_Branch" Width="300px"
                    VisibleIndex="3"  Settings-ShowFilterRowMenu="True">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Amount" FieldName="Amount" Width="120px" HeaderStyle-HorizontalAlign="Right"
                    VisibleIndex="4" >
                    <PropertiesTextEdit DisplayFormatString="0.00" ></PropertiesTextEdit>
                    <PropertiesTextEdit>
                        <MaskSettings Mask="<0..999999999>.<0..9999>" AllowMouseWheel="false" />
                    </PropertiesTextEdit>
                    <CellStyle Wrap="False" CssClass="gridcellleft" HorizontalAlign="Right"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Purpose" FieldName="Purpose" Width="200px"
                    VisibleIndex="5" >
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

              <%--  <dxe:GridViewDataTextColumn Caption="Status" FieldName="Purpose" 
                    VisibleIndex="5"  Visible="false" Width="0px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>--%>

                <dxe:GridViewDataTextColumn Caption="Project Name" FieldName="Proj_Name" 
                    VisibleIndex="6"  Width="180px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="Branch Requisition" FieldName="Indent_RequisitionNumber"
                    VisibleIndex="7"  Width="180px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>

                <dxe:GridViewDataTextColumn VisibleIndex="8" Caption="Entered By" FieldName="EnteredBy" Width="180px">
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="9" Caption="Entered On" FieldName="EnteredOn" Width="180px">
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="10" Caption="Updated By" FieldName="UpdatedBy" Width="180px">
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn VisibleIndex="11" Caption="Updated On" FieldName="Updated" Width="180px" >
                    <CellStyle CssClass="gridcellleft"></CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                <dxe:GridViewDataTextColumn Caption="IsCancel" FieldName="IsCancel"
                    VisibleIndex="13"  Width="0" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                    <CellStyle CssClass="hide" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>
                <%--Rev work start 12.07.2022 mantise no :0025011: Update E-way Bill--%>
                   <dxe:GridViewDataTextColumn Caption="E-Way Bill No" FieldName="EWayBillNumber" VisibleIndex="12" Width="100px">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>
                <%--Rev work close 12.07.2022 mantise no :0025011: Update E-way Bill--%>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="17" Width="0">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                            <% if (rights.CanView)
                               { %>
                            <a href="javascript:void(0);" onclick="OnViewClick('<%#Eval("Stk_Id")%>')" class="" title="">
                                <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>
                            <% } %>
                            <% if (rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%#Eval("Stk_Id")%>')" class="" title="" style='<%#Eval("Editlock")%>'>

                                <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a>  <% } %>
                            <% if (rights.CanCancel)
                               { %>
                            <a href="javascript:void(0);" onclick="OnCancelClick('<%#Eval("Stk_Id")%>',<%# Container.VisibleIndex %>)" class="" title="">

                                <%--<i class="fa fa-truck out" ></i>--%>
                                <span class='ico deleteColor'><i class='fa fa-times' aria-hidden='true'></i></span><span class='hidden-xs'>Cancel BTO</span>

                            </a><% } %>
                            <% if (rights.CanDelete)
                               { %>
                            <a href="javascript:void(0);" onclick="OnClickDelete('<%#Eval("Stk_Id")%>')" class="" title="" style='<%#Eval("Deletelock")%>'>
                                <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>
                            <% } %>
                            <a href="javascript:void(0);" onclick="OnClickCopy('<%#Eval("Stk_Id")%>')" class="" title=" " style="display: none">
                                <span class='ico ColorSix'><i class='fa fa-copy'></i></span><span class='hidden-xs'>Copy</span></a>
                            <a href="javascript:void(0);" onclick="OnClickStatus('<%#Eval("Stk_Id")%>')" class="" title="" style="display: none">
                                <span class='ico editColor'><i class='fa fa-check' aria-hidden='true'></i></span><span class='hidden-xs'>Status</span></a>
                            <%--Mantis Issue 25127--%>
                            <%--<% if (rights.CanView)
                               { %>--%>
                            <% if (rights.CanAddUpdateDocuments)
                               { %>
                            <%--End of Mantis Issue 25127--%>
                            <a href="javascript:void(0);" onclick="OnclickViewAttachment('<%#Eval("Stk_Id")%>')" class="" title="">
                                <span class='ico ColorSix'><i class='fa fa-paperclip'></i></span><span class='hidden-xs'>Add/View Attachment</span> </a>
                            <% } %>
                            <%--Rev work start 12.07.2022 mantise no :0025011: Update E-way Bill--%>
                            <% if (rights.CanEdit)
                               { %>
                            <a href="javascript:void(0);" onclick="OnEWayBillClick('<%#Eval("Stk_Id") %>','<%# Container.VisibleIndex %>','<%#Eval("EWayBillNumber") %>','<%#Eval("EWayBillDate") %>')" class="" title="">
                                <span class='ico ColorFour'><i class='fa fa-file-text-o'></i></span><span class='hidden-xs'>Update E-Way Bill</span></a>
                            <% } %>
                            <%--Rev work close 12.07.2022 mantise no :0025011: Update E-way Bill--%>
                            <% if (rights.CanPrint)
                               { %>
                            <a href="javascript:void(0);" onclick="onPrintJv('<%#Eval("Stk_Id")%>')" class="" title="">
                                <span class='ico ColorSeven'><i class='fa fa-print'></i></span><span class='hidden-xs'>Print</span>
                            </a><%} %>
                            <%--Mantis Issue 25238--%>
                             <% if (rights.SendSMS)
                               { %>
                           <a href="javascript:void(0);" onclick="onSmsClickJv('<%#Eval("Stk_Id")%>')" id="onSmsClickJv" class="" title="">
                               <span class='ico deleteColor'><i class='fa fa-commenting-o' aria-hidden='true'></i></span><span class='hidden-xs'>Send Sms</span>
                            </a><%} %>
                            <%--End of Mantis Issue 25238--%>
                        </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span></span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
            <%--<SettingsCookies Enabled="true" StorePaging="true" Version="2.00" />--%>
            <SettingsCookies Enabled="true" StorePaging="true" Version="5.01" />
            <SettingsSearchPanel Visible="true" />
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
            <%--<SettingsPager NumericButtonCount="20" PageSize="10" ShowSeparators="True">
                <FirstPageButton Visible="True">
                </FirstPageButton>
                <LastPageButton Visible="True">
                </LastPageButton>
            </SettingsPager>--%>
            <SettingsPager PageSize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>
            <%-- <SettingsSearchPanel Visible="True" />--%>
            <Settings ShowGroupPanel="True" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="false" />
            <SettingsLoadingPanel Text="Please Wait..." />
            <TotalSummary>
                <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" />
            </TotalSummary>
        </dxe:ASPxGridView>
        <asp:HiddenField ID="hiddenedit" runat="server" />
        <asp:HiddenField ID="hddnKeyValue" runat="server" />
        <asp:HiddenField ID="hddnIsSavedFeedback" runat="server" />

        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_BranchStockOutEntityList" />


    </div>
    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>

     <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">           
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <%--DEBASHIS--%>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="ASPxDocumentsPopup" runat="server" ClientInstanceName="cDocumentsPopup"
            Width="350px" HeaderText="Select Design(s)" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <dxe:ASPxCallbackPanel runat="server" ID="SelectPanel" ClientInstanceName="cSelectPanel" OnCallback="SelectPanel_Callback" ClientSideEvents-EndCallback="cSelectPanelEndCall">
                        <PanelCollection>
                            <dxe:PanelContent runat="server">

                                <dxe:ASPxCheckBox ID="selectBranchCopy" Text="Branch Copy" runat="server" ToolTip="Select Branch Copy"
                                    ClientInstanceName="CselectBranchCopy">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="selectGodownCopy" Text="Godown Copy" runat="server" ToolTip="Select Godown Copy"
                                    ClientInstanceName="CselectGodownCopy">
                                </dxe:ASPxCheckBox>


                                <dxe:ASPxComboBox ID="CmbDesignName" ClientInstanceName="cCmbDesignName" runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                </dxe:ASPxComboBox>
                                <div class="text-center pTop10">
                                    <dxe:ASPxButton ID="btnOK" ClientInstanceName="cbtnOK" runat="server" AutoPostBack="False" Text="OK" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) {return PerformCallToGridBind();}" />
                                    </dxe:ASPxButton>
                                </div>
                            </dxe:PanelContent>
                        </PanelCollection>
                    </dxe:ASPxCallbackPanel>
                </dxe:PopupControlContentControl>
            </ContentCollection>
        </dxe:ASPxPopupControl>
    </div>
    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <ClientSideEvents ControlsInitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
    <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
    </div>
    <%--Rev work start 12.07.2022 mantise no :0025011: Update E-way Bill--%>
    <dxe:ASPxPopupControl ID="Popup_EWayBill" runat="server" ClientInstanceName="cPopup_EWayBill"
        Width="400px" HeaderText="Update E-Way Bill" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="150px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">
                    <table style="width: 100%; margin: 0 auto; margin-top: 5px;">
                        <tr>
                            <label>
                                <dxe:ASPxLabel ID="ASPxLabel12" runat="server" Text="E-Way Bill Number">
                                </dxe:ASPxLabel>
                            </label>
                            <dxe:ASPxTextBox ID="txtEWayBillNumber" MaxLength="12" ClientInstanceName="ctxtEWayBillNumber"
                                runat="server" Width="100%">
                                <%-- <MaskSettings Mask="<0..999999999999>" AllowMouseWheel="false" />--%>
                                <ValidationSettings RequiredField-IsRequired="false" ErrorDisplayMode="None"></ValidationSettings>
                            </dxe:ASPxTextBox>
                        </tr>
                        <tr>
                            <td>
                                <label style="margin-top: 6px">
                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text="Exp. Date">
                                    </dxe:ASPxLabel>
                                </label>
                                <dxe:ASPxDateEdit ID="dt_EWayBill" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_EWayBill" Width="100%" UseMaskBehavior="True">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>                       
                    </table>
                    <div style="margin-top: 10px;">
                        <input id="btnEWayBillSave" class="btn btn-primary" onclick="CallEWayBill_save()" type="button" value="Save" />
                        <input id="btnEWayBillCancel" class="btn btn-danger" onclick="CancelEWayBill_save()" type="button" value="Cancel" />
                        <dxe:ASPxLabel ID="lblEwayBillStatus" runat="server" Text="" Style="color: red; font-size: large"></dxe:ASPxLabel>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <asp:HiddenField ID="hddnInvoiceID" runat="server" />
    <%--Rev work close 12.07.2022 mantise no :0025011: Update E-way Bill--%>
    <asp:HiddenField ID="hdnLockFromDateedit" runat="server" />
<asp:HiddenField ID="hdnLockToDateedit" runat="server" />
    <asp:HiddenField ID="hFilterType" runat="server" />
 <asp:HiddenField ID="hdnLockFromDatedelete" runat="server" />
    <asp:HiddenField ID="hdnLockToDatedelete" runat="server" />
</asp:Content>

