<%@ Page Title="" Language="C#" MasterPageFile="~/OMS/MasterPage/ERP.Master" AutoEventWireup="true" CodeBehind="ReportMain.aspx.cs" Inherits="ERP.OMS.Reports.XtraReports.ReportMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript">
        function CopyNew() {
            cpopUp_newReport.Show(); 
        }
        function hidePopup() {
            cpopUp_newReport.Hide();
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <dxe:ASPxPopupControl ID="popUp_newReport" runat="server" ClientInstanceName="cpopUp_newReport"
        Width="400px" HeaderText="New Report" PopupHorizontalAlign="WindowCenter"
        BackColor="white" Height="100px" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
        Modal="True" ContentStyle-VerticalAlign="Top" EnableHierarchyRecreation="True">
        <contentcollection>
                    <dxe:PopupControlContentControl runat="server">
                        <div class="Top clearfix">
                            <table>
                                <tr>
                                    <td>
                                        <span style="padding-right:5px">File Name</span>
                                    </td>
                                    <td>
                                         <asp:TextBox ID="txtFileName" runat="server" MaxLength="50"  ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                       <asp:Button ID="btnNewFileSave" runat="server" Text="Save" OnClick="btnNewFileSave_Click"/>
                                         <asp:Button ID="btnNewFileCancel" runat="server" Text="Cancel" OnClientClick="hidePopup(); return false;"/>
                                    </td>
                                   
                            </table>
                         
                           
                        </div>

                    </dxe:PopupControlContentControl>
                </contentcollection>
        <headerstyle backcolor="LightGray" forecolor="Black" />
    </dxe:ASPxPopupControl>
    <asp:DropDownList ID="ddReportName" runat="server" Height="16px" Width="155px">
    </asp:DropDownList><asp:LinkButton ID="lnkBtnCopy" runat="server"   OnClientClick="CopyNew();return false;">Copy</asp:LinkButton>
    <br />
    <br />
    <asp:Button ID="btnDesign" runat="server" Text="Designer" OnClick="btnDesign_Click" />
    <asp:Button ID="btnPreview" runat="server" Text="Preview" OnClick="btnPreview_Click" />
</asp:Content>
