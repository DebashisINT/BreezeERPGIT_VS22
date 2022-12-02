<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Management.management_frm_SalesVisitActivity" CodeBehind="frm_SalesVisitActivity.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>
    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff()
        }
        function height() {
            window.frameElement.height = document.body.scrollHeight;
            window.frameElement.widht = document.body.scrollWidht;
        }

        var chkobj;
        var objchk = null;
        var obGenral = null;
        function chkGenral(objGenral, val12) {
            var st = document.getElementById("txtGrdContact")

            if (obGenral == null) {
                obGenral = objGenral;
            }
            else {
                obGenral.checked = false;
                obGenral = objGenral;
                obGenral.checked = true;
            }
            st.value = val12;
        }
        function btnAddProductWindow() {
            frmOpenNewWindow1("frmAddProductBrochures.aspx", 600, 600)
        }
        function frmOpenNewWindow1(location, v_height, v_weight) {
            var x = (screen.availHeight - v_height) / 2;
            var y = (screen.availWidth - v_weight) / 2
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + x + ",left=" + y + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=yes,resizable=no,dependent=no'");
        }
        function funAddLead() {
            var val123 = document.getElementById("drpUserWork").value;
            var str = 'frm_AddLead.aspx?Call=SalesVisit&user=' + val123;
            frmOpenNewWindow1(str, 500, 800)
        }
        function funSaveClick() {
            var st = document.getElementById("txtLeadId")
            frmOpenNewWindow1('frmCancelMeeting.aspx?id=' + st.value, 500, 500)
        }
        function funCheckFunction() {
            var st = document.getElementById("txtLeadId")
            frmOpenNewWindow1('frmupdateMeeting.aspx?id=' + st.value, 500, 500)
        }
        function windowopenform() {
            var st = document.getElementById("txtLeadId")
            frmOpenNewWindow1('frmAllot_sales.aspx?id=' + st.value, 500, 500)
        }
        function windowopenform1() {
            var st = document.getElementById("txtLeadId")
            frmOpenNewWindow1('frmalloat.aspx?id=' + st.value, 500, 500)
        }
        function frmOpenNewWindow_custom(location, v_height, v_weight, top, left) {
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + top + ",left=" + left + ",location=no,directories=no,menubar=no,toolbar=no,status=no,scrollbars=yes,resizable=no,dependent=no'");
        }
        function FillLeadId(obj, val) {
            var ob = document.getElementById("txtLeadId")
            if (ob.value == null) {
                if (obj.checked == true) {
                    ob.value = val + ','
                }
            }
            else {
                if (obj.checked == true) {
                    ob.value = ob.value + val + ','
                }
                else {
                    var st = ob.value.split(",")
                    //ob.value = null;
                    var tt = ''
                    ob.value = tt;
                    for (var i = 0; i < st.length; i++) {
                        if (st[i] == val) {

                        }
                        else {
                            if (st[i] == tt) {
                            }
                            else {
                                ob.value = ob.value + st[i] + ',';
                            }
                        }
                    }
                }
            }
        }
        function fun(obj, str) {
            document.getElementById("drpProduct").disabled = str;
        }

        function TextVal1() {
            var btn = document.getElementById("btnGenratedSales");
            btn.click();
            var btn1 = document.getElementById("btnshowGenratedSaleVisit");
            btn1.click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td class="EHEADER"></td>
            </tr>
            <tr>
                <td style="text-align: left; height: auto">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnCreate" runat="server" Text="Create" CssClass="btnUpdate" OnClick="btnCreate_Click" Height="21px" />
                            </td>
                            <td>
                                <asp:Button ID="btnModify" runat="server" Text="Modify" CssClass="btnUpdate" OnClick="btnModify_Click" Visible="False" Height="21px" />
                            </td>
                            <td>
                                <asp:Button ID="btnReallocateSalesVisit" runat="server" Text="Alloted Sales Visit" CssClass="btnUpdate" OnClick="btnReallocateSalesVisit_Click" Height="21px" />
                            </td>
                            <td>
                                <asp:Button ID="btnGenratedSales" runat="server" Text="Genrated Sales" CssClass="btnUpdate" Height="21px" OnClick="btnGenratedSales_Click" />
                            </td>
                            <td>
                                <asp:Button ID="btnReAssign1" runat="server" Text="Reassign Sales Visit" CssClass="btnUpdate" Height="21px" Visible="False" />
                            </td>
                            <td>
                                <asp:Button ID="btnMainCancel" runat="server" Text="Cancel" CssClass="btnUpdate" Visible="False" OnClick="btnMainCancel_Click" Height="21px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; height: auto" runat="server" id="PnlBtn" visible="false">
                    <table>
                        <tr>
                            <td style="height: 30px">
                                <asp:Button ID="btnReassign" runat="server" Text="Reassign" CssClass="btnUpdate" OnClick="btnReassign_Click" Height="21px" />
                            </td>
                            <td style="height: 30px">
                                <asp:Button ID="btnReschedule" runat="server" Text="Reschedule" CssClass="btnUpdate" OnClick="btnReschedule_Click" Height="21px" />
                            </td>
                            <td style="height: 30px">
                                <asp:Button ID="btnShowDetail" runat="server" Text="Show Details" CssClass="btnUpdate" OnClick="btnShowDetail_Click" Height="21px" />
                            </td>
                            <td style="height: 30px">
                                <asp:Button ID="btnDelegate" runat="server" Text="Delegate To" CssClass="btnUpdate" Height="21px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td runat="server" id="userInfo1" style="height: auto">
                    <asp:GridView EnableViewState="true" ID="grdUserInfo" AutoGenerateColumns="false" runat="server" AllowPaging="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1" OnPageIndexChanging="grdUserInfo_PageIndexChanging">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle Font-Bold="false" ForeColor="black" BorderColor="AliceBlue" BorderWidth="1px" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:HyperLinkField HeaderText="User Id" DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0} &amp; type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="UserId" Visible="False" DataNavigateUrlFields="UserId" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="SNo" HeaderText="S.No." Visible="False" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="user" HeaderText="User" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Pending Acttivity" HeaderText="Pending Activity" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Scheduled End Date" HeaderText="Scheduled End Date" DataNavigateUrlFields="userid">
                                <ControlStyle Width="90px" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Expected End Date" HeaderText="Expected End Date" DataNavigateUrlFields="userid">
                                <ControlStyle Width="90px" />
                            </asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Pending Call" HeaderText="Pending Calls" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Int/Pipeline" HeaderText="Int/Pipeline" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Refixed By FOS" HeaderText="Refixed By FOS" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Refixed By Client" HeaderText="Refixed By Client" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Won/Confirm Sale" HeaderText="Won/Confirm Sale" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Non Contactable" HeaderText="Non Contactable" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Non Usable/Fake" HeaderText="Non Usable" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Lost/Not Int" HeaderText="Lost" DataNavigateUrlFields="userid" />
                            <asp:HyperLinkField DataNavigateUrlFormatString="~/management/frm_SalesVisitActivity.aspx?id={0}&amp;type=SW" NavigateUrl="~/management/frm_SalesVisitActivity.aspx" DataTextField="Refixed By TeleCaller" HeaderText="Refixed By TeleCaller" DataNavigateUrlFields="userid" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; text-align: left; height: auto">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblUserName" runat="server" Text="UserName" ForeColor="Red" Visible="False"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txtUser" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td runat="server" id="pnlShowDetail" style="height: auto">
                    <asp:GridView ID="grdDetail" runat="server" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1" AllowPaging="True" OnRowDataBound="grdDetail_RowDataBound" OnPageIndexChanging="grdDetail_PageIndexChanging">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle Font-Bold="false" ForeColor="black" CssClass="EHEADER" BorderColor="AliceBlue" BorderWidth="1px" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Activity">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkDetail" runat="server" />
                                    <asp:Label Visible="False" ID="lblActNo" runat="Server" Text='<%# Eval("Activity No") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gridheader" />
                    </asp:GridView>
                    &nbsp;
                <asp:TextBox ID="txtId" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td id="frmShowCall" runat="Server">
                    <asp:Panel ID="pnlCall" runat="server" Width="100%" Visible="false">
                        <table class="TableMain100">
                            <tr>
                                <td class="Ecoheadtxt" style="width: 8%">Activity Type :
                                </td>
                                <td class="Ecoheadtxt" style="width: 5%; text-align: left;">
                                    <asp:DropDownList ID="drpActType" AutoPostBack="true" CssClass="Ecoheadtxt" runat="server" Width="80%" Enabled="False"></asp:DropDownList>
                                </td>
                                <td class="Ecoheadtxt" style="width: 4%">Start Date/Start Time&nbsp;:
                                </td>
                                <td class="Ecoheadtxt" style="width: 7%; text-align: left;">
                                    <%--<asp:TextBox ID="TxtStartDate" runat="server"></asp:TextBox>
                     <asp:Image ID="ImgStartDate" runat="server" ImageUrl="~/images/calendar.jpg" />
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtStartDate"
                         ErrorMessage="Required" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                    <dxe:ASPxDateEdit ID="TxtStartDate" runat="server"
                                        EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td class="Ecoheadtxt" style="width: 8%"></td>
                            </tr>
                            <tr>
                                <td class="Ecoheadtxt" style="width: 8%">Assign To :
                                </td>
                                <td class="Ecoheadtxt" style="width: 5%; text-align: left;">
                                    <asp:DropDownList ID="drpUserWork" CssClass="Ecoheadtxt" runat="server" Width="80%"></asp:DropDownList>
                                </td>
                                <td class="Ecoheadtxt" style="width: 4%">End Date/End Time :</td>
                                <td class="Ecoheadtxt" style="width: 7%; text-align: left;">
                                    <%--<asp:TextBox ID="TxtEndDate" runat="server"></asp:TextBox>
                      <asp:Image ID="ImgEndDate" runat="server" ImageUrl="~/images/calendar.jpg" />
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtEndDate"
                          ErrorMessage="Required" ValidationGroup="a"></asp:RequiredFieldValidator>--%>
                                    <dxe:ASPxDateEdit ID="TxtEndDate" runat="server"
                                        EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True">
                                        <ButtonStyle Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td style="width: 8%; text-align: left;">
                                    <input type="button" id="btnAddDocument" name="btnAddDocument" class="btnUpdate" value="Add Product" onclick="btnAddProductWindow()" style="height: 21px" /></td>
                            </tr>
                            <tr>
                                <td class="Ecoheadtxt" style="width: 8%">Description :
                                </td>
                                <td style="width: 5%; text-align: left;">
                                    <asp:TextBox ID="txtDesc" TextMode="MultiLine" Rows="2" runat="server" Width="80%" Font-Size="12px" ForeColor="blue"></asp:TextBox>
                                </td>
                                <td class="Ecoheadtxt" style="width: 4%">Priority :
                                </td>
                                <td class="Ecoheadtxt" style="width: 7%; text-align: left;">
                                    <asp:DropDownList ID="drpPriority" runat="server" CssClass="Ecoheadtxt" Width="65%">
                                        <asp:ListItem Text="Low" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Normal" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="High" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Urgent" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Immediate" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="Ecoheadtxt" style="width: 8%; text-align: left;">
                                    <input type="button" value="Add Lead" onclick="funAddLead()" id="Button1" class="btnUpdate" style="height: 21px" /></td>
                            </tr>
                            <tr>
                                <td class="Ecoheadtxt" colspan="5">
                                    <table class="TableMain100">
                                        <tr>
                                            <td class="Ecoheadtxt" style="width: 25%; height: 25px;">Instruction Notes :
                                            </td>
                                            <td style="height: 25px; text-align: left;">
                                                <asp:TextBox ID="txtInstNote" runat="server" TextMode="MultiLine" Rows="5" Width="75%" Font-Size="12px" ForeColor="blue"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="5" style="height: 26px">
                                    <asp:Button ID="btnSubmit" Text="Save" runat="server" Width="95px" CssClass="btnUpdate" Enabled="False" ValidationGroup="a" OnClick="btnSubmit_Click" Height="21px" />
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" Width="95px" CssClass="btnUpdate" OnClick="btnCancel_Click" Height="21px" />
                                    <asp:TextBox ID="txtUserList" runat="server" BackColor="Transparent" BorderColor="Transparent" BorderStyle="None" ForeColor="#DDECFE"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlActivityDetail" runat="server" Visible="false" Width="100%">
                        <asp:GridView ID="grdActivityDetail" runat="server" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1" OnPageIndexChanging="grdActivityDetail_PageIndexChanging">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle Font-Bold="false" ForeColor="black" CssClass="EHEADER" BorderColor="AliceBlue" BorderWidth="1px" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td id="frmShowGenratedSalesvisit" runat="Server" visible="False">
                    <table class="TableMain100">
                        <tr>
                            <td>
                                <table class="TableMain100">
                                    <tr>
                                        <td class="Ecoheadtxt" style="width: 50%; text-align: right; height: 27px;">Genrated Sales Visit
                                        </td>
                                        <td style="text-align: left; height: 27px;">
                                            <input type="text" runat="Server" id="txtLeadId" name="txtLeadId" style="background-color: #DDECFE; border-color: #DDECFE; border-style: none; color: #DDECFE" readonly="readOnly" visible="true" />
                                            <asp:Label ID="lblError" runat="server" ForeColor="#DDECFE" BorderStyle="none" BorderColor="#DDECFE"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: left">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="Lrd" runat="server" GroupName="a" Checked="True" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" Text="From Lead Data" Font-Size="X-Small" ForeColor="Blue"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="Erd" runat="server" GroupName="a" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label5" runat="server" Text="From Existing Customer Data" Font-Size="X-Small" ForeColor="Blue"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table class="TableMain100">
                                                <tr>
                                                    <td class="Ecoheadtxt" style="width: 8%">Select Date Range
                                                    </td>
                                                    <td style="width: 136px; text-align: left;" class="Ecoheadtxt">
                                                        <%--<asp:TextBox ID="FromDate" runat="server" Width="104px"></asp:TextBox>
                                                <asp:Image ID="ImgFrom" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                                        <dxe:ASPxDateEdit ID="FromDate" runat="server"
                                                            EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True" Width="120px">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td class="Ecoheadtxt" style="width: 2%">To
                                                    </td>
                                                    <td style="width: 165px; text-align: left;" class="Ecoheadtxt">
                                                        <%--<asp:TextBox ID="ToDate" runat="server" Width="103px"></asp:TextBox>
                                                <asp:Image ID="ImgTo" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                                        <dxe:ASPxDateEdit ID="ToDate" runat="server"
                                                            EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True" Width="120px">
                                                            <ButtonStyle Width="13px">
                                                            </ButtonStyle>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                    <td class="Ecoheadtxt" style="width: 3%">Select
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="drpSelect" runat="server" Width="80px">
                                                            <asp:ListItem>All</asp:ListItem>
                                                            <asp:ListItem>Assigned</asp:ListItem>
                                                            <asp:ListItem>UnAssigned</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td class="Ecoheadtxt" style="width: 7%">Product Type
                                                    </td>
                                                    <td class="Ecoheadtxt" style="width: 32%; text-align: left">
                                                        <input type="radio" runat="Server" id="Radio1" name="rdr" value="All" checked="True" onclick="javascript: fun(this, true)" />
                                                        All Or
                                                <input type="radio" runat="Server" id="Radio2" name="rdr" value="Select" onclick="javascript: fun(this, false)" />
                                                        Selective
                                                <asp:DropDownList ID="drpProduct" runat="server" Enabled="false">
                                                    <asp:ListItem Text="Broking & DP Account" Value="Broking & DP Account"></asp:ListItem>
                                                    <asp:ListItem Text="Mutual Fund" Value="Mutual Fund"></asp:ListItem>
                                                    <asp:ListItem Text="Insurance" Value="Insurance"></asp:ListItem>
                                                </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnshowGenratedSaleVisit" runat="server" Text="Show" CssClass="btnUpdate" OnClick="btnshowGenratedSaleVisit_Click" Height="21px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="TdAllot" runat="server" style="height: 216px" colspan="2">
                                            <asp:Button ID="btnSelectAll" runat="server" Text="Select All" Visible="false" CssClass="btnUpdate" OnClick="btnSelectAll_Click" Height="21px" />
                                            <asp:Button ID="btnExport" runat="server" Text="Export" Visible="false" CssClass="btnUpdate" Height="21px" />

                                            <asp:Label ID="lblTotalRecord" runat="server" ForeColor="red"></asp:Label>
                                            <asp:GridView ID="grdGenratedSalesVisit" runat="server" AutoGenerateColumns="true" AllowPaging="True" PageSize="6" Width="100%" AllowSorting="True" CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1" OnRowDataBound="grdGenratedSalesVisit_RowDataBound" OnPageIndexChanging="grdGenratedSalesVisit_PageIndexChanging" OnSorting="grdGenratedSalesVisit_Sorting">
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                                <EditRowStyle BackColor="#2461BF" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                                <HeaderStyle Font-Bold="false" ForeColor="black" BorderColor="AliceBlue" BorderWidth="1px" />
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sel">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSel" runat="server"></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId" runat="Server" Text='<%# Eval("LeadId") + "@@@@" +  Eval("ProductId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="frmAllot" runat="Server" visible="False" colspan="2">
                                            <input type="button" id="btnAllot" name="btnAllot" value="Allot" onclick="windowopenform()" class="btnUpdate" style="height: 21px" />
                                            <asp:Button ID="btnCancelGenratedSalesVisit" runat="server" Text="Cancel" CssClass="btnUpdate" OnClick="btnCancelGenratedSalesVisit_Click" Height="21px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="Td1" runat="Server" visible="False" colspan="2">
                                            <input type="button" id="Button2" name="btnAllot" value="Allot" onclick="windowopenform1()" class="btnUpdate" style="height: 21px" />
                                            <asp:Button ID="Button3" runat="server" Text="Cancel" CssClass="btnUpdate" OnClick="btnCancelGenratedSalesVisit_Click" Height="21px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
