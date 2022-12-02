<%@ Page Title="Open inventory" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_OpenInventory" CodeBehind="OpenInventory.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v9.1.Export, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v9.1" Namespace="DevExpress.Web"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v9.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v9.1, Version=9.1.2.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>--%>

    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>

    <script src="JSFUNCTION/ajaxList_management.js" type="text/javascript"></script>

    <script src="/assests/js/init.js" type="text/javascript"></script>

    <script src="/assests/js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <link href="../css/style.css" rel="stylesheet" type="text/css" />

    <script src="/assests/js/jquery-1.3.2.js" type="text/javascript"></script>

    <style type="text/css">
        .cityDiv {
            height: 25px;
            width: 155px;
            float: left;
            margin-left: 70px;
        }

        .cityTextbox {
            height: 25px;
            width: 50px;
        }

        .Top {
            height: 90px;
            width: 400px;
            background-color: Silver;
            padding-top: 5px;
            valign: top;
        }

        .Footer {
            height: 30px;
            width: 400px;
            background-color: Silver;
            padding-top: 10px;
        }

        .ScrollDiv {
            height: 250px;
            width: 400px;
            background-color: Silver;
            overflow-x: hidden;
            overflow-y: scroll;
        }

        .ContentDiv {
            width: 400px;
            height: 300px;
            border: 2px;
            background-color: Silver;
        }

       
        .TitleArea {
            height: 20px;
            padding-left: 10px;
            padding-right: 3px;
            background-image: url( '../images/EHeaderBack.gif' );
            background-repeat: repeat-x;
            background-position: bottom;
            text-align: center;
        }

        .FilterSide {
            float: left;
            padding-left: 15px;
            width: 50%;
        }

        .SearchArea {
            width: 100%;
            height: 30px;
            padding-top: 5px;
        }
        /* Big box with list of options */ #ajax_listOfOptions {
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

            #ajax_listOfOptions div {
                /* General rule for both .optionDiv and .optionDivSelected */
                margin: 1px;
                padding: 1px;
                cursor: pointer;
                font-size: 0.9em;
            }

            #ajax_listOfOptions .optionDiv {
                /* Div for each item in list */
            }

            #ajax_listOfOptions .optionDivSelected {
                /* Selected item in the list */
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

        .form-lebel {
            display: inline-block;
            width: 30%;
            height: 20px;
            padding-top: 10px;
            margin-left: 60px;
            vertical-align: top;
            font-size: 16px;
            font-family: Arial, Helvetica, sans-serif;
            border-right: 2px solid #FFFFFF;
        }

        .form-field {
            display: inline-block;
            width: 60%;
            padding-top: 5px;
            height: 20px;
            vertical-align: top;
        }

        .form-wrapper {
            border: 1px solid #FFFFFF;
        }
    </style>
    <title></title>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#Popup_Empcitys_HCB-1Img').hide();
            if (cddlLocation.GetValue() == '0')
                $('#grdActive_ctl01_linkbtn').attr('Style', 'display:none');
            else
                $('#grdActive_ctl01_linkbtn').attr('Style', 'display:block');
        });
        function FunCallAjaxList_UC(objID, objEvent, ObjType) {

            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';
            if (ObjType == 'Digital') {

                var txtTaxRates_MainAccount = document.getElementById('Popup_Empcitys_txtProduct').value;
                strQuery_Table = "master_sproducts";
                strQuery_FieldName = "sProducts_Name +' ('+sProducts_Code+')' as name ,  sProducts_ID";
                strQuery_WhereClause = "sProducts_Name like '" + txtTaxRates_MainAccount + "%'";
            }

            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars_UC(CombinedQuery), "Main");

            //var CheckValue = document.getElementById('Spinner1_Popup_Empcitys_txtProductDetails_hidden').value;
            //ProductDetailsAutofillData();
        }

        function FunCallAjaxList_UC_Unit(objID, objEvent, ObjType) {
            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';
            if (ObjType == 'Digital') {

                var txtUnit = document.getElementById('Popup_Empcitys_txtUnit').value;
                strQuery_Table = "Master_UOM";
                strQuery_FieldName = " UOM_Name,UOM_ID";
                strQuery_WhereClause = "UOM_Name like '" + txtUnit + "%'";
            }


            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars_UC(CombinedQuery), "Main");

        }



        function FunCallAjaxList_Price_Unit(objID, objEvent, ObjType) {

            var strQuery_Table = '';
            var strQuery_FieldName = '';
            var strQuery_WhereClause = '';
            var strQuery_OrderBy = '';
            var strQuery_GroupBy = '';
            var CombinedQuery = '';
            if (ObjType == 'Digital') {

                var txtUnit = document.getElementById('Popup_Empcitys_txtpriceUnit').value;
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

                var txtQuoteCurrency = document.getElementById('Popup_Empcitys_txtCurrency').value;
                strQuery_Table = "master_currency";
                strQuery_FieldName = "Currency_Name,Currency_ID";
                strQuery_WhereClause = "Currency_Name like '" + txtQuoteCurrency + "%'";
            }


            CombinedQuery = new String(strQuery_Table + "$" + strQuery_FieldName + "$" + strQuery_WhereClause + "$" + strQuery_OrderBy + "$" + strQuery_GroupBy);

            ajax_showOptions(objID, 'GenericAjaxList', objEvent, replaceChars_UC(CombinedQuery), "Main");

        }

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

        function OpenPopup() {
            cPopup_Empcitys.Show();
        }
        function btnCancel_OnClientClick() {
            cPopup_Empcitys.Hide();
        }
        function ddlLocation_SelectedIndexChanged() {

            if (cddlLocation.GetValue() == '0')
                $('#grdActive_ctl01_linkbtn').attr('Style', 'display:none');
            else
                $('#grdActive_ctl01_linkbtn').attr('Style', 'display:block');
            $('#BtnBindGridCall').click();

        }
        function linkbtn_OnClientClick()
        { OpenPopup(); }

        function btnSave_OnClientClick() {



            var vPopup_Empcitys_txtProduct = document.getElementById('Popup_Empcitys_txtProduct').value;
            var vPopup_Empcitys_txtProduct_hidden = document.getElementById('Popup_Empcitys_txtProduct_hidden').value;
            var vPopup_Empcitys_txtBrand_I = document.getElementById('Popup_Empcitys_txtBrand_I').value;
            var vcddlSize = cddlSize.GetValue();
            var vcddlColor = cddlColor.GetValue();
            var vPopup_Empcitys_txtBatchNo_I = document.getElementById('Popup_Empcitys_txtBatchNo_I').value;
            var vcddlMonth = cddlMonth.GetValue();
            var vcddlYear = cddlYear.GetValue();
            var vctxtExpiryDate = ctxtExpiryDate.GetText();
            var vPopup_Empcitys_txtQuantity_I = document.getElementById('Popup_Empcitys_txtQuantity_I').value;
            var vPopup_Empcitys_txtUnit = document.getElementById('Popup_Empcitys_txtUnit').value;
            var vPopup_Empcitys_txtUnit_hidden = document.getElementById('Popup_Empcitys_txtUnit_hidden').value;
            var vPopup_Empcitys_txtPrice_I = document.getElementById('Popup_Empcitys_txtPrice_I').value;


            var vPopup_Empcitys_txtpricelot_I = document.getElementById('Popup_Empcitys_txtpricelot_I').value;

            var vPopup_Empcitys_txtpriceUnit = document.getElementById('Popup_Empcitys_txtpriceUnit').value;
            var vPopup_Empcitys_txtpriceUnit_hidden = document.getElementById('Popup_Empcitys_txtpriceUnit_hidden').value;

            var vPopup_Empcitys_txtCurrency = document.getElementById('Popup_Empcitys_txtCurrency').value;
            var vPopup_Empcitys_txtCurrency_hidden = document.getElementById('Popup_Empcitys_txtCurrency_hidden').value;

            var vctxtAquireDate = ctxtAquireDate.GetText();



            if (vPopup_Empcitys_txtProduct == "") {
                alert("Please enter product");
                return false;
            }
            if (vPopup_Empcitys_txtProduct_hidden == "") {
                alert("Please enter product properly");
                return false;
            }
            //            if (vPopup_Empcitys_txtBrand_I == "") {
            //                alert("Please enter brand");
            //                return false;
            //            }
            //            if (vcddlSize == "0") {
            //                alert("Please enter size");
            //                return false;
            //            }
            //            if (vcddlColor == "0") {
            //                alert("Please enter color");
            //                return false;
            //            }

            if (vPopup_Empcitys_txtBatchNo_I == "") {
                alert("Please enter batch no");
                return false;
            }
            //            if (vcddlMonth == "0") {
            //                alert("Please enter month");
            //                return false;
            //            }
            //            if (vcddlYear == "0") {
            //                alert("Please enter year");
            //                return false;
            //            }



            //            if (vctxtExpiryDate == "01-01-0100" || (vctxtExpiryDate!="")) {
            //                alert("Please enter expiry date");
            //                return false;
            //            }

            if (vPopup_Empcitys_txtQuantity_I == "") {
                alert("Please enter proper quentity");
                return false;
            }
            else {
                if (isNaN(vPopup_Empcitys_txtQuantity_I)) {
                    alert("Please enter proper value for Quantity");

                    return false;
                }
                if (vPopup_Empcitys_txtQuantity_I.indexOf('.') > -1) {
                    alert("Please enter Integer value for Quantity");

                    return false;
                }
            }
            if (vPopup_Empcitys_txtUnit == "") {
                alert("Please enter quantity unit");
                return false;
            }
            if (vPopup_Empcitys_txtUnit_hidden == "") {
                alert("Please enter proper quantity unit");
                return false;
            }

            var regex = /^\d+(\.\d{0,2})?$/g;

            //            if (vPopup_Empcitys_txtPrice_I == "") {
            //                alert("Please enter price");
            //                return false;
            //

            //}



            if (vPopup_Empcitys_txtPrice_I != "") {


                if (isNaN(vPopup_Empcitys_txtPrice_I)) {
                    alert("Please enter proper value for price");
                    return false;
                }
                if (!regex.test(vPopup_Empcitys_txtPrice_I)) {
                    alert('Please enter two decimal places price');
                    return false;
                }

                return true;
            }

            //            if (vPopup_Empcitys_txtpricelot_I == "") {
            //                alert("Please enter price lot");
            //                return false;
            //            }

            if (vPopup_Empcitys_txtpricelot_I != "") {

                if (isNaN(vPopup_Empcitys_txtpricelot_I)) {
                    alert("Please enter proper value for price lot");

                    return false;
                }
                if (vPopup_Empcitys_txtpricelot_I.indexOf('.') > -1) {
                    alert("Please enter Integer value for price lot");

                    return false;
                }

                return true;
            }


            if (vPopup_Empcitys_txtpriceUnit != "") {

                if (vPopup_Empcitys_txtpriceUnit_hidden == "") {
                    alert("Please enter proper price unit");
                    return false;
                }
            }




            //            if (vPopup_Empcitys_txtCurrency == "") {
            //                alert("Please enter currency");
            //                return false;
            //            }
            //            if (vPopup_Empcitys_txtCurrency_hidden == "") {
            //                alert("Please enter proper currency");
            //                return false;
            //            }
            if (vctxtAquireDate == "01-01-0100") {
                alert("Please enter aquire date");
                return false;
            }
        }


        function fn_DeleteOpening() {
            if (confirm("Are you sure delete the record?")) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="padding-top: 50px; background-color: #CED8E4; width: 100%">
        <div>
            <div class="form-wrapper">
                <div class="form-lebel">
                    Location
                </div>
                <div class="form-field">
                    <%--<dxe:ASPxTextBox ID="txtSize" runat="server" Width="50%">
                    </dxe:ASPxTextBox>--%>
                    <dxe:ASPxComboBox Width="204px" ID="ddlLocation" ClientInstanceName="cddlLocation"
                        runat="server" ValueType="System.String" EnableSynchronization="True" EnableIncrementalFiltering="True"
                        OnSelectedIndexChanged="ddlLocation_OnSelectedIndexChanged">
                        <ClientSideEvents SelectedIndexChanged="function(s,e){ddlLocation_SelectedIndexChanged();}" />
                    </dxe:ASPxComboBox>
                    <%--<asp:DropDownList ID="ddlLocation" runat="server" OnSelectedIndexChanged="ddlLocation_OnSelectedIndexChanged">
                    
                    </asp:DropDownList>--%>
                </div>
                <asp:Button ID="BtnBindGridCall" runat="server" Style="display: none;" Text="hi"
                    OnClick="BtnBindGridCall_OnClick" />
            </div>
        </div>
        <div>
            <asp:GridView ID="grdActive" runat="server" Width="100%" BorderColor="CornflowerBlue"
                ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
                BorderWidth="2px" CellPadding="4" ForeColor="#0000C0">
                <%--OnRowCreated="grdActive_RowCreated"--%>
                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                <Columns>
                    <asp:TemplateField HeaderText="Inventory_ID" Visible="false">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblpOrder_ID" runat="server" Text='<%# Eval("Inventory_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblStock_Company" runat="server" Text='<%# Eval("cmp_Name")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stock_FinYear">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblStock_FinYear" runat="server" Text='<%# Eval("Inventory_FinYear")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- <asp:TemplateField HeaderText="Details">
                    <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>                        
                        <a id="lnkbtnView" runat="server" href="#" onclick="DetailsPopUp(this.id)">View</a>                       
                    </ItemTemplate>
                </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Product">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblsProducts_Name" runat="server" Text='<%# Eval("sProducts_Name")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Size">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblStock_Size" runat="server" Text='<%# Eval("Size_Name")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Color" SortExpression="Color_Name">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblColor_Name" runat="server" Text='<%# Eval("Color_Name")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stock_BestBeforeMonth" SortExpression="Stock_BestBeforeMonth">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblStock_BestBeforeMonth" runat="server"></asp:Label>
                            <asp:HiddenField ID="hddStock_BestBeforeMonth" runat="server" Value='<%# Eval("Inventory_BestBeforeMonth")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stock_BestBeforeYear" SortExpression="Stock_BestBeforeYear">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <%-- <asp:HiddenField ID="hddStock_vBestBeforeYear" runat="server" Value='<%# Eval("Inventory_BestBeforeYear")%>' />--%>
                            <asp:Label ID="lblStock_BestBeforeYear" runat="server" Text='<%# Eval("Inventory_BestBeforeYear")%>'></asp:Label>
                            <%-- <asp:Label ID="lblStock_BestBeforeYearNew" runat="server" ></asp:Label>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stock_ExpiryDate" SortExpression="Stock_ExpiryDate">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblStock_ExpiryDate" runat="server" Text='<%# Eval("Inventory_ExpiryDate")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stock_Batch" SortExpression="Stock_Batch">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblStock_Batch" runat="server" Text='<%# Eval("Inventory_BatchNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UOM_Name" SortExpression="UOM_Name">
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblStock_QuantityUnit" runat="server" Text='<%# Eval("Quantity_UOM_Name")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                        <HeaderTemplate>
                            <%-- <asp:LinkButton ID="linkbtn" runat="server" Text="Add" OnClientClick="OpenPopup()"></asp:LinkButton>--%>
                            <a id="linkbtn" href="#" runat="server" onclick="OpenPopup()">Add</a>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="linkbtn" runat="server" Text="Edit" OnClientClick="linkbtn_OnClientClick();"
                                CommandArgument='<%#Eval("Inventory_ID") %>' OnClick="linkbtn_OnClick"></asp:LinkButton>
                            <asp:LinkButton ID="linkbtn_Delete" runat="server" Text="Delete" CommandArgument='<%#Eval("Inventory_ID") %>' OnClientClick="return fn_DeleteOpening()"
                                OnClick="linkbtn_Delete_OnClick"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--  <asp:TemplateField HeaderText="Approve">
                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkApprove" runat="server" onclick="PopUpOpen(this.id);" />
                        <asp:HiddenField ID="hddOrdTlId" runat="server" Value='<%# Eval("pOrderDetail_ID")%>' />
                        <asp:HiddenField ID="hddpOrder_ApprovUser1" runat="server" Value='<%# Eval("pOrder_ApprovUser1")%>' />
                        <asp:HiddenField ID="hddpOrder_ApprovUser2" runat="server" Value='<%# Eval("pOrder_ApprovUser2")%>' />
                        <asp:HiddenField ID="hddpOrder_ApprovUser3" runat="server" Value='<%# Eval("pOrder_ApprovUser3")%>' />
                        
                    </ItemTemplate>
                </asp:TemplateField>--%>
                    <%-- <asp:TemplateField HeaderText="Reject">
                    <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkReject" runat="server" onclick="rejectPopUpOpen(this.id);" />
                    </ItemTemplate>
                </asp:TemplateField>--%>
                </Columns>
                <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                    BorderWidth="1px"></RowStyle>
                <EditRowStyle BackColor="#E59930"></EditRowStyle>
                <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                    Font-Bold="False"></HeaderStyle>
            </asp:GridView>
        </div>
        <div class="PopUpArea">
            <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
                Width="1000px" HeaderText="ADD / EDIT OPENING STOCK" PopupHorizontalAlign="WindowCenter"
                BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <div style="background-color: #CED8E4">
                            <div style="background-color: #CED8E4; width: 100%">
                                <div style="background-color: #4D74A8">
                                    <h3 style="color: #fff; margin: 0; font-size: 18px; text-transform: uppercase; font-family: Arial, Helvetica, sans-serif; padding: 10px 0; text-align: center">ADD / EDIT OPENING STOCK
                                    </h3>
                                </div>
                                <div style="width: 100%; border: 2px solid #FFFFFF">
                                    <div class="form-wrapper">
                                        <div class="form-lebel">
                                            Product *
                                        </div>
                                        <div class="form-field">
                                            <asp:TextBox Width="50%" runat="server" ID="txtProduct" autocomplete="off" onkeyup="FunCallAjaxList_UC(this,event,'Digital');"></asp:TextBox>
                                            <asp:TextBox runat="server" ID="txtProduct_hidden" Style="display: none"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-wrapper">
                                        <div class="form-lebel">
                                            Brand 
                                        </div>
                                        <div class="form-field">
                                            <dxe:ASPxTextBox ID="txtBrand" runat="server" Width="50%">
                                            </dxe:ASPxTextBox>
                                        </div>
                                    </div>
                                    <div class="form-wrapper">
                                        <div class="form-lebel">
                                            Size 
                                        </div>
                                        <div class="form-field">
                                            <dxe:ASPxComboBox Width="204px" ID="ddlSize" ClientInstanceName="cddlSize" runat="server"
                                                ValueType="System.String" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                            </dxe:ASPxComboBox>
                                        </div>
                                    </div>
                                    <div class="form-wrapper">
                                        <div class="form-lebel">
                                            Color 
                                        </div>
                                        <div class="form-field">
                                            <dxe:ASPxComboBox Width="204px" ID="ddlColor" ClientInstanceName="cddlColor" runat="server"
                                                ValueType="System.String" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                            </dxe:ASPxComboBox>
                                        </div>
                                        <div class="form-wrapper">
                                            <div class="form-lebel">
                                                Batch Number *
                                            </div>
                                            <div class="form-field">
                                                <dxe:ASPxTextBox ID="txtBatchNo" runat="server" Width="50%">
                                                </dxe:ASPxTextBox>
                                            </div>
                                        </div>
                                        <div class="form-wrapper">
                                            <div class="form-lebel">
                                                Date Of Manufacture 
                                            </div>
                                            <div class="form-field">
                                                <div style="display: inline-block;">
                                                    <dxe:ASPxComboBox ID="ddlMonth" ClientInstanceName="cddlMonth" runat="server" ValueType="System.String"
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
                                                <div style="display: inline-block;">
                                                    <dxe:ASPxComboBox ID="ddlYear" ClientInstanceName="cddlYear" runat="server" ValueType="System.String"
                                                        Width="100px" EnableSynchronization="True" EnableIncrementalFiltering="True">
                                                    </dxe:ASPxComboBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-wrapper">
                                            <div class="form-lebel">
                                                Expiry Date 
                                            </div>
                                            <div class="form-field">
                                                <dxe:ASPxDateEdit ID="txtExpiryDate" runat="server" Width="180" ClientInstanceName="ctxtExpiryDate"
                                                    EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="true" />
                                            </div>
                                        </div>
                                        <div class="form-wrapper">
                                            <div class="form-lebel">
                                                Quantity *
                                            </div>
                                            <div class="form-field">
                                                <div style="display: inline-block">
                                                    <dxe:ASPxTextBox ID="txtQuantity" runat="server">
                                                    </dxe:ASPxTextBox>
                                                </div>
                                                <div style="display: inline-block; vertical-align: top;">
                                                    <asp:TextBox runat="server" ID="txtUnit" autocomplete="off" onkeyup="FunCallAjaxList_UC_Unit(this,event,'Digital');"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="txtUnit_hidden" Style="display: none"></asp:TextBox>
                                                </div>
                                                <div style="display: inline-block; vertical-align: top;">
                                                    Unit *
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-wrapper">
                                            <div class="form-lebel">
                                                Price - Lot - Unit
                                            </div>
                                            <div class="form-field">
                                                <div style="display: inline-block">
                                                    <dxe:ASPxTextBox ID="txtPrice" runat="server">
                                                    </dxe:ASPxTextBox>
                                                </div>
                                                <div style="display: inline-block">
                                                    <dxe:ASPxTextBox ID="txtpricelot" runat="server">
                                                    </dxe:ASPxTextBox>
                                                </div>
                                                <div style="display: inline-block; vertical-align: top;">
                                                    <asp:TextBox runat="server" ID="txtpriceUnit" autocomplete="off" onkeyup="FunCallAjaxList_Price_Unit(this,event,'Digital');"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="txtpriceUnit_hidden" Style="display: none"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-wrapper">
                                            <div class="form-lebel">
                                                Currency 
                                            </div>
                                            <div class="form-field">
                                                <asp:TextBox runat="server" Width="200px" ID="txtCurrency" autocomplete="off" onkeyup="FunCallAjaxList_UC_cur(this,event,'Digital');"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="txtCurrency_hidden" Style="display: none"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-wrapper">
                                            <div class="form-lebel">
                                                Aquire Date *
                                            </div>
                                            <div class="form-field">
                                                <dxe:ASPxDateEdit ID="txtAquireDate" runat="server" ClientInstanceName="ctxtAquireDate"
                                                    EditFormat="Custom" EditFormatString="dd-MM-yyyy" UseMaskBehavior="true" />
                                            </div>
                                        </div>
                                        <asp:HiddenField ID="HddInventoryID" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="background-color: #CED8E4">
                            <div style="width: 500px; margin: 0 auto; padding: 10px; text-align: center;">
                                <div style="display: inline-block;">
                                    <asp:Button ID="btnSave" Text="SAVE" runat="server" Style="padding: 3px" OnClientClick="javascript:return btnSave_OnClientClick();"
                                        OnClick="btnSave_OnClick" />
                                </div>
                                <div style="display: inline-block; text-align: center;">
                                    <asp:Button ID="btnCancel" Text="CANCEL" runat="server" Style="padding: 3px" OnClientClick="btnCancel_OnClientClick()"
                                        OnClick="btnCancel_OnClick" />
                                </div>
                            </div>
                            <%-- </div>
                            <br style="clear: both;" />--%>
                        </div>
                        <%-- </div>--%>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="#4D74A8" ForeColor="white" />
            </dxe:ASPxPopupControl>
            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
            </dxe:ASPxGridViewExporter>
        </div>
    </div>
</asp:Content>
