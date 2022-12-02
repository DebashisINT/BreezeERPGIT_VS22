<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Store.Settings_Options.management_master_Store_sProductClass" Codebehind="sProductMap.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            ListBind();
            empCode = "";
            productCode = "";
        });

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
    
        function setEmpname(obj) {
            if (obj) {
                var lstEmpname = document.getElementById("lstEmpname");

                for (var i = 0; i < lstEmpname.options.length; i++) {
                    if (lstEmpname.options[i].value == obj) {
                        lstEmpname.options[i].selected = true;
                    }
                }
                $('#lstEmpname').trigger("chosen:updated");
            }
        }
        function setProduct(obj) {
            if (obj) {
                var lstProduct = document.getElementById("lstProduct");

                for (var i = 0; i < lstProduct.options.length; i++) {
                    if (lstProduct.options[i].value == obj) {
                        lstProduct.options[i].selected = true;
                    }
                }
                $('#lstProduct').trigger("chosen:updated");
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
        function lstEmpname() {
            $('#lstEmpname').fadeIn();
        }
        function lstProduct() {
            $('#lstProduct').fadeIn();
        }
        function lstProduct() {
            $('#lstProduct').fadeIn();
        }
        function SetDorpdownNothing() {
            //clear data from vendor dropdown
            var lstEmpname = document.getElementById("lstEmpname");
            for (var i = 0; i < lstEmpname.options.length; i++) {
                if (lstEmpname.options[i].selected) {
                    lstEmpname.options[i].selected = false;
                }
            }
            $('#lstEmpname').trigger("chosen:updated");

            //-------clear data from product dropdown

            var lstProduct = document.getElementById("lstProduct");
            for (var i = 0; i < lstProduct.options.length; i++) {
                if (lstProduct.options[i].selected) {
                    lstProduct.options[i].selected = false;
                }
            }
            $('#lstProduct').trigger("chosen:updated");

        }

        function fn_PopOpen() {
            SetDorpdownNothing();
            document.getElementById('lstEmpname').value = "";
            document.getElementById('lstProduct').value = "";
            ctxtTaxes_SurchargeRate.SetText('');
            cPopup_Empcitys.Show();
        }

        function btnSave_citys() {

            if (!isValid()) {
                return;
            }

            document.getElementById('txtTaxRates_MainAccount_hidden').value = document.getElementById('lstEmpname').value;
            document.getElementById('txtTaxRates_SubAccount_hidden').value = document.getElementById('lstProduct').value;
            //if (document.getElementById('txtTaxRates_MainAccount_hidden').value == '') {
            //    alert('Please select valid item for customer/vendor!');
            //    document.getElementById('txtTaxRates_MainAccount_hidden').focus();
            //}
            //else if (document.getElementById('txtTaxRates_SubAccount_hidden').value == '') {
            //    alert('Please select valid item for product!');
            //    document.getElementById('txtTaxRates_SubAccount_hidden').focus();
            //}
            //else if (document.getElementById('txtTaxes_SurchargeRate_I').value == '') {
            //    alert('please enter value for code!');
            //    document.getElementById('txtTaxes_SurchargeRate_I').focus();
            //}
             if (document.getElementById('hiddenedit').value == '') {
                grid.PerformCallback('savecity~');
            }
            else
                grid.PerformCallback('updatecity~' + document.getElementById('hiddenedit').value);
        }


        function fn_btnCancel() {
            MakeRowInVisible();
            cPopup_Empcitys.Hide();
        }
        function fn_Editcity(keyValue) {
            grid.PerformCallback('Edit~' + keyValue);
        }
        function fn_Deletecity(keyValue) {
            grid.PerformCallback('Delete~' + keyValue);
        }
        function grid_EndCallBack() {
            if (grid.cpinsert != null) {
                if (grid.cpinsert == 'Success') {
                    alert('Inserted Successfully');
                    cPopup_Empcitys.Hide();
                }
                else if (grid.cpinsert == 'Invalid') {
                    alert("Should be compulsorily greater than the datefrom value of the earlier instance of rate-configuration of this 'TaxCode+ProductClass+Country+State+City' combination \n 'Please Try Another date!!'");
                    cPopup_Empcitys.Hide();
                }
                else {
                    alert("Error On Insertion \n 'Please Try Again!!'");
                    cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpEdit != null) {
                var empCode = grid.cpEdit.split('~')[1];
                var prodCode = grid.cpEdit.split('~')[2];
                setEmpname(empCode);
                setProduct(prodCode);
                document.getElementById('txtTaxRates_MainAccount_hidden').value = grid.cpEdit.split('~')[1];
                document.getElementById('txtTaxRates_SubAccount_hidden').value = grid.cpEdit.split('~')[2];
                ctxtTaxes_SurchargeRate.SetText(grid.cpEdit.split('~')[4]);
                document.getElementById('hiddenedit').value = grid.cpEdit.split('~')[5];
                cPopup_Empcitys.Show();
            }
            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Success') {
                    alert('Update Successfully');
                    cPopup_Empcitys.Hide();
                }
                else {
                    alert("Error on Updation\n'Please Try again!!'")
                    cPopup_Empcitys.Hide();
                }
            }
            if (grid.cpUpdateValid != null) {
                if (grid.cpUpdateValid == "StateInvalid") {
                    alert("Please Select proper country state and city");
                }
                else if (grid.cpUpdateValid == "TaxCodeInvalid")
                    alert("Please Select proper TaxCode");

                else if (grid.cpUpdateValid == "dateInvalid")
                    alert("Please Select proper date");
            }


            if (grid.cpDelete != null) {
                if (grid.cpDelete == 'Success')
                    alert('Deleted Successfully');
                else
                    alert("Error on deletion\n'Please Try again!!'")
            }
            if (grid.cpExists != null) {
                if (grid.cpExists == "Exists") {
                    alert('Record already Exists');
                    cPopup_Empcitys.Hide();
                }
                else {
                    alert("Error on operation \n 'Please Try again!!'")
                    cPopup_Empcitys.Hide();
                }
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
        /////////////////////////////////////////////
        function OnCmbCountryName_ValueChange() {
            cCmbState.PerformCallback("BindState~" + cCmbCountryName.GetValue());
        }
        function CmbState_EndCallback() {
            cCmbState.SetSelectedIndex(0);
            cCmbState.Focus();
        }
        function OnCmbStateName_ValueChange() {
            cCmbCity.PerformCallback("BindCity~" + cCmbState.GetValue());
        }
        function CmbCity_EndCallback() {
            cCmbCity.SetSelectedIndex(0);
            cCmbCity.Focus();
        }
        function MakeRowInVisible() {
            $('#MandatorylstEmpname').css({ 'display': 'none' });
            $('#MandatorylstProduct').css({ 'display': 'none' });
            $('#MandatoryCode').css({ 'display': 'none' });
        }
        function isValid() {
            var retvalue = true;
          
            if (document.getElementById('lstEmpname').value.trim() == '') {
                $('#MandatorylstEmpname').css({ 'display': 'block' });
                retvalue = false;
            } else {
                $('#MandatorylstEmpname').css({ 'display': 'none' });
            }
            if (document.getElementById('lstProduct').value.trim() == '') {
                $('#MandatorylstProduct').css({ 'display': 'block' });
                retvalue = false;
            } else {
                $('#MandatorylstProduct').css({ 'display': 'none' });
            }

            if (ctxtTaxes_SurchargeRate.GetText().trim() == '') {
                $('#MandatoryCode').css({ 'display': 'block' });
                retvalue = false;
            } else {
                $('#MandatoryCode').css({ 'display': 'none' });
            }

           
            return retvalue;
        }
       
    </script>

    


    <style type="text/css">
        .cityDiv
        {
            height: 25px;
            width: 155px;
            float: left;
            margin-left: 70px;
        }
        .cityTextbox
        {
            height: 25px;
            width: 50px;
        }
        .Top
        {
            height: 90px;
            width: 400px;
            background-color: Silver;
            padding-top: 5px;
            valign: top;
        }
        .Footer
        {
            height: 30px;
            width: 400px;
            background-color: Silver;
            padding-top: 10px;
        }
        .ScrollDiv
        {
            height: 250px;
            width: 400px;
            background-color: Silver;
            overflow-x: hidden;
            overflow-y: scroll;
        }
        .ContentDiv
        {
            width: 400px;
            height: 300px;
            border: 2px;
            background-color: Silver;
        }
        
        .TitleArea
        {
            height: 20px;
            padding-left: 10px;
            padding-right: 3px;
            background-image: url( '../images/EHeaderBack.gif' );
            background-repeat: repeat-x;
            background-position: bottom;
            text-align: center;
        }
        .FilterSide
        {
            float: left;
            padding-left: 15px;
            width: 50%;
        }
        .SearchArea
        {
            width: 100%;
            height: 30px;
            padding-top: 5px;
        }
        /* Big box with list of options */#ajax_listOfOptions
        {
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
        #ajax_listOfOptions div
        {
            /* General rule for both .optionDiv and .optionDivSelected */
            margin: 1px;
            padding: 1px;
            cursor: pointer;
            font-size: 0.9em;
        }
        #ajax_listOfOptions .optionDiv
        {
            /* Div for each item in list */
        }
        #ajax_listOfOptions .optionDivSelected
        {
            /* Selected item in the list */
            background-color: #DDECFE;
            color: Blue;
        }
        #ajax_listOfOptions_iframe
        {
            background-color: #F00;
            position: absolute;
            z-index: 3000;
        }
        form
        {
            display: inline;
        }



         .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstEmpname {
            width: 200px;
        }

        #lstEmpname_chosen {
            width: 253px !important;
        }

        #PageControl1_CC {
            overflow: visible !important;
        }

        #lstEmpname_chosen {
            margin-bottom: 5px;
        }







           #lstProduct {
            width: 200px;
        }

        #lstProduct_chosen {
            width: 253px !important;
        }

        #PageControl1_CC {
            overflow: visible !important;
        }

        #lstProduct_chosen {
            margin-bottom: 5px;
        }
       
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
        <div class="panel-title">
            <h3>Product code Map</h3>
        </div>

    </div>
    <div>
        <div class="Main">
           <%-- <div class="TitleArea">
                <strong><span style="color: #000099">Product code map</span></strong>
            </div>--%>
            <div class="SearchArea">
                <div class="FilterSide">
                    <div style="float: left; padding-right: 5px;">
                      <%--  <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                            Show Filter</span></a>--%>
                    </div>
                    <div>
                       <%-- <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                            All Records</span></a>--%>
                    </div>
                     <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="fn_PopOpen()" class="btn btn-primary">Add New</a>
                                  <% } %>
                    <% if (rights.CanExport)
                                               { %>
                     <asp:DropDownList ID="drdExport"  runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                                <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                     <% } %>
                </div>
               <%-- <div class="ExportSide">
                    <div style="float: left; padding-right: -50px;">
                        

                    </div>
                </div>--%>
            </div>
        </div>
        <div class="GridViewArea">
            <dxe:ASPxGridView ID="cityGrid" runat="server" AutoGenerateColumns="False" ClientInstanceName="grid"
                KeyFieldName="pCodeMap_ID" Width="100%" OnHtmlRowCreated="cityGrid_HtmlRowCreated"
                OnHtmlEditFormCreated="cityGrid_HtmlEditFormCreated" OnCustomCallback="cityGrid_CustomCallback">
                 <SettingsSearchPanel Visible="True" />
                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                <Columns>
                 
                    <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="Customer/Vendor" FieldName="cnt_firstName"
                        Width="12%" FixedStyle="Left" Visible="True">
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="Product
                        " FieldName="sProducts_Name" Visible="True"
                        Width="12%">
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn> 
                    <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="Code" FieldName="pCodeMap_Code" Width="12%"
                        FixedStyle="Left" Visible="True">
                        <EditFormSettings Visible="True" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="pCodeMap_ContactID" FieldName="pCodeMap_ContactID"
                        ReadOnly="True" Visible="false" FixedStyle="Left">
                        <EditFormSettings Visible="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="pCodeMap_ID" FieldName="pCodeMap_ID" ReadOnly="True"
                        Visible="false" FixedStyle="Left">
                        <EditFormSettings Visible="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="1" Caption="pCodeMap_ProductID" FieldName="pCodeMap_ProductID"
                        ReadOnly="True" Visible="false" FixedStyle="Left">
                        <EditFormSettings Visible="False" />
                    </dxe:GridViewDataTextColumn>
                    <dxe:GridViewDataTextColumn VisibleIndex="1" ReadOnly="True" Width="2%">
                        <%--<HeaderTemplate>
                            <a href="javascript:void(0);" onclick="fn_PopOpen()"><span style="color: #000099;
                                text-decoration: underline">Add New</span> </a>
                        </HeaderTemplate>--%>
                        <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    Actions
                                   
                                </HeaderTemplate>
                        <DataItemTemplate>
                            <% if (rights.CanEdit)
                                        { %>
                              <a href="javascript:void(0);" onclick="fn_Editcity('<%# Container.KeyValue %>')" class="pad">
                                        <img src="/assests/images/Edit.png" alt="Edit"></a>
                             <% } %>
                              <% if (rights.CanDelete)
                              { %>
                             <a href="javascript:void(0);" onclick="fn_Deletecity('<%# Container.KeyValue %>')"   alt="Delete" class="pad">
                                        <img src="/assests/images/Delete.png" /></a>
                             <% } %>
                        </DataItemTemplate>
                    </dxe:GridViewDataTextColumn>
                </Columns>
                <ClientSideEvents EndCallback="function (s, e) {grid_EndCallBack();}" />
            </dxe:ASPxGridView>
        </div>
        <div class="PopUpArea">
            <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
                Width="400px" HeaderText="Add Product code map Details" PopupHorizontalAlign="WindowCenter"
                BackColor="white" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
                Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True"
                ContentStyle-CssClass="pad">
                <ContentStyle VerticalAlign="Top" CssClass="pad">
                </ContentStyle>
                <ContentCollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div class="Top">
                            <div>
                                <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 40px;">
                                    Customer/Vendor
                                </div>
                                <div class="Left_Content" style="display: inline-block">
                                <%--    <asp:TextBox ID="txtTaxRates_MainAccount" Width="176px" runat="server" onkeyup="FunCallAjaxList(this,event,'Digital');"></asp:TextBox>--%>
                                   <asp:ListBox ID="lstEmpname"  CssClass="chsn"  runat="server" Font-Size="12px" Width="253px"   data-placeholder="Select..." ></asp:ListBox>
                                   <span id="MandatorylstEmpname" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:26px;top:54px;display:none" title="Mandatory"></span>
                                      <asp:TextBox ID="txtTaxRates_MainAccount_hidden" runat="server" Width="100px" Style="display: none"></asp:TextBox>
                                      </div>
                            </div>
                            <div>
                                <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 40px;">
                                    Product
                                </div>
                                <div class="Left_Content" style="display: inline-block">
                                   <asp:ListBox ID="lstProduct"  CssClass="chsn"  runat="server" Font-Size="12px" Width="253px"   data-placeholder="Select..." ></asp:ListBox>
                                <span id="MandatorylstProduct" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:26px;top:98px;display:none" title="Mandatory"></span>
                                    <asp:TextBox ID="txtTaxRates_SubAccount_hidden" runat="server" Width="100px" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                            <div>
                                <div class="cityDiv" style="display: inline-block; height: auto; margin-left: 40px;">
                                    Code
                                </div>
                                <div class="Left_Content" style="display: inline-block">
                                    <dxe:ASPxTextBox ID="txtTaxes_SurchargeRate" ClientInstanceName="ctxtTaxes_SurchargeRate"
                                        runat="server" Width="180px" MaxLength="30">
                                    </dxe:ASPxTextBox>
                                    <span id="MandatoryCode" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red; position:absolute;right:18px;top:132px;display:none" title="Mandatory"></span>
                                </div>
                            </div>
                        </div>
                        <div class="ContentDiv" style="height: 73px">
                            <div style="display: none">
                                <div style="height: 20px; width: 280px; background-color: Gray; padding-left: 120px;">
                                    <h5>
                                        Static Code</h5>
                                </div>
                                <div style="height: 20px; width: 130px; padding-left: 70px; background-color: Gray;
                                    float: left;">
                                    Exchange</div>
                                <div style="height: 20px; width: 200px; background-color: Gray; text-align: left;">
                                    Value</div>
                                <div class="ScrollDiv">
                                    <div class="cityDiv" style="padding-top: 5px;">
                                        NSE Code</div>
                                    <div style="padding-top: 5px;">
                                        <dxe:ASPxTextBox ID="txtNseCode" ClientInstanceName="ctxtNseCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        BSE Code</div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtBseCode" ClientInstanceName="ctxtBseCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        MCX Code</div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtMcxCode" ClientInstanceName="ctxtMcxCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        MCXSX Code</div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtMcsxCode" ClientInstanceName="ctxtMcsxCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        NCDEX Code</div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtNcdexCode" ClientInstanceName="ctxtNcdexCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        CDSL Code</div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtCdslCode" ClientInstanceName="ctxtCdslCode" CssClass="cityTextbox"
                                            runat="server">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        NSDL Code</div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtNsdlCode" ClientInstanceName="ctxtNsdlCode" CssClass="cityTextbox"
                                            runat="server">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        NDML Code</div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtNdmlCode" ClientInstanceName="ctxtNdmlCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        CVL Code</div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtCvlCode" ClientInstanceName="ctxtCvlCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                    <br style="clear: both;" />
                                    <div class="cityDiv">
                                        DOTEX Code</div>
                                    <div>
                                        <dxe:ASPxTextBox ID="txtDotexCode" ClientInstanceName="ctxtDotexCode" runat="server"
                                            CssClass="cityTextbox">
                                        </dxe:ASPxTextBox>
                                    </div>
                                </div>
                            </div>
                            <br style="clear: both;" />
                            <div class="Footer">
                                <div style="margin-left: 130px; width: 70px; float: left;">
                                    <dxe:ASPxButton ID="btnSave_citys" ClientInstanceName="cbtnSave_citys" runat="server"
                                        AutoPostBack="False" Text="Save" CssClass="btn btn-primary">
                                        <ClientSideEvents Click="function (s, e) {btnSave_citys();}" />
                                    </dxe:ASPxButton>
                                </div>
                                <div style="">
                                    <dxe:ASPxButton ID="btnCancel_citys" runat="server" AutoPostBack="False" Text="Cancel" CssClass="btn btn-danger">
                                        <ClientSideEvents Click="function (s, e) {fn_btnCancel();}" />
                                    </dxe:ASPxButton>
                                </div>
                                <br style="clear: both;" />
                            </div>
                            <br style="clear: both;" />
                        </div>
                        <%-- </div>--%>
                    </dxe:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle BackColor="LightGray" ForeColor="Black" />
            </dxe:ASPxPopupControl>
            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
            </dxe:ASPxGridViewExporter>
        </div>
        <div class="HiddenFieldArea" style="display: none;">
            <asp:HiddenField runat="server" ID="hiddenedit" />
            <asp:HiddenField ID="HiddenField1" runat="server" />
        </div>
    </div>
</asp:Content>