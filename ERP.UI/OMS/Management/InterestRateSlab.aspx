<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_InterestRateSlab" CodeBehind="InterestRateSlab.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>--%>
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script language="javascript" type="text/javascript">

        function OnAddButtonClick() {
            var url = 'InterestRateSlabPopup.aspx?id=' + 'ADD';
            OnMoreInfoClick(url, "Add Interest Rate Slab", '500px', '350px', "Y");
        }
        function SignOff() {
            window.parent.SignOff();
        }
        function height() {
            if (document.body.scrollHeight >= 500)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '500px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        function callback() {
            grid.PerformCallback();
        }
        function EndCall(obj) {
            height();
        }

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="TableMain100">
        <tr>
            <td class="EHEADER" align="center">
                <strong><span style="color: #000099">Interest Rate Slab</span></strong></td>
        </tr>
        <tr>
            <td style="" align="right">
                <table>
                    <tr>
                        <td>
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
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td id="Td1" align="left">
                            <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a> || <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <dxe:ASPxGridView ID="GridIntSlabSlab" runat="server" KeyFieldName="IntSlab_ID"
        AutoGenerateColumns="False" DataSourceID="IntSlabDataSource"
        Width="100%" ClientInstanceName="grid" OnCustomJSProperties="GridIntSlabSlab_CustomJSProperties" OnRowDeleting="GridIntSlabSlab_RowDeleting" OnCustomCallback="GridIntSlabSlab_CustomCallback">
        <ClientSideEvents EndCallback="function(s, e) {
	  EndCall(s.cpEND);
}" />

        <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" ColumnResizeMode="NextColumn" />
        <Styles>
            <Header CssClass="gridheader" SortingImageSpacing="5px" ImageSpacing="5px">
            </Header>
            <FocusedRow CssClass="gridselectrow">
            </FocusedRow>
            <LoadingPanel ImageSpacing="10px">
            </LoadingPanel>
            <FocusedGroupRow CssClass="gridselectrow">
            </FocusedGroupRow>
        </Styles>
        <SettingsPager ShowSeparators="True" AlwaysShowPager="True" NumericButtonCount="20"
            PageSize="20">
            <FirstPageButton Visible="True">
            </FirstPageButton>
            <LastPageButton Visible="True">
            </LastPageButton>
        </SettingsPager>
        <Columns>
            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="IntSlab_ID"
                Caption="ID">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <CellStyle CssClass="gridcellleft">
                </CellStyle>
                <EditFormCaptionStyle HorizontalAlign="Right">
                </EditFormCaptionStyle>
                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="IntSlab_Code" Width="10%"
                Caption="Code">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <CellStyle CssClass="gridcellleft">
                </CellStyle>
                <EditFormCaptionStyle HorizontalAlign="Right">
                </EditFormCaptionStyle>
                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>

            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="IntSlab_AmountFrom"
                Width="15%" Caption="Amount From">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <CellStyle CssClass="gridcellleft">
                </CellStyle>
                <EditFormCaptionStyle HorizontalAlign="Right">
                </EditFormCaptionStyle>
                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="IntSlab_AmountTo"
                Width="15%" Caption="Amount To">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                <CellStyle CssClass="gridcellleft">
                </CellStyle>
                <EditFormCaptionStyle HorizontalAlign="Right">
                </EditFormCaptionStyle>
                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="IntSlab_Rate"
                Width="10%" Caption="Rate">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <CellStyle CssClass="gridcellleft">
                </CellStyle>
                <EditFormCaptionStyle HorizontalAlign="Right">
                </EditFormCaptionStyle>
                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewCommandColumn VisibleIndex="4" Caption="Add New" ShowDeleteButton="True">
                <HeaderStyle HorizontalAlign="Center">
                </HeaderStyle>
                <HeaderTemplate>
                    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                      { %>
                    <a href="javascript:void(0);" onclick="OnAddButtonClick( )">
                        <span style="color: #000099; text-decoration: underline">Add New</span>
                    </a>
                    <%} %>
                </HeaderTemplate>
            </dxe:GridViewCommandColumn>
        </Columns>
        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" EditFormColumnCount="3" />
    </dxe:ASPxGridView>
    <asp:SqlDataSource ID="IntSlabDataSource" runat="server"
        DeleteCommand="delete from Master_IntSlab where IntSlab_ID=@IntSlab_ID">
        <DeleteParameters>
            <asp:Parameter Name="IntSlab_ID" Type="String" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <br />
    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>
</asp:Content>

