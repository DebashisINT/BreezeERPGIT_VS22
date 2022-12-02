<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.Master.management_master_ChargeHeads" CodeBehind="ChargeHeads.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script src="/assests/js/chosen.jquery.js"></script>

    <link href="../../css/chosen.css" rel="stylesheet" />
    <script type="text/javascript">

        function OnMoreInfoClick(keyValue) {

            var url = 'heads.aspx?id=' + keyValue;
            OnMoreInfoClick(url, "Edit Charge Master Details", '940px', '450px', "Y");
        }

        function OnAddButtonClick() {
            var url = 'heads.aspx?id=' + 'ADD';
            OnMoreInfoClick(url, "Add Charge Master Details", '940px', '450px', "Y");
        }

        function callback() {
            grid.PerformCallback();
        }
        function EndCall(obj) {
            if (grid.cpDelmsg != null)
                alert(grid.cpDelmsg);

        }
    </script>
    <table class="TableMain100">
        <tr>
            <td class="EHEADER" style="text-align: center;">
                <strong><span style="color: #000099">Other Charge Master</span></strong>
            </td>
        </tr>
        <tr>
            <td align="right">
                <table>
                    <tr>

                        <td>
                            <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy" Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" ValueType="System.Int32" Width="130px">
                                <items>
                                    <dxe:ListEditItem Text="Select" Value="0" />
                                    <dxe:ListEditItem Text="PDF" Value="1" />
                                    <dxe:ListEditItem Text="XLS" Value="2" />
                                    <dxe:ListEditItem Text="RTF" Value="3" />
                                    <dxe:ListEditItem Text="CSV" Value="4" />
                                </items>
                                <buttonstyle backcolor="#C0C0FF" forecolor="Black">
                                </buttonstyle>
                                <itemstyle backcolor="Navy" forecolor="White">
                                    <HoverStyle BackColor="#8080FF" ForeColor="White">
                                    </HoverStyle>
                                </itemstyle>
                                <border bordercolor="White" />
                                <dropdownbutton text="Export">
                                </dropdownbutton>
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <dxe:ASPxGridView ID="MasterGrid" runat="server" KeyFieldName="OtherCharges_ID"
        AutoGenerateColumns="False" DataSourceID="MasterDataSource" Width="100%" ClientInstanceName="grid" OnCustomCallback="MasterGrid_CustomCallback" OnCustomJSProperties="MasterGrid_CustomJSProperties" OnRowDeleting="MasterGrid_RowDeleting">
        <clientsideevents endcallback="function(s, e) {
	  EndCall(s.cpEND);
}" />
        <templates>
            <EditForm>
            </EditForm>
            <TitlePanel>
                <table style="width: 100%">
                    <tr>
                        <td align="right">
                            <table width="200px">
                                <tr>

                                    <%--         <td>
                                        <dxe:ASPxButton ID="ASPxButton2" runat="server" Text="Search" ToolTip="Search" OnClick="btnSearch"  Height="18px" Width="88px"  >
                                        </dxe:ASPxButton>
                                    </td>
                                    <td>
                                        <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd"){ %>
                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" Text="ADD" ToolTip="Add New Data"   Height="18px" Width="88px"   AutoPostBack="False">
                                            <clientsideevents click="function(s, e) {OnAddButtonClick();}" />
                                        </dxe:ASPxButton>
                                        <%} %>
                                    </td>--%>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>


            </TitlePanel>
        </templates>
        <settingsbehavior allowfocusedrow="True" confirmdelete="True" columnresizemode="NextColumn" />
        <styles>
            <Header CssClass="gridheader" SortingImageSpacing="5px" ImageSpacing="5px"></Header>

            <FocusedRow CssClass="gridselectrow"></FocusedRow>

            <LoadingPanel ImageSpacing="10px"></LoadingPanel>

            <FocusedGroupRow CssClass="gridselectrow"></FocusedGroupRow>
        </styles>
        <settingspager showseparators="True" alwaysshowpager="True" numericbuttoncount="20" pagesize="20">
            <FirstPageButton Visible="True"></FirstPageButton>

            <LastPageButton Visible="True"></LastPageButton>
        </settingspager>
        <columns>
            <dxe:GridViewDataTextColumn Visible="False" VisibleIndex="0" FieldName="OtherCharges_ID" Caption="ID">
                <CellStyle CssClass="gridcellleft"></CellStyle>

                <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="0" FieldName="OtherCharges_Name" Width="20%" Caption="Charge Name">
                <CellStyle CssClass="gridcellleft"></CellStyle>

                <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="1" FieldName="OtherCharges_Code" Width="30%" Caption="Code">
                <CellStyle CssClass="gridcellleft"></CellStyle>

                <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="OtherCharges_ChargeOn" Width="15%" Caption="Basis">
                <CellStyle CssClass="gridcellleft"></CellStyle>

                <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="OtherCharges_Frequency" Width="15%" Caption="Frequency">
                <CellStyle CssClass="gridcellleft"></CellStyle>

                <EditFormCaptionStyle HorizontalAlign="Right"></EditFormCaptionStyle>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewDataTextColumn VisibleIndex="4" Width="10%" Caption="Details">
                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                <DataItemTemplate>
                    <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')">More Info...</a>

                </DataItemTemplate>

                <CellStyle Wrap="False"></CellStyle>
                <HeaderTemplate>
                    <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                      { %>
                    <a href="javascript:void(0);" onclick="OnAddButtonClick( )">
                        <span style="color: #000099; text-decoration: underline">Add New</span>
                    </a>
                    <%} %>
                </HeaderTemplate>

                <EditFormSettings Visible="False"></EditFormSettings>
            </dxe:GridViewDataTextColumn>
            <dxe:GridViewCommandColumn VisibleIndex="5" Caption="Delete" ShowDeleteButton="True"/>
        </columns>
        <settingsediting mode="PopupEditForm" popupeditformhorizontalalign="Center" popupeditformmodal="True"
            popupeditformverticalalign="WindowCenter" popupeditformwidth="900px" editformcolumncount="3" />
        <settingstext popupeditformcaption="Add/ Modify Employee" grouppanel="Other Charge Heads" />
        <settings showgrouppanel="True" showstatusbar="Visible" />
    </dxe:ASPxGridView>
    <asp:SqlDataSource ID="MasterDataSource" runat="server"
        DeleteCommand="delete from Master_OtherCharges where OtherCharges_ID=@OtherCharges_ID">
        <DeleteParameters>
            <asp:Parameter Name="OtherCharges_ID" Type="String" />
        </DeleteParameters>
    </asp:SqlDataSource>

    <br />
    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
    </dxe:ASPxGridViewExporter>
</asp:Content>

