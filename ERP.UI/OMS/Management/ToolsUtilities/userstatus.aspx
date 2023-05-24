<%--================================================== Revision History =============================================
1.0   Pallab    V2.0.38      23-05-2023          0026204: User Status module design modification & check in small device
====================================================== Revision History =============================================--%>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_ToolsUtilities_userstatus" CodeBehind="userstatus.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function Page_Load() {
            document.getElementById('trMessage').style.display = 'none';
            height();
        }
        function ForMessage() {
            document.getElementById('trMessage').style.display = 'inline';
        }
        document.body.style.cursor = 'pointer';
        var oldColor = '';
        function ChangeRowColor(rowID, rowNumber) {
            var gridview = document.getElementById('grdActive');
            var rCount = gridview.rows.length;
            var rowIndex = 1;
            var rowCount = 0;
            if (rCount == 28)
                rowCount = 25;
            else
                rowCount = rCount - 2;
            if (rowNumber > 25 && rCount < 28)
                rowCount = rCount - 3;
            for (rowIndex; rowIndex <= rowCount; rowIndex++) {
                var rowElement = gridview.rows[rowIndex];
                rowElement.style.backgroundColor = '#FFFFFF';
            }
            var color = document.getElementById(rowID).style.backgroundColor;
            if (color != '#ffe1ac') {
                oldColor = color;
            }
            if (color == '#ffe1ac') {
                document.getElementById(rowID).style.backgroundColor = oldColor;
            }
            else
                document.getElementById(rowID).style.backgroundColor = '#ffe1ac';

        }
    </script>

    <%--Rev 1.0--%>
    <link href="/assests/css/custom/newcustomstyle.css" rel="stylesheet" />
    
    <style>
        select
        {
            z-index: 1;
        }

        #gridAdvanceAdj {
            max-width: 99% !important;
        }
        #FormDate, #toDate, #dtTDate, #dt_PLQuote, #dt_PlQuoteExpiry {
            position: relative;
            z-index: 1;
            background: transparent;
        }

        select
        {
            -webkit-appearance: none;
        }

        .calendar-icon
        {
            right: 20px;
        }

        .panel-title h3
        {
            padding-top: 0px !important;
        }

        .fakeInput
        {
                min-height: 30px;
    border-radius: 4px;
        }
        
    </style>
    <%--Rev end 1.0--%>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <script language="javascript" type="text/javascript">
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

    <%--Rev 1.0: "outer-div-main" class add --%>
    <div class="outer-div-main clearfix">
        <div class="panel-heading">
        <div class="panel-title">
            <h3>User Status</h3>
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
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; background-color: white; layer-background-color: white;'>
                                <table border='1' cellpadding='0' cellspacing='0' bordercolor='#C0D6E4'>
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <%--<asp:GridView ID="grdActive" runat="server" Width="100%" BorderColor="CornflowerBlue"
                                ShowFooter="True" AllowSorting="true" AutoGenerateColumns="false" BorderStyle="Solid"
                                BorderWidth="2px" CellPadding="4" ForeColor="#0000C0" OnSorting="grdActive_Sorting" OnRowCreated="grdActive_RowCreated">
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("user_id")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="LogIN ID" SortExpression="user_loginid">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Center" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLogINID" runat="server" Text='<%# Eval("user_loginid")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User Name" SortExpression="UserName">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("UserName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Activity" SortExpression="user_activity">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lbluseractivity" runat="server" Text='<%# Eval("user_activity")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Segment" SortExpression="seg_name">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblsegname" runat="server" Text='<%# Eval("seg_name")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Since" SortExpression="Since">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSince" runat="server" Text='<%# Eval("Since")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Duration(MIN)" SortExpression="Duration">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDuration" runat="server" Text='<%# Eval("Duration")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IP Address" SortExpression="user_lastIP">
                                        <ItemStyle Font-Size="12px" BorderWidth="1px" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastIP" runat="server" Text='<%# Eval("user_lastIP")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField  HeaderText="Send Message">
                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkDeliveryMsg" runat="server" onclick="ForMessage()" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reset">
                                        <ItemStyle BorderWidth="1px" Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderStyle Wrap="False" HorizontalAlign="Left" Font-Bold="False"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ChkDelivery" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle BackColor="White" ForeColor="#330099" BorderColor="#BFD3EE" BorderStyle="Double"
                                    BorderWidth="1px"></RowStyle>
                                <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                    Font-Bold="False"></HeaderStyle>
                            </asp:GridView>--%>

                            <dxe:ASPxGridView ID="grdActive2" runat="server" AutoGenerateColumns="False"
                                ClientInstanceName="gridDocument" KeyFieldName="Id" Width="100%" Font-Size="12px">
                                <%--OnCustomCallback="EmployeeDocumentGrid_CustomCallback"  OnRowCommand="EmployeeDocumentGrid_RowCommand"--%>
                                <SettingsSearchPanel Visible="True" />
                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <Columns>
                                    <dxe:GridViewDataTextColumn FieldName="user_id" VisibleIndex="0" Visible="False">
                                        <EditFormSettings Visible="False" />
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="user_loginid" VisibleIndex="0" Caption="LogIN ID">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="UserName" VisibleIndex="1" Caption="User Name">
                                    </dxe:GridViewDataTextColumn>
                                    <%--<dxe:GridViewDataTextColumn FieldName="user_activity" VisibleIndex="2" Caption="Activity">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="seg_name" VisibleIndex="3" Caption="Segment">
                                    </dxe:GridViewDataTextColumn>--%>
                                    <dxe:GridViewDataTextColumn FieldName="Since" VisibleIndex="4" Caption="Last Loggedin">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="Duration" VisibleIndex="5" Caption="Duration (Minutes)">
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn FieldName="user_lastIP" VisibleIndex="6" Caption="IP Address">
                                    </dxe:GridViewDataTextColumn>
                                 <%--   <dxe:GridViewDataTextColumn VisibleIndex="13" Width="8%" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center"  ">
                                        <DataItemTemplate> 
                                            <dxe:ASPxCheckBox ID="ChkDeliveryMsg" runat="server" ClientInstanceName="chkDeliveryMsg">
                                                <ClientSideEvents CheckedChanged="function(s, e) {
                                                  ForMessage();
                                                  }"></ClientSideEvents>
                                            </dxe:ASPxCheckBox>
                                        </DataItemTemplate>
                                        <HeaderTemplate>
                                            Send Message
                                        </HeaderTemplate>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>
                                    <dxe:GridViewDataTextColumn VisibleIndex="13" Width="8%" HeaderStyle-HorizontalAlign="Center" CellStyle-HorizontalAlign="Center">
                                        <DataItemTemplate>
                                            <asp:CheckBox ID="ChkDelivery" runat="server" />
                                        </DataItemTemplate>
                                        <HeaderTemplate>
                                            Reset
                                        </HeaderTemplate>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                    </dxe:GridViewDataTextColumn>--%>
                                </Columns>
                                <Settings ShowGroupPanel="True" ShowStatusBar="Visible" />
                                <SettingsEditing Mode="PopupEditForm" PopupEditFormHeight="300px" PopupEditFormHorizontalAlign="Center"
                                    PopupEditFormModal="True" PopupEditFormVerticalAlign="WindowCenter" PopupEditFormWidth="700px"
                                    EditFormColumnCount="1" />
                                <Styles>
                                    <LoadingPanel ImageSpacing="10px">
                                    </LoadingPanel>
                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                    </Header>
                                </Styles>
                                <SettingsText PopupEditFormCaption="Add/Modify Documents" ConfirmDelete="Confirm delete?" />
                                <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True">
                                    <FirstPageButton Visible="True">
                                    </FirstPageButton>
                                    <LastPageButton Visible="True">
                                    </LastPageButton>
                                </SettingsPager>
                                <SettingsBehavior ColumnResizeMode="NextColumn" ConfirmDelete="True" />
                            </dxe:ASPxGridView>
                            <%--<dxe:ASPxPopupControl ID="ASPXPopupControl" runat="server" ContentUrl="frmAddDocuments.aspx"
                                CloseAction="CloseButton" Top="120" Left="300" ClientInstanceName="popup" Height="345px"
                                Width="900px" HeaderText="Add Document" AllowResize="true" ResizingMode="Postponed" Modal="true">
                                <ContentCollection>
                                    <dxe:PopupControlContentControl runat="server">
                                    </dxe:PopupControlContentControl>
                                </ContentCollection>
                                <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
                            </dxe:ASPxPopupControl>--%>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnMessage" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
                            <%--<asp:AsyncPostBackTrigger ControlID="chkAll" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="UnChkAll" EventName="Click" />--%>
                        </Triggers>
                    </asp:UpdatePanel>
                    <br />
                    <asp:Button ID="chkAll" runat="server" Text="Select All For Message" CssClass="btnUpdate btn btn-primary hide" OnClick="chkAll_Click" />
                    <asp:Button ID="UnChkAll" runat="server" Text="DeSelect All For Message" CssClass="btnUpdate btn btn-primary hide" OnClick="UnChkAll_Click" />
                </td>
            </tr>
            <!--<tr id="trMessage">
                <td>
                    <table width="100%">
                        <tr>
                            <td style="width: 79px">Message :
                            </td>
                            <td>
                                <asp:TextBox ID="txtMessage" runat="server" Height="81px" TextMode="MultiLine" Width="729px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnMessage" runat="server" Text="Send Message" CssClass="btnUpdate btn btn-primary"
                                    OnClick="btnMessage_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>-->
            <tr>
                <td colspan="2" style="height: 27px; display:none">
                    <asp:Button ID="btnSave" runat="server" Text="Reset" CssClass="btnUpdate btn btn-primary"
                        OnClick="btnSave_Click" />
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btnUpdate btn btn-success"
                        OnClick="btnRefresh_Click" /></td>
            </tr>
        </table>
    </div>
    </div>
</asp:Content>
