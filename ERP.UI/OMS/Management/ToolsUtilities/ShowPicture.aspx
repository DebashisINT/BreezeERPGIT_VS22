<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master"
    Inherits="ERP.OMS.Management.ToolsUtilities.management_utilities_ShowPicture" Codebehind="ShowPicture.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">
        function ChangePicture(obj)
        {
            document.getElementById('hdnID').value=obj;
            document.getElementById('btnSave').click();
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div id="ShowImage" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <table>
                <tr>
                    <td>
                     
                        <asp:Image ID="img1" runat="server" ImageUrl="~/ChangePicture/1.png" onclick="ChangePicture(this.id);" /></td>
                    <td>
                        <asp:Image ID="img2" runat="server" ImageUrl="~/ChangePicture/2.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img3" runat="server" ImageUrl="~/ChangePicture/3.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img4" runat="server" ImageUrl="~/ChangePicture/4.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img5" runat="server" ImageUrl="~/ChangePicture/5.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img6" runat="server" ImageUrl="~/ChangePicture/6.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img7" runat="server" ImageUrl="~/ChangePicture/7.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img8" runat="server" ImageUrl="~/ChangePicture/8.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img9" runat="server" ImageUrl="~/ChangePicture/9.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img10" runat="server" ImageUrl="~/ChangePicture/10.png" onclick="ChangePicture(this.id);"/></td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="img11" runat="server" ImageUrl="~/ChangePicture/11.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img12" runat="server" ImageUrl="~/ChangePicture/12.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img13" runat="server" ImageUrl="~/ChangePicture/13.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img14" runat="server" ImageUrl="~/ChangePicture/14.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img15" runat="server" ImageUrl="~/ChangePicture/15.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img16" runat="server" ImageUrl="~/ChangePicture/16.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img17" runat="server" ImageUrl="~/ChangePicture/17.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img18" runat="server" ImageUrl="~/ChangePicture/18.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img19" runat="server" ImageUrl="~/ChangePicture/19.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img20" runat="server" ImageUrl="~/ChangePicture/20.png" onclick="ChangePicture(this.id);"/></td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="img21" runat="server" ImageUrl="~/ChangePicture/21.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img22" runat="server" ImageUrl="~/ChangePicture/22.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img23" runat="server" ImageUrl="~/ChangePicture/23.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img24" runat="server" ImageUrl="~/ChangePicture/24.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img25" runat="server" ImageUrl="~/ChangePicture/25.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img26" runat="server" ImageUrl="~/ChangePicture/26.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img27" runat="server" ImageUrl="~/ChangePicture/27.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img28" runat="server" ImageUrl="~/ChangePicture/28.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img29" runat="server" ImageUrl="~/ChangePicture/29.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img30" runat="server" ImageUrl="~/ChangePicture/30.png" onclick="ChangePicture(this.id);"/></td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="img31" runat="server" ImageUrl="~/ChangePicture/31.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img32" runat="server" ImageUrl="~/ChangePicture/32.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img33" runat="server" ImageUrl="~/ChangePicture/33.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img34" runat="server" ImageUrl="~/ChangePicture/34.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img35" runat="server" ImageUrl="~/ChangePicture/35.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img36" runat="server" ImageUrl="~/ChangePicture/36.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img37" runat="server" ImageUrl="~/ChangePicture/37.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img38" runat="server" ImageUrl="~/ChangePicture/38.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img39" runat="server" ImageUrl="~/ChangePicture/39.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img40" runat="server" ImageUrl="~/ChangePicture/40.png" onclick="ChangePicture(this.id);"/></td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="img41" runat="server" ImageUrl="~/ChangePicture/41.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img42" runat="server" ImageUrl="~/ChangePicture/42.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img43" runat="server" ImageUrl="~/ChangePicture/43.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img44" runat="server" ImageUrl="~/ChangePicture/44.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img45" runat="server" ImageUrl="~/ChangePicture/45.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img46" runat="server" ImageUrl="~/ChangePicture/46.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img47" runat="server" ImageUrl="~/ChangePicture/47.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img48" runat="server" ImageUrl="~/ChangePicture/48.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img49" runat="server" ImageUrl="~/ChangePicture/49.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img50" runat="server" ImageUrl="~/ChangePicture/50.png" onclick="ChangePicture(this.id);"/></td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="img51" runat="server" ImageUrl="~/ChangePicture/51.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img52" runat="server" ImageUrl="~/ChangePicture/52.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img53" runat="server" ImageUrl="~/ChangePicture/53.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img54" runat="server" ImageUrl="~/ChangePicture/54.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img55" runat="server" ImageUrl="~/ChangePicture/55.png" onclick="ChangePicture(this.id);"/></td>
                    <td>
                        <asp:Image ID="img56" runat="server" ImageUrl="~/ChangePicture/56.png" onclick="ChangePicture(this.id);"/></td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:HiddenField ID="hdnID" runat="server" />
                    </td>
                    <td colspan="5" style="display:none">
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                    </td>
                </tr>
            </table>
        </div>
</asp:Content>
