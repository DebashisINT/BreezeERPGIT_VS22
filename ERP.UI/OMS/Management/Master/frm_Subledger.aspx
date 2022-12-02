<%@ Page Title="Subledger" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_frm_Subledger" CodeBehind="frm_Subledger.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="/assests/pluggins/choosen/choosen.min.js"></script>
     <script language="javascript" type="text/javascript">

        function ShowHideFilter(obj) {

            grid.PerformCallback(obj);
        }
        function ShowHideFilterWC(obj) {

            grid1.PerformCallback(obj);
        }

        function ShowCustom(Keyvalue, MainAcID) {
            // .............................Code Commented and Added by Sam on 05122016.to change the master page ..................................... 

            //var url = "frm_OpeningBalanceSubAc.aspx?id=" + Keyvalue + "&MainAcId=" + MainAcID + "";
            var url = "../frm_OpeningBalanceSubAcWC.aspx?id=" + Keyvalue + "&MainAcId=" + MainAcID + "";
            // .............................Code Above Commented and Added by Sam on 05122016...................................... 

            
            popup.SetContentUrl(url);

            popup.Show();
            //$('iframe').css({ 'background-color': 'red' });

        }
        function ShowWCustomType(Keyvalue, MainAcID) {
            //alert(Keyvalue);
            //alert(MainAcID);
            var url = "../frm_OpeningBalanceSubAcWC.aspx?id=" + Keyvalue + "&MainAcID=" + MainAcID + "";
            //alert(url);
            popup.SetContentUrl(url);

            popup.Show();

        }
        function ShowError(obj) {
            if (obj == "b") {
                alert('Transaction Exists for this Code. Deletion Not Allowed !!');
                return false;
            }
            if (obj == "z") {
                txtAccountNo.SetEnabled(false);
                VisibleForCustom();
            }
            else {
                //alert ('1');
                // txtAccountNo.SetEnabled(false);
                VisibleForCustom();
            }
        }
        function ShowErrorWithoutCustom(obj) {
            VisibleForWithoutCustom();
        }
        function ShowAssetCustom(keyvalue, keycode) {

            var url = "AssetDetail.aspx?kval=" + keyvalue + "&kcode=" + keycode;
            
            window.location.href = url;
            // .............................Code Commented and Added by Sam on 05122016. to convert the dhtmlmodal to Page form ..................................... 

            //        editwin=dhtmlmodal.open("Editbox", "iframe", url, "Add/Modify AssetDetail", "width=900px,height=500px,center=1,resize=1,scrolling=2,top=500", "recal")
            //editwin = dhtmlmodal.open("Editbox", "iframe", url, "Add/Modify AssetDetail of " + keyvalue + "", "width=850px,height=450px,center=1,resize=1,scrolling=2,top=500", "recal")
            //document.getElementById('ctl00_ContentPlaceHolder1_Headermain1_cmbSegment').style.visibility = 'hidden';
            //editwin.onclose = function () {
            //    //alert("sds");
            //    document.getElementById('ctl00_ContentPlaceHolder1_Headermain1_cmbSegment').style.visibility = 'visible';
            //}
            //return false;


            // .............................Code Above Commented and Added by Sam on 05122016...................................... 
            

           
        }
        function showhistory(obj) {

            var URL = 'Contact_Document.aspx?idbldng=' + obj;

            window.location.href = URL;

            //OnMoreInfoClick(URL,"Modify Contact Details",'10px','10px',"Y");
            //editwin = dhtmlmodal.open("Editbox", "iframe", URL, "Document", "width=950px,height=400px,center=0,resize=1,top=-1", "recal");
            //editwin.onclose = function () {
            //    grid.PerformCallback();
            //}
        }
        function Page_Load() {
            val = 'No';
        }
        function Page_Load1() {
            val = 'Yes';
        }
        function VisibleForCustom() {
            if (val == 'No') {
                var obj = document.getElementById('TrDepreciation')
                if (obj != null)
                    obj.style.display = 'none';
            }
            else if (val = 'Yes') {
                var obj1 = document.getElementById('TrTdsAppl')
                if (obj1 != null)
                    obj1.style.display = 'none';
                var obj2 = document.getElementById('TrFbtAppl')
                if (obj2 != null)
                    obj2.style.display = 'none';
                var obj3 = document.getElementById('TrRateOInt')
                if (obj3 != null)
                    obj3.style.display = 'none';
            }
        }
        function VisibleForWithoutCustom() {
            if (val == 'No') {
                var obj = document.getElementById('TrDepreciation1')
                if (obj != null)
                    obj.style.display = 'none';
            }
            else if (val = 'Yes') {
                var obj1 = document.getElementById('TrTdsAppl1')
                if (obj1 != null)
                    obj1.style.display = 'none';
                var obj2 = document.getElementById('TrFbtAppl1')
                if (obj2 != null)
                    obj2.style.display = 'none';
                var obj3 = document.getElementById('TrRateOInt1')
                if (obj3 != null)
                    obj3.style.display = 'none';
            }
        }
        function CallTdsAccount(objid, objfunc, objevant) {
            ajax_showOptions(objid, objfunc, objevant);
        }
        FieldName = 'cmbSegment';

        $(document).ready(function () {
            $('#lstTdsType').chosen();
             

        });
        // FieldName='txtAccountNo';

        function fn_chekFbtRate(s, e) {
            var fbt = cSubAccount_IsFBT12.GetValue()
            if(fbt) 
            {
                ctxtFBTRate.SetEnabled(true);
                //grid1.GetEditor('SubAccount_IsFBT').GetValue();
                //alert(grid1.GetEditor('SubAccount_IsFBT').GetValue());
                //document.getElementById('hdnisfbtrate').value = '1'; 
                //alert(document.getElementById('hdnisfbtrate').value);
            }
            else
            {
                ctxtFBTRate.SetEnabled(false);
                ctxtFBTRate.SetText('');
                //grid1.GetEditor('SubAccount_IsFBT').GetValue();
                //alert(grid1.GetEditor('SubAccount_IsFBT').GetValue());
                //document.getElementById('hdnisfbtrate').value = '0';
                //alert(document.getElementById('hdnisfbtrate').value);
            }
        }

        function fn_chekFbtRatecus(s, e) {
            var fbt = ccsttcscus.GetValue()
            if (fbt) {
                
                ctxtFBTRatecus.SetEnabled(true);
                //grid1.GetEditor('SubAccount_IsFBT').GetValue();
                //alert(grid1.GetEditor('SubAccount_IsFBT').GetValue());
                //document.getElementById('hdnisfbtrate').value = '1'; 
                //alert(document.getElementById('hdnisfbtrate').value);
            }
            else {
                ctxtFBTRatecus.SetEnabled(false);
                ctxtFBTRatecus.SetText('');
                //grid1.GetEditor('SubAccount_IsFBT').GetValue();
                //alert(grid1.GetEditor('SubAccount_IsFBT').GetValue());
                //document.getElementById('hdnisfbtrate').value = '0';
                //alert(document.getElementById('hdnisfbtrate').value);
            }
        }
        
    </script>
     <style type="text/css">
        .auto-style1 {
            height: 28px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>
                <%ShowList();%>
                <%=accountname%> 
            </h3>
        </div>
        <div class="crossBtn"><a href="MainAccountHead.aspx"><i class="fa fa-times"></i></a></div>
    </div>
     
    <div class="form_main">
        <table class="TableMain100">
            <tr id="trCustom" runat="server">
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <%--<a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>--%>
                                            <%if (strname == "Custom")
                                              { %>
                                            <a class="btn btn-primary" href="javascript:void(0);" onclick="grid.AddNewRow()"><span>Add New</span> </a>
                                           
                                <% if (rights.CanExport)
                                               { %>
                                             <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                                             <% } %>
                                            <%} %>
                                        </td>
                                        <td id="Td1">

                                           
                                            <%-- <%if (strname != "Custom")
                                              { %>
                                            <a class="btn btn-primary" href="javascript:ShowHideFilter('All');">All Records</a>
                                             <%} %>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trWithoutCustom" runat="server" style="display:none;">
                <td>
                    <table width="100%">
                        <tr>
                            <td style="text-align: left; vertical-align: top" class="auto-style1">
                                <table>
                                     
                                    <tr>
                                        <td id="Td2">
                                              
                                            <%--<a href="javascript:ShowHideFilterWC('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>--%>
                                            <%if (strname == "Custom")
                                              { %>
                                            <a href="javascript:void(0);" onclick="grid.AddNewRow()"><span style="color: #000099; text-decoration: underline">Add New</span> </a>

                                            <%--<asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>--%>
                                            <%} %>
                                        </td>

                                        <td id="Td3">
                                            <%if (strname != "Custom")
                                              { %>
                                          <%--  <a class="btn btn-primary" href="javascript:ShowHideFilterWC('All');"><span>All Records</span></a>--%>
                                             <%} %>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="auto-style1"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            
            <tr>
                <td style="width: 100%">
                    <dxe:ASPxGridView ID="SubAccountGrid" runat="server" AutoGenerateColumns="False"
                        KeyFieldName="SubAccount_ReferenceID" ClientInstanceName="grid"
                        DataSourceID="SubAccount" Width="100%" OnRowValidating="SubAccountGrid_OnRowValidating"
                        OnHtmlEditFormCreated="SubAccountGrid_HtmlEditFormCreated" OnRowUpdating="SubAccountGrid_OnRowUpdating"
                        OnRowInserting="SubAccountGrid_RowInserting" OnStartRowEditing="SubAccountGrid_OnStartRowEditing"
                        OnCustomCallback="SubAccountGrid_CustomCallback" OnHtmlDataCellPrepared="SubAccountGrid_HtmlDataCellPrepared"
                        OnRowDeleting="SubAccountGrid_RowDeleting" OnCustomJSProperties="SubAccountGrid_CustomJSProperties" OnHtmlRowCreated="SubAccountGrid_HtmlRowCreated">
                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="Sub Account Code" FieldName="SubAccount_Code"
                                VisibleIndex="0">
                                <EditFormSettings Visible="False" VisibleIndex="5" />
                                <CellStyle>
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Name" FieldName="SubAccount_Name" VisibleIndex="1">
                                <PropertiesTextEdit>
                                    <ValidationSettings Display="Dynamic">
                                        <RequiredField ErrorText="Required" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormSettings Visible="False" VisibleIndex="13" />
                                <CellStyle>
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="SubAccount_IsTDS" Visible="False" VisibleIndex="3">
                                <EditFormSettings Visible="False" VisibleIndex="6" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="TDS Rate" FieldName="SubAccount_TDSRate" Visible="False"
                                VisibleIndex="5">
                                <PropertiesTextEdit>
                                    <ValidationSettings Display="Dynamic">
                                        <RequiredField ErrorText="Required" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormSettings Visible="False" VisibleIndex="10" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataCheckColumn FieldName="SubAccount_IsFBT" Visible="False" VisibleIndex="7">
                                <EditFormSettings Visible="False" VisibleIndex="8" />
                            </dxe:GridViewDataCheckColumn>

                             <dxe:GridViewDataTextColumn Caption="Opening DR/CR" VisibleIndex="2" FieldName="Custom">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" id="aaa4" style="color: #000099;display:block;text-align:center" runat="server">Add/Edit
                                    </a>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>

                            <%--.............................Code Commented and Added by Sam on 29112016. ..................................... --%>

                             <dxe:GridViewDataTextColumn Caption="Document" VisibleIndex="2" FieldName="AssetCustom1" Visible="false">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" id="aaa5" style="color: #000099;display:block;text-align:center" runat="server">Document</a>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>

                             <%-- <dxe:GridViewDataTextColumn Caption="Documents" VisibleIndex="8" FieldName="AssetCustom1">
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>--%>
                           
                             <%--// .............................Code Above Commented and Added by Sam on 29112016...................................... --%>

                            <dxe:GridViewCommandColumn Caption="Edit" VisibleIndex="4" ShowDeleteButton="true" ShowEditButton="true">
                                
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    Actions
                                </HeaderTemplate>
                                
                            </dxe:GridViewCommandColumn>

                            <dxe:GridViewDataTextColumn Caption="FBT Rate" FieldName="SubAccount_FBTRate" Visible="False"
                                VisibleIndex="9">
                                <PropertiesTextEdit>
                                    <ValidationSettings Display="Dynamic">
                                        <RequiredField ErrorText="Required" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditFormSettings Visible="False" VisibleIndex="12" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Rate Of Interest (P/a)" FieldName="SubAccount_RateOfInterest"
                                Visible="False" VisibleIndex="15">
                                <EditFormSettings Visible="False" VisibleIndex="15" />
                                <CellStyle>
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="Depreciation" FieldName="SubAccount_Depreciation"
                                Visible="False" VisibleIndex="16">
                                <EditFormSettings Visible="False" VisibleIndex="15" />
                                <CellStyle>
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>

                            

                           







                           <%-- <dxe:GridViewDataTextColumn Caption="Asset Details" VisibleIndex="6" FieldName="AssetCustom">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" id="aaa5" style="color: #000099;display:block;text-align:center" runat="server">Asset
                                            Details </a>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>--%>
                          

                        </Columns>
                         <settingscommandbutton>
                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
                                <Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>
                            </EditButton>
                             <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image"  Image-AlternateText="Delete">
                                  <Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                            </DeleteButton>
                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary" >
                                <Styles>
                                    <Style CssClass="btn btn-primary"></Style>
                                </Styles>
                            </UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                                <Styles>
                                    <Style CssClass="btn btn-danger"></Style>
                                </Styles>
                            </CancelButton>
                        </settingscommandbutton>
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                        </Styles>
                        <ClientSideEvents EndCallback="function(s,e) { ShowError(s.cpInsertError);
                                        }" />
                        <%--<Settings ShowGroupedColumns="True" ShowGroupPanel="True" ShowFilterRow="true" />--%>
                        

                        <SettingsText PopupEditFormCaption="Add/Modify Sub Account" ConfirmDelete="Confirm delete?" />
                        <SettingsPager NumericButtonCount="20" ShowSeparators="True">
                        </SettingsPager>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />

                        <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="false" />
                        <SettingsEditing PopupEditFormHeight="310px" PopupEditFormHorizontalAlign="Center" Mode="PopupEditForm"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px" />

                        <Templates>
                            <EditForm>
                                <table style="width: 100%;; margin-top:20px" border="0" id="main">
                                    <tr>
                                        <td style="text-align: right;">Sub Account Name </td>
                                        <td style="text-align: left;padding-bottom: 8px;">
                                            <dxe:ASPxTextBox ID="txtAccountCode" runat="server" Text='<%#Bind("SubAccount_Name") %>' 
                                                Width="203px" MaxLength="100">
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr id="trAccountName">
                                        <td style="text-align: right;">Short Name </td>
                                        <td style="text-align: left;padding-bottom: 8px;">
                                            <dxe:ASPxTextBox ID="txtAccountNo" ClientInstanceName="txtAccountNo" runat="server" Text='<%#Bind("SubAccount_Code") %>'
                                                Width="203px" MaxLength="20">
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr id="TrTdsAppl">
                                        <td style="text-align: right;">TDS Applicable </td>
                                        <td style="text-align: left;">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td colspan="2">
                                                        <%--// .............................Code Commented and Added by Sam on 09122016. ..................................... --%> 

                                                        <dxe:ASPxComboBox id="cmb_tdstcs" DataSourceID="tdstcs" Width="203px" Value='<%#Bind("SubAccount_TDSRate") %>' ClearButton-DisplayMode="Always" runat="server" TextField="tdsdescription" ValueField="tdscode" ItemStyle-Wrap="True"  >
                                                           
                                                        </dxe:ASPxComboBox>
                                                        <%--<asp:TextBox ID="txtTdsType" runat="server" Width="204px" MaxLength="50" Font-Size="12px" Text='<%#Bind("SubAccount_IsTDS") %>'
                                                            onkeyup="CallTdsAccount(this,'SearchTdsTcsCode',event)"></asp:TextBox>
                                                        <asp:HiddenField ID="txtTdsType_hidden" runat="server" Value='<%#Bind("SubAccount_TDSRate") %>' />--%>

                                                        <%--// .............................Code Above Commented and Added by Sam on 09122016...................................... --%>

                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TrFbtAppl" style="display:none">
                                        <td style="text-align: right;">FBT Applicable </td>
                                        <td style="text-align: left;">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>

                                                        <dxe:ASPxCheckBox ID="SubAccount_IsFBT" runat="server" Width="39px"  ClientInstanceName="ccsttcscus"
                                                            Checked='<%# Container.Grid.IsNewRowEditing ? false : Container.Grid.GetRowValues(Container.VisibleIndex, "SubAccount_IsFBT") %>'
                                                            ValueType="System.Boolean" >
                                                            <ClientSideEvents CheckedChanged="function(s,e){fn_chekFbtRatecus(s,e);}"  /> 
                                                        </dxe:ASPxCheckBox>
                                                    </td>
                                                    <td id="fbtrate">FBT Rate:</td>
                                                    <td id="fbtrate1">
                                                        <dxe:ASPxTextBox ID="txtFBTRate" runat="server" Text='<%#Bind("SubAccount_FBTRate") %>'  ClientInstanceName="ctxtFBTRatecus"
                                                            MaskSettings-Mask="<0..9999g>.<00..99>" ValidationSettings-ErrorDisplayMode="None"
                                                            MaskSettings-IncludeLiterals="DecimalSymbol" Width="107px" MaxLength="8">
                                                        </dxe:ASPxTextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="TrRateOInt">
                                        <td style="text-align: right;">Rate Of Interest (P/a) </td>
                                        <td style="text-align: left;">
                                            <dxe:ASPxTextBox ID="txtRateofIntrest" runat="server" Text='<%#Bind("SubAccount_RateOfInterest") %>'
                                                MaskSettings-Mask="<0..9999g>.<00..99>" ValidationSettings-ErrorDisplayMode="None"
                                                MaskSettings-IncludeLiterals="DecimalSymbol" Width="201px" MaxLength="8">
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr id="TrDepreciation">
                                        <td style="text-align: right;">Depreciation
                                        </td>
                                        <td style="text-align: left;">
                                            <dxe:ASPxTextBox ID="txtDepreciation" runat="server" Text='<%#Bind("SubAccount_Depreciation") %>'
                                                MaskSettings-Mask="<0..9999g>.<00..99>" ValidationSettings-ErrorDisplayMode="None"
                                                MaskSettings-IncludeLiterals="DecimalSymbol" Width="150px" MaxLength="8">
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" >
                                            <controls>
                                                    <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="ASPxGridViewTemplateReplacement1">
                                                     </dxe:ASPxGridViewTemplateReplacement>                                                           
                                              </controls>
                                            <div style="text-align: left; padding: 2px 2px 2px 180px; font-weight: bold;">
                                                <dxe:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement2" ReplacementType="EditFormUpdateButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                <dxe:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement3" ReplacementType="EditFormCancelButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                               <%-- <table>
                                    <tr>
                                        <td>
                                            <controls>
                        <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                         </dxe:ASPxGridViewTemplateReplacement>                                                           
                      </controls>
                                            <div style="text-align: center; padding: 2px 2px 2px 2px">
                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            </div>
                                        </td>
                                    </tr>
                                </table>--%>
                            </EditForm>
                        </Templates>
                    </dxe:ASPxGridView>
                    &nbsp;
                        <dxe:ASPxGridView ID="SubAccountWithoutCustom" runat="server" AutoGenerateColumns="False" OnCellEditorInitialize="SubAccountWithoutCustom_CellEditorInitialize"
                            KeyFieldName="Contact_ID" ClientInstanceName="grid1"
                            DataSourceID="WithoutCustom" Width="100%" OnCustomCallback="SubAccountWithoutCustom_CustomCallback"
                            OnHtmlEditFormCreated="SubAccountWithoutCustom_OnHtmlEditFormCreated" OnRowUpdating="SubAccountWithoutCustom_OnRowUpdating"
                            OnRowValidating="SubAccountWithoutCustom_OnRowValidating" OnStartRowEditing="SubAccountWithoutCustom_OnStartRowEditing"
                            OnRowInserting="SubAccountWithoutCustom_OnRowInserting" OnInitNewRow="SubAccountWithoutCustom_InitNewRow"
                            OnHtmlDataCellPrepared="SubAccountWithoutCustom_HtmlDataCellPrepared" OnCustomJSProperties="SubAccountWithoutCustom_CustomJSProperties" 
                            OnHtmlRowCreated="SubAccountWithoutCustom_HtmlRowCreated">
                            <Columns>
                                <dxe:GridViewDataTextColumn Caption="ID" FieldName="Contact_ID" VisibleIndex="0">
                                    <EditFormSettings Visible="False" VisibleIndex="1" />
                                    <CellStyle>
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Code" FieldName="Contact_Code" VisibleIndex="1">
                                    <EditFormSettings Visible="False" VisibleIndex="2" />
                                    <CellStyle Wrap="True">
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Name" FieldName="Contact_Name" VisibleIndex="2">
                                    <EditFormSettings Visible="False" VisibleIndex="3" />
                                    <CellStyle Wrap="True"  >
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="SubAccount Code" FieldName="SubAccount_Code"
                                    Visible="False" VisibleIndex="6">
                                    <PropertiesTextEdit>
                                        <ValidationSettings Display="Dynamic">
                                            <RequiredField ErrorText="Required" IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                    <CellStyle Wrap="True">
                                    </CellStyle>
                                    <EditFormSettings Visible="False" VisibleIndex="12" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="SubAccount Name" FieldName="SubAccount_Name"
                                    Visible="False" VisibleIndex="6">
                                    <PropertiesTextEdit>
                                        <ValidationSettings Display="Dynamic">
                                            <RequiredField ErrorText="Required" IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                    <EditFormSettings Visible="False" VisibleIndex="12" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="SubAccount Reference ID" FieldName="Subaccount_ReferenceID"
                                    Visible="False" VisibleIndex="6">
                                    <EditFormSettings Visible="False" VisibleIndex="12" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn FieldName="SubAccount_IsTDS" Visible="False" VisibleIndex="3">
                                    <EditFormSettings Visible="False" VisibleIndex="6" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="TDS Rate" FieldName="SubAccount_TDSRate" Visible="False"
                                    VisibleIndex="4">
                                    <EditFormSettings Visible="False" VisibleIndex="10" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataCheckColumn FieldName="SubAccount_IsFBT" Visible="false" VisibleIndex="5">
                                    <EditFormSettings Visible="False" VisibleIndex="8" />
                                </dxe:GridViewDataCheckColumn>


                                <dxe:GridViewDataTextColumn Caption="FBT Rate" FieldName="SubAccount_FBTRate" Visible="False"
                                    VisibleIndex="6">
                                    <EditFormSettings Visible="False" VisibleIndex="12" />
                                </dxe:GridViewDataTextColumn>

                                <dxe:GridViewDataTextColumn Caption="Rate Of Interest (P/a)" FieldName="SubAccount_RateOfInterest"
                                    Visible="False" VisibleIndex="15">
                                    <EditFormSettings Visible="False" VisibleIndex="15" />
                                    <CellStyle>
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="Depreciation" FieldName="SubAccount_Depreciation"
                                    Visible="False" VisibleIndex="15">
                                    <EditFormSettings Visible="False" VisibleIndex="15" />
                                    <CellStyle>
                                    </CellStyle>
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewCommandColumn Name="Edit" VisibleIndex="6" ShowEditButton="true">
                                     <HeaderTemplate>Actions</HeaderTemplate>
                                    <HeaderStyle  HorizontalAlign="Center" />
                                </dxe:GridViewCommandColumn>
                                
                                <dxe:GridViewDataTextColumn Caption="Opening DR/CR" VisibleIndex="3" FieldName="ShowWCustom" >
                                    <DataItemTemplate>
                                        
                                        <a href="javascript:void(0);" id="aaa2" style="color: #000099;display:block;text-align:center" runat="server" >Add/Edit</a>
                                        
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                    <CellStyle Wrap="true" HorizontalAlign="Center">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="Asset Details" VisibleIndex="4" FieldName="AssetShowWCustom">
                                    <DataItemTemplate>
                                        <a href="javascript:void(0);" id="aaa3" style="color: #000099;" runat="server">Asset
                                            Details </a>
                                    </DataItemTemplate>
                                    <EditFormSettings Visible="False" />
                                    <CellStyle Wrap="False">
                                    </CellStyle>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dxe:GridViewDataTextColumn>


                                <dxe:GridViewDataTextColumn Caption="Documents" VisibleIndex="5" FieldName="AssetCustom1" Visible="false">
                                    <EditFormSettings Visible="False" />
                                </dxe:GridViewDataTextColumn>

                            </Columns>
                            <settingscommandbutton>
                            <EditButton Image-Url="/assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
                                <%--<Image AlternateText="Edit" Url="/assests/images/Edit.png"></Image>--%>
                            </EditButton>
                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary" >
                                <%--<Styles>
                                    <Style CssClass="btn btn-primary"></Style>
                                </Styles>--%>
                            </UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger">
                               <%-- <Styles>
                                    <Style CssClass="btn btn-danger"></Style>
                                </Styles>--%>
                            </CancelButton>
                             </settingscommandbutton>
                            <Styles>
                                <LoadingPanel ImageSpacing="10px">
                                </LoadingPanel>
                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                </Header>
                            </Styles>
                            <ClientSideEvents EndCallback="function(s,e) { ShowErrorWithoutCustom(s.cpInsertErrorWithoutCustom);
                                        }" />
                            
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" ShowGroupedColumns="True"  />
                            <SettingsText PopupEditFormCaption="Add/Modify Sub Account" ConfirmDelete="Confirm delete?" />
                            <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <SettingsBehavior ColumnResizeMode="NextColumn" />
                            <%--<SettingsPager NumericButtonCount="20" ShowSeparators="True">
                            </SettingsPager>--%>
                            <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="false"  columnresizemode="NextColumn"/>
                            <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="350px" PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px" />

                            <Templates>
                                <EditForm>
                                    <table style="width: 100%; margin-top:20px" border="0" id="main">

                                        <tr>
                                            <td style="text-align: right;">Sub Account Name :</td>
                                            <td style="text-align: left;">
                                                <dxe:ASPxTextBox ID="txtAccountCodeWC" MaxLength="100" runat="server" Width="203px" ReadOnly="true"
                                                    ReadOnlyStyle-ForeColor="DarkGray" Text='<%#Bind("Contact_Name") %>'>
                                                </dxe:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;margin-top:4px;padding: 14px 5px 11px 11px;">Code:</td>
                                            <td style="text-align: left;;margin-top:4px">
                                                <dxe:ASPxTextBox ID="txtAccountNoWC" MaxLength="10" runat="server" Width="203px" ReadOnly="true"
                                                    ReadOnlyStyle-ForeColor="DarkGray" Text='<%#Bind("Contact_ID") %>'>
                                                </dxe:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <asp:HiddenField ID="hddnContactID" runat="server" />
                                        <tr id="TrTdsAppl1">
                                            <td style="text-align: right;">TDS Applicable:</td>
                                            <td style="text-align: left;">
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td colspan="2">

                                                             <%--<dxe:ASPxComboBox id="cmd_tdstcs" DataSourceID="tdstcs" Width="203px" Value='<%#Bind("SubAccount_IsTDS") %>' ClearButton-DisplayMode="Always" runat="server" TextField="tdsdescription" ValueField="tdscode" ></dxe:ASPxComboBox>--%>
                                                            <dxe:ASPxComboBox id="cmd_tdstcs" DataSourceID="tdstcs" Width="203px" Value='<%#Bind("SubAccount_TDSRate") %>' ClearButton-DisplayMode="Always" runat="server" TextField="tdsdescription" ValueField="tdscode" ></dxe:ASPxComboBox>
                                                             <%--<asp:TextBox ID="txtTdsType" runat="server" Width="204px" height="25px"  style="margin-bottom: 0px;" Text='<%#Bind("SubAccount_IsTDS") %>'
                                                                onkeyup="CallTdsAccount(this,'SearchTdsTcsCode',event)" MaxLength="50"></asp:TextBox>
                                                            <asp:HiddenField ID="txtTdsType_hidden" runat="server" Value='<%#Bind("SubAccount_TDSRate") %>' />--%>


                                                               
                                                          <%-- <asp:ListBox ID="lstTdsType" DataSourceID="Sqltdstype" runat="server" DataValueField="code" 
                                                                DataTextField="description" ></asp:ListBox>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                                
                                            </td>
                                            
                                        </tr>
                                        <tr id="TrFbtAppl1">
                                            <td style="text-align: right;">FBT Applicable:</td>
                                            <td style="text-align: left;">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <%--// .............................Code Commented and Added by Sam on 29112016 to get or set the value in edit mode. ..................................... --%>
                                                            <dxe:ASPxCheckBox ID="SubAccount_IsFBT12" runat="server" Width="38px" ValueType="System.Boolean" ClientInstanceName="cSubAccount_IsFBT12"
                                                                Checked='<%# Container.Grid.IsNewRowEditing ? false : Container.Grid.GetRowValues(Container.VisibleIndex, "SubAccount_IsFBT") %>' >
                                                                <ClientSideEvents CheckedChanged="function(s,e){fn_chekFbtRate(s,e);}"  />           
                                                                    
                                                            </dxe:ASPxCheckBox>
                                                            <asp:HiddenField ID="hdnisfbtrate" runat="server" Value='<%#Bind("SubAccount_IsFBT") %>'/>
                                                       
                                                            
                                                             <%--<dxe:ASPxCheckBox ID="SubAccount_IsFBT12" runat="server" Width="38px" ValueType="System.Boolean" 
                                                                Checked='<%#Bind("SubAccount_IsFBT") %>' >
                                                            
                                                        </dxe:ASPxCheckBox>--%>
                                                            <%--// .............................Code Above Commented and Added by Sam on 29112016 ...................................... --%>
                                                        </td>
                                                        <td id="fbtrate">FBT Rate:</td>
                                                        <td id="fbtrate1">
                                                            <dxe:ASPxTextBox ID="txtFBTRate" runat="server"  Width="109px" Text='<%#Bind("SubAccount_FBTRate") %>' ClientInstanceName="ctxtFBTRate"
                                                                MaskSettings-Mask="<0..9999g>.<00..99>" ValidationSettings-ErrorDisplayMode="None" MaxLength="8"
                                                                MaskSettings-IncludeLiterals="DecimalSymbol">
                                                            </dxe:ASPxTextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="TrRateOInt1">
                                            <td style="text-align: right;">Rate Of Interest (P/a):</td>
                                            <td style="text-align: left;">
                                                <dxe:ASPxTextBox ID="txtRateofIntrest" runat="server" Width="201px" MaxLength="8" Text='<%#Bind("SubAccount_RateOfInterest") %>'
                                                    MaskSettings-Mask="<0..9999g>.<00..99>" ValidationSettings-ErrorDisplayMode="None"
                                                    MaskSettings-IncludeLiterals="DecimalSymbol">
                                                </dxe:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr id="TrDepreciation1">
                                            <td style="text-align: right;">Depreciation:</td>
                                            <td style="text-align: left;">
                                                <dxe:ASPxTextBox ID="txtDepreciation" runat="server" Text='<%#Bind("SubAccount_Depreciation") %>' MaxLength="8"
                                                    MaskSettings-Mask="<0..9999g>.<00..99>" ValidationSettings-ErrorDisplayMode="None"
                                                    MaskSettings-IncludeLiterals="DecimalSymbol" Width="150px">
                                                </dxe:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <controls>
                        <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="ASPxGridViewTemplateReplacement1">
                         </dxe:ASPxGridViewTemplateReplacement>                                                           
                      </controls>
                                                <div style="text-align: left; padding: 2px 2px 2px 183px; font-weight: bold;">
                                                    <dxe:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement2" ReplacementType="EditFormUpdateButton"
                                                        runat="server" ></dxe:ASPxGridViewTemplateReplacement>
                                                    <dxe:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement3" ReplacementType="EditFormCancelButton"
                                                        runat="server" ></dxe:ASPxGridViewTemplateReplacement>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                  <%--  <table>
                                        <tr>
                                            <td>
                                                <controls>
                        <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                         </dxe:ASPxGridViewTemplateReplacement>                                                           
                      </controls>
                                                <div style="text-align: center; padding: 2px 2px 2px 2px">
                                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                        runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>--%>
                                </EditForm>
                            </Templates>
                        </dxe:ASPxGridView>
                    &nbsp;
                     <%--Top="100" Left="250"--%>
                        <dxe:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ContentUrl="frm_OpeningBalance.aspx"
                            CloseAction="CloseButton"
                             PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" 
                            ClientInstanceName="popup" Height="270px"
                            Width="430px" HeaderText="Add Opening Balance">
                        </dxe:ASPxPopupControl>
                </td>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="SubAccountWithoutCustom" Landscape="true" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
                </dxe:ASPxGridViewExporter>
        <%-- <div class="HiddenFieldArea" style="display: none;">
                <dxe:ASPxHiddenField runat="server" ClientInstanceName="chfID" ID="hfID">
                </dxe:ASPxHiddenField>
            </div>--%>
        <asp:SqlDataSource ID="SubAccount" runat="server" 
            SelectCommand="" InsertCommand="" ></asp:SqlDataSource>
            <%--UpdateCommand="update table1 set temp123='123'" 
            // .............................Code Commented and Added by Sam on 29112016. ..................................... --%>
             <%--InsertCommand="insert into table1 (temp123) values('0010')"
            UpdateCommand="update table1 set temp123='123'""--%>
            

        <%--// .............................Code Above Commented and Added by Sam on 29112016...................................... --%>

        <asp:SqlDataSource ID="WithoutCustom" runat="server" InsertCommand=""
            SelectCommand="" ></asp:SqlDataSource>

        <asp:SqlDataSource ID="tdstcs" runat="server"
            SelectCommand="prc_Subledger" SelectCommandType="StoredProcedure">
        <SelectParameters>
                <asp:SessionParameter Name="action" DefaultValue="PopulateDropDownFortdstcs" Type="String" />
            </SelectParameters></asp:SqlDataSource>

             <%-- UpdateCommand="update table1 set temp123='123'" DeleteCommand=""
            // .............................Code Commented and Added by Sam on 29112016. ..................................... --%>
           <%-- InsertCommand="insert into table1 (temp123) values('0010')"
            UpdateCommand="update table1 set temp123='123'" DeleteCommand=""--%>
         <%--// .............................Code Above Commented and Added by Sam on 29112016...................................... --%>
        

       <%-- <asp:SqlDataSource ID="Sqltdstype" runat="server" 
            SelectCommand="select ltrim(rtrim(tdstcs_description))+' ['+ltrim(rtrim(tdstcs_code))+']' as description,ltrim(rtrim(tdstcs_code)) as code 
				  from master_tdstcs order by description"></asp:SqlDataSource>--%>
    </div>

    
</asp:Content>
