<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InstallationCoupon.aspx.cs" Inherits="ERP.OMS.Management.Activities.InstallationCoupon"
    MasterPageFile="~/OMS/MasterPage/ERP.Master" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .disableClick {
            pointer-events: none;
        }
        
    </style>
    <script type="text/javascript">

        var InvoiceId = 0;
        function onPrintJv(id,Invoice_ID) {
            debugger;
            InvoiceId = Invoice_ID;
            cDocumentsPopup.Show();
            cCmbDesignName.SetSelectedIndex(0);
            cSelectPanel.PerformCallback('Bindalldesignes');
            $('#btnOK').focus();
        }

        function cSelectPanelEndCall(s, e) {
            debugger;
            if (cSelectPanel.cpSuccess != null) {
                var reportName = cCmbDesignName.GetValue();
                var module = 'Install_Coupon';
                window.open("../../Reports/REPXReports/RepxReportViewer.aspx?Previewrpt=" + reportName + '&modulename=' + module + '&id=' + InvoiceId, '_blank')
            }
            cSelectPanel.cpSuccess = null
            if (cSelectPanel.cpSuccess == null) {
                cCmbDesignName.SetSelectedIndex(0);
            }
        }

         function PerformCallToGridBind() {
              cSelectPanel.PerformCallback('Bindsingledesign');
              cDocumentsPopup.Hide();
              return false;
          }




            function InstallationDetails(InvoiceDetails_Id, Invoice_Id) {
                //alert('InvoiceDetails_Id: ' + InvoiceDetails_Id);
                //alert('Invoice_Id: ' + Invoice_Id);
                cInstallationDetailsPopup.SetContentUrl("posSalesInvoice.aspx?key=" + Invoice_Id + "&&Viemode=1&&Icoupon=yes");
                cInstallationDetailsPopup.Show();
            }
            function IconChange() {
                $(function () {

                    var $tr = $("#gridInstallationCoupon_DXMainTable > tbody > tr:gt(1)");

                    $tr.each(function (index, value) {
                        debugger;
                        var $a = $(this).find("td").eq(8).find("a").eq(1);
                        var $IsInstallVerified = $a.attr("onclick");

                        if (typeof ($IsInstallVerified) != "undefined") {
                            var params = $IsInstallVerified.split("OnVerifyButtonClick(")[1].split(',');
                            $IsInstallVerified = params[1].replace("'", "").replace("'", "");

                            if ($IsInstallVerified.indexOf("True") >= 0) {
                                var verified_Img_Src = "../../../assests/images/verified.png";
                                $a.attr("title", "verified");
                                $a.find("img").eq(0).attr('src', verified_Img_Src);
                            }
                        }

                    });

                });
            }
            function OnVerifyButtonClick(InvoiceDetails_Id, IsInstallVerified) {
                var VerifyFlag = IsInstallVerified == "True" ? 1 : 0;

                $("#hidden_InvoiceDetails_Id").val(InvoiceDetails_Id);
                cPopup_Installationverification.Show();
                cverifyCombo.SetValue(VerifyFlag);
            }
            $(document).ready(function () {

                IconChange();

                $("#btnInstallationverificationOk").on("click", function () {
                    var Isverify = cverifyCombo.GetValue();
                    if (Isverify == null || Isverify == '') {
                        jAlert('Please select verify before proceed.')
                    }
                    else {
                        $.ajax({
                            type: "POST",
                            url: "InstallationCoupon.aspx/verified",
                            data: JSON.stringify({ 'InvoiceDetails_Id': $("#hidden_InvoiceDetails_Id").val(), 'IsInstallVerified': Isverify }),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                cPopup_Installationverification.Hide();
                                grid.Refresh();
                                jAlert("Verification status updated successfully.", "Alert message!!", function () {
                                    //location.reload();
                                });
                            },
                            failure: function (response) {
                                jAlert("Error");
                            }
                        });
                    }
                });

                $("#btnInstallationverificationCancel").on("click", function () {
                    cPopup_Installationverification.Hide();
                });

            });
            function OnBeginAfterCallback(s, e) {
                IconChange();
            }

    </script>

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Installation Coupon</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100" style="width: 100%">
            <%--<tr style="display:none;">
                <td>Branch:</td>
                <dxe:ASPxComboBox ID="CmbBranch" runat="server" ClientInstanceName="ccmbbranch" TextField="branchName" ValueField="branchID"
                    OnSelectedIndexChanged="CmbBranch_SelectedIndexChanged" AutoPostBack="true">
                </dxe:ASPxComboBox>
            </tr>
            <tr style="display:none;">
                <td style="height: 10px;"></td>
            </tr>--%>
            <tr>
                <td style="vertical-align: top; text-align: left" colspan="2">
                    <%--<dxe:ASPxGridView ID="gridInstallationCoupon" ClientInstanceName="grid" Width="100%"
                        KeyFieldName="InvoiceDetails_Id" runat="server"
                        AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true">--%>
                    <dxe:ASPxGridView ID="gridInstallationCoupon" ClientInstanceName="grid" Width="100%"
                        KeyFieldName="InvoiceDetails_Id" runat="server"
                        AutoGenerateColumns="False" SettingsBehavior-AllowFocusedRow="true" DataSourceID="EntityServerModeDataSource">
                        <ClientSideEvents BeginCallback="OnBeginAfterCallback" EndCallback="OnBeginAfterCallback" />
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" FieldName="Invoice_Id" Caption="Invoice_Id">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Invoice_Number"
                                Caption="Document No.">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Invoice_Date"
                                Caption="Posting Date">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="branch_code" 
                                Caption="Unit">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="sProducts_Code"
                                Caption="Prod Code">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="brand"
                                Caption="Brand" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="model"
                                Caption="Model" Visible="true">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="InvoiceDetails_Quantity"
                                Caption="Qty" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Verified"
                                Caption="Verified" Visible="True">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <EditFormSettings Visible="True"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="0">
                                <DataItemTemplate>

                                    <%--<% if (rights.CanAdd)
                                       { %>--%>
                                    <a href="javascript:void(0);" title="Stock" onclick="InstallationDetails('<%# Container.KeyValue%>', '<%# Eval("Invoice_Id") %>')">
                                        <img src="../../../assests/images/viewIcon.png" />
                                    </a>
                                    <a href="javascript:void(0);" title="Not verified" class="pad" id="NotVerifiedLink" onclick="OnVerifyButtonClick('<%# Container.KeyValue %>', '<%# Eval("IsInstallVerified") %>')">
                                        <img src="../../../assests/images/not-verified.png" />
                                    </a>

                                    <%--<% } %>
                                    <% else
                                       { %>--%>
                                    <%--<a href="javascript:void(0);" title="received" class="disableClick">
                                        <img src="../../../assests/images/verified.png" />
                                    </a>
                                    <% } %>--%>

                                    <%--<% if (rights.CanDelete)
                                       { %>--%>
                                    <a href="javascript:void(0);" title="Print" style="display: none;">
                                        <img src="../../../assests/images/print.png" />
                                    </a>
                                    <a href="javascript:void(0);" onclick="onPrintJv('<%# Container.KeyValue %>', '<%# Eval("Invoice_Id") %>')" class="pad" title="print">
                                    <img src="../../../assests/images/Print.png" />
                                    </a>
                                    <%-- <% } %>--%>
                                </DataItemTemplate>
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>

                        </Columns>
                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <Cell CssClass="gridcellleft">
                            </Cell>
                        </Styles>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="True" />
                       <%-- <SettingsSearchPanel Visible="True" />--%>
                    </dxe:ASPxGridView>
                    <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                        ContextTypeName="ERPDataClassesDataContext" TableName="v_InstallationInvoiceDetailsList" />
                    <input type="hidden" id="hidden_Invoice_Id" />
               <%-- -----------------Subhra-------------------- --%>
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

               <%-- ------------------------------------ --%>


                </td>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>
    <!-- Installation verification Modal -->
    <dxe:ASPxPopupControl ID="Popup_Installationverification" runat="server" ClientInstanceName="cPopup_Installationverification"
        Width="400px" HeaderText="Installation verification" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                <div class="Top clearfix">

                    <table style="width: 94%">

                        <tr>
                            <td>Verify:</td>
                            <td class="relative">
                                <dxe:ASPxComboBox ID="verifyCombo" EnableIncrementalFiltering="True" ClientInstanceName="cverifyCombo"
                                    runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True">
                                    <Items>
                                        <dxe:ListEditItem Text="Yes" Value="1" />
                                        <dxe:ListEditItem Text="No" Value="0" />
                                    </Items>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 26px"></td>
                        </tr>
                        <tr>
                            <td colspan="3" style="padding-left: 121px;">
                                <input id="btnInstallationverificationOk" class="btn btn-primary" type="button" value="Ok" />
                                <input id="btnInstallationverificationCancel" class="btn btn-danger" type="button" value="Cancel" />
                            </td>

                        </tr>

                    </table>


                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />


    </dxe:ASPxPopupControl>
    <!-- Installation Details Modal -->
    <dxe:ASPxPopupControl ID="InstallationDetailsPopup" runat="server" ClientInstanceName="cInstallationDetailsPopup"
        Width="1308px" HeaderText="Installation Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="608px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">

                <div class="Top clearfix">
                </div>

            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    <input type="hidden" id="hidden_InvoiceDetails_Id" />
</asp:Content>
