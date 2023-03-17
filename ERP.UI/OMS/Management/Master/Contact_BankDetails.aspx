<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                16-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="True" Title="Bank"
    Inherits="ERP.OMS.Management.Master.management_Master_Contact_BankDetails" MasterPageFile="~/OMS/MasterPage/Erp.Master" CodeBehind="Contact_BankDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #lblCategoryErrorMsg {
            position: absolute;
            right: 150px;
            top: 46px;
        }

        #BankDetailsGrid_DXPEForm_DXEditingErrorRow {
            display: none;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function BindValue() {
            $("#hddnEditMode").val(gridBank.cpEditId);
        }
        function BindAccountNo(s, e) {
            // var EmployeeBanID = Session["EditMode"];
            // if (EmployeeBanID == "")
            //  {
            var SearchBy = combo.GetValue();
            var Bankdetails = cAspxBankCombo.GetValue();
            var accountNo = Bankdetails.split("~")[3];
            // ctxtAccountNo.SetValue(accountNo);
            ctxtIFSCcode.SetValue(Bankdetails.split("~")[4]);
            //  }           
        }
        var v = '0';
        function RefreshPage() {

            setTimeout(RefreshWork(), 3000);
            //RefreshWork();
        }
        function RefreshWork() {
            jAlert('Data Updated Successfully');
            document.location.href = "Contact_BankDetails.aspx";
        }
        function OldState() {

            document.location.href = "Contact_BankDetails.aspx";
        }

        function UpdateEdits() {
            //  alert("xx");
            ExtraFields();
            gridBank.PerformCallback('updateExtra');
        }
        function callAjax(obj, obj1, obj2, obj3) {
            var o = document.getElementById("SearchCombo")
            ajax_showOptions(obj, obj1, obj2, o.value)
        }
        function chkAct(str12, str) {
            var str = document.getElementById(str)
            str.value = str12;
        }

        function validate(key) {
            //getting key code of pressed key
            var keycode = (key.which) ? key.which : key.keyCode;
            var phn = document.getElementById('txtPhn');
            //comparing pressed keycodes
            if (!(keycode == 8 || keycode == 46) && (keycode < 48 || keycode > 57)) {
                return false;
            }
            else {
                //Condition to check textbox contains ten numbers or not
                if (phn.value.length < 10) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        function disp_prompt(name) {
            //var ID = document.getElementById(txtID);
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Contact_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "Contact_Correspondence.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                //document.location.href="Employee_BankDetails.aspx"; 
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "Contact_DPDetails.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "Contact_Document.aspx";
            }
            else if (name == "tab12") {
                //alert(name);
                document.location.href = "Contact_FamilyMembers.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "Contact_Registration.aspx";
            }
            else if (name == "tab7") {
                //alert(name);
                document.location.href = "Contact_GroupMember.aspx";
            }
            else if (name == "tab8") {
                //alert(name);
                document.location.href = "Contact_Deposit.aspx";
            }
            else if (name == "tab9") {
                //alert(name);
                document.location.href = "Contact_Remarks.aspx";
            }
            else if (name == "tab10") {
                //alert(name);
                document.location.href = "Contact_Education.aspx";
            }
            else if (name == "tab11") {
                //alert(name);
                document.location.href = "contact_brokerage.aspx";
            }
            else if (name == "tab6") {
                //alert(name);
                document.location.href = "contact_other.aspx";
            }
            else if (name == "tab13") {
                document.location.href = "contact_Subscription.aspx";
            }
            else if (name == "tab14") {
                document.location.href = "Contact_tds.aspx";
            }
            else if (name == "tab15") {
                document.location.href = "Contact_Person.aspx";
            }
        }
        function CallList(obj1, obj2, obj3) {
            if (obj1.value == "") {
                obj1.value = "%";
            }
            var obj4 = '';
            // alert(valuse);
            if (valuse == 0)
                obj4 = 'bnk_bankName';
            if (valuse == 1)
                obj4 = 'bnk_Micrno';
            if (valuse == 2)
                obj4 = 'bnk_branchName';
            //alert(obj4);


            //ajax_showOptions(obj1, obj2, obj3, obj4);
            ajax_showOptionsTEST(obj1, obj2, obj3, obj4);
            if (obj1.value == "%") {
                obj1.value = "";
            }
        }
        function setvaluetovariable(obj1) {



            valuse = obj1;
        }
        valuse = '0';
        FieldName = 'ASPxPageControl1_txtequity';

        function DeleteRow(keyValue) {
            //doIt = confirm('Confirm delete?');
            //if (doIt) {
            //    gridBank.PerformCallback('Delete~' + keyValue);
            //    height();
            //}
            //else {

            //}
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    gridBank.PerformCallback('Delete~' + keyValue);
                    height();
                }
            });


        }
        function Emailcheck(obj, obj2) {
            // alert("a");
            if (obj == 'N') {
                if (obj != 'B') {
                    jAlert("Transactions exists for this Bank Account... Deletion disallowed!!");
                    obj = 'B';
                }
            }

            if (obj2 != 'Y') {

                INR = confirm('Warning!!.\n\nThis Bank and Account Number already assigned to  ' + obj2 + '.\n\nClick OK to Accept,Otherwise Click Cancel');
                if (INR) {


                    WAR2 = confirm('Warning!!.\n\nThis Bank and Account Number already assigned to  ' + obj2 + '.\n\nClick OK to Accept,Otherwise Click Cancel');
                    if (WAR2) {


                        WAR3 = confirm('Warning!!.\n\nThis Bank and Account Number already assigned to  ' + obj2 + '.\n\nClick OK to Accept,Otherwise Click Cancel');
                        if (WAR3) {
                            jAlert('Your Bank and Account Number has been accepted.')

                        }
                        else {
                            obj = 'DeleteCurrentID';
                            gridBank.PerformCallback(obj);
                        }
                    }
                    else {
                        obj = 'DeleteCurrentID';
                        gridBank.PerformCallback(obj);
                    }


                }
                else {
                    obj = 'DeleteCurrentID';
                    gridBank.PerformCallback(obj);
                }
            }

        }


        function MaskMoney(evt) {
            if (!(evt.keyCode == 46 || (evt.keyCode >= 48 && evt.keyCode <= 57))) return false;
            var parts = evt.srcElement.value.split('.');
            if (parts.length > 2) return false;
            if (evt.keyCode == 46) return (parts.length == 1);
            if (parts[0].length >= 14) return false;
            if (parts.length == 2 && parts[1].length >= 2) return false;
        }


        //----------Update Status 


        function btnSave_Click() {
            //  alert("555");
            var obj = 'SaveOld~' + RowID;
            popPanel.PerformCallback(obj);

        }

        function OnAddEditClick(e, obj) {
            var data = obj.split('~');
            if (data.length > 1)
                RowID = data[1];
            popup.Show();
            popPanel.PerformCallback(obj);
        }
        function EndCallBack(obj) {
            //alert("test");
            if (obj == 'Y') {
                popup.Hide();
                jAlert("Successfully Update!..");
                gridBank.PerformCallback('GridBind');

            }


        }
        function btnCancel_Click() {
            popup.Hide();
        }
        function IsPOA() {
            try {

                document.getElementById("hdnIsPOA").value = document.getElementById("ASPxPageControl1_ASPxPageControl2_BankDetailsGrid_efnew_ddlPOA").value;
            }
            catch (err) {
                // document.getElementById("hdnIsPOA").value= document.getElementById("ASPxPageControl1_ASPxPageControl2_BankDetailsGrid_ef0_ddlPOA").value ;
                // alert(err.message+"yyy");
            }
        }
        function ExtraFields() {
            //  alert("1a");

            try {
                //alert("1");
                // alert(document.getElementById("ASPxPageControl1_ASPxPageControl2_BankDetailsGrid_efnew_txtPOA").value) ;
                // alert(document.getElementById("ASPxPageControl1_ASPxPageControl2_BankDetailsGrid_efnew_ddlPOA").value) ;

                // document.getElementById("hdnPOA").value=document.getElementById("ASPxPageControl1_ASPxPageControl2_BankDetailsGrid_efnew_txtPOA").value;

            }
            catch (err) {
                document.getElementById("hdnPOA").value = document.getElementById("ASPxPageControl1_ASPxPageControl2_BankDetailsGrid_ef0_txtPOA").value;
                //  alert(err.message+"xxx");
            }
            IsPOA();
            // alert(document.getElementById("hdnPOA").value);
        }
        //    function setInitialValues()
        //    {
        //        try 
        //        {
        //              document.getElementById("hdnPOA").value="";
        //             document.getElementById("hdnIsPOA").value=""; 
        //        }
        //         catch(err) {
        //                   alert(err.message);
        //                 }
        //    }
        function keyVal(obj) {
            //alert(obj);
        }

        $(document).ready(function () {
            var mod = '<%= Session["Contactrequesttype"] %>';
            if (mod == 'customer') {
                document.getElementById("lnkClose").href = 'CustomerMasterList.aspx';
            }
            else if (mod == 'Transporter') {
                document.getElementById("lnkClose").href = 'TransporterMasterList.aspx?requesttype=<%= Session["Contactrequesttype"] %>';
            }
            else {
                document.getElementById("lnkClose").href = 'frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>';
            }

        });
    </script>
    <style>
        .dxgvEditForm_PlasticBlue {
            background-color: rgb(237,243,244) !important;
        }

        .bghgnTble > tbody > tr > td {
            padding: 5px 5px;
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
            right: 20px;
            z-index: 0;
            cursor: pointer;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img
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
        select.btn
        {
            padding-right: 10px !important;
        }

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

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid
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

        select.btn
        {
           position: relative;
           z-index: 0;
        }

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
            <%-- <h3>Contact Bank List</h3>--%>
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server"></asp:Label>
            </h3>
            <%--    <div class="crossBtn"><a href="frmContactMain.aspx?requesttype=<%= Session["Contactrequesttype"] %>""><i class="fa fa-times"></i></a></div>--%>
            <div class="crossBtn"><a id="lnkClose"><i class="fa fa-times"></i></a></div>
        </div>

    </div>
        <div class="form_main">

        <table width="100%">
            <tr>
                <td class="EHEADER" style="text-align: center">
                    <asp:Label ID="lblName" runat="server" Font-Bold="True"></asp:Label>
                </td>
            </tr>
        </table>
        <table class="TableMain100">
            <tr>
                <td>

                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="2" ClientInstanceName="page" Width="100%">
                        <TabPages>
                            <dxe:TabPage Name="General" Text="General">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="CorresPondence" Text="Correspondence">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Bank" Text="Bank">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <dxe:ASPxPageControl runat="server" Width="100%" ActiveTabIndex="0" ID="ASPxPageControl2"
                                            ClientInstanceName="page">
                                            <TabPages>
                                                <dxe:TabPage Name="Bank" Text="Bank Details">

                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">
                                                            <asp:Label runat="server" Font-Bold="True" ForeColor="Red" ID="lblmessage" __designer:wfdid="w39"></asp:Label>
                                                            <div style="float: left;">
                                                                <% if (rights.CanAdd)
                                                                   { %>
                                                                <a href="javascript:void(0);" onclick="gridBank.AddNewRow();" class="btn btn-primary"><span>Add New</span> </a>
                                                                <% } %>
                                                            </div>
                                                            <div class="pull-left">
                                                                <% if (rights.CanExport)
                                                                   { %>
                                                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                                                    <asp:ListItem Value="4">CSV</asp:ListItem>

                                                                </asp:DropDownList>
                                                                <% } %>
                                                            </div>

                                                            <dxe:ASPxGridView runat="server" ClientInstanceName="gridBank" KeyFieldName="Id" DataSourceID="BankDetails"
                                                                AutoGenerateColumns="False" Width="100%" Font-Size="12px" ID="BankDetailsGrid" __designer:wfdid="w40"
                                                                OnRowUpdated="BankDetailsGrid_RowUpdated1"
                                                                OnRowDeleting="BankDetailsGrid_RowDeleting"
                                                                OnCustomJSProperties="BankDetailsGrid_CustomJSProperties"
                                                                OnCustomCallback="BankDetailsGrid_CustomCallback" OnRowInserting="BankDetailsGrid_RowInserting" OnRowValidating="BankDetailsGrid_RowValidating"
                                                                OnRowUpdating="BankDetailsGrid_RowUpdating" OnRowDeleted="BankDetailsGrid_RowDeleted" OnRowInserted="BankDetailsGrid_RowInserted"
                                                                OnPreRender="BankDetailsGrid_PreRender" OnUnload="BankDetailsGrid_Unload"
                                                                OnStartRowEditing="BankDetailsGrid_StartRowEditing"
                                                                OnCommandButtonInitialize="gridBank_CommandButtonInitialize">
                                                                <ClientSideEvents EndCallback="function(s, e) {
Emailcheck(s.cpHeight,s.cpWidth);
}"></ClientSideEvents>
                                                                <SettingsSearchPanel Visible="True" />
                                                                <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />
                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn FieldName="Id" Caption="Type" Visible="False" VisibleIndex="0">
                                                                        <EditFormSettings Visible="False" Caption="ID"></EditFormSettings>

                                                                        <CellStyle CssClass="gridcellleft"></CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Category" Caption="Category" VisibleIndex="0">
                                                                        <EditFormSettings Visible="False" Caption="Category"></EditFormSettings>

                                                                        <CellStyle CssClass="gridcellleft"></CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="AccountType" Caption="Account Type" VisibleIndex="1">
                                                                        <EditFormSettings Visible="False" Caption="AccountType"></EditFormSettings>

                                                                        <CellStyle CssClass="gridcellleft"></CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="BankName" Caption="Bank Name" VisibleIndex="2">
                                                                        <EditFormSettings Visible="False" Caption="BankName"></EditFormSettings>

                                                                        <CellStyle CssClass="gridcellleft"></CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Branch" Caption="Branch" VisibleIndex="3">
                                                                        <EditFormSettings Visible="False" Caption="Branch"></EditFormSettings>

                                                                        <CellStyle CssClass="gridcellleft"></CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="MICR" Caption="MICR" VisibleIndex="4">
                                                                        <EditFormSettings Visible="False" Caption="MICR"></EditFormSettings>

                                                                        <CellStyle CssClass="gridcellleft"></CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="IFSCcode" Caption="IFSC Code" VisibleIndex="5">
                                                                        <EditFormSettings Visible="False" Caption="MICR"></EditFormSettings>

                                                                        <CellStyle CssClass="gridcellleft"></CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn FieldName="Category" Caption="Category" Visible="False" VisibleIndex="0">
                                                                        <PropertiesComboBox ValueType="System.String"></PropertiesComboBox>

                                                                        <EditFormSettings Visible="True"></EditFormSettings>

                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False"></EditFormCaptionStyle>

                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataComboBoxColumn FieldName="AccountType" Caption="Account Type" Visible="False" VisibleIndex="0">
                                                                        <PropertiesComboBox ValueType="System.String"></PropertiesComboBox>

                                                                        <EditFormSettings Visible="True"></EditFormSettings>

                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False"></EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="AccountNumber" Caption="Account Number" VisibleIndex="6">
                                                                        <EditFormSettings Visible="True" Caption="Account Number"></EditFormSettings>

                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False"></EditFormCaptionStyle>

                                                                        <CellStyle CssClass="gridcellleft"></CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="AccountName" Caption="Account Name" VisibleIndex="7">
                                                                        <EditFormSettings Visible="True" Caption="AccountName"></EditFormSettings>

                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False"></EditFormCaptionStyle>

                                                                        <CellStyle CssClass="gridcellleft"></CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="IFSCcode" Caption="IFSC Code" VisibleIndex="8" Visible="False"
                                                                        Width="12%">
                                                                        <EditFormSettings Caption="IFSC Code" Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <%--<dxe:GridViewDataTextColumn FieldName="IsPOA" Caption="POA" VisibleIndex="8">
                                                                    <EditFormSettings Visible="True" Caption="POA"></EditFormSettings>

                                                                    <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False"></EditFormCaptionStyle>

                                                                    <CellStyle CssClass="gridcellleft"></CellStyle>
                                                                </dxe:GridViewDataTextColumn>--%>
                                                                    <dxe:GridViewDataComboBoxColumn FieldName="IsPOAdr" Width="200px" VisibleIndex="9" Caption="POA">
                                                                        <%--<PropertiesComboBox ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="False">
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Yes" Value="Yes"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="No" Value="No"></dxe:ListEditItem>
                                                                            </Items>

                                                                        </PropertiesComboBox>--%>
                                                                        <PropertiesComboBox ValueType="System.String"></PropertiesComboBox>
                                                                    </dxe:GridViewDataComboBoxColumn>

                                                                    <dxe:GridViewDataTextColumn FieldName="POAName" Caption="POA Name" VisibleIndex="10">
                                                                        <EditFormSettings Visible="True" Caption="POA Name"></EditFormSettings>

                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False"></EditFormCaptionStyle>

                                                                        <CellStyle CssClass="gridcellleft"></CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="BankName" Caption="Bank Name" Visible="False" VisibleIndex="0">
                                                                        <EditFormSettings Visible="True" Caption="BankName"></EditFormSettings>

                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False"></EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="status" VisibleIndex="11">
                                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                                        <DataItemTemplate>
                                                                            <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Edit~'+'<%# Container.KeyValue %>')">
                                                                                <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='<%# Eval("status")%>' Width="100%" ToolTip="Click to Change Status">
                                                                                </dxe:ASPxLabel>

                                                                            </a>

                                                                        </DataItemTemplate>

                                                                        <HeaderStyle Wrap="False"></HeaderStyle>

                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                        <HeaderTemplate>
                                                                            Status                                                         
                                                    
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewCommandColumn VisibleIndex="11" ShowEditButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="5%">

                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                        <CellStyle HorizontalAlign="Center"></CellStyle>

                                                                        <HeaderTemplate>
                                                                            <%-- <% if (rights.CanAdd)
                                                                                     { %>--%>
                                                                            <%--<a href="javascript:void(0);" onclick="gridBank.AddNewRow();"><span >Add New</span> </a>--%>
                                                                            <%--  <% } %>--%>
                                                                            <span>Edit</span>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>
                                                                    <dxe:GridViewDataTextColumn Width="5%" VisibleIndex="12" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                        <EditFormSettings Visible="False"></EditFormSettings>
                                                                        <DataItemTemplate>
                                                                            <% if (rights.CanDelete)
                                                                               { %>
                                                                            <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" title="Delete">
                                                                                <img src="../../../assests/images/Delete.png" />
                                                                            </a>
                                                                            <% } %>
                                                                        </DataItemTemplate>

                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                        <CellStyle Wrap="False"></CellStyle>
                                                                        <HeaderTemplate>
                                                                            <span>Delete</span>

                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewDataTextColumn>
                                                                </Columns>
                                                                <SettingsCommandButton>

                                                                    <EditButton ButtonType="Image" Image-Url="../../../assests/images/Edit.png">
                                                                        <Image Url="../../../assests/images/Edit.png"></Image>
                                                                    </EditButton>
                                                                </SettingsCommandButton>
                                                                <SettingsBehavior ConfirmDelete="True"></SettingsBehavior>

                                                                <SettingsPager PageSize="20" NumericButtonCount="20" ShowSeparators="True">
                                                                    <FirstPageButton Visible="True"></FirstPageButton>

                                                                    <LastPageButton Visible="True"></LastPageButton>
                                                                </SettingsPager>

                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormWidth="600px" PopupEditFormHeight="370px" PopupEditFormHorizontalAlign="Center" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormModal="True" EditFormColumnCount="1"></SettingsEditing>

                                                                <Settings ShowTitlePanel="True" ShowStatusBar="Visible" ShowGroupPanel="true"></Settings>

                                                                <SettingsText ConfirmDelete="Confirm delete?" PopupEditFormCaption="Add/Modify Bank Details"></SettingsText>

                                                                <Styles>
                                                                    <Header SortingImageSpacing="5px" ImageSpacing="5px" CssClass="EHEADER"></Header>

                                                                    <AlternatingRow BackColor="AliceBlue" Font-Bold="True"></AlternatingRow>

                                                                    <LoadingPanel ImageSpacing="10px"></LoadingPanel>
                                                                </Styles>

                                                                <Templates>
                                                                    <TitlePanel>
                                                                        <span style="color: Black; font-size: 12px">Bank Details</span>


                                                                    </TitlePanel>
                                                                    <EditForm>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="text-align: center;">
                                                                                    <table style="margin-left: 125px;" class="bghgnTble">
                                                                                        <tr>
                                                                                            <td style="text-align: left; padding-left: 10px">Category:<span style="font-size: 7.5pt; color: red"><strong>*</strong></span></td>
                                                                                            <td colspan="2">
                                                                                                <dxe:ASPxComboBox ID="drpCategory" runat="server" ValueType="System.String" Width="200px"
                                                                                                    Value='<%#Bind("Category") %>' SelectedIndex="0">
                                                                                                    <Items>
                                                                                                        <dxe:ListEditItem Text="Default" Value="Default" />
                                                                                                        <dxe:ListEditItem Text="Secondary" Value="Secondary" />
                                                                                                    </Items>
                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                                                                                        <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                                    </ValidationSettings>
                                                                                                </dxe:ASPxComboBox>
                                                                                                <%-- Comment by sanjib 20122016--%>
                                                                                                <%--<asp:Label ID="lblCategoryErrorMsg" runat="server" Text="Default Category already exists" ForeColor="red" Visible="false"></asp:Label>--%>
                                                                                                <asp:Label ID="lblCategoryErrorMsg" runat="server" class="dxEditors_edtError_PlasticBlue" ForeColor="red" Visible="false"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="text-align: left; padding-left: 10px">Account Type:<span style="font-size: 7.5pt; color: red"><strong>*</strong></span></td>
                                                                                            <td class="lt" colspan="2">
                                                                                                <dxe:ASPxComboBox ID="drpAccountType" runat="server" ValueType="System.String" Value='<%#Bind("AccountType") %>'
                                                                                                    Width="200px" SelectedIndex="0">
                                                                                                    <Items>
                                                                                                        <dxe:ListEditItem Text="Saving" Value="Saving" />
                                                                                                        <dxe:ListEditItem Text="Current" Value="Current" />
                                                                                                        <dxe:ListEditItem Text="Joint" Value="Joint" />
                                                                                                    </Items>
                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">
                                                                                                        <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                                    </ValidationSettings>
                                                                                                </dxe:ASPxComboBox>
                                                                                                <%--  <asp:Label ID="lblAcTypeErrorMsg" runat="server" ></asp:Label>--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="text-align: left; padding-left: 10px">Search By:</td>
                                                                                            <td>
                                                                                                <dxe:ASPxComboBox ID="drpSearchBank" runat="server" ValueType="System.String" SelectedIndex="0"
                                                                                                    ClientInstanceName="combo" Width="200px">
                                                                                                    <Items>
                                                                                                        <dxe:ListEditItem Text="Bank Name" Value="bnk_bankName" />
                                                                                                        <dxe:ListEditItem Text="MICR No" Value="bnk_Micrno" />
                                                                                                        <dxe:ListEditItem Text="Branch Name" Value="bnk_branchName" />
                                                                                                    </Items>
                                                                                                    <ClientSideEvents ValueChanged="function(s,e){
                                                                                                    var indexr = s.GetSelectedIndex();
                                                                                                         cAspxBankCombo.PerformCallback(indexr);
                                                                                                    setvaluetovariable(indexr)
                                                                                                    }"
                                                                                                        Init="function(s, e) {
                                                                                            cAspxBankCombo.PerformCallback('0');
                                                                                    }" />
                                                                                                </dxe:ASPxComboBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="text-align: left; padding-left: 10px">Bank Name:<span style="font-size: 7.5pt; color: red"><strong>*</strong></span></td>
                                                                                            <td class="lt">

                                                                                                <%--  <asp:TextBox ID="txtbankname" runat="server" Width="200px" Text='<%#Bind("BankName1") %>'></asp:TextBox>                                                                                              
                                                                                                <asp:HiddenField ID="txtbankname_hidden" runat="server" />--%>

                                                                                                <dxe:ASPxComboBox ID="AspxBankCombo" runat="server" ValueType="System.String" SelectedIndex="-1" ValueField="id" TextField="name"
                                                                                                    Value='<%#Bind("BankName1") %>' ClientInstanceName="cAspxBankCombo" Width="203px" OnCallback="AspxBankCombo_CustomCallback">

                                                                                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                                        <RequiredField ErrorText="Mandatory" IsRequired="True" />
                                                                                                    </ValidationSettings>
                                                                                                    <ClientSideEvents
                                                                                                        ValueChanged="BindAccountNo" EndCallback="BindValue" />
                                                                                                </dxe:ASPxComboBox>

                                                                                            </td>

                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="text-align: left; padding-left: 10px">Account Number:</td>
                                                                                            <td class="lt" colspan="2">

                                                                                                <%-- <asp:TextBox ID="txtAccountNo" runat="server" Text='<%#Bind("AccountNumber") %>'
                                                                                                    Width="200px" MaxLength="50"></asp:TextBox>--%>
                                                                                                <dxe:ASPxTextBox runat="server" ID="txtAccountNo" ClientInstanceName="ctxtAccountNo" MaxLength="50" Text='<%#Bind("AccountNumber") %>' Width="200px">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="text-align: left; padding-left: 10px">Account Name:</td>
                                                                                            <td class="lt" colspan="2">
                                                                                                <asp:TextBox ID="txtAnccountName" runat="server" Text='<%#Bind("AccountName") %>'
                                                                                                    Width="200px" MaxLength="50"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="text-align: left; padding-left: 10px;">IFSC Code:</td>
                                                                                            <td class="lt" colspan="2">
                                                                                                <dxe:ASPxTextBox runat="server" ID="txtIFSCcode" ClientInstanceName="ctxtIFSCcode" MaxLength="50" Text='<%#Bind("IFSCcode") %>' Width="200px" ClientEnabled="false">
                                                                                                </dxe:ASPxTextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="text-align: left; padding-left: 10px">POA:
                                                                                            </td>
                                                                                            <td class="lt" colspan="2">
                                                                                                <asp:DropDownList ID="ddlPOA" runat="server" Visible="false">
                                                                                                    <asp:ListItem Value="1">YES</asp:ListItem>
                                                                                                    <asp:ListItem Value="0" Selected="True">NO</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                                <dxe:ASPxComboBox ID="comboPOA" EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                                                    runat="server" ValueType="System.String" Width="200px" Value='<%#Bind("IsPOAdr") %>'>
                                                                                                    <Items>
                                                                                                        <dxe:ListEditItem Text="Yes" Value="Yes" />
                                                                                                        <dxe:ListEditItem Text="No" Value="No" />
                                                                                                    </Items>

                                                                                                    <ButtonStyle Width="13px">
                                                                                                    </ButtonStyle>
                                                                                                </dxe:ASPxComboBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="text-align: left; padding-left: 10px">POAName:
                                                                                            </td>
                                                                                            <td class="lt" colspan="2">
                                                                                                <asp:TextBox ID="txtPOA" runat="server" Text='<%#Bind("POAName") %>'
                                                                                                    Width="200px" MaxLength="100"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" colspan="2" style="padding-left: 113px;">
                                                                                                <dxe:ASPxButton ID="btnUpdate" runat="server" Text="Save" ToolTip="Update data"
                                                                                                    CssClass="btn btn-primary" AutoPostBack="true" OnClick="btnUpdate_Click">
                                                                                                    <%-- <ClientSideEvents Click="function(s, e) {gridBank.UpdateEdit();}" />--%>
                                                                                                    <%-- <ClientSideEvents Click="function(s, e) {UpdateEdit();}" />--%>
                                                                                                </dxe:ASPxButton>
                                                                                                <dxe:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel data"
                                                                                                    AutoPostBack="False" OnClick="btnCancel_Click" CssClass="btn btn-danger">
                                                                                                    <%-- <ClientSideEvents Click="function(s, e) {gridBank.CancelEdit();}" />--%>
                                                                                                </dxe:ASPxButton>

                                                                                            </td>
                                                                                            <td class="lt" colspan="2"></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <asp:Label ID="lblId" runat="server" Text='<%#Bind("Id") %>' Visible="false"></asp:Label>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>

                                                                    </EditForm>
                                                                </Templates>
                                                            </dxe:ASPxGridView>

                                                            <dxe:ASPxPopupControl runat="server" AllowDragging="True" ClientInstanceName="popup" CloseAction="CloseButton" EnableHotTrack="False" HeaderText="Set Account Status" PopupHorizontalAlign="WindowCenter" Width="400px" BackColor="#DDECFE" ID="ASPxPopupControl1" __designer:wfdid="w41">
                                                                <ClientSideEvents CloseButtonClick="function(s, e) {
	 popup.Hide();
}"></ClientSideEvents>

                                                                <CloseButtonImage Height="12px" Width="13px"></CloseButtonImage>

                                                                <SizeGripImage Height="16px" Width="16px"></SizeGripImage>

                                                                <HeaderStyle HorizontalAlign="Left">
                                                                    <Paddings PaddingRight="6px"></Paddings>
                                                                </HeaderStyle>
                                                                <ContentCollection>
                                                                    <dxe:PopupControlContentControl runat="server">
                                                                        <dxe:ASPxCallbackPanel runat="server" ClientInstanceName="popPanel" Width="400px" ID="ASPxCallbackPanel1" __designer:wfdid="w4" OnCustomJSProperties="ASPxCallbackPanel1_CustomJSProperties" OnCallback="ASPxCallbackPanel1_Callback">
                                                                            <ClientSideEvents EndCallback="function(s, e) {
	                                                    EndCallBack(s.cpLast);
                                                    }"></ClientSideEvents>
                                                                            <PanelCollection>
                                                                                <dxe:PanelContent runat="server">
                                                                                    <table>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td>Status: </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList runat="server" Width="100px" ID="cmbStatus" __designer:wfdid="w43" TabIndex="0">
                                                                                                        <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                                                                                        <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>Date: </td>
                                                                                                <td>
                                                                                                    <dxe:ASPxDateEdit runat="server" UseMaskBehavior="True" EditFormat="Custom" Width="99px" ClientInstanceName="StDate" Font-Size="12px" TabIndex="1" ID="StDate" __designer:wfdid="w44">
                                                                                                        <ButtonStyle Width="13px"></ButtonStyle>
                                                                                                    </dxe:ASPxDateEdit>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>Reason: </td>
                                                                                                <td>
                                                                                                    <asp:TextBox runat="server" TextMode="MultiLine" Width="250px" ID="txtReason" __designer:wfdid="w45" TabIndex="3" MaxLength="50"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td></td>
                                                                                                <td class="gridcellleft">
                                                                                                    <input style="width: 60px" id="Button2" class="btnUpdate" tabindex="4" onclick="btnSave_Click()" type="button" value="Save" />
                                                                                                    <input style="width: 60px" id="Button3" class="btnUpdate" tabindex="5" onclick="btnCancel_Click()" type="button" value="Cancel" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </dxe:PanelContent>
                                                                            </PanelCollection>
                                                                        </dxe:ASPxCallbackPanel>
                                                                    </dxe:PopupControlContentControl>
                                                                </ContentCollection>
                                                            </dxe:ASPxPopupControl>

                                                        </dxe:ContentControl>
                                                    </ContentCollection>
                                                </dxe:TabPage>
                                                <dxe:TabPage Name="Investment" Visible="false" Text="Investment">
                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">
                                                            <asp:Panel runat="server" Width="100%" ID="Panel1" __designer:wfdid="w1">
                                                                <table width="100%">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>
                                                                                <table width="100%">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                <table>
                                                                                                    <tbody>
                                                                                                        <tr>
                                                                                                            <td class="mylabel1"><span style="font-size: 8pt">Select Investment: </span></td>
                                                                                                            <td>
                                                                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel2" __designer:wfdid="w2">
                                                                                                                    <ContentTemplate>
                                                                                                                        <asp:DropDownList ID="cmbFinYear" runat="server" Width="200px" __designer:wfdid="w3" AutoPostBack="true" OnSelectedIndexChanged="cmbFinYear_SelectedIndexChanged">
                                                                                                                        </asp:DropDownList>
                                                                                                                    </ContentTemplate>
                                                                                                                    <Triggers>
                                                                                                                        <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click"></asp:AsyncPostBackTrigger>
                                                                                                                    </Triggers>
                                                                                                                </asp:UpdatePanel>







                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </tbody>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UpdatePanel1" __designer:wfdid="w4">
                                                                                    <ContentTemplate>
                                                                                        <table class="TableMain100" width="100%" border="1">
                                                                                            <tbody>
                                                                                                <tr>
                                                                                                    <td style="background-color: #a9d4fa; text-align: center" colspan="2"><span style="font-size: 8pt"><strong>Investment</strong></span> </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td colspan="2">
                                                                                                        <table>
                                                                                                            <tbody>
                                                                                                                <tr>
                                                                                                                    <td class="mylabel1"><span style="font-size: 8pt">Financial Year:</span> </td>
                                                                                                                    <td>
                                                                                                                        <asp:DropDownList ID="drpFinyear" runat="server" Width="200px" __designer:wfdid="w5" OnSelectedIndexChanged="drpFinyear_SelectedIndexChanged" AutoPostBack="true">
                                                                                                                        </asp:DropDownList>
                                                                                                                    </td>
                                                                                                                    <td class="mylabel1"><span style="font-size: 8pt">Effective Date:</span> </td>
                                                                                                                    <td>
                                                                                                                        <dxe:ASPxDateEdit ID="dtEffect" TabIndex="21" runat="server" ClientInstanceName="dtEffect" Width="99px" __designer:wfdid="w6" Font-Size="12px" EditFormat="Custom" UseMaskBehavior="True">
                                                                                                                            <ButtonStyle Width="13px">
                                                                                                                            </ButtonStyle>
                                                                                                                        </dxe:ASPxDateEdit>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </tbody>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 45%" valign="top">
                                                                                                        <table width="100%">
                                                                                                            <tbody>
                                                                                                                <tr>
                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Gross Annual Salary Range </span></td>
                                                                                                                    <td>: </td>
                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">Rs.</span><asp:TextBox ID="txtgrossannualsalary" runat="server" __designer:wfdid="w7" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">To&nbsp;&nbsp;&nbsp;&nbsp;Rs.</span><asp:TextBox ID="txtgrossannualsalary2" runat="server" __designer:wfdid="w8" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="width: 154px; text-align: left" class="mylabel1"><span style="font-size: 6pt">Annual Trunover Range </span></td>
                                                                                                                    <td>:</td>
                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span><asp:TextBox ID="txtannualTrunover" runat="server" __designer:wfdid="w9" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">To&nbsp;&nbsp;&nbsp;&nbsp;Rs.</span><asp:TextBox ID="txtannualTrunover2" runat="server" __designer:wfdid="w10" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="width: 154px; text-align: left" class="mylabel1"><span style="font-size: 6pt">Gross Profit Range </span></td>
                                                                                                                    <td>:</td>
                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span><asp:TextBox ID="txtGrossProfit" runat="server" __designer:wfdid="w11" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">To&nbsp;&nbsp;&nbsp;&nbsp;Rs.</span><asp:TextBox ID="txtGrossProfit2" runat="server" __designer:wfdid="w12" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="width: 154px; text-align: left" class="mylabel1"><span style="font-size: 6pt">Approx. Expenses (PM) Range</span></td>
                                                                                                                    <td>:</td>
                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span><asp:TextBox ID="txtPMExpenses" runat="server" __designer:wfdid="w13" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">To&nbsp;&nbsp;&nbsp;&nbsp;Rs.</span><asp:TextBox ID="txtPMExpenses2" runat="server" __designer:wfdid="w14" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="width: 154px; text-align: left" class="mylabel1"><span style="font-size: 6pt">Approx. Saving (PM) Range </span></td>
                                                                                                                    <td>:</td>
                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span><asp:TextBox ID="txtPMSaving" runat="server" __designer:wfdid="w15" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">To&nbsp;&nbsp;&nbsp;&nbsp;Rs.</span><asp:TextBox ID="txtPMSaving2" runat="server" __designer:wfdid="w16" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="width: 150px; text-align: left" class="mylabel1"><span style="font-size: 6pt">6 Month Bank Blnc Statement</span></td>
                                                                                                                    <td>:</td>
                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt; text-align: left">High</span><asp:TextBox ID="txtbank1" runat="server" __designer:wfdid="w17" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Low</span><asp:TextBox ID="txtbank2" runat="server" __designer:wfdid="w18" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </tbody>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                    <td style="width: 60%">
                                                                                                        <table width="100%">
                                                                                                            <tbody>
                                                                                                                <tr>
                                                                                                                    <td style="width: 260px">
                                                                                                                        <table>
                                                                                                                            <tbody>
                                                                                                                                <tr>
                                                                                                                                    <td style="width: 128px; text-align: left" class="mylabel1"><span style="font-size: 6pt">Annual Income Range</span></td>
                                                                                                                                    <td>:</td>
                                                                                                                                    <td class="mylabel1">
                                                                                                                                        <asp:DropDownList Style="font-size: 10px; width: 130px" ID="ddlIncomeRange" runat="server" __designer:wfdid="w19"></asp:DropDownList>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="width: 118px; text-align: left" class="mylabel1"><span style="font-size: 6pt">Equity</span></td>
                                                                                                                                    <td>:</td>
                                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span><asp:TextBox ID="txtequity" runat="server" __designer:wfdid="w20" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="width: 118px; text-align: left" class="mylabel1"><span style="font-size: 6pt">Mutual Fund</span></td>
                                                                                                                                    <td>:</td>
                                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span><asp:TextBox ID="txtMutalFund" runat="server" __designer:wfdid="w21" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="width: 118px; text-align: left" class="mylabel1"><span style="font-size: 6pt">Bank FD's</span></td>
                                                                                                                                    <td>:</td>
                                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span><asp:TextBox ID="txtBankFD" runat="server" __designer:wfdid="w22" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="width: 118px; text-align: left" class="mylabel1"><span style="font-size: 6pt">Debt's Instruments</span></td>
                                                                                                                                    <td>:</td>
                                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span><asp:TextBox ID="txtDebtsInstruments" runat="server" __designer:wfdid="w23" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="width: 118px; text-align: left" class="mylabel1"><span style="font-size: 6pt">NSS's</span></td>
                                                                                                                                    <td>:</td>
                                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span><asp:TextBox ID="txtNSS" runat="server" __designer:wfdid="w24" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </tbody>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <table>
                                                                                                                            <tbody>
                                                                                                                                <tr>
                                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">Networth</span></td>
                                                                                                                                    <td>:</td>
                                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span>
                                                                                                                                        <asp:TextBox ID="txtNetworth" onkeypress="return validate(event)" runat="server" __designer:wfdid="w25" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">Life Insurance</span></td>
                                                                                                                                    <td>:</td>
                                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span>
                                                                                                                                        <asp:TextBox ID="txtLifeInsurance" runat="server" __designer:wfdid="w26" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">Health Insurance</span></td>
                                                                                                                                    <td>:</td>
                                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span><asp:TextBox ID="txtHealthInsurance" runat="server" __designer:wfdid="w27" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">Real Estate</span></td>
                                                                                                                                    <td>:</td>
                                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span><asp:TextBox ID="txtRealEstate" runat="server" __designer:wfdid="w28" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">Precious Metals/Stones</span></td>
                                                                                                                                    <td>:</td>
                                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span><asp:TextBox ID="txtPreciousMetals" runat="server" __designer:wfdid="w29" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">Other's</span></td>
                                                                                                                                    <td>:</td>
                                                                                                                                    <td class="mylabel1"><span style="font-size: 6pt">Rs.</span><asp:TextBox ID="txtOthers" runat="server" __designer:wfdid="w30" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </tbody>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </tbody>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <table>
                                                                                                            <tbody>
                                                                                                                <tr>
                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">Has Fund For Investment </span></td>
                                                                                                                    <td>:</td>
                                                                                                                    <td style="color: blue">
                                                                                                                        <asp:CheckBox ID="chkHasFundInvestment" runat="server" __designer:wfdid="w31" ForeColor="Blue" Font-Size="12px" OnCheckedChanged="chkHasFundInvestment_CheckedChanged"></asp:CheckBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">If Yes Then Availabe Funds </span></td>
                                                                                                                    <td>:</td>
                                                                                                                    <td class="mylabel1">
                                                                                                                        <asp:TextBox ID="txtAvailableFund" runat="server" __designer:wfdid="w32" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="text-align: left" class="mylabel1"><span style="font-size: 6pt">If Yes Then Investment Horizon </span></td>
                                                                                                                    <td>:</td>
                                                                                                                    <td class="mylabel1">
                                                                                                                        <asp:TextBox ID="txtInvestmentHorizon" runat="server" __designer:wfdid="w33" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </tbody>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <table>
                                                                                                            <tbody>
                                                                                                                <tr>
                                                                                                                    <td>
                                                                                                                        <table>
                                                                                                                            <tbody>
                                                                                                                                <tr>
                                                                                                                                    <td style="width: 225px; text-align: left" class="mylabel1"><span style="font-size: 6pt">Ready to Transfer Existing Portfoilio </span></td>
                                                                                                                                    <td>:</td>
                                                                                                                                    <td class="mylabel1">
                                                                                                                                        <asp:CheckBox ID="chkPortFoilio" runat="server" __designer:wfdid="w34" ForeColor="Blue" Font-Size="12px" OnCheckedChanged="chkPortFoilio_CheckedChanged"></asp:CheckBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="width: 119px; text-align: left" class="mylabel1"><span style="font-size: 6pt">If Yes Then Amount </span></td>
                                                                                                                                    <td>:</td>
                                                                                                                                    <td style="color: black">
                                                                                                                                        <asp:TextBox ID="TxtPortFoilioAmount" runat="server" __designer:wfdid="w35" ForeColor="Black" Font-Size="12px" Width="50px"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </tbody>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        <table>
                                                                                                                            <tbody>
                                                                                                                                <tr>
                                                                                                                                    <td style="width: 138px; text-align: left"><span style="font-size: 6pt" class="mylabel1">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Own House </span></td>
                                                                                                                                    <td style="width: 3px">:</td>
                                                                                                                                    <td class="mylabel1">
                                                                                                                                        <asp:CheckBox ID="chkhouse" runat="server" __designer:wfdid="w36" ForeColor="Black" Font-Size="12px"></asp:CheckBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td style="width: 138px; text-align: left" class="mylabel1"><span style="font-size: 6pt">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; Own Vehicle </span></td>
                                                                                                                                    <td style="width: 3px">:</td>
                                                                                                                                    <td class="mylabel1">
                                                                                                                                        <asp:CheckBox ID="chkVehicle" runat="server" __designer:wfdid="w37" ForeColor="Blue" Font-Size="12px"></asp:CheckBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                            </tbody>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </tbody>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="text-align: left" align="center" colspan="2">
                                                                                                        <asp:Button ID="Button1" OnClick="btn_Finance_Save_Click" runat="server" __designer:wfdid="w38" Text="Save" CssClass="btnUpdate"></asp:Button>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tbody>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                    <Triggers>
                                                                                        <asp:AsyncPostBackTrigger ControlID="cmbFinYear" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                                                                        <asp:AsyncPostBackTrigger ControlID="drpFinyear" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                                                                                    </Triggers>
                                                                                </asp:UpdatePanel>







                                                                            </td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </asp:Panel>







                                                        </dxe:ContentControl>
                                                    </ContentCollection>
                                                </dxe:TabPage>
                                            </TabPages>
                                        </dxe:ASPxPageControl>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="DP Details" Visible="false" Text="DP">

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
                            <dxe:TabPage Name="Registration" Text="Registration">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Other" Visible="false" Text="Other">

                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Group Member" Text="Group">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Deposit" Visible="false" Text="Deposit">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Remarks" Text="UDF" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Education" Visible="false" Text="Education">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Trad. Prof." Visible="false" Text="Trad.Prof">
                                <%--<TabTemplate ><span style="font-size:x-small">Trad.Prof</span>&nbsp;<span style="color:Red;">*</span> </TabTemplate>--%>
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="FamilyMembers" Visible="false" Text="Family">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Subscription" Visible="false" Text="Subscription">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                              <dxe:TabPage Name="TDS" Visible="false" Text="TDS">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Contact Person" Name="ContactPreson">
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
	                                            var Tab3 = page.GetTab(3);
	                                            var Tab4 = page.GetTab(4);
	                                            var Tab5 = page.GetTab(5);
	                                            var Tab6 = page.GetTab(6);
	                                            var Tab7 = page.GetTab(7);
	                                            var Tab8 = page.GetTab(8);
	                                            var Tab9 = page.GetTab(9);
	                                            var Tab10 = page.GetTab(10);
	                                            var Tab11 = page.GetTab(11);
	                                            var Tab12 = page.GetTab(12);
	                                            var Tab13=page.GetTab(13);
	                                              var Tab14 = page.GetTab(14);
	                                                var Tab15=page.GetTab(15);
	                                            if(activeTab == Tab0)
	                                            {
	                                                disp_prompt('tab0');
	                                            }
	                                            if(activeTab == Tab1)
	                                            {
	                                                disp_prompt('tab1');
	                                            }
	                                            else if(activeTab == Tab2)
	                                            {
	                                                disp_prompt('tab2');
	                                            }
	                                            else if(activeTab == Tab3)
	                                            {
	                                                disp_prompt('tab3');
	                                            }
	                                            else if(activeTab == Tab4)
	                                            {
	                                                disp_prompt('tab4');
	                                            }
	                                            else if(activeTab == Tab5)
	                                            {
	                                                disp_prompt('tab5');
	                                            }
	                                            else if(activeTab == Tab6)
	                                            {
	                                                disp_prompt('tab6');
	                                            }
	                                            else if(activeTab == Tab7)
	                                            {
	                                                disp_prompt('tab7');
	                                            }
	                                            else if(activeTab == Tab8)
	                                            {
	                                                disp_prompt('tab8');
	                                            }
	                                            else if(activeTab == Tab9)
	                                            {
	                                                disp_prompt('tab9');
	                                            }
	                                            else if(activeTab == Tab10)
	                                            {
	                                                disp_prompt('tab10');
	                                            }
	                                            else if(activeTab == Tab11)
	                                            {
	                                                disp_prompt('tab11');
	                                            }
	                                            else if(activeTab == Tab12)
	                                            {
	                                                disp_prompt('tab12');
	                                            }
	                                            else if(activeTab == Tab13)
	                                            {
	                                                disp_prompt('tab13');
	                                            }
	                                            else if(activeTab == Tab14)
	                                            {
	                                                disp_prompt('tab14');
	                                            }
	                                            else if(activeTab == Tab15)
	                                            {
	                                                disp_prompt('tab15');
	                                            }
	                                            }"></ClientSideEvents>
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                        <TabStyle Font-Size="12px">
                        </TabStyle>
                    </dxe:ASPxPageControl>
                </td>
                <td></td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlBank" runat="server"></asp:SqlDataSource>
        <%--SelectCommand="BankDetailsSelect"--%>
        <asp:SqlDataSource ID="BankDetails" runat="server"
            InsertCommand="BankDetailsInsert" SelectCommand="BankDetailsSelect"
            SelectCommandType="StoredProcedure"
            OnInserted="BankDetails_Inserted" OnInserting="BankDetails_Inserting" OnUpdated="BankDetails_Updated">
            <SelectParameters>
                <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID_New" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="Category" Type="String" />
                <asp:Parameter Name="BankName1" Type="String" />
                <asp:Parameter Name="AccountNumber" Type="String" />
                <asp:Parameter Name="AccountType" Type="String" />
                <asp:Parameter Name="AccountName" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="String" />
                <asp:Parameter Name="Id" Type="String" />
                <asp:Parameter Name="IFSCcode" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="Category" Type="String" />
                <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID_New" Type="String" />
                <asp:Parameter Name="BankName1" Type="String" />
                <asp:Parameter Name="AccountNumber" Type="String" />
                <asp:Parameter Name="AccountType" Type="String" />
                <asp:Parameter Name="AccountName" Type="String" />
                <asp:Parameter Name="IFSCcode" Type="String" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:Button ID="btnForce" runat="server" Text="Force" OnClick="btnForce_Click" Visible="false" />
        <asp:HiddenField ID="hdnPOA" runat="server" />
        <asp:HiddenField ID="hdnIsPOA" runat="server" />
        <asp:HiddenField ID="hdnRefresh" runat="server" Value="n" />
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
        <asp:HiddenField ID="hddnEditMode" runat="server" />
    </div>
    </div>
</asp:Content>
