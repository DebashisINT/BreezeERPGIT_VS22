<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Reports.Reports_frmduplicateLeads" CodeBehind="frmduplicateLeads.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
    <script language="javascript" type="text/javascript">
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" language="javascript">

        function OnGridSelectionChanged() {
            grid.GetSelectedFieldValues('id', OnGridSelectionComplete);
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

        function AtTheTimePageLoad() {
            FieldName = '';
        }
        function btnDelete_click() {
            var data = "Delete";
            data += "~" + counter
            CallServer(data, "");
        }
        function ReceiveServerData(rValue) {
            var DATA = rValue.split('~');
            if (DATA[0] == "Delete") {
                if (DATA[1] == "Y") {
                    alert('Delete Successfully !!');
                    grid.PerformCallback();
                }
            }
        }
    </script>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Duplicate Lead Report</h3>
        </div>
    </div>
     <div class="crossBtn"><a href="../Management/ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <input id="btnDelete" runat="server" type="button" value="Delete" class=" btn btn-danger" onclick="btnDelete_click()"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%--<input id="Button1" runat="server" type="button" value="Upload" class="btnUpdate"  style="height: 19px; width: 48px;"  />--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%--                        <tr>
                            <td class="EHEADER" colspan="2" style="text-align: center;">
                                <strong><span style="color: #000099">Duplicate Lead Report</span></strong>
                            </td>
                        </tr>--%>
                        <tr>
                            
                            <td style="width: 90%; vertical-align: top">
                                <asp:Panel ID="panel" runat="server" BorderColor="blue" BorderWidth="0px" Width="99%">
                                    <table>
                                        <tr>
                                            <td>
                                                <dxe:ASPxGridView ID="GridDuplicateLead" ClientInstanceName="grid" KeyFieldName="id" runat="server" Width="100%" AutoGenerateColumns="False" OnCustomCallback="GridDuplicateLead_CustomCallback">
                                                    <Styles>
                                                        <LoadingPanel ImageSpacing="10px">
                                                        </LoadingPanel>
                                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                        </Header>
                                                    </Styles>

                                                    <ClientSideEvents SelectionChanged="function(s, e) { OnGridSelectionChanged(); }" />
                                                    <Columns>
                                                        <dxe:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="5%" ShowClearFilterButton="true">
                                                            <%--<ClearFilterButton Visible="True">
                                        </ClearFilterButton>--%>
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                        </dxe:GridViewCommandColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Lead ID" FieldName="id" ReadOnly="True"
                                                            VisibleIndex="1" Width="10%">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Name" FieldName="name"
                                                            ReadOnly="True" VisibleIndex="2" Width="60%">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Phone" FieldName="phone" ReadOnly="True" VisibleIndex="3">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                            <EditFormSettings Visible="False" />
                                                        </dxe:GridViewDataTextColumn>
                                                        <dxe:GridViewDataTextColumn Caption="Status" FieldName="status" VisibleIndex="4">
                                                            <CellStyle CssClass="gridcellleft">
                                                            </CellStyle>
                                                        </dxe:GridViewDataTextColumn>
                                                    </Columns>
                                                    <SettingsCommandButton>
                                                        <ClearFilterButton Text="ClearFilter"></ClearFilterButton>
                                                    </SettingsCommandButton>
                                                    <SettingsPager AlwaysShowPager="True" PageSize="20" ShowSeparators="True">
                                                        <FirstPageButton Visible="True">
                                                        </FirstPageButton>
                                                        <LastPageButton Visible="True">
                                                        </LastPageButton>
                                                    </SettingsPager>
                                                    <SettingsBehavior AllowSort="False" ColumnResizeMode="NextColumn" />
                                                </dxe:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
