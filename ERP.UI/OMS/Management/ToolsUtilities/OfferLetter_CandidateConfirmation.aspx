<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_toolsutilities_OfferLetter_CandidateConfirmation" CodeBehind="OfferLetter_CandidateConfirmation.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    

    --%>

    <script language="javascript" type="text/javascript">
        function PageLoadFirst() {
            document.getElementById("tdDateRange").style.display = "none";
            document.getElementById("tdGenerate").style.display = "none";
            document.getElementById("tdJoining").style.display = "none";
            counter = 'n';


        }

        //function callheight(obj) {

        //    height();
        //   // parent.CallMessage();
        //}

        function ShowHideFilter(obj) {
            var showrecord = 'Show~' + obj
            grid.PerformCallback(showrecord);
            height();
        }

        function DeleteRow(keyValue) {
            doIt = confirm('Confirm delete?');
            if (doIt) {
                grid.PerformCallback('Delete~' + keyValue);
                height();
            }
            else {

            }


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
            document.getElementById('HDNSelection').value = counter;
            CallServer(ReadIDs, "");
            alert("Wait !");

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
            OnMoreInfoClick(url, "Add Candidates", '960px', '550px', "Y");
        }
        function OnMoreInfoClick(keyValue) {

            var url = 'AddCandidateForOLetter.aspx?id=' + keyValue + '&mode=edit';
            OnMoreInfoClick(url, "Edit Candidate", '960px', '550px', "Y");

        }

        function OnMoreShowButtonClick(keyValue) {

            var url = 'AddCandidateForOLetter.aspx?id=' + keyValue + '&mode=show';
            OnMoreInfoClick(url, "Edit Candidate", '960px', '550px', "Y");
        }


        function OnMoreAccessCick(keyValue) {
            grid.PerformCallback('Access~' + keyValue);
            height();


        }

        function radioselect(obj) {


            if (obj == 'A') {
                document.getElementById("tdDateRange").style.display = "none";
            }
            else if (obj == 'B') {
                document.getElementById("tdDateRange").style.display = "inline";
            }



        }
        function HideFilter() {
            if (document.getElementById("RadAllRecord").checked == true) {
                document.getElementById("tdDateRange").style.display = "none";
            }
            else {
                document.getElementById("tdDateRange").style.display = "inline";
            }



        }

    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Confirm Candidate</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top; padding-left: 30px; padding-bottom: 15px;">
                    <div class="crossBtn"><a href="../ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="1" cellpadding="2" 
                        >
                        <tr>
                            <td>Select Date Range:
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RadAllRecord" runat="server" Checked="true" GroupName="e" onclick="radioselect('A')" />
                                        </td>
                                        <td>All
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="RadSRecord" runat="server" GroupName="e" onclick="radioselect('B')" />
                                        </td>
                                        <td>Specific Date Range
                                        </td>
                                        <td id="tdDateRange">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxDateEdit ID="dtDate" ClientInstanceName="dtDate" runat="server" EditFormat="Custom"
                                                            UseMaskBehavior="True" TabIndex="1" Width="135px">
                                                            <DropDownButton Text="From ">
                                                            </DropDownButton>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td class="gridcellleft">
                                                        <dxe:ASPxDateEdit ID="dtToDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                            TabIndex="2" Width="135px">
                                                            <DropDownButton Text="To">
                                                            </DropDownButton>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:Button ID="Button1" runat="server" Text="Show" OnClick="Button1_Click" CssClass="btn btn-primary"
                                                 TabIndex="12" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnAdd" runat="server" Text="Add Employee" CssClass="btn btn-primary" OnClick="btnAdd_Click" /></td>
                        </tr>
                        <%-- <tr>
                                <td valign="top">
                                    <asp:RadioButton ID="RadioButton1" runat="server" GroupName="b" onclick="radioselect('C')" />
                                    Generate Offer Letter
                                </td>
                                <td>
                                    <asp:RadioButton ID="RadioButton2" runat="server" GroupName="b" onclick="radioselect('D')" />
                                    Update Joining Status
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table id="tdJoining">
                                        <tr>
                                            <td>
                                                Confirm Joining Date:
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="txtJD" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                                    TabIndex="20" Width="204px">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td>
                                                <asp:Button ID="BtnJoin" runat="server" Text="Confirm Join" CssClass="btnUpdate"
                                                    OnClick="BtnJoin_Click" /></td>
                                        </tr>
                                    </table>
                                    <table id="tdGenerate">
                                        <tr style="display: none;">
                                            <td class="gridcellleft">
                                                Use Template :
                                            </td>
                                            <td id="tdHeader">
                                                <asp:TextBox ID="txtHeader" runat="server" Width="279px" Font-Size="12px" onkeyup="FunTemplate(this,'GetTeplate',event)"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td class="gridcellleft">
                                                Add Signatory :
                                            </td>
                                            <td id="tdAddSig">
                                                <asp:TextBox ID="txtSignature" runat="server" Width="279px" Font-Size="12px" onkeyup="FunAddSig(this,'SearchByEmployeesWithSignature',event)"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:Button ID="btnGenerate" Text="Generate" runat="server" CssClass="btnUpdate"
                                                    OnClick="btnGenerate_Click1" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; vertical-align: top">
                    <table width="100%">
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td id="Td1" align="left">
                                <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Show Filter</span></a>--%> 
                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span >All Records</span></a>--%>
                            </td>
                            <td>
                                <span id="spanshow2" style="color: Blue; font-weight: bold"></span>&nbsp;&nbsp;<span
                                    id="spanshow3"></span>
                            </td>
                            <td align="right" class="pull-right">
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
                                    <ButtonStyle >
                                    </ButtonStyle>
                                    <ItemStyle>
                                        <HoverStyle >
                                        </HoverStyle>
                                    </ItemStyle>
                                    <Border BorderColor="black" />
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
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <dxe:ASPxGridView ID="GridMessage" ClientInstanceName="grid" runat="server" Width="100%"
                                KeyFieldName="rde_id" OnCustomCallback="GridMessage_CustomCallback" AutoGenerateColumns="False"
                                OnHtmlDataCellPrepared="GridMessage_HtmlDataCellPrepared" OnPageIndexChanged="GridMessage_PageIndexChanged">
                                <ClientSideEvents SelectionChanged="function(s, e) { OnGridSelectionChanged(); }"
                                    BeginCallback="function(s, e) {
	callheight(s.cpHeight);
}" />
                                <SettingsBehavior AllowMultiSelection="True" />
                                <%--<Styles>
                                    <Header SortingImageSpacing="5px" BackColor="LightSteelBlue" ImageSpacing="5px"></Header>

                                    <FocusedRow CssClass="gridselectrow"></FocusedRow>

                                    <LoadingPanel ImageSpacing="10px"></LoadingPanel>

                                    <FocusedGroupRow BackColor="#FFC080"></FocusedGroupRow>
                                </Styles>
                                <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
                                    <FirstPageButton Visible="True"></FirstPageButton>

                                    <LastPageButton Visible="True"></LastPageButton>
                                </SettingsPager>--%>
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
                                    <dxe:GridViewDataTextColumn ReadOnly="True" VisibleIndex="1" FieldName="rde_Name" Width="10px" Caption="Cand. Name">
                                        <CellStyle CssClass="gridcellleft"></CellStyle>

                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn Visible="False" ReadOnly="True" VisibleIndex="2" Width="10px" FieldName="rde_ResidenceLocation" Caption="Address">
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
                                            <a href="javascript:void(0);" onclick="OnMoreShowButtonClick('<%# Container.KeyValue %>')">More Info</a>

                                        </DataItemTemplate>

                                        <CellStyle Wrap="False"></CellStyle>
                                        <HeaderTemplate>
                                            More Info.
                                        
                                        </HeaderTemplate>

                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="12" Width="60px" Caption="Details">
                                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                        <DataItemTemplate>
                                            <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                              { %>
                                            <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')">Delete</a>
                                            <%} %>
                                        </DataItemTemplate>

                                        <CellStyle Wrap="False"></CellStyle>
                                        <HeaderTemplate>
                                            <span style="color: #000099; text-decoration: underline">Delete </span>

                                        </HeaderTemplate>

                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>

                                </Columns>
                                <Settings ShowGroupPanel="True" />
                                <%--<Settings ShowHorizontalScrollBar="True" />--%>
                            </dxe:ASPxGridView>
                            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                            </dxe:ASPxGridViewExporter>
                            <%--  </div>--%>
                            <%--    <input id="btnRead" type="button" value="Read" class="btnUpdate" onclick="btnRead_click();"
                            style="width: 66px; height: 19px" tabindex="1" />--%>
                            <asp:HiddenField ID="HDNSelection" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
