<%@ Page Title="Correspondence" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/Erp.Master"
    Inherits="ERP.OMS.Management.Master.management_master_HRrecruitmentagent_Correspondence" CodeBehind="HRrecruitmentagent_Correspondence.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        ///---------------------Arindam---------------------

        function SetDataSourceOnComboBox(ControlObject, Source) {
            ControlObject.ClearItems();
            for (var count = 0; count < Source.length; count++) {
                ControlObject.AddItem(Source[count].name, Source[count].id);
            }
          //  ControlObject.SetSelectedIndex(0);
        }

        //------------------------------------------

        function init_distancecorrespondence()
        {
            DistanceEnable();
        }

        function DistanceEnable() {




            var AddressType = gridAddress.GetEditor("Type").GetValue();
            if (AddressType == "Billing" || AddressType == "Shipping") {
                var Distance = gridAddress.GetEditor("Distance");

                Distance.SetEnabled(true);
            }
            else {
                var Distance = gridAddress.GetEditor("Distance");
                Distance.SetValue("0.00");
                Distance.SetEnabled(false);

            }

        }
        function onChangeTypeVendor()
        {
            DistanceEnable();
        }

        function OnCountryChanged(cmbCountry) {
           
            //console.log(cmbCountry.GetValue());
            var OtherDetails = {}
            OtherDetails.country_id = cmbCountry.GetValue();
            //gridAddress.GetEditor("State").PerformCallback(cmbCountry.GetValue().toString());

            gridAddress.GetEditor("City").PerformCallback('0');
            gridAddress.GetEditor("area").PerformCallback('0');
            gridAddress.GetEditor("PinCode").PerformCallback('0');
            //---------------------------------------------------------Arindam-----------------------------------------------------//

            $.ajax({
                type: "POST",
                url: "HRrecruitmentagent_Correspondence.aspx/GetAllStateByCountry",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;

                    SetDataSourceOnComboBox(gridAddress.GetEditor("State"), returnObject);

                },
                error: function (response) {

                    console.log(response);
                }
            });

            //---------------------------------------------------------Arindam-----------------------------------------------------//

        }
        function OnStateChanged(cmbState) {

            
            //gridAddress.GetEditor("City").PerformCallback(cmbState.GetValue().toString());
            gridAddress.GetEditor("area").PerformCallback('0');
            gridAddress.GetEditor("PinCode").PerformCallback('0');

            //---------------------------------------------------------Arindam-----------------------------------------------------//
            var OtherDetails = {}
            OtherDetails.state_id = cmbState.GetValue();
            $.ajax({
                type: "POST",
                url: "HRrecruitmentagent_Correspondence.aspx/GetAllCityBYState",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d;

                    SetDataSourceOnComboBox(gridAddress.GetEditor("City"), returnObject);

                },
                error: function (response) {

                    console.log(response);
                }
            });

            //---------------------------------------------------------Arindam-----------------------------------------------------//
        }
        function OnCityChanged(cmbCity) {
           // gridAddress.GetEditor("area").PerformCallback(cmbCity.GetValue().toString());
           // gridAddress.GetEditor("PinCode").PerformCallback(cmbCity.GetValue().toString());

            //---------------------------------------------------------Arindam-----------------------------------------------------//
            var OtherDetails = {}
            OtherDetails.city_id = cmbCity.GetValue();
            $.ajax({
                type: "POST",
                url: "HRrecruitmentagent_Correspondence.aspx/GetAllPinAreaByCity",
                data: JSON.stringify(OtherDetails),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var returnObject = msg.d._receivedata_area;
                    var returnObject1 = msg.d._receivedatapin;

                    SetDataSourceOnComboBox(gridAddress.GetEditor("PinCode"), returnObject1);
                    SetDataSourceOnComboBox(gridAddress.GetEditor("area"), returnObject);

                },
                error: function (response) {

                    console.log(response);
                }
            });

            //---------------------------------------------------------Arindam-----------------------------------------------------//
        }
        function OnChildCall(cmbCity) {
            OnCityChanged(gridAddress.GetEditor("City"));
        }
        function openAreaPage() {
            var left = (screen.width - 300) / 2;
            var top = (screen.height - 250) / 2;
            var cityid = gridAddress.GetEditor("City").GetValue();
            var cityname = gridAddress.GetEditor("City").GetText();
            var URL = 'AddArea_PopUp.aspx?id=' + cityid + '&name=' + cityname + '';
            if (cityid != null) {
                //window.open(URL, '50', 'resizable=1,height=100px,width=300px,top=' + top + ',left=' + left + '');
                
                popuparea.SetContentUrl(URL);
                popuparea.Show();
            }
            else {
                jAlert('Please select a city first!');
                return false;
            }
        }
        function disp_prompt(name) { 
            if (name == "tab0") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_general.aspx";
            }
            if (name == "tab1") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_ContactPerson.aspx";
            }
            else if (name == "tab2") {
                //alert(name);
                //document.location.href="HRrecruitmentagent_Correspondence.aspx";         
            }
            else if (name == "tab3") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_BankDetails.aspx";
            }
                //else if (name == "tab4") {
                //    //alert(name);
                //    document.location.href = "HRrecruitmentagent_DPDetails.aspx";
                //}
            else if (name == "tab4") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_Document.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_Registration.aspx";
            }
            else if (name == "tab5") {
                //alert(name);
                document.location.href = "HRrecruitmentagent_GroupMember.aspx";
            }
            else if (name == "tab7") { 
                document.location.href = "vendors_tds.aspx";
            }
        }
    </script>
    <style>
        .dxeValidStEditorTable td.dxeErrorFrameSys.dxeErrorCellSys {
            position: absolute !important;
        }

        .dxeValidStEditorTable[errorframe="errorFrame"] {
            width: 100% !important;
        }
        .dxeErrorFrameSys.dxeErrorCellSys {
            position:absolute;
        }
    </style>

    <%--Add Action Func() in Email, Address & Phone Grid by Sudip On 22-12-2016--%>
    <script type="text/javascript">
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
                jAlert("Successfully Update!..");
                gridAddress.PerformCallback();
            }
            else if (obj == 'Y1') {
                var msg = 'Cannot proceed. Correspondence Address is already exist as "Active".\n You can set only one Correspondence Address as "Active". ';
                jAlert(msg);

            }
            else if (obj == 'Y2') {
                var msg = 'Cannot proceed. Residence Address is already exist as "Active".\n You can set only one Residence Address as "Active".';
                jAlert(msg);
            }
            else if (obj == 'Y3') {
                var msg = 'Cannot proceed. Office Address is already exist as "Active".\n You can set only one Office Address as "Active".';
                jAlert(msg);
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
            else if (obj == 'Y1') {
                var msg = 'Cannot proceed. Residence Phone Type is already exist as "Active".\n You can set only one Residence Phone Type as "Active". ';
                jAlert(msg);

            }
            else if (obj == 'Y2') {
                var msg = 'Cannot proceed. Office Phone Type is already exist as "Active".\n You can set only one Office Phone Type as "Active".';
                jAlert(msg);
            }
            else if (obj == 'Y3') {
                var msg = 'Cannot proceed. Correspondenc Phone Type is already exist as "Active".\n You can set only one Correspondenc Phone Type as "Active".';
                jAlert(msg);
            }
            else if (obj == 'Y4') {
                var msg = 'Cannot proceed. Mobile Phone Type is already exist as "Active".\n You can set only one Mobile Phone Type as "Active".';
                jAlert(msg);
            }
            else if (obj == 'Y5') {
                var msg = 'Cannot proceed. Fax Phone Type is already exist as "Active".\n You can set only one Fax Phone Type as "Active".';
                jAlert(msg);
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
            else if (obj == 'Y1') {
                var msg = 'Cannot proceed. Personal Email Type is already exist as "Active".\n You can set only one Residence Email Type as "Active". ';
                jAlert(msg);

            }
            else if (obj == 'Y2') {
                var msg = 'Cannot proceed. Official Email Type is already exist as "Active".\n You can set only one Office Email Type as "Active".';
                jAlert(msg);
            }
            else if (obj == 'Y3') {
                var msg = 'Cannot proceed. Web Site Email Type is already exist as "Active".\n You can set only one Web Site Email Type as "Active".';
                jAlert(msg);
            }
        }
        function btnCancel_ClickE() {
            popupE.Hide();
        }
    </script>
    <%--End--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Vendors/Service Providers</h3>
            <div class="crossBtn"><a href="HRrecruitmentagent.aspx"><i class="fa fa-times"></i></a></div>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Size="15px" ForeColor="Navy"
                        Width="819px" Height="18px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="2" ClientInstanceName="page" Width="100%">
                        <TabPages>
                            <dxe:TabPage Text="General" Name="General">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="Contact Person" Text="Contact Person">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <dxe:TabPage Text="Correspondence" Name="CorresPondence">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                        <dxe:ASPxPageControl ID="ASPxPageControl2" runat="server" ActiveTabIndex="1" ClientInstanceName="page" OnActiveTabChanged="ASPxPageControl2_ActiveTabChanged">
                                            <TabPages>
                                                <dxe:TabPage Text="Address">
                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">
                                                            <div class="pull-left">
                                                                 <% if (rights.CanAdd)
                                                                           { %>
                                                                <a href="javascript:void(0);" onclick="gridAddress.AddNewRow();" class="btn btn-primary"><span>Add New</span> </a>
                                                                <% } %>
                                                            </div>
                                                            <dxe:ASPxGridView ID="AddressGrid" runat="server" DataSourceID="Address" ClientInstanceName="gridAddress"
                                                                KeyFieldName="Id" AutoGenerateColumns="False" OnCellEditorInitialize="AddressGrid_CellEditorInitialize"
                                                                Width="100%" Font-Size="12px" OnRowValidating="AddressGrid_RowValidating" OnCustomCallback="AddressGrid_CustomCallback"
                                                                OnCommandButtonInitialize="AddressGrid_CommandButtonInitialize"  EnableRowsCache="False">
                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn Caption="Id" FieldName="Id" Visible="False" VisibleIndex="0">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataCheckColumn FieldName="Isdefault" Visible="False" VisibleIndex="1" Caption="Default">
                                                                            <EditFormSettings Visible="True" />
                                                                            <PropertiesCheckEdit>                                                                              
                                                                            </PropertiesCheckEdit>
                                                                    </dxe:GridViewDataCheckColumn>
                                                                  
                                                                    
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Address Type" FieldName="Type" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox EnableIncrementalFiltering="True" ValueType="System.String">
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Billing" Value="Billing"></dxe:ListEditItem>
                                                                                  <dxe:ListEditItem Text="Shipping" Value="Shipping"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Residence" Value="Residence"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Office" Value="Office"></dxe:ListEditItem>
                                                                                <dxe:ListEditItem Text="Correspondence" Value="Correspondence"></dxe:ListEditItem>
                                                                            </Items>
                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { onChangeTypeVendor(s); }"></ClientSideEvents> 
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField ErrorText="Mandatory" IsRequired="True" />
                                                                            </ValidationSettings>
                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="1" />
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Type" FieldName="Type" VisibleIndex="0">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <%--Rev   0018570  04-01-2019--%>
                                                                    <%--<dxe:GridViewDataTextColumn FieldName="contactperson" VisibleIndex="2" Caption="Contact Person">--%>
                                                                    <dxe:GridViewDataTextColumn FieldName="contactperson" VisibleIndex="1" Caption="Contact Person">
                                                                        <EditFormSettings Visible="true" VisibleIndex="2" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Address1" FieldName="Address1" VisibleIndex="2">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="500"></PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="2" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Address2" FieldName="Address2" VisibleIndex="3">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="500"></PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="3" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Address3" FieldName="Address3" VisibleIndex="4">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="500"></PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="4" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Landmark" FieldName="LandMark" VisibleIndex="5">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="500"></PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="5" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Country" FieldName="Country" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox DataSourceID="CountrySelect" EnableIncrementalFiltering="True"
                                                                            EnableSynchronization="False" TextField="Country" ValueField="cou_id" ValueType="System.String">


                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">

                                                                                <RequiredField IsRequired="True" />

                                                                            </ValidationSettings>

                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCountryChanged(s); }" />


                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="6" />
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="State" FieldName="State" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox DataSourceID="StateSelect" EnableIncrementalFiltering="True"
                                                                            EnableSynchronization="False" TextField="State" ValueField="ID" ValueType="System.String">


                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">

                                                                                <RequiredField IsRequired="True" />

                                                                            </ValidationSettings>

                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnStateChanged(s); }" />


                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="7" />
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Country" FieldName="Country1" VisibleIndex="6">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="State" FieldName="State1" VisibleIndex="7">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="Area" FieldName="area1" VisibleIndex="8">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn Caption="District" FieldName="City1" VisibleIndex="9">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="PinCode1" VisibleIndex="10" Caption="Pin / Zip">
                                                                        <EditFormSettings Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="Distance" VisibleIndex="11" Caption="Distance(Km)">
                                                                            <EditFormSettings Visible="true" VisibleIndex="21" />
                                                                              <PropertiesTextEdit>
                                                                                  
                                                                               <ClientSideEvents Init="init_distancecorrespondence" />      
                                                                             <MaskSettings Mask="&lt;0..999999999&gt;.&lt;00..99&gt;"/>
                                                                                  </PropertiesTextEdit>
                                                                            <CellStyle CssClass="gridcellleft">
                                                                            </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="District" FieldName="City" Visible="False"
                                                                        VisibleIndex="7">
                                                                        <PropertiesComboBox DataSourceID="SelectCity" EnableIncrementalFiltering="True" EnableSynchronization="False"
                                                                            TextField="City" ValueField="CityId" ValueType="System.String">


                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">

                                                                                <RequiredField IsRequired="True" />

                                                                            </ValidationSettings>

                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnCityChanged(s); }" />


                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="8" />
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Area" FieldName="area" VisibleIndex="7" Visible="false">
                                                                        <PropertiesComboBox DataSourceID="SelectArea" EnableIncrementalFiltering="True" TextField="area_name"
                                                                            ValueField="area_id" ValueType="System.Int32">
                                                                        </PropertiesComboBox>
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="9" />
                                                                    </dxe:GridViewDataComboBoxColumn>

                                                                    <dxe:GridViewDataHyperLinkColumn Visible="False" VisibleIndex="10">
                                                                        <EditItemTemplate>
                                                                            <a href="#" onclick="javascript:openAreaPage();"><span class="Ecoheadtxt" style="color: Blue">
                                                                                <strong>Add New Area</strong></span></a>
                                                                        </EditItemTemplate>
                                                                        <EditFormSettings Visible="True" VisibleIndex="10" />
                                                                    </dxe:GridViewDataHyperLinkColumn>

                                                                    <%--  Debjyoti 05-12-2016 --%>
                                                                    <%-- <dxe:GridViewDataTextColumn Caption="Pincode" FieldName="PinCode" VisibleIndex="9">
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="6">                                                                            
                                                                               <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom" SetFocusOnError="True">
                                                                                    <RequiredField ErrorText="Mandatory." IsRequired="True" />
                                                                                    <RegularExpression ErrorText="Enter Valid Pincode." ValidationExpression="[0-9]{6}" />
                                                                                </ValidationSettings>
                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <EditFormSettings Visible="True" VisibleIndex="11" />
                                                                    </dxe:GridViewDataTextColumn>--%>


                                                                    <dxe:GridViewDataComboBoxColumn Caption="Pin / Zip" FieldName="PinCode" Visible="False" VisibleIndex="9">
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

                                                                    <dxe:GridViewDataTextColumn VisibleIndex="12" Caption="Change Status">
                                                                        <DataItemTemplate>
                                                                            <a href="javascript:void(0);" onclick="OnAddEditClick(this,'Edit~'+'<%# Container.KeyValue %>')">
                                                                                <dxe:ASPxLabel ID="ASPxTextBox2" runat="server" Text='<%# Eval("status")%>' Width="100%" ToolTip="Click to Change Status">
                                                                                </dxe:ASPxLabel>
                                                                            </a>
                                                                        </DataItemTemplate>
                                                                        <EditFormSettings Visible="false" VisibleIndex="4" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>

                                                                    <%-- End Debjyoti 05-12-2016 --%>
                                                                    <dxe:GridViewCommandColumn VisibleIndex="13" ShowDeleteButton="true" ShowNewButton="true" HeaderStyle-HorizontalAlign="Center" ShowEditButton="True">
                                                                        <%--<DeleteButton Visible="True">
                                                                        </DeleteButton>--%>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <HeaderTemplate>
                                                                            <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                              { %>
                                                                            <a href="javascript:void(0);" onclick="gridAddress.AddNewRow();"><span>Add New</span> </a>
                                                                            <%} %>--%>
                                                                            Actions
                                                                        </HeaderTemplate>
                                                                        <%--<EditButton Visible="True">
                                                                        </EditButton>--%>
                                                                    </dxe:GridViewCommandColumn>
                                                                </Columns>
                                                                <SettingsCommandButton>



                                                                    <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
                                                                        <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>

<Styles>
<Style CssClass="pad"></Style>
</Styles>
                                                                    </EditButton>
                                                                    <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Styles-Style-CssClass="pad">
                                                                        <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>

<Styles>
<Style CssClass="pad"></Style>
</Styles>
                                                                    </DeleteButton>
                                                                    <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
                                                                        <Styles>
                                                                            <Style CssClass="btn btn-primary"></Style>
                                                                        </Styles>
                                                                    </UpdateButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                                                                        <Styles>
                                                                            <Style CssClass="btn btn-danger"></Style>
                                                                        </Styles>
                                                                    </CancelButton>
                                                                </SettingsCommandButton>
                                                                     
                                                                <Settings ShowFooter="True" ShowStatusBar="Visible" ShowTitlePanel="True" ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true"/>
                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="WindowCenter"
                                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="700px"
                                                                    EditFormColumnCount="1" />
                                                                <Styles>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                </Styles>
                                                                <SettingsText PopupEditFormCaption="Add/Modify Address" ConfirmDelete="Confirm delete?" />
                                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                                                    <FirstPageButton Visible="True"></FirstPageButton>

                                                                    <LastPageButton Visible="True"></LastPageButton>
                                                                </SettingsPager>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />



                                                                <StylesEditors>
                                                                    <ProgressBar Height="25px">
                                                                    </ProgressBar>
                                                                </StylesEditors>
                                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                                                    <FirstPageButton Visible="True">
                                                                    </FirstPageButton>
                                                                    <LastPageButton Visible="True">
                                                                    </LastPageButton>
                                                                </SettingsPager>

                                                                <Templates>
                                                                    <EditForm>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 25%"></td>
                                                                                <td style="width: 50%">
                                                                                    <controls>
                                                               <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors1">
                                                               </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                             </controls>
                                                                                    <div style="padding: 2px 2px 2px 94px">
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton1" ReplacementType="EditFormUpdateButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton1" ReplacementType="EditFormCancelButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 25%"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditForm>
                                                                    <TitlePanel>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <span class="Ecoheadtxt">
                                                                                        <bold>Add/Modify Address</bold>
                                                                                    </span>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </TitlePanel>
                                                                </Templates>
                                                            </dxe:ASPxGridView>
                                                             <dxe:ASPxPopupControl ID="ASPxPopupControl2" ClientInstanceName="popuparea" runat="server"
                                                                AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Set Address Status"
                                                                EnableHotTrack="False" BackColor="#DDECFE" Width="400px" CloseAction="CloseButton">
                                                                <ContentCollection>
                                                                    <dxe:PopupControlContentControl runat="server">
                                                                        </dxe:PopupControlContentControl></ContentCollection></dxe:ASPxPopupControl>
                                                            <dxe:ASPxPopupControl ID="ASPxPopupControl1" ClientInstanceName="popup" runat="server"
                                                                AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Set Address Status"
                                                                EnableHotTrack="False" BackColor="#DDECFE" Width="400px" CloseAction="CloseButton">
                                                                <ContentCollection>
                                                                    <dxe:PopupControlContentControl runat="server">
                                                                        <dxe:ASPxCallbackPanel ID="ASPxCallbackPanel1" runat="server" Width="400px" ClientInstanceName="popPanel"
                                                                            OnCallback="ASPxCallbackPanel1_Callback" OnCustomJSProperties="ASPxCallbackPanel1_CustomJSProperties">
                                                                            <PanelCollection>
                                                                                <dxe:PanelContent runat="server">

                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>Status:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="cmbStatus" runat="server" Width="100px">
                                                                                                    <asp:ListItem Text="Active" Value="Y"></asp:ListItem>
                                                                                                    <asp:ListItem Text="Deactive" Value="N"></asp:ListItem>
                                                                                                </asp:DropDownList>

                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>Date:
                                                                                            </td>
                                                                                            <td>
                                                                                                <dxe:ASPxDateEdit ID="StDate" runat="server" ClientInstanceName="StDate" EditFormat="Custom"
                                                                                                    UseMaskBehavior="True" Width="99px" Font-Size="12px" TabIndex="21">
                                                                                                    <ButtonStyle Width="13px">
                                                                                                    </ButtonStyle>
                                                                                                </dxe:ASPxDateEdit>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>Reason:
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Width="250px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td></td>
                                                                                            <td colspan="2" class="gridcellleft">
                                                                                                <input id="Button1" type="button" value="Save" class="btnUpdate" onclick="btnSave_Click()"
                                                                                                    style="width: 60px" tabindex="41" />
                                                                                                <input id="Button2" type="button" value="Cancel" class="btnUpdate" onclick="btnCancel_Click()"
                                                                                                    style="width: 60px" tabindex="42" />
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
                                                <dxe:TabPage Text="Phone">
                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">
                                                            <div class="pull-left">
                                                                 <% if (rights.CanAdd)
                                                                           { %>
                                                                <a href="javascript:void(0);" onclick="gridPhone.AddNewRow();" class="btn btn-primary"><span>Add New</span> </a>
                                                                <% } %>
                                                            </div>
                                                            <dxe:ASPxGridView ID="PhoneGrid" ClientInstanceName="gridPhone" DataSourceID="Phone"
                                                                KeyFieldName="phf_id" runat="server" AutoGenerateColumns="False" Width="100%"
                                                                Font-Size="12px" OnRowValidating="PhoneGrid_RowValidating" OnCustomCallback="PhoneGrid_CustomCallback"
                                                                OnCommandButtonInitialize="PhoneGrid_CommandButtonInitialize">
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
                                                                     <dxe:GridViewDataCheckColumn FieldName="Isdefault" Visible="False"  Caption="Default">
                                                                            <EditFormSettings Visible="True" />
                                                                            <PropertiesCheckEdit>
                                                                              <%--  <ClientSideEvents CheckedChanged="function(s,e){fn_chekFbtRate(s,e);}"/>--%>
                                                                            </PropertiesCheckEdit>
                                                                        </dxe:GridViewDataCheckColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Phone Type" FieldName="phf_type" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox ValueType="System.String">


                                                                            <%--<ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Mobile&quot;)
    {
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
    }
    else
    {
                  gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
    }
   
}" />--%>
                                                                            <%--<ClientSideEvents Init="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Mobile&quot;)
    {
         gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(false);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(false);
    }
    else
    {
                  gridPhone.GetEditor(&quot;phf_countryCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_areaCode&quot;).SetVisible(true);
         gridPhone.GetEditor(&quot;phf_extension&quot;).SetVisible(true);
    }
}" />--%>
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

                                                                            <Items>


                                                                                <dxe:ListEditItem Text="Residence" Value="Residence"></dxe:ListEditItem>


                                                                                <dxe:ListEditItem Text="Office" Value="Office"></dxe:ListEditItem>


                                                                                <dxe:ListEditItem Text="Correspondence" Value="Correspondence"></dxe:ListEditItem>


                                                                                <dxe:ListEditItem Text="Mobile" Value="Mobile"></dxe:ListEditItem>


                                                                                <dxe:ListEditItem Text="Fax" Value="Fax"></dxe:ListEditItem>


                                                                            </Items>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField ErrorText="Mandatory" IsRequired="True" />
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


                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">


                                                                                <RegularExpression ErrorText="Enter Valid CountryCode" ValidationExpression="[0-9]+" />


                                                                            </ValidationSettings>


                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_areaCode" VisibleIndex="1" Visible="False">
                                                                        <EditFormSettings Caption="Area Code" Visible="True" />
                                                                        <PropertiesTextEdit MaxLength="5">


                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">


                                                                                <RegularExpression ErrorText="Enter Valid AreaCode" ValidationExpression="[0-9]+" />


                                                                            </ValidationSettings>


                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_phoneNumber" VisibleIndex="1" Caption="Number"
                                                                        Visible="False">
                                                                        <EditFormSettings Visible="True" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="12">


                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">


                                                                                <RegularExpression ErrorText="Enter Valid PhoneNumber" ValidationExpression="[0-9]+" />


                                                                            </ValidationSettings>


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
                                                                    <dxe:GridViewDataTextColumn FieldName="phf_extension" VisibleIndex="2" Caption="Extension"
                                                                        Visible="False">
                                                                        <EditFormSettings Visible="True" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <PropertiesTextEdit MaxLength="50">


                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">


                                                                                <RegularExpression ErrorText="Enter Valid Extension" ValidationExpression="[0-9]+" />


                                                                            </ValidationSettings>


                                                                        </PropertiesTextEdit>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>

                                                                    <dxe:GridViewDataTextColumn FieldName="status" VisibleIndex="2" Caption="Status">
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
                                                                    <dxe:GridViewCommandColumn VisibleIndex="4" ShowDeleteButton="true"  ShowUpdateButton="true"  HeaderStyle-HorizontalAlign="Center" ShowEditButton="True">
                                                                        <%--<DeleteButton Visible="True">
                                                                        </DeleteButton>
                                                                        <EditButton Visible="True">
                                                                        </EditButton>--%>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <HeaderTemplate>
                                                                            <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                              { %>
                                                                            <a href="javascript:void(0);" onclick="gridPhone.AddNewRow();"><span>Add New</span> </a>
                                                                            <%} %>--%>
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
                                                                    <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Styles-Style-CssClass="pad">
                                                                        <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>

<Styles>
<Style CssClass="pad"></Style>
</Styles>
                                                                    </DeleteButton>
                                                                    <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
                                                                        <Styles>
                                                                            <Style CssClass="btn btn-primary"></Style>
                                                                        </Styles>
                                                                    </UpdateButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                                                                        <Styles>
                                                                            <Style CssClass="btn btn-danger"></Style>
                                                                        </Styles>
                                                                    </CancelButton>
                                                                </SettingsCommandButton>
                                                                   
                                                                <Settings ShowFooter="True" ShowStatusBar="Visible" ShowTitlePanel="True" ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true"/>
                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center"
                                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="700px"
                                                                    EditFormColumnCount="1" />
                                                                <Styles>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                </Styles>
                                                                <SettingsText PopupEditFormCaption="Add/Modify Address" ConfirmDelete="Confirm delete?" />
                                                                <SettingsPager NumericButtonCount="20" PageSize="20">
                                                                </SettingsPager>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                                                                <Templates>
                                                                    <EditForm>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 25%"></td>
                                                                                <td style="width: 50%">
                                                                                    <controls>
                                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors2">
                                                </dxe:ASPxGridViewTemplateReplacement>                                                           
                                              </controls>
                                                                                    <div style="padding: 2px 2px 2px 94px">
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton2" ReplacementType="EditFormUpdateButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton2" ReplacementType="EditFormCancelButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 25%"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditForm>
                                                                    <TitlePanel>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <span class="Ecoheadtxt">Add/Modify Phone.</span>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </TitlePanel>
                                                                </Templates>
                                                            </dxe:ASPxGridView>

                                                            <dxe:ASPxPopupControl ID="ASPxPopupControlP" ClientInstanceName="popupP" runat="server"
                                                                AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Set Phone Status"
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
                                                <dxe:TabPage Text="Email">
                                                    <ContentCollection>
                                                        <dxe:ContentControl runat="server">
                                                            <div class="pull-left">
                                                                  <% if (rights.CanAdd)
                                                                           { %>
                                                                <a href="javascript:void(0);" onclick="gridEmail.AddNewRow();" class="btn btn-primary"><span>Add New</span> </a>
                                                                <% } %>
                                                            </div>
                                                            <dxe:ASPxGridView ID="EmailGrid" runat="server" ClientInstanceName="gridEmail"
                                                                DataSourceID="Email" KeyFieldName="eml_id" AutoGenerateColumns="False" Width="100%"
                                                                Font-Size="12px" OnRowValidating="EmailGrid_RowValidating1" OnCustomCallback="EmailGrid_CustomCallback"
                                                                OnCommandButtonInitialize="EmailGrid_CommandButtonInitialize">
                                                                <Columns>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_id" VisibleIndex="1" Visible="False">
                                                                        <EditFormSettings Visible="False" />
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataComboBoxColumn Caption="Email Type" FieldName="eml_type" Visible="False"
                                                                        VisibleIndex="0">
                                                                        <PropertiesComboBox ValueType="System.String">


                                                                            <ClientSideEvents SelectedIndexChanged="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Web Site&quot;)
    {
         gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(false);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(false);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(true);
    }
    else
    {
         gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(true);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(true);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(false);
    }
}" />
                                                                            <ClientSideEvents Init="function(s, e) {
	var value = s.GetValue();
    if(value == &quot;Web Site&quot;)
    {
         gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(false);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(false);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(true);
    }
    else
    {
         gridEmail.GetEditor(&quot;eml_email&quot;).SetVisible(true);
         gridEmail.GetEditor(&quot;eml_ccEmail&quot;).SetVisible(true);
         gridEmail.GetEditor(&quot;eml_website&quot;).SetVisible(false);
    }
}" />


                                                                            <Items>


                                                                                <dxe:ListEditItem Text="Personal" Value="Personal"></dxe:ListEditItem>


                                                                                <dxe:ListEditItem Text="Official" Value="Official"></dxe:ListEditItem>


                                                                                <dxe:ListEditItem Text="Web Site" Value="Web Site"></dxe:ListEditItem>


                                                                            </Items>
                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                                                                <RequiredField ErrorText="Mandatory" IsRequired="True" />
                                                                            </ValidationSettings>

                                                                        </PropertiesComboBox>
                                                                        <EditFormSettings Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataComboBoxColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_type" VisibleIndex="0" Caption="Type"
                                                                        Width="27%">
                                                                        <EditFormSettings Caption="Email Type" Visible="False" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_email" VisibleIndex="1" Caption="Email">
                                                                        <EditFormSettings Caption="EmailId" Visible="True" />
                                                                        <CellStyle CssClass="gridcellleft">
                                                                        </CellStyle>
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <PropertiesTextEdit>


                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">


                                                                                <RegularExpression ErrorText="Enetr Valid E-Mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />


                                                                            </ValidationSettings>


                                                                        </PropertiesTextEdit>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_ccEmail" VisibleIndex="1" Visible="False">
                                                                        <EditFormSettings Caption="CC Email" Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                        <PropertiesTextEdit>


                                                                            <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="right" SetFocusOnError="True">


                                                                                <RegularExpression ErrorText="Enetr Valid CC EMail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />


                                                                            </ValidationSettings>


                                                                        </PropertiesTextEdit>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="eml_website" Caption="WebURL" VisibleIndex="1"
                                                                        Visible="true">
                                                                        <EditFormSettings Caption="WebURL" Visible="True" />
                                                                        <EditFormCaptionStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="False">
                                                                        </EditFormCaptionStyle>
                                                                    </dxe:GridViewDataTextColumn>
                                                                    <dxe:GridViewDataTextColumn FieldName="status" VisibleIndex="2" Caption="Status">
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

                                                                    <dxe:GridViewCommandColumn VisibleIndex="3" ShowDeleteButton="true" ShowEditButton="true" HeaderStyle-HorizontalAlign="Center">
                                                                        <%--<DeleteButton Visible="True">
                                                                        </DeleteButton>
                                                                        <EditButton Visible="True">
                                                                        </EditButton>--%>
                                                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                                        <HeaderTemplate>
                                                                            <%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                                                              { %>
                                                                            <a href="javascript:void(0);" onclick="gridEmail.AddNewRow();"><span>Add New</span> </a>
                                                                            <%} %>--%>
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
                                                                    <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Styles-Style-CssClass="pad">
                                                                        <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>

<Styles>
<Style CssClass="pad"></Style>
</Styles>
                                                                    </DeleteButton>
                                                                    <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
                                                                        <Styles>
                                                                            <Style CssClass="btn btn-primary"></Style>
                                                                        </Styles>
                                                                    </UpdateButton>
                                                                    <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                                                                        <Styles>
                                                                            <Style CssClass="btn btn-danger"></Style>
                                                                        </Styles>
                                                                    </CancelButton>
                                                                </SettingsCommandButton>
                                                                <Settings ShowFooter="false" ShowStatusBar="Visible" ShowTitlePanel="True" ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true"/>
                                                                <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center"
                                                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="700px"
                                                                    EditFormColumnCount="1" />
                                                                <Styles>
                                                                    <LoadingPanel ImageSpacing="10px">
                                                                    </LoadingPanel>
                                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                                    </Header>
                                                                </Styles>
                                                                <SettingsText PopupEditFormCaption="Add/Modify Address" ConfirmDelete="Confirm delete?" />
                                                                <SettingsPager NumericButtonCount="20" PageSize="20">
                                                                    <FirstPageButton Visible="True"></FirstPageButton>

                                                                    <LastPageButton Visible="True"></LastPageButton>
                                                                </SettingsPager>
                                                                <StylesEditors>
                                                                    <ProgressBar Height="25px">
                                                                    </ProgressBar>
                                                                </StylesEditors>
                                                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                                                    <FirstPageButton Visible="True">
                                                                    </FirstPageButton>
                                                                    <LastPageButton Visible="True">
                                                                    </LastPageButton>
                                                                </SettingsPager>
                                                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />



                                                                <Templates>
                                                                    <EditForm>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 25%"></td>
                                                                                <td style="width: 50%">
                                                                                    <controls>
                                                     <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors3">
                                                     </dxe:ASPxGridViewTemplateReplacement>                                                           
                                                 </controls>
                                                                                    <div style="text-align: left; padding: 2px 2px 2px 81px">
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton3" ReplacementType="EditFormUpdateButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton3" ReplacementType="EditFormCancelButton"
                                                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 25%"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditForm>
                                                                    <TitlePanel>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <span class="Ecoheadtxt">Add/Modify Email.</span>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </TitlePanel>
                                                                </Templates>
                                                            </dxe:ASPxGridView>
                                                            <dxe:ASPxPopupControl ID="ASPxPopupControlE" ClientInstanceName="popupE" runat="server"
                                                                AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Set Email Status"
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
                            <dxe:TabPage Name="BankDetails" Text="Bank Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                            <%-- <dxe:TabPage Name="DPDetails" Text="DP Details">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>--%>
                            <dxe:TabPage Name="Documents" Text="Documents">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>
                              <dxe:TabPage Name="Registration" Text="Registration">
                                <contentcollection>
                                                            <dxe:ContentControl runat="server">
                                                            </dxe:ContentControl>
                                                        </contentcollection>
                            </dxe:TabPage>
                            <dxe:TabPage Name="GroupMember" Text="Group Member">
                                <ContentCollection>
                                    <dxe:ContentControl runat="server">
                                    </dxe:ContentControl>
                                </ContentCollection>
                            </dxe:TabPage>

                            <dxe:TabPage Name="TDS" Text="TDS">
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
	                                            }" />
                        <ContentStyle>
                            <Border BorderColor="#002D96" BorderStyle="Solid" BorderWidth="1px" />
                        </ContentStyle>
                        <LoadingPanelStyle ImageSpacing="6px">
                        </LoadingPanelStyle>
                        <TabStyle Font-Size="12px">
                        </TabStyle>
                    </dxe:ASPxPageControl>
                    <asp:TextBox ID="txtID" runat="server" Visible="false"></asp:TextBox></td>
                <td></td>
            </tr>
        </table>
    </div>
    <asp:SqlDataSource ID="Address" runat="server" 
        SelectCommand="select DISTINCT  tbl_master_address.add_id AS Id, tbl_master_address.add_addressType AS Type,
                        tbl_master_address.add_address1 AS Address1,  tbl_master_address.add_address2 AS Address2, 
                        tbl_master_address.add_address3 AS Address3,tbl_master_address.add_landMark AS LandMark, 
                        tbl_master_address.add_country AS Country, 
                        tbl_master_address.add_state AS State,tbl_master_address.add_city AS City,tbl_master_address.add_pin as PinCode,
                        CASE add_country WHEN '' THEN '0' ELSE(SELECT cou_country FROM tbl_master_country WHERE cou_id = add_country) END AS Country1, 
                        CASE add_state WHEN '' THEN '0' ELSE(SELECT state FROM tbl_master_state WHERE id = add_state) END AS State1,
                        CASE add_city WHEN '' THEN '0' ELSE(SELECT city_name FROM tbl_master_city WHERE city_id = add_city) END AS City1,
                        CASE add_area WHEN '' THEN '0' Else(select area_name From tbl_master_area Where area_id = add_area) End AS area1,
                        CASE add_area WHEN '' THEN '0' Else(select area_id From tbl_master_area Where area_id = add_area) End AS area,                      
                        CASE add_pin WHEN '' THEN '' ELSE(SELECT pin_code FROM tbl_master_pinzip WHERE pin_id = add_pin) END AS PinCode1, tbl_master_address.add_landMark AS LankMark  ,
                        case when IsNull(add_status,'')='N' then 'Deactive' else 'Active' end as status,tbl_master_address.Isdefault as Isdefault, tbl_master_address.contactperson as contactperson,Convert(decimal(18,2),Distance) Distance             
                        from tbl_master_address where add_cntId=@insuId"
        DeleteCommand="contactDelete"
        DeleteCommandType="StoredProcedure" InsertCommand="insert_correspondence" UpdateCommand="update tbl_master_address set Isdefault=@Isdefault,contactperson=@contactperson, add_addressType=@Type,add_address1=@Address1,add_address2=@Address2,add_address3=@Address3,add_city=@City,add_landMark=@LandMark,add_country=@Country,add_state=@State,add_area=@area,add_pin=@PinCode,LastModifyDate=getdate(),LastModifyUser=@CreateUser,Distance=@Distance where add_id=@Id"
        InsertCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter Name="insuId" SessionField="KeyVal_InternalID" Type="String" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="decimal" />
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
             <asp:Parameter Name="contactperson" Type="string" />
             <asp:Parameter Name="Isdefault" Type="int32" />
            <asp:Parameter Name="Distance" Type="decimal" />
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
            <asp:Parameter Name="contactperson" Type="string" />
                <asp:Parameter Name="Isdefault" Type="int32" />
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            <asp:Parameter Name="Distance" Type="decimal" />
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
        SelectCommand="SELECT area_id, area_name from tbl_master_area where (city_id = @Area) ORDER BY area_name">
        <SelectParameters>
            <asp:Parameter Name="Area" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    &nbsp;
               <%--debjyoti 02-12-2016--%>
    <asp:SqlDataSource ID="SelectPin" runat="server" 
        SelectCommand="select pin_id,pin_code from tbl_master_pinzip where city_id=@City">
        <SelectParameters>
            <asp:Parameter Name="City" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <%--End Debjyoti 02-12-2016--%>


    <asp:SqlDataSource ID="Phone" runat="server" 
        DeleteCommand="delete from tbl_master_phonefax where phf_id=@phf_id" InsertCommand="insert_correspondence_phone"
        InsertCommandType="StoredProcedure" SelectCommand="select DISTINCT Isdefault,phf_id,phf_cntId,phf_entity,phf_type,phf_countryCode,phf_areaCode,phf_phoneNumber,phf_extension, ISNULL(phf_countryCode, '') + ' ' + ISNULL(phf_areaCode, '') + ' ' + ISNULL(phf_phoneNumber, '') + ISNULL(phf_faxNumber, '') AS Number, case when IsNull(phf_Status,'')='N' then 'Deactive' else 'Active' end as status
                      from tbl_master_phonefax where phf_cntId=@PhfId"
        UpdateCommand="update tbl_master_phonefax set Isdefault=@Isdefault,phf_type=@phf_type,phf_countryCode=@phf_countryCode,phf_areaCode=@phf_areaCode,phf_phoneNumber=@phf_phoneNumber,
                       phf_extension=@phf_extension,LastModifyDate=getdate(),LastModifyUser=@CreateUser where phf_id=@phf_id">
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
             <asp:Parameter Name="Isdefault" Type="int32" />
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
            <asp:Parameter Name="Isdefault" Type="int32" />
            
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
            <asp:Parameter Name="phf_id" Type="decimal" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="phf_id" Type="decimal" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="Email" runat="server"
        DeleteCommand="delete from tbl_master_email where eml_id=@eml_id" InsertCommand="insert_correspondence_email"
        InsertCommandType="StoredProcedure" SelectCommand="select eml_id,eml_cntId,eml_entity,eml_type,eml_email,eml_ccEmail,eml_website,CreateDate,CreateUser,case when eml_Status='N' then 'Deactive' else 'Active' end as status from tbl_master_email where eml_cntId=@EmlId"
        UpdateCommand="update tbl_master_email set eml_type=@eml_type,eml_email=@eml_email,eml_ccEmail=@eml_ccEmail,eml_website=@eml_website,LastModifyDate=getdate(),LastModifyUser=@CreateUser where eml_id=@eml_id">
        <DeleteParameters>
            <asp:Parameter Name="eml_id" Type="decimal" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="eml_type" Type="string" />
            <asp:Parameter Name="eml_email" Type="string" />
            <asp:Parameter Name="eml_ccEmail" Type="string" />
            <asp:Parameter Name="eml_website" Type="string" />
            <asp:Parameter Name="eml_id" Type="decimal" />
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
            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="Decimal" />
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>
