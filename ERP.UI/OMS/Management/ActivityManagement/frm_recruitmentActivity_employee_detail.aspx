<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.ActivityManagement.management_activitymanagement_frm_recruitmentActivity_employee_detail" CodeBehind="frm_recruitmentActivity_employee_detail.aspx.cs" %>


<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    
    
    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff();
        }

    </script>
    <!--___________________These files are for datetime calander___________________-->
    <link type="text/css" rel="stylesheet" href="../../CSS/dhtmlgoodies_calendar.css?random=20051112" media="screen" />
    <script type="text/javascript" src="/assests/js/dhtmlgoodies_calendar.js?random=20060118"></script>
    <!--___________________________________________________________________________-->
    <script language="javascript" type="text/javascript">
        var chkobj;
        var objchk = null;
        function chkclicked(obj, msg12) {
            //var txt = document.getElementById("hiddenleadid")
            if (objchk == null) {
                objchk = obj;
                objchk.checked = true;
            }
            else {
                objchk.checked = false;
                objchk = obj;
                objchk.checked = true;
            }
            //txt.value = msg12;
        }
        //function TABLE1_onclick() {

        //}

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblname" runat="server" Text="" ForeColor="red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:GridView ID="grdDetail" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateColumns="false" BorderWidth="1px" BorderColor="#507CD1" PageSize="20" OnRowDataBound="grdDetail_RowDataBound" Height="72px">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle Font-Bold="False" ForeColor="Black" CssClass="EHEADER" BorderColor="AliceBlue" />
                        <AlternatingRowStyle BackColor="White" />

                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkDetail" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Activity No." Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="lblActivityNo" runat="server" Text='<%# Eval("Activity No") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Activity">
                                <ItemTemplate>
                                    <asp:Label ID="lblActivity" runat="server" Text='<%# Eval("Activity") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Vacancy">
                                <ItemTemplate>
                                    <asp:Label ID="lblVacancy" runat="server" Text='<%# Eval("Vacancy") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interviewed">
                                <ItemTemplate>
                                    <asp:Label ID="lblInterviewed" runat="server" Text='<%# Eval("Interviewed") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Selected">
                                <ItemTemplate>
                                    <asp:Label ID="lblSelected" runat="server" Text='<%# Eval("Selected") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pending">
                                <ItemTemplate>
                                    <asp:Label ID="lblPending" runat="server" Text='<%# Eval("Pending") %> '></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:HiddenField ID="hdID" runat="server" />
                    <asp:HiddenField ID="hdCandiId" runat="server" />
                    <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>
                    <asp:HiddenField ID="hdUserNmae" runat="server" />
                    <asp:Button ID="btnModify" runat="server" Text="Modify" CssClass="btnUpdate" OnClick="btnModify_Click" />
                    <asp:Button ID="btnReassign" runat="server" Text="Reassign" CssClass="btnUpdate" OnClick="btnReassign_Click" />
                    <asp:Button ID="btnReschedule" runat="server" Text="Reschedule" CssClass="btnUpdate" OnClick="btnReschedule_Click" />
                    <asp:Button ID="btnShowDetail" runat="server" Text="Show Detail" CssClass="btnUpdate" OnClick="btnShowDetail_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlCall" runat="server" Width="100%">
                        <table class="TableMain100">
                            <tr class="EHEADER">
                                <td class="ColHead">
                                    <span style="color: #3300cc"><strong>Candidate Detail</strong></span>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; width: 100%;">
                                    <table>
                                        <tr>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Activity Type:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpActType" runat="server" Width="203px" Font-Size="12px" TabIndex="1">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 17px"></td>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Start Date/Start Time:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <%--  <asp:TextBox ID="txtStartDate" runat="server" Font-Size="12px" TabIndex="2" Width="179px"></asp:TextBox>
                                                    <asp:Image ID="imgStartDate" runat="server" ImageUrl="~/images/calendar.jpg"  />--%>
                                                <dxe:ASPxDateEdit ID="txtStartDate" runat="server" EditFormat="Custom" UseMaskBehavior="True" TabIndex="2">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtStartDate"
                                                    Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Assign To:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpUserWork" runat="server" Width="203px" Font-Size="12px" TabIndex="3">
                                                </asp:DropDownList></td>
                                            <td style="width: 17px"></td>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">End Date/Time:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <%--   <asp:TextBox ID="txtEndDate" runat="server" Font-Size="12px" TabIndex="4" Width="179px"></asp:TextBox>
                                                    <asp:Image ID="imgEndDate" runat="server" ImageUrl="~/images/calendar.jpg"  />--%>
                                                <dxe:ASPxDateEdit ID="txtEndDate" runat="server" EditFormat="Custom" UseMaskBehavior="True" TabIndex="4">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtEndDate"
                                                    Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Description:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:TextBox ID="txtDesc" runat="server" Width="198px" Font-Size="12px" TabIndex="5" Height="35px" TextMode="MultiLine"></asp:TextBox>&nbsp;
                                            </td>
                                            <td style="width: 17px"></td>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Priority:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpPriority" runat="server" Width="203px" Font-Size="12px" TabIndex="6">
                                                    <asp:ListItem Text="Low" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Normal" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="High" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Urgent" Value="3"></asp:ListItem>
                                                    <asp:ListItem Text="Immediate" Value="4"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Company:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpCompany" runat="server" Width="203px" Font-Size="12px" TabIndex="7">
                                                </asp:DropDownList></td>
                                            <td style="width: 17px"></td>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Department:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpDepartment" runat="server" Width="203px" Font-Size="12px" TabIndex="8">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Branch:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpBranch" runat="server" Width="203px" Font-Size="12px" TabIndex="9">
                                                </asp:DropDownList></td>
                                            <td style="width: 17px"></td>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Job Responsibility:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpJobResponsbility" runat="server" Width="203px" Font-Size="12px" TabIndex="10">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Designation:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpDesignation" Width="203px" Font-Size="12px" runat="server" TabIndex="11"></asp:DropDownList>
                                            </td>
                                            <td style="width: 17px"></td>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Vacancies:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:TextBox ID="txtVacancies" runat="server" Width="200px" Font-Size="12px" TabIndex="12" Text="1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Educational Qualification:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpEducation" runat="server" Width="203px" Font-Size="12px" TabIndex="13">
                                                </asp:DropDownList></td>
                                            <td style="width: 17px"></td>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Professional Qualification:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpProfessional" Width="203px" Font-Size="12px" runat="server" TabIndex="14"></asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Minimum Experience (Yrs):</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:TextBox ID="txtExp" runat="server" Width="200px" Font-Size="12px" TabIndex="15" Text="0"></asp:TextBox></td>
                                            <td style="width: 17px"></td>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Sex:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpSex" Width="203px" Font-Size="12px" runat="server" TabIndex="16">
                                                    <asp:ListItem Text="Male" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="FeMale" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">CTC Range:</span>
                                            </td>
                                            <td class="gridcellleft" style="text-align: center">Min<asp:TextBox ID="txtMin" runat="server" Text="0" Width="66px" Font-Size="12px" TabIndex="17"></asp:TextBox>
                                                &nbsp; &amp;&nbsp;&nbsp; Max<asp:TextBox ID="txtMax" runat="server" Text="0" Width="66px" Font-Size="12px" TabIndex="18" />
                                            </td>
                                            <td style="width: 17px"></td>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Age Group:</span>
                                            </td>
                                            <td class="gridcellleft">Min<asp:TextBox Font-Size="12px" ID="txtMinAge" runat="server" Text="18" Width="50px" TabIndex="19"></asp:TextBox>
                                                &nbsp; &amp; &nbsp;&nbsp; Max<asp:TextBox Font-Size="12px" ID="txtMaxAge" runat="server" Text="18" Width="50px" TabIndex="20" />Years
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; height: 21px;">
                                                <span class="Ecoheadtxt">Skills:</span>
                                            </td>
                                            <td class="gridcellleft" style="height: 21px">
                                                <asp:TextBox ID="txtSkills" runat="server" Width="200px" Font-Size="12px" TabIndex="21"></asp:TextBox></td>
                                            <td style="width: 17px; height: 21px;"></td>
                                            <td style="text-align: right; height: 21px;">
                                                <span class="Ecoheadtxt">Hobbies:</span>
                                            </td>
                                            <td class="gridcellleft" style="height: 21px">

                                                <asp:TextBox ID="txtHobbies" runat="server" Width="200px" Font-Size="12px" TabIndex="22"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; height: 21px;">
                                                <span class="Ecoheadtxt">ExtCurrent Activities:</span>
                                            </td>
                                            <td class="gridcellleft" style="height: 21px">
                                                <asp:TextBox ID="txtCurAct" runat="server" Font-Size="12px" Width="200px" TabIndex="23"></asp:TextBox>
                                            </td>
                                            <td style="width: 17px; height: 21px;"></td>
                                            <td style="text-align: right; height: 21px;">
                                                <span class="Ecoheadtxt">Locality Preferences:</span>
                                            </td>
                                            <td class="gridcellleft" style="height: 21px">
                                                <asp:TextBox ID="txtLocality" runat="server" Font-Size="12px" Width="200px" TabIndex="24"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Join  by Date:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <%-- <asp:TextBox ID="txtJoinDate" runat="server" Font-Size="12px" TabIndex="25" Width="179px"></asp:TextBox>
                                                    <asp:Image ID="imgJoinDate" runat="server" ImageUrl="~/images/calendar.jpg"  />--%>
                                                <dxe:ASPxDateEdit ID="txtJoinDate" runat="server" EditFormat="Custom" UseMaskBehavior="True" TabIndex="25">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtJoinDate"
                                                    Display="Dynamic" EnableTheming="True" ErrorMessage="Date required!" ValidationGroup="a"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 17px"></td>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Industry:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpIndustry" Width="203px" Font-Size="12px" runat="server" TabIndex="26">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <span class="Ecoheadtxt">Instruction Notes:</span>
                                            </td>
                                            <td class="gridcellleft" colspan="4">
                                                <asp:TextBox ID="txtInstNote" runat="server" TextMode="MultiLine" Width="558px" Font-Size="12px" TabIndex="27" Height="96px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: right">&nbsp;</td>
                                            <td style="width: 17px"></td>
                                            <td colspan="2" class="gridcellright">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnUpdate" OnClick="btnSave_Click" TabIndex="28" ValidationGroup="a" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnUpdate" OnClick="btnCancel_Click" TabIndex="29" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlActivityDetail" runat="server" Visible="false" Width="100%">
                        <asp:GridView ID="grdActivityDetail" runat="server" BackColor="White" Font-Bold="False" BorderColor="#507CD1" BorderStyle="None" BorderWidth="1px" CellPadding="2" Font-Overline="False" AllowPaging="True">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#EFF3FB" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle Font-Bold="False" ForeColor="Black" CssClass="EHEADER" BorderColor="AliceBlue" />
                            <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
