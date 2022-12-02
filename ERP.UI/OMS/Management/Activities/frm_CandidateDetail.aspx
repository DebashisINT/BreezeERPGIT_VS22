<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.Activities.management_activities_frm_CandidateDetail" Codebehind="frm_CandidateDetail.aspx.cs" %>

<%--<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web" TagPrefix="dxe000001" %>--%>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link rel="Stylesheet" href="../../CSS/style.css" type="text/css" />

    

    <script language="javascript" type="text/javascript">
        function CancelCall()
        {
            parent.editwin.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <div>
            <asp:Panel ID="panel" runat="server" Width="100%">
                <table class="TableMain100">
                    <tr class="EHEADER" style="width: 100%">
                        <td style="width: 60%; text-align: center;">
                            <span style="color: #3300cc"><strong>Candidate Information</strong></span>
                        </td>
                        <td style="width: 40%px; text-align: center;">
                            <span style="color: #3300cc"><strong>Last Interview detail</strong></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; width: 60%; vertical-align: top;">
                            <table cellspacing="0px">
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Candidate Name:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 17px">
                                    </td>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Residence Locality:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblLocality" runat="server" Text=""></asp:Label></td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Source Type:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblSourceType" runat="server" Text=""></asp:Label></td>
                                    <td style="width: 17px">
                                    </td>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Marital Status:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblMaritalStatus" runat="server" Text=""></asp:Label></td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Source Name:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblReferedBy" runat="server" Text=""></asp:Label></td>
                                    <td style="width: 17px">
                                    </td>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">No. of Dependent:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblNoofDependent" runat="server" Text=""></asp:Label></td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Sex:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblSex" runat="server" Text=""></asp:Label></td>
                                    <td style="width: 17px">
                                    </td>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Phone:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label></td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Date of Birth:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblDOB" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 17px">
                                    </td>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Email Id:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblEmailId" runat="server" Text=""></asp:Label></td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Qualification:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblQualification" runat="server" Text=""></asp:Label></td>
                                    <td style="width: 17px">
                                    </td>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Professional Qualification:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblProfQualification" runat="server" Text=""></asp:Label></td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Certifications:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblCertification" runat="server" Text=""></asp:Label></td>
                                    <td style="width: 17px">
                                    </td>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Current Employer:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblCurrentEmployer" runat="server" Text=""></asp:Label></td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Experience Yrs:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblExpYrs" runat="server" Text=""></asp:Label></td>
                                    <td style="width: 17px">
                                    </td>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">CurrentJobProfile:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblCurrentJobProfile" runat="server" Text=""></asp:Label></td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Current CTC:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblCurrentCTC" runat="server" Text=""></asp:Label></td>
                                    <td style="width: 17px">
                                    </td>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Desired CTC:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblDesiredCTC" runat="server" Text=""></asp:Label></td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Previous CTC:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblPreviousCTC" runat="server" Text=""></asp:Label></td>
                                    <td style="width: 17px;">
                                    </td>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Previous Employer:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblPreviousEmployer" runat="server" Text=""></asp:Label></td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Probable Join Date:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblPJD" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 17px;">
                                    </td>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Previous Job Profile :</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblPreviousJobProfile" runat="server" Text=""></asp:Label></td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Reason For Change :</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="tlblReasonforChange" runat="server" Text=""></asp:Label></td>
                                    <td style="width: 17px">
                                    </td>
                                    <td class="gridcellleft">
                                        <%--<span class="Ecoheadtxt" >Actual Start Date/time:</span>--%>
                                    </td>
                                    <td class="gridcellleft">
                                        <%--<asp:TextBox ID="TextBox21" runat="server" Width="200px" Font-Size="12px"></asp:TextBox>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 40%; text-align: center; vertical-align: top; padding-left: 17px">
                            <table cellspacing="0px">
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Communication Skills:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblCommunicationSkills" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Personalty:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblPersonalty" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Sales Orientataion Approach:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblSalesOrientationApproach" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Interest In Organisation:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblIntInOrganisation" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Interest In Position:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblIntInPosition" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Carrers Goal:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblCarrerGoal" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Problem Solving Skills:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblProblemSolvingSkills" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">LeaderShip Skills:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblLeadershipSkills" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Job & Computer Knowledge:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblJobKnowledge" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr onmouseover="className='TRmouseOver'" onmouseout="className='TRmouseOut'">
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Interview Outcome:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:Label ID="lblInterviewOutcome" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </td>
                    </tr>
                    <tr class="EHEADER" style="width: 100%">
                        <td style="text-align: center;" colspan="2">
                            <span style="color: #3300cc"><strong>Joining Detail</strong></span>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;" colspan="2">
                            <table>
                                <tr>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Approved CTC:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <asp:TextBox ID="txtApprovedCTC" TabIndex="17" Text="0" runat="server" Width="146px"></asp:TextBox>
                                    </td>
                                    <td style="width: 17px">
                                    </td>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Aggreement Date:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <dxe:ASPxDateEdit ID="drpAggrementDate" runat="server" EditFormat="Custom" UseMaskBehavior="true"
                                            Width="150px">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Offer Letter Date:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <dxe:ASPxDateEdit ID="drpOfferLetterDate" runat="server" EditFormat="Custom" UseMaskBehavior="true"
                                            Width="150px">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="gridcellleft">
                                        <span class="Ecoheadtxt">Join Date:</span>
                                    </td>
                                    <td class="gridcellleft">
                                        <dxe:ASPxDateEdit ID="drpJoinDateAggrement" runat="server" EditFormat="Custom" UseMaskBehavior="true"
                                            Width="150px">
                                            <ButtonStyle Width="13px">
                                            </ButtonStyle>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td>
                                    </td>
                                    <td colspan="2">
                                        <asp:HiddenField ID="hdID" runat="server" />
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnUpdate" OnClick="btnSave_Click"
                                            TabIndex="23" ValidationGroup="a" Height="22px" />
                                        <input id="btnCancel" type="button" value="Cancel" class="btnUpdate" onclick="CancelCall();"
                                            style="height: 22px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="7" class="gridcellcenter">
                                        <asp:GridView ID="GridInterviewHistory" runat="server" CellPadding="4" ForeColor="#333333"
                                            GridLines="None" BorderWidth="1px" BorderColor="#507CD1" PageSize="20" Width="100%">
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <RowStyle BackColor="#EFF3FB" BorderWidth="1px" />
                                            <EditRowStyle BackColor="#2461BF" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <HeaderStyle Font-Bold="false" ForeColor="black" CssClass="EHEADER" BorderColor="AliceBlue"
                                                BorderWidth="1px" />
                                            <AlternatingRowStyle BackColor="White" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    
    </asp:Content>