<%@ Page Title="UDF" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_category" CodeBehind="category.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function loadUdfGroup(obj) {
            cComboUdfGroup.ClearItems();
            if (combo.GetSelectedItem()) {
                var appLicable = combo.GetSelectedItem().value


                $.ajax({
                    type: "POST",
                    url: "category.aspx/GetUdfGroup",
                    data: JSON.stringify({ AppliFor: appLicable }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var list = msg.d;
                        var listItems = [];
                        cComboUdfGroup.AddItem('-Select-', '0');
                        cComboUdfGroup.SetValue(0);
                        if (list.length > 0) {

                            for (var i = 0; i < list.length; i++) {
                                var id = '';
                                var name = '';
                                id = list[i].split('|')[1];
                                name = list[i].split('|')[0];
                                cComboUdfGroup.AddItem(name, id);
                            }

                            if (obj) {
                                cComboUdfGroup.SetValue(obj);
                            }

                        }
                    }
                });
            }

        }

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function LastCall(obj) {
            // height();
            if (Action == '') {
                MakeRowInVisible();
            }

            if (grid.cpDelmsg != null) {
                if (grid.cpDelmsg.trim() != '') {
                    jAlert(grid.cpDelmsg);
                }
            }

            if (grid.cpEditJson != null) {

                var jsonData = JSON.parse(grid.cpEditJson);

                document.getElementById('txtcat_desc').value = jsonData.cat_description;
                combo.SetValue(jsonData.cat_applicablefor);
                cComboFieldType.SetValue(jsonData.cat_field_type);
                FieldTypeChange();
                loadUdfGroup(jsonData.cat_group_id);

                if (jsonData.isMandatory == 'False') {
                    document.getElementById("chkIsMandatory").checked = false;
                } else {
                    document.getElementById("chkIsMandatory").checked = true;
                }

                document.getElementById('txtMaxLength').value = jsonData.cat_max_length;
                document.getElementById('txtMaxValue').value = jsonData.cat_max_value;
                var maxDt = new Date(jsonData.cat_max_date.split('/')[2].substring(0, 4), jsonData.cat_max_date.split('/')[0] - 1, jsonData.cat_max_date.split('/')[1], 0, 0, 0, 0);
               
               
                cdtMaxDate.SetDate(maxDt);

                document.getElementById("txtComboOption").value = jsonData.cat_options;


                //Date: 12-12-2016 Name:Debjyoti
                //Reason: Bellow line commented because now data pass from server via json

                //document.getElementById('txtcat_desc').value = grid.cpEdit.split('~')[0];
                //combo.SetValue(grid.cpEdit.split('~')[1]);
                //cComboFieldType.SetValue(grid.cpEdit.split('~')[2]);
                //FieldTypeChange();
                //if (grid.cpEdit.split('~')[4] == 'False') {
                //    document.getElementById("chkIsMandatory").checked = false;
                //} else {
                //    document.getElementById("chkIsMandatory").checked = true;
                //}
                //document.getElementById('txtMaxLength').value = grid.cpEdit.split('~')[3];
                //document.getElementById('txtMaxValue').value = grid.cpEdit.split('~')[5];
                //var maxDt = new Date(grid.cpEdit.split('~')[6].split('/')[2].substring(0, 4), grid.cpEdit.split('~')[6].split('/')[0] -1, grid.cpEdit.split('~')[6].split('/')[1], 0, 0, 0, 0);
                //cdtMaxDate.SetDate(maxDt);
            }

            if (grid.cpSave != null) {
                if (grid.cpSave == 'Y') {
                    cPopup_Empcitys.Hide();
                    if (grid.cpSaveMsg != null) {
                        if (grid.cpSaveMsg != '') {
                            jAlert(grid.cpSaveMsg);
                        }
                    }
                }
            }
            if (grid.cpUpdate != null) {
                if (grid.cpUpdate == 'Y')
                    cPopup_Empcitys.Hide();
            }
        }

        function FieldTypeChange() {
             
            var fType = 0;
            if (cComboFieldType.GetSelectedItem()) {
                fType = cComboFieldType.GetSelectedItem().value;
            }

            if (fType == 1) {
                $('#txtMaxLength').css({ 'display': 'block' });
                $('#lblMaxLength').css({ 'display': 'block' });
            } else {
                $('#txtMaxLength').css({ 'display': 'none' });
                $('#lblMaxLength').css({ 'display': 'none' });
                $('#InvalidtxtMaxLength').css({ 'display': 'none' });
                $('#MoreThnDbChar').css({ 'display': 'none' });
                document.getElementById('txtMaxLength').value = '';
            }

            //if (fType == 3) {
            //    $('#lblMaxDate').css({ 'display': 'block' });
            //    $('#dtMaxDate').css({ 'display': 'block' });

            //    $('#chkIsMandatory').css({ 'display': 'none' });
            //    $('#lblIsMandatory').css({ 'display': 'none' });
            //} else {
            //    $('#lblMaxDate').css({ 'display': 'none' });
            //    $('#dtMaxDate').css({ 'display': 'none' });

            //    $('#chkIsMandatory').css({ 'display': 'block' });
            //    $('#lblIsMandatory').css({ 'display': 'block' });

            //    cdtMaxDate.SetText('');
            //}

            //if (fType == 4) {
            //    $('#lblMaxValue').css({ 'display': 'block' });
            //    $('#txtMaxValue').css({ 'display': 'block' });
            //} else {
            //    $('#lblMaxValue').css({ 'display': 'none' });
            //    $('#txtMaxValue').css({ 'display': 'none' });
            //    document.getElementById('txtMaxValue').value = '';
            //}

            if (fType == 6 || fType == 8) {
                $('#lblComboOption').css({ 'display': 'block' });
                $('#txtComboOption').css({ 'display': 'block' });
            } else {
                $('#lblComboOption').css({ 'display': 'none' });
                $('#txtComboOption').css({ 'display': 'none' });
                document.getElementById('txtComboOption').value = '';
            }

            if (fType == 7 || fType == 6 || fType == 8) {
                $('#chkIsMandatory').css({ 'display': 'none' });
                $('#lblIsMandatory').css({ 'display': 'none' });
                $('#lblComboOption').css({ 'display': 'block' });
                $('#txtComboOption').css({ 'display': 'block' });
            } else {
                $('#chkIsMandatory').css({ 'display': 'block' });
                $('#lblIsMandatory').css({ 'display': 'block' });
                $('#lblComboOption').css({ 'display': 'none' });
                $('#txtComboOption').css({ 'display': 'none' });
                document.getElementById('txtComboOption').value = '';
            }
            //if (fType == 8) {
            //    $('#lblComboOption').css({ 'display': 'block' });
            //    $('#txtComboOption').css({ 'display': 'block' });
            //} else {
            //    $('#lblComboOption').css({ 'display': 'none' });
            //    $('#txtComboOption').css({ 'display': 'none' });
            //    document.getElementById('txtComboOption').value = '';
            //}
        }

        function MakeRowVisible() {
            Action = 'add';
            //   document.getElementById("SaveRow").style.display = 'inline';
            Status = 'SAVE_NEW';
            document.getElementById('txtcat_desc').value = '';
            combo.SetText('');
            cComboFieldType.SetText('');
            cComboUdfGroup.SetText('');
            loadUdfGroup();
            FieldTypeChange();
            cPopup_Empcitys.SetHeaderText('Add UDF');
            cPopup_Empcitys.Show();
            // document.getElementById("ASPxCallbackPanel1_txtcat_desc").value = '';

        }
        function MakeRowInVisible() {
            //  document.getElementById("SaveRow").style.display = 'none';
            cPopup_Empcitys.Hide();
            combo.SetText('');
            cComboFieldType.SetText('');
            $('#MandatoryDesc').css({ 'display': 'none' });
            $('#MandatoryAppli').css({ 'display': 'none' });
            $('#MandatoryUdfGroup').css({ 'display': 'none' });

            $('#MandatoryFieldType').css({ 'display': 'none' });
            document.getElementById('txtcat_desc').value = '';
        }
        function Call_save() {
            if (validate()) {
                grid.PerformCallback(Status);
            }
        }

        function validate() {

            var desc = document.getElementById('txtcat_desc').value;
            var appliFor = combo.GetSelectedItem();
            var udfGroup = cComboUdfGroup.GetSelectedItem();
            var fieldType = cComboFieldType.GetSelectedItem();
            var maxLength = document.getElementById('txtMaxLength').value;
            var isnum = /^\d+$/.test(maxLength);
            var returnVal = true;
            if (desc.trim() == '') {
                $('#MandatoryDesc').css({ 'display': 'block' });
                returnVal = false;
            }
            else {
                $('#MandatoryDesc').css({ 'display': 'none' });
            }
            if (appliFor == null) {
                $('#MandatoryAppli').css({ 'display': 'block' });
                returnVal = false;
            }
            else {
                $('#MandatoryAppli').css({ 'display': 'none' });
            }
            if (fieldType == null) {
                $('#MandatoryFieldType').css({ 'display': 'block' });
                returnVal = false;
            }
            else {
                $('#MandatoryFieldType').css({ 'display': 'none' });
            }


            //if (udfGroup == null) {
            //    $('#MandatoryUdfGroup').css({ 'display': 'block' });
            //    returnVal = false;
            //}
            //else {
            //    $('#MandatoryUdfGroup').css({ 'display': 'none' });
            //}
            
            if (isnum != true && fieldType.value==1) {
                $('#InvalidtxtMaxLength').css({ 'display': 'block' });
                returnVal = false;
            }
            else {
                $('#InvalidtxtMaxLength').css({ 'display': 'none' });
            }

            if (isnum == true && maxLength>500) {
                $('#MoreThnDbChar').css({ 'display': 'block' });
                returnVal = false;
            }
            else {
                $('#MoreThnDbChar').css({ 'display': 'none' });
            }
            return returnVal;
        }

        function Call_edit() {
            grid.PerformCallback('edit');
        }

        function DeleteRow(keyValue) {
            //doIt = confirm('Confirm delete?');
            //if (doIt) {
            //    AddPanel.PerformCallback('Delete~' + keyValue);
            //}
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
        }

        function PEndCallBack(obj) {
            if (obj == 'Y') {
                Action = '';
                ShowHideFilter('All');

            }
            if (obj.length > 1) {
                alert(obj);
                grid.PerformCallback();

            }
        }
        function OnEdit(obj) {
            Action = 'edit';
            //document.getElementById("SaveRow").style.display = 'inline';
            Status = obj;
            grid.PerformCallback('BEFORE_' + obj);
            cPopup_Empcitys.SetHeaderText('Modify UDF');
            cPopup_Empcitys.Show();
        }

        function callback() {
            grid.PerformCallback();
        }
    </script>
    <style>
        .pullleftClass {
            position: absolute;
            right: -5px;
            top: 32px;
        }
        .col-md-10 label {
            margin-top:8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>UDF</h3>
        </div>
    </div>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
            Width="400px" HeaderText="Add/Modify Remark Category" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" ba>
            <contentcollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div class="Top clearfix">
                           <div class="row">
                               <div class="col-md-10 col-md-offset-1 relative">
                                   <label>UDF Name</label>
                                    <div>
                                        <asp:TextBox ID="txtcat_desc" runat="server" MaxLength="100"></asp:TextBox>
                                        <span id="MandatoryDesc" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-left: 9px;" title="Mandatory"></span>
                                    </div>      
                               </div>
                               <div class="col-md-10 col-md-offset-1 relative">
                                   <label>Applicable For</label>
                                    <div>
                                        <dxe:ASPxComboBox ID="CboApplicableFor" runat="server" ClientInstanceName="combo"
                                            ValueType="System.String" width="100%">
                                            <clientsideevents SelectedIndexChanged="function(s,e){
                                                loadUdfGroup(null);
                                                }" />
                                        </dxe:ASPxComboBox>
                                    </div>
                                   <span id="MandatoryAppli" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-left: 9px;" title="Mandatory"></span>
                               </div>
                               <div class="col-md-10 col-md-offset-1 relative">
                                   <label>UDF Group</label>
                                    <div>
                                        <dxe:ASPxComboBox ID="ComboUdfGroup" runat="server" ClientInstanceName="cComboUdfGroup"
                                            ValueType="System.String" width="100%">      
                                        </dxe:ASPxComboBox>
                                             
                                    </div>
                                    <span id="MandatoryUdfGroup" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-left: 9px;" title="Mandatory"></span>
                               </div>
                               <div class="col-md-10 col-md-offset-1 relative">
                                   <label>Field Type</label>
                                        <div>
                                            <dxe:ASPxComboBox ID="ComboFieldType" runat="server" ClientInstanceName="cComboFieldType"
                                                ValueType="System.String" width="100%">
                                                
                                                <clientsideevents selectedindexchanged="function(s, e) {
	                                                 FieldTypeChange();
                                                }" />
                                                
                                            </dxe:ASPxComboBox>
                                        </div>
                                        <span id="MandatoryFieldType" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-left: 9px;" title="Mandatory"></span>
                                        
                               </div>
                               <div class="col-md-10 col-md-offset-1 relative">
                                   <label class="pull-left" style="margin-right:8px;" ><span id="lblIsMandatory">Mandatory </span></label>
                                    <div class="pull-left" style="margin-top: 10px;">
                                            <asp:CheckBox runat="server" ID ="chkIsMandatory" />
                                    </div>
                                   <div class="clear"></div>
                               </div>
                               <div class="col-md-10 col-md-offset-1 relative" ID ="lblMaxLength" style="display:none">
                                   <label>
                                        <asp:Label runat="server"  Text="Max Length" ></asp:Label>
                                            </label>
                                    <div>
                                        <asp:TextBox ID="txtMaxLength" runat="server" MaxLength="3" style="display:none" Width="100%"></asp:TextBox> 
                                        <span id="InvalidtxtMaxLength" class="fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-left: 9px;    top: 33px;right: -4px; position: absolute;" title="Invalid Input"></span> 
                                        <span id="MoreThnDbChar" class="fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-left: 9px;    top: 8px;right: -4px; position: absolute;" title="Max length must be less than 500"></span> 
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtMaxLength" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                                    </div>
                               </div>
                               <div class="col-md-10 col-md-offset-1 relative" ID ="lblMaxValue" style="display:none">
                                   <label>
                                        <asp:Label runat="server"  Text="Max Value" ></asp:Label>
                                            </label>
                                    <div>
                                        <asp:TextBox ID="txtMaxValue" runat="server" MaxLength="10" style="display:none"></asp:TextBox> 

                                    </div>
                               </div>
                               <div class="col-md-10 col-md-offset-1 relative" ID ="lblComboOption" style="display:none">
                                   <label>
                                        <asp:Label runat="server"  Text="Comma Separated Values" ></asp:Label>
                                            </label>
                                    <div>
                                        <asp:TextBox ID="txtComboOption" runat="server" MaxLength="200" style="display:none"></asp:TextBox>  
                                    </div>
                               </div>
                               <div class="col-md-10 col-md-offset-1 relative" ID ="lblMaxDate" style="display:none">
                                   <label>
                                        <asp:Label runat="server"  Text="Max Date" style="display:none"></asp:Label>
                                            </label>
                                    <div>
                                        <dxe:ASPxDateEdit ID="dtMaxDate" runat="server" DisplayFormatString="dd-MM-yyyy"  NullText="dd-MM-yyyy" cssClass="hide" ClientInstanceName="cdtMaxDate">
                                            </dxe:ASPxDateEdit>
                                    </div>
                               </div>
                               <div class="col-md-10 col-md-offset-1 relative" style="padding-top:8px">
                                    <input id="btnSave" class="btn btn-primary" onclick="Call_save(status)" type="button" value="Save" />
                                    <input id="btnCancel" class="btn btn-danger" onclick="MakeRowInVisible()" type="button" value="Cancel" />
                               </div>
                           </div>
                            <table>
                                
                                <tr><td colspan="3" style="padding-left:121px;">
                                           
                                        </td>
                                        
                                    </tr>
                                </table>


                        </div>
                         
                    </dxe:PopupControlContentControl>
                </contentcollection>
            <contentstyle verticalalign="Top"></contentstyle>

            <headerstyle backcolor="LightGray" forecolor="Black" />
        </dxe:ASPxPopupControl>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>


    <div class="form_main">
         <div class="SearchArea">
            <div class="FilterSide">
                <div style="float: left; padding-right: 5px;">
                    <% if (rights.CanAdd)
                       { %>
                 <asp:HyperLink ID="HyperLink1" runat="server"
                                                NavigateUrl="javascript:void(0)" onclick="javascript:MakeRowVisible()" class="btn btn-primary">Add New</asp:HyperLink>

                    <% } %>
                </div>

                <div class="pull-left">
                    <% if (rights.CanExport)
                                               { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnChange="if(!AvailableExportOption()){return false;}" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                        <asp:ListItem Value="2">XLS</asp:ListItem>
                        <asp:ListItem Value="3">RTF</asp:ListItem>
                        <asp:ListItem Value="4">CSV</asp:ListItem>

                    </asp:DropDownList>
                      <% } %>
                </div>
            </div>

        </div>
        <table class="TableMain100">
        

            <tr>
                <td>
                    <dxe:ASPxGridView ID="gridCategory" runat="server" ClientInstanceName="grid" AutoGenerateColumns="False"
                        DataSourceID="SqlDataSource1" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  Width="100%" OnHtmlEditFormCreated="gridCategory_HtmlEditFormCreated"
                        OnHtmlRowCreated="gridCategory_HtmlRowCreated" OnCustomCallback="gridCategory_CustomCallback"
                        OnCustomJSProperties="gridCategory_CustomJSProperties" OnRowDeleting="gridCategory_RowDeleting">
                        <SettingsSearchPanel Visible="true" Delay="6000" />
                        <styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </styles>
                        <columns>
                            <dxe:GridViewDataTextColumn FieldName="id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="cat_description" Caption="UDF Name"
                                VisibleIndex="1" ShowInCustomizationForm="True">
                                <editcellstyle wrap="True">
                                </editcellstyle>
                                <CellStyle CssClass="gridcellleft" wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                             

                          <%--  <dxe:GridViewDataComboBoxColumn FieldName="cat_applicablefor" Caption="Applicable For"
                                VisibleIndex="2" Width="20%">
                                <PropertiesComboBox ValueType="System.String" ValueField="id" TextField="cat_applicablefor" EnableSynchronization="False" EnableIncrementalFiltering="True"  
                                     >
                                     

                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Applicable For"></EditFormSettings>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                    <Paddings PaddingTop="15px" />
                                </EditCellStyle>
                                <CellStyle CssClass="gridcellleft" Wrap="False">
                                </CellStyle>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataComboBoxColumn>--%>


                               <%--Debjyoti Drop down in grid filter--%>

                            <dxe:GridViewDataComboBoxColumn Caption="Applicable For" FieldName="cat_applicablefor" VisibleIndex="2" Width="30%" >
                                        <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                                            ValueType="System.String"  DataSourceID="SqlDataSourceapplicable" TextField="APP_NAME" ValueField="APP_NAME">
                               
                                                        </PropertiesComboBox> 
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                    </dxe:GridViewDataComboBoxColumn>

                            <%--End Debjyoti Drop down in grid filter--%>

                            <%--Debjyoti 21-12-2016--%>

                             <dxe:GridViewDataTextColumn FieldName="GroupId" Caption="Group Name"
                                VisibleIndex="2" ShowInCustomizationForm="True">
                                <editcellstyle wrap="True">
                                </editcellstyle>
                                <CellStyle CssClass="gridcellleft" wrap="True">
                                </CellStyle>
                                 <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <%--End Debjyoti 21-12-2016--%>


                            <dxe:GridViewDataTextColumn Caption="" VisibleIndex="3" Width="6%">
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    Actions
                                    <%--<asp:HyperLink ID="HyperLink2" runat="server"
                                        NavigateUrl="javascript:void(0)" onclick="javascript:MakeRowVisible()">Add New</asp:HyperLink>--%>
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnEdit('EDIT~'+'<%#Eval("id") %>')" class="pad">
                                        <img src="../../../assests/images/Edit.png" alt="Edit"></a>
                                    <% } %>
                                    <% if (rights.CanDelete)
                                       { %>
                                     <a href="javascript:void(0);" onclick="DeleteRow('<%#Eval("id") %>')" alt="Delete" class="">
                                        <img src="../../../assests/images/Delete.png" /></a>
                                     <% } %>
                                </DataItemTemplate>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                        </columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <settingscommandbutton>
                           
                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
<Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>
                            </EditButton>
                             <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete">
<Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                            </DeleteButton>
                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary" Image-Width>
<Styles>
<Style CssClass="btn btn-primary"></Style>
</Styles>
                            </UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button"></CancelButton>
                        </settingscommandbutton>
                        <styleseditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </styleseditors>
                        <settingssearchpanel visible="True" />
                        <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="true" />
                        <settingsediting mode="PopupEditForm" popupeditformheight="200px" popupeditformhorizontalalign="Center"
                            popupeditformmodal="True" popupeditformverticalalign="WindowCenter" popupeditformwidth="600px"
                            editformcolumncount="1" />
                        <settingstext popupeditformcaption="Add/Modify Category" confirmdelete="Confirm delete?" />
                        <settingspager numericbuttoncount="20" pagesize="20" showseparators="True" alwaysshowpager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </settingspager>
                        <settingsbehavior confirmdelete="True" columnresizemode="NextColumn" />

                        <clientsideevents endcallback="function(s, e) {
	LastCall(s.cpHeight);
}" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            DeleteCommand="DELETE FROM [tbl_master_remarksCategory] WHERE [id] = @id" InsertCommand="INSERT INTO [tbl_master_remarksCategory] ([cat_description],[cat_applicablefor]) VALUES (@cat_description,@cat_applicablefor)"
            SelectCommand="SELECT id,cat_description,cat_applicablefor=(select u.APP_NAME from tbl_master_UDFApplicable u where u.APP_CODE=tbl_master_remarksCategory.cat_applicablefor),case tbl_master_remarksCategory.cat_group_id when 0 then 'No Group'else (select  grp_description  from tbl_master_udfGroup where id=tbl_master_remarksCategory.cat_group_id) end as GroupId FROM [tbl_master_remarksCategory]"
            UpdateCommand="UPDATE [tbl_master_remarksCategory] SET [cat_description] = @cat_description,cat_applicablefor=@cat_applicablefor WHERE [id] = @id">
            <DeleteParameters>
                <asp:Parameter Name="id" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="cat_description" Type="String" />
                <asp:Parameter Name="cat_applicablefor" Type="String" />
                <asp:Parameter Name="id" Type="Int32" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="cat_description" Type="String" />
                <asp:Parameter Name="cat_applicablefor" Type="String" />
            </InsertParameters>
            <FilterParameters>
                <asp:Parameter Name="cat_description" Type="String" />
                <asp:Parameter Name="cat_applicablefor" Type="String" />
            </FilterParameters>
        </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSourceapplicable" runat="server" 
            SelectCommand="SELECT APP_NAME FROM tbl_master_UDFApplicable where IS_ACTIVE=1 order by ORDER_BY" >
        </asp:SqlDataSource>
        <br />
    </div>
    
</asp:Content>

