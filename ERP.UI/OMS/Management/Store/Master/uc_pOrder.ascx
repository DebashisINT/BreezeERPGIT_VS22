<%@ Control Language="C#" AutoEventWireup="true" Inherits="uc_pOrder" Codebehind="uc_pOrder.ascx.cs" %>
<%--<%@ Register Assembly="DevExpress.Web.v9.1.Export, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v9.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v9.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v9.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe000001" %>--%>
   <%-- <%@ Register Assembly="DevExpress.Web.v10.2.Export, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
     <%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>     
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>--%>

<script type="text/javascript" src="/assests/js/ajaxList_inner.js"></script>

<script type="text/javascript" src="/assests/js/init.js"></script>

<script type="text/javascript" src="/assests/js/jquery-1.3.2.min.js"></script>

<script type="text/javascript">
    $(document).ready(function() {
        SetMonthYear();
    });

    function fn_PopOpen() {
        //   document.getElementById('hiddenedit').value = "";
        //           alert('HidenEdit-'+GetObjectID('<%#hiddenedit.ClientID%>').value);
        // ctxtcityName.SetText('');
        //        ctxtMarkets_Code.SetText('');
        //        ctxtMarkets_Name.SetText('');
        //        ctxtMarkets_Description.SetText('');
        //        cCmbCountryName.SetValue("1");
        //        cCmbState.SetSelectedIndex(0);
        //        cCmbCity.SetSelectedIndex(0);

        //        ctxtMarkets_Address.SetText('');
        //        ctxtMarkets_Email.SetText('');
        //        ctxtMarkets_Phones.SetText('');
        //        ctxtMarkets_WebSite.SetText('');
        //        ctxtMarkets_ContactPerson.SetText('');


        //document.getElementById('Spinner1_Popup_Empcitys_txtProductDetails').focus();

        cPopup_Empcitys.Show();
        document.getElementById('Spinner1_Popup_Empcitys_txtProductDetails').focus();
    }
    function CheckSubmitValidation() {
        var Return1 = CheckMonthYearValidation();
        var Return2 = CheckPopupFormValidation();
        if (Return1 == false || Return2 == false) {
            return false;
        }

    }
    function CheckPopupFormValidation() {
        var PorductName = document.getElementById("Spinner1_Popup_Empcitys_txtProductDetails").value;
        var PorductId = document.getElementById("Spinner1_Popup_Empcitys_txtProductDetails_hidden").value;
        var CurrencyName = document.getElementById("Spinner1_Popup_Empcitys_txtQuoteCurrency").value;
        var CurrencyId = document.getElementById("Spinner1_Popup_Empcitys_txtQuoteCurrency_hidden").value;
        var Unit = document.getElementById("Spinner1_Popup_Empcitys_txtpUnit").value;
        var Per = document.getElementById("Spinner1_Popup_Empcitys_txtLotUnit").value;
        var PerUnitName = document.getElementById("Spinner1_Popup_Empcitys_txtUnit").value;
        var PerUnitId = document.getElementById("Spinner1_Popup_Empcitys_txtUnit_hidden").value;
        var Quantity = document.getElementById('Spinner1_Popup_Empcitys_txtQuantity').value;
        if (PorductName == "" || PorductName == null || PorductName === undefined) {
            alert("Please select a product");
            document.getElementById("Spinner1_Popup_Empcitys_txtProductDetails").focus();
            return false;
        }
        else if (PorductId == "" || PorductId == null || PorductId === undefined) {
            alert("Please select a Valid product");
            document.getElementById("Spinner1_Popup_Empcitys_txtProductDetails").focus();
            return false;
        }
        if (CurrencyName == "" || CurrencyName == null || CurrencyName === undefined) {
            alert("Please select currency");
            document.getElementById("Spinner1_Popup_Empcitys_txtQuoteCurrency").focus();
            return false;
        }
        else if (CurrencyId == "" || CurrencyId == null || CurrencyId === undefined) {
            alert("Please select a valid currency");
            document.getElementById("Spinner1_Popup_Empcitys_txtQuoteCurrency").focus();
            return false;
        }
        if (Unit == "" || Unit == null || Unit === undefined) {
            alert("Please enter value for unit");
            document.getElementById("Spinner1_Popup_Empcitys_txtpUnit").focus();
            return false;
        }
        if (Unit != "" || Unit != null) {
            if (isNaN(Unit)) {
                alert("Please enter numeric value for unit");
                document.getElementById("Spinner1_Popup_Empcitys_txtpUnit").focus();
                return false;
            }
        }

        if (Per == "" || Per == null || Per === undefined) {
            alert("Please enter value for per");
            document.getElementById("Spinner1_Popup_Empcitys_txtLotUnit").focus();
            return false;
        }
        if (Per % 1 != 0) {
            alert("Please enter integer value for per");
            document.getElementById("Spinner1_Popup_Empcitys_txtLotUnit").focus();
            return false;
        }
        if (Per != "" || Per != null) {
            if (isNaN(Per)) {
                alert("Please enter numeric value for per");
                document.getElementById("Spinner1_Popup_Empcitys_txtLotUnit").focus();
                return false;
            }
        }

        if (PerUnitName == "" || PerUnitName == null || PerUnitName === undefined) {
            alert("Please select per units");
            document.getElementById("Spinner1_Popup_Empcitys_txtUnit").focus();
            return false;
        }
        else if (PerUnitId == "" || PerUnitId == null || PerUnitId === undefined) {
            alert("Please select a Valid per Units");
            document.getElementById("Spinner1_Popup_Empcitys_txtUnit_hidden").focus();
            return false;
        }
        if (Quantity == "" || Quantity == null || Quantity === undefined) {
            alert("Please enter value for Quantity");
            document.getElementById("Spinner1_Popup_Empcitys_txtQuantity").focus();
            return false;
        }
        if (isNaN(Quantity)) {
            alert("Please enter numeric value for Quantity");
            document.getElementById("Spinner1_Popup_Empcitys_txtQuantity").focus();
            return false;
        }

    }
    function CheckMonthYearValidation() {
        var Notapplicable = document.getElementById("Spinner1_Popup_Empcitys_RadioButtonList1_0").checked;
        var Applicable = document.getElementById("Spinner1_Popup_Empcitys_RadioButtonList1_1").checked;
        if (Notapplicable) {
            return true;
        }
        else {
            var foo = document.getElementById("txtTaxRates_DateFrom_I").value.toString().trim();
            if (foo != "" && foo != null) {
                var Year = foo.split("-")[2];
                var month = foo.split("-")[1];
                if (cddlYear.GetValue() >= Year) {
                    if (Year == cddlYear.GetValue()) {
                        if (parseInt(month) >= cddlMonth.GetValue()) {
                            alert("Month/Year combination should not be Less than the pOrder_Date value");
                            return false;
                        }
                        else {
                            return true;
                        }
                    }
                    else { return true; }
                }
                else {
                    alert("Month/Year combination should not be Less than the pOrder_Date value");
                    return false;
                }
            }
            else {
                alert("Please enter Order Date first");
                return false;
            }
        }
    }

    function SetMonthYear() {
        var Notapplicable = document.getElementById("Spinner1_Popup_Empcitys_RadioButtonList1_0").checked;
        var Applicable = document.getElementById("Spinner1_Popup_Empcitys_RadioButtonList1_1").checked;
        if (Notapplicable) {
            cddlMonth.SetValue(0);
            cddlYear.SetValue(0);
            cddlMonth.SetEnabled(false);
            cddlYear.SetEnabled(false);
        }
        if (Applicable) {
            cddlMonth.SetValue(0);
            cddlYear.SetValue(0);
            cddlMonth.SetEnabled(true);
            cddlYear.SetEnabled(true);
        }
    }

    function FunCallAjaxList_UC(objID, objEvent, ObjType) {
        //alert("call");
        checkVal();
        txtChnagenTracker();
        var strQuery_Table = '';
        var strQuery_FieldName = '';
        var strQuery_WhereClause = '';
        var strQuery_OrderBy = '';
        var strQuery_GroupBy = '';
        var CombinedQuery = '';
        if (ObjType == 'Digital') {
            var CheckValue = document.getElementById('Spinner1_Popup_Empcitys_txtProductDetails_hidden').value;
            var contactId = document.getElementById("txttype_UserAccount_hidden").value;
            var txtTaxRates_MainAccount = document.getElementById('Spinner1_Popup_Empcitys_txtProductDetails').value;
            strQuery_Table = "master_sproducts";
            strQuery_FieldName = "sProducts_Name + ' ('+sProducts_Code+'~'+ isnull((SELECT [pCodeMap_Code] FROM  [dbo].[Trans_pCodeMap] where [pCodeMap_ProductID] = sProducts_ID and pCodeMap_ContactID='" + contactId + "'),'')+')'as name , isnull(CONVERT(varchar(10), sProducts_ID),'') +'_'+ isnull(CONVERT(varchar(10), sProducts_Size),'') as sProducts_ID";
            strQuery_WhereClause = "sProducts_Name + ' ('+sProducts_Code+'~'+ isnull((SELECT [pCodeMap_Code] FROM  [dbo].[Trans_pCodeMap] where [pCodeMap_ProductID] = sProducts_ID and pCodeMap_ContactID='" + contactId + "'),'')+')' like '%" + txtTaxRates_MainAccount + "%'";
        }
        CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
        //alert(CombinedQuery);
        ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars_UC(CombinedQuery), "Main");

        //ProductDetailsAutofillData();
    }
    function ProductDetailsAutofillData() {
        var id = document.getElementById('Spinner1_Popup_Empcitys_txtProductDetails_hidden').value;
        var originalId = id.split('_');
        var finalid = originalId[0];
        $.ajax({
            type: "POST",
            url: "pOrder.aspx/GetAutofillValue",
            data: "{'id':'" + finalid + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(msg) {
                var data = msg.d;
                var SetVal = data.split(',');
                //document.getElementById('Spinner1_Popup_Empcitys_txtQuoteCurrency').value = SetVal[1];
                //document.getElementById('Spinner1_Popup_Empcitys_txtQuoteCurrency_hidden').value = SetVal[0];
                //document.getElementById('Spinner1_Popup_Empcitys_txtpUnit').value = SetVal[2];
                document.getElementById('Spinner1_Popup_Empcitys_txtUnit').value = SetVal[4];
                document.getElementById('Spinner1_Popup_Empcitys_txtUnit_hidden').value = SetVal[3];
                document.getElementById('Spinner1_Popup_Empcitys_txtLotUnit').value = SetVal[2];
                document.getElementById('Spinner1_Popup_Empcitys_txtQntityunit').value = SetVal[4];
                document.getElementById('Spinner1_Popup_Empcitys_txtQntityunit_hidden').value = SetVal[3];
            }

        });
    }
    function FunCallAjaxList_UC_QntityUnit(objID, objEvent, ObjType) {

        var strQuery_Table = '';
        var strQuery_FieldName = '';
        var strQuery_WhereClause = '';
        var strQuery_OrderBy = '';
        var strQuery_GroupBy = '';
        var CombinedQuery = '';
        if (ObjType == 'Digital') {

            var txtUnit = document.getElementById('Spinner1_Popup_Empcitys_txtQntityunit').value;
            strQuery_Table = "Master_UOM";
            strQuery_FieldName = " UOM_Name,UOM_ID";
            strQuery_WhereClause = "UOM_Name like '" + txtUnit + "%'";
        }


        CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

        ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars_UC(CombinedQuery), "Main");

    }


    function FunCallAjaxList_UC_cur(objID, objEvent, ObjType) {

        var strQuery_Table = '';
        var strQuery_FieldName = '';
        var strQuery_WhereClause = '';
        var strQuery_OrderBy = '';
        var strQuery_GroupBy = '';
        var CombinedQuery = '';
        if (ObjType == 'Digital') {

            var txtQuoteCurrency = document.getElementById('Spinner1_Popup_Empcitys_txtQuoteCurrency').value;
            strQuery_Table = "master_currency";
            strQuery_FieldName = "Currency_Name,Currency_ID";
            strQuery_WhereClause = "Currency_Name like '" + txtQuoteCurrency + "%'";
        }


        CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

        ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars_UC(CombinedQuery), "Main");

    }

    function FunCallAjaxList_UC_Unit(objID, objEvent, ObjType) {
        var strQuery_Table = '';
        var strQuery_FieldName = '';
        var strQuery_WhereClause = '';
        var strQuery_OrderBy = '';
        var strQuery_GroupBy = '';
        var CombinedQuery = '';
        if (ObjType == 'Digital') {

            var txtUnit = document.getElementById('Spinner1_Popup_Empcitys_txtUnit').value;
            strQuery_Table = "Master_UOM";
            strQuery_FieldName = " UOM_Name,UOM_ID";
            strQuery_WhereClause = "UOM_Name like '" + txtUnit + "%'";
        }


        CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

        ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars_UC(CombinedQuery), "Main");

    }


    //function for checking value start

    var interval;
    var txtval = "";

    function checkVal() {

        if (document.getElementById('Spinner1_Popup_Empcitys_txtProductDetails_hidden').value != "") {
            //alert(document.getElementById('Spinner1_Popup_Empcitys_txtProductDetails_hidden').value);
            clearTimeout(interval);
            ProductDetailsAutofillData();
        }
        else {

            interval = setTimeout(function() { checkVal() }, 500);
        }
    }


    function txtChnagenTracker() {
        if (txtval != document.getElementById('Spinner1_Popup_Empcitys_txtProductDetails_hidden').value) {
            txtval = document.getElementById('Spinner1_Popup_Empcitys_txtProductDetails_hidden').value;
            ProductDetailsAutofillData();
        }
        var t = setTimeout(function() { txtChnagenTracker() }, 500);
    }





    //function for checking value end




    function replaceChars_UC(entry) {
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

    function fn_Deletecity2(keyValue) {
        pOrder_grid.PerformCallback('Delete~' + keyValue);
    }

    function fn_Editcity_pOrder(keyValue) {
        pOrder_grid.PerformCallback('Edit~' + keyValue);
    }
    function grid_EndCallBack_pOrder_grid2() {
        if (pOrder_grid.cpinsert != null) {
            if (pOrder_grid.cpinsert == 'Success') {
                alert('Inserted Successfully');
                cPopup_Empcitys.Hide();
            }
            else {
                alert("Error On Insertion \n 'Please Try Again!!'");
                cPopup_Empcitys.Hide();
            }
        }
        if (pOrder_grid.cpEdit != null) { 
            var month = pOrder_grid.cpEdit.split('|')[6];
            var year = pOrder_grid.cpEdit.split('|')[7];
            document.getElementById('Spinner1_Popup_Empcitys_txtProductDetails').value = pOrder_grid.cpEdit.split('|')[1];
            document.getElementById('Spinner1_Popup_Empcitys_txtProductDetails_hidden').value = pOrder_grid.cpEdit.split('|')[2];
            document.getElementById('Spinner1_Popup_Empcitys_txtProduct_Order_Detail_Brand').value = pOrder_grid.cpEdit.split('|')[3];
            cddlSize.SetValue(pOrder_grid.cpEdit.split('|')[4]);
            cddlColor.SetValue(pOrder_grid.cpEdit.split('|')[5]);
            cddlMonth.SetValue(pOrder_grid.cpEdit.split('|')[6]);
            cddlYear.SetValue(pOrder_grid.cpEdit.split('|')[7]);
            document.getElementById('Spinner1_Popup_Empcitys_txtpUnit').value = pOrder_grid.cpEdit.split('|')[9];
            document.getElementById('Spinner1_Popup_Empcitys_txtLotUnit').value = pOrder_grid.cpEdit.split('|')[10];
            document.getElementById('Spinner1_Popup_Empcitys_txtUnit').value = pOrder_grid.cpEdit.split('|')[17];
            document.getElementById('Spinner1_Popup_Empcitys_txtUnit_hidden').value = pOrder_grid.cpEdit.split('|')[18];
            document.getElementById('Spinner1_Popup_Empcitys_txtQuantity').value = pOrder_grid.cpEdit.split('|')[12];
            document.getElementById('Spinner1_Popup_Empcitys_txtQntityunit').value = pOrder_grid.cpEdit.split('|')[19];
            document.getElementById('Spinner1_Popup_Empcitys_txtQntityunit_hidden').value = pOrder_grid.cpEdit.split('|')[20];
            document.getElementById('Spinner1_Popup_Empcitys_txtAreaRemarks').value = pOrder_grid.cpEdit.split('|')[13];
            document.getElementById('Spinner1_Popup_Empcitys_txtQuoteCurrency').value = pOrder_grid.cpEdit.split('|')[16];
            document.getElementById('Spinner1_Popup_Empcitys_txtQuoteCurrency_hidden').value = pOrder_grid.cpEdit.split('|')[8];
            document.getElementById("Spinner1_Popup_Empcitys_txtProductDescription").value = pOrder_grid.cpEdit.split('|')[22];
            document.getElementById("Spinner1_Popup_Empcitys_txtRefNumber").value = pOrder_grid.cpEdit.split('|')[23];
            if (month != "" || year != "") {
                if (month != '0' || year != '0') {
                    document.getElementById('Spinner1_Popup_Empcitys_RadioButtonList1_1').checked = true;
                    cddlMonth.SetEnabled(true);
                    cddlYear.SetEnabled(true);
                }
                else {
                    document.getElementById('Spinner1_Popup_Empcitys_RadioButtonList1_0').checked = true;
                    cddlMonth.SetEnabled(false);
                    cddlYear.SetEnabled(false);
                }
            }
            else {
                document.getElementById('Spinner1_Popup_Empcitys_RadioButtonList1_0').checked = true;
                cddlMonth.SetEnabled(false);
                cddlYear.SetEnabled(false);
            }
            cPopup_Empcitys.Show();
        }
        if (pOrder_grid.cpUpdate != null) {
            if (pOrder_grid.cpUpdate == 'Success') {
                alert('Update Successfully');
                cPopup_Empcitys.Hide();
            }
            else {
                alert("Error on Updation\n'Please Try again!!'")
                cPopup_Empcitys.Hide();
            }
        }
        if (pOrder_grid.cpUpdateValid != null) {
            if (pOrder_grid.cpUpdateValid == "StateInvalid") {
                alert("Please Select proper country state and city");
                //cPopup_Empcitys.Show();
                //cCmbState.Focus();
                //alert(GetObjectID('<%#hiddenedit.ClientID%>').value);
                //grid.PerformCallback('Edit~'+GetObjectID('<%#hiddenedit.ClientID%>').value);
                //grid.cpUpdateValid=null;
            }
        }
        if (pOrder_grid.cpDelete != null) {
            if (pOrder_grid.cpDelete == 'Success')
                alert('Deleted Successfully');
            else
                alert("Error on deletion\n'Please Try again!!'")
        }
        if (pOrder_grid.cpExists != null) {
            if (pOrder_grid.cpExists == "Exists") {
                alert('Record already Exists');
                cPopup_Empcitys.Hide();
            }
            else {
                alert("Error on operation \n 'Please Try again!!'")
                cPopup_Empcitys.Hide();
            }
        }
    }
    
</script>
<script type="text/javascript">
    function fn_cPopup_Empcitys_PopUp() {
            var RefNo = document.getElementById("txtOrder_RefNumber_I").value;
            document.getElementById("Spinner1_Popup_Empcitys_txtRefNumber").value = RefNo; 
    }
</script>
<style>
    .dxbBtn a
    {
        padding: 3px;
    }
    #Spinner1_Popup_Empcitys_PW-1
    {
        top: 50px !important;
        width: 600px !important;
    }
    table#Spinner1_Popup_Empcitys_CLW-1, table#Spinner1_Popup_Empcitys_PWST-1, td.dxpcControl
    {
        width: 100% !important;
    }
    td#Spinner1_Popup_Empcitys_PWC-1
    {
        background: #fff !important;
    }
    table#Spinner1_Popup_Empcitys_RadioButtonList1 td label
    {
        display: inline-block !important;
    }
    table#Spinner1_Popup_Empcitys_RadioButtonList1 td input
    {
        display: inline-block !important;
        vertical-align: top;
        margin: 0 3px;
    }
    .txtA textarea
    {
        height: 50px;
        width: 167px;
    }
    #Spinner1_Popup_Empcitys_HCB-1Img img
    {
        display: block;
    }
</style>
<dxe:ASPxGridView ID="cityGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="pOrder_grid"
    KeyFieldName="RecordID" Width="100%" OnHtmlRowCreated="cityGrid_HtmlRowCreated"
    OnHtmlEditFormCreated="cityGrid_HtmlEditFormCreated" OnCustomCallback="cityGrid_CustomCallback">
    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
    <Columns>
        <dxe:GridViewDataTextColumn Caption="ProductDetails Name" FieldName="ProductDetailsName"
            ReadOnly="True" Visible="True" FixedStyle="Left" VisibleIndex="0">
            <EditFormSettings Visible="False" />
        </dxe:GridViewDataTextColumn>
        <dxe:GridViewDataTextColumn Caption="Detail Brand Name" FieldName="DetailBrandName"
            ReadOnly="True" Visible="True" FixedStyle="Left" VisibleIndex="0">
            <EditFormSettings Visible="False" />
        </dxe:GridViewDataTextColumn>
        <dxe:GridViewDataTextColumn Caption="Quote Currency Name" FieldName="QuoteCurrencyName"
            ReadOnly="True" Visible="True" FixedStyle="Left" VisibleIndex="1">
            <EditFormSettings Visible="False" />
        </dxe:GridViewDataTextColumn>
        <dxe:GridViewDataTextColumn ReadOnly="True" Width="9%">
            <HeaderTemplate>
                <a href="javascript:void(0);" onclick="fn_PopOpen()"><span style="color: #000099;
                    text-decoration: underline">Add New</span> </a>
            </HeaderTemplate>
            <DataItemTemplate>
                <a href="javascript:void(0);" onclick="fn_Editcity_pOrder('<%# Container.KeyValue %>')">
                    Edit</a> <a href="javascript:void(0);" onclick="fn_Deletecity2('<%# Container.KeyValue %>')">
                        Delete</a>
            </DataItemTemplate>
        </dxe:GridViewDataTextColumn>
    </Columns>
    <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack_pOrder_grid2();}" />
</dxe:ASPxGridView>
<div style="display: inline-block; margin: 0 5px;" class="dxbBtn">
    <asp:Button ID="BtnSave" runat="server" OnClick="btnSubmitAll_Click" Text="[S]Save" CssClass="btn btn-primary" />
</div>
<div style="display: inline-block; margin: 0 5px;" class="dxbBtn">
    <asp:Button ID="BtnCancel" runat="server" OnClick="btnCanAll_Click" Text="[C]Cancel" CssClass="btn btn-danger"/>
</div>
<div style="display: inline-block; margin: 0 5px;" class="dxbBtn">
    <asp:Button ID="btnDiscard" runat="server" OnClick="btnDiscard_Click" Text="[D]Discard" CssClass="btn btn-warning" />
</div>
<div class="PopUpArea">
    <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
        Width="400px" HeaderText="Add Order Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ClientSideEvents PopUp="function (s, e) {fn_cPopup_Empcitys_PopUp();}" />
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div style="width: 100%; background-color: #ccc; margin: 0px; overflow: hidden;">
                    <div class="Top">
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                            <asp:Label ID="lblRefNumberName" runat="server" Text="Reference No:" CssClass="newLblUC"></asp:Label>
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;"> 
                                <asp:TextBox ID="txtRefNumber" runat="server" Enabled="false" style="background-color: #000; color: #fff;font-size: 17px; width: 200px;"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <%--Product--%>
                                <asp:Label ID="LblUcProduct" runat="server" Text="Product" CssClass="newLblUC"></asp:Label>
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:TextBox Width="200px" runat="server" ID="txtProductDetails" autocomplete="off"
                                    onkeyup="FunCallAjaxList_UC(this,event,'Digital');"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtProductDetails_hidden" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <%--Brand--%>
                                <asp:Label ID="LblUcBrand" runat="server" Text="Brand" CssClass="newLblUC"></asp:Label>
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:TextBox Width="200px" runat="server" autocomplete="off" ID="txtProduct_Order_Detail_Brand"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtProduct_Order_Detail_Brand_hidden" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <%--Size/Strength--%>
                                <asp:Label ID="LblUcSize" runat="server" Text="Size/Strength" CssClass="newLblUC"></asp:Label>
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <dxe:ASPxComboBox Width="204px" ID="ddlSize" ClientInstanceName="cddlSize" runat="server"
                                    ValueType="System.String" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <%--Color--%>
                                <asp:Label ID="LblUcColor" runat="server" Text="Color" CssClass="newLblUC"></asp:Label>
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <dxe:ASPxComboBox Width="204px" ID="ddlColor" ClientInstanceName="cddlColor" runat="server"
                                    ValueType="System.String" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <%--BestBefore Mo/Yr--%>
                                <asp:Label ID="LblUcBestBefore" runat="server" Text="Best Before Mo/Yr" CssClass="newLblUC"></asp:Label>
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;
                                width: 200px">
                                <asp:RadioButtonList RepeatDirection="Horizontal" TextAlign="Left" ID="RadioButtonList1"
                                    runat="server">
                                    <asp:ListItem Selected="True" onclick="SetMonthYear();">Not Applicable</asp:ListItem>
                                    <asp:ListItem onclick="SetMonthYear();">Applicable</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <div style="float: left; margin-right: 5px;">
                                    <dxe:ASPxComboBox ID="ASPxMonth" ClientInstanceName="cddlMonth" runat="server" ValueType="System.String"
                                        Width="100px" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                        <Items>
                                            <dxe:ListEditItem Text="" Value="0" />
                                            <dxe:ListEditItem Text="Jan" Value="1" />
                                            <dxe:ListEditItem Text="Feb" Value="2" />
                                            <dxe:ListEditItem Text="MAR" Value="3" />
                                            <dxe:ListEditItem Text="APR" Value="4" />
                                            <dxe:ListEditItem Text="MAY" Value="5" />
                                            <dxe:ListEditItem Text="JUN" Value="6" />
                                            <dxe:ListEditItem Text="JUL" Value="7" />
                                            <dxe:ListEditItem Text="AUG" Value="8" />
                                            <dxe:ListEditItem Text="SEP" Value="9" />
                                            <dxe:ListEditItem Text="OCT" Value="10" />
                                            <dxe:ListEditItem Text="NOV" Value="11" />
                                            <dxe:ListEditItem Text="DEC" Value="12" />
                                        </Items>
                                    </dxe:ASPxComboBox>
                                </div>
                                <div style="float: left">
                                    <dxe:ASPxComboBox ID="ASPxYear" ClientInstanceName="cddlYear" runat="server" ValueType="System.String"
                                        Width="100px" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                    </dxe:ASPxComboBox>
                                </div>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <%--Quote Currency--%>
                                <asp:Label ID="LblUcQuoteCurr" runat="server" Text="Quote Currency" CssClass="newLblUC"></asp:Label>
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:TextBox runat="server" Width="200px" ID="txtQuoteCurrency" autocomplete="off"
                                    onkeyup="FunCallAjaxList_UC_cur(this,event,'Digital');"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtQuoteCurrency_hidden" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; padding: 5px 0; overflow: hidden; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <%--Unit Price--%>
                                <asp:Label ID="LblUcUnitPrice" runat="server" Text="Unit Price" CssClass="newLblUC"></asp:Label>
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <div style="float: left; margin: 0 3px">
                                    <asp:TextBox runat="server" ID="txtpUnit" Style="width: 52px"></asp:TextBox>
                                    &nbsp;/
                                </div>
                                <div style="float: left;">
                                    <asp:TextBox runat="server" ID="txtLotUnit" Style="width: 50px"></asp:TextBox>
                                </div>
                                <div style="float: left; margin: 0 2px">
                                    <asp:TextBox runat="server" ID="txtUnit" Style="width: 50px" autocomplete="off" onkeyup="FunCallAjaxList_UC_Unit(this,event,'Digital');"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtUnit_hidden" Style="width: 40px; display: none"></asp:TextBox>
                                    <%--Unit--%>
                                    <asp:Label ID="LblUcUnit" runat="server" Text="Unit" CssClass="newLblUC"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <%--Quantity--%>
                                <asp:Label ID="LblUcQuantity" runat="server" Text="Quantity" CssClass="newLblUC"></asp:Label>
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:TextBox runat="server" Width="200px" ID="txtQuantity"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <%--Quantity Unit--%>
                                <asp:Label ID="LblUcQuantityUnit" runat="server" Text="Quantity Unit" CssClass="newLblUC"></asp:Label>
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:TextBox runat="server" Width="200px" ID="txtQntityunit" autocomplete="off" onkeyup="FunCallAjaxList_UC_QntityUnit(this,event,'Digital');"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtQntityunit_hidden" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <%--Remarks--%>
                                <asp:Label ID="LblUcRemarks" runat="server" Text="Remarks" CssClass="newLblUC"></asp:Label>
                            </div>
                            <div class="Left_Content txtA" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:TextBox ID="txtAreaRemarks" Width="195px" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <%--Product Description--%>
                                <asp:Label ID="LblUcProductDescription" runat="server" Text="Product Description"
                                    CssClass="newLblUC"></asp:Label>
                            </div>
                            <div class="Left_Content txtA" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:TextBox ID="txtProductDescription" Width="195px" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv dxbBtn" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <asp:Button runat="server" ID="btnSubmit" Text="Save" OnClick="btnSubmit_Click" OnClientClick="javascript:return CheckSubmitValidation()" CssClass="btn btn-primary" />
                            </div>
                            <div class="cityDiv dxbBtn" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <asp:Button runat="server" ID="btnSavenClose" Text="Save & Close" OnClick="btnSavenClose_Click"
                                    OnClientClick="javascript:return CheckSubmitValidation()" CssClass="btn btn-danger"/>
                                <input type="button" value="Assing Value" onclick="fetchLebelUC()" />
                            </div>
                            <%--<div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:Button runat="server" ID="btnClear" Text="Cancel" />
                            </div>--%>
                        </div>
                    </div>
                    <%-- </div>--%>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    &nbsp;<dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>
</div>
<div class="HiddenFieldArea" style="display: none;">
    <asp:HiddenField runat="server" ID="hiddenedit" />
    <asp:HiddenField ID="hiddenApplicable" runat="server" />
</div>
