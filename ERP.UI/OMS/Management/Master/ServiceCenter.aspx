<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                21-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ServiceCenter.aspx.cs" Inherits="ERP.OMS.Management.Master.ServiceCenter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <title></title>
     <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            var MaxLength = 150;
            $('#txtRemarks').keypress(function (e) {
                if ($(this).val().length >= MaxLength) {
                    e.preventDefault();
                }
            });
        });
        //Code for UDF Control 
        function OpenUdf() {
            if (document.getElementById('IsUdfpresent').value == '0') {
                jAlert("UDF not define.");
            }
            else {
                var url = 'frm_BranchUdfPopUp.aspx?Type=de';
                popup.SetContentUrl(url);
                popup.Show();
            }
            return true;
        }
        // End Udf Code

        function popUpRedirect(obj) {
            jAlert('Saved successfully');
            window.location.href = obj;


        }


        var pinCodeWithAreaId = [];
      

        function ClientSaveClick() {
            Page_ClientValidate();
            if (document.getElementById('txtCode').value.trim() == '') {
                return false;
            }
            if (document.getElementById('txtdrivername').value.trim() == '') {
                return false;
            }
            return true;
        }



        function ListBind() {

            var config = {
                '.chsn': {},
                '.chsn-deselect': { allow_single_deselect: true },
                '.chsn-no-single': { disable_search_threshold: 10 },
                '.chsn-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chsn-width': { width: "100%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

        }
        function lstCountry() {
            $('#lstCountry').fadeIn();
        }
        
        function disp_prompt(name) {

           
            if (name == "tab1") {
               
               // document.location.href = "Driver_Correspondence.aspx";
            }
            else if (name == "tab2") {
               
               // document.location.href = "Driver_document.aspx";
            }


        }

        function Close() {
            editwin.close();
        }

       



        function DeleteRow(keyValue) {
            doIt = confirm('Confirm delete?');
            if (doIt) {
                grid.PerformCallback('Delete~' + keyValue);
                
            }
            else {

            }
        }


        function ShowHideFilter1(obj) {

            gridTerminal.PerformCallback(obj);
        }
       
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function setvaluetovariable(obj1) {
            combo1.PerformCallback(obj1);
        }
        function setvaluetovariable1(obj1) {
            combo2.PerformCallback(obj1);
        }
        function CallList(obj1, obj2, obj3) {
           
            if (obj1.value == "") {
                obj1.value = "%";
            }
            var obj5 = '';
            ajax_showOptionsTEST(obj1, obj2, obj3, obj5);
            if (obj1.value == "%") {
                obj1.value = "";
            }
        }
        function hide_show(obj) {
            if (obj == 'All') {
                document.getElementById("client_pro").style.display = "none";
            }

        }
        function GetClick() {
            btnC.PerformCallback();
        }
        function Page_Load() {
            document.getElementById("TdCombo").style.display = "none";
        }
        
        function CheckLengthNote1() {
            var textbox = document.getElementById('txtRemarks').value;
            if (textbox.trim().length >= 150) {
                return false;
            }
            else {
                return true;
            }
        }
        function CheckingTD(obj) {

            var gridstat = gridTerminal.cpCompCombo;
            if (gridstat == 'anew')
                combo.SetFocus();

           
        }
        FieldName = "cmbExport_DDDWS";
        function LastCall(obj) {
            //height();
        }
        // Code Added By Priti on 21122016 to check Unique Short Name
        function fn_ctxtPro_Name_TextChanged() {
            var finCd = 0;
            if (GetObjectID('hiddenedit').value != '') {
                finCd = GetObjectID('hiddenedit').value;
            }

            var ShortName = document.getElementById("txtCode").value.trim();
            var qString = window.location.href.split("=")[1];
            $.ajax({
                type: "POST",
                url: "DriverAddEdit.aspx/CheckUniqueName",
                data: JSON.stringify({ ShortName: ShortName, divId: finCd }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;

                    if (data == true) {
                        jAlert("Please Enter Unique Vehicle No");
                        document.getElementById("txtCode").value = '';
                        document.getElementById("txtCode").focus();
                        return false;
                    }
                }
            });
        }
        </script>
          <style type="text/css">
       
        .abs {
            position:absolute;
            right: -19px;
            top: 4px;
        }
          .abs1 {
            position:absolute;
            right: -19px;
            top: 4px;
        }
     
         .chosen-container.chosen-container-single {
            width:100% !important;
        }
        .chosen-choices {
            width:100% !important;
        }
        #lstCountry,#lstState,#lstCity,#lstArea,#lstPin,#lstBranchHead {
            width:100%;
        }
        #lstCountry,#lstState,#lstCity,#lstArea,#lstPin,#lstBranchHead {
            display:none !important;
            
        }
        #lstCountry_chosen,#lstState_chosen,#lstCity_chosen,#lstArea_chosen,#lstPin_chosen,#lstBranchHead_chosen{
            width:100% !important;
        }
        #PageControl1_CC {
        overflow:visible !important;
        }
        #lstState_chosen, #lstCountry_chosen, #lstCity_chosen,#lstPin_chosen,#lstBranchHead_chosen {
            margin-bottom:5px;
        }
              .divControlClass>span.controlClass {
                  margin-top:8px;
              }

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

        select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
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
            top: 6px;
            right: -2px;
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
        .TableMain100 #GrdHolidays , #gridStatus
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

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
        
            <h3><asp:Label ID="lblAddEdit" runat="server" Text=""></asp:Label></h3>
            <div class="crossBtn"><a href="ServiceCenterList.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>

        <%--debjyoti 22-12-2016--%>
        <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" 
                                            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"  ClientInstanceName="popup" Height="630px"
                                            Width="600px" HeaderText="Add/Modify UDF" Modal="true" AllowResize="true" ResizingMode="Postponed" >
                                            <ContentCollection>
                                                <dxe:PopupControlContentControl runat="server">
                                                </dxe:PopupControlContentControl>
                                            </ContentCollection>
                                        </dxe:ASPxPopupControl>

        <asp:HiddenField runat="server" ID="IsUdfpresent" />
        <asp:HiddenField runat="server" ID="hiddenedit" />
                                        <%--End debjyoti 22-12-2016--%>
        <div class="form_main">
        <table style="width: 100%">
            <tr>
                <td style="width: 100%">
                    <dxe:ASPxPageControl ID="PageControl1" runat="server" Width="100%" ActiveTabIndex="0"
                        ClientInstanceName="page">
                        <TabPages>
                            <dxe:TabPage Text="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <div class="totalWrap">
                                          
                                            <div class="col-md-3">
                                                <label>Center Code <span style="color:red">*</span></label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtCode" runat="server"  ClientIDMode="Static" Width="100%" 
                                                        MaxLength="80" onblur="fn_ctxtPro_Name_TextChanged()">
                                                                   
                                                    </asp:TextBox>

                                                    <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtCode"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="drivegrp" >                                                        
                                                    </asp:RequiredFieldValidator>
                                                 </div>
                                            </div>
                                              <div class="col-md-3">
                                                <label>Service Center Name <span style="color:red">*</span></label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtCentername" runat="server"  ClientIDMode="Static" Width="100%" 
                                                        MaxLength="150" >
                                                                   
                                                    </asp:TextBox>

                                                      <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtCentername"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="drivegrp" >                                                        
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <label>Branch </label>
                                                <div class="relative simple-select">
                                                    <asp:DropDownList ID="cmbParentBranch" runat="server" Width="100%"
                                                        >
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                     
                                            
                                            
                                              <div class="col-md-3">
                <label>Is Active?</label>
                <div>
                    <dxe:ASPxCheckBox ID="chkIsActive" runat="server" Text="">
                    </dxe:ASPxCheckBox>
                </div>
            </div><div class="clear"></div>
                                            <div class="col-md-6">
                                                <label>Remarks </label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtRemarks" runat="server" Width="100%"  TextMode="MultiLine" MaxLength="150"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="clear"></div>
                                            <div class="col-md-12" style="padding-top: 10px;">
                                                <%-- <% if (rights.CanAdd || rights.CanEdit)
                                                    { %>--%>
                                                <asp:Button ID="btnSave" runat="server" Text="Save"  ValidationGroup="drivegrp" CssClass="btn btn-primary dxbButton" OnClick="btnSave_Click" OnClientClick="return ClientSaveClick()"
                                                    />
                                                <asp:Button ID="Button2" runat="server" Text="Cancel" CssClass="btn btn-danger dxbButton" OnClick="Button2_Click"
                                               />
                                              <%--  <input type="button" id="btnUdf" value="UDF" class="btn btn-primary dxbButton" onclick="OpenUdf()" />--%>

                                                <asp:Button ID="btnUdf" runat="server" Text="UDF"  CssClass="btn btn-primary dxbButton" style="display:none;" OnClientClick="if(OpenUdf()){ return false;}"
                                                     />

                                                <%--  <% } %>--%>
                                            </div>
                                        </div>

                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            
                             <dxe:TabPage Name="Correspondence" Text="Correspondence" Visible="false">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Documents" Text="Documents" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                             <dxe:TabPage Name="UDF" Text="UDF" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                        </TabPages>
                        <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                            var activeTab   = page.GetActiveTab();
	                                            
	                                            var Tab0 = page.GetTab(0);
	                                                var Tab1 = page.GetTab(1);
	                                                var Tab2 = page.GetTab(2);
	                                                if(activeTab == Tab0)
	                                                {
	                                                    disp_prompt('tab0');
	                                                }
	                                               else  if(activeTab == Tab1)
	                                                {
	                                                    disp_prompt('tab1');
	                                                }
	                                                else if(activeTab == Tab2)
	                                                {
	                                                    disp_prompt('tab2');
	                                                }

                                             
                                             
	                                            
	                                            }"></ClientSideEvents>
                    </dxe:ASPxPageControl>

                   <%-- <asp:SqlDataSource ID="sqlCompany" runat="server" 
                        SelectCommand="select cmp_internalId,cmp_name from tbl_master_company"></asp:SqlDataSource>--%>

                  <%--  <asp:SqlDataSource ID="SqlExchange" runat="server" 
                        SelectCommand="select exch_internalId,(select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId=@CompID">
                        <SelectParameters>
                            <asp:SessionParameter Name="CompID" SessionField="ID" Type="string" />
                        </SelectParameters>
                    </asp:SqlDataSource>--%>

                   <%-- <asp:SqlDataSource ID="SqlParentTerminal" runat="server" 
                        SelectCommand="select distinct TradingTerminal_TerminalID from Master_TradingTerminal"></asp:SqlDataSource>--%>

                    <%--<asp:SqlDataSource ID="SqlVendor" runat="server" 
                        SelectCommand="select CTCLVendor_ID,CTCLVendor_Name+' ['+CTCLVendor_ProductType+']' as CTCLVendor_Name from Master_CTCLVendor where CTCLVendor_ExchangeSegment=@CompID1">
                        <SelectParameters>
                            <asp:SessionParameter Name="CompID1" SessionField="ID1" Type="string" />
                        </SelectParameters>
                    </asp:SqlDataSource>--%>

                    <%--<asp:SqlDataSource ID="TrdTerminal" runat="server"
                        SelectCommand="select td.TradingTerminal_ID,e.exh_shortName+'-'+ce.exch_segmentId as Exchange,td.TradingTerminal_TerminalID,td.TradingTerminal_ParentTerminalID,td.TradingTerminal_ProTradeID,td.TradingTerminal_brokerid ,(select isnull(cnt_firstName,'') +' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+ ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' from tbl_master_contact where cnt_internalId=td.TradingTerminal_BrokerID) as brokid,(Select  ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName from tbl_master_contact WHERE  cnt_internalid =td.TradingTerminal_ProTradeID) as ProTradeID,(Select  ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName from tbl_master_contact WHERE  cnt_internalid =td.TradingTerminal_CliTradeID) as CliTradeID,(Select  ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName from tbl_master_contact WHERE  cnt_internalid =td.TradingTerminal_AllTradeID) as AllTradeID from  Master_TradingTerminal td, tbl_master_exchange e, tbl_master_companyExchange ce where td.TradingTerminal_CompanyID=ce.exch_compId  and td.TradingTerminal_ExchangeSegmentID=ce.exch_InternalId and  e.exh_cntId=ce.exch_exchId and td.TradingTerminal_BranchID=@BranchID order by TradingTerminal_ID desc"
                        DeleteCommand="delete from Master_TradingTerminal where TradingTerminal_ID=@TradingTerminal_ID">
                        <SelectParameters>
                            <asp:SessionParameter Name="BranchID" SessionField="KeyVal_InternalID" Type="string" />
                        </SelectParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="TradingTerminal_ID" Type="Int32" />
                        </DeleteParameters>
                    </asp:SqlDataSource>--%>
                </td>
            </tr>
        </table>
    </div>
    </div>
</asp:Content>
