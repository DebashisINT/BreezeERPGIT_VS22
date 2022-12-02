<%@ Page Title="Correspondence" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="Bank_Correspondence.aspx.cs" Inherits="ERP.OMS.Management.Master.Bank_Correspondence" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /*.crossBtn {
      
    right: 14px !important;
    top: 22px!important;
             }*/
        
        /*.dxeErrorCell_PlasticBlue  {
            position: absolute;
            margin-top: 6px;
        }*/
        .dxeErrorFrameSys.dxeErrorCellSys, .dxeErrorFrameSys.dxeErrorCellSys {
            position:absolute !important;
        }
    </style>
    <script lang="javascript" type="text/javascript">
        function ul() {
            window.opener.document.getElementById('iFrmInformation').setAttribute('src', 'CallUserInformation.aspx')
        }

        function OnCountryChanged(cmbCountry) {

            
            gridAddress.GetEditor("City").PerformCallback('0');
            gridAddress.GetEditor("area").PerformCallback('0');
            gridAddress.GetEditor("PinCode").PerformCallback('0');// change cmdcountry.getvalue() to 0 on 07122016
            gridAddress.GetEditor("State").PerformCallback(cmbCountry.GetValue().toString());

            //alert("asda");

        }
        function OnStateChanged(cmbState) {

            gridAddress.GetEditor("area").PerformCallback('0');
            //gridAddress.GetEditor("City").PerformCallback('0');
            gridAddress.GetEditor("PinCode").PerformCallback('0');
            gridAddress.GetEditor("City").PerformCallback(cmbState.GetValue().toString());

        }
        function OnCityChanged(cmbCity) {
            gridAddress.GetEditor("area").PerformCallback(cmbCity.GetValue().toString());
            gridAddress.GetEditor("PinCode").PerformCallback(cmbCity.GetValue().toString());
        }

        //................ Code Added by Sam on 20102016..................
        function OnEmilTypeChanged(emiltype) {
            var tes = emiltype.GetValue().toString();
            gridEmail.PerformCallback(emiltype.GetValue().toString());
        }

        function OnPhoneTypeChanged(phonetype) {

            //var ph = phf_type.GetValue().toString();

            //alert(tes);

            gridPhone.PerformCallback(phf_type.GetValue().toString());
        }

        function HidePopupAndShowInfo() {
            popupan.Hide();

        }





        //<dxe:ListEditItem Text="Official (For sending Emails)" Value="Official"></dxe:ListEditItem>
        //                                                                            <dxe:ListEditItem Text="Personal" Value="Personal"></dxe:ListEditItem>
        //                                                                            <dxe:ListEditItem Text="Web Site" Value="Web Site"></dxe:ListEditItem>




        //................ Code Added by Sam on 20102016..................
        function OnChildCall(cmbCity) {
            OnCityChanged(gridAddress.GetEditor("City"));
        }
        function openAreaPage() {
            var left = (screen.width - 300) / 2;
            var top = (screen.height - 250) / 2;
            var cityid = gridAddress.GetEditor("City").GetValue();
            var cityname = gridAddress.GetEditor("City").GetText();
            var URL = 'AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
            //if (cityid != null) {
            //    window.open(URL, '50', 'resizable=1,height=200px,width=500px,top=' + top + ',left=' + left + '');
            //}
            //else {
            //    alert('Please select a city first.');
            //    return false;
            //}
            popupan.SetContentUrl(URL);
            //alert (url);
            popupan.Show();
        }
        function disp_prompt(name) {
            //var ID = document.getElementById(txtID);
            if (name == "tab0") {
                //alert(name);
                document.location.href = "Bank_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href="Bank_Correspondence.aspx"; 
            }
            //comment due to incompleted implemnetation

            //else if (name == "tab2") {
            //    //alert(name);
            //    document.location.href = "Contact_BankDetails.aspx";
            //}
            //else if (name == "tab3") {
            //    //alert(name);
            //    document.location.href = "Contact_DPDetails.aspx";
            //}
            //else if (name == "tab4") {
            //    //alert(name);
            //    document.location.href = "Contact_Document.aspx";
            //}
            //else if (name == "tab12") {
            //    //alert(name);
            //    document.location.href = "Contact_FamilyMembers.aspx";
            //}
            //else if (name == "tab5") {
            //    //alert(name);
            //    document.location.href = "Contact_Registration.aspx";
            //}
            //else if (name == "tab7") {
            //    //alert(name);
            //    document.location.href = "Contact_GroupMember.aspx";
            //}
            //else if (name == "tab8") {
            //    //alert(name);
            //    document.location.href = "Contact_Deposit.aspx";
            //}
            //else if (name == "tab9") {
            //    //alert(name);
            //    document.location.href = "Contact_Remarks.aspx";
            //}
            //else if (name == "tab10") {
            //    //alert(name);
            //    document.location.href = "Contact_Education.aspx";
            //}
            //else if (name == "tab11") {
            //    //alert(name);
            //    document.location.href = "contact_brokerage.aspx";
            //}
            //else if (name == "tab6") {
            //    //alert(name);
            //    document.location.href = "contact_other.aspx";
            //}
            //else if (name == "tab13") {
            //    document.location.href = "contact_Subscription.aspx";
            //}

        }
        function OnPhoneClick() {
            //if (gridPhone.GetEditor('phf_phoneNumber').GetValue() == null) {
            //    alert('Phone Number Required');
            //}
            //else {
            gridPhone.UpdateEdit();
            //}
        }
        function OnEmailClick() {
            if (gridEmail.GetEditor('eml_type').GetValue() == 'Web Site') {
                if (gridEmail.GetEditor('eml_website').GetValue() == null)
                    jAlert('Url Required');
                else
                    gridEmail.UpdateEdit();
            }
            else {
                if (gridEmail.GetEditor('eml_email').GetValue() == null)
                    jAlert('Email Required');
                else
                    gridEmail.UpdateEdit();
            }
        }
        function Emailcheck(obj) {
            if (obj == 'c') {
                jAlert("This email id has already exists for other contacts.");
            }

        }


        //-----------For Address Status -------------------
        function btnSave_Click() {
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
            if (obj == 'Y') {
                popup.Hide();
                jAlert("Successfully update");
                gridAddress.PerformCallback();
            }
            else if (obj == 'Y1') {
                var msg = 'Cannot proceed. Registered/Permanent Address is already exist as "Active".\n You can set only one Registered/Permanent Address as "Active". ';
                alert(msg);//added by sanjib 20122016

            }
            else if (obj == 'Y2') {
                var msg = 'Cannot proceed. Residence Address is already exist as "Active".\n You can set only one Residence Address as "Active".';
                alert(msg);//added by sanjib 20122016
            }
            else if (obj == 'Y3') {
                var msg = 'Cannot proceed. Office Address is already exist as "Active".\n You can set only one Office Address as "Active".';
                alert(msg);//added by sanjib 20122016
            }



        }
        function btnCancel_Click() {
            popup.Hide();
        }

        //-----------For Phone Status -------------------
        function btnSave_ClickP() {
            var obj = 'SaveOld~' + RowIDP;
            popPanelP.PerformCallback(obj);

        }

        function OnAddEditClickP(e, obj) {

            var data = obj.split('~');
            if (data.length > 1)
                RowIDP = data[1];
            popupP.Show();
            popPanelP.PerformCallback(obj);
        }
        function EndCallBackP(obj) {
            if (obj == 'Y') {
                popupP.Hide();
                jAlert("Successfully Updated");
                gridPhone.PerformCallback();
            }


        }
        function btnCancel_ClickP() {
            popupP.Hide();
        }


        //-----------For Email Status -------------------
        function btnSave_ClickE() {
            var obj = 'SaveOld~' + RowIDE;
            popPanelE.PerformCallback(obj);

        }

        function OnAddEditClickE(e, obj) {

            var data = obj.split('~');
            if (data.length > 1)
                RowIDE = data[1];
            popupE.Show();
            popPanelE.PerformCallback(obj);
        }
        function EndCallBackE(obj) {
            if (obj == 'Y') {
                popupE.Hide();
                jAlert("Successfully Updated");
                gridEmail.PerformCallback();
            }


        }
        function btnCancel_ClickE() {
            popupE.Hide();
        }
        function AddressUpdate() {
            gridAddress.UpdateEdit();


            //var countryname = gridAddress.GetEditor('Country').GetText();
            //var pin = gridAddress.GetEditor('PinCode').GetText();
            ////if (gridAddress.GetEditor('PinCode').GetValue() == null) {
            ////    alert('Pin Code can not be blank');
            ////}
            ////else {
            //    if (countryname == 'India') {
            //        if (pin.length < 6 || IsNumeric(pin) == false) {
            //            alert('Enter valid Pincode');
            //        }
            //        else {
            //            gridAddress.UpdateEdit();
            //        }

            //    }
            //    else {
            //        gridAddress.UpdateEdit();
            //    }
            //}
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

        //function addgrid_endcallback()
        //{

        //    gridAddress.GetEditor("State").PerformCallback(cmbCountry.GetValue().toString());
        //    gridAddress.GetEditor("City").PerformCallback(cmbState.GetValue().toString());
        //    gridAddress.GetEditor("area").PerformCallback(cmbCity.GetValue().toString());
        //    alert('hi');
        //}
    </script>
    <%--<style>
        .dxeButtonEdit_PlasticBlue {
            width:100%;
        }
    </style>--%>
    <style type="text/css">
        .dxeValidStEditorTable td.dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute !important;
        }

        .dxeValidStEditorTable[errorframe="errorFrame"] {
            width: 100% !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="panel-heading">
        <div class="panel-title">
            <%--<h3>Contact Correspondence List</h3>--%>
            <h3>
                <asp:Label ID="lblHeadTitle" runat="server" Text="Bank Correspondence"></asp:Label>
            </h3>
            <div class="crossBtn"><a href="bank.aspx"><i class="fa fa-times"></i></a></div>
        </div>

    </div>
    <div class="form_main">
        <div>
            <table width="100%">
                <tr>
                    <td class="EHEADER" style="text-align: center">
                        <asp:Label ID="lblName" runat="server" Font-Size="12px" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
            </table>
            <table class="TableMain100">
                <tr>
                    <td>
                        <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="1" ClientInstanceName="page"
                            OnActiveTabChanged="ASPxPageControl1_ActiveTabChanged">
                            <TabPages>
                                <dxe:TabPage Name="General" Text="General">

                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                                <dxe:TabPage Name="Correspondence" Text="Correspondence">

                                    <ContentCollection>
                                        <dxe:ContentControl runat="server">
                                            <dxe:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="0" ClientInstanceName="page">
                                                <TabPages>

                                                    <dxe:TabPage Name="Adress" Text="Address">

                                                        <ContentCollection>
                                                            <dxe:ContentControl runat="server">
                                                                <div style="float: left;">
                                                                    <a href="javascript:void(0);" class="btn btn-primary" onclick="gridAddress.AddNewRow();"><span>Add New</span> </a>
                                                                </div>

                                                                 <div class="pull-left">
                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>


                                <%-- <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                    <asp:ListItem Value="2">XLS</asp:ListItem>
                                    <asp:ListItem Value="3">RTF</asp:ListItem>
                                    <asp:ListItem Value="4">CSV</asp:ListItem>

                                </asp:DropDownList>--%>
                            </div>

                                                                <dxe:ASPxGridView ID="AddressGrid" runat="server" DataSourceID="Address" ClientInstanceName="gridAddress"
                                                                    KeyFieldName="Id" AutoGenerateColumns="False" OnCellEditorInitialize="AddressGrid_CellEditorInitialize"
                                                                    Width="100%" OnCustomCallback="AddressGrid_CustomCallback" OnRowValidating="AddressGrid_RowValidating" OnCommandButtonInitialize="AddressGrid_CommandButtonInitialize">


                                                                    <Columns>

                                                                        <dxe:GridViewDataTextColumn FieldName="Id" Visible="False" VisibleIndex="0" Caption="Id">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                        </dxe:GridViewDataTextColumn>


                                                                        <dxe:GridViewDataComboBoxColumn Caption="Address Type" FieldName="Type" Visible="False"
                                                                            VisibleIndex="19" Width="100%">
                                                                            <PropertiesComboBox ValueType="System.String">
                                                                                <Items>


                                                                                    <dxe:ListEditItem Text="Registered/Permanent Address" Value="Registered"></dxe:ListEditItem>


                                                                                    <dxe:ListEditItem Text="Residence" Value="Residence"></dxe:ListEditItem>


                                                                                    <dxe:ListEditItem Text="Office" Value="Office"></dxe:ListEditItem>

                                                                                    <dxe:ListEditItem Text="Billing" Value="Billing"></dxe:ListEditItem>


                                                                                    <dxe:ListEditItem Text="Factory/Work/Branch" Value="FactoryWorkBranch"></dxe:ListEditItem>



                                                                                </Items>

                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">

                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />

                                                                                </ValidationSettings>

                                                                            </PropertiesComboBox>
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormSettings Visible="True" VisibleIndex="1" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataComboBoxColumn>

                                                                        <dxe:GridViewDataTextColumn FieldName="Type" VisibleIndex="0" Caption="Type">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                        </dxe:GridViewDataTextColumn>


                                                                        <dxe:GridViewDataTextColumn FieldName="Address1" VisibleIndex="1" Caption="Address1" PropertiesTextEdit-MaxLength="500">
                                                                            <EditFormSettings Visible="True" VisibleIndex="2" />
                                                                            <CellStyle CssClass="gridcellleft abc">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit Width="100%"></PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>

                                                                        <dxe:GridViewDataTextColumn FieldName="Address2" VisibleIndex="2" Caption="Address2" PropertiesTextEdit-MaxLength="500">
                                                                            <EditFormSettings Visible="True" VisibleIndex="3" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit Width="100%"></PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>



                                                                        <dxe:GridViewDataTextColumn FieldName="LandMark" VisibleIndex="4" Caption="Landmark" PropertiesTextEdit-MaxLength="500"
                                                                            Visible="false">
                                                                            <EditFormSettings Visible="True" VisibleIndex="5" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit Width="100%"></PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>

                                                                        <dxe:GridViewDataComboBoxColumn Caption="Country" FieldName="Country" Visible="False"
                                                                            VisibleIndex="18">
                                                                            <PropertiesComboBox DataSourceID="CountrySelect" TextField="Country" ValueField="cou_id"
                                                                                EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String">
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory" />
                                                                                </ValidationSettings>
                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }"></ClientSideEvents>
                                                                                <%-- <ValidationSettings CausesValidation="True" SetFocusOnError="True">
                                                                                    <RequiredField IsRequired="True" ErrorText="Country Required" />
                                                                                </ValidationSettings>--%>
                                                                            </PropertiesComboBox>
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormSettings Visible="True" VisibleIndex="6" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataComboBoxColumn>

                                                                        <dxe:GridViewDataComboBoxColumn Caption="State" FieldName="State" Visible="False"
                                                                            VisibleIndex="17">
                                                                            <PropertiesComboBox DataSourceID="StateSelect" TextField="State" ValueField="ID" Width="100%"
                                                                                EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String">
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



                                                                        <dxe:GridViewDataTextColumn FieldName="Country1" VisibleIndex="5" Caption="Country">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>

                                                                        <dxe:GridViewDataTextColumn FieldName="State1" VisibleIndex="6" Caption="State">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>

                                                                        <dxe:GridViewDataTextColumn FieldName="City1" VisibleIndex="7" Caption="City">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>

                                                                        <dxe:GridViewDataComboBoxColumn Caption="City  / District" FieldName="City" VisibleIndex="16"
                                                                            Visible="False">
                                                                            <PropertiesComboBox DataSourceID="SelectCity" TextField="City" ValueField="CityId" Width="100%"
                                                                                EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String">
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

                                                                       <dxe:GridViewDataTextColumn Caption="Area" FieldName="add_area" VisibleIndex="9" Visible="true">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>

                                                                        <dxe:GridViewDataComboBoxColumn Caption="Area" FieldName="area"  Visible="false">
                                                                            <PropertiesComboBox ValueType="System.Int32" DataSourceID="SelectArea" EnableSynchronization="False" Width="100%"
                                                                                EnableIncrementalFiltering="True" ValueField="area_id" TextField="area">
                                                                            </PropertiesComboBox>
                                                                            <EditFormSettings Caption="Area" Visible="True" VisibleIndex="9" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <EditFormSettings Visible="True" VisibleIndex="9" />
                                                                        </dxe:GridViewDataComboBoxColumn>

                                                                        <dxe:GridViewDataHyperLinkColumn Visible="false" VisibleIndex="14">
                                                                            <EditFormSettings Visible="true" VisibleIndex="10" />
                                                                            <EditItemTemplate>
                                                                                <a href="javascript:void(0);" onclick="openAreaPage();"><span class="Ecoheadtxt">
                                                                                    <strong>Add New Area</strong></span></a>
                                                                            </EditItemTemplate>
                                                                        </dxe:GridViewDataHyperLinkColumn>
                                                                        <%--Debjyoti 02-12-2016--%>
                                                                        <%--<dxe:GridViewDataTextColumn FieldName="PinCode" VisibleIndex="9" Caption="Pincode">
                                                                            <EditFormSettings Visible="True" VisibleIndex="12" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <PropertiesTextEdit>
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                    <RequiredField IsRequired="True" />
                                                                                </ValidationSettings>
                                                                            </PropertiesTextEdit>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>--%>
                                                                           <dxe:GridViewDataTextColumn FieldName="PinCode1" VisibleIndex="8" Caption="Pincode / Zip" Visible="true" >
                                                                            <EditFormSettings Visible="false" VisibleIndex="12" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <PropertiesTextEdit>
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                    <RequiredField IsRequired="True" />
                                                                                </ValidationSettings>
                                                                            </PropertiesTextEdit>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataComboBoxColumn Caption="Pincode / Zip" FieldName="PinCode" Visible="false">
                                                                            <PropertiesComboBox DataSourceID="SelectPin" TextField="pin_code" ValueField="pin_id" Width="100%"
                                                                                EnableSynchronization="False" EnableIncrementalFiltering="True" ValueType="System.String" ClearButton-DisplayMode="Always" ClearButton-ImagePosition="Right">

                                                                                <%--<ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged(s); }"></ClientSideEvents>--%>
                                                                            <ClearButton DisplayMode="Always" ImagePosition="Right"></ClearButton>

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

                                                                      <%--  <dxe:GridViewDataTextColumn FieldName="PinCode1" VisibleIndex="8" Caption="Pin / Zip">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>--%>

                                                                        <%--End debjyoti02-12-2016--%>
                                                                        <dxe:GridViewDataTextColumn FieldName="status" VisibleIndex="10" Caption="Status">
                                                                            <DataItemTemplate>
                                                                                <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Edit~'+'<%# Container.KeyValue %>')">
                                                                                    <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='<%# Eval("status")%>'
                                                                                        ToolTip="Click to Change Status">
                                                                                    </dxe:ASPxLabel>
                                                                                </a>
                                                                            </DataItemTemplate>
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle Wrap="False">
                                                                            </CellStyle>
                                                                            <HeaderTemplate>
                                                                                Status
                                                                            </HeaderTemplate>
                                                                            <HeaderStyle Wrap="False" />
                                                                        </dxe:GridViewDataTextColumn>

                                                                        <dxe:GridViewCommandColumn VisibleIndex="11" ShowDeleteButton="true" ShowEditButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">

                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                            <CellStyle HorizontalAlign="Center"></CellStyle>

                                                                            <HeaderTemplate>
                                                                                Actions
                                                                                <%-- <% if (rights.CanAdd)
                                                                                     { %>--%>
                                                                                <%--<a href="javascript:void(0);" onclick="gridAddress.AddNewRow();"><span>Add New</span> </a>--%>
                                                                                <%-- <% } %>--%>
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
                                                                        <UpdateButton Text="Save" ButtonType="Button"></UpdateButton>
                                                                        <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                                                                            <Styles>
                                                                                <Style CssClass="btn btn-danger"></Style>
                                                                            </Styles>
                                                                        </CancelButton>
                                                                    </SettingsCommandButton>

                                                                    <SettingsSearchPanel Visible="True" />
                                                                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />



                                                                    <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="WindowCenter"
                                                                        PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="450px"
                                                                        EditFormColumnCount="1" />

                                                                    <Styles>
                                                                        <LoadingPanel ImageSpacing="10px">
                                                                        </LoadingPanel>
                                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                        </Header>
                                                                    </Styles>

                                                                    <SettingsText PopupEditFormCaption="Add/Modify Address" ConfirmDelete="Confirm delete?" />
                                                                    <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                                        <FirstPageButton Visible="True">
                                                                        </FirstPageButton>
                                                                        <LastPageButton Visible="True">
                                                                        </LastPageButton>
                                                                    </SettingsPager>
                                                                    <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                    <%--<ClientSideEvents EndCallback="function(s,e){addgrid_endcallback()}" />--%>

                                                                    <Templates>
                                                                        <TitlePanel>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 50%">
                                                                                        <span class="Ecoheadtxt" style="color: Black">Add/Modify Address.</span>
                                                                                    </td>

                                                                                </tr>
                                                                            </table>
                                                                        </TitlePanel>
                                                                        <EditForm>
                                                                            <div style="color: red; margin-top: 5px; margin-left: 5px;">* Denotes the mandatory field.</div>
                                                                            <div style="margin: 8px 8px 0px 8px">
                                                                                <table style="width: 100%" style="">

                                                                                    <tr>
                                                                                        <td style="width: 5%;"></td>
                                                                                        <td style="width: 90%;">

                                                                                            <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>


                                                                                            <div style="text-align: left; padding: 2px 2px 2px 110px">

                                                                                                <a id="update" href="#" onclick="AddressUpdate()" class="btn btn-primary " style="color: white; padding: 6px 18px !important;">Save</a>
                                                                                                <div class="dxbButton" style="display: inline-block; padding: 3px">
                                                                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                                </div>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="width: 5%;"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </EditForm>

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

                                                                <dxe:ASPxPopupControl ID="ASPxPopupControl1" ClientInstanceName="popup" runat="server"
                                                                    AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Set Address Status"
                                                                    EnableHotTrack="False" BackColor="#DDECFE" Width="400px" CloseAction="CloseButton">
                                                                    <ContentCollection>
                                                                        <dxe:PopupControlContentControl runat="server">
                                                                            <dxe:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="400px" ClientInstanceName="popPanel"
                                                                                OnCallback="ASPxCallbackPanel1_Callback" OnCustomJSProperties="ASPxCallbackPanel1_CustomJSProperties">
                                                                                <PanelCollection>
                                                                                    <dxe:PanelContent runat="server">
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 25%">Status:
                                                                                                </td>
                                                                                                <td style="width: 50%">
                                                                                                    <asp:DropDownList ID="cmbStatus" runat="server" Width="180px" TabIndex="0">
                                                                                                        <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                                                                                        <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="width: 25%">Date:
                                                                                                </td>
                                                                                                <td style="width: 50%">
                                                                                                    <dxe:ASPxDateEdit ID="StDate" runat="server" ClientInstanceName="StDate" EditFormat="Custom"
                                                                                                        UseMaskBehavior="True" Width="179px" TabIndex="1">
                                                                                                        <ButtonStyle Width="13px">
                                                                                                        </ButtonStyle>
                                                                                                    </dxe:ASPxDateEdit>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="width: 25%">Reason:
                                                                                                </td>
                                                                                                <td style="width: 75%">
                                                                                                    <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Width="250px" TabIndex="2" MaxLength="50"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="width: 25%"></td>
                                                                                                <td colspan="2" class="gridcellleft" style="width: 75%">
                                                                                                    <%-- <input id="Button1" type="button" value="Save" class="btnUpdate dxbButton" onclick="btnSave_Click()"
                                                                                                        style="width: 60px" tabindex="41" />--%>
                                                                                                    <input id="Button1" type="button" value="Save" class="btnUpdate btn btn-primary" onclick="btnSave_Click()"
                                                                                                        style="width: 60px" tabindex="3" />

                                                                                                    <%--<input id="Button2" type="button" value="Cancel" class="btnUpdate dxbButton" onclick="btnCancel_Click()"
                                                                                                        style="width: 60px" tabindex="42" />--%>
                                                                                                    <input id="Button2" type="button" value="Cancel" class="btnUpdate btn btn-danger" onclick="btnCancel_Click()"
                                                                                                        style="width: 60px" tabindex="4" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </dxe:PanelContent>
                                                                                </PanelCollection>
                                                                                <ClientSideEvents EndCallback="function(s, e) {
	                                                        EndCallBack(s.cpLast);
                                                        }" />
                                                                            </dxe:ASPxCallbackPanel>
                                                                        </dxe:PopupControlContentControl>
                                                                    </ContentCollection>
                                                                    <HeaderStyle HorizontalAlign="Left">
                                                                        <Paddings PaddingRight="6px" />
                                                                    </HeaderStyle>
                                                                    <SizeGripImage Height="16px" Width="16px" />
                                                                    <CloseButtonImage Height="12px" Width="13px" />
                                                                    <ClientSideEvents CloseButtonClick="function(s, e) {
	     popup.Hide();
    }" />
                                                                </dxe:ASPxPopupControl>
                                                            </dxe:ContentControl>
                                                        </ContentCollection>
                                                    </dxe:TabPage>




                                                    <dxe:TabPage Name="Phone" Text="Phone">

                                                        <ContentCollection>
                                                            <dxe:ContentControl runat="server">
                                                                <div style="float: left;">
                                                                    <a href="javascript:void(0);" class="btn btn-primary" onclick="gridPhone.AddNewRow();"><span>Add New</span> </a>
                                                                </div>
                                                                  
                                                                <dxe:ASPxGridView ID="PhoneGrid" ClientInstanceName="gridPhone" DataSourceID="Phone"
                                                                    KeyFieldName="phf_id" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                    OnRowValidating="PhoneGrid_RowValidating" OnCustomCallback="PhoneGrid_CustomCallback"
                                                                    OnBeforeGetCallbackResult="PhoneGrid_BeforeGetCallbackResult" OnCommandButtonInitialize="PhoneGrid_CommandButtonInitialize">
                                                                    <SettingsSearchPanel Visible="True" />
                                                                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
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
                                                                        <dxe:GridViewDataComboBoxColumn Caption="Phone Type" FieldName="phf_type" VisibleIndex="1" Visible="false">
                                                                            <PropertiesComboBox ValueType="System.String">
                                                                                <%--<ClientSideEvents SelectedIndexChanged="function(s, e) { OnPhoneTypeChanged(s); }"></ClientSideEvents>--%>







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


                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">


                                                                                    <RequiredField ErrorText="Mandatory" IsRequired="True" />

                                                                                </ValidationSettings>


                                                                            </PropertiesComboBox>
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormSettings Visible="True" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataComboBoxColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="phf_countryCode" VisibleIndex="2" Visible="False">
                                                                            <EditFormSettings Caption="Country Code" Visible="True" />
                                                                            <PropertiesTextEdit MaxLength="5">


                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">


                                                                                    <RegularExpression ErrorText="Mandatory" ValidationExpression="[0-9+]+" />



                                                                                </ValidationSettings>



                                                                            </PropertiesTextEdit>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="phf_areaCode" VisibleIndex="3" Visible="False">
                                                                            <EditFormSettings Caption="Area Code" Visible="True" />
                                                                            <PropertiesTextEdit MaxLength="5">



                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">



                                                                                    <RegularExpression ErrorText="Enter Valid Area Code" ValidationExpression="[0-9]+" />

                                                                                </ValidationSettings>


                                                                                <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />

                                                                            </PropertiesTextEdit>

                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="phf_phoneNumber" VisibleIndex="4" Caption="Number"
                                                                            Visible="False">
                                                                            <EditFormSettings Visible="True" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <PropertiesTextEdit MaxLength="100">

                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">

                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory"></RequiredField>

                                                                                    <RegularExpression ErrorText="Enter Valid Phone Number" ValidationExpression="[0-9]+" />

                                                                                </ValidationSettings>

                                                                                <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />

                                                                            </PropertiesTextEdit>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="Number" VisibleIndex="5" Caption="Phone Number"
                                                                            Width="40%">
                                                                            <EditFormSettings Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="status" VisibleIndex="6" Caption="Status">
                                                                            <DataItemTemplate>
                                                                                <a href="javascript:void(0);" onclick="OnAddEditClickP(this,'Edit~'+'<%# Container.KeyValue %>')">
                                                                                    <dxe:ASPxLabel ID="ASPxLabel1" runat="server" Text='<%# Eval("status")%>' Width="100%"
                                                                                        ToolTip="Click to Change Status">
                                                                                    </dxe:ASPxLabel>
                                                                                </a>
                                                                            </DataItemTemplate>
                                                                            <EditFormSettings Visible="false" VisibleIndex="3" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Change Status" Visible="false">
                                                                            <DataItemTemplate>
                                                                                <a href="javascript:void(0);" onclick="OnAddEditClickP(this,'Edit~'+'<%# Container.KeyValue %>')">
                                                                                    <u>Change Status</u> </a>
                                                                            </DataItemTemplate>
                                                                            <EditFormSettings Visible="false" VisibleIndex="4" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <%-- <dxe:GridViewDataComboBoxColumn Caption="Sms alert by Stock Exchange" FieldName="phf_SMSFacility"
                                                                        VisibleIndex="9" Visible="false">
                                                                        <PropertiesComboBox ValueType="System.String">
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Yes" Value="1"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="No" Value="2"></dxe:ListEditItem>
                                                                            </Items>
                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>--%>
                                                                        <dxe:GridViewDataTextColumn FieldName="phf_extension" VisibleIndex="6" Caption="Extension"
                                                                            Visible="False">
                                                                            <EditFormSettings Visible="True" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <PropertiesTextEdit MaxLength="50">

                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">

                                                                                    <RegularExpression ErrorText="Enter Valid Extension" ValidationExpression="[0-9]+" />

                                                                                </ValidationSettings>

                                                                                <ClientSideEvents KeyPress="function(s,e){ fn_AllowonlyNumeric(s,e);}" />

                                                                            </PropertiesTextEdit>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewCommandColumn VisibleIndex="8" ShowDeleteButton="true" ShowEditButton="true" HeaderStyle-HorizontalAlign="Center" Width="6%">

                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                            <HeaderTemplate>
                                                                                Actions
                                                                                <%-- <% if (rights.CanAdd)
                                                                                     { %>--%>
                                                                                <%--<a href="javascript:void(0);" onclick="gridPhone.AddNewRow();"><span>Add New</span> </a>--%>
                                                                                <%--  <% } %>--%>
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
                                                                    <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="WindowCenter"
                                                                        PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="450px"
                                                                        EditFormColumnCount="1" />
                                                                    <Styles>
                                                                        <LoadingPanel ImageSpacing="10px">
                                                                        </LoadingPanel>
                                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                        </Header>
                                                                    </Styles>
                                                                    <SettingsText PopupEditFormCaption="Add/Modify Phone" ConfirmDelete="Confirm delete?" />
                                                                    <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                                        <FirstPageButton Visible="True">
                                                                        </FirstPageButton>
                                                                        <LastPageButton Visible="True">
                                                                        </LastPageButton>
                                                                    </SettingsPager>
                                                                    <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                    <Templates>
                                                                        <EditForm>
                                                                            <div style="color: red; margin-top: 5px; margin-left: 5px;">* Denotes the mandatory field.</div>
                                                                            <div style="color: #000; margin: 5px 5px 0px 5px">
                                                                                <table style="width: 100%">
                                                                                    <tr>
                                                                                        <td style="width: 5%"></td>
                                                                                        <td style="width: 90%" align="">
                                                                                            <controls>
                                                    <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                                    </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                  </controls>
                                                                                            <%--<hr />--%>

                                                                                            <div style="padding: 2px 2px 2px 93px">
                                                                                                <%-- <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                                runat="server">
                                                                            </dxe:ASPxGridViewTemplateReplacement>--%>
                                                                                                <%-- onclick="OnPhoneClick()"--%>
                                                                                                <a id="update" href="#" class="btn btn-primary " onclick="OnPhoneClick()" style="color: white; padding: 6px 18px !important;">Save</a> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                            <div class="dxbButton" style="display: inline-block; padding: 3px">
                                                                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                            </div>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="width: 5%"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </EditForm>
                                                                        <TitlePanel>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 50%">
                                                                                        <span class="Ecoheadtxt" style="color: Black">Add/Modify Phone.</span>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </TitlePanel>
                                                                    </Templates>
                                                                </dxe:ASPxGridView>
                                                                <dxe:ASPxPopupControl ID="ASPxPopupControlP" ClientInstanceName="popupP" runat="server"
                                                                    AllowDragging="True" PopupHorizontalAlign="WindowCenter" HeaderText="Set Phone Status"
                                                                    EnableHotTrack="False" BackColor="#DDECFE" Width="400px" CloseAction="CloseButton">
                                                                    <ContentCollection>
                                                                        <dxe:PopupControlContentControl runat="server">
                                                                            <dxe:ASPxCallbackPanel ID="ASPxCallbackPanelP" runat="server" Width="400px" ClientInstanceName="popPanelP"
                                                                                OnCallback="ASPxCallbackPanelP_Callback" OnCustomJSProperties="ASPxCallbackPanelP_CustomJSProperties">
                                                                                <PanelCollection>
                                                                                    <dxe:PanelContent runat="server">
                                                                                        <div style="color: #000;">
                                                                                            <table style="width: 100%">
                                                                                                <tr>
                                                                                                    <td>Status:
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:DropDownList ID="cmbStatusP" runat="server" Width="180px">
                                                                                                            <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                                                                                            <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                                                                                        </asp:DropDownList>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Date:
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <dxe:ASPxDateEdit ID="StDateP" runat="server" ClientInstanceName="StDate" Width="179px" EditFormat="Custom"
                                                                                                            UseMaskBehavior="True" TabIndex="21">
                                                                                                            <ButtonStyle Width="13px">
                                                                                                            </ButtonStyle>
                                                                                                        </dxe:ASPxDateEdit>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>Reason:
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtReasonP" runat="server" TextMode="MultiLine" Width="250px"></asp:TextBox>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td></td>
                                                                                                    <td colspan="2" class="gridcellleft">
                                                                                                        <input id="Button3" type="button" value="Save" class="btnUpdate btn btn-primary" onclick="btnSave_ClickP()"
                                                                                                            style="width: 60px" tabindex="41" />
                                                                                                        <input id="Button4" type="button" value="Cancel" class="btnUpdate btn btn-danger" onclick="btnCancel_ClickP()"
                                                                                                            style="width: 60px" tabindex="42" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </dxe:PanelContent>
                                                                                </PanelCollection>
                                                                                <ClientSideEvents EndCallback="function(s, e) {
	                                                        EndCallBackP(s.cpLast);
                                                        }" />
                                                                            </dxe:ASPxCallbackPanel>
                                                                        </dxe:PopupControlContentControl>
                                                                    </ContentCollection>
                                                                    <HeaderStyle HorizontalAlign="Left">
                                                                        <Paddings PaddingRight="6px" />
                                                                    </HeaderStyle>
                                                                    <SizeGripImage Height="16px" Width="16px" />
                                                                    <CloseButtonImage Height="12px" Width="13px" />
                                                                    <ClientSideEvents CloseButtonClick="function(s, e) {
	     popup.Hide();
    }" />
                                                                </dxe:ASPxPopupControl>
                                                            </dxe:ContentControl>
                                                        </ContentCollection>
                                                    </dxe:TabPage>
                                                    <dxe:TabPage Name="Email" Text="Email">

                                                        <ContentCollection>
                                                            <dxe:ContentControl runat="server">
                                                                <div style="float: left;">
                                                                    <% if (rights.CanAdd)
                                                                       { %>
                                                                    <a href="javascript:void(0);" class="btn btn-primary" onclick="gridEmail.AddNewRow();"><span>Add New</span> </a>
                                                                    <% } %>
                                                                </div>
                                                                <dxe:ASPxGridView ID="EmailGrid" runat="server" ClientInstanceName="gridEmail"
                                                                    DataSourceID="Email" KeyFieldName="eml_id" AutoGenerateColumns="False" Width="100%"
                                                                    OnCustomCallback="EmailGrid_CustomCallback" OnRowValidating="EmailGrid_RowValidating"
                                                                    OnBeforeGetCallbackResult="EmailGrid_BeforeGetCallbackResult" OnCancelRowEditing="EmailGrid_CancelRowEditing"
                                                                    OnCustomJSProperties="EmailGrid_CustomJSProperties" OnCommandButtonInitialize="EmailGrid_CommandButtonInitialize">
                                                                   <SettingsSearchPanel Visible="True" />
                                                                     <Settings ShowFilterRow="true" ShowGroupPanel="true"  ShowFilterRowMenu="true"/>
                                                                    <Columns>
                                                                        <dxe:GridViewDataTextColumn FieldName="eml_id" VisibleIndex="0" Visible="False">
                                                                            <EditFormSettings Visible="False" />
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataComboBoxColumn Caption="Email Type" FieldName="eml_type" Visible="False"
                                                                            VisibleIndex="1">
                                                                            <PropertiesComboBox ValueType="System.String" Width="100%">







                                                                                <%--  <ClientSideEvents SelectedIndexChanged="function(s, e) { OnEmilTypeChanged(s); }"></ClientSideEvents>--%>








                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) { gridEmail.PerformCallback(); }" />








                                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) {
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







                                                                                    <dxe:ListEditItem Text="Official (For sending Emails)" Value="Official"></dxe:ListEditItem>







                                                                                    <dxe:ListEditItem Text="Personal" Value="Personal"></dxe:ListEditItem>







                                                                                    <dxe:ListEditItem Text="Web Site" Value="Web Site"></dxe:ListEditItem>







                                                                                </Items>














                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">







                                                                                    <RequiredField IsRequired="True" ErrorText="Mandatory"></RequiredField>







                                                                                </ValidationSettings>







                                                                            </PropertiesComboBox>
                                                                            <EditFormSettings Visible="True" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataComboBoxColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="eml_type" VisibleIndex="2" Caption="Type"
                                                                            Width="27%">
                                                                            <EditFormSettings Caption="Email Type" Visible="False" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="eml_email" VisibleIndex="3" Caption="Email">
                                                                            <EditFormSettings Caption="Email ID" Visible="True" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit MaxLength="200" Width="100%">







                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">
                                                                                    <%--<RequiredField IsRequired="true" ErrorText="Mandatory" />--%>







                                                                                    <RegularExpression ErrorText="Enter valid Email ID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />







                                                                                </ValidationSettings>







                                                                            </PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="eml_ccEmail" VisibleIndex="4" Visible="False">
                                                                            <EditFormSettings Caption="CC Email ID" Visible="True" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit MaxLength="200" Width="100%">







                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">







                                                                                    <RegularExpression ErrorText="Enter Valid CC Email ID" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />







                                                                                </ValidationSettings>







                                                                            </PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="status" VisibleIndex="6" Caption="Status">
                                                                            <DataItemTemplate>
                                                                                <a href="javascript:void(0);" onclick="OnAddEditClickE(this,'Edit~'+'<%# Container.KeyValue %>')">
                                                                                    <dxe:ASPxLabel ID="ASPxLabel2" runat="server" Text='<%# Eval("status")%>' Width="100%"
                                                                                        ToolTip="Click to Change Status">
                                                                                    </dxe:ASPxLabel>
                                                                                </a>
                                                                            </DataItemTemplate>
                                                                            <EditFormSettings Visible="false" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewDataTextColumn FieldName="eml_website" Caption="Website" VisibleIndex="5"
                                                                            Visible="true">
                                                                            <EditFormSettings Caption="Website" Visible="true" />
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                            <PropertiesTextEdit MaxLength="50">
                                                                                <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="true">
                                                                                    <RegularExpression ValidationExpression="(http(s)?://)?([\w-]+\.)+[\w-]+[\w-]+[\.]+[\.com]+([./?%&=]*)?" ErrorText="Enter valid url" />

                                                                                </ValidationSettings>
                                                                            </PropertiesTextEdit>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <%-- <dxe:GridViewDataComboBoxColumn Caption="" FieldName="eml_facility"
                                                                        VisibleIndex="9" Visible="false">
                                                                        <PropertiesComboBox ValueType="System.String">
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Yes" Value="1"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="No" Value="2"></dxe:ListEditItem>
                                                                            </Items>
                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>--%>
                                                                        <dxe:GridViewDataTextColumn VisibleIndex="7" Caption="Change Status" Visible="false">
                                                                            <DataItemTemplate>
                                                                                <a href="javascript:void(0);" onclick="OnAddEditClickE(this,'Edit~'+'<%# Container.KeyValue %>')">
                                                                                    <u>Change Status</u> </a>
                                                                            </DataItemTemplate>
                                                                            <EditFormSettings Visible="false" />
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                            <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                            </EditFormCaptionStyle>
                                                                        </dxe:GridViewDataTextColumn>
                                                                        <dxe:GridViewCommandColumn VisibleIndex="8" ShowDeleteButton="true" ShowEditButton="true" HeaderStyle-HorizontalAlign="Center" Width="6%" CellStyle-HorizontalAlign="Center">

                                                                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                                                            <CellStyle HorizontalAlign="Center"></CellStyle>

                                                                            <HeaderTemplate>
                                                                                Actions
                                                                                <%-- <% if (rights.CanAdd)
                                                                                     { %>--%>
                                                                                <%-- <a href="javascript:void(0);" onclick="gridEmail.AddNewRow();"><span>Add New</span> </a>--%>
                                                                                <%-- <% } %>--%>
                                                                            </HeaderTemplate>
                                                                        </dxe:GridViewCommandColumn>
                                                                        <%-- <dxe:GridViewCommandColumn VisibleIndex="3">
                                                            <EditButton Visible="True">
                                                            </EditButton>
                                                        </dxe:GridViewCommandColumn>--%>
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
                                                                    <SettingsSearchPanel Visible="True" />
                                                                    <Settings ShowStatusBar="Visible" ShowTitlePanel="True" ShowGroupPanel="true" ShowFilterRow="true" ShowFilterRowMenu="true" />

                                                                    <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center"
                                                                        PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px"
                                                                        EditFormColumnCount="1" />
                                                                    <Styles>
                                                                        <LoadingPanel ImageSpacing="10px">
                                                                        </LoadingPanel>
                                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                        </Header>
                                                                    </Styles>
                                                                    <SettingsText PopupEditFormCaption="Add/Modify Email" ConfirmDelete="Confirm delete?" />
                                                                    <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                                        <FirstPageButton Visible="True">
                                                                        </FirstPageButton>
                                                                        <LastPageButton Visible="True">
                                                                        </LastPageButton>
                                                                    </SettingsPager>
                                                                    <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                    <Templates>
                                                                        <EditForm>
                                                                            <div style="color: red; margin-top: 5px; margin-left: 5px;">* Denotes the mandatory field.</div>
                                                                            <div style="color: #000; margin: 8px 8px 0px 8px">
                                                                                <table style="width: 100%">
                                                                                    <tr>
                                                                                        <td style="width: 5%"></td>
                                                                                        <td style="width: 90%">

                                                                                            <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>

                                                                                            <div style="padding: 2px 2px 2px 89px;">
                                                                                                <%-- <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                                                runat="server">
                                                                            </dxe:ASPxGridViewTemplateReplacement>--%>
                                                                                                <a id="update1" href="#" class="btn btn-primary " style="color: white; padding: 6px 18px !important;" onclick="OnEmailClick()">Save</a> &nbsp;&nbsp;&nbsp;&nbsp;
                                                                                            <div class="dxbButton" style="display: inline-block; padding: 3px">
                                                                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                            </div>
                                                                                            </div>
                                                                                        </td>
                                                                                        <td style="width: 5%"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </EditForm>
                                                                        <TitlePanel>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 50%">
                                                                                        <span class="Ecoheadtxt" style="color: black">Add/Modify Email.</span>
                                                                                    </td>
                                                                                    <%-- <td align="right">
                                                  <table >
                                                     <tr>
    <td>
                                                           <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"   Height="18px" Width="88px" AutoPostBack="False" Font-Size="12px">
                                                                <clientsideevents click="function(s, e) {gridEmail.AddNewRow();}" />
                                                           </dxe:ASPxButton>
                                                        </td>
                                                    </tr>
                                                  </table>
                                                </td>   --%>
                                                                                </tr>
                                                                            </table>
                                                                        </TitlePanel>
                                                                    </Templates>
                                                                    <ClientSideEvents EndCallback="function(s, e) {
	    Emailcheck(s.cpHeight);
    }" />
                                                                </dxe:ASPxGridView>
                                                                <dxe:ASPxPopupControl ID="ASPxPopupControlE" ClientInstanceName="popupE" runat="server"
                                                                    AllowDragging="True" PopupHorizontalAlign="WindowCenter" HeaderText="Set Email Status"
                                                                    EnableHotTrack="False" BackColor="#DDECFE" Width="400px" CloseAction="CloseButton">
                                                                    <ContentCollection>
                                                                        <dxe:PopupControlContentControl runat="server">
                                                                            <dxe:ASPxCallbackPanel ID="ASPxCallbackPanelE" runat="server" Width="400px" ClientInstanceName="popPanelE"
                                                                                OnCallback="ASPxCallbackPanelE_Callback" OnCustomJSProperties="ASPxCallbackPanelE_CustomJSProperties">
                                                                                <PanelCollection>
                                                                                    <dxe:PanelContent runat="server">
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td>Status:
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="cmbStatusE" runat="server" Width="180px">
                                                                                                        <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                                                                                        <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>Date:
                                                                                                </td>
                                                                                                <td>
                                                                                                    <dxe:ASPxDateEdit ID="StDateE" runat="server" ClientInstanceName="StDate" EditFormat="Custom"
                                                                                                        UseMaskBehavior="True" Width="179px" TabIndex="21">
                                                                                                        <ButtonStyle Width="13px">
                                                                                                        </ButtonStyle>
                                                                                                    </dxe:ASPxDateEdit>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>Reason:
                                                                                                </td>
                                                                                                <td>
                                                                                                    <%-- <asp:TextBox ID="txtReasonE" runat="server" TextMode="MultiLine" Width="250px" Height="80px"></asp:TextBox>--%>
                                                                                                    <dxe:ASPxMemo ID="txtReasonE" runat="server" Width="250px" Height="80px" MaxLength="50"></dxe:ASPxMemo>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td></td>
                                                                                                <td colspan="2" class="gridcellleft">
                                                                                                    <input id="Button5" type="button" value="Save" class="btnUpdate btn btn-primary" onclick="btnSave_ClickE()"
                                                                                                        tabindex="41" />
                                                                                                    <input id="Button6" type="button" value="Cancel" class="btnUpdate btn btn-danger" onclick="btnCancel_ClickE()"
                                                                                                        tabindex="42" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </dxe:PanelContent>
                                                                                </PanelCollection>
                                                                                <ClientSideEvents EndCallback="function(s, e) {
	                                                        EndCallBackE(s.cpLast);
                                                        }" />
                                                                            </dxe:ASPxCallbackPanel>
                                                                        </dxe:PopupControlContentControl>
                                                                    </ContentCollection>
                                                                    <HeaderStyle HorizontalAlign="Left">
                                                                        <Paddings PaddingRight="6px" />
                                                                    </HeaderStyle>
                                                                    <SizeGripImage Height="16px" Width="16px" />
                                                                    <CloseButtonImage Height="12px" Width="13px" />
                                                                    <ClientSideEvents CloseButtonClick="function(s, e) {
	     popup.Hide();
    }" />
                                                                </dxe:ASPxPopupControl>
                                                            </dxe:ContentControl>
                                                        </ContentCollection>
                                                    </dxe:TabPage>
                                                </TabPages>
                                            </dxe:ASPxPageControl>
                                        </dxe:ContentControl>
                                    </ContentCollection>
                                </dxe:TabPage>
                               
                            </TabPages>
                            <ClientSideEvents ActiveTabChanged="function(s, e) {
	                                                var activeTab   = page.GetActiveTab();
	                                                var Tab0 = page.GetTab(0);
	                                                var Tab1 = page.GetTab(1);
	                                                
	                                               
	                                                if(activeTab == Tab0)
	                                                {
	                                                    disp_prompt('tab0');
	                                                }
	                                                if(activeTab == Tab1)
	                                                {
	                                                    disp_prompt('tab1');
	                                                }
	                                                
	                                                
	                                                }"></ClientSideEvents>
                            <ContentStyle>
                                <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                            </ContentStyle>
                            <LoadingPanelStyle ImageSpacing="6px">
                            </LoadingPanelStyle>
                            <TabStyle>
                            </TabStyle>
                        </dxe:ASPxPageControl>
                        <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
          <dxe:ASPxGridViewExporter ID="exporter" runat="server"  Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true" >
        </dxe:ASPxGridViewExporter>
       

        <asp:SqlDataSource ID="Address" runat="server" 
            SelectCommand="select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,
                        tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2, 
                        tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark, 
                        tbl_master_address.add_country AS Country, 
                        tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,CASE add_pin WHEN '' THEN '' ELSE(SELECT pin_code FROM tbl_master_pinzip WHERE pin_id = add_pin) END AS PinCode1,
                        CASE add_country WHEN '' THEN '' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1, 
                        CASE add_state WHEN '' THEN '' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,
                        CASE add_city WHEN '' THEN '' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,
                        CASE add_area WHEN '' THEN '' Else(select area_name From tbl_master_area Where area_id = add_area) End AS add_area, area = CAST(add_area as int),
                         tbl_master_address.add_pin AS PinCode, tbl_master_address.add_landMark AS LankMark ,
                            case when add_status='N' then 'Deactive' else 'Active' end as status                     
                            from tbl_master_address where add_cntId=@insuId"
            DeleteCommand="contactDelete"
            DeleteCommandType="StoredProcedure" InsertCommand="insert_correspondence" UpdateCommand="update tbl_master_address set add_addressType=@Type,add_address1=@Address1,add_address2=@Address2,add_address3=@Address3,add_city=@City,add_landMark=@LandMark,add_country=@Country,add_state=@State,add_area=@area,add_pin=@PinCode,LastModifyDate=getdate(),LastModifyUser=@CreateUser where add_id=@Id"
            InsertCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID_New" Type="String" />
            </SelectParameters>
            <DeleteParameters>
                <asp:Parameter Name="Id" Type="int32" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="int32" />
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
                <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID_New" Type="String" />
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
        <asp:SqlDataSource ID="SelectCity" runat="server" 
            SelectCommand="SELECT c.city_id AS CityId, c.city_name AS City FROM tbl_master_city c where c.state_id=@City order by c.city_name">
            
            <SelectParameters>
                <asp:Parameter Name="City" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SelectArea" runat="server" 
            SelectCommand="SELECT area_id = CAST(area_id as int), area_name as area from tbl_master_area where (city_id = @Area) ORDER BY area_name">
            <SelectParameters>
                <asp:Parameter Name="Area" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        &nbsp;
    <asp:SqlDataSource ID="Phone" runat="server" 
        DeleteCommand="PhoneDelete" DeleteCommandType="StoredProcedure" InsertCommand="insert_correspondence_phone"
        SelectCommand="select DISTINCT phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') +' '+ ISNULL(phf_extension, '') + ISNULL(phf_faxNumber, '') AS Number , case when phf_Status='N' then 'Deactive' else 'Active' end as status ,isnull(phf_SMSFacility,'') as  phf_SMSFacility
                          from tbl_master_phonefax where phf_cntId=@PhfId"
        UpdateCommand="update tbl_master_phonefax set phf_type=@phf_type,phf_countryCode=@phf_countryCode,phf_areaCode=@phf_areaCode,phf_phoneNumber=@phf_phoneNumber,
                           phf_extension=@phf_extension,LastModifyDate=getdate(),LastModifyUser=@CreateUser,phf_SMSFacility=(case when ltrim(rtrim(@phf_type))='Mobile' then @phf_SMSFacility else '2' end) where phf_id=@phf_id"
        InsertCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter Name="PhfId" SessionField="KeyVal_InternalID_New" Type="String" />
        </SelectParameters>
        <InsertParameters>
            <asp:SessionParameter Name="PhfId" SessionField="KeyVal_InternalID_New" Type="String" />
            <asp:Parameter Name="phf_type" Type="string" />
            <asp:SessionParameter Name="contacttype" SessionField="ContactType" Type="string" />
            <asp:Parameter Name="phf_countryCode" Type="string" />
            <asp:Parameter Name="phf_areaCode" Type="string" />
            <asp:Parameter Name="phf_phoneNumber" Type="string" />
            <asp:Parameter Name="phf_extension" Type="string" />
            <asp:Parameter Name="phf_Availablefrom" Type="string" />
            <asp:Parameter Name="phf_AvailableTo" Type="string" />
            <asp:Parameter Name="phf_SMSFacility" Type="int32" />
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
            <asp:Parameter Name="phf_SMSFacility" Type="int32" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            <asp:Parameter Name="phf_id" Type="decimal" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="phf_id" Type="int32" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="int32" />
        </DeleteParameters>
    </asp:SqlDataSource>


        <%--debjyoti 02-12-2016--%>
        <asp:SqlDataSource ID="SelectPin" runat="server" 
            SelectCommand="select pin_id,pin_code from tbl_master_pinzip where city_id=@City order by pin_code">
            <SelectParameters>
                <asp:Parameter Name="City" Type="string" />
            </SelectParameters>
        </asp:SqlDataSource>
        <%--End Debjyoti 02-12-2016--%>

        <asp:SqlDataSource ID="Email" runat="server" 
            DeleteCommand="EmailDelete" DeleteCommandType="StoredProcedure" InsertCommand="insert_correspondence_email"
            InsertCommandType="StoredProcedure" SelectCommand="select eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser,case when eml_Status='N' then 'Deactive' else 'Active' end as status,(case when eml_facility=1 then '1' when eml_facility=2 then '2' else null end) as eml_facility from tbl_master_email where eml_cntId=@EmlId"
            UpdateCommand="update tbl_master_email set eml_type=@eml_type,eml_email=@eml_email,eml_ccEmail=@eml_ccEmail,eml_website=@eml_website,LastModifyDate=getdate(),LastModifyUser=@CreateUser,eml_facility=(case when ltrim(rtrim(@eml_type))='Official' then @eml_facility else '2' end) where eml_id=@eml_id">
            <DeleteParameters>
                <asp:Parameter Name="eml_id" Type="int32" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="eml_type" Type="string" />
                <asp:Parameter Name="eml_email" Type="string" />
                <asp:Parameter Name="eml_ccEmail" Type="string" />
                <asp:Parameter Name="eml_website" Type="string" />
                <asp:Parameter Name="eml_id" Type="decimal" />
                <asp:Parameter Name="eml_facility" Type="int32" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            </UpdateParameters>
            <SelectParameters>
                <asp:SessionParameter Name="EmlId" SessionField="KeyVal_InternalID_New" Type="string" />
            </SelectParameters>
            <InsertParameters>
                <asp:SessionParameter Name="EmlId" SessionField="KeyVal_InternalID_New" Type="string" />
                <asp:Parameter Name="eml_type" Type="string" />
                <asp:SessionParameter Name="contacttype" SessionField="ContactType" Type="string" />
                <asp:Parameter Name="eml_email" Type="string" />
                <asp:Parameter Name="eml_ccEmail" Type="string" />
                <asp:Parameter Name="eml_website" Type="string" />
                <asp:Parameter Name="eml_facility" Type="int32" />
                <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            </InsertParameters>
        </asp:SqlDataSource>
    </div>
</asp:Content>
