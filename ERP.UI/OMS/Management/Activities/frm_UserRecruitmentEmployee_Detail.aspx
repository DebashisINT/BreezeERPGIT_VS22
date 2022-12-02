<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.management_activities_frm_UserRecruitmentEmployee_Detail" Codebehind="frm_UserRecruitmentEmployee_Detail.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    
    

    

    <script language="javascript" type="text/javascript">
    function SignOff()
    {
        window.parent.SignOff();
    }

    var chkobj;
    var objchk=null;
    function chkclicked(obj,msg12)
    {
        var txt = document.getElementById("hiddenleadid")
        if (objchk == null)
        {
            objchk = obj;
            objchk.checked = true;
        }
        else
        {
            objchk.checked = false;
            objchk = obj;
            objchk.checked = true;
        }
        txt.value = msg12;
    }
    function Showdetails(obj)
    {
        var url='frm_UserRecruitmentEmployee_Detail_popup.aspx?id='+obj;
        window.open(url,'Text','resizable=1,height=300px,width=700px');
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div>
            <table class="TableMain100">
                <tr class="EHEADER">
                    <td class="ColHead">
                        <span style="color: #3300cc"><strong>Activity Detail </strong></span>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; width: 100%;">
                        <table>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Activity ID:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblActiID" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td style="width: 15px">
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Date of Allotment:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblDateOfAllocation" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Alloted By: </span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblAllotedBy" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td style="width: 15px">
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Scheduled Start Date/time:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblShedStartDT" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Priority:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblPriority" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td style="width: 15px">
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Scheduled End Date/time:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblShedEndDT" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Vacancies:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblVacancies" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td style="width: 15px">
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Actual Start Date/time:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblActualStartDT" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="EHEADER">
                    <td class="ColHead">
                        <span style="color: #3300cc"><strong>Recruitment Detail</strong></span>
                    </td>
                </tr>
                <tr>
                    <td class="gridcellcenter">
                        <table>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Job Responsibility:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblJobResponsibility" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Department:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblDepartment" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Position: </span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblPosition" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">HOD:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblHOD" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Educational Qualification:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblEduQualification" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">CTC Range:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblCTCRange" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Min Exp:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblMinExp" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">ExtCurrentActivities:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblExtCurrentActivities" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Industry Exp:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblIndustryExp" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Sex:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblSex" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Skills: </span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblSkills" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Age Group:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblAge" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Join by Date:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblJoinByDate" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Locality Preferences:</span>
                                </td>
                                <td class="gridcellleft">
                                    <asp:Label ID="lblLocationPresference" runat="server" Text="" CssClass="EcoheadCon"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="gridcellleft">
                                    <span class="Ecoheadtxt">Note:</span>
                                </td>
                                <td class="gridcellleft" colspan="7">
                                    <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; border-top: solid 2px white; padding-top: 0px;" colspan="8">
                                    <table style="border-bottom: solid 1px white; border-right: solid 1px white; border-left: solid 1px white;">
                                        <tr>
                                            <td>
                                                <dxe:ASPxButton ID="btnAddCandidate" runat="server" Text="Add Candidate" OnClick="btnAddCandidate_Click"
                                                    Font-Size="12px">
                                                </dxe:ASPxButton>
                                            </td>
                                            <td>
                                                <dxe:ASPxButton ID="btnUpdateCandidate" runat="server" Text="Update Candidate" OnClick="btnUpdateCandidate_Click"
                                                    Font-Size="12px">
                                                </dxe:ASPxButton>
                                            </td>
                                            <td>
                                                <dxe:ASPxButton ID="btnCreateInterview" runat="server" Text="Create Interview" OnClick="btnCreateInterview_Click"
                                                    Font-Size="12px">
                                                </dxe:ASPxButton>
                                            </td>
                                            <td>
                                                <dxe:ASPxButton ID="btnModifyInterview" runat="server" Text="Modify Interview" OnClick="btnModifyInterview_Click"
                                                    Font-Size="12px">
                                                </dxe:ASPxButton>
                                            </td>
                                            <td>
                                                <dxe:ASPxButton ID="btnUpdateInterview" runat="server" Text="Update Interview" OnClick="btnUpdateInterview_Click"
                                                    Font-Size="12px">
                                                </dxe:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Label ID="lblmessage" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr class="EHEADER">
                    <td style="text-align: center; width: 100%;">
                        <span style="color: #3300cc"><strong>Candidate And Interview Detail</strong></span>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; width: 100%;">
                        <input type="hidden" id="hiddenleadid" name="hiddenleadid" />
                        <br />
                        <asp:GridView ID="GridCandi1" runat="server" CellPadding="4" ForeColor="#333333"
                            GridLines="None" AutoGenerateColumns="false" BorderWidth="1px" BorderColor="#507CD1"
                            OnRowDataBound="GridCandi1_RowDataBound" PageSize="20">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <RowStyle BackColor="#EFF3FB" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <HeaderStyle Font-Bold="false" ForeColor="black" CssClass="EHEADER" BorderColor="AliceBlue" />
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="ChkRDId" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CandidateId" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCandidateId" runat="server" Text='<%# Eval("Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Candidate Name">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Interview Date">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLastInterview" runat="server" Text='<%# Eval("InterviewDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Interviewer">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblInterviewId1" runat="server" Text='<%# Eval("Interviewer") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Result">
                                    <ItemTemplate>
                                        <asp:Label ID="lblResult" runat="server" Text='<%# Eval("Result") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Next Interview Date">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblNextInterview" runat="server" Text='<%# Eval("NextInterview") %> '></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInterviewId" runat="server" Text='<%# Eval("interviewid") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Next Interviewer">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInterviewId2" runat="server" Text='<%# Eval("NextInterviewer") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Next Interview Place">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInterviewId3" runat="server" Text='<%# Eval("NextIntPlace") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <a id="Detail" href="javascript:void(0)" onclick="Showdetails('<%# Eval("Id")%>');">
                                            <span class="Ecoheadtxt">Details</span></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                    </td>
                </tr>
            </table>
            <asp:Label ID="Label1" runat="server" Text="" Visible="false"></asp:Label>
        </div>
    </asp:Content>
