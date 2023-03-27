<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                27-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false"
    Inherits="ERP.OMS.Management.Master.management_master_groupmasterPopUp" CodeBehind="groupmasterPopUp.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Data.Linq" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v10.2.Export, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web" tagprefix="dx" %>--%>



    <link href="../../css/choosen.min.css" rel="stylesheet" />
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
  <%--  <link href="http://localhost:49428/DashBoard/css/SearchPopup.css" rel="stylesheet" />
    <script src="http://localhost:49428/DashBoard/Js/SearchPopup.js"></script>--%>
    <link href="../Activities/CSS/SearchPopup.css" rel="stylesheet" />
    <script src="../Activities/JS/SearchPopup.js"></script>
    <style type="text/css">
        /* Big box with list of options */
        #CustModel {
                z-index: 99999;
        }
        #ajax_listOfOptions {
            position: absolute; /* Never change this one */
            width: 50px; /* Width of box */
            height: auto; /* Height of box */
            overflow: auto; /* Scrolling features */
            border: 1px solid Blue; /* Blue border */
            background-color: #FFF; /* White background color */
            text-align: left;
            font-size: 0.9em;
            z-index: 32767;
        }


            #ajax_listOfOptions div { /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv { /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected { /* Selected item in the list */
                background-color: #DDECFE;
                color: Blue;
            }

        #ajax_listOfOptions_iframe {
            background-color: #F00;
            position: absolute;
            z-index: 3000;
        }

        form {
            display: inline;
        }
        
        .chosen-container.chosen-container-single {
            width:220px !important;
        }
        .chosen-choices {
            width:100% !important;
        }
        .chosen-single {
        width:200px !important;
        }
        #lstContactBy {
            width:200px  ;
        }
      <%--  #lstContactBy {
            display:none !important;
            
        }--%>
        #lstContactBy_chosen{
            width:200px !important;
        }
        
    </style>
     
    <script language="javascript" type="text/javascript">

        //function QuotationNumberChanged() {
        //    var quote_Id = gridquotationLookup.gridView.GetSelectedKeysOnPage();//gridquotationLookup.GetValue();
        //    quote_Id = quote_Id.join();

        //                cComponentDatePanel.PerformCallback('BindComponentDate' + '~' + quote_Id + '~' + type);
              
        //}
        var Customer_Id = [];
        function CheckCustomerAllreadyExist()
        {

            $.ajax({
                type: "POST",
                url: "UserControls/GroupContact.asmx/GetCustomer",
                data: JSON.stringify({ CustomerId: CustomerId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    _customerAddress = msg.d;
                }
            });
        }


        function Savegrid()
        {
            $("#hdngridstatus").val("Bind");
            CgridEdit.PerformCallback('BindEditGrid')

            //CgridEdit.Refresh();
            //location.reload();
           // grid.Refresh();
           // grid.Refresh();
           
        }
        function GroupEndCallBack()
        {
            if (CgridEdit.cpUpdatedGrid == "UpdateWithRefresh") {
                CgridEdit.cpUpdatedGrid = null;
                CPopup_EditGroup.Show();
            }
            else {
                
               grid.Refresh();
                //CgridEdit.Refresh();
                //location.reload();
                //CPopup_EditGroup.Show();
                //CPopup_EditGroup.Hide();
                CPopup_EditGroup.Hide();
            }
        }
             
        
        function CustomerButnClick(s, e) {
            $('#CustModel').modal('show');
        }

        function Customer_KeyDown(s, e) {
            if (e.htmlEvent.key == "Enter" || e.code == "NumpadEnter") {
                $('#CustModel').modal('show');
                $("#txtCustSearch").focus();
            }
        }

        function Customerkeydown(e) {
            var OtherDetails = {}

            if ($.trim($("#txtCustSearch").val()) == "" || $.trim($("#txtCustSearch").val()) == null) {
                return false;
            }
            OtherDetails.SearchKey = $("#txtCustSearch").val();
           // OtherDetails.BranchID = $('#ddl_Branch').val();

            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var HeaderCaption = [];
                HeaderCaption.push("Name");
                HeaderCaption.push("Unique Id");
                HeaderCaption.push("Address");

                if ($("#txtCustSearch").val() != "") {
                    if ($('#hdnCntType').val() == "Customers") {
                        callonServer("UserControls/GroupContact.asmx/GetCustomer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                    }
                  else  if ($('#hdnCntType').val() == "Vendors") {
                        callonServer("UserControls/GroupContact.asmx/GetVendorWithOutBranch", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                  }
                  else if (($('#hdnCntType').val() == "Relationship Partner") || ($('#hdnCntType').val() == "RelationshipPartner")) {
                      callonServer("UserControls/GroupContact.asmx/GetInfluencer", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                  }
                  else if ($('#hdnCntType').val() == "DriverTransporter") {
                      callonServer("UserControls/GroupContact.asmx/GetTransporter", OtherDetails, "CustomerTable", HeaderCaption, "customerIndex", "SetCustomer");
                  }
                }
            }
            else if (e.code == "ArrowDown") {
                if ($("input[customerindex=0]"))
                    $("input[customerindex=0]").focus();
            }
        }
        function ValueSelected(e, indexName) {
            if (e.code == "Enter" || e.code == "NumpadEnter") {
                var Id = e.target.parentElement.parentElement.cells[0].innerText;
                var name = e.target.parentElement.parentElement.cells[1].children[0].value;
                if (Id) {
                    if (indexName == "ProdIndex") {
                        SetProduct(Id, name);
                    }
                    else if (indexName == "salesmanIndex") {
                        OnFocus(Id, name);
                    }
                        // Added By Chinmoy
                        //Start
                    else if (indexName == "BillingAreaIndex") {
                        SetBillingArea(Id, name);
                    }
                    else if (indexName == "ShippingAreaIndex") {
                        SetShippingArea(Id, name);
                    }
                    else if (indexName == "customeraddressIndex") {
                        SetCustomeraddress(Id, name);
                    }
                        //End
                    else {
                        SetCustomer(Id, name);
                    }
                }
            }
            else if (e.code == "ArrowDown") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex++;
                if (thisindex < 10)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
            }
            else if (e.code == "ArrowUp") {
                thisindex = parseFloat(e.target.getAttribute(indexName));
                thisindex--;
                if (thisindex > -1)
                    $("input[" + indexName + "=" + thisindex + "]").focus();
                else {
                    if (indexName == "ProdIndex")
                        $('#txtProdSearch').focus();
                    else if (indexName == "salesmanIndex")
                        ctxtCreditDays.Focus();
                        // Added By Chinmoy
                        //Start
                    else if (indexName == "BillingAreaIndex")
                        $('#txtbillingArea').focus();
                    else if (indexName == "ShippingAreaIndex")
                        $('#txtshippingArea').focus();
                    else if (indexName == "customeraddressIndex")
                        ('#txtshippingShipToParty').focus();
                        //End
                    else
                        $('#txtCustSearch').focus();
                }
            }
        }

        function SetCustomer(id, Name) {

            var key = id;
            $('#CustomerId').val(id)
            if (key != null && key != '') {
                $('#CustModel').modal('hide');
                ClstContactBy.SetText(Name);
            }
        }

        function LoadOldSelectedKeyvalue() {
            var x = grpmasterLookup.gridView.GetSelectedKeysOnPage();
            var Ids = "";
            for (var i = 0; i < x.length; i++) {
                Ids = Ids + ',' + x[i];
            }
            document.getElementById('OldSelectedGroupvalue').value = Ids;
        }
        function Save_AddGroupClick()
        {
            CPopup_AddGroup.Show();

        }
        function Save_EditGroupClick()
        {
            $("#hdngridstatus").val("Edit");
            CgridEdit.PerformCallback('Databinding')

            //CPopup_EditGroup.Show();
           // CgridEdit.Refresh();

        }
        function BeginComponentCallback() {
        }

        function CloseGridQuotationLookup() {

            grpmasterLookup.ConfirmCurrentSelection();
            grpmasterLookup.HideDropDown();
            grpmasterLookup.Focus();
        }
        function callOnLoad() {
            $(".chzn-select").chosen();
            $(".chzn-select-deselect").chosen({ allow_single_deselect: true });
            ListBind();
        }

        $(document).ready(function () {
            callOnLoad();
            //ChangeSource();

        });
        function Changeselectedvalue() {
            var lstContactBy = document.getElementById("lstContactBy");
            if (document.getElementById("hdContactBy").value != '') {
                for (var i = 0; i < lstContactBy.options.length; i++) {
                    if (lstContactBy.options[i].value == document.getElementById("hdContactBy").value) {
                        lstContactBy.options[i].selected = true;
                    }
                }
                $('#lstContactBy').trigger("chosen:updated");
            }

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
        function lstContactBy() {

            // $('#lstReferedBy').chosen();
            $('#lstContactBy').fadeIn();
        }
        function setvalue() {
            $('#CustomerId').val()
            //document.getElementById("txtContact_hidden").value = document.getElementById("lstContactBy").value;
            document.getElementById("txtContact_hidden").value = document.getElementById("CustomerId").value;
        }
        function ChangeSource() {

            var InterId = "";
            var fname = "%";
            var a = document.getElementById("txtID").value;
            var b = document.getElementById("MType").value;
            if (document.getElementById("DDLAddData").value) {
                InterId = document.getElementById("DDLAddData").value;
            }
            var c = document.getElementById("DDLAddData").value;
          //  var d = document.getElementById("ddlValue").value;
           // var obj4 = a + '~' + b + '~' + c + '~' + d;
            var obj4 = a + '~' + b + '~' + c ;
          
            var lReferBy = $('select[id$=lstContactBy]');
            lReferBy.empty();

            $.ajax({
                type: "POST",
                url: "groupmasterPopUp.aspx/GetgroupmasterPopUp",
                data: JSON.stringify({ reqStr: fname, obj4: obj4 }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].split('|')[1];
                            name = list[i].split('|')[0];
                            $('#lstContactBy').append($('<option>').text(name).val(id));
                        }

                        $(lReferBy).append(listItems.join(''));

                        lstContactBy();
                        $('#lstContactBy').trigger("chosen:updated");
                        Changeselectedvalue();
                    }
                    else {
                        alert("No records found");
                      //  lstReportTo();
                        $('#lstContactBy').trigger("chosen:updated");
                    }
                }
            });

        }
        function showOptions(objID, objType, objEvent) {
            if (objID.value == "") {
                objID.value = "%";
            }
            var a = document.getElementById("txtID").value;
            var b = document.getElementById("MType").value;
            var c = document.getElementById("ddlText").value;
            var d = document.getElementById("ddlValue").value;
            var obj4 = a + '~' + b + '~' + c + '~' + d;
            if (c != '' && d != '') {
                if (objType == 'NSDLClientsGroupMember') {
                    strQuery_Table = " Master_NsdlClients";/*Session["usersegid"]='IN304004'//////select gpm_Type,gpm_code from tbl_master_groupMaster where gpm_id="+a+*/
                    strQuery_FieldName = " distinct Top 10 (LTRIM(rtrim(NsdlClients_BenFirstHolderName))+' ['+cast(ltrim(rtrim(NsdlClients_BenAccountID)) as varchar(10))+']') as Name,(NsdlClients_DPID+cast(NsdlClients_BenAccountID as varchar(10))) as ID ";
                    strQuery_WhereClause = " NsdlClients_DPID='" + '<%=Session["usersegid"] %>' + "' and NsdlClients_DPID+cast(NsdlClients_BenAccountID as varchar(10)) not in (Select grp_contactId from tbl_master_groupMaster,tbl_trans_group Where gpm_code=(select gpm_code from tbl_master_groupMaster where gpm_id=" + a + ") and gpm_Type=(select gpm_Type from tbl_master_groupMaster where gpm_id=" + a + ") and grp_groupMaster=gpm_id and Left(grp_contactId,2)='IN') and (NsdlClients_BenFirstHolderName like \'RequestLetter%' Or NsdlClients_BenAccountID  like \'RequestLetter%')";
                    strQuery_OrderBy = " Name";
                    strQuery_GroupBy = "";
                    CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_GroupBy + "$" + strQuery_OrderBy);
                    ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery), 'Main');
                }
                else if (objType == 'CDSLClientsGroupMember') {
                    strQuery_Table = " Master_CdslClients";
                    strQuery_FieldName = " distinct Top 10 (LTRIM(rtrim(CdslClients_FirstHolderName))+' '+LTRIM(rtrim(CdslClients_FirstHolderMiddleName))+' '+LTRIM(rtrim(CdslClients_FirstHolderLastName))+' ['+cast(ltrim(rtrim(right(CdslClients_BOID,8))) as varchar(10))+']') as Name,cast(RIGHT(CdslClients_BOID,8) as varchar(10)) as ID ";
                    strQuery_WhereClause = " CdslClients_DPID='" + '<%=Session["usersegid"] %>' + "' and CdslClients_BOID  not in (Select grp_contactId from tbl_master_groupMaster,tbl_trans_group Where gpm_code=(select gpm_code from tbl_master_groupMaster where gpm_id=" + a + ") and gpm_Type=(select gpm_Type from tbl_master_groupMaster where gpm_id=" + a + ") and grp_groupMaster=gpm_id and Left(grp_contactId,2)='12') and (CdslClients_FirstHolderName like \'RequestLetter%' Or right(CdslClients_BOID,8)  like \'RequestLetter%')";
                    strQuery_OrderBy = " Name";
                    strQuery_GroupBy = "";
                    CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_GroupBy + "$" + strQuery_OrderBy);
                    ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery), 'Main');
                }
                else {
                    ajax_showOptionsTEST(objID, objType, objEvent, obj4);
                    if (objID.value == "%") {
                        objID.value = "";
                    }
                }
        }
        else {
            alert('Please Select Contact Type...!!')
        }
    }
    function replaceChars(entry) {
        out = "+"; // replace this
        add = "--"; // with this
        temp = "" + entry; // temporary holder

        while (temp.indexOf(out) > -1) {
            pos = temp.indexOf(out);
            temp = "" + (temp.substring(0, pos) + add +
            temp.substring((pos + out.length), temp.length));
        }
        return temp;
    }
    function CallGrid() {
        // editwin.close();
        grid.PerformCallback();
    }
    FieldName = 'BtnSave';
    function GridName_EndCallBack() {
     <%--   document.getElementById('<%=txtContact.ClientID%>').focus();
       document.getElementById('<%=txtContact.ClientID%>').select();--%>
    }
    function Page_Load() {
     <%--   document.getElementById('<%=txtContact.ClientID%>').focus();
        document.getElementById('<%=txtContact.ClientID%>').select();--%>
    }
    </script>
    <style>
        .padTbl>tbody>tr:not(:last-child)> td {
            padding-bottom:10px !important;
        }

        /*Rev 1.0*/

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

        /*.panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }*/

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
        .TableMain100 #GrdHolidays , #gridudfGroup
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
        /*Rev end 1.0*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Add Member</h3>

            <div class="crossBtn">
                <asp:LinkButton ID="goBackCrossBtn" runat="server" OnClick="goBackCrossBtn_Click"><i class="fa fa-times"></i></asp:LinkButton>
                <%--<a href="frmContactMain.aspx" id="goBackCrossBtn"><i class="fa fa-times"></i></a>--%>
                <%--<asp:HiddenField ID="hidbackPagerequesttype" runat="server" />--%>
            </div>
        </div>


    </div>
        <div>
        
        <dxe:ASPxButton ID="btn_SaveGroup" ClientInstanceName="Cbtn_SaveGroup" runat="server" AutoPostBack="False" Text="Add" CssClass="btn btn-success" >
                                            <ClientSideEvents Click="function(s, e) {Save_AddGroupClick();}" />
            </dxe:ASPxButton>
         <dxe:ASPxButton ID="btn_EditGroup" ClientInstanceName="Cbtn_EditGroup" runat="server" AutoPostBack="False" Text="Edit" CssClass="btn btn-primary" >
                                            <ClientSideEvents Click="function(s, e) {Save_EditGroupClick();}" />
            </dxe:ASPxButton>
    </div>

        <div class="form_main">
        <table class="TableMain100">

            
            
            <tr>
                <td>
                    <%-- <asp:Panel ID="GridPanel" runat="server" Visible="false" Width="99%">--%>
                    <dxe:ASPxGridView ID="GridName" runat="server" KeyFieldName="grp_id" AutoGenerateColumns="False"
                        DataSourceID="SelectName" ClientInstanceName="grid" Width="100%" OnCustomCallback="GridName_CustomCallback"
                        OnRowDeleting="GridName_RowDeleting">
                     <%--   <ClientSideEvents EndCallback="function(s, e) {GridName_EndCallBack();}" />--%>
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="0" Width="80%">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Id" Visible="False">
                            </dxe:GridViewDataTextColumn>
                            <%--<dxe:GridViewCommandColumn VisibleIndex="1">
                                    <DeleteButton Visible="True">
                                    </DeleteButton>
                                </dxe:GridViewCommandColumn>--%>
                            <dxe:GridViewCommandColumn VisibleIndex="8" ShowDeleteButton="True">
                                <HeaderTemplate>
                                    <span style="color: #fffff; text-align: center;">Action</span>
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewCommandColumn>
                        </Columns>
                        <Styles>
                           <%-- <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>--%>
                        </Styles>
                        <SettingsText ConfirmDelete="Are You Sure To Delete This Record ???" />
                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                        <SettingsSearchPanel Visible="True" />
                        <settings showgrouppanel="True" showstatusbar="Hidden" showhorizontalscrollbar="False" showfilterrow="true" showfilterrowmenu="true" />
                    </dxe:ASPxGridView>
                    <%--  <asp:Button ID="AddMember" runat="server" Text="Add Member" OnClick="AddMember_Click" />--%>
                    <%-- </asp:Panel> --%>
                </td>
            </tr>
        </table>
        <%-- DeleteCommand="Delete from tbl_trans_group where grp_id=@grp_id">
                <DeleteParameters>
                    <asp:Parameter Name="grp_id" Type="decimal" />
                </DeleteParameters>
        --%>
        <dxe:ASPxPopupControl ID="Popup_AddGroup" runat="server" ClientInstanceName="CPopup_AddGroup"
                    Width="400px" HeaderText="Add Group" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
                    PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">
                            <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                           <%-- <div class="ProductMainContaint">--%>
                            <table class="padTbl">
                                    <tr>
                                            <td>
                                                <div style="width: 125px; display: inline-block;"><strong>Group Name:</strong></div>
                                                
                                            </td>
                                            <td><span>
                                                    <asp:Literal ID="litGpName" runat="server"></asp:Literal></span></td>
                                        </tr>
                                        <tr>
                                    <td style="text-align: left; width: 127px;">
                                        <asp:Label ID="Label1" runat="server" Text="Choose By"  Font-Bold="True"></asp:Label>
                                    </td>
                                    <td style="text-align: left">
                                       <%-- <asp:DropDownList ID="DDLAddData" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDLAddData_SelectedIndexChanged"
                                            Width="200px">
                                        </asp:DropDownList>--%>
                                        <asp:DropDownList ID="DDLAddData" runat="server" Enabled="False" onchange="ChangeSource()" 
                                            Width="200px">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdnCntType" runat="server" />
                                    </td>
                                </tr>
                                <tr style="padding-top: 9px">
                                    <td><asp:Label ID="Label2" runat="server" Text="Select Contact"  Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>

                                         <dxe:ASPxButtonEdit ID="lstContactBy" runat="server" ReadOnly="true" ClientInstanceName="ClstContactBy" Width="200px">
                                                    <Buttons>
                                                        <dxe:EditButton>
                                                        </dxe:EditButton>
                                                    </Buttons>
                                                    <ClientSideEvents ButtonClick="function(s,e){CustomerButnClick();}" KeyDown="function(s,e){Customer_KeyDown(s,e);}" />
                                          </dxe:ASPxButtonEdit>
                                   <%-- <td>
                                   <%--    <asp:DropDownList ID="lstContactBy" runat="server"   CssClass="chsn" runat="server" Font-Size="12px" Width="150px" data-placeholder="Select...">
                                        </asp:DropDownList>--%>
                                       <%-- <asp:ListBox ID="lstContactBy" CssClass="chsn" runat="server" Font-Size="12px" Width="150px" data-placeholder="Select..."></asp:ListBox>--%>
                                     <%--   <asp:TextBox ID="txtContact" runat="server" Width="200px" autocomplete="off"></asp:TextBox>--%>
                                        <asp:HiddenField ID="txtContact_hidden" runat="server" />
                                        <asp:HiddenField ID="CustomerId" runat="server" />

                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" 
                                            ControlToValidate="lstContactBy" ValidationGroup="a" ForeColor="Red" SetFocusOnError="true">

                                        </asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr style="padding-top: 9px;">
                                    <td colspan="2" style="padding-left: 126px">
                                        <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="btn btn-danger" OnClick="BtnSave_Click" OnClientClick="setvalue()" ValidationGroup="a" />
                                        <asp:Button ID="BtnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel" OnClick="BtnCancel_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: left" visible="false">
                                        <asp:HiddenField ID="hdContactBy" runat="server" />
                                        <asp:HiddenField ID="txtID" runat="server" />
                                        <asp:HiddenField ID="MType" runat="server" />
                                        <asp:HiddenField ID="ddlValue" runat="server" />
                                        <asp:HiddenField ID="ddlText" runat="server" />
                                        <asp:HiddenField ID="hdngridstatus" runat="server" />
                                        <%-- <asp:ListBox ID="LLbAddData" runat="server" Width="100%" Height="266px" SelectionMode="Multiple" Visible="false"></asp:ListBox>--%>
                                        <br />
                                    </td>
                                </tr>



                    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">--%>
                      <%--  <ContentTemplate>--%>
                            <%--  <asp:Panel ID="Panel1" runat="server" Width="99%" Visible="False">--%>
                          
                       <%-- </ContentTemplate>--%>
                   <%-- </asp:UpdatePanel>--%>
                    
                       </table>
                           <%-- </div>--%>
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>

                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />

                </dxe:ASPxPopupControl>
          <dxe:ASPxPopupControl ID="Popup_EditGroup" runat="server" ClientInstanceName="CPopup_EditGroup"
                    Width="1000px" HeaderText="Edit Group" PopupHorizontalAlign="WindowCenter" HeaderStyle-CssClass="wht"
                    PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ContentCollection>
                        <dxe:PopupControlContentControl runat="server">

                           <%--  <dxe:ASPxGridView ID="gridEdit" runat="server" KeyFieldName="grp_id" AutoGenerateColumns="False"
                        DataSourceID="SelectName" ClientInstanceName="CgridEdit" Width="100%" OnCustomCallback="GridEdit_CustomCallback"
                        OnRowDeleting="EditGrid_RowDeleting">--%>
                            <dxe:ASPxGridView ID="gridEdit" runat="server" KeyFieldName="ContactId"  AutoGenerateColumns="False"  SettingsDataSecurity-AllowEdit="false" 
            SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" OnCustomCallback="gridEdit_CustomCallback"  OnDataBinding="gridEdit_DataBinding"
            Width="100%" ClientInstanceName="CgridEdit" 
            SettingsBehavior-AllowFocusedRow="true" 
            HorizontalScrollBarMode="Auto"  >
                     <%--   <ClientSideEvents EndCallback="function(s, e) {GridName_EndCallBack();}" />--%>
                        <Columns>
                            <dxe:GridViewCommandColumn  VisibleIndex="0"  ShowSelectCheckbox="true" SelectAllCheckboxMode="AllPages" Width="9%" Visible="true"></dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn FieldName="Name" ReadOnly="True" VisibleIndex="1" Width="70%">
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn FieldName="ShortName" ReadOnly="True" VisibleIndex="2" Width="20%">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="gpm_id" Caption="Id"  Visible="false">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="grp_id" Caption="GroupId"  Visible="false">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="ContactId" Caption="ContactId" Visible="false" >
                            </dxe:GridViewDataTextColumn>
                          
                        </Columns>
                                <ClientSideEvents EndCallback="GroupEndCallBack" />
                      <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                     <%-- <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" />--%>
                       
                        <SettingsSearchPanel Visible="True" />
                        <settings showgrouppanel="True" showstatusbar="Hidden" showhorizontalscrollbar="False" showfilterrow="true" showfilterrowmenu="true" />
                    </dxe:ASPxGridView>

                                    <%-- <button class="btn btn-primary pull-right"  type="button" title="Save" onclick="Savegrid();">Save</button>--%>
                            <dxe:ASPxButton runat="server" AutoPostBack="false" cssClass="btn btn-primary pull-right mTop5"  Text="Save" ID="btnGrpSave" ClientInstanceName="CbtnGrpSave">
                                <ClientSideEvents Click="Savegrid" />
                            </dxe:ASPxButton>
                            
                             <dx:LinqServerModeDataSource ID="EntityServerModeDataSource" runat="server" OnSelecting="EntityServerModeDataSource_Selecting"
                            ContextTypeName="ERPDataClassesDataContext" TableName="v_GetGroupmemberList" />
                             </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <ContentStyle VerticalAlign="Top" CssClass="pad"></ContentStyle>

                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />

                </dxe:ASPxPopupControl>

        <asp:SqlDataSource ID="SelectName" runat="server">
            <SelectParameters>
                <asp:QueryStringParameter Name="RId" QueryStringField="id" Type="decimal" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </div>
     <!--Customer Modal -->
    <div class="modal fade" id="CustModel" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Customer Search</h4>
                </div>
                <div class="modal-body">
                    <input type="text" onkeydown="Customerkeydown(event)" id="txtCustSearch" autofocus width="100%" placeholder="Search By Customer Name or Unique Id" />
                    <div id="CustomerTable">
                        <table border='1' width="100%" class="dynamicPopupTbl">
                            <tr class="HeaderStyle">
                                <th class="hide">id</th>
                                <th>Name</th>
                                <th>Unique Id</th>
                                <th>Address</th>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <!--Customer Modal -->
</asp:Content>
