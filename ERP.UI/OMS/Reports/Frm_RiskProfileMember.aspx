<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_Frm_RiskProfileMember" Codebehind="Frm_RiskProfileMember.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script language="javascript" type="text/javascript">
    function SignOff()
    {
        window.parent.SignOff();
    }
    function height()
    {
        if(document.body.scrollHeight>=700)
            window.frameElement.height = document.body.scrollHeight;
        else
            window.frameElement.height = '700px';
        window.frameElement.Width = document.body.scrollWidth;
    }
   function ShowHideFilter(obj)
    {
           grid.PerformCallback(obj);
    } 
    
    </script>
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
            </asp:ScriptManager>

            <script language="javascript" type="text/javascript">
                  var prm = Sys.WebForms.PageRequestManager.getInstance(); 
                   prm.add_initializeRequest(InitializeRequest); 
                   prm.add_endRequest(EndRequest); 
                   var postBackElement; 
                   function InitializeRequest(sender, args) 
                   { 
                      if (prm.get_isInAsyncPostBack()) 

                      args.set_cancel(true); 
                      postBackElement = args.get_postBackElement(); 
                      $get('UpdateProgress1').style.display = 'block'; 
                         
                   } 
                   function EndRequest(sender, args) 
                   {
                     $get('UpdateProgress1').style.display = 'none';                         
                   } 
            </script>

            <table class="TableMain100">
                <tr>
                    <td class="EHEADER" style="text-align: center;">
                        <strong><span style="color: #000099">Risk Profile Member</span></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td class="gridcellleft">
                                    Type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="cmbType" runat="server" Width="100px" OnSelectedIndexChanged="cmbType_SelectedIndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Risk" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Delivery" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Fund" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="gridcellleft">
                                    Profile:
                                </td>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="cmbProfile" Width="250px" runat="server">
                                                <asp:ListItem Text="--All--" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="cmbType" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" TabIndex="3" Text="Show" CssClass="btnUpdate"
                                        OnClick="btnSave_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnExport" runat="server" TabIndex="4" Text="Export to Excel" CssClass="btnUpdate"
                                        OnClick="btnExport_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td id="Td1" align="left">
                                    <a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">
                                        Show Filter</span></a> || <a href="javascript:ShowHideFilter('All');"><span style="color: #000099;
                                            text-decoration: underline">All Records</span></a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <dxe:ASPxGridView ID="gridStatus" ClientInstanceName="grid" Width="100%" KeyFieldName="ProfileMember_ID"
                                    runat="server" AutoGenerateColumns="False" OnCustomCallback="gridStatus_CustomCallback">
                                    <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                                    <Styles>
                                        <FocusedRow CssClass="gridselectrow" BackColor="#FCA977">
                                        </FocusedRow>
                                        <FocusedGroupRow CssClass="gridselectrow" BackColor="#FCA977">
                                        </FocusedGroupRow>
                                    </Styles>
                                    <SettingsPager NumericButtonCount="20" PageSize="20" ShowSeparators="True" AlwaysShowPager="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Visible="False" FieldName="ProfileMember_ID" Caption="ID">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Visible="true" FieldName="MEMBER_NAME" Caption="Member"
                                            VisibleIndex="1">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="PROFILE_TYPE" Caption="Type">
                                            <CellStyle CssClass="gridcellleft" Wrap="True">
                                            </CellStyle>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="PROFILE_NAME" Caption="Profile Name">
                                            <CellStyle CssClass="gridcellleft" Wrap="True">
                                            </CellStyle>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="ProfileMember_Code" Caption="Profile Code">
                                            <CellStyle CssClass="gridcellleft" Wrap="True">
                                            </CellStyle>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="ProfileMember_DateFrom"
                                            Caption="From Date">
                                            <CellStyle CssClass="gridcellleft" Wrap="True">
                                            </CellStyle>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="ProfileMember_DateTo" Caption="Until Date">
                                            <CellStyle CssClass="gridcellleft" Wrap="True">
                                            </CellStyle>
                                            <EditFormSettings Visible="False"></EditFormSettings>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsText ConfirmDelete="Confirm delete?" />
                                    <StylesEditors>
                                        <ProgressBar Height="25px">
                                        </ProgressBar>
                                    </StylesEditors>
                                    <Settings ShowHorizontalScrollBar="True" />
                                </dxe:ASPxGridView>
                                <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                </dxe:ASPxGridViewExporter>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div id='Div2' style='position: absolute; font-family: arial; font-size: 30; left: 50%;
                                    top: 50px; background-color: white; layer-background-color: white; height: 80;
                                    width: 150;'>
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
            </table>
        </div>
</asp:Content>
