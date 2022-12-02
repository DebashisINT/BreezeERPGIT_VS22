<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frm_ManualBRSList.aspx.cs"
    Inherits="ERP.OMS.Management.DailyTask.frm_ManualBRSList" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" %>


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

    <script>
        function GlobalBillingShippingEndCallBack() { };
        var isFirstTime = true;
        function AllControlInitilize() {
            debugger;
           
            if (isFirstTime) {

                if (localStorage.getItem('FromDateODSD')) {
                    var fromdatearray = localStorage.getItem('FromDateODSD').split('-');
                    var fromdate = new Date(fromdatearray[0], parseFloat(fromdatearray[1]) - 1, fromdatearray[2], 0, 0, 0, 0);
                    cFormDate.SetDate(fromdate);
                }
                if (localStorage.getItem('ToDateODSD')) {
                    var todatearray = localStorage.getItem('ToDateODSD').split('-');
                    var todate = new Date(todatearray[0], parseFloat(todatearray[1]) - 1, todatearray[2], 0, 0, 0, 0);
                    ctoDate.SetDate(todate);
                }
                if (localStorage.getItem('BranchODSD')) {
                    if (ccmbBankfilter.FindItemByValue(localStorage.getItem('BranchODSD'))) {
                        ccmbBankfilter.SetValue(localStorage.getItem('BranchODSD'));
                    }

                }
                //updateGridByDate();
                //updateGrid();
                isFirstTime = false;
            }
        }


        //$(document).ready(function () {
        //    $('body .dpvsl-top').fadeOut('slow');
        //});

        //function updateGrid() {
        //    cGrdOrder.PerformCallback();
        //}


        //var InvoiceId = 0;
        function SetDateRange(WhichCall) {
            debugger;
            if (WhichCall == "UNCLEAR") {
                //document.getElementById("ChkConsiderAllDate").checked = true;
                //tdConsiderAllDate.style.display = 'inline';
                //tdConsiderAllDatelbl.style.display = 'inline';
                //tdDateFromlbl.style.display = 'none';
                //tdDateFromdt.style.display = 'none';
                //lblToDate.style.display = 'none';
                //ToDatedt.style.display = 'none';
            }
            if (WhichCall == "CLEAR" || WhichCall == "ALL") {
                //document.getElementById("ChkConsiderAllDate").checked = false;
                //tdConsiderAllDate.style.display = 'none';
                //tdConsiderAllDatelbl.style.display = 'none';
                //tdDateFromlbl.style.display = 'block';
                //tdDateFromdt.style.display = 'block';
                //lblToDate.style.display = 'block';
                //ToDatedt.style.display = 'block';
            }
        }

        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }

        //Function for Date wise filteration
        function updateGridByDate() {
            debugger;
            if (cFormDate.GetDate() == null) {
                jAlert('Please select from date.', 'Alert', function () { cFormDate.Focus(); });
            }
            else if (ctoDate.GetDate() == null) {
                jAlert('Please select to date.', 'Alert', function () { ctoDate.Focus(); });
            }
            else if (ccmbBankfilter.GetValue() == null) {
                jAlert('Please select Bank.', 'Alert', function () { ccmbBankfilter.Focus(); });
            }

            else {
                //Subhabrata (23-10-2017)@ To Check whether the daterange beyond 31 days for reconciliation.
                var date1 = cFormDate.GetDate();
                var date2 = ctoDate.GetDate();
                var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
                if ((diffDays * 1) > 31) {
                    jAlert('Given date range must be within 31 days.');
                    return false;
                }
                //End
                localStorage.setItem("FromDateODSD", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDateODSD", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("BranchODSD", ccmbBankfilter.GetValue());
                cgrdmanualBRS.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBankfilter.GetValue());
            }
        }
        //End

        //--------------------------26-06-2017-------------------------------------
        function callbackData(data) {

        }


        function Page_Laod() {

            
            //tdDateFromlbl.style.display = 'none';
            //tdDateFromdt.style.display = 'none';
            //lblToDate.style.display = 'none';
            //ToDatedt.style.display = 'none';

        }



        document.onkeydown = function (e) {
            if (event.keyCode == 18) isCtrl = true;


            if (event.keyCode == 65 && isCtrl == true) { //run code for alt+a -- ie, Add
                StopDefaultAction(e);
                OnAddButtonClick();
            }

        }

        function StopDefaultAction(e) {
            if (e.preventDefault) { e.preventDefault() }
            else { e.stop() };

            e.returnValue = false;
            e.stopPropagation();
        }
        function OnClickDelete(keyValue) {
            //debugger;
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    cGrdOrder.PerformCallback('Delete~' + keyValue);
                }
            });
        }

        function OnclickViewAttachment(obj) {
            var URL = '/OMS/Management/Activities/SalesOrder_Document.aspx?idbldng=' + obj + '&type=SalesOrder';
            window.location.href = URL;
        }
        function OnClickStatus(keyValue) {
            GetObjectID('hiddenedit').value = keyValue;
            cGrdOrder.PerformCallback('Edit~' + keyValue);
        }

        //function grid_EndCallBack() {
        //    if (cGrdOrder.cpEdit != null) {
        //        GetObjectID('hiddenedit').value = cGrdOrder.cpEdit.split('~')[0];
        //        cProforma.SetText(cGrdOrder.cpEdit.split('~')[1]);
        //        cCustomer.SetText(cGrdOrder.cpEdit.split('~')[4]);
        //        var pro_status = cGrdOrder.cpEdit.split('~')[2];
        //        if (pro_status != null) {
        //            var radio = $("[id*=rbl_OrderStatus] label:contains('" + pro_status + "')").closest("td").find("input");
        //            radio.attr("checked", "checked");
        //            //return false;
        //            //$('#rbl_QuoteStatus[type=radio][value=' + pro_status + ']').prop('checked', true); 
        //            cOrderRemarks.SetText(cGrdOrder.cpEdit.split('~')[3]);
        //            cOrderStatus.Show();
        //        }
        //    }
        //    if (cGrdOrder.cpDelete != null) {
        //        jAlert(cGrdOrder.cpDelete);
        //        cGrdOrder.cpDelete = null;
        //    }
        //}

        function SavePrpformaStatus() {
            if (document.getElementById('hiddenedit').value == '') {
                cGrdOrder.PerformCallback('save~');
            }
            else {
                cGrdOrder.PerformCallback('update~' + GetObjectID('hiddenedit').value);
            }

        }



        function OnAddButtonClick() {
            var url = 'SalesChallanAdd.aspx?key=' + 'ADD';
            window.location.href = url;
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading clearfix">
        <div class="panel-title ">
            <h3>
                <asp:Label ID="lblHeadertext" runat="server"></asp:Label></h3>
        </div>
        <table class="padTab " style="margin-top: 7px">
            <tr>
                <td id="lblBankname">Bank</td>
                <td width="250px" id="BankValtd">
                    <dxe:ASPxComboBox ID="cmbBankfilter" runat="server" ClientInstanceName="ccmbBankfilter" Width="100%">
                    </dxe:ASPxComboBox>

                </td>

                <td id="tdDateFromlbl">
                    <label>From Date</label>

                </td>
                <td id="tdDateFromdt">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td id="lblToDate">
                    <label>To Date</label>
                </td>
                <td id="ToDatedt">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <div class="checkbox" style="padding-left: 0">
                                    <label>
                                        <asp:RadioButton ID="RdUnCleared" runat="server" Text="" Checked="true" GroupName="R" onclick="SetDateRange('UNCLEAR');" />
                                        Uncleared
                                    </label>
                                </div>
                            </td>
                            <td>
                                <div class="checkbox">
                                    <label>
                                        <asp:RadioButton ID="RdCleared" runat="server" Text="" GroupName="R" onclick="SetDateRange('CLEAR');" />
                                        Cleared
                                    </label>
                                </div>
                            </td>
                            <td>
                                <div class="checkbox">
                                    <label>
                                        <asp:RadioButton ID="RdAll" runat="server" Text="" GroupName="R" onclick="SetDateRange('ALL');" />
                                        All
                                    </label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>

            </tr>


        </table>
        <table width="400px">

            <tr>
                <td></td>

                <%--<td>--%>


                <%--</td>--%>

                <%-- <td>--%>


                <%-- </td>--%>

                <%-- <td class="mylabel1" id="tdConsiderAllDate">
                    <div class="checkbox">
                        <label>
                            <asp:CheckBox ID="ChkConsiderAllDate" runat="server" Checked="true" onclick="ChkConsiderAllDate_OnClick(this.checked)" />
                            <span id="tdConsiderAllDatelbl">All Dates</span>
                        </label>
                    </div>
                </td>--%>
            </tr>
            <tr>
                <td colspan="2">
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" /><%--</td>--%>
                    <%--<td>--%>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLSX</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>

                </td>
            </tr>
        </table>
    </div>

    <div class="clear"></div>

    <div class="GridViewArea">
        <dxe:ASPxGridView ID="grdmanualBRS" runat="server" AutoGenerateColumns="False" KeyFieldName="cashbank_vouchernumber"
            Width="100%" ClientInstanceName="cgrdmanualBRS" OnCustomCallback="grdmanualBRS_CustomCallback" OnDataBinding="grdmanualBRS_DataBinding"
            SettingsBehavior-AllowFocusedRow="true" HorizontalScrollBarMode="Auto" >
            <columns>
                    <dxe:GridViewCommandColumn VisibleIndex="0" ShowClearFilterButton="True" width="0"></dxe:GridViewCommandColumn>
                      
                    <dxe:GridViewDataTextColumn Caption="Document Date" VisibleIndex="1" FieldName="cashbank_transactionDate" Settings-ShowFilterRowMenu="True">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Document No" VisibleIndex="2" FieldName="cashbank_vouchernumber" Settings-ShowFilterRowMenu="True">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Type" VisibleIndex="3" FieldName="Type" Settings-ShowFilterRowMenu="True">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Instrument Date" VisibleIndex="4" FieldName="cashbankdetail_instrumentdate">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Instrument No" VisibleIndex="5" FieldName="cashbankdetail_instrumentnumber">
                    </dxe:GridViewDataTextColumn>
                     <dxe:GridViewDataTextColumn Caption="Instrument Type" VisibleIndex="6" FieldName="type1" Settings-ShowFilterRowMenu="True" >
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Account Name" VisibleIndex="7" FieldName="AccountCode">
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Payment Amt" VisibleIndex="8" FieldName="cashbankdetail_paymentamount">
                            <PropertiesTextEdit DisplayFormatString="{0:0.00}" Width="100%">
                                <MaskSettings Mask="<0..999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                                </PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Receipt Amt" VisibleIndex="9" FieldName="cashbankdetail_receiptamount">
                            <PropertiesTextEdit DisplayFormatString="{0:0.00}" Width="100%">
                                                    <MaskSettings Mask="<0..999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                                </PropertiesTextEdit>
                    </dxe:GridViewDataTextColumn>
                  <dxe:GridViewDataTextColumn Caption="Payee/Payer" VisibleIndex="10" width="100%" FieldName="PaidTo">
                    </dxe:GridViewDataTextColumn>
               

                    <%--<dxe:GridViewDataDateColumn FieldName="cashbankdetail_bankstatementdate" Width="14%" VisibleIndex="11"
                        Caption="Statement Date (DD-MM-YYYY)">
                        <Settings AllowAutoFilter="False" AllowGroup="False" />
                        <DataItemTemplate>
                                <dxe:ASPxDateEdit ID="txt_cashbankdate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="ctxt_cashbankdate" AllowNull="true" EditFormatString="dd-MM-yyyy" DisplayFormatString="dd-MM-yyyy"  Date="<%#Setstatementdate(Container)%>" >
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                            </dxe:ASPxDateEdit>
                        </DataItemTemplate>
                        <PropertiesDateEdit>
                                    
                        </PropertiesDateEdit>
                    </dxe:GridViewDataDateColumn>--%>
                    <dxe:GridViewDataDateColumn FieldName="cashbankdetail_bankvaluedate" Width="14%" VisibleIndex="11"
                        Caption="Reconciliation Date (DD-MM-YYYY)">
                        <Settings AllowAutoFilter="False" AllowGroup="False"  />
                                    
                        <DataItemTemplate>
                            <dxe:ASPxDateEdit ID="bankvaluedate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="cbankvaluedate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"  Date="<%#Setbankvaluedate(Container)%>">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                                           
                            </dxe:ASPxDateEdit>
                        </DataItemTemplate>
                        <PropertiesDateEdit>

                        </PropertiesDateEdit>
                    </dxe:GridViewDataDateColumn> 
                
            </columns>

            <%--<clientsideevents endcallback="function (s, e) {grid_EndCallBack();}" />--%>


            <settingspager pagesize="10">
                           <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                             </settingspager>
            <%--<settingssearchpanel visible="True" />--%>
            <settings showgrouppanel="True" ShowFooter="true"  ShowGroupFooter="VisibleIfExpanded"  showstatusbar="Hidden" showhorizontalscrollbar="false" showfilterrow="true" showfilterrowmenu="false" />
            <settingsloadingpanel text="Please Wait..." />
            <totalsummary>
                                       <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" /> 
                                 </totalsummary>
        </dxe:ASPxGridView>

        <asp:Button ID="btnUpdate" runat="server" Text="Save" CssClass="btnUpdate btn btn-primary" OnClick="btnUpdate_Click" Width="" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnUpdate btn btn-danger" OnClick="btnCancel_Click" Width="" />

        <asp:HiddenField ID="hiddenedit" runat="server" />
        <asp:HiddenField ID="hddnTypeIdd" runat="server" />
        <asp:HiddenField ID="hddnBRSConfigSettings" runat="server" />
    </div>

    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
    <%--<div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>--%>
    <%--DEBASHIS--%>

    <dxe:ASPxGlobalEvents ID="GlobalEvents" runat="server">
        <clientsideevents controlsinitialized="AllControlInitilize" />
    </dxe:ASPxGlobalEvents>
</asp:Content>
