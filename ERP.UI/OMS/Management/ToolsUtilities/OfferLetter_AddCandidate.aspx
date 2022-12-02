<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_OfferLetter_AddCandidate" CodeBehind="OfferLetter_AddCandidate.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    

    
    <script type="text/javascript" src="/assests/js/GenericJScript.js"></script>--%>

    <script type="text/javascript">
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
            // height();
        }

        function DeleteRow(keyValue) {
            doIt = confirm('Confirm delete?');
            if (doIt) {
                grid.PerformCallback('Delete~' + keyValue);
                // height();
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
        }
        function OnAddButtonClick() {
            //        alert(IsProductExpired(ctxtJD.GetDate(),'<%=Session["ExpireDate"]%>'));
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
        }

        function radioselect(obj) {
            if (obj == 'A') {
                document.getElementById("tdDateRange").style.display = "none";
            }
            else if (obj == 'B') {
                document.getElementById("tdDateRange").style.display = "inline";
            }
            else if (obj == 'C') {
                document.getElementById("tdGenerate").style.display = "inline";
                document.getElementById("tdJoining").style.display = "none";
            }
            else if (obj == 'D') {
                document.getElementById("tdJoining").style.display = "inline";
                document.getElementById("tdGenerate").style.display = "none";
            }
        }
        function HideFilter() {
            if (document.getElementById("RadAllRecord").checked == true) {
                document.getElementById("tdDateRange").style.display = "none";
            }
            else {
                document.getElementById("tdDateRange").style.display = "inline";
            }

            if (document.getElementById("RadioButton1").checked == true) {
                document.getElementById("tdGenerate").style.display = "inline";
                document.getElementById("tdJoining").style.display = "none";
            }
            else {
                alert("455");
                document.getElementById("tdJoining").style.display = "inline";
                document.getElementById("tdGenerate").style.display = "none";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(InitializeRequest);
        prm.add_endRequest(EndRequest);
        var postBackElement;
        function InitializeRequest(sender, args) {
            if (prm.get_isInAsyncPostBack())

                args.set_cancel(true);
            postBackElement = args.get_postBackElement();
            $get('UpdateProgress1').style.display = 'block';

        }
        function EndRequest(sender, args) {
            $get('UpdateProgress1').style.display = 'none';
        }
    </script>
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Generate Offer Letter</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td style="vertical-align: top; padding-left: 30px;">
                    <div class="crossBtn"><a href='<%= Page.ResolveUrl("~/OMS/Management/ProjectMainPage.aspx")%>'><i class="fa fa-times"></i></a></div>
                </td>
            </tr>
            <tr>
                <td>
                    <table cellspacing="1" cellpadding="2" style="">
                        <tr>
                            <td>Select Date Range:
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="RadAllRecord" runat="server" GroupName="e" Checked="true" onclick="radioselect('A')" />
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
                                        <td>Confirm Joining Date:
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="txtJD" runat="server" ClientInstanceName="ctxtJD" EditFormat="Custom" UseMaskBehavior="True"
                                                TabIndex="20" Width="204px">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td>
                                            <asp:Button ID="BtnJoin" runat="server" Text="Confirm Join" CssClass="btn btn-primary"
                                                OnClick="BtnJoin_Click" /></td>
                                    </tr>
                                </table>
                                <table id="tdGenerate">
                                    <tr style="display: none;">
                                        <td class="gridcellleft">Use Template :
                                        </td>
                                        <td id="tdHeader">
                                            <asp:TextBox ID="txtHeader" runat="server" Width="279px" Font-Size="12px" onkeyup="FunTemplate(this,'GetTeplate',event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="display: none;">
                                        <td class="gridcellleft">Add Signatory :
                                        </td>
                                        <td id="tdAddSig">
                                            <asp:TextBox ID="txtSignature" runat="server" Width="279px" Font-Size="12px" onkeyup="FunAddSig(this,'SearchByEmployeesWithSignature',event)"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
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
                <td style="vertical-align: top">
                    <table width="100%">
                        <tr>
                            <td id="Td1" style="text-align: left; vertical-align: top">
                                <%--<a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>Show Filter</span></a>--%>
                                <%--<% if (rights.CanAdd)
                                               { %>--%>
                                <a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();" class="btn btn-primary"><span>Add New</span> </a>
                                <%--<%} %>--%>
                                <%--<a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>--%>
                            </td>
                            <td>
                                <span id="spanshow2" style="color: Blue; font-weight: bold"></span>&nbsp;&nbsp;<span
                                    id="spanshow3"></span>
                            </td>
                            <td class="gridcellright pull-right">
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
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                        <ProgressTemplate>
                            <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 50px; background-color: white; layer-background-color: white; height: 80; width: 150;'>
                                <table width='100' height='35' border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td height='25' align='center' bgcolor='#FFFFFF'>
                                                        <img src='/assests/images/progress.gif' width='18' height='18'></td>
                                                    <td height='10' width='100%' align='center' bgcolor='#FFFFFF'>
                                                        <font size='2' face='Tahoma'><strong align='center'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Loading..</strong></font></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
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
                                <Styles>
                                    <%--<Header SortingImageSpacing="5px" BackColor="LightSteelBlue" ImageSpacing="5px"></Header>--%>

                                    <FocusedRow CssClass="gridselectrow"></FocusedRow>

                                    <LoadingPanel ImageSpacing="10px"></LoadingPanel>

                                    <FocusedGroupRow BackColor="#FFC080"></FocusedGroupRow>
                                </Styles>
                                <%--<SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True">
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
                                            <a href="javascript:void(0);" onclick="OnMoreShowButtonClick('<%# Container.KeyValue %>')" title="More Info" style="text-decoration: none;">
                                                <img src="../../../assests/images/info.png" />
                                            </a>
                                        </DataItemTemplate>
                                        <CellStyle Wrap="False" HorizontalAlign="Center"></CellStyle>
                                        <HeaderTemplate>
                                            <%--<a href="javascript:void(0);" onclick="javascript:OnAddButtonClick();"><span style="color: #000099; text-decoration: underline">Add New</span> </a>--%>
                                            Actions
                                        </HeaderTemplate>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <%--<dxe:GridViewDataTextColumn VisibleIndex="12" Width="60px" Caption="Details">
<HeaderStyle HorizontalAlign="Center"></HeaderStyle>
<DataItemTemplate>
                                            <%if (Session["PageAccess"].ToString().Trim() == "All" || Session["PageAccess"].ToString().Trim() == "Add" || Session["PageAccess"].ToString().Trim() == "DelAdd")
                                              { %>
                                      <a href="javascript:void(0);" onclick="DeleteRow('<%# Container.KeyValue %>')">
                                                    Delete</a> 
                                            <%} %>
                                        
</DataItemTemplate>

<CellStyle Wrap="False"></CellStyle>
<HeaderTemplate>
                                            <span style="color: #000099; text-decoration: underline">Delete </span>
                                        
</HeaderTemplate>

<EditFormSettings Visible="False"></EditFormSettings>
</dxe:GridViewDataTextColumn>--%>
                                </Columns>
                                <Settings ShowGroupPanel="True" />
                            </dxe:ASPxGridView>
                            <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                            </dxe:ASPxGridViewExporter>
                            <%--  </div>--%>
                            <%--    <input id="btnRead" type="button" value="Read" class="btnUpdate" onclick="btnRead_click();"
                            style="width: 66px; height: 19px" tabindex="1" />--%>
                            <asp:HiddenField ID="HDNSelection" runat="server" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="BtnJoin" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
