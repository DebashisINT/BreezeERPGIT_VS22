<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_frm_SearchByAddress" EnableEventValidation="false" CodeBehind="frm_SearchByAddress.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function selecttion() {
            var combo = document.getElementById('cmbExport');
            combo.value = '0';
        }

        function ValidatePage() {

            selecttion();
            //      if(document.getElementById("txtClientID").value=='')
            //      {
            //         alert('Please Type Client ID!..');
            //         return false;
            //      }

        }

        //function ShowHideFilter(obj) {
        //    grid.PerformCallback(obj);
        //}
        function SearchOpt(obj) {

            var cmbt = document.getElementById('cmbDuplicate');
            if (cmbt.value == 'None') {
                document.getElementById("txtClientID").style.display = "inline";
                document.getElementById("TrFilter").style.display = "none"
            }
            else if (cmbt.value == 'PANEXEMPT') {
                document.getElementById("txtClientID").style.display = "none";
                document.getElementById("TrFilter").style.display = "inline";


            }
            else {
                document.getElementById("txtClientID").style.display = "none";
                document.getElementById("TrFilter").style.display = "none";
            }

        }
        function ShowHideFilter(obj) {
            if (document.getElementById('TxtSeg').value == 'N') {
                document.getElementById('TxtTCODE').style.display = "none";
            }
            else {
                document.getElementById('TxtTCODE').style.display = "inline";
            }
            //InitialTextVal();
            if (obj == "s")
                document.getElementById('TrFilter').style.display = "inline";
            else {
                document.getElementById('TrFilter').style.display = "none";
                grid.PerformCallback(obj);
            }
        }
        function btnSearch_click() {
            document.getElementById('TrFilter').style.display = "none";
            grid.PerformCallback('s');
        }
        function InitialTextVal() {


            document.getElementById('txtName').value = "ADD1";
            document.getElementById('txtBranchName').value = "ADD2";
            document.getElementById('txtCode').value = "ADD3";
            document.getElementById('txtRelationManager').value = "Landmark";
            document.getElementById('txtReferedBy').value = "Country";
            document.getElementById('txtPhNumber').value = "State";
            document.getElementById('txtContactStatus').value = "City";
            //        document.getElementById('txtStatus').value = "Status";

            document.getElementById('TxtTCODE').value = "Area";
            //document.getElementById('txtPAN').value = "PAN No.";
        }
        function ClearTextboxes() {
            document.getElementById('txtName').value = '';

            document.getElementById('txtBranchName').value = '';
            document.getElementById('txtCode').value = '';
            document.getElementById('TxtTCODE').value = '';
            document.getElementById('txtPAN').value = '';
            document.getElementById('txtRelationManager').value = '';
            document.getElementById('txtReferedBy').value = '';
            document.getElementById('txtPhNumber').value = '';
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
            <h3>Client Search By Address</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td>
                    <table cellspacing="0" cellpadding="0" width="100%">
                        <tr>
                            <td class="gridcellleft" width="70px">
                                <span style="font-weight: bold">Search Only </span>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="cmbDuplicate" runat="server" onchange="SearchOpt(this.value)" OnSelectedIndexChanged="cmbDuplicate_SelectedIndexChanged" class="form-control">
                                    <asp:ListItem Text="Keyword" Value="None"></asp:ListItem>
                                    <asp:ListItem Text="Duplicate" Value="Duplicate"></asp:ListItem>
                                    <asp:ListItem Text="Specific" Value="PANEXEMPT"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="cmbType" runat="server" onchange="SetDuplicate(this.value)" class="form-control">
                                    <asp:ListItem Text="ADDRESS" Value="Pancard">
                                    </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtClientID" runat="server" TabIndex="1" class="form-control"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellright">
                                <asp:Button ID="btnSave" runat="server" TabIndex="3" Text="Show" class="btn btn-primary"
                                    OnClick="btnSave_Click" />

                                <asp:Button ID="btnExport" runat="server" TabIndex="4" Text="Export to Excel" class="btn btn-warning"
                                    OnClick="btnExport_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="TrFilter" runat="server">
                <td>
                    <table>
                        <tr>

                            <td>
                                <asp:TextBox ID="txtName" runat="server" CssClass="water" Text="" ToolTip="Address1"
                                    Font-Size="12px" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBranchName" runat="server" CssClass="water" Text="" ToolTip="Address2"
                                    Font-Size="12px" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCode" runat="server" CssClass="water" Text="" ToolTip="Address3"
                                    Font-Size="12px" Width="120px"></asp:TextBox>
                            </td>

                            <td>
                                <asp:TextBox ID="TxtTCODE" runat="server" CssClass="water" Text="" ToolTip="Landmark"
                                    Font-Size="12px" Width="120px"></asp:TextBox>
                                <asp:HiddenField ID="TxtSeg" runat="server" />
                            </td>

                            <td>
                                <asp:TextBox ID="txtPAN" runat="server" CssClass="water" Text="" ToolTip="Country"
                                    Font-Size="12px" Width="120px"></asp:TextBox>
                            </td>


                            <td>
                                <asp:TextBox ID="txtRelationManager" runat="server" CssClass="water" Text="" ToolTip="State"
                                    Font-Size="12px" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferedBy" runat="server" CssClass="water" Text="" ToolTip="City"
                                    Font-Size="12px" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhNumber" runat="server" CssClass="water" Text="" ToolTip="Area" Style="display: none"
                                    Font-Size="12px" Width="110px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtpin" runat="server" CssClass="water" Text="" ToolTip="Pin"
                                    Font-Size="12px" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox2" runat="server" CssClass="water" onclick="ClearTextboxes(this.value)" Text="" ToolTip="Area" Style="display: none"
                                    Font-Size="12px" Width="80px"></asp:TextBox>
                            </td>



                            <%--  <td visible="false">
                                    <asp:TextBox ID="txtStatus" runat="server" CssClass="water" Text="Status" ToolTip="Status"
                                        Font-Size="12px" Width="97px"></asp:TextBox>
                                </td>--%>
                            <%-- <td>
                                    <input id="btnSearch" type="button" value="Search" class="btnUpdate" style="height: 21px" onclick="btnSearch_click()" />
                                </td>--%>
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
                                        <dxe:ASPxGridView ID="gridContract" ClientInstanceName="grid" Width="100%" KeyFieldName="RowNum"
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
                                                <%--<dxe:GridViewDataTextColumn Visible="False" FieldName="ID" Caption="ID">
                                                    <CellStyle CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>--%>
                                                <dxe:GridViewDataTextColumn VisibleIndex="2" FieldName="ClientName" Caption="Name">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="3" FieldName="Ucc" Caption="Ucc">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="4" FieldName="AddRess1" Caption="Address1">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="5" FieldName="AddRess2" Caption="Address2">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="AddRess3" Caption="Address3">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="landmark" Caption="Landmark">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="CountryName" Caption="Country">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="6" FieldName="StateName" Caption="State">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="7" FieldName="CityName" Caption="City">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="AreaName" Caption="Area">
                                                    <CellStyle Wrap="True" CssClass="gridcellleft">
                                                    </CellStyle>
                                                    <EditFormSettings Visible="False"></EditFormSettings>
                                                </dxe:GridViewDataTextColumn>
                                                <dxe:GridViewDataTextColumn VisibleIndex="8" FieldName="AddPin" Caption="PinNo">
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
                                            <SettingsSearchPanel Visible="True" />
                                            <SettingsText Title="Template" />
                                            <Settings ShowGroupedColumns="True" ShowGroupPanel="True" ShowFilterRow="true" ShowFilterRowMenu = "true"/>
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
