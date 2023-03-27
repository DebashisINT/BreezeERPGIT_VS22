<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                24-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Driver Master" Language="C#" AutoEventWireup="true" CodeBehind="DriverAddEdit.aspx.cs" Inherits="ERP.OMS.Management.Master.DriverAddEdit" MasterPageFile="~/OMS/MasterPage/ERP.Master"  EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   

   
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
            jAlert("Saved successfully");
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
                document.location.href = "Driver_Correspondence.aspx";
            }
            else if (name == "tab2") {               
                document.location.href = "Driver_document.aspx";
            }

           
        }

        function Close() {
            editwin.close();
        }

       

       

        function DeleteRow(keyValue) {
            doIt = confirm('Confirm delete?');
            if (doIt) {
                grid.PerformCallback('Delete~' + keyValue);
                //height();
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
            if (textbox.trim().length >=150) {
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
                        jAlert("Unique ID already exists.");
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
    </style>

     <style>
    
        .chosen-container.chosen-container-multi,
        .chosen-container.chosen-container-single {
            width:100% !important;
        }
        .chosen-choices {
            width:100% !important;
        }
        #ListBoxVehRegNo {
            width:200px;
        }
        .hide {
            display:none;
        }
        .dxtc-activeTab .dxtc-link  {
            color:#fff !important;
        }
    </style>

        <!-- Kallol 150517 -->
      <script src="/assests/pluggins/choosen/choosen.min.js"></script>
        <script lang="Javascript" type="text/javascript">
            $(document).ready(function () {
                BindActivityType();

                $("#ListBoxVehRegNo").chosen().change(function () {
                    var Id = $(this).val();
                    //alert($(this).text);
                    $('#<%=hdnVehRegNo.ClientID %>').val(Id);
                    $('#<%=hdnEditRegNo.ClientID %>').val(Id); //hid field for Edit mode 
                    $('#MandatoryVehRegNo').attr('style', 'display:none');

                });
            });


            function BindActivityType() {
                //alert("Hi");
                var lBox = $('select[id$=ListBoxVehRegNo]');
                var listItems = [];
                var selectedNoteId = '';
                //if (noteTilte) {
                    selectedNoteId = "";
                //}
                lBox.empty();

                //alert(selectedNoteId);
                $.ajax({
                    type: "POST",
                    url: 'DriverAddEdit.aspx/GetAllActiveVehicleRegNo',
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify({ NoteId: selectedNoteId }),
                    dataType: "json",
                    success: function (msg) {
                        var list = msg.d;
                       
                        if (list.length > 0) {
                            listItems.push('<option value="0">---Select---</option>')
                            for (var i = 0; i < list.length; i++) {

                                var id = '';
                                var name = '';
                                id = list[i].split('|')[1];
                                name = list[i].split('|')[0];

                                listItems.push('<option value="' +
                                id + '">' + name
                                + '</option>');

                               
                            }



                            $(lBox).append(listItems.join(''));
                            ListActivityType();

                            $('#ListBoxVehRegNo').trigger("chosen:updated");
                            $('#ListBoxVehRegNo').prop('disabled', false).trigger("chosen:updated");
                            setWithFromActivityType();
                            //alert("succ. completed");

                        }
                        else {
                            lBox.empty();
                           
                            $('#ListBoxVehRegNo').trigger("chosen:updated");
                            $('#ListBoxVehRegNo').prop('disabled', true).trigger("chosen:updated");

                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        //  alert(textStatus);
                    }
                });


            }



            function setWithFromActivityType() {
                var lstActivityType = document.getElementById("ListBoxVehRegNo");
                var listValue = document.getElementById("hdnEditRegNo").value;
                var str_array = listValue.split(',');
                for (var i = 0; i < lstActivityType.options.length; i++) {
                    var selectedValue = lstActivityType.options[i].value;
                    if (str_array.indexOf(selectedValue) > -1) {
                        lstActivityType.options[i].selected = true;                       
                    }
                    else {
                        lstActivityType.options[i].selected = false;
                    }
                }

                if (str_array != "") {

                    $('#ListBoxVehRegNo').prop('disabled', false).trigger("chosen:updated");
                }
            }



            function ListActivityType() {
                //alert("ListActivityType()");
                $('#ListBoxVehRegNo').chosen();
                $('#ListBoxVehRegNo').fadeIn();

                var config = {
                    '.chsnProduct': {},
                    '.chsnProduct-deselect': { allow_single_deselect: true },
                    '.chsnProduct-no-single': { disable_search_threshold: 10 },
                    '.chsnProduct-no-results': { no_results_text: 'Oops, nothing found!' },
                    '.chsnProduct-width': { width: "100%" }
                }
                for (var selector in config) {
                    $(selector).chosen(config[selector]);
                }
            }



        </script>
    <style type="text/css">
        .mb0 {
            margin-bottom:0px !important;
        }
        .validclass{

            position:absolute;
            right:-17px;
            top:8px;

        }
        #lstIndustry + .chosen-container .chosen-results {
            max-height:150px !important;
        }

  
  #dvLoading
        {
            background: url(loader.gif) no-repeat center center;
            height: 100px;
            width: 100px;
            position: fixed;
            z-index: 1000;
            left: 50%;
            top: 50%;
            margin: -25px 0 0 -25px;
        }        
        
  /*Rev 1.0*/

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

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
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

        .mt-27
        {
            margin-top: 27px !important;
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
        <!-- Kallol 150517 -->

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
        
            <h3><asp:Label ID="lblAddEdit" runat="server" Text=""></asp:Label></h3>
            <div class="crossBtn"><a href="frm_drivers_master.aspx"><i class="fa fa-times"></i></a></div>
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
                                                <label>Driver Name <span style="color:red">*</span></label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtdrivername" runat="server"  ClientIDMode="Static" Width="100%" 
                                                        MaxLength="150" >
                                                                   
                                                    </asp:TextBox>

                                                      <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator1" ControlToValidate="txtdrivername"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="drivegrp" >                                                        
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <label>Unique ID <span style="color:red">*</span></label>
                                                <div class="relative">
                                                    <asp:TextBox ID="txtCode" runat="server"  ClientIDMode="Static" Width="100%" MaxLength="80" onblur="fn_ctxtPro_Name_TextChanged()"></asp:TextBox>
                                                    <asp:RequiredFieldValidator Display="Dynamic" runat="server" ID="RequiredFieldValidator2" ControlToValidate="txtCode"
                                                        SetFocusOnError="true" ErrorMessage="" class="pullrightClass fa fa-exclamation-circle abs iconRed" ToolTip="Mandatory" ValidationGroup="drivegrp" >                                                        
                                                    </asp:RequiredFieldValidator>
                                                 </div>
                                            </div>

                                            <div class="col-md-3">
                                                <label>Branch </label>
                                                <%--Rev 1.0: "simple-select" class add --%>
                                                <div class="relative simple-select">
                                                    <asp:DropDownList ID="cmbParentBranch" runat="server" Width="100%"
                                                        >
                                                    </asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="col-md-3"></div>
                                            <div class="clear"></div>

                                            <div class="col-md-3">
                                                <label>Is Active?</label>
                                                <div>
                                                    <dxe:ASPxCheckBox ID="chkIsActive" runat="server" Text="">
                                                    </dxe:ASPxCheckBox>
                                                </div>
                                            </div>

                                            <!-- Kallol 150517 -->
                                            <div class="col-md-6">
                                                <label>Vehicle No. <span style="color:red">*</span></label>
                                                <div style="padding-top: 4px;"> </div>
                                                <div class="relative">
                                                    <asp:ListBox ID="ListBoxVehRegNo"   runat="server" SelectionMode="Single"  Font-Size="12px" Height="90px" Width="253px" CssClass="mb0 chsnProduct  hide" data-placeholder="---Select---" ></asp:ListBox>
                                                    <asp:HiddenField ID="hdnVehRegNo" runat="server" />
                                                    <asp:HiddenField ID="hdnVehRegNoText" runat="server" />
                                                    <asp:HiddenField ID="hdnEditRegNo" runat="server" />   
                                                    <span id="MandatoryVehRegNo" style="display:none" class="validclass"><img id="3gridHistory_DXPEForm_efnew_DXEFL_DXEditor1112_EI" class="dxEditors_edtError_PlasticBlue" src="/DXR.axd?r=1_36-tyKfc" title="Mandatory"></span>
                                                 </div>
                                            </div>
                                            <!-- Kallol 150517 -->

                                            <div class="col-md-3"></div>
                                            <div class="col-md-3"></div>
                                            <div class="clear"></div>



                                            <div class="col-md-9">
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
                                           

                                                <asp:Button ID="btnUdf" runat="server" Text="UDF"  CssClass="btn btn-primary dxbButton"  OnClientClick="if(OpenUdf()){ return false;}"
                                                     />

                                                <%--  <% } %>--%>
                                            </div>
                                        </div>

                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            
                             <dxe:TabPage Name="Correspondence" Text="Correspondence">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Documents" Text="Documents">
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

                    <asp:SqlDataSource ID="sqlCompany" runat="server"
                        SelectCommand="select cmp_internalId,cmp_name from tbl_master_company"></asp:SqlDataSource>

                    <asp:SqlDataSource ID="SqlExchange" runat="server" 
                        SelectCommand="select exch_internalId,(select exh_shortName from tbl_master_exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+ exch_segmentId as Exchange from tbl_master_companyExchange where exch_compId=@CompID">
                        <SelectParameters>
                            <asp:SessionParameter Name="CompID" SessionField="ID" Type="string" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                    <asp:SqlDataSource ID="SqlParentTerminal" runat="server" 
                        SelectCommand="select distinct TradingTerminal_TerminalID from Master_TradingTerminal"></asp:SqlDataSource>

                    <asp:SqlDataSource ID="SqlVendor" runat="server" 
                        SelectCommand="select CTCLVendor_ID,CTCLVendor_Name+' ['+CTCLVendor_ProductType+']' as CTCLVendor_Name from Master_CTCLVendor where CTCLVendor_ExchangeSegment=@CompID1">
                        <SelectParameters>
                            <asp:SessionParameter Name="CompID1" SessionField="ID1" Type="string" />
                        </SelectParameters>
                    </asp:SqlDataSource>

                    <asp:SqlDataSource ID="TrdTerminal" runat="server" 
                        SelectCommand="select td.TradingTerminal_ID,e.exh_shortName+'-'+ce.exch_segmentId as Exchange,td.TradingTerminal_TerminalID,td.TradingTerminal_ParentTerminalID,td.TradingTerminal_ProTradeID,td.TradingTerminal_brokerid ,(select isnull(cnt_firstName,'') +' '+isnull(cnt_middleName,'')+' '+isnull(cnt_lastName,'')+ ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' from tbl_master_contact where cnt_internalId=td.TradingTerminal_BrokerID) as brokid,(Select  ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName from tbl_master_contact WHERE  cnt_internalid =td.TradingTerminal_ProTradeID) as ProTradeID,(Select  ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName from tbl_master_contact WHERE  cnt_internalid =td.TradingTerminal_CliTradeID) as CliTradeID,(Select  ISNULL(ltrim(rtrim(cnt_firstName)), '') + ' ' + ISNULL(ltrim(rtrim(cnt_middleName)), '')   + ' ' + ISNULL(ltrim(rtrim(cnt_lastName)), '') + ' [' + isnull(ltrim(rtrim(cnt_UCC)),'') + '] ' AS cnt_firstName from tbl_master_contact WHERE  cnt_internalid =td.TradingTerminal_AllTradeID) as AllTradeID from  Master_TradingTerminal td, tbl_master_exchange e, tbl_master_companyExchange ce where td.TradingTerminal_CompanyID=ce.exch_compId  and td.TradingTerminal_ExchangeSegmentID=ce.exch_InternalId and  e.exh_cntId=ce.exch_exchId and td.TradingTerminal_BranchID=@BranchID order by TradingTerminal_ID desc"
                        DeleteCommand="delete from Master_TradingTerminal where TradingTerminal_ID=@TradingTerminal_ID">
                        <SelectParameters>
                            <asp:SessionParameter Name="BranchID" SessionField="KeyVal_InternalID" Type="string" />
                        </SelectParameters>
                        <DeleteParameters>
                            <asp:Parameter Name="TradingTerminal_ID" Type="Int32" />
                        </DeleteParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>
    </div>
    </div>
</asp:Content>
