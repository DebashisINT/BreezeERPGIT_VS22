<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                23-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="ProductCostMarketPrice" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ProductCostMarketPrice.aspx.cs" Inherits="ERP.OMS.Management.Master.ProductCostMarketPrice" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-confirm/3.1.0/jquery-confirm.min.css"/>

    <link href="../../../assests/css/custom/jquery.confirm.css" rel="stylesheet" />
      <style>
        #ASPXPopupControl_PW-1 iframe .crossBtn {
            display:none;
        }
          .btn.typeNotificationBtn {
                position: relative;
            padding-right: 16px !important;
        }
        .typeNotification {
                 position: absolute;
                width: 22px;
                height: 22px;
                background: #ff5140;
                display: block;
                font-size: 12px;
                border-radius: 50%;
                right: -7px;
                top: -9px;
                line-height: 22px;
        }
    </style>
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
           /* #GrdOrder_DXMainTable>tbody>tr {
                position:relative !important;

            }
            #GrdOrder_DXMainTable>tbody>tr:hover .floatedIcons {
                visibility:visible;
            }
            .floatedIcons {
                    position: absolute;
                    left: 20px;
                    transform: translateY(-7px);
                    background: #fff;
                    padding: 0 15px;
                    visibility: hidden;
            }
            .floatedIcons>a {
                margin-right:12px;
                z-index:5;
                -moz-transition:box-shadow 0.2s ease;
                -webkit-transition:box-shadow 0.2s ease;
                transition:box-shadow  0.2s ease, transform 0.3s ease;
                position:relative;
            }
            .floatedIcons>a:before {
                    content: '';
                    width: 35px;
                    height: 35px;
                    position: absolute;
                    border-radius: 50%;
                    background: rgba(247, 170, 170, 0.5);
                    left: -10px;
                    top: -9px;
                    z-index: -1;
                    opacity:0;
                    transform:scale(0);
                    -webkit-transition:all 0.1s ease;
                    -moz-transition:all 0.1s ease;
                    transition:all 0.1s ease;
            }
             .floatedIcons>a:hover:before {
                 transform:scale(1);
                 opacity:1;
             }
            .floatedIcons>a:hover {
                box-shadow:0px 0px 0px 10px rgba(0,0,0,0.4);
                border-radius:30px;
                z-index:7;
                transform:scale(1.3);
                
            }*/
           .closeApprove{

            float: right;
            margin-right: 7px;
           }

           /*Rev 1.0*/

        select
        {
            height: 30px !important;
            border-radius: 4px;
            /*-webkit-appearance: none;
            position: relative;
            z-index: 1;*/
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto , #FormDate , #toDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #txtcstVdate_B-1 ,
        #txtLocalVdate_B-1 , #txtCINVdate_B-1 , #txtincorporateDate_B-1 , #txtErpValidFrom_B-1 , #txtErpValidUpto_B-1 , #txtESICValidFrom_B-1 ,
        #txtESICValidUpto_B-1 , #FormDate_B-1 , #toDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img ,
        #txtcstVdate_B-1 #txtcstVdate_B-1Img ,
        #txtLocalVdate_B-1 #txtLocalVdate_B-1Img , #txtCINVdate_B-1 #txtCINVdate_B-1Img , #txtincorporateDate_B-1 #txtincorporateDate_B-1Img ,
        #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img , #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img , #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img ,
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img , #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #GrdHolidays , #cityGrid
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 7px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-24
        {
            margin-top: 24px;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        .dxeBase_PlasticBlue .dxichTextCellSys label
        {
            color: #fff !important;
        }

        #actv-warh label
        {
            color: #111 !important;
        }

        .btn.btn-xs
        {
                font-size: 14px !important;
        }

        /*#ShowFilter
        {
            padding-bottom: 3px !important;
        }*/

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 0.4 !important;
            color: #ffffff !important;
        }*/
                /*.padTopbutton {
            padding-top: 27px;
        }*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>

    <script>
        function openviewlog()
        {
            hdnyearlog.value = "";
            cgridlog.Refresh();
            cLogPopUp.Show();
        }
        function ddlyearinlog_SelectedIndexChanged()
        {
            var year = $("#hdnyearlog").val();
            hdnyearlog.value = year;
        }
        function ChangeState(value) {
            
            cgridrateupdate.PerformCallback('SelectAndDeSelectProducts' + '~' + value);
        }
        function Save_ButtonClick() {
            $("#hdnflag").val("1");
            grid.AddNewRow();
            grid.UpdateEdit();
        }
        function cancel_ButtonClick() {
            grid.Refresh();
        }
        function getbacktolisting() {
            debugger;
            jAlert("Please Select a Product", 'Alert Dialog: [Products]', function (r) {
                if (r == true) {
                    ChangeState('UnSelectAll')
                    cProductsPopup.Show();                   
                }
            });
        }
        function showexportpopup() {
            debugger;
            jAlert("Please Select a Year", 'Alert Dialog: [Year]', function (r) {
                if (r == true) {
                    ChangeState('UnSelectAll')
                    cProductsPopup.Show();
                }
            });
        }
        function hideimportpopup()
        {
            jAlert("Rate Imported Succesfully!", 'Alert Dialog: [Rate Import]', function (r) {
                if (r == true) {
                    popup_excelupload.Hide();
                }
            });
            
        }
        function hideimportfailurepopup() {
            jAlert("Only Numeric Data Allowed In Rate Field!", 'Alert Dialog: [Rate Import]', function (r) {
                if (r == true) {
                    popup_excelupload.Show();
                }
            });

        }
        function OnEndCallback(s,e){}
        function btndownload_clientclick()
        {
            cProductsPopup.Hide();
        }
            function grid_EndCallBack(s, e) { }
            function cgridrateupdateendcallback(s, e)
            {
          
            }
            function fn_import()
            {
                grid.Refresh();
                popup_excelupload.Show();
            }
            function fn_PopOpen() {
                Popup_exceldownload.Show();
            }
            $(document).ready(function () {
                ChangeState('UnSelectAll')
                cProductsPopup.Hide();
                hdnyear.value = "";
                hdnyearlog.value = "";
                var min = 2010,
                max = 2030,
                select = document.getElementById('ddlyear');

                for (var i = min; i <= max; i++) {
                    var opt = document.createElement('option');
                    opt.value = i;
                    opt.innerHTML = i;
                    select.appendChild(opt);
                }
                selectyear = document.getElementById('ddlimportyear');
                for (var i = min; i <= max; i++) {
                    var opt = document.createElement('option');
                    opt.value = i;
                    opt.innerHTML = i;
                    selectyear.appendChild(opt);
                }

                selectyear = document.getElementById('ddlimportyear');
                for (var i = min; i <= max; i++) {
                    var opt = document.createElement('option');
                    opt.value = i;
                    opt.innerHTML = i;
                    selectyear.appendChild(opt);
                }

                selectyear = document.getElementById('ddlyearinpopup');
                for (var i = min; i <= max; i++) {
                    var opt = document.createElement('option');
                    opt.value = i;
                    opt.innerHTML = i;
                    selectyear.appendChild(opt);
                }
                selectyear = document.getElementById('ddlyearinlog');
                for (var i = min; i <= max; i++) {
                    var opt = document.createElement('option');
                    opt.value = i;
                    opt.innerHTML = i;
                    selectyear.appendChild(opt);
                }
            }); 
            function ddlyearinpopup_SelectedIndexChanged()
            {
                //debugger;
                //var year=$("#ddlyearinpopup").val();
                //$.ajax({
                //    type: "POST",
                //    url: "ProductCostMarketPrice.aspx/AutoPopulateAltQuantity",
                //    data: JSON.stringify({ Year: year }),
                //    contentType: "application/json; charset=utf-8",
                //    dataType: "json",
                //    async: false,
                //    success: function (msg) {
                //        OutStandingAmount = msg.d;
                //        if (OutStandingAmount.CreditDays != "") {
                //            var cday = OutStandingAmount.CreditDays;
                //            ctxtCreditDays.SetValue(cday);
                //            cdt_SaleInvoiceDue.SetEnabled(false);
                //            var CreditDays = ctxtCreditDays.GetValue();
                //            var today = cPLSalesOrderDate.GetDate();
                //            var newdate = cPLSalesOrderDate.GetDate();
                //            newdate.setDate(today.getDate() + Math.round(CreditDays));
                //            cdt_SaleInvoiceDue.SetDate(newdate);
                //            cdt_SaleInvoiceDue.SetEnabled(false);
                //        }
                //    }
                //});
                var year = $("#ddlyearinpopup").val();
                hdnyear.value = year;
            }
            function ddlyearinpopup_SelectedIndexChanged() {
               
                var year = $("#ddlyearinpopup").val();
                hdnyear.value = year;
            }
            function ddlyearinomportpopup_SelectedIndexChanged() {
                var year = $("#ddlimportyear").val();
                hdnimportyear.value = year;
            }
            function btnsave_clientclick() {
                debugger;
                var year = $("#ddlyearinpopup").val();
                if (year == "--Select--")
                {
                    jAlert("Please Select a year");
                    return;
                }
            }
            function gridFocusedRowChanged(s, e) {

                globalRowIndex = e.visibleIndex;
            }
            function btnproducts_clientclick()
            {
                $("#ddlyearinpopup").val("--Select--");
                ChangeState('UnSelectAll');
                cProductsPopup.Show();
                
                //cgridproducts.PerformCallback('BindProductsDetails' + '~' + '@');
          
            }
            function GetVisibleIndex(s, e) {
                globalRowIndex = e.visibleIndex;
            }
            function updateGridByDate() {
                year = $("#ddlyear").val();
                hdngridyear.value = year;
                if (year != null && year!="") {
          
                    //localStorage.setItem("FromDateSalesOrder", cFormDate.GetDate().format('yyyy-MM-dd'));
                    //localStorage.setItem("ToDateSalesOrder", ctoDate.GetDate().format('yyyy-MM-dd'));
                    //localStorage.setItem("OrderBranch", ccmbBranchfilter.GetValue());
                    //$("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                    //$("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                    //$("#hfBranchID").val(ccmbBranchfilter.GetValue());
                    //$("#hfIsFilter").val("Y");
                    grid.Refresh();
                    //cGrdPayReqGrdPayReqdetails.Refresh();


                }
            }
            function updatelogGridByDate() {
              
                year = $("#ddlyearinlog").val();
                hdnyearlog.value = year;
                if (year != null && year != "") {

                    //localStorage.setItem("FromDateSalesOrder", cFormDate.GetDate().format('yyyy-MM-dd'));
                    //localStorage.setItem("ToDateSalesOrder", ctoDate.GetDate().format('yyyy-MM-dd'));
                    //localStorage.setItem("OrderBranch", ccmbBranchfilter.GetValue());
                    //$("#hfFromDate").val(cFormDate.GetDate().format('yyyy-MM-dd'));
                    //$("#hfToDate").val(ctoDate.GetDate().format('yyyy-MM-dd'));
                    //$("#hfBranchID").val(ccmbBranchfilter.GetValue());
                    //$("#hfIsFilter").val("Y");
                    cgridlog.Refresh();
                    //cGrdPayReqGrdPayReqdetails.Refresh();


                }
            }
            function OnCustomButtonClick(s, e) {
                debugger;
                if (e.buttonID == 'AddNew') {


                    // if (clookup_Project.GetValue() == null) {
                    //var Particulars = (grid.GetEditor('Particulars').GetText() != null) ? grid.GetEditor('Particulars').GetText() : "";
                    //if (Particulars != "") {
                        OnAddNewClick();
                    //}
                    //else {
                        OnAddNewClick();
                      //  grid.batchEditApi.StartEdit(e.visibleIndex, 2);
                    //}
                    //}
                    //else {
                    //     OnAddNewClick();
                    //  }

                }
            }
           
</script>
    <script type="text/javascript">
        $(document).ready(function () {
            setTimeout(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 200;
                    grid.SetWidth(cntWidth);
                }
            }, 1000)
            
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 200;
                    grid.SetWidth(cntWidth);
                } else {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 90;
                    grid.SetWidth(cntWidth);
                }

            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading clearfix">
        <div class="panel-title pull-left">
            <h3>Product Cost/Market Price</h3>
        </div>
          <div id="divcross" class="crossBtn" visible="false"><a href="../ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
    </div>
        <div class="form_main">
        <div class="clearfix">
             <table class="padTab pull-left" style="margin-top: 7px">
            <tr>
                <td>

                    <label>Select Year </label>
                </td>
                <td style="width:160px">                  
                    <select id="ddlyear"  runat="server" style="width:100%">
                        <option value="--Select--">--Select--</option>
                    </select>
                </td>
                <td>
                    <input type="button" value="Show" class="btn btn-success btn-radius"  onclick="updateGridByDate()"/> <%--onclick="updateGridByDate()"--%>
               
                   <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                
                    <input type="button" value="Download Format" onclick="btnproducts_clientclick()" class="btn btn-info btn-radius" />
                
                    <input type="button" value="Import(Add/Update)" onclick="fn_import()" class="btn btn-success btn-radius"  />
                
                    <input type="button" value="View Log" class="btn btn-warning  typeNotificationBtn btn-radius"  onclick="openviewlog()" />
                </td>
            </tr>

        </table>
            </div>
          </div>
    
        <div class="PopUpArea">
                <dxe:ASPxPopupControl ID="Popup_exceldownload" runat="server" ClientInstanceName="Popup_exceldownload"
                    Width="400px" HeaderText="Format Download" PopupHorizontalAlign="WindowCenter"
                    Height="130px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Modal="True">
                    <ContentCollection>
                        <dxe:PopupControlContentControl ID="SRPopupControlContentControl" runat="server">                   
                                   <div style="padding-top: 5px;" class="col-md-10 col-md-offset-1" id="divstateunique">
                                  <%--  <div class="stateDiv" style="padding-top: 5px">
                          
                                         <label>Select Year </label><span style="color: red;" id="spanstateunique">*</span><a href="#"  style="left: -12px;  "> 
                               
                                    </div>
                                    <div style="padding-top: 5px; width: 100%">
                                        <select id="ddlyearinpopup"  runat="server" style="width:100%" onchange="ddlyearinpopup_SelectedIndexChanged()"> 
                                                                    <option value="--Select--" selected></option>
                                        </select>
                                      
                                    </div>--%>
                                        <div style="padding-top: 5px; width: 100%">
                                          <asp:Button ID="btnproducts" OnClientClick="btnproducts_clientclick()"  CssClass="btn btn-primary btn-radius" runat="server" Text="Select Products" />
                                       
                                      
                                    </div>
                                       <br />
                                       <div>
                                       <%--    <asp:Button ID="btndownloadexcel"  OnClick="btndownload_click" CssClass="btn btn-primary btn-radius" runat="server" Text="Download" />--%>
                                           <%-- OnClientClick="btnsave_clientclick()" --%>
                                       </div>
                                </div>

                             <br style="clear: both;" />
                       
                            
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>



           <dxe:ASPxPopupControl ID="popup_excelupload" runat="server" ClientInstanceName="popup_excelupload"
                    Width="400px" HeaderText="Import Rate" PopupHorizontalAlign="WindowCenter"
                    Height="130px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton" Modal="True">
                    <ContentCollection>
                        <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">                   
                                   <div style="padding-top: 5px;" class="col-md-12 " id="divstateunique1">
                                    <%--<div class="stateDiv" style="padding-top: 5px">
                                         <label>Select Year </label><span style="color: red;" id="spanstateunique1">*</span><a href="#"  style="left: -12px;  "> 
                                                
                                    </div>
                                    <div style="padding-top: 5px; width: 100%">
                                        <select id="ddlyearimport"  runat="server" style="width:100%" onchange="ddlyearinpopupupload_SelectedIndexChanged()"> 
                                         <option value="--Select--" selected></option>
                                        </select>
                                      
                                    </div>--%>
                                         <div class="stateDiv" style="padding-top: 5px;">
                          
                                         <label>Select Year </label><span style="color: red;" id="spanimportyear">*</span><a href="#"  style="left: -12px;  "> 
                               
                                    </div>
                                    <div style="padding-top: 5px; width: 70%">
                                        <select id="ddlimportyear"  runat="server" style="width:100%" onchange="ddlyearinomportpopup_SelectedIndexChanged()"> 
                                                                    <option value="--Select--" selected></option>
                                        </select>
                                      
                                    </div>
                                       <br />
                                       <div>
                                      <asp:FileUpload ID="FileImport" CssClass="custom-file-input" runat="server" />  
                                       </div>
                                       <br />
                                       <div>
                                           <asp:Button ID="Button1"  OnClick="btnImport_click" CssClass="btn btn-success" runat="server" Text="Import" />
                                           <%-- OnClientClick="btnsave_clientclick()" --%>
                                       </div>
                                </div>

                             <br style="clear: both;" />
                       
                            
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
        
        
        
        <%-- Product Pop Up--%>
           <dxe:ASPxPopupControl ID="ASPxProductsPopup" runat="server" ClientInstanceName="cProductsPopup"
        Width="800px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <strong><span style="color: #fff">Select Products</span></strong>
            <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ ChangeState('UnSelectAll');
                                                            cProductsPopup.Hide();
                                                        }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                
             <%--       <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                    <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                    <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>--%>
                
            <div class="clearfix relative">
                <div class="row">
                    <div class="col-md-3 mb-10">
                        <div class="stateDiv" style="padding-top: 5px;">
                          
                                <label>Select Year </label><span style="color: red;" id="spanstateunique">*</span>
                               
                        </div>
                        <div style="padding-top: 5px; width: 100%;">
                            <select id="ddlyearinpopup"  runat="server" style="width:100%" onchange="ddlyearinpopup_SelectedIndexChanged()"> 
                                                        <option value="--Select--" selected></option>
                            </select>
                                      
                        </div>
                    </div>
                     <div class="col-md-6" style="padding-top: 30px;">
                        <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-success"></input>
                        <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-danger"></input>
                        <%--<input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>--%>
                    </div>
                </div>
                  
                
                <div class="clear"></div>
            <dxe:ASPxGridView ID="gridrateupdate" runat="server" KeyFieldName="sProducts_ID" AutoGenerateColumns="False"
                        DataSourceID="EntityServerModeDataSource" Width="100%" ClientInstanceName="cgridrateupdate" OnCustomCallback="gridrateupdate_CustomCallback"

                        SettingsDataSecurity-AllowDelete="false" Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto" >
                        <ClientSideEvents EndCallback="function(s,e) { cgridrateupdateendcallback }" />
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                     
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="700px" EditFormColumnCount="3" />

                        <Settings ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                        <SettingsText PopupEditFormCaption="Add/ Modify Employee" />
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="40" Caption=" " VisibleIndex="0" />
                            <dxe:GridViewDataTextColumn FieldName="SrlNo" ReadOnly="True" VisibleIndex="1" Width="0">
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditCellStyle Wrap="True">
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="sProducts_ID" Caption="Product Id" VisibleIndex="0" Width="0">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Product Code" FieldName="sProducts_Code" VisibleIndex="2" Width="200">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Product Name" FieldName="sProducts_Name" VisibleIndex="3" Width="200">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Description" FieldName="sProducts_Description" VisibleIndex="4" Width="400">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle HorizontalAlign="left">
                                </EditFormCaptionStyle>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>
                           

                        </Columns>
                 
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                       <%-- <ClientSideEvents RowClick="gridRowclick" />--%>
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>
                    </dxe:ASPxGridView>
        </div>

        <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="v_Product"/>

        <asp:SqlDataSource ID="EmployeeDataSource" runat="server" SelectCommand=""></asp:SqlDataSource>
                <br />
                <div class="text-center">
                 <%--   <asp:Button ID="Button3" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" UseSubmitBehavior="false" />--%>
                        <asp:Button ID="btndownloadexcel"  OnClick="btndownload_click" OnClientClick="btndownload_clientclick()"  CssClass="btn btn-primary btn-radius" runat="server" Text="Download" />
                </div>
                <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="EmployeeGrid" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
            </dxe:PopupControlContentControl>

        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
        <%-- End Product Popup --%>

        
        <%--Log table--%>
             <dxe:ASPxPopupControl ID="Popup" runat="server" ClientInstanceName="cLogPopUp"
        Width="800px" HeaderText="Select Tax" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <HeaderTemplate>
            <strong><span style="color: #fff">Rate Import Log</span></strong>
            <dxe:ASPxImage ID="ASPxImage3" runat="server" ImageUrl="/assests/images/closePop.png" Cursor="pointer" CssClass="popUpHeader pull-right">
                <ClientSideEvents Click="function(s, e){ 
                                                            cLogPopUp.Hide();
                                                        }" />
            </dxe:ASPxImage>
        </HeaderTemplate>
        <ContentCollection>
            <dxe:PopupControlContentControl runat="server">
                
             <%--       <input type="button" value="Select All Products" onclick="ChangeState('SelectAll')" class="btn btn-primary"></input>
                    <input type="button" value="De-select All Products" onclick="ChangeState('UnSelectAll')" class="btn btn-primary"></input>
                    <input type="button" value="Revert" onclick="ChangeState('Revart')" class="btn btn-primary"></input>--%>
                
            <div class="clearfix relative">
                <div class="row">
                    <div class="col-md-3 mb-10">
                        <div class="stateDiv" style="padding-top: 5px;">
                           <label>Select Year </label><span style="color: red;" id="spanstateunique">*</span><a href="#"  style="left: -12px;  ">       
                        </div>
                        <div style="padding-top: 5px; width: 100%;">
                            <select id="ddlyearinlog"  runat="server" style="width:100%" onchange="ddlyearinlog_SelectedIndexChanged()"> 
                                 <option value="--Select--" selected></option>
                            </select>            
                        </div>
                    </div>
                    <div class="col-md-3" style="padding-top: 30px;">
                        <input type="button" value="Show" class="btn btn-primary"  onclick="updatelogGridByDate()"/>
                    </div>
                </div>
                <dxe:ASPxGridView ID="gridlog" runat="server" KeyFieldName="MarketcostLog_id" AutoGenerateColumns="False"
                        DataSourceID="EntityServerlogModeDataSource" Width="100%" ClientInstanceName="cgridlog" OnCustomCallback="gridlogCustomCallback"

                        SettingsDataSecurity-AllowDelete="false" Settings-HorizontalScrollBarMode="Auto" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto" >
                        <ClientSideEvents EndCallback="function(s,e) { clogendcallback }" />
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                     
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="700px" EditFormColumnCount="3" />

                        <Settings ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" ColumnResizeMode="NextColumn" />
                        <SettingsText PopupEditFormCaption="Import Log" />
                        <Columns>
      <%--                  <dxe:GridViewDataTextColumn Caption="SrlNo"  FieldName="SrlNo"   VisibleIndex="1" Width="0">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>--%>
                                                   <dxe:GridViewDataTextColumn ReadOnly="true" CellStyle-Wrap="False" PropertiesTextEdit-ValidationSettings-ErrorFrameStyle-Wrap="False" EditFormCaptionStyle-Wrap="False" Caption="Product Code"  FieldName="ProductCode"  VisibleIndex="1" Width="150">
                                                        <PropertiesTextEdit>
                                                             <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                   
                                                               </PropertiesTextEdit>
                                                        <CellStyle Wrap="False"></CellStyle>  
                                                    </dxe:GridViewDataTextColumn>
                                       <dxe:GridViewDataTextColumn ReadOnly="true" CellStyle-Wrap="False" PropertiesTextEdit-ValidationSettings-ErrorFrameStyle-Wrap="False" EditFormCaptionStyle-Wrap="False" Caption="Product Description"  FieldName="ProductCode"  VisibleIndex="2" Width="150">
                                                        <PropertiesTextEdit>
                                                             <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                   
                                                               </PropertiesTextEdit>
                                                        <CellStyle Wrap="False"></CellStyle>  
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn Caption="January" CellStyle-HorizontalAlign="Right" FieldName="January"   VisibleIndex="3" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn Caption="February" CellStyle-HorizontalAlign="Right" FieldName="February"   VisibleIndex="4" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                              <dxe:GridViewDataTextColumn Caption="March" CellStyle-HorizontalAlign="Right" FieldName="March"   VisibleIndex="5" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                           <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>

                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn Caption="April" CellStyle-HorizontalAlign="Right" FieldName="April"   VisibleIndex="6" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn Caption="May" CellStyle-HorizontalAlign="Right" FieldName="May"   VisibleIndex="7" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn Caption="June" CellStyle-HorizontalAlign="Right" FieldName="June"   VisibleIndex="8" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                           <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn Caption="July" CellStyle-HorizontalAlign="Right" FieldName="July"   VisibleIndex="9" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                               <dxe:GridViewDataTextColumn Caption="August" CellStyle-HorizontalAlign="Right" FieldName="August"   VisibleIndex="10" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                         <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                               <dxe:GridViewDataTextColumn Caption="September" CellStyle-HorizontalAlign="Right" FieldName="September"   VisibleIndex="11" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                      <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                               <dxe:GridViewDataTextColumn Caption="October" CellStyle-HorizontalAlign="Right" FieldName="October"   VisibleIndex="12" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                         <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                               <dxe:GridViewDataTextColumn Caption="November" CellStyle-HorizontalAlign="Right" FieldName="November"   VisibleIndex="13" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                       <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                            <dxe:GridViewDataTextColumn Caption="Created By"  FieldName="CreatedBy"
                                             VisibleIndex="13" Width="100px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                                             <CellStyle CssClass="gridcellleft" Wrap="true">
                                             </CellStyle>
                                             <Settings AllowAutoFilterTextInputTimer="False" />
                                             <Settings AutoFilterCondition="Contains" />
                                               </dxe:GridViewDataTextColumn>
                                         <dxe:GridViewDataTextColumn Caption="Created On" FieldName="CreatedOn" SortOrder="Descending"
                                           VisibleIndex="14" Width="150px">
                                           <CellStyle CssClass="gridcellleft" Wrap="true">
                                           </CellStyle>
                                            <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                           <Settings AllowAutoFilterTextInputTimer="False" />
                                           <Settings AutoFilterCondition="Contains" />
                                       </dxe:GridViewDataTextColumn>
                                                              <dxe:GridViewDataTextColumn Caption="Modified By" FieldName="ModifiedBy"
                                     VisibleIndex="15" Width="100px" Settings-ShowFilterRowMenu="True" Settings-AllowAutoFilter="True">
                                     <CellStyle CssClass="gridcellleft" Wrap="true">
                                     </CellStyle>
                                     <Settings AllowAutoFilterTextInputTimer="False" />
                                     <Settings AutoFilterCondition="Contains" />
                                       </dxe:GridViewDataTextColumn>
                                         <dxe:GridViewDataTextColumn Caption="Modified On" FieldName="ModifiedOn" SortOrder="Descending"
                                       VisibleIndex="16" Width="150px">
                                       <CellStyle CssClass="gridcellleft" Wrap="true">
                                       </CellStyle>
                                        <PropertiesTextEdit DisplayFormatString="dd-MM-yyyy"></PropertiesTextEdit>
                                       <Settings AllowAutoFilterTextInputTimer="False" />
                                       <Settings AutoFilterCondition="Contains" />
                                         </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Action" CellStyle-HorizontalAlign="Right" FieldName="Action"   VisibleIndex="13" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                       <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                        </Columns>
                 
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsPager PageSize="10">
                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
                        </SettingsPager>
                       <%-- <ClientSideEvents RowClick="gridRowclick" />--%>
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>
                    </dxe:ASPxGridView>
        </div>

        <dx:LinqServerModeDataSource ID="EntityServerlogModeDataSource" runat="server" OnSelecting="EntityServerModelogDataSource_Selecting"
            ContextTypeName="ERPDataClassesDataContext" TableName="FillProductCostMarketLog"/>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" SelectCommand=""></asp:SqlDataSource>
                <br />
                <div class="text-center">
                 <%--   <asp:Button ID="Button3" runat="server" Text="OK" CssClass="btn btn-primary  mLeft mTop" UseSubmitBehavior="false" />--%>
                        <%--<asp:Button ID="Button2"  OnClick="btndownload_click" OnClientClick="btndownload_clientclick()"  CssClass="btn btn-primary btn-radius" runat="server" Text="Download" />--%>
                </div>
                <div style="display: none">
        <dxe:ASPxGridViewExporter ID="ASPxGridViewExporter1" GridViewID="gridlog" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>
            </dxe:PopupControlContentControl>

        </ContentCollection>
        <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
        <%-- End Product Popup --%>
                            </div>
        <div>
        <asp:HiddenField ID="hdngridyear" runat="server" />
        <asp:HiddenField ID="hdnyear" runat="server" />
        <asp:HiddenField ID="hdnyearlog" runat="server" />
    </div>
    

    <%--<div>--%>

         <div class="gridwraper" style="margin-top:20px;height:auto;">
               <dxe:ASPxGridView runat="server" 
                        OnCustomUnboundColumnData="grid_CustomUnboundColumnData" ClientInstanceName="grid" KeyFieldName="Marketcost_id" ID="grid"
                        Width="100%" SettingsBehavior-AllowSort="false" SettingsBehavior-AllowDragDrop="false" Settings-ShowFooter="false"
                        OnBatchUpdate="grid_BatchUpdate"
                        OnCustomCallback="grid_CustomCallback"
                        OnDataBinding="grid_DataBinding"
                        OnCellEditorInitialize="grid_CellEditorInitialize"
                        OnRowInserting="Grid_RowInserting"
                        OnRowUpdating="Grid_RowUpdating"
                        OnRowDeleting="Grid_RowDeleting"
                        OnHtmlRowCreated="grid_HtmlRowCreated"
                        OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
                        Settings-VerticalScrollBarMode="Visible" Settings-VerticalScrollableHeight="350" SettingsPager-Mode="ShowAllRecords" Settings-HorizontalScrollBarMode="Auto">
                                                <SettingsPager Visible="false"></SettingsPager>
                                                <Settings VerticalScrollBarMode="Auto" />
                                                <SettingsBehavior AllowDragDrop="False" AllowSort="False" />
                    <Settings ShowGroupPanel="false" ShowStatusBar="Hidden" ShowFilterRow="false" />
                   <SettingsSearchPanel Visible="True" Delay="5000" />
                                                <Columns>
                                                    <dxe:GridViewCommandColumn Caption=" " ShowDeleteButton="false" ShowNewButtonInHeader="false" VisibleIndex="0" Width="0">
                                                        <CustomButtons>
                                                            <dxe:GridViewCommandColumnCustomButton ID="CustomDelete" Image-Url="/assests/images/crs.png" Text=" ">
                                                                <Image Url="/assests/images/crs.png">
                                                                </Image>
                                                            </dxe:GridViewCommandColumnCustomButton>
                                                        </CustomButtons>
                                                        <%--<HeaderCaptionTemplate>
                                                            <dxe:ASPxHyperLink ID="btnNew" runat="server" ForeColor="White" Text=" ">
                                                                <ClientSideEvents Click="function (s, e) { OnAddNewClick();}"  />

                                                            </dxe:ASPxHyperLink>
                                                        </HeaderCaptionTemplate>--%>
                                                    </dxe:GridViewCommandColumn>
                                                    <dxe:GridViewDataTextColumn Caption="SrlNo"  FieldName="SrlNo"   VisibleIndex="1" Width="0">
                                                        <PropertiesTextEdit>
                                                            <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>
                                                        </PropertiesTextEdit>
                                                    </dxe:GridViewDataTextColumn>
                                                    <dxe:GridViewDataTextColumn ReadOnly="true" CellStyle-Wrap="False" PropertiesTextEdit-ValidationSettings-ErrorFrameStyle-Wrap="False" EditFormCaptionStyle-Wrap="False" Caption="Product Code"  FieldName="ProductCode"  VisibleIndex="2" Width="150">
                                                        <PropertiesTextEdit>
                                                             <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                   
                                                               </PropertiesTextEdit>
                                                        <CellStyle Wrap="False"></CellStyle>  
                                                    </dxe:GridViewDataTextColumn>
                                                   <dxe:GridViewDataTextColumn ReadOnly="true" CellStyle-Wrap="False" PropertiesTextEdit-ValidationSettings-ErrorFrameStyle-Wrap="False" EditFormCaptionStyle-Wrap="False" Caption="Product Description"  FieldName="Productdescription"  VisibleIndex="2" Width="150">
                                                        <PropertiesTextEdit>
                                                             <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                   
                                                               </PropertiesTextEdit>
                                                        <CellStyle Wrap="False"></CellStyle>  
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn Caption="January" CellStyle-HorizontalAlign="Right" FieldName="January"   VisibleIndex="3" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn Caption="February" CellStyle-HorizontalAlign="Right" FieldName="February"   VisibleIndex="4" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                              <dxe:GridViewDataTextColumn Caption="March" CellStyle-HorizontalAlign="Right" FieldName="March"   VisibleIndex="5" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                           <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>

                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn Caption="April" CellStyle-HorizontalAlign="Right" FieldName="April"   VisibleIndex="6" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn Caption="May" CellStyle-HorizontalAlign="Right" FieldName="May"   VisibleIndex="7" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn Caption="June" CellStyle-HorizontalAlign="Right" FieldName="June"   VisibleIndex="8" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                           <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                                     <dxe:GridViewDataTextColumn Caption="July" CellStyle-HorizontalAlign="Right" FieldName="July"   VisibleIndex="9" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                               <dxe:GridViewDataTextColumn Caption="August" CellStyle-HorizontalAlign="Right" FieldName="August"   VisibleIndex="10" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                         <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                               <dxe:GridViewDataTextColumn Caption="September" CellStyle-HorizontalAlign="Right" FieldName="September"   VisibleIndex="11" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                      <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                               <dxe:GridViewDataTextColumn Caption="October" CellStyle-HorizontalAlign="Right" FieldName="October"   VisibleIndex="12" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                         <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                               <dxe:GridViewDataTextColumn Caption="November" CellStyle-HorizontalAlign="Right" FieldName="November"   VisibleIndex="13" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                       <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                               <dxe:GridViewDataTextColumn Caption="December" CellStyle-HorizontalAlign="Right" FieldName="December"   VisibleIndex="14" Width="100">
                                                        <PropertiesTextEdit DisplayFormatString="0.00" Style-HorizontalAlign="Right">
                                                            <MaskSettings AllowMouseWheel="False" Mask="&lt;0..999999999&gt;.&lt;00..999&gt;" />
                                                                <ValidationSettings EnableCustomValidation="false" ErrorDisplayMode="None" ValidateOnLeave="false" RequiredField-IsRequired="false"></ValidationSettings>                                                
                                                            <Style HorizontalAlign="Right">
                                                            </Style>
                                                        </PropertiesTextEdit>
                                                        
                                                        <CellStyle HorizontalAlign="Right">
                                                        </CellStyle>
                                                    </dxe:GridViewDataTextColumn>
                                              
                                                </Columns>
                                               
                                                <ClientSideEvents EndCallback="OnEndCallback" CustomButtonClick="OnCustomButtonClick" RowClick="GetVisibleIndex" BatchEditStartEditing="gridFocusedRowChanged" />
                                                <SettingsDataSecurity AllowEdit="true" />
                                                <SettingsEditing Mode="Batch" NewItemRowPosition="Bottom">
                                                    <BatchEditSettings ShowConfirmOnLosingChanges="false" EditMode="row"/>
                                                </SettingsEditing>
                                                 <Settings VerticalScrollableHeight="150" VerticalScrollBarMode="Auto" />
                                                <SettingsBehavior ColumnResizeMode="Disabled" />
                                            </dxe:ASPxGridView>
        </div>

         <%-- BUTTON SECTION --%>
          <div style="clear: both;"></div>
        <br />
        <div class="clearfix">
            <asp:Label ID="lbl_quotestatusmsg" runat="server" Text="" Font-Bold="true" ForeColor="Red" Font-Size="Medium"></asp:Label>
            <dxe:ASPxButton ID="btn_SaveRecords" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="Update" CssClass="btn btn-success" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                <ClientSideEvents Click="function(s, e) {Save_ButtonClick();}" />
                
            </dxe:ASPxButton>
             <dxe:ASPxButton ID="btncancel" ClientInstanceName="cbtn_cancel" runat="server" AccessKey="X" AutoPostBack="False" Text="Cancel"  CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                <ClientSideEvents Click="function(s, e) {cancel_ButtonClick();}" />
                
            </dxe:ASPxButton>
            <asp:HiddenField ID="hdnflag" runat="server" />
            <asp:HiddenField ID="hdnimportyear" runat="server" />
           <%-- <dxe:ASPxButton ID="ASPxButton1" ClientInstanceName="cbtn_SaveRecords" runat="server" AccessKey="X" AutoPostBack="False" Text="Save & Ex&#818;it" CssClass="btn btn-primary" meta:resourcekey="btnSaveRecordsResource1" UseSubmitBehavior="False">
                <ClientSideEvents Click="function(s, e) {SaveExit_ButtonClick();}" />
            </dxe:ASPxButton>--%>
            </div>
        <%-- END BUTTON SECTION --%>
    </div>
</asp:Content>
