<%--=======================================================Revision History=========================================================================
 1.0   Priti   V2.0.36  02-02-2023    0025262: Listing view upgradation required of Manual BRS of Accounts & Finance
 2.0   Priti   V2.0.38  03-04-2023   0025773: Manual BRS not working

=========================================================End Revision History========================================================================--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frm_ManualBRSEntityListFetch.aspx.cs" 
    Inherits="ERP.OMS.Management.DailyTask.frm_ManualBRSEntityListFetch" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    <script>
        function GlobalBillingShippingEndCallBack() { };
        var isFirstTime = true;
        function AllControlInitilize() {
           // debugger;

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


        function OnCancelClick(keyValue, visibleIndex) {
            debugger;

            cgrdmanualBRS.SetFocusedRowIndex(visibleIndex);
            
            //var VoucherNumber = cgrdmanualBRS.GetRow(cgrdmanualBRS.GetFocusedRowIndex()).children[2].innerText;
            //var Type = cgrdmanualBRS.GetRow(cgrdmanualBRS.GetFocusedRowIndex()).children[3].innerText;
            //var BankValueDate = cgrdmanualBRS.GetRow(cgrdmanualBRS.GetFocusedRowIndex()).children[10].innerText;
            //var instrumentDate = cgrdmanualBRS.GetRow(cgrdmanualBRS.GetFocusedRowIndex()).children[4].innerText;
            //var instrumentNumber = cgrdmanualBRS.GetRow(cgrdmanualBRS.GetFocusedRowIndex()).children[5].innerText;
            //var VoucheDate = cgrdmanualBRS.GetRow(cgrdmanualBRS.GetFocusedRowIndex()).children[1].innerText;


            /*-----Arindam  Bank reconciliation not saving Revison No-31-01-2019 01*/
            var VoucherNumber = cgrdmanualBRS.GetRow(cgrdmanualBRS.GetFocusedRowIndex()).children[1].innerText;
            var Type = cgrdmanualBRS.GetRow(cgrdmanualBRS.GetFocusedRowIndex()).children[2].innerText;
            var BankValueDate = cgrdmanualBRS.GetRow(cgrdmanualBRS.GetFocusedRowIndex()).children[9].innerText;
            var instrumentDate = cgrdmanualBRS.GetRow(cgrdmanualBRS.GetFocusedRowIndex()).children[3].innerText;
            var instrumentNumber = cgrdmanualBRS.GetRow(cgrdmanualBRS.GetFocusedRowIndex()).children[4].innerText;
            var VoucheDate = cgrdmanualBRS.GetRow(cgrdmanualBRS.GetFocusedRowIndex()).children[0].innerText;

            $("#<%=hddnVoucherNumber.ClientID%>").val(VoucherNumber);
            $("#<%=hddnModuleType.ClientID%>").val(Type);
            $("#<%=hddnBankValueDate.ClientID%>").val(BankValueDate);
            $("#<%=hddnInstrumentNo.ClientID%>").val(instrumentNumber);
            $("#<%=hddnInstrumentDate.ClientID%>").val(instrumentDate);
            $("#<%=hddnVoucherdate.ClientID%>").val(VoucheDate);
            if (BankValueDate.trim() != "") {
                var now = new Date(BankValueDate);
                var FinalDate = now.getDay() + '-' + (now.getMonth()) + '-' + now.getFullYear();
                //cdt_BankValueDate.SetDate(FinalDate);
                cdt_BankValueDate.SetText(BankValueDate);
            }
            else
            {
                cdt_BankValueDate.SetDate(null);
            }
            
            cPopup_Feedback.Show();
            //if (BankValueDate.trim() == "") {
            //    cPopup_Feedback.Show();
            //}
            //else {
            //    jAlert("Bank Reconcilliation is done already.Can not proceed.");

            //}


         }


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

                $("#dtMassrec").removeClass('hide')
                $("#btnMassrec").removeClass('hide')
                $("#btnmasssrec").text("Reconcile")

            }
            else if (WhichCall == "CLEAR") {
                $("#dtMassrec").addClass('hide');
                $("#btnMassrec").removeClass('hide')
                $("#btnmasssrec").text("Unreconcile")



            }
            else
            {
                $("#dtMassrec").addClass('hide');
                $("#btnMassrec").addClass('hide');
            }
        }

        function PerformCallToGridBind() {
            cSelectPanel.PerformCallback('Bindsingledesign');
            cDocumentsPopup.Hide();
            return false;
        }

        //Function for Date wise filteration
        function updateGridByDate() {
           // debugger;
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
                //var date1 = cFormDate.GetDate();
                //var date2 = ctoDate.GetDate();
                //var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                //var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
                //if ((diffDays * 1) > 31) {
                //    jAlert('Given date range must be within 31 days.');
                //    return false;
                //}
                //End
                localStorage.setItem("FromDateODSD", cFormDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("ToDateODSD", ctoDate.GetDate().format('yyyy-MM-dd'));
                localStorage.setItem("BranchODSD", ccmbBankfilter.GetValue());

                $("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                $("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                $("#hfBranchID").val(ccmbBankfilter.GetValue());
                $("#hfIsFilter").val("Y");

              
                TotalTabValue();

                //rev 2.0
                //  cgrdmanualBRS.Refresh();
                //$("#hFilterType").val("All");
                cCallbackPanel.PerformCallback("");
                    //end rev 2.0
               

                //cgrdmanualBRS.PerformCallback('FilterGridByDate~' + cFormDate.GetDate().format('yyyy-MM-dd') + '~' + ctoDate.GetDate().format('yyyy-MM-dd') + '~' + ccmbBankfilter.GetValue());
            }
        }
        //rev 1.0
        function CallbackPanelEndCall(s, e) {
            cgrdmanualBRS.Refresh();
        }
        //end rev 1.0
        function TotalTabValue() {
            $.ajax({
                type: "POST",
                url: "frm_ManualBRSEntityListFetch.aspx/HeaderAmount",
                data: JSON.stringify({ Account_Id: ccmbBankfilter.GetValue(), FromDate: cFormDate.GetDate().format('yyyy-MM-dd'), ToDate: ctoDate.GetDate().format('yyyy-MM-dd') }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,//Added By:Subhabrata
                success: function (msg) {
                    debugger;
                    var status = msg.d;
                    var words = status.split('~');
                    console.log(words[0]);
                    $("#divUncleared").html(words[0]);
                    $("#divCleared").html(words[1]);
                    $("#divTotal").html(words[2]);
                }
            });
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


        function CallFeedback_save() {
            debugger;
            var flag = true;
            var ValueDate = "";
            var VoucherNo = $("#<%=hddnVoucherNumber.ClientID%>").val();
            var Type = $("#<%=hddnModuleType.ClientID%>").val();

            var BankValueDate = $("#<%=hddnBankValueDate.ClientID%>").val();

            if (cdt_BankValueDate.GetDate() == null) {
                if (BankValueDate.trim() == "")
                {
                    jAlert("Please enter bank value date.");
                    return false;
                }
            }
            else if (cdt_BankValueDate.GetDate() != null) {
                flag = ValidationBankValueDate(VoucherNo, cdt_BankValueDate.GetDate().format("yyyy-MM-dd"), Type);
                ValueDate = cdt_BankValueDate.GetDate().format("yyyy-MM-dd");
            }


          




            if (flag) {

                $.ajax({
                    type: "POST",
                    url: "frm_ManualBRSEntityListFetch.aspx/UpdateManualBRSList",
                    data: JSON.stringify({
                        VoucherNo: VoucherNo, Type: Type, ValueDate: ValueDate, InstrumentNo: $("#<%=hddnInstrumentNo.ClientID%>").val()
                        , InstrumentDate: $("#<%=hddnInstrumentDate.ClientID%>").val(), VoucherDate: $("#<%=hddnVoucherdate.ClientID%>").val()
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,//Added By:Subhabrata
                    success: function (msg) {
                        debugger;
                        var status = msg.d;

                        if (status == "1") {
                            jAlert("Saved successfully.");
                            cPopup_Feedback.Hide();
                            TotalTabValue();
                            //cgrdmanualBRS.Refresh();
                            updateGridByDate();
                        }
                        else if (status == "-10") {
                            jAlert("Data not saved.");
                            cPopup_Feedback.Hide();
                        }
                    }
                });

            }
            else {
                jAlert("Bank Value Date must be greater or equal to Voucher Date");
                flag = false;
            }
            return flag;
        }

        function ValidationBankValueDate(VoucherNo, BankValueDate, Type) {
            var flag = false;


            $.ajax({
                type: "POST",
                url: "frm_ManualBRSEntityListFetch.aspx/IsBankValueDateValid",
                data: JSON.stringify({
                    VoucherNo: VoucherNo,
                    BankValueDate: BankValueDate,
                    Type: Type
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,//Added By:Subhabrata
                success: function (msg) {
                    debugger;
                    var status = msg.d;

                    if (status == "1") {
                        flag = true;
                    }
                    else if (status == "0") {
                        flag = false;
                    }

                }
            });

            return flag;
        }

        function CancelFeedback_save() {

            //cdt_BankValueDate.SetText('');
            cPopup_Feedback.Hide();
        }


        //document.onkeydown = function (e) {
        //    if (event.keyCode == 18) isCtrl = true;


        //    if (event.keyCode == 65 && isCtrl == true) { //run code for alt+a -- ie, Add
        //        StopDefaultAction(e);
        //        OnAddButtonClick();
        //    }

        //}

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

        function cxdeToDate_OnChaged(s, e) {
            debugger;
            var data = "OnDateChanged";
            data += '~' + cdt_BankValueDate.GetDate();
        }

        $(document).ready(function () {
            console.log('ready');
            $('.navbar-minimalize').click(function () {
                console.log('clicked');
                TotalTabValue();
                //cgrdmanualBRS.Refresh();

                updateGridByDate();
            });
        });
        
        function gridRowclick(s, e) {
            //alert('hi');
            $('#grdmanualBRS').find('tr').removeClass('rowActive');
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
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }

        function ReconsileAll() {

            var valid = true;


            if (!$("#dtMassrec").hasClass('hide') && cmassrecdt.GetText() == "") {
                jAlert('Please Enter a valid date o proceed.', 'Alert');
                valid = false;
            }

            if (valid)
                cgrdmanualBRS.PerformCallback("SaveMassReconsile");
        }
        function endcallback(s,e) {
            if (s.cpUpdate != null) {
                if (s.cpUpdate == "SucsessUnrec")
                {
                    jAlert('Unreconciled Successfully.', 'Alert', function () {

                        s.cpUpdate = null;
                        //cgrdmanualBRS.Refresh();
                        updateGridByDate();
                    })
                }
                else if (s.cpUpdate == "SucsessRec") {

                    jAlert('Reconciled Successfully.', 'Alert', function () {

                        s.cpUpdate = null;
                        //cgrdmanualBRS.Refresh();
                        updateGridByDate();
                    })
                }

                else if (s.cpUpdate == "ErrorDate") {

                    jAlert('Bank Value Date must be greater or equal to Voucher Date.', 'Alert', function () {

                        s.cpUpdate = null;
                        
                    })
                }

            }
        }

        
    </script>
    <link href="CSS/frm_ManualBRSEntityListFetch.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <dxe:ASPxPopupControl ID="Popup_Feedback" runat="server" ClientInstanceName="cPopup_Feedback"
            Width="400px" HeaderText="Bank Reconcilliation" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                    <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                    <div class="Top clearfix">

                        <table style="width:80%;margin:0 auto">
                           
                            <tr>
                                <td>Bank Value Date<span style="color: red">*</span></td>
                                <td class="relative">
                                  <dxe:ASPxDateEdit ID="dt_BankValueDate" runat="server" EditFormat="Custom" UseMaskBehavior="True" EditFormatString="dd-MM-yyyy" ClientInstanceName="cdt_BankValueDate" Width="100%">
                                       
                                     
                                       <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                       <%--<ClientSideEvents DateChanged="cxdeToDate_OnChaged"></ClientSideEvents>--%>
                                    </dxe:ASPxDateEdit>
                                                                        <span id="MandatoryRemarksFeedback" style="display: none">
                                        <img id="gridHistory_DXPEForm_efnew_DXEFL_DXEditor1234_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                    <label>(DD-MM-YYYY)</label>
                                </td>

                            </tr>
                             
                            <tr>
                                <td></td>
                                <td style="padding-top: 15px;">
                                    <input id="btnFeedbackSave" class="btn btn-primary" onclick="CallFeedback_save()" type="button" value="Save"/>
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
        <div class="panel-title cearfix">
            <h3 class="pull-left"><asp:Label ID="lblHeadertext" runat="server"></asp:Label></h3>
            <div class="inline-block pull-right">
                <div class="inline-block">
                    <label class="stLbl ">Uncleared Balance </label>
                    <div id="divUncleared">
                        0.00
                    </div>
                </div>
                <div class="inline-block">
                    <label class="stLbl ">Cleared Balance </label>
                    <div class="disab" id="divCleared">0.00</div>
                </div>
                  <div class="inline-block">
                    <label class="stLbl ">Bank Balance </label>
                    <div class="disab" id="divTotal">0.00</div>
                </div>
            </div>
        </div>
        <div class="clear"></div>
        <table class="padTab " >
            <tr>
                <td id="lblBankname">Bank</td>
                <td width="220px" id="BankValtd">
                    <dxe:ASPxComboBox ID="cmbBankfilter" runat="server" ClientInstanceName="ccmbBankfilter" Width="100%">
                    </dxe:ASPxComboBox>

                </td>

                <td id="tdDateFromlbl">
                    <label>From Date</label>

                </td>
                <td id="tdDateFromdt" style="width:110px">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" UseMaskBehavior="True" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <buttonstyle width="13px">
                        </buttonstyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td id="lblToDate">
                    <label>To Date</label>
                </td>
                <td id="ToDatedt" style="width:110px">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" UseMaskBehavior="True" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
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
                                <div class="checkbox" style="display:block;">
                                    <label>
                                        <asp:RadioButton ID="RdAll" runat="server" Text="" GroupName="R" onclick="SetDateRange('ALL');" />
                                        All
                                    </label>
                                </div>
                            </td>
                            <td style="padding-left:15px">
                                <input type="button" value="Show" class="btn btn-success btn-radius" onclick="updateGridByDate()" />
                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLSX</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>
                             
                            </td>
                        </tr>
                    </table>
                </td>

            </tr>


        </table>
        <div class="form_main">
        <div class="clearfix">
            <div class="row mBot10">
                <div class="col-md-2" id="dtMassrec">
                    <div>
                        <label>Reconcile Date</label>
                        <dxe:ASPxDateEdit runat="server" ID="massrecdt" ClientInstanceName="cmassrecdt" UseMaskBehavior="True" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy">

                        </dxe:ASPxDateEdit>
                    </div>
                </div>
                <div class="col-md-3 " id="btnMassrec" style="padding-top: 17px;">
                    <div >
                        <button type="button" id="btnmasssrec" class="btn btn-primary" onclick="ReconsileAll();">Reconcile</button>
                    </div>
                </div>
            </div>
        </div>
        
    </div>

    <div class="clear"></div>

    <div class="GridViewArea relative">
       <%-- REV 2.0--%>
       <%-- KeyFieldName="SlNo"--%>
        <%-- REV 2.0 END--%> 
        <dxe:ASPxGridView ID="grdmanualBRS" runat="server" AutoGenerateColumns="False" KeyFieldName="SLNO"
            Width="100%" ClientInstanceName="cgrdmanualBRS" OnCustomCallback="grdmanualBRS_CustomCallback" 
            DataSourceID="EntityServerModeDataSource"  SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" 
             HorizontalScrollBarMode="Auto" Settings-VerticalScrollBarMode="auto"
                    Settings-VerticalScrollableHeight="300" >
            <SettingsSearchPanel Visible="True" Delay="5000" />
            <columns>
                    <dxe:GridViewCommandColumn VisibleIndex="0" ShowClearFilterButton="True" width="0">
                      
                    </dxe:GridViewCommandColumn>
                      
                    <dxe:GridViewDataTextColumn Caption="Posting Date" VisibleIndex="1" FieldName="CASHBANK_TRANSACTIONDATE" Settings-ShowFilterRowMenu="True" Width="100px" FixedStyle="Left">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Document No." VisibleIndex="2" FieldName="CASHBANK_VOUCHERNUMBER" Settings-ShowFilterRowMenu="True" Width="140px" FixedStyle="Left">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Type" VisibleIndex="3" FieldName="TYPE" Settings-ShowFilterRowMenu="True" Width="140px" FixedStyle="Left">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Instrument Date" VisibleIndex="4" FieldName="CASHBANKDETAIL_INSTRUMENTDATE" Settings-ShowFilterRowMenu="True" Width="120px">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Instrument No." VisibleIndex="5" FieldName="CASHBANKDETAIL_INSTRUMENTNUMBER" Width="170px">
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                     <dxe:GridViewDataTextColumn Caption="Instrument Type" VisibleIndex="6" FieldName="TYPE1" Settings-ShowFilterRowMenu="True" Width="100px">
                         <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <%--<dxe:GridViewDataTextColumn Caption="Account Name" VisibleIndex="7" FieldName="AccountCode">
                    </dxe:GridViewDataTextColumn>--%>
                    <dxe:GridViewDataTextColumn Caption="Payment Amount" VisibleIndex="8" FieldName="CASHBANKDETAIL_PAYMENTAMOUNT" HeaderStyle-HorizontalAlign="Right">
                            <PropertiesTextEdit DisplayFormatString="{0:0.00}" Width="100%">
                                <MaskSettings Mask="<0..999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                                </PropertiesTextEdit>
                        <CellStyle HorizontalAlign="Right"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn Caption="Receipt Amount" VisibleIndex="9" FieldName="CASHBANKDETAIL_RECEIPTAMOUNT" HeaderStyle-HorizontalAlign="Right">
                            <PropertiesTextEdit DisplayFormatString="{0:0.00}" Width="100%">
                                                    <MaskSettings Mask="<0..999999999>.<0..99>" IncludeLiterals="DecimalSymbol" />
                                </PropertiesTextEdit>
                        <CellStyle HorizontalAlign="Right"></CellStyle>
                        <Settings AllowAutoFilterTextInputTimer="False" />
                    </dxe:GridViewDataTextColumn>
                  <dxe:GridViewDataTextColumn Caption="Payee/Payer" VisibleIndex="10" width="250px" FieldName="PAIDTO">
                      <Settings AllowAutoFilterTextInputTimer="False" />
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
                    <dxe:GridViewDataDateColumn FieldName="CASHBANKDETAIL_BANKVALUEDATE" Width="160px" VisibleIndex="11"
                        Caption="Recon. Date" >
                        <Settings AllowAutoFilterTextInputTimer="False" />
                        <Settings AllowAutoFilter="False" AllowGroup="False"  />
                                    
                        <%--<DataItemTemplate>
                            <dxe:ASPxDateEdit ID="bankvaluedate" runat="server" Width="100%" EditFormat="custom" UseMaskBehavior="True" ClientInstanceName="cbankvaluedate" AllowNull="true" DisplayFormatString="dd-MM-yyyy" EditFormatString="dd-MM-yyyy"  Date="<%#Setbankvaluedate(Container)%>">
                                <ButtonStyle Width="13px">
                                </ButtonStyle>
                                           
                            </dxe:ASPxDateEdit>
                        </DataItemTemplate>--%>
                        <%--<PropertiesDateEdit>

                        </PropertiesDateEdit>--%>
                    </dxe:GridViewDataDateColumn> 

                <%--<dxe:GridViewDataTextColumn Caption="IsCancel" FieldName="IsCancel"
                    VisibleIndex="12" FixedStyle="Left" Width="0" HeaderStyle-CssClass="hide" FilterCellStyle-CssClass="hide">
                    <CellStyle CssClass="hide" Wrap="true">
                    </CellStyle>
                </dxe:GridViewDataTextColumn>--%>
                <dxe:GridViewCommandColumn SelectAllCheckboxMode="AllPages" VisibleIndex="13" ShowSelectCheckbox="true"></dxe:GridViewCommandColumn>
                <dxe:GridViewDataTextColumn HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="center" VisibleIndex="13" Width="0">
                    <DataItemTemplate>
                        <div class='floatedBtnArea'>
                         <% if (rights.CanEdit)
                            { %>
                         <a href="javascript:void(0);" onclick="OnCancelClick('<%#Eval("CASHBANK_VOUCHERNUMBER")%>',<%# Container.VisibleIndex %>)" class="" title="">
                            
                           
                            <span class='ico ColorFour'><i class='fa fa-money'></i></span><span class='hidden-xs'>BRS</span>
                              <% } %>

                        </a>
                        </div>
                    </DataItemTemplate>
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    <CellStyle HorizontalAlign="Center"></CellStyle>
                    <HeaderTemplate><span>Actions</span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                </dxe:GridViewDataTextColumn>
                    
                
            </columns>
               <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <%--<clientsideevents endcallback="function (s, e) {grid_EndCallBack();}" />--%>

            <ClientSideEvents EndCallback="endcallback" />
            <settingspager pagesize="10">
                           <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200"/>
                             </settingspager>
            <%--<settingssearchpanel visible="True" />--%>
            <settings showgrouppanel="True" ShowFooter="true"  ShowGroupFooter="VisibleIfExpanded"  showstatusbar="Hidden" showhorizontalscrollbar="true" showfilterrow="true" showfilterrowmenu="false" />
            <settingsloadingpanel text="Please Wait..." />
            <totalsummary>
                                       <dxe:ASPxSummaryItem FieldName="Amount" SummaryType="Sum" /> 
                                 </totalsummary>
        </dxe:ASPxGridView>

        <%--<asp:Button ID="btnUpdate" runat="server" Text="Save" Visible="false" CssClass="btnUpdate btn btn-primary" OnClick="btnUpdate_Click" Width="" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Visible="false" CssClass="btnUpdate btn btn-danger" OnClick="btnCancel_Click" Width="" />--%>

        <asp:HiddenField ID="hiddenedit" runat="server" />
        <asp:HiddenField ID="hddnTypeIdd" runat="server" />
        <asp:HiddenField ID="hddnBRSConfigSettings" runat="server" />
        <asp:HiddenField ID="hddnVoucherNumber" runat="server" />
        <asp:HiddenField ID="hddnModuleType" runat="server" />
         <asp:HiddenField ID="hddnBankValueDate" runat="server" />
         <asp:HiddenField ID="hddnInstrumentNo" runat="server" />
         <asp:HiddenField ID="hddnInstrumentDate" runat="server" />
          <asp:HiddenField ID="hddnVoucherdate" runat="server" />

        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_FetchManualBRS_ListC" />
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

      <div>
        <asp:HiddenField ID="hfIsFilter" runat="server" />
        <asp:HiddenField ID="hfFromDate" runat="server" />
        <asp:HiddenField ID="hfToDate" runat="server" />
        <asp:HiddenField ID="hfBranchID" runat="server" />
    </div>
    </div>

     <%--  REV 1.0--%>
    <dxe:ASPxCallbackPanel runat="server" ID="CallbackPanel" ClientInstanceName="cCallbackPanel" OnCallback="CallbackPanel_Callback">
        <PanelCollection>
            <dxe:PanelContent runat="server">
            </dxe:PanelContent>
        </PanelCollection>
        <ClientSideEvents EndCallback="CallbackPanelEndCall" />
    </dxe:ASPxCallbackPanel>
    <asp:HiddenField ID="hFilterType" runat="server" />
    <%--END REV 1.0--%>
</asp:Content>


