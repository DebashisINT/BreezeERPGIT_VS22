<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_CRMPhoneCallActivity" CodeBehind="CRMPhoneCallActivity.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

        function UserList() {
            var str = "UserList.aspx"
            frmOpenNewWindow1(str, 300, 800)
        }
        function frmOpenNewWindow1(location, v_height, v_weight) {
            var x = (screen.availHeight - v_height) / 2;
            var y = (screen.availWidth - v_weight) / 2
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + x + ",left=" + y + ",location=no,directories=no,menubar=no,toolbar=no,status=yes,scrollbars=yes,resizable=no,dependent=no'");
        }

        function fun123() {

            var val123 = document.getElementById("txtUserList").value;
            if (val123 != "") {
                var str = 'frm_AddLead.aspx?Call=PhoneCall&user=' + val123;
                frmOpenNewWindow1(str, 500, 800)
            }
            else {
                alert('Assign to Can Not Be Blank');
            }
        }
        function checkButton() {
            document.getElementById("Button1").disabled = false;
        }
        function FillLeadId123(val) {
            var ob = document.getElementById("txtLeadId")
            ob.value = val;
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

        function FillLeadId1(obj, val) {
            var ob = document.getElementById("txtLeadId1")
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
        function fun1(obj, str) {
            document.getElementById("drpSalesProduct").disabled = str;
        }
        function windowopenform() {
            var st = document.getElementById("txtLeadId")
            frmOpenNewWindow1('frmAlloat.aspx?id=' + st.value, 500, 500)
        }
        function windowopenform123() {
            var st = document.getElementById("txtLeadId1")
            frmOpenNewWindow1('frmAllot_sales.aspx?id=' + st.value, 500, 500)
        }
        function javascr() {
            var tdate = document.getElementById("txtToDate");
            var fdate = document.getElementById("txtFromDate");
            tdate.value = fdate.value;
            alert(tdate.value);
            alert(fdate.value);
        }
        function funCheckFunction() {

            var st = document.getElementById("txtLeadId")
            frmOpenNewWindow1('frmupdateMeeting.aspx?id=' + st.value, 500, 500)
        }
        function funSaveClick() {
            var st = document.getElementById("txtLeadId")
            frmOpenNewWindow1('frmCancelMeeting.aspx?id=' + st.value, 500, 500)
        }
        function funCheckFunction1() {

            var st = document.getElementById("txtLeadId1")
            frmOpenNewWindow1('frmupdateMeeting.aspx?id=' + st.value, 500, 500)
        }
        function funSaveClick1() {
            var st = document.getElementById("txtLeadId1")
            frmOpenNewWindow1('frmCancelMeeting.aspx?id=' + st.value, 500, 500)
        }
        function frmOpenNewWindow_custom(location, v_height, v_weight, top, left) {
            window.open(location, "Search_Conformation_Box", "height=" + v_height + ",width=" + v_weight + ",top=" + top + ",left=" + left + ",location=no,directories=no,menubar=no,toolbar=no,status=no,scrollbars=yes,resizable=no,dependent=no'");
        }
        function TextVal() {
            var btn = document.getElementById("btnGenratedSalesVisit");
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
                <td style="text-align: left; width: 10px; height: auto">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnCreate" runat="server" Text="Create" CssClass="btnUpdate" OnClick="btnCreate_Click"
                                    Height="21px" />
                            </td>
                            <td>
                                <asp:Button ID="btnModify" runat="server" Text="Modify" CssClass="btnUpdate" OnClick="btnModify_Click"
                                    Height="21px" />
                            </td>
                            <td>
                                <asp:Button ID="btnGenratedSalesVisit" runat="server" Text="Genrated Sales Visit"
                                    CssClass="btnUpdate" OnClick="btnGenratedSalesVisit_Click" Height="21px" />
                            </td>
                            <td>
                                <asp:Button ID="btnGenratedSales" runat="server" Text="Genrated Sales" CssClass="btnUpdate"
                                    OnClick="btnGenratedSales_Click" Height="21px" />
                            </td>
                            <td>
                                <asp:Button ID="btnCourtesyCall" runat="server" Text="Courtesy Call" CssClass="btnUpdate"
                                    OnClick="btnCourtesyCall_Click" Height="21px" />
                            </td>
                            <td>
                                <asp:Button ID="btnCancel1" runat="server" Text="Cancel" CssClass="btnUpdate" OnClick="btnCancel1_Click"
                                    Visible="False" Height="21px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: left; height: auto" runat="server" id="PnlBtn" visible="false">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnReassign" runat="server" Text="Reassign" CssClass="btnUpdate"
                                    OnClick="btnReassign_Click" Height="21px" />
                            </td>
                            <td>
                                <asp:Button ID="btnReschedule" runat="server" Text="Reschedule" CssClass="btnUpdate"
                                    OnClick="btnReschedule_Click" Height="21px" />
                            </td>
                            <td>
                                <asp:Button ID="btnShowDetail" runat="server" Text="Show Details" CssClass="btnUpdate"
                                    OnClick="btnShowDetail_Click" Height="21px" />
                            </td>
                            <td>
                                <asp:Button ID="btnDelegate" runat="server" Text="Delegate To" CssClass="btnUpdate"
                                    OnClick="btnDelegate_Click" Height="21px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: auto">
                    <asp:Panel ID="userInfo1" runat="server" Width="100%">
                        <asp:GridView EnableViewState="true" ID="grdUserInfo" AutoGenerateColumns="false"
                            runat="server" AllowPaging="True" Width="100%" CssClass="gridcellleft" CellPadding="4"
                            ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1"
                            OnPageIndexChanging="grdUserInfo_PageIndexChanging">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle Font-Bold="False" ForeColor="Black" BorderColor="AliceBlue" BorderWidth="1px" />
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:HyperLinkField HeaderText="User Id" DataNavigateUrlFormatString="~/management/CRMPhoneCallActivity.aspx?id={0} &amp; type=SW"
                                    NavigateUrl="~/management/CRMPhoneCallActivity.aspx" DataTextField="UserId" Visible="False"
                                    DataNavigateUrlFields="UserId" />
                                <asp:HyperLinkField DataNavigateUrlFormatString="~/management/CRMPhoneCallActivity.aspx?id={0}&amp;type=SW"
                                    NavigateUrl="~/management/CRMPhoneCallActivity.aspx" DataTextField="SNo" HeaderText="S.No."
                                    Visible="False" DataNavigateUrlFields="userid" />
                                <asp:HyperLinkField DataNavigateUrlFormatString="~/management/CRMPhoneCallActivity.aspx?id={0}&amp;type=SW"
                                    NavigateUrl="~/management/CRMPhoneCallActivity.aspx" DataTextField="user" HeaderText="User"
                                    DataNavigateUrlFields="userid" />
                                <asp:HyperLinkField DataNavigateUrlFormatString="~/management/CRMPhoneCallActivity.aspx?id={0}&amp;type=SW"
                                    NavigateUrl="~/management/CRMPhoneCallActivity.aspx" DataTextField="Pending Acttivity"
                                    HeaderText="Pending Activity" DataNavigateUrlFields="userid" />
                                <asp:HyperLinkField DataNavigateUrlFormatString="~/management/CRMPhoneCallActivity.aspx?id={0}&amp;type=SW"
                                    NavigateUrl="~/management/CRMPhoneCallActivity.aspx" DataTextField="Scheduled End Date"
                                    HeaderText="Scheduled End Date" DataNavigateUrlFields="userid">
                                    <ControlStyle Width="90px" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFormatString="~/management/CRMPhoneCallActivity.aspx?id={0}&amp;type=SW"
                                    NavigateUrl="~/management/CRMPhoneCallActivity.aspx" DataTextField="Expected End Date"
                                    HeaderText="Expected End Date" DataNavigateUrlFields="userid">
                                    <ControlStyle Width="90px" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField DataNavigateUrlFormatString="~/management/CRMPhoneCallActivity.aspx?id={0}&amp;type=SW"
                                    NavigateUrl="~/management/CRMPhoneCallActivity.aspx" DataTextField="Pending Call"
                                    HeaderText="Pending Calls" DataNavigateUrlFields="userid" />
                                <asp:HyperLinkField DataNavigateUrlFormatString="~/management/CRMPhoneCallActivity.aspx?id={0}&amp;type=SW"
                                    NavigateUrl="~/management/CRMPhoneCallActivity.aspx" DataTextField="Call Back"
                                    HeaderText="Call Back" DataNavigateUrlFields="userid" />
                                <asp:HyperLinkField DataNavigateUrlFormatString="~/management/CRMPhoneCallActivity.aspx?id={0}type=SW"
                                    NavigateUrl="~/management/CRMPhoneCallActivity.aspx" DataTextField="Non Contactable"
                                    HeaderText="Non Contactable" DataNavigateUrlFields="userid" />
                                <asp:HyperLinkField DataNavigateUrlFormatString="~/management/CRMPhoneCallActivity.aspx?id={0}&amp;type=SW"
                                    NavigateUrl="~/management/CRMPhoneCallActivity.aspx" DataTextField="Non Usable"
                                    HeaderText="Non Usable" DataNavigateUrlFields="userid" />
                                <asp:HyperLinkField DataNavigateUrlFormatString="~/management/CRMPhoneCallActivity.aspx?id={0}&amp;type=SW"
                                    NavigateUrl="~/management/CRMPhoneCallActivity.aspx" DataTextField="Pipeline/Sales Visits"
                                    HeaderText="Pipeline/Sales Visits" DataNavigateUrlFields="userid" />
                                <asp:HyperLinkField DataNavigateUrlFormatString="~/management/CRMPhoneCallActivity.aspx?id={0}&amp;type=SW"
                                    NavigateUrl="~/management/CRMPhoneCallActivity.aspx" DataTextField="Won/Confirm Sales"
                                    HeaderText="Won/Confirm Sales" DataNavigateUrlFields="userid" />
                                <asp:HyperLinkField DataNavigateUrlFormatString="~/management/CRMPhoneCallActivity.aspx?id={0}&amp;type=SW"
                                    NavigateUrl="~/management/CRMPhoneCallActivity.aspx" DataTextField="Lost/Not Interested"
                                    HeaderText="Lost/Not Interested" DataNavigateUrlFields="userid" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
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
                    <asp:GridView ID="grdDetail" runat="server" Width="100%" CssClass="gridcellleft"
                        CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1"
                        OnRowDataBound="grdDetail_RowDataBound" AllowPaging="True" OnPageIndexChanging="grdDetail_PageIndexChanging">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle Font-Bold="False" ForeColor="Black" CssClass="EHEADER" BorderColor="AliceBlue"
                            BorderWidth="1px" />
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
                    <asp:TextBox ID="txtId" runat="server" Visible="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td id="frmShowCall" runat="Server" style="height: auto">
                    <asp:Panel ID="pnlCall" runat="server" Width="100%" Visible="false">
                        <table class="TableMain100">
                            <tr>
                                <td class="Ecoheadtxt" style="width: 17%">Activity Type :
                                </td>
                                <td class="Ecoheadtxt" style="width: 13%; text-align: left;">
                                    <asp:DropDownList ID="drpActType" Enabled="false" AutoPostBack="true" CssClass="Ecoheadtxt"
                                        runat="server" Width="80%">
                                    </asp:DropDownList>
                                </td>
                                <td class="Ecoheadtxt" style="width: 8%">Start Date/Start Time&nbsp;:<%-- <asp:TextBox ID="txtStartDate" runat ="server" ></asp:TextBox>--%>
                                </td>
                                <td class="Ecoheadtxt" style="width: 15%; text-align: left;">
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
                                <td class="Ecoheadtxt" style="width: 8%">
                                    <%--<asp:Panel ID="pnlEndTime" runat ="server" Font-Bold="True" ></asp:Panel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="Ecoheadtxt" style="width: 17%">Assign To :
                                </td>
                                <td class="Ecoheadtxt" style="width: 13%; text-align: left;">
                                    <asp:TextBox ID="txtUserList" Font-Size="12px" ForeColor="Blue" runat="server" Width="80%"></asp:TextBox>
                                    <asp:HiddenField ID="hd1UserList" runat="server" />
                                    <asp:DropDownList Visible="false" ID="drpUserWork" CssClass="Ecoheadtxt" runat="server"
                                        Width="80%">
                                    </asp:DropDownList>
                                </td>
                                <td class="Ecoheadtxt" style="width: 8%">End Date/End Time :</td>
                                <td class="Ecoheadtxt" style="width: 15%; text-align: left;">
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
                                <td style="width: 8%">
                                    <%--<asp:Panel ID="pnlTime" runat ="server" ></asp:Panel>--%>
                                </td>
                            </tr>
                            <tr>
                                <td class="Ecoheadtxt" style="width: 17%">Description :
                                </td>
                                <td style="width: 13%; text-align: left;">
                                    <asp:TextBox ID="txtDesc" TextMode="MultiLine" Rows="2" runat="server" Width="80%"
                                        Font-Size="12px" ForeColor="blue"></asp:TextBox>
                                </td>
                                <td class="Ecoheadtxt" style="width: 8%">Priority :
                                </td>
                                <td class="Ecoheadtxt" style="width: 15%; text-align: left;">
                                    <asp:DropDownList ID="drpPriority" runat="server" CssClass="Ecoheadtxt" Width="80%">
                                        <asp:ListItem Text="Low" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Normal" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="High" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Urgent" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Immediate" Value="4"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="Ecoheadtxt" style="width: 8%; text-align: left;">
                                    <input type="button" value="Add Lead" disabled="disabled" onclick="fun123()" style="height: 21px;"
                                        id="Button1" class="btnUpdate" />
                                </td>
                            </tr>
                            <tr>
                                <td class="Ecoheadtxt" colspan="5">
                                    <table class="TableMain100">
                                        <tr>
                                            <td class="Ecoheadtxt" style="width: 28%; height: 25px;">Instruction Notes :
                                            </td>
                                            <td style="height: 25px; text-align: left;">
                                                <asp:TextBox ID="txtInstNote" runat="server" TextMode="MultiLine" Rows="5" Width="75%"
                                                    Font-Size="12px" ForeColor="blue"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="5" style="height: 26px">
                                    <asp:Button ID="btnSubmit" Text="Save" runat="server" Width="95px" CssClass="btnUpdate"
                                        OnClick="btnSubmit_Click" Enabled="False" ValidationGroup="a" Height="21px" />
                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" Width="95px" CssClass="btnUpdate"
                                        OnClick="btnCancel_Click" Height="21px" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlActivityDetail" runat="server" Visible="false" Width="100%">
                        <asp:GridView ID="grdActivityDetail" runat="server" Width="100%" CssClass="gridcellleft"
                            CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1"
                            OnPageIndexChanging="grdActivityDetail_PageIndexChanging">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle Font-Bold="False" ForeColor="Black" CssClass="EHEADER" BorderColor="AliceBlue"
                                BorderWidth="1px" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td id="frmShowGenratedSalesvisit" runat="Server" visible="False" style="height: auto">
                    <table class="TableMain100">
                        <tr>
                            <td>
                                <table class="TableMain100">
                                    <tr>
                                        <td class="Ecoheadtxt" style="width: 50%; text-align: right">Genrated Sales Visit
                                        </td>
                                        <td style="text-align: left">
                                            <input type="text" runat="Server" id="txtLeadId" name="txtLeadId" style="background-color: #DDECFE; border-color: #DDECFE; border-style: none; color: #DDECFE"
                                                readonly="readOnly"
                                                visible="true" />
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
                                                        <asp:Label ID="Label2" runat="server" Text="From Lead Data" Font-Size="X-Small" ForeColor="Blue"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="Erd" runat="server" GroupName="a" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label3" runat="server" Text="From Existing Customer Data" Font-Size="X-Small"
                                                            ForeColor="Blue"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table class="TableMain100">
                                                <tr>
                                                    <td class="Ecoheadtxt" style="width: 10%">Select Date Range
                                                    </td>
                                                    <td style="width: 173px; text-align: left;" class="Ecoheadtxt">
                                                        <%--<asp:TextBox ID="FromDate" runat="server" Width="104px"></asp:TextBox>
                                                <asp:Image ID="ImgFrom" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                                        <dxe:ASPxDateEdit ID="FromDate" runat="server"
                                                            EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True"
                                                            Width="115px">
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
                                                            EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True"
                                                            Width="115px">
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
                                                    <td class="Ecoheadtxt" style="width: 33%; text-align: left">
                                                        <input type="radio" runat="Server" id="Radio1" name="rdr" value="All" checked="True"
                                                            onclick="javascript: fun(this, true)" />
                                                        All Or
                                                            <input type="radio" runat="Server" id="Radio2" name="rdr" value="Select" onclick="javascript: fun(this, false)" />
                                                        Selective
                                                            <asp:DropDownList ID="drpProduct" runat="server" Enabled="false">
                                                                <asp:ListItem Text="Broking And DP Account" Value="Broking And DP Account"></asp:ListItem>
                                                                <asp:ListItem Text="Mutual Fund" Value="Mutual Fund"></asp:ListItem>
                                                                <asp:ListItem Text="Insurance" Value="Insurance"></asp:ListItem>
                                                                <asp:ListItem Text="Refreal Agent" Value="Refreal Agent"></asp:ListItem>
                                                                <asp:ListItem Text="Sub Broker" Value="Sub Broker"></asp:ListItem>
                                                            </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnshowGenratedSaleVisit" runat="server" Text="Show" CssClass="btnUpdate"
                                                            OnClick="btnshowGenratedSaleVisit_Click" Height="21px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSelectAll" runat="server" Text="Select All" Visible="false" CssClass="btnUpdate"
                                    OnClick="btnSelectAll_Click" Height="21px" />
                                <asp:Button ID="btnExport" runat="server" Text="Export" Visible="false" CssClass="btnUpdate"
                                    Height="21px" />
                                <input type="button" id="btnPostPendingMeetings" name="btnPostPendingMeetings" value="PostPone Meeting"
                                    class="btnUpdate" onclick="funCheckFunction()" style="height: 21px" />
                                <input type="button" id="btnCancelMeeting" name="btnCancelMeeting" value="Cancel Meeting"
                                    class="btnUpdate" onclick="funSaveClick()" style="height: 21px" />
                                <asp:Label ID="lblTotalRecord" runat="server" ForeColor="red"></asp:Label>
                                <asp:GridView ID="grdGenratedSalesVisit" runat="server" AutoGenerateColumns="true"
                                    AllowPaging="True" PageSize="6" Width="100%" AllowSorting="True" CssClass="gridcellleft"
                                    CellPadding="4" ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1"
                                    OnPageIndexChanging="grdGenratedSalesVisit_PageIndexChanging" OnRowDataBound="grdGenratedSalesVisit_RowDataBound"
                                    OnSorting="grdGenratedSalesVisit_Sorting">
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle Font-Bold="False" ForeColor="Black" BorderColor="AliceBlue" BorderWidth="1px" />
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
                            <td id="frmAllot" runat="Server" visible="False">
                                <input type="button" id="btnAllot" name="btnAllot" value="Allot" onclick="windowopenform()"
                                    class="btnUpdate" style="height: 21px" />
                                <asp:Button ID="btnCancelGenratedSalesVisit" runat="server" Text="Cancel" CssClass="btnUpdate"
                                    OnClick="btnCancelGenratedSalesVisit_Click" Height="21px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id="frmShowCourtesyCall" runat="Server" visible="false" style="height: auto">
                    <table class="TableMain100">
                        <tr>
                            <td>
                                <table class="TableMain100">
                                    <tr>
                                        <td colspan="7" class="Ecoheadtxt" style="text-align: center">Show Courtesy Call
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="7" style="text-align: left">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="CLrd" runat="server" GroupName="a" Checked="True" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label4" runat="server" Text="From Lead Data" Font-Size="X-Small" ForeColor="Blue"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="CCrd" runat="server" GroupName="a" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="Label5" runat="server" Text="From Existing Customer Data" Font-Size="X-Small"
                                                            ForeColor="Blue"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="Ecoheadtxt" style="width: 9%">Select Date</td>
                                        <td style="text-align: left; width: 21%;" class="Ecoheadtxt">
                                            <%--<asp:TextBox ID="CourtesyStartCallDate" runat="server" Width="150px"></asp:TextBox>
                                       <asp:Image ID="ImgStartCall" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                            <dxe:ASPxDateEdit ID="CourtesyStartCallDate" runat="server"
                                                EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="Ecoheadtxt" style="width: 2%">To</td>
                                        <td style="text-align: left; width: 24%;" class="Ecoheadtxt">
                                            <%--<asp:TextBox ID="CourtesyEndCallDate" runat="server" Width="150px"></asp:TextBox>
                                       <asp:Image ID="ImgEndCall" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                            <dxe:ASPxDateEdit ID="CourtesyEndCallDate" runat="server"
                                                EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="Ecoheadtxt" style="width: 6%">Select User</td>
                                        <td style="text-align: left;">
                                            <asp:DropDownList ID="drpSelectedUser" runat="server" AppendDataBoundItems="true"
                                                Width="150px">
                                                <asp:ListItem Value="0">All</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="btnShowCourtesyCall" runat="Server" Text="Show" CssClass="btnUpdate"
                                                OnClick="btnShowCourtesyCall_Click" Height="21px" /></td>
                                        <td style="text-align: left">&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTotalCourtestCall" runat="Server" ForeColor="red"></asp:Label>
                                <asp:GridView ID="grdCourtestCall" runat="Server" AutoGenerateColumns="true" AllowPaging="True"
                                    PageSize="6" Width="100%" AllowSorting="True" CssClass="gridcellleft" CellPadding="4"
                                    ForeColor="#333333" GridLines="None" BorderWidth="1px" BorderColor="#507CD1"
                                    OnPageIndexChanging="grdCourtestCall_PageIndexChanging" OnRowDataBound="grdCourtestCall_RowDataBound"
                                    OnSorting="grdCourtestCall_Sorting">
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle Font-Bold="False" ForeColor="Black" BorderColor="AliceBlue" BorderWidth="1px" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td id="FrmShowGenratedSales" runat="Server" visible="false" style="height: 356px; height: auto">
                    <table class="TableMain100">
                        <tr>
                            <td colspan="3" class="Ecoheadtxt" style="text-align: center" visible="false">Genrated Sales By Phone Follow Up
                                    <input type="Hidden" runat="Server" id="txtLeadId1" name="txtLeadId1" style="background-color: #DDECFE; border-color: #DDECFE; border-style: none; color: #DDECFE" />
                                <asp:Label ID="Label1" runat="server" ForeColor="#DDECFE" BorderStyle="none"></asp:Label></td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButton ID="LGLrd" runat="server" GroupName="a" Checked="True" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="From Lead Data" Font-Size="X-Small" ForeColor="Blue"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButton ID="LGCrd" runat="server" GroupName="a" />
                                        </td>
                                        <td>
                                            <asp:Label ID="Label7" runat="server" Text="From Existing Customer Data" Font-Size="X-Small"
                                                ForeColor="Blue"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table class="TableMain100">
                                    <tr>
                                        <td class="Ecoheadtxt" style="width: 10%">Select Date Range
                                        </td>
                                        <td class="Ecoheadtxt" style="width: 15%; text-align: left">
                                            <%-- <asp:TextBox ID="SalesStartDate" runat="server" Width="106px"></asp:TextBox>
                                       <asp:Image ID="ImgSalesStartDate" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                            <dxe:ASPxDateEdit ID="SalesStartDate" runat="server"
                                                EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True"
                                                Width="115px">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="Ecoheadtxt" style="width: 2%">To
                                        </td>
                                        <td class="Ecoheadtxt" style="width: 18%; text-align: left">
                                            <%--<asp:TextBox ID="SalesEndDate" runat="server" Width="127px"></asp:TextBox>
                                       <asp:Image ID="ImgSalesEndDate" runat="server" ImageUrl="~/images/calendar.jpg" />--%>
                                            <dxe:ASPxDateEdit ID="SalesEndDate" runat="server"
                                                EditFormat="Custom" EditFormatString="dd MMMM yyyy hh:mm tt" UseMaskBehavior="True"
                                                Width="115px">
                                                <ButtonStyle Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td class="Ecoheadtxt" style="width: 3%">Select
                                        </td>
                                        <td class="Ecoheadtxt" style="width: 8%; text-align: left">
                                            <asp:DropDownList ID="drpSalesSelect" runat="server" Width="75px">
                                                <asp:ListItem>All</asp:ListItem>
                                                <asp:ListItem>Assigned</asp:ListItem>
                                                <asp:ListItem>UnAssigned</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td class="Ecoheadtxt" style="width: 7%">Product Type
                                        </td>
                                        <td class="Ecoheadtxt" style="width: 31%; text-align: left">
                                            <input type="radio" runat="Server" id="Radio3" name="rdr1" value="All" onclick="javascript: fun1(this, true)"
                                                checked="True" />
                                            All Or
                                                <input type="radio" runat="Server" id="Radio4" name="rdr1" value="Select" onclick="javascript: fun1(this, false)" />
                                            Selective
                                                <asp:DropDownList ID="drpSalesProduct" runat="Server" Enabled="false">
                                                    <asp:ListItem Text="Broking & DP Account" Value="Broking & DP Account"></asp:ListItem>
                                                    <asp:ListItem Text="Mutual Fund" Value="Mutual Fund"></asp:ListItem>
                                                    <asp:ListItem Text="Insurance" Value="Insurance"></asp:ListItem>
                                                </asp:DropDownList>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:Button ID="btnShowSales" runat="Server" Text="Show" CssClass="btnUpdate" OnClick="btnShowSales_Click"
                                                Height="21px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnSalesSelectAll" runat="server" Text="Select All" Visible="False"
                                    CssClass="btnUpdate" OnClick="btnSalesSelectAll_Click" Height="21px" />
                                <asp:Button ID="btnSalesExport" runat="server" Text="Export" Visible="False" CssClass="btnUpdate"
                                    Height="21px" />
                                <input type="button" id="Button5" name="btnPostPendingMeetings" value="PostPone Meeting"
                                    onclick="funCheckFunction1()" class="btnUpdate" style="height: 21px" />
                                <input type="button" id="Button6" name="btnCancelMeeting" value="Cancel Meeting"
                                    onclick="funSaveClick1()" class="btnUpdate" style="height: 21px" />
                                <asp:Label ID="lblSalesTotalRecord" runat="Server" ForeColor="red"></asp:Label>
                                <asp:GridView ID="grdSales" runat="Server" AutoGenerateColumns="true" AllowPaging="True"
                                    PageSize="6" Width="100%" CssClass="gridcellleft" CellPadding="4" ForeColor="#333333"
                                    GridLines="None" BorderWidth="1px" BorderColor="#507CD1" AllowSorting="True"
                                    OnPageIndexChanging="grdSales_PageIndexChanging" OnRowDataBound="grdSales_RowDataBound"
                                    OnSorting="grdSales_Sorting">
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <HeaderStyle Font-Bold="False" ForeColor="Black" BorderColor="AliceBlue" BorderWidth="1px" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sel">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSel" runat="server"></asp:CheckBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="Server" Text='<%# Eval("LeadId") + "@@@@" +  Eval("ProductId") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="Center" id="frmSalesAllot" runat="Server" visible="False">
                                <input type="button" id="btnSalesAllot" name="btnAllot" value="Allot" onclick="windowopenform123()"
                                    class="btnUpdate" style="height: 21px" />
                                <asp:Button ID="btnSalesCanCel" runat="server" Text="Cancel" CssClass="btnUpdate"
                                    OnClick="btnSalesCanCel_Click" Height="21px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
