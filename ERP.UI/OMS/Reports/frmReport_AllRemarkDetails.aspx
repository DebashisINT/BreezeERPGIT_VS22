<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_AllRemarkDetails" CodeBehind="frmReport_AllRemarkDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <script type="text/javascript" language="javascript">

        function PageLoad() {
            FieldName = 'ctl00_ContentPlaceHolder3_btnShow';
            ShowCategoryFilterForm(rbCategory.GetValue());
            hide('trResultGrid');
            hide('FilterTr')
            hide('td_export');
        }
        function ShowCategoryFilterForm(obj) { //alert(obj);
            if (obj == 'A') {
                hide('tdCatType');

            }
            if (obj == 'S') {
                show('tdCatType');

            }
        }

        function show(obj1) {
            //alert(obj1);
            document.getElementById(obj1).style.display = 'inline';
        }
        function hide(obj1) {
            document.getElementById(obj1).style.display = 'none';
        }


    </script>
    <script type="text/ecmascript">
        function CallBack() {
            show('trResultGrid');
            hide('FilterTr');
            grid.PerformCallback();
            show('td_export');
        }
        function FilterClick() {
            if (document.getElementById('FilterTr').style.display == 'inline') {
                hide('FilterTr');
            }
            else {
                show('FilterTr');
            }
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="panel-heading">
       <div class="panel-title">
           <h3>All Remark Details Report</h3>
       </div>

   </div>
     <div class="form_main">

        <table class="TableMain100">
          
        </table>
        <table>
            <tr>
                <td class="gridcellleft" valign="top" colspan="2">
                    <table>
                        <tr>
                            <td>
                                <span class="Ecoheadtxt" style="color: Blue"><strong>Category Type:</strong></span></td>
                            <td>
                                <dxe:ASPxRadioButtonList ID="rbCategory" ClientInstanceName="rbCategory" runat="server" SelectedIndex="0" ItemSpacing="10px" RepeatDirection="Horizontal" TextWrap="False" Font-Size="12px" Height="28px">
                                    <Items>
                                        <dxe:ListEditItem Text="All" Value="A" />
                                        <dxe:ListEditItem Text="Specific" Value="S" />
                                    </Items>
                                    <ClientSideEvents ValueChanged="function(s, e) {ShowCategoryFilterForm(s.GetValue());}" />
                                    <Border BorderWidth="0px" />
                                </dxe:ASPxRadioButtonList>
                            </td>
                            <td valign="middle" id="tdCatType">
                                <dxe:ASPxComboBox ID="cmbRemarkType" runat="server"
                                    ValueType="System.String" Width="130px" Font-Size="12px">
                                    <ButtonStyle Width="13px">
                                    </ButtonStyle>
                                    <DropDownButton Text="RemarkType" Width="50px">
                                    </DropDownButton>
                                </dxe:ASPxComboBox>
                            </td>
                            <td valign="middle" class="gridcellright">
                                <span class="Ecoheadtxt" style="color: Blue"><strong>Remark Value</strong></span>
                            </td>
                            <td valign="middle" class="gridcellleft">
                                <asp:TextBox ID="txtRemValue" runat="server"></asp:TextBox></td>
                            <td class="gridcellleft" valign="middle">
                                <dxe:ASPxButton ID="btnSubmit" runat="server" Font-Size="12px"
                                    Text="GO" AutoPostBack="False" CssClass="btn btn-primary">
                                    <ClientSideEvents Click="function(s, e) {CallBack();}" />
                                </dxe:ASPxButton>
                            </td>
                            <td class="gridcellleft" valign="middle"></td>
                            <td class="gridcellleft" valign="middle">
                                <a href="#" id="btnFilter" onclick="FilterClick();"><span class="Ecoheadtxt" style="color: Blue"><strong>Filter Columns</strong></span></a>
                            </td>

                        </tr>
                    </table>
                </td>
                <td rowspan="3" id="FilterTr" align="right" valign="top">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt" style="color: Blue"><strong>Column Name</strong></span>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <dxe:ASPxCheckBox ID="chkName" ClientInstanceName="chkName" runat="server" Text="Name" Checked="True" ValueChecked="Y" ValueType="System.Char" ValueUnchecked="N"></dxe:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <dxe:ASPxCheckBox ID="chkShortNm" ClientInstanceName="chkShortNm" runat="server" Text="ShortName/Code" Checked="True" ValueChecked="Y" ValueType="System.Char" ValueUnchecked="N"></dxe:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <dxe:ASPxCheckBox ID="chkBranch" ClientInstanceName="chkBranch" runat="server" Text="Branch" Checked="True" ValueChecked="Y" ValueType="System.Char" ValueUnchecked="N"></dxe:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <dxe:ASPxCheckBox ID="chkPhones" ClientInstanceName="chkPhones" runat="server" Text="Phones" Checked="True" ValueChecked="Y" ValueType="System.Char" ValueUnchecked="N"></dxe:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <dxe:ASPxCheckBox ID="chkRm" ClientInstanceName="chkRm" runat="server" Text="RM" Checked="True" ValueChecked="Y" ValueType="System.Char" ValueUnchecked="N"></dxe:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <dxe:ASPxCheckBox ID="chkReff" runat="server" ClientInstanceName="chkReff" Text="RefferdBy" Checked="True" ValueChecked="Y" ValueType="System.Char" ValueUnchecked="N"></dxe:ASPxCheckBox>
                            </td>

                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <dxe:ASPxCheckBox ID="chkpan" ClientInstanceName="chkpan" runat="server" Text="Pan" Checked="True" ValueChecked="Y" ValueType="System.Char" ValueUnchecked="N"></dxe:ASPxCheckBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="gridcellright" align="right" id="td_export" runat="server">
                    <dxe:ASPxComboBox ID="cmbExport" runat="server" AutoPostBack="true" BackColor="Navy"
                        Font-Bold="False" ForeColor="White" OnSelectedIndexChanged="cmbExport_SelectedIndexChanged"
                        ValueType="System.Int32" Width="130px">
                        <Items>
                            <dxe:ListEditItem Text="Select" Value="0" />
                            <dxe:ListEditItem Text="PDF" Value="1" />
                            <dxe:ListEditItem Text="XLS" Value="2" />
                            <dxe:ListEditItem Text="RTF" Value="3" />
                            <dxe:ListEditItem Text="CSV" Value="4" />
                        </Items>
                        <ButtonStyle BackColor="#C0C0FF" ForeColor="Black">
                        </ButtonStyle>
                        <ItemStyle BackColor="Navy" ForeColor="White">
                            <HoverStyle BackColor="#8080FF" ForeColor="White">
                            </HoverStyle>
                        </ItemStyle>
                        <Border BorderColor="White" />
                        <DropDownButton Text="Export">
                        </DropDownButton>
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr id="trResultGrid">
                <td colspan="2" rowspan="2">
                    <dxe:ASPxGridView ID="ASPxRemarkGrid" ClientInstanceName="grid" KeyFieldName="SlNo" runat="server" OnCustomCallback="ASPxRemarkGrid_CustomCallback" Width="100%">
                        <Settings ShowGroupPanel="True" />
                        <SettingsBehavior AutoExpandAllGroups="False" AllowFocusedRow="True" />
                        <Styles>
                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <StylesEditors>
                            <ProgressBar Height="25px">
                            </ProgressBar>
                        </StylesEditors>
                        <SettingsPager AlwaysShowPager="True" PageSize="20" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                    </dxe:ASPxGridView>
                </td>
            </tr>
            <tr>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server">
        </dxe:ASPxGridViewExporter>
    </div>
</asp:Content>
