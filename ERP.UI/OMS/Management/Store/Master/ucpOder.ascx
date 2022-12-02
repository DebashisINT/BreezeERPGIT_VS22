<%@ Control Language="C#" AutoEventWireup="true" Inherits="Management_Store_Master_ucpOder" Codebehind="ucpOder.ascx.cs" %>
<%--<%@ Register Assembly="DevExpress.Web.v9.1.Export, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v9.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %> --%>
   <%-- <%@ Register Assembly="DevExpress.Web.v10.2.Export, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
 <%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>--%>
<style type="text/css">
    #ucprod_Popup_MoreInfo_PWST-1
    {
        top: -140px !important;
    }
</style>

<script type="text/javascript" src="/assests/js/ajaxList_inner.js"></script>

<script type="text/javascript" src="/assests/js/init.js"></script>

<script type="text/javascript">
    $(document).ready(function() {
        document.getElementById('ucprod_lblOrderType').innerHTML = "";

        if (cddlOrderType.GetValue() == 'P')
            document.getElementById('ucprod_lblOrderType').innerHTML = "Vendor/Supplier";
        else if (cddlOrderType.GetValue() == 'J')
            document.getElementById('ucprod_lblOrderType').innerHTML = "Vendor/Supplier";
        else if (cddlOrderType.GetValue() == 'S')
            document.getElementById('ucprod_lblOrderType').innerHTML = "Customer/Client";
        else if (cddlOrderType.GetValue() == 'O')
            document.getElementById('ucprod_lblOrderType').innerHTML = "Customer/Client";
        else if (cddlOrderType.GetValue() == 'R')
            document.getElementById('ucprod_lblOrderType').innerHTML = "Employee";
        CheckMoreInfo();
    });
    function CheckMoreInfo() {
        var CheckMoreInfo = document.getElementById("ucprod_cityGrid_cell1_12_hdnMoreInforCheck").value;
        if (CheckMoreInfo == "I" || CheckMoreInfo == "J") {
            document.getElementById("dvMoreInfo").style.display = "block";
        }
        else {
            document.getElementById("dvMoreInfo").style.display = "none";
        }
    }
    function CheckInfoFormPageLoad() {
        document.getElementById("ucprod_Popup_MoreInfo_RadioButtonList1_0").checked = true;
        cddlMonth.SetEnabled(false);
        cddlYear.SetEnabled(false);

    }
    function FunCallAjaxList_user(objID, objEvent, ObjType) {

        var strQuery_Table = '';
        var strQuery_FieldName = '';
        var strQuery_WhereClause = '';
        var strQuery_OrderBy = '';
        var strQuery_GroupBy = '';
        var CombinedQuery = '';
        var branchid = cCmbpOrder_Branch.GetValue();
        if (ObjType == 'Digital') {

            var ucprod_txtTaxRates_user = document.getElementById('ucprod_txtTaxRates_user').value;


            strQuery_Table = "tbl_master_contact";
            strQuery_FieldName = "cnt_firstname+' '+cnt_middlename+' '+cnt_lastname +' ('+cnt_internalId+')' as name , cnt_id";

            if (cddlOrderType.GetValue() == 'P')
                strQuery_WhereClause = "cnt_firstname like '" + ucprod_txtTaxRates_user + "%' and cnt_contacttype='VR' and cnt_branchid = '" + branchid + "'";
            else if (cddlOrderType.GetValue() == 'J')
                strQuery_WhereClause = "cnt_firstname like '" + ucprod_txtTaxRates_user + "%' and cnt_contacttype='VR' and cnt_branchid = '" + branchid + "'";
            else if (cddlOrderType.GetValue() == 'S')
                strQuery_WhereClause = "cnt_firstname like '" + ucprod_txtTaxRates_user + "%' and cnt_contacttype='CL' and cnt_branchid = '" + branchid + "'";
            else if (cddlOrderType.GetValue() == 'O')
                strQuery_WhereClause = "cnt_firstname like '" + ucprod_txtTaxRates_user + "%' and cnt_contacttype='CL' and cnt_branchid = '" + branchid + "'";
            else if (cddlOrderType.GetValue() == 'R')
                strQuery_WhereClause = "cnt_firstname like '" + ucprod_txtTaxRates_user + "%' and cnt_contacttype='EM' and cnt_branchid = '" + branchid + "'";
        }


        CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

        ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars1(CombinedQuery), "Main");

    }

    function replaceChars1(entry) {
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

    function FunCallAjaxListUser_AgentID2(objID, objEvent, ObjType) {

        var strQuery_Table = '';
        var strQuery_FieldName = '';
        var strQuery_WhereClause = '';
        var strQuery_OrderBy = '';
        var strQuery_GroupBy = '';
        var CombinedQuery = '';
        if (ObjType == 'Digital') {

            var ucprod_txtpOrder_User_AgentID = document.getElementById('ucprod_txtpOrder_User_AgentID').value;


            strQuery_Table = "tbl_master_contact";
            strQuery_FieldName = "cnt_firstname+' '+cnt_middlename+' '+cnt_lastname +' ('+cnt_internalId+')' as name , cnt_id";
            strQuery_WhereClause = "cnt_firstname like '" + ucprod_txtpOrder_User_AgentID + "%' ";
            //                if (cddlOrderType.GetValue() == 'P')
            //                    strQuery_WhereClause = "cnt_firstname like '" + txtTaxRates_MainAccount + "%' and cnt_contacttype='VR'";
            //                else if (cddlOrderType.GetValue() == 'J')
            //                    strQuery_WhereClause = "cnt_firstname like '" + txtTaxRates_MainAccount + "%' and cnt_contacttype='VR'";
            //                else if (cddlOrderType.GetValue() == 'S')
            //                    strQuery_WhereClause = "cnt_firstname like '" + txtTaxRates_MainAccount + "%' and cnt_contacttype='CL'";
            //                else if (cddlOrderType.GetValue() == 'O')
            //                    strQuery_WhereClause = "cnt_firstname like '" + txtTaxRates_MainAccount + "%' and cnt_contacttype='CL'";
            //                else if (cddlOrderType.GetValue() == 'R')
            //                    strQuery_WhereClause = "cnt_firstname like '" + txtTaxRates_MainAccount + "%' and cnt_contacttype='EM'";
        }


        CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

        ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars2(CombinedQuery), "Main");

    }

    function replaceChars2(entry) {
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
    function OncddlOrderType_ValueChange() {
        document.getElementById('ucprod_lblOrderType').innerHTML = "";

        if (cddlOrderType.GetValue() == 'P')
            document.getElementById('ucprod_lblOrderType').innerHTML = "Vendor/Supplier";
        else if (cddlOrderType.GetValue() == 'J')
            document.getElementById('ucprod_lblOrderType').innerHTML = "Vendor/Supplier";
        else if (cddlOrderType.GetValue() == 'S')
            document.getElementById('ucprod_lblOrderType').innerHTML = "Customer/Client";
        else if (cddlOrderType.GetValue() == 'O')
            document.getElementById('ucprod_lblOrderType').innerHTML = "Customer/Client";
        else if (cddlOrderType.GetValue() == 'R')
            document.getElementById('ucprod_lblOrderType').innerHTML = "Employee";

        cddlDeliveryAt.PerformCallback("bind_Delivery_At~" + cddlOrderType.GetValue());
    }

    function fn_Deletecity(keyValue) {
        grid_pOrderDetails.PerformCallback('Delete~' + keyValue);
    }
    function fn_Editcity(keyValue) {
        grid_pOrderDetails.PerformCallback('Edit~' + keyValue);

        //  document.getElementById('ucprod_txthid').value = keyValue;
    }



    function grid_pOrderDetails_EndCallBack() {
        if (grid_pOrderDetails.cpinsert != null) {
            if (grid_pOrderDetails.cpinsert == 'Success') {
                alert('Inserted Successfully');
                cPopup_Empcitys.Hide();
            }
            else {
                alert("Error On Insertion \n 'Please Try Again!!'");
                cPopup_Empcitys.Hide();
            }
        }
        if (grid_pOrderDetails.cpEdit != null) {

        }
        if (grid_pOrderDetails.cpUpdate != null) {
            if (grid_pOrderDetails.cpUpdate == 'Success') {
                alert('Update Successfully');
                cPopup_Empcitys.Hide();
            }
            else {
                alert("Error on Updation\n'Please Try again!!'")
                cPopup_Empcitys.Hide();
            }
        }
        if (grid_pOrderDetails.cpUpdateValid != null) {
            if (grid_pOrderDetails.cpUpdateValid == "StateInvalid") {
                alert("Please Select proper country state and city");
            }
        }
        if (grid_pOrderDetails.cpDelete != null) {
            if (grid_pOrderDetails.cpDelete == 'Success')
                alert('Deleted Successfully');
            else
                alert("Error on deletion\n'Please Try again!!'")
        }
        if (grid_pOrderDetails.cpExists != null) {
            if (grid_pOrderDetails.cpExists == "Exists") {
                alert('Record already Exists');
                cPopup_Empcitys.Hide();
            }
            else {
                alert("Error on operation \n 'Please Try again!!'")
                cPopup_Empcitys.Hide();
            }
        }
    }
    function SetMonthYear() {
        if (document.getElementById("ucprod_Popup_MoreInfo_RadioButtonList1_1").checked == true) {
            cddlMonth.SetEnabled(true);
            cddlYear.SetEnabled(true);
        }
        else if (document.getElementById("ucprod_Popup_MoreInfo_RadioButtonList1_0").checked == true) {
            cddlMonth.SetEnabled(false);
            cddlYear.SetEnabled(false);
        }

    }
    function fn_MoreDetails(Id) {
        Popup_MoreInfo.Show();
        document.getElementById("ucprod_hdnMoreInfoOrderId").value = Id;
        document.getElementById("ucprod_Popup_MoreInfo_txtProductDetails").value = "";
        document.getElementById("ucprod_Popup_MoreInfo_txtProduct_Order_Detail_Brand").value = "";
        cddlDeliveryAt.SetValue("0");
        cddlColor.SetValue("0");
        cddlMonth.SetValue("0");
        cddlYear.SetValue("0");
        document.getElementById("ucprod_Popup_MoreInfo_txtQuantity").value = "";
        document.getElementById("ucprod_Popup_MoreInfo_txtQntityunit").value = "";
        document.getElementById("ucprod_Popup_MoreInfo_txtAreaRemarks").value = "";
        document.getElementById("ucprod_Popup_MoreInfo_txtProductDescription").value = "";
        CheckInfoFormPageLoad();
        $.ajax({
            type: "POST",
            url: "pOrder.aspx/PageLoadMoreInfoGrigBind",
            data: "{'id':'" + Id + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function(data) {
                $("#ucprod_Popup_MoreInfo_grdMoreInfo").empty();
                if (data.d.length > 0) {
                    $("#ucprod_Popup_MoreInfo_grdMoreInfo").append("<tr>" +
                    "<th>Product Name</th>" +
                    "<th>Brand</th>" +
                    "<th>Size</th>" +
                    "<th>Color</th>" +
                    "<th>Quantity</th>" +
                    "<th>Unit</th>" +
                    "<th>Action</th>" +
                    "</tr>");
                    for (var i = 0; i < data.d.length; i++) {
                        $("#ucprod_Popup_MoreInfo_grdMoreInfo").append("<tr> <td>" +
                        data.d[i].JWorkStock_ProductName + "</td> <td>" +
                        data.d[i].JWorkStock_Brand + "</td> <td>" +
                        data.d[i].JWorkStock_SizeName + "</td> <td>" +
                        data.d[i].JWorkStock_ColorName + "</td> <td>" +
                        data.d[i].JWorkStock_Quantity + "</td> <td>" +
                        data.d[i].UOM_Name + "</td><td>" +
                        "<a href='javascript:void(0);' id='DeleteMoreInfo' onclick='fn_DeleteMoreInfo(" + data.d[i].JWorkStock_ID + ")'>Delete</a>" + "</td>" +
                        "</tr>");
                    }
                }
            }
        });

    }
    function fn_DeleteMoreInfo(id) {
        if (confirm("Are you sure delete the record?")) {
            $.ajax({
                type: "POST",
                url: "pOrder.aspx/DeleteMoreInfo",
                data: "{'id':'" + id + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data) {
                    if (data.d == true) {
                        alert("Record deleted successfully!");
                        var RecordId = document.getElementById("ucprod_hdnMoreInfoOrderId").value;
                        fn_MoreDetails(RecordId);
                    }
                }
            });
        }
        return false;

    }
</script>

<script type="text/javascript">
    function FunCallAjaxList_Product(objID, objEvent, ObjType) {
        var strQuery_Table = '';
        var strQuery_FieldName = '';
        var strQuery_WhereClause = '';
        var strQuery_OrderBy = '';
        var strQuery_GroupBy = '';
        var CombinedQuery = '';
        if (ObjType == 'Digital') {
            var ProductName = document.getElementById("ucprod_Popup_MoreInfo_txtProductDetails").value;
            strQuery_Table = "Master_sProducts";
            strQuery_FieldName = "sProducts_Name + ' ('+sProducts_Code+')' as Product_Name , sProducts_ID";
            strQuery_WhereClause = "sProducts_Name like '" + ProductName + "%'";
            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery), "Main");
        }
    }
    function FunCallAjaxList_UC_QntityUnit(objID, objEvent, ObjType) {
        var strQuery_Table = '';
        var strQuery_FieldName = '';
        var strQuery_WhereClause = '';
        var strQuery_OrderBy = '';
        var strQuery_GroupBy = '';
        var CombinedQuery = '';
        if (ObjType == 'Digital') {
            var UOM_Name = document.getElementById("ucprod_Popup_MoreInfo_txtQntityunit").value;
            strQuery_Table = "Master_UOM";
            strQuery_FieldName = "UOM_Name, UOM_ID";
            strQuery_WhereClause = "UOM_Name like '" + UOM_Name + "%'";
            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);
            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars(CombinedQuery), "Main");
        }
    }
    function CheckSubmitValidationMoreInfo() {
        var product = document.getElementById("ucprod_Popup_MoreInfo_txtProductDetails").value;
        var brand = document.getElementById("ucprod_Popup_MoreInfo_txtProduct_Order_Detail_Brand").value;
        var size = cddlDeliveryAt.GetValue();
        var color = cddlColor.GetValue();
        var month = cddlMonth.GetValue();
        var year = cddlYear.GetValue();
        var quantity = document.getElementById("ucprod_Popup_MoreInfo_txtQuantity").value;
        var quantityUnit = document.getElementById("ucprod_Popup_MoreInfo_txtQntityunit").value;
        if (product == "" || product == null || product === undefined) {
            alert("Please select value for product!");
            document.getElementById("ucprod_Popup_MoreInfo_txtProductDetails").focus();
            return false;
        }
        else if (brand == "" || brand == null || brand === undefined) {
            alert("Please enter value for brand");
            document.getElementById("ucprod_Popup_MoreInfo_txtProduct_Order_Detail_Brand").focus();
            return false;
        }
        else if (quantity == "" || quantity == null || quantity === undefined) {
            alert("Please enter value for quantity!");
            document.getElementById("ucprod_Popup_MoreInfo_txtQuantity").focus();
            return false;
        }
        else if (isNaN(quantity)) {
            alert("Please enter value numeric for quantity!");
            document.getElementById("ucprod_Popup_MoreInfo_txtQuantity").focus();
            return false;
        }
        else if (quantityUnit == "" || quantityUnit == null || quantityUnit === undefined) {
            alert("Please enter value for unit");
            document.getElementById("ucprod_Popup_MoreInfo_txtQntityunit").focus();
            return false;
        }
        else if (document.getElementById("ucprod_Popup_MoreInfo_RadioButtonList1_1").checked) {
            if (month == "0" || month == "" || month == null || month === undefined) {
                alert("Please select a value for best before month!");
                document.getElementById("ucprod_Popup_MoreInfo_ASPxMonth_I").focus();
                return false;
            }
            else if (year == "0" || year == "" || year == null || year === undefined) {
                alert("Please select a value for best before year!");
                document.getElementById("ucprod_Popup_MoreInfo_ASPxYear_I").focus();
                return false;
            }
        }
    }
</script>

<div id="divChangable">
    <div class="clearfix" style="background: #f5f4f3;padding:10px 0 22px 0;margin-bottom: 15px;border-radius:4px;border:1px solid #ccc;">
        <h4 class="text-center">Search Existing entry</h4>
        <div class="col-md-3">
            <label>
                Order Branch
            </label>
            <div>
                <dxe:ASPxComboBox ID="CmbpOrder_Branch" ClientInstanceName="cCmbpOrder_Branch" runat="server"
                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                </dxe:ASPxComboBox>
                        <asp:GridView runat="server" AutoGenerateColumns="False" 
                    EmptyDataText="No data in the data source." ID="grdMoreInfo" class="grdCommon"><Columns>
<asp:TemplateField HeaderText="Product Name"><ItemTemplate>
                                        <asp:Label ID="lblOrderCompany" runat="server" 
                                            Text='<%#Eval("JWorkStock_ProductName")%>'></asp:Label>
                                    
</ItemTemplate>

<HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Brand"><ItemTemplate>
                                        <asp:Label ID="lblOrderCompany" runat="server" 
                                            Text='<%#Eval("JWorkStock_Brand")%>'></asp:Label>
                                    
</ItemTemplate>

<HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Size"><ItemTemplate>
                                        <asp:Label ID="lblOrderCompany" runat="server" 
                                            Text='<%#Eval("JWorkStock_SizeName")%>'></asp:Label>
                                    
</ItemTemplate>

<HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Color"><ItemTemplate>
                                        <asp:Label ID="lblOrderCompany" runat="server" 
                                            Text='<%#Eval("JWorkStock_ColorName")%>'></asp:Label>
                                    
</ItemTemplate>

<HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Quantity"><ItemTemplate>
                                        <asp:Label ID="lblOrderCompany" runat="server" 
                                            Text='<%#Eval("JWorkStock_Quantity")%>'></asp:Label>
                                    
</ItemTemplate>

<HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Unit"><ItemTemplate>
                                        <asp:Label ID="lblOrderCompany" runat="server" Text='<%#Eval("UOM_Name")%>'></asp:Label>
                                    
</ItemTemplate>

<HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:TemplateField>
<asp:TemplateField HeaderText="Action"><ItemTemplate>
                                        <a href="javascript:void(0);" id="DeleteMoreInfo" 
                                            onclick='fn_DeleteMoreInfo(<%#Eval("JWorkStock_ID")%>)'>
                                            Delete</a>
                                    
</ItemTemplate>

<HeaderStyle HorizontalAlign="Left" ForeColor="Black"></HeaderStyle>

<ItemStyle HorizontalAlign="Left"></ItemStyle>
</asp:TemplateField>
</Columns>
</asp:GridView>

            </div>
        </div>
        <div class="col-md-3">
            <label>
                Order Date
            </label>
            <div>
                <dxe:ASPxDateEdit ID="txtpOrder_Date" runat="server" Width="100%" ClientInstanceName="ctxtpOrder_Date"
                    EditFormat="Custom" EditFormatString="dd/MM/yyyy" UseMaskBehavior="true" />
            </div>
        </div>
        <div class="col-md-3">
            <label>
                Order Type
            </label>
            <div>
                <dxe:ASPxComboBox ID="ddlOrderType" ClientInstanceName="cddlOrderType" runat="server"
                    ValueType="System.String" Width="100%" EnableSynchronization="True" EnableIncrementalFiltering="True">
                    <ClientSideEvents ValueChanged="function(s,e){OncddlOrderType_ValueChange()}"></ClientSideEvents>
                </dxe:ASPxComboBox>
            </div>
        </div>
        <div class="col-md-3">
            <label>
                <dxe:ASPxLabel ID="lblOrderType" runat="server">
                </dxe:ASPxLabel>
            </label>
            <div>
                <asp:TextBox ID="txtTaxRates_user" Width="100%" runat="server" autocomplete="off" onkeyup="FunCallAjaxList_user(this,event,'Digital');"></asp:TextBox>
                <asp:TextBox ID="txtTaxRates_user_hidden" runat="server" Width="100%" Style="display: none"></asp:TextBox>
            </div>
        </div>
        <div style="clear:both"></div>
        <div class="col-md-3">
            <label>
                Order RefNumber
            </label>
            <div>
                <dxe:ASPxTextBox ID="txtOrder_RefNumber" Width="100%" runat="server">
                </dxe:ASPxTextBox>
            </div>
        </div>
        <div class="col-md-3">
            <label>
                Order AgentID
            </label>
            <div>
                <asp:TextBox ID="txtpOrder_User_AgentID" Width="100%" runat="server" autocomplete="off" onkeyup="FunCallAjaxListUser_AgentID2(this,event,'Digital');"></asp:TextBox>
                <asp:TextBox ID="txtpOrder_User_AgentID_hidden" runat="server" Width="100%" Style="display: none"></asp:TextBox>
            </div>
        </div>
        <div class="col-md-3">
            <label>
                Delivery Branch
            </label>
            <div>
                <%--When Type is 'P' Or 'R' then : Branch [B] / WareHouse [W] / Other Location [O]… When Type is 'S' Or 'O' then : Customer's Address [A] / Other Location [O]…. When Type is 'J' then : Vendor's Address [A] / Other Location [O]--%>
                <dxe:ASPxComboBox ID="CmbpOrder_DeliveryBranch" ClientInstanceName="cCmbpOrder_DeliveryBranch"
                    runat="server" ValueType="System.String" Width="100%" EnableSynchronization="True"
                    EnableIncrementalFiltering="True">
                </dxe:ASPxComboBox>
            </div>
        </div>
        <div style="clear:both"></div>
        <div class="col-md-12">
            <asp:Button ID="btnsubmit" runat="server" Text="Submit" OnClick="btnsubmit_OnClick" CssClass="btn btn-primary" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" />
        </div>
    </div>
    <table>
        <tr>
            
            
            
            
            
        </tr>
        <tr>
            
            
            <%--<td>
                    Order Instructions
                </td>
                <td>
                    <dxe:ASPxMemo ID="ASPxMemo1" runat="server" Width="100%" Height="60px">
                    </dxe:ASPxMemo>
                </td>--%>
            <%--<td>
                    Order Payment Term
                </td>
                <td>
                    <dxe:ASPxComboBox ID="CmbpOrder_PaymentTerm" ClientInstanceName="cCmbpOrder_PaymentTerm"
                        runat="server" ValueType="System.String" Width="180px" EnableSynchronization="True"
                        EnableIncrementalFiltering="True">
                        <Items>
                            <dxe:ListEditItem Value="0" Text=""></dxe:ListEditItem>
                            <dxe:ListEditItem Value="P" Text="Paid-Full"></dxe:ListEditItem>
                            <dxe:ListEditItem Text="Paid-Part" Value="P" />
                            <dxe:ListEditItem Text="Payment on Delivery" Value="D" />
                            <dxe:ListEditItem Text="Payment after Delivery" Value="C" />
                            <dxe:ListEditItem Text="Free Sample" Value="S" />
                        </Items>
                    </dxe:ASPxComboBox>
                </td>--%>
            <%--<td>
                    Payment Due Date
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="txtpOrder_DeliveryDate" runat="server" Width="180" ClientInstanceName="ctxtpOrder_DeliveryDate"
                        EditFormat="Custom" EditFormatString="dd/MM/yyyy" UseMaskBehavior="true" />
                </td>--%>
            <%--<td>
                    Order Payment Date
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="txtpOrder_PaymentDate" runat="server" Width="180" ClientInstanceName="ctxtpOrder_PaymentDate"
                        EditFormat="Custom" EditFormatString="dd/MM/yyyy" UseMaskBehavior="true" />
                </td>--%>
        </tr>
        <tr>
            <%--When Type is 'P' Or 'R' then : Branch [B] / WareHouse [W] / Other Location [O]… When Type is 'S' Or 'O' then : Customer's Address [A] / Other Location [O]…. When Type is 'J' then : Vendor's Address [A] / Other Location [O]--%>
            <%--<td>
                    Delivery At
                </td>
                <td> 
                    <dxe:ASPxComboBox ID="ddlDeliveryAt" ClientInstanceName="cddlDeliveryAt" runat="server"
                        ValueType="System.String" Width="180px" EnableSynchronization="True" EnableIncrementalFiltering="True" OnCallback="ddlDeliveryAt_Callback">
                    </dxe:ASPxComboBox>
                </td>--%>
            <%--When Type is 'P' Or 'R' then : Branch [B] / WareHouse [W] / Other Location [O]… When Type is 'S' Or 'O' then : Customer's Address [A] / Other Location [O]…. When Type is 'J' then : Vendor's Address [A] / Other Location [O]--%>
            <%--<td>
                    Delivery WareHouse
                </td>
                <td> 
                    <dxe:ASPxComboBox ID="CmbpOrder_DeliveryWareHouse" ClientInstanceName="cCmbpOrder_DeliveryWareHouse"
                        runat="server" ValueType="System.String" Width="180px" EnableSynchronization="True"
                        EnableIncrementalFiltering="True">
                    </dxe:ASPxComboBox>
                </td>--%>
            <%--When Type is 'P' Or 'R' then : Branch [B] / WareHouse [W] / Other Location [O]… When Type is 'S' Or 'O' then : Customer's Address [A] / Other Location [O]…. When Type is 'J' then : Vendor's Address [A] / Other Location [O]--%>
            <%--<td>
                    Delivery Address
                </td>
                <td> 
                    <dxe:ASPxComboBox ID="CmbpOrder_DeliveryAddress" ClientInstanceName="cCmbpOrder_DeliveryAddress"
                        runat="server" ValueType="System.String" Width="180px" EnableSynchronization="True"
                        EnableIncrementalFiltering="True">
                    </dxe:ASPxComboBox>
                </td>--%>
            <%--<td>
                    Delivery Other
                </td>
                <td>
                    <dxe:ASPxMemo ID="txtpOrder_DeliveryOther" ClientInstanceName="ctxtpOrder_DeliveryOther"
                        runat="server" Width="100%" Height="60px">
                    </dxe:ASPxMemo>
                </td>--%>
        </tr>
        <tr>
            <td>
                
            </td>
        </tr>
    </table>
</div>
<div>
    <dxe:ASPxGridView ID="cityGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid_pOrderDetails"
        KeyFieldName="pOrder_ID" Width="100%" OnHtmlRowCreated="cityGrid_HtmlRowCreated"
        OnCustomCallback="cityGrid_CustomCallback" OnHtmlEditFormCreated="cityGrid_HtmlEditFormCreated"
        OnAfterPerformCallback="pOrderDetails_OnAfterPerformCallback">
        <SettingsPager Mode="ShowPager" PageSize="7" />
        <Columns>
            <%--pOrderID--%>
            <dxe:GridViewDataTextColumn Caption="pOrder_ID" FieldName="pOrder_ID" ReadOnly="True"
                Visible="False" FixedStyle="Left" VisibleIndex="0">
                <EditFormSettings Visible="False" />
            </dxe:GridViewDataTextColumn>
            <%--pOrderCompany--%>
            <dxe:GridViewDataTextColumn Caption="Company" Width="15%" FieldName="cmp_Name"
                FixedStyle="Left" Visible="false" VisibleIndex="1">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>
            <%--pOrder_Branch--%>
            <dxe:GridViewDataTextColumn Caption="Order Branch" FieldName="pOrder_Branch" FixedStyle="Left"
                Visible="False" Width="15%" VisibleIndex="2">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>
            <%--pOrder_Date--%>
            <dxe:GridViewDataTextColumn Caption="ORDER NO" FieldName="pOrder_Number" FixedStyle="Left"
                Visible="false" Width="15%" VisibleIndex="3">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>
            
            <dxe:GridViewDataTextColumn Caption="ORDER DATE" FieldName="pOrder_Date" FixedStyle="Left"
                Visible="True" Width="15%" VisibleIndex="4">
                <PropertiesTextEdit DisplayFormatString="dd-mm-yyyy" />
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>
            
            <%--pOrder_FinYear--%>
            <%--<dxe:GridViewDataTextColumn Caption="Financial Year" FieldName="pOrder_FinYear"
                FixedStyle="Left" Visible="True" Width="15%" VisibleIndex="4">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>--%>
            <%--pOrder_Type--%>
            <dxe:GridViewDataTextColumn Caption="Order Type" FieldName="pOrder_Type" FixedStyle="Left"
                Visible="false" Width="15%" VisibleIndex="5">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>
            <%--pOrder_ContactID--%>
            <%-- <dxe:GridViewDataTextColumn Caption="Contact Code" FieldName="pOrder_ContactID"
                FixedStyle="Left" Visible="True" Width="15%" VisibleIndex="6">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>--%>
            <%--pOrder_RefNumber--%>
            <dxe:GridViewDataTextColumn Caption="ORDER NUMBER" FieldName="pOrder_RefNumber"
                FixedStyle="Left" Visible="True" Width="15%" VisibleIndex="7">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>
            <%--pOrder_Number--%>
            
            <%--pOrder_Instructions--%>
            <dxe:GridViewDataTextColumn Caption="CUSTOMER / VENDOR NAME" FieldName="pOrder_Vendor"
                FixedStyle="Left" Visible="True" Width="15%" VisibleIndex="10">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>
            <%--pOrder_Product--%>
            <dxe:GridViewDataTextColumn Caption="PRODUCTS" FieldName="pOrder_Product"
                FixedStyle="Left" Visible="True" Width="15%" VisibleIndex="11">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>
            <%--pOrder_Color--%>
            <dxe:GridViewDataTextColumn Caption="COLOR" FieldName="pOrder_Color"
                FixedStyle="Left" Visible="True" Width="15%" VisibleIndex="12">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>
            
            <%--pOrder_Size--%>
            <dxe:GridViewDataTextColumn Caption="WIDTH" FieldName="pOrder_Size"
                FixedStyle="Left" Visible="True" Width="15%" VisibleIndex="13">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>
            
              <dxe:GridViewDataTextColumn Caption="ORDER QTY" FieldName="pOrderDetail_Quantity"
                FixedStyle="Left" Visible="True" Width="15%" VisibleIndex="13">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>
            
            <%--pOrder_DeliveryDate--%>
           <%-- <dxe:GridViewDataTextColumn Caption="Delivery Date" FieldName="pOrder_DeliveryDate"
                FixedStyle="Left" Visible="True" Width="15%" VisibleIndex="13">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>--%>
            <%--pOrder_DeliveryAt--%>
            <%-- <dxe:GridViewDataTextColumn Caption="Delivery At" FieldName="pOrder_DeliveryAt"
                FixedStyle="Left" Visible="True" Width="15%" VisibleIndex="14">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>--%>
            <%--pOrder_DeliveryBranch--%>
            <%-- <dxe:GridViewDataTextColumn Caption="Delivery Branch" FieldName="pOrder_DeliveryBranch"
                FixedStyle="Left" Visible="True" Width="15%" VisibleIndex="15">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>--%>
            <%--pOrder_DeliveryWareHouse--%>
            <%--  <dxe:GridViewDataTextColumn Caption="Delivery WareHouse" FieldName="pOrder_DeliveryWareHouse"
                FixedStyle="Left" Visible="True" Width="15%" VisibleIndex="16">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn>--%>
            <%--pOrder_DeliveryAddress--%>
            <%--  <dxe:GridViewDataTextColumn Caption="Delivery Address" FieldName="pOrder_DeliveryAddress"
                FixedStyle="Left" Visible="True" Width="15%" VisibleIndex="17">
                <EditFormSettings Visible="True" />
            </dxe:GridViewDataTextColumn> --%>
            <dxe:GridViewDataTextColumn VisibleIndex="4" ReadOnly="True" Width="30%">
                <HeaderTemplate>
                   ADD / DELETE
                </HeaderTemplate>
                <DataItemTemplate>
                    <a href="javascript:void(0);" onclick="fn_Editcity('<%# Container.KeyValue %>')">Edit</a>
                    <a href="javascript:void(0);" onclick="fn_Deletecity('<%# Container.KeyValue %>')">Delete</a>
                </DataItemTemplate>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="4" ReadOnly="True" Width="30%">
                <DataItemTemplate>
                    <div id="dvMoreInfo">
                    
                    <%# (Eval("pOrder_Type_Type").ToString().Equals("J") || Eval("pOrder_Type_Type").ToString().Equals("I")) ? "<a href =# onclick='fn_MoreDetails(" + Container.KeyValue + ")'> Add Items </a>  " : "N/A"%>
                    
                         
                    </div>
                </DataItemTemplate>
            </dxe:GridViewDataTextColumn>
        </Columns>
        <ClientSideEvents EndCallback="function (s, e) {grid_pOrderDetails_EndCallBack();}" />
    </dxe:ASPxGridView>
</div>
<div class="PopUpArea">
    <dxe:ASPxPopupControl ID="Popup_MoreInfo" runat="server" ClientInstanceName="Popup_MoreInfo"
        Width="400px" HeaderText="Add MoreInfo Details" PopupHorizontalAlign="WindowCenter"
        BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
        ContentStyle-CssClass="pad">
        <ContentStyle VerticalAlign="Top" CssClass="pad">
        </ContentStyle>
        <ContentCollection>
            <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div style="width: 100%; background-color: #ccc; margin: 0px; overflow: hidden;">
                    <div class="Top">
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                Product
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:TextBox Width="200px" runat="server" ID="txtProductDetails" autocomplete="off" onkeyup="FunCallAjaxList_Product(this,event,'Digital');"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtProductDetails_hidden" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                Brand
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:TextBox Width="200px" runat="server" ID="txtProduct_Order_Detail_Brand" autocomplete="off"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtProduct_Order_Detail_Brand_hidden" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                Size/Strength
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <dxe:ASPxComboBox Width="204px" ID="ddlSize" ClientInstanceName="cddlDeliveryAt"
                                    runat="server" ValueType="System.String" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                </dxe:ASPxComboBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                Color
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
                                BestBefore Mo/Yr
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
                                            <dxe:ListEditItem Text="None" Value="0" />
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
                                Quantity
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:TextBox runat="server" Width="200px" ID="txtQuantity"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                Quantity Unit
                            </div>
                            <div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:TextBox runat="server" Width="200px" ID="txtQntityunit" autocomplete="off" onkeyup="FunCallAjaxList_UC_QntityUnit(this,event,'Digital');"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtQntityunit_hidden" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                Remarks
                            </div>
                            <div class="Left_Content txtA" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:TextBox ID="txtAreaRemarks" Width="195px" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                Product Description
                            </div>
                            <div class="Left_Content txtA" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:TextBox ID="txtProductDescription" Width="195px" runat="server" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div style="margin: 10px 0; clear: both; overflow: hidden">
                            <div class="cityDiv dxbBtn" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_OnClick" Text="Save" OnClientClick="javascript:return CheckSubmitValidationMoreInfo()" />
                            </div>
                            <div class="cityDiv dxbBtn" style="display: inline-block; height: auto; margin-left: 20px;
                                float: left;">
                                <asp:Button runat="server" ID="btnSavenClose" OnClick="btnSavenClose_OnClick" Text="Save & Close"
                                    OnClientClick="javascript:return CheckSubmitValidationMoreInfo()" />
                            </div>
                            <%--<div class="Left_Content" style="display: inline-block; float: right; margin-right: 20px;">
                                <asp:Button runat="server" ID="btnClear" Text="Cancel" />
                            </div>--%>
                        </div>
                    </div>
                    <div>
                    </div>
                </div>
            </dxe:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle BackColor="LightGray" ForeColor="Black" />
    </dxe:ASPxPopupControl>
    &nbsp;<dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>
    <asp:HiddenField ID="hdnMoreInfoOrderId" runat="server" />
</div>
