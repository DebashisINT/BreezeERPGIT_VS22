<%@ Page Title="Building/Warehouses" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" EnableEventValidation="false"
    Inherits="ERP.OMS.Management.Master.management_master_RootBuildingInsertUpdate" CodeBehind="RootBuildingInsertUpdate.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">
        function CheckUniqueCode(s, e) {
            var WHID = 0;
            if (GetObjectID('hdnEditID').value != 'ADD') {
                WHID = GetObjectID('hdnEditID').value;
            }
            var ProductName = ctxtCode.GetText().trim();
            $.ajax({
                type: "POST",
                url: "RootBuildingInsertUpdate.aspx/CheckUniqueName",
                data: JSON.stringify({ ProductName: ProductName, WHID: WHID }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var data = msg.d;
                    if (data == true) {
                        jAlert("Please enter unique code", "Alert", function () { ctxtCode.SetFocus(); });
                        ctxtCode.SetText("");
                        return false;
                    }
                }

            });
        }

    </script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            ListBind();
            if (document.getElementById('txtCountry_hidden')) {
                var cntry = document.getElementById('txtCountry_hidden').value;
                document.getElementById('txtCountry_hidden').value = "";
                setCountry(cntry);
                setLevel($("#hdnLevel").val());
            }
        });

        function selectAll() {
            cBranchGridLookup.gridView.SelectRows();
        }
        function unselectAll() {
            cBranchGridLookup.gridView.UnselectRows();
        }
        function CloseGridLookup() {
            cBranchGridLookup.ConfirmCurrentSelection();
            cBranchGridLookup.HideDropDown();
        }

        function ClientSaveClick() {
            document.getElementById('txtCountry_hidden').value = document.getElementById('lstCountry').value;
            document.getElementById('txtState_hidden').value = document.getElementById('lstState').value;
            document.getElementById('txtCity_hidden').value = document.getElementById('lstCity').value;
            document.getElementById('HdPin').value = document.getElementById('lstPin').value;
            document.getElementById('hdnWarehouse').value = document.getElementById('ddl_ParentWarehouse').value;

            var level = document.getElementById('ddl_level').value;

            if (level != null && level != "") {
                if (level != "1") {
                    if (document.getElementById('ddl_ParentWarehouse').value == "" || document.getElementById('ddl_ParentWarehouse').value == null) {
                        jAlert('Please select parent warehouse.', 'Alert');
                        return false;
                    }
                }
            }
            else {
                if (!$("#dvMultiwarehouse").hasClass('hide')) {
                    jAlert('Please select warehouse level.', 'Alert');

                    return false;
                }
            }


            if ($("#TxtBuilding").val() == '') {
                return false;

            }
            if (document.getElementById('txtCountry_hidden').value == '') {
                // alert();
                return false;
            }

            else if (document.getElementById('txtState_hidden').value == '') {
                // alert();
                return false;
            }
            else {

                return true;
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
        function Show() {

            var url = "frmAddDocuments.aspx";

            popup.SetContentUrl(url);

            popup.Show();
        }


        function setCountry(obj) {
            if (obj) {
                var lstCntry = document.getElementById("lstCountry");

                for (var i = 0; i < lstCntry.options.length; i++) {
                    if (lstCntry.options[i].value == obj) {
                        lstCntry.options[i].selected = true;
                    }
                }
                $('#lstCountry').trigger("chosen:updated");
                onCountryChange();
            }
        }


        function setLevel(obj) {
            if (obj) {
                var lstCntry = document.getElementById("ddl_level");

                for (var i = 0; i < lstCntry.options.length; i++) {
                    if (lstCntry.options[i].value == obj) {
                        lstCntry.options[i].selected = true;
                    }
                }
                $('#ddl_level').trigger("chosen:updated");
                level_change();
            }
        }


        function setState(obj) {
            if (obj) {
                var lstStae = document.getElementById("lstState");

                for (var i = 0; i < lstStae.options.length; i++) {
                    if (lstStae.options[i].value == obj) {
                        lstStae.options[i].selected = true;
                    }
                }
                $('#lstState').trigger("chosen:updated");
                onStateChange();
            }
        }
        function setCity(obj) {
            if (obj) {
                var lstCity = document.getElementById("lstCity");

                for (var i = 0; i < lstCity.options.length; i++) {
                    if (lstCity.options[i].value == obj) {
                        lstCity.options[i].selected = true;
                    }
                }
                $('#lstCity').trigger("chosen:updated");
                onCityChange();
            }
        }
        function setArea(obj) {
            if (obj) {
                var lstArea = document.getElementById("lstArea");

                for (var i = 0; i < lstArea.options.length; i++) {
                    if (lstArea.options[i].value == obj) {
                        lstArea.options[i].selected = true;
                    }
                }
                $('#lstArea').trigger("chosen:updated");

            }

        }
        function setPin(obj) {
            if (obj) {
                var lstPin = document.getElementById("lstPin");

                for (var i = 0; i < lstPin.options.length; i++) {
                    if (lstPin.options[i].value == obj) {
                        lstPin.options[i].selected = true;
                    }
                }
                $('#lstPin').trigger("chosen:updated");

            }
        }

        function setWarehouse(obj) {
            if (obj) {
                var ddl_ParentWarehouse = document.getElementById("ddl_ParentWarehouse");

                for (var i = 0; i < ddl_ParentWarehouse.options.length; i++) {
                    if (ddl_ParentWarehouse.options[i].value == obj) {
                        ddl_ParentWarehouse.options[i].selected = true;
                    }
                }
                $('#ddl_ParentWarehouse').trigger("chosen:updated");

            }
        }


        function onCountryChange() {
            var CountryId = "";
            if (document.getElementById('lstCountry').value) {
                CountryId = document.getElementById('lstCountry').value;
            } else {
                return;
            }
            var lState = $('select[id$=lstState]');
            var lCity = $('select[id$=lstCity]');
            var lArea = $('select[id$=lstArea]');
            var lPin = $('select[id$=lstPin]');
            lState.empty();
            lCity.empty();
            lArea.empty();
            lPin.empty();
            $('#lstCity').trigger("chosen:updated");
            $('#lstArea').trigger("chosen:updated");
            $('#lstPin').trigger("chosen:updated");
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetStates",
                data: JSON.stringify({ CountryCode: CountryId }),
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

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lState).append(listItems.join(''));

                        $('#lstState').fadeIn();
                        $('#lstState').trigger("chosen:updated");
                        if (document.getElementById('txtState_hidden').value) {
                            var stateVal = document.getElementById('txtState_hidden').value;
                            document.getElementById('txtState_hidden').value = "";
                            setState(stateVal);
                        }
                    }
                    else {
                        $('#lstState').fadeIn();
                        $('#lstState').trigger("chosen:updated");
                    }
                }
            });
        }

        function onStateChange() {
            var StateId = "";
            if (document.getElementById('lstState').value) {
                StateId = document.getElementById('lstState').value;
            }
            else {
                return;
            }
            var lCity = $('select[id$=lstCity]');
            var lArea = $('select[id$=lstArea]');
            var lPin = $('select[id$=lstPin]');
            lArea.empty();
            lCity.empty();
            lPin.empty();
            $('#lstArea').trigger("chosen:updated");
            $('#lstPin').trigger("chosen:updated");
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetCities",
                data: JSON.stringify({ StateCode: StateId }),
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

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lCity).append(listItems.join(''));

                        $('#lstCity').fadeIn();
                        $('#lstCity').trigger("chosen:updated");
                        if (document.getElementById('txtCity_hidden').value) {
                            var cityVal = document.getElementById('txtCity_hidden').value;
                            document.getElementById('txtCity_hidden').value = "";
                            setCity(cityVal);
                        }
                    }
                    else {
                        $('#lstCity').fadeIn();
                        $('#lstCity').trigger("chosen:updated");
                    }
                }
            });
        }
        function onCityChange() {
            getPinList();
        }

        function level_change() {
            var ddl_level = "";
            if (document.getElementById('ddl_level').value) {
                ddl_level = document.getElementById('ddl_level').value;
            }
            else {
                return;
            }
            var ddl_ParentWarehouse = $('select[id$=ddl_ParentWarehouse]');
            ddl_ParentWarehouse.empty();
            $.ajax({
                type: "POST",
                url: "RootBuildingInsertUpdate.aspx/GetWarehouse",
                data: JSON.stringify({ ddl_level: ddl_level }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var list = msg.d;
                    var listItems = [];
                    if (list.length > 0) {

                        for (var i = 0; i < list.length; i++) {
                            var id = '';
                            var name = '';
                            id = list[i].id;
                            name = list[i].Name;

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(ddl_ParentWarehouse).append(listItems.join(''));

                        $('#ddl_ParentWarehouse').fadeIn();
                        $('#ddl_ParentWarehouse').trigger("chosen:updated");
                        if (document.getElementById('hdnWarehouse').value) {
                            setWarehouse(document.getElementById('hdnWarehouse').value);
                        }

                    }
                    else {
                        $('#ddl_ParentWarehouse').fadeIn();
                        $('#ddl_ParentWarehouse').trigger("chosen:updated");
                    }
                }
            });
        }


        function getPinList() {
            var CityId = "";
            if (document.getElementById('lstCity').value) {
                CityId = document.getElementById('lstCity').value;
            }
            else {
                return;
            }
            var lPin = $('select[id$=lstPin]');
            lPin.empty();
            $.ajax({
                type: "POST",
                url: "BranchAddEdit.aspx/GetPin",
                data: JSON.stringify({ CityCode: CityId }),
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

                            listItems.push('<option value="' +
                            id + '">' + name
                            + '</option>');
                        }

                        $(lPin).append(listItems.join(''));

                        $('#lstPin').fadeIn();
                        $('#lstPin').trigger("chosen:updated");
                        if (document.getElementById('HdPin').value) {
                            setPin(document.getElementById('HdPin').value);
                        }

                    }
                    else {
                        $('#lstPin').fadeIn();
                        $('#lstPin').trigger("chosen:updated");
                    }
                }
            });
        }
    </script>
    <style>
        #RequiredFieldValidator9 {
            position: absolute;
            right: 620px;
            top: 70px;
        }

        .chosen-container.chosen-container-single {
            width: 100% !important;
        }

        .chosen-choices {
            width: 100% !important;
        }

        #lstCountry, #lstState, #lstCity, #lstArea, #lstPin, #lstBranchHead {
            width: 200px;
        }

        #lstCountry, #lstState, #lstCity, #lstArea, #lstPin, #lstBranchHead, #ddl_ParentWarehouse {
            display: none !important;
        }

        #lstCountry_chosen, #lstState_chosen, #lstCity_chosen, #lstArea_chosen, #lstPin_chosen, #lstBranchHead_chosen {
            width: 100% !important;
        }

        #PageControl1_CC {
            overflow: visible !important;
        }

        #lstState_chosen, #lstCountry_chosen, #lstCity_chosen, #lstPin_chosen, #lstBranchHead_chosen {
            margin-bottom: 5px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>
                <asp:Label ID="Label12" runat="server"></asp:Label>
                Building Details</h3>
        </div>
    </div>
    <div class="crossBtn"><a href="RootBuilding.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main" style="border: 1px solid #ccc; padding: 10px 15px">

        <%--<asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>--%>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-md-4" id="DivWHCode" runat="server">
                        <label>
                            <asp:Label ID="Label1" runat="server" Text="Warehouse Code"></asp:Label>
                        </label>
                        <div>
                            <dxe:ASPxTextBox ID="txtCode" MaxLength="100" ClientInstanceName="ctxtCode" runat="server" Width="100%" CssClass="upper">
                                <ClientSideEvents TextChanged="function(s,e){CheckUniqueCode()}" />
                            </dxe:ASPxTextBox>
                             <asp:HiddenField runat="server" ID="hdnEditID" />
                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="a" runat="server" ControlToValidate="txtCode"
                                CssClass="pullleftClass fa fa-exclamation-circle ctcclass " ToolTip="Mandatory."
                                ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label2" runat="server" Text="Building/Warehouse Name "></asp:Label><span style="color: red">*</span>
                        </label>
                        <div>
                            <asp:TextBox ID="TxtBuilding" runat="server" Width="100%" MaxLength="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="a" runat="server" ControlToValidate="TxtBuilding"
                                CssClass="pullleftClass fa fa-exclamation-circle ctcclass " ToolTip="Mandatory."
                                ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label3" runat="server" Text="Caretaker/Contact Person" Width="100%"></asp:Label></label>
                        <div>
                            <asp:DropDownList ID="DDLCareTaker" runat="server" Width="100%">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label4" runat="server" Text="Address1"></asp:Label></label>
                        <div>
                            <asp:TextBox ID="TxtAdd1" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label5" runat="server" Text="Address2"></asp:Label></label>
                        <div>
                            <asp:TextBox ID="TxtAdd2" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label6" runat="server" Text="Address3 "></asp:Label></label>
                        <div>
                            <asp:TextBox ID="TxtAdd3" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label7" runat="server" Text="Landmark"></asp:Label></label>
                        <div>
                            <asp:TextBox ID="TxtLand" runat="server" Width="100%" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label8" runat="server" Text="Country "></asp:Label><span style="color: red">*</span>
                        </label>
                        <div>
                            <%--<asp:DropDownList ID="DDLCountry" runat="server" Width="100%" AutoPostBack="True"
                                OnSelectedIndexChanged="DDLCountry_SelectedIndexChanged">
                            </asp:DropDownList>--%>

                            <asp:ListBox ID="lstCountry" CssClass="chsn" runat="server" Font-Size="12px" Width="253px" data-placeholder="Select..." onchange="onCountryChange()"></asp:ListBox>
                            <asp:HiddenField ID="txtCountry_hidden" runat="server" />
                            <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator4" InitialValue="0" ValidationGroup="a" runat="server" CssClass="pullrightClass fa fa-exclamation-circle r591" SetFocusOnError="true" ControlToValidate="DDLCountry" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label9" runat="server" Text="State "></asp:Label><span style="color: red">*</span>
                        </label>
                        <div>
                            <%--<asp:DropDownList ID="DDLState" runat="server" Width="100%" AutoPostBack="True"
                                OnSelectedIndexChanged="DDLState_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue="0" ValidationGroup="a" runat="server" CssClass="pullrightClass fa fa-exclamation-circle r591" SetFocusOnError="true" ControlToValidate="DDLState" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="TxtState" runat="server" Visible="False" Width="100%"></asp:TextBox>--%>

                            <asp:ListBox ID="lstState" CssClass="chsn" runat="server" Font-Size="12px" Width="253px" data-placeholder="Select State.." onchange="onStateChange()"></asp:ListBox>
                            <asp:HiddenField ID="txtState_hidden" runat="server" />

                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label10" runat="server" Text="City / District"></asp:Label></label>
                        <div>
                            <%--<asp:DropDownList ID="DDLCity" runat="server" Width="100%">
                            </asp:DropDownList>
                            <asp:TextBox ID="TxtCity" runat="server" Visible="False" Width="100%"></asp:TextBox>--%>

                            <asp:ListBox ID="lstCity" CssClass="chsn" runat="server" Font-Size="12px" Width="253px" data-placeholder="Select City.." onchange="onCityChange()"></asp:ListBox>
                            <asp:HiddenField ID="txtCity_hidden" runat="server" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <asp:Label ID="Label11" runat="server" Text="Pin"></asp:Label></label>
                        <div>
                            <%--<asp:TextBox ID="TxtPin" runat="server" Width="100%" MaxLength="50"></asp:TextBox>--%>

                            <asp:ListBox ID="lstPin" CssClass="chsn" runat="server" Font-Size="12px" Width="253px" data-placeholder="Select..."></asp:ListBox>
                            <asp:HiddenField ID="HdPin" runat="server" />
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>
                            <dxe:ASPxLabel ID="lbl_Branch" runat="server" Text="Branch">
                            </dxe:ASPxLabel>
                        </label>
                        <div>
                            <%--   <asp:DropDownList ID="ddl_Branch" runat="server" Width="100%" TabIndex="5">
                            </asp:DropDownList>--%>
                            <dxe:ASPxGridLookup ID="BranchGridLookup" runat="server" SelectionMode="Multiple" ClientInstanceName="cBranchGridLookup" KeyFieldName="branch_id" TextFormatString="{1}" MultiTextSeparator=", "
                                DataSourceID="BranchdataSource" Width="100%">
                                <Columns>
                                    <dxe:GridViewCommandColumn ShowSelectCheckbox="True" Width="60" Caption=" " SelectAllCheckboxMode="Page">
                                    </dxe:GridViewCommandColumn>

                                    <dxe:GridViewDataColumn FieldName="branch_code" Caption="Branch Code" Width="150">
                                        <Settings AutoFilterCondition="Contains" />

                                    </dxe:GridViewDataColumn>

                                    <dxe:GridViewDataColumn FieldName="branch_description" Caption="Branch Name" Width="150">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxe:GridViewDataColumn>



                                </Columns>
                                <GridViewProperties Settings-VerticalScrollBarMode="Auto">

                                    <Templates>
                                        <StatusBar>
                                            <table class="OptionsTable" style="float: right">
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxButton ID="btn_select_all" runat="server" AutoPostBack="false" Text="Select All" ClientSideEvents-Click="selectAll" UseSubmitBehavior="false" />
                                                        <dxe:ASPxButton ID="btn_deselect_all" runat="server" AutoPostBack="false" Text="Deselect All" ClientSideEvents-Click="unselectAll" UseSubmitBehavior="false" />
                                                        <dxe:ASPxButton ID="Close" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" UseSubmitBehavior="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </StatusBar>
                                    </Templates>
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true" />

                                </GridViewProperties>
                            </dxe:ASPxGridLookup>
                        </div>
                    </div>


                    <div class="clear"></div>

                    <div id="dvMultiwarehouse" runat="server" class="hide">
                        <div class="col-md-4">
                            <label>
                                <dxe:ASPxLabel ID="lbl_Level" runat="server" Text="Level"></dxe:ASPxLabel>
                            </label>
                            <font color="red">*</font>
                            <div>
                                <asp:ListBox ID="ddl_level" onchange="level_change();" DataTextField="Level_Name" DataValueField="Level_id" DataSourceID="sdLevel" runat="server" CssClass="chsn" Font-Size="12px" Width="253px" data-placeholder="Select..."></asp:ListBox>
                            </div>
                            <asp:SqlDataSource ID="sdLevel" runat="server" SelectCommand="select Level_id,Level_Name from Master_WarehouseLayout where ISNULL(Level_Name,'')<>'' order by Level_id"></asp:SqlDataSource>
                        </div>

                        <div class="col-md-4">
                            <label>
                                <dxe:ASPxLabel ID="lbl_ParentWarehouse" runat="server" Text="Parent Warehouse"></dxe:ASPxLabel>
                            </label>
                            <div>
                                <asp:ListBox ID="ddl_ParentWarehouse" runat="server" CssClass="chsn" Font-Size="12px" Width="253px" data-placeholder="Select..."></asp:ListBox>
                                <asp:HiddenField ID="hdnWarehouse" runat="server" />
                            </div>
                        </div>
                    </div>


                    <div class="clear"></div>
                    <div class="col-md-12  pdTop15">
                        <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="btn btn-primary btnUpdate" OnClick="BtnSave_Click" OnClientClick="return ClientSaveClick()" ValidationGroup="a" />
                        <asp:Button ID="BtnAdd" runat="server" Text="Add Files" Visible="false" OnClientClick="Show()" />
                    </div>
                </div>

                <%--   <dxe:ASPxPopupControl runat="server" ClientInstanceName="popup" CloseAction="CloseButton"
                        ContentUrl="frmAddDocuments.aspx" HeaderText="Add Document" Left="150" Top="10"
                        Width="700px" Height="400px" ID="ASPXPopupControl">
                        <ContentCollection>
                            <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            </dxe:PopupControlContentControl>
                        </ContentCollection>
                    </dxe:ASPxPopupControl>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--<table>
                <tr>
                    <td >
                    </td>
                    <td>

                    </td>
                </tr>
            </table>--%>
    </div>
   
    <asp:HiddenField runat="server" ID="hdnLevel" />
    
    <asp:SqlDataSource ID="BranchdataSource" runat="server"
        SelectCommand="select branch_id,branch_code,branch_description from tbl_master_branch"></asp:SqlDataSource>
</asp:Content>
