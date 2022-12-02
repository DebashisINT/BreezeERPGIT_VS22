<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_frmAttchServerFile" CodeBehind="frmAttchServerFile.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>--%>
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript">
        function OnGridSelectionChanged() {
            //        var noofrow=grid.GetSelectedRowCount().toString();
            //        alert(noofrow);
            grid.GetSelectedFieldValues('FilePath', OnGridSelectionComplete);
        }
        function OnGridSelectionComplete(values) {
            counter = 'n';
            for (var i = 0; i < values.length; i++) {
                if (counter != 'n')
                    counter += ',' + values[i];
                else
                    counter = values[i];
            }

        }
        function btnRead_click() {
            if (counter != 'n') {
                // alert(counter);
                // var ReadIDs= 'read~' + counter;
                //CallServer(ReadIDs,"");
                // var path=counter;
                parent.document.getElementById("IFRAME_ForAllPages").contentWindow.GetServerFilePath(counter);
                parent.editwin.close();
            }
            else
                alert('Plase Select a file!');

        }
        function OngridSelectAll(obj) {
            OngridSelectionChanged();
        }

        function SignOff() {
            window.parent.SignOff();
        }


        function OnAddButtonClick() {
            var url = 'frmAddForms.aspx?id=' + 'ADD';
            OnMoreInfoClick(url, "Add New File", '700px', '370px', "Y");

        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function callback() {
            grid.PerformCallback();
        }
        function callheight(obj) {

           // parent.CallMessage();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER" style="text-align: center;">
                    <strong><span style="color: #000099">Forms & Notices
                            <asp:Label ID="lblP" runat="server" Text="Label"></asp:Label></span></strong>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table>
                        <tr>
                            <td id="ShowFilter">
                                <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a>
                            </td>
                            <td id="Td1">
                                <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <dxe:ASPxGridView ID="grdDocuments" runat="server" AutoGenerateColumns="False"
                        KeyFieldName="formID" Width="100%" OnRowDeleting="grdDocuments_RowDeleting" ClientInstanceName="grid"
                        OnCustomCallback="grdDocuments_CustomCallback">
                        <Settings ShowGroupPanel="True" />
                        <SettingsBehavior AllowMultiSelection="True" />
                        <ClientSideEvents SelectionChanged="function(s, e) { OnGridSelectionChanged(); }" />
                        <Columns>
                            <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="3%">
                                <HeaderTemplate>
                                    <input type="checkbox" onclick="grid.SelectAllRowsOnPage(this.checked);" style="vertical-align: middle;"
                                        title="Select/Unselect all rows on the page"></input>
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center">
                                    <Paddings PaddingBottom="1px" PaddingTop="1px" />
                                </HeaderStyle>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn FieldName="formID" ReadOnly="True" VisibleIndex="0"
                                Visible="false">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="FormName" ReadOnly="True" VisibleIndex="0">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="FilePath" VisibleIndex="1" Visible="false">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn FieldName="Description" VisibleIndex="1">
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="ADD/Modify " FieldName="formID" ReadOnly="True"
                                VisibleIndex="5">
                                <DataItemTemplate>
                                    <a href='ViewFile.aspx?id=<%#Eval("FilePath") %>' target="_blank">View</a>
                                </DataItemTemplate>
                                <EditFormSettings Visible="False" />
                                <CellStyle HorizontalAlign="Left">
                                </CellStyle>
                                <HeaderStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    View
                                </HeaderTemplate>
                            </dxe:GridViewDataTextColumn>
                            <%--                                <dxe:GridViewCommandColumn VisibleIndex="6" Visible="false">
                                    <DeleteButton Visible="True">
                                    </DeleteButton>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                        <a href="javascript:void(0);" onclick="OnAddButtonClick();"><span style="color: #000099;
                                            text-decoration: underline">Add New</span> </a>
                                    </HeaderTemplate>
                                </dxe:GridViewCommandColumn>--%>
                        </Columns>
                        <SettingsBehavior AllowFocusedRow="True" ColumnResizeMode="NextColumn" />
                        <Styles>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <FocusedGroupRow CssClass="gridselectrow">
                            </FocusedGroupRow>
                            <FocusedRow CssClass="gridselectrow">
                            </FocusedRow>
                        </Styles>
                        <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsEditing Mode="PopupEditForm" PopupEditFormHorizontalAlign="Center" PopupEditFormHeight="600px"
                            PopupEditFormVerticalAlign="TopSides" PopupEditFormWidth="900px" />
                        <SettingsText PopupEditFormCaption="Add/Modify Cash/Bank" ConfirmDelete="Are you sure to Delete this Record!" />
                        <ClientSideEvents EndCallback="function(s, e) {
	callheight(s.cpHeight);
}" />
                    </dxe:ASPxGridView>
                    <input id="btnRead" type="button" value="Upload" class="btnUpdate" onclick="btnRead_click();"
                        style="width: 66px; height: 19px" tabindex="1" /><%--   <asp:SqlDataSource ID="grddoc" runat="server" 
                            SelectCommand="" DeleteCommand="DELETE FROM [tbl_master_forms] WHERE [frm_id] = @formID">
                            <DeleteParameters>
                                <asp:Parameter Name="formID" Type="int32" />
                            </DeleteParameters>
                            <SelectParameters>
                                <asp:SessionParameter Name="userlist" SessionField="userchildHierarchy" Type="string" />
                            </SelectParameters>
                        </asp:SqlDataSource>--%></td>
            </tr>
        </table>
    </div>
</asp:Content>

