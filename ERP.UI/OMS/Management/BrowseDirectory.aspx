<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OMS/MasterPage/ERP.Master" Inherits="ERP.OMS.Management.BrowseDirectory" CodeBehind="BrowseDirectory.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .errorMsg {
            FONT-SIZE: 8.25pt;
            COLOR: red;
            FONT-FAMILY: Verdana, Arial;
            TEXT-DECORATION: none;
        }

        .hilite {
            BACKGROUND-COLOR: #dfe5ff;
        }

        .nohilite {
            BACKGROUND-COLOR: #ffffff;
        }

        .text {
            FONT-SIZE: 8.25pt;
            COLOR: black;
            FONT-FAMILY: Verdana, Arial;
            TEXT-DECORATION: none;
        }

        .tableOutlineWt {
            BORDER-RIGHT: #cccccc 1px solid;
            BORDER-TOP: #666666 1px solid;
            MARGIN-TOP: 0px;
            OVERFLOW: auto;
            BORDER-LEFT: #333333 1px solid;
            PADDING-TOP: 0px;
            BORDER-BOTTOM: #cccccc 1px solid;
        }
    </style>
    <script language="javascript">
    function SelectAndClose() {
        txtValue = document.getElementById('_browseTextBox');

        window.returnValue = txtValue.value;
        window.close();
        return false;
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table width="100%" border="0">
            <tr>
                <td>
                    <span class="text"><b>Browse directories:</b></span></td>
            </tr>
            <tr>
                <td>
                    <span class="text"></span></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="error" runat="server" CssClass="errorMsg"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="_browseTextBox" runat="server" CssClass="toolbar" Width="300px" /><asp:ImageButton ID="_browseButton" ImageUrl="~/images/Go.gif" runat="server" OnClick="_browseButton_Click" Visible="False" />
                </td>
            </tr>
            <tr>
                <td>
                    <div class="tableOutlineWt" style="width: 313px; height: 371px; background-color: white">
                        <table cellspacing="0" cellpadding="4" width="100%" bgcolor="#ffffff" border="0">
                            <tr>
                                <td>

                                    <asp:TreeView ID="TreeView1" runat="server" Height="326px" ImageSet="XPFileExplorer"
                                        NodeIndent="15" Width="292px">
                                        <ParentNodeStyle Font-Bold="False" />
                                        <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                                        <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="0px"
                                            VerticalPadding="0px" />
                                        <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px"
                                            NodeSpacing="0px" VerticalPadding="2px" />
                                        <LeafNodeStyle ImageUrl="~/images/folder.gif" />
                                    </asp:TreeView>

                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">


                    <asp:ImageButton ID="_selectButton" ImageUrl="~/images/ok.jpg" OnClientClick="SelectAndClose();"
                        runat="server" />
                </td>
            </tr>
        </table>

    </div>
</asp:Content>
