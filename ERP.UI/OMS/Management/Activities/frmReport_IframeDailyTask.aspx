<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.management_Activities_frmReport_IframeDailyTask" CodeBehind="frmReport_IframeDailyTask.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function frmOpenNewWindow1(location, v_height, v_weight) {
            var x = (screen.availHeight - v_height) / 2;
            var y = (screen.availWidth - v_weight) / 2
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + x + ",left=" + y + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=yes,resizable=no,dependent=no'");
        }
        function visibleProperty(obj) {
            document.getElementById('TdSelect').style.display = 'none';
            document.getElementById('TdResearch').style.display = 'inline';
            document.getElementById('spText').innerText = obj;
            height();
        }
        function Page_Load() {
            document.getElementById('TdResearch').style.display = 'none';
            height();
        }
        function btnResearch_Click() {
            document.getElementById('TdSelect').style.display = 'inline';
            document.getElementById('TdResearch').style.display = 'none';
            document.getElementById('spText').innerText = "";
        }

        FieldName = 'ctl00_ContentPlaceHolder1_Headermain1';
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>Daily Tash Sheet For Sales</h3>
        </div>

    </div>
    <div class="form_main inner">
        <table class="TableMain100">

            <tr>
                <td>
                    <span id="spText" style="color: Red"></span>
                </td>
            </tr>

            <tr>
                <td id="TdSelect">
                    <table class="TableMain100">
                        <tr>
                            <td class="gridcellleft" style=" width: 120px;">Search By User :
                            </td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="drpUser" runat="server" Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" style=" width: 120px;">Date :
                            </td>
                            <td style="text-align: left">
                                <dxe:ASPxDateEdit ID="dtDate" runat="server" EditFormat="Custom" UseMaskBehavior="True"
                                    Width="197px">
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft" style=" width: 120px;"></td>
                            <td style="text-align: left;padding-top:15px;">
                                <asp:Button ID="btnReport" runat="server" Text="Get Report" CssClass="btnUpdate btn btn-primary"
                                     OnClick="btnReport_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id="TdResearch" style="text-align: left">
                    <input id="btnResearch" type="button" value="ReSearch" onclick="btnResearch_Click()"
                        class="btnUpdate btn btn-primary" />
                </td>
            </tr>
            <tr>
                <td>

                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:GridView ID="grdReports" runat="server" AllowPaging="True" CellPadding="4" ForeColor="#333333"
                                GridLines="None"  BorderColor="#507CD1" ShowFooter="True" CssClass="gridcellleft"
                                Width="100%" OnRowDataBound="grdReports_RowDataBound">
                                <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
                                <RowStyle BackColor="#EFF3FB" BorderWidth="1px"></RowStyle>
                                <EditRowStyle BackColor="#E59930"></EditRowStyle>
                                <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" Font-Bold="True"></SelectedRowStyle>
                                <PagerStyle ForeColor="White" HorizontalAlign="Center"></PagerStyle>
                                <HeaderStyle ForeColor="Black" BorderWidth="1px" CssClass="EHEADER" BorderColor="AliceBlue"
                                    Font-Bold="False"></HeaderStyle>
                                <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
                            </asp:GridView>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnReport" EventName="Click"></asp:AsyncPostBackTrigger>
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
