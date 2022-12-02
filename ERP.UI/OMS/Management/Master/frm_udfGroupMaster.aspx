<%@ Page title="UDF Group" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_frm_udfGroupMaster" CodeBehind="frm_udfGroupMaster.aspx.cs" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/assests/pluggins/choosen/choosen.min.js"></script>
    <script type="text/javascript">
        function AddNewGroup() {
            cPopUp_groupMaster.SetHeaderText('Add UDF Group');
            $('#MandatoryDesc').css({ 'display': 'none' });
            $('#MandatoryAppli').css({ 'display': 'none' });
            document.getElementById('chkIsVisible').checked = true;

            Status = 'SAVE_NEW';
            document.getElementById('txtGrp_desc').value = '';
            combo.SetText();
            cPopUp_groupMaster.Show();
        }
        function Call_save() {
            if (validate()) {
                grid.PerformCallback(Status);
            }
        }
        function validate() {

            var desc = document.getElementById('txtGrp_desc').value;
            var appliFor = combo.GetSelectedItem(); 
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
            return returnVal;
        }

        function LastCall() {
            if (grid.cpMsg != null) {
                if (grid.cpMsg != '') {
                    jAlert(grid.cpMsg);
                    grid.cpMsg = null;
                }
            }

            if (grid.cpHide != null) {
                if (grid.cpHide == 'Y') {
                    grid.cpHide = null;
                    cPopUp_groupMaster.Hide();
                }                   
            }

            if (grid.cpEditJson != null) {
                if (grid.cpEditJson.trim() != '') {
                    var jsonData = JSON.parse(grid.cpEditJson);
                    document.getElementById('txtGrp_desc').value = jsonData.grp_description;
                    combo.SetValue(jsonData.grp_applicablefor);
                    if (jsonData.grp_isVisible == 'True')
                        document.getElementById('chkIsVisible').checked = true;
                    else
                        document.getElementById('chkIsVisible').checked = false;
                }
            }

        }

        function OnEdit(obj) {
            cPopUp_groupMaster.SetHeaderText('Modify UDF Group');
            $('#MandatoryDesc').css({ 'display': 'none' });
            $('#MandatoryAppli').css({ 'display': 'none' });

            Status = obj;
            grid.PerformCallback('BEFORE_' + obj);
            cPopUp_groupMaster.Show();
        }

        function MakeRowInVisible() {
            cPopUp_groupMaster.Hide();
        }

        function DeleteRow(keyValue) {
            jConfirm('Confirm delete?', 'Confirmation Dialog', function (r) {
                if (r == true) {
                    grid.PerformCallback('Delete~' + keyValue);
                }
            });
        }
    </script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>UDF Group</h3>
        </div>
    </div>
    <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="PopUp_groupMaster" runat="server" ClientInstanceName="cPopUp_groupMaster"
            Width="400px" HeaderText="Add UDF Group" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" ba>
            <contentcollection>
                    <dxe:PopupControlContentControl runat="server">
                        <%--<div style="Width:400px;background-color:#FFFFFF;margin:0px;border:1px solid red;">--%>
                        <div class="Top clearfix">
                           
                            <table>
                                    <tr>
                                        <td>Group Name</td>
                                        <td>
                                            <asp:TextBox ID="txtGrp_desc" runat="server" MaxLength="100"></asp:TextBox></td>
                                        <td>
                                            <span id="MandatoryDesc" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-left: 9px;" title="Mandatory"></span>
                                        </td>
                                   </tr>
                                    <tr>
                                        <td style="padding-right:15px">Applicable For</td>
                                        <td valign="middle">
                                            <dxe:ASPxComboBox ID="CboApplicableFor" runat="server" ClientInstanceName="combo"
                                                ValueType="System.String" width="100%">
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                             <span id="MandatoryAppli" class="pullleftClass fa fa-exclamation-circle iconRed " style="color:red;display:none;padding-left: 9px;" title="Mandatory"></span>
                                        </td>
                                        
                                    </tr>
                                  <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblvisible" labelFor="chkIsVisible" Text="Visible"/>
                                            </td>
                                        <td valign="middle" colspan="2">
                                            <asp:CheckBox  ID ="chkIsVisible" runat="server"/>
                                        </td>
                                       
                                        
                                    </tr>
                                <tr>
                                    <td colspan="3" style="padding-left:121px;">
                                            <input id="btnSave" class="btn btn-primary" onclick="Call_save(status)" type="button" value="Save" />
                                    <input id="btnCancel" class="btn btn-danger" onclick="MakeRowInVisible()" type="button" value="Cancel" />
                                        </td>
                                        
                                    </tr>
                                </table>


                        </div>
                         
                    </dxe:PopupControlContentControl>
                </contentcollection>
            <headerstyle backcolor="LightGray" forecolor="Black" />
        </dxe:ASPxPopupControl>

    </div>


    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td colspan="4">
                    <table class="TableMain100">
                        <tr>
                            <td colspan="4" style="text-align: left; vertical-align: top">
                                <table>
                                    <tr>
                                        <td id="ShowFilter">
                                            <% if (rights.CanAdd)
                                               { %>
                                            <asp:HyperLink ID="HyperLink2" runat="server"
                                                NavigateUrl="javascript:void(0)" onclick="javascript:AddNewGroup()" class="btn btn-primary">Add New</asp:HyperLink>
                                            <%} %>
                                            
                                        </td>
                                        <td id="Td1">
                                            <% if (rights.CanExport)
                                               { %>
                                            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0">Export to</asp:ListItem>
                                                <asp:ListItem Value="1">PDF</asp:ListItem>
                                                <asp:ListItem Value="2">XLS</asp:ListItem>
                                                <asp:ListItem Value="3">RTF</asp:ListItem>
                                                <asp:ListItem Value="4">CSV</asp:ListItem>
                                            </asp:DropDownList>
                                              <%} %>
                                            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                            </dxe:ASPxGridViewExporter>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="gridcellright"></td>
                        </tr>
                    </table>
                </td>
            </tr>

            <tr>
                <td>
                    <dxe:ASPxGridView ID="gridudfGroup" runat="server" ClientInstanceName="grid" AutoGenerateColumns="False"
                        DataSourceID="SqlDataSource1"   SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false"  Width="100%" OnCustomCallback="gridudfGroup_CustomCallback"
                        OnCustomJSProperties="gridudfGroup_CustomJSProperties" OnHtmlRowCreated="gridudfGroup_HtmlRowCreated">
                         <settingssearchpanel visible="True" Delay="6000" />
                        <settingspager pagesize="50">
<FirstPageButton Visible="True"></FirstPageButton>

<LastPageButton Visible="True"></LastPageButton>

                            <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="50,100,150,200"/>
                              </settingspager>
                        <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="true" />

                        <clientsideevents endcallback="function(s, e) {
	LastCall(s.cpHeight);
}" />

                        <settingspager numericbuttoncount="20" pagesize="50" showseparators="True" alwaysshowpager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>

<PageSizeItemSettings Items="50, 100, 150, 200" Visible="True"></PageSizeItemSettings>
                        </settingspager>

                        <settingsediting mode="PopupEditForm" popupeditformheight="200px" popupeditformhorizontalalign="Center"
                            popupeditformmodal="True" popupeditformverticalalign="WindowCenter" popupeditformwidth="600px"
                            editformcolumncount="1" />
                        <settings showgrouppanel="True" showstatusbar="Visible" showfilterrow="true" showfilterrowmenu="true" />

                        <settingsbehavior confirmdelete="True" columnresizemode="NextColumn" />

                        <settingscommandbutton>
                           
                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
<Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>
                            </EditButton>
                             <DeleteButton Image-Url="../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Styles-Style-CssClass="pad">
<Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                            </DeleteButton>
                            <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary" Image-Width>
<Styles>
<Style CssClass="btn btn-primary"></Style>
</Styles>
                            </UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button"></CancelButton>
                        </settingscommandbutton>
                       
                        <settingstext popupeditformcaption="Add/Modify Category" confirmdelete="Confirm delete?" />
                        <styleseditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </styleseditors>

                        <columns>
                            <dxe:GridViewDataTextColumn FieldName="id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                <EditFormSettings Visible="False" />
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="grp_description" Caption="Group Name" Width="30%"
                                VisibleIndex="1" ShowInCustomizationForm="True">
                                <editcellstyle wrap="True">
                                </editcellstyle>
                                <CellStyle CssClass="gridcellleft" wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                             
                             <%--Commented by debjyoti Changed to drop down--%>
                            <%-- <dxe:GridViewDataTextColumn FieldName="grp_applicablefor" Caption="Aplicable for" Width="30%"
                                VisibleIndex="1" ShowInCustomizationForm="True">
                                <editcellstyle wrap="True">
                                </editcellstyle>
                                <CellStyle CssClass="gridcellleft" wrap="True">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>--%>

                            <%--Debjyoti Drop down in grid filter--%>

                            <dxe:GridViewDataComboBoxColumn Caption="Applicable For" FieldName="grp_applicablefor" VisibleIndex="2" Width="30%">
                                        <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True"
                                            ValueType="System.String" DataSourceID="SqlDataSourceapplicable" TextField="APP_NAME" ValueField="APP_NAME">
                                                        </PropertiesComboBox> 
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                    </dxe:GridViewDataComboBoxColumn>

                            <%--End Debjyoti Drop down in grid filter--%>

                            <dxe:GridViewDataTextColumn FieldName="grp_isVisible" Caption="Visible" Width="30%"
                                VisibleIndex="3" ShowInCustomizationForm="True">
                                <editcellstyle wrap="True">
                                </editcellstyle>
                                <CellStyle CssClass="gridcellleft" wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>


                            <dxe:GridViewDataTextColumn Caption="" VisibleIndex="4" Width="6%" >
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    Actions
                                   
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <% if (rights.CanEdit)
                                       { %>
                                    <a href="javascript:void(0);" onclick="OnEdit('EDIT~'+'<%#Eval("id") %>')">
                                        <img src="../../../assests/images/Edit.png" alt="Edit"></a>
                                    <% } %>
                                    <% if (rights.CanDelete)
                                       { %>
                                     <a href="javascript:void(0);" onclick="DeleteRow('<%#Eval("id") %>')"   alt="Delete">
                                        <img src="../../../assests/images/Delete.png" /></a>
                                     <% } %>
                                </DataItemTemplate>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                      
                        </columns>
                        <SettingsContextMenu  Enabled="true"></SettingsContextMenu>

                        <styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </styles>
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
             SelectCommand="SELECT id,grp_description,grp_applicablefor=(select u.APP_NAME from tbl_master_UDFApplicable u where u.APP_CODE=tbl_master_udfGroup.grp_applicablefor),case grp_isVisible when 1 then 'Visible' else 'Invisible' end as grp_isVisible FROM tbl_master_udfGroup"
             >
           
            <FilterParameters>
                <asp:Parameter Name="pin_code" Type="String" />
                <asp:Parameter Name="city_id" Type="String" />
            </FilterParameters>
        </asp:SqlDataSource>
          <asp:SqlDataSource ID="SqlDataSourceapplicable" runat="server"  
            SelectCommand="SELECT APP_NAME FROM tbl_master_UDFApplicable where IS_ACTIVE=0 order by ORDER_BY" >
        </asp:SqlDataSource>
        <br />
    </div>
</asp:Content>

