<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Reports.Reports_frmReport_ConvertedLead" CodeBehind="frmReport_ConvertedLead.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script language="javascript" type="text/javascript">

        //  function ShowGridFilter(obj)
        //    {
        //        var date1 = TxtToDate.GetDate();
        //        var date2 = TxtFromDate.GetDate();
        //           
        //        if (date1== null && date2== null )
        //                {
        //            alert(" Please select Date.");
        //                }
        //        else
        //                {
        //                GridConvertedLead.PerformCallback(obj);
        //                }
        //  } 
        //  function callback()
        //        {
        //            GridConvertedLead.PerformCallback();
        //        } 
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Converted Leads</h3>
        </div>

    </div>
     <div class="crossBtn"><a href="../Management/ProjectMainPage.aspx"><i class="fa fa-times"></i></a></div>
    <div class="form_main">
        <table class="TableMain100" style="width: 100%">
            <%-- <tr>
                    <td class="EHEADER" colspan="4" style="text-align: center;">
                        <strong><span style="color: #000099">Converted Leads</span></strong>
                    </td>
                </tr>--%>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="PnlDate" runat="server" BorderWidth="1px" BorderColor="white">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td class="gridcellleft" style="padding-right:15px">
                                                                        <dxe:ASPxDateEdit ID="TxtFromDate" ClientInstanceName="TxtFromDate" UseMaskBehavior="True"
                                                                            runat="server" EditFormat="Custom" EditFormatString="dd MMMM yyyy">
                                                                            <ButtonStyle Width="13px">
                                                                            </ButtonStyle>
                                                                            <DropDownButton Text="From Date">
                                                                            </DropDownButton>
                                                                        </dxe:ASPxDateEdit>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtFromDate"
                                                                            Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td class="gridcellleft" style="padding-right:15px">
                                                                        <dxe:ASPxDateEdit ID="TxtToDate" ClientInstanceName="TxtToDate" UseMaskBehavior="True"
                                                                            runat="server" EditFormat="Custom" EditFormatString="dd MMMM yyyy">
                                                                            <ButtonStyle Width="13px">
                                                                            </ButtonStyle>
                                                                            <DropDownButton ImagePosition="Top" Text="To Date">
                                                                            </DropDownButton>
                                                                        </dxe:ASPxDateEdit>
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtToDate"
                                                                            Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!" ValidationGroup="a"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td class="" style="padding-right:15px">
                                                                        <span style="color: #000099">Report Type:</span></td>
                                                                    <td class="" style="padding-right:15px">
                                                                        <dxe:ASPxRadioButtonList ID="RBReportType" runat="server" RepeatDirection="Horizontal"
                                                                            Height="2px" SelectedIndex="0" TextSpacing="3px" CssPostfix="BlackGlass">
                                                                            <Items>
                                                                                <dxe:ListEditItem Text="Screen" Value="Screen" />
                                                                                <dxe:ListEditItem Text="Print" Value="Print" />
                                                                            </Items>
                                                                            <ValidationSettings ErrorText="Error has occurred">
                                                                                <ErrorImage Height="14px" Width="14px" />
                                                                            </ValidationSettings>
                                                                        </dxe:ASPxRadioButtonList>
                                                                    </td>
                                                                    <td class="gridcellleft">
                                                                        <dxe:ASPxButton ID="btnShowReport" runat="server" Text="Show"
                                                                            OnClick="btnShowReport_Click" ValidationGroup="a" CssClass="btn btn-success">
                                                                            <Border BorderColor="White" />
                                                                            <%--<ClientSideEvents Click="function(s,e) { ShowGridFilter('All');}" />--%>
                                                                        </dxe:ASPxButton>
                                                                    </td>
                                                                    <td class="gridcellleft"></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td aalign="right">
                                                            <table>
                                                                <tr>
                                                                    <td style="text-align: right; vertical-align: bottom;" id="TdExport" runat="server">
                                                                        <table id="Table1" runat="server" style="">
                                                                            <tr style="vertical-align: bottom;">
                                                                                <td style="vertical-align: middle">
                                                                                    <dxe:ASPxComboBox ID="cmbExport" runat="server" ValueType="System.String" Height="17px"
                                                                                        OnSelectedIndexChanged="cmbExport_SelectedIndexChanged" SelectedIndex="0" AutoPostBack="true"
                                                                                        Font-Overline="False" Font-Size="12px" Width="136px">
                                                                                        <ButtonStyle Width="13px">
                                                                                        </ButtonStyle>
                                                                                        <Items>                                                                                           
                                                                                            <dxe:ListEditItem Text="Select" Value="0" />
                                                                                            <dxe:ListEditItem Text="PDF" Value="1" />
                                                                                            <dxe:ListEditItem Text="XLS" Value="2" />
                                                                                            <dxe:ListEditItem Text="RTF" Value="3" />
                                                                                            <dxe:ListEditItem Text="CSV" Value="4" />
                                                                                        </Items>
                                                                                        <DropDownButton Text="Export" ToolTip="Export File">
                                                                                        </DropDownButton>
                                                                                    </dxe:ASPxComboBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
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
                        <%--  <tr>
                    <td style="vertical-align: top">
                    
                     <table style="width: 100%">
                                                            <tr>
                                                                <td style="text-align: left; vertical-align: top">
                                                                    <table>
                                                                        <tr>
                                                                            <td id="Td1">
                                                                                <a href="javascript:ShowGridFilter('All');"><span style="color: #000099; text-decoration: underline">
                                                                                    Show</span></a>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                    
                    </td>
                </tr>--%>
                        <tr>
                            <td style="vertical-align: top">
                                <dxe:ASPxGridView ID="GridConvertedLead" runat="server" Width="100%">
                                    <Styles>
                                        <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                        </Header>
                                        <LoadingPanel ImageSpacing="10px">
                                        </LoadingPanel>
                                    </Styles>
                                    <Columns>
                                        <dxe:GridViewDataTextColumn Caption="Name" FieldName="name" VisibleIndex="0">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Id" FieldName="cnt_internalid" VisibleIndex="1"
                                            Visible="False">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Total Leads" FieldName="Counmt" VisibleIndex="1">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Used" FieldName="Used" VisibleIndex="3">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Converted" FieldName="Converted" VisibleIndex="4">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                        <dxe:GridViewDataTextColumn Caption="Non Usable" FieldName="NonUsable" VisibleIndex="2">
                                            <CellStyle CssClass="gridcellleft">
                                            </CellStyle>
                                        </dxe:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager ShowSeparators="True">
                                        <FirstPageButton Visible="True">
                                        </FirstPageButton>
                                        <LastPageButton Visible="True">
                                        </LastPageButton>
                                    </SettingsPager>
                                </dxe:ASPxGridView>
                                <dxe:ASPxGridViewExporter ID="exporter" runat="server">
                                </dxe:ASPxGridViewExporter>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
