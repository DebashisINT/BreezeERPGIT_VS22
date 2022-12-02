<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_frm_EmailBook" CodeBehind="frm_EmailBook.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff();
        }
        function height() {
            if (document.body.scrollHeight >= 650)
                window.frameElement.height = document.body.scrollHeight;
            else
                window.frameElement.height = '650px';
            window.frameElement.Width = document.body.scrollWidth;
        }
    </script>--%>
    <script language="ecmascript" type="text/ecmascript">
        function FnHideShow(cmbSearch) {
            if (cmbSearch.GetValue().toString() == "1") {
                document.getElementById("tblSingle").style.display = 'inline';
                document.getElementById("TrMasterEmail").style.display = 'inline-grid';
            }
            else if (cmbSearch.GetValue().toString() == "2") {

                // Comment By Sudip on 15122016 - Page Show in Popup

                //document.getElementById("tblSingle").style.display = 'none';
                //document.getElementById("TrMasterEmail").style.display = 'none';
                //var docprint1 = window.open('frm_EmailList_Bulk_Print.aspx', '50', 'height=500px,width=800px,scrollbars=yes,location=yes');
                //return false;


                var url = "frm_EmailList_Bulk_Print.aspx";
                popup.SetContentUrl(url);
                popup.Show();
                popup.SetHeaderText("Bulk Print Email Book");
            }
            else if (cmbSearch.GetValue().toString() == "3") {
                //document.getElementById("tblSingle").style.display = 'none';
                //document.getElementById("TrMasterEmail").style.display = 'none';
                //var docprint1 = window.open('frm_EmailBook_list.aspx', '50', 'height=500px,width=800px,scrollbars=yes,location=yes');
                //return false;

                var url = "frm_EmailBook_list.aspx";
                popup.SetContentUrl(url);
                popup.Show();
                popup.SetHeaderText("Email Book");
                return false;
            }
        }
    </script>

    <script type="text/javascript" language="javascript">
        var contType = '';
        var addType = '';
        function CallMethode() {
            document.getElementById("TrMasterEmail").style.display = 'inline-grid';
            gridEmailMain.PerformCallback();
        }
        function AddressPrint(btnId) {
            var myArray = btnId.split("_");
            var tblId = "gridEmailMain_" + myArray[3] + "_SubGridMails_" + myArray[5] + "_0_tblEmailDetails";
            var tbl = document.getElementById(tblId);
            //gridEmailMain_dxdt9_SubGridMails_cell0_0_btnPrint
            //ref id Aspx_ContactMasterGrid_dxdt2_ASPx_AddDetails_cell0_0_btnPrint
            var disp_setting = "toolbar=yes,location=no,directories=no,menubar=no,";
            disp_setting += "scrollbars=yes,width=200, height=100, left=100, top=25";
            var docprint = window.open("", "", disp_setting);
            docprint.document.open();
            var content_vlue = tbl.innerHTML;
            docprint.document.write('<html><head><title></title>');
            docprint.document.write('</head><body onLoad="self.print()"><center><table width="100%" border="0">');
            docprint.document.write(content_vlue);
            docprint.document.write('</center></table></body></html>');
            docprint.document.close();
            docprint.focus();

        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Email List Book</h3>
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
                <td class="gridcellleft">
                    <div class="row">
                        <div class="col-md-3">
                            <label>Find By :</label>
                            <div>
                                <asp:TextBox ID="txtKeyWord" runat="server"  Width="100%"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label>Filter Type:
                            </label>
                            <div>
                                <asp:DropDownList ID="drpFilterType" runat="server" Width="100%">
                                    <asp:ListItem Text="Any" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Name" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Short Name/UCC_Code" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        
                        <div class="col-md-2">
                            <label>&nbsp</label>
                            <div>
                                <dxe:ASPxCheckBox ID="AspxChkWhole" runat="server" Text="Find whole words only.">
                                </dxe:ASPxCheckBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label style="width:100%">&nbsp</label>
                            <input type="button" id="btnFind" onclick="CallMethode();" class="btnUpdate btn btn-primary" value="Find" />
                        </div>
                    </div>
                    <table id="tblSingle" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            
                            
                            
                            
                        </tr>
                    </table>
                </td>
                <td align="right">
                    <dxe:ASPxComboBox ID="cmbSearchType" runat="server"
                        Font-Size="12px"
                        ValueType="System.String" EnableIncrementalFiltering="True">
                        <Items>
                            <dxe:ListEditItem Text="Single Search" Value="1" />
                            <dxe:ListEditItem Text="Bulk Search" Value="2" />
                            <dxe:ListEditItem Text="List All" Value="3" />
                        </Items>
                        <%--  <ButtonStyle Width="13px">
                        </ButtonStyle>
                        <DropDownButton Text="SearchType" ToolTip="Asking you for Email Book search type"
                            Width="50px">
                        </DropDownButton>--%>

                        <%--Comment By Sudip 15122016 to Change Dropdow design --%>
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
	FnHideShow(s);
}" />
                    </dxe:ASPxComboBox>

                    
                </td>
            </tr>
            <tr>
                <td id="TrMasterEmail" colspan="2">
                    <dxe:ASPxGridView ID="gridEmailMain" runat="server" AutoGenerateColumns="False"
                        ClientInstanceName="gridEmailMain"
                        DataSourceID="DataSourceContactMaster" Width="100%"
                        KeyFieldName="cnt_internalId" OnCustomCallback="gridEmailMain_CustomCallback">
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                            <Cell HorizontalAlign="Left">
                            </Cell>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="Name" FieldName="UserName" VisibleIndex="0"
                                Width="250px">
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Code" FieldName="cnt_shortName" VisibleIndex="1">
                                <CellStyle Wrap="False">
                                </CellStyle>
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <SettingsDetail ShowDetailRow="True" AllowOnlyOneMasterRowExpanded="True"></SettingsDetail>
                        <Templates>
                            <DetailRow>
                                <dxe:ASPxGridView ID="SubGridMails" KeyFieldName="eml_id" runat="server" DataSourceID="SqlDataSourceEmails"
                                    OnLoad="SubGridMails_Load" Width="100%">
                                    <Styles>
                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                        </Header>
                                        <LoadingPanel ImageSpacing="10px">
                                        </LoadingPanel>
                                    </Styles>

                                    <Columns>
                                        <dxe:GridViewDataDateColumn Caption="Email Details" VisibleIndex="0">
                                            <DataItemTemplate>
                                                <table border="0" class="TableMain2" id="tblEmailList" runat="server" cellpadding="0"
                                                    cellspacing="0">
                                                    <tr>
                                                        <td class="gridcellright" valign="top" style="width: 200px">
                                                            <span class="Ecoheadtxt" style="color: Blue"><strong>
                                                                <%# Eval("eml_type")%>
                                                            </strong></span>
                                                        </td>
                                                        <td colspan="4" class="gridcellleft" rowspan="2" style="width: 300px">
                                                            <table border="0" class="TableMain2" id="tblEmailDetails" runat="server" cellpadding="0"
                                                                cellspacing="0" style="padding-right: 2px; padding-left: 2px; padding-bottom: 2px; padding-top: 2px;">
                                                                <tr>
                                                                    <td colspan="3" class="gridcellleft">
                                                                        <%# Eval("eml_email")%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" class="gridcellleft">
                                                                        <%# Eval("eml_ccEmail")%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3" class="gridcellleft"></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td class="gridcellleft" valign="top" style="width: 80px">
                                                            <span class="Ecoheadtxt" style="color: Blue"><strong><a id="btnPrint" href="#" onclick="javascript:AddressPrint(this.id);"
                                                                runat="server">Print</a></strong></span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </DataItemTemplate>
                                            <EditCellStyle HorizontalAlign="Left">
                                            </EditCellStyle>
                                        </dxe:GridViewDataDateColumn>
                                    </Columns>
                                    <%--<StylesEditors>
                                        <ProgressBar Height="25px">
                                        </ProgressBar>
                                    </StylesEditors>
                                    <SettingsPager Mode="ShowAllRecords" PageSize="20">
                                    </SettingsPager>--%>
                                    <SettingsDetail AllowOnlyOneMasterRowExpanded="True" IsDetailGrid="True" />
                                </dxe:ASPxGridView>
                            </DetailRow>
                        </Templates>
                        <SettingsPager PageSize="20">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                    </dxe:ASPxGridView>
                </td>
            </tr>

        </table>
        <dxe:ASPxPopupControl ID="ASPXPopupControl_Bulk" runat="server" ContentUrl="frm_EmailBook_list.aspx"
            CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="popup"
            Width="900px" Height="300px" HeaderText="Add Document" AllowResize="true" ResizingMode="Postponed">
            <ContentCollection>
                <dxe:PopupControlContentControl runat="server">
                </dxe:PopupControlContentControl>
            </ContentCollection>
            <HeaderStyle BackColor="Blue" Font-Bold="True" ForeColor="White" />
        </dxe:ASPxPopupControl>
        <asp:SqlDataSource ID="DataSourceContactMaster" runat="server" ></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSourceEmails" runat="server" ></asp:SqlDataSource>
    </div>
</asp:Content>
