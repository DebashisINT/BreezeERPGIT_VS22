<%@ Page Title="Stock Journal (Stock Transfer)" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true"
    CodeBehind="Stock-journalTransferList.aspx.cs" Inherits="ERP.OMS.Management.Activities.Stock_journalTransferList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="CSS/Stock-journalTransferList.css" rel="stylesheet" />
    <script src="JS/Stock-journalTransferList.js"></script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-heading">
        <div class="panel-title">
            <h3>Stock Journal (Stock Transfer) </h3>
        </div>

        <table class="padTab pull-right" style="margin-top: 7px">
            <tr>
                <td>
                    From 
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="FormDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="cFormDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    To 
                </td>
                <td style="width: 150px">
                    <dxe:ASPxDateEdit ID="toDate" runat="server" EditFormat="Custom" EditFormatString="dd-MM-yyyy" ClientInstanceName="ctoDate" Width="100%">
                        <ButtonStyle Width="13px">
                        </ButtonStyle>
                    </dxe:ASPxDateEdit>
                </td>

                <td>
                    <input type="button" value="Show" class="btn btn-primary" onclick="updateGridByDate()" />
                </td>

            </tr>

        </table>
    </div>
    <div class="form_main">
        <div class="clearfix">

            <% if (rights.CanAdd)
               { %>
            <a href="javascript:void(0);" onclick="OnAddButtonClick()" class="btn btn-success btn-radius"><span class="btn-icon"><i class="fa fa-plus" ></i></span><span><u>A</u>dd New</span> </a>


            <%} %>



            <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-primary btn-radius" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}">
                <asp:ListItem Value="0">Export to</asp:ListItem>
                <asp:ListItem Value="1">PDF</asp:ListItem>
                <asp:ListItem Value="2">XLS</asp:ListItem>
                <asp:ListItem Value="3">RTF</asp:ListItem>
                <asp:ListItem Value="4">CSV</asp:ListItem>
            </asp:DropDownList>

        </div>
    </div>

    <div class="GridViewArea relative">
        <dxe:ASPxGridView ID="gridjournal" runat="server" KeyFieldName="ID" AutoGenerateColumns="False"
            Width="100%" ClientInstanceName="gridjournal" OnCustomCallback="Grdstockjournal_CustomCallback"   OnSummaryDisplayText="Grid_b2cs_SummaryDisplayText" 
            OnDataBinding="gridJournal_DataBinding" SettingsDataSecurity-AllowEdit="false" SettingsDataSecurity-AllowInsert="false" SettingsDataSecurity-AllowDelete="false" Settings-VerticalScrollableHeight="250" Settings-VerticalScrollBarMode="Auto" >
            <SettingsSearchPanel Visible="True" Delay="5000" />
            <Columns>


                <dxe:GridViewDataTextColumn Caption="Sl#" FieldName="Slno" CellStyle-HorizontalAlign="Center" CellStyle-VerticalAlign="Middle"
                    VisibleIndex="1" Width="2%">
                    <Settings AllowAutoFilter="False"
                        AllowSort="False" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <HeaderStyle HorizontalAlign="Center" />
                </dxe:GridViewDataTextColumn>




                <dxe:GridViewDataTextColumn Caption="Document Number" FieldName="Journal_Number"
                    VisibleIndex="2" Width="15%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn Caption="Posting Date" FieldName="JournalDate" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy"
                    VisibleIndex="3" Width="10%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>




                <dxe:GridViewDataTextColumn Caption="Unit" FieldName="Frombranch"
                    VisibleIndex="4" Width="15%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>



                <dxe:GridViewDataTextColumn Caption="Quantity" FieldName="Quantity"
                    VisibleIndex="5" Width="10%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <HeaderStyle HorizontalAlign="Right" />
                    <Settings AllowAutoFilter="False" />
                </dxe:GridViewDataTextColumn>




                <dxe:GridViewDataTextColumn Caption="Created By" FieldName="CreatedBy"
                    VisibleIndex="6" Width="10%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="CreatedDate" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy hh:mm:ss"
                    VisibleIndex="7" Width="15%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>


                <dxe:GridViewDataTextColumn Caption="Updated By" FieldName="UpdatedBy"
                    VisibleIndex="8" Width="10%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>

                <%--      

                <dxe:GridViewDataTextColumn Caption="Updated On" FieldName="UpdatedDate" PropertiesTextEdit-DisplayFormatString="dd-MM-yyyy"
                    VisibleIndex="9" FixedStyle="Left" Width="10%">
                    <CellStyle CssClass="gridcellleft" Wrap="true">
                    </CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxe:GridViewDataTextColumn>--%>

                <dxe:GridViewDataTextColumn VisibleIndex="11" Width="1px" CellStyle-HorizontalAlign="Right">
                    <CellStyle CssClass="gridcellleft" Wrap="true" HorizontalAlign="Center">
                    </CellStyle>
                    <HeaderStyle HorizontalAlign="Center" />
                    <Settings AllowAutoFilterTextInputTimer="False" />
                    <DataItemTemplate>

                        <div class='floatedBtnArea'>     
                        <% if (rights.CanView)
                           { %>
                        <a href="javascript:void(0);" onclick="OnMoreInfoClick('<%# Container.KeyValue %>')" title="">
                            <span class='ico ColorFive'><i class='fa fa-eye'></i></span><span class='hidden-xs'>View</span></a>

                        <% } %>


                        <% if (rights.CanDelete)
                           { %>

                        <a href="javascript:void(0);" onclick="OnClickDelete('<%# Container.KeyValue %>')" title="Delete">
                            <span class='ico deleteColor'><i class='fa fa-trash' aria-hidden='true'></i></span><span class='hidden-xs'>Delete</span></a>



                        <% } %>
                         </div>
                    </DataItemTemplate>

                    <HeaderTemplate><span></span></HeaderTemplate>
                    <EditFormSettings Visible="False"></EditFormSettings>
                </dxe:GridViewDataTextColumn>
            </Columns>
            <SettingsContextMenu Enabled="true"></SettingsContextMenu>
            <ClientSideEvents />



            <SettingsPager PageSize="10">
                <PageSizeItemSettings Visible="true" ShowAllItem="false" Items="10,50,100,150,200" />
            </SettingsPager>

            <Settings ShowFooter="true" ShowGroupPanel="true" ShowGroupFooter="VisibleIfExpanded" />


            <Settings ShowGroupPanel="True" ShowStatusBar="Hidden" ShowHorizontalScrollBar="False" ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsLoadingPanel Text="Please Wait..." />
            <ClientSideEvents EndCallback="cListPanelEndCall" RowClick="gridRowclick" />
            <TotalSummary>

                <dxe:ASPxSummaryItem FieldName="Quantity" SummaryType="Sum" />
                  <dxe:ASPxSummaryItem FieldName="Journal_Number" SummaryType="count" />





            </TotalSummary>

        </dxe:ASPxGridView>

        <asp:HiddenField ID="hiddenedit" runat="server" />
        <asp:HiddenField ID="hidden_replacementId" runat="server" />

    </div>

    <div style="display: none">
        <dxe:ASPxGridViewExporter ID="exporter" GridViewID="gridjournal" runat="server" Landscape="false" PaperKind="A4" PageHeader-Font-Size="Larger" PageHeader-Font-Bold="true">
        </dxe:ASPxGridViewExporter>
    </div>


</asp:Content>


