<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.management_frm_AddInterviews" CodeBehind="frm_AddInterviews.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link type="text/css" href="../CSS/style.css" rel="Stylesheet" />
    <script type="text/javascript" src="/assests/js/loaddata1.js"></script>
    <script language="javascript" type="text/javascript">
        function SignOff() {
            window.parent.SignOff();
        }
        function Page_Load() {
            ShowDropdown();
        }
        function ShowDropdown() {

            var data = document.getElementById("drpInterviewOutcome");
            //alert(data.value);
            switch (data.value) {
                case "0":
                    txtHoldUntillDate.SetEnabled(false);
                    document.getElementById("drpNextInterviewer").disabled = true;
                    txtNextInterviewDate.SetEnabled(false);
                    document.getElementById("drpNextInterviewPlace").disabled = true;
                    txtJoinDateAggrement.SetEnabled(false);
                    document.getElementById("txtApprovedCTC").disabled = true;
                    document.getElementById("txtEliminationReason").disabled = true;
                    break;
                case "1":
                    txtHoldUntillDate.SetEnabled(false);
                    document.getElementById("drpNextInterviewer").disabled = true;
                    txtNextInterviewDate.SetEnabled(false);
                    document.getElementById("drpNextInterviewPlace").disabled = true;
                    txtJoinDateAggrement.SetEnabled(true);
                    document.getElementById("txtApprovedCTC").disabled = false;
                    document.getElementById("txtEliminationReason").disabled = true;
                    break;
                case "2":
                    txtHoldUntillDate.SetEnabled(false);
                    document.getElementById("drpNextInterviewer").disabled = true;
                    txtNextInterviewDate.SetEnabled(false);
                    document.getElementById("drpNextInterviewPlace").disabled = true;
                    txtJoinDateAggrement.SetEnabled(false);
                    document.getElementById("txtApprovedCTC").disabled = true;
                    document.getElementById("txtEliminationReason").disabled = false;
                    break;
                case "3":
                    txtHoldUntillDate.SetEnabled(true);
                    document.getElementById("drpNextInterviewer").disabled = true;
                    txtNextInterviewDate.SetEnabled(false);
                    document.getElementById("drpNextInterviewPlace").disabled = true;
                    txtJoinDateAggrement.SetEnabled(false);
                    document.getElementById("txtApprovedCTC").disabled = true;
                    document.getElementById("txtEliminationReason").disabled = true;
                    break;
                case "4":
                    txtHoldUntillDate.SetEnabled(false);
                    document.getElementById("drpNextInterviewer").disabled = false;
                    txtNextInterviewDate.SetEnabled(true);
                    document.getElementById("drpNextInterviewPlace").disabled = false;
                    txtJoinDateAggrement.SetEnabled(false);
                    document.getElementById("txtApprovedCTC").disabled = true;
                    document.getElementById("txtEliminationReason").disabled = true;
                    break;
                case "5":
                    txtHoldUntillDate.SetEnabled(false);
                    document.getElementById("drpNextInterviewer").disabled = false;
                    txtNextInterviewDate.SetEnabled(true);
                    document.getElementById("drpNextInterviewPlace").disabled = false;
                    txtJoinDateAggrement.SetEnabled(false);
                    document.getElementById("txtApprovedCTC").disabled = true;
                    document.getElementById("txtEliminationReason").disabled = true;
                    break;
                case "6":
                    txtHoldUntillDate.SetEnabled(false);
                    document.getElementById("drpNextInterviewer").disabled = true;
                    txtNextInterviewDate.SetEnabled(false);
                    document.getElementById("drpNextInterviewPlace").disabled = true;
                    txtJoinDateAggrement.SetEnabled(false);
                    document.getElementById("txtApprovedCTC").disabled = true;
                    document.getElementById("txtEliminationReason").disabled = false;
                    break;
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table class="TableMain100">
            <tr class="EHEADER">
                <td class="ColHead" style="width: 98%">
                    <span style="color: #3300cc"><strong>Candidate Interview</strong></span>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; width: 100%;">
                    <table style="border: solid 2px white">
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Candidate Name:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtName" runat="server" Width="200px" Font-Size="12px" ValidationGroup="a"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 17px"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Interest In Organisation :</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="drpIntInOrganisation" Width="203px" Font-Size="12px" runat="server"
                                    TabIndex="1">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Interview Date/Time:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtInterviewDate" runat="server" Font-Size="12px" ReadOnly="true"
                                    ValidationGroup="a" Width="200px"></asp:TextBox>
                            </td>
                            <td style="width: 17px"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Interest In Position:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="drpInterestInPosition" runat="server" Width="203px" Font-Size="12px"
                                    TabIndex="2">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Interview Place:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtInterviewPlace" runat="server" Width="200px" Font-Size="12px"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 17px"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Carrer Goals:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="drpCarretGoals" Width="203px" Font-Size="12px" runat="server"
                                    TabIndex="3">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Interviewer:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:TextBox ID="txtInterViewer" runat="server" Font-Size="12px" ReadOnly="true"
                                    ValidationGroup="a" Width="200px"></asp:TextBox></td>
                            <td style="width: 17px"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Problem Solving Skills:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="drpProblemSolvingSkills" Width="203px" Font-Size="12px" runat="server"
                                    TabIndex="4">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Communication Skills:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="drpCommunicationSkills" Width="203px" Font-Size="12px" runat="server"
                                    TabIndex="5">
                                </asp:DropDownList></td>
                            <td style="width: 17px"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Leadership Skills:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="drpLeadershipSkills" Width="203px" Font-Size="12px" runat="server"
                                    TabIndex="6">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Personality:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="drpPersonality" Width="203px" Font-Size="12px" runat="server"
                                    TabIndex="7">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 17px"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Job & Computer Knowledge:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="drpJobKnowledge" Width="203px" Font-Size="12px" runat="server"
                                    TabIndex="8">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Sales Orientation Approach:</span></td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="drpSalesOrientationApproach" Width="203px" Font-Size="12px"
                                    runat="server" TabIndex="9">
                                </asp:DropDownList></td>
                            <td style="width: 17px"></td>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Interview Outcome:</span>
                            </td>
                            <td class="gridcellleft">
                                <asp:DropDownList ID="drpInterviewOutcome" Width="203px" Font-Size="12px" runat="server"
                                    TabIndex="10">
                                    <asp:ListItem Text="Due" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Recruit" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Eliminate" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Hold" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Further Interview " Value="4"></asp:ListItem>
                                    <asp:ListItem Text="PostPone Interview" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Cancel Interview" Value="6"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="gridcellleft">
                                <span class="Ecoheadtxt">Remarks:</span>
                            </td>
                            <td class="gridcellleft" colspan="4">
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="99%" Font-Size="12px"
                                    TabIndex="11" Height="50px" Rows="5"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%; text-align: left" colspan="5">
                                <asp:Panel ID="ShowHide" runat="server" Width="100%">
                                    <table>
                                        <tr>
                                            <td class="gridcellleft">
                                                <span id="spHoldUntillDate" class="Ecoheadtxt">Hold Until date:</span>
                                            </td>
                                            <td class="gridcellleft" style="width: 198px">
                                                <dxe:ASPxDateEdit ID="txtHoldUntillDate" ClientInstanceName="txtHoldUntillDate" runat="server"
                                                    EditFormat="Custom" UseMaskBehavior="true" Width="200px">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td style="width: 17px"></td>
                                            <td class="gridcellleft">
                                                <span id="spNextInterviewer" class="Ecoheadtxt">Next Interviewer:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpNextInterviewer" Width="203px" runat="server" TabIndex="13"
                                                    Font-Size="12px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="gridcellleft">
                                                <span id="spNextInterviewDate" class="Ecoheadtxt">Next Interview Date/Time:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <dxe:ASPxDateEdit ID="txtNextInterviewDate" ClientInstanceName="txtNextInterviewDate"
                                                    runat="server" EditFormat="Custom" UseMaskBehavior="true" Width="200px">
                                                    <ButtonStyle Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td style="width: 17px"></td>
                                            <td class="gridcellleft">
                                                <span id="spNextInterviewPlace" class="Ecoheadtxt">Next Interview Place:</span>
                                            </td>
                                            <td class="gridcellleft">
                                                <asp:DropDownList ID="drpNextInterviewPlace" Width="203px" runat="server" TabIndex="15"
                                                    Font-Size="12px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%;" colspan="5">
                                <asp:Panel ID="pnlSelected" runat="server" Width="100%">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="background-color: #92cfff">
                                                <strong><span class="Ecoheadtxt">Elimination or Recruitment Detail</span> </strong>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td class="gridcellleft">
                                                            <span id="spApprovedCTC" class="Ecoheadtxt">Approved CTC:</span>
                                                        </td>
                                                        <td class="gridcellleft">
                                                            <asp:TextBox ID="txtApprovedCTC" Text="0" runat="server" Width="200px" Font-Size="12px"
                                                                TabIndex="16"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 17px"></td>
                                                        <td class="gridcellleft">
                                                            <span id="spJoinDateAggrement" class="Ecoheadtxt">Join Date:</span>
                                                        </td>
                                                        <td class="gridcellleft" style="width: 195px; vertical-align: middle">
                                                            <dxe:ASPxDateEdit ID="txtJoinDateAggrement" ClientInstanceName="txtJoinDateAggrement"
                                                                runat="server" EditFormat="Custom" UseMaskBehavior="true" Width="200px">
                                                                <ButtonStyle Width="13px">
                                                                </ButtonStyle>
                                                            </dxe:ASPxDateEdit>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="gridcellleft">
                                                            <span id="spElimination" class="Ecoheadtxt">Elimination Reason:</span>
                                                        </td>
                                                        <td class="gridcellleft" colspan="4">
                                                            <asp:TextBox ID="txtEliminationReason" TabIndex="18" TextMode="MultiLine" runat="server"
                                                                Rows="5" Width="99%" Height="50px" Font-Size="12px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:HiddenField ID="int_id" runat="server" />
                                <asp:HiddenField ID="ID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: right">
                                <asp:Label ID="lblmessage" runat="server" ForeColor="Red"></asp:Label>&nbsp;</td>
                            <td style="width: 17px"></td>
                            <td colspan="2" class="gridcellright">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnUpdate" OnClick="btnSave_Click"
                                    TabIndex="19" ValidationGroup="a" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnUpdate" OnClick="btnCancel_Click"
                                    TabIndex="20" />
                                &nbsp; &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
