<%@ Page Title="Leave Scheme" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Master.management_master_frm_LeaveScheme" CodeBehind="frm_LeaveScheme.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v10.2.Export, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe000001" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v10.2" namespace="DevExpress.Web" tagprefix="dx" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .dxflHALSys.dxflVATSys, 
        .dxflHALSys.dxflVATSys.dxflCaptionCell_PlasticBlue {
            width:200px !important;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function alertmessage() {
            alert('This Leave Scheme Name already Exist!');
        }
        //function height() {
        //    if (document.body.scrollHeight > 500)
        //        window.frameElement.height = document.body.scrollHeight;
        //    else
        //        window.frameElement.height = '600px';
        //    window.frameElement.widht = document.body.scrollWidht;
        //}
        function LastCall(obj) {
            //height();
        }
        function gridRowclick(s, e) {
            //alert('hi');
            $('#gridcrmCampaign').find('tr').removeClass('rowActive');
            $('.floatedBtnArea').removeClass('insideGrid');
            //$('.floatedBtnArea a .ico').css({ 'opacity': '0' });
            $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea').addClass('insideGrid');
            $(s.GetRow(e.visibleIndex)).addClass('rowActive');
            setTimeout(function () {
                //alert('delay');
                var lists = $(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a');
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a .ico').css({'opacity': '1'});
                //$(s.GetRow(e.visibleIndex)).find('.floatedBtnArea a').each(function (e) {
                //    setTimeout(function () {
                //        $(this).fadeIn();
                //    }, 100);
                //});    
                $.each(lists, function (index, value) {
                    //console.log(index);
                    //console.log(value);
                    setTimeout(function () {
                        console.log(value);
                        $(value).css({ 'opacity': '1' });
                    }, 100);
                });
            }, 200);
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Leave Scheme</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100" cellpadding="0px" cellspacing="0px">
            <%--<tr>
            <td class="EHEADER" style="text-align: center">
                <span style="color: Blue"><strong>Leave Scheme</strong></span>
            </td>
        </tr>--%>
            <tr>
                <td><%--<%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                      { %>--%>
                     <% if (rights.CanAdd)
                               { %>
                    <a id="btnAddModify" href="javascript:void(0);" onclick="grid.AddNewRow();" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span> Add New</a><%} %>
                    <%-- <%} %>--%>
                    <% if (rights.CanExport)
                                               { %>
                    <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                        <asp:ListItem Value="0">Export to</asp:ListItem>
                        <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                    </asp:DropDownList>
                    <% } %>
                </td>
                <%--<td class="gridcellright pull-right">
                    <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                        Font-Bold="False" ForeColor="black" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                        ValueType="System.Int32" Width="130px">
                        <Items>
                            <dxe:ListEditItem Text="Select" Value="0" />
                            <dxe:ListEditItem Text="PDF" Value="1" />
                            <dxe:ListEditItem Text="XLS" Value="2" />
                            <dxe:ListEditItem Text="RTF" Value="3" />
                            <dxe:ListEditItem Text="CSV" Value="4" />
                        </Items>
                        <ButtonStyle>
                        </ButtonStyle>
                        <ItemStyle>
                            <HoverStyle>
                            </HoverStyle>
                        </ItemStyle>
                        <Border BorderColor="black" />
                        <DropDownButton Text="Export">
                        </DropDownButton>
                    </dxe:ASPxComboBox>
                </td>--%>
            </tr>
            <tr>
                <td class="gridcellcenter relative" colspan="2">
                    <dxe:ASPxGridView ID="GridLeave" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                        DataSourceID="SqlDataSource1"     KeyFieldName="ls_id" Width="100%"
                        OnInitNewRow="GridLeave_InitNewRow" OnRowValidating="GridLeave_RowValidating"
                        OnHtmlEditFormCreated="GridLeave_HtmlEditFormCreated" OnHtmlRowCreated="GridLeave_HtmlRowCreated" OnCustomJSProperties="GridLeave_CustomJSProperties" OnCommandButtonInitialize="GridLeave_CommandButtonInitialize">
                        <SettingsSearchPanel Visible="True"  Delay="5000"/>
                        <Settings ShowFilterRow="true" ShowGroupPanel="true" ShowFilterRowMenu="true" />
                        <SettingsBehavior AllowFocusedRow="true" ConfirmDelete="True"></SettingsBehavior>
                        <%-- <SettingsEditing Mode="PopupEditForm" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="800px"
                            EditFormColumnCount="3" PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormModal="True">--%>

                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="WindowCenter"
                            PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="1200px"
                            EditFormColumnCount="2" PopupEditFormHeight="600px" >
                        </SettingsEditing>
                        <%-- <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>--%>
                        <SettingsText PopupEditFormCaption="Add/Edit Leave Scheme" ConfirmDelete="Confirm delete?"></SettingsText>
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="ls_id" ReadOnly="True" Visible="False" VisibleIndex="0">
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataMemoColumn FieldName="ls_name" Caption="Name Of leave" VisibleIndex="0" >
                                <PropertiesMemoEdit Columns="1">
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip" ErrorText="Mandatory!"
                                        ErrorTextPosition="Right" SetFocusOnError="True">
                                        <RequiredField ErrorText="Mandatory" IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesMemoEdit>
                                <EditFormSettings Visible="True" Caption="Name Of leave" VisibleIndex="1"></EditFormSettings>
                                <EditCellStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft" HorizontalAlign="Left" Wrap="True">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataMemoColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_TotalPrevilegeLeave" Caption="Total PL"
                                VisibleIndex="1">
                                <EditFormSettings Caption="Total PL" VisibleIndex="2"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft" HorizontalAlign="Left">
                                </CellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="ls_PLapplicablefor" Visible="False"
                                VisibleIndex="2">
                                <PropertiesComboBox EnableIncrementalFiltering="True" ValueType="System.String">
                                    <Items>
                                        <dxe:ListEditItem Text="Financial Year " Value="F"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Calendar Year" Value="C"></dxe:ListEditItem>
                                    </Items>
                                    <ItemStyle Wrap="True"></ItemStyle>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="PL applicable for" VisibleIndex="3"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="ls_PLCalculationBasis" Visible="False"
                                VisibleIndex="2">
                                <PropertiesComboBox EnableIncrementalFiltering="True" ValueType="System.String">
                                    <Items>
                                        <dxe:ListEditItem Text="Pro-rata" Value="P"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Full" Value="F"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="PL Calculation basis" VisibleIndex="4"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_PLaccumulation_days" Visible="False"
                                VisibleIndex="2">
                                <EditFormSettings Visible="True" Caption="PL accumulation rate(days)" VisibleIndex="5"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="0px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="ls_PLaccumulation_M_Y" Visible="False"
                                VisibleIndex="2">
                                <PropertiesComboBox EnableIncrementalFiltering="True" ValueType="System.String">
                                    <Items>
                                        <dxe:ListEditItem Text="Month" Value="M"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Year" Value="Y"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption=" " VisibleIndex="6"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="0px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_PLentitlement" Visible="False" VisibleIndex="2">
                                <EditFormSettings Visible="True" Caption="PL Entitlement eligibility(Months)" VisibleIndex="7"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataCheckColumn FieldName="ls_PLencashed" Visible="False" VisibleIndex="2">
                                <PropertiesCheckEdit ValueType="System.Char" ValueChecked="Y" ValueUnchecked="N">
                                </PropertiesCheckEdit>
                                <EditFormSettings Visible="True" Caption="PL can be encashed?" VisibleIndex="8"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_PLencashedEligibility" Visible="False"
                                VisibleIndex="2">
                                <EditFormSettings Visible="True" Caption="PL encashment eligibility(/Month)" VisibleIndex="9"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataCheckColumn FieldName="ls_PLaccumulatedCFNextYear" Visible="False"
                                VisibleIndex="2">
                                <PropertiesCheckEdit ValueType="System.Char" ValueChecked="Y" ValueUnchecked="N">
                                </PropertiesCheckEdit>
                                <EditFormSettings Visible="True" Caption="PL can be C/F ?" VisibleIndex="10"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_PLaccumulatedMax" Visible="False" VisibleIndex="2">
                                <EditFormSettings Visible="True" Caption="maximum permissible PL(days)" VisibleIndex="11"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_PLinstallments" Visible="False" VisibleIndex="2">
                                <EditFormSettings Visible="True" Caption="PL installments(In Fin.Yr.) " VisibleIndex="12"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_PLMinDayPerInstallments" Visible="False"
                                VisibleIndex="2">
                                <EditFormSettings Visible="True" Caption="Min. no. of days/ installment" VisibleIndex="13"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="ls_PLcount_PSWO_PH" Visible="False"
                                VisibleIndex="2">
                                <PropertiesComboBox EnableIncrementalFiltering="True" ValueType="System.String">
                                    <Items>
                                        <dxe:ListEditItem Text="Yes" Value="Y"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Only if both exist" Value="O"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="No" Value="N"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Preceding/Succeeding Weekly Off/Paid Holidays to be counted as PL"
                                    VisibleIndex="14"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_PLaccountMindayForEncashment" Visible="False"
                                VisibleIndex="14">
                                <EditFormSettings Visible="True" Caption="Minimum number of days to be maintained in the PL a/c (in case of encashment)"
                                    VisibleIndex="15"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_CLtotal" Caption="Total CL " VisibleIndex="2">
                                <EditFormSettings Visible="True" Caption="Total Casual Leave " VisibleIndex="16"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft" HorizontalAlign="Left">
                                </CellStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="ls_CLapplicablefor" Visible="False"
                                VisibleIndex="3">
                                <PropertiesComboBox EnableIncrementalFiltering="True" ValueType="System.String">
                                    <Items>
                                        <dxe:ListEditItem Text="Financial Year " Value="F"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Calendar Year" Value="C"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Caption="CL applicable for" Visible="True" VisibleIndex="17" />
                                <EditCellStyle BackColor="#FFF2C8" HorizontalAlign="Left">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <EditFormCaptionStyle BackColor="#FFF2C8" HorizontalAlign="Left" Wrap="True">
                                    <Border BorderColor="Blue" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="ls_CLCalculationBasis" Visible="False"
                                VisibleIndex="2">
                                <PropertiesComboBox EnableIncrementalFiltering="True" ValueType="System.String">
                                    <Items>
                                        <dxe:ListEditItem Text="Pro-rata" Value="P"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Full" Value="F"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="CL Calculation basis" VisibleIndex="18"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_CLentitlement" Visible="False" VisibleIndex="19">
                                <EditFormSettings Visible="True" Caption="CL Entitlement eligibility: completion of Month"
                                    VisibleIndex="19"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataCheckColumn FieldName="ls_CLencashed" Visible="False" VisibleIndex="2">
                                <PropertiesCheckEdit ValueType="System.Char" ValueChecked="Y" ValueUnchecked="N">
                                </PropertiesCheckEdit>
                                <EditFormSettings Visible="True" Caption="CL can be encashed? " VisibleIndex="20"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_CLencashedEligibility" Visible="False"
                                VisibleIndex="2">
                                <EditFormSettings Visible="True" Caption="CL encashment eligibility(Months)" VisibleIndex="21"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataCheckColumn FieldName="ls_CLaccumulatedCFNextYear" Visible="False"
                                VisibleIndex="2">
                                <PropertiesCheckEdit ValueType="System.Char" ValueChecked="Y" ValueUnchecked="N">
                                </PropertiesCheckEdit>
                                <EditFormSettings Visible="True" Caption="CL can be C/F to the next FinYear" VisibleIndex="22"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_CLaccumulatedMax" Visible="False" VisibleIndex="23">
                                <EditFormSettings Visible="True" Caption="Max. permissible CL a/c balance(days)"
                                    VisibleIndex="23"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_CLMaxDayPerInstallments" Visible="False"
                                VisibleIndex="23">
                                <EditFormSettings Visible="True" Caption="Max. number of CL/installment(days)" VisibleIndex="24"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="ls_CLcount_PSWO_PH" Visible="False"
                                VisibleIndex="2">
                                <PropertiesComboBox EnableIncrementalFiltering="True" ValueType="System.String">
                                    <Items>
                                        <dxe:ListEditItem Text="Yes" Value="Y"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Only if both exist" Value="O"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="No" Value="N"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Preceding/Succeeding Weekly Off/Paid Holidays to be counted as CL"
                                    VisibleIndex="25"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_SLtotal" Caption="Total SL" VisibleIndex="3">
                                <EditFormSettings Visible="True" Caption="Total Sick Leave " VisibleIndex="26"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <CellStyle CssClass="gridcellleft" HorizontalAlign="Left">
                                </CellStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="ls_SLapplicablefor" Visible="False"
                                VisibleIndex="21">
                                <PropertiesComboBox EnableIncrementalFiltering="True" ValueType="System.String">
                                    <Items>
                                        <dxe:ListEditItem Text="Financial Year " Value="F"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Calendar Year" Value="C"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="SL applicable for" VisibleIndex="27"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="ls_SLCalculationBasis" Visible="False"
                                VisibleIndex="2">
                                <PropertiesComboBox EnableIncrementalFiltering="True" ValueType="System.String">
                                    <Items>
                                        <dxe:ListEditItem Text="Pro-rata " Value="P"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Full" Value="F"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="SL Calculation basis" VisibleIndex="28"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_SLentitlement" Visible="False" VisibleIndex="2">
                                <EditFormSettings Visible="True" Caption="SL Entitlement eligibility: completion of months"
                                    VisibleIndex="29"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataCheckColumn FieldName="ls_SLencashed" Visible="False" VisibleIndex="3">
                                <PropertiesCheckEdit ValueType="System.Char" ValueChecked="Y" ValueUnchecked="N">
                                </PropertiesCheckEdit>
                                <EditFormSettings Visible="True" Caption="SL can be encashed? " VisibleIndex="30"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="False" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_SLencashedEligibility" Visible="False"
                                VisibleIndex="17">
                                <EditFormSettings Visible="True" Caption="SL encashment eligibility(months)" VisibleIndex="31"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataCheckColumn FieldName="ls_SLaccumulatedCFNextYear" Visible="False"
                                VisibleIndex="16">
                                <PropertiesCheckEdit ValueType="System.Char" ValueChecked="Y" ValueUnchecked="N">
                                </PropertiesCheckEdit>
                                <EditFormSettings Visible="True" Caption="Accumulated SL can be C/F to the next period? "
                                    VisibleIndex="32"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_SLaccumulatedMax" Visible="False" VisibleIndex="15">
                                <EditFormSettings Visible="True" Caption="maximum permissible SL a/c balance(days)"
                                    VisibleIndex="33"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_SLMaxDayPerInstallments" Visible="False"
                                VisibleIndex="3">
                                <EditFormSettings Visible="True" Caption="Maximum number of SL per installment(days)"
                                    VisibleIndex="34"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataComboBoxColumn FieldName="ls_SLcount_PSWO_PH" Visible="False"
                                VisibleIndex="4">
                                <PropertiesComboBox EnableIncrementalFiltering="True" ValueType="System.String">
                                    <Items>
                                        <dxe:ListEditItem Text="Yes" Value="Y"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="Only if both exist" Value="O"></dxe:ListEditItem>
                                        <dxe:ListEditItem Text="No" Value="N"></dxe:ListEditItem>
                                    </Items>
                                </PropertiesComboBox>
                                <EditFormSettings Visible="True" Caption="Preceding/Succeeding Weekly Off/Paid Holidays to be counted as SL"
                                    VisibleIndex="35"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataComboBoxColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_MLtotalPre" Visible="False" VisibleIndex="4">
                                <EditFormSettings Visible="True" Caption="Total Maternity Leave(ML) (pre-delivery period)(in days)"
                                    VisibleIndex="36"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_MLtotalPos" Visible="False" VisibleIndex="4">
                                <EditFormSettings Visible="True" Caption="Total Maternity Leave(ML) (post-delivery period)(days)"
                                    VisibleIndex="37"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="ls_MLeligibility" Visible="False" VisibleIndex="4">
                                <EditFormSettings Visible="True" Caption="ML Entitlement eligibility: completion of months"
                                    VisibleIndex="38"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataCheckColumn FieldName="ls_MLisPreWpostAdjustment" Visible="False"
                                VisibleIndex="4">
                                <PropertiesCheckEdit ValueType="System.Char" ValueChecked="Y" ValueUnchecked="N">
                                </PropertiesCheckEdit>
                                <EditFormSettings Visible="True" Caption="Is pre-delivery ML adjustable with post-delivery ML? "
                                    VisibleIndex="39"></EditFormSettings>
                                <EditFormCaptionStyle HorizontalAlign="Left" Wrap="True" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderLeft BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditFormCaptionStyle>
                                <EditCellStyle HorizontalAlign="Left" BackColor="#FFF2C8">
                                    <Border BorderColor="Blue" />
                                    <BorderTop BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderBottom BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                    <BorderRight BorderColor="Blue" BorderStyle="Solid" BorderWidth="1px" />
                                </EditCellStyle>
                                <Settings AllowAutoFilterTextInputTimer="False" />
                            </dxe:GridViewDataCheckColumn>
                            <dxe:GridViewCommandColumn Width="6%" VisibleIndex="4" ShowEditButton="true" ShowDeleteButton="true" CellStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                <%--<EditButton Visible="True">
                            </EditButton>--%>
                                <HeaderTemplate>
                                    <span>Actions</span>
                                </HeaderTemplate>

                                <%--<DeleteButton Visible="True">
                            </DeleteButton>--%>
                                <%-- <ClearFilterButton Visible="True">
                            </ClearFilterButton>--%>
                              
                            </dxe:GridViewCommandColumn>
                        </Columns>
<SettingsContextMenu Enabled="true"></SettingsContextMenu>
                        <SettingsCommandButton>
                            <DeleteButton ButtonType="Image" Image-Url="../../../assests/images/Delete.png" Image-AlternateText="Delete"></DeleteButton>
                            <EditButton Image-Url="../../../assests/images/Edit.png" ButtonType="Image" Image-AlternateText="Edit" Styles-Style-CssClass="pad"></EditButton>
                            <UpdateButton Text="Save" ButtonType="Button" Styles-Style-CssClass="btn btn-primary"></UpdateButton>
                            <CancelButton Text="Cancel" ButtonType="Button" Styles-Style-CssClass="btn btn-danger"></CancelButton>
                        </SettingsCommandButton>
                        <%--<SettingsPager PageSize="20" AlwaysShowPager="True" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>--%>
                        <Templates>
                            <EditForm>
                                <table style="width: 100%">
                                    <tr>
                                        
                                        <td>

                                            <dxe:ASPxGridViewTemplateReplacement runat="server" ReplacementType="EditFormEditors" ColumnID="" ID="Editors"></dxe:ASPxGridViewTemplateReplacement>

                                            <div style="text-align: right; padding: 2px 2px 2px 2px">
                                                <dxe:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                                <dxe:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                                                    runat="server"></dxe:ASPxGridViewTemplateReplacement>
                                            </div>
                                        </td>
                                        
                                    </tr>
                                </table>
                            </EditForm>
                        </Templates>
                        <ClientSideEvents EndCallback="function(s, e) {
	LastCall(s.cpHeight);
}" />
                    </dxe:ASPxGridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConflictDetection="CompareAllValues"
                        DeleteCommand="DELETE FROM [tbl_master_LeaveScheme] WHERE [ls_id] = @original_ls_id "
                        InsertCommand="INSERT INTO [tbl_master_LeaveScheme] ([ls_name], [ls_TotalPrevilegeLeave], [ls_PLapplicablefor], [ls_PLCalculationBasis], [ls_PLaccumulation_days], [ls_PLaccumulation_M_Y], [ls_PLentitlement], [ls_PLencashed], [ls_PLencashedEligibility], [ls_PLaccumulatedCFNextYear], [ls_PLaccumulatedMax], [ls_PLinstallments], [ls_PLMinDayPerInstallments], [ls_PLcount_PSWO_PH], [ls_CLCalculationBasis], [ls_CLapplicablefor], [ls_CLtotal], [ls_PLaccountMindayForEncashment], [ls_CLentitlement], [ls_CLencashed], [ls_CLencashedEligibility], [ls_CLaccumulatedCFNextYear], [ls_SLtotal], [ls_CLcount_PSWO_PH], [ls_CLMaxDayPerInstallments], [ls_CLaccumulatedMax], [ls_SLapplicablefor], [ls_SLCalculationBasis], [ls_SLentitlement], [ls_SLencashed], [ls_SLMaxDayPerInstallments], [ls_SLaccumulatedMax], [ls_SLaccumulatedCFNextYear], [ls_SLencashedEligibility], [ls_SLcount_PSWO_PH], [ls_MLtotalPre], [ls_MLtotalPos], [ls_MLeligibility], [ls_MLisPreWpostAdjustment], [CreateDate], [CreateUser]) VALUES (@ls_name, @ls_TotalPrevilegeLeave, @ls_PLapplicablefor, @ls_PLCalculationBasis, @ls_PLaccumulation_days, @ls_PLaccumulation_M_Y, @ls_PLentitlement, @ls_PLencashed, @ls_PLencashedEligibility, @ls_PLaccumulatedCFNextYear, @ls_PLaccumulatedMax, @ls_PLinstallments, @ls_PLMinDayPerInstallments, @ls_PLcount_PSWO_PH, @ls_CLCalculationBasis, @ls_CLapplicablefor, @ls_CLtotal, @ls_PLaccountMindayForEncashment, @ls_CLentitlement, @ls_CLencashed, @ls_CLencashedEligibility, @ls_CLaccumulatedCFNextYear, @ls_SLtotal, @ls_CLcount_PSWO_PH, @ls_CLMaxDayPerInstallments, @ls_CLaccumulatedMax, @ls_SLapplicablefor, @ls_SLCalculationBasis, @ls_SLentitlement, @ls_SLencashed, @ls_SLMaxDayPerInstallments, @ls_SLaccumulatedMax, @ls_SLaccumulatedCFNextYear, @ls_SLencashedEligibility, @ls_SLcount_PSWO_PH, @ls_MLtotalPre, @ls_MLtotalPos, @ls_MLeligibility, @ls_MLisPreWpostAdjustment, getdate(), @CreateUser)"
                        OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT [ls_id], [ls_name], [ls_TotalPrevilegeLeave], [ls_PLapplicablefor], [ls_PLCalculationBasis], [ls_PLaccumulation_days], [ls_PLaccumulation_M_Y], [ls_PLentitlement], [ls_PLencashed], [ls_PLencashedEligibility], [ls_PLaccumulatedCFNextYear], [ls_PLaccumulatedMax], [ls_PLinstallments], [ls_PLMinDayPerInstallments], [ls_PLcount_PSWO_PH], [ls_CLCalculationBasis], [ls_CLapplicablefor], [ls_CLtotal], [ls_PLaccountMindayForEncashment], [ls_CLentitlement], [ls_CLencashed], [ls_CLencashedEligibility], [ls_CLaccumulatedCFNextYear], [ls_SLtotal], [ls_CLcount_PSWO_PH], [ls_CLMaxDayPerInstallments], [ls_CLaccumulatedMax], [ls_SLapplicablefor], [ls_SLCalculationBasis], [ls_SLentitlement], [ls_SLencashed], [ls_SLMaxDayPerInstallments], [ls_SLaccumulatedMax], [ls_SLaccumulatedCFNextYear], [ls_SLencashedEligibility], [ls_SLcount_PSWO_PH], [ls_MLtotalPre], [ls_MLtotalPos], [ls_MLeligibility], [ls_MLisPreWpostAdjustment], [CreateDate], [CreateUser], [LastModifyDate], [LastModifyUser] FROM [tbl_master_LeaveScheme] ORDER BY [CreateDate] DESC"
                        UpdateCommand="UPDATE [tbl_master_LeaveScheme] SET [ls_name] = @ls_name, [ls_TotalPrevilegeLeave] = @ls_TotalPrevilegeLeave, [ls_PLapplicablefor] = @ls_PLapplicablefor, [ls_PLCalculationBasis] = @ls_PLCalculationBasis, [ls_PLaccumulation_days] = @ls_PLaccumulation_days, [ls_PLaccumulation_M_Y] = @ls_PLaccumulation_M_Y, [ls_PLentitlement] = @ls_PLentitlement, [ls_PLencashed] = @ls_PLencashed, [ls_PLencashedEligibility] = @ls_PLencashedEligibility, [ls_PLaccumulatedCFNextYear] = @ls_PLaccumulatedCFNextYear, [ls_PLaccumulatedMax] = @ls_PLaccumulatedMax, [ls_PLinstallments] = @ls_PLinstallments, [ls_PLMinDayPerInstallments] = @ls_PLMinDayPerInstallments, [ls_PLcount_PSWO_PH] = @ls_PLcount_PSWO_PH, [ls_CLCalculationBasis] = @ls_CLCalculationBasis, [ls_CLapplicablefor] = @ls_CLapplicablefor, [ls_CLtotal] = @ls_CLtotal, [ls_PLaccountMindayForEncashment] = @ls_PLaccountMindayForEncashment, [ls_CLentitlement] = @ls_CLentitlement, [ls_CLencashed] = @ls_CLencashed, [ls_CLencashedEligibility] = @ls_CLencashedEligibility, [ls_CLaccumulatedCFNextYear] = @ls_CLaccumulatedCFNextYear, [ls_SLtotal] = @ls_SLtotal, [ls_CLcount_PSWO_PH] = @ls_CLcount_PSWO_PH, [ls_CLMaxDayPerInstallments] = @ls_CLMaxDayPerInstallments, [ls_CLaccumulatedMax] = @ls_CLaccumulatedMax, [ls_SLapplicablefor] = @ls_SLapplicablefor, [ls_SLCalculationBasis] = @ls_SLCalculationBasis, [ls_SLentitlement] = @ls_SLentitlement, [ls_SLencashed] = @ls_SLencashed, [ls_SLMaxDayPerInstallments] = @ls_SLMaxDayPerInstallments, [ls_SLaccumulatedMax] = @ls_SLaccumulatedMax, [ls_SLaccumulatedCFNextYear] = @ls_SLaccumulatedCFNextYear, [ls_SLencashedEligibility] = @ls_SLencashedEligibility, [ls_SLcount_PSWO_PH] = @ls_SLcount_PSWO_PH, [ls_MLtotalPre] = @ls_MLtotalPre, [ls_MLtotalPos] = @ls_MLtotalPos, [ls_MLeligibility] = @ls_MLeligibility, [ls_MLisPreWpostAdjustment] = @ls_MLisPreWpostAdjustment,[LastModifyDate] = getdate(), [LastModifyUser] = @LastModifyUser WHERE [ls_id] = @original_ls_id ">
                        <DeleteParameters>
                            <asp:Parameter Name="original_ls_id" Type="Int32" />
                            <asp:Parameter Name="original_ls_name" Type="String" />
                            <asp:Parameter Name="original_ls_TotalPrevilegeLeave" Type="Int32" />
                            <asp:Parameter Name="original_ls_PLapplicablefor" Type="String" />
                            <asp:Parameter Name="original_ls_PLCalculationBasis" Type="String" />
                            <asp:Parameter Name="original_ls_PLaccumulation_days" Type="decimal" />
                            <asp:Parameter Name="original_ls_PLaccumulation_M_Y" Type="String" />
                            <asp:Parameter Name="original_ls_PLentitlement" Type="Int32" />
                            <asp:Parameter Name="original_ls_PLencashed" Type="String" />
                            <asp:Parameter Name="original_ls_PLencashedEligibility" Type="Int32" />
                            <asp:Parameter Name="original_ls_PLaccumulatedCFNextYear" Type="String" />
                            <asp:Parameter Name="original_ls_PLaccumulatedMax" Type="Int32" />
                            <asp:Parameter Name="original_ls_PLinstallments" Type="Int32" />
                            <asp:Parameter Name="original_ls_PLMinDayPerInstallments" Type="Int32" />
                            <asp:Parameter Name="original_ls_PLcount_PSWO_PH" Type="String" />
                            <asp:Parameter Name="original_ls_CLCalculationBasis" Type="String" />
                            <asp:Parameter Name="original_ls_CLapplicablefor" Type="String" />
                            <asp:Parameter Name="original_ls_CLtotal" Type="Int32" />
                            <asp:Parameter Name="original_ls_PLaccountMindayForEncashment" Type="Int32" />
                            <asp:Parameter Name="original_ls_CLentitlement" Type="Int32" />
                            <asp:Parameter Name="original_ls_CLencashed" Type="String" />
                            <asp:Parameter Name="original_ls_CLencashedEligibility" Type="Int32" />
                            <asp:Parameter Name="original_ls_CLaccumulatedCFNextYear" Type="String" />
                            <asp:Parameter Name="original_ls_SLtotal" Type="Int32" />
                            <asp:Parameter Name="original_ls_CLcount_PSWO_PH" Type="String" />
                            <asp:Parameter Name="original_ls_CLMaxDayPerInstallments" Type="Int32" />
                            <asp:Parameter Name="original_ls_CLaccumulatedMax" Type="Int32" />
                            <asp:Parameter Name="original_ls_SLapplicablefor" Type="String" />
                            <asp:Parameter Name="original_ls_SLCalculationBasis" Type="String" />
                            <asp:Parameter Name="original_ls_SLentitlement" Type="Int32" />
                            <asp:Parameter Name="original_ls_SLencashed" Type="String" />
                            <asp:Parameter Name="original_ls_SLMaxDayPerInstallments" Type="Int32" />
                            <asp:Parameter Name="original_ls_SLaccumulatedMax" Type="Int32" />
                            <asp:Parameter Name="original_ls_SLaccumulatedCFNextYear" Type="String" />
                            <asp:Parameter Name="original_ls_SLencashedEligibility" Type="Int32" />
                            <asp:Parameter Name="original_ls_SLcount_PSWO_PH" Type="String" />
                            <asp:Parameter Name="original_ls_MLtotalPre" Type="Int32" />
                            <asp:Parameter Name="original_ls_MLtotalPos" Type="Int32" />
                            <asp:Parameter Name="original_ls_MLeligibility" Type="Int32" />
                            <asp:Parameter Name="original_ls_MLisPreWpostAdjustment" Type="String" />
                            <asp:Parameter Name="original_CreateDate" Type="DateTime" />
                            <asp:Parameter Name="original_CreateUser" Type="Int32" />
                            <asp:Parameter Name="original_LastModifyDate" Type="DateTime" />
                            <asp:Parameter Name="original_LastModifyUser" Type="Int32" />
                        </DeleteParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="ls_name" Type="String" />
                            <asp:Parameter Name="ls_TotalPrevilegeLeave" Type="Int32" />
                            <asp:Parameter Name="ls_PLapplicablefor" Type="String" />
                            <asp:Parameter Name="ls_PLCalculationBasis" Type="String" />
                            <asp:Parameter Name="ls_PLaccumulation_days" Type="decimal" />
                            <asp:Parameter Name="ls_PLaccumulation_M_Y" Type="String" />
                            <asp:Parameter Name="ls_PLentitlement" Type="Int32" />
                            <asp:Parameter Name="ls_PLencashed" Type="String" />
                            <asp:Parameter Name="ls_PLencashedEligibility" Type="Int32" />
                            <asp:Parameter Name="ls_PLaccumulatedCFNextYear" Type="String" />
                            <asp:Parameter Name="ls_PLaccumulatedMax" Type="Int32" />
                            <asp:Parameter Name="ls_PLinstallments" Type="Int32" />
                            <asp:Parameter Name="ls_PLMinDayPerInstallments" Type="Int32" />
                            <asp:Parameter Name="ls_PLcount_PSWO_PH" Type="String" />
                            <asp:Parameter Name="ls_CLCalculationBasis" Type="String" />
                            <asp:Parameter Name="ls_CLapplicablefor" Type="String" />
                            <asp:Parameter Name="ls_CLtotal" Type="Int32" />
                            <asp:Parameter Name="ls_PLaccountMindayForEncashment" Type="Int32" />
                            <asp:Parameter Name="ls_CLentitlement" Type="Int32" />
                            <asp:Parameter Name="ls_CLencashed" Type="String" />
                            <asp:Parameter Name="ls_CLencashedEligibility" Type="Int32" />
                            <asp:Parameter Name="ls_CLaccumulatedCFNextYear" Type="String" />
                            <asp:Parameter Name="ls_SLtotal" Type="Int32" />
                            <asp:Parameter Name="ls_CLcount_PSWO_PH" Type="String" />
                            <asp:Parameter Name="ls_CLMaxDayPerInstallments" Type="Int32" />
                            <asp:Parameter Name="ls_CLaccumulatedMax" Type="Int32" />
                            <asp:Parameter Name="ls_SLapplicablefor" Type="String" />
                            <asp:Parameter Name="ls_SLCalculationBasis" Type="String" />
                            <asp:Parameter Name="ls_SLentitlement" Type="Int32" />
                            <asp:Parameter Name="ls_SLencashed" Type="String" />
                            <asp:Parameter Name="ls_SLMaxDayPerInstallments" Type="Int32" />
                            <asp:Parameter Name="ls_SLaccumulatedMax" Type="Int32" />
                            <asp:Parameter Name="ls_SLaccumulatedCFNextYear" Type="String" />
                            <asp:Parameter Name="ls_SLencashedEligibility" Type="Int32" />
                            <asp:Parameter Name="ls_SLcount_PSWO_PH" Type="String" />
                            <asp:Parameter Name="ls_MLtotalPre" Type="Int32" />
                            <asp:Parameter Name="ls_MLtotalPos" Type="Int32" />
                            <asp:Parameter Name="ls_MLeligibility" Type="Int32" />
                            <asp:Parameter Name="ls_MLisPreWpostAdjustment" Type="String" />
                            <asp:Parameter Name="CreateDate" Type="DateTime" />
                            <asp:Parameter Name="CreateUser" Type="Int32" />
                            <asp:Parameter Name="LastModifyDate" Type="DateTime" />
                            <%--<asp:Parameter Name="LastModifyUser" Type="Int32" />--%>
                            <asp:SessionParameter Name="LastModifyUser" SessionField="userid" Type="int32" />
                            <asp:Parameter Name="original_ls_id" Type="Int32" />
                            <asp:Parameter Name="original_ls_name" Type="String" />
                            <asp:Parameter Name="original_ls_TotalPrevilegeLeave" Type="Int32" />
                            <asp:Parameter Name="original_ls_PLapplicablefor" Type="String" />
                            <asp:Parameter Name="original_ls_PLCalculationBasis" Type="String" />
                            <asp:Parameter Name="original_ls_PLaccumulation_days" Type="decimal" />
                            <asp:Parameter Name="original_ls_PLaccumulation_M_Y" Type="String" />
                            <asp:Parameter Name="original_ls_PLentitlement" Type="Int32" />
                            <asp:Parameter Name="original_ls_PLencashed" Type="String" />
                            <asp:Parameter Name="original_ls_PLencashedEligibility" Type="Int32" />
                            <asp:Parameter Name="original_ls_PLaccumulatedCFNextYear" Type="String" />
                            <asp:Parameter Name="original_ls_PLaccumulatedMax" Type="Int32" />
                            <asp:Parameter Name="original_ls_PLinstallments" Type="Int32" />
                            <asp:Parameter Name="original_ls_PLMinDayPerInstallments" Type="Int32" />
                            <asp:Parameter Name="original_ls_PLcount_PSWO_PH" Type="String" />
                            <asp:Parameter Name="original_ls_CLCalculationBasis" Type="String" />
                            <asp:Parameter Name="original_ls_CLapplicablefor" Type="String" />
                            <asp:Parameter Name="original_ls_CLtotal" Type="Int32" />
                            <asp:Parameter Name="original_ls_PLaccountMindayForEncashment" Type="Int32" />
                            <asp:Parameter Name="original_ls_CLentitlement" Type="Int32" />
                            <asp:Parameter Name="original_ls_CLencashed" Type="String" />
                            <asp:Parameter Name="original_ls_CLencashedEligibility" Type="Int32" />
                            <asp:Parameter Name="original_ls_CLaccumulatedCFNextYear" Type="String" />
                            <asp:Parameter Name="original_ls_SLtotal" Type="Int32" />
                            <asp:Parameter Name="original_ls_CLcount_PSWO_PH" Type="String" />
                            <asp:Parameter Name="original_ls_CLMaxDayPerInstallments" Type="Int32" />
                            <asp:Parameter Name="original_ls_CLaccumulatedMax" Type="Int32" />
                            <asp:Parameter Name="original_ls_SLapplicablefor" Type="String" />
                            <asp:Parameter Name="original_ls_SLCalculationBasis" Type="String" />
                            <asp:Parameter Name="original_ls_SLentitlement" Type="Int32" />
                            <asp:Parameter Name="original_ls_SLencashed" Type="String" />
                            <asp:Parameter Name="original_ls_SLMaxDayPerInstallments" Type="Int32" />
                            <asp:Parameter Name="original_ls_SLaccumulatedMax" Type="Int32" />
                            <asp:Parameter Name="original_ls_SLaccumulatedCFNextYear" Type="String" />
                            <asp:Parameter Name="original_ls_SLencashedEligibility" Type="Int32" />
                            <asp:Parameter Name="original_ls_SLcount_PSWO_PH" Type="String" />
                            <asp:Parameter Name="original_ls_MLtotalPre" Type="Int32" />
                            <asp:Parameter Name="original_ls_MLtotalPos" Type="Int32" />
                            <asp:Parameter Name="original_ls_MLeligibility" Type="Int32" />
                            <asp:Parameter Name="original_ls_MLisPreWpostAdjustment" Type="String" />
                            <asp:Parameter Name="original_CreateDate" Type="DateTime" />
                            <asp:Parameter Name="original_CreateUser" Type="Int32" />
                            <asp:Parameter Name="original_LastModifyDate" Type="DateTime" />
                            <%--<asp:Parameter Name="" Type="Int32" />--%>
                            <asp:SessionParameter Name="original_LastModifyUser" SessionField="userid" Type="int32" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:Parameter Name="ls_name" Type="String" />
                            <asp:Parameter Name="ls_TotalPrevilegeLeave" Type="Int32" />
                            <asp:Parameter Name="ls_PLapplicablefor" Type="String" />
                            <asp:Parameter Name="ls_PLCalculationBasis" Type="String" />
                            <asp:Parameter Name="ls_PLaccumulation_days" Type="decimal" />
                            <asp:Parameter Name="ls_PLaccumulation_M_Y" Type="String" />
                            <asp:Parameter Name="ls_PLentitlement" Type="Int32" />
                            <asp:Parameter Name="ls_PLencashed" Type="String" />
                            <asp:Parameter Name="ls_PLencashedEligibility" Type="Int32" />
                            <asp:Parameter Name="ls_PLaccumulatedCFNextYear" Type="String" />
                            <asp:Parameter Name="ls_PLaccumulatedMax" Type="Int32" />
                            <asp:Parameter Name="ls_PLinstallments" Type="Int32" />
                            <asp:Parameter Name="ls_PLMinDayPerInstallments" Type="Int32" />
                            <asp:Parameter Name="ls_PLcount_PSWO_PH" Type="String" />
                            <asp:Parameter Name="ls_CLCalculationBasis" Type="String" />
                            <asp:Parameter Name="ls_CLapplicablefor" Type="String" />
                            <asp:Parameter Name="ls_CLtotal" Type="Int32" />
                            <asp:Parameter Name="ls_PLaccountMindayForEncashment" Type="Int32" />
                            <asp:Parameter Name="ls_CLentitlement" Type="Int32" />
                            <asp:Parameter Name="ls_CLencashed" Type="String" />
                            <asp:Parameter Name="ls_CLencashedEligibility" Type="Int32" />
                            <asp:Parameter Name="ls_CLaccumulatedCFNextYear" Type="String" />
                            <asp:Parameter Name="ls_SLtotal" Type="Int32" />
                            <asp:Parameter Name="ls_CLcount_PSWO_PH" Type="String" />
                            <asp:Parameter Name="ls_CLMaxDayPerInstallments" Type="Int32" />
                            <asp:Parameter Name="ls_CLaccumulatedMax" Type="Int32" />
                            <asp:Parameter Name="ls_SLapplicablefor" Type="String" />
                            <asp:Parameter Name="ls_SLCalculationBasis" Type="String" />
                            <asp:Parameter Name="ls_SLentitlement" Type="Int32" />
                            <asp:Parameter Name="ls_SLencashed" Type="String" />
                            <asp:Parameter Name="ls_SLMaxDayPerInstallments" Type="Int32" />
                            <asp:Parameter Name="ls_SLaccumulatedMax" Type="Int32" />
                            <asp:Parameter Name="ls_SLaccumulatedCFNextYear" Type="String" />
                            <asp:Parameter Name="ls_SLencashedEligibility" Type="Int32" />
                            <asp:Parameter Name="ls_SLcount_PSWO_PH" Type="String" />
                            <asp:Parameter Name="ls_MLtotalPre" Type="Int32" />
                            <asp:Parameter Name="ls_MLtotalPos" Type="Int32" />
                            <asp:Parameter Name="ls_MLeligibility" Type="Int32" />
                            <asp:Parameter Name="ls_MLisPreWpostAdjustment" Type="String" />
                            <asp:Parameter Name="CreateDate" Type="DateTime" />
                            <%--<asp:Parameter Name="" Type="Int32" />--%>
                            <asp:SessionParameter Name="CreateUser" SessionField="userid" Type="int32" />
                            <asp:Parameter Name="LastModifyDate" Type="DateTime" />
                            <asp:Parameter Name="LastModifyUser" Type="Int32" />
                        </InsertParameters>
                    </asp:SqlDataSource>
                    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                    </dxe:ASPxGridViewExporter>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
