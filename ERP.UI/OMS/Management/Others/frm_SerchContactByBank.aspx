<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Others.management_Others_frm_SerchContactByBank" CodeBehind="frm_SerchContactByBank.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v10.2.Export, Version=10.2.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.Export" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v10.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe000001" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">


        function PageLoad() {

            document.getElementById("txtClientID").style.display = "inline";
        }




        function selecttion() {
            var combo = document.getElementById('cmbExport');
            combo.value = '0';
        }
        function SignOff() {
            window.parent.SignOff();
        }
        function height() {

            if (document.body.scrollHeight >= 800)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '800px';
            window.frameElement.Width = document.body.scrollWidth;
        }
        function ValidatePage() {

            //selecttion();
            if (document.getElementById("txtClientID").value == '') {
                alert('Please Type Client ID!..');
                return false;
            }

        }

        function SearchOpt(obj) {

            var cmbt = document.getElementById('cmbDuplicate');
            if (cmbt.value == 'Duplicate' || cmbt.value == 'Without') {

                document.getElementById("txtClientID").style.display = "none";

            }
            else {
                document.getElementById("txtClientID").style.display = "inline";
            }

        }

        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


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
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Client Search By Bank Details</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--            <tr>
                <td class="EHEADER" style="text-align: center">
                    <strong><span style="color: #000099">Client Search By Bank Details</span></strong></td>
            </tr>--%>
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" style="background-color: #B7CEEC; border: solid 1px #ffffff"
                        border="0" width="100%">
                        <tr>
                            <td class="gridcellleft" width="70px">
                                <span style="font-weight: bold">Search By </span>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="cmbDuplicate" runat="server" onchange="SearchOpt(this.value)" class="form-control">
                                    <asp:ListItem Text="Account Number" Value="AccNo"></asp:ListItem>
                                    <asp:ListItem Text="Bank Name" Value="BankName"></asp:ListItem>
                                    <asp:ListItem Text="MICR No" Value="MICR"></asp:ListItem>
                                    <asp:ListItem Text="Branch Name" Value="Branch"></asp:ListItem>
                                    <asp:ListItem Text="Duplicate A/C Number" Value="Duplicate"></asp:ListItem>
                                    <asp:ListItem Text="Without A/C Number" Value="Without"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtClientID" runat="server" TabIndex="1" class="form-control"></asp:TextBox>
                            </td>
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
                                <%--<a href="javascript:ShowHideFilter('s');"><span style="color: #000099; text-decoration: underline">Show Filter</span></a> ||--%> <a href="javascript:ShowHideFilter('All');"><span style="color: #000099; text-decoration: underline">All Records</span></a>
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
                                        <dxe:ASPxGridView ID="gridContract" ClientInstanceName="grid" Width="100%" KeyFieldName="cbd_id"
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
                                                <dxe:GridViewDataTextColumn Visible="False" FieldName="cbd_id" Caption="ID">
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="CustomerName" Caption="Name">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="AccountName" Caption="A/C Name">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="AccountType" Caption="A/C Type">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="AccountCategory" Caption="A/C Category">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="AccountNumber" Caption="A/C Number">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="BankName" Caption="Bank">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="BranchName" Caption="Branch">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="MICR" Caption="MICR">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="Phone" Caption="Phone Number">
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
                                            <Settings ShowGroupedColumns="True" ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu="true" />
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
        <table>
            <tr style="height: 400px;">
                <td style="height: 400px;"></td>
            </tr>
        </table>
    </div>


</asp:Content>

