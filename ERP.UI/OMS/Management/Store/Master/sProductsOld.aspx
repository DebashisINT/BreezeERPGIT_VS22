<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Store.Master.management_master_Store_sProductsOld" Codebehind="sProductsOld.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../../../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script type="text/javascript" src="/assests/js/jquery-1.3.2.min.js"></script>

    <script type="text/javascript">

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }

        function PopupOpen(obj) {
            var URL = '../../../Management/Master/Contact_Document.aspx?idbldng=' + obj;
            OnMoreInfoClick(URL, "Products Document Details", '1000px', '400px', "Y");
            document.getElementById("marketsGrid_DXPEForm_efnew_DXEditor1_I").focus();

        }
        
        
        function CheckColorRadio(s, e) {
            if (document.getElementById("marketsGrid_DXPEForm_efnew_editnew_14_ASPxRadioButtonList_RB0_I") != null) {
                document.getElementById("marketsGrid_DXPEForm_efnew_editnew_14_ASPxRadioButtonList_RB0_I").checked = true;
                document.getElementById("marketsGrid_DXPEForm_efnew_DXEditor15_I").style.display = 'none';
            }
            if (document.getElementById("marketsGrid_DXPEForm_ef0_DXEditor15_I") != null) {
                if (document.getElementById("marketsGrid_DXPEForm_ef0_DXEditor15_I").value == '') {
                    document.getElementById("marketsGrid_DXPEForm_efnew_editnew_14_ctl00_RB0_I").checked = true;
                }
                else {
                    document.getElementById("marketsGrid_DXPEForm_ef0_edit0_14_ctl00_RB1_I").checked = true;
                }
            }

            if (document.getElementById('marketsGrid_DXPEForm_ef1_edit1_14_ctl00_RB0_I') != null) {
                if (document.getElementById('marketsGrid_DXPEForm_ef1_DXEditor15_I').value == null || document.getElementById('marketsGrid_DXPEForm_ef1_DXEditor15_I').value == '') {
                    document.getElementById("marketsGrid_DXPEForm_ef1_edit1_14_ctl00_RB0_I").checked = true;
                    document.getElementById("marketsGrid_DXPEForm_ef1_DXEditor15_I").style.display = 'none';
                }
                else {
                    document.getElementById("marketsGrid_DXPEForm_ef1_edit1_14_ctl00_RB1_I").checked = true;
                }
            }
        }

        function CheckSizeRadio(s, e) {
            if (document.getElementById("marketsGrid_DXPEForm_efnew_editnew_16_ASPxRadioButtonListSize_RB0_I") != null) {
                document.getElementById("marketsGrid_DXPEForm_efnew_editnew_16_ASPxRadioButtonListSize_RB0_I").checked = true;
                document.getElementById("marketsGrid_DXPEForm_efnew_DXEditor17_I").style.display = 'none';
            }
            if (document.getElementById("marketsGrid_DXPEForm_ef0_DXEditor17_I") != null) {
                if (document.getElementById("marketsGrid_DXPEForm_ef0_DXEditor17_I").value == '') {
                    document.getElementById("marketsGrid_DXPEForm_ef0_edit0_16_ASPxRadioButtonListSize_RB0_I").checked = true;
                }
                else {
                    document.getElementById("marketsGrid_DXPEForm_ef0_edit0_16_ASPxRadioButtonListSize_RB1_I").checked = true;

                }
            }

            if (document.getElementById('marketsGrid_DXPEForm_ef1_edit1_16_ASPxRadioButtonListSize_RB0_I') != null) {
                if (document.getElementById('marketsGrid_DXPEForm_ef1_DXEditor17_I').value == null || document.getElementById('marketsGrid_DXPEForm_ef1_DXEditor17_I').value == '') {
                    document.getElementById("marketsGrid_DXPEForm_ef1_edit1_16_ASPxRadioButtonListSize_RB0_I").checked = true;
                    document.getElementById("marketsGrid_DXPEForm_ef1_DXEditor17_I").style.display = 'none';
                }
                else {
                    document.getElementById("marketsGrid_DXPEForm_ef1_edit1_16_ASPxRadioButtonListSize_RB1_I").checked = true;
                }
            }
        }

        function CheckColor(s, e) {
            if (document.getElementById('marketsGrid_DXPEForm_efnew_editnew_14_ASPxRadioButtonList_RB0_I') != null) {
                if (document.getElementById('marketsGrid_DXPEForm_efnew_editnew_14_ASPxRadioButtonList_RB0_I').checked) {
                    document.getElementById("marketsGrid_DXPEForm_efnew_DXEditor15_I").style.display = 'none';

                }
                else if (document.getElementById('marketsGrid_DXPEForm_efnew_editnew_14_ASPxRadioButtonList_RB1_I').checked) {
                    document.getElementById("marketsGrid_DXPEForm_efnew_DXEditor15_I").style.display = 'block';
                }
                else {
                    document.getElementById("marketsGrid_DXPEForm_efnew_DXEditor15_I").style.display = 'block';
                }
            }
            if (document.getElementById('marketsGrid_DXPEForm_ef1_edit1_14_ctl00_RB0_I') != null) {
                if (document.getElementById('marketsGrid_DXPEForm_ef1_edit1_14_ctl00_RB0_I').checked) {
                    document.getElementById("marketsGrid_DXPEForm_ef1_DXEditor15_I").style.display = 'none';

                }
                else if (document.getElementById('marketsGrid_DXPEForm_ef1_edit1_14_ctl00_RB1_I').checked) {
                    document.getElementById("marketsGrid_DXPEForm_ef1_DXEditor15_I").style.display = 'block';
                }
                else {
                    document.getElementById("marketsGrid_DXPEForm_ef1_DXEditor15_I").style.display = 'block';
                }
            }
        }

        function CheckSize(s, e) {
            if (document.getElementById('marketsGrid_DXPEForm_efnew_editnew_16_ASPxRadioButtonListSize_RB0_I').checked) {
                document.getElementById("marketsGrid_DXPEForm_efnew_DXEditor17_I").style.display = 'none';
            }
            else if (document.getElementById('marketsGrid_DXPEForm_efnew_editnew_16_ASPxRadioButtonListSize_RB0_I')) {
                document.getElementById("marketsGrid_DXPEForm_efnew_DXEditor17_I").style.display = 'block';
            }
            else {
                document.getElementById("marketsGrid_DXPEForm_efnew_DXEditor17_I").style.display = 'block';
            }

            if (document.getElementById('marketsGrid_DXPEForm_ef1_edit1_16_ASPxRadioButtonListSize_RB0_I') != null) {
                if (document.getElementById('marketsGrid_DXPEForm_ef1_edit1_16_ASPxRadioButtonListSize_RB0_I').checked) {
                    document.getElementById("marketsGrid_DXPEForm_ef1_DXEditor17_I").style.display = 'none';

                }
                else if (document.getElementById('marketsGrid_DXPEForm_ef1_edit1_16_ASPxRadioButtonListSize_RB1_I').checked) {
                    document.getElementById("marketsGrid_DXPEForm_ef1_DXEditor17_I").style.display = 'block';
                }
                else {
                    document.getElementById("marketsGrid_DXPEForm_ef1_DXEditor17_I").style.display = 'block';
                }
            }
        }
    </script>

    <style type="text/css">
        .dxgvCustomization, .dxgvPopupEditForm
        {
            margin: 0;
            overflow: auto;
            padding: 0;
            width: 100%;
        }
        .dxgvEditFormTable
        {
            background-color: #ccc !important;
        }
        .dxgvEditFormCaption
        {
            text-align: left !important;
        }
        td.dxeControlsCell table.dxeButtonEdit, #marketsGrid_DXPEForm_efnew_DXEditor4
        {
            width: 300px !important;
        }
        #marketsGrid_DXPEForm_efnew_DXEditor5
        {
            width: 300px !important;
        }
        .dxeButtonEdit
        {
            width: 65% !important;
        }
        .dxbButton
        {
            height: 20px;
            line-height: 20px;
        }
        .dxgvControl a
        {
            color: #000 !important;
            padding: 4px;
        }
        .dxpcControl
        {
            vertical-align: top;
        }
        #marketsGrid_DXPEForm_PW-1
        {
            height: 100%;
        }
       
    </style>
    
    
    
    <script type="text/javascript">

        var counter = 0;


        function fetchLebel() {

            $("#generatedForm").html("");
            counter = 0;


            $("#marketsGrid_DXPEForm_efnew_DXEFT  tr td label").each(function() {

                var newField = "<div style='width:500px; margin-left:5px; float:left; margin-bottom:5px;'><label id='LblKey" + counter + "' style='width:110px; float:left;'>" + $(this).text() + "</label>";
                newField += "<input type='text' id='TxtKey" + counter + "' value='" + $(this).text() + "' style='margin-left:41px; width:250px;' />";

                //alert($(this).attr("id").split('_')[4]);

                if (String($(this).attr("for").split('_')[4]) != "undefined") {
                    newField += "<input type='text' id='HddnKey" + counter + "' value='" + $(this).attr("for") + "' style='display:none; margin-left:41px; width:250px;' />";
                }
                else {
                    //alert($(this).attr("id"));
                    newField += "<input type='text' id='HddnKey" + counter + "' value='" + $(this).attr("for") + "' style='display:none; margin-left:41px; width:250px;' />";
                }
                newField += "</div>";

                $("#generatedForm").append(newField);

                counter++;

            });

            AssignValuePopup.Show();

        }




        function SaveDataToResource() {

            var key = "";
            var value = "";

            for (var i = 0; i < counter; i++) {

                if (key == "") {

                    key = $("#HddnKey" + i).val();
                    value = $("#TxtKey" + i).val();

                }
                else {

                    key += "," + $("#HddnKey" + i).val();
                    value += "," + $("#TxtKey" + i).val();

                }

            }

            //alert(key); alert(value);

            $("#AssignValuePopup_KeyField").val(key);
            $("#AssignValuePopup_ValueField").val(value);
            $("#AssignValuePopup_RexPageName").val("productVal");


            return true;

        }
        
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="TableMain100">
        <tr>
            <td style="text-align: center">
                <strong><span style="color: #000099">Products</span></strong>
            </td>
        </tr>
        <tr>
            <td>
                <%--   <table width="100%">
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
                        <div style="float: left; padding-right: 5px;">
                            <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                Show Filter</span></a>
                        </div>
                        <div>
                            <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">
                                All Records</span></a>
                        </div>
                    </div>
                    <div class="ExportSide" style="float: right; width: 220px; position: relative; left: 75px">
                        <div>
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
                        </div>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <dxe:ASPxGridView ID="marketsGrid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                    DataSourceID="markets" KeyFieldName="sProducts_ID" Width="100%" OnInitNewRow="marketsGrid_OnInitNewRow"
                    OnHtmlRowCreated="marketsGrid_HtmlRowCreated" OnHtmlEditFormCreated="marketsGrid_HtmlEditFormCreated"
                    OnCustomCallback="marketsGrid_CustomCallback">
                    <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
                    <Columns>
                        <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="0" FieldName="sProduct_ID">
                            <EditFormSettings Visible="False"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>
                        <%--sProducts_Code--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="sProducts_Code" Caption="Code">
                            <PropertiesTextEdit Width="300px" MaxLength="30">
                             <ClientSideEvents Init="function (s,e) {s.Focus(); }" />
                                <ValidationSettings ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                    <RequiredField IsRequired="True" ErrorText="Please Enter Products Code" />
                                    <RegularExpression ValidationExpression="^.{1,30}$" ErrorText="Please enter maximum 30 alphanumaric for Product Class Code" />
                                </ValidationSettings>
                            </PropertiesTextEdit>
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataTextColumn>
                        <%--sProducts_Name--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="sProducts_Name" Caption="Name">
                            <PropertiesTextEdit Width="300px">
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                    <RequiredField IsRequired="True" ErrorText="Please Enter Products Name" />
                                </ValidationSettings>
                            </PropertiesTextEdit>
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataTextColumn>
                        <%--sProducts_Description--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="sProducts_Description"
                            Caption="Description">
                            <EditItemTemplate>
                                <dxe:ASPxMemo ID="ASPxMemo1" runat="server" Width="300px" Height="60px" Text='<%# Bind("sProducts_Description") %>'>
                                </dxe:ASPxMemo>
                            </EditItemTemplate>
                        </dxe:GridViewDataTextColumn>
                        <%-- sProducts_Type--%>
                        <dxe:GridViewDataComboBoxColumn Visible="true" FieldName="sProducts_TypeFull" VisibleIndex="4"
                            Caption="Type">
                            <PropertiesComboBox DataSourceID="SqlSourceProductType" ValueField="sProducts_Type"
                                TextField="name" EnableIncrementalFiltering="True" EnableSynchronization="Default"
                                ValueType="System.String">
                                <ValidationSettings SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithText"
                                    ErrorTextPosition="Bottom">
                                    <RequiredField ErrorText="Please select an item" IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesComboBox>
                            <EditFormSettings Visible="True" Caption="Type" />
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataComboBoxColumn>
                        <%--ProductClassCode--%>
                        <dxe:GridViewDataComboBoxColumn FieldName="ProductClass_Code" VisibleIndex="5"
                            Caption="ProductClass Code">
                            <PropertiesComboBox DataSourceID="SqlSourceProductClass_Code" ValueField="ProductClass_ID"
                                TextField="ProductClass_Name" EnableIncrementalFiltering="True" EnableSynchronization="False"
                                ValueType="System.String">
                            </PropertiesComboBox>
                            <EditFormSettings Visible="True" Caption="ProductClass Code" />
                            <EditFormCaptionStyle HorizontalAlign="Left" VerticalAlign="Top">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataComboBoxColumn>
                        <%--sProducts_GlobalCode--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="sProducts_GlobalCode" Caption="Global Code">
                            <PropertiesTextEdit Width="300px" MaxLength="30">
                            </PropertiesTextEdit>
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataTextColumn>
                        <%--sProducts_TradingLot--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="sProducts_TradingLot" Visible="false"
                            Caption="TradingLot">
                            <PropertiesTextEdit Width="300px" MaxLength="10">
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                    <RequiredField IsRequired="True" ErrorText="Please Enter Trading Lot" />
                                    <RegularExpression ValidationExpression="^[-+]?\d+$" ErrorText="Please enter proper Trading Lot" />
                                </ValidationSettings>
                            </PropertiesTextEdit>
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <EditFormSettings Visible="True"></EditFormSettings>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataTextColumn>
                        <%--sProducts_TradingLotUnit--%>
                        <dxe:GridViewDataComboBoxColumn Visible="false" FieldName="sProducts_TradingLotUnit"
                            Width="300px" VisibleIndex="8" Caption="Trading Lot Units">
                            <PropertiesComboBox DataSourceID="SelectUOM" ValueField="UOM_ID" TextField="UOM_Name"
                                EnableIncrementalFiltering="True" EnableSynchronization="False" ValueType="System.Int32">
                                
                                <ValidationSettings SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithText"
                                    ErrorTextPosition="Bottom">
                                    <RequiredField ErrorText="Please select an item" IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesComboBox>
                            <EditFormSettings Visible="True" Caption="Trading Lot Units" />
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataComboBoxColumn>
                        <%--sProducts_QuoteCurrency--%>
                        <dxe:GridViewDataComboBoxColumn Visible="false" FieldName="sProducts_TradingLotUnit"
                            Width="300px" VisibleIndex="9" Caption="QuoteCurrency">
                            <PropertiesComboBox DataSourceID="SelectCurrency" ValueField="Currency_ID" TextField="Currency_Name"
                                EnableIncrementalFiltering="True" EnableSynchronization="False" ValueType="System.Int32">
                                  
                                <ValidationSettings SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithText"
                                    ErrorTextPosition="Bottom">
                                    <RequiredField ErrorText="Please select an item" IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesComboBox>
                            <EditFormSettings Visible="True" Caption="QuoteCurrency" />
                            <EditFormCaptionStyle HorizontalAlign="Left" VerticalAlign="Top">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataComboBoxColumn>
                        <%--sProducts_QuoteLot--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="10" FieldName="sProducts_QuoteLot" Visible="false"
                            Caption="QuoteLot">
                            <PropertiesTextEdit Width="300px" MaxLength="10">
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                    <RequiredField IsRequired="True" ErrorText="Please Enter Quote Lot" />
                                    <RegularExpression ValidationExpression="^[-+]?\d+$" ErrorText="Please enter proper Quote Lot" />
                                </ValidationSettings>
                            </PropertiesTextEdit>
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <EditFormSettings Visible="True"></EditFormSettings>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataTextColumn>
                        <%--sProducts_QuoteLotUnit--%>
                        <dxe:GridViewDataComboBoxColumn Visible="false" FieldName="sProducts_QuoteLotUnit"
                            VisibleIndex="11" Caption="QuoteLot Unit">
                            <PropertiesComboBox DataSourceID="SelectUOM" ValueField="UOM_ID" TextField="UOM_Name"
                                EnableIncrementalFiltering="True" EnableSynchronization="False" ValueType="System.Int32">
                                
                                <ValidationSettings SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithText"
                                    ErrorTextPosition="Bottom">
                                    <RequiredField ErrorText="Please select an item" IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesComboBox>
                            <EditFormSettings Visible="True" Caption="QuoteLot Unity" />
                            <EditFormCaptionStyle HorizontalAlign="Left" VerticalAlign="Top">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataComboBoxColumn>
                        <%--sProducts_DeliveryLot--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="12" FieldName="sProducts_DeliveryLot"
                            Visible="false" Caption="DeliveryLot">
                            <PropertiesTextEdit Width="300px" MaxLength="10">
                                <ValidationSettings SetFocusOnError="True" ErrorDisplayMode="ImageWithText" ErrorTextPosition="Bottom">
                                    <RequiredField IsRequired="True" ErrorText="Please Enter Delivery Lot" />
                                    <RegularExpression ValidationExpression="^[-+]?\d+$" ErrorText="Please enter proper Delivery Lot" />
                                </ValidationSettings>
                            </PropertiesTextEdit>
                            <EditCellStyle HorizontalAlign="Left" Wrap="False">
                            </EditCellStyle>
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <EditFormSettings Visible="True"></EditFormSettings>
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataTextColumn>
                        <%--sProducts_DeliveryLotUnit--%>
                        <dxe:GridViewDataComboBoxColumn Visible="false" FieldName="sProducts_DeliveryLotUnit"
                            VisibleIndex="13" Caption="Delivery Lot Unit">
                            <PropertiesComboBox DataSourceID="SelectUOM" ValueField="UOM_ID" TextField="UOM_Name"
                                EnableIncrementalFiltering="True" EnableSynchronization="False" ValueType="System.Int32">
                                
                                <ValidationSettings SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithText"
                                    ErrorTextPosition="Bottom">
                                    <RequiredField ErrorText="Please select a Delivery LotUnit" IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesComboBox>
                            <EditFormSettings Visible="True" Caption="Delivery Lot Unit" />
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataComboBoxColumn>
                        <%--sProducts_RadioColor--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="14" Visible="false">
                            <EditItemTemplate>
                                <dxe:ASPxRadioButtonList runat="server" RepeatDirection="Horizontal" ID="ASPxRadioButtonList">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){CheckColor();}" Init="function(s,e){CheckColorRadio();}" />
                                    <Items>
                                        <dxe:ListEditItem Text="Not Applicable" Value="0" />
                                        <dxe:ListEditItem Text="Applicable" Value="1" />
                                    </Items>
                                </dxe:ASPxRadioButtonList>
                            </EditItemTemplate>
                            <EditFormSettings Visible="True" />
                        </dxe:GridViewDataTextColumn>
                        <%--ProductCheckColor--%>
                        <dxe:GridViewDataComboBoxColumn Visible="false" FieldName="sProducts_Color" VisibleIndex="15"
                            Caption="Product Color">
                            <PropertiesComboBox DataSourceID="SqlSourceColorId" ValueField="Color_ID" TextField="Color_Name"
                                EnableIncrementalFiltering="True" EnableSynchronization="False" ValueType="System.Int32">
                            </PropertiesComboBox>
                            <EditFormSettings Visible="True" Caption="Product Color" />
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataComboBoxColumn>
                        <%--sProducts_Radiosize--%>
                        <dxe:GridViewDataTextColumn VisibleIndex="16" Visible="false">
                            <EditItemTemplate>
                                <dxe:ASPxRadioButtonList ID="ASPxRadioButtonListSize" runat="server" RepeatDirection="Horizontal">
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){CheckSize();}" Init="function(s,e){CheckSizeRadio();}" />
                                    <Items>
                                        <dxe:ListEditItem Text="Not Applicable" Value="0" />
                                        <dxe:ListEditItem Text="Applicable" Value="1" />
                                    </Items>
                                </dxe:ASPxRadioButtonList>
                            </EditItemTemplate>
                            <EditFormSettings Visible="True" />
                        </dxe:GridViewDataTextColumn>
                        <%--ProductChecksize--%>
                        <dxe:GridViewDataComboBoxColumn Visible="false" FieldName="sProducts_Size" VisibleIndex="17"
                            Caption="Product Size">
                            <PropertiesComboBox DataSourceID="SqlDataSourceSizeId" ValueField="Size_ID" TextField="Size_Name"
                                EnableIncrementalFiltering="True" EnableSynchronization="False" ValueType="System.Int32">
                            </PropertiesComboBox>
                            <EditFormSettings Visible="True" Caption="Product Size" />
                            <EditFormCaptionStyle Wrap="False" HorizontalAlign="Left" VerticalAlign="Top">
                            </EditFormCaptionStyle>
                        </dxe:GridViewDataComboBoxColumn>
                        <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="14" FieldName="CreateDate">
                            <EditFormSettings Visible="False"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="15" FieldName="CreateUser">
                            <EditFormSettings Visible="False"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="16" FieldName="LastModifyDate">
                            <EditFormSettings Visible="False"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="17" FieldName="LastModifyUser">
                            <EditFormSettings Visible="False"></EditFormSettings>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewCommandColumn VisibleIndex="18" ShowEditButton="True" ShowDeleteButton="True">
                            <HeaderStyle HorizontalAlign="Center"/>
                            <HeaderTemplate>
                                <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                  { %>
                                <a href="javascript:void(0);" onclick="grid.AddNewRow()">
                                    <span style="color: #000099;
                                                                        text-decoration: underline">Add New</span>
                                </a>
                                <%} %>
                            </HeaderTemplate>
                        </dxe:GridViewCommandColumn>
                        
                        
                        <dxe:GridViewDataTextColumn VisibleIndex="19" Visible="true" Caption="Document">
                            <EditFormSettings Visible="False" />
                            <HeaderTemplate>
                                <asp:Label ID="DocumentHeader" runat="server" Text="Document"></asp:Label>
                            </HeaderTemplate>
                            <DataItemTemplate>
                                <a href="#" id="DocunemtInsert" onclick="PopupOpen(<%#Eval("sProducts_ID") %>)">Document</a>
                            </DataItemTemplate>
                        </dxe:GridViewDataTextColumn>
                        
                        
                        
                    </Columns>
                    <Settings ShowStatusBar="Visible"></Settings>
                    <Styles>
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                        <Cell CssClass="gridcellleft">
                        </Cell>
                        <LoadingPanel ImageSpacing="10px">
                        </LoadingPanel>
                    </Styles>
                    <SettingsText PopupEditFormCaption="Add/Modify markets" />
                    <SettingsPager NumericButtonCount="20" PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                    <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                    <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center"
                        PopupEditFormModal="True" PopupEditFormVerticalAlign="Above" PopupEditFormWidth="450px" />
                    <Templates>
                        <EditForm>
                            <table style="width: 100%; height: 100%; padding: 5px;">
                                <tr>
                                    <%-- <td style="width: 25%">
                                    </td>--%>
                                    <td style="width: 50%">
                                        <div style="padding: 5px 0; background-color: #ccc">
                                            <controls>
                                <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors">
                                </dxe:ASPxGridViewTemplateReplacement>                                                           
                            </controls>
                                            <div style="text-align: center; padding: 2px 2px 2px 2px">
                                                <div class="dxbButton" style="display: inline-block; color: #000">
                                                    <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                        runat="server">
                                                    </dxe:ASPxGridViewTemplateReplacement>
                                                </div>
                                                <div class="dxbButton" style="display: inline-block; color: #000">
                                                    <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                        runat="server">
                                                    </dxe:ASPxGridViewTemplateReplacement>
                                                </div>
                                                <%--<div class="dxbButton" style="display: inline-block; color: #000">--%>
                                                   <input type="button" value="Assing Values" onclick="fetchLebel()">
                                                <%--</div>--%>
                                            </div>
                                        </div>
                                    </td>
                                    <%-- <td style="width: 25%">
                                    </td>--%>
                                </tr>
                            </table>
                        </EditForm>
                    </Templates>
                </dxe:ASPxGridView>
                <dxe:ASPxPopupControl runat="server" ClientInstanceName="popup" CloseAction="CloseButton"
                    ContentUrl="sProducts.aspx.cs" HeaderText="Building Master" Left="150" Top="10"
                    Width="700px" Height="700px" ID="ASPXPopupControl">
                    <ContentCollection>
                        <dxe:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        </dxe:PopupControlContentControl>
                    </ContentCollection>
                </dxe:ASPxPopupControl>
            </td>
        </tr>
    </table>
    <asp:SqlDataSource ID="SqlSourceProductClass_Code" runat="server" 
        SelectCommand="SELECT ProductClass_ID,ProductClass_Name FROM Master_ProductClass ">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlSourceProductType" runat="server"
        SelectCommand="SELECT name,sProducts_Type FROM ( SELECT 'Raw Material' AS name, 'A' AS sProducts_Type UNION SELECT 'Work-In-Process' AS name, 'B' AS sProducts_Type
         UNION SELECT 'Finished Goods' AS name, 'C' AS sProducts_Type) X order by sProducts_Type  ">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlSourceColorId" runat="server" 
        SelectCommand=" SELECT [Color_ID],[Color_Name] FROM [dbo].[Master_Color] UNION SELECT 0 AS [Color_ID],'None' AS [Color_Name] UNION SELECT NULL AS [Color_ID],'' AS [Color_Name] 
        ORDER BY [Color_ID]"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceSizeId" runat="server"
        SelectCommand="    SELECT [Size_ID],[Size_Name] FROM [dbo].[Master_Size] UNION SELECT 0 AS [Size_ID],'None' AS [Size_Name] UNION SELECT NULL AS [Size_ID],'' AS [Size_Name] ">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="markets" runat="server" ConflictDetection="CompareAllValues"
        OldValuesParameterFormatString="original_{0}" 
        DeleteCommand="DELETE FROM [Master_sProducts] WHERE [sProducts_ID] = @original_sProducts_ID"
        InsertCommand="INSERT INTO [dbo].[Master_sProducts]([sProducts_Code]  ,[sProducts_Name]
      ,[sProducts_Description]  ,[sProducts_Type] ,[ProductClass_Code],[sProducts_GlobalCode] ,[sProducts_TradingLot]
      ,[sProducts_TradingLotUnit]  ,[sProducts_QuoteCurrency]  ,[sProducts_QuoteLot]
      ,[sProducts_QuoteLotUnit] ,[sProducts_DeliveryLot]  ,[sProducts_DeliveryLotUnit],[sProducts_Color],[sProducts_Size],[sProducts_CreateUser],[sProducts_CreateTime])
      VALUES(@sProducts_Code  ,@sProducts_Name ,@sProducts_Description ,@sProducts_TypeFull,@ProductClass_Code ,@sProducts_GlobalCode
      ,@sProducts_TradingLot  ,@sProducts_TradingLotUnit  ,@sProducts_QuoteCurrency ,@sProducts_QuoteLot
      ,@sProducts_QuoteLotUnit ,@sProducts_DeliveryLot  ,@sProducts_DeliveryLotUnit,@sProducts_Color,@sProducts_Size,@CreateUser,getdate())"
        SelectCommand="
  SELECT  [sProducts_ID],[sProducts_Code],[sProducts_Name],[sProducts_Description],[sProducts_Type] 
		, CASE WHEN [sProducts_Type] ='A' THEN 'Raw Material' WHEN  [sProducts_Type] ='B' THEN 'Work-In-Process'  
		WHEN  [sProducts_Type] ='C' THEN 'Finished Goods' END AS [sProducts_TypeFull]
		,[ProductClass_Code]
      ,[sProducts_GlobalCode],[sProducts_TradingLot],[sProducts_TradingLotUnit],[sProducts_QuoteCurrency]
      ,[sProducts_QuoteLot],[sProducts_QuoteLotUnit],[sProducts_DeliveryLot],[sProducts_DeliveryLotUnit]
      ,[sProducts_Color],[sProducts_Size],[sProducts_CreateUser],[sProducts_CreateTime],[sProducts_ModifyUser] ,[sProducts_ModifyTime] FROM [dbo].[Master_sProducts]"
        UpdateCommand="Update [dbo].[Master_sProducts] set [sProducts_Code]=@sProducts_Code,[sProducts_Name]=@sProducts_Name,[sProducts_Description] =@sProducts_Description ,
        [sProducts_Type]=@sProducts_TypeFull,[ProductClass_Code] = @ProductClass_Code ,[sProducts_GlobalCode]=@sProducts_GlobalCode ,[sProducts_TradingLot]=@sProducts_TradingLot,[sProducts_TradingLotUnit]=@sProducts_TradingLotUnit,
        [sProducts_QuoteCurrency]=@sProducts_QuoteCurrency,[sProducts_QuoteLot]=@sProducts_QuoteLot,[sProducts_QuoteLotUnit]=@sProducts_QuoteLotUnit ,
        [sProducts_DeliveryLot]=@sProducts_DeliveryLot  ,[sProducts_DeliveryLotUnit]=@sProducts_DeliveryLotUnit,[sProducts_Color]=@sProducts_Color,[sProducts_Size]=@sProducts_Size,[sProducts_ModifyUser] = @CreateUser,[sProducts_ModifyTime] = getdate()
      where sProducts_ID=@sProducts_ID ">
        <DeleteParameters>
            <asp:Parameter Name="original_sProduct_ID" Type="Decimal" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="sProducts_ID" Type="Int32" />
            <asp:Parameter Name="sProducts_Code" Type="String" />
            <asp:Parameter Name="sProducts_Name" Type="String" />
            <asp:Parameter Name="sProducts_Description" Type="String" />
            <asp:Parameter Name="sProducts_TypeFull" Type="String" />
            <asp:Parameter Name="ProductClass_Code" Type="String" />
            <asp:Parameter Name="sProducts_GlobalCode" Type="String" />
            <asp:Parameter Name="sProducts_TradingLot" Type="Int32" />
            <asp:Parameter Name="sProducts_TradingLotUnit" Type="Int32" />
            <asp:Parameter Name="sProducts_QuoteCurrency" Type="Int32" />
            <asp:Parameter Name="sProducts_QuoteLot" Type="Int32" />
            <asp:Parameter Name="sProducts_QuoteLotUnit" Type="Int32" />
            <asp:Parameter Name="sProducts_DeliveryLot" Type="Int32" />
            <asp:Parameter Name="sProducts_Color" Type="Int32" />
            <asp:Parameter Name="sProducts_Size" Type="Int32" />
            <asp:Parameter Name="sProducts_DeliveryLotUnit" Type="Int32" />
            <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
            <%-- <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />--%>
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="sProducts_ID" Type="Int32" />
            <asp:Parameter Name="sProducts_Code" Type="String" />
            <asp:Parameter Name="sProducts_Name" Type="String" />
            <asp:Parameter Name="sProducts_Description" Type="String" />
            <asp:Parameter Name="sProducts_Type" Type="String" />
            <asp:Parameter Name="ProductClass_Code" Type="String" />
            <asp:Parameter Name="sProducts_GlobalCode" Type="String" />
            <asp:Parameter Name="sProducts_TradingLot" Type="Int32" />
            <asp:Parameter Name="sProducts_TradingLotUnit" Type="Int32" />
            <asp:Parameter Name="sProducts_QuoteCurrency" Type="Int32" />
            <asp:Parameter Name="sProducts_QuoteLot" Type="Int32" />
            <asp:Parameter Name="sProducts_QuoteLotUnit" Type="Int32" />
            <asp:Parameter Name="sProducts_DeliveryLot" Type="Int32" />
            <asp:Parameter Name="sProducts_Color" Type="Int32" />
            <asp:Parameter Name="sProducts_Size" Type="Int32" />
            <asp:Parameter Name="sProducts_DeliveryLotUnit" Type="Int32" />
            <asp:SessionParameter Name="CreateUser" Type="Decimal" SessionField="userid" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SelectUOM" runat="server"
        SelectCommand="select UOM_ID,UOM_Name from Master_UOM"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SelectCurrency" runat="server" 
        SelectCommand="select Currency_ID, Currency_Name  from Master_Currency"></asp:SqlDataSource>
    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>
    <br />
    
    
    <dxe:aspxpopupcontrol id="AssignValuePopup" runat="server" clientinstancename="AssignValuePopup"
    width="200px" headertext="Add / Edit Key Value" popuphorizontalalign="WindowCenter"
    backcolor="white" popupverticalalign="WindowCenter" closeaction="CloseButton"
    modal="True" contentstyle-verticalalign="Top" enablehierarchyrecreation="True"
    contentstyle-cssclass="pad">
    <ContentStyle VerticalAlign="Top" CssClass="pad">
    </ContentStyle>
    <ContentCollection>
        <dxe:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
            <div id="generatedForm">
            </div>
            <div id="Div2">
            
                <asp:TextBox ID="KeyField" runat="server" style="display:block;"></asp:TextBox>
                <asp:TextBox ID="ValueField" runat="server" style="display:block;"></asp:TextBox>
                <asp:TextBox ID="RexPageName" runat="server" style="display:block;"></asp:TextBox>
            
            
               <asp:Button ID="BTNSave" runat="server" Text="Save"  OnClientClick="return SaveDataToResource()" OnClick="BTNSaveUC_clicked" style="margin-left:155px;" />
            
            </div>
        </dxe:PopupControlContentControl>
    </ContentCollection>
    <HeaderStyle BackColor="LightGray" ForeColor="Black" />
</dxe:aspxpopupcontrol>
    
</asp:Content>
