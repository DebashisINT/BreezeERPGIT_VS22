<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_MendatoryDoc" Codebehind="frmReport_MendatoryDoc.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   

    <script language="javascript" type="text/javascript">
      
        function PageLoad() {
            FieldName = 'btnSubmit';
            ShowCategoryFilterForm(rbCategory.GetValue());
            hide('trResult');

        }
        function ShowCategoryFilterForm(obj) { //alert(obj);
            if (obj == 'A') {
                hide('tdDocType');

            }
            if (obj == 'S') {
                show('tdDocType');

            }
        }
        function show(obj1) {
            document.getElementById(obj1).style.display = 'table-row';
        }
        function hide(obj1) {
            document.getElementById(obj1).style.display = 'none';
        }
        function Filter(obj) {
            cmbDocType.PerformCallback(obj.GetValue().toString());
        }
        function CallGridBind() {
            show('trResult');
            grid.PerformCallback();
        }
        function EndCall(obj) {
           // height();
        }
        function ShowHideFilter(obj) {
            grid.PerformCallback(obj);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      
     <div class="panel-heading">
       <div class="panel-title">
           <h3>Contacts Pending Mendatory Documents Report</h3>
       </div>

   </div> 
      <div class="form_main">
        <table class="TableMain100">
            <tr>
                <td class="gridcellleft" colspan="3" valign="top">
                    <table class="TableMain100">
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td style="text-align: left; vertical-align: top" class="hide">
                                            <table>
                                                <tr>
                                                    <td id="ShowFilter">
                                                        <a href="javascript:ShowHideFilter('s');" class="btn btn-success"><span>
                                                            Show Filter</span></a>
                                                    </td>
                                                    <td id="Td1">
                                                        <a href="javascript:ShowHideFilter('All');" class="btn btn-primary"><span>
                                                            All Records</span></a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxComboBox ID="cmbContactType" ClientInstanceName="cmbContactType" runat="server"
                                                ValueField="ContactType" EnableTheming="True" ValueType="System.String" Font-Size="12px"
                                                EnableIncrementalFiltering="True">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <DropDownButton Text="ContactType">
                                                </DropDownButton>
                                                <ClientSideEvents SelectedIndexChanged="function(s, e) {Filter(s);}" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td class="gridcellright">
                                            <span class="Ecoheadtxt"><strong>Document Type</strong></span></td>
                                        <td class="gridcellleft">
                                            <dxe:ASPxRadioButtonList ID="rbCategory" runat="server" ClientInstanceName="rbCategory"
                                                Font-Size="12px" Height="28px" ItemSpacing="10px" RepeatDirection="Horizontal"
                                                SelectedIndex="0" TextWrap="False">
                                                <Items>
                                                    <dxe:ListEditItem Text="All" Value="A" />
                                                    <dxe:ListEditItem Text="Specific" Value="S" />
                                                </Items>
                                                <ClientSideEvents ValueChanged="function(s, e) {ShowCategoryFilterForm(s.GetValue());}" />
                                                <Border BorderWidth="0px" />
                                            </dxe:ASPxRadioButtonList>
                                        </td>
                                        <td class="gridcellright" id="tdDocType">
                                            <dxe:ASPxComboBox ID="cmbDocType" runat="server" ClientInstanceName="cmbDocType"
                                                ValueField="distinct dty_documentType" ValueType="System.String" DropDownWidth="100%"
                                                TextFormatString="{0}" Font-Size="12px" OnCallback="cmbDocType_Callback" EnableIncrementalFiltering="True">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                                <DropDownButton Text="DocumentType" Width="50px">
                                                </DropDownButton>
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td class="gridcellright">
                                            <dxe:ASPxButton ID="btnSubmit" runat="server" Text="GO" AutoPostBack="False" CssClass="btn btn-primary btn-xs">
                                                <ClientSideEvents Click="function(s, e) {CallGridBind();}" />
                                            </dxe:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="gridcellright" style="display:none">
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
                    </table>
                </td>
            </tr>
            <tr id="trResult">
                <td class="gridcellleft" colspan="3">
                    <dxe:ASPxGridView ID="AspXMendatoryDocGrid" ClientInstanceName="grid" runat="server" KeyFieldName="RowNo"
                        AutoGenerateColumns="False" Width="100%" OnCustomCallback="AspXMendatoryDocGrid_CustomCallback"
                        OnHtmlRowPrepared="AspXMendatoryDocGrid_HtmlRowPrepared" OnCustomJSProperties="AspXMendatoryDocGrid_CustomJSProperties">
                        <Styles>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                            <LoadingPanel ImageSpacing="10px">
                            </LoadingPanel>
                        </Styles>
                        <Columns>
                            <dxe:GridViewDataTextColumn Caption="Contact Name" FieldName="Name" VisibleIndex="0">
                                <Settings AllowSort="False" AllowAutoFilter="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Code" FieldName="code" VisibleIndex="1">
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Phone No" FieldName="PhoneNo" VisibleIndex="2">
                                <Settings AllowSort="False" AllowAutoFilter="False" />
                            </dxe:GridViewDataTextColumn>
                            <dxe:GridViewDataTextColumn Caption="Pending Documents" FieldName="DocumentType"
                                VisibleIndex="3">
                                <Settings AllowAutoFilter="False" />
                            </dxe:GridViewDataTextColumn>
                        </Columns>
                        <SettingsPager PageSize="20" ShowSeparators="True">
                            <FirstPageButton Visible="True">
                            </FirstPageButton>
                            <LastPageButton Visible="True">
                            </LastPageButton>
                        </SettingsPager>
                        <SettingsBehavior AllowFocusedRow="True" />
                        <ClientSideEvents EndCallback="function(s, e) {
	EndCall(s.cpHeight);
}" />
                    </dxe:ASPxGridView>
                </td>
            </tr>
        </table>
        <dxe:ASPxGridViewExporter ID="exporter" runat="server" GridViewID="AspXMendatoryDocGrid">
        </dxe:ASPxGridViewExporter>
         </div> 
</asp:Content>
