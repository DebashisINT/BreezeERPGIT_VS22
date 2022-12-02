<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    Inherits="ERP.OMS.Management.management_frm_workingShedule" CodeBehind="frm_workingShedule.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>

<%@ Register assembly="DevExpress.Web.v15.1" namespace="DevExpress.Web" tagprefix="dx" %>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script language="javascript" type="text/javascript">
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
        function OnMoreInfoClick(keyValue) {
            var url = 'Working_Schedule_General.aspx?id=' + keyValue;
            OnMoreInfoClick(url, "Modify Working Hour Schedule Details", '940px', '450px', "Y");

        }
        function OnAddButtonClick() {
            var url = 'Working_Schedule_General.aspx?id=' + 'ADD';
            OnMoreInfoClick(url, "Add Working Hour Schedule Details", '940px', '450px', "Y");
        }
        function callback() {
            grid.PerformCallback();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="panel-heading">
        <div class="panel-title">
            <h3>Working Schedule</h3>
        </div>
    </div>
    <div class="form_main"> 
    <table class="TableMain100" cellpadding="0px" cellspacing="0px">
        <%--<tr>
            <td class="EHEADER" style="text-align: center">
                <strong><span style="color: #000099">Working Schedule</span></strong></td>
        </tr>--%>
        <tr>
            <td style="" align="right">
                <table>
                    <tr>
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
                </table>
            </td>
        </tr>
        <tr>
            <td class="gridcellcenter">
                <dxe:ASPxGridView ID="WorkingHourGrid" runat="server" KeyFieldName="wor_id" AutoGenerateColumns="False"
                    DataSourceID="WorkingHourDataSource" Width="100%" ClientInstanceName="grid" OnCustomCallback="EmployeeGrid_CustomCallback">
                    <Columns>
                        <dxe:GridViewDataTextColumn FieldName="wor_scheduleName" ReadOnly="True" VisibleIndex="0"
                            Caption="Schedule Name">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Monday" FieldName="mondayTime" VisibleIndex="1">
                            <EditFormSettings Visible="False" />
                            <CellStyle HorizontalAlign="Left">
                            </CellStyle>
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn FieldName="tuesdayTime" VisibleIndex="2" Caption="Tuesday">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Wednesday" FieldName="wednesdayTime" VisibleIndex="3">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Thursday" FieldName="thursdayTime" VisibleIndex="4">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Friday" FieldName="fridayTime" ReadOnly="True"
                            VisibleIndex="5">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Saturday" VisibleIndex="6" FieldName="saturdayTime">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Sunday" FieldName="sundayTime" VisibleIndex="7">
                            <CellStyle CssClass="gridcellleft">
                            </CellStyle>
                            <EditFormCaptionStyle HorizontalAlign="Right">
                            </EditFormCaptionStyle>
                            <EditFormSettings Visible="False" />
                        </dxe:GridViewDataTextColumn>
                        <dxe:GridViewDataTextColumn Caption="Edit" VisibleIndex="8" Width="5%">
                            <DataItemTemplate>
                                <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')">More Info...</a>
                            </DataItemTemplate>
                            <EditFormSettings Visible="False" />
                            <CellStyle Wrap="False">
                            </CellStyle>
                            <HeaderTemplate>
                                <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                  { %>
                                <a href="javascript:void(0);" onclick="OnAddButtonClick();"><span style="color: #000099; text-decoration: underline">Add New</span> </a>
                                <%} %>
                            </HeaderTemplate>
                        </dxe:GridViewDataTextColumn>
                    </Columns>
                    <Styles>
                        <LoadingPanel ImageSpacing="10px">
                        </LoadingPanel>
                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                        </Header>
                    </Styles>
                    <Settings ShowTitlePanel="True" ShowStatusBar="Visible" />
                    <SettingsText PopupEditFormCaption="Add/ Modify Working Hour Schedule" />
                    <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormModal="True"
                        PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="900px" EditFormColumnCount="3" />
                    <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                    <SettingsPager ShowSeparators="True" AlwaysShowPager="True" NumericButtonCount="20"
                        PageSize="20">
                        <FirstPageButton Visible="True">
                        </FirstPageButton>
                        <LastPageButton Visible="True">
                        </LastPageButton>
                    </SettingsPager>
                </dxe:ASPxGridView>
            </td>
        </tr>
    </table>

    <asp:SqlDataSource ID="WorkingHourDataSource" runat="server" ></asp:SqlDataSource>
    <br />
    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>
    <br />
    </div>
</asp:Content>
