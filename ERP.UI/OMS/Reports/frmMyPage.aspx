<%@ Page Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" Inherits="ERP.OMS.Reports.Reports_frmMyPage" CodeBehind="frmMyPage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    

    <script language="Javascript" type="text/javascript">
        function frmOpenNewWindow1(url, title, v_height, v_weight) {
            OnMoreInfoClick(url, title, v_height, v_weight, "Y");
        }
       
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-heading">
        <div class="panel-title">
            <h3>USER DETAILS</h3>
        </div>
    </div>
    <div class="form_main">
        <table class="TableMain100">
            <%--            <tr>
                <td colspan="3" class="EHEADER" align="center" style="font-weight: bold; color: Maroon">USER DETAILS</td>
            </tr>--%>
            <tr>
                <td valign="top">
                    <table class="TableMain100">
                        <tr>
                            <td>
                                <asp:HyperLink ID="HpMarkAttendance" runat="server" NavigateUrl="javascript:void(0)"
                                     CssClass="btn btn-primary">Mark Attendance </asp:HyperLink></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HyperLink ID="HpAttendance" runat="server" NavigateUrl="javascript:void(0)"
                                     CssClass="btn btn-primary">Attendance Register</asp:HyperLink></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HyperLink ID="HpLeaveRegister" runat="server" NavigateUrl="javascript:void(0)"
                                     CssClass="btn btn-primary">Leave Register</asp:HyperLink></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HyperLink ID="HpPaySlipe" runat="server" NavigateUrl="javascript:void(0)" 
                                    CssClass="btn btn-primary">PaySlip</asp:HyperLink></td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table class="TableMain100">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Name:" CssClass="mylabel1"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblName" runat="server" Text="" ForeColor="rosybrown" Width="500px" Font-Bold="true"></asp:Label>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="Code:" CssClass="mylabel1"></asp:Label></td>
                            <td>
                                <asp:Label ID="lblCode" runat="server" Text="" ForeColor="rosybrown" Width="500px" Font-Bold="true"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Address:" CssClass="mylabel1"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAdd" runat="server" Text="" ForeColor="rosybrown" Width="500px" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="Phone No:" CssClass="mylabel1"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPhone" runat="server" Text="" ForeColor="rosybrown" Width="500px" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Email:" CssClass="mylabel1"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblEmail" runat="server" Text="" ForeColor="rosybrown" Width="500px" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label5" runat="server" CssClass="mylabel1" Text="Company:"></asp:Label></td>
                            <td>
                                <asp:Label ID="lblCompany" runat="server" ForeColor="rosybrown" Text="" Width="500px" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label6" runat="server" CssClass="mylabel1" Text="Branch:"></asp:Label></td>
                            <td>
                                <asp:Label ID="lblBranch" runat="server" ForeColor="rosybrown" Text="" Width="500px" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table>
                        <tr>
                            <td style="width: 31px">
                                <asp:Image ID="iPhoto" runat="server" Width="100px" Height="100px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
