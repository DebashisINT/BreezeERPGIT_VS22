<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.management_GenerateOfferLater" CodeBehind="GenerateOfferLater.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script language="javascript" type="text/javascript">

        function callheight(obj) {
            //height();
            // parent.CallMessage();
        }

        function ShowHideFilter(obj) {
            var showrecord = 'Show~' + obj
            grid.PerformCallback(showrecord);
            //height();
        }

        function DeleteRow(keyValue) {
            doIt = confirm('Confirm delete?');
            if (doIt) {
                grid.PerformCallback('Delete~' + keyValue);
                //height();
            }
            else {

            }
        }

        function PageLoadFirst() {

            counter = 'n';
            FieldName = "drp_accesslevel";

        }

        function ReceiveServerData(rValue) {
            var DATA = rValue.split('~');
            if (DATA[0] == "read") {
                if (DATA[1] == "Y") {
                    alert('Read Successfully!');
                    grid.PerformCallback('read');
                    grid.UnselectAllRowsOnPage();
                }
                else if (DATA[1] == "S")
                    alert('Please Select a message!');
            }
        }

        function callback() {
            grid.PerformCallback();
        }

        function OnGridSelectionChanged() {
            grid.GetSelectedFieldValues('rde_id', OnGridSelectionComplete);
        }

        function OnGridSelectionComplete(values) {
            counter = 'n';

            for (var i = 0; i < values.length; i++) {
                if (counter != 'n')
                    counter += ',' + values[i];

                else
                    counter = values[i];

            }

            var ReadIDs = 'read~' + counter;
            CallServer(ReadIDs, "");
            //alert(counter+'test');
        }


        //function SignOff() {
        //    window.parent.SignOff();
        //}
        //function height() {

        //    if (document.body.scrollHeight >= 600)
        //        window.frameElement.height = document.body.scrollHeight + 600;
        //    else
        //        window.frameElement.height = '600px';
        //    window.frameElement.Width = document.body.scrollWidth + 100;
        //}
        function OnAddButtonClick() {
            var url = 'AddCandidateForOLetter.aspx?id=ADD&mode=new';
            //OnMoreInfoClick(url, "Add Candidates", '960px', '550px', "Y");
            window.location.href = url;
        }

        function ClickOnMoreInfo(keyValue) {
            var url = 'AddCandidateForOLetter.aspx?id=' + keyValue + '&mode=edit';
            //OnMoreInfoClick(url, "Edit Candidate", '960px', '550px', "Y");
            window.location.href = url;
        }

        function OnMoreShowButtonClick(keyValue) {
            var url = 'AddCandidateForOLetter.aspx?id=' + keyValue + '&mode=show';
            //OnMoreInfoClick(url, "Edit Candidate", '960px', '550px', "Y");
            window.location.href = url;
        }


        function OnMoreAccessCick(keyValue) {
            grid.PerformCallback('Access~' + keyValue);
            //height();
            //       var url='AddCandidateForOLetter.aspx?id='+ keyValue+'&mode=edit' ;
            //       OnMoreInfoClick(url,"Edit Candidate",'960px','550px',"Y");
        }

    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Generate Offer Letter</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top; padding-left: 30px;">
                    <div class="crossBtn"><a href="../Management/ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();" class="btn btn-primary"><span>Add New</span> </a>
                        <% if (rights.CanExport)
                                               { %>
                        <asp:DropDownList ID="drdExport" runat="server" CssClass="btn btn-sm btn-primary" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" AutoPostBack="true" OnChange="if(!AvailableExportOption()){return false;}" >
                            <asp:ListItem Value="0">Export to</asp:ListItem>
                            <asp:ListItem Value="1">PDF</asp:ListItem>
                            <asp:ListItem Value="2">XLS</asp:ListItem>
                            <asp:ListItem Value="3">RTF</asp:ListItem>
                            <asp:ListItem Value="4">CSV</asp:ListItem>
                        </asp:DropDownList>
                        <% } %>
                    </div>
                    <table cellspacing="0px" style="width: 100%;">
                        <tr>
                            <td></td>

                            <td class="gridcellright pull-right">
                                <%--<dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true"
                                    Font-Bold="False" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
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
                                    <Border BorderColor="Black" />
                                    <DropDownButton Text="Export">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>--%>


                            </td>
                        </tr>
                    </table>
                </td>


            </tr>

            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table width="100%">
                        <tr>
                            <td>
                                <table class="pull-right">
                                    <tr>
                                        <td style="padding: 0 15px 0 0">
                                            <label>Search:</label>
                                            <dxe:ASPxDateEdit ID="dtDate" ClientInstanceName="dtDate" runat="server" EditFormat="Custom"
                                                UseMaskBehavior="True" TabIndex="1" Width="135px">
                                                <DropDownButton Text="From ">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="padding: 0 15px 0 0">
                                            <label style="display: block; width: 100%">&nbsp</label>
                                            <dxe:ASPxDateEdit ID="dtToDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                TabIndex="2" Width="135px">
                                                <DropDownButton Text="To">
                                                </DropDownButton>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="padding: 0 0px 0 0px">
                                            <label style="display: block; width: 100%">&nbsp</label>
                                            <asp:Button ID="Button1" runat="server" Text="Show" OnClick="Button1_Click" CssClass="btn btn-primary"
                                                TabIndex="12" />

                                        </td>
                                    </tr>
                                </table>
                                <table class="pull-left">
                                    <tr>

                                        <td style="padding: 0 15px 0 0">
                                            <label>Confirm Joining Date:</label>
                                            <dxe:ASPxDateEdit ID="txtJD" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                TabIndex="20" Width="204px">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="padding-right: 15px">
                                            <label style="display: block; width: 100%">&nbsp</label>
                                            <asp:Button ID="BtnJoin" runat="server" Text="Confirm Join" CssClass="btn btn-info"
                                                OnClick="BtnJoin_Click" /></td>
                                        <td style="padding-right: 15px">
                                            <label style="display: block; width: 100%">&nbsp</label>
                                            <asp:Button ID="btnAdd" runat="server" Text="Add Employee" CssClass="btn btn-success" OnClick="btnAdd_Click"
                                                Visible="false" /></td>
                                        <td>
                                            <label style="display: block; width: 100%">&nbsp</label>
                                            <asp:Button ID="btnGenerate" Text="Generate" runat="server" CssClass="btn btn-primary"
                                                OnClick="btnGenerate_Click1" />

                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <%-- <div style="border: 1px solid black; width: 1000px; height: 650px; overflow: auto"
                            id="panecontainer">--%>
                    <dxe:ASPxGridView ID="GridMessage" ClientInstanceName="grid" runat="server" Width="100%"
                        KeyFieldName="rde_id" OnCustomCallback="GridMessage_CustomCallback" AutoGenerateColumns="False" OnHtmlDataCellPrepared="GridMessage_HtmlDataCellPrepared">
                        <ClientSideEvents SelectionChanged="function(s, e) { OnGridSelectionChanged(); }"
                            BeginCallback="function(s, e) {
	callheight(s.cpHeight);
}" />
                         <SettingsSearchPanel Visible="True" />
                        <SettingsBehavior AllowMultiSelection="True" />
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px"></Header>

                            <FocusedRow CssClass="gridselectrow"></FocusedRow>

                            <LoadingPanel ImageSpacing="10px"></LoadingPanel>

                            <FocusedGroupRow BackColor="#FFC080"></FocusedGroupRow>
                        </Styles>
                        <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
                            <FirstPageButton Visible="True"></FirstPageButton>

                            <LastPageButton Visible="True"></LastPageButton>
                        </SettingsPager>
                        <Columns>
                            <dxe:GridViewCommandColumn VisibleIndex="0" Width="3%" ShowSelectCheckbox="True">
                                <HeaderStyle HorizontalAlign="Center">
                                    <Paddings PaddingTop="1px" PaddingBottom="1px"></Paddings>
                                </HeaderStyle>
                                <HeaderTemplate>
                                    <input type="checkbox" onclick="grid.SelectAllRowsOnPage(this.checked);" style="vertical-align: middle;"
                                        title="Select/Unselect all rows on the page"></input>

                                </HeaderTemplate>
                            </dxe:GridViewCommandColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="1" FieldName="rde_Name" Caption="Cand. Name">
                                <CellStyle CssClass="gridcellleft"></CellStyle>

                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="2" FieldName="rde_ResidenceLocation" Caption="Address">
                                <CellStyle CssClass="gridcellleft"></CellStyle>

                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="2" FieldName="company" Caption="Candt. Company">
                                <CellStyle CssClass="gridcellleft"></CellStyle>

                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="3" FieldName="Branch" Caption="Candt. Branch">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="4" FieldName="Designation" Caption="Candt. Designation">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="6" FieldName="rde_ApprovedCTC" Caption="Approved CTC">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="5" FieldName="CreateUserName" Caption="Created By">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="6" FieldName="CreateDate1" Caption="Create Date">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="7" FieldName="Status" Caption="Status">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="8" FieldName="rde_NoofDepedent" Caption="PAN No.">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="9" FieldName="GenerateDate" Caption="Generate Date">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="9" FieldName="JoiningDate" Caption="Joining Date">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="10" FieldName="rde_IsConfirmJoin" Caption="Join Status">
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="11" Width="60px" Caption="Details">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="OnMoreShowButtonClick('<%# Container.KeyValue %>')" title="More Info" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/info.png" />
                                    </a>
                                    <a href="javascript:void(0);" onclick="ClickOnMoreInfo('<%# Container.KeyValue %>')" title="Edit" class="pad" style="text-decoration: none;">
                                        <img src="../images/Edit.png" />
                                    </a>
                                    <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')" title="Delete" class="pad" style="text-decoration: none;">
                                        <img src="../images/Delete.png" />
                                    </a>

                                </DataItemTemplate>
                                <CellStyle Wrap="False" HorizontalAlign="Center"></CellStyle>
                                <HeaderTemplate>
                                    Actions
                                </HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn VisibleIndex="12" Width="60px" Caption="Details">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <DataItemTemplate>
                                    <a href="javascript:void(0);" onclick="OnMoreAccessCick('<%# Container.KeyValue %>')" title="Allow" class="pad" style="text-decoration: none;">
                                        <img src="../../../assests/images/info.png" />
                                    </a>
                                </DataItemTemplate>
                                <CellStyle Wrap="False"></CellStyle>
                                <HeaderTemplate>
                                    <span>Access </span>
                                </HeaderTemplate>
                                <EditFormSettings Visible="False"></EditFormSettings>
                                <CellStyle Wrap="False" HorizontalAlign="Center"></CellStyle>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowGroupPanel="True" ShowStatusBar="Visible" ShowFilterRow="true" ShowFilterRowMenu="true" />
                    </dxe:ASPxGridView>
                    <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                    </dxe:ASPxGridViewExporter>
                    <%--  </div>--%>
                    <%--    <input id="btnRead" type="button" value="Read" class="btnUpdate" onclick="btnRead_click();"
                            style="width: 66px; height: 19px" tabindex="1" />--%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
