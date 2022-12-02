<%@ Page Title="Professional/Technical Bodies" Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.Master.management_master_ProfBodies" CodeBehind="ProfBodies.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function ClickOnMoreInfo(keyValue) {
            var url = 'ProfBodies_general.aspx?id=' + keyValue;
            //OnMoreInfoClick(url, "Modify Professional/Technical Details", '940px', '450px', "Y");
            window.location.href = url;
        }
        function OnAddButtonClick() {
            var url = 'ProfBodies_general.aspx?id=' + 'ADD';
            //OnMoreInfoClick(url, "Add Professional/Technical Details", '940px', '450px', "Y");
            window.location.href = url;
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function callback() {
            grid.PerformCallback();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Professional/Technical Bodies</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100" cellpadding="0px" cellspacing="0px">
            <%--<tr>
                <td class="EHEADER" colspan="2" style="text-align: center">
                    <strong><span style="color: #000099">Professional/Technical Bodies</span></strong>
                </td>
            </tr>--%>
            <tr>
                <td style="text-align: left; vertical-align: top" class="gridcellleft">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                <% if (rights.CanAdd)
                                   { %>
                                <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-primary"><span>Add New</span> </a>
                                <%} %>
                                <% if (rights.CanExport)
                                               { %>
                                 <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true"  >
                                        <asp:ListItem Value="0">Export to</asp:ListItem>
                                        <asp:ListItem Value="1">PDF</asp:ListItem>
                                            <asp:ListItem Value="2">XLS</asp:ListItem>
                                            <asp:ListItem Value="3">RTF</asp:ListItem>
                                            <asp:ListItem Value="4">CSV</asp:ListItem>
                                    </asp:DropDownList>
                                  <%} %>
                                <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Show Filter</span></a>--%>
                            </td>
                            <td id="Td1">
                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                            </td>
                        </tr>
                    </table>
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
                <td style="vertical-align: top; text-align: left" colspan="2" class="gridcellcenter">
                    <dxe:ASPxGridView ID="gridProf" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                        DataSourceID="Sqlprof" KeyFieldName="prof_id" Width="100%" OnCustomCallback="gridProf_CustomCallback"
                        OnCustomJSProperties="gridProf_CustomJSProperties">
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn FieldName="prof_id" ReadOnly="True" Visible="False"
                                VisibleIndex="0">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Name" FieldName="prof_name" VisibleIndex="0">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Short Name" FieldName="prof_shortname" VisibleIndex="1">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Registration Number" FieldName="prof_regnNumber"
                                VisibleIndex="2">
                                <CellStyle CssClass="gridcellleft">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <%--<dxe:GridViewDataTextColumn Caption="Details" VisibleIndex="3" Width="5%">
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')">More Info...</a>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <CellStyle Wrap="False">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                </HeaderTemplate>
                            </dxe:GridViewDataTextColumn>--%>
                            <dxe:GridViewDataTextColumn VisibleIndex="6" Width="6%">
                                <DataItemTemplate>
                                    <% if(rights.CanView)
                                       { %>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')" title="More Info">
                                        <img src="../../../assests/images/info.png" />
                                    </a>
                                    <% } %>
                                </DataItemTemplate>
                                <CellStyle HorizontalAlign="Center" Wrap="False">
                                </CellStyle>
                                <HeaderTemplate>Actions</HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                                <EditFormSettings Visible="False" />                                
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <SettingsText PopupEditFormCaption="Add/ Modify Professional/Technical Bodies" />
                        <SettingsSearchPanel Visible="True" />
                        <Settings ShowGroupPanel="True" ShowTitlePanel="True" ShowStatusBar="Visible" ShowFilterRow="true" showfilterrowmenu="true"/>
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                            PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" EditFormColumnCount="3" />
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsPager ShowSeparators="True" AlwaysShowPager="True" NumericButtonCount="20"
                            PageSize="20">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <ClientSideEvents EndCallback="function(s, e) {
	                                                        LastCall(s.cpHeight);
                                                        }" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="Sqlprof" runat="server" 
            SelectCommand="SELECT [prof_id], [prof_name], [prof_shortname], [prof_regnNumber] FROM [tbl_master_profTechBodies]"></asp:SqlDataSource>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
        <br />
    </div>
</asp:Content>
