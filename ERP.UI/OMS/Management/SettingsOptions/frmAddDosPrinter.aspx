<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.SettingsOptions.management_SettingsOptions_frmAddDosPrinter" Codebehind="frmAddDosPrinter.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    

    <script type="text/javascript" src="/assests/js/init.js"></script>

    <script type="text/javascript" src="/assests/js/ajax-dynamic-list.js"></script>

    <link rel="stylesheet" href="../../windowfiles/dhtmlwindow.css" type="text/css" />

    <script type="text/javascript" src="../../windowfiles/dhtmlwindow.js"></script>

    <link rel="stylesheet" href="../../modalfiles/modal.css" type="text/css" />

    <script type="text/javascript" src="../../modalfiles/modal.js"></script>

    <script type="text/javascript" language="javascript">
       
    function OnUpdateClick() 
        {
//            uploader.UploadFile(); 
            grid.UpdateEdit();           
        }
        
    function ShowHideFilter(obj)
     {
       grid.PerformCallback(obj);
     }
     function height()
        {
            clearPreloadPage();
            
            if(document.body.scrollHeight>=300)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '300px';
                window.frameElement.Width = document.body.scrollWidth;
        }
    </script>
    <style>
        .dxgvControl_PlasticBlue a.btn {
            color:#fff;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
        <div class="panel-title">
           <%-- <h3>Employee Details</h3>--%>
        </div>

    </div>
        <div class="form_main">
            <table class="TableMain100">
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td style="text-align: left; vertical-align: top;">
                                    <table>
                                        <tr>
                                            <td >
                                                 <a href="javascript:void(0);" onclick="grid.AddNewRow()" class="btn btn-primary">Add new </a>
                                                 <span id="ShowFilter"><a href="javascript:ShowHideFilter('s');" class="btn btn-success">
                                                    Show Filter</a></span>
                                                <span id="Td1"><a href="javascript:ShowHideFilter('All');" class="btn btn-primary">
                                                    All Records</a></span>
                                            </td>
                                           
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dxe:ASPxGridView ID="DosPrinterGrid" runat="server" AutoGenerateColumns="False"
                            DataSourceID="Printerdata" KeyFieldName="DosPrinter_ID" ClientInstanceName="grid"
                            OnRowInserting="DosPrinterGrid_RowInserting" OnCustomCallback="DosPrinterGrid_CustomCallback"
                            OnRowUpdating="DosPrinterGrid_RowUpdating"
                            Width="100%" Font-Size="12px" OnRowValidating="DosPrinterGrid_RowValidating">
                            <Templates><EditForm>
                                    <table style="width: 100%;" id="main" border="0">
                                        <tbody>
                                            <tr>
                                                <td style="text-align: left">
                                                    <span style="color: black; text-align: left" class="Ecoheadtxt">Name :</span>
                                                </td>
                                                <td style="text-align: left">
                                                    <asp:TextBox ID="txtPrinterName" runat="server" Width="279px" Text='<%# Eval("DosPrinter_Name") %>' Height="30px" CssClass="EcoheadCon"></asp:TextBox>
                                                </td>
                                                <td style="text-align: left">
                                                    <span style="color: black; text-align: left" class="Ecoheadtxt">Location :</span>
                                                </td>
                                                <td style="text-align: left">
                                                    <%--<dxuc:ASPxUploadControl ID="selectLocationPrinter" ClientInstanceName="uploader" OnFileUploadComplete="selectLocationPrinter_upload" runat="server">
                                                  <ClientSideEvents FileUploadComplete="function(s, e) { if (e.isValid) { DosPrinterGrid.UpdateEdit(); }}" />
                                                  </dxuc:ASPxUploadControl>--%>
                                                    <asp:TextBox ID="txtLocation" runat="server" Width="279px" Text='<%# Eval("DosPrinter_Location") %>'  Height="30px" CssClass="EcoheadCon"></asp:TextBox>
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Label ID="lblCaption" runat="server"  Height="17px" CssClass="EcoheadCon"
                                                        Text="Format Of Path Given: //Name Of Machine Where Printer Is Installed/The Name of Folder Which Is Shared/"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="" colspan="4">
                                                    <div style="padding-right: 2px; padding-left: 2px; padding-bottom: 2px; padding-top: 2px;
                                                        ">
                                                        <a onclick="OnUpdateClick()" href="#" class="btn btn-primary">Update</a>
                                                        <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" runat="server" ReplacementType="EditFormCancelButton">
                                                        </dxe:ASPxGridViewTemplateReplacement>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                
                                    </EditForm>
                                    <TitlePanel>
                                    <table style="width: 100%">
                                        <tr>
                                            <td align="right">
                                                <table width="200">
                                                    <tr>
                                                        <td>
                                                            <%-- <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data" Height="18px" Width="88px"   Font-Size="12px" AutoPostBack="False">
                                                           <clientsideevents click="function(s, e) {gridDp.AddNewRow();}" />
                                                        </dxe:ASPxButton>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                
</TitlePanel>
</Templates>
                            <SettingsBehavior ConfirmDelete="True" AllowFocusedRow="True" />
                            <Styles>
<Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>

<FocusedRow CssClass="gridselectrowblue"></FocusedRow>

<LoadingPanel ImageSpacing="10px"></LoadingPanel>

<FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
</Styles>
                            <SettingsPager NumericButtonCount="20" PageSize="20"></SettingsPager>
                            <Columns>
<dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="DosPrinter_ID" Caption="Dosprinter ID">
<PropertiesTextEdit>
<ValidationSettings ErrorText=""></ValidationSettings>
</PropertiesTextEdit>
</dxe:GridViewDataTextColumn>
<dxe:GridViewDataTextColumn UnboundType="String" VisibleIndex="0" FieldName="DosPrinter_Name" Caption="Dosprinter Name">
<PropertiesTextEdit>
<ValidationSettings SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="Text" ErrorText="Please Enter Printer Name">
<RequiredField IsRequired="True"></RequiredField>
</ValidationSettings>
</PropertiesTextEdit>
</dxe:GridViewDataTextColumn>
<dxe:GridViewDataTextColumn UnboundType="String" VisibleIndex="1" FieldName="DosPrinter_Location" Caption="Dosprinter Location">
<PropertiesTextEdit>
<ValidationSettings SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="Text" ErrorText="Please Enter Location Path">
<RequiredField IsRequired="True"></RequiredField>
</ValidationSettings>
</PropertiesTextEdit>
</dxe:GridViewDataTextColumn>
<dxe:GridViewCommandColumn VisibleIndex="2">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>

<%--<DeleteButton Visible="True"></DeleteButton>--%>
<HeaderTemplate>
                                       Actions
                                    
</HeaderTemplate>

<%--<EditButton Visible="True"></EditButton>--%>
</dxe:GridViewCommandColumn>
</Columns>
                            <SettingsCommandButton>
                                <EditButton Text="Edit"></EditButton>
                            <DeleteButton Text="Delete"></DeleteButton>
                        </SettingsCommandButton>


                            <SettingsEditing PopupEditFormHeight="300px" PopupEditFormHorizontalAlign="Center"
                                PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="500px" />
                            <SettingsText PopupEditFormCaption="Add/Modify DP Details" ConfirmDelete="Confirm delete?" />
                            <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
                        </dxe:ASPxGridView>
                        <asp:SqlDataSource ID="Printerdata" runat="server" 
                            SelectCommand="select * from config_dosprinter where DosPrinter_User=@DosPrinter_User" InsertCommand="INSERT INTO config_dosprinter(DosPrinter_Name,DosPrinter_Location,DosPrinter_User) VALUES(@DosPrinter_Name,@DosPrinter_Location,@DosPrinter_User)"
                            UpdateCommand="Update config_dosprinter Set DosPrinter_Name=@DosPrinter_Name,DosPrinter_Location=@DosPrinter_Location,DosPrinter_User=@DosPrinter_User where DosPrinter_ID=@DosPrinter_ID"
                            DeleteCommand="Delete from config_dosprinter where DosPrinter_ID=@DosPrinter_ID">
                            <SelectParameters>                                
                                <asp:SessionParameter SessionField="userid" Name="DosPrinter_User" Type="Int32" />
                            </SelectParameters>
                            <InsertParameters>
                                <asp:Parameter Name="DosPrinter_Name" Type="string" />
                                <asp:Parameter Name="DosPrinter_Location" Type="string" />
                                <asp:Parameter Name="DosPrinter_User" Type="int32" />
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="DosPrinter_Name" Type="string" />
                                <asp:Parameter Name="DosPrinter_Location" Type="string" />
                                <asp:Parameter Name="DosPrinter_User" Type="int32" />
                                <asp:Parameter Name="DosPrinter_ID" Type="int32" />
                            </UpdateParameters>
                            <DeleteParameters>
                             <asp:Parameter Name="DosPrinter_ID" Type="int32" />
                            </DeleteParameters>
                        </asp:SqlDataSource>
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>