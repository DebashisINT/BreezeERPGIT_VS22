<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      24-05-2023          0026217: System Settings module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="SystemSettings.aspx.cs" Inherits="ERP.OMS.Management.Master.SystemSettings" %>

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

               // document.getElementById('txtVar_Name').value = jsonData.Variable_Name;
                document.getElementById('txtVal_Desc').value = jsonData.Variable_Description;

                document.getElementById('txtModuleName').value = jsonData.ModuleName;

                if (jsonData.FieldType == 'Text') {
                    $("#ComboValue").attr("style", "display:none");
                    $("#txtVal_Value").attr("style", "display:block");
                    document.getElementById('txtVal_Value').value = jsonData.Variable_Value;
                }
                else if (jsonData.FieldType == 'Dropdown')
                {
                    $("#txtVal_Value").attr("style", "display:none");
                    $("#ComboValue").attr("style", "display:block");
                    cComboUdfGroup.SetValue(jsonData.Variable_Value);
                }
             
                

                if (jsonData.IsActive == 'False') {
                    document.getElementById("chkIsMandatory").checked = false;
                } else {
                    document.getElementById("chkIsMandatory").checked = true;
                }

                           
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

       

        function MakeRowVisible() {
            Action = 'add';
            Status = 'SAVE_NEW';
            document.getElementById('txtcat_desc').value = '';
           
        }
     
        function Call_save() {
            
                grid.PerformCallback(Status);
           
        }

       

        function Call_edit() {
            grid.PerformCallback('edit');
        }

        function DeleteRow(keyValue) {
           
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
            Status = obj;
            grid.PerformCallback('BEFORE_' + obj);
            cPopup_Empcitys.SetHeaderText('Modify System Settings');
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
            margin-top: 8px;
        }
    </style>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
            margin-bottom: 0;
            -webkit-appearance: auto;
        }

        /*#grid {
            max-width: 98% !important;
        }*/
        #FormDate , #toDate , #dtTDate , #dt_PLQuote , #dt_partyInvDt
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        #FormDate_B-1 , #toDate_B-1 , #dtTDate_B-1 , #dt_PLQuote_B-1 , #dt_partyInvDt_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img , #dtTDate_B-1 #dtTDate_B-1Img , #dt_PLQuote_B-1 #dt_PLQuote_B-1Img ,
        #dt_partyInvDt_B-1 #dt_partyInvDt_B-1Img
        {
            display: none;
        }

        /*select
        {
            -webkit-appearance: auto;
        }*/

        .calendar-icon
        {
                right: 18px;
                bottom: 6px;
        }
        .padTabtype2 > tbody > tr > td
        {
            vertical-align: bottom;
        }
        #rdl_Salesquotation
        {
            margin-top: 0px;
        }

        .lblmTop8>span, .lblmTop8>label
        {
            margin-top: 8px !important;
        }

        .col-md-2, .col-md-4 {
    margin-bottom: 5px;
}

        .simple-select::after
        {
                top: 34px;
            right: 13px;
        }

        .dxeErrorFrameWithoutError_PlasticBlue.dxeControlsCell_PlasticBlue
        {
            padding: 0;
        }

        .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .backSelect {
    background: #42b39e !important;
}

        #ddlInventory
        {
                -webkit-appearance: auto;
        }

        /*.wid-90
        {
            width: 100%;
        }
        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-content
        {
            width: 97%;
        }*/
        .newLbl
        {
                margin: 3px 0 !important;
        }

        .lblBot > span, .lblBot > label
        {
                margin-bottom: 3px !important;
        }

        .col-md-2 > label, .col-md-2 > span, .col-md-1 > label, .col-md-1 > span
        {
            margin-top: 8px !important;
            font-size: 14px;
        }

        .col-md-6 span
        {
            font-size: 14px;
        }

        #gridDEstination
        {
            width:99% !important;
        }

        #txtEntity , #txtCustName
        {
            width: 100%;
        }
        .col-md-6 span
        {
            margin-top: 8px !important;
        }

        .rds
        {
            margin-top: 10px !important;
        }

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , select
        {
            height: 30px !important;
            
        }
        select
        {
            background-color: transparent;
                padding: 0 20px 0 5px !important;
        }

        .newLbl
        {
            font-size: 14px;
            margin: 3px 0 !important;
            font-weight: 500 !important;
            margin-bottom: 0 !important;
            line-height: 20px;
        }

        .crossBtn {
            top: 25px !important;
            right: 25px !important;
        }

        .wrapHolder
        {
            height: 60px;
        }
        #rdl_SaleInvoice
        {
            margin-top: 12px;
        }

        .dxeRoot_PlasticBlue
        {
            width: 100% !important;
        }

        #ShowFilter {
            padding-bottom: 0px !important;
        }

        @media only screen and (max-width: 1380px) and (min-width: 1300px)
        {
            .col-xs-1, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9, .col-xs-10, .col-xs-11, .col-xs-12, .col-sm-1, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-sm-10, .col-sm-11, .col-sm-12, .col-md-1, .col-md-2, .col-md-3, .col-md-4, .col-md-5, .col-md-6, .col-md-7, .col-md-8, .col-md-9, .col-md-10, .col-md-11, .col-md-12, .col-lg-1, .col-lg-2, .col-lg-3, .col-lg-4, .col-lg-5, .col-lg-6, .col-lg-7, .col-lg-8, .col-lg-9, .col-lg-10, .col-lg-11, .col-lg-12
            {
                 padding-right: 10px;
                 padding-left: 10px;
            }
            .simple-select::after
        {
                top: 34px;
            right: 8px;
        }
            .calendar-icon
        {
                right: 14px;
                bottom: 6px;
        }
        }

    </style>
    <%--Rev end 1.0--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>System Settings</h3>
        </div>
    </div>
        <div>
          <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
          </asp:DropDownList>
    </div>
        <div class="PopUpArea">
        <dxe:ASPxPopupControl ID="Popup_Empcitys" runat="server" ClientInstanceName="cPopup_Empcitys"
            Width="400px" HeaderText="Add/Modify System Settings" PopupHorizontalAlign="WindowCenter"
            BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
            Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True" >
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">

                    <div class="Top clearfix">
                        <div class="row">
                            <%--<div class="col-md-10 col-md-offset-1 relative">
                                <label>Variable Name</label>
                                <div>
                                    <asp:TextBox ID="txtVar_Name" runat="server" MaxLength="100" ReadOnly="true"></asp:TextBox>
                                    <span id="MandatoryName" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px;" title="Mandatory"></span>
                                </div>
                            </div>--%>

                             <div class="col-md-10 col-md-offset-1 relative">
                                <label>Variable Description</label>
                                <div>
                                    <asp:TextBox ID="txtVal_Desc" runat="server" MaxLength="300" ReadOnly="true" TextMode="MultiLine"></asp:TextBox>
                                 <%--   <span id="MandatoryDesc" class="pullleftClass fa fa-exclamation-circle iconRed " style="color: red; display: none; padding-left: 9px;" title="Mandatory"></span>--%>
                                </div>
                            </div>
                            <div class="col-md-10 col-md-offset-1 relative">
                                <label>Module Name</label>
                                <div>
                                    <asp:TextBox ID="txtModuleName" runat="server" MaxLength="500" ></asp:TextBox>                               
                                </div>
                            </div>
                            
                            <div class="col-md-10 col-md-offset-1 relative">
                                <label>Value</label>
                                <div>


                                    <dxe:ASPxComboBox ID="ComboValue" runat="server" ClientInstanceName="cComboUdfGroup"
                                        ValueType="System.String" Width="100%">
                                        <Items>
                                                     <dxe:ListEditItem Text="Yes" Value="Yes" Selected="true" />
                                                     <dxe:ListEditItem Text="No" Value="No" />
                                                        
                                        </Items>
                                    </dxe:ASPxComboBox>

                                    <asp:TextBox ID="txtVal_Value" runat="server" MaxLength="100"></asp:TextBox>


                                </div>
                            </div>
                            
                            <div class="col-md-10 col-md-offset-1 relative" style="display:none;">
                                <label class="pull-left" style="margin-right: 8px;"><span id="lblIsMandatory">Is Active?</span></label>
                                <div class="pull-left" style="margin-top: 10px;">
                                    <asp:CheckBox runat="server" ID="chkIsMandatory" />
                                </div>
                                <div class="clear"></div>
                            </div>
                            
                                              
                           
                            <div class="col-md-10 col-md-offset-1 relative" style="padding-top: 8px">
                                <input id="btnSave" class="btn btn-primary" onclick="Call_save(status)" type="button" value="Save" />
                               <%-- <input id="btnCancel" class="btn btn-danger" type="button" value="Cancel" />--%>
                            </div>
                        </div>
                        <table>

                            <tr>
                                <td colspan="3" style="padding-left: 121px;"></td>

                            </tr>
                        </table>


                    </div>

                </dxe:PopupControlContentControl>
            </ContentCollection>
            <ContentStyle VerticalAlign="Top"></ContentStyle>

            <HeaderStyle BackColor="LightGray" ForeColor="Black" />
        </dxe:ASPxPopupControl>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>


        <div class="form_main">
        <div class="SearchArea">
            <div class="FilterSide">
                <div style="float: left; padding-right: 5px;">
                    
                </div>

                <div class="pull-left">
                 
                </div>
            </div>

        </div>
        <table class="TableMain100">


            <tr>
                <td>
                    <dxe:ASPxGridView ID="gridCategory" runat="server" ClientInstanceName="grid" AutoGenerateColumns="False"
                        DataSourceID="SqlDataSource1" Width="100%" OnHtmlEditFormCreated="gridCategory_HtmlEditFormCreated"
                        OnHtmlRowCreated="gridCategory_HtmlRowCreated" OnCustomCallback="gridCategory_CustomCallback"
                        OnCustomJSProperties="gridCategory_CustomJSProperties" >
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <Columns>

                            <dxe:GridViewDataTextColumn FieldName="Variable_Name" Caption="Name" Width="300px"
                                VisibleIndex="0" ShowInCustomizationForm="True" Visible="false">
                                <EditCellStyle Wrap="True">
                                </EditCellStyle>
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn FieldName="Variable_Description" Caption="Description" Width="400px"
                                VisibleIndex="1" ShowInCustomizationForm="True">
                                <EditCellStyle Wrap="True">
                                </EditCellStyle>
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Variable_Value" Caption="Value" Width="50px"
                                VisibleIndex="2" ShowInCustomizationForm="True">
                                <EditCellStyle Wrap="True">
                                </EditCellStyle>
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                             <dxe:GridViewDataTextColumn FieldName="ModuleName" Caption="Module Name" Width="500px"
                                VisibleIndex="3" ShowInCustomizationForm="True">
                                <EditCellStyle Wrap="True">
                                </EditCellStyle>
                                <CellStyle CssClass="gridcellleft" Wrap="True">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn Caption="" VisibleIndex="4" Width="6%">
                                <CellStyle HorizontalAlign="Center">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    Actions
                                   
                                </HeaderTemplate>
                                <DataItemTemplate>
                                    <%--  <% if (rights.CanEdit)
                                       { %>--%>
                                    <a href="javascript:void(0);" onclick="OnEdit('EDIT~'+'<%#Eval("Variable_Name") %>' +'~'+'<%#Eval("FieldType") %>')" class="pad">
                                        <img src="../../../assests/images/Edit.png" alt="Edit"></a>
                                    <%--   <% } %>--%>
                                    
                                </DataItemTemplate>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsCommandButton>

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
                        </SettingsCommandButton>
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="200px" PopupEditFormHorizontalAlign="Center"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="600px"
                            EditFormColumnCount="1" />
                        <SettingsText PopupEditFormCaption="Add/Modify System Settings" ConfirmDelete="Confirm delete?" />
                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsBehavior ConfirmDelete="True" ColumnResizeMode="NextColumn" />

                        <ClientSideEvents EndCallback="function(s, e) {
	LastCall(s.cpHeight);
}" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
             SelectCommand="SELECT [Variable_Name],[Variable_Description],[Variable_Value],[IsActive],[UpdatedOn],[UpdatedBy],[FieldType],Isnull(ModuleName,'')ModuleName  FROM Config_SystemSettings  where IsActive=1">        
                   
           
            <FilterParameters>
                <asp:Parameter Name="Variable_Name" Type="String" />
                <asp:Parameter Name="Variable_Description" Type="String" />
            </FilterParameters>
        </asp:SqlDataSource> 
        
              
        <br />
    </div>
    </div>
</asp:Content>

