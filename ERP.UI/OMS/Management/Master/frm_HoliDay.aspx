<%@ Page Title="Holidays" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_frm_HoliDay" CodeBehind="frm_HoliDay.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v10.2.Export, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web" tagprefix="dx" %>--%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type="text/javascript" language="javascript">
        function btnWorkingAddmodify_Click() {
            var str = "frm_Holiday_popup.aspx?status=work&id=" + schedule;
            frmOpenNewWindow1(str, 400, 200)
        }
        function btnExchangeAddmodify_Click() {
            var str = "frm_Holiday_popup.aspx?status=exch&id=" + schedule;
            frmOpenNewWindow1(str, 500, 300)
        }
        function frmOpenNewWindow1(location, v_height, v_weight) {
            var y = (screen.availHeight - v_height) / 2;
            var x = (screen.availWidth - v_weight) / 2;
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + y + ",left=" + x + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=yes,resizable=no,dependent=no");
        }
        function PopulateGrid(obj1) {
            grid.PerformCallback();
            var item = obj1.split('~');
            schedule = obj1;
            if (item[3] == "work") {
                gridWorking.PerformCallback(obj1);

            }
            else {
                gridExchange.PerformCallback(obj1);

            }
            //height();
        }
        function btnAddNewHoliday(obj) {
            schedule = obj;
            //alert(obj);
            var items = obj.split('~');
            document.getElementById('TRGrid').style.display = 'inline';
            var send = items[0] + '~' + items[1];
            gridExchange.PerformCallback(obj);
            send = items[0] + '~' + items[2];
            gridWorking.PerformCallback(obj);
            //height();
        }
        function ShowGridsForUpdate() {
            document.getElementById('TRGrid').style.display = 'inline';
        }
        //function SignOff() {
        //    window.parent.SignOff();
        //}
        //function height() {
        //    if (document.body.scrollHeight >= 500)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '500px';
        //    window.frameElement.Width = document.body.scrollWidth;
        //}
        function LastCall(obj) {
            //height();
        }
    </script>

    <script type="text/ecmascript">
        function ReceiveServerData(rValue) {
            //alert(rValue);
            var DATA = rValue.split('~');
            if (DATA[0] == "Edit") {

            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Holidays</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100" cellpadding="0px" width="100%">
            <%--<tr>
            <td class="EHEADER" style="text-align: center">
                <span style="color: Blue"><strong>Holiday Master</strong></span>
            </td>
        </tr>--%>
            <tr>
                <td>
                    <% if (rights.CanAdd)
                               { %>
                    <a id="btnAddModify" href="javascript:void(0);" onclick="grid.AddNewRow()" class="btn btn-primary"><span>Add New</span></a><%} %>
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
                </td>
               <%-- <td class="gridcellright pull-right">
                    <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                        ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                        ValueType="System.Int32" Width="130px">
                        <Items>
                            <dxe:ListEditItem Text="Select" Value="0" />
                            <dxe:ListEditItem Text="PDF" Value="1" />
                            <dxe:ListEditItem Text="XLS" Value="2" />
                            <dxe:ListEditItem Text="RTF" Value="3" />
                            <dxe:ListEditItem Text="CSV" Value="4" />
                        </Items>
                        <Border BorderColor="black" />
                        <DropDownButton Text="Export">
                        </DropDownButton>
                    </dxe:ASPxComboBox>
                </td>--%>
            </tr>
            <tr>
                <td class="gridcellcenter" colspan="2">
                    <dxe:ASPxGridView ID="GridHoliday" ClientInstanceName="grid" runat="server" Width="100%"
                        AutoGenerateColumns="False" KeyFieldName="hol_id" DataSourceID="SqlDataSource1"
                        OnRowInserting="GridHoliday_RowInserting" OnRowUpdating="GridHoliday_RowUpdating"
                        OnInitNewRow="GridHoliday_InitNewRow" OnCustomCallback="GridHoliday_CustomCallback"
                        OnRowValidating="GridHoliday_RowValidating" OnHtmlEditFormCreated="GridHoliday_HtmlEditFormCreated" OnStartRowEditing="GridHoliday_StartRowEditing"
                        OnHtmlRowCreated="GridHoliday_HtmlRowCreated" OnCustomJSProperties="GridHoliday_CustomJSProperties" SettingsBehavior-AllowFocusedRow="true" OnCommandButtonInitialize="GridHoliday_CommandButtonInitialize"  
  >
                        <SettingsSearchPanel Visible="True" Delay="5000" />
                        <%--<Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>--%>
                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="Holiday Name" FieldName="hol_Description"
                                Width="65%" VisibleIndex="0">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormSettings Caption="Holiday Name" Visible="True" />
                                <EditCellStyle Wrap="False" HorizontalAlign="Left">
                                </EditCellStyle>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <PropertiesTextEdit MaxLength="200">
                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataDateColumn Caption="Date" FieldName="hol_DateOfHoliday" VisibleIndex="1"
                                Width="20%">
                                <PropertiesDateEdit DisplayFormatString="{0:dd MMM yyyy}" EditFormat="Custom" UseMaskBehavior="True" EditFormatString="dd-MM-yyyy">
                                    <ValidationSettings Display="Dynamic" SetFocusOnError="True">
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesDateEdit>
                                <EditFormSettings Caption="Date" Visible="True" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditCellStyle Wrap="False" HorizontalAlign="Left">
                                </EditCellStyle>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataDateColumn>
                            <dxe:GridViewDataTextColumn Caption="Is Bank Holiday" FieldName="hol_IsBankHoliday1"
                                VisibleIndex="2" Width="10%">
                                <EditFormSettings Caption="Is Bank Holiday" Visible="False" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditCellStyle Wrap="False" HorizontalAlign="Left">
                                </EditCellStyle>
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataCheckColumn Caption="Is Bank Holiday?" FieldName="hol_IsBankHoliday"
                                Visible="False" VisibleIndex="3" Width="10%">
                                <EditFormSettings Visible="True" />
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewCommandColumn VisibleIndex="4" ShowEditButton="true" ShowDeleteButton="true" Width="12%" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                                <%--  <EditButton Visible="True">
                            </EditButton>--%>
                                <%--<ClearFilterButton Visible="True">
                            </ClearFilterButton>--%>
                                <%--<DeleteButton Visible="True">
                            </DeleteButton>--%>
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<CellStyle HorizontalAlign="Center"></CellStyle>
                          
                                <HeaderTemplate>
                                    <span>Actions</span>
                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>
                       <%--     <dxe:GridViewDataTextColumn Caption="Schedule" VisibleIndex="5" Width="5%" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">

<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                                <DataItemTemplate>
                                    <a id="A1" href="javascript:void(0);" onclick="btnAddNewHoliday('<%# Container.KeyValue %>'+'~'+'<%# Eval("hol_exchange") %>'+'~'+'<%# Eval("hol_WorkingSchedule") %>')" title="Schedule">
                                        <img src="../../../assests/images/schedule.png" />
                                    </a>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                            </dxe:GridViewDataTextColumn>--%>
                       <%--     <dxe:GridViewDataCheckColumn Caption="Is Depository Holiday" FieldName="hol_IsDepositoryHoliday"
                                Visible="False" VisibleIndex="6">
                                <EditFormSettings Caption="Is Depository Holiday" Visible="True" />
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                            </dxe:GridViewDataCheckColumn>--%>
                            <dxe:GridViewDataCheckColumn Caption="Is This Holiday?" FieldName="hol_IsHoliday"
                                Visible="False" VisibleIndex="7">
                                <EditFormSettings Caption="Is This Holiday" Visible="True" />
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left">
                                </EditCellStyle>
                                <PropertiesCheckEdit ValueChecked="Y" ValueType="System.Char" ValueUnchecked="N">
                                </PropertiesCheckEdit>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                        </Columns>
                        <SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="260px" PopupEditFormHorizontalAlign="Center"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="450px"
                            EditFormColumnCount="1" />
                        <SettingsCommandButton>

                            <EditButton ButtonType="Image" Image-Url="../../../assests/images/Edit.png" Image-AlternateText="Edit" Styles-Style-CssClass="pad">
<Image AlternateText="Edit" Url="../../../assests/images/Edit.png"></Image>

<Styles>
<Style CssClass="pad"></Style>
</Styles>
                            </EditButton>
                            <DeleteButton ButtonType="Image" Image-Url="../../../assests/images/Delete.png" Image-AlternateText="Delete">
<Image AlternateText="Delete" Url="../../../assests/images/Delete.png"></Image>
                            </DeleteButton>
                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary">
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
                        <SettingsBehavior AllowFocusedRow="true" ConfirmDelete="True" />
                        <SettingsText PopupEditFormCaption="Add Holiday" ConfirmDelete="Confirm Delete?" />
                        <Templates>
                            <EditForm>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 90%">

                                            <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>

                                            <div style="text-align: center; padding: 2px 2px 2px 57px">
                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            </div>
                                        </td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                </table>
                            </EditForm>
                        </Templates>
                        <ClientSideEvents EndCallback="function(s, e) {
	LastCall(s.cpHeight);
}" />
                    </dxe:ASPxGridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues"
                        DeleteCommand="DELETE FROM [tbl_master_holiday] WHERE [hol_id] = @original_hol_id "
                        InsertCommand="INSERT INTO [tbl_master_holiday] ([hol_DateOfHoliday], [hol_IsHoliday], [hol_Description], [hol_IsBankHoliday], [hol_IsDepositoryHoliday],[Createdate],[CreateUser]) VALUES (@hol_DateOfHoliday, @hol_IsHoliday, @hol_Description, @hol_IsBankHoliday, @hol_IsDepositoryHoliday,getdate(),@createUser)"
                        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [hol_id], [hol_DateOfHoliday], convert(varchar(20),hol_DateOfHoliday,103) as hol_DateOfHoliday1,(case hol_IsHoliday when 'Y' then 'Yes' when 'N' then 'No' else '' end) as hol_IsHoliday1 ,(case hol_IsBankHoliday when 'Y' then 'Yes' when 'N' then 'No' else '' end) as hol_IsBankHoliday1, hol_exchange,hol_WorkingSchedule,
     [hol_IsHoliday],
    [hol_Description], 
    (case [hol_IsBankHoliday] when 'Y' then 'True' when 'N' then 'False' else '' end) as hol_IsBankHoliday,
    (case [hol_IsDepositoryHoliday] when 'Y' then 'True' when 'N' then 'False' else '' end) as hol_IsDepositoryHoliday  FROM [tbl_master_holiday]"
                        UpdateCommand="UPDATE [tbl_master_holiday] SET [hol_DateOfHoliday] = @hol_DateOfHoliday, [hol_IsHoliday] = @hol_IsHoliday, [hol_Description] = @hol_Description, [hol_IsBankHoliday] = @hol_IsBankHoliday, [hol_IsDepositoryHoliday] = @hol_IsDepositoryHoliday,[lastModifyDate]=getdate(),[lastModifyUser]=@modifyUser WHERE [hol_id] = @original_hol_id ">
                        <DeleteParameters>
                            <asp:Parameter Name="original_hol_id" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="hol_DateOfHoliday" Type="DateTime" />
                            <asp:Parameter Name="hol_IsHoliday" Type="String" />
                            <asp:Parameter Name="hol_Description" Type="String" />
                            <asp:Parameter Name="hol_IsBankHoliday" Type="String" />
                            <asp:Parameter Name="hol_IsDepositoryHoliday" Type="String" />
                            <asp:Parameter Name="original_hol_id" Type="Int32" />
                            <asp:Parameter Name="original_hol_DateOfHoliday" Type="DateTime" />
                            <asp:Parameter Name="original_hol_IsHoliday" Type="String" />
                            <asp:Parameter Name="original_hol_Description" Type="String" />
                            <asp:Parameter Name="original_hol_IsBankHoliday" Type="String" />
                            <asp:Parameter Name="original_hol_IsDepositoryHoliday" Type="String" />
                            <asp:SessionParameter Name="modifyUser" SessionField="userid" Type="string" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="hol_DateOfHoliday" Type="DateTime" />
                            <asp:Parameter Name="hol_IsHoliday" Type="String" />
                            <asp:Parameter Name="hol_Description" Type="String" />
                            <asp:Parameter Name="hol_IsBankHoliday" Type="String" />
                            <asp:Parameter Name="hol_IsDepositoryHoliday" Type="String" />
                            <asp:SessionParameter Name="createUser" SessionField="userid" Type="string" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                    </dxe:ASPxGridViewExporter>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top;">
                    <table class="TableMain100" cellpadding="0px" cellspacing="0px">
                        <tr id="TRGrid" style="display: none">
                            <td class="gridcellright" style="width: 93px">
                                <span class="Ecoheadtxt" style="color: Blue"><strong>Exchange:</strong></span>
                            </td>
                            <td class="gridcellleft" style="width: 329px">
                                <table class="TableMain100" cellspacing="0px">
                                    <tr>
                                        <td style="vertical-align: top; text-align: left">
                                            <dxe:ASPxGridView ID="GridExchange" ClientInstanceName="gridExchange" runat="server"
                                                AutoGenerateColumns="False" Width="100%" KeyFieldName="exh_id" OnCustomCallback="GridExchange_CustomCallback" OnCustomJSProperties="GridExchange_CustomJSProperties">
                                                <Columns>
                                                    <dxe:GridViewDataTextColumn FieldName="exh_name" VisibleIndex="0">
                                                        <HeaderTemplate>
                                                            <a id="A2" href="javascript:void(0);" onclick="btnExchangeAddmodify_Click()"><span
                                                                style="text-decoration: underline">Add/Modify</span></a>
                                                        </HeaderTemplate>
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                                <SettingsPager Visible="False">
                                                </SettingsPager>
                                                <Styles>
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                    </Header>
                                                    <LoadingPanel ImageSpacing="10px">
                                                    </LoadingPanel>
                                                </Styles>
                                                <SettingsBehavior AllowSort="False" />
                                                <ClientSideEvents EndCallback="function(s, e) {
	LastCall(s.cpHeight);
}" />
                                            </dxe:ASPxGridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="gridcellright" style="width: 97px">
                                <span class="Ecoheadtxt" style="color: Blue"><strong>Working Schedule:</strong></span>
                            </td>
                            <td style="text-align: left; width: 251px; vertical-align: top;">
                                <table class="TableMain100" cellpadding="0px" cellspacing="0px">
                                    <tr>
                                        <td style="vertical-align: top; text-align: left;">
                                            <dxe:ASPxGridView ID="GridWorking" ClientInstanceName="gridWorking" runat="server"
                                                AutoGenerateColumns="False" Width="100%" KeyFieldName="wor_id" OnCustomCallback="GridWorking_CustomCallback">
                                                <Columns>
                                                    <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="wor_scheduleName">
                                                        <HeaderTemplate>
                                                            <a id="A3" href="javascript:void(0);" onclick="btnWorkingAddmodify_Click()"><span
                                                                style="text-decoration: underline">Add/Modify</span></a>
                                                        </HeaderTemplate>
                                                    </dxe:GridViewDataTextColumn>
                                                </Columns>
                                                <SettingsPager Visible="False">
                                                </SettingsPager>
                                                <Styles>
                                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                    </Header>
                                                    <LoadingPanel ImageSpacing="10px">
                                                    </LoadingPanel>
                                                </Styles>
                                                <SettingsBehavior AllowSort="False" />
                                            </dxe:ASPxGridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 122px" colspan="3"></td>
                            <td class="gridcellleft" style="display: none">
                                <dxe:ASPxButton ID="btnModify" runat="server" Text="Modify">
                                </dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

