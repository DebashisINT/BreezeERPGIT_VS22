<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frm_ReallocatePhonecall" CodeBehind="frm_ReallocatePhonecall.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../CSS/style.css" rel="stylesheet" type="text/css" />
    <link type="text/css" rel="stylesheet" href="../CSS/dhtmlgoodies_calendar.css?random=20051112" media="screen" />
    <script type="text/javascript" src="/assests/js/dhtmlgoodies_calendar.js?random=20060118"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:Panel ID="pnlData" runat="server" Width="100%">
            <table class="TableMain">
                <tr>
                    <td class="Ecoheadtxt" style="width: 22%">Activity Type :&nbsp;</td>
                    <td style="width: 175px">
                        <asp:DropDownList ID="drpActType" Enabled="false" AutoPostBack="true" CssClass="Ecoheadtxt" runat="server" Width="100%"></asp:DropDownList>
                    </td>
                    <td class="Ecoheadtxt" style="width: 15%">Start Date/Start Time&nbsp;:<%-- <asp:TextBox ID="txtStartDate" runat ="server" ></asp:TextBox>--%>
                    </td>
                    <td style="width: 177px">
                        <asp:TextBox ID="TxtStartDate" runat="server"></asp:TextBox>
                        <asp:Image ID="ImgStart" runat="server" ImageUrl="~/images/calendar.jpg" />
                    </td>
                    <td>
                        <%--<asp:Panel ID="pnlEndTime" runat ="server" Font-Bold="True" ></asp:Panel>--%>
                    </td>
                </tr>
                <tr>
                    <td class="Ecoheadtxt" style="width: 22%">Assign To :</td>
                    <td style="width: 175px">
                        <asp:DropDownList ID="drpUserWork" runat="server" Width="100%" CssClass="Ecoheadtxt"></asp:DropDownList>
                    </td>
                    <td class="Ecoheadtxt" style="width: 15%">End Date/End Time :</td>
                    <td valign="top" style="width: 177px">
                        <asp:TextBox ID="TxtEndDate" runat="server"></asp:TextBox>
                        <asp:Image ID="ImgEnd" runat="server" ImageUrl="~/images/calendar.jpg" />
                    </td>
                    <td>
                        <%--<asp:Panel ID="pnlTime" runat ="server" ></asp:Panel> --%>                                           
                    </td>
                </tr>
                <tr>
                    <td class="Ecoheadtxt" style="width: 22%">Description :</td>
                    <td style="width: 175px">
                        <asp:TextBox ID="txtDesc" TextMode="MultiLine" Rows="2" runat="server" ForeColor="blue" Font-Size="12px"></asp:TextBox>
                    </td>
                    <td class="Ecoheadtxt" style="width: 15%">Priority :</td>
                    <td style="width: 177px">
                        <asp:DropDownList ID="drpPriority" runat="server" CssClass="Ecoheadtxt">
                            <asp:ListItem Text="Low" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Normal" Value="1"></asp:ListItem>
                            <asp:ListItem Text="High" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Urgent" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Immediate" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <table class="TableMain100">
                            <tr>
                                <td class="Ecoheadtxt" style="width: 22%">Instruction Notes :</td>
                                <td class="Ecoheadtxt" style="text-align: left">
                                    <asp:TextBox ID="txtInstNote" runat="server" TextMode="MultiLine" Rows="5" Width="187px" ForeColor="blue" Font-Size="12px"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <table class="TableMain">
            <tr>
                <td class="Ecoheadtxt" style="width: 21%; text-align: left">No. of Contact Alloted if no item select
                </td>
                <td class="Ecoheadtxt" style="text-align: left">
                    <asp:TextBox ID="txtNoCont" Text="0" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblAdd" runat="server" Visible="False" Text="Address KeyWord" CssClass="Ecoheadtxt" Height="21px" Width="133px"></asp:Label>
                    <asp:Button ID="btnCondition" runat="server" Text="Additional Condition" CssClass="btnUpdate" OnClick="btnCondition_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnlCondition" runat="server" Visible="false" Width="100%">
                        <asp:TextBox ID="txtCon1" runat="server"></asp:TextBox>
                        <asp:DropDownList ID="drpAndOr1" runat="server">
                            <asp:ListItem Text="Or" Value="Or"></asp:ListItem>
                            <asp:ListItem Text="And" Value="And"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtCon2" runat="server"></asp:TextBox>
                        <asp:DropDownList ID="drpAndOr2" runat="server">
                            <asp:ListItem Text="Or" Value="Or"></asp:ListItem>
                            <asp:ListItem Text="And" Value="And"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txtCon3" runat="server"></asp:TextBox>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="grdCalls" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="10" CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1" OnPageIndexChanging="grdCalls_PageIndexChanging">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle Font-Bold="false" ForeColor="black" CssClass="EHEADER" BorderColor="AliceBlue" BorderWidth="1px" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk1" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False" HeaderText="Lead Id">
                                <ItemTemplate>
                                    <asp:Label ID="lblLeadid" Text='<%# Eval("Leadid") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="lbl2" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="225px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Alloted by">
                                <ItemTemplate>
                                    <asp:Label ID="lbl3" runat="server" Text='<%# Eval("AllotedBy") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="225px" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False" HeaderText="Activity Id">
                                <ItemTemplate>
                                    <asp:Label ID="lblActid" Text='<%# Eval("Actid") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False" HeaderText="Phonce call Id">
                                <ItemTemplate>
                                    <asp:Label ID="lblPhoneCall" runat="server" Text='<%# Eval("PhoneCallid") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SChedule Start Date">
                                <ItemTemplate>
                                    <asp:Label ID="lbl6" runat="server" Text='<%# Eval("SchDate") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="150px" />
                            </asp:TemplateField>

                        </Columns>
                        <HeaderStyle CssClass="gridheader" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnSubmit" Text="Save" CssClass="btnUpdate" runat="server" OnClientClick='return compareDates ("0;txtStartDate;txtEndDate")' Width="95px" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnCancel1" Text="Cancel" CssClass="btnUpdate" runat="server" Width="95px" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
