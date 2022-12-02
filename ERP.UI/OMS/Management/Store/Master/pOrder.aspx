<%@ Page Title="Building Details" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Store.Master.management_DailyTask_pOrder" CodeBehind="pOrder.aspx.cs" %>
<%@ Register TagPrefix="uc" TagName="Spinner" Src="~/OMS/Management/Store/Master/uc_pOrder.ascx" %>
<%@ Register TagPrefix="uc" TagName="ucpOrd" Src="~/OMS/Management/Store/Master/ucpOder.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function txtOrder_RefNumber_TextChange() {
            var EditId = document.getElementById("pOderId_hidden").value;
            if (EditId == "") {
                var RefNumber = document.getElementById("txtOrder_RefNumber_I").value;
                $.ajax({
                    type: "POST",
                    url: "pOrder.aspx/CheckUniqueRefNumber",
                    data: "{'RefNumber':'" + RefNumber + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var data = msg.d;
                        if (data == true) {
                            alert("Please enter unique reference no");
                            return false;
                        }
                    }

                });
            }
            else {
                document.getElementById("txtOrder_RefNumber_I").readOnly = true;
            }
        }
    </script>

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            txtOrder_RefNumber_TextChange();
            document.getElementById('AssignValuePopupUC_KeyFieldUC').style.display = "None";
            document.getElementById('AssignValuePopupUC_ValueFieldUC').style.display = "None";
            document.getElementById('AssignValuePopupUC_RexPageNameUC').style.display = "None";


            //reset on page load
            DeliveryTextChanged_Reset();
            PaymentDateSet();
            document.getElementById('lblUserOrderType').innerHTML = "";
            if (cCmbddlOrderType.GetValue() == 'P')
                document.getElementById('lblUserOrderType').innerHTML = "Vendor/Supplier";
            else if (cCmbddlOrderType.GetValue() == 'J')
                document.getElementById('lblUserOrderType').innerHTML = "Vendor/Supplier";
            else if (cCmbddlOrderType.GetValue() == 'S')
                document.getElementById('lblUserOrderType').innerHTML = "Customer/Client";
            else if (cCmbddlOrderType.GetValue() == 'O')
                document.getElementById('lblUserOrderType').innerHTML = "Customer/Client";
            else if (cCmbddlOrderType.GetValue() == 'R')
                document.getElementById('lblUserOrderType').innerHTML = "Employee";
            if ($("#pOderId_hidden").val() == "") {
                $("#Spinner1_cityGrid_DXTDcol3").hide();
            }


            // for edit show hide





            var DeliveryAtValue = cddlDeliveryAt_pOrderType.GetValue();
            if (DeliveryAtValue == 'W') {
                document.getElementById("dlblDeliveryWarehouse").style.display = "block";
                document.getElementById("dtxtDeliveryWareHouse").style.display = "block";
            }
            if (DeliveryAtValue == 'B') {
                document.getElementById("dtxtDeliveryBranch").style.display = "block";
                document.getElementById("dlblDeliveryBranch").style.display = "block";
            }
            if (DeliveryAtValue == 'O') {
                document.getElementById("dlblDeliveryOther").style.display = "block";
                document.getElementById("dtxtDeliveryOther").style.display = "block";
            }
            if (DeliveryAtValue == 'A') {
                document.getElementById("dlblDeliveryAddress").style.display = "block";
                document.getElementById("dtxtDeliveryAddress").style.display = "block";
            }
            $('#txttype_UserAccount').blur(function () {
                cddlDeliveryAt_pOrderType.SetValue("0");
                cCmbpOrder_DeliveryAddress.SetValue("0");
            });
        });

        function PaymentDateSet() {
            var PaymentTerm = cCmbpOrder_PaymentTerm.GetValue();
            if (PaymentTerm == 'P') {
                document.getElementById("lblPaymentDate").style.display = "block";
                document.getElementById("dtPaymentDate").style.display = "block";
                document.getElementById("lblShipmentDate").style.display = "none";
                document.getElementById("txtShipmentDate").style.display = "none";
            }
            else if (PaymentTerm == 'C') {
                document.getElementById("lblPaymentDate").style.display = "block";
                document.getElementById("dtPaymentDate").style.display = "block";
                document.getElementById("lblShipmentDate").style.display = "none";
                document.getElementById("txtShipmentDate").style.display = "none";
            }
            else if (PaymentTerm == 'X') {
                document.getElementById("lblShipmentDate").style.display = "block";
                document.getElementById("txtShipmentDate").style.display = "block";
                document.getElementById("lblPaymentDate").style.display = "none";
                document.getElementById("dtPaymentDate").style.display = "none";
            }
            else {
                document.getElementById("lblPaymentDate").style.display = "none";
                document.getElementById("dtPaymentDate").style.display = "none";
                document.getElementById("lblShipmentDate").style.display = "none";
                document.getElementById("txtShipmentDate").style.display = "none";
            }
        }


        function PaymentTermChanged() {
            PaymentDateSet();
        }
        //function SignOff() {
        //    window.parent.SignOff();
        //}
        //        function height() {
        //            if (document.body.scrollHeight >= 1000)
        //                window.frameElement.height = document.body.scrollHeight;
        //            else
        //                window.frameElement.height = '1000px';
        //            window.frameElement.Width = document.body.scrollWidth;
        //        }

        function FunCallAjaxList_type_UserAccount(objID, objEvent, ObjType) {

            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';

            if (ObjType == 'Digital') {

                var txttype_UserAccount = document.getElementById('txttype_UserAccount').value;
                // var a = document.getElementById('CmbpOrder_Branch_I').value;
                var branchid = cCmbpOrder_Branch.GetValue();
                strQuery_Table = "tbl_master_contact";
                strQuery_FieldName = "cnt_firstname+' '+cnt_middlename+' '+cnt_lastname+ case when cnt_UCC= '' then '' else  +' ('+cnt_UCC+')' end as name , cnt_internalId";

                if (cCmbddlOrderType.GetValue() == 'P')
                    strQuery_WhereClause = "(cnt_firstname like '" + txttype_UserAccount + "%' or cnt_UCC like '" + txttype_UserAccount + "%') and cnt_contacttype='VR' and cnt_branchid = '" + branchid + "'";
                else if (cCmbddlOrderType.GetValue() == 'J')
                    strQuery_WhereClause = "(cnt_firstname like '" + txttype_UserAccount + "%' or cnt_UCC like '" + txttype_UserAccount + "%') and cnt_contacttype='VR' and cnt_branchid = '" + branchid + "'";
                else if (cCmbddlOrderType.GetValue() == 'S')
                    strQuery_WhereClause = "(cnt_firstname like '" + txttype_UserAccount + "%' or cnt_UCC like '" + txttype_UserAccount + "%') and cnt_contacttype='CL' and cnt_branchid = '" + branchid + "'";
                else if (cCmbddlOrderType.GetValue() == 'O')
                    strQuery_WhereClause = "(cnt_firstname like '" + txttype_UserAccount + "%' or cnt_UCC like '" + txttype_UserAccount + "%') and cnt_contacttype='CL' and cnt_branchid = '" + branchid + "'";
                else if (cCmbddlOrderType.GetValue() == 'R')
                    strQuery_WhereClause = "(cnt_firstname like '" + txttype_UserAccount + "%' or cnt_UCC like '" + txttype_UserAccount + "%') and cnt_contacttype='EM' and cnt_branchid = '" + branchid + "'";
                else if (cCmbddlOrderType.GetValue() == 'I')
                    strQuery_WhereClause = "(cnt_firstname like '" + txttype_UserAccount + "%' or cnt_UCC like '" + txttype_UserAccount + "%') and cnt_contacttype='CL' and cnt_branchid = '" + branchid + "'";
            }
            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery), "Main");

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

        function FunCallAjaxListpOrder2(objID, objEvent, ObjType) {
            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';
            var agentid = document.getElementById("txt_pOrder_AgentID").value;
            if (ObjType == 'Digital') {

                strQuery_Table = "tbl_master_contact";
                strQuery_FieldName = "cnt_firstname+' '+cnt_middlename+' '+cnt_lastname+ case when cnt_UCC= '' then '' else  +' ('+cnt_UCC+')' end as name , cnt_internalId";
                strQuery_WhereClause = "(cnt_firstname like '" + agentid + "%' or cnt_UCC like '" + agentid + "%')";


            }


            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceCharsorder(CombinedQuery), "Main");

        }

        function replaceCharsorder(entry) {
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
        function OncCmbddlOrderType_ValueChange() {
            // reset delivery address visible fields
            DeliveryTextChanged_Reset();
            //alert(cCmbddlOrderType.GetValue());
            document.getElementById('lblUserOrderType').innerHTML = "";
            if (cCmbddlOrderType.GetValue() == 'P') {

                document.getElementById('lblUserOrderType').innerHTML = "Vendor/Supplier";
            }
            else if (cCmbddlOrderType.GetValue() == 'J')
                document.getElementById('lblUserOrderType').innerHTML = "Vendor/Supplier";
            else if (cCmbddlOrderType.GetValue() == 'S')
                document.getElementById('lblUserOrderType').innerHTML = "Customer/Client";
            else if (cCmbddlOrderType.GetValue() == 'O')
                document.getElementById('lblUserOrderType').innerHTML = "Customer/Client";
            else if (cCmbddlOrderType.GetValue() == 'R')
                document.getElementById('lblUserOrderType').innerHTML = "Employee";
            else if (cCmbddlOrderType.GetValue() == 'I')
                document.getElementById('lblUserOrderType').innerHTML = "Customer";


            cddlDeliveryAt_pOrderType.PerformCallback("bind_Delivery_At~" + cCmbddlOrderType.GetValue());
            CmbddlOrderTypeCheck(cCmbddlOrderType.GetValue());

            document.getElementById('txttype_UserAccount').value = "";
            document.getElementById('txttype_UserAccount_hidden').value = "";

        }

        function CmbddlOrderTypeCheck(value) {
            //            if (value == 'P' || value == 'R') {
            //                document.getElementById("dlblDeliveryAddress").style.display = "none";
            //                document.getElementById("dtxtDeliveryAddress").style.display = "none";
            //                
            //                document.getElementById("dtxtDeliveryBranch").style.display = "block";
            //                document.getElementById("dlblDeliveryBranch").style.display = "block";
            //                document.getElementById("dtxtDeliveryWareHouse").style.display = "block";
            //                document.getElementById("dlblDeliveryWarehouse").style.display = "block";
            //            }
            //            if (value == 'S' || Value == 'O') {
            //                document.getElementById("dtxtDeliveryBranch").style.display = "none";
            //                document.getElementById("dlblDeliveryBranch").style.display = "none"; 
            //                document.getElementById("dtxtDeliveryWareHouse").style.display = "none";
            //                document.getElementById("dlblDeliveryWarehouse").style.display = "none";

            //                document.getElementById("dlblDeliveryAddress").style.display = "block";
            //                document.getElementById("dtxtDeliveryAddress").style.display = "block";

            //                document.getElementById('dlblDeliveryAddress').innerHTML = 'Client Delivery Address';
            //            }
            //            if (value == 'J') {
            //                document.getElementById("dtxtDeliveryBranch").style.display = "none";
            //                document.getElementById("dlblDeliveryBranch").style.display = "none";
            //                document.getElementById("dtxtDeliveryWareHouse").style.display = "none";
            //                document.getElementById("dlblDeliveryWarehouse").style.display = "none"; 
            //                
            //                document.getElementById("dlblDeliveryAddress").style.display = "block";
            //                document.getElementById("dtxtDeliveryAddress").style.display = "block";
            //            }
        }
        function DeliveryTextChanged() {
            DeliveryTextChanged_Reset();
            var DeliveryAtValue = cddlDeliveryAt_pOrderType.GetValue();
            if (DeliveryAtValue == 'W') {
                document.getElementById("dlblDeliveryWarehouse").style.display = "block";
                document.getElementById("dtxtDeliveryWareHouse").style.display = "block";
                cCmbpOrder_DeliveryBranch.SetValue("0");
                cCmbpOrder_DeliveryAddress.SetValue("0");
                ctxtpOrder_DeliveryOther.SetText('');
            }
            if (DeliveryAtValue == 'B') {
                document.getElementById("dtxtDeliveryBranch").style.display = "block";
                document.getElementById("dlblDeliveryBranch").style.display = "block";

                cCmbpOrder_DeliveryWareHouse.SetValue("0");
                cCmbpOrder_DeliveryAddress.SetValue("0");
                ctxtpOrder_DeliveryOther.SetText('');
            }
            if (DeliveryAtValue == 'O') {
                document.getElementById("dlblDeliveryOther").style.display = "block";
                document.getElementById("dtxtDeliveryOther").style.display = "block";
                cCmbpOrder_DeliveryWareHouse.SetValue("0");
                cCmbpOrder_DeliveryAddress.SetValue("0");
                cCmbpOrder_DeliveryBranch.SetValue("0");
            }
            if (DeliveryAtValue == 'A') {
                document.getElementById("dlblDeliveryAddress").style.display = "block";
                document.getElementById("dtxtDeliveryAddress").style.display = "block";

                cCmbpOrder_DeliveryWareHouse.SetValue("0");
                ctxtpOrder_DeliveryOther.SetText('');
                cCmbpOrder_DeliveryBranch.SetValue("0");

                cCmbpOrder_DeliveryAddress.PerformCallback("BindCmbpOrder_DeliveryAddress~" + document.getElementById("txttype_UserAccount_hidden").value);
            }

        }



        function DeliveryTextChanged_Reset() {
            document.getElementById("dtxtDeliveryBranch").style.display = "none";
            document.getElementById("dlblDeliveryBranch").style.display = "none";

            document.getElementById("dlblDeliveryWarehouse").style.display = "none";
            document.getElementById("dtxtDeliveryWareHouse").style.display = "none";

            document.getElementById("dlblDeliveryAddress").style.display = "none";
            document.getElementById("dtxtDeliveryAddress").style.display = "none";

            document.getElementById("dlblDeliveryOther").style.display = "none";
            document.getElementById("dtxtDeliveryOther").style.display = "none";
        }

        //Validation function
        function pOrder_btnsubmit_Click() {

            var isValidate = true;
            var Type = cCmbddlOrderType.GetValue();
            var pOrderDate = document.getElementById("txtTaxRates_DateFrom_I").value;
            var ContactName = document.getElementById("txttype_UserAccount").value;
            var ReferanceNo = document.getElementById("txtOrder_RefNumber_I").value;
            var PaymentTerm = cCmbpOrder_PaymentTerm.GetValue();
            var PaymentDate = document.getElementById("txtpOrder_PaymentDate_I").value;
            var DeliveryAt = cddlDeliveryAt_pOrderType.GetValue();
            var Branch = document.getElementById("CmbpOrder_DeliveryBranch_I").value;
            var Warehouse = document.getElementById("CmbpOrder_DeliveryWareHouse_I").value;
            var ClientAddress = document.getElementById("CmbpOrder_DeliveryAddress_I").value;
            var OtherLocation = document.getElementById("txtpOrder_DeliveryOther_I").value;
            if (Type == "" || Type == null || Type === undefined) {

                alert('Please enter value for order type');
                document.getElementById('CmbddlOrderType_I').focus();
                return false;
            }
            if (pOrderDate == "" || pOrderDate == null || pOrderDate === undefined) {
                isvalidate = false;
                alert('Please enter value for Date');
                document.getElementById('txtTaxRates_DateFrom_I').focus();
                return false;
            }
            if (ContactName == "" || ContactName == null || ContactName === undefined) {
                isvalidate = false;
                alert('Please enter select value for Client/Vendor');
                document.getElementById('txttype_UserAccount').focus();
                return false;
            }
            if (ReferanceNo == "" || ReferanceNo == null || ReferanceNo === undefined) {
                isvalidate = false;
                alert('Please enter value for order referance');
                document.getElementById('txtOrder_RefNumber_I').focus();
                return false;
            }

            if (Type == 'P' || Type == 'S' || Type == 'O') {
                if (PaymentTerm == "" || PaymentTerm == null || PaymentTerm === undefined || PaymentTerm == "0") {
                    isvalidate = false;
                    alert('Please enter value for payment term');
                    document.getElementById('CmbpOrder_PaymentTerm_I').focus();
                    return false;
                }
            }

            if (PaymentTerm == 'P' || PaymentTerm == 'C') {
                if (PaymentDate == "" || PaymentDate == null || PaymentDate === undefined) {
                    isvalidate = false;
                    alert('Please enter value for payment Date');
                    document.getElementById('txtpOrder_PaymentDate_I').focus();
                    return false;
                }
            }

            if (DeliveryAt == "" || DeliveryAt == null || DeliveryAt === undefined || DeliveryAt == "0") {
                isvalidate = false;
                alert('Please Select value for Delivery At');
                document.getElementById('ddlDeliveryAt_pOrderType_I').focus();
                return false;
            }
            if (DeliveryAt == 'B') {
                if (Branch == "" || Branch == null || Branch === undefined) {
                    isvalidate = false;
                    alert('Please select value for branch');
                    document.getElementById('CmbpOrder_DeliveryBranch_I').focus();
                    return false;
                }
            }
            if (DeliveryAt == 'W') {
                if (Warehouse == "" || Warehouse == null || Warehouse === undefined) {
                    isvalidate = false;
                    alert('Please select value for warehouse');
                    document.getElementById('CmbpOrder_DeliveryWareHouse_I').focus();
                    return false;
                }
            }
            if (DeliveryAt == 'A') {
                if (ClientAddress == "" || ClientAddress == null || ClientAddress === undefined) {
                    isvalidate = false;
                    alert('Please Select value for client address');
                    document.getElementById('CmbpOrder_DeliveryAddress_I').focus();
                    return false;
                }
            }
            if (DeliveryAt == 'O') {
                if (OtherLocation == "" || OtherLocation == null || OtherLocation === undefined) {
                    isvalidate = false;
                    alert('Please Select value for other location');
                    document.getElementById('txtpOrder_DeliveryOther_I').focus();
                    return false;
                }
            }
            txtOrder_RefNumber_TextChange();
        }
    </script>

    <%--new script added on 3 december 2015 by monojit--%>

    <script type="text/javascript">

        var counter = 0;


        function fetchLebel() {

            $("#generatedForm").html("");
            counter = 0;


            $(".newLbl").each(function () {

                var newField = "<div style='width:500px; margin-left:5px; float:left; margin-bottom:5px;'><label id='LblKey" + counter + "' style='width:110px; float:left;'>" + $(this).text() + "</label>";
                newField += "<input type='text' id='TxtKey" + counter + "' value='" + $(this).text() + "' style='margin-left:41px; width:250px;' />";

                //alert($(this).attr("id").split('_')[4]);

                if (String($(this).attr("id").split('_')[4]) != "undefined") {
                    newField += "<input type='text' id='HddnKey" + counter + "' value='" + $(this).attr("id").split('_')[4] + "' style='display:none; margin-left:41px; width:250px;' />";
                }
                else {
                    //alert($(this).attr("id"));
                    newField += "<input type='text' id='HddnKey" + counter + "' value='" + $(this).attr("id") + "' style='display:none; margin-left:41px; width:250px;' />";
                }
                newField += "</div>";

                $("#generatedForm").append(newField);

                counter++;

            });

            AssignValuePopup.Show();

        }




        function SaveDataToResource() {

            var key = "";
            var value = "";

            for (var i = 0; i < counter; i++) {

                if (key == "") {

                    key = $("#HddnKey" + i).val();
                    value = $("#TxtKey" + i).val();

                }
                else {

                    key += "," + $("#HddnKey" + i).val();
                    value += "," + $("#TxtKey" + i).val();

                }

            }

            $("#AssignValuePopup_KeyField").val(key);
            $("#AssignValuePopup_ValueField").val(value);
            $("#AssignValuePopup_RexPageName").val("CreateOrders");


            return true;

        }


    </script>

    <%--new script added on 3 december 2015 by monojit in User control--%>

    <script type="text/javascript">

        var counter = 0;


        function fetchLebelUC() {


            $("#generatedForm").html("");
            counter1 = 0;


            $(".newLblUC").each(function () {

                var newField = "<div style='width:500px; margin-left:5px; float:left; margin-bottom:5px;'><label id='LblKey" + counter1 + "' style='width:110px; float:left;'>" + $(this).text() + "</label>";
                newField += "<input type='text' id='TxtKey" + counter1 + "' value='" + $(this).text() + "' style='margin-left:41px; width:250px;' />";

                //alert($(this).attr("id").split('_')[3]);

                if (String($(this).attr("id").split('_')[3]) != "undefined") {
                    newField += "<input type='text' id='HddnKey" + counter1 + "' value='" + $(this).attr("id").split('_')[3] + "' style='display:none; margin-left:41px; width:250px;' />";
                }
                else {
                    //alert($(this).attr("id"));
                    newField += "<input type='text' id='HddnKey" + counter1 + "' value='" + $(this).attr("id") + "' style='display:none; margin-left:41px; width:250px;' />";
                }
                newField += "</div>";

                $("#generatedFormUC").append(newField);

                counter1++;

            });

            AssignValuePopupUC.Show();

        }




        function SaveDataToResourceUC() {

            //alert("2");

            var key = "";
            var value = "";

            for (var i = 0; i < counter1; i++) {

                if (key == "") {

                    key = $("#HddnKey" + i).val();
                    value = $("#TxtKey" + i).val();

                }
                else {

                    key += "," + $("#HddnKey" + i).val();
                    value += "," + $("#TxtKey" + i).val();

                }

            }

            $("#AssignValuePopupUC_KeyFieldUC").val(key);
            $("#AssignValuePopupUC_ValueFieldUC").val(value);
            $("#AssignValuePopupUC_RexPageNameUC").val("CreateOrdersUC");


            return true;

        }


    </script>

    <style type="text/css">
        #AssignValuePopupUC_PW-1 {
            width: 534px !important;
            left: 402px !important;
        }
    </style>
    <%--new script added on 3 december 2015 by monojit  in User control--%>
    <style type="text/css">
        #AssignValuePopup_PW-1 {
            width: 534px !important;
            left: 402px !important;
        }
    </style>
    <%--new script added on 3 december 2015 by monojit--%>

    <script type="text/javascript">


        $(window).bind('keydown', function (event) {
            if (event.ctrlKey || event.metaKey) {
                switch (String.fromCharCode(event.which).toLowerCase()) {
                    case 's':
                        event.preventDefault();
                        //alert('ctrl-s');
                        $("#Spinner1_BtnSave").trigger("click");
                        break;
                    case 'c':
                        event.preventDefault();
                        //alert('ctrl-c');
                        $("#Spinner1_BtnCancel").trigger("click");
                        break;
                    case 'd':
                        event.preventDefault();
                        //alert('ctrl-d');
                        $("#Spinner1_btnDiscard").trigger("click");
                        break;
                }
            }
        });



    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Building Details</h3>
        </div>
    </div>
    <div class="form_main">
        <asp:RadioButtonList runat="server" AutoPostBack="true" RepeatDirection="Horizontal"
            ID="rbShowHide" OnSelectedIndexChanged="rbShowHide_SelectedIndexChanged">
            <asp:ListItem Text="Add Edit" Value="0"></asp:ListItem>
            <asp:ListItem Text="Search" Value="1"></asp:ListItem>
        </asp:RadioButtonList>
        <asp:Panel ID="pnlAdd" runat="server">
            
            <div class="PopUpArea">
                <dxe:ASPxPopupControl ID="Popup_IsEdit" runat="server" ClientInstanceName="cPopup_IsEdit"
                    Width="407px" HeaderText="Add Order Details" PopupHorizontalAlign="WindowCenter"
                    BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                    Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                    ContentStyle-CssClass="pad">
                    <ContentStyle VerticalAlign="Top" CssClass="pad">
                    </ContentStyle>
                    <ContentCollection>
                        <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            Would you like to go for Edit existing entry or Begain with a new one?
                        <br />
                            <br />
                            <asp:Button Text="New Entry" ID="btnNewEntry" runat="server" OnClick="btnNewEntry_Click" />
                            &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnEditExt" Text="Edit existing Record" runat="server" OnClick="btnEditExt_Click" />
                            &nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnAprvOrdr" Text="Approve Order" runat="server" OnClick="btnAprvOrdr_Click" />
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
                </dxe:ASPxPopupControl>
                &nbsp;<dxe:ASPxGridViewExporter ID="exporter" runat="server">
                </dxe:ASPxGridViewExporter>
            </div>
            <div id="divChangable" style="margin-bottom: 5%">
                <div class="clearfix" style="background: #f5f4f3;padding:10px 0 22px 0;margin-bottom: 15px;border-radius:4px;border:1px solid #ccc;">
                    <h4 class="text-center">Entry form</h4>
                    <div class="col-md-3">
                        <label style="">
                            <asp:Label ID="LblParentOrderRefNo" runat="server" Text="Parent Order Number" CssClass="newLbl"></asp:Label>
                        </label>
                        <div style="">
                            <dxe:ASPxComboBox ID="ParentOrderNo" ClientInstanceName="cCmbddlOrderType" runat="server"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>
                            <asp:Label ID="LblOrderRefNo" runat="server" Text="Order RefNumber" CssClass="newLbl"></asp:Label>
                        </label>
                        <div>
                            <dxe:ASPxTextBox ID="txtOrder_RefNumber" Width="100%" runat="server">
                                <ClientSideEvents TextChanged="function(s,e){txtOrder_RefNumber_TextChange()}" />
                            </dxe:ASPxTextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>
                            <asp:Label ID="LblDate" runat="server" Text="Date" CssClass="newLbl"></asp:Label>
                        </label>
                        <div>
                            <dxe:ASPxDateEdit ID="txtTaxRates_DateFrom" runat="server" Width="100%" ClientInstanceName="ctxtTaxRates_DateFrom"
                                EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="true" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>
                            <asp:Label ID="LblType" runat="server" Text="Type" CssClass="newLbl"></asp:Label>
                        </label>
                        <div>
                            <dxe:ASPxComboBox ID="CmbddlOrderType" ClientInstanceName="cCmbddlOrderType" runat="server"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                <ClientSideEvents ValueChanged="function(s,e){OncCmbddlOrderType_ValueChange()}"></ClientSideEvents>
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>
                            <asp:Label ID="LblBranch" runat="server" Text="Branch" CssClass="newLbl"></asp:Label>
                        </label>
                        <div>
                            <dxe:ASPxComboBox ID="CmbpOrder_Branch" ClientInstanceName="cCmbpOrder_Branch" runat="server"
                                ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>
                            <dxe:ASPxLabel ID="lblUserOrderType" runat="server" Text="Order Type">
                            </dxe:ASPxLabel>
                        </label>
                        <div>
                            <asp:TextBox ID="txttype_UserAccount" Width="100%" autocomplete="off" runat="server"
                                autocorrect="off" autocapitalize="off" spellcheck="false" onkeyup="FunCallAjaxList_type_UserAccount(this,event,'Digital');"></asp:TextBox><%--onmouseout="onBlrTXT();"   --%>
                            <asp:TextBox ID="txttype_UserAccount_hidden" runat="server" Width="100px" Style="display: none"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>
                            <asp:Label ID="LblAgent" runat="server" Text="Agent" CssClass="newLbl"></asp:Label>
                        </label>
                        <div>
                            <asp:TextBox ID="txt_pOrder_AgentID" Width="100%" runat="server" autocomplete="off"
                                onkeyup="FunCallAjaxListpOrder2(this,event,'Digital');"></asp:TextBox>
                            <asp:TextBox ID="txt_pOrder_AgentID_hidden" runat="server" Width="100px" Style="display: none"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>
                            <asp:Label ID="LblPaymentTerm" runat="server" Text="Payment Term" CssClass="newLbl"></asp:Label>
                        </label>
                        <div>
                            <dxe:ASPxComboBox ID="CmbpOrder_PaymentTerm" ClientInstanceName="cCmbpOrder_PaymentTerm"
                                runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True"
                                EnableIncrementalFiltering="True">
                                <Items>
                                    <dxe:ListEditItem Value="0" Text="" />
                                    <dxe:ListEditItem Text="Days of Shipment" Value="X" />
                                    <dxe:ListEditItem Text="Free Sample" Value="S" />
                                    <dxe:ListEditItem Value="F" Text="Paid-Full" />
                                    <dxe:ListEditItem Text="Paid-Part" Value="P" />
                                    <dxe:ListEditItem Text="Payment on Delivery" Value="D" />
                                    <dxe:ListEditItem Text="Payment after Delivery" Value="C" />
                                </Items>
                                <ClientSideEvents TextChanged="function(s,e){PaymentTermChanged();}" />
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div class="clear"></div>
                    <div class="col-md-3">
                        <label id="lblPaymentDate">
                            <%--Payment Date--%>
                            <asp:Label ID="LblPaymentDate" runat="server" Text="Payment Date" CssClass="newLbl"></asp:Label>
                        </label>
                        <div id="dtPaymentDate">
                            <dxe:ASPxDateEdit ID="txtpOrder_PaymentDate" runat="server" Width="100%" ClientInstanceName="ctxtpOrder_PaymentDate"
                                EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="true" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label id="lblShipmentDate">
                            <%--payment After--%>
                            <asp:Label ID="LblPaymentAfter" runat="server" Text="Payment After" CssClass="newLbl"></asp:Label>
                        </label>
                        <div id="txtShipmentDate">
                            <dxe:ASPxTextBox ID="txtpOrder_PaymentDays" runat="server" Width="100%">
                            </dxe:ASPxTextBox>
                            <%--Days of shipment--%>
                            <asp:Label ID="LblDaysofShipment" runat="server" Text="Days of Shipment" CssClass="newLbl"></asp:Label>
                        </div>
                    </div>
                    
                    <div class="col-md-3">
                        <label>
                            <asp:Label ID="LblDeliveryDate" runat="server" Text="Delivery Date" CssClass="newLbl"></asp:Label>
                        </label>
                        <div>
                            <dxe:ASPxDateEdit ID="ASPxDateEdit1" runat="server" Width="100%" ClientInstanceName="ctxtTaxRates_DateFrom"
                                EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="true" />
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label>
                            <asp:Label ID="LblDeliveryAt" runat="server" Text="Delivery At" CssClass="newLbl"></asp:Label>
                        </label>
                        <div>
                            <%--When Type is 'P' Or 'R' then : Branch [B] / WareHouse [W] / Other Location [O]… When Type is 'S' Or 'O' then : Customer's Address [A] / Other Location [O]…. When Type is 'J' then : Vendor's Address [A] / Other Location [O]                    
                            --%>
                            <dxe:ASPxComboBox ID="ddlDeliveryAt_pOrderType" ClientInstanceName="cddlDeliveryAt_pOrderType"
                                runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True"
                                EnableIncrementalFiltering="True" OnCallback="ddlDeliveryAt_pOrderType_Callback">
                                <ClientSideEvents TextChanged="function(s,e){DeliveryTextChanged();}" />
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div style="clear:both"></div>
                    <div class="col-md-3">
                        <label id="dlblDeliveryBranch">
                            <%--Delivery Branch--%>
                            <asp:Label ID="LblDeliveryBrunch" runat="server" Text="Delivery Brunch" CssClass="newLbl"></asp:Label>
                        </label>
                        <div id="dtxtDeliveryBranch">
                            <dxe:ASPxComboBox ID="CmbpOrder_DeliveryBranch" ClientInstanceName="cCmbpOrder_DeliveryBranch"
                                runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True"
                                EnableIncrementalFiltering="True">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label id="dlblDeliveryWarehouse">
                            <%--Delivery WareHouse--%>
                            <asp:Label ID="LblDeliveryWareHouse" runat="server" Text="Delivery WareHouse" CssClass="newLbl"></asp:Label>
                        </label>
                        <div id="dtxtDeliveryWareHouse">
                            <dxe:ASPxComboBox ID="CmbpOrder_DeliveryWareHouse" ClientInstanceName="cCmbpOrder_DeliveryWareHouse"
                                runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True"
                                EnableIncrementalFiltering="True">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                         <label id="dlblDeliveryAddress">
                            <%--Delivery Address--%>
                            <asp:Label ID="LblDeliveryAddress" runat="server" Text="Delivery Address" CssClass="newLbl"></asp:Label>
                        </label>
                        <div id="dtxtDeliveryAddress">
                            <dxe:ASPxComboBox ID="CmbpOrder_DeliveryAddress" ClientInstanceName="cCmbpOrder_DeliveryAddress"
                                runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True"
                                EnableIncrementalFiltering="True" OnCallback="CmbpOrder_DeliveryAddress_Callback">
                            </dxe:ASPxComboBox>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <label id="dlblDeliveryOther">
                            <%--Delivery Other--%>
                            <asp:Label ID="LblDeliveryOther" runat="server" Text="Delivery Other" CssClass="newLbl"></asp:Label>
                        </label>
                        <div id="dtxtDeliveryOther">
                            <dxe:ASPxMemo ID="txtpOrder_DeliveryOther" ClientInstanceName="ctxtpOrder_DeliveryOther"
                                runat="server" Width="100%" >
                            </dxe:ASPxMemo>
                            <asp:HiddenField ID="pOderId_hidden" runat="server" />
                            <asp:HiddenField ID="DBEditId" runat="server" />
                        </div>
                    </div>
                    <div style="clear:both"></div>
                    <div class="col-md-3">
                        <label>
                            <asp:Label ID="LblOrderInstructions" runat="server" Text="Order Instructions" CssClass="newLbl"></asp:Label>
                        </label>
                        <div>
                            <dxe:ASPxMemo ID="ASPxMemo1" runat="server" Width="100%" Height="67px">
                            </dxe:ASPxMemo>
                        </div>
                    </div>
                    <div style="clear:both"></div>
                     <div class="col-md-12" style="padding-top:15px;">
                            <div style="display: inline-block; margin: 0 5px;" class="dxbBtn">
                                <asp:Button ID="btnsubmit" runat="server" CssClass="btn btn-primary" OnClientClick="return pOrder_btnsubmit_Click();" CcsClass="btn btn-primary"
                                    Text="Submit" OnClick="btnsubmit_Click" />
                            </div>
                            <div style="display: inline-block; margin: 0 5px" class="dxbBtn">
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-danger" Text="Cancel"  CcsClass="btn btn-danger"/>
                                <input type="button" value="Assing Value" class="btn btn-primary" onclick="fetchLebel()" />
                            </div>
                        </div>
                </div>
                
                <uc:spinner id="Spinner1" runat="server" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlSearch" runat="server">
            <div>
                
            </div>
            <br />
            <div>
                <uc:ucpord id="ucprod" runat="server" />

            </div>
        </asp:Panel>
        <dxe:ASPxPopupControl ID="AssignValuePopup" runat="server" ClientInstanceName="AssignValuePopup"
            Width="200px" HeaderText="Add / Edit Key Value" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentStyle VerticalAlign="Top" CssClass="pad">
            </ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl ID="AssignValuePopupContent" runat="server">
                    <div id="generatedForm">
                    </div>
                    <div id="SubmitFrm">
                        <asp:TextBox ID="KeyField" runat="server" Style="display: none;"></asp:TextBox>
                        <asp:TextBox ID="ValueField" runat="server" Style="display: none;"></asp:TextBox>
                        <asp:TextBox ID="RexPageName" runat="server" Style="display: none;"></asp:TextBox>
                        <asp:Button ID="Button1" runat="server" Text="Save" OnClientClick="return SaveDataToResource()"
                            OnClick="BTNSave_clicked" CssClass="btn btn-primary" Style="margin-left: 155px;" />
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <dxe:ASPxPopupControl ID="AssignValuePopupUC" runat="server" ClientInstanceName="AssignValuePopupUC"
            Width="200px" HeaderText="Add / Edit Key Value" PopupHorizontalAlign="WindowCenter"
            BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
            ContentStyle-CssClass="pad">
            <ContentStyle VerticalAlign="Top" CssClass="pad">
            </ContentStyle>
            <ContentCollection>
                <dxe:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                    <div id="generatedFormUC">
                    </div>
                    <div id="Div2">
                        <asp:TextBox ID="KeyFieldUC" runat="server" Style="display: block;"></asp:TextBox>
                        <asp:TextBox ID="ValueFieldUC" runat="server" Style="display: block;"></asp:TextBox>
                        <asp:TextBox ID="RexPageNameUC" runat="server" Style="display: block;"></asp:TextBox>
                        <asp:Button ID="BTNSaveUC" runat="server" Text="Save" OnClientClick="return SaveDataToResourceUC()" CssClass=""
                            OnClick="BTNSaveUC_clicked" Style="margin-left: 155px;" />
                    </div>
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
    </div>
</asp:Content>
