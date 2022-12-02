<%@ Page Title="Correspondence" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.Master.management_master_Employee_Correspondence" CodeBehind="Employee_Correspondence.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="javascript" type="text/javascript">

        function fn_AllowonlyNumeric(s, e) {
            var theEvent = e.htmlEvent || window.event;
            var key = theEvent.keyCode || theEvent.which;
            var keychar = String.fromCharCode(key);
            if (key == 9 || key == 37 || key == 38 || key == 39 || key == 40 || key == 8 || key == 46) { //tab/ Left / Up / Right / Down Arrow, Backspace, Delete keys
                return;
            }
            var regex = /[0-9\b]/;

            if (!regex.test(keychar)) {
                theEvent.returnValue = false;
                if (theEvent.preventDefault)
                    theEvent.preventDefault();
            }
        }

        function addnewemail() {
            $('#emailisnew').val("newinsert");
            gridEmail.AddNewRow();
        }
        function OnCountryChanged(cmbCountry) {
            gridAddress.GetEditor("State").PerformCallback(cmbCountry.GetValue().toString());
            //if (cmbCountry.GetValue().toString() == 1) {
            //    alert('hello');
            //     var pat1 = /^\d{6}$/;
            //     if (!pat1.test(pin_code.value)) {
            //         alert("Pin code should be 6 digits ");
            //         pin_code.focus();
            //         return false;
            //     }
            // }
            // else
            // {
            // }
        }

        function OnFieldXChanged1(cmbFieldX) {
            var comboValue = cmbFieldX.GetValue();

            if ('<%=Session["id"] %>' != null && '<%=Session["id"] %>' != '') {
                '<%=Session["id"]=null %>';
                if (comboValue == "Mobile") {
                    $('#PhoneGrid_DXPEForm_ef0_DXEFL_0_1, #PhoneGrid_DXPEForm_ef0_DXEFL_0_2 ,#PhoneGrid_DXPEForm_ef0_DXEFL_0_4').hide();
                }
                else {
                    $('#PhoneGrid_DXPEForm_ef0_DXEFL_0_1, #PhoneGrid_DXPEForm_ef0_DXEFL_0_2 ,#PhoneGrid_DXPEForm_ef0_DXEFL_0_4').show();
                }
            }
            else {
                if (comboValue == "Mobile") {
                    $('#PhoneGrid_DXPEForm_efnew_DXEFL_1,#PhoneGrid_DXPEForm_efnew_DXEFL_2,#PhoneGrid_DXPEForm_efnew_DXEFL_4').hide();
                }
                else {
                    $('#PhoneGrid_DXPEForm_efnew_DXEFL_1,#PhoneGrid_DXPEForm_efnew_DXEFL_2,#PhoneGrid_DXPEForm_efnew_DXEFL_4').show();
                }
            }




        }
        function OnStateChanged(cmbState) {
            gridAddress.GetEditor("City").PerformCallback(cmbState.GetValue().toString());
        }
        function OnCityChanged(abc) {

            gridAddress.GetEditor("PinCode").PerformCallback(abc.GetValue().toString());
            gridAddress.GetEditor("area").PerformCallback(abc.GetValue().toString());

        }
        function OnChildCall(cmbCity) {
            // alert(cmbCity);
            OnCityChanged(gridAddress.GetEditor("City"));
        }
        function OnChildCallNew() {

            OnCityChanged(gridAddress.GetEditor("City"));
        }
        function disp_prompt(name) {

            //var ID = document.getElementById(txtID);
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Employee_general.aspx?id=" + $("#hdKeyVal").val();
            }
            if (name == "tab1") {
                //alert(name);
                //document.location.href="Employee_Correspondence.aspx";
            }
            else if (name == "tab2") {

                // document.location.href = "Employee_BankDetails.aspx";
                document.location.href = "Employee_Education.aspx";
            }
            else if (name == "tab3") {
                document.location.href = "Employee_Employee.aspx";
                //alert(name);
                //Text="DP" document.location.href = "Employee_DPDetails.aspx";
            }
            else if (name == "tab4") {
                //alert(name);
                //   document.location.href = "Employee_Document.aspx";
                document.location.href = "Employee_Document.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                // document.location.href = "Employee_FamilyMembers.aspx";
                document.location.href = "Employee_FamilyMembers.aspx";
            }
            else if (name == "tab6") {
                document.location.href = "Employee_GroupMember.aspx";
                //alert(name);
                // document.location.href = "Employee_Registration.aspx";
            }
            else if (name == "tab7") {
                document.location.href = "Employee_EmployeeCTC.aspx";
                //alert(name);
                //  document.location.href = "Employee_GroupMember.aspx";

            }
            else if (name == "tab8") {
                document.location.href = "Employee_BankDetails.aspx";
                //alert(name);
                // document.location.href = "Employee_Employee.aspx";

            }
            else if (name == "tab9") {
                //alert(name);
                document.location.href = "Employee_Remarks.aspx";
            }
            else if (name == "tab10") {
                //alert(name);
                //  document.location.href = "Employee_Remarks.aspx";

            }
            else if (name == "tab11") {
                //alert(name);
                //  document.location.href = "Employee_Education.aspx";

            }
            else if (name == "tab12") {
                //alert(name);
                //  document.location.href = "Employee_Subscription.aspx";
            }
            else if (name == "tab13") {
                //alert(name);
                var keyValue = $("#hdnlanguagespeak").val();
                document.location.href = 'frmLanguageProfi.aspx?id=' + keyValue + '&status=speak';
                //   document.location.href="Employee_Subscription.aspx"; 
            }
                else if (name == "tab14") {
                //alert(name);
                document.location.href = "StatutoryDocumnentDtls.aspx";
                }

                else if (name == "tab15") {
                    //alert(name);
                    document.location.href = "PFESIDtls.aspx";
                }
                else if (name == "tab16") {
                    //alert(name);
                    document.location.href = "OtherDetails.aspx";
                }
                else if (name == "tab17") {
                    //alert(name);
                    document.location.href = "Resignation.aspx";
                }
              

        }
        function openAreaPage() {

            var left = (screen.width - 300) / 2;
            var top = (screen.height - 250) / 2;
            var cityid = gridAddress.GetEditor("City").GetValue();
            var cityname = gridAddress.GetEditor("City").GetText();
            var URL = 'AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
            if (cityid != null) {
                popupan.SetContentUrl(URL);
                popupan.Show();
            }
            else {

                jAlert('Please select a city first!');
                return false;
            }
        }
        function OnPhoneClick() {
            gridPhone.UpdateEdit();
        }
        function OnEmailClick() {
            //if (gridEmail.GetEditor('eml_type').GetValue() == 'Web Site') {
            //if (gridEmail.GetEditor('eml_website').GetValue() == null)
            //alert('Url Required');
            //else
            //gridEmail.UpdateEdit();
            //}
            //else {
            //if (gridEmail.GetEditor('eml_email').GetValue() == null)
            //alert('Email Required');
            //else
            //gridEmail.UpdateEdit();
            //}
            gridEmail.UpdateEdit();
        }

        function AddressUpdate() {

            var countryname = gridAddress.GetEditor('Country').GetText();
            var pin = gridAddress.GetEditor('PinCode').GetText();
            //if (gridAddress.GetEditor('PinCode').GetValue() == null) {
            //    alert('Pin Code can not be blank');
            //}
            //else {
            //if (countryname == 'India') {
            //    if (pin.length < 6 || IsNumeric(pin) == false) {
            //        alert('Enter valid Pincode');
            //    }
            //    else {
            //        gridAddress.UpdateEdit();
            //    }

            //}
            //else {
            //    gridAddress.UpdateEdit();
            //}
            //}
            gridAddress.UpdateEdit();
        }


        function IsNumeric(strString)
            //  check for valid numeric strings	
        {
            var strValidChars = "0123456789";
            var strChar;
            var blnResult = true;

            if (strString.length == 0) return false;

            //  test strString consists of valid characters listed above
            for (i = 0; i < strString.length && blnResult == true; i++) {
                strChar = strString.charAt(i);
                if (strValidChars.indexOf(strChar) == -1) {
                    blnResult = false;
                }
            }
            return blnResult;
        }
        function HidePopupAndShowInfo() {
            popupan.Hide();

        }
    </script>
    <style>
        .dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute;
        }
    </style>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Employee Correspondence</h3>
            <div class="crossBtn"><a href="employee.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="1" ClientInstanceName="page">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Correspondence" Name="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <dxe:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="0" ClientInstanceName="page">
                                            <TabPages>
                                               <%--Mantis Issue 24639 [ speccing of Address corrected ] --%>
                                                <dxe:TabPage Text="Address">
                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">
                                                            <div style="float: left;">

                                                                <%-- <%if (Convert.ToString(Session["PageAccess"]).Trim() == "All" || Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "DelAdd")
                                                                              { %>--%>
                                                                <% if (rights.CanAdd)
                                                                   { %>
                                                                <a href="javascript:void(0);" onclick="gridAddress.AddNewRow();" class="btn btn-primary"><span>Add New</span> </a>
                                                                <% } %>
                                                                <%-- <%} %>--%>
                                                            </div>
                                                            <dxe:ASPxGridView ID="AddressGrid" runat="server" DataSourceID="Address" ClientInstanceName="gridAddress"
                                                                KeyFieldName="Id" AutoGenerateColumns="False" OnCellEditorInitialize="AddressGrid_CellEditorInitialize"
                                                                Width="100%" Font-Size="12px" OnRowValidating="AddressGrid_RowValidating" OnCommandButtonInitialize="AddressGrid_CommandButtonInitialize" OnStartRowEditing="AddressGrid_StartRowEditing">
                                                                <%-- <Settings ShowFilterRow="true" ShowGroupPanel="true" />--%>
                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn FieldName="Id" Visible="False" VisibleIndex="0" Caption="Id">
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Address Type" FieldName="Type" Visible="False"
                                                                        VisibleIndex="1" EditCellStyle-HorizontalAlign="Left">
                                                                        <PropertiesComboBox ValueType="System.String" Width="260px" ClearButton-DisplayMode="Always" ClearButton-ImagePosition="Right">
                                                                            <Items>

                                                                                <dxe:ListEditItem Text="Residence" Value="Residence"></dxe:ListEditItem>

                                                                                <dxe:ListEditItem Text="Office" Value="Office"></dxe:ListEditItem>

                                                                                <dxe:ListEditItem Text="Correspondence" Value="Correspondence"></dxe:ListEditItem>

                                                                            </Items>


                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">

                                                                                <RequiredField ErrorText="Mandatory" IsRequired="True" />


                                                                            </ValidationSettings>

                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>

                                                                        <EditFormSettings Visible="true" VisibleIndex="1" />

                                                                        <EditCellStyle HorizontalAlign="Left"></EditCellStyle>

                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>

                                                                    <dxe:GridViewDataTextColumn FieldName="Type" VisibleIndex="2" Caption="Type">
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>

                                                                    <dxe:GridViewDataTextColumn FieldName="Address1" VisibleIndex="6" Caption="Address1">
                                                                        <PropertiesTextEdit Width="260px" MaxLength="500"></PropertiesTextEdit>
                                                                        <EditFormSettings Visible="True" VisibleIndex="2" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Address2" VisibleIndex="7" Caption="Address2">
                                                                        <PropertiesTextEdit Width="260px" MaxLength="500"></PropertiesTextEdit>
                                                                        <EditFormSettings Visible="True" VisibleIndex="3" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Address3" VisibleIndex="8" Caption="Address3">
                                                                        <PropertiesTextEdit Width="260px" MaxLength="500"></PropertiesTextEdit>
                                                                        <EditFormSettings Visible="True" VisibleIndex="4" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>

                                                                    <dxe:GridViewDataTextColumn FieldName="LandMark" VisibleIndex="9" Caption="Landmark">
                                                                        <PropertiesTextEdit Width="260px" MaxLength="500"></PropertiesTextEdit>
                                                                        <EditFormSettings Visible="True" VisibleIndex="5" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>

                                                                    <dxe:GridViewDataComboBoxColumn Caption="Country" FieldName="Country" Visible="False"
                                                                        VisibleIndex="3">
                                                                        <PropertiesComboBox DataSourceID="CountrySelect" TextField="Country" ValueField="cou_id" Width="260px"
                                                                            EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String" ClearButton-DisplayMode="Always" ClearButton-ImagePosition="Right">

                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }"></ClientSideEvents>


                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">

                                                                                <RequiredField ErrorText="Mandatory" IsRequired="True" />

                                                                            </ValidationSettings>

                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="6" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>


                                                                    <dxe:GridViewDataComboBoxColumn Caption="State" FieldName="State" Visible="False"
                                                                        VisibleIndex="4">
                                                                        <PropertiesComboBox DataSourceID="StateSelect" TextField="State" ValueField="ID" Width="260px"
                                                                            EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String" ClearButton-DisplayMode="Always" ClearButton-ImagePosition="Right">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                            </ValidationSettings>
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged(s); }"></ClientSideEvents>

                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="7" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>

                                                                    <dxe:GridViewDataTextColumn FieldName="Country1" VisibleIndex="10" Caption="Country">
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>

                                                                    </dxe:GridViewDataTextColumn>

                                                                    <dxe:GridViewDataTextColumn FieldName="State1" VisibleIndex="11" Caption="State">
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>

                                                                    <dxe:GridViewDataTextColumn FieldName="City1" VisibleIndex="12" Caption="City">
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>

                                                                    <dxe:GridViewDataComboBoxColumn Caption="City / District" FieldName="City" VisibleIndex="5"
                                                                        Visible="False">
                                                                        <PropertiesComboBox DataSourceID="SelectCity" TextField="City" ValueField="CityId" Width="260px"
                                                                            EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String"
                                                                            ClearButton-DisplayMode="Always" ClearButton-ImagePosition="Right">
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                            </ValidationSettings>
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCityChanged(s); }"></ClientSideEvents>
                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="8" />
                                                                    </dxe:GridViewDataComboBoxColumn>


                                                                    <dxe:GridViewDataComboBoxColumn Caption="Area" FieldName="add_area" VisibleIndex="13">
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>

                                                                    <dxe:GridViewDataComboBoxColumn Caption="Area Code" FieldName="area" VisibleIndex="14" Visible="false">
                                                                        <PropertiesComboBox ValueType="System.Int32" EnableSynchronization="False" EnableIncrementalFiltering="false" Width="260px"
                                                                            DataSourceID="SelectArea" ValueField="area_id" TextField="area_name" ClearButton-DisplayMode="Always" ClearButton-ImagePosition="Right">
                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Caption="Area" Visible="True" VisibleIndex="9" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>

                                                                    <dxe:GridViewDataHyperLinkColumn Caption="" Visible="false" VisibleIndex="16">
                                                                        <EditFormSettings Visible="true" VisibleIndex="10" />
                                                                        <EditItemTemplate>
                                                                            <a href="#" onclick="javascript:openAreaPage();"><span class="Ecoheadtxt" style="color: Blue">
                                                                                <strong>Add New Area</strong></span></a>
                                                                        </EditItemTemplate>
                                                                    </dxe:GridViewDataHyperLinkColumn>
                                                                    <%--  <dxe:GridViewDataTextColumn FieldName="PinCode" VisibleIndex="15" Caption="Pin Code">
                                                                        <EditFormSettings Visible="True" VisibleIndex="11" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>

                                                                        <PropertiesTextEdit Width="260px" MaxLength="99">

                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" SetFocusOnError="True">


                                                                                <RequiredField ErrorText="Mandatory" IsRequired="True" />

                                                                                 
                                                                            </ValidationSettings>
                                                                            <ClientSideEvents TextChanged="AddressUpdate" />
                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>--%>
                                                                    <%--debjyoti 02-12-2016--%>

                                                                    <dxe:GridViewDataTextColumn FieldName="PinCode1" VisibleIndex="16" Caption="Pin / Zip" Visible="true">
                                                                        <EditFormSettings Visible="false" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>

                                                                    <dxe:GridViewDataComboBoxColumn Caption="Pin / Zip" FieldName="PinCode" Visible="false" VisibleIndex="16">
                                                                        <PropertiesComboBox DataSourceID="SelectPin" TextField="pin_code" ValueField="pin_id" Width="260px"
                                                                            EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String" ClearButton-DisplayMode="Always" ClearButton-ImagePosition="Right">

                                                                            <%--<ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged(s); }"></ClientSideEvents>--%>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">

                                                                                <RequiredField ErrorText="Mandatory" IsRequired="True" />

                                                                            </ValidationSettings>
                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="8" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>


                                                                    <%--End debjyoti 02-12-2016 --%>


                                                                    <dxe:GridViewCommandColumn VisibleIndex="17" ShowDeleteButton="true" ShowEditButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="10%">
                                                                        <%-- <DeleteButton Visible="True">
                                                        </DeleteButton>
                                                        <EditButton Visible="True">
                                                        </EditButton>--%>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                        <HeaderTemplate>
                                                                            Actions
                                                                            <%--<%if (Convert.ToString(Session["PageAccess"]).Trim() == "All" || Convert.ToString(Session["PageAccess"]).Trim() == "Add" || Convert.ToString(Session["PageAccess"]).Trim() == "DelAdd")
                                                                              { %>
                                                                            <a href="javascript:void(0);" onclick="gridAddress.AddNewRow();"><span>Add New</span> </a>
                                                                            <%} %>--%>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsCommandButton>
                                                                    <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                                        <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>

                                                                        <Styles>
                                                                            <Style CssClass="pad"></Style>
                                                                        </Styles>
                                                                    </EditButton>
                                                                    <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                        <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                                                                    </DeleteButton>
                                                                    <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
                                                                        <Styles>
                                                                            <Style CssClass="btn btn-primary"></Style>
                                                                        </Styles>
                                                                    </UpdateButton>
                                                                    <%--<a id="update" href="#" onclick="AddressUpdate()" >Update</a>--%>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                                                                        <Styles>
                                                                            <Style CssClass="btn btn-danger"></Style>
                                                                        </Styles>
                                                                    </CancelButton>
                                                                </SettingsCommandButton>
                                                                <SettingsSearchPanel Visible="True" />
                                                                <Settings ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupPanel="true" ShowFilterRowMenu="true" />
                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="WindowCenter"
                                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                                                                    EditFormColumnCount="1" />
                                                                <Styles>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                </Styles>
                                                                <SettingsText PopupEditFormCaption="Add Address" ConfirmDelete="Confirm delete?"
                                                                    Title="Add Address" />
                                                                <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />
                                                                <SettingsPager NumericButtonCount="20" PageSize="20">
                                                                </SettingsPager>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                <Templates>
                                                                    <EditForm>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 5%"></td>
                                                                                <td style="width: 90%">
                                                                                    <controls>
                               <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors1">
                               </dxe:ASPxGridViewTemplateReplacement>                                                           
                             </controls>
                                                                                    <div style="text-align: left; padding: 2px 2px 2px 102px">
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton1" ReplacementType="EditFormUpdateButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton1" ReplacementType="EditFormCancelButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 5%"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditForm>
                                                                    <TitlePanel>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td align="center" style="color: White;">
                                                                                    <span class="Ecoheadtxt"></span>
                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </TitlePanel>
                                                                </Templates>
                                                            </dxe:ASPxGridView>
                                                            <dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="AddArea_PopUp.aspx"
                                                                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popupan" Height="250px"
                                                                Width="300px" HeaderText="Add New Area" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                                                <ContentCollection>
                                                                    <dxe:PopupControlContentControl runat="server">
                                                                    </dxe:PopupControlContentControl>
                                                                </ContentCollection>
                                                                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                                                            </dxe:ASPxPopupControl>

                                                        </dxe:ContentControl>
                                                    </ContentCollection>
                                                </dxe:TabPage>
                                                <dxe:TabPage Text="Phone">
                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">

                                                            <div style="float: left;">
                                                                <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                              { %>--%>
                                                                <% if (rights.CanAdd)
                                                                   { %>
                                                                <a href="javascript:void(0);" onclick="gridPhone.AddNewRow();" class="btn btn-primary"><span>Add New</span> </a>
                                                                <% } %>
                                                                <%--<%} %>--%>
                                                            </div>
                                                            <dxe:ASPxGridView ID="PhoneGrid" ClientInstanceName="gridPhone" DataSourceID="Phone"
                                                                KeyFieldName="phf_id" runat="server" AutoGenerateColumns="False" Width="100%" OnStartRowEditing="PhoneGrid_StartRowEditing"
                                                                Font-Size="12px" OnRowValidating="PhoneGrid_RowValidating" OnCommandButtonInitialize="PhoneGrid_CommandButtonInitialize">
                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_id" ReadOnly="True" VisibleIndex="0"
                                                                        Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_cntId" ReadOnly="True" VisibleIndex="0"
                                                                        Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_entity" ReadOnly="True" VisibleIndex="0"
                                                                        Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Phone Type" FieldName="phf_type" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox ValueType="System.String" ClearButton-DisplayMode="Always" ClearButton-ImagePosition="Right">
                                                                            <ClientSideEvents SelectedIndexChanged="function(s,e) { OnFieldXChanged1(s) ;}" />
                                                                            <%--  <ClientSideEvents Init="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Mobile&quot;)
    {
         $('#PhoneGrid_DXPEForm_efnew_DXEFL_1,#PhoneGrid_DXPEForm_efnew_DXEFL_2,#PhoneGrid_DXPEForm_efnew_DXEFL_4').hide();
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
    }
    else
    {
         $('#PhoneGrid_DXPEForm_efnew_DXEFL_1,#PhoneGrid_DXPEForm_efnew_DXEFL_2,#PhoneGrid_DXPEForm_efnew_DXEFL_4').show();
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
       
         
    }
}" />
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Mobile&quot;)
    {
         $('#PhoneGrid_DXPEForm_efnew_DXEFL_1,#PhoneGrid_DXPEForm_efnew_DXEFL_2,#PhoneGrid_DXPEForm_efnew_DXEFL_4').hide();
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
        
    }
    else
    {
         $('#PhoneGrid_DXPEForm_efnew_DXEFL_1,#PhoneGrid_DXPEForm_efnew_DXEFL_2,#PhoneGrid_DXPEForm_efnew_DXEFL_4').show();
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
         
    }
}" />--%>

                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
	    var value = s.GetValue();
        
       if(value == &quot;Mobile&quot;)
    {
             gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetEnabled(false);
             gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetEnabled(false);
             gridPhone.GetEditor(&quot;phf_extension&quot;).SetEnabled(false);
       
    }
    else
    {
             gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetEnabled(true);
             gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetEnabled(true);
             gridPhone.GetEditor(&quot;phf_extension&quot;).SetEnabled(true);
        
    }
                                                                                  
    
    }" />

                                                                            <ClientSideEvents Init="function(s, e) {
                                                                               var value = s.GetValue();
                                                                                     
        if(value == &quot;Mobile&quot;)
        {
             gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetEnabled(false);
             gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetEnabled(false);
             gridPhone.GetEditor(&quot;phf_extension&quot;).SetEnabled(false);
            
             
        }
        else
        {
             gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetEnabled(true);
             gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetEnabled(true);
             gridPhone.GetEditor(&quot;phf_extension&quot;).SetEnabled(true);
             
        }
    }" />


                                                                            <Items>

                                                                                <dxe:ListEditItem Text="Residence" Value="Residence"></dxe:ListEditItem>

                                                                                <dxe:ListEditItem Text="Office" Value="Office"></dxe:ListEditItem>

                                                                                <dxe:ListEditItem Text="Correspondence" Value="Correspondence"></dxe:ListEditItem>

                                                                                <dxe:ListEditItem Text="Mobile" Value="Mobile"></dxe:ListEditItem>

                                                                                <dxe:ListEditItem Text="Fax" Value="Fax"></dxe:ListEditItem>

                                                                            </Items>


                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">

                                                                                <RequiredField ErrorText="Select Phone Type" IsRequired="True" />

                                                                            </ValidationSettings>

                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_type" VisibleIndex="0" Caption="Type"
                                                                        Width="40%">
                                                                        <EditFormSettings Caption="Phone Type" Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_countryCode" VisibleIndex="1" Visible="False">
                                                                        <EditFormSettings Caption="Country Code" Visible="True" />
                                                                        <PropertiesTextEdit MaxLength="5">

                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">

                                                                                <RegularExpression ErrorText="Enter Valid Country Code" ValidationExpression="[0-9+]+" />

                                                                            </ValidationSettings>

                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_areaCode" VisibleIndex="1" Visible="False">
                                                                        <EditFormSettings Caption="Area Code" Visible="True" />
                                                                        <PropertiesTextEdit MaxLength="5">

                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">

                                                                                <RegularExpression ErrorText="Enter Valid Area Code" ValidationExpression="[0-9]+" />

                                                                            </ValidationSettings>
                                                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_phoneNumber" VisibleIndex="1" Caption="Number" PropertiesTextEdit-MaxLength="12"
                                                                        Visible="False">
                                                                        <EditFormSettings Visible="True" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="100">

                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">


                                                                                <RequiredField IsRequired="true" ErrorText="Mandatory" />

                                                                                <RegularExpression ErrorText="Enter valid phone number" ValidationExpression="[0-9]+" />


                                                                            </ValidationSettings>
                                                                            <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />
                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Number" VisibleIndex="1" Caption="Phone Number"
                                                                        Width="40%">
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_extension" VisibleIndex="2" Caption="Extension" PropertiesTextEdit-MaxLength="50"
                                                                        Visible="False">
                                                                        <EditFormSettings Visible="True" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit>

                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">

                                                                                <RegularExpression ErrorText="Enter Valid Extension" ValidationExpression="[0-9]+" />

                                                                            </ValidationSettings>

                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewCommandColumn VisibleIndex="2" ShowDeleteButton="true" ShowEditButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="10%">
                                                                        <%--  <DeleteButton Visible="True">
                                                        </DeleteButton>
                                                        <EditButton Visible="True">
                                                        </EditButton>--%>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                        <HeaderTemplate>
                                                                            Actions
                                                                          <%--  <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                              { %>
                                                                            <a href="javascript:void(0);" onclick="gridPhone.AddNewRow();"><span>Add New</span> </a>
                                                                            <%} %>--%>
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsCommandButton>

                                                                    <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                                        <%-- <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>

                                                                        <Styles>
                                                                            <Style CssClass="pad"></Style>
                                                                        </Styles>--%>
                                                                    </EditButton>
                                                                    <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                        <%-- <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>--%>
                                                                    </DeleteButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger ">
                                                                        <Styles>
                                                                            <Style CssClass="btn btn-danger "></Style>
                                                                        </Styles>
                                                                    </CancelButton>
                                                                </SettingsCommandButton>
                                                                <Settings ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupPanel="true" />
                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="WindowCenter"
                                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                                                                    EditFormColumnCount="1" />
                                                                <Styles>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                </Styles>
                                                                <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />
                                                                <SettingsText PopupEditFormCaption="Add Phone" ConfirmDelete="Confirm delete?" />
                                                                <SettingsPager NumericButtonCount="20" PageSize="20">
                                                                </SettingsPager>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                <Templates>
                                                                    <EditForm>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 5%"></td>
                                                                                <td style="width: 90%">
                                                                                    <controls>
                            <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors2">
                            </dxe:ASPxGridViewTemplateReplacement>                                                           
                          </controls>
                                                                                    <div style="padding: 2px 2px 2px 96px">
                                                                                        <%-- <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                            runat="server">
                                                                        </dxe:ASPxGridViewTemplateReplacement>--%>
                                                                                        <a id="update" href="#" class="btn btn-primary " style="color: white; padding: 6px 18px !important;" onclick="OnPhoneClick()">Save
                                                                                        </a>
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton2" ReplacementType="EditFormCancelButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 5%"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditForm>
                                                                    <TitlePanel>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <span class="Ecoheadtxt"></span>
                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </TitlePanel>
                                                                </Templates>
                                                            </dxe:ASPxGridView>
                                                        </dxe:ContentControl>
                                                    </ContentCollection>
                                                </dxe:TabPage>
                                                <dxe:TabPage Text="Email">
                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">
                                                            <div style="float: left;">
                                                                <%-- <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                              { %>--%>
                                                                <% if (rights.CanAdd)
                                                                   { %>
                                                                <a href="javascript:void(0);" onclick="addnewemail();" class="btn btn-primary"><span>Add New</span> </a>
                                                                <% } %>
                                                                <%--  <%} %>--%>
                                                            </div>
                                                            <dxe:ASPxGridView ID="EmailGrid" runat="server" ClientInstanceName="gridEmail"
                                                                DataSourceID="Email" KeyFieldName="eml_id" AutoGenerateColumns="False" Width="100%"
                                                                Font-Size="12px"
                                                                OnRowValidating="EmailGrid_RowValidating" OnCommandButtonInitialize="EmailGrid_CommandButtonInitialize"
                                                                OnStartRowEditing="EmailGrid_StartRowEditing">
                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_id" VisibleIndex="1" Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>



                                                                    <dxe:GridViewDataComboBoxColumn Caption="Email Type" FieldName="eml_type" Visible="False"
                                                                        VisibleIndex="2">

                                                                        <PropertiesComboBox ValueType="System.String" ClearButton-DisplayMode="Always" ClearButton-ImagePosition="Right">

                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Web Site&quot;)
    {
         gridEmail.GetEditor(&quot;eml_email&quot;).SetEnabled(false);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetEnabled(false);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetEnabled(true);
        gridEmail.GetEditor(&quot;Isdefault&quot;).SetEnabled(false);
    }
    else
    {
         gridEmail.GetEditor(&quot;eml_email&quot;).SetEnabled(true);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetEnabled(true);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetEnabled(false);
       
         gridEmail.GetEditor(&quot;eml_email&quot;).SetText('');
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetText('');
         gridEmail.GetEditor(&quot;eml_website&quot;).SetText('');

         gridEmail.GetEditor(&quot;Isdefault&quot;).SetEnabled(true);
    }
}" />
                                                                            <ClientSideEvents Init="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Web Site&quot;)
    {
        gridEmail.GetEditor(&quot;eml_email&quot;).SetEnabled(false);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetEnabled(false);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetEnabled(true);
    }
    else
    {
      
         gridEmail.GetEditor(&quot;eml_email&quot;).SetEnabled(true);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetEnabled(true);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetEnabled(false);
    }
}" />

                                                                            <Items>

                                                                                <dxe:ListEditItem Text="Personal" Value="Personal"></dxe:ListEditItem>

                                                                                <dxe:ListEditItem Text="Official" Value="Official"></dxe:ListEditItem>

                                                                                <dxe:ListEditItem Text="Web Site" Value="Web Site"></dxe:ListEditItem>

                                                                            </Items>


                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">

                                                                                <RequiredField IsRequired="True" ErrorText="Select Type"></RequiredField>

                                                                            </ValidationSettings>


                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>

                                                                    </dxe:GridViewDataComboBoxColumn>

                                                                    <dxe:GridViewDataCheckColumn FieldName="Isdefault" Visible="false" Caption="Default">
                                                                        <EditFormSettings Visible="True" VisibleIndex="3" />
                                                                        <PropertiesCheckEdit>
                                                                            <%--  <ClientSideEvents CheckedChanged="function(s,e){fn_chekFbtRate(s,e);}"/>--%>
                                                                        </PropertiesCheckEdit>
                                                                    </dxe:GridViewDataCheckColumn>

                                                                    <dxe:GridViewDataTextColumn FieldName="eml_type" VisibleIndex="3" Caption="Type"
                                                                        Width="27%">
                                                                        <EditFormSettings Caption="Email Type" Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>

                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_email" VisibleIndex="4" Caption="Email">
                                                                        <EditFormSettings Caption="Email Id" Visible="True" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <PropertiesTextEdit>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RegularExpression ErrorText="Enetr Valid E-Mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                                            </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_ccEmail" VisibleIndex="5" Visible="False">
                                                                        <EditFormSettings Caption="CC Email" Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <PropertiesTextEdit>

                                                                            <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">

                                                                                <RegularExpression ErrorText="Enetr Valid CC EMail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />

                                                                            </ValidationSettings>

                                                                        </PropertiesTextEdit>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_website" Caption="Website" VisibleIndex="9"
                                                                        Visible="true">
                                                                        <EditFormSettings Caption="Website" Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewCommandColumn VisibleIndex="10" ShowDeleteButton="true" ShowEditButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Width="10%">

                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                        <CellStyle HorizontalAlign="Center"></CellStyle>
                                                                        <HeaderTemplate>
                                                                            Actions
                                                                           
                                                                        </HeaderTemplate>
                                                                    </dxe:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsCommandButton>

                                                                    <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                                        <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>

                                                                        <Styles>
                                                                            <Style CssClass="pad"></Style>
                                                                        </Styles>
                                                                    </EditButton>
                                                                    <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
                                                                        <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                                                                    </DeleteButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger ">
                                                                        <Styles>
                                                                            <Style CssClass="btn btn-danger "></Style>
                                                                        </Styles>
                                                                    </CancelButton>
                                                                </SettingsCommandButton>
                                                                <Settings ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupPanel="true" />
                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center"
                                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                                                                    EditFormColumnCount="1" />
                                                                <Styles>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                </Styles>
                                                                <SettingsText PopupEditFormCaption="Add Email" ConfirmDelete="Confirm delete?" />
                                                                <SettingsPager NumericButtonCount="20" PageSize="20">
                                                                </SettingsPager>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                <Templates>
                                                                    <EditForm>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 5%"></td>
                                                                                <td style="width: 90%">
                                                                                    <controls>
                                 <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors3">
                                 </dxe:ASPxGridViewTemplateReplacement>                                                           
                             </controls>
                                                                                    <div style="font: 20px; text-align: center; padding: 2px 2px 2px 2px">
                                                                                        <%-- <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                            runat="server">
                                                                        </dxe:ASPxGridViewTemplateReplacement>--%>
                                                                                        <a id="update1" href="#" class="btn btn-primary " style="color: white; padding: 6px 18px !important;" onclick="OnEmailClick()">Save</a>
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton3" ReplacementType="EditFormCancelButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 5%"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditForm>
                                                                    <TitlePanel>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <span class="Ecoheadtxt"></span>
                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </TitlePanel>
                                                                </Templates>
                                                            </dxe:ASPxGridView>
                                                        </dxe:ContentControl>
                                                    </ContentCollection>
                                                </dxe:TabPage>
                                            </TabPages>
                                        </dxe:ASPxPageControl>
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Education" Text="Education">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Employee" Text="Employment">
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
                            <dxe:TabPage Name="Family Members" Text="Family">
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
                            <dxe:TabPage Name="Employee CTC" Text="CTC">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Bank Details" Text="Bank">
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

                            <dxe:TabPage Name="DP Details" Text="DP" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>


                            <dxe:TabPage Name="Registration" Text="Registration" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>




                            <dxe:TabPage Name="Subscription" Text="subscriptions" Visible="false">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>


                            <dxe:TabPage Name="Language" Text="Language">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="StautoryDocumentDetails" Text="Statutory">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="PFESI" Text="PF/ESI">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                             <dxe:TabPage Name="othrdtls" Text="Other Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="RESIGNATION" Text="Resignation">
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
	                                            var Tab12=page.GetTab(12);
                                                var Tab13 = page.GetTab(13);
                                                var Tab14 = page.GetTab(14);
                                                 var Tab15 = page.GetTab(15);
                                                var Tab16 = page.GetTab(16);
                                                var Tab17 = page.GetTab(17);
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
                                           else if(activeTab == Tab16)
	                                            {
	                                                disp_prompt('tab16');
	                                            }
                                               else if(activeTab == Tab17)
	                                            {
	                                                disp_prompt('tab17');
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
        <asp:HiddenField ID="emailisnew" runat="server" ClientIDMode="Static" />


    </div>
    <asp:SqlDataSource ID="Address" runat="server"
        SelectCommand="select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,
                        tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2, 
                        tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark, 
                        tbl_master_address.add_country AS Country, 
                        tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,
                        CASE isnull(add_country, '')WHEN '' THEN '' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1, 
                        CASE isnull(add_state, '') WHEN '' THEN '' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,                         
                        CASE isnull(add_city, '') WHEN '' THEN '' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,
                        CASE isnull(add_pin, '') WHEN '' THEN '' ELSE(SELECT pin_code FROM tbl_master_pinzip WHERE pin_id = add_pin) END AS PinCode,
                        CASE isnull(add_pin, '') WHEN '' THEN '' ELSE(SELECT pin_code FROM tbl_master_pinzip WHERE pin_id = add_pin) END AS PinCode1,
                        CASE add_area WHEN '' THEN '' Else(select area_name From tbl_master_area Where area_id = add_area) End AS add_area,  area = CAST(add_area as int),                    
                        tbl_master_address.add_landMark AS LankMark 
                        from tbl_master_address where add_cntId=@insuId"
        DeleteCommand="contactDelete"
        DeleteCommandType="StoredProcedure" InsertCommand="insert_correspondence" InsertCommandType="StoredProcedure"
        UpdateCommand="update tbl_master_address set add_addressType=@Type,add_address1=@Address1,add_address2=@Address2,add_address3=@Address3,add_city=@City,add_landMark=@LandMark,add_country=@Country,add_state=@State,add_area=@area,add_pin=@PinCode,LastModifyDate=getdate(),LastModifyUser=@CreateUser where add_id=@Id">

        <SelectParameters>
            <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID" Type="String" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="decimal" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="Type" Type="string" />
            <asp:Parameter Name="Address1" Type="string" />
            <asp:Parameter Name="Address2" Type="string" />
            <asp:Parameter Name="Address3" Type="string" />
            <asp:Parameter Name="City" Type="int32" />
            <asp:Parameter Name="area" Type="int32" />
            <asp:Parameter Name="LandMark" Type="string" />
            <asp:Parameter Name="Country" Type="int32" />
            <asp:Parameter Name="State" Type="int32" />
            <asp:Parameter Name="PinCode" Type="string" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            <asp:Parameter Name="Id" Type="decimal" />
        </UpdateParameters>
        <InsertParameters>
            <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID" Type="String" />
            <asp:Parameter Name="Type" Type="string" />
            <asp:SessionParameter Name="contacttype" SessionField="ContactType" Type="string" />
            <asp:Parameter Name="Address1" Type="string" />
            <asp:Parameter Name="Address2" Type="string" />
            <asp:Parameter Name="Address3" Type="string" />
            <asp:Parameter Name="City" Type="int32" />
            <asp:Parameter Name="area" Type="int32" />
            <asp:Parameter Name="LandMark" Type="string" />
            <asp:Parameter Name="Country" Type="int32" />
            <asp:Parameter Name="State" Type="int32" />
            <asp:Parameter Name="PinCode" Type="string" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="CountrySelect" runat="server"
        SelectCommand="SELECT cou_id, cou_country as Country FROM tbl_master_country order by cou_country"></asp:SqlDataSource>
    <asp:SqlDataSource ID="StateSelect" runat="server"
        SelectCommand="SELECT s.id as ID,s.state as State from tbl_master_state s where (s.countryId = @State) ORDER BY s.state">
        <SelectParameters>
            <asp:Parameter Name="State" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <%--debjyoti 02-12-2016--%>
    <asp:SqlDataSource ID="SelectPin" runat="server"
        SelectCommand="select pin_id,pin_code from tbl_master_pinzip where city_id=@City order by pin_code">
        <SelectParameters>
            <asp:Parameter Name="City" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <%--End Debjyoti 02-12-2016--%>
    <asp:SqlDataSource ID="SelectCity" runat="server"
        SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">
        <SelectParameters>
            <asp:Parameter Name="City" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SelectArea" runat="server"
        SelectCommand="SELECT area_id, area_name from tbl_master_area where (city_id = @Area) ORDER BY area_name">
        <SelectParameters>
            <asp:Parameter Name="Area" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="Phone" runat="server"
        DeleteCommand="PhoneDelete" DeleteCommandType="StoredProcedure"
        InsertCommand="insert_correspondence_phone" InsertCommandType="StoredProcedure"
        SelectCommand="select DISTINCT phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number 
                      from tbl_master_phonefax where phf_cntId=@PhfId"
        UpdateCommand="INSERT INTO tbl_master_phonefax_Log (phf_id, phf_cntId, phf_entity, phf_type, phf_countryCode, phf_areaCode, phf_phoneNumber, phf_faxNumber, phf_extension, phf_Availablefrom, phf_AvailableTo, phf_SMSFacility, phf_IsDefault, CreateDate, CreateUser, LastModifyDate, LastModifyUser, LastModifyDate_DLMAST, LogModifyDate, LogModifyUser, LogStatus) SELECT phf_id, phf_cntId, phf_entity, phf_type, phf_countryCode, phf_areaCode, phf_phoneNumber, phf_faxNumber, phf_extension, phf_Availablefrom, phf_AvailableTo, phf_SMSFacility, phf_IsDefault, CreateDate, CreateUser, LastModifyDate, LastModifyUser, LastModifyDate_DLMAST,getdate(),@CreateUser,'M' FROM tbl_master_phonefax WHERE  phf_id=@phf_id update tbl_master_phonefax set phf_type=@phf_type,phf_countryCode=@phf_countryCode,phf_areaCode=@phf_areaCode,phf_phoneNumber=@phf_phoneNumber,phf_extension=@phf_extension,LastModifyDate=getdate(),LastModifyUser=@CreateUser where phf_id=@phf_id">
        <SelectParameters>
            <asp:SessionParameter Name="PhfId" SessionField="KeyVal_InternalID" Type="String" />
        </SelectParameters>
        <InsertParameters>
            <asp:SessionParameter Name="PhfId" SessionField="KeyVal_InternalID" Type="String" />
            <asp:Parameter Name="phf_type" Type="string" />
            <asp:SessionParameter Name="contacttype" SessionField="ContactType" Type="string" />
            <asp:Parameter Name="phf_countryCode" Type="string" />
            <asp:Parameter Name="phf_areaCode" Type="string" />
            <asp:Parameter Name="phf_phoneNumber" Type="string" />
            <asp:Parameter Name="phf_extension" Type="string" />
            <asp:Parameter Name="phf_Availablefrom" Type="string" />
            <asp:Parameter Name="phf_AvailableTo" Type="string" />
            <asp:Parameter Name="phf_SMSFacility" Type="string" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="phf_type" Type="string" />
            <asp:Parameter Name="phf_countryCode" Type="string" />
            <asp:Parameter Name="phf_areaCode" Type="string" />
            <asp:Parameter Name="phf_phoneNumber" Type="string" />
            <asp:Parameter Name="phf_extension" Type="string" />
            <asp:Parameter Name="phf_Availablefrom" Type="string" />
            <asp:Parameter Name="phf_AvailableTo" Type="string" />
            <asp:Parameter Name="phf_SMSFacility" Type="string" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            <asp:Parameter Name="phf_id" Type="decimal" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="phf_id" Type="int32" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="Email" runat="server"
        DeleteCommand="EmailDelete" DeleteCommandType="StoredProcedure"
        InsertCommand="insert_correspondence_email" InsertCommandType="StoredProcedure"
        SelectCommand="select Isdefault,eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser from tbl_master_email where eml_cntId=@EmlId"
        UpdateCommand="INSERT INTO tbl_master_email_Log (eml_id, eml_internalId, eml_entity, eml_cntId, eml_type, eml_email, eml_ccEmail, eml_website, CreateDate, CreateUser, LastModifyDate, LastModifyUser, LastModifyDate_DLMAST, LogModifyDate, LogModifyUser, LogStatus) SELECT eml_id, eml_internalId, eml_entity, eml_cntId, eml_type, eml_email, eml_ccEmail, eml_website, CreateDate, CreateUser, LastModifyDate, LastModifyUser, LastModifyDate_DLMAST,getdate(),@CreateUser,'M' FROM tbl_master_email WHERE eml_id=@eml_id update tbl_master_email set Isdefault=@Isdefault,eml_type=@eml_type,eml_email=@eml_email,eml_ccEmail=@eml_ccEmail,eml_website=@eml_website,LastModifyDate=getdate(),LastModifyUser=@CreateUser,eml_facility=@eml_facility where eml_id=@eml_id">
        <DeleteParameters>
            <asp:Parameter Name="eml_id" Type="int32" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="eml_type" Type="string" />
            <asp:Parameter Name="eml_email" Type="string" />
            <asp:Parameter Name="eml_ccEmail" Type="string" />
            <asp:Parameter Name="eml_website" Type="string" />
            <asp:Parameter Name="eml_id" Type="decimal" />
            <asp:Parameter Name="eml_facility" Type="int32" />
            <asp:Parameter Name="Isdefault" Type="int32" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
        </UpdateParameters>
        <SelectParameters>
            <asp:SessionParameter Name="EmlId" SessionField="KeyVal_InternalID" Type="string" />
        </SelectParameters>
        <InsertParameters>
            <asp:SessionParameter Name="EmlId" SessionField="KeyVal_InternalID" Type="string" />
            <asp:Parameter Name="eml_type" Type="string" />
            <asp:SessionParameter Name="contacttype" SessionField="ContactType" Type="string" />
            <asp:Parameter Name="eml_email" Type="string" />
            <asp:Parameter Name="eml_ccEmail" Type="string" />
            <asp:Parameter Name="eml_website" Type="string" />
            <asp:Parameter Name="eml_facility" Type="int32" />
            <asp:Parameter Name="Isdefault" Type="int32" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:HiddenField ID="hdKeyVal" runat="server" />
</asp:Content>

