<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                20-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="City" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_City" CodeBehind="City.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        
        .dxpc-contentWrapper{
            background: rgb(237,243,244);
        }
        .dxpc-headerContent{
            color: white;
        }
    </style>
    <%--    <link href="../../CentralData/CSS/GenericCss.css" rel="stylesheet" type="text/css" />--%>
    <script type="text/javascript" src="../../CentralData/JSScript/GenericJScript.js"></script>

    <script type="text/javascript">
        //function is called on changing country
        //        function OnCountryChanged(cmbCountry) 
        //        {
        //            grid.GetEditor("cou_country").PerformCallback(cmbCountry.GetValue().toString());
        //        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
    </script>

    <script type="text/javascript">
        //$(document).ready(function () {
        //    alert('1');
        //    $('#valid').attr('style', 'display:none');
        //});

        function fn_PopOpen() {
            $('#valid').attr('style', 'display:none');
            document.getElementById('<%=hiddenedit.ClientID%>').value = '';
            //            alert('HidenEdit-'+GetObjectID('<%=hiddenedit.ClientID%>').value);
            ctxtcityName.SetText('');
            cCmbCountryName.SetValue("1");
            cCmbState.SetSelectedIndex(0);
            //ctxtNseCode.SetText('');
            //ctxtBseCode.SetText('');
            //ctxtMcxCode.SetText('');
            //ctxtMcsxCode.SetText('');
            //ctxtNcdexCode.SetText('');
            //ctxtCdslCode.SetText('');
            //ctxtNsdlCode.SetText('');
            //ctxtNdmlCode.SetText('');
            //ctxtDotexCode.SetText('');
            //ctxtCvlCode.SetText('');
            cPopup_Empcitys.Show();
            OnCmbCountryName_ValueChange();
        }
        function btnSave_citys() {
            if (ctxtcityName.GetText() == '') {
                //alert('Please Enter City Name');
                $('#valid').attr('style', 'display:block;position: absolute;right: -4px;top: 30px;');
                ctxtcityName.Focus();
            }
            else {
                if (document.getElementById('<%=hiddenedit.ClientID%>').value == '')
                    grid.PerformCallback('savecity~');
                else
                    grid.PerformCallback('updatecity~' + GetObjectID('<%=hiddenedit.ClientID%>').value);
            }
        }
        function fn_btnCancel() {
            cPopup_Empcitys.Hide();
        }
        function fn_Editcity(keyValue) {
            grid.PerformCallback('Edit~' + keyValue);
        }
        function fn_Deletecity(keyValue) {
            //var result=confirm('Confirm delete?');
            //if(result)
            //{
            //    grid.PerformCallback('Delete~' + keyValue);
            //}
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {             
                if (r == true) {                  
                    grid.PerformCallback('Delete~' + keyValue);
                }
                else {
                    return false;
                }
            });
           
        }

        var editedState = "";
        function grid_EndCallBack() {
            
            if (grid.cpinsert != null) {
                if (grid.cpinsert == 'Success') {

                    jAlert('Saved successfully');
                    cPopup_Empcitys.Hide();
                   
                }
                else {
                    jAlert("Error On Insertion \n 'Please Try Again!!'");
                    cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpEdit != null) {
                ctxtcityName.SetText(grid.cpEdit.split('~')[0]);
                //cCmbState.SetValue(grid.cpEdit.split('~')[1]);
                editedState = grid.cpEdit.split('~')[1];
                cCmbCountryName.SetValue(grid.cpEdit.split('~')[2]);
                OnCmbCountryName_ValueChange();
                //ctxtNseCode.SetText(grid.cpEdit.split('~')[3]);
                //ctxtBseCode.SetText(grid.cpEdit.split('~')[4]);
                //ctxtMcxCode.SetText(grid.cpEdit.split('~')[5]);
                //ctxtMcsxCode.SetText(grid.cpEdit.split('~')[6]);
                //ctxtNcdexCode.SetText(grid.cpEdit.split('~')[7]);
                //ctxtCdslCode.SetText(grid.cpEdit.split('~')[8]);
                //ctxtNsdlCode.SetText(grid.cpEdit.split('~')[9]);
                //ctxtNdmlCode.SetText(grid.cpEdit.split('~')[10]);
                //ctxtCvlCode.SetText(grid.cpEdit.split('~')[11]);
                //ctxtDotexCode.SetText(grid.cpEdit.split('~')[12]);
                GetObjectID('<%=hiddenedit.ClientID%>').value = grid.cpEdit.split('~')[13];
                cPopup_Empcitys.Show();
            }
            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Success') {
                    jAlert('Update Successfully');
                    cPopup_Empcitys.Hide();
                }
                else {
                    jAlert("Error on Updation\n'Please Try again!!'")
                    cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpUpdateValid != null) {
                if (grid.cpUpdateValid == "StateInvalid") {
                    jAlert("Please Select State");
                    //cPopup_Empcitys.Show();
                    //cCmbState.Focus();
                    //alert(GetObjectID('<%=hiddenedit.ClientID%>').value);
                    //grid.PerformCallback('Edit~'+GetObjectID('<%=hiddenedit.ClientID%>').value);
                    //grid.cpUpdateValid=null;
                }
            }
            if (grid.cpDelete != null) {
                if (grid.cpDelete == 'Success') {
                    jAlert('Deleted sccessfully');
                    grid.cpDelete = null;
                    grid.PerformCallback();
                }
                else {
                    jAlert(grid.cpDelete);
                    grid.cpDelete = null;
                    grid.PerformCallback();
                }
            }
            if (grid.cpExists != null) {
                if (grid.cpExists == "Exists") {
                    jAlert('Duplicate value.');
                    cPopup_Empcitys.Hide();
                }
                else {
                    jAlert("Error on operation \n 'Please Try again!!'")
                    cPopup_Empcitys.Hide();
                }
            }
            

        }
        function OnCmbCountryName_ValueChange() {
            cCmbState.PerformCallback("BindState~" + cCmbCountryName.GetValue());
        }
        function CmbState_EndCallback() {
            cCmbState.SetSelectedIndex(0);
            if (editedState != "") {
                cCmbState.SetValue(editedState);
                editedState = "";
            }
            cCmbState.Focus();
        } 

        function gridRowclick(s, e) {
            $('#cityGrid').find('tr').removeClass('rowActive');
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
    </script>
    <style>
       /*Rev 1.0*/
        .outer-div-main {
            background: #ffffff;
            padding: 10px;
            border-radius: 10px;
            box-shadow: 1px 1px 10px #11111154;
        }

        /*.form_main {
            overflow: hidden;
        }*/

        label , .mylabel1, .clsTo, .dxeBase_PlasticBlue
        {
            color: #141414 !important;
            font-size: 14px !important;
                font-weight: 500 !important;
                margin-bottom: 0 !important;
                    line-height: 20px;
        }

        #GrpSelLbl .dxeBase_PlasticBlue
        {
                line-height: 20px !important;
        }

        /*select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }*/

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

        .calendar-icon {
            position: absolute;
            bottom: 6px;
            right: 5px;
            z-index: 0;
            cursor: pointer;
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

        .simple-select::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 26px;
            right: 13px;
            font-size: 16px;
            transform: rotate(269deg);
            font-weight: 500;
            background: #094e8c;
            color: #fff;
            height: 18px;
            display: block;
            width: 26px;
            /* padding: 10px 0; */
            border-radius: 4px;
            text-align: center;
            line-height: 19px;
            z-index: 0;
        }
        .simple-select {
            position: relative;
        }
        .simple-select:disabled::after
        {
            background: #1111113b;
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

        .styled-checkbox {
        position: absolute;
        opacity: 0;
        z-index: 1;
    }

        .styled-checkbox + label {
            position: relative;
            /*cursor: pointer;*/
            padding: 0;
            margin-bottom: 0 !important;
        }

            .styled-checkbox + label:before {
                content: "";
                margin-right: 6px;
                display: inline-block;
                vertical-align: text-top;
                width: 16px;
                height: 16px;
                /*background: #d7d7d7;*/
                margin-top: 2px;
                border-radius: 2px;
                border: 1px solid #c5c5c5;
            }

        .styled-checkbox:hover + label:before {
            background: #094e8c;
        }


        .styled-checkbox:checked + label:before {
            background: #094e8c;
        }

        .styled-checkbox:disabled + label {
            color: #b8b8b8;
            cursor: auto;
        }

            .styled-checkbox:disabled + label:before {
                box-shadow: none;
                background: #ddd;
            }

        .styled-checkbox:checked + label:after {
            content: "";
            position: absolute;
            left: 3px;
            top: 9px;
            background: white;
            width: 2px;
            height: 2px;
            box-shadow: 2px 0 0 white, 4px 0 0 white, 4px -2px 0 white, 4px -4px 0 white, 4px -6px 0 white, 4px -8px 0 white;
            transform: rotate(45deg);
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
        .TableMain100 #GrdHolidays , #StateGrid
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
                margin-top: 3px;
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

        .for-cust-icon {
            position: relative;
            z-index: 1;
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

        .chosen-container-single .chosen-single span
        {
            min-height: 30px;
            line-height: 30px;
        }

        .chosen-container-single .chosen-single div {
        background: #094e8c;
        color: #fff;
        border-radius: 4px;
        height: 26px;
        top: 1px;
        right: 1px;
        /*position:relative;*/
    }

        .chosen-container-single .chosen-single div b {
            display: none;
        }

        .chosen-container-single .chosen-single div::after {
            /*content: '<';*/
            content: url(../../../assests/images/left-arw.png);
            position: absolute;
            top: 2px;
            right: 5px;
            font-size: 18px;
            transform: rotate(269deg);
            font-weight: 500;
        }

    .chosen-container-active.chosen-with-drop .chosen-single div {
        background: #094e8c;
        color: #fff;
    }

        .chosen-container-active.chosen-with-drop .chosen-single div::after {
            transform: rotate(90deg);
            right: 5px;
        }

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
    <script type="text/javascript">
        $(document).ready(function () {
            if ($('body').hasClass('mini-navbar')) {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 90;
                grid.SetWidth(cntWidth);
            } else {
                var windowWidth = $(window).width();
                var cntWidth = windowWidth - 220;
                grid.SetWidth(cntWidth);
            }
            $('.navbar-minimalize').click(function () {
                if ($('body').hasClass('mini-navbar')) {
                    var windowWidth = $(window).width();
                    var cntWidth = windowWidth - 220;
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
<div class="clearfix">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>District</h3>
        </div>
    </div>
    
        <div class="form_main">
            <div class="rgth pull-left full">
                <div class="Main">
                    <%--<div class="TitleArea">
                    <strong><span style="color: #000099">City List</span></strong>
                </div>--%>
                    <div class="SearchArea clearfix">
                        <div class="FilterSide clearfix">
                            <div style="float: left; padding-right: 5px;">
                                <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="fn_PopOpen()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span>Add New</span> </a><%} %>
                                <%--  <a href="javascript:ShowHideFilter('s');" class="btn btn-primary"><span >Show Filter</span></a>--%>
                                <a href="javascript:ShowHideFilter('All');" class="btn btn-primary btn-radius" style="display: none;"><span>All Records</span></a>
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

                            <%--<div class="ExportSide pull-right">

                            <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                ValueType="System.Int32" Width="130px">
                                <Items>
                                    <dxe:ListEditItem Text="Select" Value="0" />
                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                    <dxe:ListEditItem Text="CSV" Value="4" />
                                </Items>
                                <Border BorderColor="black" />
                                <DropDownButton Text="Export">
                                </DropDownButton>
                            </dxe:ASPxComboBox>

                        </div>--%>
                        </div>

                    </div>
                    <div class="clear"></div>
                </div>
                <div class="clear"></div>
                <div class="GridViewArea relative">
                    <dxe:ASPxGridView ID="cityGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                        KeyFieldName="city_id" Width="100%" OnHtmlRowCreated="cityGrid_HtmlRowCreated" Settings-VerticalScrollableHeight="280" Settings-VerticalScrollBarMode="Auto"
                        OnHtmlEditFormCreated="cityGrid_HtmlEditFormCreated" OnCustomCallback="cityGrid_CustomCallback" SettingsBehavior-AllowFocusedRow="true" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" >
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="CityID" FieldName="city_id" ReadOnly="True"
                                Visible="False" FixedStyle="Left" VisibleIndex="0">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="StateID" FieldName="state_id" ReadOnly="True"
                                Visible="False" FixedStyle="Left" VisibleIndex="1">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="CountryID" FieldName="cou_id" ReadOnly="True"
                                Visible="False" FixedStyle="Left" VisibleIndex="2">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="District" FieldName="city_name" Width="30%" FixedStyle="Left"
                                Visible="True" VisibleIndex="3" PropertiesTextEdit-MaxLength="50">
                                <EditFormSettings Visible="True" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="State" FieldName="state" Width="30%" FixedStyle="Left"
                                Visible="True" VisibleIndex="4">
                                <EditFormSettings Visible="True" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Country" FieldName="cou_country" Visible="True"
                                Width="30%" VisibleIndex="5">
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <%-- <dxe:GridViewDataTextColumn Caption="NSECode" FieldName="City_NSECode" Visible="True"
                            Width="6%" VisibleIndex="6">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="BSECode" FieldName="City_BSECode" Visible="True"
                            Width="6%" VisibleIndex="7">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="MCXCode" FieldName="City_MCXCode" Visible="True"
                            Width="6%" VisibleIndex="8">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="MCXSXCode" FieldName="City_MCXSXCode" Visible="True"
                            Width="6%" VisibleIndex="9">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="NCDEXCode" FieldName="City_NCDEXCode" Visible="True"
                            Width="6%" VisibleIndex="10">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="CDSLCode" FieldName="City_CDSLCode" Visible="True"
                            Width="6%" VisibleIndex="11">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="NSDLCode" FieldName="City_NSDLCode" Visible="True"
                            Width="6%" VisibleIndex="12">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="NDMLCode" FieldName="City_NDMLCode" Visible="True"
                            Width="6%" VisibleIndex="13">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="CVLCode" FieldName="City_CVLCode" Visible="True"
                            Width="6%" VisibleIndex="14">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="DotExCode" FieldName="City_DotExCode" Visible="True"
                            Width="6%" VisibleIndex="15">
                            <CellStyle CssClass="gridcellleft" Wrap="False">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>--%>
                            <dxe:GridViewDataTextColumn ReadOnly="True" Width="0" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <HeaderTemplate>
                                    <span></span>
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <div class='floatedBtnArea'>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="fn_Editcity('<%# Container.KeyValue %>')" class="" title="">
                                        <span class='ico editColor'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='hidden-xs'>Edit</span></a><%} %>
                                    <% if (rights.CanDelete)
                                       { %>
                                    <a href="javascript:void(0);" onclick="fn_Deletecity('<%# Container.KeyValue %>')" title="">
                                        <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a><%} %>
                                    </div>
                                </DataItemTemplate>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                  
                        <Settings ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" RowClick="gridRowclick" />
                    </dxe:ASPxGridView>
                </div>
                <div class="PopUpArea">
                    <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
                        Width="700px" HeaderText="Add/Modify District" PopupHorizontalAlign="WindowCenter"
                        BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" ba>
                        <ContentCollection>
                            <dxe:PopupControlContentControl runat="server">
                                <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                                <div class="Top clearfix">
                                    <div style="margin-bottom: 5px;" class="col-md-4">
                                        <div class="cityDiv" style="padding-top: 5px;">
                                            Country
                                        </div>
                                        <div class="Left_Content" style="padding-top: 5px;">
                                            <dxe:ASPxComboBox ID="CmbCountryName" ClientInstanceName="cCmbCountryName" runat="server"
                                                ValueType="System.String" Width="100%" EnableSynchronization="True">
                                                <ClientSideEvents ValueChanged="function(s,e){OnCmbCountryName_ValueChange()}"></ClientSideEvents>
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                    <div style="margin-bottom: 5px;" class="col-md-4">
                                        <div class="cityDiv" style="padding-top: 5px;">
                                            State
                                        </div>
                                        <div class="Left_Content" style="padding-top: 5px;">
                                            <dxe:ASPxComboBox ID="CmbState" ClientInstanceName="cCmbState" runat="server" ValueType="System.String"
                                                Width="100%" EnableSynchronization="True" OnCallback="CmbState_Callback">
                                                <ClientSideEvents EndCallback="CmbState_EndCallback"></ClientSideEvents>
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                    <div style="margin-bottom: 5px; position: relative" class="col-md-4">
                                        <div class="cityDiv" style="padding-top: 5px;">
                                            District<span style="color: red">*</span>
                                        </div>
                                        <div class="Left_Content" style="padding-top: 5px;">
                                            <dxe:ASPxTextBox ID="txtcityName" ClientInstanceName="ctxtcityName" runat="server" MaxLength="50"
                                                Width="100%">
                                            </dxe:ASPxTextBox>
                                            <div id="valid" style="display: none; position: absolute; right: -4px; top: 30px;">
                                                <img id="grid_DXPEForm_DXEFL_DXEditor2_EI" title="Mandatory" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-YRohc" alt="Required"></div>
                                            <%--<asp:RequiredFieldValidator ID="rvCity" runat="server"  ControlToValidate="txtcityName" ValidationGroup="cty" ErrorMessage="not Valid" ></asp:RequiredFieldValidator>--%>
                                        </div>

                                    </div>
                                </div>
                                <div class="ContentDiv">
                                    <%--<div style="height: 20px; width: 100%; background-color: Gray; text-align:center;">
                                    <h5>Static Code</h5>
                                </div>
                                <div class="col-md-6 text-center" style="background-color: Gray;">
                                    Exchange
                                </div>
                                <div class="col-md-6 text-center" style="background-color: Gray;">
                                    Value
                                </div>
                                <div style="clear: both"></div>
                                <div class="ScrollDiv">
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv">
                                            NSE Code
                                        </div>
                                        <div style="padding-top: 5px;">
                                            <dxe:ASPxTextBox ID="txtNseCode" ClientInstanceName="ctxtNseCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv">
                                            BSE Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtBseCode" ClientInstanceName="ctxtBseCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv">
                                            MCX Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtMcxCode" ClientInstanceName="ctxtMcxCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv">
                                            MCXSX Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtMcsxCode" ClientInstanceName="ctxtMcsxCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 mBot10">

                                        <div class="cityDiv ">
                                            NCDEX Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtNcdexCode" ClientInstanceName="ctxtNcdexCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv ">
                                            CDSL Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtCdslCode" ClientInstanceName="ctxtCdslCode" CssClass="cityTextbox"
                                                runat="server" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv ">
                                            NSDL Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtNsdlCode" ClientInstanceName="ctxtNsdlCode" CssClass="cityTextbox"
                                                runat="server" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv ">
                                            NDML Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtNdmlCode" ClientInstanceName="ctxtNdmlCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv ">
                                            CVL Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtCvlCode" ClientInstanceName="ctxtCvlCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="col-md-4 mBot10">
                                        <div class="cityDiv ">
                                            DOTEX Code
                                        </div>
                                        <div>
                                            <dxe:ASPxTextBox ID="txtDotexCode" ClientInstanceName="ctxtDotexCode" runat="server"
                                                CssClass="cityTextbox" Width="100%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <br style="clear: both;" />
                                </div>--%>
                                    <br style="clear: both;" />
                                    <div class="Footer">
                                        <div class="col-md-12">
                                            <dxe:ASPxButton ID="btnSave_citys" ClientInstanceName="cbtnSave_citys" runat="server"
                                                AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                                <ClientSideEvents Click="function (s, e) {btnSave_citys();}" />
                                            </dxe:ASPxButton>
                                            <dxe:ASPxButton ID="btnCancel_citys" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                                <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                            </dxe:ASPxButton>
                                        </div>
                                    </div>
                                </div>
                                <%-- </div>--%>
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                    </dxe:ASPxPopupControl>
                    <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                    </dxe:ASPxGridViewExporter>
                </div>
                <div class="HiddenFieldArea" style="display: none;">
                    <asp:HiddenField runat="server" ID="hiddenedit" />
                </div>
            </div>
            <%--left conte slide area--%>
        <%--<div class="ltfg">
            <span class="Closer"><i class="fa fa-angle-right" aria-hidden="true"></i></span>
            <div class="innerCont">
                <div class="part">
                    <label>Current Bank Balance</label>
                    <div class="values">
                        12211.22
                    </div>
                </div>
                <div class="part">
                    <label>Account Balance</label>
                    <div class="values">
                        125463.22
                    </div>
                </div>
            </div> 
        </div>--%>
    </div>
    
    </div>
</div>
    

</asp:Content>
