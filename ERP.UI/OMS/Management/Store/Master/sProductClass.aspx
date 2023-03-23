<%--================================================== Revision History =============================================
Rev Number         DATE              VERSION          DEVELOPER           CHANGES
1.0                23-03-2023        2.0.36           Pallab              25733 : Master pages design modification
====================================================== Revision History =============================================--%>

<%@ Page Title="Product Class/Group" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Store.Master.management_sProductClass" CodeBehind="sProductClass.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style  type="text/css">
        #marketsGrid_DXMainTable .dx-nowrap, span.dx-nowrap
        {
            white-space:normal !important;
        }

    </style>
    <script type="text/javascript">
      
        //$(document).ready(function () {
        //    document.getElementsByClassName("dxgvPopupEditForm").style.padding = "5px;";
        //});
        //marketsGrid_DXPEForm_tcefnew
        //        function SetDropdownValue() {
        //            document.getElementById('marketsGrid_DXPEForm_efnew_DXEditor4_I').value = '0';
        //        }
        function OnHsnChange(s, e) {
           
        }

        function LastCall() {
            if (grid.cpErrorMsg != null) {
                jAlert(grid.cpErrorMsg);
                grid.cpErrorMsg = null;
            }
        }
        function OnAddBusinessClick(keyValue) {
            var url = '../../master/AssignIndustry.aspx?id1=' + keyValue+'&EntType=productclass' ;
            window.location.href = url;
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function UniqueCodeCheck() {
            var proclassid = '0';
            var id = '<%= Convert.ToString(Session["id"]) %>'; 
            //var ProductClassCode = document.getElementById('marketsGrid_DXPEForm_efnew_DXEditor1_I').value;
            var ProductClassCode = grid.GetEditor('ProductClass_Code').GetValue(); 
            if ((id != null) && (id != ''))
            {
                proclassid = id;
               '<%=Session["id"]=null %>'
            }
            
            var CheckUniqueCode = false;

            $.ajax({
                type: "POST",
                url: "sProductClass.aspx/CheckUniqueCode",
                //data: "{'ProductClassCode':'" + ProductClassCode + "'}",
                data: JSON.stringify({ ProductClassCode: ProductClassCode, proclassid: proclassid }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    CheckUniqueCode = msg.d;
                    if (CheckUniqueCode == true) {
                        jAlert('Please enter unique short name');
                        grid.GetEditor('ProductClass_Code').SetValue('');
                        grid.GetEditor('ProductClass_Code').Focus();
                        //document.getElementById('marketsGrid_DXPEForm_efnew_DXEditor1_I').value = '';
                        //document.getElementById('marketsGrid_DXPEForm_efnew_DXEditor1_I').Focus();
                    }
                }

            });
        }
    </script>

     

    <style>
        .dxbButton a {
            color: #000;
        }

        .dxbButton {
            padding: 3px !important;
        }

        /*Rev 1.0*/

        /*select
        {
            height: 30px !important;
            border-radius: 4px;
            -webkit-appearance: none;
            position: relative;
            z-index: 1;
            background-color: transparent;
            padding-left: 10px !important;
            padding-right: 22px !important;
        }*/

        .dxeButtonEditSys.dxeButtonEdit_PlasticBlue , .dxeTextBox_PlasticBlue
        {
            height: 30px;
            border-radius: 4px;
        }

        .dxeButtonEditButton_PlasticBlue
        {
            background: #094e8c !important;
            border-radius: 4px !important;
            padding: 0 4px !important;
        }

        #ASPxFromDate , #ASPxToDate , #ASPxASondate , #ASPxAsOnDate , #txtDOB , #txtAnniversary , #txtcstVdate , #txtLocalVdate ,
        #txtCINVdate , #txtincorporateDate , #txtErpValidFrom , #txtErpValidUpto , #txtESICValidFrom , #txtESICValidUpto , #FormDate , #toDate
        {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        .dxeDisabled_PlasticBlue
        {
            z-index: 0 !important;
        }

        #ASPxFromDate_B-1 , #ASPxToDate_B-1 , #ASPxASondate_B-1 , #ASPxAsOnDate_B-1 , #txtDOB_B-1 , #txtAnniversary_B-1 , #txtcstVdate_B-1 ,
        #txtLocalVdate_B-1 , #txtCINVdate_B-1 , #txtincorporateDate_B-1 , #txtErpValidFrom_B-1 , #txtErpValidUpto_B-1 , #txtESICValidFrom_B-1 ,
        #txtESICValidUpto_B-1 , #FormDate_B-1 , #toDate_B-1
        {
            background: transparent !important;
            border: none;
            width: 30px;
            padding: 10px !important;
        }

        #ASPxFromDate_B-1 #ASPxFromDate_B-1Img , #ASPxToDate_B-1 #ASPxToDate_B-1Img , #ASPxASondate_B-1 #ASPxASondate_B-1Img , #ASPxAsOnDate_B-1 #ASPxAsOnDate_B-1Img ,
        #txtDOB_B-1 #txtDOB_B-1Img ,
        #txtAnniversary_B-1 #txtAnniversary_B-1Img ,
        #txtcstVdate_B-1 #txtcstVdate_B-1Img ,
        #txtLocalVdate_B-1 #txtLocalVdate_B-1Img , #txtCINVdate_B-1 #txtCINVdate_B-1Img , #txtincorporateDate_B-1 #txtincorporateDate_B-1Img ,
        #txtErpValidFrom_B-1 #txtErpValidFrom_B-1Img , #txtErpValidUpto_B-1 #txtErpValidUpto_B-1Img , #txtESICValidFrom_B-1 #txtESICValidFrom_B-1Img ,
        #txtESICValidUpto_B-1 #txtESICValidUpto_B-1Img , #FormDate_B-1 #FormDate_B-1Img , #toDate_B-1 #toDate_B-1Img
        {
            display: none;
        }

        .dxtcLite_PlasticBlue > .dxtc-stripContainer .dxtc-activeTab, .dxgvFooter_PlasticBlue
        {
            background: #1b5ea4 !important;
        }

        /*select.btn
        {
            padding-right: 10px !important;
        }*/

        .panel-group .panel
        {
            box-shadow: 1px 1px 8px #1111113b;
            border-radius: 8px;
        }

        .dxpLite_PlasticBlue .dxp-current
        {
            background-color: #1b5ea4;
            padding: 3px 5px;
            border-radius: 2px;
        }

        #accordion {
            margin-bottom: 20px;
            margin-top: 10px;
        }

        .dxgvHeader_PlasticBlue {
    background: #1b5ea4 !important;
    color: #fff !important;
}
        #ShowGrid
        {
            margin-top: 10px;
        }

        .pt-25{
                padding-top: 25px !important;
        }

        .dxgvEditFormDisplayRow_PlasticBlue td.dxgv, .dxgvDataRow_PlasticBlue td.dxgv, .dxgvDataRowAlt_PlasticBlue td.dxgv, .dxgvSelectedRow_PlasticBlue td.dxgv, .dxgvFocusedRow_PlasticBlue td.dxgv
        {
            padding: 6px 6px 6px !important;
        }

        #lookupCardBank_DDD_PW-1
        {
                left: -182px !important;
        }
        .plhead a>i
        {
                top: 9px;
        }

        .clsTo
        {
            display: flex;
    align-items: flex-start;
        }

        input[type="radio"], input[type="checkbox"]
        {
            margin-right: 5px;
        }
        .dxeCalendarDay_PlasticBlue
        {
                padding: 6px 6px;
        }

        .modal-dialog
        {
            width: 50%;
        }

        .modal-header
        {
            padding: 8px 4px 8px 10px;
            background: #094e8c !important;
        }

        .TableMain100 #ShowGrid , .TableMain100 #ShowGridList , .TableMain100 #ShowGridRet , .TableMain100 #EmployeeGrid , .TableMain100 #GrdEmployee,
        .TableMain100 #GrdHolidays , #cityGrid
        {
            max-width: 98% !important;
        }

        /*div.dxtcSys > .dxtc-content > div, div.dxtcSys > .dxtc-content > div > div
        {
            width: 95% !important;
        }*/

        .btn-info
        {
                background-color: #1da8d1 !important;
                background-image: none;
        }

        .for-cust-icon {
            position: relative;
            z-index: 1;
        }

        .dxeDisabled_PlasticBlue, .aspNetDisabled
        {
            background: #f3f3f3 !important;
        }

        .dxeButtonDisabled_PlasticBlue
        {
            background: #b5b5b5 !important;
            border-color: #b5b5b5 !important;
        }

        #ddlValTech
        {
            width: 100% !important;
            margin-bottom: 0 !important;
        }

        .dis-flex
        {
            display: flex;
            align-items: baseline;
        }

        input + label
        {
            line-height: 1;
                margin-top: 7px;
        }

        .dxtlHeader_PlasticBlue
        {
            background: #094e8c !important;
        }

        .dxeBase_PlasticBlue .dxichCellSys
        {
            padding-top: 2px !important;
        }

        .pBackDiv
        {
            border-radius: 10px;
            box-shadow: 1px 1px 10px #1111112e;
        }
        .HeaderStyle th
        {
            padding: 5px;
        }

        .dxtcLite_PlasticBlue.dxtc-top > .dxtc-stripContainer
        {
            padding-top: 15px;
        }

        .pt-2
        {
            padding-top: 5px;
        }
        .pt-10
        {
            padding-top: 10px;
        }

        .pt-15
        {
            padding-top: 15px;
        }

        .pb-10
        {
            padding-bottom: 10px;
        }

        .pTop10 {
    padding-top: 20px;
}
        .custom-padd
        {
            padding-top: 4px;
    padding-bottom: 10px;
        }

        input + label
        {
                margin-right: 10px;
        }

        .btn
        {
            margin-bottom: 0;
        }

        .pl-10
        {
            padding-left: 10px;
        }

        .col-md-3>label, .col-md-3>span
        {
            margin-top: 0 !important;
        }

        .devCheck
        {
            margin-top: 5px;
        }

        .mtc-5
        {
            margin-top: 5px;
        }

        .mtc-10
        {
            margin-top: 10px;
        }

        /*select.btn
        {
           position: relative;
           z-index: 0;
        }*/

        select
        {
            margin-bottom: 0;
        }

        .form-control
        {
            background-color: transparent;
        }

        select.btn-radius {
    padding: 4px 8px 6px 11px !important;
}
        .mt-30{
            margin-top: 30px;
        }

        .panel-title h3
        {
            padding-top: 0;
            padding-bottom: 0;
        }

        .btn-radius
        {
            padding: 4px 11px !important;
            border-radius: 4px !important;
        }

        .crossBtn
        {
             right: 30px;
             top: 25px;
        }

        .mb-10
        {
            margin-bottom: 10px;
        }

        .btn-cust
        {
            background-color: #108b47 !important;
            color: #fff;
        }

        .btn-cust:hover
        {
            background-color: #097439 !important;
            color: #fff;
        }

        .gHesder
        {
            background: #1b5ca0 !important;
            color: #ffffff !important;
            padding: 6px 0 6px !important;
        }

        .close
        {
             color: #fff;
             opacity: .5;
             font-weight: 400;
        }

        .mt-24
        {
            margin-top: 24px;
        }

        .col-md-3
        {
            margin-top: 8px;
        }

        #upldBigLogo , #upldSmallLogo
        {
            width: 100%;
        }

        #DivSetAsDefault
        {
            margin-top: 25px;
        }

        /*.dxeDisabled_PlasticBlue, .aspNetDisabled {
            opacity: 0.4 !important;
            color: #ffffff !important;
        }*/
                /*.padTopbutton {
            padding-top: 27px;
        }*/
        /*#lookup_project
        {
            max-width: 100% !important;
        }*/
        /*Rev end 1.0*/
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>Product Class/Group</h3>
        </div>
    </div>
        <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <%-- <table width="100%">
                    <tr>
                        <td style="text-align: left; vertical-align: top">
                            <table>
                                <tr>
                                    <td id="ShowFilter">
                                        <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                            Show Filter</span></a>
                                    </td>
                                    <td id="Td1">
                                        <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                            All Records</span></a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                        <td class="gridcellright">
                            <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                                Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                ValueType="System.Int32" Width="130px">
                                <Items>
                                    <dxe:ListEditItem Text="Select" Value="0" />
                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                    <dxe:ListEditItem Text="CSV" Value="4" />
                                </Items>
                                <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                                </ButtonStyle>
                                <ItemStyle BackColor="Navy" ForeColor="White">
                                    <HoverStyle BackColor="#8080FF" ForeColor="White">
                                    </HoverStyle>
                                </ItemStyle>
                                <Border BorderColor="White" />
                                <DropDownButton Text="Export">
                                </DropDownButton>
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>--%>
                    <div class="SearchArea">
                        <div class="FilterSide" style="float: left; width: 500px">
                            <div style="float: left; padding-right: 5px; padding-bottom: 10px">
                                <% if (rights.CanAdd)
                                { %>
                                <a class="btn btn-success" href="javascript:void(0);" onclick="grid.AddNewRow()"><span>Add New</span> </a>
                                <%} %>
                                <% if (rights.CanExport)
                                               { %>
                                <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}" >
                                    <asp:ListItem Value="0">Export to</asp:ListItem>
                                    <asp:ListItem Value="1">PDF</asp:ListItem>
                                        <asp:ListItem Value="2">XLS</asp:ListItem>
                                        <asp:ListItem Value="3">RTF</asp:ListItem>
                                        <asp:ListItem Value="4">CSV</asp:ListItem>
                                </asp:DropDownList>
                                 <%} %>
                            </div>
                            <%--<div>
                                <a class="btn btn-primary" href="javascript:ShowHideFilter('All');"><span>All Records</span></a>
                            </div>--%>
                        </div>
                        <%--<div class="ExportSide" style="float: right">
                            <div>
                                <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    Font-Bold="False" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                                    ValueType="System.Int32" Width="130px">
                                    <Items>
                                        <dxe:ListEditItem Text="Select" Value="0" />
                                        <dxe:ListEditItem Text="PDF" Value="1" />
                                        <dxe:ListEditItem Text="XLS" Value="2" />
                                        <dxe:ListEditItem Text="RTF" Value="3" />
                                        <dxe:ListEditItem Text="CSV" Value="4" />
                                    </Items>
                                    <Border BorderColor="Black" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>
                            </div>
                        </div>--%>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="marketsGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="markets"   KeyFieldName="ProductClass_ID" Width="100%" OnHtmlRowCreated="marketsGrid_HtmlRowCreated" OnRowDeleting="marketsGrid_RowDeleting"
                          
                        OnHtmlEditFormCreated="marketsGrid_HtmlEditFormCreated" OnCustomCallback="marketsGrid_CustomCallback"
                        OnCustomErrorText="marketsGrid_CustomErrorText" OnStartRowEditing="marketsGrid_StartRowEditing" SettingsBehavior-AllowFocusedRow="true" OnCellEditorInitialize="marketsGrid_CellEditorInitialize" OnCommandButtonInitialize="marketsGrid_CommandButtonInitialize">
                        <SettingsSearchPanel Visible="True"  Delay="5000"/>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowFilterRow="true" ShowFilterRowMenu="true" />
                        <Columns>
                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="_ID">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="ProductClass_Code" Caption="Short Name">
                                <PropertiesTextEdit Width="200px" MaxLength="50"  Style-Wrap="True">

                                    <ClientSideEvents TextChanged="function(s, e) {UniqueCodeCheck();}" Init="function (s,e) {s.Focus(); }" />
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True" >
                                        <RequiredField IsRequired="True" ErrorText="Mandatory"/>
                                        <RegularExpression ValidationExpression="^[a-zA-Z0-9\s-()]{1,50}$" ErrorText="Mandatory" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                    <Paddings PaddingTop="15px" />
                                </EditCellStyle>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>
                            
                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="ProductClass_Name" Caption=" Name">
                                <PropertiesTextEdit Width="200px" MaxLength="100"  EncodeHtml="false">
                                   
                                    
                                    <%-- <ValidationSettings SetFocusOnError="True"  Display="None" ErrorImage-AlternateText="">
                                        <RequiredField IsRequired="True"  />
                                    </ValidationSettings>--%>

                                    
                                    <ValidationSettings ErrorDisplayMode="ImageWithTooltip" ErrorTextPosition="Right" SetFocusOnError="True">
                                        <RequiredField  IsRequired="True" ErrorText="Mandatory"/>
                                    </ValidationSettings>
                                

                                </PropertiesTextEdit>
                                <EditCellStyle HorizontalAlign="Left"  >
                                    <Paddings PaddingTop="15px" />
                                </EditCellStyle>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="ProductClass_Description"
                                Caption="Description"> 
                                <EditItemTemplate>
                                    <dxe:ASPxMemo ID="ASPxMemo1" runat="server" Width="198px" Height="60px" MaxLength="300" Text='<%# Bind("ProductClass_Description") %>'>
                                    </dxe:ASPxMemo>
                                </EditItemTemplate>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewDataComboBoxColumn FieldName="ProductClass_ParentID" VisibleIndex="1"
                                Caption="Parent Class">
                                <PropertiesComboBox Width="200px" DataSourceID="SqlSourceProductClass_ParentID" EnableIncrementalFiltering="True" ValueField="ProductClass_ID"
                                    TextField="ProductClass_Name" EnableSynchronization="Default" ValueType="System.String">
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Parent Class" />
                                <EditCellStyle HorizontalAlign="Left" Wrap="False">
                                    <Paddings PaddingTop="25px" />
                                </EditCellStyle>
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditFormCaptionStyle Wrap="False" HorizontalAlign="Right" VerticalAlign="Bottom">
                                </EditFormCaptionStyle>
                            </dxe:GridViewDataComboBoxColumn>


                            <dxe:GridViewDataTextColumn VisibleIndex="0"   Caption="HSN Code"   FieldName="ProductClass_HSNCode"> 
                                <EditItemTemplate>
                                   <dxe:ASPxGridLookup ID="HsnLookUp" runat="server"  DataSourceID="HsnDataSource" ClientInstanceName="cHsnLookUp" 
                                                                        KeyFieldName="ProductClass_HSNCode" Width="200px" TextFormatString="{0}"  Value='<%# Bind("ProductClass_HSNCode") %>'>
                                                                        <Columns> 
                                                                            <dxe:GridViewDataColumn FieldName="ProductClass_HSNCode" Caption="Code" Width="50"/>
                                                                            <dxe:GridViewDataColumn FieldName="Description" Caption="Description" Width="350"/>
                                                                        </Columns>
                                                                        <GridViewProperties  Settings-VerticalScrollBarMode="Auto"    >
                                                                             
                                                                            <Templates>
                                                                                <StatusBar>
                                                                                    <table class="OptionsTable" style="float: right">
                                                                                        <tr>
                                                                                            <td>
                                                                                              <%--  <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </StatusBar>
                                                                            </Templates>
                                                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true"/>
                                                                        </GridViewProperties>
                                                                    <ClientSideEvents TextChanged="OnHsnChange"></ClientSideEvents>
                                                                    </dxe:ASPxGridLookup>
                                </EditItemTemplate>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                                   <dxe:GridViewDataTextColumn Visible="True" VisibleIndex="0" FieldName="FullServiceTax" Caption="Service Accounting Codes" >
                                <EditFormSettings Visible="False"></EditFormSettings>
                                       <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>


                            <%--Service tax section--%>
                             <dxe:GridViewDataTextColumn VisibleIndex="0"   Caption="SAC"   FieldName="TAX_ID"  Visible="False"> 
                                  <EditFormSettings Visible="True" />
                                <EditItemTemplate>
                                   <dxe:ASPxGridLookup ID="serviceTaxLookup" runat="server"  DataSourceID="servicetaxDataSource" ClientInstanceName="cserviceTaxLookup" 
                                                                        KeyFieldName="TAX_ID" Width="200px" TextFormatString="{1}"  Value='<%# Bind("TAX_ID") %>'>
                                                                        <Columns> 
                                                                            <dxe:GridViewDataColumn FieldName="TAX_ID" Caption="Code" Width="0"/>
                                                                            <dxe:GridViewDataColumn FieldName="SERVICE_CATEGORY_CODE" Caption="Description" Width="350"/>
                                                                            <dxe:GridViewDataColumn FieldName="SERVICE_TAX_NAME" Caption="Description" Width="350"/>
                                                                        </Columns>
                                                                        <GridViewProperties  Settings-VerticalScrollBarMode="Auto"    >
                                                                             
                                                                            <Templates>
                                                                                <StatusBar>
                                                                                    <table class="OptionsTable" style="float: right">
                                                                                        <tr>
                                                                                            <td>
                                                                                              <%--  <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" Text="Close" ClientSideEvents-Click="CloseGridLookup" />--%>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </StatusBar>
                                                                            </Templates>
                                                                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowStatusBar="Visible" UseFixedTableLayout="true"/>
                                                                        </GridViewProperties>
                                                                    </dxe:ASPxGridLookup>
                                </EditItemTemplate>
                                 <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                      

                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="CreateDate">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="CreateUser">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="LastModifyDate">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="1" FieldName="LastModifyUser">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>

                            <dxe:GridViewCommandColumn ShowDeleteButton="true" Width="6%" ShowEditButton="true">
                                
                                <%-- <DeleteButton Visible="True">
                            </DeleteButton>
                            <EditButton Visible="True">
                            </EditButton>--%>

                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <%--    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                  { %>--%>
                                Actions
                               <%-- <%} %>--%>
                                </HeaderTemplate>                                
                            </dxe:GridViewCommandColumn>

                             <dxe:GridViewDataTextColumn VisibleIndex="2" Width="6%" CellStyle-HorizontalAlign="Center">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                  <EditFormSettings Visible="False"></EditFormSettings>
                                  <DataItemTemplate>
                                      <% if (rights.CanIndustry)
                                        { %>
                                          <a href="javascript:void(0);" onclick="OnAddBusinessClick('<%#Eval("ProductClass_Code") %>')" title="Add Industry" class="pad" style="text-decoration: none;"> 
                                            <img src="../../../../assests/images/icoaccts.gif" />
                                          </a>
                                      <%} %>
                                      </DataItemTemplate>
                              
                                <HeaderTemplate>
                                    Map
                                </HeaderTemplate>
                                 <Settings AllowAutoFilterTextInputTimer="False" />
                              
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        

                        <SettingsCommandButton>
                            <DeleteButton Image-Url="../../../../assests/images/Delete.png" ButtonType="Image" Image-AlternateText="Delete" Styles-Style-CssClass="pad">
                            </DeleteButton>
                            <EditButton Image-Url="../../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit">
                            </EditButton>
                            <UpdateButton Text="Update" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>


                        </SettingsCommandButton>
                        
                         <SettingsText PopupEditFormCaption="Add/Modify Products Class/Group" />

                        <SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>

                        <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />

                        <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm"  PopupEditFormHorizontalAlign="WindowCenter"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="400px"  />

                        <Templates>
                            <EditForm>
                                <div style="padding: 5px; padding-bottom: 0px;">
                                    <table>
                                        <tr>
                                            <td style="width: 5%"></td>
                                            <td style="width: 90%">
                                                <%--<controls>--%>
                                <dxe:ASPxGridViewTemplateReplacement runat="server"  ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                </dxe:ASPxGridViewTemplateReplacement>                                                           
                            <%--</controls>--%>
                                                <div style="padding: 2px 2px 5px 92px">
                                                    <div class="dxbButton" style="display: inline-block; color: Black;">
                                                        <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                            runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                    </div>
                                                    <div class="dxbButton" style="display: inline-block; color: Black;">
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
                        </Templates>
                        <clientsideevents endcallback="function(s, e) {	LastCall( );}" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SqlSourceProductClass_ParentID" runat="server" 
            SelectCommand="select ProductClass_ID,ProductClass_Name from Master_ProductClass order by ProductClass_Name"></asp:SqlDataSource>
        <asp:SqlDataSource ID="markets" runat="server" ConflictDetection="CompareAllValues"
            DeleteCommand="DELETE FROM [Master_ProductClass] where ProductClass_ID=@original_ProductClass_ID;
          INSERT INTO [dbo].[Master_ProductClass_Log] ([ProductClass_ID],[ProductClass_Code],[ProductClass_Name],[ProductClass_Description],[ProductClass_ParentID],[ProductClass_CreateUser]
      ,[ProductClass_CreateTime],[ProductClass_ModifyUser],[ProductClass_ModifyTime],[ProductClass_LogType],[ProductClass_LogUser],[ProductClass_LogTime])
	  SELECT [ProductClass_ID],[ProductClass_Code],[ProductClass_Name],[ProductClass_Description],[ProductClass_ParentID],[ProductClass_CreateUser],[ProductClass_CreateTime]
      ,[ProductClass_ModifyUser],[ProductClass_ModifyTime],'D',1,GETDATE() FROM [dbo].[Master_ProductClass] WHERE [ProductClass_ID] = @original_ProductClass_ID"
            InsertCommand="INSERT INTO  [dbo].[Master_ProductClass] 
        ([ProductClass_Code],[ProductClass_Name],[ProductClass_Description],[ProductClass_ParentID]  ,[ProductClass_CreateUser],[ProductClass_CreateTime],ProductClass_HSNCode,ProductClass_SERVICE_CATEGORY_CODE) 
  values(@ProductClass_Code,@ProductClass_Name,@ProductClass_Description,@ProductClass_ParentID,@CreateUser,getdate(),@ProductClass_HSNCode,@TAX_ID)"
            OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT B.[ProductClass_ID],B.[ProductClass_Code],B.[ProductClass_Name],B.[ProductClass_Description], (select A.ProductClass_Name from Master_ProductClass A where A.ProductClass_ID=B.ProductClass_ID ) as ProductClass_Names,  B.[ProductClass_ParentID],B.[ProductClass_CreateUser],B.[ProductClass_CreateTime] 
        ,B.[ProductClass_ModifyUser],B.[ProductClass_ModifyTime],B.ProductClass_HSNCode as ProductClass_HSNCode,B.ProductClass_SERVICE_CATEGORY_CODE as TAX_ID,
            (select SERVICE_CATEGORY_CODE from TBL_MASTER_SERVICE_TAX where TAX_ID=ProductClass_SERVICE_CATEGORY_CODE) FullServiceTax FROM [dbo].[Master_ProductClass] B "
            UpdateCommand="update [dbo].[Master_ProductClass] set [ProductClass_Code]=@ProductClass_Code,[ProductClass_Name]=@ProductClass_Name,
[ProductClass_Description]=@ProductClass_Description,[ProductClass_ParentID]=@ProductClass_ParentID,[ProductClass_ModifyUser] = @CreateUser,ProductClass_ModifyTime=getdate(),ProductClass_HSNCode=@ProductClass_HSNCode
            ,ProductClass_SERVICE_CATEGORY_CODE=@TAX_ID where ProductClass_ID=@ProductClass_ID;   
            update master_sproducts set sProducts_serviceTax=@TAX_ID where ProductClass_Code=@ProductClass_ID
            ">
             <%--update master_sproducts set sProducts_HsnCode =@ProductClass_HSNCode where  ProductClass_Code =@ProductClass_ID;--%>
            <DeleteParameters>
                <asp:Parameter Name="original_ProductClass_ID" Type="Int32" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="ProductClass_ID" Type="Int32" />
                <asp:Parameter Name="ProductClass_Code" Type="String" />
                <asp:Parameter Name="ProductClass_Name" Type="String" />
                <asp:Parameter Name="ProductClass_Description" Type="String" />
                <asp:Parameter Name="ProductClass_HSNCode" Type="String" />
                 <asp:Parameter Name="TAX_ID" Type="Int32" /> 
                <asp:Parameter Name="ProductClass_ParentID" Type="Int32" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
                <%-- <asp:Parameter Name="Markets_Country" Type="Int32" />
            <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />--%>
            </UpdateParameters>
            <InsertParameters>
                <%--<asp:Parameter Name="edu_markets" Type="String" />--%>
                <asp:Parameter Name="ProductClass_Code" Type="String" />
                <asp:Parameter Name="ProductClass_Name" Type="String" />
                <asp:Parameter Name="ProductClass_Description" Type="String" />
                <asp:Parameter Name="ProductClass_ParentID" Type="Int32" />
                <asp:Parameter Name="ProductClass_HSNCode" Type="String" />
                <asp:Parameter Name="TAX_ID" Type="Int32" />
                <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
            </InsertParameters>
        </asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>

         <asp:SqlDataSource ID="HsnDataSource" runat="server" 
            SelectCommand="select Code ProductClass_HSNCode,Description from tbl_HSN_Master"
            ></asp:SqlDataSource>

           <asp:SqlDataSource ID="servicetaxDataSource" runat="server"  
            SelectCommand="select TAX_ID,SERVICE_CATEGORY_CODE,SERVICE_TAX_NAME from TBL_MASTER_SERVICE_TAX"
            ></asp:SqlDataSource>

         <div class="HiddenFieldArea" style="display: none;">
            <asp:HiddenField runat="server" ID="hiddenedit" ClientIDMode="Static" />
        </div>
    </div>
    </div>
    <br />
</asp:Content>
