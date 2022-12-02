<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frm_SerchContactByEmail" CodeBehind="frm_SerchContactByEmail.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web"
    TagPrefix="dxe000001" %>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />

    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>

    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff();
        }

        function height() {
            if (document.body.scrollHeight >= 600)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '700px';
            window.frameElement.Width = document.body.scrollWidth;
        }

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
        function SearchOpt(obj) {
            var cmbt = document.getElementById('cmbDuplicate');
            if (cmbt.value == 'Selected') {
                document.getElementById("txtClientID").style.display = "inline";
            }
            else {
                document.getElementById("txtClientID").style.display = "none";
            }
        }
    </script>
    <script language="javascript" type="text/javascript">
        //var prm = Sys.WebForms.PageRequestManager.getInstance();
        //prm.add_initializeRequest(InitializeRequest);
        //prm.add_endRequest(EndRequest);
        //var postBackElement;
        //function InitializeRequest(sender, args) {
        //    if (prm.get_isInAsyncPostBack())

        //        args.set_cancel(true);
        //    postBackElement = args.get_postBackElement();
        //    $get('UpdateProgress1').style.display = 'block';

        //}
        //function EndRequest(sender, args) {
        //    $get('UpdateProgress1').style.display = 'none';
        //}
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Client Search By Email/Phone Number</h3>
        </div>
    </div>
    <div class="form_main inner">
        <table class="TableMain100">
            <%--            <tr>
                <td class="EHEADER" style="text-align: center">
                    <strong><span style="color: #000099">Client Search By Email/Phone Number</span></strong></td>
            </tr>--%>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" 
                        border="0" width="100%">
                        <tr>
                            <td class="gridcellleft" width="70px">
                                <span style="font-weight: bold">Search Only </span>
                            </td>
                            <td class="">
                                <asp:DropDownList ID="cmbDuplicate" runat="server" onchange="SearchOpt(this.value)" class="form-control">
                                    <asp:ListItem Text="Selected" Value="Selected"></asp:ListItem>
                                    <asp:ListItem Text="Duplicate" Value="Duplicate"></asp:ListItem>
                                    <asp:ListItem Text="Without" Value="Without"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:DropDownList ID="cmbType" runat="server" class="form-control">
                                    <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                                    <asp:ListItem Text="Domain" Value="Domain"></asp:ListItem>
                                    <asp:ListItem Text="PhoneNumber" Value="PhoneNumber"></asp:ListItem>
                                    <asp:ListItem Text="PIN Code" Value="PIN Code"></asp:ListItem>
                                </asp:DropDownList>

                                <asp:TextBox ID="txtClientID" runat="server" TabIndex="1" class="form-control"></asp:TextBox>
                            </td>

                            
                        </tr>
                        <tr>
                            <td></td>
                                <td class="gridcellright">
                                <asp:Button ID="btnSave" runat="server" TabIndex="3" Text="Show" class="btn btn-primary"
                                    OnClick="btnSave_Click" />

                                <asp:Button ID="btnExport" runat="server" TabIndex="4" Text="Export to Excel" class="btn btn-danger"
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
                                <%--<a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a> ||--%> 
                                <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>All Records</span></a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>


                <td>

                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div id='Div1' style='position: absolute; font-family: arial; font-size: 30; left: 50%; top: 8; background-color: white; layer-background-color: white; height: 80; width: 150;'>
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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table class="TableMain100">
                                <tr>
                                    <td>
                                        <dxe:ASPxGridView ID="gridContract" ClientInstanceName="grid" Width="100%" KeyFieldName="EML_CNTID"
                                            runat="server" AutoGenerateColumns="False" OnCustomCallback="gridContract_CustomCallback">
                                            <SettingsBehavior AllowFocusedRow="True" ConfirmDelete="True" />
                                            <Styles>
                                                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                                </Header>
                                                <LoadingPanel ImageSpacing="10px">
                                                </LoadingPanel>
                                                <FocusedRow BackColor="#FEC6AB">
                                                </FocusedRow>
                                            </Styles>
                                            <Columns>
                                                <dxe:GridViewDataTextColumn Visible="False" FieldName="EML_CNTID" Caption="ID">
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="CONTACT_NAME" Caption="Name">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="CLIENTCODE" Caption="Code">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <%-- <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="branch" Caption="Branch">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>--%>
                                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="EML_TYPE" Caption="Type">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="EML_ENTITY" Caption="Client Type">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>

                                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="EML_EMAIL" Caption="Email">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="M_NUMBER" Caption="Phone">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="9" FieldName="branch" Caption="Branch">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>

                                            </Columns>
                                            <StylesEditors>
                                                <ProgressBar Height="25px">
                                                </ProgressBar>
                                            </StylesEditors>
                                            <SettingsBehavior AllowFocusedRow="True" AllowSort="False" AllowMultiSelection="True" />
                                            <SettingsPager AlwaysShowPager="True" NumericButtonCount="20" ShowSeparators="True"
                                                PageSize="15">
                                                <FirstPageButton Visible="True">
                                                </FirstPageButton>
                                                <LastPageButton Visible="True">
                                                </LastPageButton>
                                            </SettingsPager>
                                            <SettingsText Title="Template" />
                                            <SettingsSearchPanel Visible="True" />
                                            <Settings ShowGroupedColumns="True" ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu = "True" />
                                        </dxe:ASPxGridView>
                                        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                        </dxe:ASPxGridViewExporter>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
